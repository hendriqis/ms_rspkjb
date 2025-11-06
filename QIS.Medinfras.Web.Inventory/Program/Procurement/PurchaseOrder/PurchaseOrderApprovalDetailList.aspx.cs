using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class PurchaseOrderApprovalDetailList : BasePageTrx
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.PURCHASE_ORDER_APPROVAL;
        }

        private GetUserMenuAccess menu;

        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = false;
        }

        protected override void InitializeDataControl()
        {
            MPTrx master = (MPTrx)Master;
            menu = ((MPMain)master.Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

            hdnOrderID.Value = Page.Request.QueryString["id"];
            vPurchaseOrderHd entityPOHD = BusinessLayer.GetvPurchaseOrderHdList(String.Format("PurchaseOrderID = '{0}'", Convert.ToInt32(hdnOrderID.Value)))[0];
            EntityToControl(entityPOHD);
        }

        private void EntityToControl(vPurchaseOrderHd entity)
        {
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
            }
            hdnOrderID.Value = entity.PurchaseOrderID.ToString();
            txtOrderNo.Text = entity.PurchaseOrderNo;
            txtItemOrderDate.Text = entity.OrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtSupplierName.Text = entity.BusinessPartnerName;
            txtNotes.Text = entity.Remarks;
            txtJumlahNilai.Text = entity.TransactionAmount.ToString("N2");
            txtDiskonFinal.Text = entity.FinalDiscount.ToString("N2");
            txtPPN.Text = entity.PPN.ToString("N2");
            txtSaldoNilai.Text = entity.cfTransactionAmount.ToString("N2");

            BindGridView(1, true, ref PageCount);
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vPurchaseOrderDt entity = e.Row.DataItem as vPurchaseOrderDt;
                CheckBox chkIsSelected = e.Row.FindControl("chkIsSelected") as CheckBox;
                TextBox txtPOQty = e.Row.FindControl("txtPOQty") as TextBox;

                if (entity.GCItemDetailStatus == Constant.TransactionStatus.APPROVED || lstSelectedMember.Contains(entity.ID.ToString()))
                {
                    chkIsSelected.Checked = true;
                    txtPOQty.ReadOnly = false;
                }
                else
                {
                    chkIsSelected.Checked = false;
                    txtPOQty.ReadOnly = true;
                }

                txtPOQty.Text = entity.Quantity.ToString(Constant.FormatString.NUMERIC_2);
            }
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnOrderID.Value != "")
                filterExpression = string.Format("PurchaseOrderID = {0} AND IsDeleted = 0", hdnOrderID.Value);
            
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPurchaseOrderDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            lstSelectedMember = hdnSelectedMember.Value.Split(',');
            List<vPurchaseOrderDt> lstEntity = BusinessLayer.GetvPurchaseOrderDtList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
            grdView.DataSource = lstEntity;
            grdView.DataBind();

        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    vPurchaseOrderHd entityPOHD = BusinessLayer.GetvPurchaseOrderHdList(String.Format("PurchaseOrderID = '{0}'", Convert.ToInt32(hdnOrderID.Value)))[0];
                    EntityToControl(entityPOHD);
                    //BindGridView(1, true, ref pageCount);

                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseOrderHdDao purchaseHdDao = new PurchaseOrderHdDao(ctx);
            PurchaseOrderDtDao purchaseDtDao = new PurchaseOrderDtDao(ctx);
            PurchaseRequestHdDao entityPRHDDao = new PurchaseRequestHdDao(ctx);
            PurchaseRequestDtDao entityPRDTDao = new PurchaseRequestDtDao(ctx);

            try
            {
                if (type == "save")
                {
                    //Checklist -> SAVE DETAIL
                    string filterExpressionPurchaseOrderDt = String.Format("ID IN ({0}) AND PurchaseOrderID = {1} AND IsDeleted = 0 AND GCItemDetailStatus <> '{2}'",
                        hdnSelectedMember.Value.Substring(1), hdnOrderID.Value, Constant.TransactionStatus.VOID);
                    List<PurchaseOrderDt> lstPurchaseOrderDt = BusinessLayer.GetPurchaseOrderDtList(filterExpressionPurchaseOrderDt, ctx);

                    String[] paramID = hdnSelectedMember.Value.Substring(1).Split(',');
                    String[] paramQty = hdnSelectedMemberPOQty.Value.Substring(1).Split('|');

                    for (int i = 0; i < paramID.Count(); i++)
                    {
                        int tempID = Convert.ToInt32(paramID[i]);
                        decimal tempQty = Convert.ToDecimal(paramQty[i]);

                        PurchaseOrderDt orderDt = lstPurchaseOrderDt.Where(a => a.ID == tempID).FirstOrDefault();

                        decimal oSubTotal = tempQty * orderDt.UnitPrice;
                        decimal oDiscAmount1 = oSubTotal * orderDt.DiscountPercentage1 / 100;
                        decimal oSubTotalAfterDisc1 = oSubTotal - oDiscAmount1;
                        decimal oDiscAmount2 = oSubTotalAfterDisc1 * orderDt.DiscountPercentage2 / 100;
                        decimal oLineAmount = oSubTotal - oDiscAmount1 - oDiscAmount2;

                        orderDt.Quantity = tempQty;
                        orderDt.DiscountAmount1 = oDiscAmount1;
                        orderDt.DiscountAmount2 = oDiscAmount2;
                        orderDt.LineAmount = oLineAmount;

                        orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        purchaseDtDao.Update(orderDt);
                    }
                }
                else if (type == "approve" || type == "approveall")
                {
                    if (type == "approve")
                    {
                        //Checklist -> APPROVED
                        string filterExpressionPurchaseOrderDt = String.Format("ID IN ({0}) AND PurchaseOrderID = {1} AND IsDeleted = 0 AND GCItemDetailStatus <> '{2}'", 
                                                                                hdnSelectedMember.Value.Substring(1), hdnOrderID.Value, Constant.TransactionStatus.VOID);
                        List<PurchaseOrderDt> lstPurchaseOrderDt = BusinessLayer.GetPurchaseOrderDtList(filterExpressionPurchaseOrderDt, ctx);

                        String[] paramID = hdnSelectedMember.Value.Substring(1).Split(',');
                        String[] paramQty = hdnSelectedMemberPOQty.Value.Substring(1).Split('|');

                        for (int i = 0; i < paramID.Count(); i++)
                        {
                            int tempID = Convert.ToInt32(paramID[i]);
                            decimal tempQty = Convert.ToDecimal(paramQty[i]);

                            PurchaseOrderDt orderDt = lstPurchaseOrderDt.Where(a => a.ID == tempID).FirstOrDefault();

                            decimal oSubTotal = tempQty * orderDt.UnitPrice;
                            decimal oDiscAmount1 = oSubTotal * orderDt.DiscountPercentage1 / 100;
                            decimal oSubTotalAfterDisc1 = oSubTotal - oDiscAmount1;
                            decimal oDiscAmount2 = oSubTotalAfterDisc1 * orderDt.DiscountPercentage2 / 100;
                            decimal oLineAmount = oSubTotal - oDiscAmount1 - oDiscAmount2;

                            orderDt.Quantity = tempQty;
                            orderDt.DiscountAmount1 = oDiscAmount1;
                            orderDt.DiscountAmount2 = oDiscAmount2;
                            orderDt.LineAmount = oLineAmount;

                            orderDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                            orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            purchaseDtDao.Update(orderDt);
                        }
                        
                        //UnChecklist -> OPEN
                        string filterExpressionPurchaseOrderDt2 = String.Format("ID NOT IN ({0}) AND PurchaseOrderID = {1} AND IsDeleted = 0 AND GCItemDetailStatus <> '{2}'", 
                                                                                    hdnSelectedMember.Value.Substring(1), hdnOrderID.Value, Constant.TransactionStatus.VOID);
                        List<PurchaseOrderDt> lstPurchaseOrderDt2 = BusinessLayer.GetPurchaseOrderDtList(filterExpressionPurchaseOrderDt2, ctx);
                        foreach (PurchaseOrderDt purchaseDt in lstPurchaseOrderDt2)
                        {
                            purchaseDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                            purchaseDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            purchaseDtDao.Update(purchaseDt);
                        }

                        //Update PurchaseOrderHd
                        string filterExpressionCheck = String.Format("PurchaseOrderID = {0} AND GCItemDetailStatus = '{1}'", hdnOrderID.Value, Constant.TransactionStatus.OPEN);
                        List<PurchaseOrderDt> lstPurchaseOrderDtCheck = BusinessLayer.GetPurchaseOrderDtList(filterExpressionCheck, ctx);
                        if (lstPurchaseOrderDtCheck.Count == 0)
                        {
                            PurchaseOrderHd entity = purchaseHdDao.Get(Convert.ToInt32(hdnOrderID.Value));
                            entity.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entity.ApprovedDate = DateTime.Now;
                            entity.ApprovedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            purchaseHdDao.Update(entity);

                            List<PurchaseRequestPO> lstPurchaseRequestPO = BusinessLayer.GetPurchaseRequestPOList(string.Format("PurchaseOrderID = {0}", hdnOrderID.Value), ctx);
                            if (lstPurchaseRequestPO.Count > 0)
                            {
                                foreach (PurchaseRequestPO entityPRPO in lstPurchaseRequestPO)
                                {
                                    List<PurchaseRequestDt> entityPRDtList = BusinessLayer.GetPurchaseRequestDtList(string.Format(
                                                                "PurchaseRequestID = {0} AND IsDeleted = 0 AND GCItemDetailStatus != '{1}'",
                                                                entityPRPO.PurchaseRequestID, Constant.TransactionStatus.VOID), ctx);
                                    if (entityPRDtList.Count() > 0)
                                    {
                                        PurchaseRequestDt entityPRDt = entityPRDtList.LastOrDefault();
                                        if (entityPRDt.OrderedQuantity >= entityPRDt.Quantity)
                                        {
                                            entityPRDt.GCItemDetailStatus = Constant.TransactionStatus.CLOSED;
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            entityPRDTDao.Update(entityPRDt);
                                        }

                                        int Count = BusinessLayer.GetPurchaseRequestDtRowCount(string.Format(
                                                                            "PurchaseRequestID = {0} AND IsDeleted = 0 AND GCItemDetailStatus != '{1}'",
                                                                            entityPRDt.PurchaseRequestID, Constant.TransactionStatus.CLOSED), ctx);
                                        if (Count == 0)
                                        {
                                            PurchaseRequestHd entityPRHd = BusinessLayer.GetPurchaseRequestHdList(string.Format(
                                                                                                "PurchaseRequestID = {0} AND GCTransactionStatus != '{1}'",
                                                                                                entityPRDt.PurchaseRequestID, Constant.TransactionStatus.VOID), ctx).FirstOrDefault();

                                            entityPRHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                                            entityPRHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            entityPRHDDao.Update(entityPRHd);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else //ApproveALL
                    {
                        string filterAll = string.Format("PurchaseOrderID = {0} AND IsDeleted = 0 AND GCItemDetailStatus IN ('{1}','{2}')",
                                                            hdnOrderID.Value, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL);
                        List<PurchaseOrderDt> podtList = BusinessLayer.GetPurchaseOrderDtList(filterAll, ctx);
                        foreach (PurchaseOrderDt podt in podtList)
                        {
                            podt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                            podt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            purchaseDtDao.Update(podt);
                        }
                        
                        string filterExpressionCheck = String.Format("PurchaseOrderID = {0} AND GCItemDetailStatus IN ('{1}','{2}')",
                                                                        hdnOrderID.Value, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL);
                        List<PurchaseOrderDt> lstPurchaseOrderDtCheck = BusinessLayer.GetPurchaseOrderDtList(filterExpressionCheck, ctx);
                        if (lstPurchaseOrderDtCheck.Count == 0)
                        {
                            PurchaseOrderHd entity = purchaseHdDao.Get(Convert.ToInt32(hdnOrderID.Value));

                            entity.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entity.ApprovedDate = DateTime.Now;
                            entity.ApprovedBy = AppSession.UserLogin.UserID;
                            purchaseHdDao.Update(entity);

                            List<PurchaseRequestPO> lstPurchaseRequestPO = BusinessLayer.GetPurchaseRequestPOList(string.Format("PurchaseOrderID = {0}", hdnOrderID.Value), ctx);
                            if (lstPurchaseRequestPO.Count > 0)
                            {
                                foreach (PurchaseRequestPO entityPRPO in lstPurchaseRequestPO)
                                {
                                    List<PurchaseRequestDt> entityPRDtList = BusinessLayer.GetPurchaseRequestDtList(string.Format(
                                                                                                "PurchaseRequestID = {0} AND IsDeleted = 0 AND GCItemDetailStatus != '{1}'",
                                                                                                entityPRPO.PurchaseRequestID, Constant.TransactionStatus.VOID), ctx);
                                    if (entityPRDtList.Count() > 0)
                                    {
                                        PurchaseRequestDt entityPRDt = entityPRDtList.LastOrDefault();
                                        if (entityPRDt.OrderedQuantity >= entityPRDt.Quantity)
                                        {
                                            entityPRDt.GCItemDetailStatus = Constant.TransactionStatus.CLOSED;
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            entityPRDTDao.Update(entityPRDt);
                                        }

                                        int Count = BusinessLayer.GetPurchaseRequestDtRowCount(string.Format(
                                                                            "PurchaseRequestID = {0} AND IsDeleted = 0 AND GCItemDetailStatus != '{1}'",
                                                                            entityPRDt.PurchaseRequestID, Constant.TransactionStatus.CLOSED), ctx);
                                        if (Count == 0)
                                        {
                                            PurchaseRequestHd entityPRHd = BusinessLayer.GetPurchaseRequestHdList(string.Format(
                                                                                                "PurchaseRequestID = {0} AND GCTransactionStatus != '{1}'",
                                                                                                entityPRDt.PurchaseRequestID, Constant.TransactionStatus.VOID), ctx).FirstOrDefault();

                                            entityPRHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                                            entityPRHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            entityPRHDDao.Update(entityPRHd);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;

        }
    }
}
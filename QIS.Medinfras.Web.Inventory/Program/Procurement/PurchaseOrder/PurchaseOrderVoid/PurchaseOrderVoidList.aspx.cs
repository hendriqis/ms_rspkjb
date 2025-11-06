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
using DevExpress.Web.ASPxCallbackPanel;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class PurchaseOrderVoidList : BasePageList
    {
        protected int PageCount = 1;
        protected string filterExpressionLocation = "";

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.PURCHASE_ORDER_VOID;
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "Supplier Name", "Purchase Order No", "Created By" };
            fieldListValue = new string[] { "BusinessPartnerName", "PurchaseOrderNo", "CreatedByName" };
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            hdnFilterExpression.Value = filterExpression;
            filterExpressionLocation = string.Format("{0};{1};{2};", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.TransactionCode.PURCHASE_ORDER);

            SettingParameterDt setvarDTR = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IM_DEFAULT_ROLE_OFFICER_LOGISTIC);

            if (setvarDTR.ParameterValue != null && setvarDTR.ParameterValue != "" && setvarDTR.ParameterValue != "0")
            {
                List<UserInRole> uir = BusinessLayer.GetUserInRoleList(String.Format(
                    "HealthcareID = {0} AND UserID = {1} AND RoleID = {2}", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, setvarDTR.ParameterValue));
                if (uir.Count() > 0)
                {
                    SettingParameterDt setvarDTL = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_LOGISTIC);

                    Location loc = BusinessLayer.GetLocationList(string.Format("LocationID = '{0}'", setvarDTL.ParameterValue)).FirstOrDefault();
                    hdnLocationIDFrom.Value = loc.LocationID.ToString();
                    txtLocationCode.Text = loc.LocationCode;
                    txtLocationName.Text = loc.LocationName;
                }
                else
                {
                    SettingParameterDt setvarDTP = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_PHARMACY);

                    Location loc = BusinessLayer.GetLocationList(string.Format("LocationID = '{0}'", setvarDTP.ParameterValue)).FirstOrDefault();
                    hdnLocationIDFrom.Value = loc.LocationID.ToString();
                    txtLocationCode.Text = loc.LocationCode;
                    txtLocationName.Text = loc.LocationName;
                }
            }
            BindGridView(1, true, ref PageCount);
        }

        private string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;
            String filterLocationID = Convert.ToString(hdnLocationIDFrom.Value);
            if (filterExpression != "")
            {
                filterExpression += " AND ";
            }

            //filterExpression += String.Format("LocationID = '{0}' AND GCTransactionStatus = '{1}' AND PurchaseOrderID NOT IN (SELECT PurchaseOrderID FROM PurchaseOrderDt WHERE IsDeleted = 0 AND GCItemDetailStatus != '{1}' AND ID IN (SELECT PurchaseOrderDtID FROM PurchaseReceiveDt WHERE GCItemDetailStatus != '{1}' AND PurchaseReceiveID IN (SELECT PurchaseReceiveID FROM PurchaseReceiveHd WHERE GCTransactionStatus != '{1}')))", filterLocationID, Constant.TransactionStatus.APPROVED, Constant.TransactionStatus.VOID);
            //filterExpression += String.Format("LocationID = '{0}' AND GCTransactionStatus = '{1}' AND PurchaseOrderID NOT IN (SELECT PurchaseOrderID FROM PurchaseOrderDt WHERE ID IN (SELECT PurchaseOrderDtID FROM PurchaseReceiveDt WHERE GCItemDetailStatus != '{2}' AND PurchaseReceiveID IN (SELECT PurchaseReceiveID FROM PurchaseReceiveHd WHERE GCTransactionStatus != '{2}')))", filterLocationID, Constant.TransactionStatus.APPROVED, Constant.TransactionStatus.VOID);

            filterExpression += String.Format("LocationID = '{0}' AND GCTransactionStatus = '{1}' AND PurchaseOrderID IN (SELECT PurchaseOrderID FROM PurchaseOrderDt WHERE ReceivedQuantity = 0 AND (ReceivedInformation IS NULL OR ReceivedInformation = '') AND IsDeleted = 0)", filterLocationID, Constant.TransactionStatus.APPROVED);

            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPurchaseOrderHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPurchaseOrderHd> lstEntity = BusinessLayer.GetvPurchaseOrderHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "PurchaseOrderID DESC");
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

                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected override bool OnCustomButtonClick(string type, ref string retval, ref string errMessage)
        {
            bool result = true;
            if (type == "decline")
            {
                IDbContext ctx = DbFactory.Configure(true);
                PurchaseReceiveHdDao receiveHdDao = new PurchaseReceiveHdDao(ctx);
                PurchaseReceiveDtDao receiveDtDao = new PurchaseReceiveDtDao(ctx);
                PurchaseOrderHdDao entityDao = new PurchaseOrderHdDao(ctx);
                PurchaseOrderDtDao entityDtDao = new PurchaseOrderDtDao(ctx);
                PurchaseOrderTermDao entityTermDao = new PurchaseOrderTermDao(ctx);
                PurchaseRequestHdDao requestHdDao = new PurchaseRequestHdDao(ctx);
                PurchaseRequestDtDao requestDtDao = new PurchaseRequestDtDao(ctx);

                try
                {
                    string[] listParam = hdnParam.Value.Split(',');
                    foreach (string param in listParam)
                    {
                        int PurchaseOrderID = Convert.ToInt32(param);
                        PurchaseOrderHd entity = entityDao.Get(PurchaseOrderID);
                        string filterReceive = string.Format("PurchaseOrderID = {0} AND GCItemDetailStatus != '{1}'", entity.PurchaseOrderID, Constant.TransactionStatus.VOID);
                        filterReceive += string.Format(" AND PurchaseReceiveID IN (SELECT PurchaseReceiveID FROM PurchaseReceiveHd WHERE GCTransactionStatus != '{0}')", Constant.TransactionStatus.VOID);
                        List<PurchaseReceiveDt> lstRDt = BusinessLayer.GetPurchaseReceiveDtList(filterReceive);
                        if (lstRDt.Count == 0)
                        {
                            string filterPOTermProcessed = string.Format("PurchaseOrderID = {0} AND GCTransactionStatus NOT IN ('{1}','{2}','{3}')",
                                                                        entity.PurchaseOrderID, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.APPROVED, Constant.TransactionStatus.VOID);
                            List<PurchaseOrderTerm> lstTermProcessed = BusinessLayer.GetPurchaseOrderTermList(filterPOTermProcessed, ctx);
                            if (lstTermProcessed.Count == 0)
                            {
                                List<PurchaseOrderDt> entityDt = BusinessLayer.GetPurchaseOrderDtList(String.Format(
                                                                                "PurchaseOrderID = {0} AND GCItemDetailStatus != '{1}' AND IsDeleted = 0",
                                                                                entity.PurchaseOrderID, Constant.TransactionStatus.VOID), ctx);

                                foreach (PurchaseOrderDt obj in entityDt)
                                {
                                    obj.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                                    obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    entityDtDao.Update(obj);
                                }

                                entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                entityDao.Update(entity);


                                string filterPOTerm = string.Format("PurchaseOrderID = {0} AND GCTransactionStatus IN ('{1}','{2}')",
                                                                            entity.PurchaseOrderID, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.APPROVED);
                                List<PurchaseOrderTerm> lstTerm = BusinessLayer.GetPurchaseOrderTermList(filterPOTerm, ctx);
                                foreach (PurchaseOrderTerm poTerm in lstTerm)
                                {
                                    poTerm.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                    poTerm.GCVoidReason = Constant.DeleteReason.OTHER;
                                    poTerm.VoidReason = "VOID from menu Pembatalan Pemesanan Barang";
                                    poTerm.VoidDate = DateTime.Now;
                                    poTerm.VoidBy = AppSession.UserLogin.UserID;
                                    poTerm.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    entityTermDao.Update(poTerm);
                                }

                                List<PurchaseRequestPO> lstPurchaseRequestPO = BusinessLayer.GetPurchaseRequestPOList(string.Format("PurchaseOrderID = {0}", entity.PurchaseOrderID), ctx);
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
                                                entityPRDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();
                                                requestDtDao.Update(entityPRDt);
                                            }

                                            int Count = BusinessLayer.GetPurchaseRequestDtRowCount(string.Format(
                                                                                "PurchaseRequestID = {0} AND IsDeleted = 0 AND GCItemDetailStatus NOT IN ('{1}','{2}')",
                                                                                entityPRDt.PurchaseRequestID, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID), ctx);
                                            if (Count != 0)
                                            {
                                                PurchaseRequestHd entityPRHd = BusinessLayer.GetPurchaseRequestHdList(string.Format(
                                                                                                    "PurchaseRequestID = {0} AND GCTransactionStatus != '{1}'",
                                                                                                    entityPRDt.PurchaseRequestID, Constant.TransactionStatus.VOID), ctx).FirstOrDefault();

                                                entityPRHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                                                entityPRHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();
                                                requestHdDao.Update(entityPRHd);
                                            }
                                        }
                                    }
                                }
                                ctx.CommitTransaction();
                            }
                            else
                            {
                                errMessage = "Maaf, PO tidak dapat dibatalkan karena sudah diproses tukar faktur untuk termin.";
                                result = false;
                                ctx.RollBackTransaction();
                            }
                        }
                        else
                        {
                            errMessage = "Maaf, PO tidak dapat dibatalkan karena sudah diproses.";
                            result = false;
                            ctx.RollBackTransaction();
                        }
                    }
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
            }
            return result;
        }
    }
}
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
    public partial class PurchaseOrderApprovalList : BasePageTrx
    {
        protected int PageCount = 1;
        protected string filterExpressionLocation = "";

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

        protected override void InitializeDataControl()
        {
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

            MPTrx master = (MPTrx)Master;
            menu = ((MPMain)master.Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            if (hdnLocationIDFrom.Value.ToString() != "" && hdnLocationIDFrom.Value.ToString() != "0" && hdnLocationIDFrom.Value.ToString() != null)
            {
                filterExpression += String.Format("GCTransactionStatus = '{0}' AND TransactionCode = '{1}' AND LocationID = {2}",
                    Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionCode.PURCHASE_ORDER, hdnLocationIDFrom.Value);
            }
            else
            {
                filterExpression += String.Format("GCTransactionStatus = '{0}' AND TransactionCode = '{1}'",
                    Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionCode.PURCHASE_ORDER);
            }

            //int count = BusinessLayer.GetLocationUserRowCount(string.Format("UserID = {0} AND IsDeleted = 0", AppSession.UserLogin.UserID));

            //if (count > 0)
            //{
            //    filterExpression += string.Format(" AND LocationID IN (SELECT LocationID FROM LocationUser WHERE UserID = {0} AND IsDeleted = 0)", 
            //        AppSession.UserLogin.UserID);
            //}
            //else
            //{
            //    count = BusinessLayer.GetLocationUserRoleRowCount(string.Format(
            //        "RoleID IN (SELECT RoleID FROM UserInRole WHERE UserID = {0} AND HealthcareID = '{1}') AND IsDeleted = 0", 
            //        AppSession.UserLogin.UserID, AppSession.UserLogin.HealthcareID));
            //    if (count > 0)
            //        filterExpression += string.Format(
            //            " AND LocationID IN (SELECT LocationID FROM LocationUserRole WHERE RoleID IN (SELECT RoleID FROM UserInRole WHERE UserID = {0} AND HealthcareID = '{1}') AND IsDeleted = 0)", 
            //            AppSession.UserLogin.UserID, AppSession.UserLogin.HealthcareID);
            //}

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPurchaseOrderHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPurchaseOrderHd> lstEntity = BusinessLayer.GetvPurchaseOrderHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "PurchaseOrderNo DESC");
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

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseOrderHdDao purchaseHdDao = new PurchaseOrderHdDao(ctx);
            PurchaseOrderDtDao purchaseDtDao = new PurchaseOrderDtDao(ctx);
            PurchaseRequestHdDao entityPRHDDao = new PurchaseRequestHdDao(ctx);
            PurchaseRequestDtDao entityPRDTDao = new PurchaseRequestDtDao(ctx);

            if (type == "approve")
            {
                try
                {
                    string filterExpressionPurchaseOrderDt = String.Format("PurchaseOrderID IN ({0}) AND GCItemDetailStatus = '{1}' AND IsDeleted = 0",
                        hdnParam.Value, Constant.TransactionStatus.OPEN);
                    List<PurchaseOrderDt> lstPurchaseOrderDt = BusinessLayer.GetPurchaseOrderDtList(filterExpressionPurchaseOrderDt, ctx);
                    if (lstPurchaseOrderDt.Count == 0)
                    {
                        string filterExpressionPurchaseOrderHd = String.Format("PurchaseOrderID IN ({0})", hdnParam.Value);
                        List<PurchaseOrderHd> lstPurchaseOrderHd = BusinessLayer.GetPurchaseOrderHdList(filterExpressionPurchaseOrderHd, ctx);
                        foreach (PurchaseOrderHd purchaseHd in lstPurchaseOrderHd)
                        {
                            purchaseHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                            purchaseHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            purchaseHd.ApprovedDate = DateTime.Now;
                            purchaseHd.ApprovedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            purchaseHdDao.Update(purchaseHd);

                            List<PurchaseRequestPO> lstPurchaseRequestPO = BusinessLayer.GetPurchaseRequestPOList(string.Format("PurchaseOrderID = {0}", purchaseHd.PurchaseOrderID), ctx);
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

                        string filterExpressionPurchaseOrderDtUpdate = String.Format("PurchaseOrderID IN ({0}) AND IsDeleted = 0", hdnParam.Value);
                        List<PurchaseOrderDt> lstPurchaseOrderDtUpdate = BusinessLayer.GetPurchaseOrderDtList(filterExpressionPurchaseOrderDtUpdate, ctx);
                        foreach (PurchaseOrderDt purchaseDt in lstPurchaseOrderDtUpdate)
                        {
                            purchaseDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                            purchaseDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            purchaseDtDao.Update(purchaseDt);
                        }
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        errMessage = "Maaf, masih ada Item yang statusnya masih OPEN";
                        result = false;
                        ctx.RollBackTransaction();
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
                return result;
            }
            else
            {
                try
                {
                    string[] listParam = hdnParam.Value.Split(',');
                    foreach (string param in listParam)
                    {
                        int PurchaseOrderID = Convert.ToInt32(param);
                        PurchaseOrderHd entity = purchaseHdDao.Get(PurchaseOrderID);
                        entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        purchaseHdDao.Update(entity);

                        string filterExpressionPurchaseOrderHd = String.Format("PurchaseOrderID IN ({0})", param);
                        List<PurchaseOrderDt> lstPurchaseOrderDt = BusinessLayer.GetPurchaseOrderDtList(filterExpressionPurchaseOrderHd);
                        foreach (PurchaseOrderDt purchaseDt in lstPurchaseOrderDt)
                        {
                            if (purchaseDt.GCItemDetailStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                            {
                                purchaseDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                                purchaseDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                purchaseDtDao.Update(purchaseDt);
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
}
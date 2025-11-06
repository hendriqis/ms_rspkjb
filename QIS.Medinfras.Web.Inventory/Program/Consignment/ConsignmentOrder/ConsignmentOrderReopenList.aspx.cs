using System;
using System.Collections.Generic;
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
    public partial class ConsignmentOrderReopenList : BasePageTrx
    {
        protected int PageCount = 1;
        protected string filterExpressionLocation = "";

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.CONSIGNMENT_ORDER_REOPEN;
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
            hdnIsAPConsignmentFromOrder.Value = AppSession.IsAPConsignmentFromOrder;

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
            {
                filterExpression += " AND ";
            }

            if (hdnLocationIDFrom.Value.ToString() != "" && hdnLocationIDFrom.Value.ToString() != "0" && hdnLocationIDFrom.Value.ToString() != null)
            {
                filterExpression += String.Format("GCTransactionStatus = '{0}' AND TransactionCode = '{1}' AND LocationID = {2}",
                    Constant.TransactionStatus.APPROVED, Constant.TransactionCode.CONSIGNMENT_ORDER, hdnLocationIDFrom.Value);
            }
            else
            {
                filterExpression += String.Format("GCTransactionStatus = '{0}' AND TransactionCode = '{1}'",
                    Constant.TransactionStatus.APPROVED, Constant.TransactionCode.CONSIGNMENT_ORDER);
            }

            if (hdnIsAPConsignmentFromOrder.Value == "0")
            {
                filterExpression += string.Format(" AND PurchaseOrderID IN (SELECT PurchaseOrderID FROM PurchaseOrderDt WHERE ReceivedQuantity = 0 AND (ReceivedInformation IS NULL OR ReceivedInformation = '') AND IsDeleted = 0)");
            }
            else
            {
                filterExpression += string.Format(" AND PurchaseOrderID NOT IN (SELECT ISNULL(PurchaseOrderID,0) FROM PurchaseInvoiceDt WHERE IsDeleted = 0 AND PurchaseInvoiceID NOT IN (SELECT PurchaseInvoiceID FROM PurchaseInvoiceHD WHERE GCTransactionStatus = '{0}' AND LocationID = '{1}'))", Constant.TransactionStatus.VOID, hdnLocationIDFrom.Value);
            }

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
            AuditLogDao entityAuditLogDao = new AuditLogDao(ctx);
            AuditLog entityAuditLog = new AuditLog();

            if (type == "decline")
            {
                try
                {
                    string[] listParam = hdnParam.Value.Split(',');
                    foreach (string param in listParam)
                    {
                        int PurchaseOrderID = Convert.ToInt32(param);
                        PurchaseOrderHd entity = purchaseHdDao.Get(PurchaseOrderID);
                        if (entity.GCTransactionStatus == Constant.TransactionStatus.APPROVED)
                        {
                            entityAuditLog.OldValues = JsonConvert.SerializeObject(entity);
                            entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            purchaseHdDao.Update(entity);
                            entityAuditLog.ObjectType = Constant.BusinessObjectType.PURCHASE_ORDER;
                            entityAuditLog.NewValues = JsonConvert.SerializeObject(entity);
                            entityAuditLog.UserID = AppSession.UserLogin.UserID;
                            entityAuditLog.LogDate = DateTime.Now;
                            entityAuditLog.TransactionID = entity.PurchaseOrderID;
                            entityAuditLogDao.Insert(entityAuditLog);

                            string filterExpressionPurchaseOrderHd = String.Format("PurchaseOrderID IN ({0})", param);
                            List<PurchaseOrderDt> lstPurchaseOrderDt = BusinessLayer.GetPurchaseOrderDtList(filterExpressionPurchaseOrderHd);
                            foreach (PurchaseOrderDt purchaseDt in lstPurchaseOrderDt)
                            {
                                if (purchaseDt.GCItemDetailStatus == Constant.TransactionStatus.APPROVED)
                                {
                                    purchaseDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                                    purchaseDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    purchaseDtDao.Update(purchaseDt);
                                }
                            }
                            ctx.CommitTransaction();
                        }
                        else
                        {
                            result = false;
                            errMessage = "Pemesanan konsinyasi tidak dapat diubah. Harap refresh halaman ini.";
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
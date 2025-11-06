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
    public partial class PurchaseOrderReopenList : BasePageTrx
    {
        protected int PageCount = 1;
        protected string filterExpressionLocation = "";

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.PURCHASE_ORDER_REOPEN;
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
            PurchaseOrderTermDao entityTermDao = new PurchaseOrderTermDao(ctx);

            if (type == "decline")
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
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        purchaseHdDao.Update(entity);
                        
                        string filterPOTermProcessed = string.Format("PurchaseOrderID = {0} AND GCTransactionStatus NOT IN ('{1}','{2}','{3}')",
                                                                    entity.PurchaseOrderID, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.APPROVED, Constant.TransactionStatus.VOID);
                        List<PurchaseOrderTerm> lstTermProcessed = BusinessLayer.GetPurchaseOrderTermList(filterPOTermProcessed, ctx);
                        if (lstTermProcessed.Count == 0)
                        {
                            string filterExpressionPurchaseOrderHd = String.Format("PurchaseOrderID IN ({0})", param);
                            List<PurchaseOrderDt> lstPurchaseOrderDt = BusinessLayer.GetPurchaseOrderDtList(filterExpressionPurchaseOrderHd, ctx);
                            foreach (PurchaseOrderDt purchaseDt in lstPurchaseOrderDt)
                            {
                                if (purchaseDt.GCItemDetailStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                                {
                                    purchaseDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                                    purchaseDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    purchaseDtDao.Update(purchaseDt);
                                }
                            }

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

                        }
                        else
                        {
                            result = false;
                            errMessage = "Maaf, PO tidak dapat dibatalkan karena sudah diproses tukar faktur untuk termin.";
                            break;
                        }
                    }

                    if (result)
                    {
                        ctx.CommitTransaction();
                    }
                    else
                    {
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
            }
            return result;
        }
    }
}
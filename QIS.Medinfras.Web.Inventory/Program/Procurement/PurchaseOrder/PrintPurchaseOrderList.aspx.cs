using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using System.Data;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class PrintPurchaseOrderList : BasePageTrx
    {
        protected int PageCount = 1;
        protected string filterExpressionLocation = "";

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.PRINT_PURCHASE_ORDER;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        protected override void InitializeDataControl()
        {
            filterExpressionLocation = string.Format("{0};{1};{2};", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.TransactionCode.PURCHASE_ORDER);

            SettingParameterDt setvarDTR = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IM_DEFAULT_ROLE_OFFICER_LOGISTIC);

            if (setvarDTR.ParameterValue != "" && setvarDTR.ParameterValue != "0" && setvarDTR.ParameterValue != null)
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

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            //string filterExpression = hdnFilterExpression.Value;
            //if (filterExpression != "")
            //    filterExpression += " AND ";

            string locationID = hdnLocationIDFrom.Value.ToString();
            string filterExpression;
            if (locationID != "" && locationID != null)
            {
                filterExpression = String.Format("GCTransactionStatus IN ('{0}') AND TransactionCode = '{1}' AND LocationID = {2} AND PrintNumber = 0",
                    Constant.TransactionStatus.APPROVED, Constant.TransactionCode.PURCHASE_ORDER, locationID);
            }
            else
            {
                filterExpression = String.Format("GCTransactionStatus IN ('{0}') AND TransactionCode = '{1}' AND PrintNumber = 0",
                    Constant.TransactionStatus.APPROVED, Constant.TransactionCode.PURCHASE_ORDER);
            }

            if (hdnSupplierID.Value != null && hdnSupplierID.Value != "")
            {
                if (filterExpression != null && filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression += string.Format("BusinessPartnerID = {0}", hdnSupplierID.Value);
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
            return result;
        }
    }
}
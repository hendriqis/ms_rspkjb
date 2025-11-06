using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxPivotGrid;
using QIS.Medinfras.Web.Common;
using DevExpress.Utils;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxEditors;
using System.Web.UI.HtmlControls;


namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class PurchaseReceiveInfoPerSupplier : BasePageTrx
    {
        protected int PageCount = 1;
        protected string filterExpressionLocation = "";
        //protected string filterExpressionLocationTo = "";
        
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.ITEM_PURCHASE_RECEIVE_PER_SUPPLIER;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        //protected string OnGetLocationFilterExpression()
        //{
        //    return string.Format("{0};{1};;", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID);
        //}

        protected override void InitializeDataControl()
        {
            filterExpressionLocation = string.Format("{0};{1};{2};", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.TransactionCode.PURCHASE_RECEIVE);
            txtDateFrom.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDateTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            BindGridView(1, true, ref PageCount);

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref PageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridView(1, true, ref PageCount);
                    result = "refresh|" + PageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "";
            if (hdnBusinessPartnerID.Value != "")
            {
                filterExpression = String.Format("SupplierID = {0} AND ReceivedDate BETWEEN '{1}' AND '{2}' AND GCItemDetailStatus != '{3}'", hdnBusinessPartnerID.Value, Helper.GetDatePickerValue(txtDateFrom.Text), Helper.GetDatePickerValue(txtDateTo.Text),Constant.TransactionStatus.VOID);
                if (hdnLocationID.Value != null && hdnLocationID.Value != "")
                    filterExpression += String.Format(" AND LocationID = {0}",hdnLocationID.Value);
                if (hdnFilterExpressionQuickSearch.Value == "Search")
                    hdnFilterExpressionQuickSearch.Value = " ";
                if (hdnFilterExpressionQuickSearch.Value != "")
                    filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            }
            else
                filterExpression = "1 = 0";

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPurchaseReceiveDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 10);
            }

            List<vPurchaseReceiveDt> lstPurchaseReceive = BusinessLayer.GetvPurchaseReceiveDtList(filterExpression, 10, pageIndex, "PurchaseReceiveNo");
            grdView.DataSource = lstPurchaseReceive;
            grdView.DataBind();
        }
    }
}
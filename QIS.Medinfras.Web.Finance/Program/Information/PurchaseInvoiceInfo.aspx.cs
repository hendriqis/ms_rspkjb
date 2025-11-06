using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using DevExpress.Utils;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxEditors;


namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class PurchaseInvoiceInfo : BasePageList
    {
        protected int PageCount = 1;
        protected string filterExpressionLocation = "";
        protected string filterExpressionLocationTo = "";
        
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.PURCHASE_INVOICE_INFORMATION;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            txtDateFrom.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDateTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            BindGridView(1, true, ref PageCount);
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

        protected string GetFilterExpression()
        {
            string filterExpression = "1 = 0";

            //if (txtFromLocationCode.Text != "" || txtToLocationCode.Text != "")
            //{
            //    if (txtFromLocationCode.Text != "")
            //        filterExpression += string.Format("FromLocationID = {0}", hdnFromLocationID.Value);
            //    if (txtToLocationCode.Text != "")
            //    {
            //        if (filterExpression != "")
            //            filterExpression += " AND ";
            //        filterExpression += String.Format("ToLocationID = {0}", hdnToLocationID.Value);
            //    }
            //    if (txtDateFrom.Text != "" && txtDateTo.Text != "")
            //        filterExpression += String.Format(" AND DeliveryDate BETWEEN '{0}' AND '{1}'", Helper.GetDatePickerValue(txtDateFrom.Text), Helper.GetDatePickerValue(txtDateTo.Text));

            //    if (hdnFilterExpressionQuickSearch.Value == "Search")
            //        hdnFilterExpressionQuickSearch.Value = " ";
            //    if (hdnFilterExpressionQuickSearch.Value != "")
            //        filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);

            //    filterExpression += " AND IsDeleted = 0";
            //}
            //else
            //    filterExpression = "1 = 0";
            return filterExpression;
        }
        
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();

            //if (isCountPageCount)
            //{
            //    int rowCount = BusinessLayer.GetvItemDistributionDtRowCount(filterExpression);
            //    pageCount = Helper.GetPageCount(rowCount, 10);
            //}

            String periode = String.Format("{0}|{1}", Helper.GetDatePickerValue(txtDateFrom.Text).ToString("yyyyMMdd"), Helper.GetDatePickerValue(txtDateTo.Text).ToString("yyyyMMdd"));
            List<GetMutasiHutang> lstEntity = BusinessLayer.GetMutasiHutangList(periode);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
    }
}
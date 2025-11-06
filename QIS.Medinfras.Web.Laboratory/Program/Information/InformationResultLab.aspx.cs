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

namespace QIS.Medinfras.Web.Laboratory.Program
{
    public partial class InformationResultLab : BasePageTrx
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Laboratory.INFORMATION_RESULT_LABORATORIUM;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected string GetRefreshGridInterval()
        {
            return AppSession.RefreshGridInterval;
        }

        protected override void InitializeDataControl()
        {
            txtFromDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtToDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

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

        protected string GetFilterExpression()
        {
            string filterExpression = "";

            if (hdnFilterExpression.Value != "" || hdnFilterExpression.Value != "")
            {
                if (hdnFilterExpressionQuickSearch.Value == "Search")
                    hdnFilterExpressionQuickSearch.Value = " ";
                if (hdnFilterExpressionQuickSearch.Value != "")
                    filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);

                filterExpression += " AND IsDeleted = 0";
            }
            else
                filterExpression = "1 = 0";
            hdnFilterExpression.Value = filterExpression;
            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string fromDate = Helper.GetDatePickerValue(txtFromDate).ToString("yyyyMMdd");
            string toDate = Helper.GetDatePickerValue(txtToDate).ToString("yyyyMMdd");
            String filterExpression = String.Format("TransactionCode = '{0}'  AND CONVERT(date, TransactionDate) BETWEEN '{1}' AND '{2}'", Constant.TransactionCode.LABORATORY_CHARGES, fromDate, toDate);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientChargesHd7RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 10);
            }

            if (hdnFilterExpressionQuickSearch.Value != null && hdnFilterExpressionQuickSearch.Value != "")
            {
                filterExpression += String.Format(" AND {0} ", hdnFilterExpressionQuickSearch.Value);
            }

            List<vPatientChargesHd7> lstEntity = BusinessLayer.GetvPatientChargesHd7List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "TransactionID");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
    }
}
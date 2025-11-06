using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class ChartOfAccountList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.CHART_OF_ACCOUNT;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            hdnFilterExpression.Value = filterExpression;
            hdnID.Value = keyValue;
            filterExpression = GetFilterExpression();
            if (keyValue != "")
            {
                int row = BusinessLayer.GetvChartOfAccountRowIndex(filterExpression, keyValue) + 1;
                CurrPage = Helper.GetPageCount(row, Constant.GridViewPageSize.GRID_MASTER);
            }
            else
                CurrPage = 1;

            BindGridView(CurrPage, true, ref PageCount);
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "GLAccount No", "GLAccount Name" };
            fieldListValue = new string[] { "GLAccountNo", "GLAccountName" };
        }

        private string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("HealthcareID = '{0}' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID);
            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvChartOfAccountRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vChartOfAccount> lstEntity = BusinessLayer.GetvChartOfAccountList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "GLAccountNo ASC");
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

        protected override bool OnAddRecord(ref string url, ref string errMessage)
        {
            url = ResolveUrl("~/Program/Master/ChartOfAccount/ChartOfAccountEntry.aspx");
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                url = ResolveUrl(string.Format("~/Program/Master/ChartOfAccount/ChartOfAccountEntry.aspx?id={0}", hdnID.Value));
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                ChartOfAccount entity = BusinessLayer.GetChartOfAccount(Convert.ToInt32(hdnID.Value));
                GLBalance entityBalance = BusinessLayer.GetGLBalanceList(string.Format("GLAccount = {0} AND BalanceEND != 0 ", entity.GLAccountID)).LastOrDefault();

                if (entityBalance != null)
                {
                    errMessage = Helper.GetErrorMessageText(this, "Chart of Account tidak bisa dihapus karena sudah memiliki saldo");
                    return false;
                }
                else
                {
                    entity.IsDeleted = true;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateChartOfAccount(entity);
                    return true;
                }
            }
            errMessage = Helper.GetErrorMessageText(this, "Chart of Account tidak bisa dihapus karena sudah memiliki saldo");
            return false;
        }
    }
}
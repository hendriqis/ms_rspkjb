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
using System.Diagnostics;
using System.Reflection;
using QIS.Medinfras.ReportDesktop;


namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class ReportConfigurationUserList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.REPORT_CONFIGURATION_USER;
        }
        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            hdnFilterExpression.Value = filterExpression;
            hdnID.Value = keyValue;
            filterExpression = GetFilterExpression();
            if (keyValue != "")
            {
                int row = BusinessLayer.GetvReportMasterRowIndex(filterExpression, keyValue, "ReportCode") + 1;
                CurrPage = Helper.GetPageCount(row, Constant.GridViewPageSize.GRID_MASTER);
            }
            else
                CurrPage = 1;
            List<QIS.Medinfras.Data.Service.Module> lst = BusinessLayer.GetModuleList("");
            Methods.SetComboBoxField<QIS.Medinfras.Data.Service.Module>(cboModuleName, lst, "ModuleName", "ModuleID");
            BindGridView(CurrPage, true, ref PageCount);
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "Report Title", "Report Code", "Module Name" };
            fieldListValue = new string[] { "ReportTitle1 ReportTitle2", "ReportCode", "ModuleName ModuleID" };
        }

        private string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;
            if (cboModuleName.Value != null)
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression += string.Format("ModuleID = '{0}'", cboModuleName.Value);
            }
            if (filterExpression != "")
            {
                filterExpression += " AND ";
            }
            filterExpression += "IsDeleted = 0";
            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvReportMasterRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vReportMaster> lstEntity = BusinessLayer.GetvReportMasterList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ReportCode");
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

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                url = ResolveUrl(string.Format("~/Program/ControlPanel/ReportConfigurationUser/ReportConfigurationUserEntry.aspx?id={0}", hdnID.Value));
                return true;
            }
            return false;
        }
    }
}
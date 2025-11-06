using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class SettingParameterList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.SYSTEM_PARAMETER;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            hdnFilterExpression.Value = filterExpression;
            hdnID.Value = keyValue;
            if (keyValue != "")
            {
                int row = BusinessLayer.GetSettingParameterRowIndex(filterExpression, keyValue) + 1;
                CurrPage = Helper.GetPageCount(row, Constant.GridViewPageSize.GRID_MASTER);
            }
            else
                CurrPage = 1;
            List<Module> lst = BusinessLayer.GetModuleList("1=1 ORDER BY ModuleName");
            Methods.SetComboBoxField<Module>(cboModuleName, lst, "ModuleName", "ModuleID");
            BindGridView(CurrPage, true, ref PageCount);
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "ParameterName", "ParameterCode", "ParameterValue" };
            fieldListValue = new string[] { "ParameterName", "ParameterCode", "ParameterValue" };
        }

        private string GenerateFilterExpression()
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
            if (!AppSession.UserLogin.IsSysAdmin)
            {
                if (filterExpression != "")
                    filterExpression += " AND ";
                filterExpression += "IsUsedBySystem = 0";
            }
            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GenerateFilterExpression();
            
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvSettingParameterRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vSettingParameter> lstEntity = BusinessLayer.GetvSettingParameterList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
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
            url = ResolveUrl("~/Program/ControlPanel/SettingVariables/SettingParameter/SettingParameterEntry.aspx");
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                url = ResolveUrl(string.Format("~/Program/ControlPanel/SettingVariables/SettingParameter/SettingParameterEntry.aspx?id={0}", hdnID.Value));
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                SettingParameter entity = BusinessLayer.GetSettingParameter(hdnID.Value);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateSettingParameter(entity);
                return true;
            }
            return false;
        }

        protected void cbpViewDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            List<vSettingParameterDt> lstSettingParameterDt = BusinessLayer.GetvSettingParameterDtList(string.Format("ParameterCode = '{0}'", hdnExpandID.Value));
            grdDetail.DataSource = lstSettingParameterDt;
            grdDetail.DataBind();
        }
    }
}

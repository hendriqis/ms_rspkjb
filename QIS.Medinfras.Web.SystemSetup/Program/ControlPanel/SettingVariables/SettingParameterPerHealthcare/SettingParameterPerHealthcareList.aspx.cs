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
    public partial class SettingParameterPerHealthcareList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.PER_HEALTHCARE_PARAMETER;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            List<Healthcare> lstHealthcare = BusinessLayer.GetHealthcareList("");
            Methods.SetComboBoxField<Healthcare>(cboHealthcare, lstHealthcare, "HealthcareName", "HealthcareID");
            cboHealthcare.SelectedIndex = 0;

            hdnFilterExpression.Value = filterExpression;
            hdnID.Value = keyValue;
            
            if (keyValue != "")
            {
                int row = BusinessLayer.GetHealthcareParameterRowIndex(filterExpression, keyValue) + 1;
                CurrPage = Helper.GetPageCount(row, Constant.GridViewPageSize.GRID_MASTER);
            }
            else
                CurrPage = 1;

            BindGridView(CurrPage, true, ref PageCount);
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "Parameter Code", "Parameter Name", "Parameter Value" };
            fieldListValue = new string[] { "ParameterCode", "ParameterName", "ParameterValue" };
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
            {
                filterExpression += " AND ";
            }
            filterExpression += "HealthcareID = '" + cboHealthcare.Value.ToString() + "'";
            
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetHealthcareParameterRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<HealthcareParameter> lstEntity = BusinessLayer.GetHealthcareParameterList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
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
            url = ResolveUrl("~/Program/ControlPanel/SettingVariables/SettingParameterPerHealthcare/SettingParameterPerHealthcareEntry.aspx");
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                url = ResolveUrl(string.Format("~/Program/ControlPanel/SettingVariables/SettingParameterPerHealthcare/SettingParameterPerHealthcareEntry.aspx?id={0}", hdnID.Value+ "|" + cboHealthcare.Value.ToString()));
                return true;
            }
            return false;
        }

        //protected override bool OnDeleteRecord(ref string errMessage)
        //{
        //    if (hdnID.Value.ToString() != "")
        //    {
        //        HealthcareParameter entity = BusinessLayer.GetHealthcareParameter(cboHealthcare.Value.ToString(),hdnID.Value);
        //        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
        //        BusinessLayer.UpdateHealthcareParameter(entity);
        //        return true;
        //    }
        //    return false;
        //}
    }
}
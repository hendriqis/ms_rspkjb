using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.EmergencyCare.Program
{
    public partial class PatientDischargeList : BasePageContent
    {
        public override string OnGetMenuCode()
        {
            string id = Page.Request.QueryString["id"];
            switch (id)
            {
                case "pd": return Constant.MenuCode.EmergencyCare.PATIENT_EMERGENCY_DISCHARGE;
                default: return Constant.MenuCode.EmergencyCare.PATIENT_EMERGENCY_DISCHARGE;
            }
        }
        private GetUserMenuAccess menu;
        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }
        protected string GetRefreshGridInterval()
        {
            return refreshGridInterval;
        }
        private string refreshGridInterval = "";
        protected int PageCount = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;

                hdnIsPatientDischarge.Value = "0";
                if (Page.Request.QueryString["id"] == "pd")
                    hdnIsPatientDischarge.Value = "1";

                txtVisitDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

                List<GetServiceUnitUserList> lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.Facility.EMERGENCY, "IsUsingRegistration = 1");
                Methods.SetComboBoxField<GetServiceUnitUserList>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                cboServiceUnit.SelectedIndex = 0;

                Helper.SetControlEntrySetting(cboServiceUnit, new ControlEntrySetting(true, true, true), "mpPatientList");
                Helper.SetControlEntrySetting(txtVisitDate, new ControlEntrySetting(true, true, true), "mpPatientList");

                BindGridView(1, true, ref PageCount);

                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());
            }
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

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression =
                string.Format("HealthcareServiceUnitID = {0} AND DepartmentID = '{1}' AND VisitDate = '{2}' AND (RegistrationID IN (SELECT RegistrationID FROM Bed WHERE RoomID IN (SELECT RoomID FROM vServiceUnitRoom WHERE DepartmentID = '{3}')))", 
                cboServiceUnit.Value, Constant.Facility.EMERGENCY, Helper.GetDatePickerValue(txtVisitDate).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.Facility.EMERGENCY);
            
            if (hdnFilterExpressionQuickSearch.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvConsultVisitRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_PATIENT_LIST);
            }

            List<vConsultVisit> lstEntity = BusinessLayer.GetvConsultVisitList(filterExpression, Constant.GridViewPageSize.GRID_PATIENT_LIST, pageIndex, "RegistrationID");
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }
    }
}
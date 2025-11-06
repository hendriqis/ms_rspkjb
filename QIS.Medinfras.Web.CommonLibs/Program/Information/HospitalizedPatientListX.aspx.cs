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
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.CommonLibs.Controls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class HospitalizedPatientListX : BasePageRegisteredPatient
    {
        public override string OnGetMenuCode()
        {
            if (Request.QueryString["id"] == "IP")
                return Constant.MenuCode.Inpatient.HOSPITALIZED_PATIENT_LIST;
            else
                return Constant.MenuCode.Nutrition.HOSPITALIZED_PATIENT_LIST;
        }

        protected string GetRefreshGridInterval()
        {
            return refreshGridInterval;
        }
        private string refreshGridInterval = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;

                List<GetServiceUnitUserList> lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.Facility.INPATIENT, "");
                lstServiceUnit.Insert(0, new GetServiceUnitUserList { HealthcareServiceUnitID = 0, ServiceUnitName = "" });
                Methods.SetComboBoxField<GetServiceUnitUserList>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                cboServiceUnit.SelectedIndex = 0;
                ((GridInpatientRegistrationCtl)grdInpatientReg).InitializeControl();
            }
        }

        public override string GetFilterExpression()
        {
            string filterExpression = "";
            if (cboServiceUnit.Value != null && cboServiceUnit.Value.ToString() != "0")
                filterExpression = string.Format("HealthcareServiceUnitID = {0}", cboServiceUnit.Value);
            else
                filterExpression = string.Format("DepartmentID = '{0}'", Constant.Facility.INPATIENT);
            filterExpression += string.Format(" AND GCRegistrationStatus NOT IN ('{0}','{1}','{2}','{3}')", Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, Constant.VisitStatus.OPEN,Constant.VisitStatus.DISCHARGED);
            if (hdnFilterExpressionQuickSearch.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            return filterExpression;
        }

        public override void OnGrdRowClick(string transactionNo)
        {
        }

        public override void LoadAllWords()
        {
            LoadWords();
        }
    }
}
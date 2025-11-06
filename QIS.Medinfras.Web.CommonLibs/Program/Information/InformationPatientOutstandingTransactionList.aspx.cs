using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class InformationPatientOutstandingTransactionList : BasePageRegisteredPatient
    {
        public override string OnGetMenuCode()
        {
            if (Request.QueryString["id"] == "IP")
               return Constant.MenuCode.Inpatient.REGISTERED_PATIENT_LIST;
            else
                return Constant.MenuCode.Inpatient.REGISTERED_PATIENT_LIST;
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
                grdPatientOutstanding.InitializeControl();

                string filterExpression = string.Format("IsUsingRegistration = 1 AND HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, Constant.Facility.INPATIENT);
                List<vHealthcareServiceUnit> lstHSU = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression);
                lstHSU = lstHSU.OrderBy(unit => unit.ServiceUnitName).ToList();
                lstHSU.Insert(0, new vHealthcareServiceUnit { HealthcareServiceUnitID = 0, ServiceUnitName = "" });
                Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnit, lstHSU, "ServiceUnitName", "HealthcareServiceUnitID");
                cboServiceUnit.SelectedIndex = 0;
                string filterExpressionBSU = string.Format("HealthcareID = '{0}' AND BusinessPartnerID IN (SELECT DISTINCT BusinessPartnerID FROM Registration WHERE GCRegistrationStatus = '{1}') AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, Constant.VisitStatus.CHECKED_IN);
                List<BusinessPartners> lstBSU = BusinessLayer.GetBusinessPartnersList(filterExpressionBSU);
                lstBSU.Insert(0, new BusinessPartners { BusinessPartnerID = 0, BusinessPartnerName = "" });
                Methods.SetComboBoxField<BusinessPartners>(cboBusinessPartner, lstBSU, "BusinessPartnerName", "BusinessPartnerID");
                cboBusinessPartner.SelectedIndex = 0;
            }
        }
        public override string GetFilterExpression()
        {
            string filterExpression = string.Format("DepartmentID = '{0}' AND GCRegistrationStatus = '{1}'", Constant.Facility.INPATIENT, Constant.VisitStatus.CHECKED_IN);
            //string filterExpression = string.Format("DepartmentID = '{0}'", Constant.Facility.INPATIENT);

            if (cboServiceUnit.Value != null && cboServiceUnit.Value.ToString() != "0")
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", cboServiceUnit.Value);

            if (cboBusinessPartner.Value != null && cboBusinessPartner.Value.ToString() != "0")
                filterExpression += string.Format(" AND BusinessPartnerID = {0}", cboBusinessPartner.Value);

            if (hdnFilterExpressionQuickSearch.Value == "Search")
                hdnFilterExpressionQuickSearch.Value = " ";
      
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
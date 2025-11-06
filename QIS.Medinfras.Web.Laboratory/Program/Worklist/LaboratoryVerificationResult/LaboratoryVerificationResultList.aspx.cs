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

namespace QIS.Medinfras.Web.Laboratory.Program
{
    public partial class LaboratoryVerificationResultList : BasePageRegisteredPatient
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Laboratory.LAB_RESULT_VERIFICATION;
        }
        private GetUserMenuAccess menu;
        public override bool IsShowRightPanel()
        {
            return false;
        }
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;

                string laboratoryID = BusinessLayer.GetSettingParameter(Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).ParameterValue;
                hdnID.Value = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND ServiceUnitID = {1}", AppSession.UserLogin.HealthcareID, laboratoryID))[0].HealthcareServiceUnitID.ToString();

                //string filterExpression = string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}'", AppSession.UserLogin.HealthcareID, Constant.Facility.DIAGNOSTIC);

                //if (filterExpression != "") 
                trPenunjangMedis.Style.Add("Display", "None");

                //List<vHealthcareServiceUnit> lstHServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression);
                //Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnit, lstHServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                //cboServiceUnit.SelectedIndex = 1;

                List<Department> lstDepartment = BusinessLayer.GetDepartmentList("IsHasRegistration = 1");
                Methods.SetComboBoxField<Department>(cboDepartment, lstDepartment, "DepartmentName", "DepartmentID");

                List<Variable> lstVariable = new List<Variable>();
                lstVariable.Add(new Variable { Code = "0", Value = "Semua" });
                lstVariable.Add(new Variable { Code = "1", Value = "Belum Diproses" });
                lstVariable.Add(new Variable { Code = "2", Value = "Sudah Diproses" });
                Methods.SetComboBoxField<Variable>(cboResultType, lstVariable, "Value", "Code");
                cboResultType.Value = "1";

                txtDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

                Helper.SetControlEntrySetting(txtDate, new ControlEntrySetting(true, true, false), "mpPatientList");
                Helper.SetControlEntrySetting(cboDepartment, new ControlEntrySetting(true, true, false), "mpPatientList");

                grdLabResultVerification.InitializeControl();
                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());
            }
        }

        public override string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;

            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("HealthcareServiceUnitID = {0}", hdnID.Value);

            if (!chkIsIgnoreDate.Checked)
            {
                filterExpression += string.Format(" AND TransactionDate = '{0}'", Helper.GetDatePickerValue(txtDate).ToString(Constant.FormatString.DATE_FORMAT_112));
            }

            string resultType = cboResultType.Value.ToString();
            switch (resultType)
            {
                case "1": filterExpression += string.Format(" AND LabGCTransactionStatus = '{0}'", Constant.TransactionStatus.WAIT_FOR_APPROVAL); break;
                case "2": filterExpression += string.Format(" AND LabGCTransactionStatus = '{0}'", Constant.TransactionStatus.PROCESSED); break;
                default: filterExpression += string.Format(" AND LabGCTransactionStatus IN ('{0}','{1}')", Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.WAIT_FOR_APPROVAL); break;
            }

            if (cboDepartment.Value != null)
                filterExpression += string.Format(" AND DepartmentID = '{0}'", cboDepartment.Value);

            if (hdnServiceUnitID.Value != "0" && hdnServiceUnitID.Value != "")
                filterExpression += string.Format(" AND VisitHealthcareServiceUnitID = {0}", hdnServiceUnitID.Value);

            if (hdnFilterExpressionQuickSearch.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);

            return filterExpression;
        }

        public override void LoadAllWords()
        {
            LoadWords();
        }

        public override void OnGrdRowClick(string oTransactionID)
        {
            vPatientChargesHd entity = BusinessLayer.GetvPatientChargesHdList(string.Format("TransactionID = {0}", oTransactionID)).FirstOrDefault();
            RegisteredPatient pt = new RegisteredPatient();
            pt.MRN = entity.MRN;
            pt.MedicalNo = entity.MedicalNo;
            pt.PatientName = entity.PatientName;
            pt.RegistrationID = entity.RegistrationID;
            pt.VisitID = entity.VisitID;
            pt.RegistrationNo = entity.RegistrationNo;
            pt.BusinessPartnerID = entity.BusinessPartnerID;
            pt.LinkedRegistrationID = entity.LinkedRegistrationID;
            pt.LinkedToRegistrationID = entity.LinkedToRegistrationID;
            pt.HealthcareServiceUnitID = entity.VisitHealthcareServiceUnitID;
            AppSession.RegisteredPatient = pt;

            string url = "";
            url = string.Format("~/Program/Worklist/LaboratoryVerificationResult/LaboratoryVerificationResultDetail.aspx?id={0}", oTransactionID);
            Response.Redirect(url);
        }
    }
}
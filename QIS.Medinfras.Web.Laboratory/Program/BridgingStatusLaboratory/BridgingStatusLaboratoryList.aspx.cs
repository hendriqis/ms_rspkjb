using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Controls;
using QIS.Medinfras.Web.CommonLibs.MasterPage;

namespace QIS.Medinfras.Web.Laboratory.Program
{
    public partial class BridgingStatusLaboratoryList : BasePageRegisteredPatient
    {
        List<Department> lstDept = null;

        public override string OnGetMenuCode()
        {
            switch (hdnModuleID.Value)
            {
                case Constant.Module.IMAGING: return Constant.MenuCode.Imaging.BRIDGING_STATUS;
                default: return Constant.MenuCode.Imaging.BRIDGING_STATUS;
            }
        }

        protected int PageCount = 1;
        protected int CurrPage = 1;
        protected string GetRefreshGridInterval()
        {
            return refreshGridInterval;
        }
        private string refreshGridInterval = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                #region Region Registrasi
                txtRealisationDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                cboMedicalDiagnostic.SelectedIndex = 0;
                Helper.SetControlEntrySetting(txtServiceUnitName, new ControlEntrySetting(false, false, false), "mpServiceUnit");

                string moduleName = Helper.GetModuleName();
                string ModuleID = Helper.GetModuleID(moduleName);
                hdnModuleID.Value = ModuleID;

                if (ModuleID == Constant.Module.EMERGENCY || ModuleID == Constant.Module.INPATIENT || ModuleID == Constant.Module.OUTPATIENT || ModuleID == Constant.Module.PHARMACY)
                {
                    string departmentID = "";
                    if (ModuleID == Constant.Module.EMERGENCY)
                        departmentID = Constant.Facility.EMERGENCY;
                    else if (ModuleID == Constant.Module.PHARMACY)
                        departmentID = Constant.Facility.PHARMACY;
                    else if (ModuleID == Constant.Module.INPATIENT)
                        departmentID = Constant.Facility.INPATIENT;
                    else
                        departmentID = Constant.Facility.OUTPATIENT;
                    List<GetServiceUnitUserList> lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, departmentID, "");
                    Methods.SetComboBoxField<GetServiceUnitUserList>(cboMedicalDiagnostic, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                    cboMedicalDiagnostic.SelectedIndex = 0;
                }
                else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging || AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                    trServiceUnitOrder.Style.Add("display", "none");
                else
                {
                    string filterExpression = string.Format("IsUsingRegistration = 1 AND HealthcareServiceUnitID NOT IN ({0},{1})", AppSession.MedicalDiagnostic.ImagingHealthcareServiceUnitID, AppSession.MedicalDiagnostic.LaboratoryHealthcareServiceUnitID);
                    List<GetServiceUnitUserList> lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.Facility.DIAGNOSTIC, filterExpression);
                    Methods.SetComboBoxField<GetServiceUnitUserList>(cboMedicalDiagnostic, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                    cboMedicalDiagnostic.SelectedIndex = 0;
                }

                if (ModuleID == Constant.Module.PHARMACY)
                    lstDept = BusinessLayer.GetDepartmentList("IsHasRegistration = 1");
                else
                    lstDept = BusinessLayer.GetDepartmentList(string.Format("IsHasRegistration = 1 AND DepartmentID != '{0}' AND IsActive = 1", Constant.Facility.PHARMACY));

                Methods.SetComboBoxField<Department>(cboPatientFrom, lstDept, "DepartmentName", "DepartmentID");
                cboPatientFrom.SelectedIndex = 0;
                #endregion

                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;
                ((GridPatientRegOrderCtl)grdRegisteredPatient).InitializeControl();
            }
        }

        public override string GetFilterExpression()
        {
            string filterExpression = string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}','{3}')", Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, Constant.VisitStatus.DISCHARGED, Constant.VisitStatus.OPEN);

            if (hdnServiceUnitID.Value != "0" && hdnServiceUnitID.Value != "")
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", hdnServiceUnitID.Value);

            if (cboPatientFrom.Value != null)
                filterExpression += string.Format(" AND DepartmentID = '{0}'", cboPatientFrom.Value);

            if (cboPatientFrom.Value != null && cboPatientFrom.Value.ToString() != Constant.Facility.INPATIENT)
                filterExpression += string.Format(" AND VisitDate = '{0}'", Helper.GetDatePickerValue(txtRealisationDate).ToString(Constant.FormatString.DATE_FORMAT_112));

            if (hdnFilterExpressionQuickSearchReg.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearchReg.Value);

            return filterExpression;
        }

        public override void LoadAllWords()
        {
            LoadWords();
        }

        public override void OnGrdRowClick(string visitID)
        {
            string healthcareServiceUnitID = "";
            if (hdnModuleID.Value == Constant.Module.LABORATORY || hdnModuleID.Value == Constant.Module.IMAGING)
                healthcareServiceUnitID = AppSession.MedicalDiagnostic.HealthcareServiceUnitID.ToString();
            else
                healthcareServiceUnitID = cboMedicalDiagnostic.Value.ToString();

            string url = "";
            url = string.Format("~/Program/BridgingStatusLaboratory/BridgingStatusLaboratoryEntry.aspx?id={0}|{1}", healthcareServiceUnitID, visitID);
            Response.Redirect(url);
        }
    }
}
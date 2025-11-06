using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Linq;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Controls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ChangeOrderStatusList : BasePageRegisteredPatient
    {
        List<Department> lstDept = null;

        public override string OnGetMenuCode()
        {
            switch (hdnModuleID.Value)
            {
                case Constant.Module.OUTPATIENT: return Constant.MenuCode.Outpatient.CHANGE_PATIENT_ORDER_STATUS;
                case Constant.Module.INPATIENT: return Constant.MenuCode.Inpatient.CHANGE_PATIENT_ORDER_STATUS;
                case Constant.Module.EMERGENCY: return Constant.MenuCode.EmergencyCare.CHANGE_PATIENT_ORDER_STATUS;
                case Constant.Module.IMAGING: return Constant.MenuCode.Imaging.CHANGE_PATIENT_ORDER_STATUS;
                case Constant.Module.LABORATORY: return Constant.MenuCode.Laboratory.CHANGE_PATIENT_ORDER_STATUS;
                case Constant.Module.MEDICAL_DIAGNOSTIC: return Constant.MenuCode.MedicalDiagnostic.CHANGE_PATIENT_ORDER_STATUS;
                default: return Constant.MenuCode.Outpatient.CHANGE_PATIENT_ORDER_STATUS;
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
                //cboMedicalDiagnostic.SelectedIndex = 0;
                Helper.SetControlEntrySetting(txtServiceUnitName, new ControlEntrySetting(false, false, false), "mpServiceUnit");

                string moduleName = Helper.GetModuleName();
                string ModuleID = Helper.GetModuleID(moduleName);
                hdnModuleID.Value = ModuleID;

                hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

                //if (ModuleID == Constant.Module.EMERGENCY || ModuleID == Constant.Module.INPATIENT || ModuleID == Constant.Module.OUTPATIENT || ModuleID == Constant.Module.PHARMACY)
                //{
                //    string departmentID = "";
                //    if (ModuleID == Constant.Module.EMERGENCY)
                //        departmentID = Constant.Facility.EMERGENCY;
                //    else if (ModuleID == Constant.Module.PHARMACY)
                //        departmentID = Constant.Facility.PHARMACY;
                //    else if (ModuleID == Constant.Module.INPATIENT)
                //        departmentID = Constant.Facility.INPATIENT;
                //    else
                //        departmentID = Constant.Facility.OUTPATIENT;

                //    List<GetServiceUnitUserList> lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, departmentID, "IsUsingRegistration = 1");
                //    Methods.SetComboBoxField<GetServiceUnitUserList>(cboMedicalDiagnostic, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                //    cboMedicalDiagnostic.SelectedIndex = 0;
                //}
                //else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging || AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                //{
                //    trServiceUnitOrder.Style.Add("display", "none");
                //}
                //else
                //{
                //    string filterExpression = string.Format("HealthcareServiceUnitID NOT IN ({0},{1}) AND IsUsingRegistration = 1", AppSession.MedicalDiagnostic.ImagingHealthcareServiceUnitID, AppSession.MedicalDiagnostic.LaboratoryHealthcareServiceUnitID);
                //    List<GetServiceUnitUserList> lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.Facility.DIAGNOSTIC, filterExpression);
                //    Methods.SetComboBoxField<GetServiceUnitUserList>(cboMedicalDiagnostic, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                //    cboMedicalDiagnostic.SelectedIndex = 0;
                //}


                if (ModuleID == Constant.Module.PHARMACY)
                    lstDept = BusinessLayer.GetDepartmentList("");
                else
                    lstDept = BusinessLayer.GetDepartmentList(string.Format("DepartmentID != '{0}' AND IsActive = 1 AND IsHasRegistration = 1", Constant.Facility.PHARMACY));

                Methods.SetComboBoxField<Department>(cboPatientFrom, lstDept, "DepartmentName", "DepartmentID");
                cboPatientFrom.SelectedIndex = 0;

                #endregion

                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;
                ((GridPatientRegOrderCtl)grdRegisteredPatient).InitializeControl();
            }
        }

        public override string GetFilterExpression()
        {
            string filterExpression = string.Format("GCVisitStatus IN ('{0}')", Constant.VisitStatus.CHECKED_IN);

            if (hdnServiceUnitID.Value != "0" && hdnServiceUnitID.Value != "")
            {
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", hdnServiceUnitID.Value);
            }
            else if (cboPatientFrom.Value != null)
            {
                filterExpression += string.Format(" AND DepartmentID = '{0}'", cboPatientFrom.Value);
            }

            if (cboPatientFrom.Value != null && cboPatientFrom.Value.ToString() != Constant.Facility.INPATIENT)
            {
                filterExpression += string.Format(" AND VisitDate = '{0}'", Helper.GetDatePickerValue(txtRealisationDate).ToString(Constant.FormatString.DATE_FORMAT_112));
            }

            if (hdnFilterExpressionQuickSearchReg.Value != "")
            {
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearchReg.Value);
            }

            return filterExpression;
        }

        public override void LoadAllWords()
        {
            LoadWords();
        }

        public override void OnGrdRowClick(string transactionNo)
        {
            ConsultVisit oVisit = BusinessLayer.GetConsultVisit(Convert.ToInt32(transactionNo));
            RegisteredPatient pt = new RegisteredPatient();
            pt.RegistrationID = oVisit.RegistrationID;
            pt.VisitID = oVisit.VisitID;
            AppSession.RegisteredPatient = pt;
            
            string healthcareServiceUnitID = "";
            if (hdnModuleID.Value == Constant.Module.LABORATORY || hdnModuleID.Value == Constant.Module.IMAGING)
                healthcareServiceUnitID = AppSession.MedicalDiagnostic.HealthcareServiceUnitID.ToString();
            else
                healthcareServiceUnitID = hdnServiceUnitID.Value.ToString();
            if (String.IsNullOrEmpty(healthcareServiceUnitID))
            {
                if (oVisit != null)
                {
                    healthcareServiceUnitID = oVisit.HealthcareServiceUnitID.ToString();
                }
            }
            if (healthcareServiceUnitID != "" || healthcareServiceUnitID != "0")
            {
                string url = "";
                url = string.Format("~/Libs/Program/Module/PatientManagement/ChangeOrderStatus/ChangeOrderStatusEntry.aspx?id={0}|{1}", healthcareServiceUnitID, transactionNo);
                Response.Redirect(url);
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }
    }
}
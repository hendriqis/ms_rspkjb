using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Controls;
using QIS.Medinfras.Web.CommonLibs.MasterPage;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class MedicalFileRequestList : BasePageCheckRegisteredPatient
    {
        public override string OnGetMenuCode()
        {
            string id = Page.Request.QueryString["id"];
            switch (id)
            {
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.MEDICAL_FOLDER_REQUEST;
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.MEDICAL_FOLDER_REQUEST;
                case Constant.Facility.OUTPATIENT: return Constant.MenuCode.Outpatient.MEDICAL_FOLDER_REQUEST;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.MEDICAL_FOLDER_REQUEST;
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.MEDICAL_FOLDER_REQUEST;
                    return Constant.MenuCode.MedicalDiagnostic.MEDICAL_FOLDER_REQUEST;
                default: return Constant.MenuCode.Outpatient.MEDICAL_FOLDER_REQUEST;
            }            
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected string GetRefreshGridInterval()
        {
            return refreshGridInterval;
        }
        private string refreshGridInterval = "";

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;

            string departmentID = Page.Request.QueryString["id"];
            if (departmentID == Constant.Facility.EMERGENCY)
            {
                hdnHealthCareServiceUnitID.Value = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, Constant.Facility.EMERGENCY))[0].HealthcareServiceUnitID.ToString();
                trServiceUnit.Style.Add("display", "none");
            }
            else if (departmentID == Constant.Facility.DIAGNOSTIC && AppSession.MedicalDiagnostic.MedicalDiagnosticType != MedicalDiagnosticType.OtherMedicalDiagnostic)
            {
                hdnHealthCareServiceUnitID.Value = AppSession.MedicalDiagnostic.HealthcareServiceUnitID.ToString();
                trServiceUnit.Style.Add("display", "none");
            }
            else
            {
                int serviceUnitUserCount = BusinessLayer.GetvServiceUnitUserRowCount(string.Format("DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2}", departmentID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID));
                string filterExpression = string.Format("IsUsingRegistration = 1 AND HealthcareID = '{0}' AND DepartmentID = '{1}'", AppSession.UserLogin.HealthcareID, departmentID);
                if (departmentID == Constant.Facility.DIAGNOSTIC)
                    filterExpression += string.Format(" AND HealthcareServiceUnitID NOT IN ({0},{1})", AppSession.MedicalDiagnostic.ImagingHealthcareServiceUnitID, AppSession.MedicalDiagnostic.LaboratoryHealthcareServiceUnitID);
                if (serviceUnitUserCount > 0)
                    filterExpression += string.Format(" AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vServiceUnitUser WHERE DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2})", departmentID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID);

                List<vHealthcareServiceUnit> lstServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression);
                Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                cboServiceUnit.SelectedIndex = 0;
            }

            txtRegistrationDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            ((GridPatientBillDetailCtl)grdRegisteredPatient).InitializeControl();
        }

        public override void LoadAllWords()
        {
            LoadWords();
        }

        public override string GetFilterExpression()
        {
            string healthcareServiceUnitID = "";
            string id = Page.Request.QueryString["id"];

            if (id == Constant.Facility.EMERGENCY || (id == Constant.Facility.DIAGNOSTIC && AppSession.MedicalDiagnostic.MedicalDiagnosticType != MedicalDiagnosticType.OtherMedicalDiagnostic))
                healthcareServiceUnitID = hdnHealthCareServiceUnitID.Value;
            else
                healthcareServiceUnitID = cboServiceUnit.Value.ToString();
            string filterExpression = string.Format("RegistrationDate = '{0}' AND DepartmentID = '{1}' AND HealthcareServiceUnitID = {2}", Helper.GetDatePickerValue(txtRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112), id, healthcareServiceUnitID);
            if (hdnFilterExpressionQuickSearch.Value == "Search")
                hdnFilterExpressionQuickSearch.Value = " ";
            if (hdnFilterExpressionQuickSearch.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            return filterExpression;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowNextPrev = IsAllowSave = IsAllowVoid = false;
        }

    }
}
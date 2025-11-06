using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using QIS.Medinfras.Web.CommonLibs.Controls;

namespace QIS.Medinfras.Web.MedicalCheckup.Program
{
    public partial class MCVisitList : BasePagePatientOrder
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalCheckup.PATIENT_PAGE;
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

                #region Region Registrasi
                txtRealisationDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                cboClinic.SelectedIndex = 0;
                Helper.SetControlEntrySetting(txtServiceUnitName, new ControlEntrySetting(false, false, false), "mpServiceUnit");

                if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                {
                    trServiceUnitOrder.Style.Add("display", "none");
                }
                else
                {
                    string filterExpression = string.Format("IsUsingRegistration = 1");
                    List<GetServiceUnitUserList> lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.Facility.MEDICAL_CHECKUP, filterExpression);
                    Methods.SetComboBoxField<GetServiceUnitUserList>(cboClinic, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                    cboClinic.SelectedIndex = 0;
                }

                List<Department> lstDept = BusinessLayer.GetDepartmentList(string.Format("IsHasRegistration = 1 AND DepartmentID != '{0}' AND IsActive = 1", Constant.Facility.PHARMACY));
                Methods.SetComboBoxField<Department>(cboPatientFrom, lstDept, "DepartmentName", "DepartmentID");
                cboPatientFrom.SelectedIndex = 0;
                #endregion

                ((GridPatientRegOrderCtl)grdRegisteredPatient).InitializeControl();

                Helper.SetControlEntrySetting(cboPatientFrom, new ControlEntrySetting(true, true, false), "mpPatientList");
                Helper.SetControlEntrySetting(txtRealisationDate, new ControlEntrySetting(true, true, true), "mpPatientList");
            }
        }
        public override bool IsShowRightPanel()
        {
            return false;
        }

        public override string GetFilterExpression()
        {
            string filterExpression = string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}','{3}')",
                Constant.VisitStatus.OPEN, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.DISCHARGED, Constant.VisitStatus.CLOSED);

            if (hdnServiceUnitID.Value != "0" && hdnServiceUnitID.Value != "")
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", hdnServiceUnitID.Value);
            else if (cboPatientFrom.Value != null)
                filterExpression += string.Format(" AND DepartmentID = '{0}'", cboPatientFrom.Value);

            if (cboPatientFrom.Value != null && cboPatientFrom.Value.ToString() != Constant.Facility.INPATIENT)
            {
                if (!chkIsPreviousEpisodePatientReg.Checked)
                {
                    filterExpression += string.Format(" AND VisitDate = '{0}'", Helper.GetDatePickerValue(txtRealisationDate).ToString(Constant.FormatString.DATE_FORMAT_112));
                }
            }

            if (hdnFilterExpressionQuickSearchReg.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearchReg.Value);
            return filterExpression;
        }
        
        public override void LoadAllWords()
        {
            LoadWords();
        }

        public override void OnGrdRowClick(string transactionNo)
        {
            string url = "";
            vConsultVisit4 entity = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", transactionNo))[0];
            RegisteredPatient pt = new RegisteredPatient();
            pt.MRN = entity.MRN;
            pt.MedicalNo = entity.MedicalNo;
            pt.GCGender = entity.GCGender;
            pt.GCSex = entity.GCSex;
            pt.DateOfBirth = entity.DateOfBirth;
            pt.RegistrationID = entity.RegistrationID;
            pt.RegistrationNo = entity.RegistrationNo;
            pt.RegistrationDate = entity.RegistrationDate;
            pt.RegistrationTime = entity.RegistrationTime;
            pt.VisitID = entity.VisitID;
            pt.VisitDate = entity.VisitDate;
            pt.VisitTime = entity.VisitTime;
            pt.StartServiceDate = entity.StartServiceDate;
            pt.StartServiceTime = entity.StartServiceTime;
            pt.DischargeDate = entity.DischargeDate;
            pt.DischargeTime = entity.DischargeTime;
            pt.GCCustomerType = entity.GCCustomerType;
            pt.BusinessPartnerID = entity.BusinessPartnerID;
            pt.ParamedicID = entity.ParamedicID;
            pt.ParamedicCode = entity.ParamedicCode;
            pt.ParamedicName = entity.ParamedicName;
            pt.SpecialtyID = entity.SpecialtyID;
            pt.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
            pt.DepartmentID = entity.DepartmentID;
            pt.ServiceUnitName = entity.ServiceUnitName;
            pt.RoomCode = entity.RoomCode;
            pt.BedCode = entity.BedCode;
            pt.DepartmentID = entity.DepartmentID;
            pt.ChargeClassID = entity.ChargeClassID;
            pt.ClassID = entity.ClassID;
            pt.GCRegistrationStatus = entity.GCVisitStatus;
            pt.IsLockDown = entity.IsLockDown;
            pt.IsBillingReopen = entity.IsBillingReopen;
            pt.LinkedRegistrationID = entity.LinkedRegistrationID;
            pt.LinkedToRegistrationID = entity.LinkedToRegistrationID;
            pt.LastAcuteInitialAssessmentDate = entity.LastAcuteInitialAssessmentDate;
            pt.LastChronicInitialAssessmentDate = entity.LastChronicInitialAssessmentDate;
            pt.IsNeedRenewalAcuteInitialAssessment = Helper.GetIsNeedRenewalOPInitialAssessment(pt.LastAcuteInitialAssessmentDate, Convert.ToInt16(AppSession.OP0027));
            pt.IsNeedRenewalChronicInitialAssessment = Helper.GetIsNeedRenewalOPInitialAssessment(pt.LastChronicInitialAssessmentDate, Convert.ToInt16(AppSession.OP0027));
            AppSession.RegisteredPatient = pt;

            string parentCode = "";
            parentCode = Constant.MenuCode.MedicalCheckup.PATIENT_PAGE;
            string filterExpression = string.Format("ParentCode = '{0}'", parentCode);
            List<GetUserMenuAccess> lstMenu = BusinessLayer.GetUserMenuAccess(Constant.Module.MEDICAL_CHECKUP, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, filterExpression);
            int parentID = (int)lstMenu.Where(p => p.MenuIndex > 0 && (p.DepartmentID.Contains(AppSession.RegisteredPatient.DepartmentID) || string.IsNullOrEmpty(p.DepartmentID))).OrderBy(p => p.MenuIndex).FirstOrDefault().MenuID;

            filterExpression = string.Format("ParentID = {0}", parentID);
            lstMenu = BusinessLayer.GetUserMenuAccess(Constant.Module.MEDICAL_CHECKUP, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, filterExpression);
            GetUserMenuAccess menu = lstMenu.Where(p => (p.DepartmentID.Contains(AppSession.RegisteredPatient.DepartmentID) || string.IsNullOrEmpty(p.DepartmentID))).OrderBy(p => p.MenuIndex).FirstOrDefault();
            url = Page.ResolveUrl(menu.MenuUrl);
 
            Response.Redirect(url);
        }

        public override void OnGrdRowClickTestOrder(string transactionNo, string TestOrderID)
        {
            vConsultVisit4 entity = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", transactionNo)).FirstOrDefault();
            RegisteredPatient pt = new RegisteredPatient();
            pt.MRN = entity.MRN;
            pt.MedicalNo = entity.MedicalNo;
            pt.GCGender = entity.GCGender;
            pt.GCSex = entity.GCSex;
            pt.DateOfBirth = entity.DateOfBirth;
            pt.RegistrationID = entity.RegistrationID;
            pt.RegistrationNo = entity.RegistrationNo;
            pt.RegistrationDate = entity.RegistrationDate;
            pt.RegistrationTime = entity.RegistrationTime;
            pt.VisitID = entity.VisitID;
            pt.VisitDate = entity.VisitDate;
            pt.VisitTime = entity.VisitTime;
            pt.StartServiceDate = entity.StartServiceDate;
            pt.StartServiceTime = entity.StartServiceTime;
            pt.DischargeDate = entity.DischargeDate;
            pt.DischargeTime = entity.DischargeTime;
            pt.GCCustomerType = entity.GCCustomerType;
            pt.BusinessPartnerID = entity.BusinessPartnerID;
            pt.ParamedicID = entity.ParamedicID;
            pt.ParamedicCode = entity.ParamedicCode;
            pt.ParamedicName = entity.ParamedicName;
            pt.SpecialtyID = entity.SpecialtyID;
            pt.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
            pt.DepartmentID = entity.DepartmentID;
            pt.ServiceUnitName = entity.ServiceUnitName;
            pt.RoomCode = entity.RoomCode;
            pt.BedCode = entity.BedCode;
            pt.DepartmentID = entity.DepartmentID;
            pt.ChargeClassID = entity.ChargeClassID;
            pt.ClassID = entity.ClassID;
            pt.GCRegistrationStatus = entity.GCVisitStatus;
            pt.IsLockDown = entity.IsLockDown;
            pt.IsBillingReopen = entity.IsBillingReopen;
            pt.LinkedRegistrationID = entity.LinkedRegistrationID;
            pt.LinkedToRegistrationID = entity.LinkedToRegistrationID;
            AppSession.RegisteredPatient = pt;
            string url = string.Format("~/Libs/Program/Module/PatientManagement/Transaction/PatientManagementTransactionDetail.aspx?id=to|{0}|{1}", TestOrderID, transactionNo);
            Response.Redirect(url);
        }

        public override string GetFilterExpressionTestOrder()
        {
            string filterExpression = "";
            string healthcareServiceUnitID = "";

            healthcareServiceUnitID = AppSession.MedicalDiagnostic.HealthcareServiceUnitID.ToString();

            filterExpression += string.Format("HealthcareServiceUnitID = {0} AND GCVisitStatus NOT IN ('{1}','{2}','{3}','{4}') AND ",
                    healthcareServiceUnitID, Constant.VisitStatus.OPEN, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.DISCHARGED, Constant.VisitStatus.CLOSED);

            if (hdnFilterExpressionQuickSearchOrder.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearchOrder.Value);

            return filterExpression;
        }

        public override string GetSortingTestOrder()
        {
            string sortBy = "";

            return sortBy;
        }
    }
}
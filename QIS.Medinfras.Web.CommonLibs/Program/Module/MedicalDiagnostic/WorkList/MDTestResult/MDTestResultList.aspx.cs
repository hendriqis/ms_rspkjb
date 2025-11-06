using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class MDTestResultList : BasePageRegisteredPatient
    {
        public override string OnGetMenuCode()
        {
            if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                return Constant.MenuCode.Imaging.IMAGING_RESULT;
            else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                return Constant.MenuCode.Radiotheraphy.RADIOTERAPHY_RESULT;
            else
                return Constant.MenuCode.MedicalDiagnostic.MD_RESULT;
        }
        public override bool IsShowRightPanel()
        {
            return false;
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
                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;

                if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                    trServiceUnit.Style.Add("display", "none");
                else
                {
                    string filterExpression = string.Format("IsUsingRegistration = 1 AND HealthcareServiceUnitID NOT IN ({0},{1})", AppSession.MedicalDiagnostic.ImagingHealthcareServiceUnitID, AppSession.MedicalDiagnostic.LaboratoryHealthcareServiceUnitID); 

                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                        filterExpression = string.Format("IsUsingRegistration = 1 AND HealthcareServiceUnitID = {0}", AppSession.RT0001); 

                    List<GetServiceUnitUserList> lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.Facility.DIAGNOSTIC, filterExpression);
                    Methods.SetComboBoxField<GetServiceUnitUserList>(cboMedicSupport, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                    cboMedicSupport.SelectedIndex = 0;
                }

                List<Variable> lstVariable = new List<Variable>();
                lstVariable.Add(new Variable { Code = "0", Value = "Semua" });
                lstVariable.Add(new Variable { Code = "1", Value = "Belum Ada Hasil" });
                lstVariable.Add(new Variable { Code = "2", Value = "Sudah Ada Hasil" });
                Methods.SetComboBoxField<Variable>(cboOrderResultType, lstVariable, "Value", "Code");
                cboOrderResultType.Value = "1";

                List<Variable> lstVariableCoverage = new List<Variable>();
                lstVariableCoverage.Add(new Variable { Code = "0", Value = "Semua" });
                lstVariableCoverage.Add(new Variable { Code = "1", Value = "Belum Dibayar" });
                lstVariableCoverage.Add(new Variable { Code = "2", Value = "Sudah Dibayar" });
                Methods.SetComboBoxField<Variable>(cboCoverage, lstVariableCoverage, "Value", "Code");
                cboCoverage.Value = "0";

                List<Department> lstDept = BusinessLayer.GetDepartmentList("IsHasRegistration = 1 AND IsActive = 1");
                lstDept.Insert(0, new Department { DepartmentID = "", DepartmentName = "" });
                Methods.SetComboBoxField<Department>(cboDepartment, lstDept, "DepartmentName", "DepartmentID");
                cboDepartment.SelectedIndex = 0;

                txtTransactionDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                
                Helper.SetControlEntrySetting(txtTransactionDate, new ControlEntrySetting(true, true, false), "mpPatientList");
                Helper.SetControlEntrySetting(cboMedicSupport, new ControlEntrySetting(true, true, false), "mpPatientList");
                Helper.SetControlEntrySetting(cboDepartment, new ControlEntrySetting(true, true, false), "mpPatientList");
                Helper.SetControlEntrySetting(txtServiceUnitCode, new ControlEntrySetting(true, true, false), "mpPatientList");
                Helper.SetControlEntrySetting(txtServiceUnitName, new ControlEntrySetting(true, true, false), "mpPatientList");

                grdPatientResult.InitializeControl();
            }
        }

        public override string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;

            if (filterExpression != "")
                filterExpression += " AND ";

            string medicSupport = "";
            if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                medicSupport = AppSession.MedicalDiagnostic.HealthcareServiceUnitID.ToString();
            else
                medicSupport = cboMedicSupport.Value.ToString();

            filterExpression += string.Format("TransactionDate = '{0}' AND HealthcareServiceUnitID = {1} AND GCTransactionStatus NOT IN ('{2}','{3}')",
                Helper.GetDatePickerValue(txtTransactionDate).ToString(Constant.FormatString.DATE_FORMAT_112), medicSupport, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.VOID);

            if (cboOrderResultType.Value.ToString() == "1")
                filterExpression += " AND TransactionID NOT IN (SELECT ChargeTransactionID FROM ImagingResultHd WHERE IsDeleted=0)";
            else if (cboOrderResultType.Value.ToString() == "2")
                filterExpression += " AND TransactionID IN (SELECT ChargeTransactionID FROM ImagingResultHd WHERE IsDeleted=0)";

            if (cboCoverage.Value.ToString() == "1")
                filterExpression += string.Format(" AND GCTransactionStatus NOT IN ('{0}')", Constant.TransactionStatus.CLOSED);
            else if (cboCoverage.Value.ToString() == "2")
                filterExpression += string.Format(" AND GCTransactionStatus = '{0}'", Constant.TransactionStatus.CLOSED);

            if (cboDepartment.Value != null && cboDepartment.Value.ToString() != "")
                filterExpression += string.Format(" AND DepartmentID = '{0}'", cboDepartment.Value);
            if (hdnServiceUnitID.Value != "0" && hdnServiceUnitID.Value != "")
                filterExpression += string.Format("AND VisitHealthcareServiceUnitID = {0}", hdnServiceUnitID.Value);
            if (hdnFilterExpressionQuickSearch.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            return filterExpression;
        }

        public override void LoadAllWords()
        {
            LoadWords();
        }

        public override void OnGrdRowClick(string transactionNo)
        {
            PatientChargesHd entityCh = BusinessLayer.GetPatientChargesHdList(string.Format("TransactionID = {0}", transactionNo)).FirstOrDefault();

            vConsultVisit4 entity = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", entityCh.VisitID))[0];
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

            string url = "";
            url = string.Format("~/Libs/Program/Module/MedicalDiagnostic/Worklist/MDTestResult/MDTestResultDetail.aspx?id=to|{0}", transactionNo);
            Response.Redirect(url);
        }

    }
}
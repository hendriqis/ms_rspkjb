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

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class PatientPageList : BasePageRegisteredPatient
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.PATIENT_PAGE_LIST;
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

                string filterExpression = string.Format("IsUsingRegistration = 1 AND HealthcareServiceUnitID NOT IN ({0},{1})", AppSession.MedicalDiagnostic.ImagingHealthcareServiceUnitID, AppSession.MedicalDiagnostic.LaboratoryHealthcareServiceUnitID);
                List<GetServiceUnitUserList> lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.Facility.DIAGNOSTIC, filterExpression);
                Methods.SetComboBoxField<GetServiceUnitUserList>(cboMedicSupport, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                cboMedicSupport.SelectedIndex = 0;

                List<Variable> lstVariable = new List<Variable>();
                lstVariable.Add(new Variable { Code = "0", Value = "Semua" });
                lstVariable.Add(new Variable { Code = "1", Value = "Belum Ada Hasil" });
                lstVariable.Add(new Variable { Code = "2", Value = "Sudah Ada Hasil" });
                Methods.SetComboBoxField<Variable>(cboOrderResultType, lstVariable, "Value", "Code");
                cboOrderResultType.Value = "0";

                List<Department> lstDept = BusinessLayer.GetDepartmentList("IsHasRegistration = 1 AND IsActive = 1");
                lstDept.Insert(0, new Department { DepartmentID = "", DepartmentName = "" });
                Methods.SetComboBoxField<Department>(cboDepartment, lstDept, "DepartmentName", "DepartmentID");
                cboDepartment.SelectedIndex = 0;

                txtTransactionDateFrom.Text = DateTime.Today.AddDays(-1).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtTransactionDateTo.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                Helper.SetControlEntrySetting(txtTransactionDateFrom, new ControlEntrySetting(true, true, false), "mpPatientList");
                Helper.SetControlEntrySetting(txtTransactionDateTo, new ControlEntrySetting(true, true, false), "mpPatientList");
                Helper.SetControlEntrySetting(cboMedicSupport, new ControlEntrySetting(true, true, false), "mpPatientList");
                Helper.SetControlEntrySetting(cboDepartment, new ControlEntrySetting(true, true, false), "mpPatientList");
                Helper.SetControlEntrySetting(txtServiceUnitCode, new ControlEntrySetting(true, true, false), "mpPatientList");
                Helper.SetControlEntrySetting(txtServiceUnitName, new ControlEntrySetting(true, true, false), "mpPatientList");

                if (AppSession.LastContentImagingRealizationResult != null)
                {
                    LastContentImagingRealizationResult lc = AppSession.LastContentImagingRealizationResult;

                    txtTransactionDateFrom.Text = lc.ImagingDateFrom;
                    txtTransactionDateTo.Text = lc.ImagingDateTo;
                    
                    if (lc.ImagingFromDepartmentID != "x")
                    {
                        cboDepartment.Value = lc.ImagingFromDepartmentID;
                    }
                    hdnQuickText.Value = lc.QuickText.Equals("Search") ? "" : lc.QuickText;
                    hdnFilterExpressionQuickSearch.Value = lc.QuickFilterExpression;
                }

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

            hdnImagingServiceUnitID.Value = medicSupport;

            filterExpression += string.Format("TransactionDate BETWEEN '{0}' AND '{1}' AND HealthcareServiceUnitID = {2} AND GCTransactionStatus NOT IN ('{3}','{4}')",
                Helper.GetDatePickerValue(txtTransactionDateFrom).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtTransactionDateTo).ToString(Constant.FormatString.DATE_FORMAT_112), hdnImagingServiceUnitID.Value, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.VOID);

            if (cboOrderResultType.Value.ToString() == "1")
                filterExpression += " AND TransactionID NOT IN (SELECT ChargeTransactionID FROM ImagingResultHd WHERE IsDeleted=0)";
            else if (cboOrderResultType.Value.ToString() == "2")
                filterExpression += " AND TransactionID IN (SELECT ChargeTransactionID FROM ImagingResultHd WHERE IsDeleted=0)";

            if (cboDepartment.Value != null && cboDepartment.Value.ToString() != "")
                filterExpression += string.Format(" AND DepartmentID = '{0}'", cboDepartment.Value);

            if (!string.IsNullOrEmpty(txtPhysicianCode.Text))
                filterExpression += string.Format(" AND OrderDetail LIKE '%({0})%'", txtPhysicianCode.Text);

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
            LastContentEMRPatientListTrx lc = new LastContentEMRPatientListTrx()
            {
                DateFrom = txtTransactionDateFrom.Text,
                DateTo = txtTransactionDateTo.Text,
                QuickText = txtSearchView.Text,
                QuickFilterExpression = hdnFilterExpressionQuickSearch.Value,
                FromDepartmentID = cboDepartment.Value != null ? cboDepartment.Value.ToString() : "x",
            };
            AppSession.LastContentEMRPatientListTrx = lc;

            vConsultVisit4 entity = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", transactionNo))[0];
            RegisteredPatient pt = new RegisteredPatient();
            pt.MRN = entity.MRN;
            pt.MedicalNo = entity.MedicalNo;
            pt.PatientName = entity.PatientName;
            pt.RegistrationID = entity.RegistrationID;
            pt.RegistrationNo = entity.RegistrationNo;
            pt.LinkedRegistrationID = entity.LinkedRegistrationID;
            pt.VisitID = entity.VisitID;
            pt.VisitDate = entity.VisitDate;
            pt.VisitTime = entity.VisitTime;
            pt.ParamedicID = entity.ParamedicID;
            pt.SpecialtyID = entity.SpecialtyID;
            pt.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
            pt.ServiceUnitName = entity.ServiceUnitName;
            pt.RoomCode = entity.RoomCode;
            pt.BedCode = entity.BedCode;
            pt.DepartmentID = entity.DepartmentID;
            pt.ClassID = entity.ClassID;
            pt.ChargeClassID = entity.ChargeClassID;
            pt.StartServiceDate = entity.StartServiceDate;
            pt.StartServiceTime = entity.StartServiceTime;
            pt.GCCustomerType = entity.GCCustomerType;
            pt.BusinessPartnerID = entity.BusinessPartnerID;
            pt.DateOfBirth = entity.DateOfBirth;
            pt.IsPlanDischarge = entity.IsPlanDischarge;
            pt.IsUsingImplant = entity.IsUsingImplant;
            AppSession.RegisteredPatient = pt;

            Response.Redirect(string.Format("~/Program/PatientPage/PatientDataView.aspx?type={0}", 0));
        }

    }
}
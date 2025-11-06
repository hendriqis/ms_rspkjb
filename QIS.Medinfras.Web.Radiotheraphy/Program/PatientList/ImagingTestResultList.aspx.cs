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

namespace QIS.Medinfras.Web.Imaging.Program
{
    public partial class ImagingTestResultList : BasePageRegisteredPatient
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Imaging.IMAGING_RESULT;
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
                {
                    trServiceUnit.Style.Add("display", "none");
                    string filterExpression = string.Format("ParentID = '{0}' AND IsActive = 1", Constant.StandardCode.MEDICAL_IMAGING_MODALITIES);
                    List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
                    lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
                    Methods.SetComboBoxField<StandardCode>(cboModality, lstStandardCode, "StandardCodeName", "StandardCodeID");
                    cboModality.SelectedIndex = 0;
                }
                else
                {
                    string filterExpression = string.Format("IsUsingRegistration = 1 AND HealthcareServiceUnitID NOT IN ({0},{1})", AppSession.MedicalDiagnostic.ImagingHealthcareServiceUnitID, AppSession.MedicalDiagnostic.LaboratoryHealthcareServiceUnitID);
                    List<GetServiceUnitUserList> lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.Facility.DIAGNOSTIC, filterExpression);
                    Methods.SetComboBoxField<GetServiceUnitUserList>(cboMedicSupport, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                    cboMedicSupport.SelectedIndex = 0;
                }

                List<Variable> lstVariable = new List<Variable>();
                lstVariable.Add(new Variable { Code = "0", Value = "Semua" });
                lstVariable.Add(new Variable { Code = "1", Value = "Belum Ada Hasil" });
                lstVariable.Add(new Variable { Code = "2", Value = "Sudah Ada Hasil" });
                Methods.SetComboBoxField<Variable>(cboOrderResultType, lstVariable, "Value", "Code");
                cboOrderResultType.Value = "0";

                List<Variable> lstVariableCoverage = new List<Variable>();
                lstVariableCoverage.Add(new Variable { Code = "0", Value = "Semua" });
                lstVariableCoverage.Add(new Variable { Code = "1", Value = "Belum Dibayar" });
                lstVariableCoverage.Add(new Variable { Code = "2", Value = "Sudah Dibayar" });
                Methods.SetComboBoxField<Variable>(cboCoverage, lstVariableCoverage, "Value", "Code");
                cboCoverage.Value = "0";

                List<Variable> lstSortBy = new List<Variable>();
                lstSortBy.Add(new Variable { Code = "0", Value = "No. Transaksi (ASC)" });
                lstSortBy.Add(new Variable { Code = "1", Value = "Nama Pasien (ASC)" });
                lstSortBy.Add(new Variable { Code = "2", Value = "No. RM (ASC)" });
                lstSortBy.Add(new Variable { Code = "3", Value = "No. Registrasi (ASC)" });
                lstSortBy.Add(new Variable { Code = "4", Value = "Nama Item (ASC)" });
                lstSortBy.Add(new Variable { Code = "5", Value = "No. Transaksi (DESC)" });
                lstSortBy.Add(new Variable { Code = "6", Value = "Nama Pasien (DESC)" });
                lstSortBy.Add(new Variable { Code = "7", Value = "No. RM (DESC)" });
                lstSortBy.Add(new Variable { Code = "8", Value = "No. Registrasi (DESC)" });
                lstSortBy.Add(new Variable { Code = "9", Value = "Nama Item (DESC)" });
                Methods.SetComboBoxField<Variable>(cboSortBy, lstSortBy, "Value", "Code");
                cboSortBy.Value = "0";

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
                    if (lc.ImagingGCModality != "x")
                    {
                        cboModality.Value = lc.ImagingGCModality;
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

            filterExpression += string.Format("TransactionDate BETWEEN '{0}' AND '{1}' AND HealthcareServiceUnitID = {2} AND GCTransactionStatus NOT IN ('{3}','{4}')",
                Helper.GetDatePickerValue(txtTransactionDateFrom).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtTransactionDateTo).ToString(Constant.FormatString.DATE_FORMAT_112), medicSupport, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.VOID);

            if (cboOrderResultType.Value.ToString() == "1")
                filterExpression += " AND TransactionID NOT IN (SELECT ChargeTransactionID FROM ImagingResultHd WHERE IsDeleted=0)";
            else if (cboOrderResultType.Value.ToString() == "2")
                filterExpression += " AND TransactionID IN (SELECT ChargeTransactionID FROM ImagingResultHd WHERE IsDeleted=0)";

            if (cboCoverage.Value.ToString() == "1")
                filterExpression += string.Format(" AND GCTransactionStatus NOT IN ('{0}')", Constant.TransactionStatus.CLOSED);
            else if (cboCoverage.Value.ToString() == "2")
                filterExpression += string.Format (" AND GCTransactionStatus = '{0}'", Constant.TransactionStatus.CLOSED);

            if (cboDepartment.Value != null && cboDepartment.Value.ToString() != "")
                filterExpression += string.Format(" AND VisitDepartmentID = '{0}'", cboDepartment.Value);

            if (cboModality.Value != null && cboModality.Value.ToString() != "")
                filterExpression += string.Format(" AND OrderDetail LIKE '%({0})%'", cboModality.Value.ToString().Substring(5));

            if (hdnServiceUnitID.Value != "0" && hdnServiceUnitID.Value != "")
                filterExpression += string.Format("AND VisitHealthcareServiceUnitID = {0}", hdnServiceUnitID.Value);
            if (hdnFilterExpressionQuickSearch.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);

            if (cboSortBy.Value.ToString() == "0")
            {
                filterExpression += " ORDER BY TransactionNo ASC";
            }
            else if (cboSortBy.Value.ToString() == "1")
            {
                filterExpression += " ORDER BY PatientName ASC";
            }
            else if (cboSortBy.Value.ToString() == "2")
            {
                filterExpression += " ORDER BY MedicalNo ASC";
            }
            else if (cboSortBy.Value.ToString() == "3")
            {
                filterExpression += " ORDER BY RegistrationNo ASC";
            }
            else if (cboSortBy.Value.ToString() == "4")
            {
                filterExpression += " ORDER BY OrderDetail ASC";
            }
            else if (cboSortBy.Value.ToString() == "5")
            {
                filterExpression += " ORDER BY TransactionNo DESC";
            }
            else if (cboSortBy.Value.ToString() == "6")
            {
                filterExpression += " ORDER BY PatientName DESC";
            }
            else if (cboSortBy.Value.ToString() == "7")
            {
                filterExpression += " ORDER BY MedicalNo DESC";
            }
            else if (cboSortBy.Value.ToString() == "8")
            {
                filterExpression += " ORDER BY RegistrationNo DESC";
            }
            else if (cboSortBy.Value.ToString() == "9")
            {
                filterExpression += " ORDER BY OrderDetail DESC";
            }

            return filterExpression;
        }

        public override void LoadAllWords()
        {
            LoadWords();
        }

        public override void OnGrdRowClick(string oTransactionID)
        {
            LastContentImagingRealizationResult lc = new LastContentImagingRealizationResult()
            {
                ImagingDateFrom = txtTransactionDateFrom.Text,
                ImagingDateTo = txtTransactionDateTo.Text,
                QuickText = txtSearchView.Text,
                QuickFilterExpression = hdnFilterExpressionQuickSearch.Value,
                ImagingFromDepartmentID = cboDepartment.Value != null ? cboDepartment.Value.ToString() : "x",
                ImagingGCModality = cboModality.Value != null ? cboModality.Value.ToString() : "x"
            };
            AppSession.LastContentImagingRealizationResult = lc;

            vPatientChargesHd entity = BusinessLayer.GetvPatientChargesHdList(string.Format("TransactionID = {0}", oTransactionID)).FirstOrDefault();
            RegisteredPatient pt = new RegisteredPatient();
            pt.MRN = entity.MRN;
            pt.MedicalNo = entity.MedicalNo;
            pt.RegistrationID = entity.RegistrationID;
            pt.BusinessPartnerID = entity.BusinessPartnerID;
            pt.VisitID = entity.VisitID;
            AppSession.RegisteredPatient = pt;

            string url = "";
            url = string.Format("~/Libs/Program/Module/MedicalDiagnostic/Worklist/MDTestResult/MDTestResultDetail.aspx?id=to|{0}", oTransactionID);
            Response.Redirect(url);
        }

    }
}
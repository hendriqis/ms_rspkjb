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
    public partial class MDVerificationResultList : BasePageRegisteredPatient
    {
        public override string OnGetMenuCode()
        {
            if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                return Constant.MenuCode.Imaging.IMAGING_RESULT_VERIFICATION;
            else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                return Constant.MenuCode.Radiotheraphy.RADIOTERAPHY_RESULT_VERIFICATION;
            else
                return Constant.MenuCode.MedicalDiagnostic.MD_RESULT_VERIFICATION;
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

                List<Department> lstDept = BusinessLayer.GetDepartmentList("IsHasRegistration = 1 AND IsActive = 1");
                lstDept.Add(new Department { DepartmentID = "", DepartmentName = "" });
                Methods.SetComboBoxField<Department>(cboPatientFrom, lstDept, "DepartmentName", "DepartmentID");
                cboPatientFrom.Value = "";

                txtTransactionDateFrom.Text = DateTime.Today.AddDays(-1).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtTransactionDateTo.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

                List<Variable> lstVariable = new List<Variable>();
                lstVariable.Add(new Variable { Code = "0", Value = "Semua" });
                lstVariable.Add(new Variable { Code = "1", Value = "Belum Diproses" });
                lstVariable.Add(new Variable { Code = "2", Value = "Sudah Diproses" });
                Methods.SetComboBoxField<Variable>(cboResultType, lstVariable, "Value", "Code");
                cboResultType.Value = "1";

                Helper.SetControlEntrySetting(txtTransactionDateFrom, new ControlEntrySetting(true, true, false), "mpPatientList");
                Helper.SetControlEntrySetting(txtTransactionDateTo, new ControlEntrySetting(true, true, false), "mpPatientList");
                Helper.SetControlEntrySetting(cboMedicSupport, new ControlEntrySetting(true, true, false), "mpPatientList");
                Helper.SetControlEntrySetting(cboPatientFrom, new ControlEntrySetting(true, true, false), "mpPatientList");
                Helper.SetControlEntrySetting(txtServiceUnitCode, new ControlEntrySetting(true, true, false), "mpPatientList");
                Helper.SetControlEntrySetting(txtServiceUnitName, new ControlEntrySetting(true, true, false), "mpPatientList");
                grdVerificationResult.InitializeControl();
                //grdPatientResult.InitializeControl();
            }
        }

        public override string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;

            if (filterExpression != "")
                filterExpression += " AND ";

            string medicSupport = "";
            if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
            {
                medicSupport = AppSession.MedicalDiagnostic.HealthcareServiceUnitID.ToString();
            }
            else
            {
                medicSupport = cboMedicSupport.Value.ToString();
            }

            filterExpression += string.Format("TransactionDate BETWEEN '{0}' AND '{1}' AND HealthcareServiceUnitID = {2}",
                                                        Helper.GetDatePickerValue(txtTransactionDateFrom).ToString(Constant.FormatString.DATE_FORMAT_112),
                                                        Helper.GetDatePickerValue(txtTransactionDateTo).ToString(Constant.FormatString.DATE_FORMAT_112),
                                                        medicSupport);

            string resultType = cboResultType.Value.ToString();
            switch (resultType)
            {
                case "1": filterExpression += string.Format(" AND ImagingHdGCTransactionStatus = '{0}'", Constant.TransactionStatus.WAIT_FOR_APPROVAL); break;
                case "2": filterExpression += string.Format(" AND ImagingHdGCTransactionStatus = '{0}'", Constant.TransactionStatus.PROCESSED); break;
                default: filterExpression += string.Format(" AND ImagingHdGCTransactionStatus IN ('{0}','{1}')", Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.WAIT_FOR_APPROVAL); break;
            }

            if (cboPatientFrom.Value != null)
                filterExpression += string.Format(" AND DepartmentID = '{0}'", cboPatientFrom.Value);

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
            vPatientChargesHd entity = BusinessLayer.GetvPatientChargesHdList(string.Format("TransactionID = {0}", transactionNo)).FirstOrDefault();
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
            url = string.Format("~/Libs/Program/Module/MedicalDiagnostic/Worklist/MDVerificationResult/MDVerificationResultDetail.aspx?id={0}", transactionNo);
            Response.Redirect(url);
        }
        
    }
}
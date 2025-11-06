using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class ClaimDiagnoseProcedureList : BasePagePatientOrder
    {
        private string refreshGridInterval = "";

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.DIAGNOSE_PROCEDURE_CLAIM;
        }

        private GetUserMenuAccess menu;

        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                List<Department> lstDept = BusinessLayer.GetDepartmentList(string.Format("DepartmentID != '{0}' AND IsActive = 1 AND IsHasRegistration = 1", Constant.Facility.PHARMACY));
                Methods.SetComboBoxField<Department>(cboPatientFrom, lstDept, "DepartmentName", "DepartmentID");
                Methods.SetComboBoxField<Department>(cboPatientFrom2, lstDept, "DepartmentName", "DepartmentID");

                List<Variable> lstVariable = new List<Variable>();
                lstVariable.Add(new Variable { Code = "0", Value = "Semua" });
                lstVariable.Add(new Variable { Code = "1", Value = "Pulang" });
                lstVariable.Add(new Variable { Code = "2", Value = "Dirawat" });
                Methods.SetComboBoxField<Variable>(cboRegistrationStatus, lstVariable, "Value", "Code");
                Methods.SetComboBoxField<Variable>(cboRegistrationStatus2, lstVariable, "Value", "Code");

                Helper.SetControlEntrySetting(txtFromRegistrationDate, new ControlEntrySetting(false, false, false), "mpPatientList");
                Helper.SetControlEntrySetting(txtToRegistrationDate, new ControlEntrySetting(false, false, false), "mpPatientList");
                Helper.SetControlEntrySetting(txtFromRegistrationDate2, new ControlEntrySetting(false, false, false), "mpPatientList");
                Helper.SetControlEntrySetting(txtToRegistrationDate2, new ControlEntrySetting(false, false, false), "mpPatientList");

                InitializeFilterParameter();

                SettingControlProperties();

                grdRegisteredPatient.InitializeControl();
                grdRegisteredPatient2.InitializeControl();

                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;
            }
        }

        private void InitializeFilterParameter()
        {
            #region Ada Diagnosa
            cboPatientFrom.Value = Constant.Facility.OUTPATIENT;
            hdnServiceUnitID.Value = string.Empty;
            txtServiceUnitCode.Text = string.Empty;
            txtFromRegistrationDate.Text = DateTime.Today.AddDays(-1).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtToRegistrationDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            cboRegistrationStatus.Value = "0";
            txtSearchView.Text = "Search";
            hdnFilterExpressionQuickSearch.Value = string.Empty;
            #endregion

            #region Belum Ada Diagnosa
            cboPatientFrom2.Value = Constant.Facility.OUTPATIENT;
            hdnServiceUnitID2.Value = string.Empty;
            txtServiceUnitCode2.Text = string.Empty;
            txtFromRegistrationDate2.Text = DateTime.Today.AddDays(-1).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtToRegistrationDate2.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            cboRegistrationStatus2.Value = "0";
            txtSearchView2.Text = "Search";
            hdnFilterExpressionQuickSearch2.Value = string.Empty;
            #endregion
        }

        private void SettingControlProperties()
        {
            if (AppSession.LastContentClaimDiagnoseProcedure != null)
            {
                LastContentClaimDiagnoseProcedure lcMain = AppSession.LastContentClaimDiagnoseProcedure;
                hdnLastContentID.Value = lcMain.ContentID;
                if (lcMain.ContentID.Equals("containerSudahDiagnosa"))
                {
                    LastContentClaimDiagnoseProcedureListHaveDiag lc = AppSession.LastContentClaimDiagnoseProcedureListHaveDiag;
                    txtFromRegistrationDate.Text = DateTime.ParseExact(lc.FromRegistrationDate, Constant.FormatString.DATE_FORMAT_112, CultureInfo.InvariantCulture).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtToRegistrationDate.Text = DateTime.ParseExact(lc.ToRegistrationDate, Constant.FormatString.DATE_FORMAT_112, CultureInfo.InvariantCulture).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    hdnServiceUnitID.Value = lc.HealthcareServiceUnitID.ToString();
                    txtServiceUnitCode.Text = lc.ServiceUnitCode;
                    txtServiceUnitName.Text = lc.ServiceUnitName;
                    cboPatientFrom.Value = lc.DepartmentID;
                    hdnQuickText.Value = lc.FilterExpressionSearchView.Equals("Search") ? "" : lc.FilterExpressionSearchView;
                    hdnFilterExpressionQuickSearch.Value = lc.FilterExpressionQuickSearch;
                    cboRegistrationStatus.Value = lc.RegistrationStatus;
                }
                else if (lcMain.ContentID.Equals("containerBelumDiagnosa"))
                {
                    LastContentClaimDiagnoseProcedureList lc = AppSession.LastContentClaimDiagnoseProcedureList;
                    txtFromRegistrationDate2.Text = DateTime.ParseExact(lc.FromRegistrationDate, Constant.FormatString.DATE_FORMAT_112, CultureInfo.InvariantCulture).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtToRegistrationDate2.Text = DateTime.ParseExact(lc.ToRegistrationDate, Constant.FormatString.DATE_FORMAT_112, CultureInfo.InvariantCulture).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    hdnServiceUnitID2.Value = lc.HealthcareServiceUnitID.ToString();
                    txtServiceUnitCode2.Text = lc.ServiceUnitCode;
                    txtServiceUnitName2.Text = lc.ServiceUnitName;
                    cboPatientFrom2.Value = lc.DepartmentID;
                    hdnQuickText2.Value = lc.FilterExpressionSearchView.Equals("Search") ? "" : lc.FilterExpressionSearchView;
                    hdnFilterExpressionQuickSearch2.Value = lc.FilterExpressionQuickSearch;
                    cboRegistrationStatus2.Value = lc.RegistrationStatus;
                }
            }
            else
            {
                InitializeFilterParameter();
            }
        }

        public override void LoadAllWords()
        {
            LoadWords();
        }

        #region Ada Diagnosa
        public override string GetFilterExpression()
        {
            string filterExpression = string.Format("RegistrationDate BETWEEN '{0}' AND '{1}' AND ClaimDiagnosisID IS NOT NULL",
                                                        Helper.GetDatePickerValue(txtFromRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112),
                                                        Helper.GetDatePickerValue(txtToRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112));

            if (hdnServiceUnitID.Value != "0" && hdnServiceUnitID.Value != "")
            {
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", hdnServiceUnitID.Value);
            }
            else if (cboPatientFrom.Value != null)
            {
                filterExpression += string.Format(" AND DepartmentID = '{0}'", cboPatientFrom.Value);
            }

            if (hdnFilterExpressionQuickSearch.Value != "")
            {
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            }

            string registrationStatus = cboRegistrationStatus.Value.ToString();
            if (cboRegistrationStatus.Value != null)
            {
                registrationStatus = cboRegistrationStatus2.Value.ToString();
                if (registrationStatus == "1")
                {
                    filterExpression += string.Format(" AND GCRegistrationStatus = '{0}'", Constant.VisitStatus.CLOSED);
                }
                else if (registrationStatus == "2")
                {
                    filterExpression += string.Format(" AND GCRegistrationStatus != '{0}'", Constant.VisitStatus.CLOSED);
                }
            }
            return filterExpression;
        }

        public override void OnGrdRowClick(string transactionNo)
        {
            string id = Page.Request.QueryString["id"];
            string url = "";

            LastContentClaimDiagnoseProcedure lcMain = new LastContentClaimDiagnoseProcedure()
            {
                ContentID = hdnLastContentID.Value
            };
            AppSession.LastContentClaimDiagnoseProcedure = lcMain;

            LastContentClaimDiagnoseProcedureListHaveDiag lc = new LastContentClaimDiagnoseProcedureListHaveDiag()
            {
                FromRegistrationDate = Helper.GetDatePickerValue(txtFromRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112),
                ToRegistrationDate = Helper.GetDatePickerValue(txtToRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112),
                HealthcareServiceUnitID = hdnServiceUnitID.Value,
                ServiceUnitCode = txtServiceUnitCode.Text,
                ServiceUnitName = txtServiceUnitName.Text,
                DepartmentID = cboPatientFrom.Value.ToString(),
                FilterExpressionSearchView = txtSearchView.Text,
                FilterExpressionQuickSearch = hdnFilterExpressionQuickSearch.Value,
                RegistrationStatus = cboRegistrationStatus.Value.ToString()
            };
            AppSession.LastContentClaimDiagnoseProcedureListHaveDiag = lc;

            vConsultVisit4 entity = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", transactionNo)).FirstOrDefault();
            RegisteredPatient pt = new RegisteredPatient();
            pt.MRN = entity.MRN;
            pt.MedicalNo = entity.MedicalNo;
            pt.PatientName = entity.PatientName;
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
            url = "~/Program/BPJS/ClaimDiagnoseProcedure/ClaimDiagnoseEntry.aspx";
            Response.Redirect(url);
        }
        #endregion

        #region Belum Ada Diagnosa
        public override string GetFilterExpressionTestOrder()
        {
            string filterExpression = string.Format("RegistrationDate BETWEEN '{0}' AND '{1}' AND ClaimDiagnosisID IS NULL",
                                                        Helper.GetDatePickerValue(txtFromRegistrationDate2).ToString(Constant.FormatString.DATE_FORMAT_112),
                                                        Helper.GetDatePickerValue(txtToRegistrationDate2).ToString(Constant.FormatString.DATE_FORMAT_112));

            if (hdnServiceUnitID2.Value != "0" && hdnServiceUnitID2.Value != "")
            {
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", hdnServiceUnitID2.Value);
            }
            else if (cboPatientFrom2.Value != null)
            {
                filterExpression += string.Format(" AND DepartmentID = '{0}'", cboPatientFrom2.Value);
            }

            if (hdnFilterExpressionQuickSearch2.Value != "")
            {
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch2.Value);
            }

            string registrationStatus = cboRegistrationStatus2.Value.ToString();
            if (cboRegistrationStatus2.Value != null)
            {
                registrationStatus = cboRegistrationStatus2.Value.ToString();
                if (registrationStatus == "1")
                {
                    filterExpression += string.Format(" AND GCRegistrationStatus = '{0}'", Constant.VisitStatus.CLOSED);
                }
                else if (registrationStatus == "2")
                {
                    filterExpression += string.Format(" AND GCRegistrationStatus != '{0}'", Constant.VisitStatus.CLOSED);
                }
            }

            return filterExpression;
        }

        public override void OnGrdRowClickTestOrder(string transactionNo, string TestOrderID)
        {
            TestOrderID = "";
            string id = Page.Request.QueryString["id"];
            string url = "";

            if (hdnLastContentID.Value == null || hdnLastContentID.Value == "")
            {
                hdnLastContentID.Value = "containerBelumDiagnosa";
            }

            LastContentClaimDiagnoseProcedure lcMain = new LastContentClaimDiagnoseProcedure()
            {
                ContentID = hdnLastContentID.Value
            };
            AppSession.LastContentClaimDiagnoseProcedure = lcMain;

            LastContentClaimDiagnoseProcedureList lc = new LastContentClaimDiagnoseProcedureList()
            {
                FromRegistrationDate = Helper.GetDatePickerValue(txtFromRegistrationDate2).ToString(Constant.FormatString.DATE_FORMAT_112),
                ToRegistrationDate = Helper.GetDatePickerValue(txtToRegistrationDate2).ToString(Constant.FormatString.DATE_FORMAT_112),
                HealthcareServiceUnitID = hdnServiceUnitID2.Value,
                ServiceUnitCode = txtServiceUnitCode2.Text,
                ServiceUnitName = txtServiceUnitName2.Text,
                DepartmentID = cboPatientFrom2.Value.ToString(),
                FilterExpressionSearchView = txtSearchView2.Text,
                FilterExpressionQuickSearch = hdnFilterExpressionQuickSearch2.Value,
                RegistrationStatus = cboRegistrationStatus2.Value.ToString()
            };
            AppSession.LastContentClaimDiagnoseProcedureList = lc;

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
            url = "~/Program/BPJS/ClaimDiagnoseProcedure/ClaimDiagnoseEntry.aspx";
            Response.Redirect(url);
        }

        public override string GetSortingTestOrder()
        {
            string sortBy = "";

            return sortBy;
        }
        #endregion
    }
}
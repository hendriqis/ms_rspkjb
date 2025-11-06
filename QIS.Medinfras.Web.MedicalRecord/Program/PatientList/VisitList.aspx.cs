using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class VisitList : BasePagePatientOrder2
    {
        //private string refreshGridInterval = "";

        public override string OnGetMenuCode()
        {
            string id = Page.Request.QueryString["id"];
            switch (id)
            {
                case "ps": return Constant.MenuCode.MedicalRecord.PATIENT_SOAP;
                case "ina": return Constant.MenuCode.MedicalRecord.INACBGS;
                default: return Constant.MenuCode.MedicalRecord.PATIENT_FOLDER_STATUS;
            }
        }

        private GetUserMenuAccess menu;
        public override bool IsShowRightPanel()
        {
            string id = Page.Request.QueryString["id"];
            if (id == "ps")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }
        //protected string GetRefreshGridInterval()
        //{
        //    return refreshGridInterval;
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                List<Variable> lstVariable = new List<Variable>();
                lstVariable.Add(new Variable { Code = "0", Value = "Semua" });
                lstVariable.Add(new Variable { Code = "1", Value = "Belum Diproses" });
                lstVariable.Add(new Variable { Code = "2", Value = "Sudah Diproses" });
                Methods.SetComboBoxField<Variable>(cboResultType, lstVariable, "Value", "Code");
                Methods.SetComboBoxField<Variable>(cboResultType2, lstVariable, "Value", "Code");
                Methods.SetComboBoxField<Variable>(cboResultType3, lstVariable, "Value", "Code");

                lstVariable = new List<Variable>();
                lstVariable.Add(new Variable { Code = "0", Value = "Semua" });
                lstVariable.Add(new Variable { Code = "1", Value = "Pulang" });
                lstVariable.Add(new Variable { Code = "2", Value = "Dirawat" });
                Methods.SetComboBoxField<Variable>(cboRegistrationStatus, lstVariable, "Value", "Code");
                Methods.SetComboBoxField<Variable>(cboRegistrationStatus2, lstVariable, "Value", "Code");
                Methods.SetComboBoxField<Variable>(cboRegistrationStatus3, lstVariable, "Value", "Code");

                lstVariable = new List<Variable>();
                lstVariable.Add(new Variable { Code = "0", Value = "Tanggal Registrasi" });
                lstVariable.Add(new Variable { Code = "1", Value = "Tanggal Pulang" });
                Methods.SetComboBoxField<Variable>(cboDateFilter, lstVariable, "Value", "Code");
                Methods.SetComboBoxField<Variable>(cboDateFilter2, lstVariable, "Value", "Code");
                Methods.SetComboBoxField<Variable>(cboDateFilter3, lstVariable, "Value", "Code");
                cboDateFilter.SelectedIndex = 0;
                cboDateFilter2.SelectedIndex = 0;
                cboDateFilter3.SelectedIndex = 0;

                lstVariable = new List<Variable>();
                lstVariable.Add(new Variable { Code = "0", Value = "" });
                lstVariable.Add(new Variable { Code = "1", Value = "Hamil" });
                lstVariable.Add(new Variable { Code = "2", Value = "Partus" });
                lstVariable.Add(new Variable { Code = "3", Value = "Bayi Baru Lahir" });
                Methods.SetComboBoxField<Variable>(cboPatientCondition, lstVariable, "Value", "Code");
                Methods.SetComboBoxField<Variable>(cboPatientCondition2, lstVariable, "Value", "Code");
                Methods.SetComboBoxField<Variable>(cboPatientCondition3, lstVariable, "Value", "Code");
                cboPatientCondition.SelectedIndex = 0;
                cboPatientCondition2.SelectedIndex = 0;
                cboPatientCondition3.SelectedIndex = 0;

                List<Department> lstDept = BusinessLayer.GetDepartmentList(string.Format("DepartmentID != '{0}' AND IsActive = 1 AND IsHasRegistration = 1", Constant.Facility.PHARMACY));
                Methods.SetComboBoxField<Department>(cboPatientFrom, lstDept, "DepartmentName", "DepartmentID");
                Methods.SetComboBoxField<Department>(cboPatientFrom2, lstDept, "DepartmentName", "DepartmentID");
                Methods.SetComboBoxField<Department>(cboPatientFrom3, lstDept, "DepartmentName", "DepartmentID");

                InitializeFilterParameter();
                hdnLastContentID.Value = "containerBelumDiagnosa";
                string id = Page.Request.QueryString["id"];
                if (id == "ps" || id == "ina")
                {
                    SettingControlProperties();
                }
                else
                {
                    InitializeFilterParameter();
                }

                Helper.SetControlEntrySetting(txtFromRegistrationDate, new ControlEntrySetting(false, false, false), "mpPatientList");
                Helper.SetControlEntrySetting(txtToRegistrationDate, new ControlEntrySetting(false, false, false), "mpPatientList");
                Helper.SetControlEntrySetting(txtFromRegistrationDate2, new ControlEntrySetting(false, false, false), "mpPatientList");
                Helper.SetControlEntrySetting(txtToRegistrationDate2, new ControlEntrySetting(false, false, false), "mpPatientList");
                Helper.SetControlEntrySetting(txtFromRegistrationDate3, new ControlEntrySetting(false, false, false), "mpPatientList");
                Helper.SetControlEntrySetting(txtToRegistrationDate3, new ControlEntrySetting(false, false, false), "mpPatientList");

                grdRegisteredPatient.InitializeControl();
                grdRegisteredPatient2.InitializeControl();
                grdRegisteredPatient3.InitializeControl();

                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

                //refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;
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
            cboResultType.Value = "0";
            chkChiefComplaint.Checked = false;
            chkPatientDiagnosis.Checked = false;
            chkDischarge.Checked = false;
            txtSearchView.Text = "Search";
            //hdnFilterExpressionQuickSearch.Value = string.Empty;
            cboDateFilter.Value = "0";
            cboPatientCondition.Value = "0";
            #endregion

            #region Belum Ada Diagnosa
            cboPatientFrom2.Value = Constant.Facility.OUTPATIENT;
            hdnServiceUnitID2.Value = string.Empty;
            txtServiceUnitCode2.Text = string.Empty;
            txtFromRegistrationDate2.Text = DateTime.Today.AddDays(-1).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtToRegistrationDate2.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            cboRegistrationStatus2.Value = "0";
            cboResultType2.Value = "0";
            chkChiefComplaint2.Checked = false;
            chkPatientDiagnosis2.Checked = false;
            chkDischarge2.Checked = false;
            txtSearchView2.Text = "Search";
            //hdnFilterExpressionQuickSearch2.Value = string.Empty;
            cboDateFilter2.Value = "0";
            cboPatientCondition2.Value = "0";
            #endregion

            #region Tidak Perlu Kodefikasi
            cboPatientFrom3.Value = Constant.Facility.OUTPATIENT;
            hdnServiceUnitID3.Value = string.Empty;
            txtServiceUnitCode3.Text = string.Empty;
            txtFromRegistrationDate3.Text = DateTime.Today.AddDays(-1).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtToRegistrationDate3.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            cboRegistrationStatus3.Value = "0";
            cboResultType3.Value = "0";
            chkChiefComplaint3.Checked = false;
            chkPatientDiagnosis3.Checked = false;
            chkPhysicianDischarge3.Checked = false;
            chkDischarge3.Checked = false;
            txtSearchView3.Text = "Search";
            //hdnFilterExpressionQuickSearch3.Value = string.Empty;
            cboDateFilter3.Value = "0";
            cboPatientCondition3.Value = "0";
            #endregion
        }

        private void SettingControlProperties()
        {
            if (AppSession.LastContentVisitListMR != null)
            {
                LastContentVisitListMR lc = AppSession.LastContentVisitListMR;
                if (lc.ContentID != null)
                {
                    if (lc.ContentID.Equals("containerSudahDiagnosa"))
                    {
                        LastPagingMR2 mr2 = AppSession.LastPagingMR2;
                        hdnLastPage.Value = mr2.PageID.ToString();

                        hdnLastContentID.Value = "containerSudahDiagnosa";
                        cboPatientFrom.Value = lc.DepartmentID;
                        txtFromRegistrationDate.Text = DateTime.ParseExact(lc.FromDate,
                                  Constant.FormatString.DATE_FORMAT_112,
                                   CultureInfo.InvariantCulture).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                        txtToRegistrationDate.Text = DateTime.ParseExact(lc.ToDate,
                                  Constant.FormatString.DATE_FORMAT_112,
                                   CultureInfo.InvariantCulture).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                        hdnServiceUnitID.Value = lc.HealthcareServiceUnitID.ToString();
                        txtServiceUnitCode.Text = lc.ServiceUnitCode;
                        txtServiceUnitName.Text = lc.ServiceUnitName;
                        hdnQuickText.Value = lc.QuickText.Equals("Search") ? "" : lc.QuickText;
                        hdnFilterExpressionQuickSearch.Value = lc.QuickFilterExpression;
                        cboRegistrationStatus.Value = lc.RegistrationStatus;
                        cboResultType.Value = lc.ProcessStatus;
                        chkAllEmpty.Checked = lc.IsAllEmpty;
                        chkChiefComplaint.Checked = lc.IsChiefComplaintNull;
                        chkPatientDiagnosis.Checked = lc.IsPatientDiagnosisNull;
                        chkPhysicianDischarge.Checked = lc.IsPhysicianDischargeNull;
                        chkDischarge.Checked = lc.IsDischargeDateNull;
                        cboDateFilter.Value = lc.DateFilter;
                        cboPatientCondition.Value = lc.PatientCondition;
                    }
                    else if (lc.ContentID.Equals("containerBelumDiagnosa"))
                    {
                        LastPagingMR1 mr1 = AppSession.LastPagingMR1;
                        hdnLastPage.Value = mr1.PageID.ToString();

                        hdnLastContentID.Value = "containerBelumDiagnosa";
                        cboPatientFrom2.Value = lc.DepartmentID;
                        txtFromRegistrationDate2.Text = DateTime.ParseExact(lc.FromDate,
                                  Constant.FormatString.DATE_FORMAT_112,
                                   CultureInfo.InvariantCulture).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                        txtToRegistrationDate2.Text = DateTime.ParseExact(lc.ToDate,
                                  Constant.FormatString.DATE_FORMAT_112,
                                   CultureInfo.InvariantCulture).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                        hdnServiceUnitID2.Value = lc.HealthcareServiceUnitID.ToString();
                        txtServiceUnitCode2.Text = lc.ServiceUnitCode;
                        txtServiceUnitName2.Text = lc.ServiceUnitName;
                        hdnQuickText2.Value = lc.QuickText.Equals("Search") ? "" : lc.QuickText;
                        hdnFilterExpressionQuickSearch2.Value = lc.QuickFilterExpression;
                        cboRegistrationStatus2.Value = lc.RegistrationStatus;
                        cboResultType2.Value = lc.ProcessStatus;
                        chkAllEmpty2.Checked = lc.IsAllEmpty;
                        chkChiefComplaint2.Checked = lc.IsChiefComplaintNull;
                        chkPatientDiagnosis2.Checked = lc.IsPatientDiagnosisNull;
                        chkPhysicianDischarge2.Checked = lc.IsPhysicianDischargeNull;
                        chkDischarge2.Checked = lc.IsDischargeDateNull;
                        cboDateFilter2.Value = lc.DateFilter;
                        cboPatientCondition2.Value = lc.PatientCondition;
                    }
                    else if (lc.ContentID.Equals("containerTidakPerluKodefikasi"))
                    {
                        LastPagingMR3 mr3 = AppSession.LastPagingMR3;
                        hdnLastPage.Value = mr3.PageID.ToString();

                        hdnLastContentID.Value = "containerTidakPerluKodefikasi";
                        cboPatientFrom3.Value = lc.DepartmentID;
                        txtFromRegistrationDate3.Text = DateTime.ParseExact(lc.FromDate,
                                  Constant.FormatString.DATE_FORMAT_112,
                                   CultureInfo.InvariantCulture).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                        txtToRegistrationDate3.Text = DateTime.ParseExact(lc.ToDate,
                                  Constant.FormatString.DATE_FORMAT_112,
                                   CultureInfo.InvariantCulture).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                        hdnServiceUnitID3.Value = lc.HealthcareServiceUnitID.ToString();
                        txtServiceUnitCode3.Text = lc.ServiceUnitCode;
                        txtServiceUnitName3.Text = lc.ServiceUnitName;
                        hdnQuickText3.Value = lc.QuickText.Equals("Search") ? "" : lc.QuickText;
                        hdnFilterExpressionQuickSearch3.Value = lc.QuickFilterExpression;
                        cboRegistrationStatus3.Value = lc.RegistrationStatus;
                        cboResultType3.Value = lc.ProcessStatus;
                        chkAllEmpty3.Checked = lc.IsAllEmpty;
                        chkChiefComplaint3.Checked = lc.IsChiefComplaintNull;
                        chkPatientDiagnosis3.Checked = lc.IsPatientDiagnosisNull;
                        chkPhysicianDischarge3.Checked = lc.IsPhysicianDischargeNull;
                        chkDischarge3.Checked = lc.IsDischargeDateNull;
                        cboDateFilter3.Value = lc.DateFilter;
                        cboPatientCondition3.Value = lc.PatientCondition;
                    }
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

        #region Belum Ada Diagnosa
        public override string GetFilterExpressionTestOrder()
        {
            string filterExpression = string.Format("IsNeedCodification = 1");

            if (cboDateFilter2.SelectedIndex == 0)
            {
                filterExpression += string.Format(" AND (VisitDate BETWEEN '{0}' AND '{1}') AND GCVisitStatus != '{2}' AND FinalDiagnoseID IS NULL", Helper.GetDatePickerValue(txtFromRegistrationDate2).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtToRegistrationDate2).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.VisitStatus.CANCELLED);
            }
            else
            {
                filterExpression += string.Format(" AND (DischargeDate BETWEEN '{0}' AND '{1}') AND GCVisitStatus != '{2}' AND FinalDiagnoseID IS NULL", Helper.GetDatePickerValue(txtFromRegistrationDate2).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtToRegistrationDate2).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.VisitStatus.CANCELLED);
            }

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

            string registrationStatus = "";
            if (cboRegistrationStatus2.Value != null)
            {
                registrationStatus = cboRegistrationStatus2.Value.ToString();
                if (registrationStatus == "1")
                {
                    filterExpression += string.Format(" AND GCVisitStatus = '{0}'", Constant.VisitStatus.CLOSED);
                }
                else if (registrationStatus == "2")
                {
                    filterExpression += string.Format(" AND GCVisitStatus != '{0}'", Constant.VisitStatus.CLOSED);
                }
            }

            string id = Page.Request.QueryString["id"];
            if (id == "mfs")
            {
                string resultType = cboResultType2.Value.ToString();
                switch (resultType)
                {
                    case "1":
                        filterExpression += string.Format(" AND VisitID NOT IN (SELECT VisitID FROM VisitMRFolderStatus)");
                        break;
                    case "2":
                        filterExpression += string.Format(" AND VisitID IN (SELECT VisitID FROM VisitMRFolderStatus)");
                        break;
                    default:
                        break;
                }
            }
            else
            {
                string resultType = "";
                if (cboResultType2.Value != null)
                {
                    resultType = cboResultType2.Value.ToString();
                    switch (resultType)
                    {
                        case "1":
                            filterExpression += string.Format(" AND VisitID NOT IN (SELECT VisitID FROM VisitMRFolderStatus)");
                            break;
                        case "2":
                            filterExpression += string.Format(" AND VisitID IN (SELECT VisitID FROM PatientDiagnosis WHERE FinalDiagnoseID IS NULL AND IsDeleted = 0)");
                            break;
                        default:
                            break;
                    }
                }

                if (chkAllEmpty2.Checked)
                {
                    filterExpression += string.Format(" AND ChiefComplaint IS NULL AND ChiefComplaint IS NULL AND PatientDiagnosis IS NULL AND PhysicianDischargedDate IS NULL AND DischargeDate IS NULL");
                }

                if (chkChiefComplaint2.Checked)
                {
                    filterExpression += string.Format(" AND ChiefComplaint IS NULL");
                }

                if (chkPatientDiagnosis2.Checked)
                {
                    filterExpression += string.Format(" AND PatientDiagnosis IS NULL");
                }

                if (chkPhysicianDischarge2.Checked)
                {
                    filterExpression += string.Format(" AND PhysicianDischargedDate IS NULL");
                }

                if (chkDischarge2.Checked)
                {
                    filterExpression += string.Format(" AND DischargeDate IS NULL");
                }
            }

            if (chkIsControlDocument2.Checked)
            {
                filterExpression += string.Format(" AND GCMedicalFileStatus = '{0}'", Constant.MedicalFileStatus.PROCESSED);
            }

            string condition = string.Empty;
            if (cboPatientCondition2.Value.ToString() != "")
            {
                condition = cboPatientCondition2.Value.ToString();
                switch (condition)
                {
                    case "0":
                        filterExpression += string.Format("");
                        break;
                    case "1":
                        filterExpression += string.Format(" AND IsPregnant = 1");
                        break;
                    case "2":
                        filterExpression += string.Format(" AND IsParturition = 1");
                        break;
                    case "3":
                        filterExpression += string.Format(" AND IsNewBorn = 1");
                        break;
                }
            }
            return filterExpression;
        }

        public override void OnGrdRowClickTestOrder(string transactionNo, string TestOrderID)
        {
            TestOrderID = "";
            string id = Page.Request.QueryString["id"];
            string url = "";

            if (id == "ps" || id == "mfs" || id == "ina")
            {
                if (id == "ps" || id == "ina")
                {
                    LastContentVisitListMR lc = new LastContentVisitListMR()
                    {
                        ContentID = hdnLastContentID.Value,
                        DepartmentID = cboPatientFrom2.Value.ToString(),
                        FromDate = Helper.GetDatePickerValue(txtFromRegistrationDate2).ToString(Constant.FormatString.DATE_FORMAT_112),
                        ToDate = Helper.GetDatePickerValue(txtToRegistrationDate2).ToString(Constant.FormatString.DATE_FORMAT_112),
                        HealthcareServiceUnitID = string.IsNullOrEmpty(hdnServiceUnitID2.Value) ? 0 : Convert.ToInt32(hdnServiceUnitID2.Value),
                        ServiceUnitCode = txtServiceUnitCode2.Text,
                        ServiceUnitName = txtServiceUnitName2.Text,
                        QuickText = txtSearchView2.Text,
                        QuickFilterExpression = hdnFilterExpressionQuickSearch2.Value,
                        RegistrationStatus = cboRegistrationStatus2.Value.ToString(),
                        ProcessStatus = cboResultType2.Value.ToString(),
                        IsAllEmpty = chkAllEmpty2.Checked,
                        IsChiefComplaintNull = chkChiefComplaint2.Checked,
                        IsPatientDiagnosisNull = chkPatientDiagnosis2.Checked,
                        IsPhysicianDischargeNull = chkPhysicianDischarge2.Checked,
                        IsDischargeDateNull = chkDischarge2.Checked,
                        DateFilter = cboDateFilter2.Value.ToString(),
                        PatientCondition = cboPatientCondition2.Value.ToString()
                    };
                    AppSession.LastContentVisitListMR = lc;

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

                    url = "~/Program/PatientMedicalRecord/MRPatientSOAP/MRPatientDiagnose/MRPatientDiagnose.aspx";
                    if (id == "ina")
                    {
                        string parentCode = Constant.MenuCode.MedicalRecord.INACBGS;
                        string filterExpression = string.Format("ParentCode = '{0}'", parentCode);
                        List<GetUserMenuAccess> lstMenu = BusinessLayer.GetUserMenuAccess(Constant.Module.MEDICAL_RECORD, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, filterExpression);
                        int parentID = (int)lstMenu.Where(p => p.MenuIndex > 0).OrderBy(p => p.MenuIndex).FirstOrDefault().MenuID;

                        filterExpression = string.Format("ParentID = {0}", parentID);
                        lstMenu = BusinessLayer.GetUserMenuAccess(Constant.Module.MEDICAL_RECORD, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, filterExpression);
                        GetUserMenuAccess menu = lstMenu.OrderBy(p => p.MenuIndex).FirstOrDefault();
                        url = Page.ResolveUrl(menu.MenuUrl);
                    }
                }
                else if (id == "mfs")
                {
                    url = string.Format("~/Program/PatientMedicalRecord/MedicalFolderStatus/MedicalFolderStatusEntry.aspx?id={0}", transactionNo);
                }
            }
            Response.Redirect(url);
        }

        public override string GetSortingTestOrder()
        {
            string sortBy = "";

            return sortBy;
        }
        #endregion

        #region Ada Diagnosa
        public override string GetFilterExpression()
        {
            string filterExpression = string.Format("IsNeedCodification = 1");
            if (cboDateFilter.SelectedIndex == 0)
            {
                filterExpression += string.Format(" AND (VisitDate BETWEEN '{0}' AND '{1}') AND GCVisitStatus != '{2}' AND FinalDiagnoseID IS NOT NULL", Helper.GetDatePickerValue(txtFromRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtToRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.VisitStatus.CANCELLED);
            }
            else
            {
                filterExpression += string.Format(" AND (DischargeDate BETWEEN '{0}' AND '{1}') AND GCVisitStatus != '{2}' AND FinalDiagnoseID IS NOT NULL", Helper.GetDatePickerValue(txtFromRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtToRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.VisitStatus.CANCELLED);
            }
            if (hdnServiceUnitID.Value != "0" && hdnServiceUnitID.Value != "")
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", hdnServiceUnitID.Value);
            else if (cboPatientFrom.Value != null)
                filterExpression += string.Format(" AND DepartmentID = '{0}'", cboPatientFrom.Value);
            if (hdnFilterExpressionQuickSearch.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);

            string registrationStatus = "";
            if (cboRegistrationStatus.Value != null)
            {
                registrationStatus = cboRegistrationStatus.Value.ToString();
                if (registrationStatus == "1")
                    filterExpression += string.Format(" AND GCVisitStatus = '{0}'", Constant.VisitStatus.CLOSED);
                else if (registrationStatus == "2")
                    filterExpression += string.Format(" AND GCVisitStatus != '{0}'", Constant.VisitStatus.CLOSED);
            }

            string id = Page.Request.QueryString["id"];
            if (id == "mfs")
            {
                string resultType = cboResultType.Value.ToString();
                switch (resultType)
                {
                    case "1":
                        filterExpression += string.Format(" AND VisitID NOT IN (SELECT VisitID FROM VisitMRFolderStatus)");
                        break;
                    case "2":
                        filterExpression += string.Format(" AND VisitID IN (SELECT VisitID FROM VisitMRFolderStatus)");
                        break;
                    default:
                        break;
                }
            }
            else
            {
                string resultType = "";
                if (cboResultType.Value != null)
                {
                    resultType = cboResultType.Value.ToString();
                    switch (resultType)
                    {
                        case "1":
                            filterExpression += string.Format(" AND VisitID NOT IN (SELECT VisitID FROM VisitMRFolderStatus)");
                            break;
                        case "2":
                            filterExpression += string.Format(" AND VisitID IN (SELECT VisitID FROM PatientDiagnosis WHERE FinalDiagnoseID IS NOT NULL AND IsDeleted = 0)");
                            break;
                        default:
                            break;
                    }
                }

                if (chkAllEmpty.Checked)
                {
                    filterExpression += string.Format(" AND ChiefComplaint IS NULL AND ChiefComplaint IS NULL AND PatientDiagnosis IS NULL AND PhysicianDischargedDate IS NULL AND DischargeDate IS NULL");
                }

                if (chkChiefComplaint.Checked)
                {
                    filterExpression += string.Format(" AND ChiefComplaint IS NULL");
                }

                if (chkPatientDiagnosis.Checked)
                {
                    filterExpression += string.Format(" AND PatientDiagnosis IS NULL");
                }

                if (chkPhysicianDischarge.Checked)
                {
                    filterExpression += string.Format(" AND PhysicianDischargedDate IS NULL");
                }

                if (chkDischarge.Checked)
                {
                    filterExpression += string.Format(" AND DischargeDate IS NULL");
                }
            }

            if (chkIsControlDocument.Checked)
                filterExpression += string.Format(" AND GCMedicalFileStatus = '{0}'", Constant.MedicalFileStatus.PROCESSED);

            string condition = string.Empty;
            if (cboPatientCondition.Value.ToString() != "")
            {
                condition = cboPatientCondition.Value.ToString();
                switch (condition)
                {
                    case "0":
                        filterExpression += string.Format("");
                        break;
                    case "1":
                        filterExpression += string.Format(" AND IsPregnant = 1");
                        break;
                    case "2":
                        filterExpression += string.Format(" AND IsParturition = 1");
                        break;
                    case "3":
                        filterExpression += string.Format(" AND IsNewBorn = 1");
                        break;
                }
            }
            return filterExpression;
        }

        public override void OnGrdRowClick(string transactionNo)
        {
            string id = Page.Request.QueryString["id"];
            string url = "";

            if (id == "ps" || id == "mfs" || id == "ina")
            {
                if (id == "ps" || id == "ina")
                {
                    LastContentVisitListMR lc = new LastContentVisitListMR()
                    {
                        ContentID = hdnLastContentID.Value,
                        DepartmentID = cboPatientFrom.Value.ToString(),
                        FromDate = Helper.GetDatePickerValue(txtFromRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112),
                        ToDate = Helper.GetDatePickerValue(txtToRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112),
                        HealthcareServiceUnitID = string.IsNullOrEmpty(hdnServiceUnitID.Value) ? 0 : Convert.ToInt32(hdnServiceUnitID.Value),
                        ServiceUnitCode = txtServiceUnitCode.Text,
                        ServiceUnitName = txtServiceUnitName.Text,
                        QuickText = txtSearchView.Text,
                        QuickFilterExpression = hdnFilterExpressionQuickSearch.Value,
                        RegistrationStatus = cboRegistrationStatus.Value.ToString(),
                        ProcessStatus = cboResultType.Value.ToString(),
                        IsAllEmpty = chkAllEmpty.Checked,
                        IsChiefComplaintNull = chkChiefComplaint.Checked,
                        IsPatientDiagnosisNull = chkPatientDiagnosis.Checked,
                        IsPhysicianDischargeNull = chkPhysicianDischarge.Checked,
                        IsDischargeDateNull = chkDischarge.Checked,
                        DateFilter = cboDateFilter.Value.ToString(),
                        PatientCondition = cboPatientCondition.Value.ToString()
                    };
                    AppSession.LastContentVisitListMR = lc;

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

                    url = "~/Program/PatientMedicalRecord/MRPatientSOAP/MRPatientDiagnose/MRPatientDiagnose.aspx";
                    if (id == "ina")
                    {
                        string parentCode = Constant.MenuCode.MedicalRecord.INACBGS;
                        string filterExpression = string.Format("ParentCode = '{0}'", parentCode);
                        List<GetUserMenuAccess> lstMenu = BusinessLayer.GetUserMenuAccess(Constant.Module.MEDICAL_RECORD, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, filterExpression);
                        int parentID = (int)lstMenu.Where(p => p.MenuIndex > 0).OrderBy(p => p.MenuIndex).FirstOrDefault().MenuID;

                        filterExpression = string.Format("ParentID = {0}", parentID);
                        lstMenu = BusinessLayer.GetUserMenuAccess(Constant.Module.MEDICAL_RECORD, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, filterExpression);
                        GetUserMenuAccess menu = lstMenu.OrderBy(p => p.MenuIndex).FirstOrDefault();
                        url = Page.ResolveUrl(menu.MenuUrl);
                    }
                }
                else if (id == "mfs")
                {
                    url = string.Format("~/Program/PatientMedicalRecord/MedicalFolderStatus/MedicalFolderStatusEntry.aspx?id={0}", transactionNo);
                }
            }
            Response.Redirect(url);
        }
        #endregion

        #region Tidak Perlu Kodefikasi
        public override string GetFilterExpressionCodification()
        {
            string filterExpression = string.Format("IsNeedCodification = 0");

            if (cboDateFilter3.SelectedIndex == 0)
            {
                filterExpression += string.Format(" AND (VisitDate BETWEEN '{0}' AND '{1}') AND GCVisitStatus != '{2}'", Helper.GetDatePickerValue(txtFromRegistrationDate3).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtToRegistrationDate3).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.VisitStatus.CANCELLED);
            }
            else
            {
                filterExpression += string.Format(" AND (DischargeDate BETWEEN '{0}' AND '{1}') AND GCVisitStatus != '{2}'", Helper.GetDatePickerValue(txtFromRegistrationDate3).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtToRegistrationDate3).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.VisitStatus.CANCELLED);
            }

            if (hdnServiceUnitID3.Value != "0" && hdnServiceUnitID3.Value != "")
            {
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", hdnServiceUnitID3.Value);
            }
            else if (cboPatientFrom3.Value != null)
            {
                filterExpression += string.Format(" AND DepartmentID = '{0}'", cboPatientFrom3.Value);
            }

            if (hdnFilterExpressionQuickSearch3.Value != "")
            {
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch3.Value);
            }

            string registrationStatus = "";
            if (cboRegistrationStatus3.Value != null)
            {
                registrationStatus = cboRegistrationStatus3.Value.ToString();
                if (registrationStatus == "1")
                {
                    filterExpression += string.Format(" AND GCVisitStatus = '{0}'", Constant.VisitStatus.CLOSED);
                }
                else if (registrationStatus == "2")
                {
                    filterExpression += string.Format(" AND GCVisitStatus != '{0}'", Constant.VisitStatus.CLOSED);
                }
            }

            string id = Page.Request.QueryString["id"];
            if (id == "mfs")
            {
                string resultType = cboResultType3.Value.ToString();
                switch (resultType)
                {
                    case "1":
                        filterExpression += string.Format(" AND VisitID NOT IN (SELECT VisitID FROM VisitMRFolderStatus)");
                        break;
                    case "2":
                        filterExpression += string.Format(" AND VisitID IN (SELECT VisitID FROM VisitMRFolderStatus)");
                        break;
                    default:
                        break;
                }
            }
            else
            {
                string resultType = "";
                if (cboResultType3.Value != null)
                {
                    resultType = cboResultType3.Value.ToString();
                    switch (resultType)
                    {
                        case "1":
                            filterExpression += string.Format(" AND VisitID NOT IN (SELECT VisitID FROM VisitMRFolderStatus)");
                            break;
                        case "2":
                            filterExpression += string.Format(" AND VisitID IN (SELECT VisitID FROM PatientDiagnosis WHERE FinalDiagnoseID IS NULL AND IsDeleted = 0)");
                            break;
                        default:
                            break;
                    }
                }

                if (chkAllEmpty3.Checked)
                {
                    filterExpression += string.Format(" AND ChiefComplaint IS NULL AND ChiefComplaint IS NULL AND PatientDiagnosis IS NULL AND PhysicianDischargedDate IS NULL AND DischargeDate IS NULL");
                }

                if (chkChiefComplaint3.Checked)
                {
                    filterExpression += string.Format(" AND ChiefComplaint IS NULL");
                }

                if (chkPatientDiagnosis3.Checked)
                {
                    filterExpression += string.Format(" AND PatientDiagnosis IS NULL");
                }

                if (chkPhysicianDischarge3.Checked)
                {
                    filterExpression += string.Format(" AND PhysicianDischargedDate IS NULL");
                }

                if (chkDischarge3.Checked)
                {
                    filterExpression += string.Format(" AND DischargeDate IS NULL");
                }
            }

            if (chkIsControlDocument3.Checked)
            {
                filterExpression += string.Format(" AND GCMedicalFileStatus = '{0}'", Constant.MedicalFileStatus.PROCESSED);
            }

            string condition = string.Empty;
            if (cboPatientCondition3.Value.ToString() != "")
            {
                condition = cboPatientCondition3.Value.ToString();
                switch (condition)
                {
                    case "0":
                        filterExpression += string.Format("");
                        break;
                    case "1":
                        filterExpression += string.Format(" AND IsPregnant = 1");
                        break;
                    case "2":
                        filterExpression += string.Format(" AND IsParturition = 1");
                        break;
                    case "3":
                        filterExpression += string.Format(" AND IsNewBorn = 1");
                        break;
                }
            }
            return filterExpression;
        }

        public override void OnGrdRowClickCodification(string transactionNo, string TestOrderID, string VisitID)
        {
            TestOrderID = "";
            VisitID = "";
            string id = Page.Request.QueryString["id"];
            string url = "";

            if (id == "ps" || id == "mfs" || id == "ina")
            {
                if (id == "ps" || id == "ina")
                {

                        string ContentID = hdnLastContentID.Value;
                        string DepartmentID = cboPatientFrom3.Value.ToString();
                        string FromDate = Helper.GetDatePickerValue(txtFromRegistrationDate3).ToString(Constant.FormatString.DATE_FORMAT_112);
                        string ToDate = Helper.GetDatePickerValue(txtToRegistrationDate3).ToString(Constant.FormatString.DATE_FORMAT_112);
                        int HealthcareServiceUnitID = string.IsNullOrEmpty(hdnServiceUnitID3.Value) ? 0 : Convert.ToInt32(hdnServiceUnitID3.Value);
                        string ServiceUnitCode = txtServiceUnitCode3.Text;
                        string ServiceUnitName = txtServiceUnitName3.Text;
                        string QuickText = txtSearchView3.Text;
                        string QuickFilterExpression = hdnFilterExpressionQuickSearch3.Value;
                        string RegistrationStatus = cboRegistrationStatus3.Value.ToString();
                        string ProcessStatus = cboResultType3.Value.ToString();
                        bool IsAllEmpty = chkAllEmpty3.Checked;
                        bool IsChiefComplaintNull = chkChiefComplaint3.Checked;
                        bool IsPatientDiagnosisNull = chkPatientDiagnosis3.Checked;
                        bool IsPhysicianDischargeNull = chkPhysicianDischarge3.Checked;
                        bool IsDischargeDateNull = chkDischarge3.Checked;
                        string DateFilter = cboDateFilter3.Value.ToString();
                        string PatientCondition = cboPatientCondition3.Value.ToString();

                    LastContentVisitListMR lc = new LastContentVisitListMR()
                    {
                        ContentID = hdnLastContentID.Value,
                        DepartmentID = cboPatientFrom3.Value.ToString(),
                        FromDate = Helper.GetDatePickerValue(txtFromRegistrationDate3).ToString(Constant.FormatString.DATE_FORMAT_112),
                        ToDate = Helper.GetDatePickerValue(txtToRegistrationDate3).ToString(Constant.FormatString.DATE_FORMAT_112),
                        HealthcareServiceUnitID = string.IsNullOrEmpty(hdnServiceUnitID3.Value) ? 0 : Convert.ToInt32(hdnServiceUnitID3.Value),
                        ServiceUnitCode = txtServiceUnitCode3.Text,
                        ServiceUnitName = txtServiceUnitName3.Text,
                        QuickText = txtSearchView3.Text,
                        QuickFilterExpression = hdnFilterExpressionQuickSearch3.Value,
                        RegistrationStatus = cboRegistrationStatus3.Value.ToString(),
                        ProcessStatus = cboResultType3.Value.ToString(),
                        IsAllEmpty = chkAllEmpty3.Checked,
                        IsChiefComplaintNull = chkChiefComplaint3.Checked,
                        IsPatientDiagnosisNull = chkPatientDiagnosis3.Checked,
                        IsPhysicianDischargeNull = chkPhysicianDischarge3.Checked,
                        IsDischargeDateNull = chkDischarge3.Checked,
                        DateFilter = cboDateFilter3.Value.ToString(),
                        PatientCondition = cboPatientCondition3.Value.ToString()
                    };
                    AppSession.LastContentVisitListMR = lc;

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

                    url = "~/Program/PatientMedicalRecord/MRPatientSOAP/MRPatientDiagnose/MRPatientDiagnose.aspx";
                    if (id == "ina")
                    {
                        string parentCode = Constant.MenuCode.MedicalRecord.INACBGS;
                        string filterExpression = string.Format("ParentCode = '{0}'", parentCode);
                        List<GetUserMenuAccess> lstMenu = BusinessLayer.GetUserMenuAccess(Constant.Module.MEDICAL_RECORD, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, filterExpression);
                        int parentID = (int)lstMenu.Where(p => p.MenuIndex > 0).OrderBy(p => p.MenuIndex).FirstOrDefault().MenuID;

                        filterExpression = string.Format("ParentID = {0}", parentID);
                        lstMenu = BusinessLayer.GetUserMenuAccess(Constant.Module.MEDICAL_RECORD, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, filterExpression);
                        GetUserMenuAccess menu = lstMenu.OrderBy(p => p.MenuIndex).FirstOrDefault();
                        url = Page.ResolveUrl(menu.MenuUrl);
                    }
                }
                else if (id == "mfs")
                {
                    url = string.Format("~/Program/PatientMedicalRecord/MedicalFolderStatus/MedicalFolderStatusEntry.aspx?id={0}", transactionNo);
                }
            }
            Response.Redirect(url);
        }

        public override string GetSortingCodification()
        {
            string sortBy = "";

            return sortBy;
        }
        #endregion

        protected void btnDownloadProcess_Click(object sender, EventArgs e)
        {
            #region filterExpression

            string filterExpression = GetFilterExpression();

            #endregion

            #region download
            string result = "";
            string reportCode = "";
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '001'")).FirstOrDefault();
            if (oHealthcare.Initial == "NHS")
            {
                reportCode = string.Format("ReportCode = '{0}'", "MR-00017");
            }
            else
            {
                reportCode = string.Format("ReportCode = '{0}'", "MR-00013");
            }
            ReportMaster rm = BusinessLayer.GetReportMasterList(reportCode).FirstOrDefault();
            string fileName = string.Format(@"{0}_{1}_{2}_{3}", rm.ClassName, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112), DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString());
            try
            {
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".csv");
                Response.Charset = "";
                Response.ContentType = "application/text";

                StringBuilder sbResult = new StringBuilder();

                MethodInfo method = typeof(BusinessLayer).GetMethod(rm.ObjectTypeName, new[] { typeof(string) });
                Object obj = method.Invoke(null, new string[] { filterExpression });
                IList collection = (IList)obj;
                dynamic fields = collection[0];

                foreach (var prop in fields.GetType().GetProperties())
                {
                    sbResult.Append(prop.Name);
                    sbResult.Append(",");
                }
                sbResult.Append("\r\n");

                foreach (object temp in collection)
                {
                    foreach (var prop in temp.GetType().GetProperties())
                    {
                        var text = prop.GetValue(temp, null);
                        string textValid = "";

                        if (text != null)
                        {
                            textValid = text.ToString();
                        }

                        sbResult.Append(textValid.Replace(',', '_'));
                        sbResult.Append(",");
                    }

                    sbResult.Append("\r\n");
                }

                Response.Output.Write(sbResult.ToString());
                result = "success";
            }
            catch (Exception ex)
            {
                result = string.Format("fail|{0}", ex.Message);
            }
            finally
            {
                Response.Flush();
                Response.End();
            }
            #endregion
        }

        protected void btnDownloadProcess2_Click(object sender, EventArgs e)
        {
            #region filterExpression

            string filterExpression = GetFilterExpressionTestOrder();

            #endregion

            #region download
            string result = "";
            string reportCode = "";
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '001'")).FirstOrDefault();
            if (oHealthcare.Initial == "NHS")
            {
                reportCode = string.Format("ReportCode = '{0}'", "MR-00017");
            }
            else
            {
                reportCode = string.Format("ReportCode = '{0}'", "MR-00012");
            }
            ReportMaster rm = BusinessLayer.GetReportMasterList(reportCode).FirstOrDefault();
            string fileName = string.Format(@"{0}_{1}_{2}_{3}", rm.ClassName, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112), DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString());
            try
            {
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".csv");
                Response.Charset = "";
                Response.ContentType = "application/text";

                StringBuilder sbResult = new StringBuilder();

                MethodInfo method = typeof(BusinessLayer).GetMethod(rm.ObjectTypeName, new[] { typeof(string) });
                Object obj = method.Invoke(null, new string[] { filterExpression });
                IList collection = (IList)obj;
                dynamic fields = collection[0];

                foreach (var prop in fields.GetType().GetProperties())
                {
                    sbResult.Append(prop.Name);
                    sbResult.Append(",");
                }
                sbResult.Append("\r\n");

                foreach (object temp in collection)
                {
                    foreach (var prop in temp.GetType().GetProperties())
                    {
                        var text = prop.GetValue(temp, null);
                        string textValid = "";

                        if (text != null)
                        {
                            textValid = text.ToString();
                        }

                        sbResult.Append(textValid.Replace(',', '_'));
                        sbResult.Append(",");
                    }

                    sbResult.Append("\r\n");
                }

                Response.Output.Write(sbResult.ToString());
                result = "success";
            }
            catch (Exception ex)
            {
                result = string.Format("fail|{0}", ex.Message);
            }
            finally
            {
                Response.Flush();
                Response.End();
            }
            #endregion
        }

        protected void btnDownloadProcess3_Click(object sender, EventArgs e)
        {
            #region filterExpression

            string filterExpression = GetFilterExpressionCodification();

            #endregion

            #region download
            string result = "";
            string reportCode = "";
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '001'")).FirstOrDefault();
            if (oHealthcare.Initial == "NHS")
            {
                reportCode = string.Format("ReportCode = '{0}'", "MR-00017");
            }
            else
            {
                reportCode = string.Format("ReportCode = '{0}'", "MR-00012");
            }
            ReportMaster rm = BusinessLayer.GetReportMasterList(reportCode).FirstOrDefault();
            string fileName = string.Format(@"{0}_{1}_{2}_{3}", rm.ClassName, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112), DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString());
            try
            {
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".csv");
                Response.Charset = "";
                Response.ContentType = "application/text";

                StringBuilder sbResult = new StringBuilder();

                MethodInfo method = typeof(BusinessLayer).GetMethod(rm.ObjectTypeName, new[] { typeof(string) });
                Object obj = method.Invoke(null, new string[] { filterExpression });
                IList collection = (IList)obj;
                dynamic fields = collection[0];

                foreach (var prop in fields.GetType().GetProperties())
                {
                    sbResult.Append(prop.Name);
                    sbResult.Append(",");
                }
                sbResult.Append("\r\n");

                foreach (object temp in collection)
                {
                    foreach (var prop in temp.GetType().GetProperties())
                    {
                        var text = prop.GetValue(temp, null);
                        string textValid = "";

                        if (text != null)
                        {
                            textValid = text.ToString();
                        }

                        sbResult.Append(textValid.Replace(',', '_'));
                        sbResult.Append(",");
                    }

                    sbResult.Append("\r\n");
                }

                Response.Output.Write(sbResult.ToString());
                result = "success";
            }
            catch (Exception ex)
            {
                result = string.Format("fail|{0}", ex.Message);
            }
            finally
            {
                Response.Flush();
                Response.End();
            }
            #endregion
        }
    }
}

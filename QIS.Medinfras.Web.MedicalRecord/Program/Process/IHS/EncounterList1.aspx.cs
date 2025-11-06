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
    public partial class EncounterList1 : BasePagePatientOrderSatuSehatSudahIntegrasi
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
                hdnLastContentID.Value = "containerBelumIntegrasi";
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
            #region Belum Integrasi
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

            #region Dalam Integrasi
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

            #region Sudah Integrasi
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
                    if (lc.ContentID.Equals("containerDalamIntegrasi"))
                    {
                        LastPagingMR2 mr2 = AppSession.LastPagingMR2;
                        hdnLastPage.Value = mr2.PageID.ToString();

                        hdnLastContentID.Value = "containerDalamIntegrasi";
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
                    else if (lc.ContentID.Equals("containerBelumIntegrasi"))
                    {
                        LastPagingMR1 mr1 = AppSession.LastPagingMR1;
                        hdnLastPage.Value = mr1.PageID.ToString();

                        hdnLastContentID.Value = "containerBelumIntegrasi";
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
                    else if (lc.ContentID.Equals("containerSudahIntegrasi"))
                    {
                        LastPagingMR3 mr3 = AppSession.LastPagingMR3;
                        hdnLastPage.Value = mr3.PageID.ToString();

                        hdnLastContentID.Value = "containerSudahIntegrasi";
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
        public override string GetFilterExpressionTestOrder()
        {
            string result = "";

            return result;
        }
        public override string GetSortingTestOrder()
        {
            string result = "";

            return result;
        }

        public override string GetFilterExpression()
        {
            string result = "";

            return result;
        }
        public override void OnGrdRowClick(string transactionNo)
        {

        }
        public override void OnGrdRowClickTestOrder(string transactionNo, string TestOrderID)
        {
            
        }

        #region Belum Integrasi
        public override string GetFilterExpressionSatuSehatBelumIntegrasi()
        {
            string filterExpression = string.Format("IsNeedCodification = 1 AND GCSatuSehatIntegrationStatus = '{0}'", Constant.SatuSehat_Bridging_Status.OPEN);

            if (chkOnlyAgreeBelumIntegrasi.Checked)
            {
                filterExpression += " AND PelepasanInformasiSatuSEHAT = '1'";
            }

            if (cboDateFilter2.SelectedIndex == 0)
            {
                filterExpression += string.Format(" AND (CONVERT(VARCHAR,VisitDate,112) BETWEEN '{0}' AND '{1}') AND GCVisitStatus != '{2}'", Helper.GetDatePickerValue(txtFromRegistrationDate2).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtToRegistrationDate2).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.VisitStatus.CANCELLED);
            }
            else
            {
                filterExpression += string.Format(" AND (CONVERT(VARCHAR,DischargeDate,112) BETWEEN '{0}' AND '{1}') AND GCVisitStatus != '{2}'", Helper.GetDatePickerValue(txtFromRegistrationDate2).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtToRegistrationDate2).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.VisitStatus.CANCELLED);
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

            //string id = Page.Request.QueryString["id"];
            //if (id == "mfs")
            //{
            //    string resultType = cboResultType2.Value.ToString();
            //    switch (resultType)
            //    {
            //        case "1":
            //            filterExpression += string.Format(" AND VisitID NOT IN (SELECT VisitID FROM VisitMRFolderStatus)");
            //            break;
            //        case "2":
            //            filterExpression += string.Format(" AND VisitID IN (SELECT VisitID FROM VisitMRFolderStatus)");
            //            break;
            //        default:
            //            break;
            //    }
            //}
            //else
            //{
            //    string resultType = "";
            //    if (cboResultType2.Value != null)
            //    {
            //        resultType = cboResultType2.Value.ToString();
            //        switch (resultType)
            //        {
            //            case "1":
            //                filterExpression += string.Format(" AND VisitID NOT IN (SELECT VisitID FROM VisitMRFolderStatus)");
            //                break;
            //            case "2":
            //                filterExpression += string.Format(" AND VisitID IN (SELECT VisitID FROM PatientDiagnosis WHERE FinalDiagnoseID IS NULL AND IsDeleted = 0)");
            //                break;
            //            default:
            //                break;
            //        }
            //    }

                //if (chkAllEmpty2.Checked)
                //{
                //    filterExpression += string.Format(" AND ChiefComplaint IS NULL AND ChiefComplaint IS NULL AND PatientDiagnosis IS NULL AND PhysicianDischargedDate IS NULL AND DischargeDate IS NULL");
                //}

                //if (chkChiefComplaint2.Checked)
                //{
                //    filterExpression += string.Format(" AND ChiefComplaint IS NULL");
                //}

                //if (chkPatientDiagnosis2.Checked)
                //{
                //    filterExpression += string.Format(" AND PatientDiagnosis IS NULL");
                //}

                //if (chkPhysicianDischarge2.Checked)
                //{
                //    filterExpression += string.Format(" AND PhysicianDischargedDate IS NULL");
                //}

                //if (chkDischarge2.Checked)
                //{
                //    filterExpression += string.Format(" AND DischargeDate IS NULL");
                //}
            //}

            //if (chkIsControlDocument2.Checked)
            //{
            //    filterExpression += string.Format(" AND GCMedicalFileStatus = '{0}'", Constant.MedicalFileStatus.PROCESSED);
            //}

            //string condition = string.Empty;
            //if (cboPatientCondition2.Value.ToString() != "")
            //{
            //    condition = cboPatientCondition2.Value.ToString();
            //    switch (condition)
            //    {
            //        case "0":
            //            filterExpression += string.Format("");
            //            break;
            //        case "1":
            //            filterExpression += string.Format(" AND IsPregnant = 1");
            //            break;
            //        case "2":
            //            filterExpression += string.Format(" AND IsParturition = 1");
            //            break;
            //        case "3":
            //            filterExpression += string.Format(" AND IsNewBorn = 1");
            //            break;
            //    }
            //}
            return filterExpression;
        }

        public override void OnGrdRowClickSatuSehatBelumIntegrasi(string transactionNo, string TestOrderID, string VisitID)
        {
            
        }

        public override string GetSortingSatuSehatBelumIntegrasi()
        {
            string sortBy = "";

            return sortBy;
        }
        #endregion

        #region Dalam Integrasi
        public override string GetFilterExpressionSatuSehatDalamIntegrasi()
        {
            string filterExpression = string.Format("IsNeedCodification = 1 AND GCSatuSehatIntegrationStatus IN ('{0}','{1}')", Constant.SatuSehat_Bridging_Status.PENDING, Constant.SatuSehat_Bridging_Status.READY);

            if (chkOnlyAgreeDalamIntegrasi.Checked)
            {
                filterExpression += " AND PelepasanInformasiSatuSEHAT = '1'";
            }

            if (cboDateFilter.SelectedIndex == 0)
            {
                filterExpression += string.Format(" AND (CONVERT(VARCHAR,VisitDate,112) BETWEEN '{0}' AND '{1}') AND GCVisitStatus != '{2}'", Helper.GetDatePickerValue(txtFromRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtToRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.VisitStatus.CANCELLED);
            }
            else
            {
                filterExpression += string.Format(" AND (CONVERT(VARCHAR,DischargeDate,112) BETWEEN '{0}' AND '{1}') AND GCVisitStatus != '{2}'", Helper.GetDatePickerValue(txtFromRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtToRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.VisitStatus.CANCELLED);
            }

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

            string registrationStatus = "";
            if (cboRegistrationStatus.Value != null)
            {
                registrationStatus = cboRegistrationStatus.Value.ToString();
                if (registrationStatus == "1")
                {
                    filterExpression += string.Format(" AND GCVisitStatus = '{0}'", Constant.VisitStatus.CLOSED);
                }
                else if (registrationStatus == "2")
                {
                    filterExpression += string.Format(" AND GCVisitStatus != '{0}'", Constant.VisitStatus.CLOSED);
                }
            }

            //string id = Page.Request.QueryString["id"];
            //if (id == "mfs")
            //{
            //    string resultType = cboResultType.Value.ToString();
            //    switch (resultType)
            //    {
            //        case "1":
            //            filterExpression += string.Format(" AND VisitID NOT IN (SELECT VisitID FROM VisitMRFolderStatus)");
            //            break;
            //        case "2":
            //            filterExpression += string.Format(" AND VisitID IN (SELECT VisitID FROM VisitMRFolderStatus)");
            //            break;
            //        default:
            //            break;
            //    }
            //}
            //else
            //{
            //    string resultType = "";
            //    if (cboResultType.Value != null)
            //    {
            //        resultType = cboResultType.Value.ToString();
            //        switch (resultType)
            //        {
            //            case "1":
            //                filterExpression += string.Format(" AND VisitID NOT IN (SELECT VisitID FROM VisitMRFolderStatus)");
            //                break;
            //            case "2":
            //                filterExpression += string.Format(" AND VisitID IN (SELECT VisitID FROM PatientDiagnosis WHERE FinalDiagnoseID IS NULL AND IsDeleted = 0)");
            //                break;
            //            default:
            //                break;
            //        }
            //    }

                //if (chkAllEmpty.Checked)
                //{
                //    filterExpression += string.Format(" AND ChiefComplaint IS NULL AND ChiefComplaint IS NULL AND PatientDiagnosis IS NULL AND PhysicianDischargedDate IS NULL AND DischargeDate IS NULL");
                //}

                //if (chkChiefComplaint.Checked)
                //{
                //    filterExpression += string.Format(" AND ChiefComplaint IS NULL");
                //}

                //if (chkPatientDiagnosis.Checked)
                //{
                //    filterExpression += string.Format(" AND PatientDiagnosis IS NULL");
                //}

                //if (chkPhysicianDischarge.Checked)
                //{
                //    filterExpression += string.Format(" AND PhysicianDischargedDate IS NULL");
                //}

                //if (chkDischarge.Checked)
                //{
                //    filterExpression += string.Format(" AND DischargeDate IS NULL");
                //}
            //}

            //if (chkIsControlDocument.Checked)
            //{
            //    filterExpression += string.Format(" AND GCMedicalFileStatus = '{0}'", Constant.MedicalFileStatus.PROCESSED);
            //}

            //string condition = string.Empty;
            //if (cboPatientCondition.Value.ToString() != "")
            //{
            //    condition = cboPatientCondition.Value.ToString();
            //    switch (condition)
            //    {
            //        case "0":
            //            filterExpression += string.Format("");
            //            break;
            //        case "1":
            //            filterExpression += string.Format(" AND IsPregnant = 1");
            //            break;
            //        case "2":
            //            filterExpression += string.Format(" AND IsParturition = 1");
            //            break;
            //        case "3":
            //            filterExpression += string.Format(" AND IsNewBorn = 1");
            //            break;
            //    }
            //}
            return filterExpression;
        }

        public override void OnGrdRowClickSatuSehatDalamIntegrasi(string transactionNo, string TestOrderID, string VisitID)
        {

        }

        public override string GetSortingSatuSehatDalamIntegrasi()
        {
            string sortBy = "";

            return sortBy;
        }
        #endregion

        #region Sudah Integrasi
        public override string GetFilterExpressionSatuSehatSudahIntegrasi()
        {
            string filterExpression = string.Format("IsNeedCodification = 1 AND GCSatuSehatIntegrationStatus = '{0}'", Constant.SatuSehat_Bridging_Status.SENT);

            if (chkOnlyAgreeSudahIntegrasi.Checked)
            {
                filterExpression += " AND PelepasanInformasiSatuSEHAT = '1'";
            }

            if (cboDateFilter3.SelectedIndex == 0)
            {
                filterExpression += string.Format(" AND (CONVERT(VARCHAR,VisitDate,112) BETWEEN '{0}' AND '{1}') AND GCVisitStatus != '{2}'", Helper.GetDatePickerValue(txtFromRegistrationDate3).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtToRegistrationDate3).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.VisitStatus.CANCELLED);
            }
            else
            {
                filterExpression += string.Format(" AND (CONVERT(VARCHAR,DischargeDate,112) BETWEEN '{0}' AND '{1}') AND GCVisitStatus != '{2}'", Helper.GetDatePickerValue(txtFromRegistrationDate3).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtToRegistrationDate3).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.VisitStatus.CANCELLED);
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

            //string id = Page.Request.QueryString["id"];
            //if (id == "mfs")
            //{
            //    string resultType = cboResultType3.Value.ToString();
            //    switch (resultType)
            //    {
            //        case "1":
            //            filterExpression += string.Format(" AND VisitID NOT IN (SELECT VisitID FROM VisitMRFolderStatus)");
            //            break;
            //        case "2":
            //            filterExpression += string.Format(" AND VisitID IN (SELECT VisitID FROM VisitMRFolderStatus)");
            //            break;
            //        default:
            //            break;
            //    }
            //}
            //else
            //{
            //    string resultType = "";
            //    if (cboResultType3.Value != null)
            //    {
            //        resultType = cboResultType3.Value.ToString();
            //        switch (resultType)
            //        {
            //            case "1":
            //                filterExpression += string.Format(" AND VisitID NOT IN (SELECT VisitID FROM VisitMRFolderStatus)");
            //                break;
            //            case "2":
            //                filterExpression += string.Format(" AND VisitID IN (SELECT VisitID FROM PatientDiagnosis WHERE FinalDiagnoseID IS NULL AND IsDeleted = 0)");
            //                break;
            //            default:
            //                break;
            //        }
            //    }

                //if (chkAllEmpty3.Checked)
                //{
                //    filterExpression += string.Format(" AND ChiefComplaint IS NULL AND ChiefComplaint IS NULL AND PatientDiagnosis IS NULL AND PhysicianDischargedDate IS NULL AND DischargeDate IS NULL");
                //}

                //if (chkChiefComplaint3.Checked)
                //{
                //    filterExpression += string.Format(" AND ChiefComplaint IS NULL");
                //}

                //if (chkPatientDiagnosis3.Checked)
                //{
                //    filterExpression += string.Format(" AND PatientDiagnosis IS NULL");
                //}

                //if (chkPhysicianDischarge3.Checked)
                //{
                //    filterExpression += string.Format(" AND PhysicianDischargedDate IS NULL");
                //}

                //if (chkDischarge3.Checked)
                //{
                //    filterExpression += string.Format(" AND DischargeDate IS NULL");
                //}
            //}

            //if (chkIsControlDocument3.Checked)
            //{
            //    filterExpression += string.Format(" AND GCMedicalFileStatus = '{0}'", Constant.MedicalFileStatus.PROCESSED);
            //}

            //string condition = string.Empty;
            //if (cboPatientCondition3.Value.ToString() != "")
            //{
            //    condition = cboPatientCondition3.Value.ToString();
            //    switch (condition)
            //    {
            //        case "0":
            //            filterExpression += string.Format("");
            //            break;
            //        case "1":
            //            filterExpression += string.Format(" AND IsPregnant = 1");
            //            break;
            //        case "2":
            //            filterExpression += string.Format(" AND IsParturition = 1");
            //            break;
            //        case "3":
            //            filterExpression += string.Format(" AND IsNewBorn = 1");
            //            break;
            //    }
            //}
            return filterExpression;
        }

        public override void OnGrdRowClickSatuSehatSudahIntegrasi(string transactionNo, string TestOrderID, string VisitID)
        {
            
        }

        public override string GetSortingSatuSehatSudahIntegrasi()
        {
            string sortBy = "";

            return sortBy;
        }
        #endregion

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            string errMessage = string.Empty;
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "ready")
                {
                    result = "ready|";
                    string[] resultInfo = onUpdateStatus(Constant.SatuSehat_Bridging_Status.READY, param[1], ref errMessage).Split('|');
                    if (resultInfo[0] == "1")
                    {
                        result += "success";
                    }
                    else
                    {
                        result += "fail|" + errMessage;
                    }
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private string onUpdateStatus(string status, string lstRegID, ref string errMessage)
        {
            string result = "1|";

            List<RegistrationInfo> lstReg = BusinessLayer.GetRegistrationInfoList(string.Format("RegistrationID IN ({0})", lstRegID));

            if (lstReg.Count > 0)
            {
                IDbContext ctx = DbFactory.Configure(true);
                RegistrationInfoDao entityDao = new RegistrationInfoDao(ctx);

                try
                {
                    foreach (RegistrationInfo reg in lstReg)
                    {
                        reg.GCSatuSehatIntegrationStatus = status;
                        reg.SatuSehatIntegratedDateTime = DateTime.Now;
                        reg.SatuSehatIntegratedBy = AppSession.UserLogin.UserID;
                        entityDao.Update(reg);
                    }

                    ctx.CommitTransaction();
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;

                    ctx.RollBackTransaction();
                }
                finally
                {
                    ctx.Close();
                }
            }
            else
            {
                errMessage = "0|Tidak ada registrasi yang ditemukan untuk proses kirim ke Satu Sehat";
            }

            return result;
        }
    }
}

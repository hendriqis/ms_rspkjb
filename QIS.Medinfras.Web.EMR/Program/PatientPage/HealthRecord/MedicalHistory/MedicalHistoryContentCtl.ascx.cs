using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common.UI;
using System.Web.UI.HtmlControls;
using QIS.Medinfras.Web.Common;
using System.Text;
using System.Text.RegularExpressions;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class MedicalHistoryContentCtl : BaseViewPopupCtl
    {
        protected string _visitID = "";
        protected string _isFromMigration = "0";
        protected List<vReviewOfSystemDt> lstReviewOfSystemDt = null;
        protected List<vVitalSignDt> lstVitalSignDt = null;
        protected List<vPatientChargesDt> lstLaboratory = null;
        protected vPastMedical _vPastMedical = null;

        class SubjectiveContent
        {
            public string HistoryDate { get; set; }
            public string HistoryTime { get; set; }
            public string ParamedicName { get; set; }
            public string ChiefComplaintText { get; set; }
            public string HPISummaryText { get; set; }
        }

        class DiagnosisInfo
        {
            public string DiagnosisDate { get; set; }
            public string DiagnosisTime { get; set; }
            public string PhysicianName { get; set; }
            public string DiagnosisText { get; set; }
            public string DiagnosisType { get; set; }
        }

        class LaboratoryHdInfo
        {
            public int ChargesID { get; set; }
            public string TestDate { get; set; }
            public string TestTime { get; set; }
            public string PhysicianName { get; set; }
            public int ItemID { get; set; }
            public string ItemName { get; set; }
        }

        class LaboratoryDtInfo
        {
            public int ChargesID { get; set; }
            public int ItemID { get; set; }
            public string FractionName { get; set; }
            public string ResultValue { get; set; }
            public string ResultUnit { get; set; }
            public string RefRange { get; set; }
            public string ResultFlag { get; set; }
            public bool IsNormal { get; set; }
            public bool IsPanicRange { get; set; }
        }

        class ImagingHdInfo
        {
            public int ChargesID { get; set; }
            public string TestDate { get; set; }
            public string TestTime { get; set; }
            public string PhysicianName { get; set; }
            public int ItemID { get; set; }
            public string ItemName { get; set; }
        }

        class ImagingDtInfo
        {
            public int ChargesID { get; set; }
            public int ItemID { get; set; }
            public string ItemName { get; set; }
            public string ResultValue { get; set; }
        }

        public override void InitializeDataControl(string queryString)
        {
            string filterExpression = string.Empty;
            string[] param = queryString.Split('|');
            _visitID = param[0];
            _isFromMigration = param[1];

            List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID, Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID));
            string laboratoryID = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).ParameterValue;
            string imagingID = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID).ParameterValue;

            List<vHealthcareServiceUnit> lstHealthcareServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND ServiceUnitID IN ({1},{2}) AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, laboratoryID, imagingID));
            hdnHSUImagingID.Value = lstHealthcareServiceUnit.FirstOrDefault(p => p.ServiceUnitID == Convert.ToInt32(imagingID)).HealthcareServiceUnitID.ToString();
            hdnHSULaboratoryID.Value = lstHealthcareServiceUnit.FirstOrDefault(p => p.ServiceUnitID == Convert.ToInt32(laboratoryID)).HealthcareServiceUnitID.ToString();

            if (_isFromMigration == "1")
            {
                filterExpression = string.Format("PmHxID = {0} AND IsDeleted = 0", _visitID);
                vPastMedical oHistory = BusinessLayer.GetvPastMedicalList(filterExpression, 5, 1, "PmHxID DESC").FirstOrDefault();
                if (oHistory != null)
                {
                    _vPastMedical = oHistory;
                    divRegistrationDate.InnerHtml = string.Format("{0} {1}", oHistory.cfHistoryDateText, string.Empty);
                    divInformationLine2.InnerHtml = oHistory.ServiceUnitName;
                    divInformationLine3.InnerHtml = oHistory.PhysicianName;
                }
            }
            else
            {
                vConsultVisit entity = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", queryString))[0];
                divRegistrationDate.InnerHtml = string.Format("{0}, {1}", entity.VisitDateInString, entity.VisitTime);
                divInformationLine2.InnerHtml = entity.ServiceUnitName;
                divInformationLine3.InnerHtml = entity.ParamedicName;
            }

            //filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", queryString);
            //rptTreatmentProcedure.DataSource = BusinessLayer.GetvPatientProcedureList(filterExpression, 5, 1, "ID DESC");
            //rptTreatmentProcedure.DataBind();

            LoadSubjectiveContent();
            LoadObjectiveContent();
            LoadAssessmentContent();
            LoadPlanningContent();
            LoadPlanningNotesContent();
            LoadDischargeContent();
        }

        private void LoadDischargeContent()
        {
            string filterExpression = string.Format("FromVisitID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}')", _visitID, Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED);
            rptFollowUpVisit.DataSource = BusinessLayer.GetvAppointmentList(filterExpression, 3, 1, "AppointmentID DESC");
            rptFollowUpVisit.DataBind();
        }

        private void LoadSubjectiveContent()
        {
            if (_isFromMigration == "0")
            {

                List<SubjectiveContent> list = new List<SubjectiveContent>();

                string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", _visitID);
                vChiefComplaint oChiefComplaint = BusinessLayer.GetvChiefComplaintList(filterExpression, 5, 1, "ID DESC").FirstOrDefault();

                if (oChiefComplaint != null)
                {
                    SubjectiveContent oContent = new SubjectiveContent();
                    StringBuilder chiefComplainText = new StringBuilder();

                    oContent.HistoryDate = oChiefComplaint.ObservationDateInString;
                    oContent.HistoryTime = oChiefComplaint.ObservationTime;
                    oContent.ParamedicName = oChiefComplaint.ParamedicName;
                    oContent.ChiefComplaintText = string.Format("{0}{1}{2}",oChiefComplaint.ChiefComplaintText,Environment.NewLine, oChiefComplaint.HPISummary);
                    list.Add(oContent);

                    //if (!String.IsNullOrEmpty(oChiefComplaint.HPISummary))
                    //{
                    //    chiefComplainText.AppendLine("RPS :");
                    //    chiefComplainText.AppendLine(oChiefComplaint.HPISummary);
                    //}

                    //StringBuilder sbNotes = new StringBuilder();
                    //if (!string.IsNullOrEmpty(oChiefComplaint.Location))
                    //    sbNotes.AppendLine(string.Format("- Location    (R) : {0}", oChiefComplaint.Location));
                    //if (!string.IsNullOrEmpty(oChiefComplaint.DisplayQuality))
                    //    sbNotes.AppendLine(string.Format("- Quality     (Q) : {0}", oChiefComplaint.DisplayQuality));
                    //if (!string.IsNullOrEmpty(oChiefComplaint.DisplaySeverity))
                    //    sbNotes.AppendLine(string.Format("- Severity    (S) : {0}", oChiefComplaint.DisplaySeverity));
                    //if (!string.IsNullOrEmpty(oChiefComplaint.DisplayOnset))
                    //    sbNotes.AppendLine(string.Format("- Onset       (O) : {0}", oChiefComplaint.DisplayOnset));
                    //if (!string.IsNullOrEmpty(oChiefComplaint.CourseTiming))
                    //    sbNotes.AppendLine(string.Format("- Timing      (T) : {0}", oChiefComplaint.CourseTiming));
                    //if (!string.IsNullOrEmpty(oChiefComplaint.DisplayProvocation))
                    //    sbNotes.AppendLine(string.Format("- Provocation (T) : {0}", oChiefComplaint.DisplayProvocation));

                    //chiefComplainText.AppendLine(sbNotes.ToString());
                    //divHPI.InnerHtml = chiefComplainText.ToString();

                    divDiagnosticResultSummaryContent.InnerHtml = oChiefComplaint.DiagnosticResultSummary;
                    divPlanningSummary.InnerHtml = oChiefComplaint.PlanningSummary;
                }
                else
                {
                    SubjectiveContent oContent = new SubjectiveContent();
                    #region Subjective Notes from Medical Record
                    filterExpression = string.Format("VisitID = {0} AND GCPatientNoteType = '{1}' AND IsDeleted = 0", _visitID, Constant.PatientVisitNotes.SUBJECTIVE_NOTES);
                    List<vPatientVisitNote> lstNotes = new List<vPatientVisitNote>();

                    filterExpression += " ORDER BY NoteDate,NoteTime";
                    lstNotes = BusinessLayer.GetvPatientVisitNoteList(filterExpression);
                    if (lstNotes.Count > 0)
                    {
                        foreach (vPatientVisitNote item in lstNotes)
                        {
                            oContent = new SubjectiveContent();
                            oContent.HistoryDate = item.NoteDateInString;
                            oContent.HistoryTime = item.NoteTime;
                            oContent.ParamedicName = string.Format("{0} (Entried By : {1})", item.ParamedicName, item.CreatedByName);
                            oContent.ChiefComplaintText = item.NoteText;
                            list.Add(oContent);
                        }
                    }
                    else
                    {
                        oContent.HistoryDate = string.Empty;
                        oContent.HistoryTime = string.Empty;
                        oContent.ParamedicName = string.Empty;
                        oContent.ChiefComplaintText = string.Empty;
                        list.Add(oContent);
                    }
                    #endregion
                }

                rptChiefComplaint.DataSource = list;
                rptChiefComplaint.DataBind();
            }
            else
            {
                List<SubjectiveContent> list = new List<SubjectiveContent>();
                SubjectiveContent oContent = new SubjectiveContent();

                if (_vPastMedical != null)
                {
                    string medicalSummary = _vPastMedical.MedicalSummary;

                    oContent.HistoryDate = _vPastMedical.cfHistoryDateText;
                    oContent.HistoryTime = string.Empty;
                    oContent.ParamedicName = _vPastMedical.PhysicianName;
                    oContent.ChiefComplaintText = _vPastMedical.MedicalSummary;

                } list.Add(oContent);
                rptChiefComplaint.DataSource = list;
                rptChiefComplaint.DataBind();
            }
        }

        private void LoadObjectiveContent()
        {
            if (_isFromMigration == "0")
            {
                string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", _visitID);
                List<vReviewOfSystemHd> lstReviewOfSystemHd = BusinessLayer.GetvReviewOfSystemHdList(filterExpression, 10, 1, "ID DESC");
                if (lstReviewOfSystemHd.Count > 0)
                {
                    string lstID = String.Join(",", lstReviewOfSystemHd.Select(c => c.ID).ToArray());
                    lstReviewOfSystemDt = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID IN ({0}) AND IsDeleted = 0", lstID));
                    rptReviewOfSystemHd.DataSource = lstReviewOfSystemHd;
                    rptReviewOfSystemHd.DataBind();
                }

                List<vVitalSignHd> lstVitalSignHd = BusinessLayer.GetvVitalSignHdList(filterExpression, 3, 1, "ID DESC");
                if (lstVitalSignHd.Count > 0)
                {
                    string lstID = String.Join(",", lstVitalSignHd.Select(c => c.ID).ToArray());
                    lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("ID IN ({0}) AND IsDeleted = 0", lstID));
                    rptVitalSignHd.DataSource = lstVitalSignHd;
                    rptVitalSignHd.DataBind();
                }
            }
            else
            {
                ///TODO : Temporary unavailable
                List<vReviewOfSystemHd> lstReviewOfSystemHd = new List<vReviewOfSystemHd>();
                vReviewOfSystemHd oROSHeader = new vReviewOfSystemHd();
                lstReviewOfSystemHd.Add(oROSHeader);
                lstReviewOfSystemDt = null;

                List<vVitalSignHd> lstVitalSignHd = new List<vVitalSignHd>();
                vVitalSignHd oVitalHd = new vVitalSignHd();
                lstVitalSignHd.Add(oVitalHd);
                lstVitalSignDt = null;
            }
        }

        private void LoadAssessmentContent()
        {
            List<DiagnosisInfo> lstDiagnosis = new List<DiagnosisInfo>();
            if (_isFromMigration == "0")
            {
                string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", _visitID);
                List<vPatientDiagnosis> lstPatientDiagnose = BusinessLayer.GetvPatientDiagnosisList(filterExpression);
                foreach (vPatientDiagnosis dxItem in lstPatientDiagnose)
                {
                    DiagnosisInfo oDiagnosis = new DiagnosisInfo();
                    oDiagnosis.DiagnosisDate = dxItem.DifferentialDateInString;
                    oDiagnosis.DiagnosisTime = dxItem.DifferentialTime;
                    oDiagnosis.PhysicianName = dxItem.ParamedicName;
                    oDiagnosis.DiagnosisText = dxItem.DiagnosisText;
                    oDiagnosis.DiagnosisType = dxItem.DiagnoseType;
                    lstDiagnosis.Add(oDiagnosis);
                }
            }
            else
            {
                string[] dxHistory = _vPastMedical.DiagnosisSummary.Split(new string[] { "\r", "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string dxItem in dxHistory)
                {
                    if (dxItem.Contains('|'))
                    {
                        string[] dx = dxItem.Split('|');
                        DiagnosisInfo oDiagnosis = new DiagnosisInfo();
                        oDiagnosis.DiagnosisDate = _vPastMedical.cfHistoryDateText;
                        oDiagnosis.DiagnosisTime = "";
                        oDiagnosis.PhysicianName = _vPastMedical.PhysicianName;
                        oDiagnosis.DiagnosisType = dx[2];
                        oDiagnosis.DiagnosisText = string.Format("{0} ({1})", dx[1], dx[0]);
                        lstDiagnosis.Add(oDiagnosis);
                    }
                }
            }
            rptDifferentDiagnosis.DataSource = lstDiagnosis.OrderBy(p => p.DiagnosisType);
            rptDifferentDiagnosis.DataBind();
        }

        private void LoadPlanningContent()
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", _visitID);
            #region Laboratory
            List<LaboratoryHdInfo> lstLaboratoryHd = new List<LaboratoryHdInfo>();
            if (_isFromMigration == "0")
            {
                filterExpression = string.Format("VisitID = {0} AND HealthcareServiceUnitID = {1} ORDER BY TransactionDate DESC", _visitID, hdnHSULaboratoryID.Value);
                List<vPatientVisitLaboratory> lstCharges = BusinessLayer.GetvPatientVisitLaboratoryList(filterExpression);
                if (lstCharges.Count > 0)
                {
                    foreach (vPatientVisitLaboratory item in lstCharges)
                    {
                        LaboratoryHdInfo oHeader = new LaboratoryHdInfo();
                        oHeader.ChargesID = item.TransactionID;
                        oHeader.TestDate = item.TransactionDate.ToString(Constant.FormatString.DATE_FORMAT);
                        oHeader.TestTime = item.TransactionTime;
                        oHeader.ItemID = item.ItemID;
                        //oHeader.ItemName = string.Format("{0} ({1})",item.ItemName1,item.DiagnoseTestOrder);
                        oHeader.ItemName = string.Format("{0}", item.ItemName1);
                        lstLaboratoryHd.Add(oHeader);
                    }
                }
                divLaboratoryNA.Style.Add("display", "none");
            }
            else
            {
                if (!string.IsNullOrEmpty(_vPastMedical.TreatmentSummary))
                {
                    string[] _summary = _vPastMedical.TreatmentSummary.Split('|');
                    string[] _labInfo = _summary[0].Split(new string[] { "\r", "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    if (_labInfo.Length > 0)
                    {
                        LaboratoryHdInfo oHeader = new LaboratoryHdInfo();
                        oHeader.ChargesID = Convert.ToInt32(_visitID);
                        oHeader.TestDate = _vPastMedical.HistoryDate.ToString(Constant.FormatString.DATE_FORMAT);
                        oHeader.TestTime = string.Empty;
                        oHeader.ItemID = 0;
                        oHeader.ItemName = "Laboratory Result";
                        lstLaboratoryHd.Add(oHeader);
                    }
                    divLaboratoryNA.Style.Add("display", "none");
                }
                else
                {
                    divLaboratoryNA.Style.Add("display", "block");
                }
            }
            rptLabTestOrder.DataSource = lstLaboratoryHd;
            rptLabTestOrder.DataBind();

            #endregion

            #region Imaging
            List<ImagingHdInfo> lstImagingHd = new List<ImagingHdInfo>();
            if (_isFromMigration == "0")
            {
                filterExpression = string.Format("VisitID = {0} AND HealthcareServiceUnitID = {1} ORDER BY TransactionDate DESC", _visitID, hdnHSUImagingID.Value);
                List<vPatientVisitImaging> lstCharges = BusinessLayer.GetvPatientVisitImagingList(filterExpression);
                if (lstCharges.Count > 0)
                {
                    foreach (vPatientVisitImaging item in lstCharges)
                    {
                        ImagingHdInfo oHeader = new ImagingHdInfo();
                        oHeader.ChargesID = item.TransactionID;
                        oHeader.TestDate = item.TransactionDate.ToString(Constant.FormatString.DATE_FORMAT);
                        oHeader.TestTime = item.TransactionTime;
                        oHeader.ItemID = item.ItemID;
                        oHeader.ItemName = string.Format("{0} ({1})", item.ItemName1, item.DiagnoseTestOrder);
                        lstImagingHd.Add(oHeader);
                    }
                }
                divImagingNA.Style.Add("display", "none");
            }
            else
            {
                if (!string.IsNullOrEmpty(_vPastMedical.TreatmentSummary))
                {
                    string[] _summary = _vPastMedical.TreatmentSummary.Split('|');
                    string[] _imgInfo = _summary[1].Split(new string[] { "\r", "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    if (_imgInfo.Length > 0)
                    {
                        ImagingHdInfo oHeader = new ImagingHdInfo();
                        oHeader.ChargesID = Convert.ToInt32(_visitID);
                        oHeader.TestDate = _vPastMedical.HistoryDate.ToString(Constant.FormatString.DATE_FORMAT);
                        oHeader.TestTime = string.Empty;
                        oHeader.ItemID = 0;
                        oHeader.ItemName = "Imaging";
                        lstImagingHd.Add(oHeader);
                    }
                    divImagingNA.Style.Add("display", "none");
                }
                else
                {
                    divImagingNA.Style.Add("display", "block");
                }
            }
            rptImagingTestOrder.DataSource = lstImagingHd;
            rptImagingTestOrder.DataBind();

            #endregion

            #region Medication
            if (_isFromMigration == "0")
            {
                filterExpression = string.Format("VisitID = {0} ORDER BY PrescriptionOrderDetailID DESC", _visitID);
                rptMedication.DataSource = BusinessLayer.GetvPatientVisitPrescriptionList(filterExpression);
                rptMedication.DataBind();

                divMedicationNA.Style.Add("display", "none");
            }
            else
            {
                if (!string.IsNullOrEmpty(_vPastMedical.MedicationSummary))
                {
                    if (_vPastMedical.MedicationSummary != "-")
                    {
                        string[] _medicationInfo = _vPastMedical.MedicationSummary.Split(new string[] { "\r", "\n", "\\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                        List<vPatientVisitPrescription> list = new List<vPatientVisitPrescription>();
                        decimal dispenseQty = 0;
                        foreach (string item in _medicationInfo)
                        {
                            if (item.Contains(";"))
                            {
                                // Compound Prescription

                                string[] _compoundInfo = item.Split(';');

                                //Compound Parent Information
                                string[] temp = _compoundInfo[0].Split(new string[] { "No." }, StringSplitOptions.RemoveEmptyEntries);
                                string[] parentInfo = temp[1].Trim().Split(' ');
                                string qty = parentInfo[0];
                                if (!string.IsNullOrEmpty(qty))
                                    dispenseQty = Convert.ToDecimal(qty);

                                vPatientVisitPrescription oItem = new vPatientVisitPrescription();
                                oItem.IsRFlag = true;
                                oItem.ItemID = 0;
                                oItem.ItemCode = "00";
                                oItem.CompoundDrugname = _compoundInfo[0].Contains("R/") ? _compoundInfo[0].Replace("R/", "") : _compoundInfo[0].Replace("R/", "   ");
                                oItem.DosingUnit = string.Empty;
                                oItem.DispenseQty = dispenseQty;
                                oItem.Frequency = 0;
                                oItem.NumberOfDosageInString = string.Empty;
                                oItem.DosingUnit = string.Empty;
                                oItem.IsCompound = true;

                                StringBuilder medicationLine = new StringBuilder();

                                foreach (string compoundItem in _compoundInfo)
                                {
                                    string itemName = compoundItem.Contains("R/") ? compoundItem.Replace("R/", "") : compoundItem.Replace("R/", "   ");
                                    medicationLine.AppendLine(string.Format("{0}",itemName));
                                }

                                oItem.MedicationLine = medicationLine.ToString();
                                list.Add(oItem);
                            }
                            else
                            {
                                string[] temp = item.Split(new string[] { "No." }, StringSplitOptions.RemoveEmptyEntries);
                                string[] dispenseQtyInfo = temp[1].Trim().Split(' ');
                                string qty = dispenseQtyInfo[0];
                                if (!string.IsNullOrEmpty(qty))
                                    dispenseQty = Convert.ToDecimal(qty);

                                vPatientVisitPrescription oItem = new vPatientVisitPrescription();
                                oItem.IsRFlag = true;
                                oItem.IsCompound = false;
                                oItem.ItemID = 0;
                                oItem.ItemCode = "00";
                                oItem.DrugName = item.Replace("R/", "");
                                oItem.DosingUnit = string.Empty;
                                oItem.DispenseQty = dispenseQty;
                                oItem.Frequency = 0;
                                oItem.NumberOfDosageInString = string.Empty;
                                oItem.DosingUnit = string.Empty;
                                list.Add(oItem);
                            }
                        }
                        rptMedication.DataSource = list;
                        rptMedication.DataBind();

                        divMedicationNA.Style.Add("display", "none");
                    }
                    else
                    {
                        divMedicationNA.Style.Add("display", "block");
                    }
                }
            }
            #endregion
        }

        private void LoadPlanningNotesContent()
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", _visitID);
            #region Physician
            List<vPatientVisitNote> lstNotes = new List<vPatientVisitNote>();
            if (_isFromMigration == "0")
            {
                filterExpression += " ORDER BY NoteDate,NoteTime";
                lstNotes = BusinessLayer.GetvPatientVisitNoteList(filterExpression);

                List<vPatientVisitNote> lstPhysicianNotes = lstNotes.Where(lst => lst.GCPatientNoteType.Equals(Constant.PatientVisitNotes.PLANNING_NOTES) || lst.GCPatientNoteType.Equals(Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES)).OrderByDescending(lst => lst.ID).ToList();
                List<vPatientVisitNote> lstNursingNotes = lstNotes.Where(lst => lst.GCPatientNoteType.Equals(Constant.PatientVisitNotes.NURSING_NOTES)).OrderByDescending(lst => lst.ID).ToList();

                rptNotes.DataSource = lstPhysicianNotes;
                rptNotes.DataBind();

                rptNursingNotes.DataSource = lstNursingNotes;
                rptNursingNotes.DataBind();
            }
            else
            {
                //divLaboratoryNA.Style.Add("display", "block");
                filterExpression += " ORDER BY NoteDate,NoteTime";
                List<vPastMedicalNote> lstPastNotes = new List<vPastMedicalNote>();
                lstPastNotes = BusinessLayer.GetvPastMedicalNoteList(filterExpression);

                List<vPastMedicalNote> lstPhysicianNotes = lstPastNotes.Where(lst => lst.GCPatientNoteType.Equals(Constant.PatientVisitNotes.PLANNING_NOTES) || lst.GCPatientNoteType.Equals(Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES)).OrderByDescending(lst => lst.ID).ToList();
                List<vPastMedicalNote> lstNursingNotes = lstPastNotes.Where(lst => lst.GCPatientNoteType.Equals(Constant.PatientVisitNotes.NURSING_NOTES)).OrderByDescending(lst => lst.ID).ToList();

                rptNotes.DataSource = lstPhysicianNotes;
                rptNotes.DataBind();

                rptNursingNotes.DataSource = lstNursingNotes;
                rptNursingNotes.DataBind();
            }
            #endregion
        }

        protected void rptReviewOfSystemHd_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                vReviewOfSystemHd obj = (vReviewOfSystemHd)e.Item.DataItem;
                Repeater rptReviewOfSystemDt = (Repeater)e.Item.FindControl("rptReviewOfSystemDt");
                rptReviewOfSystemDt.DataSource = lstReviewOfSystemDt.Where(p => p.ID == obj.ID).ToList();
                rptReviewOfSystemDt.DataBind();
            }
        }

        protected void rptVitalSignHd_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                vVitalSignHd obj = (vVitalSignHd)e.Item.DataItem;
                Repeater rptVitalSignDt = (Repeater)e.Item.FindControl("rptVitalSignDt");
                rptVitalSignDt.DataSource = lstVitalSignDt.Where(p => p.ID == obj.ID).ToList();
                rptVitalSignDt.DataBind();
            }
        }

        protected void rptLabTestOrder_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LaboratoryHdInfo obj = (LaboratoryHdInfo)e.Item.DataItem;
                List<LaboratoryDtInfo> lstResultDt = new List<LaboratoryDtInfo>();
                Repeater rptLaboratoryDt = (Repeater)e.Item.FindControl("rptLaboratoryDt");
                if (obj.ItemID != 0) // Is Not From Migration
                {
                    PatientChargesHd entityHD = BusinessLayer.GetPatientChargesHd(obj.ChargesID);
                    if (entityHD != null && entityHD.GCTransactionStatus != Constant.TransactionStatus.VOID)
                    {
                        String filterExpression = String.Format("TransactionID = {0} AND IsDeleted = 0 AND ISNULL(GCTransactionDetailStatus,'') != '{1}' AND ItemID = {2}", entityHD.TransactionID, Constant.TransactionStatus.VOID, obj.ItemID);
                        PatientChargesDt entityDt = BusinessLayer.GetPatientChargesDtList(filterExpression).FirstOrDefault();
                        if (entityDt != null)
                        {
                            List<vLaboratoryResultDt> lstLabEntity = BusinessLayer.GetvLaboratoryResultDtList(string.Format("ChargeTransactionID = {0} AND ItemID = {1} ORDER BY FractionDisplaySequence", obj.ChargesID, obj.ItemID));
                            if (lstLabEntity.Count > 0)
                            {
                                foreach (vLaboratoryResultDt result in lstLabEntity)
                                {
                                    LaboratoryDtInfo oDetail = new LaboratoryDtInfo();
                                    oDetail.ChargesID = result.ChargeTransactionID;
                                    oDetail.ItemID = result.ItemID;
                                    oDetail.FractionName = result.FractionName1;
                                    if (!result.IsNumeric)
                                    {
                                        string resultText = CleanHTMLTagFromResult(result.TextValue);
                                        oDetail.ResultValue = StripHTML(resultText);
                                        oDetail.ResultUnit = result.MetricUnit;
                                    }
                                    else
                                    {
                                        oDetail.ResultValue = result.MetricResultValue.ToString("G29");
                                        oDetail.ResultUnit = result.MetricUnit;
                                    }
                                    oDetail.RefRange = result.cfReferenceRange;
                                    oDetail.ResultFlag = string.IsNullOrEmpty(result.ResultFlag) ? "N" : result.ResultFlag;
                                    oDetail.IsNormal = oDetail.ResultFlag == "N";
                                    oDetail.IsPanicRange = (oDetail.ResultFlag == "HH" || oDetail.ResultFlag == "LL");

                                    lstResultDt.Add(oDetail);
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (_vPastMedical != null)
                    {
                        string[] _summary = _vPastMedical.TreatmentSummary.Split('|');
                        string[] _labInfo = _summary[0].Split(new string[] { @"\r", @"\n", @"\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                        if (_labInfo.Length > 0)
                        {
                            foreach (string result in _labInfo)
                            {
                                LaboratoryDtInfo oDetail = new LaboratoryDtInfo();
                                oDetail.ChargesID = 0;
                                oDetail.ItemID = 0;
                                oDetail.FractionName = result;
                                oDetail.ResultValue = string.Empty;
                                oDetail.ResultUnit = string.Empty;
                                oDetail.RefRange = string.Empty;
                                oDetail.ResultFlag = string.Empty;

                                lstResultDt.Add(oDetail);
                            }
                        }
                    }
                }
                rptLaboratoryDt.DataSource = lstResultDt;
                rptLaboratoryDt.DataBind();
            }
        }

        private string CleanHTMLTagFromResult(string resultText)
        {
            string result = resultText.Replace(@"<strong>", "");
            result = resultText.Replace(@"</strong>", "");
            result = resultText.Replace("&nbsp;", "");
            result = resultText.Replace(@"<br />", Environment.NewLine);
            return result;
        }

        protected void rptImagingTestOrder_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ImagingHdInfo obj = (ImagingHdInfo)e.Item.DataItem;
                List<ImagingDtInfo> lstResultDt = new List<ImagingDtInfo>();
                List<vImagingResultDt> lstEntity = BusinessLayer.GetvImagingResultDtList(string.Format("ID IN (SELECT ID FROM ImagingResultHd WHERE ChargeTransactionID = {0}) AND ItemID = {1}", obj.ChargesID, obj.ItemID));
                Repeater rptImagingTestOrderDt = (Repeater)e.Item.FindControl("rptImagingTestOrderDt");
                if (obj.ItemID != 0)
                {
                    if (lstEntity.Count > 0)
                    {
                        foreach (vImagingResultDt result in lstEntity)
                        {
                            ImagingDtInfo oDetail = new ImagingDtInfo();
                            oDetail.ChargesID = obj.ChargesID;
                            oDetail.ItemID = result.ItemID;
                            oDetail.ResultValue = result.TestResult1;

                            lstResultDt.Add(oDetail);
                        }
                    }
                }
                else
                {
                    if (_vPastMedical != null)
                    {
                        string[] _summary = _vPastMedical.TreatmentSummary.Split('|');
                        string[] _imgInfo = _summary[1].Split(new string[] { @"\r", @"\n", @"\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                        if (_imgInfo.Length > 0)
                        {
                            StringBuilder itemList = new StringBuilder();
                            foreach (string result in _imgInfo)
                            {
                                itemList.AppendLine(result);
                            }
                            ImagingDtInfo oDetail = new ImagingDtInfo();
                            oDetail.ChargesID = 0;
                            oDetail.ItemID = 0;
                            oDetail.ResultValue = itemList.ToString();

                            lstResultDt.Add(oDetail);
                        }
                    }
                }
                rptImagingTestOrderDt.DataSource = lstResultDt;
                rptImagingTestOrderDt.DataBind();
            }
        }

        public static string StripHTML(string HTMLText, bool decode = true)
        {
            Regex reg = new Regex("<[^>]+>", RegexOptions.IgnoreCase);
            var stripped = reg.Replace(HTMLText, "");
            return decode ? HttpUtility.HtmlDecode(stripped) : stripped;
        }
    }
}
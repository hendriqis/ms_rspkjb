﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using System.Data;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class MCUAssessment1 : BasePagePatientPageList
    {
        protected int gridAllergyPageCount = 1;
        protected int gridVitalSignPageCount = 1;
        protected int gridProcedurePageCount = 1;
        protected int gridROSPageCount = 1;
        protected int gridDiagnosisPageCount = 1;
        protected int gridLaboratoryPageCount = 1;
        protected int gridImagingPageCount = 1;
        protected int gridDiagnosticPageCount = 1;
        protected int gridInstructionPageCount = 1;
        protected List<vVitalSignDt> lstVitalSignDt = null;
        protected List<vReviewOfSystemDt> lstReviewOfSystemDt = null;
        protected static string _visitNoteID;

        protected string RegistrationDateTime = "";

        class SubjectiveContent
        {
            public string HistoryDate { get; set; }
            public string HistoryTime { get; set; }
            public string ParamedicName { get; set; }
            public string ChiefComplaintText { get; set; }
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

        protected string GetUserID()
        {
            return AppSession.UserLogin.UserID.ToString();
        }

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.MCU_SOAP_GENERAL_ASSESSMENT_1;
        }

        protected override void InitializeDataControl()
        {
            SetControlProperties();
            SetSettingParameter();

            Helper.SetControlEntrySetting(cboVisitType, new ControlEntrySetting(true, true, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtHPISummary, new ControlEntrySetting(true, true, false), "mpPatientStatus");

            hdnDatePickerToday.Value = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnTimeToday.Value = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            SetEntityToControl();

            BindGridViewAllergy(1, true, ref gridAllergyPageCount);
            BindGridViewVitalSign(1, true, ref gridVitalSignPageCount);
            BindGridViewROS(1, true, ref gridROSPageCount);
            BindGridViewDiagnosis(1, true, ref gridDiagnosisPageCount);
            BindGridViewLaboratory(1, true, ref gridLaboratoryPageCount);
            BindGridViewImaging(1, true, ref gridImagingPageCount);
            BindGridViewDiagnostic(1, true, ref gridDiagnosticPageCount);
            BindGridViewInstruction(1, true, ref gridInstructionPageCount);

            LoadBodyDiagram();

            hdnIsChanged.Value = "0";
        }

        private void SetSettingParameter()
        {
            string FilterExpression = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
                                                        AppSession.UserLogin.HealthcareID,
                                                        Constant.SettingParameter.CC_COPY_FROM_NCC,
                                                        Constant.SettingParameter.EM_IS_ASSESMENT_NON_INPATIENT_DEFAULT_USING_REGISTRATION_DATE
                                                    );
            List<SettingParameterDt> lstSetPar = BusinessLayer.GetSettingParameterDtList(FilterExpression);

            hdnIsCCCopyFromNCC.Value = lstSetPar.Where(p => p.ParameterCode == Constant.SettingParameter.CC_COPY_FROM_NCC).FirstOrDefault().ParameterValue;
            hdnAssessmentDateIsUsingRegDate.Value = lstSetPar.Where(p => p.ParameterCode == Constant.SettingParameter.EM_IS_ASSESMENT_NON_INPATIENT_DEFAULT_USING_REGISTRATION_DATE).FirstOrDefault().ParameterValue;
        }

        private void SetEntityToControl()
        {
            vConsultVisit1 entityVisit = BusinessLayer.GetvConsultVisit1List(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();

            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            cboVisitType.Value = entityVisit.VisitTypeID.ToString();

            if (hdnAssessmentDateIsUsingRegDate.Value == "1")
            {
                if (entityVisit.StartServiceDate == null || entityVisit.StartServiceTime == "")
                {
                    txtServiceDate.Text = entityVisit.ActualVisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtServiceTime.Text = entityVisit.ActualVisitTime;
                }
                else
                {
                    txtServiceDate.Text = entityVisit.StartServiceDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtServiceTime.Text = entityVisit.StartServiceTime;
                }
            }
            else
            {
                txtServiceDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtServiceTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            }

            if (entityVisit.DischargeDate == null || entityVisit.DischargeTime == "")
            {
                txtDischargeDate.Text = entityVisit.ActualVisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtDischargeTime.Text = entityVisit.ActualVisitTime;
            }
            else
            {
                txtDischargeDate.Text = entityVisit.DischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtDischargeTime.Text = entityVisit.DischargeTime;
            }

            if (!string.IsNullOrEmpty(entityVisit.StartServiceTime))
            {
                RegistrationDateTime = entityVisit.StartServiceDate.ToString(Constant.FormatString.DATE_FORMAT_112);
                RegistrationDateTime += entityVisit.StartServiceTime.Replace(":", "");
            }
            else
            {
                RegistrationDateTime = AppSession.RegisteredPatient.VisitDate.ToString(Constant.FormatString.DATE_FORMAT_112);
                RegistrationDateTime += AppSession.RegisteredPatient.VisitTime.Replace(":", "");
            }

            chkIsFastTrack.Checked = entityVisit.IsFastTrack;
            hdnDepartmentID.Value = entityVisit.DepartmentID;
            hdnIsPhysicianDischarge.Value = entityVisit.DischargeDate != DateTime.MinValue ? "1" : "0";

            vChiefComplaint obj = BusinessLayer.GetvChiefComplaintList(string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            if (obj != null)
            {
                if (obj.ID == 0)
                {
                    hdnChiefComplaintID.Value = "0";
                    hdnParamedicID.Value = "0";
                }
                else
                {
                    hdnChiefComplaintID.Value = obj.ID.ToString();
                    hdnParamedicID.Value = obj.ParamedicID.ToString();
                    txtServiceDate.Text = obj.ObservationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtServiceTime.Text = obj.ObservationTime;
                    txtChiefComplaint.Text = obj.ChiefComplaintText;
                    txtHPISummary.Text = obj.HPISummary;
                    chkIsPatientAllergyExists.Checked = !chkIsPatientAllergyExists.Checked;
                    chkAlloAnamnesis.Checked = obj.IsAlloAnamnesis;
                    chkAutoAnamnesis.Checked = obj.IsAutoAnamnesis;
                    txtMedicalProblem.Text = obj.MedicalProblem;
                }
            }

            #region Nurse Anamnesis
            vNurseChiefComplaint entity = BusinessLayer.GetvNurseChiefComplaintList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();

            txtNurseAnamnesis.Text = entity.NurseChiefComplaintText;
            if (obj != null)
            {
                if (hdnIsCCCopyFromNCC.Value == "1")
                {
                    if (String.IsNullOrEmpty(obj.ChiefComplaintText))
                    {
                        txtChiefComplaint.Text = entity.NurseChiefComplaintText;
                    }
                }
            }
            #endregion

            #region Appointment
            Appointment oAppointment = BusinessLayer.GetAppointmentList(string.Format("FromVisitID  = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            if (oAppointment != null)
            {
                hdnAppointmentID.Value = oAppointment.AppointmentID.ToString();
                cboFollowupVisitType.Value = oAppointment.VisitTypeID.ToString();
                txtFollowupVisitDate.Text = oAppointment.StartDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtFollowupVisitRemarks.Text = oAppointment.Notes;
            }
            #endregion


            #region Past Medical History
            vPastMedical pastMedical = BusinessLayer.GetvPastMedicalList(string.Format("RegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID)).LastOrDefault();
            if (pastMedical != null)
            {
                txtMedicalHistory.Text = pastMedical.MedicalSummary;
                txtMedicationHistory.Text = pastMedical.MedicationSummary;
            }
            #endregion

            if (!entityVisit.IsNewPatient)
            {
                LoadPreviousVisitInformation(AppSession.RegisteredPatient.MRN, true);
            }
            else
            {
                lblVisitHistoryInfo.Text = "Not Available";
            }

            List<PatientVisitNote> lstPatientVisitNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = '{0}' AND GCPatientNoteType = '{1}'", AppSession.RegisteredPatient.VisitID, Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES));

            if (lstPatientVisitNote.Count > 0)
            {
                PatientVisitNote entitypvn = lstPatientVisitNote.First();
                txtPlanningNotes.Text = entitypvn.PlanningText;
            }
        }

        private void LoadPreviousVisitInformation(int mrn, bool isShowSameServiceUnit)
        {
            string filterExpression = string.Format("MRN = {0} AND DepartmentID = '{1}' AND ParamedicID = {2} AND VisitID != {3} ORDER BY VisitID DESC", mrn, AppSession.RegisteredPatient.DepartmentID, AppSession.UserLogin.ParamedicID, AppSession.RegisteredPatient.VisitID);
            vConsultVisitCustom oLastVisit = BusinessLayer.GetvConsultVisitCustomList(filterExpression).FirstOrDefault();
            if (oLastVisit != null)
            {
                hdnPreviousVisitID.Value = oLastVisit.VisitID.ToString();
                lblVisitHistoryInfo.Text = string.Format("{0}, {1}", oLastVisit.ActualVisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT), oLastVisit.ParamedicName);
                lblPreviousDiagnosis.Text = oLastVisit.PatientDiagnosis;
                LoadSubjectiveContent(oLastVisit.VisitID);
                LoadObjectiveContent(oLastVisit.VisitID);
                LoadAssessmentContent(oLastVisit.VisitID);
                LoadPlanningContent(oLastVisit.VisitID);
            }
        }

        private void LoadSubjectiveContent(int visitID)
        {
            List<SubjectiveContent> list = new List<SubjectiveContent>();

            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", visitID);
            vChiefComplaint oChiefComplaint = BusinessLayer.GetvChiefComplaintList(filterExpression, 5, 1, "ID DESC").FirstOrDefault();

            if (oChiefComplaint != null)
            {
                SubjectiveContent oContent = new SubjectiveContent();
                oContent.HistoryDate = oChiefComplaint.ObservationDateInString;
                oContent.HistoryTime = oChiefComplaint.ObservationTime;
                oContent.ParamedicName = oChiefComplaint.ParamedicName;
                oContent.ChiefComplaintText = oChiefComplaint.ChiefComplaintText;
                list.Add(oContent);

                divHPI.InnerHtml = oChiefComplaint.HPISummary;
            }
            else
            {
                SubjectiveContent oContent = new SubjectiveContent();
                #region Subjective Notes from Medical Record
                filterExpression = string.Format("VisitID = {0} AND GCPatientNoteType = '{1}' AND IsDeleted = 0", visitID, Constant.PatientVisitNotes.SUBJECTIVE_NOTES);
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

        private void LoadObjectiveContent(int visitID)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", visitID);
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

        private void LoadAssessmentContent(int visitID)
        {
            List<DiagnosisInfo> lstDiagnosis = new List<DiagnosisInfo>();
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", visitID);
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

            rptDifferentDiagnosis.DataSource = lstDiagnosis.OrderBy(p => p.DiagnosisType);
            rptDifferentDiagnosis.DataBind();
        }

        private void LoadPlanningContent(int visitID)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", visitID);
            #region Laboratory
            List<LaboratoryHdInfo> lstLaboratoryHd = new List<LaboratoryHdInfo>();

            filterExpression = string.Format("VisitID = {0} AND HealthcareServiceUnitID = {1} ORDER BY TransactionDate DESC", visitID, hdnLaboratoryServiceUnitID.Value);
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
                    oHeader.ItemName = item.ItemName1;
                    lstLaboratoryHd.Add(oHeader);
                }
            }

            divLaboratoryNA.Style.Add("display", "none");

            rptLabTestOrder.DataSource = lstLaboratoryHd;
            rptLabTestOrder.DataBind();

            #endregion

            #region Imaging
            List<ImagingHdInfo> lstImagingHd = new List<ImagingHdInfo>();
            filterExpression = string.Format("VisitID = {0} AND HealthcareServiceUnitID = {1} ORDER BY TransactionDate DESC", visitID, hdnImagingServiceUnitID.Value);
            List<vPatientVisitImaging> lstCharges2 = BusinessLayer.GetvPatientVisitImagingList(filterExpression);
            if (lstCharges.Count > 0)
            {
                foreach (vPatientVisitImaging item in lstCharges2)
                {
                    ImagingHdInfo oHeader = new ImagingHdInfo();
                    oHeader.ChargesID = item.TransactionID;
                    oHeader.TestDate = item.TransactionDate.ToString(Constant.FormatString.DATE_FORMAT);
                    oHeader.TestTime = item.TransactionTime;
                    oHeader.ItemID = item.ItemID;
                    oHeader.ItemName = item.ItemName1;
                    lstImagingHd.Add(oHeader);
                }
            }
            divImagingNA.Style.Add("display", "none");


            rptImagingTestOrder.DataSource = lstImagingHd;
            rptImagingTestOrder.DataBind();

            #endregion

            #region Medication
            filterExpression = string.Format("VisitID = {0} ORDER BY PrescriptionOrderDetailID DESC", visitID);
            rptMedication.DataSource = BusinessLayer.GetvPatientVisitPrescriptionList(filterExpression);
            rptMedication.DataBind();

            divMedicationNA.Style.Add("display", "none");
            #endregion
        }


        private void LoadBodyDiagram()
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);
            int pageCount = BusinessLayer.GetvPatientBodyDiagramHdRowCount(filterExpression);
            hdnPageCount.Value = pageCount.ToString();

            if (pageCount > 0)
            {
                hdnPageIndex.Value = "0";
                OnLoadBodyDiagram(0);
                tblBodyDiagramNavigation.Style.Remove("display");
            }
            else
            {
                divBodyDiagram.Style.Add("display", "none");
                tblEmpty.Style.Remove("display");
            }
        }

        protected override void SetControlProperties()
        {
            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.ALLERGEN_TYPE, Constant.StandardCode.DIAGNOSIS_TYPE, Constant.StandardCode.DIFFERENTIAL_DIAGNOSIS_STATUS, Constant.StandardCode.PATIENT_OUTCOME, Constant.StandardCode.DISCHARGE_ROUTINE, Constant.StandardCode.PATIENT_INSTRUCTION_GROUP);
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(filterExpression);

            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField(cboAllergenType, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.ALLERGEN_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboDiagnosisType, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.DIAGNOSIS_TYPE && sc.StandardCodeID != Constant.DiagnoseType.EARLY_DIAGNOSIS).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboDiagnosisStatus, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.DIFFERENTIAL_DIAGNOSIS_STATUS).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboPatientOutcome, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.PATIENT_OUTCOME).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboInstructionType, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.PATIENT_INSTRUCTION_GROUP).ToList(), "StandardCodeName", "StandardCodeID");

            string defaultInstruction = lstSc.Where(sc => sc.ParentID == Constant.StandardCode.PATIENT_INSTRUCTION_GROUP && sc.IsDefault == true).FirstOrDefault().StandardCodeID;
            if (!string.IsNullOrEmpty(defaultInstruction))
            {
                cboInstructionType.Value = defaultInstruction;
            }

            cboPatientOutcome.Value = Constant.PatientOutcome.BELUM_SEMBUH;
            cboDiagnosisStatus.Value = Constant.DifferentialDiagnosisStatus.UNDER_INVESTIGATION;

            List<GetParamedicVisitTypeList> visitTypeList = BusinessLayer.GetParamedicVisitTypeList(AppSession.RegisteredPatient.HealthcareServiceUnitID, (int)AppSession.UserLogin.ParamedicID, "");
            Methods.SetComboBoxField(cboVisitType, visitTypeList, "VisitTypeName", "VisitTypeID");
            Methods.SetComboBoxField(cboFollowupVisitType, visitTypeList, "VisitTypeName", "VisitTypeID");

            List<SettingParameterDt> lstSettingParameter = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM, Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI));
            hdnImagingServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).ParameterValue;
            hdnLaboratoryServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).ParameterValue;
        }

        private void ControlToEntity(PatientVisitNote entitypvn)
        {
            string soapNote = GenerateSOAPText();

            entitypvn.NoteDate = Helper.GetDatePickerValue(txtServiceDate);
            entitypvn.NoteTime = txtServiceTime.Text;

            entitypvn.SubjectiveText = hdnSubjectiveText.Value;
            entitypvn.ObjectiveText = hdnObjectiveText.Value;
            entitypvn.AssessmentText = hdnAssessmentText.Value;
            entitypvn.PlanningText = txtPlanningNotes.Text;
            entitypvn.InstructionText = hdnInstructionText.Value;
            entitypvn.NoteText = soapNote;
        }

        private string GenerateSOAPText()
        {
            StringBuilder sbNotes = new StringBuilder();
            StringBuilder sbSubjective = new StringBuilder();
            StringBuilder sbDiagnose = new StringBuilder();
            StringBuilder sbInstruction = new StringBuilder();
            sbNotes.AppendLine("SUBJEKTIVE :");
            sbNotes.AppendLine("-".PadRight(15, '-'));
            if ((AppSession.RegisteredPatient.ParamedicID == AppSession.UserLogin.ParamedicID))
            {
                vChiefComplaint oChiefComplaint = BusinessLayer.GetvChiefComplaintList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
                if (oChiefComplaint != null)
                {
                    sbNotes.AppendLine(string.Format("Chief Complaint  : "));
                    sbNotes.AppendLine(string.Format(" {0}   ", oChiefComplaint.ChiefComplaintText));
                    sbNotes.AppendLine(string.Format("HPI  : "));
                    sbNotes.AppendLine(string.Format(" {0}   ", oChiefComplaint.HPISummary));
                }
            }
            hdnSubjectiveText.Value = sbNotes.ToString();

            sbNotes.AppendLine(" ");
            sbNotes.AppendLine("OBJEKTIF :");
            sbNotes.AppendLine("-".PadRight(15, '-'));
            List<vVitalSignDt> lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID = {0} AND ObservationDate = '{1}' AND IsDeleted = 0 ORDER BY DisplayOrder", AppSession.RegisteredPatient.VisitID, Helper.GetDatePickerValue(txtServiceDate).ToString(Constant.FormatString.DATE_FORMAT_112)));
            if (lstVitalSignDt.Count > 0)
            {
                sbNotes.AppendLine("Vital Signs :");
                foreach (vVitalSignDt vitalSign in lstVitalSignDt)
                {
                    sbNotes.AppendLine(string.Format(" {0} {1} {2}", vitalSign.VitalSignLabel, vitalSign.VitalSignValue, vitalSign.ValueUnit));
                }
            }
            List<vReviewOfSystemDt> lstROS = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID IN (SELECT ID FROM ReviewOfSystemHd WHERE VisitID = {0} AND  ObservationDate = '{1}') AND IsDeleted = 0 ORDER BY GCRoSystem", AppSession.RegisteredPatient.VisitID, Helper.GetDatePickerValue(txtServiceDate).ToString(Constant.FormatString.DATE_FORMAT_112)));
            if (lstROS.Count > 0)
            {
                sbNotes.AppendLine(" ");
                sbNotes.AppendLine("Review of System :");
                foreach (vReviewOfSystemDt item in lstROS)
                {
                    sbNotes.AppendLine(string.Format(" {0}: {1}", item.ROSystem, item.cfRemarks));
                }
            }

            hdnObjectiveText.Value = sbNotes.ToString();

            sbNotes.AppendLine(" ");
            sbNotes.AppendLine("ASSESSMENT :");
            sbNotes.AppendLine("-".PadRight(20, '-'));

            vPatientDiagnosis entityDiagnose = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0 AND IsNutritionDiagnosis = 0", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            if (entityDiagnose != null)
            {
                string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0 AND IsNutritionDiagnosis = 0 ORDER BY GCDiagnoseType", AppSession.RegisteredPatient.VisitID);
                List<vPatientDiagnosis> lstDiagnosis = BusinessLayer.GetvPatientDiagnosisList(filterExpression);
                if (lstDiagnosis.Count > 0)
                {
                    sbDiagnose = new StringBuilder();
                    foreach (vPatientDiagnosis item in lstDiagnosis)
                    {
                        if (string.IsNullOrEmpty(item.DiagnoseID))
                            sbDiagnose.AppendLine(string.Format(" {0}", item.cfDiagnosisText));
                        else
                            sbDiagnose.AppendLine(string.Format(" {0} ({1})", item.DiagnoseName, item.DiagnoseID));
                    }
                    hdnAssessmentText.Value = sbDiagnose.ToString();
                }
                else
                {
                    hdnAssessmentText.Value = null;
                }
            }
            else
            {
                hdnAssessmentText.Value = null;
            }
            sbNotes.AppendLine(string.Format(" {0}", hdnAssessmentText.Value));

            sbNotes.AppendLine(" ");
            sbNotes.AppendLine("PLANNING :");
            sbNotes.AppendLine("-".PadRight(20, '-'));
            if (!string.IsNullOrEmpty(hdnLaboratorySummary.Value))
            {
                sbNotes.AppendLine("Laboratorium :");
                sbNotes.AppendLine(hdnLaboratorySummary.Value);
            }
            if (!string.IsNullOrEmpty(hdnImagingSummary.Value))
            {
                sbNotes.AppendLine("Radiologi :");
                sbNotes.AppendLine(hdnImagingSummary.Value);
            }
            if (!string.IsNullOrEmpty(hdnDiagnosticSummary.Value))
            {
                sbNotes.AppendLine("Penunjang Medis Lainnya :");
                sbNotes.AppendLine(hdnDiagnosticSummary.Value);
            }

            sbNotes.AppendLine(" ");
            sbNotes.AppendLine("INSTRUKSI : ");
            sbNotes.AppendLine("-".PadRight(20, '-'));
            string filterExpInstruction = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);
            List<vPatientInstruction> lstInstruction = BusinessLayer.GetvPatientInstructionList(filterExpInstruction);
            if (lstInstruction.Count > 0)
            {
                sbInstruction = new StringBuilder();
                foreach (vPatientInstruction item in lstInstruction)
                {
                    sbInstruction.AppendLine(string.Format("- {0}", item.Description));
                }
                hdnInstructionText.Value = sbInstruction.ToString();
            }
            else
            {
                hdnInstructionText.Value = null;
            }
            sbNotes.AppendLine(hdnInstructionText.Value);

            return sbNotes.ToString();
        }

        private string GenerateSOAPSummaryText(ref string subjectiveText, ref string objectiveText, ref string assessmentText, ref string planningText)
        {
            StringBuilder sbNotes = new StringBuilder();
            sbNotes.AppendLine("Subjective :");
            sbNotes.AppendLine("-".PadRight(15, '-'));
            if ((AppSession.RegisteredPatient.ParamedicID == AppSession.UserLogin.ParamedicID))
            {
                vChiefComplaint oChiefComplaint = BusinessLayer.GetvChiefComplaintList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
                if (oChiefComplaint != null)
                {

                    sbNotes.AppendLine(string.Format("{0}   ", oChiefComplaint.ChiefComplaintText));
                    if (!string.IsNullOrEmpty(oChiefComplaint.HPISummary))
                    {
                        sbNotes.AppendLine(string.Format("RPS  : "));
                        sbNotes.AppendLine(string.Format("{0}", txtHPISummary.Text));
                    }
                    if (!string.IsNullOrEmpty(txtMedicationHistory.Text))
                    {
                        sbNotes.AppendLine(string.Format("RPD  : "));
                        sbNotes.AppendLine(string.Format("{0}", txtMedicationHistory.Text));
                    }
                }

                subjectiveText = sbNotes.ToString();
            }

            string vitalSummary = string.Empty;
            sbNotes = new StringBuilder();
            List<vVitalSignDt> lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID = {0} AND ObservationDate = '{1}' AND IsDeleted = 0 ORDER BY DisplayOrder", AppSession.RegisteredPatient.VisitID, Helper.GetDatePickerValue(txtServiceDate).ToString(Constant.FormatString.DATE_FORMAT_112)));
            if (lstVitalSignDt.Count > 0)
            {
                foreach (vVitalSignDt vitalSign in lstVitalSignDt)
                {
                    sbNotes.AppendLine(string.Format(" {0} {1} {2}", vitalSign.VitalSignLabel, vitalSign.VitalSignValue, vitalSign.ValueUnit));
                }
            }

            vitalSummary = sbNotes.ToString();

            string rosSummary = string.Empty;
            sbNotes = new StringBuilder();
            List<vReviewOfSystemDt> lstROS = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID IN (SELECT ID FROM ReviewOfSystemHd WHERE VisitID = {0} AND  ObservationDate = '{1}') AND IsDeleted = 0 ORDER BY GCRoSystem", AppSession.RegisteredPatient.VisitID, Helper.GetDatePickerValue(txtServiceDate).ToString(Constant.FormatString.DATE_FORMAT_112)));
            if (lstROS.Count > 0)
            {
                foreach (vReviewOfSystemDt item in lstROS)
                {
                    sbNotes.AppendLine(string.Format(" {0}: {1}", item.ROSystem, item.cfRemarks));
                }
            }
            rosSummary = sbNotes.ToString();

            //sbNotes.AppendLine(" ");
            //sbNotes.AppendLine("Assessment");
            //sbNotes.AppendLine("-".PadRight(15, '-'));
            //if (!string.IsNullOrEmpty(txtDiagnoseText.Text))
            //{
            //    sbNotes.AppendLine(string.Format("{0} ({1})", txtDiagnoseText.Text, txtDiagnoseCode.Text));
            //}

            return sbNotes.ToString();
        }

        private void UpdateConsultVisitRegistration(IDbContext ctx)
        {
            ChiefComplaintDao chiefComplaintDao = new ChiefComplaintDao(ctx);
            ConsultVisitDao consultVisitDao = new ConsultVisitDao(ctx);
            RegistrationDao registrationDao = new RegistrationDao(ctx);
            PastMedicalDao pastMedicalDao = new PastMedicalDao(ctx);

            ConsultVisit entityConsultVisit = consultVisitDao.Get(AppSession.RegisteredPatient.VisitID);
            if (entityConsultVisit.GCVisitStatus == Constant.VisitStatus.CHECKED_IN)
            {
                entityConsultVisit.StartServiceDate = Helper.GetDatePickerValue(txtServiceDate);
                entityConsultVisit.StartServiceTime = txtServiceTime.Text;
            }

            if (!string.IsNullOrEmpty(cboVisitType.Value.ToString()))
            {
                entityConsultVisit.VisitTypeID = Convert.ToInt16(cboVisitType.Value);
            }

            if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.EMERGENCY)
            {
                entityConsultVisit.TimeElapsed0 = string.Format("{0}:{1}", hdnTimeElapsed0hour.Value.PadLeft(2, '0'), hdnTimeElapsed0minute.Value.PadLeft(2, '0'));
            }
            entityConsultVisit.TimeElapsed1 = string.Format("{0}:{1}", hdnTimeElapsed1hour.Value.PadLeft(2, '0'), hdnTimeElapsed1minute.Value.PadLeft(2, '0'));
            entityConsultVisit.LastUpdatedBy = AppSession.UserLogin.UserID;

            consultVisitDao.Update(entityConsultVisit);

            Registration entityRegistration = registrationDao.Get(entityConsultVisit.RegistrationID);
            entityRegistration.IsFastTrack = chkIsFastTrack.Checked;
            registrationDao.Update(entityRegistration);

            ChiefComplaint entity = null;
            bool isNewChiefComplaint = true;

            if (hdnChiefComplaintID.Value != "" && hdnChiefComplaintID.Value != "0")
            {
                entity = chiefComplaintDao.Get(Convert.ToInt32(hdnChiefComplaintID.Value));
                isNewChiefComplaint = false;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
            }
            else
            {
                entity = new ChiefComplaint();
                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                entity.CreatedBy = AppSession.UserLogin.UserID;
            }
            entity.ObservationDate = Helper.GetDatePickerValue(txtServiceDate);
            entity.ObservationTime = txtServiceTime.Text;
            entity.ChiefComplaintText = txtChiefComplaint.Text;
            entity.HPISummary = txtHPISummary.Text;
            entity.IsPatientAllergyExists = !chkIsPatientAllergyExists.Checked;

            entity.IsAutoAnamnesis = Convert.ToBoolean(chkAutoAnamnesis.Checked);
            entity.IsAlloAnamnesis = Convert.ToBoolean(chkAlloAnamnesis.Checked);
            entity.MedicalProblem = txtMedicalProblem.Text;

            if (isNewChiefComplaint)
            {
                chiefComplaintDao.Insert(entity);
                hdnChiefComplaintID.Value = BusinessLayer.GetChiefComplaintMaxID(ctx).ToString();
            }
            else
            {
                chiefComplaintDao.Update(entity);
            }

            #region Past Medical
            if ((!string.IsNullOrEmpty(txtMedicalHistory.Text) || !string.IsNullOrEmpty(txtMedicationHistory.Text)))
            {
                PastMedical oPastMedical = null;
                bool isNewPastMedical = false;

                if ((hdnPastMedicalID.Value != "" && hdnPastMedicalID.Value != "0"))
                {
                    oPastMedical = pastMedicalDao.Get(Convert.ToInt32(hdnPastMedicalID.Value));
                    oPastMedical.MedicalSummary = txtMedicalHistory.Text;
                    oPastMedical.MedicationSummary = txtMedicationHistory.Text;
                    oPastMedical.LastUpdatedBy = AppSession.UserLogin.UserID;
                    isNewPastMedical = false;
                }
                else
                {
                    isNewPastMedical = true;
                    oPastMedical = new PastMedical();
                    oPastMedical.RegistrationID = entityConsultVisit.RegistrationID;
                    oPastMedical.MRN = AppSession.RegisteredPatient.MRN;
                    oPastMedical.HistoryDate = entityConsultVisit.ActualVisitDate;
                    oPastMedical.DischargeDate = entityConsultVisit.ActualVisitDate;
                    oPastMedical.MedicalSummary = txtMedicalHistory.Text;
                    oPastMedical.MedicationSummary = txtMedicationHistory.Text;
                    oPastMedical.CreatedBy = AppSession.UserLogin.UserID;
                }

                if (isNewPastMedical)
                    pastMedicalDao.Insert(oPastMedical);
                else
                    pastMedicalDao.Update(oPastMedical);
            }
            #endregion

            #region Followup Visit
            Appointment oAppointment;
            AppointmentDao oAppointmentDao;
            if (!string.IsNullOrEmpty(hdnAppointmentID.Value))
            {
                oAppointmentDao = new AppointmentDao(ctx);
                oAppointment = oAppointmentDao.Get(Convert.ToInt32(hdnAppointmentID.Value));
                if (oAppointment != null)
                {
                    oAppointment.StartDate = Helper.GetDatePickerValue(txtFollowupVisitDate);
                    oAppointment.StartTime = "00:00";
                    oAppointment.VisitTypeID = Convert.ToInt32(cboFollowupVisitType.Value);
                    oAppointment.Notes = txtFollowupVisitRemarks.Text;
                    oAppointment.LastUpdatedBy = AppSession.UserLogin.UserID;
                    oAppointment.LastUpdatedDate = DateTime.Now;
                    oAppointmentDao.Update(oAppointment);
                }
            }
            else
            {
                if (cboFollowupVisitType.Value != null)
                {
                    oAppointmentDao = new AppointmentDao(ctx);
                    oAppointment = new Appointment();
                    oAppointment.FromVisitID = AppSession.RegisteredPatient.VisitID;
                    oAppointment.HealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                    oAppointment.ParamedicID = AppSession.RegisteredPatient.ParamedicID;
                    oAppointment.MRN = AppSession.RegisteredPatient.MRN;
                    oAppointment.TransactionCode = Constant.TransactionCode.OP_APPOINTMENT;
                    oAppointment.StartDate = Helper.GetDatePickerValue(txtFollowupVisitDate);
                    oAppointment.StartTime = "00:00";
                    oAppointment.GCAppointmentStatus = Constant.AppointmentStatus.STARTED;
                    oAppointment.AppointmentNo = BusinessLayer.GenerateTransactionNo(oAppointment.TransactionCode, oAppointment.StartDate, ctx);
                    oAppointment.VisitTypeID = Convert.ToInt32(cboFollowupVisitType.Value);
                    oAppointment.Notes = txtFollowupVisitRemarks.Text;
                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    entity.CreatedDate = DateTime.Now;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    oAppointmentDao.Insert(oAppointment);
                }
            }
            #endregion
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);

            try
            {
                if (type == "save")
                {
                    ChiefComplaint obj = BusinessLayer.GetChiefComplaintList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID), ctx).FirstOrDefault();
                    if (obj != null)
                        hdnChiefComplaintID.Value = obj.ID.ToString();
                    else
                        hdnChiefComplaintID.Value = "0";

                    StringBuilder sbNotes = new StringBuilder();
                    sbNotes.AppendLine(string.Format("{0}   ", txtChiefComplaint.Text));
                    if (obj != null)
                    {
                        if (!string.IsNullOrEmpty(txtHPISummary.Text))
                        {
                            sbNotes.AppendLine(string.Format("RPS  : "));
                            sbNotes.AppendLine(string.Format("{0}", txtHPISummary.Text));
                        }
                        if (!string.IsNullOrEmpty(txtMedicationHistory.Text))
                        {
                            sbNotes.AppendLine(string.Format("RPD  : "));
                            sbNotes.AppendLine(string.Format("{0}", txtMedicationHistory.Text));
                        }
                    }

                    string subjectiveText = sbNotes.ToString();

                    #region Objective Content
                    sbNotes = new StringBuilder();
                    List<vVitalSignDt> lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID = {0} AND ObservationDate = '{1}' ORDER BY DisplayOrder", AppSession.RegisteredPatient.VisitID, Helper.GetDatePickerValue(txtServiceDate.Text).ToString(Constant.FormatString.DATE_FORMAT_112)));
                    if (lstVitalSignDt.Count > 0)
                    {
                        foreach (vVitalSignDt vitalSign in lstVitalSignDt)
                        {
                            sbNotes.AppendLine(string.Format(" {0} {1} {2}", vitalSign.VitalSignLabel, vitalSign.VitalSignValue, vitalSign.ValueUnit));
                        }
                    }
                    sbNotes.AppendLine(" ");

                    List<vReviewOfSystemDt> lstROS = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID IN (SELECT ID FROM ReviewOfSystemHd WHERE VisitID = {0} AND  ObservationDate = '{1}') AND IsDeleted = 0 ORDER BY GCRoSystem", AppSession.RegisteredPatient.VisitID, Helper.GetDatePickerValue(txtServiceDate).ToString(Constant.FormatString.DATE_FORMAT_112)));
                    if (lstROS.Count > 0)
                    {
                        foreach (vReviewOfSystemDt item in lstROS)
                        {
                            sbNotes.AppendLine(string.Format(" {0}: {1}", item.ROSystem, item.cfRemarks));
                        }
                    }

                    string objectiveText = sbNotes.ToString();
                    #endregion

                    #region Assessment Content
                    sbNotes = new StringBuilder();
                    string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0 ORDER BY GCDiagnoseType", AppSession.RegisteredPatient.VisitID);
                    List<vPatientDiagnosis> lstDiagnosis = BusinessLayer.GetvPatientDiagnosisList(filterExpression);
                    if (lstDiagnosis.Count > 0)
                    {
                        foreach (vPatientDiagnosis item in lstDiagnosis)
                        {
                            if (string.IsNullOrEmpty(item.DiagnoseID))
                                sbNotes.AppendLine(string.Format("- {0}", item.cfDiagnosisText));
                            else
                                sbNotes.AppendLine(string.Format("- {0} ({1})", item.cfDiagnosisText, item.DiagnoseID));
                        }
                    }
                    string assessmentText = sbNotes.ToString();
                    #endregion

                    PatientVisitNoteDao patientVisitNoteDao = new PatientVisitNoteDao(ctx);
                    PatientVisitNote entityVisitNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = {0} AND GCPatientNoteType = '{1}'", AppSession.RegisteredPatient.VisitID, Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES), ctx).FirstOrDefault();
                    bool isEntityVisitNoteNull = false;
                    if (entityVisitNote == null)
                    {
                        isEntityVisitNoteNull = true;
                        entityVisitNote = new PatientVisitNote();
                    }
                    ControlToEntity(entityVisitNote);

                    if (isEntityVisitNoteNull)
                    {
                        entityVisitNote.VisitID = AppSession.RegisteredPatient.VisitID;
                        entityVisitNote.HealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                        entityVisitNote.ParamedicID = AppSession.UserLogin.ParamedicID;
                        entityVisitNote.GCPatientNoteType = Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES;
                        entityVisitNote.CreatedBy = AppSession.UserLogin.UserID;
                        hdnPatientVisitNoteID.Value = patientVisitNoteDao.InsertReturnPrimaryKeyID(entityVisitNote).ToString();
                    }
                    else
                    {
                        entityVisitNote.ParamedicID = AppSession.UserLogin.ParamedicID;
                        entityVisitNote.LastUpdatedBy = AppSession.UserLogin.UserID;
                        patientVisitNoteDao.Update(entityVisitNote);

                        hdnPatientVisitNoteID.Value = entityVisitNote.ID.ToString();
                    }

                    _visitNoteID = hdnPatientVisitNoteID.Value;

                    UpdateConsultVisitRegistration(ctx);
                }
                else if (type == "discharge")
                {
                    string message = "";
                    result = ProcessDischargePatient(ctx, ref message);
                    if (!result)
                    {
                        errMessage = message;
                    }
                }

                ctx.CommitTransaction();

                hdnIsSaved.Value = "1";
                hdnIsChanged.Value = "0";
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                hdnIsSaved.Value = "0";
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }

            return result;
        }

        private bool ProcessDischargePatient(IDbContext ctx, ref string errMessage)
        {
            if (IsValidToDischarge())
            {

                if (hdnIsChanged.Value == "1")
                {
                    ChiefComplaint obj = BusinessLayer.GetChiefComplaintList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID), ctx).FirstOrDefault();
                    if (obj != null)
                        hdnChiefComplaintID.Value = obj.ID.ToString();
                    else
                        hdnChiefComplaintID.Value = "0";

                    UpdateConsultVisitRegistration(ctx);
                }

                ConsultVisitDao visitDao = new ConsultVisitDao(ctx);
                List<ConsultVisit> lstConsultVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = '{0}'", AppSession.RegisteredPatient.RegistrationID), ctx);
                ConsultVisit entity = lstConsultVisit.Where(t => t.VisitID == AppSession.RegisteredPatient.VisitID).FirstOrDefault();
                if (entity.GCVisitStatus != Constant.VisitStatus.DISCHARGED && entity.GCVisitStatus != Constant.VisitStatus.CLOSED)
                {
                    entity.GCVisitStatus = Constant.VisitStatus.PHYSICIAN_DISCHARGE;
                }
                entity.GCDischargeCondition = cboPatientOutcome.Value.ToString(); ;
                entity.GCDischargeMethod = Constant.DischargeMethod.ATAS_PERSETUJUAN;
                entity.PhysicianDischargedBy = AppSession.UserLogin.UserID;
                entity.PhysicianDischargedDate = Helper.GetDatePickerValue(txtDischargeDate);
                entity.DischargeDate = Helper.GetDatePickerValue(txtDischargeDate);
                entity.DischargeTime = txtDischargeTime.Text;
                entity.LOSInDay = Convert.ToDecimal(hdnLOSInDay.Value);
                entity.LOSInHour = Convert.ToDecimal(hdnLOSInHour.Value);
                entity.LOSInMinute = Convert.ToDecimal(hdnLOSInMinute.Value);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                if (lstConsultVisit.Where(t => t.GCVisitStatus != Constant.VisitStatus.PHYSICIAN_DISCHARGE && t.GCVisitStatus != Constant.VisitStatus.DISCHARGED && t.GCVisitStatus != Constant.VisitStatus.CLOSED && t.GCVisitStatus != Constant.VisitStatus.CANCELLED).Count() == 0)
                {
                    RegistrationDao registrationDao = new RegistrationDao(ctx);
                    Registration entityRegis = registrationDao.Get(entity.RegistrationID);
                    entityRegis.GCRegistrationStatus = Constant.VisitStatus.PHYSICIAN_DISCHARGE;
                    entityRegis.LastUpdatedBy = AppSession.UserLogin.UserID;
                    registrationDao.Update(entityRegis);
                }

                visitDao.Update(entity);

                #region Display Transaction Popup Entry

                #endregion

                return true;
            }
            else
            {
                errMessage = "You must be Registered Physician and must entry Patient Chief Complaint, Diagnosis before complete session";
                return false;
            }
        }

        private bool IsValidToDischarge()
        {
            bool isDPJPPhysician = AppSession.RegisteredPatient.ParamedicID == AppSession.UserLogin.ParamedicID;

            vChiefComplaint oChiefComplaint = BusinessLayer.GetvChiefComplaintList(string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            bool isChiefComplaintExist = oChiefComplaint != null;

            vPatientDiagnosis oDiagnosis = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            bool isDiagnosisExist = oDiagnosis != null;

            bool isPatientOutcomeExist = cboPatientOutcome.Value != null;

            return isChiefComplaintExist && isDiagnosisExist && isDPJPPhysician;
        }

        private void BindGridViewAllergy(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("MRN = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.MRN);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientAllergyRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientAllergy> lstEntity = BusinessLayer.GetvPatientAllergyList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
            grdAllergyView.DataSource = lstEntity;
            grdAllergyView.DataBind();
        }

        protected void cbpAllergyView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewAllergy(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewAllergy(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        #region Vital Sign
        private void BindGridViewVitalSign(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Empty;
            filterExpression += string.Format("VisitID = {0} AND IsDischargeVitalSign = 0 AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvVitalSignHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_COMPACT);
            }

            List<vVitalSignHd> lstEntity = BusinessLayer.GetvVitalSignHdList(filterExpression, Constant.GridViewPageSize.GRID_COMPACT, pageIndex, "ID DESC");
            lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID = {0} ORDER BY DisplayOrder", AppSession.RegisteredPatient.VisitID));
            grdVitalSignView.DataSource = lstEntity;
            grdVitalSignView.DataBind();
        }

        protected void grdVitalSignView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vVitalSignHd obj = (vVitalSignHd)e.Row.DataItem;
                Repeater rptVitalSignDt = (Repeater)e.Row.FindControl("rptVitalSignDt");
                rptVitalSignDt.DataSource = GetVitalSignDt(obj.ID);
                rptVitalSignDt.DataBind();
            }
        }

        protected List<vVitalSignDt> GetVitalSignDt(Int32 ID)
        {
            return lstVitalSignDt.Where(p => p.ID == ID).ToList();
        }

        protected void cbpVitalSignView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewVitalSign(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewVitalSign(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }
        }

        protected void cbpDeleteVitalSign_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|";

            if (hdnVitalSignRecordID.Value != "")
            {
                VitalSignHd entity = BusinessLayer.GetVitalSignHd(Convert.ToInt32(hdnVitalSignRecordID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateVitalSignHd(entity);
            }
            else
            {
                result = "0|There is no record to be deleted !";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        protected void cbpAllergy_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|add|";

            try
            {
                if (e.Parameter != null && e.Parameter != "")
                {
                    string[] param = e.Parameter.Split('|');
                    if (param[0] == "add")
                    {
                        PatientAllergy oAllergy = new PatientAllergy();

                        oAllergy.MRN = AppSession.RegisteredPatient.MRN;
                        oAllergy.AllergyLogDate = DateTime.Now.Date;
                        oAllergy.GCAllergenType = cboAllergenType.Value.ToString();
                        oAllergy.Allergen = txtAllergenName.Text;
                        oAllergy.GCAllergySource = Constant.AllergenFindingSource.PATIENT;
                        oAllergy.GCAllergySeverity = Constant.AllergySeverity.UNKNOWN;
                        oAllergy.KnownDate = DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112);
                        oAllergy.Reaction = txtReaction.Text;
                        oAllergy.CreatedBy = AppSession.UserLogin.UserID;

                        BusinessLayer.InsertPatientAllergy(oAllergy);
                        result = "1|add|";
                    }
                    else if (param[0] == "edit")
                    {
                        int allergyID = Convert.ToInt32(hdnAllergyID.Value);
                        PatientAllergy oAllergy = BusinessLayer.GetPatientAllergy(allergyID);

                        if (oAllergy != null)
                        {
                            oAllergy.GCAllergenType = cboAllergenType.Value.ToString();
                            oAllergy.Allergen = txtAllergenName.Text;
                            oAllergy.Reaction = txtReaction.Text;
                            oAllergy.LastUpdatedBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdatePatientAllergy(oAllergy);
                            result = "1|edit|";
                        }
                        else
                        {
                            result = string.Format("0|delete|{0}", "Invalid Patient Allergy Record Information");
                        }
                    }
                    else
                    {
                        int allergyID = Convert.ToInt32(hdnAllergyID.Value);
                        PatientAllergy oAllergy = BusinessLayer.GetPatientAllergy(allergyID);

                        if (oAllergy != null)
                        {
                            //TODO : Prompt user for delete reason
                            oAllergy.IsDeleted = true;
                            oAllergy.LastUpdatedBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdatePatientAllergy(oAllergy);
                            result = "1|delete|";
                        }
                        else
                        {
                            result = string.Format("0|edit|{0}", "Invalid Patient Allergy Record Information");
                        }
                        result = "1|delete|";
                    }

                }

            }
            catch (Exception ex)
            {
                result = string.Format("0|process|{0}", ex.Message);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        #region Review of System
        protected void grdROSView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vReviewOfSystemHd obj = (vReviewOfSystemHd)e.Row.DataItem;
                Repeater rptReviewOfSystemDt = (Repeater)e.Row.FindControl("rptReviewOfSystemDt");
                rptReviewOfSystemDt.DataSource = GetReviewOfSystemDt(obj.ID);
                rptReviewOfSystemDt.DataBind();
            }
        }

        protected List<vReviewOfSystemDt> GetReviewOfSystemDt(Int32 ID)
        {
            return lstReviewOfSystemDt.Where(p => p.ID == ID).ToList();
        }

        protected void cbpROSView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewROS(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewROS(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridViewROS(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvReviewOfSystemHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vReviewOfSystemHd> lstEntity = BusinessLayer.GetvReviewOfSystemHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
            lstReviewOfSystemDt = BusinessLayer.GetvReviewOfSystemDtList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID));
            grdROSView.DataSource = lstEntity;
            grdROSView.DataBind();
        }

        protected void cbpDeleteROS_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|";

            if (hdnReviewOfSystemID.Value != "")
            {
                ReviewOfSystemHd entity = BusinessLayer.GetReviewOfSystemHd(Convert.ToInt32(hdnReviewOfSystemID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateReviewOfSystemHd(entity);
            }
            else
            {
                result = "0|There is no record to be deleted !";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        #region Body Diagram
        protected void cbpBodyDiagramView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            int pageIndex = Convert.ToInt32(hdnPageIndex.Value);
            int pageCount = Convert.ToInt32(hdnPageCount.Value);
            if (e.Parameter == "refresh")
            {
                string filterExpression = "";
                filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);

                pageCount = BusinessLayer.GetvPatientBodyDiagramHdRowCount(filterExpression);
                result = "count|" + pageCount;
                if (pageCount > 0)
                    OnLoadBodyDiagram(0);
            }
            else if (e.Parameter == "edit")
            {
                result = "edit";
                OnLoadBodyDiagram(pageIndex);
            }
            else
            {
                if (e.Parameter == "next")
                {
                    pageIndex++;
                    if (pageIndex == pageCount)
                        pageIndex = 0;
                }
                else if (e.Parameter == "prev")
                {
                    pageIndex--;
                    if (pageIndex < 0)
                        pageIndex = pageCount - 1;
                }
                OnLoadBodyDiagram(pageIndex);
                result = "index|" + pageIndex;
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void OnLoadBodyDiagram(int PageIndex)
        {
            string filterExpression = "";
            filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);
            vPatientBodyDiagramHd entity = BusinessLayer.GetvPatientBodyDiagramHd(filterExpression, PageIndex, "ID DESC");
            BodyDiagramToControl(entity);

            filterExpression = string.Format("ID = {0} AND IsDeleted = 0", entity.ID);
            rptRemarks.DataSource = BusinessLayer.GetvPatientBodyDiagramDtList(filterExpression);
            rptRemarks.DataBind();
        }

        private void BodyDiagramToControl(vPatientBodyDiagramHd entity)
        {
            spnParamedicName.InnerHtml = entity.ParamedicName;
            spnObservationDateTime.InnerHtml = entity.DisplayObservationDateTime;
            spnDiagramName.InnerHtml = entity.DiagramName;

            imgBodyDiagram.Src = entity.FileImageUrl;
            hdnBodyDiagramID.Value = entity.ID.ToString();

        }

        protected void cbpDeleteBodyDiagram_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|";

            if (hdnBodyDiagramID.Value != "")
            {
                PatientBodyDiagramHd entity = BusinessLayer.GetPatientBodyDiagramHd(Convert.ToInt32(hdnBodyDiagramID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePatientBodyDiagramHd(entity);
            }
            else
            {
                result = "0|There is no record to be deleted !";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        #region Diagnosis
        private void BindGridViewDiagnosis(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientDiagnosisRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientDiagnosis> lstEntity = BusinessLayer.GetvPatientDiagnosisList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "GCDiagnoseType");
            List<vPatientDiagnosis> lstMainDiagnosis = lstEntity.Where(lst => lst.GCDiagnoseType == Constant.DiagnoseType.MAIN_DIAGNOSIS).ToList();
            vPatientDiagnosis registrationDiagnosis = lstEntity.Where(lst => lst.GCDiagnoseType == Constant.DiagnoseType.EARLY_DIAGNOSIS).FirstOrDefault();

            hdnIsMainDiagnosisExists.Value = lstMainDiagnosis.Count > 0 ? "1" : "0";

            grdDiagnosisView.DataSource = lstEntity.Where(lst => lst.GCDiagnoseType != Constant.DiagnoseType.EARLY_DIAGNOSIS);
            grdDiagnosisView.DataBind();

            if (hdnIsMainDiagnosisExists.Value == "1")
                cboDiagnosisType.Value = Constant.DiagnoseType.COMPLICATION;
            else
                cboDiagnosisType.Value = Constant.DiagnoseType.MAIN_DIAGNOSIS;

            if (registrationDiagnosis != null)
                txtDiagnosis.Text = registrationDiagnosis.cfDiagnosisText;
            else
                txtDiagnosis.Text = string.Empty;

            //Create Diagnosis Summary for : CPOE Clinical Notes
            StringBuilder strDiagnosis = new StringBuilder();
            foreach (var item in lstEntity)
            {
                if (item.GCDifferentialStatus != Constant.DifferentialDiagnosisStatus.RULED_OUT)
                {
                    strDiagnosis.AppendLine(string.Format("{0}", item.cfDiagnosisText)); 
                }
            }
            hdnDiagnosisSummary.Value = strDiagnosis.ToString();
        }

        protected void cbpDiagnosisView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDiagnosis(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDiagnosis(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = hdnIsMainDiagnosisExists.Value;
        }
        protected void cbpDiagnosis_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|add|";

            try
            {
                if (e.Parameter != null && e.Parameter != "")
                {
                    string[] param = e.Parameter.Split('|');
                    if (param[0] == "add")
                    {
                        PatientDiagnosis entity = new PatientDiagnosis();

                        entity.VisitID = AppSession.RegisteredPatient.VisitID;
                        entity.DifferentialDate = Helper.GetDatePickerValue(txtServiceDate);
                        entity.DifferentialTime = txtServiceTime.Text;

                        entity.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                        entity.GCDiagnoseType = cboDiagnosisType.Value.ToString();
                        hdnIsMainDiagnosisExists.Value = entity.GCDiagnoseType == Constant.DiagnoseType.MAIN_DIAGNOSIS ? "1" : "0";
                        entity.GCDifferentialStatus = cboDiagnosisStatus.Value.ToString();

                        if (hdnEntryDiagnoseID.Value != "")
                            entity.DiagnoseID = hdnEntryDiagnoseID.Value;
                        else
                            entity.DiagnoseID = null;

                        entity.DiagnosisText = txtDiagnosisText.Text;
                        entity.MorphologyID = null;
                        entity.IsChronicDisease = false;
                        entity.IsFollowUpCase = false;
                        entity.Remarks = string.Empty;
                        entity.CreatedBy = AppSession.UserLogin.UserID;

                        BusinessLayer.InsertPatientDiagnosis(entity);

                        result = "1|add|";
                    }
                    else if (param[0] == "edit")
                    {
                        int recordID = Convert.ToInt32(hdnDiagnosisID.Value);
                        PatientDiagnosis entity = BusinessLayer.GetPatientDiagnosis(recordID);

                        if (entity != null)
                        {
                            entity.DifferentialDate = Helper.GetDatePickerValue(txtServiceDate);
                            entity.DifferentialTime = txtServiceTime.Text;

                            entity.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                            entity.GCDiagnoseType = cboDiagnosisType.Value.ToString();
                            hdnIsMainDiagnosisExists.Value = entity.GCDiagnoseType == Constant.DiagnoseType.MAIN_DIAGNOSIS ? "1" : "0";
                            entity.GCDifferentialStatus = cboDiagnosisStatus.Value.ToString();

                            if (hdnEntryDiagnoseID.Value != "")
                                entity.DiagnoseID = hdnEntryDiagnoseID.Value;
                            else
                                entity.DiagnoseID = null;

                            entity.DiagnosisText = txtDiagnosisText.Text;
                            entity.MorphologyID = null;
                            entity.IsChronicDisease = false;
                            entity.IsFollowUpCase = false;
                            entity.Remarks = string.Empty;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdatePatientDiagnosis(entity);
                            result = "1|edit|";
                        }
                        else
                        {
                            result = string.Format("0|delete|{0}", "Invalid Patient Diagnosis Record Information");
                        }
                    }
                    else
                    {
                        int recordID = Convert.ToInt32(hdnDiagnosisID.Value);
                        PatientDiagnosis entity = BusinessLayer.GetPatientDiagnosis(recordID);
                        if (entity != null)
                        {
                            if (entity.ClaimDiagnosisID != null && entity.ClaimDiagnosisID != "")
                            {
                                result = string.Format("0|delete|{0}", "Data diagnosa pasien ini tidak dapat dihapus karena sudah dilengkapi oleh Casemix.");
                            }
                            else
                            {
                                entity.IsDeleted = true;
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                BusinessLayer.UpdatePatientDiagnosis(entity);

                                result = "1|delete|";
                            }
                        }
                        else
                        {
                            result = string.Format("0|delete|{0}", "Invalid Patient Diagnosis Record Information");
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                result = string.Format("0|process|{0}", ex.Message);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        #region Laboratory
        private void BindGridViewLaboratory(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND HealthcareServiceUnitID = {1} AND GCTransactionStatus != '{2}'", AppSession.RegisteredPatient.VisitID, hdnLaboratoryServiceUnitID.Value, Constant.TransactionStatus.VOID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvTestOrderHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vTestOrderHd1> lstEntity = BusinessLayer.GetvTestOrderHd1List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "TestOrderID DESC");
            StringBuilder sbText = new StringBuilder();
            foreach (vTestOrderHd1 item in lstEntity)
            {
                foreach (CompactTestOrderDtInfo detail in item.cfTestOrderDetailList)
                {
                    sbText.AppendLine(string.Format("- {0}", detail.ItemName1));
                }
            }
            hdnLaboratorySummary.Value = sbText.ToString();
            
            grdLaboratoryView.DataSource = lstEntity;
            grdLaboratoryView.DataBind();
        }

        protected void cbpLaboratoryView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewLaboratory(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewLaboratory(1, true, ref pageCount);
                    result = "refresh|" + pageCount + '|' + hdnLaboratorySummary.Value;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpSummary"] = hdnLaboratorySummary.Value;
        }

        protected void grdLaboratoryView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vTestOrderHd1 obj = (vTestOrderHd1)e.Row.DataItem;
                Repeater rptLaboratoryDt = (Repeater)e.Row.FindControl("rptLaboratoryDt");
                rptLaboratoryDt.DataSource = GetTestOrderDt(obj.TestOrderID);
                rptLaboratoryDt.DataBind();
            }
        }

        private object GetTestOrderDt(int testOrderID)
        {
            List<vTestOrderDt> lstOrderDt = BusinessLayer.GetvTestOrderDtList(string.Format("TestOrderID = {0} AND IsDeleted = 0 ORDER BY ItemName1", testOrderID));
            return lstOrderDt;
        }
        #endregion

        #region Imaging
        private void BindGridViewImaging(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND HealthcareServiceUnitID = {1} AND GCTransactionStatus != '{2}'", AppSession.RegisteredPatient.VisitID, hdnImagingServiceUnitID.Value, Constant.TransactionStatus.VOID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvTestOrderHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vTestOrderHd1> lstEntity = BusinessLayer.GetvTestOrderHd1List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "TestOrderID DESC");
            StringBuilder sbText = new StringBuilder();
            foreach (vTestOrderHd1 item in lstEntity)
            {
                foreach (CompactTestOrderDtInfo detail in item.cfTestOrderDetailList)
                {
                    sbText.AppendLine(string.Format("- {0}", detail.ItemName1));
                }
            }
            hdnImagingSummary.Value = sbText.ToString();

            grdImagingView.DataSource = lstEntity;
            grdImagingView.DataBind();
        }

        protected void cbpImagingView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewImaging(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewImaging(1, true, ref pageCount);
                    result = "refresh|" + pageCount + '|' + hdnImagingSummary.Value;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpSummary"] = hdnImagingSummary.Value;
        }

        protected void grdImagingView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vTestOrderHd1 obj = (vTestOrderHd1)e.Row.DataItem;
                Repeater rptImagingDt = (Repeater)e.Row.FindControl("rptImagingDt");
                rptImagingDt.DataSource = GetTestOrderDt(obj.TestOrderID);
                rptImagingDt.DataBind();
            }
        }
        #endregion

        #region Diagnostic
        private void BindGridViewDiagnostic(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND HealthcareServiceUnitID NOT IN ({1},{2}) AND GCTransactionStatus != '{3}'", AppSession.RegisteredPatient.VisitID, hdnLaboratoryServiceUnitID.Value, hdnImagingServiceUnitID.Value, Constant.TransactionStatus.VOID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvTestOrderHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vTestOrderHd1> lstEntity = BusinessLayer.GetvTestOrderHd1List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "TestOrderID DESC");
            StringBuilder sbText = new StringBuilder();
            foreach (vTestOrderHd1 item in lstEntity)
            {
                foreach (CompactTestOrderDtInfo detail in item.cfTestOrderDetailList)
                {
                    sbText.AppendLine(string.Format("- {0}", detail.ItemName1));
                }
            }
            hdnDiagnosticSummary.Value = sbText.ToString();

            grdDiagnosticView.DataSource = lstEntity;
            grdDiagnosticView.DataBind();
        }

        protected void grdDiagnosticView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vTestOrderHd1 obj = (vTestOrderHd1)e.Row.DataItem;
                Repeater rptDiagnosticDt = (Repeater)e.Row.FindControl("rptDiagnosticDt");
                rptDiagnosticDt.DataSource = obj.cfTestOrderDetailList;
                rptDiagnosticDt.DataBind();
            }
        }

        protected void cbpDiagnosticView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDiagnostic(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDiagnostic(1, true, ref pageCount);
                    result = "refresh|" + pageCount + '|' + hdnDiagnosticSummary.Value;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpSummary"] = hdnDiagnosticSummary.Value;
        }
        #endregion

        #region Instruction
        private void BindGridViewInstruction(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string visitNoteID = !string.IsNullOrEmpty(_visitNoteID) ? _visitNoteID : "0";
            string filterExpression = string.Format("VisitID IN ({0}) AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, visitNoteID);

            if (visitNoteID != "0")
            {
                filterExpression += string.Format(" AND PatientVisitNoteID = {0}", visitNoteID);
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientInstructionRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientInstruction> lstEntity = BusinessLayer.GetvPatientInstructionList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "PatientInstructionID");

            StringBuilder instructionText = new StringBuilder();
            foreach (vPatientInstruction obj in lstEntity)
            {
                instructionText.AppendLine(string.Format("- {0}", obj.Description));
            }
            hdnInstructionText.Value = instructionText.ToString();

            grdInstructionView.DataSource = lstEntity;
            grdInstructionView.DataBind();

            //if (lstEntity.Count == 0)
            //    trWriteInstructionCPPT.Style.Add("display", "none");
            //else
            //    trWriteInstructionCPPT.Style.Remove("display");
        }

        protected void cbpInstructionView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewInstruction(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewInstruction(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = "";
        }
        protected void cbpInstruction_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|add|";

            try
            {
                if (e.Parameter != null && e.Parameter != "")
                {
                    string[] param = e.Parameter.Split('|');
                    if (param[0] == "add")
                    {
                        if (_visitNoteID == "0" || _visitNoteID == "")
                        {
                            PatientVisitNote soapNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = {0} AND GCPatientNoteType = '{1}' AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES)).FirstOrDefault();
                            if (soapNote != null)
                            {
                                _visitNoteID = soapNote.ID.ToString();
                                hdnPatientVisitNoteID.Value = soapNote.ID.ToString();
                            }
                        }

                        PatientInstruction entity = new PatientInstruction();

                        entity.VisitID = AppSession.RegisteredPatient.VisitID;
                        entity.PatientVisitNoteID = Convert.ToInt32(_visitNoteID);
                        entity.InstructionDate = DateTime.Now.Date;
                        entity.InstructionTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

                        entity.PhysicianID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                        entity.GCInstructionGroup = cboInstructionType.Value.ToString();
                        entity.Description = txtInstructionText.Text;

                        if (!string.IsNullOrEmpty(hdnPatientVisitNoteID.Value) && hdnPatientVisitNoteID.Value != "0")
                        {
                            entity.PatientVisitNoteID = Convert.ToInt32(hdnPatientVisitNoteID.Value);
                        }

                        BusinessLayer.InsertPatientInstruction(entity);

                        hdnIsChanged.Value = "1";
                        hdnIsSaved.Value = "0";

                        result = "1|add|";
                    }
                    else if (param[0] == "edit")
                    {
                        int recordID = Convert.ToInt32(hdnInstructionID.Value);
                        PatientInstruction entity = BusinessLayer.GetPatientInstruction(recordID);

                        if (entity != null)
                        {
                            entity.GCInstructionGroup = cboInstructionType.Value.ToString();
                            entity.Description = txtInstructionText.Text;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdatePatientInstruction(entity);

                            hdnIsChanged.Value = "1";
                            hdnIsSaved.Value = "0";

                            result = "1|edit|";
                        }
                        else
                        {
                            result = string.Format("0|delete|{0}", "Invalid Patient Instruction Record Information");
                        }
                    }
                    else
                    {
                        int recordID = Convert.ToInt32(hdnInstructionID.Value);
                        PatientInstruction entity = BusinessLayer.GetPatientInstruction(recordID);

                        if (entity != null)
                        {
                            //TODO : Prompt user for delete reason
                            entity.IsDeleted = true;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdatePatientInstruction(entity);

                            hdnIsChanged.Value = "1";
                            hdnIsSaved.Value = "0";

                            result = "1|delete|";
                        }
                        else
                        {
                            result = string.Format("0|edit|{0}", "Invalid Patient Instruction Record Information");
                        }
                        result = "1|delete|";
                    }

                }

            }
            catch (Exception ex)
            {
                result = string.Format("0|process|{0}", ex.Message);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion


        protected void cbpDeleteTestOrder_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|";

            if (e.Parameter != null && e.Parameter != "")
            {
                string testOrderID = "0";

                switch (e.Parameter)
                {
                    case "LB":
                        testOrderID = hdnLaboratoryTestOrderID.Value;
                        break;
                    case "IS":
                        testOrderID = hdnImagingTestOrderID.Value;
                        break;
                    case "MD":
                        testOrderID = hdnDiagnosticTestOrderID.Value;
                        break;
                    default:
                        break;
                }

                if (testOrderID != "0")
                {
                    TestOrderHd entity = BusinessLayer.GetTestOrderHd(Convert.ToInt32(testOrderID));
                    entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateTestOrderHd(entity);
                    result = string.Format("1|{0}", e.Parameter);
                }
                else
                {
                    result = string.Format("0|{0}|There is no record to be deleted !");
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpDoctorFee_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string[] param = e.Parameter.Split('|');
            int transactionID = Convert.ToInt32(param[2]);
            result = param[0] + "|" + param[1] + "|";
            try
            {
                if (param[0] == "sendOrder")
                {
                    if (param[1] != "PH")
                    {
                        TestOrderHd entity = BusinessLayer.GetTestOrderHdList(String.Format("TestOrderID = {0}", transactionID))[0];
                        if (entity.GCTransactionStatus != Constant.TransactionStatus.CLOSED)
                        {
                            entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                            BusinessLayer.UpdateTestOrderHd(entity);
                            try
                            {
                                HealthcareServiceUnit hsu = BusinessLayer.GetHealthcareServiceUnit(Convert.ToInt32(entity.HealthcareServiceUnitID));
                                string ipAddress = hsu.IPAddress == null ? string.Empty : hsu.IPAddress;

                                if (!String.IsNullOrEmpty(ipAddress))
                                {
                                    SendNotification(entity, ipAddress);
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                }
                result += string.Format("success|{0}|{1}", param[1], errMessage);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result += string.Format("fail|{0}|{1}", param[1], errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTransactionID"] = transactionID;
        }

        protected void cbpSendOrder_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string[] param = e.Parameter.Split('|');
            int transactionID = Convert.ToInt32(param[2]);
            result = param[0] + "|" + param[1] + "|";
            try
            {
                if (param[0] == "sendOrder")
                {
                    if (param[1] != "PH")
                    {
                        TestOrderHd entity = BusinessLayer.GetTestOrderHdList(String.Format("TestOrderID = {0}", transactionID))[0];
                        if (entity.GCTransactionStatus != Constant.TransactionStatus.CLOSED)
                        {
                            entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                            entity.SendOrderDateTime = DateTime.Now;
                            entity.SendOrderBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdateTestOrderHd(entity);
                            try
                            {
                                HealthcareServiceUnit hsu = BusinessLayer.GetHealthcareServiceUnit(Convert.ToInt32(entity.HealthcareServiceUnitID));
                                string ipAddress = hsu.IPAddress == null ? string.Empty : hsu.IPAddress;

                                if (!String.IsNullOrEmpty(ipAddress))
                                {
                                    SendNotification(entity, ipAddress);
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                }
                result += string.Format("success|{0}{1}", param[1], errMessage);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result += string.Format("fail|{0}|{1}", param[1], errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTransactionID"] = transactionID;
        }

        private void SendNotification(TestOrderHd order, string ipAddress)
        {
            StringBuilder sbMessage = new StringBuilder();
            sbMessage.AppendLine(string.Format("No  : {0}", order.TestOrderNo));
            sbMessage.AppendLine(string.Format("Fr  : {0}", string.Format("{0} ({1})", AppSession.RegisteredPatient.ServiceUnitName, AppSession.UserLogin.UserFullName)));
            sbMessage.AppendLine(string.Format("Px  : {0}", AppSession.RegisteredPatient.PatientName));
            sbMessage.AppendLine(string.Format("PDx :    "));
            sbMessage.AppendLine(string.Format("{0}", order.Remarks));
            TcpClient client = new TcpClient();
            client.Connect(IPAddress.Parse(ipAddress), 6000);
            NetworkStream stream = client.GetStream();
            using (BinaryWriter w = new BinaryWriter(stream))
            {
                using (BinaryReader r = new BinaryReader(stream))
                {
                    w.Write(string.Format(@"{0}", sbMessage.ToString()).ToCharArray());
                }
            }
        }

        protected void cbpIntegrationNote_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            result = "fail|no error information available";

            if ((hdnChiefComplaintID.Value != "" && hdnChiefComplaintID.Value != "0") && !IsValidToSave(ref errMessage))
            {
                hdnIsSaved.Value = "0";
                result = "fail|" + errMessage;
            }
            else
            {

                IDbContext ctx = DbFactory.Configure(true);
                PatientVisitNoteDao patientVisitNoteDao = new PatientVisitNoteDao(ctx);

                try
                {
                    PatientVisitNote entityVisitNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = {0} AND GCPatientNoteType = '{1}'", AppSession.RegisteredPatient.VisitID, Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES), ctx).FirstOrDefault();
                    bool isEntityVisitNoteNull = false;
                    if (entityVisitNote == null)
                    {
                        isEntityVisitNoteNull = true;
                        entityVisitNote = new PatientVisitNote();
                    }

                    #region Summarize Instruction Text
                    string filterExpression = string.Format("VisitID IN ({0}) AND PatientVisitNoteID = {1} AND IsDeleted = 0 ORDER BY PatientInstructionID", AppSession.RegisteredPatient.VisitID, _visitNoteID);
                    List<PatientInstruction> lstEntity = BusinessLayer.GetPatientInstructionList(filterExpression, ctx);

                    StringBuilder instructionText = new StringBuilder();
                    foreach (PatientInstruction obj in lstEntity)
                    {
                        instructionText.AppendLine(string.Format("- {0}", obj.Description));
                    }

                    hdnInstructionText.Value = instructionText.ToString();
                    #endregion

                    ControlToEntity(entityVisitNote);

                    if (isEntityVisitNoteNull)
                    {
                        entityVisitNote.VisitID = AppSession.RegisteredPatient.VisitID;
                        entityVisitNote.ParamedicID = AppSession.UserLogin.ParamedicID;
                        entityVisitNote.GCPatientNoteType = Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES;
                        entityVisitNote.CreatedBy = AppSession.UserLogin.UserID;
                        hdnPatientVisitNoteID.Value = patientVisitNoteDao.InsertReturnPrimaryKeyID(entityVisitNote).ToString();
                    }
                    else
                    {
                        entityVisitNote.ParamedicID = AppSession.UserLogin.ParamedicID;
                        entityVisitNote.LastUpdatedBy = AppSession.UserLogin.UserID;
                        patientVisitNoteDao.Update(entityVisitNote);

                        hdnPatientVisitNoteID.Value = entityVisitNote.ID.ToString();
                    }

                    _visitNoteID = hdnPatientVisitNoteID.Value;

                    ctx.CommitTransaction();

                    result += string.Format("success|{0}", errMessage);
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    result += string.Format("fail|{0}", errMessage);
                }

                ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
                panel.JSProperties["cpResult"] = result;
                panel.JSProperties["cpVisitNoteID"] = _visitNoteID;
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

        public static string StripHTML(string HTMLText, bool decode = true)
        {
            Regex reg = new Regex("<[^>]+>", RegexOptions.IgnoreCase);
            var stripped = reg.Replace(HTMLText, "");
            return decode ? HttpUtility.HtmlDecode(stripped) : stripped;
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
                    List<vLaboratoryResultDt> lstLabEntity = BusinessLayer.GetvLaboratoryResultDtList(string.Format("ChargeTransactionID = {0} AND ItemID = {1} ORDER BY DisplayOrder", obj.ChargesID, obj.ItemID));
                    if (lstLabEntity.Count > 0)
                    {
                        foreach (vLaboratoryResultDt result in lstLabEntity)
                        {
                            LaboratoryDtInfo oDetail = new LaboratoryDtInfo();
                            oDetail.ChargesID = result.ChargeTransactionID;
                            oDetail.ItemID = result.ItemID;
                            oDetail.FractionName = result.FractionName1;
                            if (!String.IsNullOrEmpty(result.TextValue))
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
                            oDetail.RefRange = string.Format("{0} - {1}", result.MinMetricNormalValue.ToString("G29"), result.MaxMetricNormalValue.ToString("G29"));
                            oDetail.ResultFlag = string.IsNullOrEmpty(result.ResultFlag) ? "N" : result.ResultFlag;

                            lstResultDt.Add(oDetail);
                        }
                    }
                }

                rptLaboratoryDt.DataSource = lstResultDt;
                rptLaboratoryDt.DataBind();
            }
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

                rptImagingTestOrderDt.DataSource = lstResultDt;
                rptImagingTestOrderDt.DataBind();
            }
        }

        protected void cbpSOAPCopy_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";

            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                ChiefComplaint obj = BusinessLayer.GetChiefComplaintList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID), ctx).FirstOrDefault();
                if (obj != null)
                    hdnChiefComplaintID.Value = obj.ID.ToString();
                else
                    hdnChiefComplaintID.Value = "0";

                CopyFromPreviousVisit(ctx);
                ctx.CommitTransaction();

                hdnIsSaved.Value = "1";
                hdnIsChanged.Value = "0";

                result += string.Format("success|{0}", errMessage);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result += string.Format("fail|{0}", errMessage);
                hdnIsSaved.Value = "0";
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTransactionID"] = 0;
        }

        private void CopyFromPreviousVisit(IDbContext ctx)
        {
            int visitID = Convert.ToInt32(hdnPreviousVisitID.Value);

            ChiefComplaintDao chiefComplaintDao = new ChiefComplaintDao(ctx);
            ConsultVisitDao consultVisitDao = new ConsultVisitDao(ctx);
            RegistrationDao registrationDao = new RegistrationDao(ctx);
            PastMedicalDao pastMedicalDao = new PastMedicalDao(ctx);

            ConsultVisit entityConsultVisit = consultVisitDao.Get(AppSession.RegisteredPatient.VisitID);
            if (entityConsultVisit.GCVisitStatus == Constant.VisitStatus.CHECKED_IN)
            {
                entityConsultVisit.StartServiceDate = Helper.GetDatePickerValue(txtServiceDate);
                entityConsultVisit.StartServiceTime = txtServiceTime.Text;
            }

            if (!string.IsNullOrEmpty(cboVisitType.Value.ToString()))
            {
                entityConsultVisit.VisitTypeID = Convert.ToInt16(cboVisitType.Value);
            }

            if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.EMERGENCY)
            {
                entityConsultVisit.TimeElapsed0 = string.Format("{0}:{1}", hdnTimeElapsed0hour.Value.PadLeft(2, '0'), hdnTimeElapsed0minute.Value.PadLeft(2, '0'));
            }
            entityConsultVisit.TimeElapsed1 = string.Format("{0}:{1}", hdnTimeElapsed1hour.Value.PadLeft(2, '0'), hdnTimeElapsed1minute.Value.PadLeft(2, '0'));
            entityConsultVisit.LastUpdatedBy = AppSession.UserLogin.UserID;

            consultVisitDao.Update(entityConsultVisit);

            ChiefComplaint entity = null;
            bool isNewChiefComplaint = true;

            if (hdnChiefComplaintID.Value != "" && hdnChiefComplaintID.Value != "0")
            {
                entity = chiefComplaintDao.Get(Convert.ToInt32(hdnChiefComplaintID.Value));
                isNewChiefComplaint = false;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
            }
            else
            {
                entity = new ChiefComplaint();
                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                entity.CreatedBy = AppSession.UserLogin.UserID;
            }
            entity.ObservationDate = Helper.GetDatePickerValue(txtServiceDate);
            entity.ObservationTime = txtServiceTime.Text;

            if (chkIsCopyChiefComplaint.Checked)
            {
                string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", visitID);
                vChiefComplaint oChiefComplaint = BusinessLayer.GetvChiefComplaintList(filterExpression, 5, 1, "ID DESC").FirstOrDefault();

                if (oChiefComplaint != null)
                {
                    entity.ChiefComplaintText = oChiefComplaint.ChiefComplaintText;
                    if (chkIsCopyHPI.Checked)
                    {
                        entity.HPISummary = oChiefComplaint.HPISummary;
                    }
                    entity.IsPatientAllergyExists = oChiefComplaint.IsPatientAllergyExists;

                    entity.IsAutoAnamnesis = Convert.ToBoolean(oChiefComplaint.IsAutoAnamnesis);
                    entity.IsAlloAnamnesis = Convert.ToBoolean(oChiefComplaint.IsAlloAnamnesis);
                }
            }
            else
            {
                entity.ChiefComplaintText = txtChiefComplaint.Text;
                entity.HPISummary = txtHPISummary.Text;
                entity.IsPatientAllergyExists = !chkIsPatientAllergyExists.Checked;

                entity.IsAutoAnamnesis = Convert.ToBoolean(chkAutoAnamnesis.Checked);
                entity.IsAlloAnamnesis = Convert.ToBoolean(chkAlloAnamnesis.Checked);
            }

            if (isNewChiefComplaint)
            {
                chiefComplaintDao.Insert(entity);
                hdnChiefComplaintID.Value = BusinessLayer.GetChiefComplaintMaxID(ctx).ToString();
            }
            else
            {
                chiefComplaintDao.Update(entity);
            }

            if (chkIsCopyROS.Checked)
            {
                ReviewOfSystemHdDao entityROSDao = new ReviewOfSystemHdDao(ctx);
                ReviewOfSystemDtDao entityROSDtDao = new ReviewOfSystemDtDao(ctx);

                string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", visitID);
                List<vReviewOfSystemHd> lstReviewOfSystemHd = BusinessLayer.GetvReviewOfSystemHdList(filterExpression, 10, 1, "ID DESC");
                if (lstReviewOfSystemHd.Count > 0)
                {
                    string lstID = String.Join(",", lstReviewOfSystemHd.Select(c => c.ID).ToArray());

                    ReviewOfSystemHd entityROS = new ReviewOfSystemHd();
                    List<ReviewOfSystemDt> lstEntityDt = BusinessLayer.GetReviewOfSystemDtList(string.Format("ID IN ({0})", lstID));

                    entityROS.ObservationDate = Helper.GetDatePickerValue(txtServiceDate);
                    entityROS.ObservationTime = txtServiceTime.Text;
                    entityROS.ParamedicID = AppSession.UserLogin.ParamedicID;
                    entityROS.VisitID = AppSession.RegisteredPatient.VisitID;
                    entityROS.CreatedBy = AppSession.UserLogin.UserID;
                    entityROSDao.Insert(entityROS);

                    entityROS.ID = BusinessLayer.GetReviewOfSystemHdMaxID(ctx);

                    foreach (ReviewOfSystemDt entityROSDt in lstEntityDt)
                    {
                        entityROSDt.ID = entity.ID;
                        entityROSDtDao.Insert(entityROSDt);
                    }
                }
            }

            if (chkIsCopyVitalSign.Checked)
            {
                VitalSignHdDao entityVSDao = new VitalSignHdDao(ctx);
                VitalSignDtDao entityVSDtDao = new VitalSignDtDao(ctx);

                string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", visitID);
                vVitalSignHd oVitalSignHd = BusinessLayer.GetvVitalSignHdList(filterExpression, 3, 1, "ID DESC").FirstOrDefault();
                if (oVitalSignHd != null)
                {
                    List<VitalSignDt> lstEntityVSDt = BusinessLayer.GetVitalSignDtList(string.Format("ID = {0}", oVitalSignHd.ID));

                    VitalSignHd entityVS = new VitalSignHd();
                    entityVS.ObservationDate = Helper.GetDatePickerValue(txtServiceDate);
                    entityVS.ObservationTime = txtServiceTime.Text;
                    entityVS.Remarks = oVitalSignHd.Remarks;
                    entityVS.IsDischargeVitalSign = false;
                    entityVS.VisitID = AppSession.RegisteredPatient.VisitID;
                    entityVS.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                    entityVS.CreatedBy = AppSession.UserLogin.UserID;
                    entityVSDao.Insert(entityVS);

                    entityVS.ID = BusinessLayer.GetVitalSignHdMaxID(ctx);

                    foreach (VitalSignDt entityDt in lstEntityVSDt)
                    {
                        entityDt.ID = entityVS.ID;
                        entityVSDtDao.Insert(entityDt);
                    }
                }
            }
        }

        private bool IsValidToSave(ref string errMessage)
        {
            bool result = true;
            if (hdnParamedicID.Value != "" && hdnParamedicID.Value != "0")
            {
                int paramedicID = Convert.ToInt32(hdnParamedicID.Value);
                if (AppSession.UserLogin.ParamedicID != paramedicID)
                {
                    errMessage = "Perubahan Kajian Awal Pasien hanya dapat dilakukan oleh Dokter yang melakukan Pengkajian";
                    result = false;
                }
            }
            return result;
        }
    }
}

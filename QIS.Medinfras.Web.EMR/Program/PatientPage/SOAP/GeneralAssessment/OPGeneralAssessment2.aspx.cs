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

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class OPGeneralAssessment2 : BasePagePatientPageList
    {
        protected int gridAllergyPageCount = 1;
        protected int gridVitalSignPageCount = 1;
        protected int gridProcedurePageCount = 1;
        protected int gridROSPageCount = 1;
        protected int gridDiagnosisPageCount = 1;
        protected int gridLaboratoryPageCount = 1;
        protected int gridImagingPageCount = 1;
        protected int gridDiagnosticPageCount = 1;
        protected List<vVitalSignDt> lstVitalSignDt = null;
        protected List<vReviewOfSystemDt> lstReviewOfSystemDt = null;

        protected string RegistrationDateTime = "";

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.SOAP_TEMPLATE_OUTPATIENT_2;
        }

        protected override void InitializeDataControl()
        {
            SetControlProperties();

            Helper.SetControlEntrySetting(cboVisitType, new ControlEntrySetting(true, true, true), "mpPatientStatus");

            SetEntityToControl();

            BindGridViewAllergy(1, true, ref gridAllergyPageCount);
            BindGridViewVitalSign(1, true, ref gridVitalSignPageCount);
            BindGridViewROS(1, true, ref gridROSPageCount);
            BindGridViewDiagnosis(1, true, ref gridDiagnosisPageCount);
            BindGridViewLaboratory(1, true, ref gridLaboratoryPageCount);
            BindGridViewImaging(1, true, ref gridImagingPageCount);
            BindGridViewDiagnostic(1, true, ref gridDiagnosticPageCount);

            LoadBodyDiagram();

            hdnIsChanged.Value = "0";
        }

        private void SetEntityToControl()
        {
            vConsultVisit1 entityVisit = BusinessLayer.GetvConsultVisit1List(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();

            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();

            List<PatientVisitNote> lstPatientVisitNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = '{0}' AND GCPatientNoteType = '{1}'", AppSession.RegisteredPatient.VisitID, Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES));

            if (lstPatientVisitNote.Count > 0)
            {
                PatientVisitNote entitypvn = lstPatientVisitNote.First();
                EntityToControl(entitypvn);
            }

            cboVisitType.Value = entityVisit.VisitTypeID.ToString();

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

            hdnDepartmentID.Value = entityVisit.DepartmentID;
            hdnIsPhysicianDischarge.Value = entityVisit.DischargeDate != DateTime.MinValue ? "1" : "0";

            vRegistration entityRegistration = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", entityVisit.RegistrationID)).FirstOrDefault();
            if (entityRegistration.GCReferrerGroup != null)
                cboReferral.Value = entityRegistration.GCReferrerGroup;
            else
                cboReferral.SelectedIndex = 0;
            hdnReferrerID.Value = entityRegistration.ReferrerID.ToString();
            hdnReferrerParamedicID.Value = entityRegistration.ReferrerParamedicID.ToString();
            if (entityRegistration.ReferrerID != 0)
            {
                txtReferralDescriptionCode.Text = entityRegistration.ReferrerCode;
                txtReferralDescriptionName.Text = entityRegistration.ReferrerName;
            }
            else if (entityRegistration.ReferrerParamedicID != 0)
            {
                ParamedicMaster pm = BusinessLayer.GetParamedicMaster(entityRegistration.ReferrerParamedicID);
                txtReferralDescriptionCode.Text = pm.ParamedicCode;
                txtReferralDescriptionName.Text = pm.FullName;
            }
            chkIsFastTrack.Checked = entityRegistration.IsFastTrack;

            vChiefComplaint obj = BusinessLayer.GetvChiefComplaintList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            if (obj != null)
            {
                if (obj.ID == 0)
                {
                    hdnChiefComplaintID.Value = "0";
                    lblVisitHistoryInfo.Visible = true;
                }
                else
                {
                    lblVisitHistoryInfo.Visible = false;
                    hdnChiefComplaintID.Value = obj.ID.ToString();
                    txtPreviousChiefComplaint.Text = obj.ChiefComplaintText;
                    chkIsPatientAllergyExists.Checked = !chkIsPatientAllergyExists.Checked;
                    chkAlloAnamnesis.Checked = obj.IsAlloAnamnesis;
                    chkAutoAnamnesis.Checked = obj.IsAutoAnamnesis;

                    Appointment oAppointment = BusinessLayer.GetAppointmentList(string.Format("FromVisitID  = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
                    if (oAppointment != null)
                    {
                        hdnAppointmentID.Value = oAppointment.AppointmentID.ToString();
                        cboFollowupVisitType.Value = oAppointment.VisitTypeID.ToString();
                        txtFollowupVisitDate.Text = oAppointment.StartDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                        txtFollowupVisitRemarks.Text = oAppointment.Notes;
                    }
                }
                txtMedicalProblem.Text = obj.MedicalProblem;
            }

            if (!entityVisit.IsNewPatient)
            {
                LoadPreviousVisitInformation(AppSession.RegisteredPatient.MRN, true);
            }
            else
            {
                lblVisitHistoryInfo.Text = "";
            }
        }

        private void LoadPreviousVisitInformation(int mrn, bool isShowSameServiceUnit)
        {
            string filterExpression = string.Format("MRN = {0} AND DepartmentID = '{1}' AND ParamedicID = {2} AND VisitID != {3} AND VisitID < {3} AND GCVisitStatus != '{4}' ORDER BY VisitID DESC", mrn, AppSession.RegisteredPatient.DepartmentID, AppSession.UserLogin.ParamedicID, AppSession.RegisteredPatient.VisitID, Constant.VisitStatus.CANCELLED);
            vConsultVisitCustom oLastVisit = BusinessLayer.GetvConsultVisitCustomList(filterExpression).FirstOrDefault();
            string previousChiefComplaint = string.Empty;
            if (oLastVisit != null)
            {
                previousChiefComplaint = oLastVisit.ChiefComplaint;
                lblVisitHistoryInfo.Text = string.Format("{0}, {1}", oLastVisit.ActualVisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT),oLastVisit.ParamedicName);
            }
            else
            {
                filterExpression = string.Format("MRN = {0} AND VisitID != {1} AND ParamedicID = {2} ORDER BY VisitID DESC", mrn, AppSession.RegisteredPatient.VisitID, AppSession.UserLogin.UserID);
                vChiefComplaint oChiefComplaint = BusinessLayer.GetvChiefComplaintList(filterExpression).FirstOrDefault();
                if (oChiefComplaint != null)
                {
                    previousChiefComplaint = oChiefComplaint.ChiefComplaintText;
                    lblVisitHistoryInfo.Text = string.Format("{0}, {1}", oChiefComplaint.ObservationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT), oChiefComplaint.ParamedicName);
                }
            }
            if (hdnChiefComplaintID.Value == "0" || hdnChiefComplaintID.Value == "")
            {
                txtPreviousChiefComplaint.Text = string.Format("{0}{1}{2}", previousChiefComplaint, Environment.NewLine, lblVisitHistoryInfo.Text);
            }
        }

        private void EntityToControl(PatientVisitNote entitypvn)
        {
            txtSubjectiveText.Text = entitypvn.SubjectiveText;
            txtObjectiveText.Text = entitypvn.ObjectiveText;
            txtAssessmentText.Text = entitypvn.AssessmentText;
            txtPlanningText.Text = entitypvn.PlanningText;
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
            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.REFERRAL, Constant.StandardCode.ALLERGEN_TYPE, Constant.StandardCode.DIAGNOSIS_TYPE, Constant.StandardCode.DIFFERENTIAL_DIAGNOSIS_STATUS,Constant.StandardCode.PATIENT_OUTCOME, Constant.StandardCode.DISCHARGE_ROUTINE);
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(filterExpression);

            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboReferral, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.REFERRAL || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboAllergenType, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.ALLERGEN_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboDiagnosisType, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.DIAGNOSIS_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboDiagnosisStatus, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.DIFFERENTIAL_DIAGNOSIS_STATUS).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboPatientOutcome, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.PATIENT_OUTCOME).ToList(), "StandardCodeName", "StandardCodeID");

            cboPatientOutcome.Value = Constant.PatientOutcome.BELUM_SEMBUH;
            cboDiagnosisStatus.Value = Constant.DifferentialDiagnosisStatus.UNDER_INVESTIGATION;

            List<GetParamedicVisitTypeList> visitTypeList = BusinessLayer.GetParamedicVisitTypeList(AppSession.RegisteredPatient.HealthcareServiceUnitID, (int)AppSession.UserLogin.ParamedicID, "");
            Methods.SetComboBoxField(cboVisitType, visitTypeList, "VisitTypeName", "VisitTypeID");
            Methods.SetComboBoxField(cboFollowupVisitType, visitTypeList, "VisitTypeName", "VisitTypeID");

            List<SettingParameterDt> lstSettingParameter = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM, Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI));
            hdnImagingServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).ParameterValue;
            hdnLaboratoryServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).ParameterValue;
        }

        private string GenerateSOAPText()
        {
            StringBuilder sbNotes = new StringBuilder();
            sbNotes.AppendLine("Subjective :");
            sbNotes.AppendLine("-".PadRight(15, '-'));
            if ((AppSession.RegisteredPatient.ParamedicID == AppSession.UserLogin.ParamedicID))
            {
                vChiefComplaint oChiefComplaint = BusinessLayer.GetvChiefComplaintList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
                if (oChiefComplaint != null)
                {
                    sbNotes.AppendLine(string.Format("Chief Complaint  : "));
                    sbNotes.AppendLine(string.Format(" {0}   ", oChiefComplaint.ChiefComplaintText));
                    if ((!string.IsNullOrEmpty(oChiefComplaint.Location)) || (!string.IsNullOrEmpty(oChiefComplaint.DisplayQuality)) ||
                       (!string.IsNullOrEmpty(oChiefComplaint.DisplaySeverity)) || (!string.IsNullOrEmpty(oChiefComplaint.DisplayOnset)) ||
                       (!string.IsNullOrEmpty(oChiefComplaint.CourseTiming)) || (!string.IsNullOrEmpty(oChiefComplaint.DisplayProvocation)))
                    {
                        sbNotes.AppendLine(string.Format("HPI  : "));
                    }
                    if (!string.IsNullOrEmpty(oChiefComplaint.Location))
                        sbNotes.AppendLine(string.Format("- Location    (R) : {0}", oChiefComplaint.Location));
                    if (!string.IsNullOrEmpty(oChiefComplaint.DisplayQuality))
                        sbNotes.AppendLine(string.Format("- Quality     (Q) : {0}", oChiefComplaint.DisplayQuality));
                    if (!string.IsNullOrEmpty(oChiefComplaint.DisplaySeverity))
                        sbNotes.AppendLine(string.Format("- Severity    (S) : {0}", oChiefComplaint.DisplaySeverity));
                    if (!string.IsNullOrEmpty(oChiefComplaint.DisplayOnset))
                        sbNotes.AppendLine(string.Format("- Onset       (O) : {0}", oChiefComplaint.DisplayOnset));
                    if (!string.IsNullOrEmpty(oChiefComplaint.CourseTiming))
                        sbNotes.AppendLine(string.Format("- Timing      (T) : {0}", oChiefComplaint.CourseTiming));
                    if (!string.IsNullOrEmpty(oChiefComplaint.DisplayProvocation))
                        sbNotes.AppendLine(string.Format("- Provocation (T) : {0}", oChiefComplaint.DisplayProvocation));
                }
            }

            sbNotes.AppendLine(" ");
            sbNotes.AppendLine("Objective :");
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
            if (cboReferral.Value != null)
                entityRegistration.GCReferrerGroup = cboReferral.Value.ToString();
            else entityRegistration.GCReferrerGroup = null;
            if (hdnReferrerID.Value == "" || hdnReferrerID.Value == "0")
                entityRegistration.ReferrerID = null;
            else
                entityRegistration.ReferrerID = Convert.ToInt32(hdnReferrerID.Value);

            if (hdnReferrerParamedicID.Value == "" || hdnReferrerParamedicID.Value == "0")
                entityRegistration.ReferrerParamedicID = null;
            else
                entityRegistration.ReferrerParamedicID = Convert.ToInt32(hdnReferrerParamedicID.Value);

            entityRegistration.IsFastTrack = chkIsFastTrack.Checked;
            entityRegistration.LastUpdatedBy = AppSession.UserLogin.UserID;
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
            if (chkIsNewChiefComplaint.Checked)            
                entity.ChiefComplaintText = txtPreviousChiefComplaint.Text;             
            else
                entity.ChiefComplaintText = txtPreviousChiefComplaint.Text;             

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
                    oAppointment.IsWaitingList = true;
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
                    PatientVisitNoteDao patientVisitNoteDao = new PatientVisitNoteDao(ctx);
                    PatientVisitNote entityVisitNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = {0} AND GCPatientNoteType = '{1}'", AppSession.RegisteredPatient.VisitID, Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES), ctx).FirstOrDefault();
                    bool isEntityVisitNoteNull = false;
                    if (entityVisitNote == null)
                    {
                        isEntityVisitNoteNull = true;
                        entityVisitNote = new PatientVisitNote();
                    }

                    entityVisitNote.VisitID = AppSession.RegisteredPatient.VisitID;
                    entityVisitNote.NoteDate = Helper.GetDatePickerValue(txtServiceDate);
                    entityVisitNote.NoteTime = txtServiceTime.Text;
                    entityVisitNote.ParamedicID = AppSession.UserLogin.ParamedicID;
                    entityVisitNote.GCPatientNoteType = Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES;
                    entityVisitNote.NoteText = string.Format(@"S:{0}{1}{2}O:{3}{4}{5}A:{6}{7}{8}P:{9}{10}",
                        Environment.NewLine, txtSubjectiveText.Text, Environment.NewLine,
                        Environment.NewLine, txtObjectiveText.Text, Environment.NewLine,
                        Environment.NewLine, txtAssessmentText.Text, Environment.NewLine,
                        Environment.NewLine, txtPlanningText.Text);
                    entityVisitNote.SubjectiveText = txtSubjectiveText.Text;
                    entityVisitNote.ObjectiveText = txtObjectiveText.Text;
                    entityVisitNote.AssessmentText = txtAssessmentText.Text;
                    entityVisitNote.PlanningText = txtPlanningText.Text;

                    if (isEntityVisitNoteNull)
                    {
                        entityVisitNote.CreatedBy = AppSession.UserLogin.UserID;
                        patientVisitNoteDao.Insert(entityVisitNote);
                    }
                    else
                    {
                        entityVisitNote.LastUpdatedBy = AppSession.UserLogin.UserID;
                        patientVisitNoteDao.Update(entityVisitNote);
                    }

                    ChiefComplaint obj = BusinessLayer.GetChiefComplaintList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID), ctx).FirstOrDefault();
                    if (obj != null)
                        hdnChiefComplaintID.Value = obj.ID.ToString();
                    else
                        hdnChiefComplaintID.Value = "0";

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
                entity.DischargeDate = Helper.GetDatePickerValue(txtServiceDate);
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

                        Patient oPatient = BusinessLayer.GetPatient(AppSession.RegisteredPatient.MRN);
                        if (!oPatient.IsHasAllergy)
                        {
                            oPatient.IsHasAllergy = true;
                            oPatient.LastUpdatedBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdatePatient(oPatient);
                        }

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
            lstReviewOfSystemDt = BusinessLayer.GetvReviewOfSystemDtList(string.Format("VisitID = {0} ORDER BY TagProperty", AppSession.RegisteredPatient.VisitID));
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
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0 AND IsNutritionDiagnosis = 0", AppSession.RegisteredPatient.VisitID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientDiagnosisRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientDiagnosis> lstEntity = BusinessLayer.GetvPatientDiagnosisList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "GCDiagnoseType");
            List<vPatientDiagnosis> lstMainDiagnosis = lstEntity.Where(lst => lst.GCDiagnoseType == Constant.DiagnoseType.MAIN_DIAGNOSIS).ToList();
            hdnIsMainDiagnosisExists.Value = lstMainDiagnosis.Count > 0 ? "1" : "0";

            grdDiagnosisView.DataSource = lstEntity;
            grdDiagnosisView.DataBind();

            if (hdnIsMainDiagnosisExists.Value == "1")
                cboDiagnosisType.Value = Constant.DiagnoseType.COMPLICATION;
            else
                cboDiagnosisType.Value = Constant.DiagnoseType.MAIN_DIAGNOSIS;
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
                        entity.GCFinalStatus = cboDiagnosisStatus.Value.ToString();

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
                            entity.GCFinalStatus = cboDiagnosisStatus.Value.ToString();

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

            List<vTestOrderHd> lstEntity = BusinessLayer.GetvTestOrderHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "TestOrderID DESC");
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
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void grdLaboratoryView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vTestOrderHd obj = (vTestOrderHd)e.Row.DataItem;
                Repeater rptLaboratoryDt = (Repeater)e.Row.FindControl("rptLaboratoryDt");
                rptLaboratoryDt.DataSource = GetTestOrderDt(obj.TestOrderID);
                rptLaboratoryDt.DataBind();
            }
        }

        private object GetTestOrderDt(int testOrderID)
        {
            List<vTestOrderDt> lstOrderDt = BusinessLayer.GetvTestOrderDtList(string.Format("TestOrderID = {0} ORDER BY ItemName1", testOrderID));
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

            List<vTestOrderHd> lstEntity = BusinessLayer.GetvTestOrderHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "TestOrderID DESC");
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
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void grdImagingView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vTestOrderHd obj = (vTestOrderHd)e.Row.DataItem;
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

            List<vTestOrderHd> lstEntity = BusinessLayer.GetvTestOrderHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "TestOrderID DESC");
            grdDiagnosticView.DataSource = lstEntity;
            grdDiagnosticView.DataBind();
        }

        protected void grdDiagnosticView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vTestOrderHd obj = (vTestOrderHd)e.Row.DataItem;
                Repeater rptDiagnosticDt = (Repeater)e.Row.FindControl("rptDiagnosticDt");
                rptDiagnosticDt.DataSource = GetTestOrderDt(obj.TestOrderID);
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
                    result = "refresh|" + pageCount;
                }
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
                result += string.Format("success|{0}", errMessage);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result += string.Format("fail|{0}", errMessage);
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
    }
}

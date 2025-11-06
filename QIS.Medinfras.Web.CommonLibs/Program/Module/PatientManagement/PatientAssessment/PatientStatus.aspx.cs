using System;
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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientStatus : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.INPATIENT: return Constant.MenuCode.EmergencyCare.CHIEF_COMPLAINT;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.CHIEF_COMPLAINT;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.PATIENT_TRANSACTION_MEDICATION_ORDER;
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.PATIENT_TRANSACTION_MEDICATION_ORDER;
                    return Constant.MenuCode.MedicalDiagnostic.PATIENT_TRANSACTION_MEDICATION_ORDER;
                default: return Constant.MenuCode.Outpatient.CHIEF_COMPLAINT;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            hdnIsNotAllowNurseFillChiefComplaint.Value = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.ER_NURSE_NOT_ALLOWED_ENTRY_PATIENT_STATUS).ParameterValue;

            vHealthcareServiceUnit hsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("DepartmentID = '{0}'", AppSession.RegisteredPatient.DepartmentID)).FirstOrDefault();
            hdnHealthcareServiceUnitID.Value = hsu.HealthcareServiceUnitID.ToString();
            hdnParamedicID.Value = AppSession.ParamedicID.ToString();

            ConsultVisit entityConsultVisit = BusinessLayer.GetConsultVisit(AppSession.RegisteredPatient.VisitID);
            hdnVisitTypeID.Value = entityConsultVisit.VisitTypeID.ToString();
            if (entityConsultVisit.VisitTypeID != null && entityConsultVisit.VisitTypeID != 0)
            {
                VisitType vt = BusinessLayer.GetVisitType(entityConsultVisit.VisitTypeID);
                txtVisitTypeCode.Text = vt.VisitTypeCode;
                txtVisitTypeName.Text = vt.VisitTypeName;
            }

            hdnMRN.Value = AppSession.RegisteredPatient.MRN.ToString();
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

        protected override void OnControlEntrySetting()
        {
            if (hdnIsNotAllowNurseFillChiefComplaint.Value == "0")
            {
                SetControlEntrySetting(txtChiefComplaint, new ControlEntrySetting(true, true, true));
                SetControlEntrySetting(chkAutoAnamnesis, new ControlEntrySetting(true, true, false, false));
                SetControlEntrySetting(chkAlloAnamnesis, new ControlEntrySetting(true, true, false, false));
                SetControlEntrySetting(txtLocation, new ControlEntrySetting(true, true, true));
                SetControlEntrySetting(cboOnset, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(txtOnset, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(cboProvocation, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(txtProvocation, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(cboQuality, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(txtQuality, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(cboSeverity, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(txtSeverity, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(cboTime, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(txtTime, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(cboRelievedBy, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(txtRelievedBy, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(cboVisitReason, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(txtVisitNotes, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(hdnDiagnoseID, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(txtDiagnose, new ControlEntrySetting(true, true, false));
            }
            else tdChiefComplaint.Style.Add("display", "none");
            SetControlEntrySetting(txtEmergencyCaseDate, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtEmergencyCaseTime, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRegistrationDate, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRegistrationTime, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRegistrationDateDiff, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtServiceDate, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtServiceTime, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtServiceDateDiff, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboTriage, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboAdmissionCondition, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtEmergencyCase, new ControlEntrySetting(true, true, false));
        }

        protected override void SetControlProperties()
        {
            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.ONSET, Constant.StandardCode.EXACERBATED, Constant.StandardCode.QUALITY,
                Constant.StandardCode.SEVERITY, Constant.StandardCode.COURSE_TIMING, Constant.StandardCode.RELIEVED_BY, Constant.StandardCode.TRIAGE, Constant.StandardCode.VISIT_REASON, Constant.StandardCode.ADMISSION_CONDITION);
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(filterExpression);
            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboOnset, lstSc.Where(p => p.ParentID == Constant.StandardCode.ONSET || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboProvocation, lstSc.Where(p => p.ParentID == Constant.StandardCode.EXACERBATED || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboQuality, lstSc.Where(p => p.ParentID == Constant.StandardCode.QUALITY || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboSeverity, lstSc.Where(p => p.ParentID == Constant.StandardCode.SEVERITY || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboTime, lstSc.Where(p => p.ParentID == Constant.StandardCode.COURSE_TIMING || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboRelievedBy, lstSc.Where(p => p.ParentID == Constant.StandardCode.RELIEVED_BY || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboTriage, lstSc.Where(p => p.ParentID == Constant.StandardCode.TRIAGE || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboAdmissionCondition, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.ADMISSION_CONDITION || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboVisitReason, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.VISIT_REASON || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            if (hdnIsNotAllowNurseFillChiefComplaint.Value == "1")
            {
                tdChiefComplaint.Style.Add("display", "none");
            }
            vPatientEarlyStatus entityPatientEarlyStatus = BusinessLayer.GetvPatientEarlyStatusList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            EntityToControl(entityPatientEarlyStatus);
        }

        private void EntityToControl(vPatientEarlyStatus entity)
        {
            hdnDepartmentID.Value = entity.DepartmentID;
            if (entity.ChiefComplaintID == 0)
            {
                hdnID.Value = "";
            }
            else hdnID.Value = entity.ChiefComplaintID.ToString();

            #region table sebelah kanan

            txtRegistrationDate.Text = txtEmergencyCaseDate.Text = txtServiceDate.Text = entity.ActualVisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtRegistrationTime.Text = txtEmergencyCaseTime.Text = txtServiceTime.Text = entity.ActualVisitTime;

            if (entity.EmergencyCaseDate != null && !string.IsNullOrEmpty(entity.EmergencyCaseTime))
            {
                txtEmergencyCaseDate.Text = entity.EmergencyCaseDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtEmergencyCaseTime.Text = entity.EmergencyCaseTime;
            }
            if (entity.StartServiceDate != null && !string.IsNullOrEmpty(entity.StartServiceTime))
            {
                txtServiceDate.Text = entity.StartServiceDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtServiceTime.Text = entity.StartServiceTime;
            }
            cboTriage.Value = entity.GCTriage;
            cboAdmissionCondition.Value = entity.GCAdmissionCondition;
            txtEmergencyCase.Text = entity.NoteText;
            #endregion

            #region table sebelah kiri
            #region chiefComplaint
            txtChiefComplaint.Text = entity.ChiefComplaintText;
            chkAutoAnamnesis.Checked = entity.IsAutoAnamnesis;
            chkAlloAnamnesis.Checked = entity.IsAlloAnamnesis;
            txtLocation.Text = entity.Location;
            cboOnset.Value = entity.GCOnset;
            cboProvocation.Value = entity.GCProvocation;
            cboQuality.Value = entity.GCQuality;
            cboSeverity.Value = entity.GCSeverity;
            cboTime.Value = entity.GCCourse;
            cboRelievedBy.Value = entity.GCRelieved;
            txtOnset.Text = entity.Onset;
            txtProvocation.Text = entity.Provocation;
            txtQuality.Text = entity.Quality;
            txtSeverity.Text = entity.Severity;
            txtTime.Text = entity.CourseTiming;
            txtRelievedBy.Text = entity.RelievedBy;
            #endregion

            cboVisitReason.Value = entity.GCVisitReason;
            txtVisitNotes.Text = entity.VisitReason;
            txtDiagnose.Text = entity.DiagnosisText;
            #endregion
        }

        private void ControlToEntity(ChiefComplaint entity, PatientVisitNote entitypvn, PatientDiagnosis patientDiagnosis)
        {
            #region Chief Complaint
            entity.ChiefComplaintText = txtChiefComplaint.Text;
            entity.Location = txtLocation.Text;
            entity.IsAutoAnamnesis = Convert.ToBoolean(chkAutoAnamnesis.Checked);
            entity.IsAlloAnamnesis = Convert.ToBoolean(chkAlloAnamnesis.Checked);

            if (cboQuality.Value == null)
                entity.GCQuality = "";
            else
                entity.GCQuality = cboQuality.Value.ToString();
            if (cboOnset.Value == null)
                entity.GCOnset = "";
            else
                entity.GCOnset = cboOnset.Value.ToString();
            if (cboRelievedBy.Value == null)
                entity.GCRelieved = "";
            else
                entity.GCRelieved = cboRelievedBy.Value.ToString();
            if (cboSeverity.Value == null)
                entity.GCSeverity = "";
            else
                entity.GCSeverity = cboSeverity.Value.ToString();
            if (cboProvocation.Value == null)
                entity.GCProvocation = "";
            else
                entity.GCProvocation = cboProvocation.Value.ToString();
            if (cboTime.Value == null)
                entity.GCCourse = "";
            else
                entity.GCCourse = cboTime.Value.ToString();
            entity.Quality = txtQuality.Text;
            entity.Onset = txtOnset.Text;
            entity.RelievedBy = txtRelievedBy.Text;
            entity.Severity = txtSeverity.Text;
            entity.Provocation = txtProvocation.Text;
            entity.CourseTiming = txtTime.Text;
            entity.ObservationDate = Helper.GetDatePickerValue(txtServiceDate);
            entity.ObservationTime = txtServiceTime.Text;
            #endregion

            #region Patient Visit Note
            entitypvn.NoteDate = Helper.GetDatePickerValue(txtEmergencyCaseDate);
            entitypvn.NoteTime = txtEmergencyCaseTime.Text;
            entitypvn.NoteText = txtEmergencyCase.Text;
            #endregion

            #region Patient Diagnosis
            patientDiagnosis.DiagnosisText = txtDiagnose.Text;
            if (!hdnDiagnoseID.Equals(string.Empty)) patientDiagnosis.DiagnoseID = Convert.ToString(hdnDiagnoseID.Value);
            #endregion
        }

        private void UpdateConsultVisitRegistration(IDbContext ctx)
        {
            ConsultVisitDao consultVisitDao = new ConsultVisitDao(ctx);
            RegistrationDao registrationDao = new RegistrationDao(ctx);
            ConsultVisit entityConsultVisit = consultVisitDao.Get(AppSession.RegisteredPatient.VisitID);
            entityConsultVisit.VisitTypeID = Convert.ToInt32(hdnVisitTypeID.Value);
            entityConsultVisit.ActualVisitDate = Helper.GetDatePickerValue(txtRegistrationDate);
            entityConsultVisit.ActualVisitTime = txtRegistrationTime.Text;
            entityConsultVisit.StartServiceDate = Helper.GetDatePickerValue(txtServiceDate);
            entityConsultVisit.StartServiceTime = txtServiceTime.Text;
            if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.EMERGENCY)
            {
                entityConsultVisit.TimeElapsed0 = string.Format("{0}:{1}", hdnTimeElapsed0hour.Value.PadLeft(2, '0'), hdnTimeElapsed0minute.Value.PadLeft(2, '0'));
            }
            entityConsultVisit.TimeElapsed1 = string.Format("{0}:{1}", hdnTimeElapsed1hour.Value.PadLeft(2, '0'), hdnTimeElapsed1minute.Value.PadLeft(2, '0'));
            entityConsultVisit.LastUpdatedBy = AppSession.UserLogin.UserID;
            if (cboVisitReason.Value != null)
            {
                entityConsultVisit.GCVisitReason = cboVisitReason.Value.ToString();
                if (entityConsultVisit.GCVisitReason == Constant.VisitReason.OTHER)
                    entityConsultVisit.VisitReason = txtVisitNotes.Text;
            }
            else
                entityConsultVisit.VisitReason = txtVisitNotes.Text;
            if (cboAdmissionCondition.Value != null)
                entityConsultVisit.GCAdmissionCondition = cboAdmissionCondition.Value.ToString();
            consultVisitDao.Update(entityConsultVisit);

            Registration entityRegistration = registrationDao.Get(entityConsultVisit.RegistrationID);
            if (cboTriage.Value == null)
                entityRegistration.GCTriage = "";
            else
                entityRegistration.GCTriage = cboTriage.Value.ToString();
            entityRegistration.EmergencyCaseDate = Helper.GetDatePickerValue(txtEmergencyCaseDate);
            entityRegistration.EmergencyCaseTime = txtEmergencyCaseTime.Text;
            entityRegistration.LastUpdatedBy = AppSession.UserLogin.UserID;
            registrationDao.Update(entityRegistration);
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            bool result = true;
            if (type == "save")
            {
                IDbContext ctx = DbFactory.Configure(true);
                try
                {
                    UpdateConsultVisitRegistration(ctx);
                    if (hdnIsNotAllowNurseFillChiefComplaint.Value == "0")
                    {
                        OnSaveAddEditPatientStatus(ctx, ref retval);
                    }
                    ctx.CommitTransaction();
                }
                catch (Exception ex)
                {
                    result = false;
                    errMessage = ex.Message;
                    ctx.RollBackTransaction();
                }
                finally
                {
                    ctx.Close();
                }
            }
            return result;
        }

        private void OnSaveAddEditPatientStatus(IDbContext ctx, ref string retval)
        {
            PatientDiagnosisDao patientDiagnosisDao = new PatientDiagnosisDao(ctx);
            ChiefComplaintDao chiefComplaintDao = new ChiefComplaintDao(ctx);
            PatientVisitNoteDao patientVisitNoteDao = new PatientVisitNoteDao(ctx);
            bool isChiefComplaintNull = false;
            bool isEntityEmergencyCaseNoteNull = false;
            bool isEntityPatientDiagnosisNull = false;

            ChiefComplaint entity = null;
            if (hdnID.Value != "")
            {
                entity = chiefComplaintDao.Get(Convert.ToInt32(hdnID.Value));
            }
            else
            {
                isChiefComplaintNull = true;
                entity = new ChiefComplaint();
                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                entity.CreatedBy = AppSession.UserLogin.UserID;
            }
            entity.LastUpdatedBy = AppSession.UserLogin.UserID;

            PatientDiagnosis patientDiagnosis = BusinessLayer.GetPatientDiagnosisList(string.Format("VisitID = {0} AND GCDiagnoseType = '{1}' AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, Constant.DiagnoseType.MAIN_DIAGNOSIS), ctx).FirstOrDefault();
            if (patientDiagnosis == null)
            {
                isEntityPatientDiagnosisNull = true;
                patientDiagnosis = new PatientDiagnosis();
                patientDiagnosis.VisitID = AppSession.RegisteredPatient.VisitID;
                patientDiagnosis.ParamedicID = AppSession.RegisteredPatient.ParamedicID;
                patientDiagnosis.DifferentialDate = DateTime.Now;
                patientDiagnosis.DifferentialTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                patientDiagnosis.GCDiagnoseType = Constant.DiagnoseType.MAIN_DIAGNOSIS;
                patientDiagnosis.GCDifferentialStatus = Constant.DifferentialDiagnosisStatus.UNDER_INVESTIGATION;
                patientDiagnosis.CreatedBy = AppSession.UserLogin.UserID;
            }
            patientDiagnosis.LastUpdatedBy = AppSession.UserLogin.UserID;

            PatientVisitNote entityEmergencyCaseNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = {0} AND GCPatientNoteType = '{1}'", AppSession.RegisteredPatient.VisitID, Constant.PatientVisitNotes.EMERGENCY_CASE_NOTE), ctx).FirstOrDefault();
            if (entityEmergencyCaseNote == null)
            {
                isEntityEmergencyCaseNoteNull = true;
                entityEmergencyCaseNote = new PatientVisitNote();
                entityEmergencyCaseNote.VisitID = AppSession.RegisteredPatient.VisitID;
                entityEmergencyCaseNote.GCPatientNoteType = Constant.PatientVisitNotes.EMERGENCY_CASE_NOTE;
                entityEmergencyCaseNote.CreatedBy = AppSession.UserLogin.UserID;
            }
            entityEmergencyCaseNote.LastUpdatedBy = AppSession.UserLogin.UserID;

            ControlToEntity(entity, entityEmergencyCaseNote, patientDiagnosis);

            if (isChiefComplaintNull)
            {

                chiefComplaintDao.Insert(entity);
                hdnID.Value = BusinessLayer.GetChiefComplaintMaxID(ctx).ToString();
            }
            else chiefComplaintDao.Update(entity);
            retval = hdnID.Value;

            if (isEntityEmergencyCaseNoteNull)
            {
                patientVisitNoteDao.Insert(entityEmergencyCaseNote);
            }
            else
            {
                patientVisitNoteDao.Update(entityEmergencyCaseNote);
            }

            if (isEntityPatientDiagnosisNull)
            {
                patientDiagnosisDao.Insert(patientDiagnosis);
            }
            else
            {
                patientDiagnosisDao.Update(patientDiagnosis);
            }
        }
    }
}
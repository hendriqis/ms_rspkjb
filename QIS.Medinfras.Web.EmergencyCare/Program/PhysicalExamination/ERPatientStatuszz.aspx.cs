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

namespace QIS.Medinfras.Web.EmergencyCare.Program
{
    public partial class ERPatientStatus : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EmergencyCare.PHYSICAL_EXAMINATION;
        }

        protected override void InitializeDataControl()
        {
            ctlToolbar.SetSelectedMenu(1);
            SetControlProperties();

            Helper.SetControlEntrySetting(txtEmergencyCaseDate, new ControlEntrySetting(true, true, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtEmergencyCaseTime, new ControlEntrySetting(true, true, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtChiefComplaint, new ControlEntrySetting(true, true, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtLocation, new ControlEntrySetting(true, true, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboOnset, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtOnset, new ControlEntrySetting(true, true, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboProvocation, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtProvocation, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboQuality, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtQuality, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboSeverity, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtSeverity, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboTime, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtTime, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboRelievedBy, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtRelievedBy, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboTriage, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtDiagnose, new ControlEntrySetting(true, true, false), "mpPatientStatus");

            ConsultVisit entityVisit = BusinessLayer.GetConsultVisit(AppSession.RegisteredPatient.VisitID);
            List<ChiefComplaint> lstChiefComplaint = BusinessLayer.GetChiefComplaintList(string.Format("VisitID = '{0}'", AppSession.RegisteredPatient.VisitID));
            List<PatientVisitNote> lstPatientVisitNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = '{0}' AND GCPatientNoteType = '{1}'", AppSession.RegisteredPatient.VisitID, Constant.PatientVisitNotes.EMERGENCY_CASE_NOTE));
            if (lstChiefComplaint.Count > 0 && lstPatientVisitNote.Count == 0)
            {
                ChiefComplaint entity = lstChiefComplaint.FirstOrDefault();
                PatientVisitNote entitypvn = new PatientVisitNote();
                hdnID.Value = entity.ID.ToString();
                EntityToControl(entity, entitypvn);
            }
            if (lstChiefComplaint.Count > 0 && lstPatientVisitNote.Count > 0)
            {
                ChiefComplaint entity = lstChiefComplaint.FirstOrDefault();
                PatientVisitNote entitypvn = lstPatientVisitNote.First();
                hdnID.Value = entity.ID.ToString();
                EntityToControl(entity, entitypvn);
            }
            else
            {
                hdnID.Value = "";
            }
            
            txtRegistrationDate.Text = entityVisit.ActualVisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtRegistrationTime.Text = entityVisit.ActualVisitTime;


            Registration entityRegistration = BusinessLayer.GetRegistration(entityVisit.RegistrationID);
            cboTriage.Value = entityRegistration.GCTriage;

            if (entityRegistration.EmergencyCaseDate == null || entityRegistration.EmergencyCaseTime == "")
            {
                txtEmergencyCaseDate.Text = entityVisit.ActualVisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtEmergencyCaseTime.Text = entityVisit.ActualVisitTime;
            }
            else
            {
                txtEmergencyCaseDate.Text = entityRegistration.EmergencyCaseDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtEmergencyCaseTime.Text = entityRegistration.EmergencyCaseTime;
            }

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

            cboVisitReason.Value = entityVisit.GCVisitReason;
            txtVisitNotes.Text = entityVisit.VisitReason;
            cboAdmissionCondition.Value = entityVisit.GCAdmissionCondition;

            PatientDiagnosis patientDiagnosis = BusinessLayer.GetPatientDiagnosisList(string.Format("VisitID = {0} AND GCDiagnoseType = '{1}' AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, Constant.DiagnoseType.MAIN_DIAGNOSIS)).FirstOrDefault();
            if (patientDiagnosis != null)
                txtDiagnose.Text = patientDiagnosis.DiagnosisText;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

        protected override void SetControlProperties()
        {
            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.ONSET, Constant.StandardCode.EXACERBATED, Constant.StandardCode.QUALITY,
                Constant.StandardCode.SEVERITY, Constant.StandardCode.COURSE_TIMING, Constant.StandardCode.RELIEVED_BY, Constant.StandardCode.TRIAGE, Constant.StandardCode.VISIT_REASON, Constant.StandardCode.ADMISSION_CONDITION);
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(filterExpression);
            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField(cboOnset, lstSc.Where(p => p.ParentID == Constant.StandardCode.ONSET || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboProvocation, lstSc.Where(p => p.ParentID == Constant.StandardCode.EXACERBATED || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboQuality, lstSc.Where(p => p.ParentID == Constant.StandardCode.QUALITY || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboSeverity, lstSc.Where(p => p.ParentID == Constant.StandardCode.SEVERITY || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboTime, lstSc.Where(p => p.ParentID == Constant.StandardCode.COURSE_TIMING || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboRelievedBy, lstSc.Where(p => p.ParentID == Constant.StandardCode.RELIEVED_BY || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboTriage, lstSc.Where(p => p.ParentID == Constant.StandardCode.TRIAGE || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboAdmissionCondition, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.ADMISSION_CONDITION || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboVisitReason, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.VISIT_REASON || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
        }

        private void EntityToControl(ChiefComplaint entity, PatientVisitNote entitypvn)
        {
            txtEmergencyCase.Text = entitypvn.NoteText;
            txtEmergencyCaseDate.Text = entity.ObservationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtEmergencyCaseTime.Text = entity.ObservationTime;
            txtChiefComplaint.Text = entity.ChiefComplaintText;
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
        }

        private void ControlToEntity(ChiefComplaint entity, PatientVisitNote entitypvn, PatientDiagnosis patientDiagnosis)
        {
            entitypvn.NoteDate = Helper.GetDatePickerValue(txtEmergencyCaseDate);
            entitypvn.NoteTime = txtEmergencyCaseTime.Text;
            entitypvn.NoteText = txtEmergencyCase.Text;

            entity.ChiefComplaintText = txtChiefComplaint.Text;
            entity.Location = txtLocation.Text;
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
            patientDiagnosis.DiagnosisText = txtDiagnose.Text;
        }

        private void UpdateConsultVisitRegistration(IDbContext ctx)
        {
            ConsultVisitDao consultVisitDao = new ConsultVisitDao(ctx);
            RegistrationDao registrationDao = new RegistrationDao(ctx);
            ConsultVisit entityConsultVisit = consultVisitDao.Get(AppSession.RegisteredPatient.VisitID);
            entityConsultVisit.ActualVisitDate = Helper.GetDatePickerValue(txtRegistrationDate);
            entityConsultVisit.ActualVisitTime = txtRegistrationTime.Text;
            entityConsultVisit.StartServiceDate = Helper.GetDatePickerValue(txtServiceDate);
            entityConsultVisit.StartServiceTime = txtServiceTime.Text;
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
            if (type == "save")
            {
                if (hdnID.Value != "")
                {
                    bool result = true;
                    IDbContext ctx = DbFactory.Configure(true);
                    PatientDiagnosisDao patientDiagnosisDao = new PatientDiagnosisDao(ctx);
                    ChiefComplaintDao chiefComplaintDao = new ChiefComplaintDao(ctx);
                    PatientVisitNoteDao patientVisitNoteDao = new PatientVisitNoteDao(ctx);
                    try
                    {
                        ChiefComplaint entity = chiefComplaintDao.Get(Convert.ToInt32(hdnID.Value));
                        PatientDiagnosis patientDiagnosis = BusinessLayer.GetPatientDiagnosisList(string.Format("VisitID = {0} AND GCDiagnoseType = '{1}' AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, Constant.DiagnoseType.MAIN_DIAGNOSIS), ctx).FirstOrDefault();
                        bool isEntityPatientDiagnosisNull = false;
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
                        }

                        PatientVisitNote entityEmergencyCaseNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = {0} AND GCPatientNoteType = '{1}'", AppSession.RegisteredPatient.VisitID, Constant.PatientVisitNotes.EMERGENCY_CASE_NOTE), ctx).FirstOrDefault();
                        bool isEntityEmergencyCaseNoteNull = false;
                        if (entityEmergencyCaseNote == null)
                        {
                            isEntityEmergencyCaseNoteNull = true;
                            entityEmergencyCaseNote = new PatientVisitNote();
                        }
                        ControlToEntity(entity, entityEmergencyCaseNote, patientDiagnosis);
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        chiefComplaintDao.Update(entity);

                        if (isEntityEmergencyCaseNoteNull)
                        {
                            entityEmergencyCaseNote.VisitID = AppSession.RegisteredPatient.VisitID;
                            entityEmergencyCaseNote.GCPatientNoteType = Constant.PatientVisitNotes.EMERGENCY_CASE_NOTE;
                            entityEmergencyCaseNote.CreatedBy = AppSession.UserLogin.UserID;
                            patientVisitNoteDao.Insert(entityEmergencyCaseNote);
                        }
                        else
                        {
                            entityEmergencyCaseNote.LastUpdatedBy = AppSession.UserLogin.UserID;
                            patientVisitNoteDao.Update(entityEmergencyCaseNote);
                        }
                        if (isEntityPatientDiagnosisNull)
                        {
                            patientDiagnosis.CreatedBy = AppSession.UserLogin.UserID;
                            patientDiagnosisDao.Insert(patientDiagnosis);
                        }
                        else
                        {
                            patientDiagnosis.LastUpdatedBy = AppSession.UserLogin.UserID;
                            patientDiagnosisDao.Update(patientDiagnosis);
                        }
                        UpdateConsultVisitRegistration(ctx);
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

                    return result;
                }
                else
                {
                    bool result = true;
                    IDbContext ctx = DbFactory.Configure(true);
                    PatientDiagnosisDao patientDiagnosisDao = new PatientDiagnosisDao(ctx);
                    ChiefComplaintDao chiefComplaintDao = new ChiefComplaintDao(ctx);
                    PatientVisitNoteDao patientVisitNoteDao = new PatientVisitNoteDao(ctx);
                    try
                    {
                        ChiefComplaint entity = new ChiefComplaint();
                        PatientVisitNote entitypvn = new PatientVisitNote();
                        PatientDiagnosis patientDiagnosis = BusinessLayer.GetPatientDiagnosisList(string.Format("VisitID = {0} AND GCDiagnoseType = '{1}' AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, Constant.DiagnoseType.MAIN_DIAGNOSIS), ctx).FirstOrDefault();
                        bool isEntityPatientDiagnosisNull = false;
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
                        }
                        ControlToEntity(entity, entitypvn, patientDiagnosis);
                        entity.VisitID = AppSession.RegisteredPatient.VisitID;
                        entity.CreatedBy = AppSession.UserLogin.UserID;
                        chiefComplaintDao.Insert(entity);
                        retval = BusinessLayer.GetChiefComplaintMaxID(ctx).ToString();

                        entitypvn.VisitID = AppSession.RegisteredPatient.VisitID;
                        entitypvn.GCPatientNoteType = Constant.PatientVisitNotes.EMERGENCY_CASE_NOTE;
                        entitypvn.CreatedBy = AppSession.UserLogin.UserID;
                        patientVisitNoteDao.Insert(entitypvn);

                        if (isEntityPatientDiagnosisNull)
                        {
                            patientDiagnosis.CreatedBy = AppSession.UserLogin.UserID;
                            patientDiagnosisDao.Insert(patientDiagnosis);
                        }
                        else
                        {
                            patientDiagnosis.LastUpdatedBy = AppSession.UserLogin.UserID;
                            patientDiagnosisDao.Update(patientDiagnosis);
                        }

                        UpdateConsultVisitRegistration(ctx);
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

                    return result;
                }
            }
            return true;
        }
    }
}
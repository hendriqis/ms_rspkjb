using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using System.Text;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class NursingAnamnesis : BasePagePatientPageList
    {
        string menuType = string.Empty;

        public override string OnGetMenuCode()
        {
            if (menuType == "fo")
            {
                if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
                    return Constant.MenuCode.Outpatient.FOLLOWUP_PATIENT_PAGE_NURSE_ANAMNESIS;
                else if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
                    return Constant.MenuCode.MedicalDiagnostic.FOLLOWUP_PATIENT_PAGE_NURSE_ANAMNESIS;
                else
                    return Constant.MenuCode.Outpatient.FOLLOWUP_PATIENT_PAGE_NURSE_ANAMNESIS;
            }
            else
            {
                if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
                    return Constant.MenuCode.Outpatient.PATIENT_PAGE_NURSE_ANAMNESIS;
                else if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
                    return Constant.MenuCode.MedicalDiagnostic.PATIENT_PAGE_NURSE_ANAMNESIS;
                else
                    return Constant.MenuCode.Outpatient.PATIENT_PAGE_NURSE_ANAMNESIS;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString["id"] != null)
            {
                string[] param = Page.Request.QueryString["id"].Split('|');
                if (param.Length > 1)
                {
                    hdnDepartmentID.Value = param[0];
                    menuType = param[1];
                }
                else
                {
                    hdnDepartmentID.Value = Page.Request.QueryString["id"];
                }
            }
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            vHealthcareServiceUnit hsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("DepartmentID = '{0}'", AppSession.RegisteredPatient.DepartmentID)).FirstOrDefault();
            hdnHealthcareServiceUnitID.Value = hsu.HealthcareServiceUnitID.ToString();

            ConsultVisit entityConsultVisit = BusinessLayer.GetConsultVisit(AppSession.RegisteredPatient.VisitID);
            hdnVisitTypeID.Value = entityConsultVisit.VisitTypeID.ToString();
            if (entityConsultVisit.VisitTypeID != null && entityConsultVisit.VisitTypeID != 0)
            {
                VisitType vt = BusinessLayer.GetVisitType(entityConsultVisit.VisitTypeID);
                txtVisitTypeCode.Text = vt.VisitTypeCode;
                txtVisitTypeName.Text = vt.VisitTypeName;
            }
            hdnRegistrationDate.Value = entityConsultVisit.ActualVisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnRegistrationTime.Value = entityConsultVisit.ActualVisitTime;

            txtServiceDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnTimeNow1.Value = txtServiceTime1.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT).Substring(0, 2);
            hdnTimeNow2.Value = txtServiceTime2.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT).Substring(3, 2);
        }

        protected override void SetControlProperties()
        {
            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format(
                                                    "GCParamedicMasterType IN ('{0}','{1}','{2}') AND ParamedicID = {3}",
                                                    Constant.ParamedicType.Nurse, Constant.ParamedicType.Bidan, Constant.ParamedicType.Nutritionist, paramedicID));
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");
            hdnParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();

            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.ONSET, Constant.StandardCode.EXACERBATED, Constant.StandardCode.QUALITY,
                Constant.StandardCode.SEVERITY, Constant.StandardCode.COURSE_TIMING, Constant.StandardCode.RELIEVED_BY, Constant.StandardCode.VISIT_REASON);
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(filterExpression);
            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboOnset, lstSc.Where(p => p.ParentID == Constant.StandardCode.ONSET || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboProvocation, lstSc.Where(p => p.ParentID == Constant.StandardCode.EXACERBATED || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboQuality, lstSc.Where(p => p.ParentID == Constant.StandardCode.QUALITY || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboSeverity, lstSc.Where(p => p.ParentID == Constant.StandardCode.SEVERITY || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboTime, lstSc.Where(p => p.ParentID == Constant.StandardCode.COURSE_TIMING || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboRelievedBy, lstSc.Where(p => p.ParentID == Constant.StandardCode.RELIEVED_BY || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

            vNurseChiefComplaint entity = BusinessLayer.GetvNurseChiefComplaintList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            PatientVisitNote oPatientVisitNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = '{0}' AND GCPatientNoteType = '{1}'", AppSession.RegisteredPatient.VisitID, Constant.PatientVisitNotes.NURSE_INITIAL_ASSESSMENT)).FirstOrDefault();

            if (oPatientVisitNote != null)
                hdnPatientVisitNoteID.Value = oPatientVisitNote.ID.ToString();
            else
                hdnPatientVisitNoteID.Value = "0";

            EntityToControl(entity);

            bool isEnabled = true;
            if (entity != null)
            {
                isEnabled = entity.ParamedicID == AppSession.UserLogin.ParamedicID;
            }

            Helper.SetControlEntrySetting(txtChiefComplaint, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtHPISummary, new ControlEntrySetting(isEnabled, isEnabled, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(chkAutoAnamnesis, new ControlEntrySetting(isEnabled, isEnabled, false, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(chkAlloAnamnesis, new ControlEntrySetting(isEnabled, isEnabled, false, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtLocation, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboOnset, new ControlEntrySetting(isEnabled, isEnabled, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtOnset, new ControlEntrySetting(isEnabled, isEnabled, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboProvocation, new ControlEntrySetting(isEnabled, isEnabled, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtProvocation, new ControlEntrySetting(isEnabled, isEnabled, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboQuality, new ControlEntrySetting(isEnabled, isEnabled, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboSeverity, new ControlEntrySetting(isEnabled, isEnabled, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtSeverity, new ControlEntrySetting(isEnabled, isEnabled, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboTime, new ControlEntrySetting(isEnabled, isEnabled, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtTime, new ControlEntrySetting(isEnabled, isEnabled, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboRelievedBy, new ControlEntrySetting(isEnabled, isEnabled, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtRelievedBy, new ControlEntrySetting(isEnabled, isEnabled, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(hdnDiagnoseID, new ControlEntrySetting(isEnabled, isEnabled, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtDiagnose, new ControlEntrySetting(isEnabled, isEnabled, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtServiceDate, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtServiceTime1, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtServiceTime2, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");


            if (AppSession.UserLogin.ParamedicID != 0 && AppSession.UserLogin.ParamedicID != null)
            {
                cboParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
                cboParamedicID.Enabled = false;
            }
            else
            {
                cboParamedicID.SelectedIndex = 0;
            }

            if (AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Nurse
                    || AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Bidan
                    || AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Nutritionist)
            {
                int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                if (userLoginParamedic != 0 && userLoginParamedic != null)
                {
                    Helper.SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(false, false, true, userLoginParamedic), "mpPatientStatus");
                }
                else
                {
                    Helper.SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
                }
            }
            else
            {
                Helper.SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            }

            hdnIsNotAllowNurseFillChiefComplaint.Value = isEnabled ? "0" : "1";
        }

        private void EntityToControl(vNurseChiefComplaint entity)
        {
            hdnDepartmentID.Value = entity.DepartmentID;
            if (entity.ChiefComplaintID == 0)
            {
                hdnID.Value = "";
            }
            else hdnID.Value = entity.ChiefComplaintID.ToString();

            if (entity.StartServiceDate != null && !string.IsNullOrEmpty(entity.StartServiceTime))
            {
                txtServiceDate.Text = entity.StartServiceDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtServiceTime1.Text = entity.StartServiceTime.Substring(0, 2);
                txtServiceTime2.Text = entity.StartServiceTime.Substring(3, 2);
            }

            #region table sebelah kiri
            #region NurseChiefComplaint
            txtChiefComplaint.Text = entity.NurseChiefComplaintText;
            txtHPISummary.Text = entity.HPISummary;
            //cboParamedicID.Value = entity.ParamedicID.ToString();
            chkAutoAnamnesis.Checked = entity.IsAutoAnamnesis;
            chkAlloAnamnesis.Checked = entity.IsAlloAnamnesis;
            chkIsPatientAllergyExists.Checked = !entity.IsPatientAllergyExists;
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

            txtDiagnose.Text = entity.DiagnosisText;
            #endregion
        }

        private void ControlToEntity(NurseChiefComplaint entity, PatientDiagnosis patientDiagnosis)
        {
            #region Chief Complaint
            entity.NurseChiefComplaintText = txtChiefComplaint.Text;
            entity.HPISummary = txtHPISummary.Text;
            if (cboParamedicID.Value != null)
            {
                entity.ParamedicID = Convert.ToInt32(cboParamedicID.Value); 
            }
            entity.Location = txtLocation.Text;
            entity.IsAutoAnamnesis = Convert.ToBoolean(chkAutoAnamnesis.Checked);
            entity.IsAlloAnamnesis = Convert.ToBoolean(chkAlloAnamnesis.Checked);
            entity.IsPatientAllergyExists = Convert.ToBoolean(!chkIsPatientAllergyExists.Checked);

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
            entity.ChiefComplaintDate = Helper.GetDatePickerValue(txtServiceDate);
            entity.ChiefComplaintTime = string.Format("{0}:{1}", txtServiceTime1.Text, txtServiceTime2.Text);
            #endregion

            #region Patient Diagnosis
            patientDiagnosis.DiagnosisText = txtDiagnose.Text;
            if (!hdnDiagnoseID.Equals(string.Empty)) patientDiagnosis.DiagnoseID = Convert.ToString(hdnDiagnoseID.Value);
            #endregion
        }

        private void UpdateConsultVisitRegistration(IDbContext ctx)
        {
            ConsultVisitDao consultVisitDao = new ConsultVisitDao(ctx);
            ConsultVisit entityConsultVisit = consultVisitDao.Get(AppSession.RegisteredPatient.VisitID);
            entityConsultVisit.VisitTypeID = Convert.ToInt32(hdnVisitTypeID.Value);
            entityConsultVisit.StartServiceDate = Helper.GetDatePickerValue(txtServiceDate);
            entityConsultVisit.StartServiceTime = string.Format("{0}:{1}", txtServiceTime1.Text, txtServiceTime2.Text);
            entityConsultVisit.TimeElapsed1 = string.Format("{0}:{1}", hdnTimeElapsed1hour.Value.PadLeft(2, '0'), hdnTimeElapsed1minute.Value.PadLeft(2, '0'));
            entityConsultVisit.LastUpdatedBy = AppSession.UserLogin.UserID;
            consultVisitDao.Update(entityConsultVisit);
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            if (type == "save")
            {
                IDbContext ctx = DbFactory.Configure(true);
                PatientVisitNoteDao patientVisitNoteDao = new PatientVisitNoteDao(ctx);
                try
                {
                    PatientVisitNote entityNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = {0} AND GCPatientNoteType = '{1}'", AppSession.RegisteredPatient.VisitID, Constant.PatientVisitNotes.NURSE_INITIAL_ASSESSMENT), ctx).FirstOrDefault();
                    bool isEntityNoteNull = false;
                    if (entityNote == null)
                    {
                        isEntityNoteNull = true;
                        entityNote = new PatientVisitNote();
                    }
                    ControlToEntity(entityNote);

                    if (isEntityNoteNull)
                    {
                        entityNote.VisitID = AppSession.RegisteredPatient.VisitID;
                        entityNote.ParamedicID = AppSession.UserLogin.ParamedicID;
                        entityNote.GCPatientNoteType = Constant.PatientVisitNotes.NURSE_INITIAL_ASSESSMENT;
                        entityNote.CreatedBy = AppSession.UserLogin.UserID;
                        hdnPatientVisitNoteID.Value = patientVisitNoteDao.InsertReturnPrimaryKeyID(entityNote).ToString();
                    }
                    else
                    {
                        entityNote.ParamedicID = AppSession.UserLogin.ParamedicID;
                        entityNote.LastUpdatedBy = AppSession.UserLogin.UserID;
                        patientVisitNoteDao.Update(entityNote);

                        hdnPatientVisitNoteID.Value = entityNote.ID.ToString();
                    }

                    UpdateConsultVisitRegistration(ctx);
                    if (hdnIsNotAllowNurseFillChiefComplaint.Value == "1")
                    {
                        OnSaveAddEditPatientStatus(ctx, ref errMessage);
                    }
                    ctx.CommitTransaction();
                }
                catch (Exception ex)
                {
                    result = false;
                    errMessage = ex.Message;
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }
                finally
                {
                    ctx.Close();
                }
            }
            return result;
        }

        private void ControlToEntity(PatientVisitNote entityNote)
        {
            string soapNote = GenerateSOAPText();

            string subjectiveText = soapNote;
            string objectiveText = string.Empty;
            string assessmentText = string.Empty;
            string planningText = string.Empty;

            entityNote.NoteText = soapNote;
            entityNote.SubjectiveText = soapNote;
            entityNote.NoteDate = Helper.GetDatePickerValue(txtServiceDate);
            entityNote.NoteTime = string.Format("{0}:{1}", txtServiceTime1.Text, txtServiceTime2.Text);
        }

        private string GenerateSOAPText()
        {
            StringBuilder sbNotes = new StringBuilder();
            sbNotes.AppendLine("Anamnesa Perawat :");
            sbNotes.AppendLine("-".PadRight(15, '-'));
            sbNotes.AppendLine(string.Format(" {0}   ", txtChiefComplaint.Text));
            if (!string.IsNullOrEmpty(txtHPISummary.Text))
            {
                sbNotes.AppendLine(string.Format("RPS  : "));
                sbNotes.AppendLine(string.Format(" {0}   ", txtHPISummary.Text));
            }
            return sbNotes.ToString();
        }

        private void OnSaveAddEditPatientStatus(IDbContext ctx, ref string errMessage)
        {
            PatientDiagnosisDao patientDiagnosisDao = new PatientDiagnosisDao(ctx);
            NurseChiefComplaintDao chiefComplaintDao = new NurseChiefComplaintDao(ctx);
            PatientDao patientDao = new PatientDao(ctx);

            bool isChiefComplaintNull = false;
            bool isEntityPatientDiagnosisNull = false;
            
            List<NurseChiefComplaint> lstNCC = BusinessLayer.GetNurseChiefComplaintList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID), ctx);
            
            NurseChiefComplaint entity = null;
            if (lstNCC.Count() > 0)
            {
                hdnID.Value = lstNCC.FirstOrDefault().ID.ToString();
                entity = chiefComplaintDao.Get(Convert.ToInt32(hdnID.Value));
            }
            else
            {
                isChiefComplaintNull = true;
                entity = new NurseChiefComplaint();
                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                entity.CreatedBy = AppSession.UserLogin.UserID;
            }
            entity.LastUpdatedBy = AppSession.UserLogin.UserID;

            PatientDiagnosis patientDiagnosis = BusinessLayer.GetPatientDiagnosisList(string.Format("VisitID = {0} AND GCDiagnoseType = '{1}' AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, Constant.DiagnoseType.EARLY_DIAGNOSIS), ctx).FirstOrDefault();
            if (patientDiagnosis == null)
            {
                isEntityPatientDiagnosisNull = true;
                patientDiagnosis = new PatientDiagnosis();
                patientDiagnosis.VisitID = AppSession.RegisteredPatient.VisitID;
                patientDiagnosis.ParamedicID = AppSession.RegisteredPatient.ParamedicID;
                patientDiagnosis.DifferentialDate = DateTime.Now;
                patientDiagnosis.DifferentialTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                patientDiagnosis.GCDiagnoseType = Constant.DiagnoseType.EARLY_DIAGNOSIS;
                patientDiagnosis.GCDifferentialStatus = Constant.DifferentialDiagnosisStatus.UNDER_INVESTIGATION;
                patientDiagnosis.CreatedBy = AppSession.UserLogin.UserID;
            }
            patientDiagnosis.LastUpdatedBy = AppSession.UserLogin.UserID;

            ControlToEntity(entity, patientDiagnosis);

            if (isChiefComplaintNull)
            {
                hdnID.Value = chiefComplaintDao.InsertReturnPrimaryKeyID(entity).ToString();
            }
            else
            {
                chiefComplaintDao.Update(entity);
            }

            if (isEntityPatientDiagnosisNull)
            {
                if ((patientDiagnosis.DiagnoseID != "" && patientDiagnosis.DiagnoseID != null) || (patientDiagnosis.DiagnosisText.Trim() != "" && patientDiagnosis.DiagnosisText.Trim() != null))
                {
                    patientDiagnosisDao.Insert(patientDiagnosis);
                }
            }
            else
            {
                patientDiagnosisDao.Update(patientDiagnosis);
            }

            #region Patient Status
            //Patient oPatient = patientDao.Get(AppSession.RegisteredPatient.MRN);
            //if (oPatient != null)
            //{
            //    oPatient.IsBreastFeeding = chkIsBreastFeeding.Checked;
            //    patientDao.Update(oPatient);
            //} 
            #endregion
        }
    }
}
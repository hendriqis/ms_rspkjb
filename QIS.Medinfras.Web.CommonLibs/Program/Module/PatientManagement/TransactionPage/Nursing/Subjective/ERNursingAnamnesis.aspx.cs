using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ERNursingAnamnesis : BasePagePatientPageList
    {
        string menuType = string.Empty;
        public override string OnGetMenuCode()
        {
            if (menuType == "fo")
            {
                return Constant.MenuCode.EmergencyCare.FOLLOWUP_PATIENT_PAGE_NURSE_ANAMNESIS;
            }
            else
            {
                return Constant.MenuCode.EmergencyCare.PATIENT_PAGE_NURSE_ANAMNESIS;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            menuType = Page.Request.QueryString["id"];

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            vHealthcareServiceUnit hsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("DepartmentID = '{0}'", AppSession.RegisteredPatient.DepartmentID)).FirstOrDefault();
            hdnHealthcareServiceUnitID.Value = hsu.HealthcareServiceUnitID.ToString();
            hdnParamedicID.Value = AppSession.ParamedicID.ToString();

            ConsultVisit entityConsultVisit = BusinessLayer.GetConsultVisit(AppSession.RegisteredPatient.VisitID);
            hdnVisitTypeID.Value = entityConsultVisit.VisitTypeID.ToString();
            hdnRegistrationID.Value = entityConsultVisit.RegistrationID.ToString();
            if (entityConsultVisit.VisitTypeID != null && entityConsultVisit.VisitTypeID != 0)
            {
                VisitType vt = BusinessLayer.GetVisitType(entityConsultVisit.VisitTypeID);
                txtVisitTypeCode.Text = vt.VisitTypeCode;
                txtVisitTypeName.Text = vt.VisitTypeName;
            }

            List<SettingParameterDt> lstSettingParameter = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode = ('{0}')", Constant.SettingParameter.IS_PENDAFTARAN_DENGAN_RUANG));
            String IsEmergencyUsingRoom = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_PENDAFTARAN_DENGAN_RUANG).ParameterValue;

            if (entityConsultVisit.BedID == null)
            {
                if (IsEmergencyUsingRoom == "0")
                {
                    trBedQuickPicks.Style.Add("display", "none");
                    trRoom.Style.Add("display", "none");
                    trBed.Style.Add("display", "none");
                    trClass.Style.Add("display", "none");
                    trChargeClass.Style.Add("display", "none");
                }
                else
                {
                    trBedQuickPicks.Style.Add("display", "table-row");
                    trRoom.Style.Add("display", "table-row");
                    trBed.Style.Add("display", "table-row");
                    trClass.Style.Add("display", "table-row");
                    trChargeClass.Style.Add("display", "table-row");
                }
            }
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        protected override void SetControlProperties()
        {
            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}') AND IsActive = 1 AND IsDeleted = 0",
                Constant.StandardCode.ONSET, Constant.StandardCode.EXACERBATED, Constant.StandardCode.QUALITY,
                Constant.StandardCode.SEVERITY, Constant.StandardCode.COURSE_TIMING, Constant.StandardCode.RELIEVED_BY,
                Constant.StandardCode.TRIAGE, Constant.StandardCode.VISIT_REASON, Constant.StandardCode.ADMISSION_CONDITION,
                Constant.StandardCode.AIRWAY, Constant.StandardCode.BREATHING, Constant.StandardCode.CIRCULATION,
                Constant.StandardCode.DISABILITY, Constant.StandardCode.EXPOSURE, Constant.StandardCode.ADMISSION_ROUTE);
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(filterExpression);
            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboOnset, lstSc.Where(p => p.ParentID == Constant.StandardCode.ONSET || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboProvocation, lstSc.Where(p => p.ParentID == Constant.StandardCode.EXACERBATED || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboQuality, lstSc.Where(p => p.ParentID == Constant.StandardCode.QUALITY || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboSeverity, lstSc.Where(p => p.ParentID == Constant.StandardCode.SEVERITY || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboTime, lstSc.Where(p => p.ParentID == Constant.StandardCode.COURSE_TIMING || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboRelievedBy, lstSc.Where(p => p.ParentID == Constant.StandardCode.RELIEVED_BY || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboTriage, lstSc.Where(p => p.ParentID == Constant.StandardCode.TRIAGE || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboAirway, lstSc.Where(p => p.ParentID == Constant.StandardCode.AIRWAY || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboBreathing, lstSc.Where(p => p.ParentID == Constant.StandardCode.BREATHING || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboCirculation, lstSc.Where(p => p.ParentID == Constant.StandardCode.CIRCULATION || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDisability, lstSc.Where(p => p.ParentID == Constant.StandardCode.DISABILITY || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboExposure, lstSc.Where(p => p.ParentID == Constant.StandardCode.EXPOSURE || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboAdmissionRoute, lstSc.Where(p => p.ParentID == Constant.StandardCode.ADMISSION_ROUTE || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboAdmissionCondition, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.ADMISSION_CONDITION || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboVisitReason, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.VISIT_REASON || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format(
                                                    "GCParamedicMasterType IN ('{0}','{1}','{2}') AND ParamedicID = {3}",
                                                    Constant.ParamedicType.Nurse, Constant.ParamedicType.Bidan, Constant.ParamedicType.Nutritionist, paramedicID));
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");

            if (AppSession.UserLogin.ParamedicID != 0 && AppSession.UserLogin.ParamedicID != null)
            {
                cboParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
                cboParamedicID.Enabled = false;
            }
            else
            {
                cboParamedicID.SelectedIndex = 0;
            }

            vNurseChiefComplaint entity = BusinessLayer.GetvNurseChiefComplaintList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            vConsultVisit entityVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            EntityToControl(entity, entityVisit);

            bool isEnabled = true;
            if (entity != null)
            {
                isEnabled = entity.ParamedicID == AppSession.UserLogin.ParamedicID;
            }

            Helper.SetControlEntrySetting(txtChiefComplaint, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
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
            Helper.SetControlEntrySetting(cboVisitReason, new ControlEntrySetting(isEnabled, isEnabled, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtVisitNotes, new ControlEntrySetting(isEnabled, isEnabled, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtEmergencyCaseDate, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtEmergencyCaseTime1, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtEmergencyCaseTime2, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtRegistrationDate, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtRegistrationTime, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtRegistrationDateDiff, new ControlEntrySetting(isEnabled, isEnabled, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtServiceDate, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtServiceTime1, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtServiceTime2, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtServiceDateDiff, new ControlEntrySetting(isEnabled, isEnabled, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboTriage, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboAirway, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboBreathing, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboCirculation, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboDisability, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboExposure, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboAdmissionRoute, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboAdmissionCondition, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtEmergencyCase, new ControlEntrySetting(isEnabled, isEnabled, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(chkIsFastTrack, new ControlEntrySetting(isEnabled, isEnabled, false), "mpPatientStatus");

            if (AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Nurse
                    || AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Bidan
                    || AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Nutritionist)
            {
                //int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                Helper.SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(false, false, true, AppSession.UserLogin.ParamedicID), "mpPatientStatus");
            }
            else
            {
                Helper.SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            }

            hdnIsNotAllowNurseFillChiefComplaint.Value = isEnabled ? "0" : "1";
        }

        private void EntityToControl(vNurseChiefComplaint entity, vConsultVisit entityVisit)
        {
            hdnDepartmentID.Value = entity.DepartmentID;
            if (entity.ChiefComplaintID == 0)
            {
                hdnID.Value = "";
            }
            else hdnID.Value = entity.ChiefComplaintID.ToString();

            #region table sebelah kanan

            txtRegistrationDate.Text = txtEmergencyCaseDate.Text = txtServiceDate.Text = entity.ActualVisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtRegistrationTime.Text = entity.ActualVisitTime;
            hdnTimeNow1.Value = txtEmergencyCaseTime1.Text = txtServiceTime1.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT).Substring(0, 2);
            hdnTimeNow2.Value = txtEmergencyCaseTime2.Text = txtServiceTime2.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT).Substring(3, 2);

            if (entity.EmergencyCaseDate != null && !string.IsNullOrEmpty(entity.EmergencyCaseTime))
            {
                txtEmergencyCaseDate.Text = entity.EmergencyCaseDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtEmergencyCaseTime1.Text = entity.EmergencyCaseTime.Substring(0, 2);
                txtEmergencyCaseTime2.Text = entity.EmergencyCaseTime.Substring(3, 2);
            }

            if (entity.StartServiceDate != null && !string.IsNullOrEmpty(entity.StartServiceTime))
            {
                txtServiceDate.Text = entity.StartServiceDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtServiceTime1.Text = entity.StartServiceTime.Substring(0, 2);
                txtServiceTime2.Text = entity.StartServiceTime.Substring(3, 2);
            }
            txtEmergencyCase.Text = entity.EmergencyCaseLocation;

            cboTriage.Value = entity.GCTriage;
            cboAdmissionRoute.Value = entity.GCAdmissionRoute;
            cboAirway.Value = entity.GCAirway;
            cboBreathing.Value = entity.GCBreathing;
            cboCirculation.Value = entity.GCCirculation;
            cboDisability.Value = entity.GCDisability;
            cboExposure.Value = entity.GCExposure;
            cboAdmissionCondition.Value = entity.GCAdmissionCondition;
            //txtEmergencyCase.Text = entity.NoteText;
            hdnClassID.Value = Convert.ToString(entityVisit.ClassID);
            txtClassCode.Text = entityVisit.ClassCode;
            txtClassName.Text = entityVisit.ClassName;
            hdnChargeClassID.Value = Convert.ToString(entityVisit.ChargeClassID);
            txtChargeClassCode.Text = entityVisit.ChargeClassCode;
            txtChargeClassName.Text = entityVisit.ChargeClassName;
            hdnRoomID.Value = Convert.ToString(entityVisit.RoomID);
            txtRoomCode.Text = entityVisit.RoomCode;
            txtRoomName.Text = entityVisit.RoomName;
            hdnBedID.Value = Convert.ToString(entityVisit.BedID);
            txtBedCode.Text = entityVisit.BedCode;
            #endregion

            #region table sebelah kiri

            #region Chief Complaint

            txtChiefComplaint.Text = entity.NurseChiefComplaintText;
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
            txtMedicalHistory.Text = entity.MedicalHistory;
            txtMedicationHistory.Text = entity.MedicationHistory;
            chkIsFastTrack.Checked = entity.IsFastTrack;
            #endregion

            cboVisitReason.Value = entity.GCVisitReason;
            txtVisitNotes.Text = entity.VisitReason;

            #endregion
        }

        private void ControlToEntity(NurseChiefComplaint entity, PatientVisitNote entitypvn)
        {
            #region Chief Complaint
            entity.NurseChiefComplaintText = txtChiefComplaint.Text;
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
            entity.MedicalHistory = txtMedicalHistory.Text;
            entity.MedicationHistory = txtMedicationHistory.Text;
            #endregion

            #region Patient Visit Note
            string soapNote = GenerateSOAPText();

            string subjectiveText = string.Empty;
            string objectiveText = string.Empty;
            string assessmentText = string.Empty;

            entitypvn.NoteText = soapNote;
            entitypvn.NoteDate = Helper.GetDatePickerValue(txtEmergencyCaseDate);
            entitypvn.NoteTime = string.Format("{0}:{1}", txtEmergencyCaseTime1.Text, txtEmergencyCaseTime2.Text);
            entitypvn.GCPatientNoteType = Constant.PatientVisitNotes.NURSE_INITIAL_ASSESSMENT;
            entitypvn.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
            #endregion
        }

        private string GenerateSOAPText()
        {
            StringBuilder sbNotes = new StringBuilder();

            sbNotes.AppendLine(string.Format("Anamnesa Perawat  : "));
            sbNotes.AppendLine(string.Format(" {0}   ", txtChiefComplaint.Text));
            sbNotes.AppendLine(" ");

            if (cboAirway.Value.ToString() != "")
            {
                StandardCode sc = BusinessLayer.GetStandardCode(Convert.ToString(cboAirway.Value));
                sbNotes.AppendLine(string.Format("Air Way  : "));
                sbNotes.AppendLine(string.Format(" {0}   ", sc.StandardCodeName));
            }

            if (cboBreathing.Value.ToString() != "")
            {
                StandardCode sc = BusinessLayer.GetStandardCode(Convert.ToString(cboBreathing.Value));
                sbNotes.AppendLine(string.Format("Breathing  : "));
                sbNotes.AppendLine(string.Format(" {0}   ", sc.StandardCodeName));
            }

            if (cboCirculation.Value.ToString() != "")
            {
                StandardCode sc = BusinessLayer.GetStandardCode(Convert.ToString(cboCirculation.Value));
                sbNotes.AppendLine(string.Format("Circulation  : "));
                sbNotes.AppendLine(string.Format(" {0}   ", sc.StandardCodeName));
            }

            if (cboDisability.Value.ToString() != "")
            {
                StandardCode sc = BusinessLayer.GetStandardCode(Convert.ToString(cboDisability.Value));
                sbNotes.AppendLine(string.Format("Disability  : "));
                sbNotes.AppendLine(string.Format(" {0}   ", sc.StandardCodeName));
            }

            if (cboExposure.Value.ToString() != "")
            {
                StandardCode sc = BusinessLayer.GetStandardCode(Convert.ToString(cboExposure.Value));
                sbNotes.AppendLine(string.Format("Exposure  : "));
                sbNotes.AppendLine(string.Format(" {0}   ", sc.StandardCodeName));
            }

            sbNotes.AppendLine(" ");

            if (txtMedicalHistory.Text != "")
            {
                sbNotes.AppendLine(string.Format("Riwayat Terdahulu  : "));
                sbNotes.AppendLine(string.Format(" {0}   ", txtMedicalHistory.Text));
            }

            if (txtMedicationHistory.Text != "")
            {
                sbNotes.AppendLine(string.Format("Riwayat Pengobatan  : "));
                sbNotes.AppendLine(string.Format(" {0}   ", txtMedicationHistory.Text));
            }
            return sbNotes.ToString();
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
            entityConsultVisit.StartServiceTime = string.Format("{0}:{1}", txtServiceTime1.Text, txtServiceTime2.Text);
            if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.EMERGENCY)
            {
                entityConsultVisit.TimeElapsed0 = string.Format("{0}:{1}", hdnTimeElapsed0hour.Value.PadLeft(2, '0'), hdnTimeElapsed0minute.Value.PadLeft(2, '0'));
            }
            entityConsultVisit.TimeElapsed1 = string.Format("{0}:{1}", hdnTimeElapsed1hour.Value.PadLeft(2, '0'), hdnTimeElapsed1minute.Value.PadLeft(2, '0'));
            entityConsultVisit.LastUpdatedBy = AppSession.UserLogin.UserID;
            if (cboVisitReason.Value != null)
            {
                entityConsultVisit.GCVisitReason = cboVisitReason.Value.ToString();
                if (entityConsultVisit.GCVisitReason == Constant.VisitReason.OTHER || entityConsultVisit.GCVisitReason == Constant.VisitReason.ACCIDENT)
                    entityConsultVisit.VisitReason = txtVisitNotes.Text;
            }
            else
                entityConsultVisit.VisitReason = txtVisitNotes.Text;
            if (cboAdmissionCondition.Value != null)
                entityConsultVisit.GCAdmissionCondition = cboAdmissionCondition.Value.ToString();
            consultVisitDao.Update(entityConsultVisit);

            Registration entityRegistration = registrationDao.Get(entityConsultVisit.RegistrationID);
            if (cboTriage.Value != null)
                entityRegistration.GCTriage = cboTriage.Value.ToString();
            else
                entityRegistration.GCTriage = "";
            if (cboAdmissionRoute.Value != null)
                entityRegistration.GCAdmissionRoute = cboAdmissionRoute.Value.ToString();
            else
                entityRegistration.GCAdmissionRoute = "";
            if (cboAirway.Value != null)
                entityRegistration.GCAirway = cboAirway.Value.ToString();
            else
                entityRegistration.GCAirway = "";
            if (cboBreathing.Value != null)
                entityRegistration.GCBreathing = cboBreathing.Value.ToString();
            else
                entityRegistration.GCBreathing = "";
            if (cboCirculation.Value != null)
                entityRegistration.GCCirculation = cboCirculation.Value.ToString();
            else
                entityRegistration.GCCirculation = "";
            if (cboDisability.Value != null)
                entityRegistration.GCDisability = cboDisability.Value.ToString();
            else
                entityRegistration.GCDisability = "";
            if (cboExposure.Value != null)
                entityRegistration.GCExposure = cboExposure.Value.ToString();
            else
                entityRegistration.GCExposure = "";
            entityRegistration.IsFastTrack = chkIsFastTrack.Checked;
            entityRegistration.EmergencyCaseDate = Helper.GetDatePickerValue(txtEmergencyCaseDate);
            entityRegistration.EmergencyCaseTime = string.Format("{0}:{1}", txtEmergencyCaseTime1.Text, txtEmergencyCaseTime2.Text);
            entityRegistration.EmergencyCaseLocation = txtEmergencyCase.Text;
            entityRegistration.LastUpdatedBy = AppSession.UserLogin.UserID;
            registrationDao.Update(entityRegistration);
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            if (type == "save")
            {
                IDbContext ctx = DbFactory.Configure(true);
                try
                {
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

        private void OnSaveAddEditPatientStatus(IDbContext ctx, ref string errMessage)
        {
            NurseChiefComplaintDao chiefComplaintDao = new NurseChiefComplaintDao(ctx);
            PatientVisitNoteDao patientVisitNoteDao = new PatientVisitNoteDao(ctx);
            ConsultVisitDao patientVisitDao = new ConsultVisitDao(ctx);
            BedDao BedDao = new BedDao(ctx);
            bool isChiefComplaintNull = false;
            bool isEntityEmergencyCaseNoteNull = false;

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

            PatientVisitNote entityEmergencyCaseNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = {0} AND GCPatientNoteType = '{1}'", AppSession.RegisteredPatient.VisitID, Constant.PatientVisitNotes.NURSE_INITIAL_ASSESSMENT), ctx).FirstOrDefault();
            if (entityEmergencyCaseNote == null)
            {
                isEntityEmergencyCaseNoteNull = true;
                entityEmergencyCaseNote = new PatientVisitNote();
                entityEmergencyCaseNote.VisitID = AppSession.RegisteredPatient.VisitID;
                entityEmergencyCaseNote.GCPatientNoteType = Constant.PatientVisitNotes.NURSE_INITIAL_ASSESSMENT;
                entityEmergencyCaseNote.CreatedBy = AppSession.UserLogin.UserID;
            }
            entityEmergencyCaseNote.LastUpdatedBy = AppSession.UserLogin.UserID;
            
            ControlToEntity(entity, entityEmergencyCaseNote);

            if (isChiefComplaintNull)
            {
                hdnID.Value = chiefComplaintDao.InsertReturnPrimaryKeyID(entity).ToString();
                //hdnID.Value = BusinessLayer.GetChiefComplaintMaxID(ctx).ToString();
            }
            else
            {
                chiefComplaintDao.Update(entity);
            }

            if (isEntityEmergencyCaseNoteNull)
            {
                patientVisitNoteDao.Insert(entityEmergencyCaseNote);
            }
            else
            {
                patientVisitNoteDao.Update(entityEmergencyCaseNote);
            }

            ConsultVisit entityVisit = BusinessLayer.GetConsultVisitList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID), ctx).FirstOrDefault();

            if (hdnBedID.Value != "" && hdnBedID.Value != null & hdnBedID.Value != "0")
            {
                Bed entityBed = BusinessLayer.GetBedList(string.Format("BedID = {0}", hdnBedID.Value), ctx).FirstOrDefault();

                #region Consult Visit
                entityVisit.ClassID = Convert.ToInt32(hdnClassID.Value);
                entityVisit.ChargeClassID = Convert.ToInt32(hdnChargeClassID.Value);
                entityVisit.RoomID = Convert.ToInt32(hdnRoomID.Value);
                entityVisit.BedID = Convert.ToInt32(hdnBedID.Value);

                entityBed.RegistrationID = AppSession.RegisteredPatient.RegistrationID;
                entityBed.GCBedStatus = Constant.BedStatus.OCCUPIED;
                #endregion

                BedDao.Update(entityBed);
                patientVisitDao.Update(entityVisit);
            }
        }
    }
}
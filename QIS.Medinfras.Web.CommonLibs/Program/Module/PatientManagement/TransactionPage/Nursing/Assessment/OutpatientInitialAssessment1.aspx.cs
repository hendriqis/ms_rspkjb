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
    public partial class OutpatientInitialAssessment1 : BasePagePatientPageList
    {
        string deptType = string.Empty;
        string menuType = string.Empty;
        protected int gridVitalSignPageCount = 1;
        protected int gridDiagnosisPageCount = 1;
        protected int gridEducationPageCount = 1;
        protected List<vVitalSignDt> lstVitalSignDt = null;
        protected List<vPatientEducationDt> lstPatientEducationDt = null;
        protected static string _visitNoteID;
        protected static string _linkedVisitID;

        public override string OnGetMenuCode()
        {
            if (menuType == "fo")
            {
                if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
                    return Constant.MenuCode.Outpatient.FOLLOWUP_PATIENT_PAGE_NURSE_INITIAL_ASSESSMENT;
                else if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
                    return Constant.MenuCode.MedicalDiagnostic.FOLLOWUP_PATIENT_PAGE_NURSE_INITIAL_ASSESSMENT;
                else
                    return Constant.MenuCode.Outpatient.FOLLOWUP_PATIENT_PAGE_NURSE_INITIAL_ASSESSMENT;
            }
            else
            {
                if (deptType == Constant.Facility.OUTPATIENT)
                    return Constant.MenuCode.Outpatient.PATIENT_PAGE_NURSE_INITIAL_ASSESSMENT;
                else if (deptType == Constant.Facility.DIAGNOSTIC)
                    return Constant.MenuCode.MedicalDiagnostic.PATIENT_PAGE_NURSE_INITIAL_ASSESSMENT;
                else
                    return Constant.MenuCode.Outpatient.PATIENT_PAGE_NURSE_INITIAL_ASSESSMENT;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected string GetVisitNoteID()
        {
            return _visitNoteID;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            if (!string.IsNullOrEmpty(Page.Request.QueryString["id"]))
            {
                string[] param = Page.Request.QueryString["id"].Split('|');
                if (param.Count() > 1)
                {
                    deptType = param[0];
                    menuType = param[1];
                }
                else
                {
                    deptType = param[0];
                }
            }
            else
            {
                deptType = Constant.Facility.OUTPATIENT;
                menuType = "";
            }

            vHealthcareServiceUnitCustom hsu = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("DepartmentID = '{0}'", AppSession.RegisteredPatient.DepartmentID)).FirstOrDefault();
            hdnHealthcareServiceUnitID.Value = hsu.HealthcareServiceUnitID.ToString();

            ConsultVisit entityConsultVisit = BusinessLayer.GetConsultVisit(AppSession.RegisteredPatient.VisitID);
            Registration registration = BusinessLayer.GetRegistration(entityConsultVisit.RegistrationID);
            if (registration != null)
            {
                if (registration.MRN == AppSession.RegisteredPatient.MRN)
                {
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

                    PatientVisitNote oVisitNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID IN ({0}) AND GCPatientNoteType = '{1}' AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, Constant.PatientVisitNotes.NURSE_INITIAL_ASSESSMENT)).FirstOrDefault();
                    if (oVisitNote != null)
                    {
                        hdnPatientVisitNoteID.Value = oVisitNote.ID.ToString();
                    }
                    _visitNoteID = hdnPatientVisitNoteID.Value;

                    Patient oPatient = BusinessLayer.GetPatientList(string.Format("MRN='{0}'", AppSession.RegisteredPatient.MRN)).FirstOrDefault();
                    if (oPatient != null)
                    {
                        chkIsGeriatricPatient.Checked = oPatient.IsGeriatricPatient;
                    }

                    LoadBodyDiagram();

                    BindGridViewDiagnosis(1, true, ref gridDiagnosisPageCount);
                    BindGridViewVitalSign(1, true, ref gridVitalSignPageCount);
                    BindGridViewEducation(1, true, ref gridEducationPageCount);
                }
                else
                {
                    DisplayMessageToUser(String.Format("Maaf ada perbedaan data antara session. Harap Refresh Halaman Ini."));
                }
            }
            else
            {
                DisplayMessageToUser(String.Format("Maaf ada perbedaan data antara session. Harap Refresh Halaman Ini."));
            }

            List<SettingParameterDt> lstSetpar = BusinessLayer.GetSettingParameterDtList(string.Format(
                    "HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
                    AppSession.UserLogin.HealthcareID, //0
                    Constant.SettingParameter.EM0089 //1
                    ));

            hdnDisableServiceDateTime.Value = lstSetpar.Where(t => t.ParameterCode == Constant.SettingParameter.EM0089).FirstOrDefault().ParameterValue;
        }

        private void DisplayMessageToUser(string errMessage)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<script type = 'text/javascript'>");
            sb.Append("window.onload=function(){");
            sb.Append("alert('");
            sb.Append(errMessage);
            sb.Append("')};");
            sb.Append("</script>");
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
        }

        protected override void SetControlProperties()
        {
            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}') AND IsActive = 1 AND IsDeleted = 0",
                Constant.StandardCode.ONSET,
                Constant.StandardCode.EXACERBATED,
                Constant.StandardCode.QUALITY,
                Constant.StandardCode.SEVERITY,
                Constant.StandardCode.COURSE_TIMING,
                Constant.StandardCode.RELIEVED_BY,
                Constant.StandardCode.VISIT_REASON,
                Constant.StandardCode.FUNCTIONAL_TYPE,
                Constant.StandardCode.PATIENT_EDUCATION_TYPE,
                Constant.StandardCode.PSYCHOLOGY_STATUS,
                Constant.StandardCode.RAPUH_RESISTENSI,
                Constant.StandardCode.RAPUH_AKTIFITAS,
                Constant.StandardCode.RAPUH_PENYAKIT,
                Constant.StandardCode.RAPUH_USAHA_BERJALAN,
                Constant.StandardCode.RAPUH_BERAT_BADAN,
                Constant.StandardCode.RAPUH_SCORE);
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(filterExpression);
            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboOnset, lstSc.Where(p => p.ParentID == Constant.StandardCode.ONSET || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboProvocation, lstSc.Where(p => p.ParentID == Constant.StandardCode.EXACERBATED || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboQuality, lstSc.Where(p => p.ParentID == Constant.StandardCode.QUALITY || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboSeverity, lstSc.Where(p => p.ParentID == Constant.StandardCode.SEVERITY || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboTime, lstSc.Where(p => p.ParentID == Constant.StandardCode.COURSE_TIMING || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboRelievedBy, lstSc.Where(p => p.ParentID == Constant.StandardCode.RELIEVED_BY || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboFunctionalType, lstSc.Where(p => p.ParentID == Constant.StandardCode.FUNCTIONAL_TYPE || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboPsychologyStatus, lstSc.Where(p => p.ParentID == Constant.StandardCode.PSYCHOLOGY_STATUS || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboRAPUH_R, lstSc.Where(p => p.ParentID == Constant.StandardCode.RAPUH_RESISTENSI || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboRAPUH_A, lstSc.Where(p => p.ParentID == Constant.StandardCode.RAPUH_AKTIFITAS || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboRAPUH_P, lstSc.Where(p => p.ParentID == Constant.StandardCode.RAPUH_PENYAKIT || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboRAPUH_U, lstSc.Where(p => p.ParentID == Constant.StandardCode.RAPUH_USAHA_BERJALAN || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboRAPUH_H, lstSc.Where(p => p.ParentID == Constant.StandardCode.RAPUH_BERAT_BADAN || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboRAPUHScore, lstSc.Where(p => p.ParentID == Constant.StandardCode.RAPUH_SCORE || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

            vNurseChiefComplaint entity = BusinessLayer.GetvNurseChiefComplaintList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            PatientVisitNote oPatientVisitNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = '{0}' AND GCPatientNoteType = '{1}'", AppSession.RegisteredPatient.VisitID, Constant.PatientVisitNotes.NURSE_INITIAL_ASSESSMENT)).FirstOrDefault();

            if (oPatientVisitNote != null)
                hdnPatientVisitNoteID.Value = oPatientVisitNote.ID.ToString();
            else
                hdnPatientVisitNoteID.Value = "0";

            EntityToControl(entity);

            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;
            filterExpression = string.Empty;
            if (paramedicID != 0)
                filterExpression = string.Format("ParamedicID = {0}", paramedicID);
            else
                filterExpression = string.Format("GCParamedicMasterType != '{0}'", Constant.ParamedicType.Physician);

            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(filterExpression);
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");

            hdnParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();

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
            if (hdnDisableServiceDateTime.Value == "0")
            {
                Helper.SetControlEntrySetting(txtServiceDate, new ControlEntrySetting(isEnabled, false, true), "mpPatientStatus");
                Helper.SetControlEntrySetting(txtServiceTime1, new ControlEntrySetting(isEnabled, false, true), "mpPatientStatus");
                Helper.SetControlEntrySetting(txtServiceTime2, new ControlEntrySetting(isEnabled, false, true), "mpPatientStatus");
                txtServiceDate.Attributes.Add("readonly", "readonly");
                txtServiceTime1.Attributes.Add("readonly", "readonly");
                txtServiceTime2.Attributes.Add("readonly", "readonly");
            }
            else
            {
                Helper.SetControlEntrySetting(txtServiceDate, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
                Helper.SetControlEntrySetting(txtServiceTime1, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
                Helper.SetControlEntrySetting(txtServiceTime2, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            }
            Helper.SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(chkIsNeedAcuteInitialAssessment, new ControlEntrySetting(isEnabled, isEnabled, false, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(chkIsNeedChronicInitialAssessment, new ControlEntrySetting(isEnabled, isEnabled, false, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(chkIsNeedPatientEducation, new ControlEntrySetting(isEnabled, isEnabled, false, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboFunctionalType, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtFunctionalTypeRemarks, new ControlEntrySetting(isEnabled, isEnabled, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(rblFamilyRelationship, new ControlEntrySetting(isEnabled, isEnabled, false, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtFamilyRelationshipRemarks, new ControlEntrySetting(isEnabled, isEnabled, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(rblIsNeedAdditionalPrivacy, new ControlEntrySetting(isEnabled, isEnabled, false, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtNeedAdditionalPrivacyRemarks, new ControlEntrySetting(isEnabled, isEnabled, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboPsychologyStatus, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtCommitSuicideRemarks, new ControlEntrySetting(isEnabled, isEnabled, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(rblIsNeedAdditionalPrivacy, new ControlEntrySetting(isEnabled, isEnabled, false, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtFinancialProblemRemarks, new ControlEntrySetting(isEnabled, isEnabled, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboRAPUH_R, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboRAPUH_A, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboRAPUH_P, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboRAPUH_U, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboRAPUH_H, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtRAPUHScore, new ControlEntrySetting(isEnabled, isEnabled, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboRAPUHScore, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(chkIsFastTrack, new ControlEntrySetting(isEnabled, isEnabled, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(chkIsGeriatricPatient, new ControlEntrySetting(isEnabled, isEnabled, false), "mpPatientStatus");

            if (AppSession.UserLogin.ParamedicID != 0 && AppSession.UserLogin.ParamedicID != null)
            {
                if (!string.IsNullOrEmpty(hdnAssessmentParamedicID.Value) && hdnAssessmentParamedicID.Value != "0")
                {
                    if (Convert.ToInt32(hdnAssessmentParamedicID.Value) != AppSession.UserLogin.ParamedicID)
                        cboParamedicID.Value = Convert.ToInt32(hdnAssessmentParamedicID.Value);
                    else
                        cboParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
                }
                else
                {
                    cboParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
                }
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
                    hdnIsNotAllowNurseFillChiefComplaint.Value = "1";
                }
                else
                {
                    Helper.SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
                    hdnIsNotAllowNurseFillChiefComplaint.Value = "0";
                }
            }
            else
            {
                Helper.SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
                hdnIsNotAllowNurseFillChiefComplaint.Value = "0";
            }
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

        private void EntityToControl(vNurseChiefComplaint entity)
        {
            hdnDepartmentID.Value = entity.DepartmentID;
            hdnAssessmentParamedicID.Value = entity.ParamedicID.ToString();

            if (entity.ChiefComplaintID == 0)
            {
                hdnID.Value = "";
            }
            else
            {
                hdnID.Value = entity.ChiefComplaintID.ToString();
            }

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
            txtMedicalHistory.Text = entity.MedicalHistory;
            txtMedicationHistory.Text = entity.MedicationHistory;
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

            chkIsNeedAcuteInitialAssessment.Checked = entity.IsNeedAccuteInitialAssessment;
            chkIsNeedChronicInitialAssessment.Checked = entity.IsNeedChronicInitialAssessment;

            cboFunctionalType.Value = entity.GCFunctionalType;
            txtFunctionalTypeRemarks.Text = entity.FunctionalTypeRemarks;

            chkIsNeedPatientEducation.Checked = entity.IsNeedPatientEducation;
            chkIsFastTrack.Checked = entity.IsFastTrack;

            rblFamilyRelationship.SelectedValue = entity.IsHasGoodFamilyRelationship ? "1" : "0";
            txtFamilyRelationshipRemarks.Text = entity.FamilyRelationshipRemarks;

            rblIsNeedAdditionalPrivacy.SelectedValue = entity.IsNeedAdditionalPrivacy ? "1" : "0";
            txtNeedAdditionalPrivacyRemarks.Text = entity.NeedAdditionalPrivacyRemarks;

            cboPsychologyStatus.Value = entity.GCPsychologyStatus;
            txtCommitSuicideRemarks.Text = entity.ReportToPotentiallyCommitSuicide;

            rblHasFinancialProblem.SelectedValue = entity.IsHasFinancialProblem ? "1" : "0";
            txtFinancialProblemRemarks.Text = entity.FinancialProblemRemarks;

            chkIsHasRAPUHAssessment.Checked = entity.IsHasRAPUHAssessment;
            cboRAPUH_R.Value = entity.GCRAPUH_R;
            cboRAPUH_A.Value = entity.GCRAPUH_A;
            cboRAPUH_P.Value = entity.GCRAPUH_P;
            cboRAPUH_U.Value = entity.GCRAPUH_U;
            cboRAPUH_H.Value = entity.GCRAPUH_H;
            txtRAPUHScore.Text = entity.RAPUHScore.ToString("G29");
            cboRAPUHScore.Value = entity.GCRAPUHScore;
            #endregion

            txtDiagnose.Text = entity.DiagnosisText;
            #endregion

            List<PatientVisitNote> lstPatientVisitNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = '{0}' AND GCPatientNoteType = '{1}'", AppSession.RegisteredPatient.VisitID, Constant.PatientVisitNotes.NURSE_INITIAL_ASSESSMENT));
            if (lstPatientVisitNote.Count > 0)
            {
                PatientVisitNote entitypvn = lstPatientVisitNote.First();
                if (string.IsNullOrEmpty(txtPlanningNotes.Text))
                {
                    txtPlanningNotes.Text = entitypvn.PlanningText;
                    txtInstructionText.Text = entitypvn.InstructionText;
                }
            }
        }

        private void ControlToEntity(NurseChiefComplaint entity, PatientDiagnosis patientDiagnosis)
        {
            #region Chief Complaint
            entity.NurseChiefComplaintText = txtChiefComplaint.Text;
            entity.HPISummary = txtHPISummary.Text;
            entity.MedicalHistory = txtMedicalHistory.Text;
            entity.MedicationHistory = txtMedicationHistory.Text;
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

            entity.IsInitialAssessment = true;
            entity.IsNeedAccuteInitialAssessment = chkIsNeedAcuteInitialAssessment.Checked;
            entity.IsNeedChronicInitialAssessment = chkIsNeedChronicInitialAssessment.Checked;

            if (cboFunctionalType.Value != null)
                entity.GCFunctionalType = cboFunctionalType.Value.ToString();
            if (!string.IsNullOrEmpty(txtFunctionalTypeRemarks.Text))
                entity.FunctionalTypeRemarks = txtFunctionalTypeRemarks.Text;

            entity.IsNeedPatientEducation = chkIsNeedPatientEducation.Checked;

            entity.IsHasGoodFamilyRelationship = rblFamilyRelationship.SelectedValue == "1" ? true : false;
            if (!string.IsNullOrEmpty(txtFamilyRelationshipRemarks.Text))
                entity.FamilyRelationshipRemarks = txtFamilyRelationshipRemarks.Text;

            entity.IsNeedAdditionalPrivacy = rblIsNeedAdditionalPrivacy.SelectedValue == "1" ? true : false;
            if (!string.IsNullOrEmpty(txtNeedAdditionalPrivacyRemarks.Text))
                entity.NeedAdditionalPrivacyRemarks = txtNeedAdditionalPrivacyRemarks.Text;

            if (cboPsychologyStatus.Value != null)
                entity.GCPsychologyStatus = cboPsychologyStatus.Value.ToString();
            if (!string.IsNullOrEmpty(txtCommitSuicideRemarks.Text))
                entity.ReportToPotentiallyCommitSuicide = txtCommitSuicideRemarks.Text;

            entity.IsHasFinancialProblem = rblHasFinancialProblem.SelectedValue == "1" ? true : false;
            if (!string.IsNullOrEmpty(txtFinancialProblemRemarks.Text))
                entity.FinancialProblemRemarks = txtFinancialProblemRemarks.Text;

            entity.IsHasRAPUHAssessment = chkIsHasRAPUHAssessment.Checked;
            if (cboRAPUH_R.Value != null)
                entity.GCRAPUH_R = cboRAPUH_R.Value.ToString();
            if (cboRAPUH_A.Value != null)
                entity.GCRAPUH_A = cboRAPUH_A.Value.ToString();
            if (cboRAPUH_P.Value != null)
                entity.GCRAPUH_P = cboRAPUH_P.Value.ToString();
            if (cboRAPUH_U.Value != null)
                entity.GCRAPUH_U = cboRAPUH_U.Value.ToString();
            if (cboRAPUH_H.Value != null)
                entity.GCRAPUH_H = cboRAPUH_H.Value.ToString();

            entity.RAPUHScore = !string.IsNullOrEmpty(txtRAPUHScore.Text) ? Convert.ToInt16(txtRAPUHScore.Text) : 0;
            if (cboRAPUHScore.Value != null)
                entity.GCRAPUHScore = cboRAPUHScore.Value.ToString();
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
            Registration entityRegistration = registrationDao.Get(AppSession.RegisteredPatient.RegistrationID);
            entityConsultVisit.VisitTypeID = Convert.ToInt32(hdnVisitTypeID.Value);
            //entityConsultVisit.GCVisitStatus = Constant.VisitStatus.RECEIVING_TREATMENT;
            //entityConsultVisit.StartServiceDate = Helper.GetDatePickerValue(txtServiceDate);
            //entityConsultVisit.StartServiceTime = string.Format("{0}:{1}", txtServiceTime1.Text, txtServiceTime2.Text);
            if (entityConsultVisit.GCVisitStatus == Constant.VisitStatus.CHECKED_IN)
            {
                entityConsultVisit.StartServiceDate = Helper.GetDatePickerValue(txtServiceDate);
                entityConsultVisit.StartServiceTime = string.Format("{0}:{1}", txtServiceTime1.Text, txtServiceTime2.Text);
                entityConsultVisit.GCVisitStatus = Constant.VisitStatus.RECEIVING_TREATMENT;
            }
            entityConsultVisit.TimeElapsed1 = string.Format("{0}:{1}:{2}", hdnTimeElapsed1day.Value.PadLeft(2, '0'), hdnTimeElapsed1hour.Value.PadLeft(2, '0'), hdnTimeElapsed1minute.Value.PadLeft(2, '0'));
            entityConsultVisit.LastUpdatedBy = AppSession.UserLogin.UserID;
            entityConsultVisit.NurseParamedicID = AppSession.UserLogin.ParamedicID;

            consultVisitDao.Update(entityConsultVisit);

            entityRegistration.IsFastTrack = chkIsFastTrack.Checked;
            registrationDao.Update(entityRegistration);
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            if (type == "save")
            {
                IDbContext ctx = DbFactory.Configure(true);
                PatientVisitNoteDao patientVisitNoteDao = new PatientVisitNoteDao(ctx);
                bool isAllowSave = true;
                try
                {
                    if (!string.IsNullOrEmpty(hdnAssessmentParamedicID.Value) && hdnAssessmentParamedicID.Value != "0")
                    {
                        if (Convert.ToInt32(hdnAssessmentParamedicID.Value) != AppSession.UserLogin.ParamedicID)
                            isAllowSave = false;
                    }

                    if (isAllowSave)
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
                        OnSaveAddEditPatientStatus(ctx, ref errMessage);

                        ctx.CommitTransaction();

                        hdnIsSaved.Value = "1";
                        hdnIsChanged.Value = "0";
                    }
                    else
                    {
                        errMessage = "Maaf, Perubahan Pengkajian Pasien hanya bisa dilakukan oleh Perawat yang melakukan pengkajian pertama kali";
                        hdnIsChanged.Value = "0";

                        ctx.RollBackTransaction();
                        result = false;
                    }
                }
                catch (Exception ex)
                {
                    result = false;
                    errMessage = ex.Message;
                    hdnIsSaved.Value = "0";
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
            entityNote.SubjectiveText = hdnSubjectiveText.Value;
            entityNote.ObjectiveText = hdnObjectiveText.Value;
            entityNote.AssessmentText = hdnAssessmentText.Value;
            entityNote.PlanningText = txtPlanningNotes.Text;
            entityNote.InstructionText = txtInstructionText.Text;
            entityNote.NoteDate = Helper.GetDatePickerValue(txtServiceDate);
            entityNote.NoteTime = string.Format("{0}:{1}", txtServiceTime1.Text, txtServiceTime2.Text);
        }

        private string GenerateSOAPText()
        {
            StringBuilder sbNotes = new StringBuilder();
            StringBuilder sbSubjective = new StringBuilder();
            sbNotes.AppendLine("SUBJEKTIVE :");
            sbNotes.AppendLine("-".PadRight(20, '-'));

            sbNotes.AppendLine(string.Format("Keluhan Pasien  : "));
            sbNotes.AppendLine(string.Format(" {0}   ", txtChiefComplaint.Text));
            if (!string.IsNullOrEmpty(txtHPISummary.Text))
            {
                sbNotes.AppendLine(string.Format("Riwayat Penyakit Sekarang  : "));
                sbNotes.AppendLine(string.Format(" {0}   ", txtHPISummary.Text));
            }
            if (!string.IsNullOrEmpty(txtMedicalHistory.Text))
            {
                sbNotes.AppendLine(string.Format("Riwayat Penyakit Dahulu  : "));
                sbNotes.AppendLine(string.Format(" {0}   ", txtMedicalHistory.Text));
            }
            if (!string.IsNullOrEmpty(txtMedicationHistory.Text))
            {
                sbNotes.AppendLine(string.Format("Riwayat Penggunaan Obat : "));
                sbNotes.AppendLine(string.Format(" {0}   ", txtMedicationHistory.Text));
            }

            hdnSubjectiveText.Value = sbNotes.ToString();

            sbNotes.AppendLine(" ");
            sbNotes = new StringBuilder();
            sbNotes.AppendLine("OBJEKTIF :");
            sbNotes.AppendLine("-".PadRight(20, '-'));
            List<vVitalSignDt> lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID = {0} AND ObservationDate = '{1}' AND IsDeleted = 0 ORDER BY DisplayOrder", AppSession.RegisteredPatient.VisitID, Helper.GetDatePickerValue(txtServiceDate).ToString(Constant.FormatString.DATE_FORMAT_112)));
            if (lstVitalSignDt.Count > 0)
            {
                sbNotes.AppendLine("Tanda Vital :");
                foreach (vVitalSignDt vitalSign in lstVitalSignDt)
                {
                    sbNotes.AppendLine(string.Format(" - {0} {1} {2}", vitalSign.VitalSignLabel, vitalSign.VitalSignValue, vitalSign.ValueUnit));
                }
            }

            List<vReviewOfSystemDt> lstROS = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID IN (SELECT ID FROM ReviewOfSystemHd WHERE VisitID = {0} AND  ObservationDate = '{1}') AND IsDeleted = 0 ORDER BY GCRoSystem", AppSession.RegisteredPatient.VisitID, Helper.GetDatePickerValue(txtServiceDate).ToString(Constant.FormatString.DATE_FORMAT_112)));
            if (lstROS.Count > 0)
            {
                sbNotes.AppendLine(" ");
                sbNotes.AppendLine("Pemeriksaan Fisik :");
                foreach (vReviewOfSystemDt item in lstROS)
                {
                    sbNotes.AppendLine(string.Format(" - {0}: {1}", item.ROSystem, item.cfRemarks));
                }
            }

            hdnObjectiveText.Value = sbNotes.ToString();

            sbNotes.AppendLine(" ");
            sbNotes.AppendLine("ASSESSMENT :");
            sbNotes.AppendLine("-".PadRight(20, '-'));

            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);
            List<vNursingPatientProblem> lstEntity = BusinessLayer.GetvNursingPatientProblemList(filterExpression);

            //Create Diagnosis Summary for : CPOE Clinical Notes
            StringBuilder strDiagnosis = new StringBuilder();
            foreach (var item in lstEntity)
            {
                strDiagnosis.AppendLine(string.Format("- {0}", item.ProblemName));
            }
            hdnAssessmentText.Value = strDiagnosis.ToString();
            sbNotes.AppendLine(string.Format(" {0}", hdnAssessmentText.Value));

            sbNotes.AppendLine(" ");
            sbNotes.AppendLine("PLANNING :");
            sbNotes.AppendLine("-".PadRight(20, '-'));
            sbNotes.AppendLine(string.Format(" {0}", txtPlanningNotes.Text));

            sbNotes.AppendLine(" ");
            sbNotes.AppendLine("INSTRUKSI : ");
            sbNotes.AppendLine("-".PadRight(20, '-'));
            sbNotes.AppendLine(string.Format(" {0}", txtInstructionText.Text));

            return hdnSubjectiveText.Value + " " + sbNotes.ToString();
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
                entity.IsDeleted = false;
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

            Patient oPatient = patientDao.Get(AppSession.RegisteredPatient.MRN);
            if (oPatient != null)
            {
                if (chkIsNeedAcuteInitialAssessment.Checked)
                {
                    oPatient.LastAcuteInitialAssessmentDate = Helper.GetDatePickerValue(txtServiceDate);
                }
                if (chkIsNeedChronicInitialAssessment.Checked)
                {
                    oPatient.LastChronicInitialAssessmentDate = Helper.GetDatePickerValue(txtServiceDate);
                }
                oPatient.IsGeriatricPatient = chkIsGeriatricPatient.Checked;
                patientDao.Update(oPatient);
            }

            #endregion
        }

        #region Diagnosis
        private void BindGridViewDiagnosis(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvNursingPatientProblemRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vNursingPatientProblem> lstEntity = BusinessLayer.GetvNursingPatientProblemList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ProblemCode");

            //Create Diagnosis Summary for : CPOE Clinical Notes
            StringBuilder strDiagnosis = new StringBuilder();
            foreach (var item in lstEntity)
            {
                strDiagnosis.AppendLine(string.Format("- {0}", item.ProblemName));
            }
            hdnDiagnosisSummary.Value = strDiagnosis.ToString();
            hdnAssessmentText.Value = hdnDiagnosisSummary.Value;

            grdDiagnosisView.DataSource = lstEntity;
            grdDiagnosisView.DataBind();
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
            panel.JSProperties["cpRetval"] = string.Empty;
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
                        NursingPatientProblem entity = new NursingPatientProblem();

                        entity.ProblemDate = Helper.GetDatePickerValue(txtServiceDate);
                        entity.ProblemTime = string.Format("{0}:{1}", txtServiceTime1.Text, txtServiceTime2.Text);
                        entity.VisitID = AppSession.RegisteredPatient.VisitID;
                        entity.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                        entity.ProblemID = Convert.ToInt32(hdnEntryDiagnoseID.Value);
                        entity.CreatedBy = AppSession.UserLogin.UserID;

                        BusinessLayer.InsertNursingPatientProblem(entity);

                        result = "1|add|";
                    }
                    else if (param[0] == "edit")
                    {
                        int recordID = Convert.ToInt32(hdnDiagnosisID.Value);
                        NursingPatientProblem entity = BusinessLayer.GetNursingPatientProblem(recordID);

                        if (entity != null)
                        {
                            entity.ProblemDate = Helper.GetDatePickerValue(txtServiceDate);
                            entity.ProblemTime = string.Format("{0}:{1}", txtServiceTime1.Text, txtServiceTime2.Text);

                            entity.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);

                            if (hdnEntryDiagnoseID.Value != "")
                                entity.ProblemID = Convert.ToInt32(hdnEntryDiagnoseID.Value);

                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdateNursingPatientProblem(entity);
                            result = "1|edit|";
                        }
                        else
                        {
                            result = string.Format("0|delete|{0}", "Informasi masalah keperawatan pasien tidak valid");
                        }
                    }
                    else
                    {
                        int recordID = Convert.ToInt32(hdnDiagnosisID.Value);
                        NursingPatientProblem entity = BusinessLayer.GetNursingPatientProblem(recordID);

                        if (entity != null)
                        {
                            //TODO : Prompt user for delete reason
                            entity.IsDeleted = true;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdateNursingPatientProblem(entity);
                            result = "1|delete|";
                        }
                        else
                        {
                            result = string.Format("0|edit|{0}", "Informasi masalah keperawatan pasien tidak valid");
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

        #region Vital Sign
        private void BindGridViewVitalSign(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Empty;

            string visitNoteID = !string.IsNullOrEmpty(_visitNoteID) ? _visitNoteID : "0";
            string linkedVisitID = !string.IsNullOrEmpty(_linkedVisitID) ? _linkedVisitID : "0";

            filterExpression += string.Format("VisitID IN ({0},{1}) AND IsInitialAssessment = 1 AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, linkedVisitID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvVitalSignHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_COMPACT);
            }

            List<vVitalSignHd> lstEntity = BusinessLayer.GetvVitalSignHdList(filterExpression, Constant.GridViewPageSize.GRID_COMPACT, pageIndex, "ID DESC");
            lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID IN ({0},{1}) AND PatientVisitNoteID = {2} AND IsInitialAssessment = 1 ORDER BY DisplayOrder", AppSession.RegisteredPatient.VisitID, linkedVisitID, visitNoteID));
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

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
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

        #region Education
        private void BindGridViewEducation(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string visitNoteID = (!string.IsNullOrEmpty(_visitNoteID) && _visitNoteID != "0") ? _visitNoteID : "-1";

            string filterExpression = string.Format("VisitID IN ({0}) AND (PatientVisitNoteID = {1} OR PatientVisitNoteID IS NULL) AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, visitNoteID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientEducationHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientEducationHd> lstEntity = BusinessLayer.GetvPatientEducationHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
            lstPatientEducationDt = BusinessLayer.GetvPatientEducationDtList(filterExpression);
            grdEducationView.DataSource = lstEntity;
            grdEducationView.DataBind();
        }

        protected void grdEducationView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vPatientEducationHd obj = (vPatientEducationHd)e.Row.DataItem;
                Repeater rptPatientEducationDt = (Repeater)e.Row.FindControl("rptPatientEducationDt");
                rptPatientEducationDt.DataSource = GetPatientEducationDt(obj.ID);
                rptPatientEducationDt.DataBind();
            }
        }

        protected List<vPatientEducationDt> GetPatientEducationDt(Int32 ID)
        {
            return lstPatientEducationDt.Where(p => p.ID == ID && p.IsEducationTypeStatus).ToList();
        }

        protected void cbpEducationView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewEducation(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewEducation(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpDeleteEducation_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|";

            if (hdnPatientEducationID.Value != "")
            {
                PatientEducationHd entity = BusinessLayer.GetPatientEducationHd(Convert.ToInt32(hdnPatientEducationID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePatientEducationHd(entity);
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
    }
}
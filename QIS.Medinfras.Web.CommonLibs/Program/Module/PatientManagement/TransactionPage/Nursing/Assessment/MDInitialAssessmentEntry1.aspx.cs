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
using System.IO;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class MDInitialAssessmentEntry1 : BasePagePatientPageList
    {
        string deptType = string.Empty;
        string menuType = string.Empty;
        protected int gridAllergyPageCount = 1;
        protected int gridVitalSignPageCount = 1;
        protected int gridDiagnosisPageCount = 1;
        protected int gridROSPageCount = 1;
        protected List<vVitalSignDt> lstVitalSignDt = null;
        protected List<vReviewOfSystemDt> lstReviewOfSystemDt = null;

        protected static string _visitNoteID;

        public override string OnGetMenuCode()
        {
            if (hdnMenuType.Value == "fo")
            {
                switch (hdnDepartmentID.Value)
                {
                    case Constant.Module.MEDICAL_DIAGNOSTIC: return Constant.MenuCode.MedicalDiagnostic.FOLLOWUP_PATIENT_PAGE_MD_NURSE_INITIAL_ASSESSMENT;
                    default: return Constant.MenuCode.MedicalDiagnostic.FOLLOWUP_PATIENT_PAGE_MD_NURSE_INITIAL_ASSESSMENT;
                }
            }
            else
            {
                switch (hdnDeptType.Value)
                {
                    case Constant.Module.MEDICAL_DIAGNOSTIC:
                        return Constant.MenuCode.MedicalDiagnostic.PATIENT_PAGE_MD_NURSE_INITIAL_ASSESSMENT;
                    case Constant.Module.RADIOTHERAPHY:
                        return Constant.MenuCode.Radiotheraphy.PATIENT_PAGE_RT_NURSE_INITIAL_ASSESSMENT;
                    default:
                        return Constant.MenuCode.MedicalDiagnostic.PATIENT_PAGE_MD_NURSE_INITIAL_ASSESSMENT;
                }
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

            if (Page.Request.QueryString["id"] != null)
            {
                hdnID.Value = string.IsNullOrEmpty(Page.Request.QueryString["id"]) ? "0" : Page.Request.QueryString["id"];
            }

            string[] param = Page.Request.QueryString["id"].Split('|');
            if (param.Count() > 1)
            {
                hdnDeptType.Value = param[0];
                hdnMenuType.Value = param[1];
                hdnID.Value = param[2];
            }
            else
            {
                hdnDeptType.Value = param[0];
            }
            
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            vHealthcareServiceUnitCustom hsu = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("DepartmentID = '{0}'", AppSession.RegisteredPatient.DepartmentID)).FirstOrDefault();
            hdnHealthcareServiceUnitID.Value = hsu.HealthcareServiceUnitID.ToString();

            ConsultVisit entityConsultVisit = BusinessLayer.GetConsultVisit(AppSession.RegisteredPatient.VisitID);
            hdnRegistrationDate.Value = entityConsultVisit.ActualVisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnRegistrationTime.Value = entityConsultVisit.ActualVisitTime;

            txtAsessmentDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnTimeNow1.Value = txtAsessmentTime1.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT).Substring(0, 2);
            hdnTimeNow2.Value = txtAsessmentTime2.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT).Substring(3, 2);

            hdnIsChanged.Value = "0";
        }

        private void PopulateFormContent()
        {
            string filePath = HttpContext.Current.Server.MapPath("~/Libs/App_Data");
            string fileName = string.Format(@"{0}\medicalForm\PhysicalExam\{1}", filePath, "physicalExam.html");
            IEnumerable<string> lstText = File.ReadAllLines(fileName);
            StringBuilder innerHtml = new StringBuilder();

            #region Psikososial
            fileName = string.Format(@"{0}\medicalForm\Psychosocial\{1}", filePath, AppSession.MD0013);
            lstText = File.ReadAllLines(fileName);
            innerHtml = new StringBuilder();
            foreach (string text in lstText)
            {
                innerHtml.AppendLine(text);
            }

            divFormContent2.InnerHtml = innerHtml.ToString();
            hdnPsychosocialLayout.Value = innerHtml.ToString();
            #endregion

            #region Kebutuhan Edukasi

            fileName = string.Format(@"{0}\medicalForm\Education\{1}", filePath, AppSession.MD0014);
            lstText = File.ReadAllLines(fileName);
            innerHtml = new StringBuilder();
            foreach (string text in lstText)
            {
                innerHtml.AppendLine(text);
            }

            divFormContent3.InnerHtml = innerHtml.ToString();
            hdnEducationLayout.Value = innerHtml.ToString();
            #endregion

            #region Kebutuhan Assessment Tambahan
            fileName = string.Format(@"{0}\medicalForm\Population\{1}", filePath, "populationAssessment.html");
            lstText = File.ReadAllLines(fileName);
            innerHtml = new StringBuilder();
            foreach (string text in lstText)
            {
                innerHtml.AppendLine(text);
            }

            divFormContent5.InnerHtml = innerHtml.ToString();
            hdnAdditionalLayout.Value = innerHtml.ToString();
            #endregion
        }

        protected override void SetControlProperties()
        {
            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}') AND IsActive = 1 AND IsDeleted = 0",
                Constant.StandardCode.ONSET,
                Constant.StandardCode.EXACERBATED,
                Constant.StandardCode.QUALITY,
                Constant.StandardCode.SEVERITY,
                Constant.StandardCode.COURSE_TIMING,
                Constant.StandardCode.RELIEVED_BY,
                Constant.StandardCode.VISIT_REASON,
                Constant.StandardCode.FUNCTIONAL_TYPE,
                Constant.StandardCode.FAMILY_RELATION,
                Constant.StandardCode.ALLERGEN_TYPE,
                Constant.StandardCode.HOUSING_CONDITION,
                Constant.StandardCode.RAPUH_RESISTENSI,
                Constant.StandardCode.RAPUH_AKTIFITAS,
                Constant.StandardCode.RAPUH_PENYAKIT,
                Constant.StandardCode.RAPUH_USAHA_BERJALAN,
                Constant.StandardCode.RAPUH_BERAT_BADAN,
                Constant.StandardCode.RAPUH_SCORE
                );

            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(filterExpression);
            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField(cboAllergenType, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.ALLERGEN_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboOnset, lstSc.Where(p => p.ParentID == Constant.StandardCode.ONSET || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboProvocation, lstSc.Where(p => p.ParentID == Constant.StandardCode.EXACERBATED || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboQuality, lstSc.Where(p => p.ParentID == Constant.StandardCode.QUALITY || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboSeverity, lstSc.Where(p => p.ParentID == Constant.StandardCode.SEVERITY || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboTime, lstSc.Where(p => p.ParentID == Constant.StandardCode.COURSE_TIMING || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboRelievedBy, lstSc.Where(p => p.ParentID == Constant.StandardCode.RELIEVED_BY || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboFunctionalType, lstSc.Where(p => p.ParentID == Constant.StandardCode.FUNCTIONAL_TYPE || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboFamilyRelation, lstSc.Where(p => p.ParentID == Constant.StandardCode.FAMILY_RELATION || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboHousingCondition, lstSc.Where(p => p.ParentID == Constant.StandardCode.HOUSING_CONDITION || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboRAPUH_R, lstSc.Where(p => p.ParentID == Constant.StandardCode.RAPUH_RESISTENSI || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboRAPUH_A, lstSc.Where(p => p.ParentID == Constant.StandardCode.RAPUH_AKTIFITAS || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboRAPUH_P, lstSc.Where(p => p.ParentID == Constant.StandardCode.RAPUH_PENYAKIT || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboRAPUH_U, lstSc.Where(p => p.ParentID == Constant.StandardCode.RAPUH_USAHA_BERJALAN || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboRAPUH_H, lstSc.Where(p => p.ParentID == Constant.StandardCode.RAPUH_BERAT_BADAN || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboRAPUHScore, lstSc.Where(p => p.ParentID == Constant.StandardCode.RAPUH_SCORE || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

            if (hdnID.Value == "")
            {
                hdnID.Value = "0";
            }

            vNurseChiefComplaint entity = BusinessLayer.GetvNurseChiefComplaintList(string.Format("VisitID = {0} AND ChiefComplaintID = {1}", AppSession.RegisteredPatient.VisitID, hdnID.Value)).FirstOrDefault();

            if (entity != null)
            {
                EntityToControl(entity);
                if (entity.PatientVisitNoteID != 0)
                {
                    hdnPatientVisitNoteID.Value = entity.PatientVisitNoteID.ToString();
                    _visitNoteID = hdnPatientVisitNoteID.Value;
                }
            }
            else
            {
                hdnPatientVisitNoteID.Value = "0";
                _visitNoteID = "0";

                hdnID.Value = "0";
            }

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
            Helper.SetControlEntrySetting(txtAsessmentDate, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtAsessmentTime1, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtAsessmentTime2, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(chkIsNeedAcuteInitialAssessment, new ControlEntrySetting(isEnabled, isEnabled, false, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(chkIsNeedChronicInitialAssessment, new ControlEntrySetting(isEnabled, isEnabled, false, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboFunctionalType, new ControlEntrySetting(isEnabled, isEnabled, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtFunctionalTypeRemarks, new ControlEntrySetting(isEnabled, isEnabled, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboHousingCondition, new ControlEntrySetting(isEnabled, isEnabled, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtHousingConditionRemarks, new ControlEntrySetting(isEnabled, isEnabled, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboRAPUH_R, new ControlEntrySetting(isEnabled, isEnabled, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboRAPUH_A, new ControlEntrySetting(isEnabled, isEnabled, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboRAPUH_P, new ControlEntrySetting(isEnabled, isEnabled, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboRAPUH_U, new ControlEntrySetting(isEnabled, isEnabled, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboRAPUH_H, new ControlEntrySetting(isEnabled, isEnabled, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtRAPUHScore, new ControlEntrySetting(isEnabled, isEnabled, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboRAPUHScore, new ControlEntrySetting(isEnabled, isEnabled, false), "mpPatientStatus");


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

            #region Pembaharuan Asessmen Awal
            chkIsNeedAcuteInitialAssessment.Enabled = AppSession.RegisteredPatient.IsNeedRenewalAcuteInitialAssessment;
            lblLastAcuteInitialAssessmentDate.Visible = AppSession.RegisteredPatient.IsNeedRenewalAcuteInitialAssessment;
            chkIsNeedChronicInitialAssessment.Enabled = AppSession.RegisteredPatient.IsNeedRenewalChronicInitialAssessment;
            lblLastChronicInitialAssessmentDate.Visible = AppSession.RegisteredPatient.IsNeedRenewalChronicInitialAssessment;

            string innerHTML = string.Empty;

            if (AppSession.RegisteredPatient.IsNeedRenewalAcuteInitialAssessment)
            {
                if (AppSession.RegisteredPatient.LastAcuteInitialAssessmentDate.ToString(Constant.FormatString.DATE_FORMAT) != Constant.ConstantDate.DEFAULT_NULL_DATE_FORMAT)
                {
                    innerHTML = string.Format("(<i>Terakhir Tanggal</i> : <b>{0}<b/>)", AppSession.RegisteredPatient.LastAcuteInitialAssessmentDate.ToString(Constant.FormatString.DATE_FORMAT));
                }
                else
                {
                    innerHTML = string.Format("(<i>Terakhir Tanggal</i> : <b>{0}<b/>)", "dd-MMM-yyyy");
                }
                lblLastAcuteInitialAssessmentDate.InnerHtml = innerHTML;
            }

            if (AppSession.RegisteredPatient.IsNeedRenewalChronicInitialAssessment)
            {
                if (AppSession.RegisteredPatient.LastChronicInitialAssessmentDate.ToString(Constant.FormatString.DATE_FORMAT) != Constant.ConstantDate.DEFAULT_NULL_DATE_FORMAT)
                {
                    innerHTML = string.Format("(<i>Terakhir Tanggal</i> : <b>{0}<b/>)", AppSession.RegisteredPatient.LastChronicInitialAssessmentDate.ToString(Constant.FormatString.DATE_FORMAT));
                }
                else
                {
                    innerHTML = string.Format("(<i>Terakhir Tanggal</i> : <b>{0}<b/>)", "dd-MMM-yyyy");
                }
                lblLastChronicInitialAssessmentDate.InnerHtml = innerHTML;
            }
            #endregion

            BindGridViewDiagnosis(1, true, ref gridDiagnosisPageCount);
            BindGridViewVitalSign(1, true, ref gridVitalSignPageCount);
            BindGridViewAllergy(1, true, ref gridAllergyPageCount);
            BindGridViewROS(1, true, ref gridROSPageCount);

            PopulateFormContent();

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

            if (entity.ChiefComplaintDate != null && !string.IsNullOrEmpty(entity.ChiefComplaintTime))
            {
                txtAsessmentDate.Text = entity.ChiefComplaintDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtAsessmentTime1.Text = entity.ChiefComplaintTime.Substring(0, 2);
                txtAsessmentTime2.Text = entity.ChiefComplaintTime.Substring(3, 2);
            }
            else
            {
                txtAsessmentDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtAsessmentTime1.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT).Substring(0, 2);
                txtAsessmentTime2.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT).Substring(3, 2);
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
            if (!string.IsNullOrEmpty(entity.GCFamilyRelation))
            {
                cboFamilyRelation.Value = entity.GCFamilyRelation;
            }
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

            txtFamilyHistory.Text = entity.FamilyHistory;

            cboFunctionalType.Value = entity.GCFunctionalType;
            txtFunctionalTypeRemarks.Text = entity.FunctionalTypeRemarks;

            rblIsHasFinancialProblem.SelectedValue = entity.IsHasFinancialProblem ? "1" : "0";
            txtFinancialProblemRemarks.Text = entity.FinancialProblemRemarks;

            cboHousingCondition.Value = entity.GCHousingCondition;
            txtHousingConditionRemarks.Text = entity.HousingConditionRemarks;

            if (!string.IsNullOrEmpty(entity.SocialHistoryLayout))
            {
                hdnPsychosocialLayout.Value = entity.SocialHistoryLayout;
                hdnPsychosocialValue.Value = entity.SocialHistoryValues;
            }
            else
            {
                hdnPsychosocialLayout.Value = GetFormLayout("PsychoSocial");
                hdnPsychosocialValue.Value = string.Empty;
            }

            if (!string.IsNullOrEmpty(entity.EducationValues))
            {
                hdnEducationLayout.Value = entity.EducationLayout;
                hdnEducationValue.Value = entity.EducationValues;
            }
            else
            {
                hdnEducationLayout.Value = GetFormLayout("Education");
                hdnEducationValue.Value = string.Empty;
            }

            chkIsHasRAPUHAssessment.Checked = entity.IsHasRAPUHAssessment;
            cboRAPUH_R.Value = entity.GCRAPUH_R;
            cboRAPUH_A.Value = entity.GCRAPUH_A;
            cboRAPUH_P.Value = entity.GCRAPUH_P;
            cboRAPUH_U.Value = entity.GCRAPUH_U;
            cboRAPUH_H.Value = entity.GCRAPUH_H;
            txtRAPUHScore.Text = entity.RAPUHScore.ToString("G29");
            cboRAPUHScore.Value = entity.GCRAPUHScore;

            if (!string.IsNullOrEmpty(entity.AdditionalAssessmentLayout))
            {
                hdnAdditionalLayout.Value = entity.AdditionalAssessmentLayout;
                hdnAdditionalValue.Value = entity.AdditionalAssessmentValues;
            }
            else
            {
                hdnAdditionalLayout.Value = GetFormLayout("Additional");
                hdnAdditionalValue.Value = string.Empty;
            }
            #endregion

            txtDiagnose.Text = entity.DiagnosisText;
            #endregion
            hdnIsChanged.Value = "0";
        }

        private string GetFormLayout(string typeName)
        {
            string filePath = HttpContext.Current.Server.MapPath("~/Libs/App_Data");
            string fileLocation = string.Empty;
            string formName = string.Empty;

            switch (typeName)
            {
                case "PsychoSocial":
                    fileLocation = @"medicalForm\Psychosocial";
                    formName = AppSession.MD0013;
                    break;
                case "Education":
                    fileLocation = @"medicalForm\Education";
                    formName = AppSession.MD0014;
                    break;
                case "Additional":
                    fileLocation = @"medicalForm\Population";
                    formName = "populationAssessment.html";
                    break;
                default:
                    fileLocation = @"medicalForm\PhysicalExam";
                    formName = string.Empty;
                    break;
            }

            string fileName = string.Format(@"{0}\{1}\{2}", filePath, fileLocation, formName);
            IEnumerable<string> lstText = File.ReadAllLines(fileName);
            StringBuilder innerHtml = new StringBuilder();

            foreach (string text in lstText)
            {
                innerHtml.AppendLine(text);
            }

            return innerHtml.ToString();
        }

        private void ControlToEntity(NurseChiefComplaint entity)
        {
            #region Chief Complaint
            entity.NurseChiefComplaintText = txtChiefComplaint.Text;
            entity.PatientVisitNoteID = Convert.ToInt32(hdnPatientVisitNoteID.Value);
            entity.HPISummary = txtHPISummary.Text;
            entity.MedicalHistory = txtMedicalHistory.Text;
            entity.MedicationHistory = txtMedicationHistory.Text;
            entity.HealthcareServiceUnitID = Convert.ToInt32(AppSession.HealthcareServiceUnitID);
            if (cboParamedicID.Value != null)
            {
                entity.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
            }
            entity.Location = txtLocation.Text;
            entity.IsAutoAnamnesis = Convert.ToBoolean(chkAutoAnamnesis.Checked);
            entity.IsAlloAnamnesis = Convert.ToBoolean(chkAlloAnamnesis.Checked);
            if (cboFamilyRelation.Value != null && entity.IsAlloAnamnesis)
            {
                entity.GCFamilyRelation = cboFamilyRelation.Value.ToString();
            }
            entity.IsPatientAllergyExists = !chkIsPatientAllergyExists.Checked;

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
            entity.ChiefComplaintDate = Helper.GetDatePickerValue(txtAsessmentDate);
            entity.ChiefComplaintTime = string.Format("{0}:{1}", txtAsessmentTime1.Text, txtAsessmentTime2.Text);

            entity.IsNeedAccuteInitialAssessment = chkIsNeedAcuteInitialAssessment.Checked;
            entity.IsNeedChronicInitialAssessment = chkIsNeedChronicInitialAssessment.Checked;
            entity.IsInitialAssessment = (entity.IsNeedAccuteInitialAssessment || entity.IsNeedChronicInitialAssessment);
            entity.IsMedicalDiagnostic = true;

            if (cboFunctionalType.Value != null)
                entity.GCFunctionalType = cboFunctionalType.Value.ToString();
            if (!string.IsNullOrEmpty(txtFunctionalTypeRemarks.Text))
                entity.FunctionalTypeRemarks = txtFunctionalTypeRemarks.Text;


            entity.FamilyHistory = txtFamilyHistory.Text;

            if (rblIsHasFinancialProblem.SelectedValue == "1" || rblIsHasFinancialProblem.SelectedValue == "0")
            {
                entity.IsHasFinancialProblem = rblIsHasFinancialProblem.SelectedValue == "1" ? true : false;
                entity.FinancialProblemRemarks = !string.IsNullOrEmpty(txtFinancialProblemRemarks.Text) ? txtFinancialProblemRemarks.Text : null;
            }

            if (cboHousingCondition.Value != null)
                entity.GCHousingCondition = cboHousingCondition.Value.ToString();
            if (!string.IsNullOrEmpty(txtHousingConditionRemarks.Text))
                entity.HousingConditionRemarks = txtHousingConditionRemarks.Text;

            entity.SocialHistoryLayout = hdnPsychosocialLayout.Value;
            entity.SocialHistoryValues = hdnPsychosocialValue.Value;

            entity.EducationLayout = hdnEducationLayout.Value;
            entity.EducationValues = hdnEducationValue.Value;

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


            entity.AdditionalAssessmentLayout = hdnAdditionalLayout.Value;
            entity.AdditionalAssessmentValues = hdnAdditionalValue.Value;

            entity.IsDeleted = false;

            #endregion
        }

        protected override bool OnCustomButtonClick(string type, ref string messages)
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
                        Int32 objID = 0;
                        NurseChiefComplaint obj = BusinessLayer.GetNurseChiefComplaintList(string.Format("VisitID = {0} AND IsDeleted = 0 AND IsMedicalDiagnostic = 1", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
                        if (obj != null)
                        {
                            objID = obj.ID;
                            hdnID.Value = obj.ID.ToString();
                        }
                        else
                        {
                            hdnID.Value = "0";
                        }

                        if (objID == Convert.ToInt32(hdnID.Value))
                        {
                            PatientVisitNote entityNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = {0} AND GCPatientNoteType = '{1}' AND HealthcareServiceUnitID = {2} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, Constant.PatientVisitNotes.NURSE_INITIAL_ASSESSMENT, Convert.ToInt32(AppSession.HealthcareServiceUnitID)), ctx).FirstOrDefault();
                            bool isEntityNoteNull = false;
                            if (entityNote == null)
                            {
                                isEntityNoteNull = true;
                                entityNote = new PatientVisitNote();
                            }
                            ControlToEntity(entityNote);
                            entityNote.IsDeleted = false;

                            if (isEntityNoteNull)
                            {
                                entityNote.VisitID = AppSession.RegisteredPatient.VisitID;
                                entityNote.ParamedicID = AppSession.UserLogin.ParamedicID;
                                entityNote.HealthcareServiceUnitID = Convert.ToInt32(AppSession.HealthcareServiceUnitID);
                                entityNote.GCPatientNoteType = Constant.PatientVisitNotes.NURSE_INITIAL_ASSESSMENT;
                                entityNote.CreatedBy = AppSession.UserLogin.UserID;
                                hdnPatientVisitNoteID.Value = patientVisitNoteDao.InsertReturnPrimaryKeyID(entityNote).ToString();
                            }
                            else
                            {
                                entityNote.HealthcareServiceUnitID = Convert.ToInt32(AppSession.HealthcareServiceUnitID);
                                entityNote.ParamedicID = AppSession.UserLogin.ParamedicID;
                                entityNote.LastUpdatedBy = AppSession.UserLogin.UserID;
                                patientVisitNoteDao.Update(entityNote);

                                hdnPatientVisitNoteID.Value = entityNote.ID.ToString();
                            }

                            if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.DIAGNOSTIC)
                            {
                                UpdateConsultVisitRegistration(ctx);
                            }
                            OnSaveAddEditPatientStatus(ctx, ref messages);
                            _visitNoteID = hdnPatientVisitNoteID.Value;
                            ctx.CommitTransaction();

                            hdnIsSaved.Value = "1";
                            hdnIsChanged.Value = "0";

                            messages = hdnID.Value;
                        }
                        else
                        {
                            messages = "Hanya boleh ada 1 kajian awal pasien rawat inap dalam 1 episode perawatan.";
                            hdnIsChanged.Value = "0";

                            ctx.RollBackTransaction();
                            result = false;
                        }
                    }
                    else
                    {
                        messages = "Maaf, Perubahan Pengkajian Pasien hanya bisa dilakukan oleh Perawat yang melakukan pengkajian pertama kali";
                        hdnIsChanged.Value = "0";

                        ctx.RollBackTransaction();
                        result = false;
                    }
                }
                catch (Exception ex)
                {
                    result = false;
                    messages = ex.Message;
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

        private void UpdateConsultVisitRegistration(IDbContext ctx)
        {
            ConsultVisitDao consultVisitDao = new ConsultVisitDao(ctx);
            ConsultVisit entityConsultVisit = consultVisitDao.Get(AppSession.RegisteredPatient.VisitID);
            entityConsultVisit.StartServiceDate = Helper.GetDatePickerValue(txtAsessmentDate);
            entityConsultVisit.StartServiceTime = string.Format("{0}:{1}", txtAsessmentTime1.Text, txtAsessmentTime2.Text);
            entityConsultVisit.LastUpdatedBy = AppSession.UserLogin.UserID;
            entityConsultVisit.NurseParamedicID = AppSession.UserLogin.ParamedicID;
            entityConsultVisit.TimeElapsed1 = string.Format("{0}:{1}:{2}", hdnTimeElapsed1day.Value.PadLeft(2, '0'), hdnTimeElapsed1hour.Value.PadLeft(2, '0'), hdnTimeElapsed1minute.Value.PadLeft(2, '0'));
            consultVisitDao.Update(entityConsultVisit);
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
            entityNote.NoteDate = Helper.GetDatePickerValue(txtAsessmentDate);
            entityNote.NoteTime = string.Format("{0}:{1}", txtAsessmentTime1.Text, txtAsessmentTime2.Text);
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
            List<vVitalSignDt> lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID = {0} AND ObservationDate = '{1}' AND IsDeleted = 0 ORDER BY DisplayOrder", AppSession.RegisteredPatient.VisitID, Helper.GetDatePickerValue(txtAsessmentDate).ToString(Constant.FormatString.DATE_FORMAT_112)));
            if (lstVitalSignDt.Count > 0)
            {
                sbNotes.AppendLine("Tanda Vital :");
                foreach (vVitalSignDt vitalSign in lstVitalSignDt)
                {
                    sbNotes.AppendLine(string.Format(" - {0} {1} {2}", vitalSign.VitalSignLabel, vitalSign.VitalSignValue, vitalSign.ValueUnit));
                }
            }

            List<vReviewOfSystemDt> lstROS = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID IN (SELECT ID FROM ReviewOfSystemHd WHERE VisitID = {0} AND  ObservationDate = '{1}') AND IsDeleted = 0 ORDER BY GCRoSystem", AppSession.RegisteredPatient.VisitID, Helper.GetDatePickerValue(txtAsessmentDate).ToString(Constant.FormatString.DATE_FORMAT_112)));
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
            sbNotes.AppendLine(string.Format(" {0}", txtFamilyHistory.Text));
            sbNotes.AppendLine(string.Format(" {0}", txtPlanningNotes.Text));

            if (!string.IsNullOrEmpty(txtInstructionText.Text))
            {
                sbNotes.AppendLine(" ");
                sbNotes.AppendLine("INSTRUKSI : ");
                sbNotes.AppendLine("-".PadRight(20, '-'));
                sbNotes.AppendLine(string.Format(" {0}", txtInstructionText.Text));
            }

            return hdnSubjectiveText.Value + " " + sbNotes.ToString();
        }

        private void OnSaveAddEditPatientStatus(IDbContext ctx, ref string messages)
        {
            PatientDiagnosisDao patientDiagnosisDao = new PatientDiagnosisDao(ctx);
            NurseChiefComplaintDao chiefComplaintDao = new NurseChiefComplaintDao(ctx);
            PatientDao patientDao = new PatientDao(ctx);

            bool isChiefComplaintNull = false;

            List<NurseChiefComplaint> lstNCC = BusinessLayer.GetNurseChiefComplaintList(string.Format("VisitID = {0} AND ID = {1}", AppSession.RegisteredPatient.VisitID, hdnID.Value), ctx);

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

            ControlToEntity(entity);

            if (isChiefComplaintNull)
            {
                hdnID.Value = chiefComplaintDao.InsertReturnPrimaryKeyID(entity).ToString();
            }
            else
            {
                hdnID.Value = entity.ID.ToString();
                chiefComplaintDao.Update(entity);
            }

            #region Patient Status
            if (chkIsNeedAcuteInitialAssessment.Checked || chkIsNeedChronicInitialAssessment.Checked)
            {
                Patient oPatient = patientDao.Get(AppSession.RegisteredPatient.MRN);
                if (oPatient != null)
                {
                    if (chkIsNeedAcuteInitialAssessment.Checked)
                    {
                        oPatient.LastAcuteInitialAssessmentDate = Helper.GetDatePickerValue(txtAsessmentDate);
                        AppSession.RegisteredPatient.IsNeedRenewalAcuteInitialAssessment = false;
                    }
                    if (chkIsNeedChronicInitialAssessment.Checked)
                    {
                        oPatient.LastChronicInitialAssessmentDate = Helper.GetDatePickerValue(txtAsessmentDate);
                        AppSession.RegisteredPatient.IsNeedRenewalChronicInitialAssessment = false;
                    }

                    patientDao.Update(oPatient);
                }
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

                        entity.ProblemDate = Helper.GetDatePickerValue(txtAsessmentDate);
                        entity.ProblemTime = string.Format("{0}:{1}", txtAsessmentTime1.Text, txtAsessmentTime2.Text);
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
                            entity.ProblemDate = Helper.GetDatePickerValue(txtAsessmentDate);
                            entity.ProblemTime = string.Format("{0}:{1}", txtAsessmentTime1.Text, txtAsessmentTime2.Text);

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

        #region Allergy
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

            chkIsPatientAllergyExists.Checked = !(lstEntity.Count > 0);
            chkIsPatientAllergyExists.Enabled = (lstEntity.Count == 0);
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

        protected void cbpAllergy_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|add|";

            bool isError = false;

            IDbContext ctx = DbFactory.Configure(true);
            PatientAllergyDao allergyDao = new PatientAllergyDao(ctx);
            PatientDao patientDao = new PatientDao(ctx);
            PatientAllergy oAllergy;

            try
            {
                if (e.Parameter != null && e.Parameter != "")
                {
                    string[] param = e.Parameter.Split('|');
                    if (param[0] == "add")
                    {
                        oAllergy = new PatientAllergy();
                        oAllergy.MRN = AppSession.RegisteredPatient.MRN;
                        oAllergy.AllergyLogDate = DateTime.Now.Date;
                        oAllergy.GCAllergenType = cboAllergenType.Value.ToString();
                        oAllergy.Allergen = txtAllergenName.Text;
                        oAllergy.GCAllergySource = Constant.AllergenFindingSource.PATIENT;
                        oAllergy.GCAllergySeverity = Constant.AllergySeverity.UNKNOWN;
                        oAllergy.KnownDate = DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112);
                        oAllergy.Reaction = txtReaction.Text;
                        oAllergy.CreatedBy = AppSession.UserLogin.UserID;
                        allergyDao.Insert(oAllergy);

                        Patient oPatient = patientDao.Get(AppSession.RegisteredPatient.MRN);
                        if (!oPatient.IsHasAllergy)
                        {
                            oPatient.IsHasAllergy = true;
                            oPatient.LastUpdatedBy = AppSession.UserLogin.UserID;
                            patientDao.Update(oPatient);
                        }

                        result = "1|add|1";
                    }
                    else if (param[0] == "edit")
                    {
                        int allergyID = Convert.ToInt32(hdnAllergyID.Value);
                        oAllergy = allergyDao.Get(allergyID);

                        if (oAllergy != null)
                        {
                            oAllergy.GCAllergenType = cboAllergenType.Value.ToString();
                            oAllergy.Allergen = txtAllergenName.Text;
                            oAllergy.Reaction = txtReaction.Text;
                            oAllergy.LastUpdatedBy = AppSession.UserLogin.UserID;
                            allergyDao.Update(oAllergy);

                            result = "1|edit|1";
                        }
                        else
                        {
                            result = string.Format("0|delete|{0}", "Invalid Patient Allergy Record Information");
                            isError = true;
                        }
                    }
                    else
                    {
                        int allergyID = Convert.ToInt32(hdnAllergyID.Value);
                        oAllergy = BusinessLayer.GetPatientAllergy(allergyID);

                        if (oAllergy != null)
                        {
                            //TODO : Prompt user for delete reason
                            oAllergy.IsDeleted = true;
                            oAllergy.LastUpdatedBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdatePatientAllergy(oAllergy);
                            result = "1|delete|";
                            string isHasAllergy = "0";

                            List<PatientAllergy> lstAllergy = BusinessLayer.GetPatientAllergyList(string.Format("MRN = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.MRN), ctx);
                            if (lstAllergy.Count > 0)
                            {
                                Patient oPatient = patientDao.Get(AppSession.RegisteredPatient.MRN);
                                if (!oPatient.IsHasAllergy)
                                {
                                    oPatient.IsHasAllergy = true;
                                    oPatient.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    patientDao.Update(oPatient);
                                }

                                isHasAllergy = "1";
                            }
                            else
                            {
                                Patient oPatient = patientDao.Get(AppSession.RegisteredPatient.MRN);
                                if (oPatient.IsHasAllergy)
                                {
                                    oPatient.IsHasAllergy = false;
                                    oPatient.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    patientDao.Update(oPatient);
                                }

                                isHasAllergy = "0";
                            }
                            result = "1|delete|" + isHasAllergy;
                        }
                        else
                        {
                            result = string.Format("0|edit|{0}", "Invalid Patient Allergy Record Information");
                        }
                    }

                    if (!isError)
                        ctx.CommitTransaction();
                    else
                        ctx.RollBackTransaction();
                }

            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                result = string.Format("0|process|{0}", ex.Message);
            }
            finally
            {
                ctx.Close();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        #region Vital Sign
        private void BindGridViewVitalSign(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Empty;

            ///string visitNoteID = !string.IsNullOrEmpty(_visitNoteID) ? _visitNoteID : "0";
            string visitNoteID = hdnPatientVisitNoteID.Value; ////// !string.IsNullOrEmpty(_visitNoteID) ? _visitNoteID : "0";

            filterExpression += string.Format("VisitID = {0} AND NurseAssessmentID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvVitalSignHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_COMPACT);
            }

            List<vVitalSignHd> lstEntity = BusinessLayer.GetvVitalSignHdList(filterExpression, Constant.GridViewPageSize.GRID_COMPACT, pageIndex, "ID DESC");
            lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID IN ({0}) AND NurseAssessmentID = {1} ORDER BY DisplayOrder", AppSession.RegisteredPatient.VisitID, hdnID.Value));
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
            string filterExpression = string.Format("VisitID = {0} AND NurseAssessmentID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvReviewOfSystemHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vReviewOfSystemHd> lstEntity = BusinessLayer.GetvReviewOfSystemHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
            lstReviewOfSystemDt = BusinessLayer.GetvReviewOfSystemDtList(string.Format("VisitID = {0} AND NurseAssessmentID = {1}", AppSession.RegisteredPatient.VisitID, hdnID.Value));
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
    }
}
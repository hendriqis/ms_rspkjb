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
    public partial class ERInitialAssessmentEntry1 : BasePagePatientPageList
    {
        string menuType = string.Empty;
        protected int gridVitalSignPageCount = 1;
        protected int gridDiagnosisPageCount = 1;
        protected int gridEducationPageCount = 1;
        protected int gridROSPageCount = 1;
        protected List<vPatientEducationDt> lstPatientEducationDt = null;
        protected List<vVitalSignDt> lstVitalSignDt = null;
        protected List<vReviewOfSystemDt> lstReviewOfSystemDt = null;
        protected static string _visitNoteID;

        public override string OnGetMenuCode()
        {
            if (menuType == "fo")
            {
                return Constant.MenuCode.EmergencyCare.FOLLOWUP_PATIENT_PAGE_NURSE_INITIAL_ASSESSMENT;
            }
            else
            {
                return Constant.MenuCode.EmergencyCare.PATIENT_PAGE_EMERGENCY_INITIAL_ASSESSMENT;
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
            if (entityConsultVisit.VisitTypeID != 0)
            {
                VisitType vt = BusinessLayer.GetVisitType(entityConsultVisit.VisitTypeID);
                txtVisitTypeCode.Text = vt.VisitTypeCode;
                txtVisitTypeName.Text = vt.VisitTypeName;
            }

            List<SettingParameterDt> lstSettingParameter = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode = ('{0}')", Constant.SettingParameter.IS_PENDAFTARAN_DENGAN_RUANG));
            hdnIsEmergencyUsingRoom.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_PENDAFTARAN_DENGAN_RUANG).ParameterValue;

            if (entityConsultVisit.BedID == null)
            {
                if (hdnIsEmergencyUsingRoom.Value == "0")
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

            PatientVisitNote oVisitNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID IN ({0}) AND GCPatientNoteType = '{1}' AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, Constant.PatientVisitNotes.NURSE_INITIAL_ASSESSMENT)).FirstOrDefault();
            if (oVisitNote != null)
            {
                hdnPatientVisitNoteID.Value = oVisitNote.ID.ToString();
            }
            _visitNoteID = hdnPatientVisitNoteID.Value;

            BindGridViewDiagnosis(1, true, ref gridDiagnosisPageCount);
            BindGridViewEducation(1, true, ref gridEducationPageCount);
            BindGridViewVitalSign(1, true, ref gridVitalSignPageCount);
            BindGridViewROS(1, true, ref gridROSPageCount);
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        protected override void SetControlProperties()
        {
            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}') AND IsActive = 1 AND IsDeleted = 0",
                Constant.StandardCode.ONSET, Constant.StandardCode.EXACERBATED, Constant.StandardCode.QUALITY,
                Constant.StandardCode.SEVERITY, Constant.StandardCode.COURSE_TIMING, Constant.StandardCode.RELIEVED_BY,
                Constant.StandardCode.TRIAGE, Constant.StandardCode.VISIT_REASON, Constant.StandardCode.ADMISSION_CONDITION,
                Constant.StandardCode.AIRWAY, Constant.StandardCode.BREATHING, Constant.StandardCode.CIRCULATION,
                Constant.StandardCode.DISABILITY, Constant.StandardCode.EXPOSURE, Constant.StandardCode.ADMISSION_ROUTE,
                Constant.StandardCode.FUNCTIONAL_TYPE,
                //Constant.StandardCode.PATIENT_EDUCATION_TYPE,
                Constant.StandardCode.PSYCHOLOGY_STATUS,
                Constant.StandardCode.RAPUH_RESISTENSI,
                Constant.StandardCode.RAPUH_AKTIFITAS,
                Constant.StandardCode.RAPUH_PENYAKIT,
                Constant.StandardCode.RAPUH_USAHA_BERJALAN,
                Constant.StandardCode.RAPUH_BERAT_BADAN,
                Constant.StandardCode.RAPUH_SCORE,
                Constant.StandardCode.FAMILY_RELATION,
                Constant.StandardCode.VISIT_CASE_TYPE
                );
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
            Methods.SetComboBoxField<StandardCode>(cboFunctionalType, lstSc.Where(p => p.ParentID == Constant.StandardCode.FUNCTIONAL_TYPE || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboPsychologyStatus, lstSc.Where(p => p.ParentID == Constant.StandardCode.PSYCHOLOGY_STATUS || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            //Methods.SetComboBoxField<StandardCode>(cboPatientEducationType, lstSc.Where(p => p.ParentID == Constant.StandardCode.PATIENT_EDUCATION_TYPE || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboRAPUH_R, lstSc.Where(p => p.ParentID == Constant.StandardCode.RAPUH_RESISTENSI || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboRAPUH_A, lstSc.Where(p => p.ParentID == Constant.StandardCode.RAPUH_AKTIFITAS || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboRAPUH_P, lstSc.Where(p => p.ParentID == Constant.StandardCode.RAPUH_PENYAKIT || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboRAPUH_U, lstSc.Where(p => p.ParentID == Constant.StandardCode.RAPUH_USAHA_BERJALAN || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboRAPUH_H, lstSc.Where(p => p.ParentID == Constant.StandardCode.RAPUH_BERAT_BADAN || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboRAPUHScore, lstSc.Where(p => p.ParentID == Constant.StandardCode.RAPUH_SCORE || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboFamilyRelation, lstSc.Where(p => p.ParentID == Constant.StandardCode.FAMILY_RELATION || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboVisitCaseType, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.VISIT_CASE_TYPE || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");


            vNurseChiefComplaint entity = BusinessLayer.GetvNurseChiefComplaintList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            PatientVisitNote oPatientVisitNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = '{0}' AND GCPatientNoteType = '{1}'", AppSession.RegisteredPatient.VisitID, Constant.PatientVisitNotes.NURSE_INITIAL_ASSESSMENT)).FirstOrDefault();
            ConsultVisit entityCV = BusinessLayer.GetConsultVisit(AppSession.RegisteredPatient.VisitID);
            if (entityCV.SpecialtyID != null && entityCV.SpecialtyID != "0" && entityCV.SpecialtyID != "")
            {
                if (entityCV.GCCaseType != null)
                {
                    cboVisitCaseType.Value = entityCV.GCCaseType;
                }
                else
                {
                    Specialty entitySpecialty = BusinessLayer.GetSpecialty(entityCV.SpecialtyID);
                    if (entitySpecialty.GCCaseType != null)
                    {
                        cboVisitCaseType.Value = entitySpecialty.GCCaseType;
                    }
                    else
                    {
                        cboVisitCaseType.Value = "";
                    }
                }
            }

            if (oPatientVisitNote != null)
                hdnPatientVisitNoteID.Value = oPatientVisitNote.ID.ToString();
            else
                hdnPatientVisitNoteID.Value = "0";

            vConsultVisit entityVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            if (entity != null)
            {
                EntityToControl(entity, entityVisit);
            }

            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;
            filterExpression = string.Format("GCParamedicMasterType != '{0}'", Constant.ParamedicType.Physician);

            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(filterExpression);
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");

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
            Helper.SetControlEntrySetting(cboVisitReason, new ControlEntrySetting(isEnabled, isEnabled, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboVisitCaseType, new ControlEntrySetting(isEnabled, isEnabled, false), "mpPatientStatus");
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
            Helper.SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(chkIsNeedAcuteInitialAssessment, new ControlEntrySetting(isEnabled, isEnabled, false, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(chkIsNeedChronicInitialAssessment, new ControlEntrySetting(isEnabled, isEnabled, false, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(chkIsNeedPatientEducation, new ControlEntrySetting(isEnabled, isEnabled, false, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboFunctionalType, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtFunctionalTypeRemarks, new ControlEntrySetting(isEnabled, isEnabled, false), "mpPatientStatus");
            //Helper.SetControlEntrySetting(cboPatientEducationType, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            //Helper.SetControlEntrySetting(txtPatientEducationRemarks, new ControlEntrySetting(isEnabled, isEnabled, false), "mpPatientStatus");
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
        }

        private void EntityToControl(vNurseChiefComplaint entity, vConsultVisit entityVisit)
        {
            hdnAssessmentParamedicID.Value = entity.ParamedicID.ToString();
            hdnRevisedByParamedicID.Value = entity.RevisedByParamedicID.ToString();
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

            cboFunctionalType.Value = entity.GCFunctionalType;
            txtFunctionalTypeRemarks.Text = entity.FunctionalTypeRemarks;

            chkIsNeedPatientEducation.Checked = entity.IsNeedPatientEducation;
            chkIsFastTrack.Checked = entity.IsFastTrack;
            //cboPatientEducationType.Value = entity.GCPatientEducationType;
            //txtPatientEducationRemarks.Text = entity.PatientEducationRemarks;

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

            #region table sebelah kiri

            #region Chief Complaint

            txtChiefComplaint.Text = entity.NurseChiefComplaintText;
            txtHPISummary.Text = entity.HPISummary;
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
            txtMedicalHistory.Text = entity.MedicalHistory;
            txtMedicationHistory.Text = entity.MedicationHistory;
            #endregion

            chkIsNeedAcuteInitialAssessment.Checked = entity.IsNeedAccuteInitialAssessment;
            chkIsNeedChronicInitialAssessment.Checked = entity.IsNeedChronicInitialAssessment;

            cboVisitReason.Value = entity.GCVisitReason;
            txtVisitNotes.Text = entity.VisitReason;

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

        private void ControlToEntity(NurseChiefComplaint entity, ConsultVisit entityVisit, Bed entityBed)
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
            if (cboFamilyRelation.Value != null && entity.IsAlloAnamnesis)
            {
                entity.GCFamilyRelation = cboFamilyRelation.Value.ToString();
            }
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

            entity.IsInitialAssessment = true;

            entity.IsNeedAccuteInitialAssessment = chkIsNeedAcuteInitialAssessment.Checked;
            entity.IsNeedChronicInitialAssessment = chkIsNeedChronicInitialAssessment.Checked;
            entity.IsInitialAssessment = (entity.IsNeedAccuteInitialAssessment || entity.IsNeedChronicInitialAssessment);
            entity.IsMedicalDiagnostic = false;

            if (cboFunctionalType.Value != null)
                entity.GCFunctionalType = cboFunctionalType.Value.ToString();
            if (!string.IsNullOrEmpty(txtFunctionalTypeRemarks.Text))
                entity.FunctionalTypeRemarks = txtFunctionalTypeRemarks.Text;

            entity.IsNeedPatientEducation = chkIsNeedPatientEducation.Checked;

            //if (cboPatientEducationType.Value != null)
            //    entity.GCPatientEducationType = cboPatientEducationType.Value.ToString();
            //if (!string.IsNullOrEmpty(txtPatientEducationRemarks.Text))
            //    entity.PatientEducationRemarks = txtPatientEducationRemarks.Text;

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

            entity.IsDeleted = false;

            #endregion

            #region Consult Visit
            entityVisit.ClassID = Convert.ToInt32(hdnClassID.Value);
            entityVisit.ChargeClassID = Convert.ToInt32(hdnChargeClassID.Value);
            if (cboVisitCaseType.Value != null && cboVisitCaseType.Value != "")
            {
                entityVisit.GCCaseType = cboVisitCaseType.Value.ToString();
            }
            else
            {
                entityVisit.GCCaseType = null;
            }
            if (hdnIsEmergencyUsingRoom.Value == "1")
            {
                entityVisit.RoomID = Convert.ToInt32(hdnRoomID.Value);
                entityVisit.BedID = Convert.ToInt32(hdnBedID.Value);
                if (hdnBedID.Value != "0")
                {
                    entityBed.RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
                    entityBed.GCBedStatus = Constant.BedStatus.OCCUPIED;
                }
            }
            else
            {
                entityVisit.RoomID = null;
                entityVisit.BedID = null;
            }
            #endregion
        }

        private string GenerateSOAPText()
        {
            StringBuilder sbNotes = new StringBuilder();
            StringBuilder sbSubjective = new StringBuilder();
            sbNotes.AppendLine("SUBJEKTIVE :");
            sbNotes.AppendLine("-".PadRight(20, '-'));

            sbNotes.AppendLine(string.Format("Anamnesa Perawat  : "));
            sbNotes.AppendLine(string.Format(" {0}   ", txtChiefComplaint.Text));

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

            sbNotes.AppendLine(" ");
            sbNotes = new StringBuilder();
            sbNotes.AppendLine("OBJEKTIF :");
            sbNotes.AppendLine("-".PadRight(20, '-'));

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

            List<vVitalSignDt> lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID = {0} AND ObservationDate = '{1}' AND IsDeleted = 0 ORDER BY DisplayOrder", AppSession.RegisteredPatient.VisitID, Helper.GetDatePickerValue(txtServiceDate).ToString(Constant.FormatString.DATE_FORMAT_112)));
            if (lstVitalSignDt.Count > 0)
            {
                sbNotes.AppendLine(" ");
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

        private void UpdateConsultVisitRegistration(IDbContext ctx, List<NurseChiefComplaint> lstNCC, ref string chiefComplaintID)
        {
            NurseChiefComplaintDao chiefComplaintDao = new NurseChiefComplaintDao(ctx);
            ConsultVisitDao consultVisitDao = new ConsultVisitDao(ctx);
            RegistrationDao registrationDao = new RegistrationDao(ctx);
            ConsultVisit entityConsultVisit = consultVisitDao.Get(AppSession.RegisteredPatient.VisitID);
            entityConsultVisit.VisitTypeID = Convert.ToInt32(hdnVisitTypeID.Value);
            entityConsultVisit.ActualVisitDate = Helper.GetDatePickerValue(txtRegistrationDate);
            entityConsultVisit.ActualVisitTime = txtRegistrationTime.Text;
            if (entityConsultVisit.GCVisitStatus == Constant.VisitStatus.CHECKED_IN)
            {
                entityConsultVisit.GCVisitStatus = Constant.VisitStatus.RECEIVING_TREATMENT;
                entityConsultVisit.StartServiceDate = Helper.GetDatePickerValue(txtServiceDate);
                entityConsultVisit.StartServiceTime = string.Format("{0}:{1}", txtServiceTime1.Text, txtServiceTime2.Text);
            }
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

            if (cboVisitCaseType.Value != null && cboVisitCaseType.Value != "")
            {
                entityConsultVisit.GCCaseType = cboVisitCaseType.Value.ToString();
            }
            else
            {
                entityConsultVisit.GCCaseType = null;
            }

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

            #region Nurse Chief Complaint
            NurseChiefComplaint entity = null;
            bool isNewChiefComplaint = true;

            if (lstNCC.Count > 0)
            {
                entity = lstNCC.FirstOrDefault();
                isNewChiefComplaint = false;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
            }
            else
            {
                entity = new NurseChiefComplaint();
                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                entity.GCAssessmentStatus = Constant.AssessmentStatus.OPEN;
                entity.CreatedBy = AppSession.UserLogin.UserID;
            }

            entity.PatientVisitNoteID = Convert.ToInt32(hdnPatientVisitNoteID.Value);
            entity.ChiefComplaintDate = Helper.GetDatePickerValue(txtServiceDate);
            entity.ChiefComplaintTime = string.Format("{0}:{1}", txtServiceTime1.Text, txtServiceTime2.Text);
            entity.NurseChiefComplaintText = txtChiefComplaint.Text;
            entity.ParamedicID = Convert.ToInt16(AppSession.UserLogin.ParamedicID);
            entity.HPISummary = txtHPISummary.Text;
            entity.MedicalHistory = txtMedicalHistory.Text;
            entity.MedicationHistory = txtMedicationHistory.Text;
            // entity.FamilyHistory = txtFamilyHistory.Text;
            entity.IsPatientAllergyExists = !chkIsPatientAllergyExists.Checked;
            entity.IsInitialAssessment = (entity.IsNeedAccuteInitialAssessment || entity.IsNeedChronicInitialAssessment);
            entity.IsMedicalDiagnostic = true;
            entity.IsAutoAnamnesis = Convert.ToBoolean(chkAutoAnamnesis.Checked);
            entity.IsAlloAnamnesis = Convert.ToBoolean(chkAlloAnamnesis.Checked);
            if (cboFamilyRelation.Value != null)
            {
                entity.GCFamilyRelation = cboFamilyRelation.Value.ToString();
            }

            if (isNewChiefComplaint)
            {
                hdnChiefComplaintID.Value = chiefComplaintDao.InsertReturnPrimaryKeyID(entity).ToString();
                chiefComplaintID = hdnChiefComplaintID.Value;
            }
            else
            {
                chiefComplaintDao.Update(entity);
                chiefComplaintID = entity.ID.ToString();
            }
            #endregion
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            string chiefComplaintID = string.Empty;
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
                        if (Convert.ToInt32(hdnRevisedByParamedicID.Value) != AppSession.UserLogin.ParamedicID)
                        {
                            if (Convert.ToInt32(hdnAssessmentParamedicID.Value) != AppSession.UserLogin.ParamedicID)
                            isAllowSave = false;
                        }
                    }

                    if (isAllowSave)
                    {
                        Int32 objID = 0;
                        NurseChiefComplaint obj = BusinessLayer.GetNurseChiefComplaintList(string.Format("VisitID = {0} AND IsMedicalDiagnostic = 0 AND IsInitialAssessment = 1 AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
                        if (obj != null)
                        {
                            objID = obj.ID;
                            hdnChiefComplaintID.Value = obj.ID.ToString();
                        }
                        else
                        {
                            hdnChiefComplaintID.Value = "0";
                        }

                        if (objID == Convert.ToInt32(hdnChiefComplaintID.Value))
                        {
                            PatientVisitNote entityNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = {0} AND GCPatientNoteType = '{1}'", AppSession.RegisteredPatient.VisitID, Constant.PatientVisitNotes.NURSE_INITIAL_ASSESSMENT), ctx).FirstOrDefault();
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

                            if (String.IsNullOrEmpty(txtChiefComplaint.Text))
                            {
                                errMessage = "Mohon isi Keluhan Utama Pasien - Anamnesa Perawat!";
                                result = false;
                            }
                            else if (cboAdmissionRoute.Value == null)
                            {
                                errMessage = "Mohon isi Triase & Survey Premier - Cara Datang!";
                                result = false;
                            }
                            else if (cboAdmissionCondition.Value == null)
                            {
                                errMessage = "Mohon isi Triase & Survey Premier - Keadaan Datang!";
                                result = false;
                            }
                            else if (cboTriage.Value == null)
                            {
                                errMessage = "Mohon isi Triase & Survey Premier - Triage!";
                                result = false;
                            }
                            else if (cboAirway.Value == null)
                            {
                                errMessage = "Mohon isi Triase & Survey Premier - Airway!";
                                result = false;
                            }
                            else if (cboBreathing.Value == null)
                            {
                                errMessage = "Mohon isi Triase & Survey Premier - Breathing!";
                                result = false;
                            }
                            else if (cboCirculation.Value == null)
                            {
                                errMessage = "Mohon isi Triase & Survey Premier - Circulation!";
                                result = false;
                            }
                            else if (cboDisability.Value == null)
                            {
                                errMessage = "Mohon isi Triase & Survey Premier - Disability!";
                                result = false;
                            }
                            else if (cboExposure.Value == null)
                            {
                                errMessage = "Mohon isi Triase & Survey Premier - Exposure!";
                                result = false;
                            }
                            else
                            {
                                List<NurseChiefComplaint> lstNCC = BusinessLayer.GetNurseChiefComplaintList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID), ctx);

                                UpdateConsultVisitRegistration(ctx, lstNCC, ref chiefComplaintID);
                                OnSaveAddEditPatientStatus(ctx, ref chiefComplaintID, ref errMessage);

                                _visitNoteID = hdnPatientVisitNoteID.Value;

                                errMessage = hdnPatientVisitNoteID.Value + ";" + chiefComplaintID;
                            }

                            ctx.CommitTransaction();

                            hdnIsSaved.Value = "1";
                            hdnIsChanged.Value = "0";
                        }
                        else
                        {
                            errMessage = "Hanya boleh ada 1 kajian awal pasien rawat inap dalam 1 episode perawatan.";
                            hdnIsChanged.Value = "0";

                            ctx.RollBackTransaction();
                            result = false;
                        }
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
            entityNote.HealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
            entityNote.InstructionText = txtInstructionText.Text;
            entityNote.NoteDate = Helper.GetDatePickerValue(txtServiceDate);
            entityNote.NoteTime = string.Format("{0}:{1}", txtServiceTime1.Text, txtServiceTime2.Text);
        }

        private void OnSaveAddEditPatientStatus(IDbContext ctx, ref string chiefComplaintID, ref string errMessage)
        {
            NurseChiefComplaintDao chiefComplaintDao = new NurseChiefComplaintDao(ctx);
            PatientVisitNoteDao patientVisitNoteDao = new PatientVisitNoteDao(ctx);
            ConsultVisitDao patientVisitDao = new ConsultVisitDao(ctx);
            BedDao BedDao = new BedDao(ctx);
            PatientDao patientDao = new PatientDao(ctx);
            bool isChiefComplaintNull = false;
            bool isEntityEmergencyCaseNoteNull = false;

            List<NurseChiefComplaint> lstNCC = BusinessLayer.GetNurseChiefComplaintList(string.Format("VisitID = {0} AND IsRevised = 0 AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID), ctx);

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

            ConsultVisit entityVisit = BusinessLayer.GetConsultVisitList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID), ctx).FirstOrDefault();
            Bed entityBed = BusinessLayer.GetBedList(string.Format("BedID = {0}", hdnBedID.Value), ctx).FirstOrDefault();

            ControlToEntity(entity, entityVisit, entityBed);

            if (isChiefComplaintNull)
            {
                hdnID.Value = chiefComplaintDao.InsertReturnPrimaryKeyID(entity).ToString();
            }
            else
            {
                chiefComplaintDao.Update(entity);
            }

            if (isEntityEmergencyCaseNoteNull)
            {
                patientVisitDao.Update(entityVisit);
                if (hdnIsEmergencyUsingRoom.Value == "1")
                {
                    BedDao.Update(entityBed);
                }
            }
            else
            {
                patientVisitDao.Update(entityVisit);
                if (hdnIsEmergencyUsingRoom.Value == "1")
                {
                    BedDao.Update(entityBed);
                }
            }

            //if (isEntityPatientDiagnosisNull)
            //{
            //    if ((patientDiagnosis.DiagnoseID != "" && patientDiagnosis.DiagnoseID != null) || (patientDiagnosis.DiagnosisText.Trim() != "" && patientDiagnosis.DiagnosisText.Trim() != null))
            //    {
            //        patientDiagnosisDao.Insert(patientDiagnosis);
            //    }
            //}
            //else
            //{
            //    patientDiagnosisDao.Update(patientDiagnosis);
            //}

            #region Patient Status
            if (chkIsNeedAcuteInitialAssessment.Checked || chkIsNeedChronicInitialAssessment.Checked)
            {
                Patient oPatient = patientDao.Get(AppSession.RegisteredPatient.MRN);
                if (oPatient != null)
                {
                    if (chkIsNeedAcuteInitialAssessment.Checked)
                        oPatient.LastAcuteInitialAssessmentDate = Helper.GetDatePickerValue(txtServiceDate);
                    if (chkIsNeedChronicInitialAssessment.Checked)
                        oPatient.LastChronicInitialAssessmentDate = Helper.GetDatePickerValue(txtServiceDate);

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
            //StringBuilder strDiagnosis = new StringBuilder();
            //foreach (var item in lstEntity)
            //{
            //    strDiagnosis.AppendLine(string.Format("- {0}", item.ProblemName));
            //}
            //hdnDiagnosisSummary.Value = strDiagnosis.ToString();
            //hdnAssessmentText.Value = hdnDiagnosisSummary.Value;

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

            string visitNoteID = hdnPatientVisitNoteID.Value; ////// !string.IsNullOrEmpty(_visitNoteID) ? _visitNoteID : "0";

            filterExpression += string.Format("VisitID IN ({0}) AND PatientVisitNoteID = {1} AND IsInitialAssessment = 1 AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, visitNoteID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvVitalSignHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_COMPACT);
            }

            List<vVitalSignHd> lstEntity = BusinessLayer.GetvVitalSignHdList(filterExpression, Constant.GridViewPageSize.GRID_COMPACT, pageIndex, "ID DESC");
            lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID IN ({0}) AND PatientVisitNoteID = {1} AND IsInitialAssessment = 1 ORDER BY DisplayOrder", AppSession.RegisteredPatient.VisitID, visitNoteID));
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
    }
}
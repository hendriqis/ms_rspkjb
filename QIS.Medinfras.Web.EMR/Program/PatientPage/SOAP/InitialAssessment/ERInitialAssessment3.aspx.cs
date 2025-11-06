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
using QIS.Medinfras.Web.CommonLibs.Service;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class ERInitialAssessment3 : BasePagePatientPageList
    {
        protected int gridAllergyPageCount = 1;
        protected int gridVitalSignPageCount = 1;
        protected int gridROSPageCount = 1;
        protected int gridDiagnosisPageCount = 1;
        protected int gridProcedurePageCount = 1;
        protected int gridLaboratoryPageCount = 1;
        protected int gridImagingPageCount = 1;
        protected int gridDiagnosticPageCount = 1;
        protected List<vVitalSignDt> lstVitalSignDt = null;
        protected List<vReviewOfSystemDt> lstReviewOfSystemDt = null;
        protected static string _visitNoteID;

        protected string GetUserID()
        {
            return AppSession.UserLogin.UserID.ToString();
        }

        protected string GetVisitNoteID()
        {
            return _visitNoteID;
        }

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.EMERGENCY_SOAP_INITIAL_ASSESSMENT_2;
        }

        protected override void InitializeDataControl()
        {
            Helper.SetControlEntrySetting(cboTriage, new ControlEntrySetting(true, true, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboAirway, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboBreathing, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboCirculation, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboDisability, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboExposure, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboAdmissionRoute, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboAdmissionCondition, new ControlEntrySetting(true, true, false), "mpPatientStatus");

            Helper.SetControlEntrySetting(cboVisitCaseType, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboVisitType, new ControlEntrySetting(true, true, true), "mpPatientStatus");

            Helper.SetControlEntrySetting(txtHPISummary, new ControlEntrySetting(true, true, false), "mpPatientStatus");

            List<SettingParameterDt> lstSettingParameter = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM, Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI, Constant.SettingParameter.EM0063, Constant.SettingParameter.EM_IS_ASSESMENT_NON_INPATIENT_DEFAULT_USING_REGISTRATION_DATE));
            hdnImagingServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).ParameterValue;
            hdnLaboratoryServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).ParameterValue;
            hdnIsHPIShow.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.EM0063).ParameterValue;
            hdnAssessmentDateIsUsingRegDate.Value = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.EM_IS_ASSESMENT_NON_INPATIENT_DEFAULT_USING_REGISTRATION_DATE).FirstOrDefault().ParameterValue;

            if (hdnIsHPIShow.Value == "1")
            {
                trHPI.Style.Remove("display");
            }
            else
            {
                trHPI.Style.Add("display", "none");
            }

            string hsuFilterExp = string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsLaboratoryUnit=1 AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, Constant.Facility.DIAGNOSTIC);
            List<vHealthcareServiceUnitCustom> lstServiceUnit = BusinessLayer.GetvHealthcareServiceUnitCustomList(hsuFilterExp);
            Methods.SetComboBoxField<vHealthcareServiceUnitCustom>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
            cboServiceUnit.SelectedIndex = 0;

            hdnDatePickerToday.Value = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnTimeToday.Value = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            SetEntityToControl();

            hdnPatientVisitNoteID.Value = _visitNoteID;

            BindGridViewAllergy(1, true, ref gridAllergyPageCount);
            BindGridViewVitalSign(1, true, ref gridVitalSignPageCount);
            BindGridViewROS(1, true, ref gridROSPageCount);
            BindGridViewDiagnosis(1, true, ref gridDiagnosisPageCount);
            BindGridViewProcedure(1, true, ref gridProcedurePageCount);
            BindGridViewLaboratory(1, true, ref gridLaboratoryPageCount);
            BindGridViewImaging(1, true, ref gridImagingPageCount);
            BindGridViewDiagnostic(1, true, ref gridDiagnosticPageCount);

            LoadBodyDiagram();

            hdnIsChanged.Value = "0";
        }

        private void SetEntityToControl()
        {
            vConsultVisit1 entityVisit = BusinessLayer.GetvConsultVisit1List(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            ConsultVisit entityVisit2 = BusinessLayer.GetConsultVisitList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            hdnMRN.Value = entityVisit.MRN.ToString();
            hdnVisitID.Value = entityVisit.VisitID.ToString();
            hdnRegistrationID.Value = entityVisit.RegistrationID.ToString();
            hdnHealthcareServiceUnitID.Value = entityVisit.HealthcareServiceUnitID.ToString();
            hdnDepartmentID.Value = entityVisit.DepartmentID;
            hdnParamedicID.Value = entityVisit.ParamedicID.ToString();

            if (cboVisitCaseType.Value != null && cboVisitCaseType.Value != "")
            {
                entityVisit2.GCCaseType = cboVisitCaseType.Value.ToString();
            }
            else
            {
                entityVisit2.GCCaseType = null;
            }

            hdnPatientInformation.Value = string.Format("{0} (MRN = {1}, REG = {2}, LOC = {3}, DOB = {4})", entityVisit.cfPatientNameInLabel, entityVisit.MedicalNo, entityVisit.RegistrationNo, entityVisit.ServiceUnitName, entityVisit.DateOfBirthInString);

            cboVisitType.Value = entityVisit.VisitTypeID.ToString();

            List<PatientVisitNote> lstPatientVisitNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = '{0}' AND GCPatientNoteType = '{1}' AND IsDeleted = 0", hdnVisitID.Value, Constant.PatientVisitNotes.EMERGENCY_INITIAL_ASSESSMENT));

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

            hdnDepartmentID.Value = entityVisit.DepartmentID;

            vNurseChiefComplaint entity = BusinessLayer.GetvNurseChiefComplaintList(string.Format("VisitID = {0}", hdnVisitID.Value)).FirstOrDefault();

            string medicalHistory = string.Empty;
            string medicationHistory = string.Empty;

            if (entity != null)
            {
                txtNurseAnamnesis.Text = entity.NurseChiefComplaintText;
                cboAdmissionRoute.Value = entity.GCAdmissionRoute;
                cboAirway.Value = entity.GCAirway;
                cboBreathing.Value = entity.GCBreathing;
                cboCirculation.Value = entity.GCCirculation;
                cboDisability.Value = entity.GCDisability;
                cboExposure.Value = entity.GCExposure;
                cboAdmissionCondition.Value = entity.GCAdmissionCondition;

                medicalHistory = entity.MedicalHistory;
                medicationHistory = entity.MedicationHistory;
            }

            if (lstPatientVisitNote.Count > 0)
            {
                PatientVisitNote entitypvn = lstPatientVisitNote.First();
                EntityToControl(entitypvn);
            }
            else
            {
                _visitNoteID = "0";
            }

            vRegistration entityRegistration = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", entityVisit.RegistrationID)).FirstOrDefault();
            cboTriage.Value = entityRegistration.GCTriage;
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

            cboVisitReason.Value = entityVisit.GCVisitReason;
            txtVisitNotes.Text = entityVisit.VisitReason;
            cboAdmissionCondition.Value = entityVisit.GCAdmissionCondition;

            vChiefComplaint obj = BusinessLayer.GetvChiefComplaintList(string.Format("VisitID = {0}", hdnVisitID.Value)).FirstOrDefault();
            if (obj != null)
            {
                if (obj.ID == 0)
                {
                    hdnChiefComplaintID.Value = "0";
                    hdnChiefComplaintParamedicID.Value = "0";
                    txtMedicalHistory.Text = entity.MedicalHistory;
                    txtMedicationHistory.Text = entity.MedicationHistory;
                    chkIsPatientAllergyExists.Checked = entity.IsPatientAllergyExists;
                }
                else
                {
                    hdnChiefComplaintID.Value = obj.ID.ToString();
                    hdnChiefComplaintParamedicID.Value = obj.ParamedicID.ToString();
                    txtServiceDate.Text = obj.ObservationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtServiceTime.Text = obj.ObservationTime;
                    txtChiefComplaint.Text = obj.ChiefComplaintText;
                    txtHPISummary.Text = obj.HPISummary;
                    txtMedicalHistory.Text = obj.PastMedicalHistory;
                    txtMedicationHistory.Text = obj.PastMedicationHistory;
                    txtDiagnosticResultSummary.Text = obj.DiagnosticResultSummary;
                    txtPlanningNotes.Text = obj.PlanningSummary;
                    chkIsPatientAllergyExists.Checked = !chkIsPatientAllergyExists.Checked;
                    chkAlloAnamnesis.Checked = obj.IsAlloAnamnesis;
                    chkAutoAnamnesis.Checked = obj.IsAutoAnamnesis;

                    if (String.IsNullOrEmpty(obj.PastMedicalHistory))
                        txtMedicationHistory.Text = entity.MedicalHistory;
                    else
                        txtMedicationHistory.Text = obj.PastMedicalHistory;

                    if (String.IsNullOrEmpty(obj.PastMedicationHistory))
                        txtMedicationHistory.Text = entity.MedicationHistory;
                    else
                        txtMedicationHistory.Text = obj.PastMedicationHistory;
                }
                txtMedicalProblem.Text = obj.MedicalProblem;
            }
            else
            {
                txtMedicalHistory.Text = medicalHistory;
                txtMedicationHistory.Text = medicationHistory;
            }

            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", hdnVisitID.Value);
            MSTAssessment mst = BusinessLayer.GetMSTAssessmentList(filterExpression).FirstOrDefault();
            if (mst != null)
            {
                hdnMSTAssessmentID.Value = mst.ID.ToString();
                cboGCWeightChangedStatus.Value = mst.GCWeightChangedStatus;
                cboGCWeightChangedGroup.Value = mst.GCWeightChangedGroup;
                cboGCMSTDiagnosis.Value = mst.GCMSTDiagnosis;

                if (mst.GCFoodIntakeChanged != null)
                    rblIsFoodIntakeChanged.SelectedValue = mst.GCFoodIntakeChanged == "X450^01" ? "1" : "0";
                else
                    rblIsFoodIntakeChanged.SelectedValue = "0";
                txtFoodIntakeScore.Text = rblIsFoodIntakeChanged.SelectedValue;

                if (mst.IsHasSpecificDiagnosis == true)
                {
                    rblIsHasSpecificDiagnosis.SelectedValue = "1";
                }
                else
                {
                    rblIsHasSpecificDiagnosis.SelectedValue = "0";
                }

                if (mst.IsReadedByNutritionist == true)
                {
                    rblIsReadedByNutritionist.SelectedValue = "1";
                }
                else
                {
                    rblIsReadedByNutritionist.SelectedValue = "0";
                }

                txtOtherMSTDiagnosis.Text = mst.OtherMSTDiagnosis;
                txtTotalMST.Text = mst.MSTScore.ToString();
            }
            else
            {
                hdnMSTAssessmentID.Value = "0";
                cboGCWeightChangedStatus.Value = null;
                cboGCWeightChangedGroup.Value = null;
                cboGCMSTDiagnosis.Value = null;
                rblIsFoodIntakeChanged.SelectedValue = null;
                rblIsHasSpecificDiagnosis.SelectedValue = null;
                rblIsReadedByNutritionist.SelectedValue = null;
                txtOtherMSTDiagnosis.Text = string.Empty;
                txtFoodIntakeScore.Text = "0";
                txtTotalMST.Text = "0";
            }
        }

        private void LoadBodyDiagram()
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", hdnVisitID.Value);
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
            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}') AND IsActive = 1 AND IsDeleted = 0",
                Constant.StandardCode.TRIAGE, Constant.StandardCode.VISIT_REASON, Constant.StandardCode.ADMISSION_CONDITION,
                Constant.StandardCode.REFERRAL, Constant.StandardCode.TRIAGE,
    Constant.StandardCode.VISIT_REASON, Constant.StandardCode.ADMISSION_CONDITION,
    Constant.StandardCode.ALLERGEN_TYPE, Constant.StandardCode.DIAGNOSIS_TYPE,
    Constant.StandardCode.DIFFERENTIAL_DIAGNOSIS_STATUS, Constant.StandardCode.AIRWAY,
    Constant.StandardCode.BREATHING, Constant.StandardCode.CIRCULATION, Constant.StandardCode.DISABILITY,
    Constant.StandardCode.EXPOSURE, Constant.StandardCode.ADMISSION_ROUTE, Constant.StandardCode.PATIENT_INSTRUCTION_GROUP,
    Constant.StandardCode.MST_WEIGHT_CHANGED, Constant.StandardCode.MST_WEIGHT_CHANGED_GROUP, Constant.StandardCode.MST_DIAGNOSIS, Constant.StandardCode.VISIT_CASE_TYPE);

            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(filterExpression);

            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField(cboTriage, lstSc.Where(p => p.ParentID == Constant.StandardCode.TRIAGE || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboAdmissionCondition, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.ADMISSION_CONDITION || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboVisitReason, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.VISIT_REASON || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboReferral, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.REFERRAL || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboAirway, lstSc.Where(p => p.ParentID == Constant.StandardCode.AIRWAY || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboBreathing, lstSc.Where(p => p.ParentID == Constant.StandardCode.BREATHING || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboCirculation, lstSc.Where(p => p.ParentID == Constant.StandardCode.CIRCULATION || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDisability, lstSc.Where(p => p.ParentID == Constant.StandardCode.DISABILITY || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboExposure, lstSc.Where(p => p.ParentID == Constant.StandardCode.EXPOSURE || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboAdmissionRoute, lstSc.Where(p => p.ParentID == Constant.StandardCode.ADMISSION_ROUTE || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboAdmissionCondition, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.ADMISSION_CONDITION || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboAllergenType, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.ALLERGEN_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboDiagnosisType, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.DIAGNOSIS_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboDiagnosisStatus, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.DIFFERENTIAL_DIAGNOSIS_STATUS).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboGCWeightChangedStatus, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.MST_WEIGHT_CHANGED || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboGCWeightChangedGroup, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.MST_WEIGHT_CHANGED_GROUP || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboGCMSTDiagnosis, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.MST_DIAGNOSIS || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

            Methods.SetComboBoxField<StandardCode>(cboVisitCaseType, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.VISIT_CASE_TYPE || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
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

            string defaultInstruction = lstSc.Where(sc => sc.ParentID == Constant.StandardCode.PATIENT_INSTRUCTION_GROUP && sc.IsDefault == true).FirstOrDefault().StandardCodeID;
            hdnDefaultInstructionType.Value = !string.IsNullOrEmpty(defaultInstruction) ? "X139^003" : defaultInstruction;

            cboAllergenType.Value = Constant.AllergenType.DRUG;
            cboDiagnosisStatus.Value = Constant.DifferentialDiagnosisStatus.UNDER_INVESTIGATION;

            List<GetParamedicVisitTypeList> visitTypeList = BusinessLayer.GetParamedicVisitTypeList(Convert.ToInt32(hdnHealthcareServiceUnitID.Value), (int)AppSession.UserLogin.ParamedicID, "");
            Methods.SetComboBoxField(cboVisitType, visitTypeList, "VisitTypeName", "VisitTypeID");
        }

        private void EntityToControl(PatientVisitNote entitypvn)
        {
            hdnPatientVisitNoteID.Value = entitypvn.ID.ToString();
            hdnPlanningText.Value = entitypvn.PlanningText;
            txtInstructionText.Text = entitypvn.InstructionText;
            _visitNoteID = hdnPatientVisitNoteID.Value;
        }

        private void ControlToEntity(PatientVisitNote entitypvn)
        {
            string soapNote = GenerateSOAPText();

            entitypvn.NoteDate = Helper.GetDatePickerValue(txtServiceDate);
            entitypvn.NoteTime = txtServiceTime.Text;
            entitypvn.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);

            entitypvn.SubjectiveText = hdnSubjectiveText.Value;
            entitypvn.ObjectiveText = hdnObjectiveText.Value;
            entitypvn.AssessmentText = hdnAssessmentText.Value;
            entitypvn.PlanningText = hdnPlanningText.Value;
            entitypvn.InstructionText = txtInstructionText.Text;
            entitypvn.NoteText = soapNote;
        }

        [Obsolete]
        private string GenerateSOAPSummaryText(ref string subjectiveText, ref string objectiveText, ref string assessmentText, ref string planningText)
        {
            StringBuilder sbNotes = new StringBuilder();
            sbNotes.AppendLine("Subjective :");
            sbNotes.AppendLine("-".PadRight(15, '-'));
            if ((Convert.ToInt32(hdnParamedicID.Value) == Convert.ToInt32(hdnParamedicID.Value)))
            {
                vChiefComplaint oChiefComplaint = BusinessLayer.GetvChiefComplaintList(string.Format("VisitID = {0}", Convert.ToInt32(hdnVisitID.Value))).FirstOrDefault();
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
            List<vVitalSignDt> lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID = {0} AND ObservationDate = '{1}' AND IsDeleted = 0 ORDER BY DisplayOrder", Convert.ToInt32(hdnVisitID.Value), Helper.GetDatePickerValue(txtServiceDate).ToString(Constant.FormatString.DATE_FORMAT_112)));
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
            List<vReviewOfSystemDt> lstROS = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID IN (SELECT ID FROM ReviewOfSystemHd WHERE VisitID = {0} AND  ObservationDate = '{1}') AND IsDeleted = 0 ORDER BY GCRoSystem", Convert.ToInt32(hdnVisitID.Value), Helper.GetDatePickerValue(txtServiceDate).ToString(Constant.FormatString.DATE_FORMAT_112)));
            if (lstROS.Count > 0)
            {
                foreach (vReviewOfSystemDt item in lstROS)
                {
                    sbNotes.AppendLine(string.Format(" {0}: {1}", item.ROSystem, item.cfRemarks));
                }
            }

            if (!string.IsNullOrEmpty(txtDiagnosticResultSummary.Text))
            {
                sbNotes.AppendLine(" ");
                sbNotes.AppendLine("Catatan Hasil Pemeriksaan Penunjang: ");
                sbNotes.AppendLine(txtDiagnosticResultSummary.Text);
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

        private string GenerateSOAPText()
        {
            StringBuilder sbNotes = new StringBuilder();
            StringBuilder sbSubjective = new StringBuilder();
            StringBuilder sbDiagnose = new StringBuilder();
            sbNotes.AppendLine("SUBJEKTIVE :");
            sbNotes.AppendLine("-".PadRight(20, '-'));

            sbNotes.AppendLine(string.Format("Keluhan Utama  : "));
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
                sbNotes.AppendLine(string.Format("Riwayat Pengobatan Dahulu  : "));
                sbNotes.AppendLine(string.Format(" {0}   ", txtMedicationHistory.Text));
            }

            hdnSubjectiveText.Value = sbNotes.ToString();

            sbNotes = new StringBuilder();
            sbNotes.AppendLine(" ");
            sbNotes.AppendLine("OBJEKTIF :");
            sbNotes.AppendLine("-".PadRight(20, '-'));
            vVitalSignHd entityVitalSignHd = BusinessLayer.GetvVitalSignHdList(string.Format("VisitID = {0} AND IsDeleted = 0", hdnVisitID.Value)).FirstOrDefault();
            if (entityVitalSignHd != null)
            {
                List<vVitalSignDt> lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0} AND ObservationDate = '{1}' AND IsDeleted = 0 ORDER BY DisplayOrder", entityVitalSignHd.ID, Helper.GetDatePickerValue(txtServiceDate).ToString(Constant.FormatString.DATE_FORMAT_112)));
                if (lstVitalSignDt.Count > 0)
                {
                    sbNotes.AppendLine("Tanda Vital :");
                    foreach (vVitalSignDt vitalSign in lstVitalSignDt)
                    {
                        sbNotes.AppendLine(string.Format(" - {0} {1} {2}", vitalSign.VitalSignLabel, vitalSign.VitalSignValue, vitalSign.ValueUnit));
                    }
                }
            }

            vReviewOfSystemHd entityReviewOfSystem = BusinessLayer.GetvReviewOfSystemHdList(string.Format("VisitID = {0} AND IsDeleted = 0", hdnVisitID.Value)).FirstOrDefault();
            if (entityReviewOfSystem != null)
            {
                List<vReviewOfSystemDt> lstROS = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID = {0} AND IsDeleted = 0 ORDER BY GCRoSystem", entityReviewOfSystem.ID, Helper.GetDatePickerValue(txtServiceDate).ToString(Constant.FormatString.DATE_FORMAT_112)));
                if (lstROS.Count > 0)
                {
                    sbNotes.AppendLine(" ");
                    sbNotes.AppendLine("Pemeriksaan Fisik :");
                    foreach (vReviewOfSystemDt item in lstROS)
                    {
                        sbNotes.AppendLine(string.Format(" - {0}: {1}", item.ROSystem, item.cfRemarks));
                    }
                }
            }

            if (!string.IsNullOrEmpty(txtDiagnosticResultSummary.Text))
            {
                sbNotes.AppendLine(" ");
                sbNotes.AppendLine("Catatan Hasil Pemeriksaan Penunjang: ");
                sbNotes.AppendLine(txtDiagnosticResultSummary.Text);
            }

            hdnObjectiveText.Value = sbNotes.ToString();

            sbNotes = new StringBuilder();
            sbNotes.AppendLine(" ");
            sbNotes.AppendLine("ASSESSMENT :");
            sbNotes.AppendLine("-".PadRight(20, '-'));

            vPatientDiagnosis entityDiagnose = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0 AND IsNutritionDiagnosis = 0", hdnVisitID.Value)).FirstOrDefault();
            if (entityDiagnose != null)
            {
                string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0 AND IsNutritionDiagnosis = 0 ORDER BY GCDiagnoseType", hdnVisitID.Value);
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

            //hdnAssessmentText.Value = hdnDiagnosisSummary.Value;
            sbNotes.AppendLine(string.Format(" {0}", hdnAssessmentText.Value));

            sbNotes = new StringBuilder();
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
            if (!string.IsNullOrEmpty(txtPlanningNotes.Text))
            {
                sbNotes.AppendLine("Rencana Tindakan :");
                sbNotes.AppendLine(txtPlanningNotes.Text);
            }

            hdnPlanningText.Value = sbNotes.ToString();

            //if (!string.IsNullOrEmpty(txtInstructionText.Text))
            //{
            //    sbNotes.AppendLine(" ");
            //    sbNotes.AppendLine("INSTRUKSI : ");
            //    sbNotes.AppendLine("-".PadRight(20, '-'));
            //    sbNotes.AppendLine(txtInstructionText.Text);

            //    hdnInstructionText.Value = txtInstructionText.Text;
            //}

            return sbNotes.ToString();
        }

        private void UpdateConsultVisitRegistration(IDbContext ctx)
        {
            ChiefComplaintDao chiefComplaintDao = new ChiefComplaintDao(ctx);
            ConsultVisitDao consultVisitDao = new ConsultVisitDao(ctx);
            RegistrationDao registrationDao = new RegistrationDao(ctx);
            PastMedicalDao pastMedicalDao = new PastMedicalDao(ctx);
            MSTAssessmentDao mstDao = new MSTAssessmentDao(ctx);

            ConsultVisit entityConsultVisit = consultVisitDao.Get(Convert.ToInt32(hdnVisitID.Value));
            if (entityConsultVisit.GCVisitStatus == Constant.VisitStatus.CHECKED_IN)
            {
                entityConsultVisit.GCVisitStatus = Constant.VisitStatus.RECEIVING_TREATMENT;
                entityConsultVisit.StartServiceDate = Helper.GetDatePickerValue(txtServiceDate);
                entityConsultVisit.StartServiceTime = txtServiceTime.Text;
            }
            else
            {
                DateTime assesmentDateTime = DateTime.Parse(string.Format("{0} {1}", Helper.GetDatePickerValue(txtServiceDate).ToString(Constant.FormatString.DATE_FORMAT), txtServiceTime.Text));
                DateTime serviceDateTime = DateTime.Parse(string.Format("{0} {1}", entityConsultVisit.StartServiceDate.ToString(Constant.FormatString.DATE_FORMAT), entityConsultVisit.StartServiceTime));
                if (assesmentDateTime < serviceDateTime)
                {
                    entityConsultVisit.StartServiceDate = Helper.GetDatePickerValue(txtServiceDate);
                    entityConsultVisit.StartServiceTime = txtServiceTime.Text;                  
                }
            }

            if (cboVisitCaseType.Value != null && cboVisitCaseType.Value != "")
            {
                entityConsultVisit.GCCaseType = cboVisitCaseType.Value.ToString();
            }
            else
            {
                entityConsultVisit.GCCaseType = null;
            }

            if (!string.IsNullOrEmpty(cboVisitType.Value.ToString()))
            {
                entityConsultVisit.VisitTypeID = Convert.ToInt16(cboVisitType.Value);
            }

            if (hdnDepartmentID.Value == Constant.Facility.EMERGENCY)
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
            //else
            //    entityConsultVisit.GCAdmissionCondition = null;

            consultVisitDao.Update(entityConsultVisit);

            Registration entityRegistration = registrationDao.Get(entityConsultVisit.RegistrationID);
            if (cboTriage.Value == null)
                entityRegistration.GCTriage = "";
            else
                entityRegistration.GCTriage = cboTriage.Value.ToString();
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

            if (cboAdmissionRoute.Value != null)
                entityRegistration.GCAdmissionRoute = cboAdmissionRoute.Value.ToString();
            //else
            //    entityRegistration.GCAdmissionRoute = null;
            if (cboAirway.Value != null)
                entityRegistration.GCAirway = cboAirway.Value.ToString();
            //else
            //    entityRegistration.GCAirway = null;
            if (cboBreathing.Value != null)
                entityRegistration.GCBreathing = cboBreathing.Value.ToString();
            //else
            //    entityRegistration.GCBreathing = "";
            if (cboCirculation.Value != null)
                entityRegistration.GCCirculation = cboCirculation.Value.ToString();
            //else
            //    entityRegistration.GCCirculation = null;
            if (cboDisability.Value != null)
                entityRegistration.GCDisability = cboDisability.Value.ToString();
            //else
            //    entityRegistration.GCDisability = null;
            if (cboExposure.Value != null)
                entityRegistration.GCExposure = cboExposure.Value.ToString();
            //else
            //    entityRegistration.GCExposure = null;

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
                entity.VisitID = Convert.ToInt32(hdnVisitID.Value);
                entity.CreatedBy = AppSession.UserLogin.UserID;
            }
            entity.ObservationDate = Helper.GetDatePickerValue(txtServiceDate);
            entity.ObservationTime = txtServiceTime.Text;
            entity.ChiefComplaintText = txtChiefComplaint.Text;
            entity.HPISummary = txtHPISummary.Text;
            entity.PastMedicalHistory = txtMedicalHistory.Text;
            entity.PastMedicationHistory = txtMedicationHistory.Text;
            entity.DiagnosticResultSummary = txtDiagnosticResultSummary.Text;
            entity.PlanningSummary = txtPlanningNotes.Text;
            entity.IsPatientAllergyExists = !chkIsPatientAllergyExists.Checked;

            entity.IsAutoAnamnesis = Convert.ToBoolean(chkAutoAnamnesis.Checked);
            entity.IsAlloAnamnesis = Convert.ToBoolean(chkAlloAnamnesis.Checked);
            entity.MedicalProblem = txtMedicalProblem.Text;

            if (isNewChiefComplaint)
            {
                hdnChiefComplaintID.Value = chiefComplaintDao.InsertReturnPrimaryKeyID(entity).ToString();
            }
            else
            {
                chiefComplaintDao.Update(entity);
            }

            if (cboGCWeightChangedStatus.Value != null || cboGCWeightChangedStatus.Value != null || cboGCMSTDiagnosis.Value != null)
            {
                string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", hdnVisitID.Value);
                MSTAssessment entityMST = BusinessLayer.GetMSTAssessmentList(filterExpression).FirstOrDefault();
                if (entityMST != null)
                {
                    hdnMSTAssessmentID.Value = entityMST.ID.ToString();
                }
                else
                {
                    hdnMSTAssessmentID.Value = "0";
                }
                bool isNewMST = true;
                if (hdnMSTAssessmentID.Value != "" && hdnMSTAssessmentID.Value != "0")
                {
                    entityMST = mstDao.Get(Convert.ToInt32(hdnMSTAssessmentID.Value));
                    isNewMST = false;
                    entityMST.LastUpdatedBy = AppSession.UserLogin.UserID;
                }
                else
                {
                    entityMST = new MSTAssessment();
                    entityMST.VisitID = Convert.ToInt32(hdnVisitID.Value);
                    entityMST.CreatedBy = AppSession.UserLogin.UserID;
                }

                if (cboGCWeightChangedStatus.Value != null)
                {
                    if (!string.IsNullOrEmpty(cboGCWeightChangedStatus.Value.ToString()))
                    {
                        entityMST.GCWeightChangedStatus = cboGCWeightChangedStatus.Value.ToString();
                    }
                }
                if (cboGCWeightChangedGroup.Value != null)
                {
                    if (!string.IsNullOrEmpty(cboGCWeightChangedGroup.Value.ToString()))
                    {
                        entityMST.GCWeightChangedGroup = cboGCWeightChangedGroup.Value.ToString();
                    }
                }

                if (rblIsFoodIntakeChanged.SelectedValue != null)
                {
                    entityMST.GCFoodIntakeChanged = rblIsFoodIntakeChanged.SelectedValue == "1" ? "X450^01" : "X450^02";
                }

                if (cboGCMSTDiagnosis.Value != null)
                {
                    if (!string.IsNullOrEmpty(cboGCMSTDiagnosis.Value.ToString()))
                    {
                        entityMST.GCMSTDiagnosis = cboGCMSTDiagnosis.Value.ToString();
                    }
                }
                if (!string.IsNullOrEmpty(txtOtherMSTDiagnosis.Text))
                {
                    entityMST.OtherMSTDiagnosis = txtOtherMSTDiagnosis.Text;
                }
                entityMST.IsHasSpecificDiagnosis = rblIsHasSpecificDiagnosis.SelectedValue == "1" ? true : false;
                entityMST.IsReadedByNutritionist = rblIsReadedByNutritionist.SelectedValue == "1" ? true : false;
                entityMST.MSTScore = Convert.ToInt16(Request.Form[txtTotalMST.UniqueID]);
                entityMST.AssessmentDate = Helper.GetDatePickerValue(txtServiceDate);
                entityMST.AssessmentTime = txtServiceTime.Text;

                if (isNewMST)
                {
                    mstDao.Insert(entityMST);
                    hdnMSTAssessmentID.Value = BusinessLayer.GetMSTAssessmentMaxID(ctx).ToString();
                }
                else
                {
                    mstDao.Update(entityMST);
                }
            }
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            if (type == "save")
            {
                bool result = true;

                if ((hdnChiefComplaintID.Value != "" && hdnChiefComplaintID.Value != "0") && !IsValidToSave(ref errMessage))
                {
                    result = false;
                    hdnIsSaved.Value = "0";
                    return result;
                }

                IDbContext ctx = DbFactory.Configure(true);
                PatientVisitNoteDao patientVisitNoteDao = new PatientVisitNoteDao(ctx);
                PatientInstructionDao patientInstructionDao = new PatientInstructionDao(ctx);
                try
                {

                    ChiefComplaint obj = BusinessLayer.GetChiefComplaintList(string.Format("VisitID = {0}", hdnVisitID.Value), ctx).FirstOrDefault();
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
                    vVitalSignHd entityVitalSignHd = BusinessLayer.GetvVitalSignHdList(string.Format("VisitID = {0} AND IsDeleted = 0", hdnVisitID.Value)).FirstOrDefault();
                    if (entityVitalSignHd != null)
                    {
                        List<vVitalSignDt> lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0} AND ObservationDate = '{1}' AND IsDeleted = 0 ORDER BY DisplayOrder", entityVitalSignHd.ID, Helper.GetDatePickerValue(txtServiceDate).ToString(Constant.FormatString.DATE_FORMAT_112)));
                        if (lstVitalSignDt.Count > 0)
                        {
                            foreach (vVitalSignDt vitalSign in lstVitalSignDt)
                            {
                                sbNotes.AppendLine(string.Format(" {0} {1} {2}", vitalSign.VitalSignLabel, vitalSign.VitalSignValue, vitalSign.ValueUnit));
                            }
                        }
                    }

                    sbNotes.AppendLine(" ");

                    vReviewOfSystemHd entityReviewOfSystem = BusinessLayer.GetvReviewOfSystemHdList(string.Format("VisitID = {0} AND IsDeleted = 0", hdnVisitID.Value)).FirstOrDefault();
                    if (entityReviewOfSystem != null)
                    {
                        List<vReviewOfSystemDt> lstROS = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID = {0} AND IsDeleted = 0 ORDER BY GCRoSystem", entityReviewOfSystem.ID, Helper.GetDatePickerValue(txtServiceDate).ToString(Constant.FormatString.DATE_FORMAT_112)));
                        if (lstROS.Count > 0)
                        {
                            foreach (vReviewOfSystemDt item in lstROS)
                            {
                                sbNotes.AppendLine(string.Format(" {0}: {1}", item.ROSystem, item.cfRemarks));
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(txtDiagnosticResultSummary.Text))
                    {
                        sbNotes.AppendLine(" ");
                        sbNotes.AppendLine("Catatan Hasil Pemeriksaan Penunjang: ");
                        sbNotes.AppendLine(txtDiagnosticResultSummary.Text);
                    }

                    string objectiveText = sbNotes.ToString();
                    #endregion

                    #region Assessment Content
                    sbNotes = new StringBuilder();
                    string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0 AND IsNutritionDiagnosis = 0 ORDER BY GCDiagnoseType", hdnVisitID.Value);
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

                    #region Planning Content
                    string planningText = string.Empty;
                    sbNotes = new StringBuilder();
                    if (!string.IsNullOrEmpty(hdnLaboratorySummary.Value))
                    {
                        sbNotes.AppendLine("Pemeriksaan Laboratorium :");
                        sbNotes.AppendLine(hdnLaboratorySummary.Value);
                    }
                    if (!string.IsNullOrEmpty(hdnImagingSummary.Value))
                    {
                        sbNotes.AppendLine("Pemeriksaan Radiologi :");
                        sbNotes.AppendLine(hdnImagingSummary.Value);
                    }
                    if (!string.IsNullOrEmpty(hdnDiagnosticSummary.Value))
                    {
                        sbNotes.AppendLine("Pemeriksaan Penunjang :");
                        sbNotes.AppendLine(hdnDiagnosticSummary.Value);
                    }
                    if (!string.IsNullOrEmpty(hdnProcedureSummary.Value))
                    {
                        sbNotes.AppendLine(hdnProcedureSummary.Value);
                    }
                    if (!string.IsNullOrEmpty(txtPlanningNotes.Text))
                    {
                        sbNotes.AppendLine("Rencana Tindakan :");
                        sbNotes.AppendLine(txtPlanningNotes.Text);
                    }
                    planningText = sbNotes.ToString();
                    #endregion

                    #region Instruction Content
                    string instructionText = string.Empty;
                    sbNotes = new StringBuilder();
                    if (!string.IsNullOrEmpty(txtInstructionText.Text))
                    {
                        sbNotes.AppendLine(txtInstructionText.Text);
                    }
                    instructionText = sbNotes.ToString();
                    #endregion

                    UpdateConsultVisitRegistration(ctx);
                    PatientVisitNote entityEmergencyCaseNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = {0} AND GCPatientNoteType = '{1}'", hdnVisitID.Value, Constant.PatientVisitNotes.EMERGENCY_INITIAL_ASSESSMENT), ctx).FirstOrDefault();
                    bool isEntityEmergencyCaseNoteNull = false;
                    if (entityEmergencyCaseNote == null)
                    {
                        isEntityEmergencyCaseNoteNull = true;
                        entityEmergencyCaseNote = new PatientVisitNote();
                    }

                    entityEmergencyCaseNote.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                    entityEmergencyCaseNote.VisitID = Convert.ToInt32(hdnVisitID.Value);
                    entityEmergencyCaseNote.NoteDate = Helper.GetDatePickerValue(txtServiceDate);
                    entityEmergencyCaseNote.NoteTime = txtServiceTime.Text;
                    entityEmergencyCaseNote.ParamedicID = AppSession.UserLogin.ParamedicID;
                    entityEmergencyCaseNote.GCPatientNoteType = Constant.PatientVisitNotes.EMERGENCY_INITIAL_ASSESSMENT;
                    entityEmergencyCaseNote.SubjectiveText = subjectiveText;
                    entityEmergencyCaseNote.ObjectiveText = objectiveText;
                    entityEmergencyCaseNote.AssessmentText = assessmentText;
                    entityEmergencyCaseNote.PlanningText = planningText;
                    entityEmergencyCaseNote.InstructionText = instructionText;
                    entityEmergencyCaseNote.ChiefComplaintID = Convert.ToInt32(hdnChiefComplaintID.Value);
                    entityEmergencyCaseNote.NoteText = string.Format(@"S:{0}{1}{2}O:{3}{4}{5}A:{6}{7}{8}P:{9}{10}",
                        Environment.NewLine, subjectiveText, Environment.NewLine,
                        Environment.NewLine, objectiveText, Environment.NewLine,
                        Environment.NewLine, assessmentText, Environment.NewLine,
                        Environment.NewLine, planningText);

                    if (isEntityEmergencyCaseNoteNull)
                    {
                        entityEmergencyCaseNote.ChiefComplaintID = Convert.ToInt32(hdnChiefComplaintID.Value);
                        entityEmergencyCaseNote.CreatedBy = AppSession.UserLogin.UserID;
                        hdnPatientVisitNoteID.Value = patientVisitNoteDao.InsertReturnPrimaryKeyID(entityEmergencyCaseNote).ToString();
                    }
                    else
                    {
                        entityEmergencyCaseNote.ParamedicID = AppSession.UserLogin.ParamedicID;
                        entityEmergencyCaseNote.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityEmergencyCaseNote.ChiefComplaintID = Convert.ToInt32(hdnChiefComplaintID.Value);
                        patientVisitNoteDao.Update(entityEmergencyCaseNote);

                        hdnPatientVisitNoteID.Value = entityEmergencyCaseNote.ID.ToString();
                    }

                    _visitNoteID = hdnPatientVisitNoteID.Value;

                    if (!string.IsNullOrEmpty(txtInstructionText.Text))
                    {
                        string filterExpInstruction = string.Format("PatientVisitNoteID = {0}", _visitNoteID);
                        PatientInstruction oInstruction = BusinessLayer.GetPatientInstructionList(filterExpInstruction, ctx).FirstOrDefault();

                        if (oInstruction == null)
                        {
                            oInstruction = new PatientInstruction();
                            oInstruction.VisitID = Convert.ToInt32(hdnVisitID.Value);
                            oInstruction.PatientVisitNoteID = Convert.ToInt32(_visitNoteID);
                            oInstruction.PhysicianID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                            oInstruction.GCInstructionGroup = hdnDefaultInstructionType.Value;
                            oInstruction.Description = txtInstructionText.Text;
                            oInstruction.InstructionDate = Helper.GetDatePickerValue(txtServiceDate);
                            oInstruction.InstructionTime = txtServiceTime.Text;
                            oInstruction.CreatedBy = AppSession.UserLogin.UserID;
                            patientInstructionDao.Insert(oInstruction);
                        }
                        else
                        {
                            oInstruction.VisitID = Convert.ToInt32(hdnVisitID.Value);
                            oInstruction.PatientVisitNoteID = Convert.ToInt32(_visitNoteID);
                            oInstruction.PhysicianID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                            oInstruction.GCInstructionGroup = hdnDefaultInstructionType.Value;
                            oInstruction.Description = txtInstructionText.Text;
                            oInstruction.InstructionDate = Helper.GetDatePickerValue(txtServiceDate);
                            oInstruction.InstructionTime = txtServiceTime.Text;
                            oInstruction.LastUpdatedBy = AppSession.UserLogin.UserID;
                            patientInstructionDao.Update(oInstruction);
                        }
                    }

                    ctx.CommitTransaction();
                    errMessage = hdnPatientVisitNoteID.Value;
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
            return true;
        }

        private bool IsValidToSave(ref string errMessage)
        {
            bool result = true;
            if (hdnChiefComplaintParamedicID.Value != "" && hdnChiefComplaintParamedicID.Value != "0")
            {
                int paramedicID = Convert.ToInt32(hdnChiefComplaintParamedicID.Value);
                if (AppSession.UserLogin.ParamedicID != paramedicID)
                {
                    errMessage = "Perubahan Kajian Awal Pasien hanya dapat dilakukan oleh Dokter yang melakukan Pengkajian";
                    result = false;
                }
            }
            return result;
        }

        #region Allergy
        private void BindGridViewAllergy(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("MRN = {0} AND IsDeleted = 0", hdnMRN.Value);

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
            int processType = 0;

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
                        oAllergy.MRN = Convert.ToInt32(hdnMRN.Value);
                        oAllergy.AllergyLogDate = DateTime.Now.Date;
                        oAllergy.GCAllergenType = cboAllergenType.Value.ToString();
                        oAllergy.Allergen = txtAllergenName.Text;
                        oAllergy.GCAllergySource = Constant.AllergenFindingSource.PATIENT;
                        oAllergy.GCAllergySeverity = Constant.AllergySeverity.UNKNOWN;
                        oAllergy.KnownDate = DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112);
                        oAllergy.Reaction = txtReaction.Text;
                        oAllergy.CreatedBy = AppSession.UserLogin.UserID;
                        allergyDao.Insert(oAllergy);

                        Patient oPatient = patientDao.Get(Convert.ToInt32(hdnMRN.Value));
                        if (!oPatient.IsHasAllergy)
                        {
                            oPatient.IsHasAllergy = true;
                            oPatient.LastUpdatedBy = AppSession.UserLogin.UserID;
                            patientDao.Update(oPatient);
                        }

                        result = "1|add|1";

                        processType = 1;
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

                            processType = 2;
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

                            List<PatientAllergy> lstAllergy = BusinessLayer.GetPatientAllergyList(string.Format("MRN = {0} AND IsDeleted = 0", Convert.ToInt32(hdnMRN.Value)), ctx);
                            if (lstAllergy.Count > 0)
                            {
                                Patient oPatient = patientDao.Get(Convert.ToInt32(hdnMRN.Value));
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
                                Patient oPatient = patientDao.Get(Convert.ToInt32(hdnMRN.Value));
                                if (oPatient.IsHasAllergy)
                                {
                                    oPatient.IsHasAllergy = false;
                                    oPatient.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    patientDao.Update(oPatient);
                                }

                                isHasAllergy = "0";
                            }
                            result = "1|delete|" + isHasAllergy;

                            processType = 2;
                        }
                        else
                        {
                            result = string.Format("0|edit|{0}", "Invalid Patient Allergy Record Information");
                        }
                    }

                    if (!isError)
                    {
                        ctx.CommitTransaction();

                        if (AppSession.SA0137 == "1")
                        {
                            if (AppSession.SA0133 == Constant.CenterBackConsumerAPI.MEDINFRAS_EMR_V1)
                            {
                                BridgingToMedinfrasV1("allergy", processType, null, null, null);
                            }
                        }
                    }
                    else
                    {
                        ctx.RollBackTransaction();
                    }
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
            filterExpression += string.Format("VisitID = {0} AND IsDischargeVitalSign = 0 AND IsDeleted = 0", hdnVisitID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvVitalSignHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_COMPACT);
            }

            List<vVitalSignHd> lstEntity = BusinessLayer.GetvVitalSignHdList(filterExpression, Constant.GridViewPageSize.GRID_COMPACT, pageIndex, "ID DESC");
            lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID = {0} ORDER BY DisplayOrder", hdnVisitID.Value));
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
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", hdnVisitID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvReviewOfSystemHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vReviewOfSystemHd> lstEntity = BusinessLayer.GetvReviewOfSystemHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
            lstReviewOfSystemDt = BusinessLayer.GetvReviewOfSystemDtList(string.Format("VisitID = {0} ORDER BY TagProperty", hdnVisitID.Value));
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
                filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", hdnVisitID.Value);

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

            if (pageCount > 0)
            {
                hdnPageIndex.Value = "0";
                tblBodyDiagramNavigation.Style.Remove("display");
            }
            else
            {
                divBodyDiagram.Style.Add("display", "none");
                tblEmpty.Style.Remove("display");
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void OnLoadBodyDiagram(int PageIndex)
        {
            string filterExpression = "";
            filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", hdnVisitID.Value);
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
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0 AND IsNutritionDiagnosis = 0", hdnVisitID.Value);

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
            hdnAssessmentText.Value = hdnDiagnosisSummary.Value;
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
            panel.JSProperties["cpDiagnosis"] = hdnDiagnosisSummary.Value;
        }
        protected void cbpDiagnosis_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|add|";
            bool isValid = true;

            try
            {
                if (e.Parameter != null && e.Parameter != "")
                {
                    string[] param = e.Parameter.Split('|');
                    if (param[0] == "add")
                    {
                        if (hdnIsMainDiagnosisExists.Value == "1" && cboDiagnosisType.Value.ToString() == Constant.DiagnoseType.MAIN_DIAGNOSIS)
                        {
                            result = string.Format("0|add|{0}", "Dalam 1 episode kunjungan/perawatan, hanya boleh ada 1 Diagnosa Utama");
                            isValid = false;
                        }

                        if (isValid)
                        {
                            PatientDiagnosis entity = new PatientDiagnosis();

                            entity.VisitID = Convert.ToInt32(hdnVisitID.Value);
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

                            if (AppSession.SA0137 == "1")
                            {
                                if (AppSession.SA0133 == Constant.CenterBackConsumerAPI.MEDINFRAS_EMR_V1)
                                {
                                    BridgingToMedinfrasV1("diagnose", 1, entity, null, null);
                                }
                            }

                            result = "1|add|"; 
                        }
                    }
                    else if (param[0] == "edit")
                    {
                        int recordID = Convert.ToInt32(hdnDiagnosisID.Value);
                        PatientDiagnosis entity = BusinessLayer.GetPatientDiagnosis(recordID);

                        if (entity != null)
                        {
                            if ((hdnIsMainDiagnosisExists.Value == "1" && cboDiagnosisType.Value.ToString() == Constant.DiagnoseType.MAIN_DIAGNOSIS) && entity.GCDiagnoseType != Constant.DiagnoseType.MAIN_DIAGNOSIS)
                            {
                                result = string.Format("0|edit|{0}", "Dalam 1 episode kunjungan/perawatan, hanya boleh ada 1 Diagnosa Utama");
                                isValid = false;
                            }

                            if (isValid)
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

                                if (AppSession.SA0137 == "1")
                                {
                                    if (AppSession.SA0133 == Constant.CenterBackConsumerAPI.MEDINFRAS_EMR_V1)
                                    {
                                        BridgingToMedinfrasV1("diagnose", 2, entity, null, null);
                                    }
                                }

                                result = "1|edit|"; 
                            }
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

                                if (AppSession.SA0137 == "1")
                                {
                                    if (AppSession.SA0133 == Constant.CenterBackConsumerAPI.MEDINFRAS_EMR_V1)
                                    {
                                        BridgingToMedinfrasV1("diagnose", 3, entity, null, null);
                                    }
                                }

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

        #region Procedure
        private void BindGridViewProcedure(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", hdnVisitID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientProcedureRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientProcedure> lstEntity = BusinessLayer.GetvPatientProcedureList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID");
            grdProcedureView.DataSource = lstEntity;
            grdProcedureView.DataBind();

            //Create Diagnosis Summary for : CPOE Clinical Notes
            if (lstEntity.Count > 0)
            {
                StringBuilder strProcedures = new StringBuilder();
                strProcedures.AppendLine("Prosedur/Tindakan :");
                foreach (var item in lstEntity)
                {
                    strProcedures.AppendLine(string.Format("- {0}", item.cfProcedureText));
                }
                hdnProcedureSummary.Value = strProcedures.ToString();
            }
            else
            {
                hdnProcedureSummary.Value = string.Empty;
            }
        }

        protected void cbpProcedureView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewProcedure(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewProcedure(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpSummary"] = hdnProcedureSummary.Value;
        }

        protected void cbpProcedure_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|add|";
            bool isValid = true;

            try
            {
                if (e.Parameter != null && e.Parameter != "")
                {
                    string[] param = e.Parameter.Split('|');
                    if (param[0] == "add")
                    {
                        if (isValid)
                        {
                            PatientProcedure entity = new PatientProcedure();

                            entity.VisitID = Convert.ToInt32(hdnVisitID.Value);
                            entity.ProcedureDate = Helper.GetDatePickerValue(txtServiceDate);
                            entity.ProcedureTime = txtServiceTime.Text;
                            entity.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                            if (hdnEntryProcedureID.Value != "")
                                entity.ProcedureID = hdnEntryProcedureID.Value;
                            else
                                entity.ProcedureID = null;

                            entity.ProcedureText = txtProcedureText.Text;
                            entity.Remarks = string.Empty;
                            entity.CreatedBy = AppSession.UserLogin.UserID;

                            BusinessLayer.InsertPatientProcedure(entity);

                            if (AppSession.SA0137 == "1")
                            {
                                if (AppSession.SA0133 == Constant.CenterBackConsumerAPI.MEDINFRAS_EMR_V1)
                                {
                                    BridgingToMedinfrasV1("procedures", 1, null, entity, null);
                                }
                            }

                            result = "1|add|";
                        }
                    }
                    else if (param[0] == "edit")
                    {
                        int recordID = Convert.ToInt32(hdnProcedureID.Value);
                        PatientProcedure entity = BusinessLayer.GetPatientProcedure(recordID);

                        if (entity != null)
                        {
                            if (isValid)
                            {
                                entity.ProcedureDate = Helper.GetDatePickerValue(txtServiceDate);
                                entity.ProcedureTime = txtServiceTime.Text;

                                entity.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                                if (hdnEntryProcedureID.Value != "")
                                    entity.ProcedureID = hdnEntryProcedureID.Value;
                                else
                                    entity.ProcedureID = null;

                                entity.ProcedureText = txtProcedureText.Text;
                                entity.Remarks = string.Empty;
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                BusinessLayer.UpdatePatientProcedure(entity);

                                if (AppSession.SA0137 == "1")
                                {
                                    if (AppSession.SA0133 == Constant.CenterBackConsumerAPI.MEDINFRAS_EMR_V1)
                                    {
                                        BridgingToMedinfrasV1("procedures", 2, null, entity, null);
                                    }
                                }

                                result = "1|edit|";
                            }
                        }
                        else
                        {
                            result = string.Format("0|delete|{0}", "Invalid Patient Procedure Record Information");
                        }
                    }
                    else
                    {
                        int recordID = Convert.ToInt32(hdnProcedureID.Value);
                        PatientProcedure entity = BusinessLayer.GetPatientProcedure(recordID);

                        if (entity != null)
                        {
                            //TODO : Prompt user for delete reason
                            entity.IsDeleted = true;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdatePatientProcedure(entity);

                            if (AppSession.SA0137 == "1")
                            {
                                if (AppSession.SA0133 == Constant.CenterBackConsumerAPI.MEDINFRAS_EMR_V1)
                                {
                                    BridgingToMedinfrasV1("procedures", 3, null, entity, null);
                                }
                            }

                            result = "1|delete|";
                        }
                        else
                        {
                            result = string.Format("0|edit|{0}", "Invalid Patient Procedure Record Information");
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

        #region Laboratory
        private void BindGridViewLaboratory(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND TransactionCode  = {1} AND GCTransactionStatus != '{2}'", hdnVisitID.Value, Constant.TransactionCode.LABORATORY_TEST_ORDER, Constant.TransactionStatus.VOID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvTestOrderHd1RowCount(filterExpression);
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
                    result = "refresh|" + pageCount;
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
                rptLaboratoryDt.DataSource = obj.cfTestOrderDetailList;
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
            string filterExpression = string.Format("VisitID = {0} AND TransactionCode = {1} AND GCTransactionStatus != '{2}'", hdnVisitID.Value, Constant.TransactionCode.IMAGING_TEST_ORDER, Constant.TransactionStatus.VOID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvTestOrderHd1RowCount(filterExpression);
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
                    result = "refresh|" + pageCount;
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
                rptImagingDt.DataSource = obj.cfTestOrderDetailList;
                rptImagingDt.DataBind();
            }
        }
        #endregion

        #region Diagnostic
        private void BindGridViewDiagnostic(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND TransactionCode NOT IN ({1},{2}) AND GCTransactionStatus != '{3}'", hdnVisitID.Value, Constant.TransactionCode.LABORATORY_TEST_ORDER, Constant.TransactionCode.IMAGING_TEST_ORDER, Constant.TransactionStatus.VOID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvTestOrderHd1RowCount(filterExpression);
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
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpSummary"] = hdnDiagnosticSummary.Value;
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
                    default:
                        testOrderID = hdnDiagnosticTestOrderID.Value;
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

                            if (AppSession.SA0137 == "1")
                            {
                                if (AppSession.SA0133 == Constant.CenterBackConsumerAPI.MEDINFRAS_EMR_V1)
                                {
                                    BridgingToMedinfrasV1("order", 1, null, null, entity);
                                }
                            }

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
                    PatientVisitNote entityEmergencyCaseNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = {0} AND GCPatientNoteType = '{1}'", hdnVisitID.Value, Constant.PatientVisitNotes.EMERGENCY_INITIAL_ASSESSMENT), ctx).FirstOrDefault();
                    bool isEntityEmergencyCaseNoteNull = false;
                    if (entityEmergencyCaseNote == null)
                    {
                        isEntityEmergencyCaseNoteNull = true;
                        entityEmergencyCaseNote = new PatientVisitNote();
                    }

                    #region Summarize Instruction Text
                    hdnInstructionText.Value = txtInstructionText.Text;
                    #endregion

                    ControlToEntity(entityEmergencyCaseNote);

                    if (isEntityEmergencyCaseNoteNull)
                    {
                        entityEmergencyCaseNote.VisitID = Convert.ToInt32(hdnVisitID.Value);
                        entityEmergencyCaseNote.ParamedicID = AppSession.UserLogin.ParamedicID;
                        entityEmergencyCaseNote.GCPatientNoteType = Constant.PatientVisitNotes.EMERGENCY_INITIAL_ASSESSMENT;
                        entityEmergencyCaseNote.CreatedBy = AppSession.UserLogin.UserID;
                        hdnPatientVisitNoteID.Value = patientVisitNoteDao.InsertReturnPrimaryKeyID(entityEmergencyCaseNote).ToString();
                    }
                    else
                    {
                        entityEmergencyCaseNote.ParamedicID = AppSession.UserLogin.ParamedicID;
                        entityEmergencyCaseNote.LastUpdatedBy = AppSession.UserLogin.UserID;
                        patientVisitNoteDao.Update(entityEmergencyCaseNote);

                        hdnPatientVisitNoteID.Value = entityEmergencyCaseNote.ID.ToString();
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

        private void BridgingToMedinfrasV1(string type, int ProcessType, PatientDiagnosis entity, PatientProcedure entityProcedure, TestOrderHd entityOrder)
        {
            APIMessageLog apiLog = new APIMessageLog();
            apiLog.MessageDateTime = DateTime.Now;
            apiLog.Sender = Constant.BridgingVendor.HIS;
            apiLog.Recipient = Constant.BridgingVendor.MEDINFRAS_API;

            MedinfrasV1Service oService = new MedinfrasV1Service();
            string serviceResult = string.Empty;
            if (type == "diagnose")
            {
                serviceResult = oService.OnSendPatientDiagnoseInformation(ProcessType, entity);
            }
            else if (type == "procedures")
            {
                serviceResult = oService.OnSendPatientProceduresInformation(ProcessType, entityProcedure);
            }
            else if (type == "order")
            {
                serviceResult = oService.OnSendOrderMedicalDiagnosticServices(ProcessType, entityOrder, null, null);
            }
            else if (type == "allergy")
            {
                serviceResult = oService.OnSendPatientAllergiesInformation(ProcessType, AppSession.RegisteredPatient.RegistrationNo);
            }
            if (!string.IsNullOrEmpty(serviceResult))
            {
                string[] serviceResultInfo = serviceResult.Split('|');
                if (serviceResultInfo[0] == "1")
                {
                    apiLog.IsSuccess = true;
                    apiLog.MessageText = serviceResultInfo[1];
                    apiLog.Response = serviceResultInfo[2];
                }
                else
                {
                    apiLog.IsSuccess = false;
                    apiLog.MessageText = serviceResultInfo[1];
                    apiLog.Response = serviceResultInfo[2];
                    apiLog.ErrorMessage = serviceResultInfo[2];
                }
                BusinessLayer.InsertAPIMessageLog(apiLog);
            }
        }
    }
}

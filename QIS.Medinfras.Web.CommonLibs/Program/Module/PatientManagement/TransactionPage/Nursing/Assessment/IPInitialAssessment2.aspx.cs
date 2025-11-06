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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class IPNurseInitialAssessment2 : BasePagePatientPageList
    {
        protected int gridAllergyPageCount = 1;
        protected int gridVitalSignPageCount = 1;
        protected int gridDiagnosisPageCount = 1;
        protected List<vVitalSignDt> lstVitalSignDt = null;
        protected static string _visitNoteID;
        protected static string _linkedVisitID;

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
            return Constant.MenuCode.Inpatient.NURSE_INITIAL_ASSESSMENT;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        protected override void InitializeDataControl()
        {
            Helper.SetControlEntrySetting(cboVisitType, new ControlEntrySetting(true, true, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtHPISummary, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboFunctionalType, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtFunctionalTypeRemarks, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(rblFamilyRelationship, new ControlEntrySetting(true, true, false, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtFamilyRelationshipRemarks, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(rblIsNeedAdditionalPrivacy, new ControlEntrySetting(true, true, false, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtNeedAdditionalPrivacyRemarks, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboPsychologyStatus, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtCommitSuicideRemarks, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(rblIsNeedAdditionalPrivacy, new ControlEntrySetting(true, true, false, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtFinancialProblemRemarks, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboRAPUH_R, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboRAPUH_A, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboRAPUH_P, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboRAPUH_U, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboRAPUH_H, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtRAPUHScore, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboRAPUHScore, new ControlEntrySetting(true, true, false), "mpPatientStatus");

            txtServiceDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtServiceTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            hdnImagingServiceUnitID.Value = AppSession.ImagingServiceUnitID;
            hdnLaboratoryServiceUnitID.Value = AppSession.LaboratoryServiceUnitID;

            SetEntityToControl();

            BindGridViewDiagnosis(1, true, ref gridDiagnosisPageCount);
            BindGridViewAllergy(1, true, ref gridAllergyPageCount);
            BindGridViewVitalSign(1, true, ref gridVitalSignPageCount);

            LoadBodyDiagram();

            hdnPatientVisitNoteID.Value = _visitNoteID;
            hdnLinkedVisitID.Value = _linkedVisitID;

            PopulateFormContent();
        }

        private void PopulateFormContent()
        {
            string filePath = HttpContext.Current.Server.MapPath("~/Libs/App_Data");

            #region Pemeriksaan Fisik
            string fileName = string.Format(@"{0}\medicalForm\PhysicalExam\{1}", filePath, "physicalExam.html");
            IEnumerable<string> lstText = File.ReadAllLines(fileName);
            StringBuilder innerHtml = new StringBuilder();
            foreach (string text in lstText)
            {
                innerHtml.AppendLine(text);
            }

            divFormContent1.InnerHtml = innerHtml.ToString();
            hdnPhysicalExamLayout.Value = innerHtml.ToString();
            #endregion

            #region Psikososial

            fileName = string.Format(@"{0}\medicalForm\Psychosocial\{1}", filePath, "psychosocial01.html");
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

            fileName = string.Format(@"{0}\medicalForm\Education\{1}", filePath, "education01.html");
            lstText = File.ReadAllLines(fileName);
            innerHtml = new StringBuilder();
            foreach (string text in lstText)
            {
                innerHtml.AppendLine(text);
            }

            divFormContent3.InnerHtml = innerHtml.ToString();
            hdnEducationLayout.Value = innerHtml.ToString();
            #endregion

            #region Perencanaan Pasien Pulang

            fileName = string.Format(@"{0}\medicalForm\DischargePlanning\{1}", filePath, "dischargePlanning01.html");
            lstText = File.ReadAllLines(fileName);
            innerHtml = new StringBuilder();
            foreach (string text in lstText)
            {
                innerHtml.AppendLine(text);
            }

            divFormContent4.InnerHtml = innerHtml.ToString();
            hdnDischargePlanningLayout.Value = innerHtml.ToString();
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

        private void SetEntityToControl()
        {
            vConsultVisit1 entityVisit = BusinessLayer.GetvConsultVisit1List(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            cboVisitType.Value = entityVisit.VisitTypeID.ToString();
            hdnPatientInformation.Value = string.Format("{0} (MRN = {1}, REG = {2}, LOC = {3}, DOB = {4})", entityVisit.cfPatientNameInLabel, entityVisit.MedicalNo, entityVisit.RegistrationNo, entityVisit.ServiceUnitName, entityVisit.DateOfBirthInString);
            hdnDepartmentID.Value = entityVisit.DepartmentID;

            string visitReasonType = entityVisit.GCVisitReason;
            string hospitalIndication = entityVisit.VisitReason;

            if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT)
            {
                vConsultVisit4 entityLinkedRegistration = BusinessLayer.GetvConsultVisit4List(string.Format("RegistrationID = (SELECT LinkedRegistrationID FROM Registration WHERE RegistrationID = {0})", AppSession.RegisteredPatient.RegistrationID)).FirstOrDefault();
                if (entityLinkedRegistration != null)
                {
                    hdnLinkedVisitID.Value = entityLinkedRegistration.VisitID.ToString();
                    hospitalIndication = entityLinkedRegistration.HospitalizationIndication;
                }
                else
                {
                    hdnLinkedVisitID.Value = "0";
                }

            }
            else
            {
                hdnLinkedVisitID.Value = "0";
            }
            _linkedVisitID = hdnLinkedVisitID.Value;

            vNurseChiefComplaint obj = BusinessLayer.GetvNurseChiefComplaintList(string.Format("VisitID = {0} AND IsInitialAssessment = 1", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            if (obj == null)
            {
                hdnChiefComplaintID.Value = "0";

                vNurseChiefComplaint sourceCC = BusinessLayer.GetvNurseChiefComplaintList(string.Format("VisitID = {0} AND IsInitialAssessment = 1", _linkedVisitID)).FirstOrDefault();

                cboVisitReason.Value = Constant.VisitReason.OTHER;
                txtVisitNotes.Text = hospitalIndication;

                if (sourceCC != null)
                {
                    txtChiefComplaint.Text = sourceCC.NurseChiefComplaintText;
                    txtHPISummary.Text = sourceCC.HPISummary;
                    txtMedicalHistory.Text = sourceCC.MedicalHistory;
                    txtMedicationHistory.Text = sourceCC.MedicationHistory;
                    txtFamilyHistory.Text = sourceCC.FamilyHistory; 
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(visitReasonType))
                    cboVisitReason.Value = visitReasonType; 
                txtVisitNotes.Text = hospitalIndication;

                hdnChiefComplaintID.Value = obj.ChiefComplaintID.ToString();
                txtServiceDate.Text = obj.ChiefComplaintDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtServiceTime.Text = obj.ChiefComplaintTime;
                txtChiefComplaint.Text = obj.NurseChiefComplaintText;
                txtHPISummary.Text = obj.HPISummary;
                txtMedicalHistory.Text = obj.MedicalHistory;
                txtMedicationHistory.Text = obj.MedicationHistory;
                txtFamilyHistory.Text = obj.FamilyHistory;
                chkIsPatientAllergyExists.Checked = !chkIsPatientAllergyExists.Checked;
                chkAlloAnamnesis.Checked = obj.IsAlloAnamnesis;
                chkAutoAnamnesis.Checked = obj.IsAutoAnamnesis;
                if (!string.IsNullOrEmpty(obj.GCFamilyRelation))
                {
                    cboFamilyRelation.Value = obj.GCFamilyRelation; 
                }
                if (obj.ClinicalPathwayID != 0)
                {
                    cboClinicalPathway.Value = obj.ClinicalPathwayID;
                }

                hdnPhysicalExamLayout.Value = obj.PhysicalExamLayout;
                hdnPhysicalExamValue.Value = obj.PhysicalExamValues;

                hdnPsychosocialLayout.Value = obj.SocialHistoryLayout;
                hdnPsychosocialValue.Value = obj.SocialHistoryValues;

                hdnEducationLayout.Value = obj.EducationLayout;
                hdnEducationValue.Value = obj.EducationValues;

                hdnDischargePlanningLayout.Value = obj.DischargePlanningLayout;
                hdnDischargePlanningValue.Value = obj.DischargePlanningValues;

                hdnAdditionalLayout.Value = obj.AdditionalAssessmentLayout;
                hdnAdditionalValue.Value = obj.AdditionalAssessmentValues;

                cboFunctionalType.Value = obj.GCFunctionalType;
                txtFunctionalTypeRemarks.Text = obj.FunctionalTypeRemarks;

                chkIsNeedPatientEducation.Checked = obj.IsNeedPatientEducation;

                rblFamilyRelationship.SelectedValue = obj.IsHasGoodFamilyRelationship ? "1" : "0";
                txtFamilyRelationshipRemarks.Text = obj.FamilyRelationshipRemarks;

                rblIsNeedAdditionalPrivacy.SelectedValue = obj.IsNeedAdditionalPrivacy ? "1" : "0";
                txtNeedAdditionalPrivacyRemarks.Text = obj.NeedAdditionalPrivacyRemarks;

                cboPsychologyStatus.Value = obj.GCPsychologyStatus;
                txtCommitSuicideRemarks.Text = obj.ReportToPotentiallyCommitSuicide;

                rblHasFinancialProblem.SelectedValue = obj.IsHasFinancialProblem ? "1" : "0";
                txtFinancialProblemRemarks.Text = obj.FinancialProblemRemarks;

                chkIsHasRAPUHAssessment.Checked = obj.IsHasRAPUHAssessment;
                cboRAPUH_R.Value = obj.GCRAPUH_R;
                cboRAPUH_A.Value = obj.GCRAPUH_A;
                cboRAPUH_P.Value = obj.GCRAPUH_P;
                cboRAPUH_U.Value = obj.GCRAPUH_U;
                cboRAPUH_H.Value = obj.GCRAPUH_H;
                txtRAPUHScore.Text = obj.RAPUHScore.ToString("G29");
                cboRAPUHScore.Value = obj.GCRAPUHScore;

                hdnIsChanged.Value = "0";
            }


            PatientVisitNote oVisitNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = {0} AND GCPatientNoteType = '{1}' AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, Constant.PatientVisitNotes.NURSE_INITIAL_ASSESSMENT)).FirstOrDefault();
            if (oVisitNote != null)
            {
                hdnPatientVisitNoteID.Value = oVisitNote.ID.ToString();
            }
            _visitNoteID = hdnPatientVisitNoteID.Value;
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
            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}') AND IsActive = 1 AND IsDeleted = 0",
    Constant.StandardCode.ALLERGEN_TYPE, Constant.StandardCode.DIAGNOSIS_TYPE,
    Constant.StandardCode.DIFFERENTIAL_DIAGNOSIS_STATUS, Constant.StandardCode.PATIENT_INSTRUCTION_GROUP,
    Constant.StandardCode.MST_WEIGHT_CHANGED, Constant.StandardCode.MST_WEIGHT_CHANGED_GROUP, Constant.StandardCode.MST_DIAGNOSIS,
                    Constant.StandardCode.FUNCTIONAL_TYPE,
                Constant.StandardCode.PSYCHOLOGY_STATUS,
                Constant.StandardCode.RAPUH_RESISTENSI,
                Constant.StandardCode.RAPUH_AKTIFITAS,
                Constant.StandardCode.RAPUH_PENYAKIT,
                Constant.StandardCode.RAPUH_USAHA_BERJALAN,
                Constant.StandardCode.RAPUH_BERAT_BADAN,
                Constant.StandardCode.RAPUH_SCORE,
                Constant.StandardCode.VISIT_REASON,
                Constant.StandardCode.FAMILY_RELATION
    );

            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(filterExpression);

            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField(cboAllergenType, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.ALLERGEN_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboFunctionalType, lstSc.Where(p => p.ParentID == Constant.StandardCode.FUNCTIONAL_TYPE || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboPsychologyStatus, lstSc.Where(p => p.ParentID == Constant.StandardCode.PSYCHOLOGY_STATUS || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboRAPUH_R, lstSc.Where(p => p.ParentID == Constant.StandardCode.RAPUH_RESISTENSI || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboRAPUH_A, lstSc.Where(p => p.ParentID == Constant.StandardCode.RAPUH_AKTIFITAS || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboRAPUH_P, lstSc.Where(p => p.ParentID == Constant.StandardCode.RAPUH_PENYAKIT || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboRAPUH_U, lstSc.Where(p => p.ParentID == Constant.StandardCode.RAPUH_USAHA_BERJALAN || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboRAPUH_H, lstSc.Where(p => p.ParentID == Constant.StandardCode.RAPUH_BERAT_BADAN || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboRAPUHScore, lstSc.Where(p => p.ParentID == Constant.StandardCode.RAPUH_SCORE || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboVisitReason, lstSc.Where(p => p.ParentID == Constant.StandardCode.VISIT_REASON || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboFamilyRelation, lstSc.Where(p => p.ParentID == Constant.StandardCode.FAMILY_RELATION || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

            List<ClinicalPathway> lstClinicalPathway = BusinessLayer.GetClinicalPathwayList(string.Format("IsDeleted = 0"));
            Methods.SetComboBoxField<ClinicalPathway>(cboClinicalPathway, lstClinicalPathway, "ClinicalPathwayName", "ClinicalPathwayID");

            cboAllergenType.Value = Constant.AllergenType.DRUG;

            if (AppSession.UserLogin.ParamedicID != null)
            {
                List<GetParamedicVisitTypeList> visitTypeList = BusinessLayer.GetParamedicVisitTypeList(AppSession.RegisteredPatient.HealthcareServiceUnitID, (int)AppSession.UserLogin.ParamedicID, "");
                Methods.SetComboBoxField(cboVisitType, visitTypeList, "VisitTypeName", "VisitTypeID");
            }
            else
            {
                List<vServiceUnitVisitType> visitTypeList = BusinessLayer.GetvServiceUnitVisitTypeList(string.Format("HealthcareServiceUnitID = {0}", AppSession.RegisteredPatient.HealthcareServiceUnitID));
                Methods.SetComboBoxField(cboVisitType, visitTypeList, "VisitTypeName", "VisitTypeID");
            }
        }

        private void ControlToEntity(PatientVisitNote entitypvn)
        {
            string soapNote = GenerateSOAPText();

            string subjectiveText = string.Empty;
            string objectiveText = string.Empty;
            string assessmentText = string.Empty;
            string planningText = string.Empty;

            entitypvn.NoteText = soapNote;
            entitypvn.PlanningText = hdnPlanningText.Value;
            entitypvn.InstructionText = hdnInstructionText.Value;
            entitypvn.NoteDate = Helper.GetDatePickerValue(txtServiceDate);
            entitypvn.NoteTime = txtServiceTime.Text;
        }

        private string GenerateSOAPSummaryText(ref string subjectiveText, ref string objectiveText, ref string assessmentText, ref string planningText)
        {
            StringBuilder sbNotes = new StringBuilder();
            sbNotes.AppendLine("Subjective :");
            sbNotes.AppendLine("-".PadRight(15, '-'));
            if ((AppSession.RegisteredPatient.ParamedicID == AppSession.UserLogin.ParamedicID))
            {
                vNurseChiefComplaint oChiefComplaint = BusinessLayer.GetvNurseChiefComplaintList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
                if (oChiefComplaint != null)
                {

                    sbNotes.AppendLine(string.Format("{0}   ", oChiefComplaint.NurseChiefComplaintText));
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
                    if (!string.IsNullOrEmpty(txtFamilyHistory.Text))
                    {
                        sbNotes.AppendLine(string.Format("Riwayat Kesehatan Keluarga  : "));
                        sbNotes.AppendLine(string.Format("{0}", txtFamilyHistory.Text));
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

            return sbNotes.ToString();
        }

        private string GenerateSOAPText()
        {
            StringBuilder sbNotes = new StringBuilder();
            StringBuilder sbSubjective = new StringBuilder();
            sbNotes.AppendLine("Subjektive :");
            sbNotes.AppendLine("-".PadRight(15, '-'));
            
            sbNotes.AppendLine(string.Format("Keluhan Utama  : "));
            sbNotes.AppendLine(string.Format(" {0}   ", txtChiefComplaint.Text));
            sbNotes.AppendLine(string.Format("RPS  : "));
            sbNotes.AppendLine(string.Format(" {0}   ", txtHPISummary.Text));
            sbNotes.AppendLine(string.Format("RPD  : "));
            sbNotes.AppendLine(string.Format(" {0}   ", txtMedicalHistory.Text));
            sbNotes.AppendLine(string.Format("Riwayat Keluarga  : "));
            sbNotes.AppendLine(string.Format(" {0}   ", txtFamilyHistory.Text));
            //if ((AppSession.RegisteredPatient.ParamedicID == AppSession.UserLogin.ParamedicID))
            //{

            //}

            hdnSubjectiveText.Value = sbNotes.ToString();

            sbNotes = new StringBuilder();
            sbNotes.AppendLine("Objektif :");
            sbNotes.AppendLine("-".PadRight(15, '-'));

            VitalSignHd lstVitalSignHd = BusinessLayer.GetVitalSignHdList(string.Format("VisitID = {0} AND ObservationDate = '{1}' AND IsDeleted = 0 ORDER BY ID", AppSession.RegisteredPatient.VisitID, Helper.GetDatePickerValue(txtServiceDate).ToString(Constant.FormatString.DATE_FORMAT_112))).FirstOrDefault();
            if (lstVitalSignHd == null)
            {
                sbNotes.AppendLine("");
            }
            else
            {
                List<vVitalSignDt> lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0} ORDER BY DisplayOrder", lstVitalSignHd.ID));
                if (lstVitalSignDt.Count > 0)
                {
                    sbNotes.AppendLine("Tanda Vital :");
                    foreach (vVitalSignDt vitalSign in lstVitalSignDt)
                    {
                        sbNotes.AppendLine(string.Format(" {0} {1} {2}", vitalSign.VitalSignLabel, vitalSign.VitalSignValue, vitalSign.ValueUnit));
                    }
                }
            }

            hdnObjectiveText.Value = sbNotes.ToString();

            return hdnSubjectiveText.Value + " " + sbNotes.ToString();
        }

        private void UpdateConsultVisitRegistration(IDbContext ctx)
        {
            NurseChiefComplaintDao chiefComplaintDao = new NurseChiefComplaintDao(ctx);
            ConsultVisitDao consultVisitDao = new ConsultVisitDao(ctx);
            RegistrationDao registrationDao = new RegistrationDao(ctx);
            ConsultVisit entityConsultVisit = consultVisitDao.Get(AppSession.RegisteredPatient.VisitID);

            NurseChiefComplaint entity = null;
            bool isNewChiefComplaint = true;

            if (hdnChiefComplaintID.Value != "" && hdnChiefComplaintID.Value != "0")
            {
                entity = chiefComplaintDao.Get(Convert.ToInt32(hdnChiefComplaintID.Value));
                isNewChiefComplaint = false;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
            }
            else
            {
                entity = new NurseChiefComplaint();
                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                entity.CreatedBy = AppSession.UserLogin.UserID;
            }
            entity.ChiefComplaintDate = Helper.GetDatePickerValue(txtServiceDate);
            entity.ChiefComplaintTime = txtServiceTime.Text;
            entity.NurseChiefComplaintText = txtChiefComplaint.Text;
            entity.ParamedicID = Convert.ToInt16(AppSession.UserLogin.ParamedicID);
            entity.HPISummary = txtHPISummary.Text; 
            entity.MedicalHistory = txtMedicalHistory.Text;
            entity.MedicationHistory = txtMedicationHistory.Text;
            entity.FamilyHistory = txtFamilyHistory.Text;
            entity.IsPatientAllergyExists = !chkIsPatientAllergyExists.Checked;
            entity.IsInitialAssessment = true;
            entity.IsAutoAnamnesis = Convert.ToBoolean(chkAutoAnamnesis.Checked);
            entity.IsAlloAnamnesis = Convert.ToBoolean(chkAlloAnamnesis.Checked);
            if (cboFamilyRelation.Value != null)
            {
                entity.GCFamilyRelation = cboFamilyRelation.Value.ToString(); 
            }
            entity.IsInitialAssessment = true;

            entity.PhysicalExamLayout = hdnPhysicalExamLayout.Value;
            entity.PhysicalExamValues = hdnPhysicalExamValue.Value;

            entity.SocialHistoryLayout = hdnPsychosocialLayout.Value;
            entity.SocialHistoryValues = hdnPsychosocialValue.Value;

            entity.EducationLayout = hdnEducationLayout.Value;
            entity.EducationValues = hdnEducationValue.Value;

            entity.DischargePlanningLayout = hdnDischargePlanningLayout.Value;
            entity.DischargePlanningValues = hdnDischargePlanningValue.Value;

            entity.AdditionalAssessmentLayout = hdnAdditionalLayout.Value;
            entity.AdditionalAssessmentValues = hdnAdditionalValue.Value;

            //if (cboFunctionalType.Value != null)
            //    entity.GCFunctionalType = cboFunctionalType.Value.ToString();
            //if (!string.IsNullOrEmpty(txtFunctionalTypeRemarks.Text))
            //    entity.FunctionalTypeRemarks = txtFunctionalTypeRemarks.Text;

            entity.IsNeedPatientEducation = chkIsNeedPatientEducation.Checked;

            //entity.IsHasGoodFamilyRelationship = rblFamilyRelationship.SelectedValue == "1" ? true : false;
            //if (!string.IsNullOrEmpty(txtFamilyRelationshipRemarks.Text))
            //    entity.FamilyRelationshipRemarks = txtFamilyRelationshipRemarks.Text;

            //entity.IsNeedAdditionalPrivacy = rblIsNeedAdditionalPrivacy.SelectedValue == "1" ? true : false;
            //if (!string.IsNullOrEmpty(txtNeedAdditionalPrivacyRemarks.Text))
            //    entity.NeedAdditionalPrivacyRemarks = txtNeedAdditionalPrivacyRemarks.Text;

            //if (cboPsychologyStatus.Value != null)
            //    entity.GCPsychologyStatus = cboPsychologyStatus.Value.ToString();
            //if (!string.IsNullOrEmpty(txtCommitSuicideRemarks.Text))
            //    entity.ReportToPotentiallyCommitSuicide = txtCommitSuicideRemarks.Text;

            //entity.IsHasFinancialProblem = rblHasFinancialProblem.SelectedValue == "1" ? true : false;
            //if (!string.IsNullOrEmpty(txtFinancialProblemRemarks.Text))
            //    entity.FinancialProblemRemarks = txtFinancialProblemRemarks.Text;

            //entity.IsHasRAPUHAssessment = chkIsHasRAPUHAssessment.Checked;
            //if (cboRAPUH_R.Value != null)
            //    entity.GCRAPUH_R = cboRAPUH_R.Value.ToString();
            //if (cboRAPUH_A.Value != null)
            //    entity.GCRAPUH_A = cboRAPUH_A.Value.ToString();
            //if (cboRAPUH_P.Value != null)
            //    entity.GCRAPUH_P = cboRAPUH_P.Value.ToString();
            //if (cboRAPUH_U.Value != null)
            //    entity.GCRAPUH_U = cboRAPUH_U.Value.ToString();
            //if (cboRAPUH_H.Value != null)
            //    entity.GCRAPUH_H = cboRAPUH_H.Value.ToString();

            //entity.RAPUHScore = !string.IsNullOrEmpty(txtRAPUHScore.Text) ? Convert.ToInt16(txtRAPUHScore.Text) : 0;
            //if (cboRAPUHScore.Value != null)
            //    entity.GCRAPUHScore = cboRAPUHScore.Value.ToString();

            if (isNewChiefComplaint)
            {
                hdnChiefComplaintID.Value = chiefComplaintDao.InsertReturnPrimaryKeyID(entity).ToString();
            }
            else
            {
                chiefComplaintDao.Update(entity);
            }

            if (cboVisitReason.Value != null)
            {
                entityConsultVisit.GCVisitReason = cboVisitReason.Value.ToString();
                entityConsultVisit.VisitReason = txtVisitNotes.Text;
            }
            else
            {
                entityConsultVisit.VisitReason = txtVisitNotes.Text;
            }

            entityConsultVisit.NurseParamedicID = AppSession.UserLogin.ParamedicID;

            consultVisitDao.Update(entityConsultVisit);

            if (cboClinicalPathway.Value !=  null)
            {
                Registration entityReg = registrationDao.Get(AppSession.RegisteredPatient.RegistrationID);
                entityReg.ClinicalPathwayID = Convert.ToInt32(cboClinicalPathway.Value);
                registrationDao.Update(entityReg);
            }
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            if (type == "save")
            {
                bool result = true;
                IDbContext ctx = DbFactory.Configure(true);
                PatientVisitNoteDao patientVisitNoteDao = new PatientVisitNoteDao(ctx);
                try
                {

                    PatientVisitNote soapNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = {0} AND GCPatientNoteType = '{1}' AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, Constant.PatientVisitNotes.NURSE_INITIAL_ASSESSMENT), ctx).FirstOrDefault();
                    bool isSoapNoteNull = false;
                    if (soapNote == null)
                    {
                        isSoapNoteNull = true;
                        soapNote = new PatientVisitNote();
                    }

                    ControlToEntity(soapNote);

                    if (isSoapNoteNull)
                    {
                        soapNote.NoteDate = Helper.GetDatePickerValue(txtServiceDate);
                        soapNote.NoteTime = txtServiceTime.Text;
                        soapNote.VisitID = AppSession.RegisteredPatient.VisitID;
                        soapNote.ParamedicID = AppSession.UserLogin.ParamedicID;
                        soapNote.GCPatientNoteType = Constant.PatientVisitNotes.NURSE_INITIAL_ASSESSMENT;
                        soapNote.CreatedBy = AppSession.UserLogin.UserID;
                        hdnPatientVisitNoteID.Value = patientVisitNoteDao.InsertReturnPrimaryKeyID(soapNote).ToString();
                    }
                    else
                    {
                        soapNote.ParamedicID = AppSession.UserLogin.ParamedicID;
                        soapNote.LastUpdatedBy = AppSession.UserLogin.UserID;
                        patientVisitNoteDao.Update(soapNote);

                        hdnPatientVisitNoteID.Value = soapNote.ID.ToString();
                    }
                    _visitNoteID = hdnPatientVisitNoteID.Value;

                    NurseChiefComplaint obj = BusinessLayer.GetNurseChiefComplaintList(string.Format("VisitID = {0} AND IsInitialAssessment = 1 AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID), ctx).FirstOrDefault();
                    if (obj != null)
                        hdnChiefComplaintID.Value = obj.ID.ToString();
                    else
                        hdnChiefComplaintID.Value = "0";

                    UpdateConsultVisitRegistration(ctx);
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
            return true;
        }

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

            grdDiagnosisView.DataSource = lstEntity;
            grdDiagnosisView.DataBind();

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
        #endregion

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

            string visitNoteID = !string.IsNullOrEmpty(_visitNoteID) ? _visitNoteID : "0";
            string linkedVisitID = !string.IsNullOrEmpty(_linkedVisitID) ? _linkedVisitID : "0";

            filterExpression += string.Format("VisitID IN ({0},{1}) AND PatientVisitNoteID = {2} AND IsInitialAssessment = 1 AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, linkedVisitID, visitNoteID);

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

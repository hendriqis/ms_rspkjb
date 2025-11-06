﻿using System;
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

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class MedicalResumeEntry1 : BasePagePatientPageList
    {
        protected int gridAllergyPageCount = 1;
        protected int gridVitalSignPageCount = 1;
        protected int gridROSPageCount = 1;
        protected int gridROSINPageCount = 1;
        protected int gridDiagnosisPageCount = 1;
        protected int gridLaboratoryPageCount = 1;
        protected int gridImagingPageCount = 1;
        protected int gridDiagnosticPageCount = 1;
        protected int gridInstructionPageCount = 1;
        protected List<vVitalSignDt> lstVitalSignDt = null;
        protected List<vReviewOfSystemDt> lstReviewOfSystemDt = null;

        protected List<vReviewOfSystemDt> lstReviewOfSystemInDt = null;

        protected static string _medicalResumeID = "0";
        protected static string _linkedVisitID;

        protected string GetUserID()
        {
            return AppSession.UserLogin.UserID.ToString();
        }

        protected string GetMedicalResumeID()
        {
            return _medicalResumeID;
        }

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.MEDICAL_RESUME;
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString["id"] != null)
            {
                hdnMedicalResumeID.Value = string.IsNullOrEmpty(Page.Request.QueryString["id"]) ? "0" : Page.Request.QueryString["id"];
            }


            Helper.SetControlEntrySetting(txtSubjectiveResumeText, new ControlEntrySetting(true, true, true), "mpPatientStatus");

            txtResumeDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtResumeTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            txtDateOfDeath.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTimeOfDeath.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            SetEntityToControl();

            if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
            {
                Helper.SetControlEntrySetting(txtHospitalIndication, new ControlEntrySetting(true, true, true), "mpPatientStatus");
                trHospitalIndication.Attributes.Remove("style");
            }
            else
            {
                Helper.SetControlEntrySetting(txtHospitalIndication, new ControlEntrySetting(true, true, false), "mpPatientStatus");
                trHospitalIndication.Attributes.Add("style", "display:none");
            }

            if (AppSession.IsBridgingToEKlaim)
            {
                trEKlaim1.Style.Add("display", "table-row");
            }
            else
            {
                trEKlaim1.Style.Add("display", "none");
            }

            BindGridViewVitalSign(1, true, ref gridVitalSignPageCount);
            BindGridViewROS(1, true, ref gridROSPageCount);
            BindGridViewDiagnosis(1, true, ref gridDiagnosisPageCount);
            BindGridViewROSIN(1, true, ref gridROSINPageCount);
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnLinkedVisitID.Value = _linkedVisitID;

            string filterSetVar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
                                                        AppSession.UserLogin.HealthcareID, //1
                                                        Constant.SettingParameter.EM_CASEMIX_ALLOW_CHANGE_MEDICAL_RESUME, //2
                                                        Constant.SettingParameter.EM_IS_MEDICAL_RESUME_CAN_INSERT_RESIDUAL_PRESCRIPTION //3
                                                    );
            List<SettingParameterDt> lstSetVarDt = BusinessLayer.GetSettingParameterDtList(filterSetVar);

            hdnIsCasemixAllowChange.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.EM_CASEMIX_ALLOW_CHANGE_MEDICAL_RESUME).FirstOrDefault().ParameterValue;
            hdnIsHasResidualEffect.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.EM_IS_MEDICAL_RESUME_CAN_INSERT_RESIDUAL_PRESCRIPTION).FirstOrDefault().ParameterValue;

            if (hdnIsHasResidualEffect.Value == "1")
            {
                trResidualEffect.Attributes.Remove("style");
            }
            else
            {
                trResidualEffect.Attributes.Add("style", "display:none");
            }
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        private void SetEntityToControl()
        {
            List<SettingParameterDt> lstSetpar = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode = '{0}' AND HealthcareID = '{1}'", Constant.SettingParameter.EM0057, AppSession.UserLogin.HealthcareID));
            String EM0057 = lstSetpar.Where(p => p.ParameterCode == Constant.SettingParameter.EM0057).FirstOrDefault().ParameterValue;
            if (EM0057 == "1")
            {
                divPage11_tab.Style.Remove("display");
                divPage2_tab.InnerText = "Pemeriksaan Fisik Saat Keluar";
                divPage2_tab.Attributes.Add("title", "Pemeriksaan Fisik Saat Keluar");
            }
            else
            {
                divPage2_tab.InnerText = "Pemeriksaan Fisik";
                divPage2_tab.Attributes.Add("title", "Pemeriksaan Fisik");
            }

            vConsultVisit1 entityVisit = BusinessLayer.GetvConsultVisit1List(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnPatientInformation.Value = string.Format("{0} (MRN = {1}, REG = {2}, LOC = {3}, DOB = {4})", entityVisit.cfPatientNameInLabel, entityVisit.MedicalNo, entityVisit.RegistrationNo, entityVisit.ServiceUnitName, entityVisit.DateOfBirthInString);
            hdnDepartmentID.Value = entityVisit.DepartmentID;
            txtHospitalIndication.Text = entityVisit.HospitalizationIndication;
            cboPatientOutcome.Value = entityVisit.GCDischargeCondition;
            cboDischargeRoutine.Value = entityVisit.GCDischargeMethod;
            if (entityVisit.GCDischargeCondition == Constant.PatientOutcome.DEAD_BEFORE_48 || entityVisit.GCDischargeCondition == Constant.PatientOutcome.DEAD_AFTER_48)
            {
                txtDateOfDeath.Text = entityVisit.DateOfDeath.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtTimeOfDeath.Text = entityVisit.TimeOfDeath;
            }
            hdnReferrerID.Value = entityVisit.ReferralTo.ToString();
            cboReferrerGroup.Value = entityVisit.GCReferrerGroup;
            hdnGCReferrerGroup.Value = entityVisit.GCReferrerGroup;
            txtReferrerCode.Text = entityVisit.ReferrerCode;
            txtReferrerName.Text = entityVisit.ReferrerName;
            cboDischargeReason.Value = entityVisit.GCReferralDischargeReason;
            txtDischargeOtherReason.Text = entityVisit.ReferralDischargeReasonOther;

            if (entityVisit.PlanFollowUpVisitDate.Year != 1900)
            {
                txtPlanFollowUpVisitDate.Text = entityVisit.PlanFollowUpVisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            }

            if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT)
            {
                vConsultVisit4 entityLinkedRegistration = BusinessLayer.GetvConsultVisit4List(string.Format("RegistrationID = (SELECT LinkedRegistrationID FROM Registration WHERE RegistrationID = {0})", AppSession.RegisteredPatient.RegistrationID)).FirstOrDefault();
                if (entityLinkedRegistration != null)
                {
                    hdnLinkedVisitID.Value = entityLinkedRegistration.VisitID.ToString();
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

            if (string.IsNullOrEmpty(hdnMedicalResumeID.Value) || hdnMedicalResumeID.Value == "0")
            {
                hdnMedicalResumeID.Value = "0";
                _medicalResumeID = "0";
                hdnParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();

                vConsultVisit4 objLinkedVisitID = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", hdnLinkedVisitID.Value)).FirstOrDefault();

                if (objLinkedVisitID != null)
                {
                    txtHospitalIndication.Text = objLinkedVisitID.HospitalizationIndication;
                }

                //New Medical Resume
                vChiefComplaint sourceCC = BusinessLayer.GetvChiefComplaintList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
                if (sourceCC != null)
                {
                    StringBuilder subjectiveResumeText = new StringBuilder();

                    subjectiveResumeText.AppendLine(sourceCC.ChiefComplaintText);
                    if (!string.IsNullOrEmpty(sourceCC.HPISummary))
                    {
                        subjectiveResumeText.AppendLine("Keluhan lain yang menyertai :");
                        subjectiveResumeText.AppendLine(sourceCC.HPISummary);
                    }

                    List<vPatientAllergy> lstAllergy = BusinessLayer.GetvPatientAllergyList(string.Format("MRN = {0} AND IsDeleted = 0 ORDER BY GCAllergenType", sourceCC.MRN));
                    if (lstAllergy.Count >= 1)
                    {
                        StringBuilder allergyResume1 = new StringBuilder();
                        string allergenType = lstAllergy.FirstOrDefault().AllergenType;
                        string allergenOrder = string.Empty;
                        foreach (vPatientAllergy item in lstAllergy)
                        {
                            if (allergyResume1.ToString() == string.Empty)
                            {
                                switch (item.GCAllergenType)
                                {
                                    case Constant.AllergenType.DRUG:
                                        allergyResume1.AppendLine("OBAT");
                                        allergenOrder = item.AllergenType;
                                        break;
                                    case Constant.AllergenType.FOOD:
                                        allergyResume1.AppendLine("MAKANAN");
                                        allergenOrder = item.AllergenType;
                                        break;
                                    default:
                                        allergyResume1.AppendLine("LAIN-LAIN");
                                        allergenOrder = item.AllergenType;
                                        break;
                                }
                            }
                            else
                            {
                                if (item.AllergenType != allergenOrder)
                                {
                                    switch (item.GCAllergenType)
                                    {
                                        case Constant.AllergenType.DRUG:
                                            allergyResume1.AppendLine("OBAT");
                                            allergenOrder = item.AllergenType;
                                            break;
                                        case Constant.AllergenType.FOOD:
                                            allergyResume1.AppendLine("MAKANAN");
                                            allergenOrder = item.AllergenType;
                                            break;
                                        default:
                                            allergyResume1.AppendLine("LAIN-LAIN");
                                            allergenOrder = item.AllergenType;
                                            break;
                                    }
                                }
                            }
                            allergyResume1.AppendLine(string.Format("- {0}", item.Allergen.TrimEnd()));
                        }
                        subjectiveResumeText.AppendLine("Riwayat Alergi: \r\n" + allergyResume1.ToString());
                    }

                    txtResumeDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtResumeTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                    txtSubjectiveResumeText.Text = subjectiveResumeText.ToString();

                    if (!string.IsNullOrEmpty(sourceCC.PastMedicalHistory))
                    {
                        StringBuilder comorbidities = new StringBuilder();
                        comorbidities.AppendLine(sourceCC.PastMedicalHistory);
                        txtComorbiditiesText.Text = comorbidities.ToString();
                    }

                    txtPlanningResumeText.Text = string.Format("Hasil Pemeriksaan Penunjang - Terlampir {0}", Environment.NewLine);


                    string filterProcedures = string.Format("VisitID = {0} AND IsDeleted = 0", entityVisit.VisitID);
                    List<PatientProcedure> lstProcedure = BusinessLayer.GetPatientProcedureList(filterProcedures);
                    StringBuilder procedure = new StringBuilder();
                    foreach (PatientProcedure e in lstProcedure)
                    {
                        procedure.AppendLine(string.Format("- {0}", e.ProcedureText));
                    }
                    txtSurgeryResumeText.Text = procedure.ToString();

                    hdnIsChanged.Value = "1";
                    hdnIsSaved.Value = "0";
                }
                hdnIsCasemixRevision.Value = "0";
            }
            else
            {
                vMedicalResume obj = BusinessLayer.GetvMedicalResumeList(string.Format("ID = {0} AND IsDeleted = 0", hdnMedicalResumeID.Value)).FirstOrDefault();
                if (obj != null)
                {
                    _medicalResumeID = obj.ID.ToString();
                    hdnMedicalResumeID.Value = obj.ID.ToString();
                    hdnParamedicID.Value = obj.ParamedicID.ToString();
                    txtResumeDate.Text = obj.MedicalResumeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtResumeTime.Text = obj.MedicalResumeTime;
                    txtSubjectiveResumeText.Text = obj.SubjectiveResumeText;
                    txtObjectiveResumeText.Text = obj.ObjectiveResumeText;
                    txtMedicalResumeText.Text = obj.MedicalResumeText;
                    txtMedicationResumeText.Text = obj.MedicationResumeText;
                    txtPlanningResumeText.Text = obj.PlanningResumeText;

                    txtComorbiditiesText.Text = obj.ComorbiditiesText;
                    txtDischargeMedicationResumeText.Text = obj.DischargeMedicationResumeText;
                    txtDischargeMedicalSummary.Text = obj.DischargeMedicalResumeText;
                    txtPrescriptionResidualEffectText.Text = obj.ResidualMedicationResumeText;
                    txtSurgeryResumeText.Text = obj.SurgeryResumeText;
                    txtInstructionResumeText.Text = obj.InstructionResumeText;

                    rblIsHasSickLetter.SelectedValue = obj.IsHasSickLetter ? "1" : "0";
                    txtNoOfDays.Text = obj.NoOfAbsenceDays.ToString();

                    hdnIsCasemixRevision.Value = obj.IsCasemixRevision ? "1" : "0";
                    hdnRevisionParamedicID.Value = obj.RevisedByParamedicID.ToString();

                    hdnIsChanged.Value = "0";
                    hdnIsSaved.Value = "0";
                }
            }
        }

        protected override void SetControlProperties()
        {
            string Department = hdnDepartmentID.Value;
            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}') AND IsActive = 1 AND IsDeleted = 0",
                                            Constant.StandardCode.ALLERGEN_TYPE, Constant.StandardCode.DIAGNOSIS_TYPE,
                                            Constant.StandardCode.DIFFERENTIAL_DIAGNOSIS_STATUS, Constant.StandardCode.PATIENT_INSTRUCTION_GROUP,
                                            Constant.StandardCode.MST_WEIGHT_CHANGED, Constant.StandardCode.MST_WEIGHT_CHANGED_GROUP, Constant.StandardCode.MST_DIAGNOSIS,
                                            Constant.StandardCode.PATIENT_OUTCOME, Constant.StandardCode.DISCHARGE_ROUTINE, Constant.StandardCode.DISCHARGE_REASON_TO_OTHER_HOSPITAL);

            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(filterExpression);
            List<StandardCode> lstSc2 = BusinessLayer.GetStandardCodeList(String.Format("StandardCodeID IN ('{0}','{1}')", Constant.Referrer.FASKES, Constant.Referrer.RUMAH_SAKIT));


            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField(cboDiagnosisType, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.DIAGNOSIS_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboDiagnosisStatus, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.DIFFERENTIAL_DIAGNOSIS_STATUS).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboDiagnosisTypeEKlaim, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.DIAGNOSIS_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboPatientOutcome, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.PATIENT_OUTCOME).ToList(), "StandardCodeName", "StandardCodeID");
            if (Department == Constant.Facility.INPATIENT)
            {
                Methods.SetComboBoxField<StandardCode>(cboDischargeRoutine, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.DISCHARGE_ROUTINE && sc.StandardCodeID != Constant.DischargeMethod.DISCHARGED_TO_WARD && sc.IsActive == true).OrderBy(lst => lst.TagProperty).ToList(), "StandardCodeName", "StandardCodeID");
            }
            else
            {
                Methods.SetComboBoxField<StandardCode>(cboDischargeRoutine, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.DISCHARGE_ROUTINE && sc.IsActive == true).OrderBy(lst => lst.TagProperty).ToList(), "StandardCodeName", "StandardCodeID");
            }
            Methods.SetComboBoxField<StandardCode>(cboReferrerGroup, lstSc2.ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDischargeReason, lstSc.Where(p => p.ParentID == Constant.StandardCode.DISCHARGE_REASON_TO_OTHER_HOSPITAL).ToList(), "StandardCodeName", "StandardCodeID");

            cboDiagnosisStatus.Value = Constant.DifferentialDiagnosisStatus.UNDER_INVESTIGATION;
        }

        private void ControlToEntity(PatientVisitNote entitypvn)
        {
            string soapNote = GenerateSOAPText();

            entitypvn.NoteDate = Helper.GetDatePickerValue(txtResumeDate);
            entitypvn.NoteTime = txtResumeTime.Text;

            entitypvn.SubjectiveText = hdnSubjectiveText.Value;
            entitypvn.ObjectiveText = hdnObjectiveText.Value;
            entitypvn.AssessmentText = hdnAssessmentText.Value;
            entitypvn.PlanningText = hdnPlanningText.Value;
            entitypvn.InstructionText = hdnInstructionText.Value;
            entitypvn.NoteText = soapNote;
        }

        private string GenerateSOAPText()
        {
            StringBuilder sbNotes = new StringBuilder();
            StringBuilder sbSubjective = new StringBuilder();
            sbNotes.AppendLine("SUBJEKTIVE :");
            sbNotes.AppendLine("-".PadRight(20, '-'));

            sbNotes.AppendLine(string.Format("Keluhan Utama  : "));
            sbNotes.AppendLine(string.Format(" {0}   ", txtHospitalIndication.Text));
            if (!string.IsNullOrEmpty(txtSubjectiveResumeText.Text))
            {
                sbNotes.AppendLine(string.Format("Riwayat Penyakit Sekarang  : "));
                sbNotes.AppendLine(string.Format(" {0}   ", txtSubjectiveResumeText.Text));
            }
            if (!string.IsNullOrEmpty(txtMedicalHistory.Text))
            {
                sbNotes.AppendLine(string.Format("Riwayat Penyakit Dahulu  : "));
                sbNotes.AppendLine(string.Format(" {0}   ", txtMedicalHistory.Text));
            }
            hdnSubjectiveText.Value = sbNotes.ToString();

            sbNotes.AppendLine(" ");
            sbNotes.AppendLine("OBJEKTIF :");
            sbNotes.AppendLine("-".PadRight(20, '-'));
            vVitalSignHd entityVitalSignHd = BusinessLayer.GetvVitalSignHdList(string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            if (entityVitalSignHd != null)
            {
                List<vVitalSignDt> lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0} AND ObservationDate = '{1}' AND IsDeleted = 0 ORDER BY DisplayOrder", entityVitalSignHd.ID, Helper.GetDatePickerValue(txtResumeDate).ToString(Constant.FormatString.DATE_FORMAT_112)));
                if (lstVitalSignDt.Count > 0)
                {
                    sbNotes.AppendLine("Tanda Vital :");
                    foreach (vVitalSignDt vitalSign in lstVitalSignDt)
                    {
                        sbNotes.AppendLine(string.Format(" - {0} {1} {2}", vitalSign.VitalSignLabel, vitalSign.VitalSignValue, vitalSign.ValueUnit));
                    }
                }
            }

            vReviewOfSystemHd entityReviewOfSystem = BusinessLayer.GetvReviewOfSystemHdList(string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            if (entityReviewOfSystem != null)
            {
                List<vReviewOfSystemDt> lstROS = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID = {0} AND IsDeleted = 0 ORDER BY GCRoSystem", entityReviewOfSystem.ID, Helper.GetDatePickerValue(txtResumeDate).ToString(Constant.FormatString.DATE_FORMAT_112)));
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

            if (!string.IsNullOrEmpty(txtPlanningResumeText.Text))
            {
                sbNotes.AppendLine(" ");
                sbNotes.AppendLine("Catatan Hasil Pemeriksaan Penunjang: ");
                sbNotes.AppendLine(txtPlanningResumeText.Text);
            }

            hdnObjectiveText.Value = sbNotes.ToString();

            sbNotes.AppendLine(" ");
            sbNotes.AppendLine("ASSESSMENT :");
            sbNotes.AppendLine("-".PadRight(20, '-'));
            sbNotes.AppendLine(string.Format(" {0}", hdnAssessmentText.Value));

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
            //sbNotes.AppendLine(string.Format(" {0}", hdnPlanningText.Value));

            sbNotes.AppendLine(" ");
            sbNotes.AppendLine("INSTRUKSI : ");
            sbNotes.AppendLine("-".PadRight(20, '-'));
            sbNotes.AppendLine(hdnInstructionText.Value);

            return sbNotes.ToString();
        }

        private void UpdateConsultVisitRegistration(IDbContext ctx)
        {
            MedicalResumeDao medicalResumeDao = new MedicalResumeDao(ctx);
            ConsultVisitDao consultVisitDao = new ConsultVisitDao(ctx);
            PatientDao patientDao = new PatientDao(ctx);

            MedicalResume entity = null;
            bool isNewMedicalResume = true;
            string objectiveText = string.Empty;

            objectiveText = txtObjectiveResumeText.Text;

            if (hdnMedicalResumeID.Value != "" && hdnMedicalResumeID.Value != "0")
            {
                entity = medicalResumeDao.Get(Convert.ToInt32(hdnMedicalResumeID.Value));
                isNewMedicalResume = false;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
            }
            else
            {
                entity = new MedicalResume();
                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                entity.CreatedBy = AppSession.UserLogin.UserID;
            }
            entity.MedicalResumeDate = Helper.GetDatePickerValue(txtResumeDate);
            entity.MedicalResumeTime = txtResumeTime.Text;
            entity.SubjectiveResumeText = txtSubjectiveResumeText.Text;
            entity.ObjectiveResumeText = objectiveText;
            entity.MedicationResumeText = txtMedicationResumeText.Text;
            entity.MedicalResumeText = txtMedicalResumeText.Text;
            entity.ComorbiditiesText = txtComorbiditiesText.Text;
            entity.PlanningResumeText = txtPlanningResumeText.Text;
            entity.DischargeMedicationResumeText = txtDischargeMedicationResumeText.Text;
            entity.DischargeMedicalResumeText = txtDischargeMedicalSummary.Text;
            entity.ResidualMedicationResumeText = txtPrescriptionResidualEffectText.Text;
            entity.SurgeryResumeText = txtSurgeryResumeText.Text;
            entity.InstructionResumeText = txtInstructionResumeText.Text;

            entity.IsHasSickLetter = rblIsHasSickLetter.SelectedValue == "1";
            if (!string.IsNullOrEmpty(txtNoOfDays.Text))
            {
                entity.NoOfAbsenceDays = Convert.ToInt32(txtNoOfDays.Text);
            }

            if (isNewMedicalResume)
            {
                entity.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                hdnParamedicID.Value = entity.ParamedicID.ToString();
                _medicalResumeID = medicalResumeDao.InsertReturnPrimaryKeyID(entity).ToString();
                hdnMedicalResumeID.Value = _medicalResumeID.ToString();
            }
            else
            {
                medicalResumeDao.Update(entity);
            }

            if (hdnMedicalResumeID.Value != "0" && hdnMedicalResumeID.Value != "")
            {
                ConsultVisit oVisit = consultVisitDao.Get(AppSession.RegisteredPatient.VisitID);
                if (oVisit != null)
                {
                    oVisit.HospitalizationIndication = txtHospitalIndication.Text;
                    if (cboPatientOutcome.Value != null)
                    {
                        oVisit.GCDischargeCondition = cboPatientOutcome.Value.ToString();
                        oVisit.GCDischargeMethod = cboDischargeRoutine.Value.ToString();

                        if (cboPatientOutcome.Value.ToString() == Constant.PatientOutcome.DEAD_BEFORE_48 || cboPatientOutcome.Value.ToString() == Constant.PatientOutcome.DEAD_AFTER_48)
                        {
                            oVisit.DateOfDeath = Helper.GetDatePickerValue(txtDateOfDeath);
                            oVisit.TimeOfDeath = txtTimeOfDeath.Text;

                            oVisit.ReferralUnitID = null;
                            oVisit.ReferralPhysicianID = null;
                            oVisit.ReferralDate = Helper.InitializeDateTimeNull();
                            oVisit.IsRefferralProcessed = false;

                            //Update Patient Death Status
                            Patient oPatient = patientDao.Get(AppSession.RegisteredPatient.MRN);
                            if (oPatient != null)
                            {
                                oPatient.IsAlive = false;
                                oPatient.DateOfDeath = Helper.GetDatePickerValue(txtDateOfDeath);
                            }
                            else
                            {
                                oPatient.IsAlive = true;
                                oPatient.DateOfDeath = DateTime.MinValue;
                            }
                            oPatient.LastVisitDate = AppSession.RegisteredPatient.VisitDate;
                            oPatient.LastUpdatedBy = AppSession.UserLogin.UserID;
                            patientDao.Update(oPatient);
                        }
                    }

                    if (cboDischargeRoutine.Value != null)
                    {
                        if (cboDischargeRoutine.Value.ToString() == Constant.DischargeMethod.REFFERRED_TO_EXTERNAL_PROVIDER)
                        {
                            if (hdnReferrerID.Value != null && hdnReferrerID.Value != "" && hdnReferrerID.Value != "0")
                            {
                                oVisit.ReferralTo = Convert.ToInt32(hdnReferrerID.Value);
                            }

                            if (cboDischargeReason.Value != null)
                            {
                                oVisit.GCReferralDischargeReason = cboDischargeReason.Value.ToString();
                                oVisit.ReferralDischargeReasonOther = txtDischargeOtherReason.Text;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(txtPlanFollowUpVisitDate.Text))
                    {
                        oVisit.PlanFollowUpVisitDate = Helper.GetDatePickerValue(txtPlanFollowUpVisitDate);
                    }

                    consultVisitDao.Update(oVisit);
                }
            }
        }

        protected override bool OnCustomButtonClick(string type, ref string message)
        {
            if (type == "save")
            {
                bool result = true;

                if ((hdnMedicalResumeID.Value != "" && hdnMedicalResumeID.Value != "0") && !IsValidToSave(ref message))
                {
                    result = false;
                    hdnIsSaved.Value = "0";
                    return result;
                }

                IDbContext ctx = DbFactory.Configure(true);
                try
                {
                    //MedicalResume obj = BusinessLayer.GetMedicalResumeList(string.Format("ID = {0} AND IsDeleted = 0", hdnMedicalResumeID.Value), ctx).FirstOrDefault();
                    //if (obj != null)
                    //    hdnMedicalResumeID.Value = obj.ID.ToString();
                    //else
                    //    hdnMedicalResumeID.Value = "0";

                    UpdateConsultVisitRegistration(ctx);
                    ctx.CommitTransaction();

                    if (hdnMedicalResumeID.Value != "0")
                    {
                        hdnIsSaved.Value = "1";
                        hdnIsChanged.Value = "0";
                    }
                    else
                    {
                        hdnIsSaved.Value = "0";
                        hdnIsChanged.Value = "1";
                    }

                    message = hdnMedicalResumeID.Value;
                }
                catch (Exception ex)
                {
                    result = false;
                    message = ex.Message;
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

        #region Vital Sign
        private void BindGridViewVitalSign(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Empty;

            string medicalResumeID = (!string.IsNullOrEmpty(_medicalResumeID) && _medicalResumeID != "0") ? _medicalResumeID : "-1";
            string linkedVisitID = !string.IsNullOrEmpty(_linkedVisitID) ? _linkedVisitID : "0";

            filterExpression += string.Format("VisitID IN ({0}) AND MedicalResumeID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, _medicalResumeID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvVitalSignHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_COMPACT);
            }

            List<vVitalSignHd> lstEntity = BusinessLayer.GetvVitalSignHdList(filterExpression, Constant.GridViewPageSize.GRID_COMPACT, pageIndex, "ID DESC");
            lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID IN ({0}) AND MedicalResumeID = {1} ORDER BY DisplayOrder", AppSession.RegisteredPatient.VisitID, _medicalResumeID));
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

        #region Review Of System (IN)
        protected void cbpROSInView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewROSIN(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewROSIN(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridViewROSIN(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string medicalResumeID = (!string.IsNullOrEmpty(_medicalResumeID) && _medicalResumeID != "0") ? _medicalResumeID : "-1";

            Registration entityRegistration = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0}", hdnRegistrationID.Value)).FirstOrDefault();
            string filterExpression = "";
            if (entityRegistration.LinkedRegistrationID != null)
            {
                ConsultVisit entityLinkedentityVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", entityRegistration.LinkedRegistrationID)).FirstOrDefault();
                filterExpression = string.Format("VisitID IN ({0}) AND MedicalResumeID IS NULL AND IsDeleted = 0", entityLinkedentityVisit.VisitID);
            }
            else
            {
                ConsultVisit entityVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", entityRegistration.RegistrationID)).FirstOrDefault();
                filterExpression = string.Format("VisitID IN ({0}) AND MedicalResumeID IS NULL AND IsDeleted = 0", entityVisit.VisitID);
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvReviewOfSystemHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vReviewOfSystemHd> lstEntity = BusinessLayer.GetvReviewOfSystemHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
            lstReviewOfSystemInDt = BusinessLayer.GetvReviewOfSystemDtList(filterExpression);
            grdROSInView.DataSource = lstEntity;
            grdROSInView.DataBind();
        }

        protected void grdROSINView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vReviewOfSystemHd obj = (vReviewOfSystemHd)e.Row.DataItem;
                Repeater rptReviewOfSystemINDt = (Repeater)e.Row.FindControl("rptReviewOfSystemINDt");
                rptReviewOfSystemINDt.DataSource = GetReviewOfSystemINDt(obj.ID);
                rptReviewOfSystemINDt.DataBind();
            }
        }

        protected List<vReviewOfSystemDt> GetReviewOfSystemINDt(Int32 ID)
        {
            return lstReviewOfSystemInDt.Where(p => p.ID == ID).ToList();
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
            string medicalResumeID = (!string.IsNullOrEmpty(_medicalResumeID) && _medicalResumeID != "0") ? _medicalResumeID : "-1";

            string filterExpression = string.Format("VisitID IN ({0}) AND MedicalResumeID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, medicalResumeID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvReviewOfSystemHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vReviewOfSystemHd> lstEntity = BusinessLayer.GetvReviewOfSystemHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
            lstReviewOfSystemDt = BusinessLayer.GetvReviewOfSystemDtList(filterExpression);
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
                    strDiagnosis.AppendLine(string.Format("- {0}", item.cfDiagnosisText));
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
                        entity.DifferentialDate = Helper.GetDatePickerValue(txtResumeDate);
                        entity.DifferentialTime = txtResumeTime.Text;

                        entity.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                        entity.GCDiagnoseType = cboDiagnosisType.Value.ToString();
                        hdnIsMainDiagnosisExists.Value = entity.GCDiagnoseType == Constant.DiagnoseType.MAIN_DIAGNOSIS ? "1" : "0";
                        entity.GCDifferentialStatus = cboDiagnosisStatus.Value.ToString();

                        if (hdnEntryDiagnoseID.Value != "")
                            entity.DiagnoseID = hdnEntryDiagnoseID.Value;
                        else
                            entity.DiagnoseID = null;

                        if (AppSession.IsBridgingToEKlaim)
                        {
                            if (hdnEntryDiagnoseID2.Value != "")
                            {
                                entity.ClaimDiagnosisID = hdnEntryDiagnoseID2.Value;
                                entity.ClaimINADiagnoseID = hdnEntryDiagnoseID2.Value;
                                if (hdnIsCasemixRevision.Value == "1")
                                    entity.IsIgnorePhysicianDiagnose = true;
                                else
                                    entity.IsIgnorePhysicianDiagnose = false;
                            }
                            else
                            {
                                entity.ClaimDiagnosisID = null;
                                entity.ClaimINADiagnoseID = null;
                            }
                            entity.ClaimDiagnosisText = hdnEntryDiagnoseText2.Value;
                            entity.ClaimINADiagnoseText = hdnEntryDiagnoseID2.Value;

                            if (cboDiagnosisTypeEKlaim.Value != null)
                            {
                                entity.GCDiagnoseTypeClaim = cboDiagnosisTypeEKlaim.Value.ToString();
                            }
                            entity.ClaimDiagnosisBy = AppSession.UserLogin.UserID;
                        }

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
                            entity.DifferentialDate = Helper.GetDatePickerValue(txtResumeDate);
                            entity.DifferentialTime = txtResumeTime.Text;

                            entity.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                            entity.GCDiagnoseType = cboDiagnosisType.Value.ToString();
                            hdnIsMainDiagnosisExists.Value = entity.GCDiagnoseType == Constant.DiagnoseType.MAIN_DIAGNOSIS ? "1" : "0";
                            entity.GCDifferentialStatus = cboDiagnosisStatus.Value.ToString();

                            if (hdnEntryDiagnoseID.Value != "")
                                entity.DiagnoseID = hdnEntryDiagnoseID.Value;
                            else
                                entity.DiagnoseID = null;
                            entity.DiagnosisText = txtDiagnosisText.Text;

                            if (AppSession.IsBridgingToEKlaim)
                            {
                                if (hdnEntryDiagnoseID2.Value != "")
                                {
                                    entity.ClaimDiagnosisID = hdnEntryDiagnoseID2.Value;
                                    entity.ClaimINADiagnoseID = hdnEntryDiagnoseID2.Value;
                                }
                                else
                                {
                                    entity.ClaimDiagnosisID = null;
                                    entity.ClaimINADiagnoseID = null;
                                }
                                entity.ClaimDiagnosisText = hdnEntryDiagnoseText2.Value;
                                if (cboDiagnosisTypeEKlaim.Value != null)
                                {
                                    entity.GCDiagnoseTypeClaim = cboDiagnosisTypeEKlaim.Value.ToString();
                                }
                                entity.ClaimDiagnosisBy = AppSession.UserLogin.UserID;
                            }

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
                            //TODO : Prompt user for delete reason
                            entity.IsDeleted = true;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdatePatientDiagnosis(entity);
                            result = "1|delete|";
                        }
                        else
                        {
                            result = string.Format("0|edit|{0}", "Invalid Patient Diagnosis Record Information");
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

        private bool IsValidToSave(ref string errMessage)
        {
            StringBuilder errMsg = new StringBuilder();

            if ((hdnParamedicID.Value != "" && hdnParamedicID.Value != "0") || (hdnRevisionParamedicID.Value != "" && hdnRevisionParamedicID.Value != "0"))
            {
                int paramedicID = Convert.ToInt32(hdnParamedicID.Value);
                int revisionParamedicID = 0;

                if (!String.IsNullOrEmpty(hdnRevisionParamedicID.Value) && hdnRevisionParamedicID.Value != "0")
                {
                    revisionParamedicID = Convert.ToInt32(hdnRevisionParamedicID.Value);
                }

                if (hdnIsCasemixAllowChange.Value == "0")
                {
                    if ((AppSession.UserLogin.ParamedicID != paramedicID))
                    {
                        errMsg.AppendLine("Perubahan Kajian Awal Pasien hanya dapat dilakukan oleh Dokter yang melakukan Pengkajian atau Revisi Pengkajian");
                    }
                    else if (revisionParamedicID != 0)
                    {
                        if (AppSession.UserLogin.ParamedicID != revisionParamedicID)
                        {
                            errMsg.AppendLine("Perubahan Kajian Awal Pasien hanya dapat dilakukan oleh Dokter yang melakukan Pengkajian atau Revisi Pengkajian");
                        }
                    }
                }
            }
            errMessage = errMsg.ToString();

            return (errMessage == string.Empty);
        }
    }
}

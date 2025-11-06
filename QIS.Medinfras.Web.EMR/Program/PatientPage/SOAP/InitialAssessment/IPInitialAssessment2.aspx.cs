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

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class IPInitialAssessmentER1 : BasePagePatientPageList
    {
        protected int gridAllergyPageCount = 1;
        protected int gridVitalSignPageCount = 1;
        protected int gridROSPageCount = 1;
        protected int gridDiagnosisPageCount = 1;
        protected int gridLaboratoryPageCount = 1;
        protected int gridImagingPageCount = 1;
        protected int gridDiagnosticPageCount = 1;
        protected int gridInstructionPageCount = 1;
        protected List<vVitalSignDt> lstVitalSignDt = null;
        protected List<vReviewOfSystemDt> lstReviewOfSystemDt = null;
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
            if (Page.Request.QueryString["id"] != null)
            {
                string type = Page.Request.QueryString["id"];
                switch (type)
                {
                    case "ER":
                        return Constant.MenuCode.EMR.EMERGENCY_SOAP_IP_INITIAL_ASSESSMENT_1;
                    case "IP":
                        return Constant.MenuCode.EMR.SOAP_TEMPLATE_INPATIENT_INITIAL_ASSESSMENT_2;
                    default:
                        return Constant.MenuCode.EMR.EMERGENCY_SOAP_IP_INITIAL_ASSESSMENT_1;
                }
            }
            else
            {
                return Constant.MenuCode.EMR.EMERGENCY_SOAP_IP_INITIAL_ASSESSMENT_1;
            }
        }

        protected override void InitializeDataControl()
        {
            Helper.SetControlEntrySetting(cboVisitType, new ControlEntrySetting(true, true, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtHPISummary, new ControlEntrySetting(true, true, false), "mpPatientStatus");

            txtServiceDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtServiceTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            hdnDatePickerToday.Value = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnTimeToday.Value = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            hdnImagingServiceUnitID.Value = AppSession.ImagingServiceUnitID;
            hdnLaboratoryServiceUnitID.Value = AppSession.LaboratoryServiceUnitID;

            SetEntityToControl();

            BindGridViewAllergy(1, true, ref gridAllergyPageCount);
            BindGridViewVitalSign(1, true, ref gridVitalSignPageCount);
            BindGridViewROS(1, true, ref gridROSPageCount);
            BindGridViewDiagnosis(1, true, ref gridDiagnosisPageCount);
            BindGridViewLaboratory(1, true, ref gridLaboratoryPageCount);
            BindGridViewImaging(1, true, ref gridImagingPageCount);
            BindGridViewDiagnostic(1, true, ref gridDiagnosticPageCount);
            BindGridViewInstruction(1, true, ref gridInstructionPageCount);

            LoadBodyDiagram();

            hdnPatientVisitNoteID.Value = _visitNoteID;
            hdnLinkedVisitID.Value = _linkedVisitID;
        }

        private void SetEntityToControl()
        {
            vConsultVisit1 entityVisit = BusinessLayer.GetvConsultVisit1List(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnHealthcareServiceUnitID.Value = entityVisit.HealthcareServiceUnitID.ToString();
            cboVisitType.Value = entityVisit.VisitTypeID.ToString();
            hdnPatientInformation.Value = string.Format("{0} (MRN = {1}, REG = {2}, LOC = {3}, DOB = {4})", entityVisit.cfPatientNameInLabel, entityVisit.MedicalNo, entityVisit.RegistrationNo, entityVisit.ServiceUnitName, entityVisit.DateOfBirthInString);
            hdnDepartmentID.Value = entityVisit.DepartmentID;


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

            chkIsFastTrack.Checked = entityVisit.IsFastTrack;

            vChiefComplaint obj = BusinessLayer.GetvChiefComplaintList(string.Format("VisitID IN ({0},{1}) AND IsInpatientInitialAssessment = 1", AppSession.RegisteredPatient.VisitID, _linkedVisitID)).FirstOrDefault();
            if (obj != null)
            {
                if (obj.ID == 0)
                {
                    hdnChiefComplaintID.Value = "0";
                    hdnParamedicID.Value = "0";
                }
                else
                {
                    hdnChiefComplaintID.Value = obj.ID.ToString();
                    hdnParamedicID.Value = obj.ParamedicID.ToString();
                    txtServiceDate.Text = obj.ObservationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtServiceTime.Text = obj.ObservationTime;
                    txtChiefComplaint.Text = obj.ChiefComplaintText;
                    txtHPISummary.Text = obj.HPISummary;
                    txtMedicalHistory.Text = obj.PastMedicalHistory;
                    txtMedicationHistory.Text = obj.PastMedicationHistory;
                    txtDiagnosticResultSummary.Text = obj.DiagnosticResultSummary;
                    txtFamilyHistory.Text = obj.FamilyHistory;
                    txtNursingObjectives.Text = obj.NursingObjectives;
                    rblIsNeedDischargePlan.SelectedValue = obj.IsNeedDischargePlan ? "1" : "0";
                    txtEstimatedLOS.Text = !string.IsNullOrEmpty(obj.EstimatedLOS.ToString("N0")) ? obj.EstimatedLOS.ToString("N0") : "0";
                    rblEstimatedLOSUnit.SelectedValue = obj.IsEstimatedLOSInDays ? "1" : "0";
                    chkIsPatientAllergyExists.Checked = !chkIsPatientAllergyExists.Checked;
                    chkAlloAnamnesis.Checked = obj.IsAlloAnamnesis;
                    chkAutoAnamnesis.Checked = obj.IsAutoAnamnesis;

                    hdnIsChanged.Value = "0";
                    hdnIsSaved.Value = "0";
                }
            }
            else
            {
                //New Inpatient Chief Complaint
                vChiefComplaint sourceCC = BusinessLayer.GetvChiefComplaintList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
                if (sourceCC != null)
                {
                    txtServiceDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtServiceTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                    txtChiefComplaint.Text = sourceCC.ChiefComplaintText;
                    txtHPISummary.Text = sourceCC.HPISummary;
                    txtMedicalHistory.Text = sourceCC.PastMedicalHistory;
                    txtMedicationHistory.Text = sourceCC.PastMedicationHistory;
                    txtFamilyHistory.Text = sourceCC.FamilyHistory;
                    chkIsPatientAllergyExists.Checked = !chkIsPatientAllergyExists.Checked;
                    chkAlloAnamnesis.Checked = sourceCC.IsAlloAnamnesis;
                    chkAutoAnamnesis.Checked = sourceCC.IsAutoAnamnesis;

                    hdnIsChanged.Value = "1";
                    hdnIsSaved.Value = "0";
                    txtMedicalProblem.Text = sourceCC.MedicalProblem;
                }
                else
                {
                    #region Nurse Anamnesis
                    vNurseChiefComplaint entity = BusinessLayer.GetvNurseChiefComplaintList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();

                    if (entity != null)
                    {
                        txtMedicalHistory.Text = entity.MedicalHistory;
                        txtMedicationHistory.Text = entity.MedicationHistory;

                        chkIsPatientAllergyExists.Checked = entity.IsPatientAllergyExists;
                    }
                    #endregion 
                }
            }

            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);
            MSTAssessment mst = BusinessLayer.GetMSTAssessmentList(filterExpression).FirstOrDefault();
            if (mst != null)
                hdnMSTAssessmentID.Value = mst.ID.ToString();
            else
                hdnMSTAssessmentID.Value = "0";

            PatientVisitNote oVisitNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID IN ({0},{1}) AND GCPatientNoteType = '{2}' AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, _linkedVisitID, Constant.PatientVisitNotes.INPATIENT_INITIAL_ASSESSMENT)).FirstOrDefault();
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
            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}') AND IsActive = 1 AND IsDeleted = 0",
    Constant.StandardCode.ALLERGEN_TYPE, Constant.StandardCode.DIAGNOSIS_TYPE,
    Constant.StandardCode.DIFFERENTIAL_DIAGNOSIS_STATUS, Constant.StandardCode.PATIENT_INSTRUCTION_GROUP,
    Constant.StandardCode.MST_WEIGHT_CHANGED, Constant.StandardCode.MST_WEIGHT_CHANGED_GROUP, Constant.StandardCode.MST_DIAGNOSIS);

            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(filterExpression);

            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField(cboAllergenType, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.ALLERGEN_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboDiagnosisType, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.DIAGNOSIS_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboDiagnosisStatus, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.DIFFERENTIAL_DIAGNOSIS_STATUS).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboInstructionType, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.PATIENT_INSTRUCTION_GROUP).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboGCWeightChangedStatus, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.MST_WEIGHT_CHANGED || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboGCWeightChangedGroup, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.MST_WEIGHT_CHANGED_GROUP || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboGCMSTDiagnosis, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.MST_DIAGNOSIS || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

            string defaultInstruction = lstSc.Where(sc => sc.ParentID == Constant.StandardCode.PATIENT_INSTRUCTION_GROUP && sc.IsDefault == true).FirstOrDefault().StandardCodeID;
            if (!string.IsNullOrEmpty(defaultInstruction))
            {
                cboInstructionType.Value = defaultInstruction;
            }

            cboAllergenType.Value = Constant.AllergenType.DRUG;
            cboDiagnosisStatus.Value = Constant.DifferentialDiagnosisStatus.UNDER_INVESTIGATION;

            List<GetParamedicVisitTypeList> visitTypeList = BusinessLayer.GetParamedicVisitTypeList(AppSession.RegisteredPatient.HealthcareServiceUnitID, (int)AppSession.UserLogin.ParamedicID, "");
            Methods.SetComboBoxField(cboVisitType, visitTypeList, "VisitTypeName", "VisitTypeID");
        }

        private void ControlToEntity(PatientVisitNote entitypvn)
        {
            string soapNote = GenerateSOAPText();

            entitypvn.NoteDate = Helper.GetDatePickerValue(txtServiceDate);
            entitypvn.NoteTime = txtServiceTime.Text;

            entitypvn.SubjectiveText = hdnSubjectiveText.Value;
            entitypvn.ObjectiveText = hdnObjectiveText.Value;
            entitypvn.AssessmentText = hdnAssessmentText.Value;
            entitypvn.PlanningText = hdnPlanningText.Value;
            entitypvn.InstructionText = hdnInstructionText.Value;
            entitypvn.NoteText = soapNote;
        }

        [Obsolete]
        private string GenerateSOAPSummaryText(ref string subjectiveText, ref string objectiveText, ref string assessmentText, ref string planningText)
        {
            StringBuilder sbNotes = new StringBuilder();
            sbNotes.AppendLine("Subjective :");
            sbNotes.AppendLine("-".PadRight(15, '-'));
            if ((AppSession.RegisteredPatient.ParamedicID == AppSession.UserLogin.ParamedicID))
            {
                vChiefComplaint oChiefComplaint = BusinessLayer.GetvChiefComplaintList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
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
            List<vVitalSignDt> lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID = {0} AND ObservationDate = '{1}' AND IsDeleted = 0 ORDER BY DisplayOrder", AppSession.RegisteredPatient.VisitID, Helper.GetDatePickerValue(txtServiceDate).ToString(Constant.FormatString.DATE_FORMAT_112)));
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
            List<vReviewOfSystemDt> lstROS = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID IN (SELECT ID FROM ReviewOfSystemHd WHERE VisitID = {0} AND  ObservationDate = '{1}') AND IsDeleted = 0 ORDER BY GCRoSystem", AppSession.RegisteredPatient.VisitID, Helper.GetDatePickerValue(txtServiceDate).ToString(Constant.FormatString.DATE_FORMAT_112)));
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
                sbNotes.AppendLine(string.Format("Riwayat Pengobatan : "));
                sbNotes.AppendLine(string.Format(" {0}   ", txtMedicationHistory.Text));
            }
            if (!string.IsNullOrEmpty(txtFamilyHistory.Text))
            {
                sbNotes.AppendLine(string.Format("Riwayat Keluarga  : "));
                sbNotes.AppendLine(string.Format(" {0}   ", txtFamilyHistory.Text)); 
            }

            hdnSubjectiveText.Value = sbNotes.ToString();

            sbNotes.AppendLine(" ");
            sbNotes.AppendLine("OBJEKTIF :");
            sbNotes.AppendLine("-".PadRight(20, '-'));
            vVitalSignHd entityVitalSignHd = BusinessLayer.GetvVitalSignHdList(string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
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

            vReviewOfSystemHd entityReviewOfSystem = BusinessLayer.GetvReviewOfSystemHdList(string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
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
            if (!string.IsNullOrEmpty(txtNursingObjectives.Text))
            {
                sbNotes.AppendLine("Sasaran Asuhan :");
                sbNotes.AppendLine(txtNursingObjectives.Text);
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
            ChiefComplaintDao chiefComplaintDao = new ChiefComplaintDao(ctx);
            ConsultVisitDao consultVisitDao = new ConsultVisitDao(ctx);
            MSTAssessmentDao mstDao = new MSTAssessmentDao(ctx);
            RegistrationDao registrationDao = new RegistrationDao(ctx);

            ConsultVisit entityConsultVisit = consultVisitDao.Get(AppSession.RegisteredPatient.VisitID);
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
                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                entity.CreatedBy = AppSession.UserLogin.UserID;
            }
            entity.ObservationDate = Helper.GetDatePickerValue(txtServiceDate);
            entity.ObservationTime = txtServiceTime.Text;
            entity.ChiefComplaintText = txtChiefComplaint.Text;
            entity.HPISummary = txtHPISummary.Text; 
            entity.PastMedicalHistory = txtMedicalHistory.Text;
            entity.PastMedicationHistory = txtMedicationHistory.Text;
            entity.DiagnosticResultSummary = txtDiagnosticResultSummary.Text;
            entity.FamilyHistory = txtFamilyHistory.Text;
            entity.IsPatientAllergyExists = !chkIsPatientAllergyExists.Checked;
            entity.IsInpatientInitialAssessment = true;
            entity.NursingObjectives = txtNursingObjectives.Text;
            entity.IsNeedDischargePlan = rblIsNeedDischargePlan.SelectedValue == "1" ? true : false;
            entity.EstimatedLOS = !string.IsNullOrEmpty(txtEstimatedLOS.Text) ? Convert.ToDecimal(txtEstimatedLOS.Text) : 0;
            entity.IsEstimatedLOSInDays = rblEstimatedLOSUnit.SelectedValue == "1" ? true : false;
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

            Registration oRegistration = registrationDao.Get(entityConsultVisit.RegistrationID);
            if (oRegistration != null)
            {
                oRegistration.IsFastTrack = chkIsFastTrack.Checked;
                registrationDao.Update(oRegistration);
            }

            if (cboGCWeightChangedStatus.Value != null || cboGCWeightChangedStatus.Value != null || cboGCMSTDiagnosis.Value != null)
            {
                string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);
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
                    entityMST.VisitID = AppSession.RegisteredPatient.VisitID;
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
                try
                {

                    PatientVisitNote soapNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = {0} AND GCPatientNoteType = '{1}' AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, Constant.PatientVisitNotes.INPATIENT_INITIAL_ASSESSMENT), ctx).FirstOrDefault();
                    bool isSoapNoteNull = false;
                    if (soapNote == null)
                    {
                        isSoapNoteNull = true;
                        soapNote = new PatientVisitNote();
                    }

                    ControlToEntity(soapNote);

                    if (isSoapNoteNull)
                    {
                        soapNote.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                        soapNote.NoteDate = Helper.GetDatePickerValue(txtServiceDate);
                        soapNote.NoteTime = txtServiceTime.Text;
                        soapNote.VisitID = AppSession.RegisteredPatient.VisitID;
                        soapNote.ParamedicID = AppSession.UserLogin.ParamedicID;
                        soapNote.GCPatientNoteType = Constant.PatientVisitNotes.INPATIENT_INITIAL_ASSESSMENT;
                        soapNote.CreatedBy = AppSession.UserLogin.UserID;
                        hdnPatientVisitNoteID.Value = patientVisitNoteDao.InsertReturnPrimaryKeyID(soapNote).ToString();
                    }
                    else
                    {
                        soapNote.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                        soapNote.ParamedicID = AppSession.UserLogin.ParamedicID;
                        soapNote.LastUpdatedBy = AppSession.UserLogin.UserID;
                        patientVisitNoteDao.Update(soapNote);

                        hdnPatientVisitNoteID.Value = soapNote.ID.ToString();
                    }
                    _visitNoteID = hdnPatientVisitNoteID.Value;

                    ChiefComplaint obj = BusinessLayer.GetChiefComplaintList(string.Format("VisitID = {0} AND IsInpatientInitialAssessment = 1 AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID), ctx).FirstOrDefault();
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
            string visitNoteID = (!string.IsNullOrEmpty(_visitNoteID) && _visitNoteID != "0") ? _visitNoteID : "-1";
            string linkedVisitID = !string.IsNullOrEmpty(_linkedVisitID) ? _linkedVisitID : "0";

            string filterExpression = string.Format("VisitID IN ({0},{1}) AND PatientVisitNoteID = {2} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, linkedVisitID, visitNoteID);

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

        #region Diagnosis
        private void BindGridViewDiagnosis(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0 AND IsNutritionDiagnosis = 0", AppSession.RegisteredPatient.VisitID);

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

                        result = "1|add|";
                    }
                    else if (param[0] == "edit")
                    {
                        int recordID = Convert.ToInt32(hdnDiagnosisID.Value);
                        PatientDiagnosis entity = BusinessLayer.GetPatientDiagnosis(recordID);

                        if (entity != null)
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
                            if (entity.ClaimDiagnosisID != null && entity.ClaimDiagnosisID != "")
                            {
                                result = string.Format("0|delete|{0}", "Data diagnosa pasien ini tidak dapat dihapus karena sudah dilengkapi oleh Casemix.");
                            }
                            else
                            {
                                entity.IsDeleted = true;
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                BusinessLayer.UpdatePatientDiagnosis(entity);

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

        #region Laboratory
        private void BindGridViewLaboratory(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND TransactionCode  = {1} AND GCTransactionStatus != '{2}'", AppSession.RegisteredPatient.VisitID, Constant.TransactionCode.LABORATORY_TEST_ORDER, Constant.TransactionStatus.VOID);

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
                    sbText.AppendLine(string.Format("- {0}",detail.ItemName1));
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
            string filterExpression = string.Format("VisitID = {0} AND TransactionCode = {1} AND GCTransactionStatus != '{2}'", AppSession.RegisteredPatient.VisitID, Constant.TransactionCode.IMAGING_TEST_ORDER, Constant.TransactionStatus.VOID);

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
            string filterExpression = string.Format("VisitID = {0} AND TransactionCode NOT IN ({1},{2}) AND GCTransactionStatus != '{3}'", AppSession.RegisteredPatient.VisitID, Constant.TransactionCode.LABORATORY_TEST_ORDER, Constant.TransactionCode.IMAGING_TEST_ORDER, Constant.TransactionStatus.VOID);

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
                rptDiagnosticDt.DataSource = GetTestOrderDt(obj.TestOrderID);
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
        }
        #endregion

        #region Instruction
        private void BindGridViewInstruction(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string visitNoteID = !string.IsNullOrEmpty(_visitNoteID) ? _visitNoteID : "0";
            string linkedVisitID = !string.IsNullOrEmpty(_linkedVisitID) ? _linkedVisitID : "0";

            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, linkedVisitID, visitNoteID);

            if (visitNoteID != "0")
            {
                filterExpression += string.Format(" AND PatientVisitNoteID = {0}", visitNoteID);
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientInstructionRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientInstruction> lstEntity = BusinessLayer.GetvPatientInstructionList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "PatientInstructionID");

            StringBuilder instructionText = new StringBuilder();
            foreach (vPatientInstruction obj in lstEntity)
            {
                instructionText.AppendLine(string.Format("- {0}", obj.Description));
            }
            hdnInstructionText.Value = instructionText.ToString();

            grdInstructionView.DataSource = lstEntity;
            grdInstructionView.DataBind();
        }

        protected void cbpInstructionView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewInstruction(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewInstruction(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = "";
        }
        protected void cbpInstruction_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|add|";

            try
            {
                if (e.Parameter != null && e.Parameter != "")
                {
                    string[] param = e.Parameter.Split('|');
                    if (param[0] == "add")
                    {
                        if (_visitNoteID == "0" || _visitNoteID == "")
                        {
                            PatientVisitNote soapNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = {0} AND GCPatientNoteType = '{1}' AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, Constant.PatientVisitNotes.INPATIENT_INITIAL_ASSESSMENT)).FirstOrDefault();
                            if (soapNote != null)
                            {
                                _visitNoteID = soapNote.ID.ToString();
                                hdnPatientVisitNoteID.Value = soapNote.ID.ToString();
                            }
                        }
                        else
                        {
                            hdnPatientVisitNoteID.Value = _visitNoteID;
                        }

                        PatientInstruction entity = new PatientInstruction();

                        entity.VisitID = AppSession.RegisteredPatient.VisitID;
                        entity.InstructionDate = DateTime.Now.Date;
                        entity.InstructionTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

                        entity.PhysicianID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                        entity.GCInstructionGroup = cboInstructionType.Value.ToString();
                        entity.Description = txtInstructionText.Text;

                        if (!string.IsNullOrEmpty(hdnPatientVisitNoteID.Value) || hdnPatientVisitNoteID.Value != "0")
                        {
                            entity.PatientVisitNoteID = Convert.ToInt32(hdnPatientVisitNoteID.Value);
                        }

                        BusinessLayer.InsertPatientInstruction(entity);

                        result = "1|add|";
                    }
                    else if (param[0] == "edit")
                    {
                        int recordID = Convert.ToInt32(hdnInstructionID.Value);
                        PatientInstruction entity = BusinessLayer.GetPatientInstruction(recordID);

                        if (entity != null)
                        {
                            entity.GCInstructionGroup = cboInstructionType.Value.ToString();
                            entity.Description = txtInstructionText.Text;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdatePatientInstruction(entity);
                            result = "1|edit|";
                        }
                        else
                        {
                            result = string.Format("0|delete|{0}", "Invalid Patient Instruction Record Information");
                        }
                    }
                    else
                    {
                        int recordID = Convert.ToInt32(hdnInstructionID.Value);
                        PatientInstruction entity = BusinessLayer.GetPatientInstruction(recordID);

                        if (entity != null)
                        {
                            //TODO : Prompt user for delete reason
                            entity.IsDeleted = true;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdatePatientInstruction(entity);
                            result = "1|delete|";
                        }
                        else
                        {
                            result = string.Format("0|edit|{0}", "Invalid Patient Instruction Record Information");
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

        private bool IsValidToSave(ref string errMessage)
        {
            bool result = true;
            if (hdnParamedicID.Value != "" && hdnParamedicID.Value != "0")
            {
                int paramedicID = Convert.ToInt32(hdnParamedicID.Value);
                if (AppSession.UserLogin.ParamedicID != paramedicID)
                {
                    errMessage = "Perubahan Kajian Awal Pasien hanya dapat dilakukan oleh Dokter yang melakukan Pengkajian";
                    result = false;
                }
            }
            return result;
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

                    PatientVisitNote soapNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = {0} AND GCPatientNoteType = '{1}' AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, Constant.PatientVisitNotes.INPATIENT_INITIAL_ASSESSMENT), ctx).FirstOrDefault();
                    bool isSoapNoteNull = false;
                    if (soapNote == null)
                    {
                        isSoapNoteNull = true;
                        soapNote = new PatientVisitNote();
                    }

                    #region Summarize Instruction Text
                    string filterExpression = string.Format("VisitID IN ({0}) AND PatientVisitNoteID = {1} AND IsDeleted = 0 ORDER BY PatientInstructionID", AppSession.RegisteredPatient.VisitID, _visitNoteID);
                    List<PatientInstruction> lstEntity = BusinessLayer.GetPatientInstructionList(filterExpression, ctx);

                    StringBuilder instructionText = new StringBuilder();
                    foreach (PatientInstruction obj in lstEntity)
                    {
                        instructionText.AppendLine(string.Format("- {0}", obj.Description));
                    }

                    hdnInstructionText.Value = instructionText.ToString();
                    #endregion

                    ControlToEntity(soapNote);

                    if (isSoapNoteNull)
                    {
                        soapNote.NoteDate = Helper.GetDatePickerValue(txtServiceDate);
                        soapNote.NoteTime = txtServiceTime.Text;
                        soapNote.VisitID = AppSession.RegisteredPatient.VisitID;
                        soapNote.ParamedicID = AppSession.UserLogin.ParamedicID;
                        soapNote.GCPatientNoteType = Constant.PatientVisitNotes.INPATIENT_INITIAL_ASSESSMENT;
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
    }
}

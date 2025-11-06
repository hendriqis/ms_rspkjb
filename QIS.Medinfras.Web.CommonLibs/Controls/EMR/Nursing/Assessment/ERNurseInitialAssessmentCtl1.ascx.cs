using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using System.Text;
using System.IO;
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class ERNurseInitialAssessmentCtl1 : BaseViewPopupCtl
    {
        protected int gridAllergyPageCount = 1;
        protected int gridDiagnosisPageCount = 1;
        protected int gridNursingProblemPageCount = 1;
        protected int gridReviewOfSystemPageCount = 1;
        protected int gridVitalSignPageCount = 1;
        protected int gridPatientEducationPageCount = 1;
        private List<vVitalSignDt> lstVitalSignDt = null;
        private List<vReviewOfSystemDt> lstReviewOfSystemDt = null;
        private List<vPatientEducationDt> lstPatientEducationDt = null;
        protected int VisitID = 0;
        protected int _assessmentID = 0;

        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            if (paramInfo[0] != "")
                VisitID = Convert.ToInt32(paramInfo[0]);
            else
                VisitID = AppSession.RegisteredPatient.VisitID;

            #region Patient Information
            vConsultVisit9 registeredPatient = BusinessLayer.GetvConsultVisit9List(string.Format("VisitID = {0}", VisitID))[0];
            lblPatientName.InnerHtml = registeredPatient.cfPatientNameSalutation;
            lblGender.InnerHtml = registeredPatient.Gender;
            lblDateOfBirth.InnerHtml = string.Format("{0} ({1})", registeredPatient.DateOfBirthInString, Helper.GetPatientAge(words, registeredPatient.DateOfBirth));

            lblRegistrationDateTime.InnerHtml = string.Format("{0} / {1}", registeredPatient.VisitDateInString, registeredPatient.VisitTime);
            lblRegistrationNo.InnerHtml = registeredPatient.RegistrationNo;
            lblPhysician.InnerHtml = registeredPatient.ParamedicName;

            lblMedicalNo.InnerHtml = registeredPatient.MedicalNo;

            lblPayerInformation.InnerHtml = registeredPatient.BusinessPartnerName;
            lblPatientLocation.InnerHtml = registeredPatient.cfPatientLocation;
            imgPatientImage.Src = registeredPatient.PatientImageUrl; 
            #endregion

            string filterExpCC = string.Format("VisitID = {0} AND IsDeleted = 0 ORDER BY ChiefComplaintID DESC", VisitID);
            if (_assessmentID != 0)
            {
                filterExpCC = string.Format("VisitID = {0} AND ChiefComplaintID = {1} AND IsDeleted = 0 ORDER BY ChiefComplaintID DESC", VisitID, _assessmentID);
            }
            vNurseChiefComplaint oChiefComplaint = BusinessLayer.GetvNurseChiefComplaintList(filterExpCC).FirstOrDefault();
            lblPhysicianName2.InnerHtml = registeredPatient.ParamedicName;

            VisitType vt = BusinessLayer.GetVisitType(registeredPatient.VisitTypeID);

            #region StandardCode
            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}') AND IsActive = 1 AND IsDeleted = 0",
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
                Constant.StandardCode.FAMILY_RELATION
                );
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(filterExpression);
            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            //Methods.SetComboBoxField<StandardCode>(cboOnset, lstSc.Where(p => p.ParentID == Constant.StandardCode.ONSET || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            //Methods.SetComboBoxField<StandardCode>(cboProvocation, lstSc.Where(p => p.ParentID == Constant.StandardCode.EXACERBATED || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            //Methods.SetComboBoxField<StandardCode>(cboQuality, lstSc.Where(p => p.ParentID == Constant.StandardCode.QUALITY || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            //Methods.SetComboBoxField<StandardCode>(cboSeverity, lstSc.Where(p => p.ParentID == Constant.StandardCode.SEVERITY || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            //Methods.SetComboBoxField<StandardCode>(cboTime, lstSc.Where(p => p.ParentID == Constant.StandardCode.COURSE_TIMING || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            //Methods.SetComboBoxField<StandardCode>(cboRelievedBy, lstSc.Where(p => p.ParentID == Constant.StandardCode.RELIEVED_BY || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
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
            ////Methods.SetComboBoxField<StandardCode>(cboPatientEducationType, lstSc.Where(p => p.ParentID == Constant.StandardCode.PATIENT_EDUCATION_TYPE || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboRAPUH_R, lstSc.Where(p => p.ParentID == Constant.StandardCode.RAPUH_RESISTENSI || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboRAPUH_A, lstSc.Where(p => p.ParentID == Constant.StandardCode.RAPUH_AKTIFITAS || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboRAPUH_P, lstSc.Where(p => p.ParentID == Constant.StandardCode.RAPUH_PENYAKIT || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboRAPUH_U, lstSc.Where(p => p.ParentID == Constant.StandardCode.RAPUH_USAHA_BERJALAN || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboRAPUH_H, lstSc.Where(p => p.ParentID == Constant.StandardCode.RAPUH_BERAT_BADAN || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboRAPUHScore, lstSc.Where(p => p.ParentID == Constant.StandardCode.RAPUH_SCORE || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboFamilyRelation, lstSc.Where(p => p.ParentID == Constant.StandardCode.FAMILY_RELATION || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            #endregion

            if (oChiefComplaint != null)
            {
                lblAssessmentParamedicName.InnerHtml = oChiefComplaint.ParamedicName;
                lblPrimaryNurseName.InnerHtml = oChiefComplaint.PrimaryNurseName;
                hdnMRN.Value = oChiefComplaint.MRN.ToString();

                #region Chief Complaint and History Of Illness
                txtDate.Text = oChiefComplaint.ChiefComplaintDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtTime.Text = oChiefComplaint.ChiefComplaintTime;
                txtPhysicianName.Text = oChiefComplaint.ParamedicName;
                txtChiefComplaint.Text = oChiefComplaint.NurseChiefComplaintText;
                txtHPISummary.Text = oChiefComplaint.HPISummary;
                chkAutoAnamnesis.Checked = oChiefComplaint.IsAutoAnamnesis;
                chkAlloAnamnesis.Checked = oChiefComplaint.IsAlloAnamnesis;
                if (!string.IsNullOrEmpty(oChiefComplaint.GCFamilyRelation))
                {
                    cboFamilyRelation.Value = oChiefComplaint.GCFamilyRelation;
                }
                chkIsPatientAllergyExists.Checked = !oChiefComplaint.IsPatientAllergyExists;
                cboAirway.Value = oChiefComplaint.GCAirway;
                cboBreathing.Value = oChiefComplaint.GCBreathing;
                cboCirculation.Value = oChiefComplaint.GCCirculation;
                cboDisability.Value = oChiefComplaint.GCDisability;
                cboExposure.Value = oChiefComplaint.GCExposure;
                txtMedicalHistory.Text = oChiefComplaint.MedicalHistory;
                txtMedicationHistory.Text = oChiefComplaint.MedicationHistory;
                txtVisitTypeCode.Text = vt.VisitTypeCode;
                txtVisitTypeName.Text = vt.VisitTypeName;
                cboVisitReason.Value = oChiefComplaint.GCVisitReason;
                txtVisitNotes.Text = oChiefComplaint.VisitReason;
                txtEmergencyCaseDate.Text = oChiefComplaint.EmergencyCaseDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtEmergencyCaseTime.Text = oChiefComplaint.EmergencyCaseTime;
                txtServiceDate.Text = oChiefComplaint.StartServiceDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtServiceTime.Text = oChiefComplaint.StartServiceTime;
                txtRegistrationDate.Text = oChiefComplaint.ActualVisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtRegistrationTime.Text = oChiefComplaint.ActualVisitTime;
                cboAdmissionRoute.Value = oChiefComplaint.GCAdmissionRoute;
                cboTriage.Value = oChiefComplaint.GCTriage;
                txtTriageDate.Text = oChiefComplaint.TriageDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtTriageTime.Text = oChiefComplaint.TriageTime;
                txtTriageByParamedicName.Text = oChiefComplaint.TriageByParamedicName;
                cboAdmissionCondition.Value = oChiefComplaint.GCAdmissionCondition;
                txtEmergencyCase.Text = oChiefComplaint.EmergencyCaseLocation;
                cboFunctionalType.Value = oChiefComplaint.GCFunctionalType;
                txtFunctionalTypeRemarks.Text = oChiefComplaint.FunctionalTypeRemarks;
                rblFamilyRelationship.SelectedValue = oChiefComplaint.IsHasGoodFamilyRelationship ? "1" : "0";
                txtFamilyRelationshipRemarks.Text = oChiefComplaint.FamilyRelationshipRemarks;
                rblIsNeedAdditionalPrivacy.SelectedValue = oChiefComplaint.IsNeedAdditionalPrivacy ? "1" : "0";
                txtNeedAdditionalPrivacyRemarks.Text = oChiefComplaint.NeedAdditionalPrivacyRemarks;
                cboPsychologyStatus.Value = oChiefComplaint.GCPsychologyStatus;
                txtCommitSuicideRemarks.Text = oChiefComplaint.ReportToPotentiallyCommitSuicide;
                rblHasFinancialProblem.SelectedValue = oChiefComplaint.IsHasFinancialProblem ? "1" : "0";
                txtFinancialProblemRemarks.Text = oChiefComplaint.FinancialProblemRemarks;
                chkIsHasRAPUHAssessment.Checked = oChiefComplaint.IsHasRAPUHAssessment;
                cboRAPUH_R.Value = oChiefComplaint.GCRAPUH_R;
                cboRAPUH_A.Value = oChiefComplaint.GCRAPUH_A;
                cboRAPUH_P.Value = oChiefComplaint.GCRAPUH_P;
                cboRAPUH_U.Value = oChiefComplaint.GCRAPUH_U;
                cboRAPUH_H.Value = oChiefComplaint.GCRAPUH_H;
                txtRAPUHScore.Text = oChiefComplaint.RAPUHScore.ToString("G29");
                cboRAPUHScore.Value = oChiefComplaint.GCRAPUHScore;
                List<PatientVisitNote> lstPatientVisitNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = '{0}' AND GCPatientNoteType = '{1}'", VisitID, Constant.PatientVisitNotes.NURSE_INITIAL_ASSESSMENT));
                if (lstPatientVisitNote.Count > 0)
                {
                    PatientVisitNote entitypvn = lstPatientVisitNote.First();
                    if (string.IsNullOrEmpty(txtPlanningNotes.Text))
                    {
                        txtPlanningNotes.Text = entitypvn.PlanningText;
                        txtInstructionText.Text = entitypvn.InstructionText;
                    }
                }
                chkIsNeedPatientEducation.Checked = oChiefComplaint.IsNeedPatientEducation;
                chkIsNeedAcuteInitialAssessment.Checked = oChiefComplaint.IsNeedAccuteInitialAssessment;
                chkIsNeedChronicInitialAssessment.Checked = oChiefComplaint.IsNeedChronicInitialAssessment;
                txtDiagnose.Text = oChiefComplaint.DiagnosisText;
                #endregion

                #region HTML Form
                hdnPhysicalExamLayout.Value = oChiefComplaint.PhysicalExamLayout;
                hdnPhysicalExamValue.Value = oChiefComplaint.PhysicalExamValues;

                hdnSocialHistoryLayout.Value = oChiefComplaint.SocialHistoryLayout;
                hdnSocialHistoryValue.Value = oChiefComplaint.SocialHistoryValues;

                hdnEducationLayout.Value = oChiefComplaint.EducationLayout;
                hdnEducationValue.Value = oChiefComplaint.EducationValues;

                hdnDischargePlanningLayout.Value = oChiefComplaint.DischargePlanningLayout;
                hdnDischargePlanningValue.Value = oChiefComplaint.DischargePlanningValues;

                hdnAdditionalAssessmentLayout.Value = oChiefComplaint.AdditionalAssessmentLayout;
                hdnAdditionalAssessmentValue.Value = oChiefComplaint.AdditionalAssessmentValues; 
                #endregion

                BindGridViewAllergy(1, true, ref gridAllergyPageCount);
                BindGridViewDiagnosis(1, true, ref gridDiagnosisPageCount);
                BindGridViewNursingProblem(1, true, ref gridNursingProblemPageCount);
                BindGridViewReviewOfSystem(1, true, ref gridReviewOfSystemPageCount);
                BindGridViewVitalSign(1, true, ref gridVitalSignPageCount);
                BindGridViewPatientEducation(1, true, ref gridPatientEducationPageCount);

                LoadBodyDiagram();
            }
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

            chkIsPatientAllergyExists2.Checked = !(lstEntity.Count > 0);
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
        #endregion

        #region Diagnosis
        private void BindGridViewDiagnosis(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", VisitID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientDiagnosisRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientDiagnosis> lstEntity = BusinessLayer.GetvPatientDiagnosisList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "GCDiagnoseType");

            grdDiagnosisView.DataSource = lstEntity;
            grdDiagnosisView.DataBind();

            StringBuilder diagnosis = new StringBuilder();
            foreach (vPatientDiagnosis patientDiagnosis in lstEntity)
            {
                if (patientDiagnosis.GCDiagnoseType == Constant.DiagnoseType.MAIN_DIAGNOSIS)
                    diagnosis.AppendLine(string.Format("{0} ({1})", patientDiagnosis.DiagnosisText, patientDiagnosis.DiagnoseType));
                else
                    diagnosis.AppendLine(string.Format("{0}", patientDiagnosis.DiagnosisText));
            }

            lblDiagnosis.InnerHtml = diagnosis.ToString();
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
        #endregion

        #region Masalah Keperawatan
        private void BindGridViewNursingProblem(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", VisitID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvNursingPatientProblemRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vNursingPatientProblem> lstEntity = BusinessLayer.GetvNursingPatientProblemList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
            grdNursingProblemView.DataSource = lstEntity;
            grdNursingProblemView.DataBind();
        }

        protected void cbpNursingProblemView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewNursingProblem(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewNursingProblem(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        #region Review Of System
        private void BindGridViewReviewOfSystem(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Empty;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID IN ({0}) AND GCParamedicMasterType != '{1}' AND IsDeleted = 0 ORDER BY ID DESC", VisitID, Constant.ParamedicType.Physician);

            List<vReviewOfSystemHd> lstEntity = BusinessLayer.GetvReviewOfSystemHdList(filterExpression);
            lstReviewOfSystemDt = BusinessLayer.GetvReviewOfSystemDtList(filterExpression);
            grdReviewOfSystemView.DataSource = lstEntity;
            grdReviewOfSystemView.DataBind();
        }

        protected void grdReviewOfSystemView_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void cbpReviewOfSystemView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewReviewOfSystem(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewReviewOfSystem(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }
        }
        #endregion

        #region Vital Sign
        private void BindGridViewVitalSign(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Empty;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID IN ({0}) AND IsInitialAssessment = 1 AND IsDeleted = 0 AND GCParamedicMasterType != '{1}' ORDER BY ID DESC", VisitID, Constant.ParamedicType.Physician);

            List<vVitalSignHd> lstEntity = BusinessLayer.GetvVitalSignHdList(filterExpression);
            lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID IN ({0}) AND IsInitialAssessment = 1 AND GCParamedicMasterType != '{1}' ORDER BY DisplayOrder", VisitID, Constant.ParamedicType.Physician));
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

        private void LoadBodyDiagram()
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", VisitID);
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

        protected void OnLoadBodyDiagram(int PageIndex)
        {
            string filterExpression = "";
            filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", VisitID);
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
        #endregion

        #region Patient Education
        private void BindGridViewPatientEducation(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Empty;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID IN ({0}) AND IsDeleted = 0 ORDER BY ID DESC", VisitID);

            List<vPatientEducationHd> lstEntity = BusinessLayer.GetvPatientEducationHdList(filterExpression);
            lstPatientEducationDt = BusinessLayer.GetvPatientEducationDtList(string.Format("VisitID IN ({0})", VisitID));
            grdPatientEducationView.DataSource = lstEntity;
            grdPatientEducationView.DataBind();
        }

        protected void grdPatientEducationView_RowDataBound(object sender, GridViewRowEventArgs e)
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
            return lstPatientEducationDt.Where(p => p.ID == ID).ToList();
        }

        protected void cbpPatientEducationView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewPatientEducation(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewPatientEducation(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }
        }
        #endregion
    }
}
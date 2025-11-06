using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class MedicalSummaryCtl : BaseViewPopupCtl
    {
        protected int gridAllergyPageCount = 1;
        protected int gridDiagnosisPageCount = 1;
        protected int gridDiagnosisParamedicPageCount = 1;
        protected int gridNursingProblemPageCount = 1;
        protected int gridReviewOfSystemPageCount = 1;
        protected int gridVitalSignPageCount = 1;
        protected int gridPatientEducationPageCount = 1;
        protected int gridPatientNursingJournalPageCount = 1;
        protected int gridROSPageCount = 1;
        protected int gridFallRiskAssessmentPageCount = 1;
        protected int gridPainAssessmentPageCount = 1;
        protected int Content4PageCount = 1;
        protected int PageCountIntakeOutput = 1;
        protected int PageCount2IntakeOutput = 1;
        protected int PageCountPatientTransferList = 1;
        protected int PageCountPatientReferralList = 1;
        protected int PageCountFallRiskAssessment = 1;
        protected int PageCountPainAssessment = 1;
        protected int PageCountSurgeryHistory = 1;
        protected int PageCountLaboratory = 1;
        protected int PageCountLaboratory2 = 1;
        protected int PageCountImaging = 1;
        protected int PageCountDrug = 1;

        private List<vVitalSignDt> lstVitalSignDt = null;
        private List<vReviewOfSystemDt> lstReviewOfSystemDt = null;

        protected int _visitNoteID = 0;

        class LaboratoryDtInfo
        {
            public int ChargesID { get; set; }
            public int ItemID { get; set; }
            public string FractionName { get; set; }
            public string ResultValue { get; set; }
            public string ResultUnit { get; set; }
            public string RefRange { get; set; }
            public string ResultFlag { get; set; }
            public string TextNormalValue { get; set; }
        }

        public override void InitializeDataControl(string param)
        {
            if (param != "")
                hdnVisitIDPopUpCtl.Value = param;
            else
                hdnVisitIDPopUpCtl.Value = hdnVisitIDPopUpCtl.Value.ToString();

            List<SettingParameter> setvar = BusinessLayer.GetSettingParameterList(String.Format("ParameterCode IN ('{0}')",
                Constant.SettingParameter.RM_CETAK_HASIL_LAB_DI_RINGKASAN_PERAWATAN
                ));

            hdnIsUsingPrintLBResult.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.RM_CETAK_HASIL_LAB_DI_RINGKASAN_PERAWATAN).FirstOrDefault().ParameterValue;

            Healthcare oHc = BusinessLayer.GetHealthcare(AppSession.UserLogin.HealthcareID);
            if (oHc != null)
            {
                if (oHc.Initial == "RSBL")
                {
                    hdnRptCodeSurgery.Value = "PM-00673";
                    hdnReportCodeRadResult.Value = "IS-00021";
                    hdnReportCodeLabResult.Value = "LB-00032";
                    hdnRptCodePatientRefferal.Value = "PM-00671";
                    hdnRptCodePatientRefferalAnswer.Value = "PM-00672";
                }
                else if (oHc.Initial == "RSSA")
                {
                    hdnRptCodeSurgery.Value = "PM-00731";
                    hdnReportCodeRadResult.Value = "IS-00021";
                    hdnReportCodeLabResult.Value = "LB-00032";
                    hdnRptCodePatientRefferal.Value = "PM-00671";
                    hdnRptCodePatientRefferalAnswer.Value = "PM-00672";
                }
                else if (oHc.Initial == "RSSEB" || oHc.Initial == "RSSEBK" || oHc.Initial == "RSSEBS" || oHc.Initial == "RSDI")
                {
                    hdnRptCodeSurgery.Value = "PM-00588";
                    hdnReportCodeRadResult.Value = "IS-00001";
                    hdnReportCodeLabResult.Value = "LB-00036";
                    hdnRptCodePatientRefferal.Value = "PM-00671";
                    hdnRptCodePatientRefferalAnswer.Value = "PM-00672";
                }
                else if (oHc.Initial == "RSPW")
                {
                    hdnRptCodePatientRefferal.Value = "PM-00691";
                    hdnRptCodePatientRefferalAnswer.Value = "PM-00692";
                    hdnReportCodeLabResult.Value = "LB-00001";
                    hdnReportCodeRadResult.Value = "IS-00001";
                    hdnRptCodeSurgery.Value = "PM-00567";
                }
                else if (oHc.Initial == "RSSES")
                {
                    hdnRptCodeSurgery.Value = "PM-005671";
                    hdnRptCodePatientRefferal.Value = "PM-00146";
                    hdnRptCodePatientRefferalAnswer.Value = "PM-00147";
                    hdnReportCodeRadResult.Value = "IS-00001";
                }
                else if (oHc.Initial == "RSSK")
                {
                    hdnRptCodeSurgery.Value = "PM-90086";
                    hdnRptCodePatientRefferal.Value = "PM-00146";
                    hdnRptCodePatientRefferalAnswer.Value = "PM-00147";
                    hdnReportCodeRadResult.Value = "IS-00001";
                }
                else
                {
                    hdnRptCodeSurgery.Value = "PM-00567";
                    hdnRptCodePatientRefferal.Value = "PM-00146";
                    hdnRptCodePatientRefferalAnswer.Value = "PM-00147";
                    hdnReportCodeLabResult.Value = "LB-00001";
                    hdnReportCodeRadResult.Value = "IS-00001";
                }
            }

            GetMSConsultVisit registeredPatient = BusinessLayer.GetMSConsultVisitList(Convert.ToInt32(hdnVisitIDPopUpCtl.Value))[0];
            hdnRegistrationID.Value = registeredPatient.RegistrationID.ToString();
            hdnLinkedRegistrationID.Value = registeredPatient.LinkedRegistrationID.ToString();
            lblMedicalNoTitle.InnerHtml = registeredPatient.MedicalNo;
            hdnMedicalNo.Value = registeredPatient.MedicalNo;

            LoadContentInformation1(registeredPatient);
            LoadContentInformation2(registeredPatient);
            LoadContentInformation3(registeredPatient);
            LoadContentInformation4(registeredPatient);
            LoadContentInformation5(registeredPatient);
            LoadContentInformation6(registeredPatient);
            LoadContentInformation7(registeredPatient);
            LoadContentInformation8(registeredPatient);
            LoadContentInformation9(registeredPatient);
            LoadContentInformation10_11_12(registeredPatient);
            LoadContentInformation13(registeredPatient);
            LoadContentInformation14(registeredPatient);
        }

        private void LoadContentInformation1(GetMSConsultVisit registeredPatient)
        {
            lblPatientName.InnerHtml = registeredPatient.cfPatientNameSalutation;
            lblGender.InnerHtml = registeredPatient.Gender;
            lblDateOfBirth.InnerHtml = string.Format("{0} ({1})", registeredPatient.DateOfBirthInString, Helper.GetPatientAge(words, registeredPatient.DateOfBirth));

            lblRegistrationDateTime.InnerHtml = string.Format("{0} / {1}", registeredPatient.VisitDateInString, registeredPatient.VisitTime);
            lblRegistrationNo.InnerHtml = registeredPatient.RegistrationNo;
            lblPhysician.InnerHtml = registeredPatient.ParamedicName;

            lblMedicalNo.InnerHtml = registeredPatient.MedicalNo;

            List<vPatientDiagnosis> lstPatientDiagnosis = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", registeredPatient.VisitID));
            StringBuilder diagnosis = new StringBuilder();
            foreach (vPatientDiagnosis patientDiagnosis in lstPatientDiagnosis)
            {
                if (patientDiagnosis.GCDiagnoseType == Constant.DiagnoseType.MAIN_DIAGNOSIS)
                    diagnosis.AppendLine(string.Format("{0} ({1})", patientDiagnosis.DiagnosisText, patientDiagnosis.DiagnoseType));
                else
                    diagnosis.AppendLine(string.Format("{0}", patientDiagnosis.DiagnosisText));
            }

            lblPayerInformation.InnerHtml = registeredPatient.BusinessPartnerName;
            lblPatientLocation.InnerHtml = registeredPatient.cfPatientLocation;
            lblDiagnosis.InnerHtml = diagnosis.ToString();
            imgPatientImage.Src = registeredPatient.PatientImageUrl;
        }

        private void LoadContentInformation2(GetMSConsultVisit registeredPatient)
        {
            GetMSChiefComplaint obj = BusinessLayer.GetMSChiefComplaintList(registeredPatient.VisitID).LastOrDefault();
            if (obj != null)
            {
                txtServiceDate.Text = obj.ObservationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtServiceTime.Text = obj.ObservationTime;
                txtChiefComplaint.Text = obj.ChiefComplaintText;
                txtHPISummary.Text = obj.HPISummary;
                txtDiagnosticResultSummary.Text = obj.DiagnosticResultSummary;
                txtFamilyHistory.Text = obj.FamilyHistory;
                txtNursingObjectives.Text = obj.NursingObjectives;
                rblIsNeedDischargePlan.SelectedValue = obj.IsNeedDischargePlan ? "1" : "0";
                txtEstimatedLOS.Text = !string.IsNullOrEmpty(obj.EstimatedLOS.ToString("N0")) ? obj.EstimatedLOS.ToString("N0") : "0";
                rblEstimatedLOSUnit.SelectedValue = obj.IsEstimatedLOSInDays ? "1" : "0";
                chkAlloAnamnesis.Checked = obj.IsAlloAnamnesis;
                chkAutoAnamnesis.Checked = obj.IsAutoAnamnesis;
                txtVisitTypeName.Text = obj.VisitTypeName;
            }

            vPastMedical pastMedical = BusinessLayer.GetvPastMedicalList(string.Format("RegistrationID = {0}", registeredPatient.RegistrationID)).LastOrDefault();
            if (pastMedical != null)
            {
                txtMedicalHistory.Text = pastMedical.MedicalSummary;
                txtMedicationHistory.Text = pastMedical.MedicationSummary;
            }
            else
            {
                if (obj != null)
                {
                    txtMedicalHistory.Text = obj.PastMedicalHistory;
                    txtMedicationHistory.Text = obj.PastMedicationHistory;
                }
                else
                {
                    txtMedicalHistory.Text = "";
                    txtMedicationHistory.Text = "";
                }
            }

            PatientVisitNote oVisitNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = {0} AND GCPatientNoteType = '{1}' AND IsDeleted = 0", registeredPatient.VisitID, Constant.PatientVisitNotes.INPATIENT_INITIAL_ASSESSMENT)).FirstOrDefault();
            if (oVisitNote != null)
            {
                _visitNoteID = oVisitNote.ID;
                txtInstructionText.Text = oVisitNote.InstructionText;
            }

            GetMSMSTAssessment mst = BusinessLayer.GetMSMSTAssessmentList(registeredPatient.VisitID).FirstOrDefault();
            if (mst != null)
            {
                txtGCWeightChangedStatus.Text = mst.WeightChangedStatus;
                txtGCWeightChangedGroup.Text = mst.WeightChangedGroup;
                txtGCMSTDiagnosis.Text = mst.MSTDiagnosis;

                if (mst.GCFoodIntakeChanged != null)
                    rblIsFoodIntakeChanged.SelectedValue = mst.GCFoodIntakeChanged == "X450^01" ? "1" : "0";
                else
                    rblIsFoodIntakeChanged.SelectedValue = "0";
                txtFoodIntakeScore.Text = rblIsFoodIntakeChanged.SelectedValue;

                txtOtherMSTDiagnosis.Text = mst.OtherMSTDiagnosis;
                txtTotalMST.Text = mst.MSTScore.ToString();

                int a = 0;
                int b = 0;
                int c = 0;

                if (!String.IsNullOrEmpty(mst.GCWeightChangedStatus))
                {
                    string[] WeightChangedStatus = mst.GCWeightChangedStatus.Split('^');
                    if (WeightChangedStatus[1] == "01")
                    {
                        txtWeightChangedStatusScore.Text = "0";
                    }
                    else if (WeightChangedStatus[1] == "02")
                    {
                        txtWeightChangedStatusScore.Text = "2";
                        a = 2;
                    }
                    else
                    {
                        txtWeightChangedStatusScore.Text = "";
                    }
                }

                if (!String.IsNullOrEmpty(mst.GCWeightChangedGroup))
                {
                    string[] WeightChangedGroup = mst.GCWeightChangedGroup.Split('^');
                    txtWeightChangedGroupScore.Text = Convert.ToInt32(WeightChangedGroup[1]).ToString();

                    if (!String.IsNullOrEmpty(txtWeightChangedGroupScore.Text))
                    {
                        {
                            b = Convert.ToInt32(WeightChangedGroup[1]);
                        }
                    }
                }
                if (txtFoodIntakeScore.Text == "1")
                {
                    c = 1;
                }
                txtTotalMST.Text = (a + b + c).ToString();

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
            }
            else
            {
                txtGCWeightChangedStatus.Text = "";
                txtGCWeightChangedGroup.Text = "";
                txtGCMSTDiagnosis.Text = "";
                rblIsFoodIntakeChanged.SelectedValue = null;
                txtOtherMSTDiagnosis.Text = string.Empty;
                txtFoodIntakeScore.Text = "0";
                txtTotalMST.Text = "0";
                txtWeightChangedStatusScore.Text = "";
                txtWeightChangedGroupScore.Text = "";
                txtFoodIntakeScore.Text = "";
                txtTotalMST.Text = "";
            }

            int visitID = registeredPatient.VisitID;
            BindGridViewROS(1, true, ref gridROSPageCount, ref visitID);
        }

        private void LoadContentInformation3(GetMSConsultVisit registeredPatient)
        {
            GetMSNurseChiefComplaint oChiefComplaint = BusinessLayer.GetMSNurseChiefComplaintList(registeredPatient.VisitID).FirstOrDefault();
            hdnMRN.Value = registeredPatient.MRN.ToString();

            if (oChiefComplaint != null)
            {
                #region Chief Complaint and History Of Illness
                txtDate.Text = oChiefComplaint.ChiefComplaintDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtTime.Text = oChiefComplaint.ChiefComplaintTime;
                txtPhysicianName.Text = oChiefComplaint.ParamedicName;
                txtChiefComplaintNurse.Text = oChiefComplaint.NurseChiefComplaintText;
                txtFamilyHistoryNurse.Text = oChiefComplaint.FamilyHistory;

                txtHPISummaryNurse.Text = oChiefComplaint.HPISummary;
                chkAutoAnamnesisNurse.Checked = oChiefComplaint.IsAutoAnamnesis;
                chkAlloAnamnesisNurse.Checked = oChiefComplaint.IsAlloAnamnesis;

                txtMedicalHistoryNurse.Text = oChiefComplaint.MedicalHistory;
                txtMedicationHistoryNurse.Text = oChiefComplaint.MedicationHistory;
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

                hdnSocialHistoryLayout.Value = oChiefComplaint.SocialHistoryLayout;
                hdnSocialHistoryValue.Value = oChiefComplaint.SocialHistoryValues;
                #endregion
            }

            int visitID = registeredPatient.VisitID;

            BindGridViewAllergy(1, true, ref gridAllergyPageCount);
            BindGridViewDiagnosis(1, true, ref gridDiagnosisPageCount, ref visitID);
            BindGridViewDiagnosisParamedic(1, true, ref gridDiagnosisParamedicPageCount, ref visitID);
            BindGridViewVitalSign(1, true, ref gridVitalSignPageCount, ref visitID);
            BindGridViewFallRiskAssessmentHeader(1, true, ref gridFallRiskAssessmentPageCount);
            BindGridViewPainAssessmentHeader(1, true, ref gridPainAssessmentPageCount);

            LoadBodyDiagram(visitID);
            LoadBodyDiagram2(visitID);
        }

        private void LoadContentInformation4(GetMSConsultVisit registeredPatient)
        {
            List<Variable> lstDisplay = new List<Variable>() { new Variable() { Code = "Catatan Semua PPA", Value = "0" }
                , new Variable() { Code = "Catatan Satu Profesi (Perawat)", Value = "1" }
                , new Variable() { Code = "Catatan Saya", Value = "2" }};
            Methods.SetComboBoxField(cboDisplay, lstDisplay, "Code", "Value");
            cboDisplay.Value = "0";

            string registrationID = "0";
            if (registeredPatient != null)
            {

                if (registeredPatient.GCVisitStatus != Constant.VisitStatus.DISCHARGED && registeredPatient.GCVisitStatus != Constant.VisitStatus.CLOSED)
                {
                    txtFromDate.Text = DateTime.Today.AddDays(-1).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtToDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                }
                else
                {
                    if (registeredPatient.DischargeDate.ToString("dd-MM-yyyy") == Constant.ConstantDate.DEFAULT_NULL)
                    {
                        txtFromDate.Text = DateTime.Today.AddDays(-1).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                        txtToDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    }
                    else
                    {
                        txtFromDate.Text = registeredPatient.DischargeDate.AddDays(-1).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                        txtToDate.Text = registeredPatient.DischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    }
                }

                registrationID = string.Format("{0},{1}", registeredPatient.RegistrationID, registeredPatient.LinkedRegistrationID);
            }

            hdnContentRegistrationID.Value = registrationID;

            BindGridView(1, true, ref Content4PageCount);
        }

        private void LoadContentInformation5(GetMSConsultVisit registeredPatient)
        {
            hdnCurrentSessionID.Value = AppSession.UserLogin.UserID.ToString();

            List<Variable> lstDisplay = new List<Variable>() { new Variable() { Code = "All Notes", Value = "0" }, new Variable() { Code = "My Notes Only", Value = "1" } };
            Methods.SetComboBoxField(cboDisplay1, lstDisplay, "Code", "Value");
            cboDisplay1.Value = "0";

            BindGridViewNursingJournal(1, true, ref gridPatientNursingJournalPageCount);
        }

        private void LoadContentInformation6(GetMSConsultVisit registeredPatient)
        {
            BindGridViewPatientReferral(1, true, ref PageCountPatientReferralList);
        }

        private void LoadContentInformation7(GetMSConsultVisit registeredPatient)
        {
            List<vPatientSurgery> lst = BusinessLayer.GetvPatientSurgeryList(string.Format("VisitID = {0} AND IsDeleted = 0  ORDER BY PatientSurgeryID DESC", registeredPatient.VisitID));
            lvwViewSugery.DataSource = lst;
            lvwViewSugery.DataBind();
        }

        private void LoadContentInformation8(GetMSConsultVisit registeredPatient)
        {
            hdnCurrentParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
            string filterSetvar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
                            AppSession.UserLogin.HealthcareID,
                            Constant.SettingParameter.MD_SERVICE_UNIT_OPERATING_THEATRE);
            List<SettingParameterDt> lstSetvarDt = BusinessLayer.GetSettingParameterDtList(filterSetvar);

            hdnOperatingRoomID.Value = lstSetvarDt.Where(a => a.ParameterCode == Constant.SettingParameter.MD_SERVICE_UNIT_OPERATING_THEATRE).FirstOrDefault().ParameterValue;

            hdnPatientDocumentUrl.Value = string.Format(@"{0}/{1}/", AppConfigManager.QISVirtualDirectory, AppConfigManager.QISPatientDocumentsPath.Replace("#MRN", hdnMedicalNo.Value));

            BindGridViewSurgeryHistory(1, true, ref PageCountSurgeryHistory);
        }

        private void LoadContentInformation9(GetMSConsultVisit registeredPatient)
        {
            BindGridViewIntakeOutput(1, true, ref PageCountIntakeOutput);
            BindGridViewDt2IntakeOutput(1, true, ref PageCountIntakeOutput);
            BindGridView6IntakeOutput(1, true, ref PageCount2IntakeOutput);
        }

        private void LoadContentInformation10_11_12(GetMSConsultVisit registeredPatient)
        {
            string filterExpression = string.Format("TransactionID IN (SELECT TransactionID FROM PatientChargesHd WITH(NOLOCK) WHERE VisitID = {0} AND TransactionCode IN ('{1}') AND GCTransactionStatus <> '{2}') AND IsDeleted = 0 ORDER BY ID DESC", registeredPatient.VisitID, Constant.TransactionCode.OTHER_DIAGNOSTIC_CHARGES, Constant.TransactionStatus.VOID);
            List<vPatientChargesDt> lstEntity = BusinessLayer.GetvPatientChargesDtList(filterExpression);

            #region Laboratory
            string filterSetvar = string.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID, Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID);
            List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(filterSetvar);

            hdnLabHealthcareServiceUnitID.Value = BusinessLayer.GetHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND ServiceUnitID = {1}", AppSession.UserLogin.HealthcareID, lstSettingParameter.Where(t => t.ParameterCode == Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).FirstOrDefault().ParameterValue))[0].HealthcareServiceUnitID.ToString();

            txtPeriodFromLaboratory.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPeriodToLaboratory.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            BindGridViewLaboratory(1, true, ref PageCountLaboratory);
            BindGridViewDtLaboratory(1, true, ref PageCountLaboratory);

            BindGridView2Laboratory(1, true, ref PageCountLaboratory2);
            #endregion

            #region Imaging
            hdnImagingHealthcareServiceUnitID.Value = BusinessLayer.GetHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND ServiceUnitID = {1}", AppSession.UserLogin.HealthcareID, lstSettingParameter.Where(t => t.ParameterCode == Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID).FirstOrDefault().ParameterValue))[0].HealthcareServiceUnitID.ToString();

            List<SettingParameterDt> setvar = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}', '{2}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IS_RIS_WEB_VIEW_URL, Constant.SettingParameter.IS_RIS_USING_RESULT_IN_PDF));
            hdnRISVendorImaging.Value = AppSession.RIS_HL7_MESSAGE_FORMAT;
            string viewerUrl = setvar.Where(w => w.ParameterCode == Constant.SettingParameter.IS_RIS_WEB_VIEW_URL).FirstOrDefault().ParameterValue;
            hdnIsRISUsingPDFResult.Value = setvar.Where(w => w.ParameterCode == Constant.SettingParameter.IS_RIS_USING_RESULT_IN_PDF).FirstOrDefault().ParameterValue;
            if (setvar != null && !String.IsNullOrEmpty(viewerUrl))
            {
                hdnViewerUrlImaging.Value = viewerUrl;
            }
            else
            {
                hdnViewerUrlImaging.Value = AppSession.RIS_WEB_VIEW_URL;
            }

            hdnDocumentPathImaging.Value = string.Format(@"{0}/{1}/", AppConfigManager.QISVirtualDirectory, AppConfigManager.QISPatientDocumentsPath.Replace("#MRN", hdnMedicalNo.Value));

            BindGridViewImaging(1, true, ref PageCountImaging);
            BindGridViewDtImaging(1, true, ref PageCountImaging);
            #endregion

            #region otherDiagnostic
            grdViewOtherDiagnostic.DataSource = lstEntity;
            grdViewOtherDiagnostic.DataBind();
            #endregion
        }

        private void LoadContentInformation13(GetMSConsultVisit registeredPatient)
        {
            //string filterExpression = string.Format("VisitID = {0} AND GCPrescriptionOrderStatus != '{1}' ORDER BY PrescriptionOrderDetailID DESC", registeredPatient.VisitID, Constant.OrderStatus.CANCELLED);
            //rptMedication.DataSource = BusinessLayer.GetvPatientVisitPrescriptionList(filterExpression);
            //rptMedication.DataBind();

            List<Variable> lstDisplay = new List<Variable>() { new Variable() { Code = "All", Value = "0" }
                , new Variable() { Code = "Obat Oral/Supposituria/Topikal", Value = "1" }, new Variable() { Code = "Obat Injeksi", Value = "2" }};
            Methods.SetComboBoxField(cboDisplayDrug, lstDisplay, "Code", "Value");
            cboDisplayDrug.Value = hdnDisplayMode.Value;

            List<Variable> lstStatus = new List<Variable>() { 
                new Variable() { Code = "All", Value = "0" }, 
            new Variable() { Code = "Active", Value = "1" }, 
            new Variable() { Code = "Stop", Value = "2" }};
            Methods.SetComboBoxField(cboMedicationStatus, lstStatus, "Code", "Value");
            cboMedicationStatus.Value = hdnMedicationStatus.Value;

            BindGridViewDrug(1, true, ref PageCountDrug);
        }

        private void LoadContentInformation14(GetMSConsultVisit registeredPatient)
        {
            BindGridViewPatientTransfer(1, true, ref PageCountPatientTransferList);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string startDate = Helper.GetDatePickerValue(txtFromDate).ToString(Constant.FormatString.DATE_FORMAT_112);
            string endDate = Helper.GetDatePickerValue(txtToDate).ToString(Constant.FormatString.DATE_FORMAT_112);
            
            List<GetMSPatientVisitNote> lstEntity = BusinessLayer.GetMSPatientVisitNoteList(hdnContentRegistrationID.Value, startDate, endDate);
            grdMedicalSummaryContent4.DataSource = lstEntity;
            grdMedicalSummaryContent4.DataBind();
        }

        private void BindGridViewNursingJournal(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            vConsultVisit4 entityLinkedRegistration = BusinessLayer.GetvConsultVisit4List(string.Format("RegistrationID = (SELECT LinkedRegistrationID FROM Registration WITH(NOLOCK) WHERE RegistrationID = {0})", hdnRegistrationID.Value)).FirstOrDefault();
            int cvLinkedID = 0;
            if (entityLinkedRegistration != null)
            {
                cvLinkedID = entityLinkedRegistration.VisitID;
            }

            //string filterExpression = string.Format("VisitID IN ({0},{1})", hdnVisitIDPopUpCtl.Value, cvLinkedID);

            //if (cboDisplay.Value.ToString() != "1")
            //{
            //    filterExpression = string.Format("VisitID IN ({0},{1})", hdnVisitIDPopUpCtl.Value, cvLinkedID);
            //}
            //else
            //{
            //    filterExpression = string.Format("VisitID IN ({0},{1}) AND ParamedicID = {2}", hdnVisitIDPopUpCtl.Value, cvLinkedID, AppSession.UserLogin.ParamedicID);
            //}

            //if (filterExpression != "")
            //{
            //    filterExpression += " AND IsDeleted = 0";
            //}
            //else
            //{
            //    filterExpression = "IsDeleted = 0";
            //}

            string oListVisitID;
            int oParamedicID;

            oListVisitID = hdnVisitIDPopUpCtl.Value + "," + cvLinkedID.ToString();

            if (cboDisplay.Value.ToString() != "1")
            {
                oParamedicID = 0;
            }
            else
            {
                oParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
            }

            List<GetMSNursingJournal> lstEntity = BusinessLayer.GetMSNursingJournalList(oListVisitID, oParamedicID);
            grdMedicalSummaryContent5.DataSource = lstEntity;
            grdMedicalSummaryContent5.DataBind();
        }

        protected void grdViewImaging_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vPatientChargesDt oChargesDt = e.Row.DataItem as vPatientChargesDt;
                vImagingResultDt oResultDt = BusinessLayer.GetvImagingResultDtList(string.Format("ID IN (SELECT ID FROM ImagingResultHd WITH(NOLOCK) WHERE ChargeTransactionID = {0}) AND ItemID = {1} AND IsDeleted = 0", oChargesDt.TransactionID, oChargesDt.ItemID)).FirstOrDefault();

                Literal literal = (Literal)e.Row.FindControl("literal");
                if (oResultDt != null)
                {
                    literal.Text = oResultDt.TestResult1;
                }
                //HtmlTextArea taResultValue = (HtmlTextArea)e.Row.FindControl("taResultValue");
                //if (oResultDt != null && taResultValue != null)
                //    taResultValue.InnerText = oResultDt.TestResult1;
            }
        }

        protected void grdViewOtherDiagnostic_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vPatientChargesDt oChargesDt = e.Row.DataItem as vPatientChargesDt;
                vImagingResultDt oResultDt = BusinessLayer.GetvImagingResultDtList(string.Format("ID IN (SELECT ID FROM ImagingResultHd WITH(NOLOCK) WHERE ChargeTransactionID = {0}) AND ItemID = {1} AND IsDeleted = 0", oChargesDt.TransactionID, oChargesDt.ItemID)).FirstOrDefault();

                Literal literal = (Literal)e.Row.FindControl("literalOtherDiagnostic");
                if (oResultDt != null)
                {
                    literal.Text = oResultDt.TestResult1;
                }
            }
        }

        protected void grdMedicalSummaryContent4_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GetMSPatientVisitNote entity = e.Row.DataItem as GetMSPatientVisitNote;
                HtmlInputButton btnVerified = e.Row.FindControl("btnVerify") as HtmlInputButton;
                HtmlControl divVerifiedInfo = e.Row.FindControl("divVerifiedInformation") as HtmlControl;
                HtmlControl divPhysicianVerifiedInfo = e.Row.FindControl("divPhysicianVerifiedInformation") as HtmlControl;
                HtmlControl divNursingNotesInfo = (HtmlControl)e.Row.FindControl("divNursingNotesInfo");
                HtmlControl divConfirmationInfo = (HtmlControl)e.Row.FindControl("divConfirmationInfo");
                HtmlControl divSignature = e.Row.FindControl("divParamedicSignature") as HtmlControl;
                HtmlControl divView = e.Row.FindControl("divView") as HtmlControl;

                if (entity.GCPatientNoteType == Constant.PatientVisitNotes.NURSING_NOTES)
                {
                    if (!entity.IsWrite)
                    {
                        divNursingNotesInfo.Style.Add("display", "none");
                    }

                    if (!entity.IsConfirmed)
                    {
                        if (divConfirmationInfo != null)
                            divConfirmationInfo.Style.Add("display", "none");
                    }
                    else
                    {
                        if (divConfirmationInfo != null)
                            divConfirmationInfo.Style.Add("display", "block");
                        divNursingNotesInfo.Style.Add("display", "block");
                    }
                }
                else
                {
                    divNursingNotesInfo.Style.Add("display", "none");
                    divConfirmationInfo.Style.Add("display", "none");
                }

                if (!entity.IsVerified)
                {
                    if (divPhysicianVerifiedInfo != null)
                        divPhysicianVerifiedInfo.Visible = false;
                }
                else
                {
                    if (divPhysicianVerifiedInfo != null)
                        divPhysicianVerifiedInfo.Visible = true;
                }

                if (AppSession.UserLogin.IsPrimaryNurse)
                {
                    if (!entity.IsVerifiedByPrimaryNurse && entity.GCPatientNoteType == Constant.PatientVisitNotes.NURSING_NOTES)
                    {
                        btnVerified.Visible = true;
                        divVerifiedInfo.Visible = false;
                    }
                    else
                    {
                        btnVerified.Visible = false;
                        if (entity.GCPatientNoteType == Constant.PatientVisitNotes.NURSING_NOTES)
                        {
                            divVerifiedInfo.Visible = true;
                        }
                        else
                        {
                            divVerifiedInfo.Visible = false;
                        }
                    }
                }
                else
                {
                    btnVerified.Visible = false;
                    divVerifiedInfo.Visible = false;
                }

                if (divSignature != null)
                {
                    divSignature.Visible = entity.ParamedicID == AppSession.UserLogin.ParamedicID;
                }

                if (divView != null)
                {
                    divView.Visible = entity.GCPatientNoteType == Constant.PatientVisitNotes.NURSE_INITIAL_ASSESSMENT;
                }
            }
        }

        protected void cbpMedicalSummaryContent4View_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref Content4PageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridView(1, true, ref Content4PageCount);
                    result = "refresh|" + Content4PageCount;
                }
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpMedicalSummaryContent5View_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewNursingJournal(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewNursingJournal(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
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
        private void BindGridViewDiagnosis(int pageIndex, bool isCountPageCount, ref int pageCount, ref int visitID)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", visitID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientDiagnosisRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientDiagnosis> lstEntity = BusinessLayer.GetvPatientDiagnosisList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "GCDiagnoseType");

            grdDiagnosisView.DataSource = lstEntity;
            grdDiagnosisView.DataBind();
        }

        protected void cbpDiagnosisView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            int visitID = Convert.ToInt32(hdnVisitIDPopUpCtl.Value);
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDiagnosis(Convert.ToInt32(param[1]), false, ref pageCount, ref visitID);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDiagnosis(1, true, ref pageCount, ref visitID);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = string.Empty;
        }
        #endregion

        #region Diagnosis Kajian Awal Medis
        private void BindGridViewDiagnosisParamedic(int pageIndex, bool isCountPageCount, ref int pageCount, ref int visitID)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", visitID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientDiagnosisRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientDiagnosis> lstEntity = BusinessLayer.GetvPatientDiagnosisList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "GCDiagnoseType");

            grdDiagnosisParamedicView.DataSource = lstEntity;
            grdDiagnosisParamedicView.DataBind();
        }

        protected void cbpDiagnosisParamedicView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            int visitID = Convert.ToInt32(hdnVisitIDPopUpCtl.Value);
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDiagnosisParamedic(Convert.ToInt32(param[1]), false, ref pageCount, ref visitID);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDiagnosisParamedic(1, true, ref pageCount, ref visitID);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = string.Empty;
        }
        #endregion

        #region Vital Sign
        private void BindGridViewVitalSign(int pageIndex, bool isCountPageCount, ref int pageCount, ref int visitID)
        {
            string filterExpression = string.Empty;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID IN ({0}) AND IsInitialAssessment = 1 AND IsDeleted = 0 ORDER BY ID DESC", visitID);

            List<vVitalSignHd> lstEntity = BusinessLayer.GetvVitalSignHdList(filterExpression);
            lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID IN ({0}) ORDER BY DisplayOrder", visitID));
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
            int visitID = Convert.ToInt32(hdnVisitIDPopUpCtl.Value);
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewVitalSign(Convert.ToInt32(param[1]), false, ref pageCount, ref visitID);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewVitalSign(1, true, ref pageCount, ref visitID);
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
            int visitID = Convert.ToInt32(hdnVisitIDPopUpCtl.Value);
            if (e.Parameter == "refresh")
            {
                string filterExpression = "";
                filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", hdnVisitIDPopUpCtl.Value);

                pageCount = BusinessLayer.GetvPatientBodyDiagramHdRowCount(filterExpression);
                result = "count|" + pageCount;
                if (pageCount > 0)
                    OnLoadBodyDiagram(0, visitID);
            }
            else if (e.Parameter == "edit")
            {
                result = "edit";
                OnLoadBodyDiagram(pageIndex, visitID);
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
                OnLoadBodyDiagram(pageIndex, visitID);
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

        private void LoadBodyDiagram(int visitID)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", visitID);
            int pageCount = BusinessLayer.GetvPatientBodyDiagramHdRowCount(filterExpression);
            hdnPageCount.Value = pageCount.ToString();

            if (pageCount > 0)
            {
                hdnPageIndex.Value = "0";
                OnLoadBodyDiagram(0, visitID);
                tblBodyDiagramNavigation.Style.Remove("display");
            }
            else
            {
                divBodyDiagram.Style.Add("display", "none");
                tblEmpty.Style.Remove("display");
            }
        }

        protected void OnLoadBodyDiagram(int PageIndex, int visitID)
        {
            string filterExpression = "";
            filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", visitID);
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

        #region Body Diagram 2
        protected void cbpBodyDiagramView2_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            int pageIndex = Convert.ToInt32(hdnPageIndex.Value);
            int pageCount = Convert.ToInt32(hdnPageCount.Value);
            int visitID = Convert.ToInt32(hdnVisitIDPopUpCtl.Value);
            if (e.Parameter == "refresh")
            {
                string filterExpression = "";
                filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", hdnVisitIDPopUpCtl.Value);

                pageCount = BusinessLayer.GetvPatientBodyDiagramHdRowCount(filterExpression);
                result = "count|" + pageCount;
                if (pageCount > 0)
                    OnLoadBodyDiagram2(0, visitID);
            }
            else if (e.Parameter == "edit")
            {
                result = "edit";
                OnLoadBodyDiagram2(pageIndex, visitID);
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
                OnLoadBodyDiagram2(pageIndex, visitID);
                result = "index|" + pageIndex;
            }

            if (pageCount > 0)
            {
                hdnPageIndex.Value = "0";
                tblBodyDiagramNavigation2.Style.Remove("display");
            }
            else
            {
                divBodyDiagram2.Style.Add("display", "none");
                tblEmpty2.Style.Remove("display");
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void LoadBodyDiagram2(int visitID)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", visitID);
            int pageCount = BusinessLayer.GetvPatientBodyDiagramHdRowCount(filterExpression);
            hdnPageCount.Value = pageCount.ToString();

            if (pageCount > 0)
            {
                hdnPageIndex.Value = "0";
                OnLoadBodyDiagram2(0, visitID);
                tblBodyDiagramNavigation2.Style.Remove("display");
            }
            else
            {
                divBodyDiagram2.Style.Add("display", "none");
                tblEmpty2.Style.Remove("display");
            }
        }

        protected void OnLoadBodyDiagram2(int PageIndex, int visitID)
        {
            string filterExpression = "";
            filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", visitID);
            vPatientBodyDiagramHd entity = BusinessLayer.GetvPatientBodyDiagramHd(filterExpression, PageIndex, "ID DESC");
            BodyDiagramToControl2(entity);

            filterExpression = string.Format("ID = {0} AND IsDeleted = 0", entity.ID);
            rptRemarks2.DataSource = BusinessLayer.GetvPatientBodyDiagramDtList(filterExpression);
            rptRemarks2.DataBind();
        }

        private void BodyDiagramToControl2(vPatientBodyDiagramHd entity)
        {
            spnParamedicName2.InnerHtml = entity.ParamedicName;
            spnObservationDateTime2.InnerHtml = entity.DisplayObservationDateTime;
            spnDiagramName2.InnerHtml = entity.DiagramName;

            imgBodyDiagram2.Src = entity.FileImageUrl;
            hdnBodyDiagram2ID.Value = entity.ID.ToString();

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
            int visitID = Convert.ToInt32(hdnVisitIDPopUpCtl.Value);
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewROS(Convert.ToInt32(param[1]), false, ref pageCount, ref visitID);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewROS(1, true, ref pageCount, ref visitID);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridViewROS(int pageIndex, bool isCountPageCount, ref int pageCount, ref int visitID)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", visitID);

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
        #endregion

        #region Intake Output
        protected void cbpViewDtIntakeOutput_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt1IntakeOutput(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDt1IntakeOutput(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpView6IntakeOutput_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView6IntakeOutput(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridView6IntakeOutput(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewDt2IntakeOutput_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt2IntakeOutput(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewDt2IntakeOutput(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewDt3IntakeOutput_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt3IntakeOutput(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewDt3IntakeOutput(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewDt4IntakeOutput_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt4IntakeOutput(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewDt4IntakeOutput(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewDt5IntakeOutput_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt5IntakeOutput(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewDt5IntakeOutput(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridViewIntakeOutput(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "";
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID = {0}", hdnVisitIDPopUpCtl.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvFluidBalanceSummaryRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_COMPACT);
            }

            List<vFluidBalanceSummary> lstEntity = BusinessLayer.GetvFluidBalanceSummaryList(filterExpression, Constant.GridViewPageSize.GRID_COMPACT, pageIndex, "LogDate DESC");
            if (lstEntity.Count > 0)
            {
                hdnLogDate.Value = lstEntity.FirstOrDefault().cfLogDate1;
            }
            else
            {
                hdnLogDate.Value = "0";
            }

            grdViewIntakeOutput.DataSource = lstEntity;
            grdViewIntakeOutput.DataBind();
        }

        private void BindGridViewDt1IntakeOutput(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            if (hdnLogDate.Value != "0")
            {
                string filterExpression = string.Format("VisitID = {0} AND LogDate = '{1}' AND GCFluidGroup = '{2}' AND IsInitializeIntake = 1 AND IsDeleted = 0", hdnVisitIDPopUpCtl.Value, hdnLogDate.Value, Constant.FluidBalanceGroup.Intake);

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvFluidBalanceRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }

                List<vFluidBalance> lstEntity = BusinessLayer.GetvFluidBalanceList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "LogTime DESC");

                grdViewDtIntakeOutput.DataSource = lstEntity;
                grdViewDtIntakeOutput.DataBind();
            }
        }

        private void BindGridViewDt2IntakeOutput(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            if (hdnLogDate.Value != "0")
            {
                string filterExpression = string.Format("VisitID = {0} AND LogDate = '{1}' AND GCFluidGroup = '{2}' AND IsDeleted = 0", hdnVisitIDPopUpCtl.Value, hdnLogDate.Value, Constant.FluidBalanceGroup.Output);

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvFluidBalanceRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }

                List<vFluidBalance> lstEntity = BusinessLayer.GetvFluidBalanceList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "LogTime DESC");

                grdViewDt2IntakeOutput.DataSource = lstEntity;
                grdViewDt2IntakeOutput.DataBind();
            }
        }

        private void BindGridViewDt3IntakeOutput(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            if (hdnLogDate.Value != "0")
            {
                string filterExpression = string.Format("VisitID = {0} AND LogDate = '{1}' AND GCFluidGroup = '{2}' AND IsDeleted = 0", hdnVisitIDPopUpCtl.Value, hdnLogDate.Value, Constant.FluidBalanceGroup.Output_Tidak_Diukur);

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvFluidBalanceRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }

                List<vFluidBalance> lstEntity = BusinessLayer.GetvFluidBalanceList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "LogTime ASC");

                grdViewDt3IntakeOutput.DataSource = lstEntity;
                grdViewDt3IntakeOutput.DataBind();
            }
        }

        private void BindGridViewDt4IntakeOutput(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            if (hdnLogDate.Value != "0")
            {
                string filterExpression = string.Format("VisitID = {0} AND LogDate = '{1}' AND GCFluidGroup = '{2}' AND FluidName = '{3}' AND IsInitializeIntake = 0 AND IsDeleted = 0", hdnVisitIDPopUpCtl.Value, hdnLogDate.Value, Constant.FluidBalanceGroup.Intake, hdnFluidName.Value);

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvFluidBalanceRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }

                List<vFluidBalance> lstEntity = BusinessLayer.GetvFluidBalanceList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "LogTime DESC");

                grdViewDt4IntakeOutput.DataSource = lstEntity;
                grdViewDt4IntakeOutput.DataBind();
            }
        }

        private void BindGridViewDt5IntakeOutput(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            if (hdnLogDate.Value != "0")
            {
                string filterExpression = string.Format("VisitID = {0} AND LogDate = '{1}' AND GCFluidGroup = '{2}' AND IsDeleted = 0", hdnVisitIDPopUpCtl.Value, hdnLogDate.Value, Constant.FluidBalanceGroup.Intake_Tidak_Diukur);

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvFluidBalanceRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }

                List<vFluidBalance> lstEntity = BusinessLayer.GetvFluidBalanceList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "LogTime DESC");

                grdViewDt5IntakeOutput.DataSource = lstEntity;
                grdViewDt5IntakeOutput.DataBind();
            }
        }

        private void BindGridView6IntakeOutput(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "";
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID = {0} AND IsDeleted = 0", hdnVisitIDPopUpCtl.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvIVTheraphyNoteRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_COMPACT);
            }

            List<vIVTheraphyNote> lstEntity = BusinessLayer.GetvIVTheraphyNoteList(filterExpression, Constant.GridViewPageSize.GRID_COMPACT, pageIndex, "IVTherapyNoteDate DESC");
            grdView6IntakeOutput.DataSource = lstEntity;
            grdView6IntakeOutput.DataBind();
        }
        #endregion

        #region Patient Transfer
        protected void cbpViewPatientTransferList_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewPatientTransfer(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewPatientTransfer(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridViewPatientTransfer(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            List<GetMSPatientNurseTransfer> lstView = BusinessLayer.GetMSPatientNurseTransferList(Convert.ToInt32(hdnRegistrationID.Value));
            grdViewPatientTransferList.DataSource = lstView;
            grdViewPatientTransferList.DataBind();
            grdPatientTransferDetail.DataSource = lstView;
            grdPatientTransferDetail.DataBind();
        }

        protected void cbpViewPatientTransferDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            List<vPatientNurseTransfer> lstDt = BusinessLayer.GetvPatientNurseTransferList(string.Format("RegistrationID = {0} AND ID = {1} AND IsDeleted = 0", hdnRegistrationID.Value, hdnCollapseID.Value));
            grdPatientTransferDetail.DataSource = lstDt;
            grdPatientTransferDetail.DataBind();
        }
        #endregion

        #region Patient Referral
        private void BindGridViewPatientReferral(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "";
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("RegistrationID = {0} AND IsDeleted = 0", hdnRegistrationID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientReferralRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientReferral> lstView = BusinessLayer.GetvPatientReferralList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
            grdViewPatientReferralList.DataSource = lstView;
            grdViewPatientReferralList.DataBind();
        }

        protected void cbpViewPatientReferralList_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewPatientReferral(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewPatientReferral(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        #region FallRiskAssessment
        protected void cbpFormListFallRiskAssessmentHeader_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewFallRiskAssessmentHeader(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewFallRiskAssessmentHeader(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpFallRiskAssessmentDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewFallRiskAssessmentDetail(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewFallRiskAssessmentDetail(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridViewFallRiskAssessmentHeader(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("ParentID = '{0}' ORDER BY StandardCodeID", Constant.StandardCode.FALL_RISK_ASSESSMENT);
            List<StandardCode> lstEntity = BusinessLayer.GetStandardCodeList(filterExpression);
            grdFallRiskAssessmentHeader.DataSource = lstEntity;
            grdFallRiskAssessmentHeader.DataBind();
        }

        private void BindGridViewFallRiskAssessmentDetail(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = ""; //hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";

            filterExpression += string.Format("RegistrationID IN ({0},{1}) AND GCFallRiskAssessmentType = '{2}' AND IsDeleted = 0", hdnRegistrationID.Value, hdnLinkedRegistrationID.Value, hdnGCAssessmentType.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvFallRiskAssessmentRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vFallRiskAssessment> lstEntity = BusinessLayer.GetvFallRiskAssessmentList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "AssessmentID DESC");
            grdFallRiskAssessmentDetail.DataSource = lstEntity;
            grdFallRiskAssessmentDetail.DataBind();
        }
        #endregion

        #region PainAssessment
        protected void cbpPainAssessmentHeader_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewPainAssessmentHeader(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewPainAssessmentHeader(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpPainAssessmentDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDtPainAssessmentDetail(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewDtPainAssessmentDetail(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridViewPainAssessmentHeader(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("ParentID = '{0}' AND IsActive = 1 ORDER BY StandardCodeID", Constant.StandardCode.PAIN_ASSESSMENT);
            List<StandardCode> lstEntity = BusinessLayer.GetStandardCodeList(filterExpression);
            grdcbpPainAssessmentHeader.DataSource = lstEntity;
            grdcbpPainAssessmentHeader.DataBind();
        }

        private void BindGridViewDtPainAssessmentDetail(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = ""; //hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";

            filterExpression += string.Format("RegistrationID IN ({0},{1}) AND GCPainAssessmentType = '{2}' AND IsDeleted = 0", hdnRegistrationID.Value, hdnLinkedRegistrationID.Value, hdnGCAssessmentTypePainAssessment.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPainAssessmentRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPainAssessment> lstEntity = BusinessLayer.GetvPainAssessmentList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "AssessmentID DESC");
            grdcbpPainAssessmentDetail.DataSource = lstEntity;
            grdcbpPainAssessmentDetail.DataBind();
        }
        #endregion

        #region SurgeryHistory
        protected void cbpViewSurgeryHistory_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewSurgeryHistory(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewSurgeryHistory(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewDtSurgeryHistory_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt1(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewDt1(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewDt2SurgeryHistory_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt2(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewDt2(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewDt3SurgeryHistory_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt3(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewDt3(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewDt4SurgeryHistory_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt4(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewDt4(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewDt5SurgeryHistory_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt5(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewDt5(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewDt6SurgeryHistory_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt6(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewDt6(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewDt7SurgeryHistory_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt7(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewDt7(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewDt8SurgeryHistory_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt8(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewDt8(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridViewSurgeryHistory(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "";

            if (rblItemTypeSurgeryHistory.SelectedValue == "1")
            {
                filterExpression += string.Format("VisitID = {0} AND HealthcareServiceUnitID = {1} AND GCOrderStatus NOT IN ('{2}','{3}')", hdnVisitIDPopUpCtl.Value, hdnOperatingRoomID.Value, Constant.OrderStatus.OPEN, Constant.OrderStatus.CANCELLED);
            }
            else
            {
                filterExpression += string.Format("MRN = {0} AND HealthcareServiceUnitID = {1} AND GCOrderStatus NOT IN ('{2}','{3}')", hdnMRN.Value, hdnOperatingRoomID.Value, Constant.OrderStatus.OPEN, Constant.OrderStatus.CANCELLED);
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvSurgeryTestOrderHd2RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            if (pageCount == 0)
            {
                hdnID.Value = "0";
            }

            List<vSurgeryTestOrderHd2> lstEntity = BusinessLayer.GetvSurgeryTestOrderHd2List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "TestOrderID DESC");

            grdViewSurgeryHistory.DataSource = lstEntity;
            grdViewSurgeryHistory.DataBind();
        }


        #region Detail List
        private void BindGridViewDt1(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("TestOrderID = {0} AND IsDeleted = 0", hdnSurgeryHistoryID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPreSurgeryAssessmentRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPreSurgeryAssessment> lstEntity = BusinessLayer.GetvPreSurgeryAssessmentList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "PreSurgicalAssessmentID DESC");

            grdViewDtSurgeryHistory.DataSource = lstEntity;
            grdViewDtSurgeryHistory.DataBind();
        }

        private void BindGridViewDt2(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("TestOrderID = {0} AND IsDeleted = 0", hdnSurgeryHistoryID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvSurgicalSafetyCheckRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vSurgicalSafetyCheck> lstEntity = BusinessLayer.GetvSurgicalSafetyCheckList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");

            grdViewDt2SurgeryHistory.DataSource = lstEntity;
            grdViewDt2SurgeryHistory.DataBind();
        }

        private void BindGridViewDt3(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("TestOrderID = {0} AND IsDeleted = 0", hdnSurgeryHistoryID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientMedicalDeviceRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientMedicalDevice> lstEntity = BusinessLayer.GetvPatientMedicalDeviceList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
            grdViewDt3SurgeryHistory.DataSource = lstEntity;
            grdViewDt3SurgeryHistory.DataBind();
        }

        private void BindGridViewDt4(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("TestOrderID = {0} AND IsDeleted = 0", hdnSurgeryHistoryID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientSurgeryRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientSurgery> lstEntity = BusinessLayer.GetvPatientSurgeryList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "PatientSurgeryID DESC");

            grdViewDt4SurgeryHistory.DataSource = lstEntity;
            grdViewDt4SurgeryHistory.DataBind();
        }

        private void BindGridViewDt5(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("TestOrderID = {0} AND IsDeleted = 0", hdnSurgeryHistoryID.Value);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientDocumentRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientDocument> lstEntity = BusinessLayer.GetvPatientDocumentList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
            grdViewDt5SurgeryHistory.DataSource = lstEntity;
            grdViewDt5SurgeryHistory.DataBind();
        }

        private void BindGridViewDt6(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("TestOrderID = {0} AND IsDeleted = 0", hdnSurgeryHistoryID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPerioperativeNursingRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPerioperativeNursing> lstEntity = BusinessLayer.GetvPerioperativeNursingList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");

            grdViewDt6SurgeryHistory.DataSource = lstEntity;
            grdViewDt6SurgeryHistory.DataBind();
        }

        private void BindGridViewDt7(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("TestOrderID = {0} AND IsDeleted = 0", hdnSurgeryHistoryID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPreAnesthesyAssessmentRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPreAnesthesyAssessment> lstEntity = BusinessLayer.GetvPreAnesthesyAssessmentList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "PreAnesthesyAssessmentID DESC");

            grdViewDt7SurgeryHistory.DataSource = lstEntity;
            grdViewDt7SurgeryHistory.DataBind();
        }

        private void BindGridViewDt8(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("TestOrderID = {0} AND IsDeleted = 0", hdnSurgeryHistoryID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvSurgeryAnesthesyStatusRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vSurgeryAnesthesyStatus> lstEntity = BusinessLayer.GetvSurgeryAnesthesyStatusList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "AnesthesyStatusID DESC");

            grdViewDt8SurgeryHistory.DataSource = lstEntity;
            grdViewDt8SurgeryHistory.DataBind();
        }
        #endregion
        #endregion

        #region LaboratoryResult
        #region Header
        private void BindGridViewLaboratory(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("MRN = {0} AND HealthcareServiceUnitID = {1} AND GCTransactionStatus != '{2}'", hdnMRN.Value, hdnLabHealthcareServiceUnitID.Value, Constant.TransactionStatus.VOID);

            if (rblItemTypeLaboratoryTab1.SelectedValue == "1")
            {
                filterExpression += string.Format(" AND VisitID = {0}", hdnVisitIDPopUpCtl.Value);
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientChargesHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_LIMA_PULUH);
            }

            List<vPatientChargesHd> lstEntity = BusinessLayer.GetvPatientChargesHdList(filterExpression, Constant.GridViewPageSize.GRID_LIMA_PULUH, pageIndex, "TransactionID DESC");
            grdViewLaboratory.DataSource = lstEntity;
            grdViewLaboratory.DataBind();
        }

        protected void grdViewLaboratory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vPatientChargesHd obj = e.Row.DataItem as vPatientChargesHd;
                HtmlControl divBtnLaboratoryResult = e.Row.FindControl("divBtnLaboratoryResult") as HtmlControl;
                HtmlInputButton btnViewLaboratoryReport = e.Row.FindControl("btnViewLaboratoryReport") as HtmlInputButton;

                if (hdnIsUsingPrintLBResult.Value == "1")
                {
                    btnViewLaboratoryReport.Visible = true;
                    divBtnLaboratoryResult.Visible = true;
                }
                else
                {
                    btnViewLaboratoryReport.Visible = false;
                    divBtnLaboratoryResult.Visible = false;
                }
            }
        }

        protected void cbpViewLaboratory_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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
        #endregion

        #region Detail
        private void BindGridViewDtLaboratory(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnLaboratoryID1.Value != "")
            {
                string orderBy = "FractionDisplayOrder";
                filterExpression = string.Format("ChargeTransactionID = {0} AND IsDeleted = 0", hdnLaboratoryID1.Value);

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvLaboratoryResultDtRowCount(filterExpression, orderBy);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_LIMA_PULUH);
                }
            }

            List<vLaboratoryResultDt> lstEntity = BusinessLayer.GetvLaboratoryResultDtList(filterExpression, Constant.GridViewPageSize.GRID_LIMA_PULUH, pageIndex);
            grdViewDtLaboratory.DataSource = lstEntity;
            grdViewDtLaboratory.DataBind();
        }

        protected void cbpViewDtLaboratory_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDtLaboratory(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewDtLaboratory(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        #region Header2
        private void BindGridView2Laboratory(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string startDate = Helper.GetDatePickerValue(txtPeriodFromLaboratory).ToString(Constant.FormatString.DATE_FORMAT_112);
            string endDate = Helper.GetDatePickerValue(txtPeriodToLaboratory).ToString(Constant.FormatString.DATE_FORMAT_112);
            Int32 mrn = Convert.ToInt32(hdnMRN.Value);
            Int32 registrationID = Convert.ToInt32(hdnRegistrationID.Value);
            if (rblItemTypeLaboratoryTab2.SelectedValue != "1")
            {
                registrationID = 0;
            }

            List<GetDistinctFraction> lstEntity = BusinessLayer.GetDistinctFraction(startDate, endDate, mrn, 0, registrationID);
            grdViewTab2Laboratory.DataSource = lstEntity;

            pageCount = lstEntity.Count;

            grdViewTab2Laboratory.DataBind();
        }

        protected void cbpViewTab2Laboratory_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                BindGridView2Laboratory(1, true, ref pageCount);
                result = "refresh|" + pageCount;
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        #region Detail2
        private void BindGridViewDt2Laboratory(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string startDate = Helper.GetDatePickerValue(txtPeriodFromLaboratory).ToString(Constant.FormatString.DATE_FORMAT_112);
            string endDate = Helper.GetDatePickerValue(txtPeriodToLaboratory).ToString(Constant.FormatString.DATE_FORMAT_112);
            Int32 mrn = Convert.ToInt32(hdnMRN.Value);
            Int32 fractionID = Convert.ToInt32(hdnLaboratoryID2.Value);
            Int32 registrationID = Convert.ToInt32(hdnRegistrationID.Value);
            if (rblItemTypeLaboratoryTab2.SelectedValue != "1")
            {
                registrationID = 0;
            }

            List<GetDistinctFractionPerDetail> lstEntity = BusinessLayer.GetDistinctFractionPerDetail(startDate, endDate, mrn, 0, fractionID, registrationID);
            List<PartialData> lstData = new List<PartialData>();
            List<FractionValue> lstValue = new List<FractionValue>();

            if (lstEntity.Count > 0)
            {
                PartialData entity = new PartialData();
                entity.SequenceNo = 1;
                entity.cfCreatedDate = lstEntity.FirstOrDefault().cfCreatedDate;

                foreach (GetDistinctFractionPerDetail e in lstEntity)
                {
                    FractionValue obj = new FractionValue();
                    obj.SequenceNo = 1;
                    obj.IsNormal = e.IsNormal;
                    obj.Fractionvalue = e.ResultValue;
                    obj.MetricUnitName = e.MetricUnitName;
                    lstValue.Add(obj);
                }

                entity.FractionValue = lstValue;
                lstData.Add(entity);

                lvwViewDtLaboratory.DataSource = lstData;
                lvwViewDtLaboratory.DataBind();

                rptFractionDateHeader.DataSource = lstEntity;
                rptFractionDateHeader.DataBind();
            }
            else
            {
                lvwViewDtLaboratory.DataSource = lstData;
                lvwViewDtLaboratory.DataBind();

                rptFractionDateHeader.DataSource = lstEntity;
                rptFractionDateHeader.DataBind();
            }
        }

        protected void lvwViewDtLaboratory_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                PartialData obj = (PartialData)e.Item.DataItem;
                Repeater rptFractionDetailValue = (Repeater)e.Item.FindControl("rptFractionDetailValue");
                rptFractionDetailValue.DataSource = obj.FractionValue;
                rptFractionDetailValue.DataBind();
            }
        }

        protected void cbpViewDtTab2Laboratory_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt2Laboratory(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDt2Laboratory(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        public class PartialData
        {
            private int _SequenceNo;

            public int SequenceNo
            {
                get { return _SequenceNo; }
                set { _SequenceNo = value; }
            }

            private string _cfCreatedDate;

            public string cfCreatedDate
            {
                get { return _cfCreatedDate; }
                set { _cfCreatedDate = value; }
            }

            private List<FractionValue> _FractionValue;

            public List<FractionValue> FractionValue
            {
                get { return _FractionValue; }
                set { _FractionValue = value; }
            }
        }

        public partial class FractionValue
        {
            public int SequenceNo;
            public bool IsNormal { get; set; }
            public String Fractionvalue { get; set; }
            public String MetricUnitName { get; set; }
        }
        #endregion

        #region ImagingResult
        #region Header
        private void BindGridViewImaging(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("MRN = {0} AND HealthcareServiceUnitID = {1} AND GCTransactionStatus != '{2}'", hdnMRN.Value, hdnImagingHealthcareServiceUnitID.Value, Constant.TransactionStatus.VOID);

            if (rblItemTypeImagingTab.SelectedValue == "1")
            {
                filterExpression += string.Format(" AND VisitID = {0}", hdnVisitIDPopUpCtl.Value);
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientChargesHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientChargesHd> lstEntity = BusinessLayer.GetvPatientChargesHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "TransactionID DESC");
            grdViewImaging.DataSource = lstEntity;
            grdViewImaging.DataBind();
        }

        protected void cbpViewImaging_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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
        #endregion

        #region Detail
        private void BindGridViewDtImaging(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnImagingID.Value != "")
            {
                filterExpression = string.Format("TransactionID = {0} AND IsDeleted = 0", hdnImagingID.Value);

                if (rblItemTypeImagingTab.SelectedValue == "1")
                {
                    filterExpression += string.Format(" AND VisitID = {0}", hdnVisitIDPopUpCtl.Value);
                }

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvPatientChargesDtImagingResultRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }
            }
            List<vPatientChargesDtImagingResult> lstEntity = BusinessLayer.GetvPatientChargesDtImagingResultList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
            grdViewDtImaging.DataSource = lstEntity;
            grdViewDtImaging.DataBind();
        }

        protected void cbpViewDtImaging_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDtImaging(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDtImaging(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void grdViewDtImaging_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vPatientChargesDtImagingResult entity = e.Row.DataItem as vPatientChargesDtImagingResult;
                HtmlInputButton btnPDF = (HtmlInputButton)e.Row.FindControl("btnViewPDF");
                if (hdnIsRISUsingPDFResult.Value == "0")
                {
                    btnPDF.Attributes.Add("style", "display:none");
                }
            }
        }
        #endregion
        #endregion

        #region Drug
        private void BindGridViewDrug(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string displayMode = cboDisplayDrug.Value.ToString();
            List<PatientMedicationSummary> lstEntity = BusinessLayer.GetPatientMedicationSummaryList(Convert.ToInt32(hdnRegistrationID.Value), displayMode, cboMedicationStatus.Value.ToString());
            lvwViewDrug.DataSource = lstEntity;
            lvwViewDrug.DataBind();
        }

        protected void lvwViewDrug_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                PatientMedicationSummary entity = e.Item.DataItem as PatientMedicationSummary;
                if (entity.IsUsingUDD)
                {
                    HtmlImage imgStatusImageUri = (HtmlImage)e.Item.FindControl("imgStatusImageUri");
                    if (Convert.ToDecimal(entity.cfRemainingQuantity) > 0)
                        imgStatusImageUri.Src = ResolveUrl(string.Format("~/Libs/Images/Status/udd_wip.png"));
                    else
                        imgStatusImageUri.Src = ResolveUrl(string.Format("~/Libs/Images/Status/udd_finish.png"));
                }
            }
        }

        protected void cbpViewDrug_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDrug(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewDrug(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion
    }
}
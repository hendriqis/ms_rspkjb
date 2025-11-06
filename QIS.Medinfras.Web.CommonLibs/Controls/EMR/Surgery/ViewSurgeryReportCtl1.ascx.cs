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
    public partial class ViewSurgeryReportCtl1 : BaseViewPopupCtl
    {
        protected int gridImplantPageCount = 1;
        protected int gridParamedicTeamPageCount = 1;
        protected int gridProcedureGroupPageCount = 1;
        protected int visitID = 0;
        protected int testOrderID = 0;
        protected int reportID = 0;

        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            visitID = Convert.ToInt32(paramInfo[0]);
            testOrderID = Convert.ToInt32(paramInfo[1]);
            reportID = Convert.ToInt32(paramInfo[2]);
            txtTestOrderNo.Text = paramInfo[3];

            hdnVisitID.Value = paramInfo[0];
            hdnTestOrderID.Value = paramInfo[1];
            hdnReportID.Value = paramInfo[2];

            SetControlProperties();

            #region Patient Information
            vConsultVisit4 registeredPatient = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", visitID))[0];
            lblPatientName.InnerHtml = registeredPatient.cfPatientNameSalutation;
            lblGender.InnerHtml = registeredPatient.Gender;
            lblDateOfBirth.InnerHtml = string.Format("{0} ({1})", registeredPatient.cfDateOfBirth, Helper.GetPatientAge(words, registeredPatient.DateOfBirth));

            lblRegistrationDateTime.InnerHtml = string.Format("{0} / {1}", registeredPatient.cfVisitDate, registeredPatient.VisitTime);
            lblRegistrationNo.InnerHtml = registeredPatient.RegistrationNo;
            lblPhysician.InnerHtml = registeredPatient.ParamedicName;

            lblMedicalNo.InnerHtml = registeredPatient.MedicalNo;

            lblPayerInformation.InnerHtml = registeredPatient.BusinessPartnerName;
            lblPatientLocation.InnerHtml = registeredPatient.cfPatientLocation;
            imgPatientImage.Src = registeredPatient.PatientImageUrl; 
            #endregion

            vPatientSurgery obj = BusinessLayer.GetvPatientSurgeryList(string.Format("VisitID = {0} AND PatientSurgeryID = {1} AND IsDeleted = 0", visitID, reportID)).FirstOrDefault();
            if (obj != null)
            {
                #region Data Operasi
                txtTestOrderNo.Text = obj.TestOrderNo;
                txtReportDate.Text = obj.ReportDate.ToString(Constant.FormatString.DATE_FORMAT);
                txtReportTime.Text = obj.ReportTime;
                txtStartDate.Text = obj.StartDate.ToString(Constant.FormatString.DATE_FORMAT);
                txtStartTime.Text = obj.StartTime;
                txtDuration.Text = obj.Duration.ToString();
                txtSurgeryNo.Text = obj.SurgeryNo.ToString();
                rblSurgeryNoType.SelectedValue = obj.IsFirstSurgery ? "1" : "0";
                if (obj.GCAnesthesiaType != null)
                {
                    cboAnesthesiaType.Value = obj.GCAnesthesiaType;
                }
                if (obj.GCWoundType != null)
                {
                    cboWoundType.Value = obj.GCWoundType;
                }
                rblIsUsingProfilaksis.SelectedValue = obj.IsUsingAntibiotics ? "1" : "0";
                txtProfilaxis.Text = obj.AntibioticsType;
                txtProfilaxisTime.Text = obj.AntibioticsTime;
                rblIsHasComplexity.SelectedValue = obj.IsHasComplication ? "1" : "0";
                txtComplexityRemarks.Text = obj.ComplicationRemarks;
                rblIsHemorrhage.SelectedValue = obj.IsHasHemorrhage ? "1" : "0";
                txtHemorrhage.Text = obj.Hemorrhage.ToString();
                rblIsBloodDrain.SelectedValue = obj.IsBloodDrain ? "1" : "0";
                txtOtherBloodDrainType.Text = obj.OtherBloodDrainType;
                rblIsUsingTampon.SelectedValue = obj.IsUsingTampon ? "1" : "0";
                rblIsUsingTourniquet.SelectedValue = obj.IsUsingTourniquet ? "1" : "0";
                txtTamponType.Text = obj.TamponType;
                rblIsBloodTransfussion.SelectedValue = obj.IsBloodTransfussion ? "1" : "0";
                txtBloodTransfussion.Text = obj.BloodTransfussion.ToString();
                rblIsTestKultur.SelectedValue = obj.IsTestKultur ? "1" : "0";
                txtOtherTestKulturType.Text = obj.OtherTestKulturType;
                rblIsTestCytology.SelectedValue = obj.IsTestCytology ? "1" : "0";
                txtOtherTestCytologyType.Text = obj.OtherTestCytologyType;
                rblIsSpecimenTest.SelectedValue = obj.IsSpecimenTest ? "1" : "0";
                if (obj.SpecimenID != 0)
                {
                    cboSpecimen.Value = obj.SpecimenID.ToString();
                }
                #endregion

                #region Diagnosa dan Jenis Operasi
                txtPreDiagnosis.Text = string.Format("{0}", obj.PreOperativeDiagnosisText);
                txtPostDiagnosis.Text = string.Format("{0}", obj.PostOperativeDiagnosisText);
                txtProcedureGroupRemarks.Text = obj.ProcedureGroupRemarks;
                #endregion

                BindGridViewImplant(1, true, ref gridImplantPageCount);
                BindGridViewParamedicTeam(1, true, ref gridParamedicTeamPageCount);
                BindGridViewProcedureGroup(1, true, ref gridProcedureGroupPageCount);

                #region Team Pelaksana
                txtReferralSummary.Text = obj.ReferralSummary;
                #endregion

                #region Uraian Pembedahan
                txtRemarks.Text = obj.PostOperativeDiagnosisRemarks;
                #endregion
            }

            //string filterExp = string.Format("VisitID = {0} AND TestOrderID = {1} AND PreSurgicalAssessmentID = {2} AND IsDeleted = 0 ORDER BY PreSurgicalAssessmentID DESC", visitID, testOrderID, reportID);
            //vPreSurgeryAssessment obj = BusinessLayer.GetvPreSurgeryAssessmentList(filterExp).FirstOrDefault();
            lblPhysicianName2.InnerHtml = obj.ParamedicName;

            hdnMRN.Value = obj.MRN.ToString();

            if (obj != null)
            {
                //#region Chief Complaint and History Of Illness
                //txtDate.Text = obj.AssessmentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                //txtTime.Text = obj.AssessmentTime;
                //txtPhysicianName.Text = obj.ParamedicName;
                //txtAssessmentText.Text = obj.PreSurgeryAssessmentText;
                //txtFamilyHistory.Text = obj.FamilyHistory;

                //txtHPISummary.Text = obj.HPISummary;
                //chkAutoAnamnesis.Checked = obj.IsAutoAnamnesis;
                //chkAlloAnamnesis.Checked = obj.IsAlloAnamnesis;

                //txtMedicalHistory.Text = obj.PastMedicalHistory;
                //txtMedicationHistory.Text = obj.PastMedicationHistory;
                //txtPastSurgicalHistory.Text = obj.PastSurgicalHistory;
                //#endregion

                //#region HTML Form
                //hdnDiagnosticTestLayout.Value = obj.DiagnosticTestChecklistLayout;
                //hdnDiagnosticTestValue.Value = obj.DiagnosticTestChecklistValue;

                //hdnDocumentChecklistLayout.Value = obj.DocumentChecklistLayout;
                //hdnDocumentChecklistValue.Value = obj.DocumentChecklistValue;
                //txtDiagnosticResultSummary.Text = obj.DiagnosticResultSummary;
                //#endregion

                //if (!string.IsNullOrEmpty(obj.PreDiagnoseID))
                //    txtPreDiagnosisID.Text = string.Format("{0} ({1})", obj.PreDiagnoseID, obj.PreDiagnoseText);
                //else
                //    txtPreDiagnosisID.Text = obj.PreDiagnoseText;

                //txtProfilaxis.Text = obj.ProphylaxisSummary;
                //txtPatientPositionSummary.Text = obj.PatientPositionSummary;
                //txtEstimatedDuration.Text = obj.EstimatedDuration.ToString();
                //txtSurgeryItemSummary.Text = obj.SurgeryItemSummary;
                //txtReferralSummary.Text = obj.ReferralSummary;
                //txtOtherSummary.Text = obj.OtherSummary;

                //BindGridViewAllergy(1, true, ref gridAllergyPageCount);
                //BindGridViewVitalSign(1, true, ref gridVitalSignPageCount);
                //BindGridViewROS(1, true, ref gridROSPageCount);
                //BindGridViewProcedureGroup(1, true, ref gridProcedureGroupPageCount);
            }
        }

        private void SetControlProperties()
        {
            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}') AND IsActive = 1 AND IsDeleted = 0",
Constant.StandardCode.ANESTHESIA_TYPE, Constant.StandardCode.JENIS_PEMBEDAHAN, Constant.StandardCode.SURGERY_TEAM_ROLE);

            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(filterExpression);

            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField(cboAnesthesiaType, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.ANESTHESIA_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboWoundType, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.JENIS_PEMBEDAHAN).ToList(), "StandardCodeName", "StandardCodeID");
            //Methods.SetComboBoxField<StandardCode>(cboParamedicType, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.SURGERY_TEAM_ROLE).ToList(), "StandardCodeName", "StandardCodeID");

            List<Specimen> lstSpecimen = BusinessLayer.GetSpecimenList("IsDeleted = 0 ORDER BY SpecimenName");
            Methods.SetComboBoxField(cboSpecimen, lstSpecimen, "SpecimenName", "SpecimenID");
        }

        #region Team
        private void BindGridViewParamedicTeam(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Empty;

            filterExpression += string.Format("VisitID IN ({0}) AND TestOrderID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnTestOrderID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientSurgeryTeamRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_CTL);
            }

            List<vPatientSurgeryTeam> lstEntity = BusinessLayer.GetvPatientSurgeryTeamList(filterExpression, Constant.GridViewPageSize.GRID_CTL, pageIndex, "ID DESC");
            grdParamedicTeamView.DataSource = lstEntity;
            grdParamedicTeamView.DataBind();
        }

        protected void cbpParamedicTeamView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewParamedicTeam(Convert.ToInt32(param[1]), true, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewParamedicTeam(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }
        }
        #endregion

        #region Implant
        private void BindGridViewImplant(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("MRN = {0} AND TestOrderID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.MRN, hdnTestOrderID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientMedicalDeviceRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientMedicalDevice> lstEntity = BusinessLayer.GetvPatientMedicalDeviceList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
            grdImplantView.DataSource = lstEntity;
            grdImplantView.DataBind();

            chkIsUsingImplant.Checked = (lstEntity.Count > 0);
        }

        protected void cbpImplantView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewImplant(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewImplant(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        #region Procedure Group
        private void BindGridViewProcedureGroup(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND TestOrderID = {1} AND IsDeleted = 0", visitID, testOrderID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientSurgeryProcedureGroupRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientSurgeryProcedureGroup> lstEntity = BusinessLayer.GetvPatientSurgeryProcedureGroupList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ProcedureGroupCode");

            grdProcedureGroupView.DataSource = lstEntity;
            grdProcedureGroupView.DataBind();
        }
        protected void cbpProcedureGroupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewProcedureGroup(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewProcedureGroup(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = string.Empty;
        }
        #endregion
    }
}
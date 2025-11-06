using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using System.Web;
using System.IO;
using System.Text;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class ViewSurgeryAssessmentCtl1 : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');

            string _testOrderID = paramInfo[0];

            PatientSurgeryInformation obj = BusinessLayer.GetPatientSurgeryInformation(_testOrderID).FirstOrDefault();
            if (obj !=  null)
            {
                SetEntityToControls(obj);
            }
        }

        private void SetEntityToControls(PatientSurgeryInformation entity)
        {
            #region Patient Information
            lblPatientName.InnerHtml = entity.cfPatientNameSalutation;
            lblMRN.InnerHtml = entity.MedicalNo;
            imgPatientProfilePicture.Src = HttpUtility.HtmlEncode(entity.cfPatientImageUrl);

            if (entity.OldMedicalNo != null)
            {
                lblMRN.InnerHtml = HttpUtility.HtmlEncode(entity.MedicalNo);
                spnOldMedicalNo.Style.Add("display", "block");
                lblOldMRN.InnerHtml = HttpUtility.HtmlEncode("/" + entity.OldMedicalNo);
            }
            else
            {
                lblMRN.InnerHtml = HttpUtility.HtmlEncode(entity.MedicalNo);
                spnOldMedicalNo.Style.Add("display", "none");
                lblOldMRN.InnerHtml = "";
            }

            words = ((BasePage)Page).GetWords();
            if (entity.DateOfBirth.ToString("dd-MM-yyyy") != Constant.ConstantDate.DEFAULT_NULL)
            {
                lblDOB.InnerHtml = entity.cfDateOfBirthInString;
                lblPatientAge.InnerHtml = string.Format("{0} - {1}", Helper.GetPatientAge(words, entity.DateOfBirth), entity.Religion);
            }
            else
            {
                lblDOB.InnerHtml = "";
                lblPatientAge.InnerHtml = string.Format("{0} - {1}", "", entity.Religion);
            }

            lblGender.InnerHtml = HttpUtility.HtmlEncode(entity.Sex);
            lblPatientCategory.InnerHtml = HttpUtility.HtmlEncode(entity.PatientCategory);

            if (entity.GCTriage != "")
                divPatientBannerImgInfo.Style.Add("background-color", entity.TriageColor);
            else
                divPatientBannerImgInfo.Style.Add("display", "none");

            #region Patient Status
            if (entity.IsHasAllergy)
            {
                divPatientStatusAllergy.Style.Add("background-color", "red");
                divPatientStatusAllergy.InnerHtml = HttpUtility.HtmlEncode("A");
            }
            else
            {
                divPatientStatusAllergy.Style.Add("background-color", "white");
                divPatientStatusAllergy.InnerHtml = HttpUtility.HtmlEncode("");
            }
            if (entity.IsFallRisk)
            {
                divPatientStatusFallRisk.Style.Add("background-color", "yellow");
                divPatientStatusFallRisk.InnerHtml = HttpUtility.HtmlEncode("F");
            }
            else
            {
                divPatientStatusFallRisk.Style.Add("background-color", "white");
                divPatientStatusFallRisk.InnerHtml = HttpUtility.HtmlEncode("");
            }
            if (entity.IsDNR)
            {
                divPatientStatusDNR.Style.Add("background-color", "purple");
                divPatientStatusDNR.InnerHtml = HttpUtility.HtmlEncode("D");
            }
            else
            {
                divPatientStatusDNR.Style.Add("background-color", "white");
                divPatientStatusDNR.InnerHtml = HttpUtility.HtmlEncode("");
            }

            if (entity.PatientAllergy.Length > 20)
                lblAllergy.InnerHtml = HttpUtility.HtmlEncode(entity.PatientAllergy.Substring(0, 20).ToUpper()) + "...";
            else
                lblAllergy.InnerHtml = HttpUtility.HtmlEncode(entity.PatientAllergy.ToUpper());

            lblReferralNo.InnerText = "-";
            if (entity.ReferralNo != null && entity.ReferralNo != "")
            {
                lblReferralNo.InnerText = entity.ReferralNo;
            }

            lblPayer.InnerHtml = HttpUtility.HtmlEncode(entity.cfBusinessPartner);
            #endregion
            #endregion

            #region Asesmen Pra Bedah
            txtPreAssesmentDate.Text = entity.PreSurgeryAssesmentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPreAssessmentTime.Text = entity.PreSurgeryAssessmentTime;
            txtPreAssessmentPhysicianName.Text = entity.PreSurgeryParamedicName;
            txtPreAssessmentText.Text = entity.PreSurgeryAssessmentText;

            txtFamilyHistory.Text = entity.PreSurgeryFamilyHistory;

            txtHPISummary.Text = entity.PreSurgeryHPISummary;
            chkAutoAnamnesis.Checked = entity.PreSurgeryIsAutoAnamnesis;
            chkAlloAnamnesis.Checked = entity.PreSurgeryIsAlloAnamnesis;

            txtMedicalHistory.Text = entity.PreSurgeryPastMedicalHistory;
            txtMedicationHistory.Text = entity.PreSurgeryPastMedicationHistory;
            txtPastSurgicalHistory.Text = entity.PreSurgeryPastSurgicalHistory;

            #region HTML Form
            //hdnDiagnosticTestLayout.Value = entity.PreSurgeryDiagnosticTestChecklistLayout;
            //hdnDiagnosticTestValue.Value = entity.PreSurgeryDiagnosticTestChecklistValue;

            //hdnDocumentChecklistLayout.Value = entity.PreSurgeryDocumentChecklistLayout;
            //hdnDocumentChecklistValue.Value = entity.PreSurgeryDocumentChecklistValue;
            //txtDiagnosticResultSummary.Text = entity.PreSurgeryDiagnosticResultSummary;
            #endregion

            if (!string.IsNullOrEmpty(entity.PreSurgeryPreDiagnoseID))
                txtPreDiagnosisID.Text = string.Format("{0} ({1})", entity.PreSurgeryPreDiagnoseID, entity.PreSurgeryPreDiagnoseText);
            else
                txtPreDiagnosisID.Text = entity.PreSurgeryPreDiagnoseText;

            txtProfilaxis.Text = entity.PreSurgeryProphylaxisSummary;
            txtPatientPositionSummary.Text = entity.PreSurgeryPositionSummary;
            txtEstimatedDuration.Text = entity.EstimatedDuration.ToString();
            txtSurgeryItemSummary.Text = entity.PreSurgeryItemSummary;
            txtReferralSummary.Text = entity.PreSurgeryReferralSummary;
            txtOtherSummary.Text = entity.PreSurgeryOtherSummary;
            #endregion
        }
    }
}
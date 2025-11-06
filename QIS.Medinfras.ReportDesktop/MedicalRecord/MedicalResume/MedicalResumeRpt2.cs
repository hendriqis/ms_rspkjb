using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class MedicalResumeRpt2 : BaseCustomDailyPotraitRpt
    {
        public MedicalResumeRpt2()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vConsultVisit9 cv = BusinessLayer.GetvConsultVisit9List(string.Format("VisitID = {0}", param[0])).FirstOrDefault();
            vMedicalResume obj = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID = {0} AND IsDeleted = 0", param[0])).FirstOrDefault();
            vHealthcare healthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID)).FirstOrDefault();
            List<vPatientReferral> lstReferral = BusinessLayer.GetvPatientReferralList(string.Format("VisitID = {0} AND IsDeleted = 0", param[0]));

            lblPatientName.Text = cv.PatientName;
            lblDOB.Text = cv.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT);
            lblParticipantNo.Text = cv.CorporateAccountNo;
            lblBusinessPartner.Text = cv.BusinessPartnerName;
            lblMedicalNo.Text = cv.MedicalNo;


            if (obj != null)
            {
                lblSubjectiveSummaryText.Text = obj.SubjectiveResumeText;
                lblObjectiveSummaryText.Text = obj.ObjectiveResumeText;
                lblAssessmentSummaryText.Text = obj.AssessmentResumeText;
                lblPlanningSummaryText.Text = obj.PlanningResumeText;
                lblMedicationSummaryText.Text = obj.MedicationResumeText;
                if (obj.MedicalResumeText == null || obj.MedicalResumeText == "")
                {
                    lblMedicalSummaryText.Text = obj.SurgeryResumeText;
                }
                else
                {
                    lblMedicalSummaryText.Text = obj.MedicalResumeText;
                }
                lblInstructionSummaryText.Text = obj.InstructionResumeText;

                lblSignatureTimeStamp1.Text = string.Format("{0},{1}", healthcare.City, obj.MedicalResumeDate.ToString(Constant.FormatString.DATE_FORMAT));
                lblMedicalResumeDate.Text = string.Format("{0},{1}", healthcare.City, obj.MedicalResumeDate.ToString(Constant.FormatString.DATE_FORMAT));
                lblPhysicianName.Text = obj.ParamedicName;
                ttdDokter.ImageUrl = string.Format("{0}{1}/Signature/{2}.png", AppConfigManager.QISPhysicalDirectory, AppConfigManager.QISParamedicImagePath, obj.ParamedicCode);
                ttdDokter.Visible = true;

                string refferalInfo = string.Empty;
                if (lstReferral.Count > 0)
                {
                    foreach (vPatientReferral reff in lstReferral)
                    {
                        if (!String.IsNullOrEmpty(refferalInfo))
                            refferalInfo += ",";
                        refferalInfo += string.Format("{0} ({1})", reff.ToPhysicianName, reff.ToServiceUnitName);
                    }
                    chkDischargeRoutine2.Checked = true;
                    lblDischargeRoutine.Text = refferalInfo;
                }
                else
                {
                    switch (cv.GCDischargeMethod)
                    {
                        case Constant.DischargeMethod.REFFERRED_TO_OUTPATIENT:
                            chkDischargeRoutine2.Checked = true;
                            lblDischargeRoutine.Text = refferalInfo;
                            break;
                        case Constant.DischargeMethod.DISCHARGED_TO_WARD:
                            chkDischargeRoutine3.Checked = true;
                            break;
                        default:
                            chkDischargeRoutine1.Checked = true;
                            break;
                    }
                }

                chkSickLetterYes.Checked = obj.IsHasSickLetter;

                if (obj.IsHasSickLetter)
                    lblNoOfDays.Text = obj.NoOfAbsenceDays.ToString("G29");
                else
                    lblNoOfDays.Text = string.Empty;

            }
            else
            {
                lblSubjectiveSummaryText.Text = "";
                lblObjectiveSummaryText.Text = "";
                lblAssessmentSummaryText.Text = "";
                lblPlanningSummaryText.Text = "";
                lblMedicationSummaryText.Text = "";
                lblMedicalSummaryText.Text = "";
                lblInstructionSummaryText.Text = "";

                lblSignatureTimeStamp1.Text = "";
                lblMedicalResumeDate.Text = "";
                lblPhysicianName.Text = "";

                lblDischargeRoutine.Text = "";
                chkSickLetterYes.Checked = false;
                chkSickLetterNo.Checked = false;

                lblNoOfDays.Text = string.Empty;
            }

            base.InitializeReport(param);
        }

    }
}

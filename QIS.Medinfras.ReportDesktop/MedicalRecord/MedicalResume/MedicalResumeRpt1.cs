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
    public partial class MedicalResumeRpt1 : BaseCustomDailyPotraitRpt
    {
        public MedicalResumeRpt1()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vConsultVisit1 cv = BusinessLayer.GetvConsultVisit1List(string.Format("VisitID = {0}", param[0])).FirstOrDefault();
            vMedicalResume obj = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID = {0} AND IsDeleted = 0 AND IsRevised = 0", param[0])).FirstOrDefault();
            vHealthcare healthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID)).FirstOrDefault();

            lblPatientName.Text = cv.PatientName;
            lblDOB.Text = cv.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT);
            lblPatientAge.Text = cv.PatientAge;
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

                lblMedicalResumeDate.Text = string.Format("{0}, {1}", healthcare.City, obj.MedicalResumeDate.ToString(Constant.FormatString.DATE_FORMAT));
                ParamedicMaster entityPM = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", obj.ParamedicID)).FirstOrDefault();
                ttdDokter.ImageUrl = string.Format("{0}{1}/Signature/{2}.png", AppConfigManager.QISPhysicalDirectory, AppConfigManager.QISParamedicImagePath, entityPM.ParamedicCode);
                ttdDokter.Visible = true;
                lblPhysicianName.Text = obj.ParamedicName;
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

                lblMedicalResumeDate.Text = "";
                lblPhysicianName.Text = "";
            }

            base.InitializeReport(param);
        }

    }
}

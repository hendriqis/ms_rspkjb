using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Linq;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BPJSResumeRpt1 : BaseCustomDailyPotraitRpt
    {
        public BPJSResumeRpt1()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vConsultVisit1 cv = BusinessLayer.GetvConsultVisit1List(string.Format("RegistrationID = {0}", param[0])).FirstOrDefault();
            vRegistrationBPJS obj = BusinessLayer.GetvRegistrationBPJSList(string.Format("RegistrationID = {0}", param[0])).FirstOrDefault();
            vHealthcare healthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID)).FirstOrDefault();

            lblPatientName.Text = cv.PatientName;
            lblDOB.Text = cv.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT);
            lblPatientAge.Text = cv.PatientAge;
            lblMedicalNo.Text = cv.MedicalNo;

            if (obj != null)
            {
                lblAssessmentSummaryText.Text = obj.AssessmentSummaryText;
                lblPlanningSummaryText.Text = obj.PlanningResumeText;
                chkIsNotReferBack.Checked = obj.IsReferBack;
                chkIsNotReferBack.Checked = !obj.IsReferBack;
                chkIsNotReferBackDueToMedication.Checked = obj.IsNotReferBackDueToMedication;
                chkNotReferBackDueToTheraphy.Checked = obj.IsNotReferBackDueToTheraphy;
                chkNotReferBackDueToOthers.Checked = obj.IsNotReferBackDueToOthers;
                lblNotReferBackOtherRemarks.Text = obj.NotReferBackOtherRemarks;

                chkIsPlanForMedication.Checked = obj.IsPlanForMedication;
                chkIsPlanForTheraphy.Checked = obj.IsPlanForTheraphy;
                chkIsPlanForOthers.Checked = obj.IsPlanForOthers;
                lblPlanForOtherRemarks.Text = obj.PlanOtherRemarks;

                if (obj.FollowupVisitDate != null && obj.FollowupVisitDate.ToString(Constant.FormatString.DATE_FORMAT) != "01-Jan-1900")
                {
                    lblFollowupDate.Text = obj.FollowupVisitDate.ToString(Constant.FormatString.DATE_FORMAT);
                }
                else
                {
                    lblFollowupDate.Text = "";
                }

                lblMedicalResumeDate.Text = string.Format("{0}, {1}", healthcare.City, cv.ActualVisitDate.ToString(Constant.FormatString.DATE_FORMAT));
                lblDocumentDate.Text = lblMedicalResumeDate.Text;

                lblPhysicianName.Text = obj.ParamedicName;
            }
            else
            {
                lblAssessmentSummaryText.Text = "";
                lblPlanningSummaryText.Text = "";
                chkIsNotReferBack.Checked = false;
                chkIsNotReferBack.Checked = false;
                chkIsNotReferBackDueToMedication.Checked = false;
                chkNotReferBackDueToTheraphy.Checked = false;
                chkNotReferBackDueToOthers.Checked = false;
                lblNotReferBackOtherRemarks.Text = "";

                chkIsPlanForMedication.Checked = false;
                chkIsPlanForTheraphy.Checked = false;
                chkIsPlanForOthers.Checked = false;
                lblPlanForOtherRemarks.Text = "";

                lblFollowupDate.Text = "";

                lblMedicalResumeDate.Text = string.Format("{0}, {1}", healthcare.City, cv.ActualVisitDate.ToString(Constant.FormatString.DATE_FORMAT));
                lblDocumentDate.Text = lblMedicalResumeDate.Text;

                lblPhysicianName.Text = "";
            }

            

            base.InitializeReport(param);
        }

    }
}

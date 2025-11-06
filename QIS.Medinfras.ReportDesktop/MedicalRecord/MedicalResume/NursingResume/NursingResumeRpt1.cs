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
    public partial class NursingResumeRpt1 : BaseRpt
    {
        public NursingResumeRpt1()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vConsultVisit4 cv = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", param[0])).FirstOrDefault();
            vNurseMedicalResume obj = BusinessLayer.GetvNurseMedicalResumeList(string.Format("VisitID = {0} AND IsDeleted = 0", param[0])).FirstOrDefault();
            vHealthcare healthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID)).FirstOrDefault();


            lblPatientName.Text = cv.PatientName;
            lblDOB.Text = cv.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT);
            lblPatientAge.Text = cv.cfPatientAge;
            lblMedicalNo.Text = cv.MedicalNo;

            if (obj != null)
            {
                lblIsGeriatric.Text = obj.IsGeriatric ? "YA" : "TIDAK";
                lblIsImmobility.Text = obj.IsImmobility ? "YA" : "TIDAK";
                lblIsNeedFollowupCare.Text = obj.IsNeedFollowupCare ? "YA" : "TIDAK";
                lblIsHasDepedency.Text = obj.IsHasDependency ? "YA" : "TIDAK";

                lblSubjectiveSummaryText.Text = obj.SubjectiveResumeText;
                lblPlanningSummaryText.Text = obj.PlanningResumeText;
                lblEvaluationSummaryText.Text = obj.EvaluationResumeText;
                lblMedicalSummaryText.Text = obj.DischargeMedicalResumeText;
                lblMedicationSummaryText.Text = obj.DischargeMedicationResumeText;
                lblNutritionistSummaryText.Text = obj.NutritionistResumeText;

                lblDischargeTransportation.Text = obj.DischargeTransportation;
                lblHomecarePIC.Text = obj.HomecarePIC;

                string[] instructionInfo = obj.InstructionResumeText.Split('|');
                lblInstructionSummaryText1.Text = instructionInfo[0];
                lblInstructionSummaryText2.Text = instructionInfo[1];
                lblEducationSummaryText.Text = obj.EducationSummaryText;

                lblFamillyName.Text = string.Format("{0} ({1})", obj.FamilyName, obj.FamilyRelation);

                lblMedicalResumeDate.Text = string.Format("{0},{1}", healthcare.City, obj.MedicalResumeDate.ToString(Constant.FormatString.DATE_FORMAT));
                lblPhysicianName.Text = obj.ParamedicName;
            }
            else
            {
                lblDischargeTransportation.Text = "";
                lblHomecarePIC.Text = "";

                lblSubjectiveSummaryText.Text = "";
                lblPlanningSummaryText.Text = "";
                lblNutritionistSummaryText.Text = "";
                lblEvaluationSummaryText.Text = "";
                lblMedicationSummaryText.Text = "";
                lblMedicalSummaryText.Text = "";
                lblInstructionSummaryText1.Text = "";
                lblInstructionSummaryText2.Text = "";
                lblEducationSummaryText.Text = "";

                lblFamillyName.Text = "";

                lblMedicalResumeDate.Text = "";
                lblPhysicianName.Text = "";
            }

            if (healthcare != null)
            {
                xrLogo.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/logo.png");
                cHealthcareName.Text = healthcare.HealthcareName;
                cHealthcareAddress.Text = healthcare.StreetName;
                cHealthcareCityZipCodes.Text = string.Format("{0} {1}", healthcare.City, healthcare.ZipCode);
                cHealthcarePhone.Text = healthcare.PhoneNo1;
                cHealthcareFax.Text = healthcare.FaxNo1;
                //cHealthcareEmail.Text = oHealthcare.Email;
            }

            lblReportTitle.Text = reportMaster.ReportTitle1;
            lblReportSubTitle.Text = reportMaster.ReportTitle2;

            base.InitializeReport(param);
        }

    }
}

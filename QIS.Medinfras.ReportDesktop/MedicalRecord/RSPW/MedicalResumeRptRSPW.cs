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
    public partial class MedicalResumeRptRSPW : BaseCustomDailyPotraitRpt
    {
        public MedicalResumeRptRSPW()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vConsultVisit1 cv = BusinessLayer.GetvConsultVisit1List(string.Format("VisitID = {0}", param[0])).FirstOrDefault();
            vMedicalResume obj = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID = {0} AND IsDeleted = 0", param[0])).FirstOrDefault();
            vHealthcare healthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID)).FirstOrDefault();
            RegistrationBPJS entitybpjs = BusinessLayer.GetRegistrationBPJSList(string.Format("RegistrationID = {0}", cv.RegistrationID)).FirstOrDefault();
            Patient entityPatient = BusinessLayer.GetPatientList(string.Format("MRN = {0}", cv.MRN)).FirstOrDefault();

            cPatientName.Text = cv.PatientName;
            cDOB.Text = cv.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT);
            cPatientAge.Text = cv.PatientAge;
            cMedicalNo.Text = cv.MedicalNo;
            cVisitDate.Text = cv.cfVisitDateInString;
            cServiceUnit.Text = cv.ServiceUnitName;

            #region Body Diagram
            if (obj != null)
            {
                List<vPatientBodyDiagramHd> lstBd = BusinessLayer.GetvPatientBodyDiagramHdList(string.Format("VisitID = {0} AND IsDeleted = 0", cv.VisitID));

                if (lstBd.Count() > 0)
                {
                    subBodyDiagram.CanGrow = true;
                    mrBodyDiagramRptRSPW.InitializeReport(cv.VisitID);
                }
                else
                {
                    subBodyDiagram.Visible = false;
                }
            }
            else
            {
                subBodyDiagram.Visible = false;
            }
            #endregion

            if (entitybpjs != null)
            {
                cKartuBPJS.Text = entityPatient.NHSRegistrationNo;
                cNoSEP.Text = entitybpjs.NoSEP;
            }
            else
            {
                xrTableCell27.Visible = false;
                xrTableCell28.Visible = false;
                xrTableCell30.Visible = false;
                xrTableCell31.Visible = false;
                cKartuBPJS.Visible = false;
                cNoSEP.Visible = false;
            }

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

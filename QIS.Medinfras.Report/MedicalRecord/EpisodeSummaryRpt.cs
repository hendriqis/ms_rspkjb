using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Text;

namespace QIS.Medinfras.Report
{
    public partial class EpisodeSummaryRpt : QIS.Medinfras.Report.BaseRpt
    {
        public EpisodeSummaryRpt()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            lblReportProperties.Text = string.Format("MEDINFRAS - {0}, Print Date/Time:{1}, User ID:{2}", reportMaster.ReportCode, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"), "admin");

            vConsultVisit entity = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", param[0]))[0];
            lblRegistrationNo.Text = string.Format("Registration No : {0}", entity.RegistrationNo);
            lblRegistrationDateTime.Text = string.Format("{0} / {1}", entity.VisitDate.ToString(Constant.FormatString.DATE_FORMAT), entity.VisitTime);
            lblLOS.Text = entity.LOS;
            lblServiceUnitName.Text = entity.ServiceUnitName;
            lblVisitType.Text = entity.VisitTypeName;

            tdRegistrationDateTime.Text = string.Format("{0} / {1}", entity.VisitDate.ToString(Constant.FormatString.DATE_FORMAT), entity.VisitTime);
            tdPatientAge.Text = string.Format("{0} / {1}", entity.Gender, Helper.GetPatientAge(words, entity.DateOfBirth));
            lblRegistrationNoCover.Text = entity.RegistrationNo;
            lblMRN.Text = string.Format("MRN : {0}", entity.MedicalNo);
            imgPatientPicture.ImageUrl = entity.PatientImageUrl;
            lblPatientName.Text = entity.PatientName;
            lblPhysicianName.Text = entity.ParamedicName;
            List<vPatientDiagnosis> lstPatientDiagnosis = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", entity.VisitID));
            StringBuilder diagnosis = new StringBuilder();
            foreach (vPatientDiagnosis patientDiagnosis in lstPatientDiagnosis)
            {
                if (diagnosis.ToString() != "")
                    diagnosis.Append(", ");
                diagnosis.Append(patientDiagnosis.DiagnosisText);
            }
            lblDiagnose.Text = diagnosis.ToString();

            sbrChiefComplaint.CanGrow = true;
            episodeSummaryChiefComplaintRpt1.InitializeReport(entity.VisitID);

            cbrAllergy.CanGrow = true;
            episodeSummaryAllergyRpt1.InitializeReport(entity.MRN);

            cbrVitalSign.CanGrow = true;
            episodeSummaryVitalSignRpt1.InitializeReport(entity.VisitID);

            cbrReviewOfSystem.CanGrow = true;
            episodeSummaryReviewOfSystemRpt1.InitializeReport(entity.VisitID);

            sbrBodyDiagram.CanGrow = true;
            episodeSummaryBodyDiagramRpt1.InitializeReport(entity.VisitID);
        }
    }
}

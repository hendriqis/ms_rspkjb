using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using ThoughtWorks.QRCode.Codec;


namespace QIS.Medinfras.ReportDesktop
{
    public partial class BLembarKonsultasi : BaseA6Rpt
    {
        public BLembarKonsultasi()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vConsultVisit entity = BusinessLayer.GetvConsultVisitList(param[0])[0];
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];

            if (entityHealthcare != null)
            {
                xrLogo.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/logo.png");
                xrLogo.WidthF = 180;
                xrLogo.HeightF = 180;
            }

            lblReferralPhysician.Text = param[1];
            lblPatientName.Text = entity.PatientName;
            lblPatientAge.Text = entity.PatientAge;
            lblGender.Text = entity.cfGenderInitial;
            lblMedicalNo.Text = entity.MedicalNo;
            lblPatientAddress.Text = entity.HomeAddress;

            List<vPatientDiagnosis> entityDiag = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", entity.VisitID));
            if (entityDiag == null)
            {
                lblDiagnose.Text = "";
            }
            else
            {
                List<vPatientDiagnosis> lstPatientDiagnosis = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", entity.VisitID));
                StringBuilder diagNotes = new StringBuilder();
                foreach (vPatientDiagnosis patientDiagnosis in lstPatientDiagnosis)
                {
                    if (diagNotes.ToString() != "")
                        diagNotes.Append(", ");
                    diagNotes.Append(patientDiagnosis.DiagnosisText);
                }
                lblDiagnose.Text = diagNotes.ToString();
            }

            List<PatientReferral> entityRefList = BusinessLayer.GetPatientReferralList(string.Format("VisitID = {0} AND IsDeleted = 0", entity.VisitID));
            if (entityRefList.Count() == 0)
            {
                lblReplyS.Text = "";
                lblReplyO.Text = "";
                lblReplyA.Text = "";
                lblReplyP.Text = "";
                lblReplyI.Text = "";
                lblJam.Text = "";
            }
            else 
            {
                PatientReferral entityRef = entityRefList.FirstOrDefault();
                lblReplyS.Text = entityRef.ReplySubjectiveText;
                lblReplyO.Text = string.Format("{0}, {1}", entityRef.ReplyObjectiveText, entityRef.ReplyMedicalResumeText);
                lblReplyA.Text = entityRef.ReplyDiagnosisText;
                lblReplyP.Text = entityRef.ReplyPlanningResumeText;
                lblReplyI.Text = entityRef.ReplyInstructionResumeText;
                lblJam.Text = string.Format("{0}, {1}", entityRef.ReferralDateinString, entityRef.ReferralTime);
            }

            lblTherapy.Text = param[2];

            lblPrintDate.Text = string.Format("{0}, {1}", entityHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));
            lblPhysician.Text = entity.ParamedicName;

            base.InitializeReport(param);
        }

    }
}

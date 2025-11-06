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
    public partial class BSuratPengantarRawatInapRSPSB : BaseCustomDailyPotraitRpt
    {
        public BSuratPengantarRawatInapRSPSB()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", param[0])).FirstOrDefault();
            vConsultVisit entityCV = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0}", param[0])).FirstOrDefault();

            lblPrintDate.Text = string.Format("{0}, {1}", entityHealthcare.City, entityCV.ActualVisitDateInString);

            if (entityCV.ReferralPhysicianID == null || entityCV.ReferralPhysicianID == 0)
            {
                lblDokterDPJP.Text = "";
            }
            else
            {
                ParamedicMaster entityPM = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", entityCV.ReferralPhysicianID)).FirstOrDefault();
                lblDokterDPJP.Text = entityPM.FullName;
            }

            if (!String.IsNullOrEmpty(entity.DiagnoseID))
            {
                lblDianosis.Text = string.Format("{0}", entity.DiagnoseName);
            }
            else if (!String.IsNullOrEmpty(entity.DiagnoseName))
            {
                lblDianosis.Text = string.Format("{0}", entity.DiagnosisText);
            }
            else
            {
                List<vPatientDiagnosis> entityDiag = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));
                if (entityDiag == null)
                {
                    lblDianosis.Text = "-";
                }
                else
                {
                    List<vPatientDiagnosis> lstPatientDiagnosis = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));
                    StringBuilder diagNotes = new StringBuilder();
                    foreach (vPatientDiagnosis patientDiagnosis in lstPatientDiagnosis)
                    {
                        if (diagNotes.ToString() != "")
                            diagNotes.Append(", ");
                        diagNotes.Append(patientDiagnosis.DiagnosisText);
                    }
                    lblDianosis.Text = diagNotes.ToString();
                }
            }

            lblParamedicVisit.Text = string.Format("({0})", entity.ParamedicName);
            
            base.InitializeReport(param);
        }

      
    }
}

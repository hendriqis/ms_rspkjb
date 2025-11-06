using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class MRTreatmentNewRSRT : DevExpress.XtraReports.UI.XtraReport
    {
        public MRTreatmentNewRSRT()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID, int MedicalResumeID)
        {
            vMedicalResume entityMR = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID IN ({0}) AND IsDeleted = 0", VisitID)).FirstOrDefault();
            if (entityMR.MedicalResumeText != null && entityMR.MedicalResumeText != "")
            {
                xrLabel7.Text = string.Format("Perkembangan Selama Perawatan");
                xrLabel8.Text = string.Format(":");
                lblMedicalResume.Text = string.Format("{0}", entityMR.MedicalResumeText);
            }
            else
            {
                xrLabel7.Visible = false;
                xrLabel8.Visible = false;
                lblMedicalResume.Visible = false;
            }
        }
    }
}

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
    public partial class MRTreatmentRSSES : DevExpress.XtraReports.UI.XtraReport
    {
        public MRTreatmentRSSES()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID, int MedicalResumeID)
        {
            vMedicalResume entityMR = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID IN ({0}) AND IsDeleted = 0 AND ISNULL(GCMedicalResumeStatus,'') NOT IN ('{1}')", VisitID, Constant.MedicalResumeStatus.REVISED)).FirstOrDefault();
            if (entityMR.MedicalResumeText != null && entityMR.MedicalResumeText != "")
            {
                xrLabel1.Text = string.Format("Perkembangan Dalam Perawatan");
                xrLabel2.Text = string.Format(":");
                lblTindakan.Text = string.Format("{0}", entityMR.MedicalResumeText);
            }
            else
            {
                xrLabel1.Visible = false;
                xrLabel2.Visible = false;
                lblTindakan.Visible = false;
            }
        }
    }
}

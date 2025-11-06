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
    public partial class MRSurgeryNewRSSES : DevExpress.XtraReports.UI.XtraReport
    {
        public MRSurgeryNewRSSES()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID, int MedicalResumeID)
        {
            vMedicalResume entityMR = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID IN ({0}) AND IsDeleted = 0 AND ISNULL(GCMedicalResumeStatus,'') NOT IN ('{1}')", VisitID, Constant.MedicalResumeStatus.REVISED)).FirstOrDefault();
            if (entityMR.SurgeryResumeText != null && entityMR.SurgeryResumeText != "")
            {
                xrLabel1.Text = string.Format("Prosedur Terapi dan Tindakan");
                xrLabel2.Text = string.Format(":");
                lblTindakan.Text = string.Format("{0}", entityMR.SurgeryResumeText);
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

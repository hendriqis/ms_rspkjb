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
    public partial class MRDiagnosticExamNewMRRSSES : DevExpress.XtraReports.UI.XtraReport
    {
        public MRDiagnosticExamNewMRRSSES()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID, int MedicalResumeID)
        {
            vMedicalResume entityMR = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID IN ({0}) AND IsDeleted = 0 AND ISNULL(GCMedicalResumeStatus,'') NOT IN ('{1}') ORDER BY ID DESC", VisitID, Constant.MedicalResumeStatus.REVISED)).FirstOrDefault();
            if (entityMR.PlanningResumeText != null && entityMR.PlanningResumeText != "")
            {
                xrLabel7.Text = string.Format("Catatan Pemeriksaan Penunjang :");
                lblPlanningResume.Text = string.Format("{0}", entityMR.PlanningResumeText);
            }
            else
            {
                xrLabel7.Visible = false;
                lblPlanningResume.Visible = false;
            }
        }
    }
}

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
    public partial class MRDiagnosticExamNewMR : DevExpress.XtraReports.UI.XtraReport
    {
        public MRDiagnosticExamNewMR()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID, int MedicalResumeID)
        {
            vMedicalResume entityMR = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID IN ({0}) AND IsDeleted = 0", VisitID)).FirstOrDefault();
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

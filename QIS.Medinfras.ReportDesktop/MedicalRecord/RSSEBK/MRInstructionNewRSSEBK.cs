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
    public partial class MRInstructionNewRSSEBK : DevExpress.XtraReports.UI.XtraReport
    {
        public MRInstructionNewRSSEBK()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID, int MedicalResumeID)
        {
            vMedicalResume entityMR = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID IN ({0}) AND IsDeleted = 0", VisitID)).FirstOrDefault();
            if (entityMR.InstructionResumeText != null && entityMR.InstructionResumeText != "")
            {
                lblInstruction.Text = string.Format("- {0}", entityMR.InstructionResumeText);
            }
            else
            {
                lblInstruction.Visible = false;
            }
        }
    }
}

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
    public partial class MRInstructionNewRSSES : DevExpress.XtraReports.UI.XtraReport
    {
        public MRInstructionNewRSSES()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID, int MedicalResumeID)
        {
            vConsultVisit1 entityCV = BusinessLayer.GetvConsultVisit1List(string.Format("VisitID IN ({0})", VisitID)).FirstOrDefault();
            vMedicalResume entityMR = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID IN ({0}) AND IsDeleted = 0 AND ISNULL(GCMedicalResumeStatus,'') NOT IN ('{1}')", VisitID, Constant.MedicalResumeStatus.REVISED)).FirstOrDefault();
            if (entityCV.PlanFollowUpVisitDate.ToString(Constant.FormatString.DATE_FORMAT) != "01-Jan-1900")
            {
                if (entityMR.InstructionResumeText != null && entityMR.InstructionResumeText != "")
                {
                    lblLabel1.Text = string.Format("{0}{1}{2}",
                                "Rencana Kontrol Kembali",
                                Environment.NewLine,
                                "Instruksi dan Rencana Tindak Lanjut");
                    lblLabel2.Text = string.Format("{0}{1}{2}",
                                ":",
                                Environment.NewLine,
                                ":");
                    lblInstruction.Text = string.Format("{0}{1}{2}",
                                entityCV.PlanFollowUpVisitDateInString,
                                Environment.NewLine,
                                entityMR.InstructionResumeText);
                }
                else
                {
                    lblLabel1.Text = string.Format("{0}",
                                "Rencana Kontrol Kembali");
                    lblLabel2.Text = string.Format("{0}",
                                ":");
                    lblInstruction.Text = string.Format("{0}",
                                entityCV.PlanFollowUpVisitDateInString);
                }
            }
            else
            {
                if (entityMR.InstructionResumeText != null && entityMR.InstructionResumeText != "")
                {
                    lblLabel1.Text = string.Format("{0}",
                                "Instruksi dan Rencana Tindak Lanjut");
                    lblLabel2.Text = string.Format("{0}",
                                ":");
                    lblInstruction.Text = string.Format("{0}",
                                entityMR.InstructionResumeText);
                }
                else
                {
                    lblLabel1.Visible = false;
                    lblLabel2.Visible = false;
                    lblInstruction.Visible = false;
                }
            }
        }
    }
}

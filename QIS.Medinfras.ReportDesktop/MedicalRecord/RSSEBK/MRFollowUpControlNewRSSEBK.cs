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
    public partial class MRFollowUpControlNewRSSEBK : DevExpress.XtraReports.UI.XtraReport
    {
        public MRFollowUpControlNewRSSEBK()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID, int MedicalResumeID)
        {
            vConsultVisit1 entityCV = BusinessLayer.GetvConsultVisit1List(string.Format("VisitID IN ({0})", VisitID)).FirstOrDefault();
            vMedicalResume entityMR = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID IN ({0}) AND IsDeleted = 0", VisitID)).FirstOrDefault();
            if (entityCV.PlanFollowUpVisitDate.ToString(Constant.FormatString.DATE_FORMAT) != "01-Jan-1900")
            {
                if (entityMR.InstructionResumeText != null && entityMR.InstructionResumeText != "")
                {
                    lblLabel1.Text = string.Format("{0}", "Kontrol Kembali");
                    lblLabel2.Text = string.Format("{0}", ":");
                    lblInstruction.Text = string.Format("{0}", entityCV.PlanFollowUpVisitDateInString);
                }
                else
                {
                    lblLabel1.Text = string.Format("{0}", "Kontrol Kembali");
                    lblLabel2.Text = string.Format("{0}", ":");
                    lblInstruction.Text = string.Format("{0}", entityCV.PlanFollowUpVisitDateInString);
                }
            }
            else
            {
                if (entityMR.InstructionResumeText != null && entityMR.InstructionResumeText != "")
                {
                    lblLabel1.Text = string.Format("{0}", "Kontrol Kembali");
                    lblLabel2.Text = string.Format("{0}", ":");
                    lblInstruction.Text = string.Format("{0}", entityCV.PlanFollowUpVisitDateInString);
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

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
    public partial class MRSummaryOPRSPKSB : DevExpress.XtraReports.UI.XtraReport
    {
        public MRSummaryOPRSPKSB()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID, int MedicalResumeID)
        {
            vConsultVisit1 entityCV = BusinessLayer.GetvConsultVisit1List(string.Format("VisitID IN ({0})", VisitID)).FirstOrDefault();
            vMedicalResume entityMR = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID IN ({0}) AND IsDeleted = 0 AND ISNULL(GCMedicalResumeStatus,'') NOT IN ('{1}')", VisitID, Constant.MedicalResumeStatus.REVISED)).FirstOrDefault();
            if (entityCV.DepartmentID == Constant.Facility.INPATIENT)
            {

                if (entityMR.SubjectiveResumeText != null && entityMR.SubjectiveResumeText != "")
                {
                    lblAnamnesa.Text = string.Format("{0}", entityMR.SubjectiveResumeText);
                }
                else
                {
                    lblAnamnesa.Visible = false;
                }

                if (entityMR.ComorbiditiesText != null && entityMR.ComorbiditiesText != "")
                {
                    lblKomorbiditas.Text = string.Format("{0}", entityMR.ComorbiditiesText);
                }
                else
                {
                    lblKomorbiditas.Visible = false;
                }
            }
            else
            {
                if (entityMR.SubjectiveResumeText != null && entityMR.SubjectiveResumeText != "")
                {
                    lblAnamnesa.Text = string.Format("{0}", entityMR.SubjectiveResumeText);
                }
                else
                {
                    lblAnamnesa.Visible = false;
                }

                if (entityMR.ComorbiditiesText != null && entityMR.ComorbiditiesText != "")
                {
                    lblKomorbiditas.Text = string.Format("{0}", entityMR.ComorbiditiesText);
                }
                else
                {
                    lblKomorbiditas.Visible = false;
                }
            }
        }
    }
}

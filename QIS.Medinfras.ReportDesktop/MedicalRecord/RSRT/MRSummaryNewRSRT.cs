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
    public partial class MRSummaryNewRSRT : DevExpress.XtraReports.UI.XtraReport
    {
        public MRSummaryNewRSRT()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID, int MedicalResumeID)
        {
            vConsultVisit1 entityCV = BusinessLayer.GetvConsultVisit1List(string.Format("VisitID IN ({0})", VisitID)).FirstOrDefault();
            vMedicalResume entityMR = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID IN ({0}) AND IsDeleted = 0", VisitID)).FirstOrDefault();

            lblIndikasi.Text = string.Format("{0}", entityCV.HospitalizationIndication);
            lblAnamnesa.Text = string.Format("{0}", entityMR.SubjectiveResumeText);
            lblKomorbiditas.Text = string.Format("{0}", entityMR.ComorbiditiesText);
        }
    }
}

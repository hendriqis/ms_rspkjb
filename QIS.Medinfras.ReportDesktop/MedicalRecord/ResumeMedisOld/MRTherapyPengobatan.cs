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
    public partial class MRTherapyPengobatan : DevExpress.XtraReports.UI.XtraReport
    {
        public MRTherapyPengobatan()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID, int MedicalResumeID)
        {
            vMedicalResume entityMR = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID IN ({0}) AND IsDeleted = 0", VisitID)).FirstOrDefault();

            lblRingkasan.Text = string.Format("{0}", entityMR.MedicationResumeText);
            lblTerapiObat.Text = string.Format("{0}", entityMR.DischargeMedicationResumeText);
        }
    }
}

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
    public partial class MRTherapyPengobatanNewRSRT : DevExpress.XtraReports.UI.XtraReport
    {
        public MRTherapyPengobatanNewRSRT()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID, int MedicalResumeID)
        {
            vMedicalResume entityMR = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID IN ({0}) AND IsDeleted = 0", VisitID)).FirstOrDefault();
            if (entityMR.MedicationResumeText != null && entityMR.MedicationResumeText != "")
            {
                xrLabel7.Text = string.Format("Terapi Selama Perawatan");
                xrLabel1.Text = string.Format(":");
                lblRingkasan.Text = string.Format("{0}", entityMR.MedicationResumeText);
            }
            else
            {
                xrLabel7.Visible = false;
                xrLabel1.Visible = false;
                lblRingkasan.Visible = false;
            }
            if (entityMR.DischargeMedicationResumeText != null && entityMR.DischargeMedicationResumeText != "")
            {
                xrLabel2.Text = string.Format("Terapi Obat Pulang");
                xrLabel3.Text = string.Format(":");
                lblTerapiObat.Text = string.Format("{0}", entityMR.DischargeMedicationResumeText);
            }
            else
            {
                xrLabel2.Visible = false;
                xrLabel3.Visible = false;
                lblTerapiObat.Visible = false;
            }
        }
    }
}

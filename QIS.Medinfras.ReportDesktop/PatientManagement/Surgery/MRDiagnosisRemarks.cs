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
    public partial class MRDiagnosisRemarks : DevExpress.XtraReports.UI.XtraReport
    {
        public MRDiagnosisRemarks()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID, int PatientSurgeryID)
        {
            vPatientSurgery entity = BusinessLayer.GetvPatientSurgeryList(string.Format("VisitID IN ({0}) AND PatientSurgeryID = '{1}' AND IsDeleted = 0", VisitID, PatientSurgeryID)).FirstOrDefault();

            if (entity.PostOperativeDiagnosisRemarks != null && entity.PostOperativeDiagnosisRemarks != "")
            {
                xrLabel1.Text = string.Format("Uraian Pembedahan");
                xrLabel2.Text = string.Format(":");
                lblRemarks.Text = string.Format("{0}", entity.PostOperativeDiagnosisRemarks);
            }
            else
            {
                xrLabel1.Visible = false;
                xrLabel2.Visible = false;
                lblRemarks.Visible = false;
            }
        }
    }
}

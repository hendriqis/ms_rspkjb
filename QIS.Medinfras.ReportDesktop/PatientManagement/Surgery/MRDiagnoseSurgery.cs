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
    public partial class MRDiagnoseSurgery : DevExpress.XtraReports.UI.XtraReport
    {
        public MRDiagnoseSurgery()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID, int PatientSurgeryID)
        {
            vPatientSurgery entity = BusinessLayer.GetvPatientSurgeryList(string.Format("VisitID IN ({0}) AND PatientSurgeryID = '{1}' AND IsDeleted = 0", VisitID, PatientSurgeryID)).FirstOrDefault();
            vDiagnose entityD = BusinessLayer.GetvDiagnoseList(string.Format("DiagnoseID = '{0}'", entity.PreOperativeDiagnosisID)).FirstOrDefault();
            vDiagnose entityDD = BusinessLayer.GetvDiagnoseList(string.Format("DiagnoseID = '{0}'", entity.PostOperativeDiagnosisID)).FirstOrDefault();

            if (entity.PreOperativeDiagnosisID != null || entity.PreOperativeDiagnosisText.Length > 0)
            {
                xrLabel1.Text = string.Format("Pre Diagnosis (ICD X)");
                xrLabel2.Text = string.Format(":");
                lblPreDiagnosis.Text = string.Format("{0}", entity.PreOperativeDiagnosisText);
                //    lblPreDiagnosis.Text = string.Format("{0}", entityD.DiagnoseName);
                xrLabel4.Text = string.Format("Pre Diagnosis Text"); 
                xrLabel5.Text = string.Format(":");
                lblPreDiagnosisText.Text = entity.PreOperativeDiagnosisText;
            }
            else
            {
                xrLabel1.Visible = false;
                xrLabel2.Visible = false;
                lblPreDiagnosis.Visible = false;
                xrLabel4.Visible = false;
                xrLabel5.Visible = false;
                lblPreDiagnosisText.Visible = false;
            }

            if (entity.PostOperativeDiagnosisID != null || entity.PostOperativeDiagnosisText.Length > 0)
            {
                xrLabel7.Text = string.Format("Post Diagnosis (ICD X)");
                xrLabel8.Text = string.Format(":");
                lblPostDiagnosis.Text = string.Format("{0}", entity.PostOperativeDiagnosisText);
                //    lblPostDiagnosis.Text = string.Format("{0}", entityDD.DiagnoseName);
                xrLabel10.Text = string.Format("Post Diagnosis Text");
                xrLabel11.Text = string.Format(":");
                lblPostDiagnosisText.Text = entity.PostOperativeDiagnosisText;
            }
            else
            {
                xrLabel7.Visible = false;
                xrLabel8.Visible = false;
                lblPostDiagnosis.Visible = false;
                xrLabel10.Visible = false;
                xrLabel11.Visible = false;
                lblPostDiagnosisText.Visible = false;
            }
        }
    }
}

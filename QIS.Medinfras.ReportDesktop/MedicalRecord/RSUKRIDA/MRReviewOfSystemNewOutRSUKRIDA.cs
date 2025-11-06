using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using System.Collections.Generic;
using DevExpress.XtraReports.Parameters;
using System.Linq;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class MRReviewOfSystemNewOutRSUKRIDA : DevExpress.XtraReports.UI.XtraReport
    {
        public MRReviewOfSystemNewOutRSUKRIDA()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID, int MedicalResumeID)
        {
            string filterExpression = string.Format("VisitID IN ({0}) AND MedicalResumeID = {1} AND IsDeleted = 0", VisitID, MedicalResumeID);
            vMedicalResume entityMR = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID = {0}", VisitID)).FirstOrDefault();

            if (entityMR.ObjectiveResumeText != null && entityMR.ObjectiveResumeText != "")
            {
                xrLabel2.Text = string.Format("Catatan Tambahan Pemeriksaan Fisik :");
                lblRemarks.Text = string.Format("{0}", entityMR.ObjectiveResumeText);
            }
            else
            {
                xrLabel2.Visible = false;
                lblRemarks.Visible = false;
            }

            if (filterExpression != null)
            {
                lblRemarks.Text = string.Format("{0}", entityMR.ObjectiveResumeText);
                List<vReviewOfSystemHd> lstHd = BusinessLayer.GetvReviewOfSystemHdList(filterExpression);
                List<vReviewOfSystemDt> lstDt = BusinessLayer.GetvReviewOfSystemDtList(filterExpression);
                var lst = (from p in lstHd
                           select new
                           {
                               ReviewOfSystemDts = lstDt.Where(dt => dt.ID == p.ID).ToList()
                           }).ToList();

                this.DataSource = lst;
                DetailReport.DataMember = "ReviewOfSystemDts";
            }
            else
            {
                xrLabel1.Visible = false;
                xrLabel2.Visible = false;
                xrLabel3.Visible = false;
                xrLabel4.Visible = false;
                xrLabel6.Visible = false;
                lblRemarks.Visible = false;
            }
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (DetailReport.RowCount == 0)
            {
                Detail.Visible = false;
            }
        }

    }
}

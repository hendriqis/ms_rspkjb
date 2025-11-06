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
    public partial class MRReviewOfSystemRSUKI : DevExpress.XtraReports.UI.XtraReport
    {
        public MRReviewOfSystemRSUKI()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID, int MedicalResumeID)
        {
            string filterExpression = string.Format("VisitID IN ({0}) AND ISNULL(MedicalResumeID,0) = {1} AND IsDeleted = 0", VisitID, MedicalResumeID);
            List<vReviewOfSystemHd> lstHd = BusinessLayer.GetvReviewOfSystemHdList(filterExpression);
            List<vReviewOfSystemDt> lstDt = BusinessLayer.GetvReviewOfSystemDtList(filterExpression);
            vMedicalResume entityMR = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID = {0}", VisitID)).FirstOrDefault();

            var lst = (from p in lstHd
                       select new
                       {
                           ObservationDateInString = p.ObservationDateInString,
                           ObservationTime = p.ObservationTime,
                           ParamedicName = p.ParamedicName,
                           ReviewOfSystemDts = lstDt.Where(dt => dt.ID == p.ID).ToList()
                       }).ToList();

            this.DataSource = lst;
            DetailReport.DataMember = "ReviewOfSystemDts";
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

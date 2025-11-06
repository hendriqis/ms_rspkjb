using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using System.Collections.Generic;
using DevExpress.XtraReports.Parameters;
using System.Linq;

namespace QIS.Medinfras.Report
{
    public partial class EpisodeSummaryReviewOfSystemRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public EpisodeSummaryReviewOfSystemRpt()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", VisitID);
            List<vReviewOfSystemHd> lstHd = BusinessLayer.GetvReviewOfSystemHdList(filterExpression);
            List<vReviewOfSystemDt> lstDt = BusinessLayer.GetvReviewOfSystemDtList(filterExpression);

            var lst = (from p in lstHd
                       select new
                       {
                           ObservationDateInString = p.ObservationDateInString,
                           ObservationTime = p.ObservationTime,
                           ReviewOfSystemDts = lstDt.Where(dt => dt.ID == p.ID).ToList()
                       }).ToList();

            this.DataSource = lst;
            DetailReport.DataMember = "ReviewOfSystemDts";
        }

    }
}

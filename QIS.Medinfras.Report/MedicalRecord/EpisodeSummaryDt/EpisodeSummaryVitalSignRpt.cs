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
    public partial class EpisodeSummaryVitalSignRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public EpisodeSummaryVitalSignRpt()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", VisitID);
            List<vVitalSignHd> lstVitalSignHd = BusinessLayer.GetvVitalSignHdList(filterExpression);
            List<vVitalSignDt> lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(filterExpression);

            var lst = (from p in lstVitalSignHd
                       select new
                       {
                           ObservationDateInString = p.ObservationDateInString,
                           ObservationTime = p.ObservationTime,
                           VitalSignDts = lstVitalSignDt.Where(dt => dt.ID == p.ID).ToList()
                       }).ToList();

            this.DataSource = lst;
            DetailReport.DataMember = "VitalSignDts";
        }

    }
}

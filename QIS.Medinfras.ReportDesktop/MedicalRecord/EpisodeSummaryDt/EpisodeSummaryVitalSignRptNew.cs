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
    public partial class EpisodeSummaryVitalSignRptNew : DevExpress.XtraReports.UI.XtraReport
    {
        public EpisodeSummaryVitalSignRptNew()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", VisitID);
            string filterExpressionDt = string.Format("VisitID = {0} AND IsDeleted = 0 AND VitalSignID IN (1,2,3,4,5,34,35,73)", VisitID);
            List<vVitalSignHd> lstVitalSignHd = BusinessLayer.GetvVitalSignHdList(filterExpression);
            List<vVitalSignDt> lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(filterExpressionDt);

            var lst = (from p in lstVitalSignHd
                       select new
                       {
                           ObservationDateInString = p.ObservationDateInString,
                           ObservationTime = p.ObservationTime,
                           ParamedicName = p.ParamedicName,
                           VitalSignDts = lstVitalSignDt.Where(dt => dt.ID == p.ID).ToList()
                       }).ToList();

            this.DataSource = lst;
            DetailReport.DataMember = "VitalSignDts";
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

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using System.Collections.Generic;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class EpisodeSummaryDischargeRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public EpisodeSummaryDischargeRpt()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID)
        {
            List<vConsultVisit> lstEntity = BusinessLayer.GetvConsultVisitList(String.Format("VisitID = {0} AND IsMainVisit = 1", VisitID));
            this.DataSource = lstEntity;
        }
    }
}

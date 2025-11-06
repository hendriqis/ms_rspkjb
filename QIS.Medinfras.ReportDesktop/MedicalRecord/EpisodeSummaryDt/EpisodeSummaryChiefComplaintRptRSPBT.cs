using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using System.Collections.Generic;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class EpisodeSummaryChiefComplaintRptRSPBT : DevExpress.XtraReports.UI.XtraReport
    {
        public EpisodeSummaryChiefComplaintRptRSPBT()
        {
            InitializeComponent();
        }

        public void InitializeReport(int visitID)
        {
            List<vChiefComplaint> lstChiefComplaint = BusinessLayer.GetvChiefComplaintList(string.Format("VisitID = {0} AND IsDeleted = 0", visitID));
            this.DataSource = lstChiefComplaint;
        }

    }
}

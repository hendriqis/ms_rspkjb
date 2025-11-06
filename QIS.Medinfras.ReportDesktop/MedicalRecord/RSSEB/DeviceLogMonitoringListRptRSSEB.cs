using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class DeviceLogMonitoringListRptRSSEB : BaseDailyLandscapeRpt
    {
        public DeviceLogMonitoringListRptRSSEB()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            lblReportSubtitle.Visible = false;
            lblReportTitle.Visible = false;

            if (param[1] == "X525^001")
            {
                xrLabel1.Text = "Laporan Penggunaan Ventilator";
            }
            else if (param[1] == "X525^005")
            {
                xrLabel1.Text = "Laporan Penggunaan CPAP";
            }
            else
            {
                xrLabel1.Text = "Laporan Penggunaan Alat";
            }

            base.InitializeReport(param);
        }
    }
}

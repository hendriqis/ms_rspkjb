using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class DeviceLogMonitoringListRpt : BaseDailyPortraitRpt
    {
        public DeviceLogMonitoringListRpt()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            lblReportSubtitle.Visible = false;
            lblReportTitle.Visible = false;

            if (param[1] == "X525^001")
            {
                lblTitle.Text = "Laporan Penggunaan Ventilator";
            }
            else if (param[1] == "X525^005")
            {
                lblTitle.Text = "Laporan Penggunaan CPAP";
            }
            else
            {
                lblTitle.Text = "Laporan Penggunaan Alat";
            }

            base.InitializeReport(param);
        }
    }
}

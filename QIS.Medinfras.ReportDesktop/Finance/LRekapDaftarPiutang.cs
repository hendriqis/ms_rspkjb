using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LRekapDaftarPiutang : BaseCustomDailyLandscapeRpt
    {
        public LRekapDaftarPiutang()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            var month = "";
            if (param[1] == "1")
            {
                month = "JANUARI";
            }
            if (param[1] == "2")
            {
                month = "FEBUARI";
            }
            if (param[1] == "3")
            {
                month = "MARET";
            }
            if (param[1] == "4")
            {
                month = "APRIL";
            }
            if (param[1] == "5")
            {
                month = "MEI";
            }
            if (param[1] == "6")
            {
                month = "JUNI";
            }
            if (param[1] == "7")
            {
                month = "JULI";
            }
            if (param[1] == "8")
            {
                month = "AGUSTUS";
            }
            if (param[1] == "9")
            {
                month = "SEPTEMBER";
            }
            if (param[1] == "10")
            {
                month = "OKTOBER";
            }
            if (param[1] == "11")
            {
                month = "NOVEMBER";
            }
            if (param[1] == "12")
            {
                month = "DESEMBER";
            }
            lblBulan.Text = string.Format("Bulan : {0} {1}", month, param[0]);
            lblDate1.Text = string.Format("{0} {1}", month, param[0]);
            lblDate2.Text = string.Format("{0} {1}", month, param[0]);

            base.InitializeReport(param);
        }

    }
}

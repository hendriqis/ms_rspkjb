using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using ThoughtWorks.QRCode.Codec;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LStatistikPenggunaanEMR2 : BaseCustomDailyLandscapeRpt
    {
        public LStatistikPenggunaanEMR2()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            Int32 bulan = Convert.ToInt32(param[1]);
            String bulanName = "";

            if (bulan == 1)
            {
                bulanName = "Januari";
            }
            else if (bulan == 2)
            {
                bulanName = "Februari";
            }
            else if (bulan == 3)
            {
                bulanName = "Maret";
            }
            else if (bulan == 4)
            {
                bulanName = "April";
            }
            else if (bulan == 5)
            {
                bulanName = "Mei";
            }
            else if (bulan == 6)
            {
                bulanName = "Juni";
            }
            else if (bulan == 7)
            {
                bulanName = "Juli";
            }
            else if (bulan == 8)
            {
                bulanName = "Agustus";
            }
            else if (bulan == 9)
            {
                bulanName = "September";
            }
            else if (bulan == 10)
            {
                bulanName = "Oktober";
            }
            else if (bulan == 11)
            {
                bulanName = "November";
            }
            else if (bulan == 12)
            {
                bulanName = "Desember";
            }

            lblPeriod.Text = string.Format("Periode : {0} {1}", bulanName, param[0]);
            base.InitializeReport(param);
        }

    }
}
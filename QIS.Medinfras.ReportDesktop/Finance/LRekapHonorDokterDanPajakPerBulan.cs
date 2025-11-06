using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LRekapHonorDokterDanPajakPerBulan : BaseCustomDailyLandscapeA3Rpt
    {
        public LRekapHonorDokterDanPajakPerBulan()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            String result = "";
            if (param[1] == "1")
            {
                result = "Januari";
            }
            else if (param[1] == "2")
            {
                result = "Februari";
            }
            else if (param[1] == "3")
            {
                result = "Maret";
            }
            else if (param[1] == "4")
            {
                result = "April";
            }
            else if (param[1] == "5")
            {
                result = "Mei";
            }
            else if (param[1] == "6")
            {
                result = "Juni";
            }
            else if (param[1] == "7")
            {
                result = "Juli";
            }
            else if (param[1] == "8")
            {
                result = "Agustus";
            }
            else if (param[1] == "9")
            {
                result = "September";
            }
            else if (param[1] == "10")
            {
                result = "Oktober";
            }
            else if (param[1] == "11")
            {
                result = "November";
            }
            else if (param[1] == "12")
            {
                result = "Desember";
            }

            lblPeriod.Text = string.Format("Periode : {0} {1}", result, param[0]);

            base.InitializeReport(param);
        }

    }
}

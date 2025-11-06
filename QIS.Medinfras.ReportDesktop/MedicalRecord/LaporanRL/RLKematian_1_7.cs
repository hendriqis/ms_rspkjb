using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class RLKematian_1_7 : BaseCustomDailyLandscapeA3Rpt
    {
        public RLKematian_1_7()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            Int32 Bulan = Convert.ToInt32(param[1].ToString());
            String result = "";
                if (Bulan == 1)
                {
                    result = "Januari";
                }
                else if (Bulan == 2)
                {
                    result = "Februari";
                }
                else if (Bulan == 3)
                {
                    result = "Maret";
                }
                else if (Bulan == 4)
                {
                    result = "April";
                }
                else if (Bulan == 5)
                {
                    result = "Mei";
                }
                else if (Bulan == 6)
                {
                    result = "Juni";
                }
                else if (Bulan == 7)
                {
                    result = "Juli";
                }
                else if (Bulan == 8)
                {
                    result = "Agustus";
                }
                else if (Bulan == 9)
                {
                    result = "September";
                }
                else if (Bulan == 10)
                {
                    result = "Oktober";
                }
                else if (Bulan == 11)
                {
                    result = "November";
                }
                else if (Bulan == 12)
                {
                    result = "Desember";
                }

            lblPeriod.Text = string.Format("Periode : {0} {1}", result, param[0].ToString());
            base.InitializeReport(param);
        }

    }
}

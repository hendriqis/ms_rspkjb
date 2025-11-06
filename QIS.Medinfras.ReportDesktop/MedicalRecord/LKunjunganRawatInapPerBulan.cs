using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;
using System.Data;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LKunjunganRawatInapPerBulan : BaseDailyLandscapeRpt
    {
        public LKunjunganRawatInapPerBulan()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string result = "";
            int month = Convert.ToInt32(param[1].ToString());

            if (month == 1)
            {
                result = "Januari";
            }
            else if (month == 2)
            {
                result = "Februari";
            }
            else if (month == 3)
            {
                result = "Maret";
            }
            else if (month == 4)
            {
                result = "April";
            }
            else if (month == 5)
            {
                result = "Mei";
            }
            else if (month == 6)
            {
                result = "Juni";
            }
            else if (month == 7)
            {
                result = "Juli";
            }
            else if (month == 8)
            {
                result = "Agustus";
            }
            else if (month == 9)
            {
                result = "September";
            }
            else if (month == 10)
            {
                result = "Oktober";
            }
            else if (month == 11)
            {
                result = "November";
            }
            else if (month == 12)
            {
                result = "Desember";
            }

            lblTahun.Text = string.Format("Tahun : {0}", param[0]);
            lblBulan.Text = string.Format("Bulan : {0}", result);
            base.InitializeReport(param);
        }
    }
}

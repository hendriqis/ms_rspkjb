using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPemakaianAlatInfeksiRanap : BaseCustomDailyPotraitRpt
    {
        public LPemakaianAlatInfeksiRanap()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            lblParamYear.Text = string.Format("Tahun = {0}", param[0]);
            string paramMonth = "";
            switch (Convert.ToInt32(param[1]))
            {
                case 1: paramMonth = "Januari"; break;
                case 2: paramMonth = "Februari"; break;
                case 3: paramMonth = "Maret"; break;
                case 4: paramMonth = "April"; break;
                case 5: paramMonth = "Mei"; break;
                case 6: paramMonth = "Juni"; break;
                case 7: paramMonth = "Juli"; break;
                case 8: paramMonth = "Agustus"; break;
                case 9: paramMonth = "September"; break;
                case 10: paramMonth = "Oktober"; break;
                case 11: paramMonth = "November"; break;
                case 12: paramMonth = "Desember"; break;
            }
            lblParamMonth.Text = string.Format("Bulan = {0}", paramMonth);
            base.InitializeReport(param);
        }

    }
}

using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LDaftarOrderDietPasienRSRT : BaseCustomDailyLandscapeA3Rpt
    {
        public LDaftarOrderDietPasienRSRT()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            base.InitializeReport(param);

            string[] data = param[0].Split(';');
            lblPeriode.Text = string.Format("Tanggal Periode {0} s.d {1}", data[0], data[1]);
        }

    }
}

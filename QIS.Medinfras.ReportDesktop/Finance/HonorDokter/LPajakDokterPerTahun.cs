using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPajakDokterPerTahun : BaseCustomDailyLandscapeRpt
    {
        public LPajakDokterPerTahun()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            lblYear.Text = string.Format("Tahun : {0}", param[0]);

            base.InitializeReport(param);
        }

    }
}

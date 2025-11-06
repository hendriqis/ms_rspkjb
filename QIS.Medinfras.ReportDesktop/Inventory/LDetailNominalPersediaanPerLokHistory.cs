using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LDetailNominalPersediaanPerLokHistory : BaseCustomDailyLandscapeA3Rpt
    {
        public LDetailNominalPersediaanPerLokHistory()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            lblPeriode.Text = string.Format("Periode : {0}", param[0]);
            base.InitializeReport(param);
        }
    }
}

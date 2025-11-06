using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;


namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPorsiMakanPasien : BaseCustomDailyLandscapeA3Rpt
    {
        public LPorsiMakanPasien()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            lblMonthYear.Text = string.Format("Periode : {0} {1}", param[1], param[0]);

            base.InitializeReport(param);
        }
    }
}

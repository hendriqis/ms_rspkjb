using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LDaftarAktivaTetapOpnameAset : BaseDailyLandscapeRpt
    {
        public LDaftarAktivaTetapOpnameAset()
        {
            InitializeComponent();
        }

        private void bs_CurrentChanged(object sender, EventArgs e)
        {

        }
    }
}

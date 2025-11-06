using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPemesananBarangPerSupplierNonRutin : BaseCustomDailyLandscapeA3Rpt
    {
        public LPemesananBarangPerSupplierNonRutin()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            lblPeriod.Text = string.Format("Lokasi : {0}", BusinessLayer.GetLocation(Convert.ToInt32(param[1])).LocationName);
            base.InitializeReport(param);
        }

    }
}

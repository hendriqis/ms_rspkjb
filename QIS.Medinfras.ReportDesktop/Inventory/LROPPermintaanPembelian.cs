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
    public partial class LROPPermintaanPembelian : BaseCustomDailyLandscapeRpt
    {
        public LROPPermintaanPembelian()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vLocation loc = BusinessLayer.GetvLocationList(string.Format("LocationID = {0}", param[0])).FirstOrDefault();
            lblParamLocation.Text = string.Format("Location : {0}", loc.LocationName);
            base.InitializeReport(param);
        }

    }
}

using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LDistribusiBarangMasuk_All : QIS.Medinfras.ReportDesktop.BaseCustomDailyPotraitRpt
    {
        public LDistribusiBarangMasuk_All()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            lblToLocation.Text = String.Format("Ke Lokasi : {0}", BusinessLayer.GetLocation(Convert.ToInt32(param[2])).LocationName);
            base.InitializeReport(param);
        }

    }
}

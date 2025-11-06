using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LDistribusiBarangKeluar_All : QIS.Medinfras.ReportDesktop.BaseCustomDailyPotraitRpt
    {
        public LDistribusiBarangKeluar_All()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            lblFromLocation.Text = String.Format("Dari Lokasi : {0}", BusinessLayer.GetLocation(Convert.ToInt32(param[2])).LocationName);
            base.InitializeReport(param);
        }

    }
}

using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LAnalisaWaktuPelayananPasien : BaseCustomDailyLandscapeRpt
    {
        public LAnalisaWaktuPelayananPasien()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
          //  StandardCode sc = BusinessLayer.GetStandardCode(param[2].ToString());
           // xrLabel2.Text = String.Format("Kelompok Klinik : {0}" ,sc.StandardCodeName);
            base.InitializeReport(param);
        }

    }
}

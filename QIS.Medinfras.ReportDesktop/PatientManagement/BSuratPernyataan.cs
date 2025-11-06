using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BSuratPernyataan : BaseDailyPortraitRpt
    {
        public BSuratPernyataan()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            xrLabel31.Text = DateTime.Now.ToString("dd-MM-yyyy");
            xrLabel32.Text = DateTime.Now.ToString("HH:mm");
            base.InitializeReport(param);
        }

    }
}

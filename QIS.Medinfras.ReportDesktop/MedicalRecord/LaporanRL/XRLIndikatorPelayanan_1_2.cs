using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class XRLIndikatorPelayanan_1_2 : BaseCustomDailyPotraitRpt
    {
        public XRLIndikatorPelayanan_1_2()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            lblPeriod.Text = string.Format("Tahun : {0}", param[0].ToString());
            base.InitializeReport(param);
        }
    }
}

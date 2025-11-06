using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;
using System.Data;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LKunjunganRawatInapLama : BaseDailyLandscapeRpt
    {
        public LKunjunganRawatInapLama()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            base.InitializeReport(param);
        }
    }
}

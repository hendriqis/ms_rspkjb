using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LRekapitulasiPemotonganPPH21Summary : BaseCustom2DailyPotraitRpt
    {
        public LRekapitulasiPemotonganPPH21Summary()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
         {
            lblPeriod.Text = string.Format("Periode Tahun : {0}", param[0]);
            
            base.InitializeReport(param);
        }

    }
}

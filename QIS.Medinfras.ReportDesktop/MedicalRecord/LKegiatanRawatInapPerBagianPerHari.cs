using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using QIS.Medinfras.Web.Common;
using DevExpress.XtraReports.UI;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LKegiatanRawatInapPerBagianPerHari : BaseCustomDailyLandscapeA3Rpt
    {
        public LKegiatanRawatInapPerBagianPerHari()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            lblPeriod.Text = string.Format("Tanggal : {0}", Helper.YYYYMMDDToDate(param[0]).ToString(Constant.FormatString.DATE_FORMAT));

            base.InitializeReport(param);
        }

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LStatistikCatatanTerintegrasiSudahVerifikasi : BaseCustomDailyLandscapeRpt
    {
        public LStatistikCatatanTerintegrasiSudahVerifikasi()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] tempDate = param[0].Split(';');
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(tempDate[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(tempDate[1]).ToString(Constant.FormatString.DATE_FORMAT));

            base.InitializeReport(param);
        }

    }
}

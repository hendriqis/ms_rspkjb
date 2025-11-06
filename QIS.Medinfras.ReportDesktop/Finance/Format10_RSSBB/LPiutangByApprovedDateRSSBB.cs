using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPiutangByApprovedDateRSSBB : BaseCustomDailyLandscapeA3Rpt
    {
        public LPiutangByApprovedDateRSSBB()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] periode = param[0].Split(';');
            lblProcessDate.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(periode[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(periode[1]).ToString(Constant.FormatString.DATE_FORMAT));
            base.InitializeReport(param);
        }

    }
}

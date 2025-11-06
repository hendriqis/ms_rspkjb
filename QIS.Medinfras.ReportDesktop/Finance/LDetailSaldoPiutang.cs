using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LDetailSaldoPiutang : BaseCustomDailyLandscapeA3Rpt
    {
        public LDetailSaldoPiutang()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            lblPeriode.Text = string.Format("Tanggal Analisa : {0}", Helper.YYYYMMDDToDate(param[0]).ToString(Constant.FormatString.DATE_FORMAT));

            base.InitializeReport(param);
        }
    }
}

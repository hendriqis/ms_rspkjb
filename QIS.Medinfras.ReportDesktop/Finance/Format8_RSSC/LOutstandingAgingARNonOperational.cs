using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LOutstandingAgingARNonOperational : BaseCustomDailyLandscapeA3Rpt
    {
        public LOutstandingAgingARNonOperational()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            lblProcessDate.Text = string.Format("Tanggal Proses : {0}", Helper.YYYYMMDDToDate(param[0]).ToString(Constant.FormatString.DATE_FORMAT));
            base.InitializeReport(param);
        }

    }
}

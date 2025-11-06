using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LTransaksiPerUser : BaseCustomDailyLandscapeA3Rpt
    {
        public LTransaksiPerUser()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            string[] temp1 = param[1].Split(';');
            lblPeriod.Text = String.Format("Periode Tagihan : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
            lblJam.Text = String.Format("Jam Pembuatan Tagihan : {0} s/d {1}", temp1[0],temp1[1]);
            base.InitializeReport(param);
        }

    }
}

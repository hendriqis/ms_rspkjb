using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPemakaianDanPengeluaran : BaseDailyLandscapeRpt
    {
        public LPemakaianDanPengeluaran()
        {
            InitializeComponent();
        }
         
        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            string[] tempTime = param[1].Split(';');
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
            lblTime.Text = string.Format("Time : {0} s/d {1}", tempTime[0], tempTime[1]);
            base.InitializeReport(param);
        }

    }
}

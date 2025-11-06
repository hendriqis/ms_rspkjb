using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BSlipBangsalHarian : BaseDailyPortraitRpt
    {
        public BSlipBangsalHarian()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            //string[] temp = param[0].Split('|');
            string[] tempDate = param[1].Split(';');
            lblPeriod.Text = string.Format("{0} s/d {1}", Helper.YYYYMMDDToDate(tempDate[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(tempDate[1]).ToString(Constant.FormatString.DATE_FORMAT));
            base.InitializeReport(param);
        }

    }
}

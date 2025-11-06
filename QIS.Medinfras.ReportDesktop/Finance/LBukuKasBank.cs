using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Globalization;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.XtraReports.UI;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LBukuKasBank : BaseDailyLandscapeRpt
    {
        public LBukuKasBank()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');

            lblPeriod.Text = string.Format("Period : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
            lblAkun.Text = string.Format("Akun Treasury : {0}", BusinessLayer.GetChartOfAccount(Convert.ToInt16(param[1])).GLAccountName);

            try
            {
                this.AdditionalReportSubTitle = BusinessLayer.GetChartOfAccount(Convert.ToInt16(param[0])).GLAccountName;
            }
            catch (Exception ex)
            {
                this.AdditionalReportSubTitle = string.Empty;
            }

            base.InitializeReport(param);
        }
       
    }
}

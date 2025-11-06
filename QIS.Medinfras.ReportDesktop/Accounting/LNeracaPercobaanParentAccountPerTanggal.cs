using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using DevExpress.XtraReports.UI;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LNeracaPercobaanParentAccountPerTanggal : BaseDailyPortraitRpt
    {
        public LNeracaPercobaanParentAccountPerTanggal()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[1].Split(';');
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));

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

        private void lblBalance_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lblBalance.Text = this.xrTableCell8.Text;
        }      
    }
}

using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Linq;
using System.Globalization;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LNeracaPercobaanDetilRpt : BaseDailyPortraitRpt
    {
        public LNeracaPercobaanDetilRpt()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string monthName = Enumerable.Range(1, 12).Select(a => new
            {
                MonthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(a),
                MonthNumber = a
            }).FirstOrDefault(p => p.MonthNumber == Convert.ToInt32(param[2])).MonthName;

            lblPeriod.Text = string.Format("Period : {0} {1}", monthName, param[1]);
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

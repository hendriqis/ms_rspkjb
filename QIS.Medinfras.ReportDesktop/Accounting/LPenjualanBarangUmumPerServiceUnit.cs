using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPenjualanBarangUmumPerServiceUnit : BaseDailyPortraitRpt
    {
        public LPenjualanBarangUmumPerServiceUnit()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
            base.InitializeReport(param);
        }

        int currentLine = 0;
        private void xrTableCell3_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            
        }

        private void xrTableCell21_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            currentLine = 0;
        }

        private void xrTableCell3_BeforePrint_1(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrTableCell3.Text = (++currentLine).ToString();
        }

        private void xrTableCell5_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            currentLine = 0;
        }

        private void xrTableCell4_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            currentLine = 0;
        }

        private void GroupHeader2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

        private void xrTableCell19_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            currentLine = 0;
        }

        private void xrTableCell5_BeforePrint_1(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            currentLine = 0;
        }
    }
}

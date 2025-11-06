using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPemesananBarang : BaseDailyLandscapeRpt
    {
        public LPemesananBarang()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
            base.InitializeReport(param);
        }

        private Decimal totalprice = 0;
        private Decimal priceb4disc = 0;
        private Decimal totaldisc = 0;
        private Decimal grandtotal = 0;

        private void lblTotalPrice_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            Decimal Qty = Convert.ToDecimal(lblQty.Text);
            Decimal Price = Convert.ToDecimal(lblPrice.Text);
            Decimal Disc1 = Convert.ToDecimal(lblDisc1.Text);
            Decimal Disc2 = Convert.ToDecimal(lblDisc2.Text);

            priceb4disc = Qty * Price;
            Disc1 = Disc1 / 100 * priceb4disc;
            Disc2 = Disc2 / 100 * priceb4disc;
            totaldisc = Disc1 + Disc2;

            totalprice = priceb4disc - totaldisc;
           
            grandtotal += totalprice;

            e.Result = totalprice;
            e.Handled = true;
        
        }
        private void lblGrandTotal_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            e.Result = grandtotal;
            e.Handled = true;
        }

        private void xrTableCell17_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }
    }
}

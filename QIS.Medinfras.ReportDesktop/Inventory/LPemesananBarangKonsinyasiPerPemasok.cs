using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPemesananBarangKonsinyasiPerPemasok : BaseDailyLandscapeRpt
    {
        public LPemesananBarangKonsinyasiPerPemasok()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
            base.InitializeReport(param);
        }

        private Decimal totItemPrice = 0;
        private Decimal totSup = 0;
        private Decimal grandTotal = 0;
        private Decimal priceafterdics1;
        private Decimal totalprice;
        private int recordno = 0;

        void lblSubTotal_SummaryCalculated(object sender, TextFormatEventArgs e)
        {
            if (e.Value != null)
                totItemPrice = Convert.ToDecimal(e.Value);
        }

        void lblTotal_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            Decimal Qty = Convert.ToDecimal(lblQty.Text);
            Decimal diskon1 = Convert.ToDecimal(lblHeaderDisc1.Text)/100;
            Decimal diskon2 = Convert.ToDecimal(lblHeaderDisc2.Text)/100;
            totItemPrice = totItemPrice * Qty;
            priceafterdics1 = totItemPrice - (totItemPrice * diskon1);
            totalprice = priceafterdics1 - (priceafterdics1 * diskon2);
            
            
            totSup += totalprice;
            grandTotal += totalprice;
            e.Result = totalprice;
            e.Handled = true;
        }

        void GroupHeader2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            recordno = 0;
            totSup = 0;
        }

        void GroupHeader1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            recordno++;
        }

        void xrTableCell11_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            ((XRLabel)sender).Text = recordno.ToString();
        }

        void lblTotalTgl_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            e.Result = totSup;
            e.Handled = true;
        }

        void lblGrandTotal_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            e.Result = grandTotal;
            e.Handled = true;
        }

        //private Decimal Price = 0;
        //private Decimal totalprice = 0;
        //private Decimal priceb4disc = 0;
        //private Decimal grandtotal = 0;
        //private Decimal totSup = 0;

        //void lblPrice_SummaryCalculated(object sender, TextFormatEventArgs e)
        //{
        //    if (e.Value != null)
        //        Price = Convert.ToDecimal(e.Value);
        //}

        
        //private void lblTotalPrice_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        //{
        //    Decimal Qty = Convert.ToDecimal(lblQty.Text);
        //    Decimal Disc1 = Convert.ToDecimal(lblDisc1.Text);
        //    Decimal Disc2 = Convert.ToDecimal(lblDisc2.Text);


        //    priceb4disc = Convert.ToDecimal(Qty) * Price;
        //    Decimal Discount1 = Convert.ToDecimal(Disc1) / 100 * priceb4disc;
        //    Decimal priceafterdisc1 = priceb4disc - Discount1;
        //    Decimal Discount2 = Convert.ToDecimal(Disc2) / 100 * priceafterdisc1;
        //    Decimal priceafterdisc2 = priceafterdisc1 - Discount2;

        //    grandtotal += totalprice;

        //    e.Result = totalprice;
        //    e.Handled = true;

        //}
        //private void lblGrandTotal_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        //{
        //    e.Result = grandtotal;
        //    e.Handled = true;
        //}

    }
}

using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LUmurHutangPerSupplierRSSC : BaseDailyLandscapeRpt
    {
        public LUmurHutangPerSupplierRSSC()
        {
            InitializeComponent();
        }

        //private Decimal umur = 0;
        //private Decimal kosong = 0;

        //private void lblSubTotal_SummaryCalculated(object sender, TextFormatEventArgs e)
        //{
        //    Decimal duedate = Convert.ToDecimal(xrTableCell9.Text);
        //    Decimal voucherdate = Convert.ToDecimal(xrTableCell5.Text);

        //    umur = duedate - voucherdate;
  
        //}

        //private void xrTableCell10_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        //{
        //    if (umur < 30)
        //    {
        //        ((XRLabel)sender).Text = umur.ToString();
        //    }
        //    else
        //    {
        //        ((XRLabel)sender).Text = kosong.ToString();
        //    }
        //}

        //private void xrTableCell11_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        //{
        //    if (umur > 30)
        //    {
        //        if (umur < 60)
        //        {
        //            ((XRLabel)sender).Text = umur.ToString();
        //        }
        //        else 
        //        {
        //            ((XRLabel)sender).Text = kosong.ToString();
        //        }
        //    }
        //    else
        //    {
        //        ((XRLabel)sender).Text = kosong.ToString();
        //    }
        //}

        //private void xrTableCell12_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        //{
        //    if (umur > 60)
        //    {
        //        if (umur < 90)
        //        {
        //            ((XRLabel)sender).Text = umur.ToString();
        //        }
        //        else
        //        {
        //            ((XRLabel)sender).Text = kosong.ToString();
        //        }
        //    }
        //    else
        //    {
        //        ((XRLabel)sender).Text = kosong.ToString();
        //    }
        //}

        //private void xrTableCell13_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        //{
        //    if (umur > 90)
        //    {
        //        if (umur < 120)
        //        {
        //            ((XRLabel)sender).Text = umur.ToString();
        //        }
        //        else
        //        {
        //            ((XRLabel)sender).Text = kosong.ToString();
        //        }
        //    }
        //    else
        //    {
        //        ((XRLabel)sender).Text = kosong.ToString();
        //    }
        //}

        //private void xrTableCell14_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        //{
        //    if (umur > 120)
        //    {
        //        ((XRLabel)sender).Text = umur.ToString();
        //    }
        //    else
        //    {
        //        ((XRLabel)sender).Text = kosong.ToString();
        //    }
        //}

    }
   
}

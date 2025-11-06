using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LUmurHutang2 : BaseDailyLandscapeRpt
    {
        public LUmurHutang2()
        {
            InitializeComponent();
        }

        private void xrTableRow2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableRow row = (XRTableRow)sender;
            DateTime dueDate = DateTime.Parse(GetCurrentColumnValue("DueDate").ToString());
            XRTableCell tcUntil30Days = (XRTableCell)row.FindControl("tcUntil30Days", false);
            XRTableCell tcUntil60Days = (XRTableCell)row.FindControl("tcUntil60Days", false);
            XRTableCell tcUntil90Days = (XRTableCell)row.FindControl("tcUntil90Days", false);
            XRTableCell tcUntil120Days = (XRTableCell)row.FindControl("tcUntil120Days", false);
            XRTableCell tcMoreThan120Days = (XRTableCell)row.FindControl("tcMoreThan120Days", false);
            tcUntil30Days.Text = Function.GetPatientAgeInDay(dueDate, new DateTime(2013, 1, 1)).ToString();
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

using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace QIS.Medinfras.Report
{
    public partial class LPenerimaanBarang : BaseDailyLandscapeRpt
    {
        public LPenerimaanBarang()
        {
            InitializeComponent();
        }

        private Decimal totItemPrice = 0;
        private Decimal totTgl = 0;
        private Decimal grandTotal = 0;
        private int recordno = 0;

        private void lblSubTotal_SummaryCalculated(object sender, TextFormatEventArgs e)
        {
            if (e.Value != null)
                totItemPrice = Convert.ToDecimal(e.Value);
        }

        private void lblTotal_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            Decimal ppn = Convert.ToDecimal(lblHeaderPPN.Text);
            Decimal diskon = Convert.ToDecimal(lblHeaderDisc.Text);
            Decimal materai = Convert.ToDecimal(lblHeaderMaterai.Text);
            Decimal ongkos = Convert.ToDecimal(lblHeaderOngkos.Text);
            totItemPrice -= diskon;
            ppn = ppn / 100 * totItemPrice;
            Decimal total = totItemPrice + ppn + materai + ongkos;
            totTgl += total;
            grandTotal += total;
            e.Result = total;
            e.Handled = true;
        }

        private void GroupHeader2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            recordno = 0;
            totTgl = 0;
        }

        private void GroupHeader1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            recordno++;
        }

        private void xrTableCell11_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            ((XRLabel)sender).Text = recordno.ToString();
        }

        private void lblTotalTgl_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            e.Result = totTgl;
            e.Handled = true;
        }

        private void lblGrandTotal_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            e.Result = grandTotal;
            e.Handled = true;
        }



    }
}

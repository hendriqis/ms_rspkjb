using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LKunjunganInstalasiGawatDaruratPerBulanPerTanggal : BaseDailyLandscapeRpt
    {
        public LKunjunganInstalasiGawatDaruratPerBulanPerTanggal()
        {
            InitializeComponent();
        }

        private void xrTableCell74_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Decimal result = Convert.ToDecimal(xrTableCell74.Text);
            if (1 - result < 1 && 1 - result > 0)
            {
                xrTableCell74.Text = result.ToString("#.##") + "%";
            }
            else if (result == 0)
            {
                xrTableCell74.Text = "0.00%";
            }
            else if (result > 100)
            {
                xrTableCell74.Text = "100%";
            }
            else
            {
                xrTableCell74.Text = result.ToString("#.##") + "%";
            }
        }
    }
}
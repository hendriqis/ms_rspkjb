using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using System.Linq;
using System.Data;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LJenisTindakanDiagnostikdiIGD : BaseDailyPortraitRpt
    {
        decimal sumTotalSekarang = 0;
        decimal sumTotalSebelum = 0;

        public LJenisTindakanDiagnostikdiIGD()
        {
            InitializeComponent();            
        }

        public override void InitializeReport(string[] param)
        {
            base.InitializeReport(param);
            sumTotalSekarang = lstDynamic.Sum(t => t.JmlhSekarang);
            sumTotalSebelum = lstDynamic.Sum(t => t.JmlhSebelum);
        }

        private void xrTableCell7_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            decimal colSummary = Convert.ToDecimal(xrTableCell6.Text);
            decimal totSummary = sumTotalSekarang;
            if (totSummary == 0)
            {
                totSummary = totSummary + 1;
            }
            Decimal result = (colSummary / totSummary * 100);

            if (1 - result < 1 && 1 - result > 0)
            {
                xrTableCell7.Text = "0" + result.ToString("#.##");
            }
            else if (result == 0)
            {
                xrTableCell7.Text = "0.00";
            }
            else
            {
                xrTableCell7.Text = result.ToString("#.##");
            }

        }

        private void xrTableCell15_BeforePrint_1(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            decimal colSummary = Convert.ToDecimal(xrTableCell11.Text);
            decimal totSummary = sumTotalSebelum;
            if (totSummary == 0)
            {
                totSummary = totSummary + 1;
            }
            Decimal result = (colSummary / totSummary * 100);

            if (1 - result < 1 && 1 - result > 0)
            {
                xrTableCell15.Text = "0" + result.ToString("#.##");
            }
            else if (result == 0)
            {
                xrTableCell15.Text = "0.00";
            }
            else
            {
                xrTableCell15.Text = result.ToString("#.##");
            }
        }

        private void xrTableCell8_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            decimal colSummarySekarang = Convert.ToDecimal(xrTableCell6.Text);
            decimal totSummarySekarang = sumTotalSekarang;
            if (totSummarySekarang == 0)
            {
                totSummarySekarang = totSummarySekarang + 1;
            }
            Decimal resultSekarang = (colSummarySekarang / totSummarySekarang * 100);

            decimal colSummarySebelum = Convert.ToDecimal(xrTableCell11.Text);
            decimal totSummarySebelum = sumTotalSebelum;
            if (totSummarySebelum == 0)
            {
                totSummarySebelum = totSummarySebelum + 1;
            }
            Decimal resultSebelum = (colSummarySebelum / totSummarySebelum * 100);

            Decimal hasil = resultSekarang - resultSebelum;

            if (1 - hasil < 1 && 1 - hasil > 0)
            {
                xrTableCell8.Text = "0" + hasil.ToString("#.##");
            }
            else if (hasil == 0)
            {
                xrTableCell8.Text = "0.00";
            }
            else
            {
                xrTableCell8.Text = hasil.ToString("#.##");
            }
        }

    }
}
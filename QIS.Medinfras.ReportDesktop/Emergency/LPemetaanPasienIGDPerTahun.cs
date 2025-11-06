using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPemetaanPasienIGDPerTahun : BaseCustomDailyLandscapeA3Rpt
    {
        public LPemetaanPasienIGDPerTahun()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {

            base.InitializeReport(param);
        }

        private void xrTableCell60_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            decimal colSummary = (Decimal)xrTableCell59.Summary.GetResult();
            decimal totSummary = (Decimal)xrTableCell110.Summary.GetResult();
            Decimal result = (colSummary / totSummary * 100);

            if (totSummary == 0)
            {
                totSummary = totSummary + 1;
            }

            if (result.ToString("#.##").Equals("1"))
            {
                xrTableCell60.Text = "0.99";
            }
            else if (1 - result < 1 && 1 - result > 0)
            {
                xrTableCell60.Text = "0" + result.ToString("#.##");
            }
            else if (result == 0)
            {
                xrTableCell60.Text = "0.00";
            }
            else
            {
                xrTableCell60.Text = result.ToString("#.##");
            }
        }

        private void xrTableCell70_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Decimal colSummary = (Decimal)xrTableCell89.Summary.GetResult();
            Decimal totSummary = (Decimal)xrTableCell110.Summary.GetResult();

            if (totSummary == 0)
            {
                totSummary = totSummary + 1;
            }

            Decimal result = (colSummary / totSummary * 100);


            if (result.ToString("#.##").Equals("1"))
            {
                xrTableCell70.Text = "0.99";
            }
            else if (1 - result < 1 && 1 - result > 0)
            {
                xrTableCell70.Text = "0" + result.ToString("#.##");
            }
            else if (result == 0)
            {
                xrTableCell70.Text = "0.00";
            }
            else
            {
                xrTableCell70.Text = result.ToString("#.##");
            }
        }

        private void xrTableCell71_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            decimal colSummary = (Decimal)xrTableCell90.Summary.GetResult();
            decimal totSummary = (Decimal)xrTableCell110.Summary.GetResult();
            Decimal result = (colSummary / totSummary * 100);

            if (totSummary == 0)
            {
                totSummary = totSummary + 1;
            }

            if (result.ToString("#.##").Equals("1"))
            {
                xrTableCell71.Text = "0.99";
            }
            else if (1 - result < 1 && 1 - result > 0)
            {
                xrTableCell71.Text = "0" + result.ToString("#.##");
            }
            else if (result == 0)
            {
                xrTableCell71.Text = "0.00";
            }
            else
            {
                xrTableCell71.Text = result.ToString("#.##");
            }
        }

        private void xrTableCell72_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            decimal colSummary = (Decimal)xrTableCell91.Summary.GetResult();
            decimal totSummary = (Decimal)xrTableCell110.Summary.GetResult();
            Decimal result = (colSummary / totSummary * 100);

            if (totSummary == 0)
            {
                totSummary = totSummary + 1;
            }

            if (result.ToString("#.##").Equals("1"))
            {
                xrTableCell72.Text = "0.99";
            }
            else if (1 - result < 1 && 1 - result > 0)
            {
                xrTableCell72.Text = "0" + result.ToString("#.##");
            }
            else if (result == 0)
            {
                xrTableCell72.Text = "0.00";
            }
            else
            {
                xrTableCell72.Text = result.ToString("#.##");
            }
        }

        private void xrTableCell73_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            decimal colSummary = (Decimal)xrTableCell92.Summary.GetResult();
            decimal totSummary = (Decimal)xrTableCell110.Summary.GetResult();
            Decimal result = (colSummary / totSummary * 100);

            if (totSummary == 0)
            {
                totSummary = totSummary + 1;
            }

            if (result.ToString("#.##").Equals("1"))
            {
                xrTableCell73.Text = "0.99";
            }
            else if (1 - result < 1 && 1 - result > 0)
            {
                xrTableCell73.Text = "0" + result.ToString("#.##");
            }
            else if (result == 0)
            {
                xrTableCell73.Text = "0.00";
            }
            else
            {
                xrTableCell73.Text = result.ToString("#.##");
            }
        }

        private void xrTableCell74_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            decimal colSummary = (Decimal)xrTableCell93.Summary.GetResult();
            decimal totSummary = (Decimal)xrTableCell110.Summary.GetResult();
            Decimal result = (colSummary / totSummary * 100);

            if (totSummary == 0)
            {
                totSummary = totSummary + 1;
            }

            if (result.ToString("#.##").Equals("1"))
            {
                xrTableCell74.Text = "0.99";
            }
            else if (1 - result < 1 && 1 - result > 0)
            {
                xrTableCell74.Text = "0" + result.ToString("#.##");
            }
            else if (result == 0)
            {
                xrTableCell74.Text = "0.00";
            }
            else
            {
                xrTableCell74.Text = result.ToString("#.##");
            }
        }

        private void xrTableCell75_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            decimal colSummary = (Decimal)xrTableCell94.Summary.GetResult();
            decimal totSummary = (Decimal)xrTableCell110.Summary.GetResult();
            Decimal result = (colSummary / totSummary * 100);

            if (totSummary == 0)
            {
                totSummary = totSummary + 1;
            }

            if (result.ToString("#.##").Equals("1"))
            {
                xrTableCell75.Text = "0.99";
            }
            else if (1 - result < 1 && 1 - result > 0)
            {
                xrTableCell75.Text = "0" + result.ToString("#.##");
            }
            else if (result == 0)
            {
                xrTableCell75.Text = "0.00";
            }
            else
            {
                xrTableCell75.Text = result.ToString("#.##");
            }
        }

        private void xrTableCell76_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            decimal colSummary = (Decimal)xrTableCell95.Summary.GetResult();
            decimal totSummary = (Decimal)xrTableCell110.Summary.GetResult();
            Decimal result = (colSummary / totSummary * 100);

            if (totSummary == 0)
            {
                totSummary = totSummary + 1;
            }

            if (result.ToString("#.##").Equals("1"))
            {
                xrTableCell76.Text = "0.99";
            }
            else if (1 - result < 1 && 1 - result > 0)
            {
                xrTableCell76.Text = "0" + result.ToString("#.##");
            }
            else if (result == 0)
            {
                xrTableCell76.Text = "0.00";
            }
            else
            {
                xrTableCell76.Text = result.ToString("#.##");
            }
        }

        private void xrTableCell77_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            decimal colSummary = (Decimal)xrTableCell96.Summary.GetResult();
            decimal totSummary = (Decimal)xrTableCell110.Summary.GetResult();
            Decimal result = (colSummary / totSummary * 100);

            if (totSummary == 0)
            {
                totSummary = totSummary + 1;
            }

            if (result.ToString("#.##").Equals("1"))
            {
                xrTableCell77.Text = "0.99";
            }
            else if (1 - result < 1 && 1 - result > 0)
            {
                xrTableCell77.Text = "0" + result.ToString("#.##");
            }
            else if (result == 0)
            {
                xrTableCell77.Text = "0.00";
            }
            else
            {
                xrTableCell77.Text = result.ToString("#.##");
            }
        }

        private void xrTableCell78_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Decimal colSummary = (Decimal)xrTableCell97.Summary.GetResult();
            Decimal totSummary = (Decimal)xrTableCell110.Summary.GetResult();
            Decimal result = (colSummary / totSummary * 100);

            if (totSummary == 0)
            {
                totSummary = totSummary + 1;
            }

            if (result.ToString("#.##").Equals("1"))
            {
                xrTableCell78.Text = "0.99";
            }
            else if (1 - result < 1 && 1 - result > 0)
            {
                xrTableCell78.Text = "0" + result.ToString("#.##");
            }
            else if (result == 0)
            {
                xrTableCell78.Text = "0.00";
            }
            else
            {
                xrTableCell78.Text = result.ToString("#.##");
            }
        }

        private void xrTableCell79_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            decimal colSummary = (Decimal)xrTableCell98.Summary.GetResult();
            decimal totSummary = (Decimal)xrTableCell110.Summary.GetResult();
            Decimal result = (colSummary / totSummary * 100);

            if (totSummary == 0)
            {
                totSummary = totSummary + 1;
            }

            if (result.ToString("#.##").Equals("1"))
            {
                xrTableCell79.Text = "0.99";
            }
            else if (1 - result < 1 && 1 - result > 0)
            {
                xrTableCell79.Text = "0" + result.ToString("#.##");
            }
            else if (result == 0)
            {
                xrTableCell79.Text = "0.00";
            }
            else
            {
                xrTableCell79.Text = result.ToString("#.##");
            }
        }

        private void xrTableCell130_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            decimal colSummary = (Decimal)xrTableCell129.Summary.GetResult();
            decimal totSummary = (Decimal)xrTableCell110.Summary.GetResult();
            Decimal result = (colSummary / totSummary * 100);

            if (totSummary == 0)
            {
                totSummary = totSummary + 1;
            }

            if (result.ToString("#.##").Equals("1"))
            {
                xrTableCell130.Text = "0.99";
            }
            else if (1 - result < 1 && 1 - result > 0)
            {
                xrTableCell130.Text = "0" + result.ToString("#.##");
            }
            else if (result == 0)
            {
                xrTableCell130.Text = "0.00";
            }
            else
            {
                xrTableCell130.Text = result.ToString("#.##");
            }
        }

        private void xrTableCell80_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            decimal colSummary = (Decimal)xrTableCell99.Summary.GetResult();
            decimal totSummary = (Decimal)xrTableCell110.Summary.GetResult();
            Decimal result = (colSummary / totSummary * 100);

            if (totSummary == 0)
            {
                totSummary = totSummary + 1;
            }

            if (result.ToString("#.##").Equals("1"))
            {
                xrTableCell80.Text = "0.99";
            }
            else if (1 - result < 1 && 1 - result > 0)
            {
                xrTableCell80.Text = "0" + result.ToString("#.##");
            }
            else if (result == 0)
            {
                xrTableCell80.Text = "0.00";
            }
            else
            {
                xrTableCell80.Text = result.ToString("#.##");
            }
        }

        private void xrTableCell81_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            decimal colSummary = (Decimal)xrTableCell100.Summary.GetResult();
            decimal totSummary = (Decimal)xrTableCell110.Summary.GetResult();
            Decimal result = (colSummary / totSummary * 100);

            if (totSummary == 0)
            {
                totSummary = totSummary + 1;
            }

            if (result.ToString("#.##").Equals("1"))
            {
                xrTableCell81.Text = "0.99";
            }
            else if (1 - result < 1 && 1 - result > 0)
            {
                xrTableCell81.Text = "0" + result.ToString("#.##");
            }
            else if (result == 0)
            {
                xrTableCell81.Text = "0.00";
            }
            else
            {
                xrTableCell81.Text = result.ToString("#.##");
            }
        }

        private void xrTableCell82_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            decimal colSummary = (Decimal)xrTableCell101.Summary.GetResult();
            decimal totSummary = (Decimal)xrTableCell110.Summary.GetResult();
            Decimal result = (colSummary / totSummary * 100);

            if (totSummary == 0)
            {
                totSummary = totSummary + 1;
            }

            if (result.ToString("#.##").Equals("1"))
            {
                xrTableCell82.Text = "0.99";
            }
            else if (1 - result < 1 && 1 - result > 0)
            {
                xrTableCell82.Text = "0" + result.ToString("#.##");
            }
            else if (result == 0)
            {
                xrTableCell82.Text = "0.00";
            }
            else
            {
                xrTableCell82.Text = result.ToString("#.##");
            }
        }

        private void xrTableCell83_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            decimal colSummary = (Decimal)xrTableCell102.Summary.GetResult();
            decimal totSummary = (Decimal)xrTableCell110.Summary.GetResult();
            Decimal result = (colSummary / totSummary * 100);

            if (totSummary == 0)
            {
                totSummary = totSummary + 1;
            }

            if (result.ToString("#.##").Equals("1"))
            {
                xrTableCell83.Text = "0.99";
            }
            else if (1 - result < 1 && 1 - result > 0)
            {
                xrTableCell83.Text = "0" + result.ToString("#.##");
            }
            else if (result == 0)
            {
                xrTableCell83.Text = "0.00";
            }
            else
            {
                xrTableCell83.Text = result.ToString("#.##");
            }
        }

        private void xrTableCell84_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            decimal colSummary = (Decimal)xrTableCell103.Summary.GetResult();
            decimal totSummary = (Decimal)xrTableCell110.Summary.GetResult();
            Decimal result = (colSummary / totSummary * 100);

            if (totSummary == 0)
            {
                totSummary = totSummary + 1;
            }

            if (result.ToString("#.##").Equals("1"))
            {
                xrTableCell84.Text = "0.99";
            }
            else if (1 - result < 1 && 1 - result > 0)
            {
                xrTableCell84.Text = "0" + result.ToString("#.##");
            }
            else if (result == 0)
            {
                xrTableCell84.Text = "0.00";
            }
            else
            {
                xrTableCell84.Text = result.ToString("#.##");
            }
        }

        private void xrTableCell85_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            decimal colSummary = (Decimal)xrTableCell104.Summary.GetResult();
            decimal totSummary = (Decimal)xrTableCell110.Summary.GetResult();
            Decimal result = (colSummary / totSummary * 100);

            if (totSummary == 0)
            {
                totSummary = totSummary + 1;
            }

            if (result.ToString("#.##").Equals("1"))
            {
                xrTableCell85.Text = "0.99";
            }
            else if (1 - result < 1 && 1 - result > 0)
            {
                xrTableCell85.Text = "0" + result.ToString("#.##");
            }
            else if (result == 0)
            {
                xrTableCell85.Text = "0.00";
            }
            else
            {
                xrTableCell85.Text = result.ToString("#.##");
            }
        }

        private void xrTableCell86_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            decimal colSummary = (Decimal)xrTableCell105.Summary.GetResult();
            decimal totSummary = (Decimal)xrTableCell110.Summary.GetResult();
            Decimal result = (colSummary / totSummary * 100);

            if (totSummary == 0)
            {
                totSummary = totSummary + 1;
            }

            if (result.ToString("#.##").Equals("1"))
            {
                xrTableCell86.Text = "0.99";
            }
            else if (1 - result < 1 && 1 - result > 0)
            {
                xrTableCell86.Text = "0" + result.ToString("#.##");
            }
            else if (result == 0)
            {
                xrTableCell86.Text = "0.00";
            }
            else
            {
                xrTableCell86.Text = result.ToString("#.##");
            }
        }

        private void xrTableCell87_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            decimal colSummary = (Decimal)xrTableCell106.Summary.GetResult();
            decimal totSummary = (Decimal)xrTableCell110.Summary.GetResult();
            Decimal result = (colSummary / totSummary * 100);

            if (totSummary == 0)
            {
                totSummary = totSummary + 1;
            }

            if (result.ToString("#.##").Equals("1"))
            {
                xrTableCell87.Text = "0.99";
            }
            else if (1 - result < 1 && 1 - result > 0)
            {
                xrTableCell87.Text = "0" + result.ToString("#.##");
            }
            else if (result == 0)
            {
                xrTableCell87.Text = "0.00";
            }
            else
            {
                xrTableCell87.Text = result.ToString("#.##");
            }
        }

        private void xrTableCell88_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            decimal colSummary = (Decimal)xrTableCell107.Summary.GetResult();
            decimal totSummary = (Decimal)xrTableCell110.Summary.GetResult();
            Decimal result = (colSummary / totSummary * 100);

            if (totSummary == 0)
            {
                totSummary = totSummary + 1;
            }

            if (result.ToString("#.##").Equals("1"))
            {
                xrTableCell88.Text = "0.99";
            }
            else if (1 - result < 1 && 1 - result > 0)
            {
                xrTableCell88.Text = "0" + result.ToString("#.##");
            }
            else if (result == 0)
            {
                xrTableCell88.Text = "0.00";
            }
            else
            {
                xrTableCell88.Text = result.ToString("#.##");
            }
        }

        private void xrTableCell58_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            decimal colSummary = (Decimal)xrTableCell57.Summary.GetResult();
            decimal totSummary = (Decimal)xrTableCell110.Summary.GetResult();
            Decimal result = (colSummary / totSummary * 100);

            if (totSummary == 0)
            {
                totSummary = totSummary + 1;
            }

            if (result.ToString("#.##").Equals("1"))
            {
                xrTableCell58.Text = "0.99";
            }
            else if (1 - result < 1 && 1 - result > 0)
            {
                xrTableCell58.Text = "0" + result.ToString("#.##");
            }
            else if (result == 0)
            {
                xrTableCell58.Text = "0.00";
            }
            else
            {
                xrTableCell58.Text = result.ToString("#.##");
            }
        }

        private void xrTableCell69_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            decimal colSummary = (Decimal)xrTableCell68.Summary.GetResult();
            decimal totSummary = (Decimal)xrTableCell110.Summary.GetResult();
            Decimal result = (colSummary / totSummary * 100);

            if (totSummary == 0)
            {
                totSummary = totSummary + 1;
            }

            if (result.ToString("#.##").Equals("1"))
            {
                xrTableCell69.Text = "0.99";
            }
            else if (1 - result < 1 && 1 - result > 0)
            {
                xrTableCell69.Text = "0" + result.ToString("#.##");
            }
            else if (result == 0)
            {
                xrTableCell69.Text = "0.00";
            }
            else
            {
                xrTableCell69.Text = result.ToString("#.##");
            }
        }

        private void xrTableCell113_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            decimal colSummary = (Decimal)xrTableCell112.Summary.GetResult();
            decimal totSummary = (Decimal)xrTableCell110.Summary.GetResult();
            Decimal result = (colSummary / totSummary * 100);

            if (totSummary == 0)
            {
                totSummary = totSummary + 1;
            }

            if (result.ToString("#.##").Equals("1"))
            {
                xrTableCell113.Text = "0.99";
            }
            else if (1 - result < 1 && 1 - result > 0)
            {
                xrTableCell113.Text = "0" + result.ToString("#.##");
            }
            else if (result == 0)
            {
                xrTableCell113.Text = "0.00";
            }
            else
            {
                xrTableCell113.Text = result.ToString("#.##");
            }
        }

        private void xrTableCell120_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            decimal colSummary = (Decimal)xrTableCell119.Summary.GetResult();
            decimal totSummary = (Decimal)xrTableCell110.Summary.GetResult();
            Decimal result = (colSummary / totSummary * 100);

            if (totSummary == 0)
            {
                totSummary = totSummary + 1;
            }

            if (result.ToString("#.##").Equals("1"))
            {
                xrTableCell120.Text = "0.99";
            }
            else if (1 - result < 1 && 1 - result > 0)
            {
                xrTableCell120.Text = "0" + result.ToString("#.##");
            }
            else if (result == 0)
            {
                xrTableCell120.Text = "0.00";
            }
            else
            {
                xrTableCell120.Text = result.ToString("#.##");
            }
        }

        private void xrTableCell116_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            decimal colSummary = (Decimal)xrTableCell115.Summary.GetResult();
            decimal totSummary = (Decimal)xrTableCell110.Summary.GetResult();
            Decimal result = (colSummary / totSummary * 100);

            if (totSummary == 0)
            {
                totSummary = totSummary + 1;
            }

            if (result.ToString("#.##").Equals("1"))
            {
                xrTableCell116.Text = "0.99";
            }
            else if (1 - result < 1 && 1 - result > 0)
            {
                xrTableCell116.Text = "0" + result.ToString("#.##");
            }
            else if (result == 0)
            {
                xrTableCell116.Text = "0.00";
            }
            else
            {
                xrTableCell116.Text = result.ToString("#.##");
            }
        }

        private void xrTableCell118_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            decimal colSummary = (Decimal)xrTableCell117.Summary.GetResult();
            decimal totSummary = (Decimal)xrTableCell110.Summary.GetResult();
            Decimal result = (colSummary / totSummary * 100);

            if (totSummary == 0)
            {
                totSummary = totSummary + 1;
            }

            if (result.ToString("#.##").Equals("1"))
            {
                xrTableCell118.Text = "0.99";
            }
            else if (1 - result < 1 && 1 - result > 0)
            {
                xrTableCell118.Text = "0" + result.ToString("#.##");
            }
            else if (result == 0)
            {
                xrTableCell118.Text = "0.00";
            }
            else
            {
                xrTableCell118.Text = result.ToString("#.##");
            }
        }

        private void xrTableCell122_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            decimal colSummary = (Decimal)xrTableCell121.Summary.GetResult();
            decimal totSummary = (Decimal)xrTableCell110.Summary.GetResult();
            Decimal result = (colSummary / totSummary * 100);

            if (totSummary == 0)
            {
                totSummary = totSummary + 1;
            }

            if (result.ToString("#.##").Equals("1"))
            {
                xrTableCell122.Text = "0.99";
            }
            else if (1 - result < 1 && 1 - result > 0)
            {
                xrTableCell122.Text = "0" + result.ToString("#.##");
            }
            else if (result == 0)
            {
                xrTableCell122.Text = "0.00";
            }
            else
            {
                xrTableCell122.Text = result.ToString("#.##");
            }
        }
    }
}
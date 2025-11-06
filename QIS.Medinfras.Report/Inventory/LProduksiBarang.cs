using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace QIS.Medinfras.Report
{
    public partial class LProduksiBarang : BaseDailyPortraitRpt
    {
        public LProduksiBarang()
        {
            InitializeComponent();
        }

        private int recordno;

        private void GroupHeader2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            recordno = 0;
        }

        private void GroupHeader1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            recordno++;
        }

        private void xrTableCell9_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            ((XRLabel)sender).Text = recordno.ToString();
        }


    }
}

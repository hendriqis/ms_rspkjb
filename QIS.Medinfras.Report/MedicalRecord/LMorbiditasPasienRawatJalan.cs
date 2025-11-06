using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace QIS.Medinfras.Report
{
    public partial class LMorbiditasPasienRawatJalan : BaseDailyLandscapeRpt
    {
        public LMorbiditasPasienRawatJalan()
        {
            InitializeComponent();
        }

        int recordNo=0;

        private void xrTableCell129_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            ((XRLabel)sender).Text = recordNo.ToString();
        }

        private void GroupHeader2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            recordNo++;
        }

    }
}

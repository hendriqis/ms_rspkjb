using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPembelianBarangLGTunai : BaseDailyPortraitRpt
    {
        public LPembelianBarangLGTunai()
        {
            InitializeComponent();
        }

        private int recordNo;

        private void GroupHeader1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            recordNo++;
        }

        private void GroupHeader2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            recordNo = 0;
        }

        private void xrTableCell15_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            ((XRLabel)sender).Text = recordNo.ToString();
        }



    }
}

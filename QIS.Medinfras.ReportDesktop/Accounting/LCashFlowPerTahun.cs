using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LCashFlowPerTahun : BaseCustomDailyLandscapeA3Rpt
    {
        public LCashFlowPerTahun()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            lblPeriode.Text = string.Format("Periode : {0}", param[0]);

            base.InitializeReport(param);
        }

        private void xrTableRow7_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableRow row = (XRTableRow)sender;
            bool isHeader = Convert.ToBoolean(GetCurrentColumnValue("isHeader"));
            if (isHeader)
            {
                row.Font = new Font("Tahoma", 7, FontStyle.Bold);
            }
            else
            {
                row.Font = new Font("Tahoma", 7, FontStyle.Regular);
            }
        }       
    }
}

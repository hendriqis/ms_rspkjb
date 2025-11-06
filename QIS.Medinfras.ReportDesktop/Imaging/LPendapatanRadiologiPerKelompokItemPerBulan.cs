using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPendapatanRadiologiPerKelompokItemPerBulan : BaseCustomDailyLandscapeRpt
    {
        private int _lineNumber = 0;

        public LPendapatanRadiologiPerKelompokItemPerBulan()
        {
            _lineNumber = 0;     
            InitializeComponent();
        }

        private void xrTableCell14_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrTableCell14.Text = (++_lineNumber).ToString();
        }

        private void xrTableCell26_AfterPrint(object sender, EventArgs e)
        {
            _lineNumber = 0;
        }
    }
}

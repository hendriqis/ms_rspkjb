using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LHarianProsesPiutangPasienPribadi : BaseDailyLandscapeRpt
    {
        int CountGroupNo = 0;
        int InvoiceIDHD;
        public LHarianProsesPiutangPasienPribadi()
        {
            InitializeComponent();
        }

        private void GroupNo_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            e.Result = CountGroupNo;
            e.Handled = true;
        }

        private void GroupNo_SummaryRowChanged(object sender, EventArgs e)
        {
            int ARNo = Convert.ToInt32(GetCurrentColumnValue("InvoiceIDHD"));
            if (ARNo != InvoiceIDHD)
            {
                InvoiceIDHD = ARNo;
                CountGroupNo++;
            }
        }

    }
}

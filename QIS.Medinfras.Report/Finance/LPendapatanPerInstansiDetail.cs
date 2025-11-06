using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace QIS.Medinfras.Report
{
    public partial class LPendapatanPerInstansiDetail : BaseDailyPortraitRpt
    {
        public LPendapatanPerInstansiDetail()
        {
            InitializeComponent();
        }
        int CountGroupNo = 0;
        int BusinessPartnerID;
        private void xrTableCell33_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            e.Result = CountGroupNo;
            e.Handled = true;
        }

        private void xrTableCell33_SummaryRowChanged(object sender, EventArgs e)
        {
            int BPID = Convert.ToInt32(GetCurrentColumnValue("BusinessPartnerID"));
            if (BPID != BusinessPartnerID)
            {
                BusinessPartnerID = BPID;
                CountGroupNo++;
            }
        }

    }
}

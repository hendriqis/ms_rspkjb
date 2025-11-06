using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace QIS.Medinfras.Report
{
    public partial class LPendapatanPerInstansiRekap : BaseDailyPortraitRpt
    {
        public LPendapatanPerInstansiRekap()
        {
            InitializeComponent();
        }
        int CountGroupNo = 0;
        int BusinessPartnerID;
        private void GroupNo_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            e.Result = CountGroupNo;
            e.Handled = true;
        }

        private void GroupNo_SummaryRowChanged(object sender, EventArgs e)
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

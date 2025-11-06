using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPenerimaanPerInstansiRekap : BaseDailyPortraitRpt
    {
        int CountGroupNo = 0;
        int BusinessPartnerID;

        public LPenerimaanPerInstansiRekap()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
            base.InitializeReport(param);
        }

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

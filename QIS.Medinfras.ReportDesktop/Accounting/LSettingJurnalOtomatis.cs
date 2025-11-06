using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LSettingJurnalOtomatis : BaseDailyPortraitRpt
    {
        public LSettingJurnalOtomatis()
        {
            InitializeComponent();
        }

        private void GroupFooter2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string curr = this.GetCurrentColumnValue("JournalGroup").ToString();
            string next = this.GetNextColumnValue("JournalGroup").ToString();
            if (curr != next)
            {
                this.GroupFooter2.PageBreak = DevExpress.XtraReports.UI.PageBreak.AfterBand;
            }
            else
            {
                this.GroupFooter2.PageBreak = DevExpress.XtraReports.UI.PageBreak.None;
            }
        }
    }
}

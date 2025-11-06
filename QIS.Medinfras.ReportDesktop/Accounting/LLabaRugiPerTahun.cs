using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LLabaRugiPerTahun : BaseCustomDailyLandscapeA3Rpt
    {
        public LLabaRugiPerTahun()
        {
            InitializeComponent();
        }

        private void lblGLAccountNo_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRControl control = (XRControl)sender;
            int level = Convert.ToInt32(GetCurrentColumnValue("Level"));
            control.Padding = new DevExpress.XtraPrinting.PaddingInfo(level * 10, 0, 0, 0);
        }

        private void xrTableRow7_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableRow row = (XRTableRow)sender;
            bool isHeader = Convert.ToBoolean(GetCurrentColumnValue("IsHeader"));
            int isVisible = Convert.ToInt32(GetCurrentColumnValue("IsVisible"));
            if (isHeader)
            {
                row.Font = new Font("Tahoma", 7, FontStyle.Bold);
            }
            else
            {
                row.Font = new Font("Tahoma", 7, FontStyle.Regular);
            }

            if (isVisible == 0)
            {
                xrTableRow7.Visible = false;
            }
        }   
    }
}

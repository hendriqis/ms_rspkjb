using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using System.Data;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPerolehanAktivaTetapRpt : BaseDailyPortraitRpt
    {
        public LPerolehanAktivaTetapRpt()
        {
            InitializeComponent();
        }

        private void lblGLAccountNo_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRControl control = (XRControl)sender;
            int level = Convert.ToInt32(GetCurrentColumnValue("Level"));
            control.Padding = new DevExpress.XtraPrinting.PaddingInfo(level * 10, 0, 0, 0);
        }

        private void lblBalanceEND_AfterPrint(object sender, EventArgs e)
        {
            XRControl control = (XRControl)sender;
            Decimal balanceEnd = Convert.ToDecimal(GetCurrentColumnValue("BalanceEND"));
            if (balanceEnd == 0) lblBalanceEND.Visible = false;
        }

        private void xrTableRow2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableRow row = (XRTableRow)sender;
            bool isHeader = Convert.ToBoolean(GetCurrentColumnValue("IsHeader"));
            if (isHeader)
                row.Font = new Font("Tahoma", 9, FontStyle.Bold);
            else
                row.Font = new Font("Tahoma", 9, FontStyle.Regular);
        }        
    }
}

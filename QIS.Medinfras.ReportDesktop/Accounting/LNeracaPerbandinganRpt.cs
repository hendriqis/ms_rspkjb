using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using System.Data;
using QIS.Medinfras.Data.Service.Helper;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LNeracaPerbandinganRpt : BaseDailyPortraitRpt
    {
        public LNeracaPerbandinganRpt()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            int year = Convert.ToInt32(param[1]);
            int month = Convert.ToInt32(param[2]);
            int year2 = Convert.ToInt32(param[3]);
            int month2 = Convert.ToInt32(param[4]);

            DateTime date1 = new DateTime(year, month, 1).AddMonths(1).AddDays(-1);
            DateTime date2 = new DateTime(year2, month2, 1).AddMonths(1).AddDays(-1);
            lblYearMonth.Text = String.Format("{0}",date1.ToString(Constant.FormatString.DATE_FORMAT));
            lblYearMonth2.Text = String.Format("{0}", date2.ToString(Constant.FormatString.DATE_FORMAT));
            base.InitializeReport(param);
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

        private void xrTable2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableRow row = (XRTableRow)sender;
            bool isHeader = Convert.ToBoolean(GetCurrentColumnValue("IsHeader"));
            if (isHeader)
                row.Font = new Font("Tahoma", 8, FontStyle.Bold);
            else
                row.Font = new Font("Tahoma", 8, FontStyle.Regular);
        }        
    }
}

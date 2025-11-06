using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using System.Data;
using System.Linq;
using System.Globalization;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LNeraca : BaseDailyPortraitRpt
    {

        List<GetGLBalancePerPeriodPerLevel> lst = new List<GetGLBalancePerPeriodPerLevel>();

        public LNeraca()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            //string monthName = Enumerable.Range(1, 12).Select(a => new
            //{
            //    MonthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(a),
            //    MonthNumber = a
            //}).FirstOrDefault(p => p.MonthNumber == Convert.ToInt32(param[2])).MonthName;

            //lblPeriod.Text = string.Format("Period : {0} {1}", monthName, param[1]);

            lst = BusinessLayer.GetGLBalancePerPeriodPerLevelList("001", Convert.ToInt32(param[1]), Convert.ToInt32(param[2]), Convert.ToInt32(param[3]), Convert.ToInt32(param[4]));

            String result = "";
            if (param[2] == "1")
            {
                result = "Januari";
            }
            else if (param[2] == "2")
            {
                result = "Februari";
            }
            else if (param[2] == "3")
            {
                result = "Maret";
            }
            else if (param[2] == "4")
            {
                result = "April";
            }
            else if (param[2] == "5")
            {
                result = "Mei";
            }
            else if (param[2] == "6")
            {
                result = "Juni";
            }
            else if (param[2] == "7")
            {
                result = "Juli";
            }
            else if (param[2] == "8")
            {
                result = "Agustus";
            }
            else if (param[2] == "9")
            {
                result = "September";
            }
            else if (param[2] == "10")
            {
                result = "Oktober";
            }
            else if (param[2] == "11")
            {
                result = "November";
            }
            else if (param[2] == "12")
            {
                result = "Desember";
            }
            else if (param[2] == "13")
            {
                result = "Desember [Audited]";
            }

            Int32 lastDay = 0;
            if (param[2] == "13")
            {
                lastDay = DateTime.DaysInMonth(Convert.ToInt32(param[1]), 12);
            }
            else
            {
                lastDay = DateTime.DaysInMonth(Convert.ToInt32(param[1]), Convert.ToInt32(param[2]));
            }
            lblPeriod.Text = string.Format("Per {0} {1} {2}", lastDay, result, param[1]);
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

        private void xrTableRow2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableRow row = (XRTableRow)sender;
            bool isHeader = Convert.ToBoolean(GetCurrentColumnValue("IsHeader"));
            if (isHeader)
                row.Font = new Font("Tahoma", 8, FontStyle.Bold);
            else
                row.Font = new Font("Tahoma", 8, FontStyle.Regular);
        }

        private void xrLabel3_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String accountType = Convert.ToString(GetCurrentColumnValue("GCGLAccountType"));
            List<GetGLBalancePerPeriodPerLevel> lstTemp = lst.Where(t => t.GCGLAccountType == accountType && t.AccountLevel == 1).ToList();
            Decimal amount = lstTemp.Sum(x => x.BalanceEND);

            if (amount < 0)
            {
                amount = amount * -1;
                xrLabel3.Text = string.Format("({0})", amount.ToString("N2"));
            }
            else
            {
                xrLabel3.Text = string.Format("{0}", amount.ToString("N2"));
            }
        }

    }
}

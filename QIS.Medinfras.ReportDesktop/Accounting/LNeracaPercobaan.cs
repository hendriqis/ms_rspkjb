using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Linq;
using System.Globalization;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LNeracaPercobaan : BaseDailyPortraitRpt
    {
        public LNeracaPercobaan()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string monthName = "";

            if (param[2] == "13")
            {
                monthName = "Desember [Audited]";
            }
            else
            {
                monthName = Enumerable.Range(1, 12).Select(a => new
                 {
                     MonthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(a),
                     MonthNumber = a
                 }).FirstOrDefault(p => p.MonthNumber == Convert.ToInt32(param[2])).MonthName;
            }

            lblPeriod.Text = string.Format("Period : {0} {1}", monthName, param[1]);
            base.InitializeReport(param);
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
    }
}

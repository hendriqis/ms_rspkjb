using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPerolehanAktivaTetap : BaseDailyPortraitRpt
    {
        public LPerolehanAktivaTetap()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lblPeriode.Text = string.Format("Periode Pembelian : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));

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
                row.Font = new Font("Tahoma", 9, FontStyle.Bold);
            else
                row.Font = new Font("Tahoma", 9, FontStyle.Regular);
        }        
    }
}

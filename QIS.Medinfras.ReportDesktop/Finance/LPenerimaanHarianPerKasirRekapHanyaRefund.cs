using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPenerimaanHarianPerKasirRekapHanyaRefund : BaseCustomDailyLandscapeA3Rpt
    {
        private int _lineNumber = 0;

        public LPenerimaanHarianPerKasirRekapHanyaRefund()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));

            if (param[3].ToString() == "1")
            {
                lblStatus.Text = "Status Transaksi : NON VOID";
            }
            else {
                lblStatus.Text = "Status Transaksi : VOID";            
            }

            _lineNumber = 0;

            base.InitializeReport(param);
        }

        private void xrTableCell9_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrTableCell9.Text = (++_lineNumber).ToString();
        }

        private void xrTableCell40_AfterPrint(object sender, EventArgs e)
        {
            _lineNumber = 0;
        }

    }
}

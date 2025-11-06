using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPenerimaanHarianKasirBros : BaseCustomDailyLandscapeRpt
    {
        public LPenerimaanHarianKasirBros()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));

            if (param[3].ToString() == "1")
            {
                xrLabel6.Text = "Status Transaksi : NON VOID";
            }
            else
            {
                xrLabel6.Text = "Status Transaksi : VOID";
            }

            base.InitializeReport(param);
        }

        private void xrTable2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if ((GetCurrentColumnValue("GCTransactionStatus") != null))
            {
                String GCTransactionStatus = GetCurrentColumnValue("GCTransactionStatus").ToString();

                if (GCTransactionStatus == Constant.TransactionStatus.VOID)
                {
                    xrTable2.ForeColor = Color.Red;
                }
                else
                {
                    xrTable2.ForeColor = Color.Black;
                }
            }
        }

    }
}

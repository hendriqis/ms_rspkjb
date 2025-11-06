using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPenerimaanKasirPerMetodePembayaran : BaseCustomDailyLandscapeRpt
    {
        public LPenerimaanKasirPerMetodePembayaran()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            string[] temp2 = param[1].Split(';');
            lblPeriode.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
            lblPeriodeTime.Text = string.Format("Jam : {0} s/d {1}", (temp2[0]).ToString(), (temp2[1]).ToString());
            //if (param[3] == "1")
            //{
            //    lblStatusTransaksi.Text = string.Format("Status Transaksi : NON VOID");
            //}
            //else
            //{
            //    lblStatusTransaksi.Text = string.Format("Status Transaksi : VOID");
            //}

            //lblPrintDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT);
            //lblCreatedByName.Text = appSession.UserFullName;

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

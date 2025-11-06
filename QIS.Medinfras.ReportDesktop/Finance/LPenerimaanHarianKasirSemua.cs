using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Linq;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPenerimaanHarianKasirSemua : BaseDailyLandscapeRpt
    {

        private struct PaymentInfo
        {
            public double CashAmount { get; set; }
            public double CardAmount { get; set; }
            public double CardFeeAmount { get; set; }
            public double CreditAmount { get; set; }
            public double DiscAmount { get; set; }
            public double DownPaymentAmount { get; set; }
        }

        private List<PaymentInfo> lstVoidTrx = new List<PaymentInfo>();
        private List<PaymentInfo> lstValidTrx = new List<PaymentInfo>();

        public LPenerimaanHarianKasirSemua()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
            base.InitializeReport(param);
        }

        private void xrTableCell12_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (GetCurrentRow() != null)
            {
                vPatientPaymentHdReportCustom entity = (vPatientPaymentHdReportCustom)GetCurrentRow();
                if (entity.GCTransactionStatus == Constant.TransactionStatus.VOID)
                {
                    xrTableCell12.ForeColor = Color.Red;
                    xrTableCell12.Text += "*";
                }
                else xrTableCell12.ForeColor = Color.Black;
            }
        }

        private void xrTableCell17_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            PaymentInfo payment = new PaymentInfo();
            payment.CashAmount = Convert.ToDouble(GetCurrentColumnValue("Cash"));
            payment.CardAmount = Convert.ToDouble(GetCurrentColumnValue("Card"));
            payment.CardFeeAmount = Convert.ToDouble(GetCurrentColumnValue("TotalFeeAmount"));
            payment.CreditAmount = Convert.ToDouble(GetCurrentColumnValue("Piutang"));
            payment.DiscAmount = Convert.ToDouble(GetCurrentColumnValue("Diskon"));
            payment.DownPaymentAmount = Convert.ToDouble(GetCurrentColumnValue("UangMukaKeluar"));

            int status = Convert.ToInt32(GetCurrentColumnValue("VoidStatus"));
            if (status == 1)
                lstVoidTrx.Add(payment);
            else
                lstValidTrx.Add(payment);

        }

        private void lblGTCash_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

            double validAmount = lstValidTrx.Sum(lst => lst.CashAmount);
            double voidAmount = lstVoidTrx.Sum(lst => lst.CashAmount);

            lblGTCash.Text = (validAmount - voidAmount).ToString("N2");

        }

        private void lblGTCards_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

            double validAmount = lstValidTrx.Sum(lst => lst.CardAmount);
            double voidAmount = lstVoidTrx.Sum(lst => lst.CardAmount);

            lblGTCards.Text = (validAmount - voidAmount).ToString("N2");

        }

        private void lblGTCardsFee_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

            double validAmount = lstValidTrx.Sum(lst => lst.CardFeeAmount);
            double voidAmount = lstVoidTrx.Sum(lst => lst.CardFeeAmount);

            lblGTCardsFee.Text = (validAmount - voidAmount).ToString("N2");

        }

        private void lblGTCredit_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

            double validAmount = lstValidTrx.Sum(lst => lst.CreditAmount);
            double voidAmount = lstVoidTrx.Sum(lst => lst.CreditAmount);

            lblGTCredit.Text = (validAmount - voidAmount).ToString("N2");

        }

        private void lblGTDisc_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

            double validAmount = lstValidTrx.Sum(lst => lst.DiscAmount);
            double voidAmount = lstVoidTrx.Sum(lst => lst.DiscAmount);

            lblGTDisc.Text = (validAmount - voidAmount).ToString("N2");

        }

        private void lblGTDownPayment_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

            double validAmount = lstValidTrx.Sum(lst => lst.DownPaymentAmount);
            double voidAmount = lstVoidTrx.Sum(lst => lst.DownPaymentAmount);

            lblGTDownPayment.Text = (validAmount - voidAmount).ToString("N2");

        }

    }
}

using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BCorporatePaymentSlipNHS : BaseCustomDailyPotraitRpt
    {
        public BCorporatePaymentSlipNHS()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');

            vARReceivingHd entity = BusinessLayer.GetvARReceivingHdList(temp[0]).FirstOrDefault();
            cInstansi.Text = entity.BusinessPartnerName;
            cNoPembayaran.Text = entity.ARReceivingNo;
            cTanggalPembayaran.Text = entity.ReceivingDateInString;
            cKeterangan.Text = entity.Remarks;

            #region Receive DT
            List<vARReceivingDt> lstDT = BusinessLayer.GetvARReceivingDtList(string.Format(
                "ARReceivingID = {0} AND IsDeleted = 0", entity.ARReceivingID));

            decimal cash = 0;
            decimal creditCard = 0;
            decimal debitCard = 0;
            decimal transfer = 0;
            decimal biayaadmin = 0;
            decimal diskon = 0;
            decimal writeoff = 0;
            decimal pph23 = 0;

            cash += lstDT.Where(a => a.GCARPaymentMethod == Constant.AR_PAYMENT_METHODS.CASH).Sum(a => a.PaymentAmount + a.CardFeeAmount);
            creditCard += lstDT.Where(a => a.GCARPaymentMethod == Constant.AR_PAYMENT_METHODS.CREDIT_CARD).Sum(a => a.PaymentAmount + a.CardFeeAmount);
            debitCard += lstDT.Where(a => a.GCARPaymentMethod == Constant.AR_PAYMENT_METHODS.DEBIT_CARD).Sum(a => a.PaymentAmount + a.CardFeeAmount);
            transfer += lstDT.Where(a => a.GCARPaymentMethod == Constant.AR_PAYMENT_METHODS.BANK_TRANSFER).Sum(a => a.PaymentAmount + a.CardFeeAmount);
            biayaadmin += lstDT.Where(a => a.GCARPaymentMethod == Constant.AR_PAYMENT_METHODS.BIAYA_ADMIN).Sum(a => a.PaymentAmount + a.CardFeeAmount);
            diskon += lstDT.Where(a => a.GCARPaymentMethod == Constant.AR_PAYMENT_METHODS.DISKON).Sum(a => a.PaymentAmount + a.CardFeeAmount);
            writeoff += lstDT.Where(a => a.GCARPaymentMethod == Constant.AR_PAYMENT_METHODS.WRITE_OFF).Sum(a => a.PaymentAmount + a.CardFeeAmount);
            pph23 += lstDT.Where(a => a.GCARPaymentMethod == Constant.AR_PAYMENT_METHODS.PPH23).Sum(a => a.PaymentAmount + a.CardFeeAmount);

            cCash.Text = cash.ToString(Constant.FormatString.NUMERIC_2);
            cCreditCard.Text = creditCard.ToString(Constant.FormatString.NUMERIC_2);
            cDebitCard.Text = debitCard.ToString(Constant.FormatString.NUMERIC_2);
            cBankTransfer.Text = transfer.ToString(Constant.FormatString.NUMERIC_2);
            cAdmin.Text = biayaadmin.ToString(Constant.FormatString.NUMERIC_2);
            cDiscount.Text = diskon.ToString(Constant.FormatString.NUMERIC_2);
            cWriteOff.Text = writeoff.ToString(Constant.FormatString.NUMERIC_2);
            cPPH23.Text = pph23.ToString(Constant.FormatString.NUMERIC_2);
            #endregion

            cTTDCorporate.Text = entity.BusinessPartnerName;
            cTTDPrintedBy.Text = AppSession.UserLogin.UserFullName;

            string[] invoice = new String[] { temp[1] };
            base.InitializeReport(invoice);
        }

    }
}

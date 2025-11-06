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
    public partial class BNewPatientBillSummaryNHSNew : BaseRpt
    {
        public BNewPatientBillSummaryNHSNew()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            //hideHeader();

            vRegistration entityReg = BusinessLayer.GetvRegistrationList(param[0]).FirstOrDefault();
            Registration entityRegLinked = BusinessLayer.GetRegistrationList(string.Format("LinkedRegistrationID = {0}", entityReg.RegistrationID)).FirstOrDefault();
            Healthcare entityHealthcare = BusinessLayer.GetHealthcare(appSession.HealthcareID);

            List<GetPatientChargesHdDtALLNHS> lstHDDT = BusinessLayer.GetPatientChargesHdDtALLNHSList(entityReg.RegistrationID);

            List<vPatientBill> lstBill = BusinessLayer.GetvPatientBillList(string.Format("RegistrationID = {0} AND GCTransactionStatus != '{1}'",
                entityReg.RegistrationID, Constant.TransactionStatus.VOID));

            List<vPatientPaymentHd> lstPayment = BusinessLayer.GetvPatientPaymentHdList(string.Format("RegistrationID IN ({0}) AND GCTransactionStatus != '{1}'",
                entityReg.RegistrationID, Constant.TransactionStatus.VOID));
            List<vPatientPaymentHdDt> lstPaymentHdDt = BusinessLayer.GetvPatientPaymentHdDtList(string.Format("RegistrationID IN ({0}) AND GCTransactionStatus != '{1}'",
                entityReg.RegistrationID, Constant.TransactionStatus.VOID));

            #region Transaction
            subTransaction.CanGrow = true;
            billPaymentSummaryTransaction.InitializeReport(lstHDDT, param[0]);
            #endregion

            #region Registration All
            subDetailRegistration.CanGrow = true;
            billPaymentDetailRegistrationAllNHSNew.InitializeReport(lstHDDT, param[0]);
            #endregion

            #region Payment All
            subPaymentAll.CanGrow = true;
            billPaymentDetailPaymentAll.InitializeReport(lstPaymentHdDt);
            #endregion

            #region Summary Left

            decimal cash = 0;
            decimal debitCard = 0;
            decimal creditCard = 0;
            decimal transfer = 0;
            decimal credit = 0;
            decimal voucher = 0;
            decimal downPayment = 0;
            decimal refund = 0;

            foreach (vPatientPaymentHd hd in lstPayment)
            {
                List<PatientPaymentDt> lstDT = BusinessLayer.GetPatientPaymentDtList(string.Format("PaymentID = {0} AND IsDeleted = 0", hd.PaymentID));
                cash += lstDT.Where(a => a.GCPaymentMethod == Constant.PaymentMethod.CASH && a.PaymentAmount > 0).Sum(a => a.PaymentAmount) - hd.CashBackAmount;
                debitCard += lstDT.Where(a => a.GCPaymentMethod == Constant.PaymentMethod.DEBIT_CARD && a.PaymentAmount > 0).Sum(a => a.PaymentAmount);
                creditCard += lstDT.Where(a => a.GCPaymentMethod == Constant.PaymentMethod.CREDIT_CARD && a.PaymentAmount > 0).Sum(a => a.PaymentAmount);
                transfer += lstDT.Where(a => a.GCPaymentMethod == Constant.PaymentMethod.BANK_TRANSFER && a.PaymentAmount > 0).Sum(a => a.PaymentAmount);
                credit += lstDT.Where(a => a.GCPaymentMethod == Constant.PaymentMethod.CREDIT && a.PaymentAmount > 0).Sum(a => a.PaymentAmount);
                voucher += lstDT.Where(a => a.GCPaymentMethod == Constant.PaymentMethod.VOUCHER && a.PaymentAmount > 0).Sum(a => a.PaymentAmount);
                refund += lstDT.Where(a => a.PaymentAmount < 0).Sum(a => a.PaymentAmount);
            }

            refund = refund * -1;

            foreach (vPatientPaymentHd hd in lstPayment.Where(b => b.GCPaymentType == Constant.PaymentType.DOWN_PAYMENT))
            {
                downPayment += hd.TotalPaymentAmount;
            }

            cCash.Text = cash.ToString("N2");
            cDebitCard.Text = debitCard.ToString("N2");
            cCreditCard.Text = creditCard.ToString("N2");
            cTransferBank.Text = transfer.ToString("N2");
            cVoucher.Text = voucher.ToString("N2");
            cDownPayment.Text = downPayment.ToString("N2");
            cRefund.Text = refund.ToString("N2");
            cAccountReceivable.Text = credit.ToString("N2");

            #endregion

            #region Summary Right

            decimal transP = lstHDDT.Sum(p => p.PatientAmount);
            decimal transC = lstHDDT.Sum(p => p.PayerAmount);

            decimal adminP = lstBill.Sum(p => p.PatientAdminFeeAmount);
            decimal adminC = lstBill.Sum(p => p.AdministrationFeeAmount);

            decimal coverP = 0;
            decimal coverC = lstBill.Sum(p => p.CoverageAmount);

            decimal billP = lstBill.Sum(p => p.TotalPatientAmount);
            decimal billC = lstBill.Sum(p => p.TotalPayerAmount);

            decimal discP = lstBill.Sum(p => p.PatientDiscountAmount);
            decimal discC = lstBill.Sum(p => p.PayerDiscountAmount);

            decimal temp = transC + adminC;
            decimal amountP = 0;
            decimal amountC = 0;
            if (temp > coverC)
            {
                amountP = temp - coverC;
                amountC = coverC;
            }
            else
            {
                amountC = temp;
            }
            decimal gtP = transP + adminP + amountP - discP;
            decimal gtC = amountC - discC;

            decimal uangMuka = lstPayment.Sum(a => a.DownPaymentOut);

            decimal paymentP = lstPayment.Where(p => p.GCPaymentType == Constant.PaymentType.SETTLEMENT || p.GCPaymentType == Constant.PaymentType.CUSTOM || p.GCPaymentType == Constant.PaymentType.DOWN_PAYMENT).Sum(p => p.NotInDownPayment)
                                - uangMuka;
            decimal paymentC = 0;

            decimal balanceP = gtP - paymentP;
            decimal balanceC = gtC - paymentC;

            cTotalTransactionP.Text = transP.ToString("N2");
            cTotalTransactionC.Text = transC.ToString("N2");

            cCoverageP.Text = coverP.ToString("N2");
            cCoverageC.Text = coverC.ToString("N2");

            cBillP.Text = billP.ToString("N2");
            cBillC.Text = billC.ToString("N2");

            if (discP == 0 && discC == 0)
            {
                lblDiskon.Visible = false;
                cDiscP.Visible = false;
                cDiscC.Visible = false;
            }
            else
            {
                cDiscP.Text = discP.ToString("N2");
                cDiscC.Text = discC.ToString("N2");
            }

            cGrandTotalP.Text = gtP.ToString("N2");
            cGrandTotalC.Text = gtC.ToString("N2");

            cPaymentP.Text = paymentP.ToString("N2");
            cPaymentC.Text = paymentC.ToString("N2");

            cBalanceP.Text = balanceP.ToString("N2");
            cBalanceC.Text = balanceC.ToString("N2");

            #endregion

            #region Footer
            cTTDPatient.Text = entityReg.PatientName;
            cTTDPrintedBy.Text = appSession.UserFullName;
            #endregion

            base.InitializeReport(param);
        }
    }
}

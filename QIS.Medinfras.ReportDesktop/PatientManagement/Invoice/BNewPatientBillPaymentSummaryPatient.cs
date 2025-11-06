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
    public partial class BNewPatientBillPaymentSummaryPatient : BaseCustomDailyPotraitRpt
    {
        public BNewPatientBillPaymentSummaryPatient()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vRegistration entityReg = BusinessLayer.GetvRegistrationList(param[0]).FirstOrDefault();
            //Registration entityRegLinked = BusinessLayer.GetRegistrationList(string.Format("LinkedRegistrationID = {0} AND GCRegistrationStatus != '{1}'", entityReg.RegistrationID, Constant.VisitStatus.CANCELLED)).FirstOrDefault();
            Registration entityRegLinkedTo = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0} AND GCRegistrationStatus != '{1}'", entityReg.LinkedToRegistrationID, Constant.VisitStatus.CANCELLED)).FirstOrDefault();
            PatientPaymentHd entityPayment = BusinessLayer.GetPatientPaymentHdList(param[1]).FirstOrDefault();
            Healthcare entityHealthcare = BusinessLayer.GetHealthcare(appSession.HealthcareID);

            List<GetPatientChargesHdDtALL> lstHDDT = BusinessLayer.GetPatientChargesHdDtALLList(entityReg.RegistrationID);

            List<vPatientBill> lstBill = BusinessLayer.GetvPatientBillList(string.Format("RegistrationID = {0} AND GCTransactionStatus != '{1}'",
                entityReg.RegistrationID, Constant.TransactionStatus.VOID));

            List<vPatientPaymentHd> lstPayment = BusinessLayer.GetvPatientPaymentHdList(string.Format("RegistrationID IN ({0}) AND GCTransactionStatus != '{1}'",
                entityReg.RegistrationID, Constant.TransactionStatus.VOID));
            List<vPatientPaymentHdDt> lstPaymentHdDt = BusinessLayer.GetvPatientPaymentHdDtList(string.Format("RegistrationID IN ({0}) AND GCTransactionStatus != '{1}'",
                entityReg.RegistrationID, Constant.TransactionStatus.VOID));

            lblCompany.Text = string.Format("{0} ( NPWP : {1} )", entityHealthcare.CompanyName, entityHealthcare.TaxRegistrantNo);

            #region Header : Patient Detail
            cPatientName.Text = entityReg.PatientName;
            cDOB.Text = entityReg.DateOfBirthInString;
            cRegisteredPhysician.Text = entityReg.ParamedicName;

            if (entityReg.ReferrerParamedicID != null && entityReg.ReferrerParamedicID != 0)
            {
                cReferrerPhysician.Text = entityReg.ReferrerParamedicName;
            }
            else
            {
                if (entityReg.ReferrerID != null && entityReg.ReferrerID != 0)
                {
                    cReferrerPhysician.Text = entityReg.ReferrerName;
                }
                else
                {
                    cReferrerPhysician.Text = "";
                }
            }

            if (entityReg.OldMedicalNo != "" && entityReg.OldMedicalNo != null)
            {
                cMedicalNo.Text = string.Format("{0} ({1})", entityReg.MedicalNo, entityReg.OldMedicalNo);
            }
            else
            {
                cMedicalNo.Text = string.Format("{0}", entityReg.MedicalNo);
            }

            cServiceUnitClass.Text = string.Format("{0} | {1}", entityReg.ServiceUnitName, entityReg.ClassName);
            cRoomBed.Text = string.Format("{0} | {1}", entityReg.RoomName, entityReg.BedCode);
            cDischargeDate.Text = entityReg.cfDischargeDateInString;

            if (entityRegLinkedTo != null)
            {
                cNotes.Text = string.Format("Transfer Rawat Inap ({0})", entityRegLinkedTo.RegistrationNo);
            }
            else
            {
                cNotes.Text = "-";
            }
            #endregion

            #region Header : Per Page
            cHeaderPatient.Text = string.Format("{0} | {1} | {2} yr {3} mnth {4} day",
                entityReg.PatientName, entityReg.Gender, entityReg.AgeInYear, entityReg.AgeInMonth, entityReg.AgeInDay);
            cHeaderRegistration.Text = entityReg.RegistrationNo;
            cHeaderMedicalNo.Text = entityReg.MedicalNo;
            #endregion

            #region Transaction
            if (entityPayment.GCPaymentType != Constant.PaymentType.DOWN_PAYMENT)
            {
                lblTransaction.Visible = true;
                subTransaction.CanGrow = true;
                billPaymentSummaryTransactionPatient.InitializeReport(lstHDDT);
            }
            else
            {
                lblTransaction.Visible = false;
            }
            #endregion

            #region Discount
            subDiscount.CanGrow = true;
            billPaymentDetailDiscountAll.InitializeReport(entityReg.RegistrationID);
            #endregion

            #region Payment
            subPayment.CanGrow = true;
            billPaymentDetailPayment.InitializeReport(lstPaymentHdDt.Where(t => t.PaymentID == entityPayment.PaymentID).ToList());
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

            decimal adminP = lstBill.Sum(p => p.PatientAdminFeeAmount);

            decimal coverP = 0;

            decimal billP = lstBill.Sum(p => p.TotalPatientAmount);

            decimal discP = lstBill.Sum(p => p.PatientDiscountAmount);

            decimal amountP = 0;

            decimal gtP = transP + adminP + amountP - discP;

            decimal uangMuka = lstPayment.Sum(a => a.DownPaymentOut);

            decimal paymentP = lstPayment.Where(p => p.GCPaymentType == Constant.PaymentType.SETTLEMENT || p.GCPaymentType == Constant.PaymentType.CUSTOM || p.GCPaymentType == Constant.PaymentType.DOWN_PAYMENT).Sum(p => p.NotInDownPayment)
                                - uangMuka;

            decimal balanceP = gtP - paymentP;

            cTotalTransactionP.Text = transP.ToString("N2");

            cAdminP.Text = adminP.ToString("N2");

            cCoverageP.Text = coverP.ToString("N2");

            cBillP.Text = billP.ToString("N2");

            if (discP == 0)
            {
                lblDiskonHeader.Visible = false;
                subDiscount.Visible = false;
                lblDiskon.Visible = false;
                cDiscP.Visible = false;
            }
            else
            {
                cDiscP.Text = discP.ToString("N2");
            }

            cGrandTotalP.Text = gtP.ToString("N2");

            cPaymentP.Text = paymentP.ToString("N2");

            cBalanceP.Text = balanceP.ToString("N2");

            #endregion

            #region Footer
            cTTDPatient.Text = entityReg.PatientName;
            cTTDPrintedBy.Text = appSession.UserFullName;
            #endregion

            base.InitializeReport(param);
        }
    }
}

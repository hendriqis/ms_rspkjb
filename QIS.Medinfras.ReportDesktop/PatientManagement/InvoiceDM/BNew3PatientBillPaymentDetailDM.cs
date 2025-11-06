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
    public partial class BNew3PatientBillPaymentDetailDM : BaseCustomDailyPotraitRpt
    {
        public BNew3PatientBillPaymentDetailDM()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vRegistration entityReg = BusinessLayer.GetvRegistrationList(param[0])[0];
            RegistrationBPJS entityRegBPJS = BusinessLayer.GetRegistrationBPJSList(String.Format("RegistrationID = {0}", entityReg.RegistrationID)).FirstOrDefault();
            PatientPaymentHd entityPayment = BusinessLayer.GetPatientPaymentHdList(param[1])[0];
            Healthcare entityHealthcare = BusinessLayer.GetHealthcare(appSession.HealthcareID);

            List<GetPatientChargesHdDtAllPerBilling> lstHDDT = BusinessLayer.GetPatientChargesHdDtAllPerBillingList(entityReg.RegistrationID, entityPayment.PaymentID);

            int paymentID = 0;
            if (entityPayment.GCPaymentType != Constant.PaymentType.DOWN_PAYMENT)
            {
                paymentID = entityPayment.PaymentID;
            }
            List<PatientBillPayment> lstEntityBillPayment = BusinessLayer.GetPatientBillPaymentList(string.Format("PaymentID = {0}", paymentID));

            List<vPatientPaymentHd> lstPaymentAll = BusinessLayer.GetvPatientPaymentHdList(string.Format("RegistrationID IN ({0}) AND GCTransactionStatus != '{1}'",
                entityReg.RegistrationID, Constant.TransactionStatus.VOID));
            List<vPatientPaymentHd> lstPayment = lstPaymentAll.Where(t => t.PaymentID == entityPayment.PaymentID).ToList();
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

            if (entityRegBPJS != null)
            {
                cSEPNo.Text = entityRegBPJS.NoSEP;
            }
            else
            {
                cSEPNoCaption.Visible = false;
                xrTableCell9.Visible = false;
                cSEPNo.Text = "";
                cSEPNo.Visible = false;
            }

            cServiceUnitClass.Text = string.Format("{0} | {1}", entityReg.ServiceUnitName, entityReg.ClassName);
            cRoomBed.Text = string.Format("{0} | {1}", entityReg.RoomName, entityReg.BedCode);
            cDischargeDate.Text = entityReg.cfDischargeDateInString;
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
                billPaymentDetailTransactionByPaymentDM.InitializeReport(lstHDDT);
            }
            else
            {
                lblTransaction.Visible = false;
            }
            #endregion

            #region Discount
            string billID = "";
            foreach (PatientBillPayment pbp in lstEntityBillPayment)
            {
                billID += "," + pbp.PatientBillingID;
            }
            subDiscount.CanGrow = true;
            billPaymentDetailDiscountByBillingDM.InitializeReport(entityReg.RegistrationID, billID);
            #endregion

            #region Payment All
            subPaymentAll.CanGrow = true;
            billPaymentDetailPaymentAllDM.InitializeReport(lstPaymentHdDt);
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
            //decimal depositin = 0;
            decimal depositout = 0;
            decimal depositout2 = 0;
            decimal depositinFinal = 0;

            foreach (vPatientPaymentHd hd in lstPayment)
            {
                List<PatientPaymentDt> lstDT = BusinessLayer.GetPatientPaymentDtList(string.Format("PaymentID = {0} AND IsDeleted = 0", hd.PaymentID));
                cash += lstDT.Where(a => a.GCPaymentMethod == Constant.PaymentMethod.CASH && a.PaymentAmount > 0 && hd.GCPaymentType != Constant.PaymentType.DEPOSIT_IN).Sum(a => a.PaymentAmount) - hd.CashBackAmount;
                debitCard += lstDT.Where(a => a.GCPaymentMethod == Constant.PaymentMethod.DEBIT_CARD && a.PaymentAmount > 0 && hd.GCPaymentType != Constant.PaymentType.DEPOSIT_IN).Sum(a => a.PaymentAmount);
                creditCard += lstDT.Where(a => a.GCPaymentMethod == Constant.PaymentMethod.CREDIT_CARD && a.PaymentAmount > 0 && hd.GCPaymentType != Constant.PaymentType.DEPOSIT_IN).Sum(a => a.PaymentAmount);
                transfer += lstDT.Where(a => a.GCPaymentMethod == Constant.PaymentMethod.BANK_TRANSFER && a.PaymentAmount > 0 && hd.GCPaymentType != Constant.PaymentType.DEPOSIT_IN).Sum(a => a.PaymentAmount);
                credit += lstDT.Where(a => a.GCPaymentMethod == Constant.PaymentMethod.CREDIT && a.PaymentAmount > 0 && hd.GCPaymentType != Constant.PaymentType.DEPOSIT_IN).Sum(a => a.PaymentAmount);
                voucher += lstDT.Where(a => a.GCPaymentMethod == Constant.PaymentMethod.VOUCHER && a.PaymentAmount > 0 && hd.GCPaymentType != Constant.PaymentType.DEPOSIT_IN).Sum(a => a.PaymentAmount);
                refund += lstDT.Where(a => a.GCPaymentMethod != Constant.PaymentMethod.DEPOSIT_OUT && a.GCPaymentMethod != Constant.PaymentMethod.DOWN_PAYMENT && a.PaymentAmount > 0 && hd.GCPaymentType == Constant.PaymentType.SETTLEMENT).Sum(a => a.PaymentAmount);
                depositout2 += lstDT.Where(a => a.GCPaymentMethod == Constant.PaymentMethod.DEPOSIT_OUT && a.PaymentAmount > 0 && hd.GCPaymentType != Constant.PaymentType.DEPOSIT_IN).Sum(a => a.PaymentAmount);
            }


            List<GetPatientDepositIn> lstDepositIN = BusinessLayer.GetPatientDepositIn(entityReg.RegistrationID, entityReg.RegistrationID);

            //refund = refund * -1;

            foreach (vPatientPaymentHd hd in lstPayment.Where(b => b.GCPaymentType == Constant.PaymentType.DOWN_PAYMENT))
            {
                downPayment += hd.TotalPaymentAmount;
            }
            foreach (vPatientPaymentHd hd in lstPayment.Where(b => b.GCPaymentType == Constant.PaymentType.DEPOSIT_IN))
            {
                //depositin += hd.TotalPaymentAmount;
            }
            foreach (vPatientPaymentHd hd in lstPayment.Where(b => b.GCPaymentType == Constant.PaymentType.DEPOSIT_OUT))
            {
                depositout += hd.TotalPaymentAmount;
            }

            //depositinFinal = depositin + lstDepositIN.FirstOrDefault().Deposit;
            if (lstDepositIN.Count != 0)
            {
                depositinFinal = lstDepositIN.FirstOrDefault().Deposit;
            }
            else
            {
                depositinFinal = 0;
            }

            cCash.Text = cash.ToString("N2");
            cDebitCard.Text = debitCard.ToString("N2");
            cCreditCard.Text = creditCard.ToString("N2");
            cTransferBank.Text = transfer.ToString("N2");
            cVoucher.Text = voucher.ToString("N2");
            cDownPayment.Text = downPayment.ToString("N2");
            cRefund.Text = refund.ToString("N2");
            cAccountReceivable.Text = credit.ToString("N2");
            //cDepositIn.Text = depositin.ToString("N2");
            cDepositIn.Text = depositinFinal.ToString("N2");
            cDepositOut.Text = depositout.ToString("N2");
            cDepositOut2.Text = depositout2.ToString("N2");

            #endregion

            #region Summary Right

            decimal transP = 0;
            decimal transC = 0;
            decimal adminP = 0;
            decimal adminC = 0;
            decimal coverP = 0;
            decimal coverC = 0;
            decimal grouperP = 0;
            decimal grouperC = entityReg.BPJSAmount;
            decimal billP = 0;
            decimal billC = 0;
            decimal discP = 0;
            decimal discC = 0;
            //decimal temp = 0;
            //decimal amountP = 0;
            //decimal amountC = 0;
            decimal gtP = 0;
            decimal gtC = 0;
            decimal paymentP = 0;
            decimal paymentC = 0;
            decimal balanceP = 0;
            decimal balanceC = 0;
            decimal roundingP = 0;
            decimal roundingC = 0;
            foreach (PatientBillPayment pbp in lstEntityBillPayment)
            {
                List<vPatientBill> lstBill = BusinessLayer.GetvPatientBillList(string.Format("PatientBillingID = {0} AND GCTransactionStatus != '{1}'",
                    pbp.PatientBillingID, Constant.TransactionStatus.VOID));

                adminP += lstBill.Sum(p => p.PatientAdminFeeAmount);
                adminC += lstBill.Sum(p => p.AdministrationFeeAmount);

                coverP += 0;
                coverC += lstBill.Sum(p => p.CoverageAmount);

                billP += lstBill.Sum(p => p.TotalPatientAmount);
                billC += lstBill.Sum(p => p.TotalPayerAmount);

                discP += lstBill.Sum(p => p.PatientDiscountAmount);
                discC += lstBill.Sum(p => p.PayerDiscountAmount);
                roundingP += lstBill.Sum(p => p.PatientRoundingAmount);
                roundingC += lstBill.Sum(p => p.PayerRoundingAmount);

                //temp = transC + adminC;
                //if (temp > coverC)
                //{
                //    amountP += temp - coverC;
                //    amountC += coverC;
                //}
                //else
                //{
                //    amountC += temp;
                //}
            }

            transP += lstHDDT.Sum(p => p.PatientAmount);
            transC += lstHDDT.Sum(p => p.PayerAmount);

            gtP = billP - discP;
            gtC = billC - discC;

            decimal uangMuka = lstPayment.Sum(a => a.DownPaymentOut);

            paymentP = lstPayment.Where(p => p.GCPaymentType == Constant.PaymentType.SETTLEMENT || p.GCPaymentType == Constant.PaymentType.CUSTOM || p.GCPaymentType == Constant.PaymentType.DOWN_PAYMENT).Sum(p => p.NotInDownPayment);
            paymentC = 0;

            if (paymentP != 0)
            {
                List<SettingParameterDt> lstRounding = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                                        "ParameterCode IN ('{0}','{1}','{2}')",
                                                                        Constant.SettingParameter.FN_IS_ALLOW_ROUNDING_AMOUNT,
                                                                        Constant.SettingParameter.FN_NILAI_PEMBULATAN_TAGIHAN,
                                                                        Constant.SettingParameter.FN_PEMBULATAN_TAGIHAN_KE_ATAS));

                string isAllowRounding = lstRounding.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_ALLOW_ROUNDING_AMOUNT).FirstOrDefault().ParameterValue;
                string nilaiPembulatanString = lstRounding.Where(a => a.ParameterCode == Constant.SettingParameter.FN_NILAI_PEMBULATAN_TAGIHAN).FirstOrDefault().ParameterValue;
                string pembulatanKeAtas = lstRounding.Where(a => a.ParameterCode == Constant.SettingParameter.FN_PEMBULATAN_TAGIHAN_KE_ATAS).FirstOrDefault().ParameterValue;

                roundingP = 0;
                roundingC = 0;

                if (isAllowRounding == "1")
                {
                    decimal totalTemp = billP;
                    decimal nilaiPembulatan = Convert.ToDecimal(nilaiPembulatanString);
                    decimal sisaPembulatanTemp = totalTemp % nilaiPembulatan;

                    if (pembulatanKeAtas == "1")
                    {
                        roundingP = nilaiPembulatan - sisaPembulatanTemp;
                    }
                    else
                    {
                        roundingP = sisaPembulatanTemp * -1;
                    }

                    if (roundingP % nilaiPembulatan == 0)
                    {
                        roundingP = 0;
                    }
                }
            }

            billP += roundingP;
            billC += roundingC;

            gtP += roundingP;
            gtC += roundingC;

            balanceP = gtP - paymentP;
            balanceC = gtC - paymentC;

            cTotalTransactionP.Text = transP.ToString("N2");
            cTotalTransactionC.Text = transC.ToString("N2");

            cAdminP.Text = adminP.ToString("N2");
            cAdminC.Text = adminC.ToString("N2");

            cCoverageP.Text = coverP.ToString("N2");
            cCoverageC.Text = coverC.ToString("N2");

            cGrouperP.Text = grouperP.ToString("N2");
            cGrouperC.Text = grouperC.ToString("N2");

            cRoundingP.Text = roundingP.ToString("N2");
            cRoundingC.Text = roundingC.ToString("N2");

            cBillP.Text = billP.ToString("N2");
            cBillC.Text = billC.ToString("N2");

            if (discP == 0 && discC == 0)
            {
                lblDiskonHeader.Visible = false;
                subDiscount.Visible = false;
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

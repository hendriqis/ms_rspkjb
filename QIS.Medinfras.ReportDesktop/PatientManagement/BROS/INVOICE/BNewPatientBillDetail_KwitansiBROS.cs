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
    public partial class BNewPatientBillDetail_KwitansiBROS : BaseCustomDailyPotraitRpt
    {
        public BNewPatientBillDetail_KwitansiBROS()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vRegistration entityReg = BusinessLayer.GetvRegistrationList(param[0]).FirstOrDefault();
            //Registration entityRegLinked = BusinessLayer.GetRegistrationList(string.Format("LinkedRegistrationID = {0} AND GCRegistrationStatus != '{1}'", entityReg.RegistrationID, Constant.VisitStatus.CANCELLED)).FirstOrDefault();
            Registration entityRegLinkedTo = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0} AND GCRegistrationStatus != '{1}'", entityReg.LinkedToRegistrationID, Constant.VisitStatus.CANCELLED)).FirstOrDefault();
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID)).FirstOrDefault();

            List<GetPatientChargesHdDtALLWithOutDP> lstData;
            lstData = BusinessLayer.GetPatientChargesHdDtALLWithOutDPList(entityReg.RegistrationID);
            List<GetPatientChargesHdDtALLWithOutDP> lstHDDT = BusinessLayer.GetPatientChargesHdDtALLWithOutDPList(entityReg.RegistrationID);
            List<GetPatientChargesHdDtALLWithOutDP> lstDiscount = lstData.Where(t => t.GroupTransaction != "Non-DP").ToList();

            List<PatientBill> lstBill = BusinessLayer.GetPatientBillList(string.Format("RegistrationID = {0} AND GCTransactionStatus != '{1}'",
                entityReg.RegistrationID, Constant.TransactionStatus.VOID));
            List<vPatientPaymentHd> lstPayment = BusinessLayer.GetvPatientPaymentHdList(string.Format("RegistrationID IN ({0}) AND GCTransactionStatus != '{1}' ORDER BY PaymentDate",
                entityReg.RegistrationID, Constant.TransactionStatus.VOID));
            List<vPatientPaymentHdDt> lstPaymentHdDt = BusinessLayer.GetvPatientPaymentHdDtList(string.Format("RegistrationID IN ({0}) AND GCTransactionStatus != '{1}'",
                entityReg.RegistrationID, Constant.TransactionStatus.VOID));
            List<GetPaymentReceiptCustomBROS> lstPaymentReceipt = BusinessLayer.GetPaymentReceiptCustomBROSList(entityReg.RegistrationID);


            string corporate = "";
            string filterRegPayer = string.Format("RegistrationID = {0} AND IsDeleted = 0 ORDER BY IsPrimaryPayer DESC, BusinessPartnerID ASC", entityReg.RegistrationID);
            List<vRegistrationPayer> lstRegPayer = BusinessLayer.GetvRegistrationPayerList(filterRegPayer);
            foreach (vRegistrationPayer rp in lstRegPayer)
            {
                if (rp.BusinessPartnerOldCode != null && rp.BusinessPartnerOldCode != "")
                {
                    if (corporate != "")
                    {
                        corporate += ", " + "(" + rp.BusinessPartnerOldCode + "*) " + rp.BusinessPartnerName;
                    }
                    else
                    {
                        corporate = "(" + rp.BusinessPartnerOldCode + "*) " + rp.BusinessPartnerName;
                    }
                }
                else
                {
                    if (corporate != "")
                    {
                        corporate += ", " + "(" + rp.BusinessPartnerCode + ") " + rp.BusinessPartnerName;
                    }
                    else
                    {
                        corporate = "(" + rp.BusinessPartnerCode + ") " + rp.BusinessPartnerName;
                    }
                }
            }

            if (corporate == "")
            {
                if (entityReg.BusinessPartnerID != 1)
                {
                    if (entityReg.BusinessPartnerOldCode != null && entityReg.BusinessPartnerOldCode != "")
                    {
                        corporate = "(" + entityReg.BusinessPartnerOldCode + "*) " + entityReg.BusinessPartnerName;
                    }
                    else
                    {
                        corporate = "(" + entityReg.BusinessPartnerCode + ") " + entityReg.BusinessPartnerName;
                    }
                }
                else
                {
                    corporate = entityReg.BusinessPartnerName;
                }
            }

            lblCompany.Text = string.Format("{0} ( NPWP : {1} )", entityHealthcare.CompanyName, entityHealthcare.TaxRegistrantNo);

            #region Header : Patient Detail
            cPatientName.Text = string.Format("{0} ({1})", entityReg.cfPatientNameSalutation, entityReg.cfGenderInitial);//entityReg.cfPatientNameSalutation;
            lblPatientName.Text = entityReg.cfPatientNameSalutation;
            cDOB.Text = string.Format("{0} ({1} Tahun {2} Bulan {3} Hari)", entityReg.DateOfBirthInString, entityReg.AgeInYear, entityReg.AgeInMonth, entityReg.AgeInDay);//entityReg.DateOfBirthInString;
            cRegisteredPhysician.Text = entityReg.ParamedicName;

            cCorporate.Text = corporate;

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

            string planDischarge = entityReg.PlanDischargeDate.ToString(Constant.FormatString.DATE_FORMAT);
            string discharge = entityReg.DischargeDate.ToString(Constant.FormatString.DATE_FORMAT);
            string dischargeDateInfo = "-";

            if (entityReg.DischargeDate != null && discharge != "01-Jan-1900")
            {
                dischargeDateInfo = discharge + " " + entityReg.DischargeTime;
            }
            else
            {
                if (entityReg.PlanDischargeDate != null && planDischarge != "01-Jan-1900")
                {
                    dischargeDateInfo = planDischarge + "(*)";
                }
                else
                {
                    if (entityReg.PhysicianDischargedDate != null && entityReg.PhysicianDischargedDate.ToString(Constant.FormatString.DATE_FORMAT) != "01-Jan-1900")
                    {
                        dischargeDateInfo = entityReg.PhysicianDischargedDate.ToString(Constant.FormatString.DATE_FORMAT) + " " + entityReg.PhysicianDischargedDate.ToString(Constant.FormatString.TIME_FORMAT);
                    }
                }
            }

            cDischargeDate.Text = dischargeDateInfo;

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
            subTransaction.CanGrow = true;
            billPaymentDetailTransactionBROS.InitializeReport(lstHDDT);
            #endregion

            #region Discount
            subDiscount.CanGrow = true;
            billPaymentDetailDiscountAllDM.InitializeReport(entityReg.RegistrationID);
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

            decimal transfixP = lstHDDT.Sum(p => p.PatientAmount);
            decimal transdiscP = lstDiscount.Sum(p => p.PatientAmount) * -1;
            decimal transfixC = lstHDDT.Sum(p => p.PayerAmount);
            decimal transdiscC = lstDiscount.Sum(p => p.PayerAmount) * -1;

            decimal transP = transfixP + transdiscP;
            decimal transC = transfixC + transdiscC;

            decimal adminP = lstBill.Sum(p => p.PatientAdminFeeAmount);
            decimal adminC = lstBill.Sum(p => p.AdministrationFeeAmount);

            decimal coverP = 0;
            decimal coverC = lstBill.Sum(p => p.CoverageAmount);

            decimal roundingP = lstPayment.Sum(a => a.PatientRoundingAmount);
            decimal roundingC = lstPayment.Sum(a => a.PayerRoundingAmount);

            decimal billfixP = lstBill.Sum(p => p.TotalPatientAmount);
            decimal billDiscP = lstDiscount.Sum(p => p.PatientAmount) * -1;
            decimal billfixC = lstBill.Sum(p => p.TotalPayerAmount);
            decimal billDiscC = lstDiscount.Sum(p => p.PayerAmount) * -1;

            decimal billP = 0;
            decimal billC = 0;

            if (lstBill.Count > 0)
            {
                billP = billfixP + billDiscP;
                billC = billfixC + billDiscC;
            }

            decimal discP = lstBill.Sum(p => p.PatientDiscountAmount);
            decimal discC = lstBill.Sum(p => p.PayerDiscountAmount);
            decimal discGroupP = lstDiscount.Sum(p => p.PatientAmount) * -1;
            decimal discGroupC = lstDiscount.Sum(p => p.PayerAmount) * -1;

            decimal discfixP = discP + discGroupP;
            decimal discfixC = discC + discGroupC;

            //decimal temp = transC + adminC;
            decimal temp = billC + adminC;
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
            //decimal gtP = transP + adminP + amountP - discP;
            decimal gtP = 0;
            decimal gtC = 0;
            if (lstBill.Count > 0)
            {
                gtP = billP - discP - discGroupP;
                gtC = billC - discC - discGroupC;
            }
            else
            {
                gtP = transP - discP - discGroupP;
                gtC = transC - discC - discGroupC;
            }

            decimal uangMuka = lstPayment.Sum(a => a.DownPaymentOut);

            decimal paymentP = lstPayment.Where(p => p.GCPaymentType == Constant.PaymentType.SETTLEMENT || p.GCPaymentType == Constant.PaymentType.CUSTOM || p.GCPaymentType == Constant.PaymentType.DOWN_PAYMENT).Sum(p => p.NotInDownPayment)
                                - uangMuka;
            decimal paymentC = 0;

            if (paymentP == 0)
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

            decimal balanceP = gtP - paymentP;
            decimal balanceC = gtC - paymentC;

            cTotalTransactionP.Text = transP.ToString("N2");
            cTotalTransactionC.Text = transC.ToString("N2");

            cAdminP.Text = adminP.ToString("N2");
            cAdminC.Text = adminC.ToString("N2");

            cCoverageP.Text = coverP.ToString("N2");
            cCoverageC.Text = coverC.ToString("N2");

            cBillP.Text = billP.ToString("N2");
            cBillC.Text = billC.ToString("N2");

            cRoundingP.Text = roundingP.ToString("N2");
            cRoundingC.Text = roundingC.ToString("N2");

            if (discP == 0 && discC == 0)
            {
                if (discGroupP == 0 && discGroupC == 0)
                {
                    lblDiskonHeader.Visible = false;
                    subDiscount.Visible = false;
                    lblDiskon.Visible = false;
                    cDiscP.Visible = false;
                    cDiscC.Visible = false;
                }
                else
                {
                    cDiscP.Text = discfixP.ToString("N2");
                    cDiscC.Text = discfixC.ToString("N2");
                }
            }
            else
            {
                cDiscP.Text = discfixP.ToString("N2");
                cDiscC.Text = discfixC.ToString("N2");
            }

            cGrandTotalP.Text = gtP.ToString("N2");
            cGrandTotalC.Text = gtC.ToString("N2");

            cPaymentP.Text = paymentP.ToString("N2");
            cPaymentC.Text = paymentC.ToString("N2");

            cBalanceP.Text = balanceP.ToString("N2");
            cBalanceC.Text = balanceC.ToString("N2");

            #endregion

            #region Payment All
            subPaymentAllDM.CanGrow = true;
            billPaymentDetailPaymentAllDMBROS.InitializeReport(lstPaymentHdDt);
            #endregion

            #region Footer
            lblTanggal.Text = string.Format("{0}, {1}", entityHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT));
            lblTandaTangan.Text = appSession.UserFullName;
            #endregion

            #region Receipt All
            subReceipt.CanGrow = true;
            billPaymentReceiptBROS.InitializeReport(lstPaymentReceipt);
            #endregion

            base.InitializeReport(param);
        }
    }
}

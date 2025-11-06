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
    public partial class BNewPatientBillSummaryAllBillRSPM : BaseCustom3DailyPotraitRpt
    {
        public BNewPatientBillSummaryAllBillRSPM()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vRegistration entityReg = BusinessLayer.GetvRegistrationList(param[0]).FirstOrDefault();
            Registration entityRegLinked = BusinessLayer.GetRegistrationList(string.Format("LinkedRegistrationID = {0} AND GCRegistrationStatus != '{1}'", entityReg.RegistrationID, Constant.VisitStatus.CANCELLED)).FirstOrDefault();
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID)).FirstOrDefault();

            List<GetPatientChargesHdDtAllBillRSPM> lstHDDT = BusinessLayer.GetPatientChargesHdDtAllBillRSPMList(entityReg.RegistrationID);

            List<PatientBill> lstBill = BusinessLayer.GetPatientBillList(string.Format("RegistrationID = {0} AND GCTransactionStatus != '{1}'",
                entityReg.RegistrationID, Constant.TransactionStatus.VOID));

            List<vPatientPaymentHd> lstPayment = BusinessLayer.GetvPatientPaymentHdList(string.Format("RegistrationID IN ({0}) AND GCTransactionStatus != '{1}'",
                entityReg.RegistrationID, Constant.TransactionStatus.VOID));
            List<vPatientPaymentHdDtRSPM> lstPaymentHdDt = BusinessLayer.GetvPatientPaymentHdDtRSPMList(string.Format("RegistrationID IN ({0}) AND GCTransactionStatus != '{1}'",
                entityReg.RegistrationID, Constant.TransactionStatus.VOID));

            string corporate = "";
            if (entityReg.BusinessPartnerID != 1)
            {
                string filterRegPayer = string.Format("RegistrationID = {0} AND IsDeleted = 0 ORDER BY IsPrimaryPayer DESC, BusinessPartnerID ASC", entityReg.RegistrationID);
                List<vRegistrationPayer> lstRegPayer = BusinessLayer.GetvRegistrationPayerList(filterRegPayer);
                foreach (vRegistrationPayer rp in lstRegPayer)
                {
                    //if (lstRegPayer.Where(a => a.GCCustomerType == Constant.CustomerType.BPJS).ToList().Count() > 0)
                    //{
                    //    corporate = "(" + lstRegPayer.Where(a => a.GCCustomerType == Constant.CustomerType.BPJS).FirstOrDefault().BusinessPartnerCode + "*) " + lstRegPayer.Where(a => a.GCCustomerType == Constant.CustomerType.BPJS).FirstOrDefault().BusinessPartnerName;
                    //}
                    //else
                    //{
                    if (corporate != "")
                    {
                        corporate += ", " + "(" + rp.BusinessPartnerCode + ") " + rp.BusinessPartnerName;
                    }
                    else
                    {
                        corporate = "(" + rp.BusinessPartnerCode + ") " + rp.BusinessPartnerName;
                    }
                    //}
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
            cPatientName.Text = entityReg.cfPatientNameSalutation;
            cDOB.Text = entityReg.DateOfBirthInString;
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

            if (entityReg.DischargeDate.Date.ToString(Constant.FormatString.DATE_FORMAT_112) != "19000101")
            {
                dischargeDateInfo = discharge;
            }
            else
            {
                if (entityReg.PlanDischargeDate.Date.ToString(Constant.FormatString.DATE_FORMAT_112) != "19000101")
                {
                    dischargeDateInfo = planDischarge + "(*)";
                }
            }

            cDischargeDate.Text = dischargeDateInfo;

            if (entityRegLinked != null)
            {
                cNotes.Text = string.Format("Transfer Rawat Inap ({0})", entityRegLinked.RegistrationNo);
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
            billPaymentSummaryTransactionAllBillRSPM.InitializeReport(lstHDDT);
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

            decimal transP = lstHDDT.Sum(p => p.PatientAmount);
            decimal transC = lstHDDT.Sum(p => p.PayerAmount);

            decimal adminP = lstBill.Sum(p => p.PatientAdminFeeAmount);
            decimal adminC = lstBill.Sum(p => p.AdministrationFeeAmount);

            decimal serviceP = lstBill.Sum(p => p.PatientServiceFeeAmount);
            decimal serviceC = lstBill.Sum(p => p.ServiceFeeAmount);

            decimal coverC = lstBill.Sum(p => p.CoverageAmount);

            decimal roundingP = lstPayment.Sum(a => a.PatientRoundingAmount);
            decimal roundingC = lstPayment.Sum(a => a.PayerRoundingAmount);

            decimal billP = lstBill.Sum(p => p.TotalPatientAmount) + roundingP;
            decimal billC = lstBill.Sum(p => p.TotalPayerAmount) + roundingC;

            decimal discP = lstBill.Sum(p => p.PatientDiscountAmount);
            decimal discC = lstBill.Sum(p => p.PayerDiscountAmount);

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
            decimal gtP = billP - discP;
            decimal gtC = amountC - discC;

            decimal uangMuka = lstPayment.Sum(a => a.DownPaymentOut);

            //ditutup 20200109 oleh RN
            decimal paymentP = lstPayment.Where(p => p.GCPaymentType == Constant.PaymentType.SETTLEMENT || p.GCPaymentType == Constant.PaymentType.CUSTOM || p.GCPaymentType == Constant.PaymentType.DOWN_PAYMENT).Sum(p => p.NotInDownPayment)
                                - uangMuka;

            //decimal paymentP = lstPayment.Where(p => p.GCPaymentType != Constant.PaymentType.DOWN_PAYMENT
            //                                            && p.GCPaymentType != Constant.PaymentType.AR_PAYER
            //                                            && p.GCPaymentType != Constant.PaymentType.DEPOSIT_IN
            //                                    ).Sum(p => (p.TotalPaymentAmount - p.CashBackAmount));
            decimal paymentC = 0;

            if ((lstPayment.Where(p => p.GCPaymentType == Constant.PaymentType.SETTLEMENT || p.GCPaymentType == Constant.PaymentType.CUSTOM).Sum(p => p.NotInDownPayment)
                                - uangMuka) == 0)
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
                    decimal totalTemp = transP - discP;
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

            gtP += roundingP;
            gtC += roundingC;

            decimal balanceP = gtP - paymentP;
            decimal balanceC = gtC - paymentC;

            cTotalTransactionAll.Text = (transP + transC).ToString("N2");

            cAdminAll.Text = (adminP + adminC).ToString("N2");

            cServiceAll.Text = (serviceP + serviceC).ToString("N2");

            cCoverageLimit.Text = coverC.ToString("N2");

            if (discP == 0 && discC == 0)
            {
                lblDiskonHeader.Visible = false;
                subDiscount.Visible = false;
            }
            else
            {
                cDisc.Text = (discP + discC).ToString("N2");
            }
            cTotalRounding.Text = (roundingP + roundingC).ToString("N2");
            decimal total = (transP + adminP + serviceP + transC + adminC + serviceC - (discP + discC) - coverC);
            cTotal.Text = (total + roundingP + roundingC).ToString("N2");
            #endregion

            #region Payment All
            subPaymentAllDM.CanGrow = true;
            billPaymentDetailPaymentAllDM.InitializeReport(lstPaymentHdDt);
            #endregion

            #region tax Amount
            decimal dpp = 0;
            decimal ppn = 0;
            if (entityReg.DepartmentID == Constant.Facility.PHARMACY || entityReg.DepartmentID == Constant.Facility.MEDICAL_CHECKUP)
            {
                dpp = lstHDDT.Where(t => !String.IsNullOrEmpty(t.GCPrescriptionType)).Sum(t => t.LineAmount) / Convert.ToDecimal(1.11);
                ppn = dpp * Convert.ToDecimal(0.11);
            }
            else
            {
                dpp = lstHDDT.Where(t => t.GCPrescriptionType == Constant.PrescriptionType.DISCHARGE_PRESCRIPTION).Sum(t => t.LineAmount) / Convert.ToDecimal(1.11);
                ppn = dpp * Convert.ToDecimal(0.11);
            }

            cDPP.Text = dpp.ToString("N2");
            cPPN.Text = ppn.ToString("N2");

            if (entityReg.DepartmentID == Constant.Facility.INPATIENT)
            {
                xrTable1.Visible = false;
            }
            #endregion

            #region Footer
            lblTanggal.Text = string.Format("{0}, {1}", entityHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT));
            lblTandaTangan.Text = appSession.UserFullName;
            #endregion

            //#region Discount Comp2 Custom RSSES
            //subDiscountComp2.CanGrow = true;
            //billPaymentDetailDiscComp2TransactionAllBillRSSES.InitializeReport(lstHDDT.Where(a => a.cfDiscountAmountComp2 != 0).ToList());
            //#endregion

            base.InitializeReport(param);
        }
    }
}

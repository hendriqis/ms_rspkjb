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
    public partial class BNewPatientBillDetailWithoutDiscBROS : BaseCustomDailyPotraitRpt
    {
        public BNewPatientBillDetailWithoutDiscBROS()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vRegistration entityReg = BusinessLayer.GetvRegistrationList(param[0]).FirstOrDefault();
            //Registration entityRegLinked = BusinessLayer.GetRegistrationList(string.Format("LinkedRegistrationID = {0} AND GCRegistrationStatus != '{1}'", entityReg.RegistrationID, Constant.VisitStatus.CANCELLED)).FirstOrDefault();
            Registration entityRegLinkedTo = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0} AND GCRegistrationStatus != '{1}'", entityReg.LinkedToRegistrationID, Constant.VisitStatus.CANCELLED)).FirstOrDefault();
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID)).FirstOrDefault();

            List<GetPatientChargesHdDtALLWithOutDP> lstHDDT = BusinessLayer.GetPatientChargesHdDtALLWithOutDPList(entityReg.RegistrationID);

            List<PatientBill> lstBill = BusinessLayer.GetPatientBillList(string.Format("RegistrationID = {0} AND GCTransactionStatus != '{1}'",
                entityReg.RegistrationID, Constant.TransactionStatus.VOID));
            List<vPatientPaymentHd> lstPayment = BusinessLayer.GetvPatientPaymentHdList(string.Format("RegistrationID IN ({0}) AND GCTransactionStatus != '{1}' ORDER BY PaymentDate",
                entityReg.RegistrationID, Constant.TransactionStatus.VOID));
            List<vPatientPaymentHdDt> lstPaymentHdDt = BusinessLayer.GetvPatientPaymentHdDtList(string.Format("RegistrationID IN ({0}) AND GCTransactionStatus != '{1}' AND GCPaymentType = '{2}'",
                entityReg.RegistrationID, Constant.TransactionStatus.VOID, Constant.PaymentType.AR_PAYER));

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
            billPaymentDetailTransactionWithoutDiscBROS.InitializeReport(lstHDDT);
            #endregion

            #region Summary Right

            decimal transP = lstHDDT.Sum(p => p.PatientAmount);
            decimal transC = lstHDDT.Sum(p => p.TransactionAmount);

            decimal adminC = lstBill.Sum(p => p.AdministrationFeeAmount);
            decimal adminP = 0;

            decimal coverP = 0;
            decimal coverC = lstBill.Sum(p => p.CoverageAmount);

            decimal roundingC = lstPayment.Sum(a => a.PayerRoundingAmount);
            decimal roundingP = 0;

            decimal billC = lstBill.Sum(p => p.TotalPayerAmount);
            decimal billP = 0;

            decimal discC = lstBill.Sum(p => p.PayerDiscountAmount);
            decimal discP = 0;

            //decimal temp = transC + adminC;
            decimal temp = billC + adminC;
            decimal amountC = 0;
            if (temp > coverC)
            {
                amountC = coverC;
            }
            else
            {
                amountC = temp;
            }
            decimal gtC = billC - discC;
            decimal gtP = 0;

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

                roundingC = 0;
                roundingP = 0;

                //if (isAllowRounding == "1")
                //{
                //    decimal nilaiPembulatan = Convert.ToDecimal(nilaiPembulatanString);
                //    //decimal sisaPembulatanTemp = totalTemp % nilaiPembulatan;

                //    //if (pembulatanKeAtas == "1")
                //    //{
                //    //    roundingP = nilaiPembulatan - sisaPembulatanTemp;
                //    //}
                //    //else
                //    //{
                //    //    roundingP = sisaPembulatanTemp * -1;
                //    //}

                //    //if (roundingP % nilaiPembulatan == 0)
                //    //{
                //    //    roundingP = 0;
                //    //}
                //}
            }

            billP += roundingP;
            billC += roundingC;

            gtP += roundingP;
            gtC += roundingC;

            decimal balanceP = 0;
            decimal balanceC = gtC - paymentC;

            cTotalTransactionP.Text = transP.ToString("N2");
            cTotalTransactionC.Text = transC.ToString("N2");

            cAdminP.Text = "0.00";
            cAdminC.Text = adminC.ToString("N2");

            cCoverageP.Text = "0.00";
            cCoverageC.Text = coverC.ToString("N2");

            cBillP.Text = "0.00";
            cBillC.Text = billC.ToString("N2");

            cRoundingP.Text = "0.00";
            cRoundingC.Text = roundingC.ToString("N2");

            if (discC == 0)
            {
                lblDiskon.Visible = false;
                cDiscP.Visible = false;
                cDiscC.Visible = false;
            }
            else
            {
                cDiscP.Text = "0.00";
                cDiscC.Text = discC.ToString("N2");
            }

            cGrandTotalP.Text = "0.00";
            cGrandTotalC.Text = gtC.ToString("N2");

            cPaymentP.Text = paymentP.ToString("N2");
            cPaymentC.Text = paymentC.ToString("N2");

            xrTableCell48.Visible = false;
            cPaymentP.Visible = false;
            cPaymentC.Visible = false;

            cBalanceP.Text = "0.00";
            cBalanceC.Text = balanceC.ToString("N2");
            #endregion

            #region Payment All
            subPaymentAllDM.CanGrow = true;
            billPaymentDetailPaymentAllDM.InitializeReport(lstPaymentHdDt);
            #endregion

            #region Footer
            lblTanggal.Text = string.Format("{0}, {1}", entityHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT));
            lblTandaTangan.Text = appSession.UserFullName;
            #endregion

            base.InitializeReport(param);
        }
    }
}

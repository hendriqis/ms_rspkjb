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
    public partial class BNewPatientBillDetailNewRSPBT : BaseCustomDailyPotraitRpt
    {
        public BNewPatientBillDetailNewRSPBT()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vRegistration entityReg = BusinessLayer.GetvRegistrationList(param[0]).FirstOrDefault();
            Registration entityRegLinked = BusinessLayer.GetRegistrationList(string.Format("LinkedRegistrationID = {0}", entityReg.RegistrationID)).FirstOrDefault();
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID)).FirstOrDefault();

            List<GetPatientChargesHdDtAllPerServiceUnit> lstHDDT = BusinessLayer.GetPatientChargesHdDtAllPerServiceUnitList(entityReg.RegistrationID, 0);

            List<vPatientBill> lstBill = BusinessLayer.GetvPatientBillList(string.Format("RegistrationID = {0} AND GCTransactionStatus != '{1}'",
                entityReg.RegistrationID, Constant.TransactionStatus.VOID));
            List<vPatientPaymentHd> lstPayment = BusinessLayer.GetvPatientPaymentHdList(string.Format("RegistrationID IN ({0}) AND GCTransactionStatus != '{1}'",
                entityReg.RegistrationID, Constant.TransactionStatus.VOID));
            List<vPatientPaymentHdDt> lstPaymentHdDt = BusinessLayer.GetvPatientPaymentHdDtList(string.Format("RegistrationID IN ({0}) AND GCTransactionStatus != '{1}'",
                entityReg.RegistrationID, Constant.TransactionStatus.VOID));

            string corporate = "";
            if (entityReg.BusinessPartnerID != 1)
            {
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
            if (entityReg.Salutation != "")
            {
                cPatientName.Text = entityReg.Salutation + " " + entityReg.PatientName;
            }
            else
            {
                cPatientName.Text = entityReg.PatientName;
            }
            lblPatientName.Text = entityReg.cfPatientNameSalutation;
            cDOB.Text = entityReg.DateOfBirthInString;
            cRegisteredPhysician.Text = "(" + entityReg.ParamedicCode + ") " + entityReg.ParamedicName;

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

            cServiceUnitClass.Text = string.Format("{0}", entityReg.ServiceUnitName);
            cRoomBed.Text = string.Format("{0} | {1}", entityReg.RoomName, entityReg.BedCode);

            cCorporate.Text = corporate;

            string dischargeDateInfo = entityReg.cfDischargeDateInString;

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
            billPaymentDetailTransactionByServiceUnitDMRSPBT.InitializeReport(lstHDDT);
            #endregion

            #region Discount
            subDiscount.CanGrow = true;
            billPaymentDetailDiscountAllRSPBT.InitializeReport(entityReg.RegistrationID);
            #endregion

            #region Summary Right

            decimal transP = lstHDDT.Sum(p => p.PatientAmount);
            decimal transC = lstHDDT.Sum(p => p.PayerAmount);

            //decimal adminP = lstBill.Sum(p => p.PatientAdminFeeAmount);
            //decimal adminC = lstBill.Sum(p => p.AdministrationFeeAmount);

            decimal coverP = 0;
            decimal coverC = lstBill.Sum(p => p.CoverageAmount);

            decimal roundingP = lstPayment.Sum(a => a.PatientRoundingAmount);
            decimal roundingC = lstPayment.Sum(a => a.PayerRoundingAmount);

            decimal billP = lstBill.Sum(p => p.TotalPatientAmount);
            decimal billC = lstBill.Sum(p => p.TotalPayerAmount);

            decimal discP = lstBill.Sum(p => p.PatientDiscountAmount);
            decimal discC = lstBill.Sum(p => p.PayerDiscountAmount);

            //decimal temp = transC + adminC;
            //decimal amountP = 0;
            //decimal amountC = 0;
            //if (temp > coverC)
            //{
            //    amountP = temp - coverC;
            //    amountC = coverC;
            //}
            //else
            //{
            //    amountC = temp;
            //}
            ////decimal gtP = transP + adminP + amountP - discP;
            ////decimal gtC = amountC - discC;

            decimal gtP = billP - discP;
            decimal gtC = billC - discC;

            decimal totalBilling = gtP + gtC;

            decimal uangMuka = lstPayment.Sum(a => a.DownPaymentOut);

            decimal paymentP = lstPayment.Where(p => p.GCPaymentType == Constant.PaymentType.SETTLEMENT || p.GCPaymentType == Constant.PaymentType.CUSTOM || p.GCPaymentType == Constant.PaymentType.DOWN_PAYMENT).Sum(p => p.NotInDownPayment)
                                - uangMuka;
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

                //    if (isAllowRounding == "1")
                //    {
                //        decimal totalTemp = transP - disc;
                //        decimal nilaiPembulatan = Convert.ToDecimal(nilaiPembulatanString);
                //        decimal sisaPembulatanTemp = totalTemp % nilaiPembulatan;

                //        if (pembulatanKeAtas == "1")
                //        {
                //            roundingP = nilaiPembulatan - sisaPembulatanTemp;
                //        }
                //        else
                //        {
                //            roundingP = sisaPembulatanTemp * -1;
                //        }

                //        if (roundingP % nilaiPembulatan == 0)
                //        {
                //            roundingP = 0;
                //        }
                //    }
            }

            //gtP += roundingP;
            //gtC += roundingC;

            //decimal balanceP = gtP - paymentP;
            //decimal balanceC = gtC - paymentC;

            //cTotalTransactionP.Text = transP.ToString("N2");
            //cTotalTransactionC.Text = transC.ToString("N2");

            //cAdminP.Text = adminP.ToString("N2");
            //cAdminC.Text = adminC.ToString("N2");

            cCoverageP.Text = gtP.ToString("N2");
            cCoverageC.Text = coverC.ToString("N2");

            //cBillP.Text = billP.ToString("N2");
            //cBillC.Text = billC.ToString("N2");

            //cDiscP.Text = discP.ToString("N2");
            //cDiscC.Text = discC.ToString("N2");

            //if (discP == 0 && discC == 0)
            //{
            //    lblDiskonHeader.Visible = false;
            //    subDiscount.Visible = false;
            //    lblDiskon.Visible = false;
            //    cDiscP.Visible = false;
            //    cDiscC.Visible = false;
            //}
            //else
            //{
            //    cDiscP.Text = discP.ToString("N2");
            //    cDiscC.Text = discC.ToString("N2");
            //}

            cRoundingP.Text = roundingP.ToString("N2");
            cRoundingC.Text = roundingC.ToString("N2");

            cGrandTotalP.Text = gtP.ToString("N2");
            cGrandTotalC.Text = gtC.ToString("N2");

            cTotalBilling.Text = totalBilling.ToString("N2");

            if (paymentP == 0 && paymentC == 0)
            {
                subPaymentAll.Visible = false;
            }

            //cPaymentP.Text = paymentP.ToString("N2");
            //cPaymentC.Text = paymentC.ToString("N2");

            //cBalanceP.Text = balanceP.ToString("N2");
            //cBalanceC.Text = balanceC.ToString("N2");

            #endregion

            #region Payment All
            subPaymentAll.CanGrow = true;
            billPaymentDetailPaymentAllRSPBT.InitializeReport(lstPaymentHdDt);
            #endregion

            if (entityReg.DepartmentID != Constant.Facility.OUTPATIENT || entityReg.DepartmentID != Constant.Facility.PHARMACY || entityReg.DepartmentID != Constant.Facility.INPATIENT)
            {
                lblInfoPPN.Visible = false;
            }

            lblTanggal.Text = string.Format("{0}, {1}", entityHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT));
            lblTandaTangan.Text = appSession.UserFullName;

            if (entityReg.DepartmentID == Constant.Facility.INPATIENT)
            {
                xrTable4.Visible = false;
                xrLabel14.Visible = false;
                xrLabel15.Visible = false;
                xrLabel16.Visible = false;
            }
            else
            {
                xrTable4.Visible = true;
                xrLabel14.Visible = true;
                xrLabel15.Visible = true;
                xrLabel16.Visible = true;
            }
            
            base.InitializeReport(param);
        }
    }
}

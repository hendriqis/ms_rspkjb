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
    public partial class BNewPatientBillSummaryPerServiceUnitDM : BaseCustomDailyPotraitRpt
    {
        public BNewPatientBillSummaryPerServiceUnitDM()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] newParam = param[0].Split(';');
            string[] reg = new string[] { string.Format("RegistrationID = {0}", newParam[0]) };
            string hsu = string.Format("ItemHealthcareServiceUnitID = {0}", newParam[1]);

            vRegistration entityReg = BusinessLayer.GetvRegistrationList(reg[0]).FirstOrDefault();
            RegistrationBPJS entityRegBPJS = BusinessLayer.GetRegistrationBPJSList(String.Format("RegistrationID = {0}", entityReg.RegistrationID)).FirstOrDefault();
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID)).FirstOrDefault();

            List<GetPatientChargesHdDtAllPerServiceUnit> lst;
            if (Convert.ToInt32(newParam[1]) != 0)
            {
                lst = BusinessLayer.GetPatientChargesHdDtAllPerServiceUnitList(entityReg.RegistrationID, Convert.ToInt32(newParam[1]));
            }
            else
            {
                lst = BusinessLayer.GetPatientChargesHdDtAllPerServiceUnitList(entityReg.RegistrationID, 0);
            }

            List<vPatientBill> lstBill = BusinessLayer.GetvPatientBillList(string.Format("RegistrationID = {0} AND GCTransactionStatus != '{1}'",
                entityReg.RegistrationID, Constant.TransactionStatus.VOID));
            List<vPatientPaymentHd> lstPayment = BusinessLayer.GetvPatientPaymentHdList(string.Format("RegistrationID IN ({0}) AND GCTransactionStatus != '{1}'",
                entityReg.RegistrationID, Constant.TransactionStatus.VOID));
            List<vPatientPaymentHdDt> lstPaymentHdDt = BusinessLayer.GetvPatientPaymentHdDtList(string.Format("RegistrationID IN ({0}) AND GCTransactionStatus != '{1}'",
                entityReg.RegistrationID, Constant.TransactionStatus.VOID));

            string corporate = "";
            string filterRegPayer = string.Format("RegistrationID = {0} AND IsDeleted = 0 ORDER BY IsPrimaryPayer DESC, BusinessPartnerID ASC", entityReg.RegistrationID);
            List<vRegistrationPayer> lstRegPayer = BusinessLayer.GetvRegistrationPayerList(filterRegPayer);
            foreach (vRegistrationPayer rp in lstRegPayer)
            {
                if (corporate != "")
                {
                    corporate += ", " + rp.BusinessPartnerName;
                }
                else
                {
                    corporate = rp.BusinessPartnerName;
                }
            }

            if (corporate == "")
            {
                corporate = entityReg.BusinessPartnerName;
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

            cCorporate.Text = corporate;

            //string dischargeDateInfo = entityReg.cfDischargeDateInString;

            //cDischargeDate.Text = dischargeDateInfo;
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

            if (entityRegBPJS != null)
            {
                cSEPNoCaption.Text = "SEP No";
                cSEPNoTitik2.Text = ":";
                cSEPNo.Text = entityRegBPJS.NoSEP;
            }
            else if (entityReg.ReferralNo != null && entityReg.ReferralNo != "")
            {
                cSEPNoCaption.Text = "Referral No";
                cSEPNoTitik2.Text = ":";
                cSEPNo.Text = entityReg.ReferralNo;
            }
            else
            {
                cSEPNoCaption.Visible = false;
                cSEPNoTitik2.Text = "";
                cSEPNo.Text = "";
                cSEPNo.Visible = false;
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
            billPaymentSummaryTransactionByServiceUnitDM.InitializeReport(lst);
            #endregion

            #region Discount
            subDiscount.CanGrow = true;
            billPaymentDetailDiscountAllDM.InitializeReport(entityReg.RegistrationID);
            #endregion

            #region Summary Right

            decimal transP = lst.Sum(p => p.PatientAmount);
            decimal transC = lst.Sum(p => p.PayerAmount);

            decimal adminP = lstBill.Sum(p => p.PatientAdminFeeAmount);
            decimal adminC = lstBill.Sum(p => p.AdministrationFeeAmount);

            decimal coverP = 0;
            decimal coverC = lstBill.Sum(p => p.CoverageAmount);

            decimal grouperP = 0;
            decimal grouperC = entityReg.BPJSAmount;

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
            //decimal gtP = transP + adminP + amountP - discP;
            //decimal gtC = amountC - discC;

            decimal gtP = billP - discP;
            decimal gtC = billC - discC;

            decimal uangMuka = lstPayment.Sum(a => a.DownPaymentOut);

            decimal paymentP = lstPayment.Where(p => p.GCPaymentType == Constant.PaymentType.SETTLEMENT || p.GCPaymentType == Constant.PaymentType.CUSTOM || p.GCPaymentType == Constant.PaymentType.DOWN_PAYMENT).Sum(p => p.NotInDownPayment)
                                - uangMuka;
            decimal paymentC = 0;

            decimal balanceP = gtP - paymentP;
            decimal balanceC = gtC - paymentC;

            cTotalTransactionP.Text = transP.ToString("N2");
            cTotalTransactionC.Text = transC.ToString("N2");

            cAdminP.Text = adminP.ToString("N2");
            cAdminC.Text = adminC.ToString("N2");

            cCoverageP.Text = coverP.ToString("N2");
            cCoverageC.Text = coverC.ToString("N2");

            cGrouperP.Text = grouperP.ToString("N2");
            cGrouperC.Text = grouperC.ToString("N2");

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

            #region Payment All
            subPaymentAll.CanGrow = true;
            billPaymentDetailPaymentAllRSSBB.InitializeReport(lstPaymentHdDt);
            #endregion

            lblTanggal.Text = string.Format("{0}, {1}", entityHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_TIME_FORMAT));
            lblTandaTangan.Text = appSession.UserFullName;
            
            base.InitializeReport(reg);
        }
    }
}

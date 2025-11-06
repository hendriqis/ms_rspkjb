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
    public partial class BNewPatientBillDetailPerServiceUnitBeforeBill : BaseCustomDailyPotraitRpt
    {
        public BNewPatientBillDetailPerServiceUnitBeforeBill()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] newParam = param[0].Split(';');
            string[] reg = new string[] { string.Format("RegistrationID = {0}", newParam[0]) };
            string hsu = string.Format("ItemHealthcareServiceUnitID = {0}", newParam[1]);

            vRegistration entityReg = BusinessLayer.GetvRegistrationList(reg[0]).FirstOrDefault();
            //Registration entityRegLinked = BusinessLayer.GetRegistrationList(string.Format("LinkedRegistrationID = {0} AND GCRegistrationStatus != '{1}'", entityReg.RegistrationID, Constant.VisitStatus.CANCELLED)).FirstOrDefault();
            Registration entityRegLinkedTo = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0} AND GCRegistrationStatus != '{1}'", entityReg.LinkedToRegistrationID, Constant.VisitStatus.CANCELLED)).FirstOrDefault();
            Healthcare entityHealthcare = BusinessLayer.GetHealthcare(appSession.HealthcareID);

            List<GetPatientChargesHdDtAllPerServiceUnitBeforeBill> lst;
            if (Convert.ToInt32(newParam[1]) != 0)
            {
                lst = BusinessLayer.GetPatientChargesHdDtAllPerServiceUnitBeforeBillList(entityReg.RegistrationID, Convert.ToInt32(newParam[1]));
            }
            else
            {
                lst = BusinessLayer.GetPatientChargesHdDtAllPerServiceUnitBeforeBillList(entityReg.RegistrationID, 0);
            }

            List<vPatientBill> lstBill = BusinessLayer.GetvPatientBillList(string.Format("RegistrationID = {0} AND GCTransactionStatus != '{1}'",
                entityReg.RegistrationID, Constant.TransactionStatus.VOID));
            List<vPatientPaymentHd> lstPayment = BusinessLayer.GetvPatientPaymentHdList(string.Format("RegistrationID IN ({0}) AND GCTransactionStatus != '{1}'",
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

            if (entityReg.CoverageTypeID != null && entityReg.CoverageTypeID != 0)
            {
                if (entityReg.CoverageTypeCode == "R001" && entityHealthcare.Initial == "NHS")
                {
                    cCorporate.Text = string.Format("{0} ({1})", entityReg.BusinessPartnerName, entityReg.CoverageTypeName);
                }
                else
                {
                    cCorporate.Text = string.Format("{0}", entityReg.BusinessPartnerName);
                }
            }
            else
            {
                cCorporate.Text = string.Format("{0}", entityReg.BusinessPartnerName);
            }

            string dischargeDateInfo = entityReg.cfDischargeDateInString;
            //if (dischargeDateInfo != "")
            //{
            //    if (entityReg.GCDischargeCondition == Constant.PatientOutcome.DEAD_BEFORE_48 || entityReg.GCDischargeCondition == Constant.PatientOutcome.DEAD_AFTER_48)
            //    {
            //        dischargeDateInfo = dischargeDateInfo + " (" + "RIP" + ")";
            //    }
            //}
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
            billPaymentDetailTransactionByServiceUnitPatient.InitializeReport(lst);
            #endregion

            #region Discount
            subDiscount.CanGrow = true;
            billPaymentDetailDiscountAll.InitializeReport(entityReg.RegistrationID);
            #endregion

            #region Summary Right

            decimal transP = lst.Sum(p => p.PatientAmount);
            decimal transC = lst.Sum(p => p.PayerAmount);

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

            cAdminP.Text = adminP.ToString("N2");
            cAdminC.Text = adminC.ToString("N2");

            cCoverageP.Text = coverP.ToString("N2");
            cCoverageC.Text = coverC.ToString("N2");

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

            base.InitializeReport(reg);
        }
    }
}

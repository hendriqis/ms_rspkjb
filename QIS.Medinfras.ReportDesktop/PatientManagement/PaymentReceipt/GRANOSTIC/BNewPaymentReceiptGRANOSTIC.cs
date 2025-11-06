using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BNewPaymentReceiptGRANOSTIC : BaseCustomDailyLandscapeA5Rpt
    {
        private string city = "";
        private string healthcareName = "";

        public BNewPaymentReceiptGRANOSTIC()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            #region Header 1 : Healthcare
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            if (oHealthcare != null)
            {
                //xrLogo.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/logo.png");
                cHealthcareName.Text = oHealthcare.HealthcareName;
                cHealthcareAddress.Text = oHealthcare.StreetName;
                cHealthcareCityZipCodes.Text = string.Format("{0} {1}", oHealthcare.City, oHealthcare.ZipCode);
                cHealthcarePhone.Text = oHealthcare.PhoneNo1;
                cHealthcareFax.Text = oHealthcare.FaxNo1;
            }
            #endregion

            #region Footer
            //lblReportProperties.Text = string.Format("MEDINFRAS - {0}, Print Date/Time:{1}, User ID:{2}", reportMaster.ReportCode, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"), appSession.UserName);
            #endregion

            String paymentReceiptID = param[0];
            PaymentReceipt PaymentReceipt = BusinessLayer.GetPaymentReceipt(Convert.ToInt32(paymentReceiptID));

            List<vRegistration> lstRegistration = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", PaymentReceipt.RegistrationID));
            vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", PaymentReceipt.RegistrationID)).FirstOrDefault();
            List<GetPaymentReceiptCustom> lstReceipt = BusinessLayer.GetPaymentReceiptCustomList(Convert.ToInt32(paymentReceiptID));
            List<GetPaymentReceiptCustomDetailTransactionGRANOSTIC> lstHDDT = BusinessLayer.GetPaymentReceiptCustomDetailTransactionGRANOSTICList(Convert.ToInt32(paymentReceiptID));
            PatientChargesHd entityCharges = BusinessLayer.GetPatientChargesHdList(string.Format("VisitID = {0}", entityReg.VisitID)).FirstOrDefault();
            string filterBill = string.Format("GCTransactionStatus != '{0}' AND PatientBillingID IN (SELECT PatientBillingID FROM PatientBillPayment WHERE PaymentID IN (SELECT PaymentID FROM PatientPaymentHd WHERE GCTransactionStatus != '{0}' AND PaymentReceiptID = {1}))", Constant.TransactionStatus.VOID, paymentReceiptID);
            List<vPatientBill> lstBill = BusinessLayer.GetvPatientBillList(filterBill);
            List<vPatientPaymentHd> lstPayment = BusinessLayer.GetvPatientPaymentHdList(string.Format("PaymentID IN (SELECT PaymentID FROM PatientPaymentHd WHERE PaymentReceiptID = {0}) AND GCTransactionStatus != '{1}'",
                paymentReceiptID, Constant.TransactionStatus.VOID));
            List<vPatientPaymentHdDt> lstPaymentHdDt = BusinessLayer.GetvPatientPaymentHdDtList(string.Format("PaymentID IN (SELECT PaymentID FROM PatientPaymentHd WHERE PaymentReceiptID = {0}) AND GCTransactionStatus != '{1}'",
                paymentReceiptID, Constant.TransactionStatus.VOID));

            #region Data Pasienn
            lblNoReg.Text = string.Format("{0}", PaymentReceipt.PaymentReceiptNo);
            lblNamaPasien.Text = string.Format("{0},{1}", lstRegistration.FirstOrDefault().PatientName, lstRegistration.FirstOrDefault().Salutation); 
            lblPenanggung.Text = PaymentReceipt.PrintAsName;
            DateTime TanggalLahir = lstRegistration.FirstOrDefault().DateOfBirth;
            //lblTanggalLahir.Text = TanggalLahir.ToString(Constant.FormatString.DATE_FORMAT);
            lblNoPasien.Text = lstRegistration.FirstOrDefault().MedicalNo;
            lblRujukan.Text = entityReg.ReferrerGroup;
            lblDeskripsiRujukan.Text = entityReg.ReferrerName;
            lblDokterUtama.Text = lstRegistration.FirstOrDefault().ParamedicName;
            if (entityCharges != null)
            {
                lblNoTransaksi.Text = entityCharges.TransactionNo;
            }
            else
            {
                lblNoTransaksi.Text = " ";
            }
            DateTime TanggalMasuk = lstRegistration.FirstOrDefault().ActualVisitDate;
            lblTanggalMasuk.Text = string.Format("{0}/{1}", TanggalMasuk.ToString(Constant.FormatString.DATE_FORMAT), lstRegistration.FirstOrDefault().ActualVisitTime);
            decimal totalBersih = 0;
            if (lstPayment.Count() > 0)
            {
                totalBersih = lstPayment.Sum(a => a.NotInDownPayment);
            }

            cfTotalBersih.Text = totalBersih.ToString(Constant.FormatString.NUMERIC_2);
            #endregion

            #region Charges
            subTransaction.CanGrow = true;
            billPaymentSummaryTransactionByServiceUnitDMGRANOSTIC.InitializeReport(lstHDDT);
            #endregion

            #region Discount
            subDiscount.CanGrow = true;
            billPaymentDetailDiscountAllGRANOSTIC.InitializeReport(entityReg.RegistrationID);
            #endregion

            #region Payment All
            subPaymentAll.CanGrow = true;
            billPaymentDetailPaymentAllGRANOSTIC.InitializeReport(lstPaymentHdDt);
            #endregion

            Int32 PaymentReceiptID = lstReceipt.FirstOrDefault().PaymentReceiptID;
            decimal vatPercent = Convert.ToDecimal(BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.VAT_PERCENTAGE).ParameterValue);

            String HealthcareID = appSession.HealthcareID;
            city = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", HealthcareID)).FirstOrDefault().City;
            lblNoLab.Text = entityReg.RegistrationNo;
            healthcareName = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", HealthcareID)).FirstOrDefault().HealthcareName;
            xrQR.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/GRANOSTIC.png");
            lblCashier.Text = string.Format("CS : '{0}', Kasir : '{1}'",entityReg.CreatedByName, AppSession.UserLogin.UserFullName);
            lblNotaPrint.Text = string.Format("Dicetak : '{0}' ",DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"));
            if (entityReg.GCResultDeliveryPlan == "X546^999")
            {
                lblDistribusi.Text = entityReg.ResultDeliveryPlanOthers;
            }
            else
            {
                lblDistribusi.Text = entityReg.ResultDeliveryPlan;
            }
            lblNoHP.Text = entityReg.MobilePhoneNo1;
            lblAddress.Text = entityReg.StreetName;
            cDPP.Text = string.Format("DPP {0}", totalBersih.ToString(Constant.FormatString.NUMERIC_2));
            decimal ppn = Convert.ToDecimal(totalBersih * (vatPercent / 100));
            cPPN.Text = string.Format("PPN {0}", ppn.ToString(Constant.FormatString.NUMERIC_2));
            #region Judul Kwintansi

            List<PatientPaymentHd> lstEntityHd = BusinessLayer.GetPatientPaymentHdList(string.Format("PaymentReceiptID = '{0}'", PaymentReceiptID));

            //string temp = "";
            //foreach (PatientPaymentHd phd in lstEntityHd)
            //{
            //    if (phd.GCPaymentType == Constant.PaymentType.DOWN_PAYMENT)
            //    {
            //        temp = "DOWN_PAYMENT";
            //    }
            //    else if (phd.GCPaymentType == Constant.PaymentType.SETTLEMENT)
            //    {
            //        temp = "SETTLEMENT";
            //    }
            //    else if (phd.GCPaymentType == Constant.PaymentType.AR_PAYER || phd.GCPaymentType == Constant.PaymentType.AR_PATIENT)
            //    {
            //        temp = "AR";
            //    }
            //}

            //if (temp == "DOWN_PAYMENT")
            //{
            //    lblJudulKwitansi.Text = "KWITANSI UANG MUKA";
            //}
            //else if (temp == "AR")
            //{
            //    lblJudulKwitansi.Text = "PIUTANG BIAYA PERAWATAN";
            //}
            //else
            //{
            //    lblJudulKwitansi.Text = "KWITANSI BIAYA PERAWATAN";
            //}

            //PatientPaymentHd entityHd = BusinessLayer.GetPatientPaymentHdList(string.Format("PaymentReceiptID = '{0}'", PaymentReceiptID)).FirstOrDefault();

            //if (entityHd.GCPaymentType == Constant.PaymentType.DOWN_PAYMENT)
            //{
            //    lblJudulKwitansi.Text = "KWITANSI UANG MUKA";
            //}
            //else if (entityHd.GCPaymentType == Constant.PaymentType.AR_PAYER || entityHd.GCPaymentType == Constant.PaymentType.AR_PATIENT)
            //{
            //    lblJudulKwitansi.Text = "PIUTANG BIAYA PERAWATAN";
            //}
            //else
            //{
            //    lblJudulKwitansi.Text = "KWITANSI BIAYA PERAWATAN";
            //}
            #endregion

            this.DataSource = lstReceipt;
        }
    }
}

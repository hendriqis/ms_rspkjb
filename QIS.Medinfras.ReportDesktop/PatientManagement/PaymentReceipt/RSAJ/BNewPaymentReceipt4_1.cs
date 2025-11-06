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
    public partial class BNewPaymentReceipt4_1 : BaseRpt
    {
        private string city = "";
        private string healthcareName = "";

        public BNewPaymentReceipt4_1()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            #region Header 1 : Healthcare
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            if (oHealthcare != null)
            {
                xrLogo.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/logo.png");
                cHealthcareName.Text = oHealthcare.HealthcareName;
                cHealthcareAddress.Text = oHealthcare.StreetName;
                cHealthcareCityZipCodes.Text = string.Format("{0} {1}", oHealthcare.City, oHealthcare.ZipCode);
                cHealthcarePhone.Text = oHealthcare.PhoneNo1;
                cHealthcareFax.Text = oHealthcare.FaxNo1;
            }
            #endregion

            #region Footer
            lblReportProperties.Text = string.Format("MEDINFRAS - {0}, Print Date/Time:{1}, User ID:{2}", reportMaster.ReportCode, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"), appSession.UserName);
            #endregion

            String paymentReceiptID = param[0];
            PaymentReceipt PaymentReceipt = BusinessLayer.GetPaymentReceipt(Convert.ToInt32(paymentReceiptID));

            List<vRegistration> lstRegistration = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", PaymentReceipt.RegistrationID));
            List<GetPatientChargesHdDtALLPerPayment2> lstHDDT = BusinessLayer.GetPatientChargesHdDtALLPerPayment2List(Convert.ToInt32(paymentReceiptID), PaymentReceipt.RegistrationID);
            List<GetPaymentReceiptCustom> lstReceipt = BusinessLayer.GetPaymentReceiptCustomList(Convert.ToInt32(paymentReceiptID));

            string filterBill = string.Format("GCTransactionStatus != '{0}' AND PatientBillingID IN (SELECT PatientBillingID FROM PatientBillPayment WHERE PaymentID IN (SELECT PaymentID FROM PatientPaymentHd WHERE GCTransactionStatus != '{0}' AND PaymentReceiptID = {1}))", Constant.TransactionStatus.VOID, paymentReceiptID);
            List<vPatientBill> lstBill = BusinessLayer.GetvPatientBillList(filterBill);

            #region Data Pasien
            lblNoReg.Text = string.Format("{0} / {1}", lstRegistration.FirstOrDefault().RegistrationNo, lstRegistration.FirstOrDefault().MedicalNo);
            lblNamaPasien.Text = lstRegistration.FirstOrDefault().PatientName;

            DateTime TanggalLahir = lstRegistration.FirstOrDefault().DateOfBirth;
            lblTanggalLahir.Text = TanggalLahir.ToString(Constant.FormatString.DATE_FORMAT);

            lblDokterUtama.Text = lstRegistration.FirstOrDefault().ParamedicName;

            DateTime TanggalMasuk = lstRegistration.FirstOrDefault().ActualVisitDate;
            lblTanggalMasuk.Text = string.Format("{0}/{1}", TanggalMasuk.ToString(Constant.FormatString.DATE_FORMAT), lstRegistration.FirstOrDefault().ActualVisitTime);

            lblRuangPerawatan.Text = lstRegistration.FirstOrDefault().RoomName;
            lblKelas.Text = lstRegistration.FirstOrDefault().ClassName;
            //lblPenjaminBayar.Text = lstRegistration.FirstOrDefault().CustomerType;
            //lblInstansi.Text = lstRegistration.FirstOrDefault().BusinessPartnerName;

            DateTime DischargeDate = lstRegistration.FirstOrDefault().DischargeDate;
            String DischargeDateInString = string.Format("{0}/{1}",DischargeDate.ToString(Constant.FormatString.DATE_FORMAT), lstRegistration.FirstOrDefault().DischargeTime);

            if (DischargeDate.ToString(Constant.FormatString.DATE_FORMAT) == "01-Jan-1900")
            {
                lblTanggalKeluar.Text = "-";
            }
            else
            {
                if (lstRegistration.FirstOrDefault().GCDischargeCondition == Constant.PatientOutcome.DEAD_BEFORE_48 || lstRegistration.FirstOrDefault().GCDischargeCondition == Constant.PatientOutcome.DEAD_AFTER_48)
                {
                    DischargeDateInString = DischargeDateInString + " (RIP)";
                }

                lblTanggalKeluar.Text = DischargeDateInString;
            }
            #endregion

            #region Charges
            subPaymentReceipt6DtRekap2.CanGrow = true;
            bNewPaymentReceipt6DtRekap21.InitializeReport(lstHDDT);
            #endregion

            #region Billing
            subPaymentReceipt6DtRekap.CanGrow = true;
            bNewPaymentReceipt6DtRekap1.InitializeReport(lstBill, PaymentReceipt.RegistrationID);
            #endregion

            Int32 PaymentReceiptID = lstReceipt.FirstOrDefault().PaymentReceiptID;

            String HealthcareID = appSession.HealthcareID;
            city = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", HealthcareID)).FirstOrDefault().City;
            healthcareName = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", HealthcareID)).FirstOrDefault().HealthcareName;

            lblHealthcareName.Text = healthcareName;

            #region Judul Kwintansi

            List<PatientPaymentHd> lstEntityHd = BusinessLayer.GetPatientPaymentHdList(string.Format("PaymentReceiptID = '{0}'", PaymentReceiptID));

            string temp = "";
            foreach (PatientPaymentHd phd in lstEntityHd)
            {
                if (phd.GCPaymentType == Constant.PaymentType.DOWN_PAYMENT)
                {
                    temp = "DOWN_PAYMENT";
                }
                else if (phd.GCPaymentType == Constant.PaymentType.SETTLEMENT)
                {
                    temp = "SETTLEMENT";
                }
                else if (phd.GCPaymentType == Constant.PaymentType.AR_PAYER || phd.GCPaymentType == Constant.PaymentType.AR_PATIENT)
                {
                    temp = "AR";
                }
            }

            if (temp == "DOWN_PAYMENT")
            {
                lblJudulKwitansi.Text = "KWITANSI UANG MUKA";
            }
            else if (temp == "AR")
            {
                lblJudulKwitansi.Text = "PIUTANG BIAYA PERAWATAN";
            }
            else
            {
                lblJudulKwitansi.Text = "KWITANSI BIAYA PERAWATAN";
            }

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

        private void lblTanggal_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string receiptDate = Convert.ToDateTime(GetCurrentColumnValue("ReceiptDate")).ToString(Constant.FormatString.DATE_FORMAT);
            string createdDate = Convert.ToDateTime(GetCurrentColumnValue("CreatedDate")).ToString(Constant.FormatString.DATE_FORMAT);

            if (receiptDate != "01-Jan-1900")
            {
                lblTanggal.Text = string.Format("{0}, {1}", city, receiptDate);
            }
            else
            {
                lblTanggal.Text = string.Format("{0}, {1}", city, createdDate);
            }
        }

        private void PageFooter_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

        //private void lblPrintNumber_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        //{
        //    SettingParameterDt setPar = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, Constant.SettingParameter.FN_IS_USE_COUNTER_IN_PAYMENT_RECEIPT);

        //    if (setPar.ParameterValue == "1")
        //    {
        //        Int32 PrintNumber = Convert.ToInt32((GetCurrentColumnValue("PrintNumber")).ToString());

        //        if (PrintNumber > 0)
        //        {

        //            lblPrintNumber.Text = string.Format("{0} - {1}", GetCurrentColumnValue("PaymentReceiptNo").ToString(), GetCurrentColumnValue("PrintNumber").ToString());
        //        }
        //        else
        //        {
        //            lblPrintNumber.Text = string.Format("{0}", GetCurrentColumnValue("PaymentReceiptNo").ToString());
        //        }
        //    }
        //    else
        //    {
        //        lblPrintNumber.Text = string.Format("{0}", GetCurrentColumnValue("PaymentReceiptNo").ToString());
        //    }
        //}
    }
}

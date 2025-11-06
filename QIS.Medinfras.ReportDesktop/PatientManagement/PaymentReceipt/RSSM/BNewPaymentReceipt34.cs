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
    public partial class BNewPaymentReceipt34 : BaseRpt
    {
        private string HealthcareName = "";
        private string Address = "";
        private string City = "";

        public BNewPaymentReceipt34()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            String paymentReceiptID = param[0];


            PaymentReceipt entity = BusinessLayer.GetPaymentReceipt(Convert.ToInt32(param[0]));
            if (entity.PrintNumber > 1)
            {
                lblCopy.Visible = true;
            }

            //String HealthcareID = AppSession.UserLogin.HealthcareID;
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '001'")).FirstOrDefault();

            xrLogo.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/logo.png");

            HealthcareName = oHealthcare.HealthcareName;
            Address = oHealthcare.StreetName;
            City = oHealthcare.City;

            lblHealthcareName1.Text = HealthcareName;
            lblAddress.Text = Address;
            lblCity.Text = City;
            lblEmail.Text = oHealthcare.Email;

            List<vRegistration> lstRegistration = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID));
            List<GetPatientChargesHdDtALLPerPayment3> lstHDDT = BusinessLayer.GetPatientChargesHdDtALLPerPayment3List(Convert.ToInt32(paymentReceiptID), AppSession.RegisteredPatient.RegistrationID);
            List<GetPaymentReceiptCustom> lstReceipt = BusinessLayer.GetPaymentReceiptCustomList(Convert.ToInt32(paymentReceiptID));

            string filterBill = string.Format("GCTransactionStatus != '{0}' AND PatientBillingID IN (SELECT PatientBillingID FROM PatientBillPayment WHERE PaymentID IN (SELECT PaymentID FROM PatientPaymentHd WHERE GCTransactionStatus != '{0}' AND PaymentReceiptID = {1}))", Constant.TransactionStatus.VOID, paymentReceiptID);
            List<vPatientBill> lstBill = BusinessLayer.GetvPatientBillList(filterBill);

            #region Data Pasien
            //lblNoReg.Text = string.Format("{0} / {1}", lstRegistration.FirstOrDefault().RegistrationNo, lstRegistration.FirstOrDefault().MedicalNo);
            //lblNamaPasien.Text = lstRegistration.FirstOrDefault().PatientName;

            //DateTime TanggalLahir = lstRegistration.FirstOrDefault().DateOfBirth;
            //lblTanggalLahir.Text = TanggalLahir.ToString(Constant.FormatString.DATE_FORMAT);

            //lblDokterUtama.Text = lstRegistration.FirstOrDefault().ParamedicName;

            //DateTime TanggalMasuk = lstRegistration.FirstOrDefault().ActualVisitDate;
            //lblTanggalMasuk.Text = string.Format("{0}/{1}", TanggalMasuk.ToString(Constant.FormatString.DATE_FORMAT), lstRegistration.FirstOrDefault().ActualVisitTime);

            //lblRuangPerawatan.Text = lstRegistration.FirstOrDefault().RoomName;
            //lblKelas.Text = lstRegistration.FirstOrDefault().ClassName;
            ////lblPenjaminBayar.Text = lstRegistration.FirstOrDefault().CustomerType;
            ////lblInstansi.Text = lstRegistration.FirstOrDefault().BusinessPartnerName;

            //DateTime DischargeDate = lstRegistration.FirstOrDefault().DischargeDate;
            //String DischargeDateInString = string.Format("{0}/{1}",DischargeDate.ToString(Constant.FormatString.DATE_FORMAT), lstRegistration.FirstOrDefault().DischargeTime);

            //if (DischargeDate.ToString(Constant.FormatString.DATE_FORMAT) == "01-Jan-1900")
            //{
            //    lblTanggalKeluar.Text = "-";
            //}
            //else
            //{
            //    if (lstRegistration.FirstOrDefault().GCDischargeCondition == Constant.PatientOutcome.DEAD_BEFORE_48 || lstRegistration.FirstOrDefault().GCDischargeCondition == Constant.PatientOutcome.DEAD_AFTER_48)
            //    {
            //        DischargeDateInString = DischargeDateInString + " (RIP)";
            //    }

            //    lblTanggalKeluar.Text = DischargeDateInString;
            //}
            #endregion

            #region Charges
            subPaymentReceipt34DtRekap2.CanGrow = true;
            bNewPaymentReceipt34DtRekap21.InitializeReport(lstHDDT, lstBill);
            #endregion

            Int32 PaymentReceiptID = lstReceipt.FirstOrDefault().PaymentReceiptID;

            #region Billing
            subPaymentReceipt34DtRekap.CanGrow = true;
            bNewPaymentReceipt34DtRekap1.InitializeReport(lstBill, PaymentReceiptID);
            #endregion
            #region set PaymentReceiptHistory Info

            Int32 registraionID = AppSession.RegisteredPatient.RegistrationID;
            List<PaymentReceipt> pr = BusinessLayer.GetPaymentReceiptList(String.Format("RegistrationID = {0} AND IsDeleted = 0 AND GCVoidReason IS NULL AND PaymentReceiptID ! = {1}", registraionID, PaymentReceiptID));
            String kwitansiNo = "";
            foreach (PaymentReceipt p in pr)
            {
                if (string.IsNullOrEmpty(kwitansiNo))
                {
                    kwitansiNo = p.PaymentReceiptNo;
                }
                else
                {
                    kwitansiNo += ", " + p.PaymentReceiptNo;
                }
            }
            lblNoKwitansi.Text = kwitansiNo;
            if (kwitansiNo == "")
            {
                lblJudulNoKwitansi.Text = "";
            }
            #endregion

            #region Judul Kwintansi

            List<PatientPaymentHd> lstEntityHd = BusinessLayer.GetPatientPaymentHdList(string.Format("PaymentReceiptID = '{0}'", PaymentReceiptID)).OrderBy(t => t.PaymentID).ToList();

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

            //if (temp == "DOWN_PAYMENT")
            //{
            //    lblJudulKwitansi.Text = "KWITANSI UANG MUKA";
            //}
            //else if (temp == "AR")
            //{
            //    lblJudulKwitansi.Text = "KWITANSI BIAYA PERAWATAN";
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

        //private void lblDiagnosa_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        //{
        //    if (GetCurrentColumnValue("Diagnose1").ToString() != "" && GetCurrentColumnValue("Diagnose2").ToString() != "")
        //    {
        //        lblDiagnosa.Text = string.Format("Diagnosa = {0}, {1}", GetCurrentColumnValue("Diagnose1").ToString(), GetCurrentColumnValue("Diagnose2").ToString());
        //    }
        //    else if (GetCurrentColumnValue("Diagnose1").ToString() != "")
        //    {
        //        lblDiagnosa.Text = string.Format("Diagnosa = {0}", GetCurrentColumnValue("Diagnose1").ToString());
        //    }
        //    else
        //    {
        //        lblDiagnosa.Text = "";
        //    }
        //}

        private void lblTanggal_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string receiptDate = Convert.ToDateTime(GetCurrentColumnValue("ReceiptDate")).ToString(Constant.FormatString.DATE_FORMAT);
            string createdDate = Convert.ToDateTime(GetCurrentColumnValue("CreatedDate")).ToString(Constant.FormatString.DATE_FORMAT);

            if (receiptDate != "01-Jan-1900")
            {
                lblTanggal.Text = string.Format("{0}, {1}", City, receiptDate);
            }
            else
            {
                lblTanggal.Text = string.Format("{0}, {1}", City, createdDate);
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

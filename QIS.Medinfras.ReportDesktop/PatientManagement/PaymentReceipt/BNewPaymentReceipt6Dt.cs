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
    public partial class BNewPaymentReceipt6Dt : BaseRpt
    {
        private string city = "";
        private string healthcareName = "";

        public BNewPaymentReceipt6Dt()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<GetPaymentReceiptCustom> lstReceipt)
        {
            Int32 PaymentReceiptID = lstReceipt.FirstOrDefault().PaymentReceiptID;

            String HealthcareID = AppSession.UserLogin.HealthcareID;
            city = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", HealthcareID)).FirstOrDefault().City;
            healthcareName = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", HealthcareID)).FirstOrDefault().HealthcareName;

            lblHealthcareName.Text = healthcareName;

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

        private void lblDiagnosa_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (GetCurrentColumnValue("Diagnose1").ToString() != "" && GetCurrentColumnValue("Diagnose2").ToString() != "")
            {
                lblDiagnosa.Text = string.Format("Diagnosa = {0}, {1}", GetCurrentColumnValue("Diagnose1").ToString(), GetCurrentColumnValue("Diagnose2").ToString());
            }
            else if (GetCurrentColumnValue("Diagnose1").ToString() != "")
            {
                lblDiagnosa.Text = string.Format("Diagnosa = {0}", GetCurrentColumnValue("Diagnose1").ToString());
            }
            else
            {
                lblDiagnosa.Text = "";
            }
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

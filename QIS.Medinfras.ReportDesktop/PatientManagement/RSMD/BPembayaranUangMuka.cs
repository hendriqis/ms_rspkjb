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
    public partial class BPembayaranUangMuka : BaseA6Rpt
    {
        public BPembayaranUangMuka()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];

            vPatientPaymentHd entityPayment = BusinessLayer.GetvPatientPaymentHdList(string.Format("{0}", param[0]))[0];
            vPaymentReceipt receipt = null;
            if (entityPayment.PaymentReceiptID != 0) {
                
                receipt = BusinessLayer.GetvPaymentReceiptList(string.Format("ReceiptID = {0}", entityPayment.PaymentReceiptID)).FirstOrDefault();

                cNoBayar.Text = receipt.PaymentReceiptNo;
                cSudahTerimaDari.Text = receipt.PrintAsName;

                cInformation.Text = receipt.PaymentMethod + " " + receipt.ReceiptAmount.ToString("N2");
            }

            cNama.Text = entityPayment.PatientName;
            cNoReg.Text = entityPayment.RegistrationNo + " / " + entityPayment.MedicalNo;
            cJumlah.Text = entityPayment.ReceiptAmount.ToString("N2");
            cTerbilang.Text = "Terbilang " + entityPayment.TotalPaymentAmountInString;

            vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", entityPayment.RegistrationID))[0];
            cRuangRawat.Text = entityReg.RoomName;

            cDate.Text = entityHealthcare.City + ", " + DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT);
            cUserName.Text = "( " + entityPayment.CreatedByUser + " )";
            base.InitializeReport(param);
        }
    }
}
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
    public partial class BNotaPengembalianUangMuka : BaseA6Rpt
    {
        public BNotaPengembalianUangMuka()
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

                cNoNota.Text = receipt.PaymentReceiptNo;
                cRuangRawat.Text = receipt.PrintAsName;
            }

            cTanggal.Text = entityPayment.PaymentDateTimeInString;
            cNoRM.Text = entityPayment.MedicalNo;


            cAtasNama.Text = receipt.PrintAsName;
            cJumlahKembali.Text = entityPayment.ReceiveAmountInString;
            cTerbilang.Text = entityPayment.TotalPaymentAmountInString;

            vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", entityPayment.RegistrationID))[0];
            cTanggalMasuk.Text = entityReg.RegistrationDateInString;
            cKeterangan.Text = string.Format("UNTUK PENGEMBALIAN UANG MUKA PASIEN {0}, NO.REG. {1}", entityReg.PatientName, entityReg.RegistrationNo);
            base.InitializeReport(param);
        }
    }
}
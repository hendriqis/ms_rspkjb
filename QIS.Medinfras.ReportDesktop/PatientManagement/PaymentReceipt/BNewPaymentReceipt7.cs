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
    public partial class BNewPaymentReceipt7 : BaseRpt
    {
        public BNewPaymentReceipt7()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            String paymentReceiptID = param[0];

            List<vRegistration> lstRegistration = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID));
            List<GetPatientChargesHdDtALLPerPayment> lstHDDT = BusinessLayer.GetPatientChargesHdDtALLPerPaymentList(Convert.ToInt32(paymentReceiptID), AppSession.RegisteredPatient.RegistrationID);
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
            lblPenjaminBayar.Text = lstRegistration.FirstOrDefault().CustomerType;
            lblInstansi.Text = lstRegistration.FirstOrDefault().BusinessPartnerName;

            DateTime DischargeDate = lstRegistration.FirstOrDefault().DischargeDate;
            String DischargeDateInString = string.Format("{0}/{1}",DischargeDate.ToString(Constant.FormatString.DATE_FORMAT), lstRegistration.FirstOrDefault().DischargeTime);

            if (DischargeDate.ToString(Constant.FormatString.DATE_FORMAT) == "01-Jan-1900")
            {
                lblTanggalKeluar.Text = "-";
            }
            else
            {
                lblTanggalKeluar.Text = DischargeDateInString;
            }
            #endregion

            #region Charges
            subPaymentReceipt7DtRekap2.CanGrow = true;
            bNewPaymentReceipt7DtRekap21.InitializeReport(lstHDDT);
            #endregion

            #region Kwitansi
            subPaymentReceipt7Dt.CanGrow = true;
            bNewPaymentReceipt7Dt1.InitializeReport(lstReceipt);
            #endregion

            #region Billing
            subPaymentReceipt7DtRekap.CanGrow = true;
            bNewPaymentReceipt7DtRekap1.InitializeReport(lstBill);
            #endregion

            base.InitializeReport(param);
        }

    }

}
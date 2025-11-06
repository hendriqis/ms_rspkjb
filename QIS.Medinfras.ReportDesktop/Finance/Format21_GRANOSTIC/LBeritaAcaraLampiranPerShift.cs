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
    public partial class LBeritaAcaraLampiranPerShift : BaseCustomDailyLandscapeA5Rpt
    {
        public LBeritaAcaraLampiranPerShift()
        {
            InitializeComponent();
        }
        //private List<GetAttachmentMinutesPerShift> lstAttachment = null;
        public override void InitializeReport(string[] param)
        {
            #region Header 1 : Healthcare
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            if (oHealthcare != null)
            {
                lblCabang.Text = oHealthcare.HealthcareName;
            }
            #endregion  
            List<vPatientChargesHd> lstPatientCharges = BusinessLayer.GetvPatientChargesHdList(string.Format("TransactionDate = '{0}' AND GCTransactionStatus != '{1}'", param[0], Constant.TransactionStatus.VOID));
            List<GetAttachmentMinutesPerShift> lstAttachment = BusinessLayer.GetAttachmentMinutesPerShiftList(param[0] ,param[1], param[2]);
            string[] empty = new string[5];
            var total = empty.Count();
            Int32 totalTransaksi = Convert.ToInt32(lstAttachment.FirstOrDefault().PaymentReceiptID).ToString("N2").Count();
            Int32 totalNota = Convert.ToInt32(lstAttachment.FirstOrDefault().PaymentReceiptID).ToString("N2").Count();
            Int32 totalOrang = Convert.ToInt32(lstAttachment.FirstOrDefault().RegistrationID).ToString("N2").Count();
            lblTanggalShift.Text = string.Format("{0} / {1} / {2}", Helper.YYYYMMDDToDate(param[0]).ToString(Constant.FormatString.DATE_PICKER_FORMAT), lstAttachment.FirstOrDefault().Shift, lstAttachment.FirstOrDefault().CreatedByName);
            lblJumlahTransaksi.Text = string.Format("{0} transaksi / {1} nota / {2} orang", totalTransaksi, totalNota, totalOrang);
            if (lstAttachment.Where(a => a.PaymentMethod == "Manajemen") == null)
            {
                lblTransaksiLalu.Text = lstAttachment.Where(a => a.GCPaymentMethod == "Manajemen").FirstOrDefault().PaymentAmount.ToString(Constant.FormatString.NUMERIC_2);
            }
            else 
            {
                lblTransaksiLalu.Text = "0";
            }
            if (lstAttachment.Where(a => a.GCPaymentMethod == Constant.PaymentMethod.CASH) == null)
            {
                lblCash.Text = "0";
            }
            else
            {
                lblCash.Text = lstAttachment.Where(a => a.GCPaymentMethod == Constant.PaymentMethod.CASH).FirstOrDefault().PaymentAmount.ToString(Constant.FormatString.NUMERIC_2);
            }
            if (lstAttachment.Where(a => a.GCPaymentMethod == Constant.PaymentMethod.DEBIT_CARD) == null)
            {
                lblCard.Text = lstAttachment.Where(a => a.GCPaymentMethod == Constant.PaymentMethod.DEBIT_CARD).FirstOrDefault().PaymentAmount.ToString(Constant.FormatString.NUMERIC_2);
            }
            else
            {
                lblCard.Text = "0";
            }
            if (lstAttachment.Where(a => a.GCPaymentMethod == Constant.PaymentMethod.VOUCHER) == null)
            {
                lblVoucher.Text = lstAttachment.Where(a => a.GCPaymentMethod == Constant.PaymentMethod.VOUCHER).FirstOrDefault().PaymentAmount.ToString(Constant.FormatString.NUMERIC_2);
            }
            else
            {
                lblVoucher.Text = "0";
            }
            if (lstAttachment.Where(a => a.GCPaymentMethod == Constant.PaymentMethod.BANK_TRANSFER) == null)
            {
                lblTransfer.Text = lstAttachment.Where(a => a.GCPaymentMethod == Constant.PaymentMethod.BANK_TRANSFER).FirstOrDefault().PaymentAmount.ToString(Constant.FormatString.NUMERIC_2);
            }
            else
            {
                lblTransfer.Text = "0";
            }
            if (lstAttachment.Where(a => a.GCPaymentMethod == Constant.PaymentMethod.CREDIT) == null)
            {
                lblPiutang.Text = lstAttachment.Where(a => a.GCPaymentMethod == Constant.PaymentMethod.CREDIT).FirstOrDefault().PaymentAmount.ToString(Constant.FormatString.NUMERIC_2);
            }
            else
            {
                lblPiutang.Text = "0";
            }
            if (lstAttachment.Where(a => a.PaymentMethod == "Manajemen") == null)
            {
                lblManajemen.Text = lstAttachment.Where(a => a.GCPaymentMethod == "Manajemen").FirstOrDefault().PaymentAmount.ToString(Constant.FormatString.NUMERIC_2);
            }
            else
            {
                lblManajemen.Text = "0";
            }
            lblAmountInString.Text = string.Format("Rp.{0} ({1})", lstAttachment.Where(a => a.GCPaymentMethod == Constant.PaymentMethod.CASH).FirstOrDefault().PaymentAmount.ToString(Constant.FormatString.NUMERIC_2), Function.NumberWithPointInWordsInIndonesian(Convert.ToDouble(lstAttachment.Where(a => a.GCPaymentMethod == Constant.PaymentMethod.CASH).FirstOrDefault().PaymentAmount), true));
            lblDateAmount.Text = string.Format("sebagai setoran penjualan tanggal {0} shift {1}", Helper.YYYYMMDDToDate(param[0]).ToString(Constant.FormatString.DATE_PICKER_FORMAT), lstAttachment.FirstOrDefault().Shift);
            lblDatePrint.Text = string.Format("Surabaya, {0}", DateTime.Now.ToString("dd-MMMM-yyyy"));
            base.InitializeReport(param);
        }
    }
}

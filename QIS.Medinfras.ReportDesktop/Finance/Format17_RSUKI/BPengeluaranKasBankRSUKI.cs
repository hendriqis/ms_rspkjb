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
    public partial class BPengeluaranKasBankRSUKI : BaseCustomDailyPotraitRpt
    {
        decimal subTotalPembayaran = 0, stampAmount = 0;

        public BPengeluaranKasBankRSUKI()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vSupplierPaymentHd entity = BusinessLayer.GetvSupplierPaymentHdList(param[0]).FirstOrDefault();
            cNoPembayaran.Text = entity.SupplierPaymentNo;
            cTanggalVerifikasi.Text = entity.VerificationDateInString;
            cNoReferensi.Text = entity.ReferenceNo;
            cTanggalReferensi.Text = entity.ReferenceDateInString;
            cKeterangan.Text = entity.Remarks;

            subTotalPembayaran = entity.PaymentAmountHd;
            stampAmount = entity.StampAmount;
            
            lblttd1.Text = "Kabag. Keuangan";
            lblttd2.Text = "Direksi";
            lblttd3.Text = "Direksi";

            if (entity.BankName != "" && entity.BankName != null)
            {
                lblBank.Text = string.Format("Bank : {0}", entity.BankName);
            }
            else
            {
                lblBank.Text = string.Format("Bank : -");
            }

            Decimal paymentAmount = Convert.ToDecimal(subTotalPembayaran + stampAmount);
            string text = Function.NumberInWords(Convert.ToInt64(paymentAmount), true);
            lblPaymentAmount.Text = paymentAmount.ToString(Constant.FormatString.NUMERIC_2);
            lblPaymentAmountInString.Text = text;
            base.InitializeReport(param);
        }
    }
}

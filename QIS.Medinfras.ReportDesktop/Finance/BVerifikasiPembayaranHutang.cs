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
    public partial class BVerifikasiPembayaranHutang : BaseCustomDailyPotraitRpt
    {
        private decimal totalAmount = 0, stampAmount = 0, grandTotalAmount = 0;

        public BVerifikasiPembayaranHutang()
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
            //cSupplier.Text = entity.BusinessPartnerName;
            if (entity.BankID != null && entity.BankID != 0)
            {
                cCaraPembayaran.Text = entity.PaymentMethod + " | " + entity.BankName;
            }
            else
            {
                cCaraPembayaran.Text = entity.PaymentMethod;
            }
            cKeterangan.Text = entity.Remarks;

            totalAmount = BusinessLayer.GetSupplierPaymentDtList(string.Format("SupplierPaymentID = {0}", entity.SupplierPaymentID)).Sum(a => a.PaymentAmount);
            stampAmount = entity.StampAmount;
            grandTotalAmount = totalAmount + stampAmount;

            cStampAmount.Text = stampAmount.ToString(Constant.FormatString.NUMERIC_2);
            cGrandTotal.Text = grandTotalAmount.ToString(Constant.FormatString.NUMERIC_2);

            base.InitializeReport(param);
        }
    }
}

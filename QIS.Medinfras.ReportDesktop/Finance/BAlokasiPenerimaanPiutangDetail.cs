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
    public partial class BAlokasiPenerimaanPiutangDetail : BaseCustomDailyPotraitRpt
    {
        public BAlokasiPenerimaanPiutangDetail()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {

            vARReceivingHd entity = BusinessLayer.GetvARReceivingHdList(param[0]).FirstOrDefault();
           
            txtNoBayar.Text = entity.ARReceivingNo;
            txtTglBayar.Text = entity.ReceivingDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtKeterangan.Text = entity.Remarks;
            txtCustomer.Text = entity.BusinessPartnerName;
            txtNoVoucher.Text = entity.VoucherNo;
            if (entity.VoucherDate.ToString(Constant.FormatString.DATE_FORMAT) != "01-Jan-1900")
            {
                txtTglVoucher.Text = entity.VoucherDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            }

            string filterInvRcv = param[0] + " AND IsDeleted = 0";
            List<ARInvoiceReceiving> lstInvoiceDt = BusinessLayer.GetARInvoiceReceivingList(filterInvRcv);
            Decimal amountDetail = lstInvoiceDt.Sum(a => a.ReceivingAmount);
            Decimal totalAmount = entity.cfReceiveAmount;
            Decimal remainingAmount = totalAmount - amountDetail;

            txtPenerimaan.Text = entity.cfReceiveAmount.ToString(Constant.FormatString.NUMERIC_2);
            txtAlokasi.Text = amountDetail.ToString(Constant.FormatString.NUMERIC_2);
            txtSisa.Text = remainingAmount.ToString(Constant.FormatString.NUMERIC_2);

            vHealthcare h = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();
            lblTanggalTTD.Text = string.Format("{0}, {1}", h.City, entity.ReceivingDateInString);
            lblNamaTTD.Text = appSession.UserFullName;

            base.InitializeReport(param);
        }

    }
}

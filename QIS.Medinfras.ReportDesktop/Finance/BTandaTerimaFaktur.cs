using System;
using System.Data.OleDb;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.XtraReports;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Extensions;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BTandaTerimaFaktur : BaseDailyPortraitRpt
    {
        public BTandaTerimaFaktur()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            base.InitializeReport(param);

            vPurchaseInvoiceHd entity = BusinessLayer.GetvPurchaseInvoiceHdList(param[0])[0];
            lblNoInvoice.Text = entity.PurchaseInvoiceNo;
            lblTanggalProses.Text = entity.PurchaseInvoiceDate.ToString(Constant.FormatString.DATE_FORMAT);
            lblTanggalJatuhTempo.Text = entity.DueDate.ToString(Constant.FormatString.DATE_FORMAT);
            lblCatatan.Text = entity.Remarks;
            lblNoFakturSupplier.Text = entity.SupplierInvoiceNo;
            lblTanggalFakturSupplier.Text = entity.SupplierInvoiceDate.ToString(Constant.FormatString.DATE_FORMAT);
            lblNoFakturPajak.Text = entity.TaxInvoiceNo;
            lblTanggalFakturPajak.Text = entity.TaxInvoiceDate.ToString(Constant.FormatString.DATE_FORMAT);

            lblTotalPenerimaan.Text = String.Format("{0:#,##0.00}", entity.TotalTransactionAmount);
            lblDiskonFinal.Text = String.Format("{0:#,##0.00}", entity.DiscountValue);
            lblPPn.Text = String.Format("{0:#,##0.00}", entity.VATValue);
            lblPPh.Text = String.Format("{0:#,##0.00}", entity.PPHValue);
            lblOngkosKirim.Text = String.Format("{0:#,##0.00}", entity.ChargesAmount);
            lblMaterai.Text = String.Format("{0:#,##0.00}", entity.StampAmount);
            lblTotalFaktur.Text = String.Format("{0:#,##0.00}", entity.TotalNetTransactionAmount);

            lblSupplier.Text = entity.BusinessPartnerName;
            lblCreatedByName.Text = entity.LastUpdatedByName;
            
        }
    }
}

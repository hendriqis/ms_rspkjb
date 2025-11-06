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
            lblSupplier.Text = entity.BusinessPartnerName;
            lblNoInvoice.Text = entity.PurchaseInvoiceNo;
            lblTanggalProses.Text = entity.PurchaseInvoiceDate.ToString(Constant.FormatString.DATE_FORMAT);
            lblTanggalJatuhTempo.Text = entity.DueDate.ToString(Constant.FormatString.DATE_FORMAT);
            lblCatatan.Text = entity.Remarks;

            lblTotalPenerimaan.Text = entity.TotalTransactionAmount.ToString("N2");
            lblDiskonFinal.Text = entity.DiscountValue.ToString("N2");
            lblPPn.Text = entity.VATValue.ToString("N2");
            lblPPh.Text = entity.PPHValue.ToString("N2");
            lblOngkosKirim.Text = entity.ChargesAmount.ToString("N2");
            lblMaterai.Text = entity.StampAmount.ToString("N2");
            decimal TotalFaktur = entity.TotalTransactionAmount - entity.DiscountValue + entity.VATValue + entity.PPHValue + entity.ChargesAmount + entity.StampAmount;
            lblTotalFaktur.Text = TotalFaktur.ToString("N2");

            lblCreatedByName.Text = entity.LastUpdatedByName;

        }

        private void xrTableCell8_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (xrTableCell8.Text == "01-Jan-1900")
            {
                xrTableCell8.Text = "";
            }
        }
    }
}

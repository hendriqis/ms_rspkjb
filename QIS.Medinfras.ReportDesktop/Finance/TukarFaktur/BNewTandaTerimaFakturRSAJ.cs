using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BNewTandaTerimaFakturRSAJ : BaseCustomDailyPotraitRpt
    {
        public BNewTandaTerimaFakturRSAJ()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vPurchaseInvoiceHd entity = BusinessLayer.GetvPurchaseInvoiceHdList(param[0])[0];
            cPurchaseInvoiceNo.Text = entity.PurchaseInvoiceNo;
            cPurchaseInvoiceDate.Text = entity.PInvoiceDateInString;
            cDueDate.Text = entity.DueDateInString;
            //cDocumentDate.Text = entity.cfDocumentDateInString;
            cSupplier.Text = entity.BusinessPartnerName;
            cRemarks.Text = entity.Remarks;
            cReferenceNo.Text = entity.ReferenceNo;

            if (entity.TotalCreditNoteAmount > 0)
            {
                subCN.CanGrow = true;
                bNewTandaTerimaFakturDtCN.InitializeReport(entity.PurchaseInvoiceID);
            } else {
                xrLabel1.Visible = false;
                subCN.Visible = false;
            }

            cTotalPenerimaan.Text = entity.TotalTransactionAmount.ToString("N2");
            cTotalNotaKredit.Text = entity.TotalCreditNoteAmount.ToString("N2");
            cDiskonFinal.Text = entity.DiscountValue.ToString("N2");
            cPPN.Text = entity.VATValue.ToString("N2");
            cPPH.Text = entity.PPHValue.ToString("N2");
            cOngkosKirim.Text = entity.ChargesAmount.ToString("N2");
            cMaterai.Text = entity.StampAmount.ToString("N2");
            cTotalFaktur.Text = entity.TotalNetTransactionAmount.ToString("N2");

            lblCreatedByName.Text = entity.CreatedByName;

            base.InitializeReport(param);
        }

        private void xrTableCell25_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (xrTableCell25.Text == "01-Jan-1900")
            {
                xrTableCell25.Text = "";
            }
        }

        private void xrTableCell27_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (xrTableCell27.Text == "01-Jan-1900")
            {
                xrTableCell27.Text = "";
            }
        }

        private void xrTableCell23_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (xrTableCell23.Text == "01-Jan-1900")
            {
                xrTableCell23.Text = "";
            }
        }

    }
}

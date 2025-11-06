using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BNewTandaTerimaFakturDMRSMD : BaseCustomDailyPotraitRpt
    {
        public BNewTandaTerimaFakturDMRSMD()
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
                bNewTandaTerimaFakturDtCNDM.InitializeReport(entity.PurchaseInvoiceID);
            } else {
                xrLabel1.Visible = false;
                subCN.Visible = false;
            }

            cTotalPenerimaan.Text = entity.TotalTransactionAmount.ToString("N2");
            if (cTotalPenerimaan.Text == "0.00")
            {
                cTotalPenerimaan.Text = "0.00";
            }
            cTotalNotaKredit.Text = entity.TotalCreditNoteAmount.ToString("N2");
            if (cTotalNotaKredit.Text == "0.00")
            {
                cTotalNotaKredit.Text = "";
            }
            cDiskonFinal.Text = entity.DiscountValue.ToString("N2");
            if(cDiskonFinal.Text == "0.00")
            {
                cDiskonFinal.Text = "";
            }
            //cPPN.Text = entity.VATValue.ToString("N2");
            //if (cPPN.Text == "0.00")
            //{
            //    cPPN.Text = "";
            //}
            cPPH.Text = entity.PPHValue.ToString("N2");
            if (cPPH.Text == "0.00")
            {
                cPPH.Text = "";
            }
            cOngkosKirim.Text = entity.ChargesAmount.ToString("N2");
            if (cOngkosKirim.Text == "0.00")
            {
                cOngkosKirim.Text = "";
            }
            //cMaterai.Text = entity.StampAmount.ToString("N2");
            //if (cMaterai.Text == "0.00")
            //{
            //    cMaterai.Text = "";
            //}
            cTotalFaktur.Text = entity.TotalNetTransactionAmount.ToString("N2");
            if (cTotalFaktur.Text == "0.00")
            {
                cTotalFaktur.Text = "";
            }
            lblCreatedByName.Text = entity.CreatedByName;
            lblTanggal.Text = string.Format("Tanggal : {0} {1}", DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT), DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT));
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

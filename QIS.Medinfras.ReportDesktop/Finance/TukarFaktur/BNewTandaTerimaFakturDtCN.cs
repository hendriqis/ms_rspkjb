using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BNewTandaTerimaFakturDtCN : DevExpress.XtraReports.UI.XtraReport
    {
        public BNewTandaTerimaFakturDtCN()
        {
            InitializeComponent();
        }

        public void InitializeReport(int PurchaseInvoiceID)
        {
            List<vPurchaseInvoiceDt> lst = BusinessLayer.GetvPurchaseInvoiceDtList(string.Format(
                "PurchaseInvoiceID = {0} AND CreditNoteID IS NOT NULL AND IsDeleted = 0", PurchaseInvoiceID));

            this.DataSource = lst;
        }

    }
}

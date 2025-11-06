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
    public partial class DetailSuratPenagihan : DevExpress.XtraReports.UI.XtraReport
    {
        decimal kurs = 0;
        public DetailSuratPenagihan()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<vARInvoiceDt> lst, decimal exchange)
        {
            lblKurs.Text = exchange.ToString("N2");
            lblKurs1.Text = exchange.ToString("N2");
            kurs = exchange;
            this.DataSource = lst;
        }

        private void lblAmountUSD_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String claimedAmount = Convert.ToString(GetCurrentColumnValue("ClaimedAmount"));
            String ARInvoiceID = Convert.ToString(GetCurrentColumnValue("ARInvoiceID"));
            List<ARInvoiceDt> lstDt = BusinessLayer.GetARInvoiceDtList(string.Format("ARInvoiceID = {0}" , ARInvoiceID));
            decimal ClaimedAmount2 = Convert.ToDecimal(claimedAmount);
            decimal USD = ClaimedAmount2 / kurs;
            lblAmountUSD.Text = USD.ToString("N2");
            decimal USDTotal = 0;
            USDTotal =+ lstDt.Sum(p => p.ClaimedAmount / kurs);
            lblAmountUSDTotal.Text = USDTotal.ToString("N2");
        }
    }
}

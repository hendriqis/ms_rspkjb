using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using System.Collections.Generic;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPenagihanPiutangDataPasienRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public LPenagihanPiutangDataPasienRpt()
        {
            InitializeComponent();
        }

        public void InitializeReport(int ARInvoiceID)
        {
            List<vARInvoiceDt> lstARInvoiceDt = BusinessLayer.GetvARInvoiceDtList(string.Format("ARInvoiceID = {0}", ARInvoiceID));
            this.DataSource = lstARInvoiceDt;
        }

    }
}

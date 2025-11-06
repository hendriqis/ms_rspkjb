using System;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BSuratPenagihanPiutangRSRTHDt : DevExpress.XtraReports.UI.XtraReport
    {
        public BSuratPenagihanPiutangRSRTHDt()
        {
            InitializeComponent();
        }

        public void InitializeReport(int ARInvoiceID)
        {
            List<vARInvoiceDtRSRTH> lst = BusinessLayer.GetvARInvoiceDtRSRTHList(string.Format(
                                        "ARInvoiceID = {0} AND ISNULL(GCTransactionDetailStatus,'') != '{1}'", ARInvoiceID, Constant.TransactionStatus.VOID));

            this.DataSource = lst;
        }

        private void cNama_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string referenceno = GetCurrentColumnValue("ReferenceNo").ToString();
            string patientname = GetCurrentColumnValue("PatientName").ToString();

            if (!String.IsNullOrEmpty(referenceno))
            {
                cNama.Text = string.Format("{0} ({1})", patientname, referenceno);
            }
            else
            {
                cNama.Text = patientname;
            }
        }

    }
}

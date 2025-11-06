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
    public partial class BNewSuratPenagihanPiutangDt : DevExpress.XtraReports.UI.XtraReport
    {
        public BNewSuratPenagihanPiutangDt()
        {
            InitializeComponent();
        }

        private decimal total = 0;
        private decimal diskon = 0;
        private decimal diskonInPct = 0;
        private decimal penyesuaian = 0;

        public void InitializeReport(int ARInvoiceID)
        {
            List<vARInvoiceDt> lst = BusinessLayer.GetvARInvoiceDtList(string.Format(
                                        "ARInvoiceID = {0} AND ISNULL(GCTransactionDetailStatus,'') != '{1}'", ARInvoiceID, Constant.TransactionStatus.VOID));
            diskon = Convert.ToDecimal(lst.Sum(b => b.DiscountAmount));
            penyesuaian = Convert.ToDecimal(lst.Sum(b => b.VarianceAmount));
            total = Convert.ToDecimal(lst.Sum(b => b.TransactionAmount));

            diskonInPct = (diskon/total * 100);

            if (diskonInPct != 0)
            {
                cDiscountText.Text = string.Format("Diskon {0}%", diskonInPct.ToString(Constant.FormatString.NUMERIC_2));
            }
            else
            {
                cDiscountText.Text = "Diskon";
            }

            this.DataSource = lst;
        }

        private void cNama_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string referenceno = GetCurrentColumnValue("ReferenceNo").ToString();
            string medicalno = GetCurrentColumnValue("MedicalNo").ToString();
            string patientname = GetCurrentColumnValue("PatientName").ToString();

            if (!String.IsNullOrEmpty(referenceno))
            {
                cNama.Text = string.Format("{0}|{1} ({2})", medicalno, patientname, referenceno);
            }
            else
            {
                cNama.Text = string.Format("{0}|{1}", medicalno, patientname);
            }
        }

        private void rowDiscount_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (diskon != 0)
            {
                rowDiscount.Visible = true;
            }
            else
            {
                rowDiscount.Visible = false;
            }
        }

        private void rowPenyesuaian_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (penyesuaian != 0)
            {
                rowPenyesuaian.Visible = true;
            }
            else
            {
                rowPenyesuaian.Visible = false;
            }
        }
    }
}

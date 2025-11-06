using System;
using System.Linq;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BSuratPenagihanPiutangRawatInapPerusahaanENG : BaseCustomDailyPotraitRpt
    {
        public BSuratPenagihanPiutangRawatInapPerusahaanENG()
        {
            InitializeComponent();
        }

        private decimal transaksi = 0;
        private decimal klaim = 0;
        private decimal diskon = 0;
        private decimal penyesuaian = 0;

        public override void InitializeReport(string[] param)
        {
            vHealthcare h = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();
            lblTanggalTTD.Text = string.Format("{0}, {1}", h.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));
            lblNamaTTD.Text = appSession.UserFullName;

            string filterARInvoiceDt = string.Format("{0} AND ISNULL(GCTransactionDetailStatus,'') != '{1}'", param[0], Constant.TransactionStatus.VOID);
            List<vARInvoiceDt> arInvoiceDt = BusinessLayer.GetvARInvoiceDtList(filterARInvoiceDt);
            Int64 total = Convert.ToInt64(arInvoiceDt.Sum(x => x.ClaimedAmount));
            lblTerbilang.Text = Helper.NumberInWordsInEnglish(total, true);

            //transaksi = Convert.ToDecimal(arInvoiceDt.Sum(a => a.TransactionAmount));
            //klaim = Convert.ToDecimal(arInvoiceDt.Sum(b => b.ClaimedAmount));
            //diskon = klaim - transaksi;

            diskon = Convert.ToDecimal(arInvoiceDt.Sum(b => b.DiscountAmount));
            penyesuaian = Convert.ToDecimal(arInvoiceDt.Sum(b => b.VarianceAmount));

            base.InitializeReport(param);
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

        private void xrTableCell11_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String PatientName = GetCurrentColumnValue("PatientName").ToString();
            String ReferenceNo = GetCurrentColumnValue("ReferenceNo").ToString();

            if (ReferenceNo != null && ReferenceNo != "")
            {
                xrTableCell11.Text = string.Format("{0} ({1})", PatientName, ReferenceNo);
            }
            else
            {
                xrTableCell11.Text = string.Format("{0}", PatientName);
            }
        }

        private void xrTableCell13_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String RegistrationNo = GetCurrentColumnValue("RegistrationNo").ToString();
            String LinkedRegistrationNo = GetCurrentColumnValue("LinkedRegistrationNo").ToString();

            if (RegistrationNo != null && RegistrationNo != "")
            {
                xrTableCell13.Text = string.Format("{0} {1}", RegistrationNo, LinkedRegistrationNo);
            }
            else
            {
                xrTableCell13.Text = string.Format("{0}", RegistrationNo);
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

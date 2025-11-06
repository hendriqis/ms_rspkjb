using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BPembayaranHutang : BaseCustomDailyPotraitRpt
    {
        private int _lineNumber = 0;

        public BPembayaranHutang()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            _lineNumber = 0;
            List<vSupplierPaymentInvoiceReport> lstEntity = BusinessLayer.GetvSupplierPaymentInvoiceReportList(param[0].ToString());

            if (lstEntity.Count > 0)
            {
                var groupData = lstEntity.GroupBy(test => test.PurchaseInvoiceNo).Select(grp => grp.First()).ToList();

                Decimal totalTransaksi = 0;
                foreach (vSupplierPaymentInvoiceReport e in groupData)
                {
                    totalTransaksi = totalTransaksi + e.PaymentAmount;
                }

                cTotalPembayaran.Text = totalTransaksi.ToString("N2");
            }
            else
            {
                cTotalPembayaran.Text = "0";
            }

            base.InitializeReport(param);
        }

        //private void lblNoHeader_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        //{
        //    lblNoHeader.Text = (++_lineNumber).ToString();
        //}

        //private void xrTableCell22_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        //{

        //}

        private void cSumCreditNote_AfterPrint(object sender, EventArgs e)
        {
            _lineNumber = _lineNumber;
        }

        //private void xrTableCell33_AfterPrint(object sender, EventArgs e)
        //{
        //    _lineNumber = 0;
        //}
    }
}

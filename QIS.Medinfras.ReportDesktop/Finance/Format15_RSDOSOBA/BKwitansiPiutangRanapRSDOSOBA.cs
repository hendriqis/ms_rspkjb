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
    public partial class BKwitansiPiutangRanapRSDOSOBA : BaseCustomDailyPotraitA5Rpt
    {
        private string City = "";

        public BKwitansiPiutangRanapRSDOSOBA()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare h = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();

            string ARInvoiceID = param[0].Replace("ARInvoiceID = ", "");
            ARInvoiceHd arInvoiceHd = BusinessLayer.GetARInvoiceHd(Convert.ToInt32(ARInvoiceID));

            lblTTD1.Text = appSession.UserFullName;
            lblTanggal.Text = string.Format("{0}, {1}", h.City, arInvoiceHd.DocumentDate.ToString(Constant.FormatString.DATE_FORMAT));

            base.InitializeReport(param);
        }
        private void lblTanggal_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string receiptDate = Convert.ToDateTime(GetCurrentColumnValue("ARReceiptDate")).ToString(Constant.FormatString.DATE_FORMAT);
            string createdDate = Convert.ToDateTime(GetCurrentColumnValue("CreatedDate")).ToString(Constant.FormatString.DATE_FORMAT);

            if (receiptDate != "01-Jan-1900")
            {
                lblTanggal.Text = string.Format("{0}, {1}", City, receiptDate);
            }
            else
            {
                lblTanggal.Text = string.Format("{0}, {1}", City, createdDate);
            }
        }
    }
}

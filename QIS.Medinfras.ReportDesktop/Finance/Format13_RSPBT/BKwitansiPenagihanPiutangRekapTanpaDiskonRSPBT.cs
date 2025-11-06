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
    public partial class BKwitansiPenagihanPiutangRekapTanpaDiskonRSPBT : BaseCustomDailyPotraitRpt
    {
        public BKwitansiPenagihanPiutangRekapTanpaDiskonRSPBT()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare h = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();

            pctReceiptLogo.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/LOGO_TEXT.png");
            cReceiptHealthcareName.Text = h.HealthcareName;
            cReceiptHealthcareAddress.Text = string.Format("{0} {1} {2}", h.StreetName, h.City, h.ZipCode);
            cReceiptHealthcarePhone.Text = string.Format("{0} {1}", h.PhoneNo1, h.FaxNo1);

            vARInvoiceHd arInvoice = BusinessLayer.GetvARInvoiceHdList(param[0]).FirstOrDefault();
            lblReceivedFrom.Text = arInvoice.CustomerBillToName;

            lblTanggal.Text = string.Format("{0}, {1}", h.City, arInvoice.ARInvoiceDateInString);
            lblTTD1.Text = arInvoice.ApprovedByName;

            base.InitializeReport(param);
        }

    }
}

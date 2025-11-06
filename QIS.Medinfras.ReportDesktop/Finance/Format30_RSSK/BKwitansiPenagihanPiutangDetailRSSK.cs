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
    public partial class BKwitansiPenagihanPiutangDetailRSSK : BaseCustomDailyPotraitA5Rpt
    {
        public BKwitansiPenagihanPiutangDetailRSSK()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string HealthcareID = "001";
            vHealthcare h = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", HealthcareID)).FirstOrDefault();
            SettingParameter setvar = BusinessLayer.GetSettingParameterList(string.Format(
                    "ParameterCode = '{0}'", Constant.SettingParameter.MANAGER_KEUANGAN)).FirstOrDefault();

            pctReceiptLogo.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/logo.png");
            cReceiptHealthcareName.Text = h.HealthcareName;
            cReceiptHealthcareAddress.Text = string.Format("{0} {1} {2}", h.StreetName, h.City, h.ZipCode);
            cReceiptHealthcarePhone.Text = string.Format("{0} {1}", h.PhoneNo1, h.FaxNo1);

            string ARInvoiceID = param[0];
            ARInvoiceHd arInvoiceHd = BusinessLayer.GetARInvoiceHdList(ARInvoiceID).FirstOrDefault();
            vARInvoiceDt entity = BusinessLayer.GetvARInvoiceDtList(ARInvoiceID).FirstOrDefault();

            lblTanggal.Text = string.Format("{0}, {1}", h.City, entity.SignatureDateInString);

            lblTTD1.Text = setvar.ParameterValue;
            lblTTD2.Text = setvar.Notes;

            base.InitializeReport(param);
        }

    }
}

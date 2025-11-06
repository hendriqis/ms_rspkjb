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
    public partial class BKwitansiPenagihanPiutangRekapRSSM : BaseCustomDailyPotraitRpt
    {
        public BKwitansiPenagihanPiutangRekapRSSM()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare h = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();
            SettingParameter setvar = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode = '{0}'", Constant.SettingParameter.FN_KEPALABAGIAN_KEUANGAN)).FirstOrDefault();
            SettingParameterDt setvardt = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, setvar.ParameterCode);

            pctReceiptLogo.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/logo.png");
            cReceiptHealthcareName.Text = h.HealthcareName;
            cReceiptHealthcareAddress.Text = string.Format("{0} {1} {2}", h.StreetName, h.City, h.ZipCode);
            cReceiptHealthcarePhone.Text = string.Format("{0} {1}", h.PhoneNo1, h.FaxNo1);
            cReceiptHealhcareEmail.Text = string.Format("{0}", h.Email);

            string ARInvoiceID = param[0].Replace("ARInvoiceID = ", "");
            ARInvoiceHd arInvoiceHd = BusinessLayer.GetARInvoiceHd(Convert.ToInt32(ARInvoiceID));

            lblTanggal.Text = string.Format("{0}, {1}", h.City, arInvoiceHd.DocumentDate.ToString(Constant.FormatString.DATE_FORMAT));
            lblTTD1.Text = setvardt.ParameterValue;

            base.InitializeReport(param);
        }

    }
}

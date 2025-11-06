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
    public partial class BKwitansiPenagihanPiutangDetailNonOpRSAJ : BaseCustomDailyPotraitA5Rpt
    {
        public BKwitansiPenagihanPiutangDetailNonOpRSAJ()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vARInvoiceHd entity = BusinessLayer.GetvARInvoiceHdList(param[0]).FirstOrDefault();
            vHealthcare h = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();
            SettingParameter setvar = BusinessLayer.GetSettingParameterList(string.Format(
                    "ParameterCode = '{0}'", Constant.SettingParameter.MANAGER_KEUANGAN)).FirstOrDefault();
            SettingParameterDt setvardt = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, setvar.ParameterCode);

            cBusinessPartnerName.Text = entity.PrintAsName;
            pctReceiptLogo.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/LOGO_TEXT.png");
            cReceiptHealthcareName.Text = h.HealthcareName;
            cReceiptHealthcareAddress.Text = string.Format("{0} {1} {2}", h.StreetName, h.City, h.ZipCode);
            cReceiptHealthcarePhone.Text = string.Format("{0} {1}", h.PhoneNo1, h.FaxNo1);

            string ARInvoiceID = param[0].Replace("ARInvoiceID = ", "");
            ARInvoiceHd arInvoiceHd = BusinessLayer.GetARInvoiceHd(Convert.ToInt32(ARInvoiceID));

            lblTanggal.Text = string.Format("{0}, {1}", h.City, arInvoiceHd.ARInvoiceDate.ToString(Constant.FormatString.DATE_FORMAT));
            lblTTD1.Text = setvardt.ParameterValue;
            lblTTD2.Text = setvar.ParameterName;

            base.InitializeReport(param);
        }

    }
}

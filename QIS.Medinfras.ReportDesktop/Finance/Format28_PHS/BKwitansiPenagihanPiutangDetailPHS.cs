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
    public partial class BKwitansiPenagihanPiutangDetailPHS : BaseCustomDailyPotraitRpt
    {
        public BKwitansiPenagihanPiutangDetailPHS()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare h = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();
            SettingParameter setvar = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode = '{0}'", Constant.SettingParameter.FN_KEPALABAGIAN_KEUANGAN)).FirstOrDefault();
            SettingParameterDt setvardt = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, setvar.ParameterCode);

            pctReceiptLogo.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/LOGO_TEXT_PHS.png");
            cReceiptHealthcareName.Text = h.HealthcareName;
            cReceiptHealthcareAddress.Text = string.Format("{0} {1} {2}", h.StreetName, h.City, h.ZipCode);
            cReceiptHealthcarePhone.Text = string.Format("{0} {1}", h.PhoneNo1, h.FaxNo1);

            vARInvoiceHd arInvoice = BusinessLayer.GetvARInvoiceHdList(param[0]).FirstOrDefault();
            lblReceivedFrom.Text = arInvoice.PrintAsName;
            lblInvoiceNo.Text = arInvoice.ARInvoiceNo;
            lblInvoiceDate.Text = arInvoice.ARInvoiceDateInString;

            lblPaymentDate.Text = arInvoice.ARInvoiceDateInString;

            lblTanggal.Text = string.Format("{0}, {1}", h.City, arInvoice.ARInvoiceDateInString);
            lblTTD1.Text = setvardt.ParameterValue;
            lblTTD2.Text = setvar.ParameterName;

            if (arInvoice.TotalClaimedAmount < 5000000)
            {
                ttdpic.ImageUrl = string.Format("{0}/Finance/ttd/ttd_manager_keuangan.png", AppConfigManager.QISPhysicalDirectory);
            }
            base.InitializeReport(param);
        }

    }
}

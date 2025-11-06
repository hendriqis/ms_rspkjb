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
    public partial class BInvoicePaymentSlipRekapRSRT : BaseCustomDailyPotraitRSRTRpt
    {
        public BInvoicePaymentSlipRekapRSRT()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare h = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();
            vARInvoiceHd1 entity = BusinessLayer.GetvARInvoiceHd1List(param[0]).FirstOrDefault();

            SettingParameter setvar = BusinessLayer.GetSettingParameterList(string.Format(
                    "ParameterCode = '{0}'", Constant.SettingParameter.FN_KEPALABAGIAN_KEUANGAN)).FirstOrDefault();
            SettingParameterDt setvardt = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, setvar.ParameterCode);

            lblTanggalTTD.Text = string.Format("{0}, {1}", h.City, entity.ARInvoiceDateInString);
            lblTTD1.Text = entity.ARInvoiceDateInString;
            lblTTD2.Text = "Bagian Keuangan";  //setvar.ParameterName;

            base.InitializeReport(param);
        }

    }
}

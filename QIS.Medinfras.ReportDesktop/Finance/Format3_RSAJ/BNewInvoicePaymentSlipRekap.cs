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
    public partial class BNewInvoicePaymentSlipRekap : BaseCustomDailyPotraitRpt
    {
        public BNewInvoicePaymentSlipRekap()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vARInvoiceHd entity = BusinessLayer.GetvARInvoiceHdList(param[0]).FirstOrDefault();
            vHealthcare h = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();
            SettingParameter setvar = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode = '{0}'", Constant.SettingParameter.MANAGER_KEUANGAN)).FirstOrDefault();
            SettingParameterDt setvardt = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, setvar.ParameterCode);

            lblTanggalTTD.Text = string.Format("{0}, {1}", h.City, entity.ARInvoiceDateInString);
            lblNamaTTD.Text = setvardt.ParameterValue;
            lblKeteranganTTD.Text = setvar.ParameterName;

            base.InitializeReport(param);
        }

    }
}

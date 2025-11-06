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
    public partial class BNew6SuratPenagihanPiutang : BaseCustomDailyPotraitRpt
    {
        public BNew6SuratPenagihanPiutang()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare h = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();
            SettingParameter setvar = BusinessLayer.GetSettingParameterList(string.Format(
                    "ParameterCode = '{0}'", Constant.SettingParameter.FN_KEPALABAGIAN_KEUANGAN)).FirstOrDefault();
            SettingParameterDt setvardt = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, setvar.ParameterCode);

            SettingParameterDt setvarFax = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, Constant.SettingParameter.FN_FAX_NO_AR);
            SettingParameterDt setvarEmail = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, Constant.SettingParameter.FN_EMAIL_AR);
            lblNoFax.Text = setvarFax.ParameterValue;
            lblEmail.Text = setvarEmail.ParameterValue;

            lblHeaderTTD1.Text = string.Format("{0}, {1}", h.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));
            lblTTD1.Text = setvardt.ParameterValue;
            lblTTD2.Text = setvar.ParameterName;

            ARInvoiceHd invoiceHd = BusinessLayer.GetARInvoiceHdList(param[0]).FirstOrDefault();

            List<GetTransferAccountBankRSSC> lstVirtualAccount = BusinessLayer.GetTransferAccountBankRSSCList(invoiceHd.ARInvoiceID);

            #region Transaction
            subVirtualAccount.CanGrow = true;
            BNewPenagihanPiutangVirtualAccountDetail.InitializeReport(lstVirtualAccount);
            #endregion

            base.InitializeReport(param);
        }
    }
}

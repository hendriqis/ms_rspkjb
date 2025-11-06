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
    public partial class BNewSuratPenagihanPiutang : BaseCustomDailyPotraitRpt
    {
        public BNewSuratPenagihanPiutang()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare h = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();
            SettingParameter setvar = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode = '{0}'", Constant.SettingParameter.MANAGER_KEUANGAN)).FirstOrDefault();
            SettingParameterDt setvardt = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, setvar.ParameterCode);
            
            SettingParameterDt setvarEmail = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, Constant.SettingParameter.FN_EMAIL_AR);
            lblEmailRS.Text = setvarEmail.ParameterValue;

            lblTTD1.Text = setvardt.ParameterValue;
            lblTTD2.Text = setvar.ParameterName;

            ARInvoiceHd invoiceHd = BusinessLayer.GetARInvoiceHdList(param[0]).FirstOrDefault();
            subPiutangDt.CanGrow = true;
            bNewSuratPenagihanPiutangDt.InitializeReport(invoiceHd.ARInvoiceID);

            lblHeaderTTD1.Text = string.Format("{0}, {1}", h.City, invoiceHd.ARInvoiceDate.ToString(Constant.FormatString.DATE_FORMAT));

            base.InitializeReport(param);
        }

    }
}

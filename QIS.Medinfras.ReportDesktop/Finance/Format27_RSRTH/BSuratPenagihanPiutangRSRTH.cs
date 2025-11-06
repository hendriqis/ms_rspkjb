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
    public partial class BSuratPenagihanPiutangRSRTH : BaseCustomDailyPotraitRpt
    {
        public BSuratPenagihanPiutangRSRTH()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare h = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();

            ARInvoiceHd invoiceHd = BusinessLayer.GetARInvoiceHdList(param[0]).FirstOrDefault();
            vARInvoiceHd1 entity = BusinessLayer.GetvARInvoiceHd1List(param[0]).FirstOrDefault();

            SettingParameter setvar = BusinessLayer.GetSettingParameterList(string.Format(
                    "ParameterCode = '{0}'", Constant.SettingParameter.DIREKTUR_UMUM)).FirstOrDefault();

            List<SettingParameterDt> setvarAR = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                                    "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                                                                    appSession.HealthcareID,
                                                                    Constant.SettingParameter.FN_EMAIL_AR,
                                                                    Constant.SettingParameter.FN_FAX_NO_AR,
                                                                    Constant.SettingParameter.DIREKTUR_UMUM));
            lblEmailPenagihan.Text = setvarAR.Where(a => a.ParameterCode == Constant.SettingParameter.FN_EMAIL_AR).FirstOrDefault().ParameterValue;
            lblContactPenagihan.Text = setvarAR.Where(a => a.ParameterCode == Constant.SettingParameter.FN_FAX_NO_AR).FirstOrDefault().ParameterValue;

            lblHeaderTTD1.Text = string.Format("{0}, {1}", h.City, entity.SignatureDateInString);
            lblTTD1.Text = setvarAR.Where(a => a.ParameterCode == Constant.SettingParameter.DIREKTUR_UMUM).FirstOrDefault().ParameterValue;
            lblTTD2.Text = setvar.Notes;

            subPiutangDt.CanGrow = true;
            bSuratPenagihanPiutangRSRTHDt.InitializeReport(invoiceHd.ARInvoiceID);

            base.InitializeReport(param);
        }

    }
}

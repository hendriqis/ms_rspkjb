using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.XtraReports.UI;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BSuratPengantarTagihanRJRSDOSOBA : BaseRpt
    {
        public BSuratPengantarTagihanRJRSDOSOBA()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[]param)
        {
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            vARInvoiceHdTagihan entity = BusinessLayer.GetvARInvoiceHdTagihanList(param[0]).FirstOrDefault();

            lblInvoiceNo.Text = string.Format("No : {0}", entity.ARInvoiceNo);
            lblCustomerBillToName.Text = entity.CustomerBillToName;
            lblTotalClaimedAmount.Text = string.Format("Rp. {0}" , entity.TotalClaimedAmount.ToString("N2"));
            lblTotalClaimedAmountInStringInd.Text = entity.TotalClaimedAmountInStringInd;
            lblcfBankAccountInformation.Text = entity.cfBankAccountInformation;

            lblHealthcare.Text = oHealthcare.HealthcareName;
            lblSignDate.Text = string.Format("{0}, {1}", oHealthcare.City, entity.ARInvoiceDateInString);
            lblSignHealthcareName.Text = oHealthcare.HealthcareName;

            List<SettingParameter> lstSP = BusinessLayer.GetSettingParameterList(
                                        string.Format("ParameterCode IN ('{0}','{1}')",
                                                        Constant.SettingParameter.FN_EMAIL_AR,
                                                        Constant.SettingParameter.DIREKTUR_KEUANGAN
                                                     )
                               );

            List<SettingParameterDt> lstSPDT = BusinessLayer.GetSettingParameterDtList(
                                                    string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
                                                                    appSession.HealthcareID,
                                                                    Constant.SettingParameter.FN_EMAIL_AR,
                                                                    Constant.SettingParameter.DIREKTUR_KEUANGAN
                                                                 )
                                           );
            lblEmailPenagihan1.Text = string.Format("1. Kekurangan berkas claim mohon e-mail ke : {0}", lstSPDT.Where(a => a.ParameterCode == Constant.SettingParameter.FN_EMAIL_AR).FirstOrDefault().ParameterValue);
            lblEmailPenagihan2.Text = string.Format("2. Untuk informasi pembayaran di e-mail ke : {0}", lstSPDT.Where(a => a.ParameterCode == Constant.SettingParameter.FN_EMAIL_AR).FirstOrDefault().ParameterValue);

            lblSignCaption.Text = lstSP.Where(a => a.ParameterCode == Constant.SettingParameter.DIREKTUR_KEUANGAN).FirstOrDefault().ParameterName;
            lblSignName.Text = lstSPDT.Where(a => a.ParameterCode == Constant.SettingParameter.DIREKTUR_KEUANGAN).FirstOrDefault().ParameterValue;
            base.InitializeReport(param);
        }
    }
}


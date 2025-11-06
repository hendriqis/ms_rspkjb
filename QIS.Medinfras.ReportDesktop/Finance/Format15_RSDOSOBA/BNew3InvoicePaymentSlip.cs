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
    public partial class BNew3InvoicePaymentSlip : BaseCustomDailyPotraitRpt
    {
        public BNew3InvoicePaymentSlip()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID)).FirstOrDefault();

            vARInvoiceHd entity = BusinessLayer.GetvARInvoiceHdList(param[0]).FirstOrDefault();

            cBusinessPartnerName.Text = entity.CustomerBillToName;

            lblRemarksHd.Text = entity.Remarks;

            cARInvoiceNo.Text = entity.ARInvoiceNo;
            cARInvoiceDate.Text = entity.ARInvoiceDateInString;
            cDueDate.Text = entity.DueDateInString;

            List<GetTransferAccountBank> lstVirtualAccount = BusinessLayer.GetTransferAccountBankList(entity.ARInvoiceID);

            #region Transaction
            subVirtualAccount.CanGrow = true;
            BNew1InvoicePaymentVirtualAccountDetail.InitializeReport(lstVirtualAccount);
            #endregion

            List<SettingParameter> lstSP = BusinessLayer.GetSettingParameterList(
                                                    string.Format("ParameterCode IN ('{0}','{1}')",
                                                                    Constant.SettingParameter.FN_EMAIL_AR,
                                                                    Constant.SettingParameter.FN_KEPALABAGIAN_KEUANGAN
                                                                 )
                                           );

            List<SettingParameterDt> lstSPDT = BusinessLayer.GetSettingParameterDtList(
                                                    string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
                                                                    appSession.HealthcareID,
                                                                    Constant.SettingParameter.FN_EMAIL_AR
                                                                //Constant.SettingParameter.DIREKTUR_KEUANGAN
                                                                 )
                                           );
            lblEmailPenagihan.Text = lstSPDT.Where(a => a.ParameterCode == Constant.SettingParameter.FN_EMAIL_AR).FirstOrDefault().ParameterValue;

            lblSignDate.Text = string.Format("{0}, {1}", entityHealthcare.City, entity.ARInvoiceDateInString);
            lblSignHealthcare.Text = entityHealthcare.HealthcareName;
            lblSignCaption.Text = lstSP.Where(a => a.ParameterCode == Constant.SettingParameter.FN_KEPALABAGIAN_KEUANGAN).FirstOrDefault().ParameterName;
           // lblSignName.Text = lstSPDT.Where(a => a.ParameterCode == Constant.SettingParameter.DIREKTUR_KEUANGAN).FirstOrDefault().ParameterValue;

            base.InitializeReport(param);
        }

    }
}

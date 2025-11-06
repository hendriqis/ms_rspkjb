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
    public partial class BNew8SuratPenagihanIng : BaseCustomDailyPotraitRpt
    {
        public BNew8SuratPenagihanIng()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string ARInvoiceID = param[0];
            string type = param[2];
            string diskon = param[3];
            decimal exchange = Convert.ToDecimal(param[4]);

            vHealthcare h = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();

            List<SettingParameterDt> setvarAR = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                                    "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
                                                                    appSession.HealthcareID,
                                                                    Constant.SettingParameter.FN_EMAIL_AR,
                                                                    Constant.SettingParameter.FN_FAX_NO_AR));
            lblEmailPenagihan.Text = setvarAR.Where(a => a.ParameterCode == Constant.SettingParameter.FN_EMAIL_AR).FirstOrDefault().ParameterValue;
            lblContactPenagihan.Text = setvarAR.Where(a => a.ParameterCode == Constant.SettingParameter.FN_FAX_NO_AR).FirstOrDefault().ParameterValue;
            lblDate.Text = string.Format("{0}, {1}", h.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));

            vARInvoiceHd entityhd = BusinessLayer.GetvARInvoiceHdList(string.Format("{0}", ARInvoiceID)).FirstOrDefault();

            #region AccountIDR
            lblAccountIDR.Text = string.Format("{0} ACCOUNT DETAIL (IDR)", h.HealthcareName);
            lblAccountName.Text = entityhd.BankAccountName;
            lblBankName.Text = entityhd.BankName;
            lblAccountNo.Text = entityhd.BankAccountNo;
            lblBankCode.Text = entityhd.BankCode;
            lblBranch.Text = entityhd.BankBranch;
            lblBankAddress.Text = string.Format("{0} {1} {2} {3} {4}", entityhd.BankStreetName, entityhd.BankDistrict, entityhd.BankCounty, entityhd.BankCity, entityhd.BankState);
            #endregion

            #region AccountUSD
            lblAccountUSD.Text = string.Format("{0} ACCOUNT DETAIL (USD)", h.HealthcareName);
            lblAccountName2.Text = entityhd.BankAccountName;
            lblBankName2.Text = entityhd.BankName;
            lblAccountNo2.Text = entityhd.BankAccountNo;
            lblBankCode2.Text = entityhd.BankCode;
            lblBranch2.Text = entityhd.BankBranch;
            lblBankAddress2.Text = string.Format("{0} {1} {2} {3} {4}", entityhd.BankStreetName, entityhd.BankDistrict, entityhd.BankCounty, entityhd.BankCity, entityhd.BankState);
            #endregion

            List<vARInvoiceDt> lstInvoice = BusinessLayer.GetvARInvoiceDtList(string.Format("{0}", ARInvoiceID));

            #region Transaction
            subTransaction.CanGrow = true;
            detailSuratPenagihan.InitializeReport(lstInvoice, exchange);
            #endregion

            base.InitializeReport(param);
        }
    }
}

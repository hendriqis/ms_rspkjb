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
    public partial class BInvoicePaymentSlipDetailRSASIH : BaseCustomDailyPotraitRpt
    {
        public BInvoicePaymentSlipDetailRSASIH()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vARInvoiceHd1 entity = BusinessLayer.GetvARInvoiceHd1List(param[0]).FirstOrDefault();

            cBusinessPartnerName.Text = entity.CustomerBillToName;
            cBusinessPartnerAddressLine1.Text = entity.CustomerBillToStreetName;
            cBusinessPartnerAddressLine2.Text = entity.CustomerBillToCity + " " + entity.CustomerBillToState;

            cARInvoiceNo.Text = entity.ARInvoiceNo;
            cARInvoiceDate.Text = entity.ARInvoiceDateInString;
            cDueDate.Text = entity.DueDateInString;

            cRemarks.Text = entity.Remarks;

            cBankNameDisplay.Text = entity.BankNameDisplay;
            cBankAccountNoCaption.Text = entity.BankAccountNoCaption;
            cBankAccountNo.Text = entity.BankAccountNo;
            cBankAccountName.Text = entity.BankAccountName;
            cContactPerson.Text = entity.ContactPerson;

            vHealthcare h = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();

            SettingParameter setvar = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode = '{0}'", Constant.SettingParameter.FN_KEPALABAGIAN_KEUANGAN)).FirstOrDefault();
            SettingParameterDt setvardt = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, setvar.ParameterCode);

            SettingParameter emailFN = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode = '{0}'", Constant.SettingParameter.FN_EMAIL_AR)).FirstOrDefault();
            SettingParameterDt emailFNdt = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, emailFN.ParameterCode);

            cKeterangan2.Text = string.Format("dikirimkan kebagian keuangan {0} melalui email ke alamat : {1}", h.ShortName, emailFNdt.ParameterValue);

            lblTanggalTTD.Text = string.Format("{0}, {1}", h.City, entity.DocumentDateInString);
            lblTTD1.Text = setvardt.ParameterValue;
            lblTTD2.Text = setvar.Notes;

            base.InitializeReport(param);
        }

    }
}

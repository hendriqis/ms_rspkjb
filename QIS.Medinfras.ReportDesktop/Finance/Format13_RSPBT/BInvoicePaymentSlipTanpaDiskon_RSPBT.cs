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
    public partial class BInvoicePaymentSlipTanpaDiskon_RSPBT : BaseCustomDailyPotraitRpt
    {
        public BInvoicePaymentSlipTanpaDiskon_RSPBT()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vARInvoiceHd entity = BusinessLayer.GetvARInvoiceHdList(param[0]).FirstOrDefault();

            cBusinessPartnerName.Text = entity.CustomerBillToName;
            cBusinessPartnerAddressLine1.Text = entity.CustomerBillToStreetName;
            cBusinessPartnerAddressLine2.Text = entity.CustomerBillToCity + " " + entity.CustomerBillToState;

            cARInvoiceNo.Text = entity.ARInvoiceNo;
            cARInvoiceDate.Text = entity.ARInvoiceDateInString;
            cDueDate.Text = entity.DueDateInString;

            cBankName.Text = entity.BankName;
            cBankAccountNo.Text = entity.BankAccountNo;
            cBankAccountName.Text = entity.BankAccountName;

            vHealthcare h = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();
            lblTanggalTTD.Text = string.Format("{0}, {1}", h.City, entity.ARInvoiceDateInString);
            lblNamaTTD.Text = appSession.UserFullName;

            base.InitializeReport(param);
        }

    }
}

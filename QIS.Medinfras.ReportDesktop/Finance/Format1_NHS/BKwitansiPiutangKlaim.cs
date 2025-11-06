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
    public partial class BKwitansiPiutangKlaim : BaseCustomDailyPotraitRpt
    {
        public BKwitansiPiutangKlaim()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare h = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();
            xrHealthcareLogoText.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/LOGO_TEXT.png");

            string filterExpression1 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.SA_KABAG_BILLING);
            SettingParameterDt lstParam1 = BusinessLayer.GetSettingParameterDtList(filterExpression1).FirstOrDefault();
            lblTTD.Text = lstParam1.ParameterValue;

            vARInvoiceHd arInvoice = BusinessLayer.GetvARInvoiceHdList(param[0]).FirstOrDefault();
            lblReceivedFrom.Text = arInvoice.CustomerBillToName;
            lblInvoiceNo.Text = arInvoice.ARInvoiceNo;
            lblInvoiceDate.Text = arInvoice.ARInvoiceDateInString;

            base.InitializeReport(param);
        }

    }
}

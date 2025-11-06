using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using System.Data;

namespace QIS.Medinfras.Report
{
    public partial class BPaymentReceipt : BaseDailyPortraitRpt
    {
        public BPaymentReceipt()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare entity = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID))[0];
            lblReceiptDateCity.Text = string.Format("{0}, [ReceiptDateInString]", entity.City);
            base.InitializeReport(param);
        }

        private void xrLabel19_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Int64 total = Convert.ToInt64(GetCurrentColumnValue("ReceiptAmount"));
            ((XRLabel)sender).Text = Helper.NumberInWords(total, true);
        }

        private void xrLabel28_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String nama = GetCurrentColumnValue("PatientName").ToString();
            ((XRLabel)sender).Text = Helper.GetTextFormatText(page, Constant.TextFormat.PAYMENT_RECEIPT_DESCRIPTION).Replace("[PatientName]", nama);
        }

    }
}

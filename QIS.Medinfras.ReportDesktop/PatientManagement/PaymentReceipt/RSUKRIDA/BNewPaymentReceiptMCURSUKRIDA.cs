using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BNewPaymentReceiptMCURSUKRIDA : BaseRpt
    {
        private string HealthcareName = "";
        private string Address = "";
        private string City = "";

        public BNewPaymentReceiptMCURSUKRIDA()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            //String HealthcareID = AppSession.UserLogin.HealthcareID;
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '001'")).FirstOrDefault();

            xrLogo.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/logo.png");

            HealthcareName = oHealthcare.HealthcareName;
            Address = oHealthcare.StreetName;
            City = oHealthcare.City;

            lblHealthcareName.Text = HealthcareName;
            lblAddress.Text = Address;
            lblCity.Text = City;

            List<GetPaymentReceiptCustomMCURSUKRIDA> lstEntity = BusinessLayer.GetPaymentReceiptCustomMCURSUKRIDAList(Convert.ToInt32(param[0]));

            this.DataSource = lstEntity;
        }

        private void lblTanggal_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string receiptDate = Convert.ToDateTime(GetCurrentColumnValue("ReceiptDate")).ToString(Constant.FormatString.DATE_FORMAT);
            string createdDate = Convert.ToDateTime(GetCurrentColumnValue("CreatedDate")).ToString(Constant.FormatString.DATE_FORMAT);

            if (receiptDate != "01-Jan-1900")
            {
                lblTanggal.Text = string.Format("{0}, {1}", City, receiptDate);
            }
            else
            {
                lblTanggal.Text = string.Format("{0}, {1}", City, createdDate);
            }
        }

        private void lblPrintNumber_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            SettingParameterDt Param = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, Constant.SettingParameter.FN_IS_USE_COUNTER_IN_PAYMENT_RECEIPT);
            if (Param.ParameterValue == "1")
            {
                Int32 PrintNumber = Convert.ToInt32((GetCurrentColumnValue("PrintNumber")).ToString());

                if (PrintNumber > 0)
                {

                    lblPrintNumber.Text = string.Format("{0}", GetCurrentColumnValue("PrintNumber").ToString());
                }
                else
                {
                    lblPrintNumber.Text = "";
                }
            }
            else
            {
                lblPrintNumber.Text = "";
            }
        }

        private void lblDiagnosa_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (GetCurrentColumnValue("Diagnose1").ToString() != "" && GetCurrentColumnValue("Diagnose2").ToString() != "")
            {
                lblDiagnosa.Text = string.Format("Diagnosa = {0}, {1}", GetCurrentColumnValue("Diagnose1").ToString(), GetCurrentColumnValue("Diagnose2").ToString());
            }
            else if (GetCurrentColumnValue("Diagnose1").ToString() != "")
            {
                lblDiagnosa.Text = string.Format("Diagnosa = {0}", GetCurrentColumnValue("Diagnose1").ToString());
            }
            else
            {
                lblDiagnosa.Text = "";
            }
        }
    }
}

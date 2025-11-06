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
    public partial class BNewPaymentReceiptRSASIHEn : BaseRpt
    {
        private string city = "";
        private string HealthcareName = "";
        private string Address = "";
        private string City = "";
        public BNewPaymentReceiptRSASIHEn()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID)).FirstOrDefault();
            if (oHealthcare != null)
            {
                xrLogo.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/logo1.png");
            }

            city = oHealthcare.City;
            HealthcareName = oHealthcare.HealthcareName;
            Address = oHealthcare.StreetName;
            City = oHealthcare.City;
            lblAddress.Text = Address;
            lblCity.Text = City;

            List<GetPaymentReceiptCustom> lstEntity = BusinessLayer.GetPaymentReceiptCustomList(Convert.ToInt32(param[0]));
            
            this.DataSource = lstEntity;
        }

        private void lblTanggal_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string receiptDate = Convert.ToDateTime(GetCurrentColumnValue("ReceiptDate")).ToString(Constant.FormatString.DATE_FORMAT);
            string createdDate = Convert.ToDateTime(GetCurrentColumnValue("CreatedDate")).ToString(Constant.FormatString.DATE_FORMAT);

            if (receiptDate != "01-Jan-1900")
            {
                lblTanggal.Text = string.Format("{0}, {1}", city, receiptDate);
            }
            else
            {
                lblTanggal.Text = string.Format("{0}, {1}", city, createdDate);
            }
        }

        private void lblPrintNumber_BeforePrint_1(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            SettingParameterDt Param = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, Constant.SettingParameter.FN_IS_USE_COUNTER_IN_PAYMENT_RECEIPT);
            if (Param.ParameterValue == "1")
            {
                Int32 PrintNumber = Convert.ToInt32((GetCurrentColumnValue("PrintNumber")).ToString());

                if (PrintNumber > 0)
                {

                    lblPrintNumber.Text = string.Format("Copy");
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

         
    }
}

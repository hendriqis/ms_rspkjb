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
    public partial class BNewPaymentReceipt20 : BaseRpt
    {
        private string City = "";

        public BNewPaymentReceipt20()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();
            City = entityHealthcare.City;

            List<GetPaymentReceiptCustom> lstEntity = BusinessLayer.GetPaymentReceiptCustomList(Convert.ToInt32(param[0]));

            #region Page Header
            cHeaderPatient.Text = string.Format("({0}) {1}", lstEntity.FirstOrDefault().MedicalNo, lstEntity.FirstOrDefault().PatientName);
            cHeaderRegTrans.Text = string.Format("{0}", lstEntity.FirstOrDefault().PaymentReceiptNo);
            #endregion
            
            #region Detail Transaction
            List<GetPaymentReceiptCustomDetailTransaction1> lstPayment = BusinessLayer.GetPaymentReceiptCustomDetailTransaction1List(lstEntity.FirstOrDefault().PaymentReceiptID);
            subPaymentReceipt20Dt.CanGrow = true;
            bNewPaymentReceipt20Dt.InitializeReport(lstPayment);
            #endregion

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
                    lblPrintNumber.Text = string.Format("Print number {0}", GetCurrentColumnValue("PrintNumber").ToString());
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

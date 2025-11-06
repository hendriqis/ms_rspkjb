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
    public partial class BNewPaymentReceiptRanapEngRSPW : BaseCustomA6Rpt
    {
        private int lineNumber = 0;
        private int detailID = 0;
        private int oldDetailID = 0;
        private decimal totalAmount = 0;

        public BNewPaymentReceiptRanapEngRSPW()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            lineNumber = 0;

            lblReportTitle.Text = string.Format("PAYMENT RECEIPT");

            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();
            if (entityHealthcare != null)
            {
                lblHealthcare.Text = entityHealthcare.HealthcareName;
                lblAddress.Text = string.Format("{0} {1} {2}", entityHealthcare.StreetName, entityHealthcare.City, entityHealthcare.ZipCode);
                lblHealthcareInfoDetail.Text = string.Format("Telp/Fax : {0}/{1}", entityHealthcare.PhoneNo1, entityHealthcare.FaxNo1);
            }
            PaymentReceipt entityHd = BusinessLayer.GetPaymentReceipt(Convert.ToInt32(param[0]));
            vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", entityHd.RegistrationID)).FirstOrDefault();

            txtCityAndDate.Text = string.Format("{0}, {1}", entityHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));

            if (entityHd.PrintNumber > 1)
            {
                lblDuplikat.Visible = true;
            }

            List<GetPaymentReceiptCustom> lstEntity = BusinessLayer.GetPaymentReceiptCustomList(Convert.ToInt32(param[0]));
            //if (lstEntity.FirstOrDefault().ReceiptAmount < 0)
            //{
            //    PatientPaymentHd entityPaymentHd = BusinessLayer.GetPatientPaymentHdList(string.Format("PaymentReceiptID = {0} AND GCTransactionStatus != '{1}'", lstEntity.FirstOrDefault().PaymentReceiptID, Constant.TransactionStatus.VOID)).FirstOrDefault();
            //    xrLabel3.Text = Function.NumberInWords(Convert.ToInt64(entityPaymentHd.TotalPaymentAmount), true);
            //    txtTotal.Text = string.Format("Terbilang Rp. {0}", entityPaymentHd.TotalPaymentAmount.ToString("N2"));
            //}
            //else
            //{
            //    xrLabel3.Text = Function.NumberInWords(Convert.ToInt64(lstEntity.FirstOrDefault().ReceiptAmount), true);
            //    txtTotal.Text = string.Format("Terbilang Rp. {0}", lstEntity.FirstOrDefault().ReceiptAmount.ToString("N2"));
            //}

            List<PatientBill> lstBill = BusinessLayer.GetPatientBillList(string.Format("RegistrationID = {0} AND GCTransactionStatus != '{1}'", entityHd.RegistrationID, Constant.TransactionStatus.VOID));
            Int64 totalPatientAmount = Convert.ToInt64(lstBill.Sum(s => s.TotalPatientAmount));
            xrLabel3.Text = Function.NumberInWordsInEnglish(Convert.ToInt64(totalPatientAmount), true);
            txtTotal.Text = string.Format("Amount Rp. {0}", totalPatientAmount.ToString("N2"));

            if (!string.IsNullOrEmpty(lstEntity.FirstOrDefault().Remarks))
            {
                xrLabel10.Visible = true;
                xrLabel11.Visible = true;
                xrLabel12.Visible = true;
            }
            else
            {
                xrLabel10.Visible = false;
                xrLabel11.Visible = false;
                xrLabel12.Visible = false;
            }
            this.DataSource = lstEntity;

            #region Transaction
            //List<GetPatientChargesHdDtAllRSPW> lstHDDT = BusinessLayer.GetPatientChargesHdDtAllRSPWList(entityReg.RegistrationID);
            //txtTotal.Text = string.Format("Terbilang Rp. {0}", lstHDDT.Sum(s => s.LineAmount).ToString("N2"));
            //xrLabel3.Text = Function.NumberInWords(Convert.ToInt64(lstHDDT.Sum(s => s.LineAmount)), true); 
            //subTransaction.CanGrow = true;
            //billPaymentSummaryTransactionRSSES.InitializeReport(lstHDDT);
            #endregion

            
            //base.InitializeReport(param);
        }

        private void GroupFooter2_AfterPrint(object sender, EventArgs e)
        {
            lineNumber = 0;
            oldDetailID = detailID;
        }
    }
}

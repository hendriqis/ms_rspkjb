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
    public partial class BNewPaymentReceiptRajalEngRSPW : BaseCustomA6Rpt
    {
        private int lineNumber = 0;
        private int detailID = 0;
        private int oldDetailID = 0;
        private decimal totalAmount = 0;

        public BNewPaymentReceiptRajalEngRSPW()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            lblReportTitle.Text = string.Format("PAYMENT RECEIPT");
            lineNumber = 0;

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

            List<PatientBill> lstBill = BusinessLayer.GetPatientBillList(string.Format("RegistrationID = {0} AND GCTransactionStatus != '{1}'", entityHd.RegistrationID, Constant.TransactionStatus.VOID));
            Int64 totalPatientAmount = Convert.ToInt64(lstBill.Sum(s => s.TotalPatientAmount));
            Int64 totalPayerAmount = Convert.ToInt64(lstBill.Sum(s => s.TotalPayerAmount));
            xrLabel3.Text = Function.NumberInWordsInEnglish(Convert.ToInt64(totalPatientAmount), true);
            txtTotal.Text = string.Format("Amount Rp. {0}", totalPatientAmount.ToString("N2"));

            if (totalPayerAmount == 0)
            {
                #region Transaction
                List<GetPatientChargesHdDtAllRSPW> lstHDDT = BusinessLayer.GetPatientChargesHdDtAllRSPWList(entityReg.RegistrationID).Where(w => w.LineAmount > 0).GroupBy(g=> g.BillingGroupID).Select(s => s.FirstOrDefault()).ToList();
                txtTotal.Text = string.Format("Amount Rp. {0}", lstHDDT.Sum(s => s.LineAmount).ToString("N2"));
                xrLabel3.Text = Function.NumberInWordsInEnglish(Convert.ToInt64(lstHDDT.Sum(s => s.LineAmount)), true);
                subTransaction.CanGrow = true;
                billPaymentSummaryTransactionRSSES.InitializeReport(lstHDDT);
                #endregion
            }
            else
            {
                subTransaction.Visible = false;
            }
            
            //base.InitializeReport(param);
        }

        private void GroupFooter2_AfterPrint(object sender, EventArgs e)
        {
            lineNumber = 0;
            oldDetailID = detailID;
        }
    }
}

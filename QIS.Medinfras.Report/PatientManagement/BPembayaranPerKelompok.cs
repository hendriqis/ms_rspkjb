using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Report
{
    public partial class BPembayaranPerKelompok : BaseDailyPortraitRpt
    {
        Boolean isControlCoverageLimit;
        PatientBill entityBill = null;
        String custType;

        public BPembayaranPerKelompok()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID))[0];

            vPatientChargesDtPayment entity = BusinessLayer.GetvPatientChargesDtPaymentList(string.Format("{0}", param[0]))[0];
            lblLastUpdatedBy.Text = AppSession.UserLogin.UserFullName;
            lblLastUpdatedDate.Text = entityHealthcare.City + ", " + DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT);

            vPatientPaymentHd entityPayment = BusinessLayer.GetvPatientPaymentHdList(string.Format("{0}", param[0]))[0];
            lblBillingNo.Text = entityPayment.PaymentNo;
            lblPaymentType.Text = entityPayment.PaymentType;
            lblPaymentDate.Text = string.Format("{0}", entityPayment.PaymentDateInString);
            lblTotalAmount.Text = entityPayment.TotalPaymentAmount.ToString("N2");

            vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", entityPayment.RegistrationID)).FirstOrDefault();
            custType = entityReg.GCCustomerType;
            lblRegistrationNo.Text = entityReg.RegistrationNo;
            lblPayer.Text = entityReg.BusinessPartner;
            lblPatientName.Text = entityReg.PatientName;
            lblMedicalNo.Text = entityReg.MedicalNo;
            lblDateOfBirth.Text = entityReg.DateOfBirthInString;
            isControlCoverageLimit = entityReg.IsControlCoverageLimit;

            entityBill = BusinessLayer.GetPatientBillList(string.Format("PatientBillingID IN(SELECT PatientBillingID FROM PatientBillPayment WHERE PaymentID={0})", entityPayment.PaymentID))[0];
            
            base.InitializeReport(param);
        }

        private void lblAdmFeeOrDiskonPatientValue_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (custType == Constant.CustomerType.PERSONAL)
                lblAdmFeeOrDiskonPatientValue.Text = entityBill.PatientDiscountAmount.ToString("N2");
        }

        private void lblSvcFeeOrGrandPatientValue_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (custType == Constant.CustomerType.PERSONAL)
                lblSvcFeeOrGrandPatientValue.Text = (entityBill.TotalPatientAmount - entityBill.PatientDiscountAmount).ToString("N2");
        }

        private void lblDiskonOrCoveragePasien_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (!(custType == Constant.CustomerType.PERSONAL))
            {
                if (!isControlCoverageLimit)
                    lblDiskonOrCoveragePasien.Text = entityBill.PatientDiscountAmount.ToString("N2");
            }
        }

        private void lblGrandTotalPatientValue_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (!(custType == Constant.CustomerType.PERSONAL))
            {
                if (isControlCoverageLimit)
                    lblGrandTotalPatientValue.Text = (entityBill.TotalPatientAmount - entityBill.PatientDiscountAmount).ToString("N2");
            }
        }

        private void lblDiskonOrGrandPasienValue_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (!(custType == Constant.CustomerType.PERSONAL))
            {
                if (isControlCoverageLimit)
                    lblDiskonOrGrandPasienValue.Text = entityBill.PatientDiscountAmount.ToString("N2");
                else
                    lblDiskonOrGrandPasienValue.Text = (entityBill.TotalPatientAmount - entityBill.PatientDiscountAmount).ToString("N2");
            }
        }

        private void lblDiskonOrCoveragePayer_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (!(custType == Constant.CustomerType.PERSONAL))
            {
                if (isControlCoverageLimit)
                    lblDiskonOrCoveragePayer.Text = entityBill.CoverageAmount.ToString("N2");
                else
                    lblDiskonOrCoveragePayer.Text = entityBill.PayerDiscountAmount.ToString("N2");
            }
        }

        private void lblSvcFeeOrGrandPayerValue_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (custType == Constant.CustomerType.PERSONAL)
                lblSvcFeeOrGrandPayerValue.Text = (entityBill.TotalPayerAmount - entityBill.PayerDiscountAmount).ToString("N2");
            else
                lblSvcFeeOrGrandPayerValue.Text = entityBill.ServiceFeeAmount.ToString("N2");
        }

        private void lblAdmFeeOrDiskonPayerValue_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (custType == Constant.CustomerType.PERSONAL)
                lblAdmFeeOrDiskonPayerValue.Text = entityBill.PayerDiscountAmount.ToString("N2");
            else
                lblAdmFeeOrDiskonPayerValue.Text = entityBill.AdministrationFeeAmount.ToString("N2");
        }

        private void lblGrandTotalPayerValue_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (!(custType == Constant.CustomerType.PERSONAL))
            {
                if (isControlCoverageLimit)
                    lblGrandTotalPayerValue.Text = (entityBill.TotalPayerAmount - entityBill.PayerDiscountAmount).ToString("N2");
            }
        }

        private void lblDiskonOrGrandPayerValue_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (!(custType == Constant.CustomerType.PERSONAL))
            {
                if (isControlCoverageLimit)
                    lblDiskonOrGrandPayerValue.Text = entityBill.PayerDiscountAmount.ToString("N2");
                else
                    lblDiskonOrGrandPayerValue.Text = (entityBill.TotalPayerAmount - entityBill.PayerDiscountAmount).ToString("N2");
            }
        }

        private void lblDiskonOrAdmFee_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (custType == Constant.CustomerType.PERSONAL)
            {
                lblDiskonOrAdmFee.Text = "Diskon";
                lblGrandOrSvcFee.Text = "Total Tagihan";
            }
            else
            {
                lblDiskonOrAdmFee.Text = "Administration Fee";
                lblGrandOrSvcFee.Text = "Service Fee";
                
                if (isControlCoverageLimit)
                {
                    lblDiskonOrGrand.Text = "Diskon";
                    lblGrandTotal.Text = "Total Tagihan";
                    lblDiskonOrCoverage.Text = "Batas Tanggungan";
                    lblTitikDua1.Text = lblTitikDua2.Text = lblTitikDua3.Text = ":";
                }
                else
                {
                    lblDiskonOrCoverage.Text = "Diskon";
                    lblDiskonOrGrand.Text = "Total Tagihan";
                    lblTitikDua1.Text = lblTitikDua2.Text = ":";
                }
            }
        }

        private void lblGrandOrSvcFee_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }
    }
}

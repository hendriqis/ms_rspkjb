using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Linq;

namespace QIS.Medinfras.Report
{
    public partial class BPiutangInstansiDetail : BaseDailyPortraitRpt
    {
        Boolean isControlCoverageLimit;
        List<PatientBill> lstEntityBill = null;
        String custType;

        public BPiutangInstansiDetail()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {

            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = {0}", AppSession.UserLogin.HealthcareID))[0];

            vPatientChargesDtPayment entity = BusinessLayer.GetvPatientChargesDtPaymentList(string.Format("{0}", param[0]))[0];
            lblLastUpdatedBy.Text = AppSession.UserLogin.UserFullName;
            lblLastUpdatedDate.Text = string.Format("{0}, {1}", entityHealthcare.City, entity.LastUpdatedDate.ToString(Constant.FormatString.DATE_FORMAT));
            
            vPatientPaymentHd entityPayment = BusinessLayer.GetvPatientPaymentHdList(string.Format("{0}", param[0]))[0];
            lblBillingNo.Text = entityPayment.PaymentNo;
            lblPaymentDate.Text = string.Format("{0}", entityPayment.PaymentDateInString);

            lstEntityBill = BusinessLayer.GetPatientBillList(string.Format("PatientBillingID IN (SELECT PatientBillingID FROM PatientBillPayment WHERE PaymentID = {0})", entityPayment.PaymentID));
            lblTotalAmount.Text = lstEntityBill.Sum(p => p.TotalPayerAmount).ToString("N2");

            vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", entityPayment.RegistrationID))[0];
            custType = entityReg.GCCustomerType;
            lblRegistrationNo.Text = string.Format("{0} / {1}", entityReg.RegistrationNo, entityPayment.PaymentNo);
            lblPayer.Text = entityReg.BusinessPartner;
            lblPatientName.Text = entityReg.PatientName;
            lblMedicalNo.Text = entityReg.MedicalNo;
            lblDateOfBirth.Text = entityReg.DateOfBirthInString;
            isControlCoverageLimit = entityReg.IsControlCoverageLimit;

            base.InitializeReport(param);
        }

        private void lblSvcFeeOrGrandPatientValue_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (custType == Constant.CustomerType.PERSONAL)
                lblSvcFeeOrGrandPatientValue.Text = (lstEntityBill.Sum(p => p.TotalPatientAmount) - lstEntityBill.Sum(p => p.PatientDiscountAmount)).ToString("N2");
            else
                lblSvcFeeOrGrandPayerValue.Text = (lstEntityBill.Sum(p => p.TotalPayerAmount) - lstEntityBill.Sum(p => p.PayerDiscountAmount)).ToString("N2");
        }

        private void lblSvcFeeOrGrandPayerValue_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (custType == Constant.CustomerType.PERSONAL)
                lblSvcFeeOrGrandPatientValue.Text = (lstEntityBill.Sum(p => p.TotalPayerAmount) - lstEntityBill.Sum(p => p.PayerDiscountAmount)).ToString("N2");
            else
                lblSvcFeeOrGrandPayerValue.Text = lstEntityBill.Sum(p => p.ServiceFeeAmount).ToString("N2");
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

        private void lblAdmFeeOrDiskonPatientValue_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (custType == Constant.CustomerType.PERSONAL)
                lblAdmFeeOrDiskonPatientValue.Text = lstEntityBill.Sum(p => p.PatientDiscountAmount).ToString("N2");
            else
                lblAdmFeeOrDiskonPayerValue.Text = lstEntityBill.Sum(p => p.PayerDiscountAmount).ToString("N2");
        }

        private void lblDiskonOrCoveragePasien_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (!(custType == Constant.CustomerType.PERSONAL))
            {
                if (!isControlCoverageLimit)
                    lblDiskonOrCoveragePasien.Text = lstEntityBill.Sum(p => p.PatientDiscountAmount).ToString("N2");
            }
        }

        private void lblDiskonOrCoveragePayer_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (!(custType == Constant.CustomerType.PERSONAL))
            {
                if (isControlCoverageLimit)
                    lblDiskonOrCoveragePayer.Text = lstEntityBill.Sum(p => p.CoverageAmount).ToString("N2");
                else
                    lblDiskonOrCoveragePayer.Text = lstEntityBill.Sum(p => p.PayerDiscountAmount).ToString("N2");
            }
        }

        private void lblDiskonOrGrandPayerValue_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (!(custType == Constant.CustomerType.PERSONAL))
            {
                if (isControlCoverageLimit)
                    lblDiskonOrGrandPayerValue.Text = lstEntityBill.Sum(p => p.PayerDiscountAmount).ToString("N2");
                else
                    lblDiskonOrGrandPayerValue.Text = (lstEntityBill.Sum(p => p.TotalPayerAmount) - lstEntityBill.Sum(p => p.PayerDiscountAmount)).ToString("N2");
            }
        }

        private void lblDiskonOrGrandPasienValue_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (!(custType == Constant.CustomerType.PERSONAL))
            {
                if (isControlCoverageLimit)
                    lblDiskonOrGrandPasienValue.Text = lstEntityBill.Sum(p => p.PatientDiscountAmount).ToString("N2");
                else
                    lblDiskonOrGrandPasienValue.Text = (lstEntityBill.Sum(p => p.TotalPatientAmount) - lstEntityBill.Sum(p => p.PatientDiscountAmount)).ToString("N2");
            }
        }

        private void lblGrandTotalPayerValue_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (!(custType == Constant.CustomerType.PERSONAL))
            {
                if (isControlCoverageLimit)
                    lblGrandTotalPayerValue.Text = (lstEntityBill.Sum(p => p.TotalPayerAmount) - lstEntityBill.Sum(p => p.PayerDiscountAmount)).ToString("N2");
            }
        }

        private void lblAdmFeeOrDiskonPayerValue_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (custType == Constant.CustomerType.PERSONAL)
                lblAdmFeeOrDiskonPatientValue.Text = lstEntityBill.Sum(p => p.PayerDiscountAmount).ToString("N2");
            else
                lblAdmFeeOrDiskonPayerValue.Text = lstEntityBill.Sum(p => p.AdministrationFeeAmount).ToString("N2");
        }

        private void lblGrandTotalPatientValue_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (!(custType == Constant.CustomerType.PERSONAL))
            {
                if (isControlCoverageLimit)
                    lblGrandTotalPatientValue.Text = (lstEntityBill.Sum(p => p.TotalPatientAmount) - lstEntityBill.Sum(p => p.PatientDiscountAmount)).ToString("N2");
            }
        }
    }
}

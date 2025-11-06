using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BRincianTagihan : BaseDailyPortraitRpt
    {
        Boolean isControlCoverageLimit;
        PatientBill entityBill = null;
        String custType;

        public BRincianTagihan()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vPatientChargesDt entityCharges = BusinessLayer.GetvPatientChargesDtList(param[0])[0];
            
            entityBill = BusinessLayer.GetPatientBillList(param[0])[0];

            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", entityCharges.RegistrationID))[0];
            lblAge.Text = entity.PatientAge;
            custType = entity.GCCustomerType;
            lblBusinessPartner.Text = entity.BusinessPartnerName;
            lblDoctor.Text = entity.ParamedicName;
            lblNoReg.Text = string.Format("{0} / {1}", entity.RegistrationNo, entityBill.PatientBillingNo);
            lblPatient.Text = string.Format("{0} / {1}", entity.PatientName, entity.MedicalNo);
            lblRegDate.Text = entity.RegistrationDateInString;
            lblTglGender.Text = string.Format("{0} / {1}", entity.DateOfBirthInString, entity.Gender);
            isControlCoverageLimit = entity.IsControlCoverageLimit;

            base.InitializeReport(param);
        }

        private void xrLabel26_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            vHealthcare entity = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = {0}", appSession.HealthcareID))[0];
            ((XRLabel)sender).Text = entity.City+", "+DateTime.Now.ToString("dd-MMM-yyyy");
        }

        private void lblSvcFeeOrGrandPatientValue_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (custType == Constant.CustomerType.PERSONAL)
                lblSvcFeeOrGrandPatientValue.Text = (entityBill.TotalPatientAmount - entityBill.PatientDiscountAmount).ToString("N");
        }

        private void lblSvcFeeOrGrandPayerValue_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (custType == Constant.CustomerType.PERSONAL)
                lblSvcFeeOrGrandPayerValue.Text = (entityBill.TotalPayerAmount - entityBill.PayerDiscountAmount).ToString("N");
            else
                lblSvcFeeOrGrandPayerValue.Text = entityBill.ServiceFeeAmount.ToString("N");
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

        private void lblDiskonOrCoveragePasien_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (!(custType == Constant.CustomerType.PERSONAL))
            {
                if (!isControlCoverageLimit)
                    lblDiskonOrCoveragePasien.Text = entityBill.PatientDiscountAmount.ToString("N");
            }
        }

        private void lblDiskonOrCoveragePayer_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (!(custType == Constant.CustomerType.PERSONAL))
            {
                if (isControlCoverageLimit)
                    lblDiskonOrCoveragePayer.Text = entityBill.CoverageAmount.ToString("N");
                else
                    lblDiskonOrCoveragePayer.Text = entityBill.PayerDiscountAmount.ToString("N");
            }
        }

        private void lblDiskonOrGrandPayerValue_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (!(custType == Constant.CustomerType.PERSONAL))
            {
                if (isControlCoverageLimit)
                    lblDiskonOrGrandPayerValue.Text = entityBill.PayerDiscountAmount.ToString("N");
                else
                    lblDiskonOrGrandPayerValue.Text = (entityBill.TotalPayerAmount - entityBill.PayerDiscountAmount).ToString("N");
            }
        }

        private void lblDiskonOrGrandPasienValue_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (!(custType == Constant.CustomerType.PERSONAL))
            {
                if (isControlCoverageLimit)
                    lblDiskonOrGrandPasienValue.Text = entityBill.PatientDiscountAmount.ToString("N");
                else
                    lblDiskonOrGrandPasienValue.Text = (entityBill.TotalPatientAmount - entityBill.PatientDiscountAmount).ToString("N");
            }
        }

        private void lblGrandTotalPayerValue_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (!(custType == Constant.CustomerType.PERSONAL))
            {
                if (isControlCoverageLimit)
                    lblGrandTotalPayerValue.Text = (entityBill.TotalPayerAmount - entityBill.PayerDiscountAmount).ToString("N");
            }
        }

        private void lblAdmFeeOrDiskonPayerValue_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (custType == Constant.CustomerType.PERSONAL)
                lblAdmFeeOrDiskonPayerValue.Text = entityBill.PayerDiscountAmount.ToString("N");
            else
                lblAdmFeeOrDiskonPayerValue.Text = entityBill.AdministrationFeeAmount.ToString("N");

        }

        private void lblGrandTotalPatientValue_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (!(custType == Constant.CustomerType.PERSONAL))
            {
                if (isControlCoverageLimit)
                    lblGrandTotalPatientValue.Text = (entityBill.TotalPatientAmount - entityBill.PatientDiscountAmount).ToString("N");
            }
        }

        private void lblAdmFeeOrDiskonPatientValue_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (custType == Constant.CustomerType.PERSONAL)
                lblAdmFeeOrDiskonPatientValue.Text = entityBill.PatientDiscountAmount.ToString("N");
        }
    }
}

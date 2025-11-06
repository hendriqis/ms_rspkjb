using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using System.Linq;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BPatientBillDetail_RSSMP : BaseDailyPortraitRpt
    {
        List<PatientBill> lstPatientBill = null;
        vRegistration entity = null;
        string customer = "";

        public BPatientBillDetail_RSSMP()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vPatientChargesDt en = BusinessLayer.GetvPatientChargesDtList(param[0])[0];
            List<vRegistration> lstRegistration = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID IN (SELECT RegistrationID FROM vPatientChargesDt WHERE {0})", param[0]));
            
            int countIP = 0;
            foreach (vRegistration regTemp in lstRegistration)
            {
                if (regTemp.DepartmentID == Constant.Facility.INPATIENT)
                {
                    countIP += 1;
                }
            }
            if (countIP == 0)
            {
                entity = lstRegistration.FirstOrDefault();
            }
            else
            {
                entity = lstRegistration.FirstOrDefault(p => p.DepartmentID == Constant.Facility.INPATIENT);
            }

            RegistrationBPJS entityRegBPJS = BusinessLayer.GetRegistrationBPJSList(string.Format("RegistrationID = {0}", entity.RegistrationID)).FirstOrDefault();
            lstPatientBill = BusinessLayer.GetPatientBillList(string.Format("RegistrationID = {0} AND GCTransactionStatus <> '{1}'", en.RegistrationID, Constant.TransactionStatus.VOID));
            if (entity == null)
            {
                entity = lstRegistration.FirstOrDefault();
            }

            lblAge.Text = entity.PatientAge;
            lblBusinessPartner.Text = entity.BusinessPartnerName;
            lblDoctor.Text = entity.ParamedicName;
            lblNoReg.Text = entity.RegistrationNo;
            if (entityRegBPJS != null)
            {
                lblSEP.Text = "No. RM / No. SEP";
                lblSEPNo.Text = string.Format("{0} / {1}", entity.MedicalNo, entityRegBPJS.NoSEP);
            }
            else
            {
                lblSEP.Text = "No. RM";
                lblSEPNo.Text = string.Format("{0}", entity.MedicalNo);
            }
            lblPatient.Text = string.Format("{0}", entity.PatientName);
            lblRegDate.Text = entity.RegistrationDateInString;
            lblTglGender.Text = string.Format("{0} / {1}", entity.DateOfBirthInString, entity.Gender);

            if (entity.BusinessPartnerID != 0 && entity.BusinessPartnerID != null)
            {
                customer = BusinessLayer.GetCustomer(entity.BusinessPartnerID).GCCustomerType;
            }

            base.InitializeReport(param);
        }

        private void xrLabel26_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            vHealthcare entity = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = {0}", appSession.HealthcareID))[0];
            ((XRLabel)sender).Text = entity.City + ", " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:dd");
        }

        private void lblDiskonPasien_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lblDiskonPasien.Text = lstPatientBill.Sum(p => p.PatientDiscountAmount).ToString("N");
        }

        private void lblDiskonPayer_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lblDiskonPayer.Text = lstPatientBill.Sum(p => p.PayerDiscountAmount).ToString("N");
        }

        private void lblGrandTotalPasien_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            decimal totalPasien = ((Decimal)lblTotPasien.Summary.GetResult() + lstPatientBill.Sum(p => p.PatientAdminFeeAmount) + lstPatientBill.Sum(p => p.PatientServiceFeeAmount) - lstPatientBill.Sum(p => p.PatientDiscountAmount));
            decimal totalPayer = ((Decimal)lblTotPayer.Summary.GetResult() + lstPatientBill.Sum(p => p.AdministrationFeeAmount) + lstPatientBill.Sum(p => p.ServiceFeeAmount) - lstPatientBill.Sum(p => p.PayerDiscountAmount));
            decimal coverageLimit = Convert.ToDecimal(entity.CoverageLimitAmount);

            string paramBPJS = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, Constant.SettingParameter.FN_TIPE_CUSTOMER_BPJS).ParameterValue;
            if (customer != "" && customer != null)
            {
                if (customer == paramBPJS)
                {
                    totalPasien = lstPatientBill.Sum(a => a.TotalPatientAmount - a.PatientDiscountAmount);
                }
                else
                {
                    if (coverageLimit < totalPayer)
                    {
                        totalPasien += totalPayer - coverageLimit;
                    }
                }
            }
            else
            {
                if (coverageLimit < totalPayer)
                {
                    totalPasien += totalPayer - coverageLimit;
                }
            }
            ((XRLabel)sender).Text = totalPasien.ToString("N");
        }

        private void lblGrandTotalPayer_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            decimal totalPayer = ((Decimal)lblTotPayer.Summary.GetResult() + lstPatientBill.Sum(p => p.AdministrationFeeAmount) + lstPatientBill.Sum(p => p.ServiceFeeAmount) - lstPatientBill.Sum(p => p.PayerDiscountAmount));
            decimal coverageLimit = Convert.ToDecimal(entity.CoverageLimitAmount);
            if (coverageLimit < totalPayer)
            {
                totalPayer = coverageLimit;
            }
            ((XRLabel)sender).Text = totalPayer.ToString("N");
        }

        private void lblCoverageLimit_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //List<PatientBill> lstPatientBill = BusinessLayer.GetPatientBillList(string.Format("RegistrationID = {0}", registrationID));
            //lblCoverageLimit.Text = lstPatientBill.Sum(p => p.CoverageAmount).ToString("N");
            //Registration regis = BusinessLayer.GetRegistration(Convert.ToInt32(registrationID));
            lblCoverageLimit.Text = entity.CoverageLimitAmount.ToString("N");
        }

        private void lblPatientAdministration_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            ((XRLabel)sender).Text = lstPatientBill.Sum(p => p.PatientAdminFeeAmount).ToString("N");
        }

        private void lblPayerAdministration_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            ((XRLabel)sender).Text = lstPatientBill.Sum(p => p.AdministrationFeeAmount).ToString("N");
        }

        private void lblPatientService_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            ((XRLabel)sender).Text = lstPatientBill.Sum(p => p.PatientServiceFeeAmount).ToString("N");
        }

        private void lblPayerService_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            ((XRLabel)sender).Text = lstPatientBill.Sum(p => p.ServiceFeeAmount).ToString("N");
        }


    }
}

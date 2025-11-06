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
    public partial class BKwitansiInvoiceRekap : BaseDailyPortraitRpt
    {
        String registrationID;
        List<PatientBill> lstPatientBill;
        vRegistration entity;
        string customer = "";

        public BKwitansiInvoiceRekap()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vPatientChargesDt en = BusinessLayer.GetvPatientChargesDtList(param[0]).LastOrDefault();
            List<vRegistration> lstRegistration = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID IN (SELECT RegistrationID FROM vPatientChargesDt WHERE {0})", param[0]));
            entity = lstRegistration.FirstOrDefault(p => p.DepartmentID == Constant.Facility.INPATIENT);
            if (entity == null)
                entity = lstRegistration.FirstOrDefault();
            registrationID = en.RegistrationID.ToString();

            lblBusinessPartner.Text = entity.BusinessPartnerName;
            lblDoctor.Text = entity.ParamedicName;
            lblNoReg.Text = entity.RegistrationNo;
            lblPatient.Text = string.Format("{0} / {1}", entity.PatientName, entity.MedicalNo);
            lblRegDate.Text = entity.RegistrationDateInString;
            lblTglGender.Text = string.Format("{0} / {1}", entity.DateOfBirthInString, entity.Gender);
            lstPatientBill = BusinessLayer.GetPatientBillList(string.Format("RegistrationID = {0} AND GCTransactionStatus != '{1}'", registrationID, Constant.TransactionStatus.VOID));
            lblAge.Text = entity.PatientAge;
            base.InitializeReport(param);
        }

        private void xrLabel26_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            vHealthcare entityHC = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = {0}", appSession.HealthcareID))[0];
            ((XRLabel)sender).Text = entityHC.City + ", " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:dd");
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
            //if (lblTotPasien.Summary.GetResult() != null)
            //    lblGrandTotalPasien.Text = ((Decimal)lblTotPasien.Summary.GetResult() - lstPatientBill.Sum(p => p.PatientDiscountAmount)).ToString("N");
            //else
            //    lblGrandTotalPasien.Text = "0";
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
            //if (lblTotPayer.Summary.GetResult() != null)
            //    lblGrandTotalPayer.Text = ((Decimal)lblTotPayer.Summary.GetResult() - lstPatientBill.Sum(p => p.PayerDiscountAmount)).ToString("N");
            //else
            //    lblGrandTotalPayer.Text = "0";
        }

        private void lblCoverageLimit_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lblCoverageLimit.Text = entity.CoverageLimitAmount.ToString("N");
        }

        private void lblIsPrintDoctorname_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (IsPrintWithDoctorName.Checked == false)
            {
                lblIsPrintDoctorName.Visible = false;
            }
            else
            {
                lblIsPrintDoctorName.Visible = true; 
            }
        }

        private void lblIsPrintPatientAmount_BeforePrint_1(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (IsPrintWithDoctorName.Checked == false)
            {
                lblIsPrintPatientAmount.Visible = false;
            }
            else
            {
                lblIsPrintPatientAmount.Visible = true;
            }
        }

        private void lblIsPrintPayerAmount_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (IsPrintWithDoctorName.Checked == false)
            {
                lblIsPrintPayerAmount.Visible = false;
            }
            else
            {
                lblIsPrintPayerAmount.Visible = true;
            }
        }

        private void lblPatientAdministration_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRLabel label = (XRLabel)sender;
            decimal patientAdministration = (from b in lstPatientBill
                                             select b.PatientAdminFeeAmount).Sum();
            label.Text = patientAdministration.ToString("N");
        }

        private void lblPayerAdministration_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRLabel label = (XRLabel)sender;
            decimal payerAdministration = (from b in lstPatientBill
                                           select b.AdministrationFeeAmount).Sum();
            label.Text = payerAdministration.ToString("N");
        }

        private void lblPatientService_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRLabel label = (XRLabel)sender;
            decimal payerAdministration = (from b in lstPatientBill
                                           select b.PatientServiceFeeAmount).Sum();
            label.Text = payerAdministration.ToString("N");
        }

        private void lblPayerService_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRLabel label = (XRLabel)sender;
            decimal payerAdministration = (from b in lstPatientBill
                                           select b.ServiceFeeAmount).Sum();
            label.Text = payerAdministration.ToString("N");
        }
    }
}

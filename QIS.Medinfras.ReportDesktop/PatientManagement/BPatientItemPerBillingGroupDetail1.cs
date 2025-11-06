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
    public partial class BPatientItemPerBillingGroupDetail : BaseDailyPortraitRpt
    {
        String registrationID, totalPasien, totalPayer;
        List<PatientBill> lstPatientBill;
        vRegistration entity;
        public BPatientItemPerBillingGroupDetail()
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
            lblRoom.Text = entity.ServiceUnitName;
            lblClass.Text = entity.ClassName + " / "+ entity.BedCode;
            lblDate.Text = entity.StartServiceDate.ToString(Constant.FormatString.DATE_FORMAT) + " - " + entity.CustomDischargeDate;
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
            vHealthcare entity = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = {0}", appSession.HealthcareID))[0];

            ((XRLabel)sender).Text = entity.City + ", " + DateTime.Now.ToString("dd-MMM-yyyy");
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
            if (lblTotPasien.Summary.GetResult() != null)
            {
                decimal grandTotalPatient = (Decimal)lblTotPasien.Summary.GetResult();
                grandTotalPatient -= lstPatientBill.Sum(p => p.PatientDiscountAmount);

                decimal grandTotalPayer = (Decimal)lblTotPayer.Summary.GetResult();
                grandTotalPayer -= lstPatientBill.Sum(p => p.PayerDiscountAmount);
                decimal minus = 0;
                if (entity.CoverageLimitAmount < grandTotalPayer) minus = grandTotalPayer - entity.CoverageLimitAmount;
                grandTotalPatient += minus;
                lblGrandTotalPasien.Text = grandTotalPatient.ToString("N");
                //lblGrandTotalPasien.Text = ((Decimal)lblTotPasien.Summary.GetResult() - lstPatientBill.Sum(p => p.PatientDiscountAmount)).ToString("N");
            }
            else
                lblGrandTotalPasien.Text = "0";
        }

        private void lblGrandTotalPayer_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (lblTotPayer.Summary.GetResult() != null)
            {
                decimal grandTotalPayer = (Decimal)lblTotPayer.Summary.GetResult();
                grandTotalPayer -= lstPatientBill.Sum(p => p.PayerDiscountAmount);
                //grandTotalPayer -= entity.CoverageLimitAmount;
                //lblGrandTotalPayer.Text = ((Decimal)lblTotPayer.Summary.GetResult() - lstPatientBill.Sum(p => p.PayerDiscountAmount)).ToString("N");
                if (entity.CoverageLimitAmount > grandTotalPayer) lblGrandTotalPayer.Text = grandTotalPayer.ToString("N");
                else lblGrandTotalPayer.Text = entity.CoverageLimitAmount.ToString("N");
            }
            else
                lblGrandTotalPayer.Text = "0";
        }

        private void lblCoverageLimit_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //lblCoverageLimit.Text = lstPatientBill.Sum(p => p.CoverageAmount).ToString("N");
            lblCoverageLimit.Text = entity.CoverageLimitAmount.ToString("N");
        }

        private void lblIsPrintDoctorname_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //if (IsPrintWithDoctorName.Checked == false)
            //{
            //    lblIsPrintDoctorName.Visible = false;
            //}
            //else
            //{
            //    lblIsPrintDoctorName.Visible = true; 
            //}
        }

        private void lblSubTotalPatient_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //vPatientChargesDtPerBillingGroup current = (GetCurrentRow() as vPatientChargesDtPerBillingGroup);
            //if (string.IsNullOrEmpty(current.GCPrintOption) && !string.IsNullOrEmpty(current.BillingGroupName1))
            //{
            //    lblSubTotalPatient.Visible = true;
            //    lblSubTotalPayer.Visible = true;
            //    Detail.Visible = false;
            //}
            //else
            //{
            //    Detail.Visible = true;
            //    lblSubTotalPatient.Visible = false;
            //    lblSubTotalPayer.Visible = false;
            //    e.Cancel = true;
            //}
        }

        int counter = 1;
        private void xrLine4_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (counter == 1) e.Cancel = true;
            counter++;
        }

        private void lblBillingGroupName_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRLabel label = (XRLabel)sender;
            vPatientChargesDtPerBillingGroup current = (GetCurrentRow() as vPatientChargesDtPerBillingGroup);
            if (string.IsNullOrEmpty(current.BillingGroupName1)) label.Text = "Lain-lain";
        }
    }
}

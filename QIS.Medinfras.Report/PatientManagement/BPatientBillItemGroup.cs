using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using System.Web.UI.HtmlControls;
using System.Linq;

namespace QIS.Medinfras.Report
{
    public partial class BPatientBillItemGroup : BaseDailyPortraitRpt
    {
        String registrationID, totalPasien, totalPayer;
        List<PatientBill> lstPatientBill;
        public BPatientBillItemGroup()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vPatientChargesDt en = BusinessLayer.GetvPatientChargesDtList(param[0]).LastOrDefault();
            //lblDiskonPasien.Text = lstEn.
            List<vRegistration> lstRegistration = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID IN (SELECT RegistrationID FROM vPatientChargesDt WHERE {0})", param[0]));
            vRegistration entity = lstRegistration.FirstOrDefault(p => p.DepartmentID == Constant.Facility.INPATIENT);
            if (entity == null)
                entity = lstRegistration.FirstOrDefault();
            registrationID = en.RegistrationID.ToString();

            lblAge.Text = entity.PatientAge;
            lblBusinessPartner.Text = entity.BusinessPartnerName;
            lblDoctor.Text = entity.ParamedicName;
            lblNoReg.Text = entity.RegistrationNo;
            lblPatient.Text = string.Format("{0} / {1}", entity.PatientName, entity.MedicalNo);
            lblRegDate.Text = entity.RegistrationDateInString;
            lblTglGender.Text = string.Format("{0} / {1}", entity.DateOfBirthInString, entity.Gender);
            lstPatientBill = BusinessLayer.GetPatientBillList(string.Format("RegistrationID = {0} AND GCTransactionStatus != '{1}'", registrationID, Constant.TransactionStatus.VOID));
            base.InitializeReport(param);
        }

        private void xrLabel26_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            vHealthcare entity = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = {0}", AppSession.UserLogin.HealthcareID))[0];

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
                lblGrandTotalPasien.Text = ((Decimal)lblTotPasien.Summary.GetResult() - lstPatientBill.Sum(p => p.PatientDiscountAmount)).ToString("N");
            else
                lblGrandTotalPasien.Text = "0";
        }

        private void lblGrandTotalPayer_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (lblTotPayer.Summary.GetResult() != null)
                lblGrandTotalPayer.Text = ((Decimal)lblTotPayer.Summary.GetResult() - lstPatientBill.Sum(p => p.PayerDiscountAmount)).ToString("N");
            else
                lblGrandTotalPayer.Text = "0";
        }

        private void lblCoverageLimit_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lblCoverageLimit.Text = lstPatientBill.Sum(p => p.CoverageAmount).ToString("N");
        }
    }
}

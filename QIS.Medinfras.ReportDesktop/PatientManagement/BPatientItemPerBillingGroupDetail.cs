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
    public partial class BPatientItemPerBillingGroupDetail : BaseCustomDailyPotraitRpt
    {
        private string registrationID;
        private decimal coverageLimitAmount;
        vRegistration entity;

        List<PatientBill> lstPatientBill;

        public BPatientItemPerBillingGroupDetail()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
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

            RegistrationBPJS entityRegBPJS = BusinessLayer.GetRegistrationBPJSList(String.Format("RegistrationID = {0}", entity.RegistrationID)).FirstOrDefault();

            if (entity == null)
            {
                entity = lstRegistration.FirstOrDefault();
            }

            registrationID = entity.RegistrationID.ToString();

            cRegistrationNo.Text = entity.RegistrationNo;
            cRegistrationDate.Text = entity.RegistrationDateInString;
            if (entityRegBPJS != null)
            {
                cSEP.Text = "No. RM / No. SEP";
                cSEPNo.Text = string.Format("{0} / {1}", entity.MedicalNo, entityRegBPJS.NoSEP);
            }
            else
            {
                cSEP.Text = "No. RM";
                cSEPNo.Text = string.Format("{0}", entity.MedicalNo);
            }
            cPatient.Text = string.Format("{0}", entity.PatientName);
            cDOBGender.Text = string.Format("{0} / {1}", entity.DateOfBirthInString, entity.Gender);
            cAgePatient.Text = string.Format("{0}tahun {1}bulan {2}hari", entity.AgeInYear, entity.AgeInMonth, entity.AgeInDay);
            cParamedicRegName.Text = entity.ParamedicName;
            cPayerReg.Text = entity.BusinessPartnerName;
            cServiceUnit.Text = entity.ServiceUnitName;
            cClassName.Text = entity.ClassName + " / " + entity.BedCode;

            String ServiceDate = entity.StartServiceDate.ToString(Constant.FormatString.DATE_FORMAT);
            if (ServiceDate != null && ServiceDate != "01-Jan-1900")
            {
                cServiceDate.Text = entity.StartServiceDate.ToString(Constant.FormatString.DATE_FORMAT) + " - " + entity.CustomDischargeDate;
            }
            else
            {
                cServiceDate.Text = entity.RegistrationDateInString;
            }

            coverageLimitAmount = entity.CoverageLimitAmount;

            lstPatientBill = BusinessLayer.GetPatientBillList(string.Format(
                "RegistrationID = {0} AND GCTransactionStatus != '{1}'", registrationID, Constant.TransactionStatus.VOID));
            
            List<vPatientChargesDtPerBillingGroup> lstEntity = BusinessLayer.GetvPatientChargesDtPerBillingGroupList(param[0]);

            base.InitializeReport(param);
            this.DataSource = lstEntity;
        }

        protected override bool IsSkipBinding()
        {
            return true;
        }

        private void lblSignRemarks_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            vHealthcare entity = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();

            ((XRLabel)sender).Text = entity.City + ", " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:dd");
        }

        private void lblTeamDt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (GetCurrentColumnValue("TeamDtParamedicName") != null && GetCurrentColumnValue("TeamDtParamedicName").ToString() != "")
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void lblDiskonPasien_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lblDiskonPasien.Text = lstPatientBill.Sum(a => a.PatientDiscountAmount).ToString("N2");
        }

        private void lblDiskonInstansi_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lblDiskonInstansi.Text = lstPatientBill.Sum(b => b.PayerDiscountAmount).ToString("N2");
        }

        private void lblGTPasien_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            decimal gt = lstPatientBill.Sum(a => a.TotalPatientAmount) - lstPatientBill.Sum(a => a.PatientDiscountAmount);
            lblGTPasien.Text = gt.ToString("N2");
        }

        private void lblGTInstansi_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            decimal gt = lstPatientBill.Sum(b => b.TotalPayerAmount) - lstPatientBill.Sum(b => b.PayerDiscountAmount);
            lblGTInstansi.Text = gt.ToString("N2");
        }

        private void lblCoverageLimit_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lblCoverageLimit.Text = coverageLimitAmount.ToString("N2");
        }
    }
}

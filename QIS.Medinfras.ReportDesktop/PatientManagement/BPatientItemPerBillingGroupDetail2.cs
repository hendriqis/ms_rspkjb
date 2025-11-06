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
    public partial class BPatientItemPerBillingGroupDetail2 : BaseCustomDailyPotraitRpt
    {
        private string registrationID;
        private decimal coverageLimitAmount;
        vRegistrationBillingReport entity;

        List<PatientBill> lstPatientBill;

        public BPatientItemPerBillingGroupDetail2()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string filter1 = string.Format("((LinkedToRegistrationID = {0} AND IsChargesTransfered = 1) OR RegistrationID = {0})", param[0]);
            string filterexp = string.Format("RegistrationID IN (SELECT RegistrationID FROM vPatientChargesDtBillingReport WHERE {0} AND IsDeleted = 0 AND GCTransactionStatus != '{1}')", filter1, Constant.TransactionStatus.VOID);
            List<vRegistrationBillingReport> lstRegistration = BusinessLayer.GetvRegistrationBillingReportList(filterexp);
            int countIP = 0;
            foreach (vRegistrationBillingReport regTemp in lstRegistration)
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
            vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", param[0])).FirstOrDefault();

            if (!String.IsNullOrEmpty(entityReg.LinkedRegistrationID.ToString()) && entityReg.LinkedRegistrationID != 0)
            {
                registrationID = entityReg.RegistrationID.ToString();

                cRegistrationNo.Text = entityReg.RegistrationNo;
                if (entityRegBPJS != null)
                {
                    cSEP.Text = "No. RM / No. SEP";
                    cSEPNo.Text = string.Format("{0} / {1}", entityReg.MedicalNo, entityRegBPJS.NoSEP);
                }
                else
                {
                    cSEP.Text = "No. RM";
                    cSEPNo.Text = string.Format("{0}", entityReg.MedicalNo);
                }
                cRegistrationDate.Text = entityReg.RegistrationDateInString;
                cDischargeDate.Text = entityReg.cfDischargeDateInString;
                cPatient.Text = string.Format("{0}", entityReg.PatientName);
                cDOBGender.Text = string.Format("{0} / {1}", entityReg.DateOfBirthInString, entity.Gender);
                cAgePatient.Text = string.Format("{0}tahun {1}bulan {2}hari", entityReg.AgeInYear, entityReg.AgeInMonth, entityReg.AgeInDay);
                cParamedicRegName.Text = entityReg.ParamedicName;
                cPayerReg.Text = entityReg.BusinessPartnerName;
                cServiceUnit.Text = entityReg.ServiceUnitName;
                cClassName.Text = entityReg.ClassName + " / " + entityReg.BedCode;
            }
            else
            {
                registrationID = entity.RegistrationID.ToString();
                cRegistrationNo.Text = entity.RegistrationNo;
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
                cRegistrationDate.Text = entity.RegistrationDateInString;
                cDischargeDate.Text = entity.cfDischargeDateInString;
                cPatient.Text = string.Format("{0}", entity.PatientName);
                cDOBGender.Text = string.Format("{0} / {1}", entity.DateOfBirthInString, entity.Gender);
                cAgePatient.Text = string.Format("{0}tahun {1}bulan {2}hari", entity.AgeInYear, entity.AgeInMonth, entity.AgeInDay);
                cParamedicRegName.Text = entity.ParamedicName;
                cPayerReg.Text = entity.BusinessPartnerName;
                cServiceUnit.Text = entity.ServiceUnitName;
                cClassName.Text = entity.ClassName + " / " + entity.BedCode;
            }
            lblNamaPetugas.Text = appSession.UserFullName;

            //String ServiceDate = entity.StartServiceDate.ToString(Constant.FormatString.DATE_FORMAT);
            //if (ServiceDate != null && ServiceDate != "01-Jan-1900")
            //{
            //    cServiceDate.Text = entity.StartServiceDate.ToString(Constant.FormatString.DATE_FORMAT) + " - " + entity.CustomDischargeDate;
            //}
            //else
            //{
            //    cServiceDate.Text = entity.RegistrationDateInString;
            //}
            
            coverageLimitAmount = entity.CoverageLimitAmount;

            lstPatientBill = BusinessLayer.GetPatientBillList(string.Format(
                "RegistrationID = {0} AND GCTransactionStatus != '{1}'", registrationID, Constant.TransactionStatus.VOID));

            List<GetPatientChargesDtPerBillingGroupReport> lstEntity = BusinessLayer.GetPatientChargesDtPerBillingGroupReport(Convert.ToInt32(param[0]));

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

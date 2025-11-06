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
    public partial class BPembayaranDetail : BaseDailyPortraitRpt
    {
        List<PatientBill> lstEntityBill = null;
        String custType;

        public BPembayaranDetail()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = {0}", appSession.HealthcareID))[0];

            vPatientPaymentHd entityPayment = BusinessLayer.GetvPatientPaymentHdList(string.Format("{0}", param[0]))[0];
            lblPaymentNo.Text = entityPayment.PaymentNo;
            lblPaymentType.Text = entityPayment.PaymentType;
            lblPaymentDate.Text = string.Format("{0}", entityPayment.PaymentDateInString);

            if (entityPayment.LastUpdatedByUserName != "")
            {
                lblLastUpdatedBy.Text = entityPayment.LastUpdatedByUserName;
            }
            else
            {
                lblLastUpdatedBy.Text = entityPayment.CreatedByUserName;
            }

            lblLastUpdatedDate.Text = entityHealthcare.City + ", " + entityPayment.PaymentDate.ToString(Constant.FormatString.DATE_FORMAT) + " " +entityPayment.PaymentTime.ToString();

            vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", entityPayment.RegistrationID))[0];
            RegistrationBPJS entityRegBPJS = BusinessLayer.GetRegistrationBPJSList(String.Format("RegistrationID = {0}", entityPayment.RegistrationID)).FirstOrDefault();

            custType = entityReg.GCCustomerType;
            lblRegistrationNo.Text = entityReg.RegistrationNo;
            if (entityRegBPJS != null)
            {
                lblSEP.Text = string.Format("No RM / No SEP");
                lblSEPNo.Text = string.Format("{0} / {1}", entityReg.MedicalNo, entityRegBPJS.NoSEP);
            }
            else
            {
                lblSEP.Text = string.Format("No RM");
                lblSEPNo.Text = string.Format("{0}", entityReg.MedicalNo);
            }
            lblPatientName.Text = entityReg.PatientName;
            lblPayer.Text = entityReg.BusinessPartnerName;

            lstEntityBill = BusinessLayer.GetPatientBillList(string.Format("PatientBillingID IN (SELECT PatientBillingID FROM PatientBillPayment WHERE PaymentID = {0})", entityPayment.PaymentID));
            lblCoverageLimit.Text = entityReg.CoverageLimitAmount.ToString("N2");
            lblPatientAdministration.Text = lstEntityBill.Sum(t => t.PatientAdminFeeAmount).ToString("N2");
            lblAdministration.Text = lstEntityBill.Sum(t => t.AdministrationFeeAmount).ToString("N2");
            lblPatientService.Text = lstEntityBill.Sum(t => t.PatientServiceFeeAmount).ToString("N2");
            lblService.Text = lstEntityBill.Sum(t => t.ServiceFeeAmount).ToString("N2");
            lblTotalPasien.Text = lstEntityBill.Sum(p => p.TotalPatientAmount).ToString("N2");
            lblTotalInstansi.Text = lstEntityBill.Sum(p => p.TotalPayerAmount).ToString("N2");
            lblDiskonPasien.Text = lstEntityBill.Sum(p => p.PatientDiscountAmount).ToString("N2");
            lblDiskonInstansi.Text = lstEntityBill.Sum(p => p.PayerDiscountAmount).ToString("N2");
            lblGTPasien.Text = (lstEntityBill.Sum(p => p.TotalPatientAmount - p.PatientDiscountAmount)).ToString("N2");
            lblGTInstansi.Text = (lstEntityBill.Sum(p => p.TotalPayerAmount - p.PayerDiscountAmount)).ToString("N2");

            List<PatientBillPayment> entityBillPayment = BusinessLayer.GetPatientBillPaymentList(string.Format("PaymentID = {0}", entityPayment.PaymentID));
            if (entityPayment.GCPaymentType != Constant.PaymentType.AR_PATIENT && entityPayment.GCPaymentType != Constant.PaymentType.AR_PAYER)
            {
                lblBayarPasien.Text = entityPayment.TotalPaymentAmount.ToString("N2");
                lblKembalianPasien.Text = entityPayment.CashBackAmount.ToString("N2");
            }
            else
            {
                lblBayarPasien.Text = "0.00";
                lblKembalianPasien.Text = "0.00";
            }
            lblBayarInstansi.Text = "0.00";
            lblKembalianInstansi.Text = "0.00";

            if (entityPayment.TotalPaymentAmount != 0)
            {
                lblTerbilang.Text = string.Format("# {0} #", Function.NumberInWords(Convert.ToInt32(entityPayment.TotalPaymentAmount), true));
            }
            else
            {
                lblTerbilang.Text = string.Format("# {0} #", Function.NumberInWords(Convert.ToInt32(0), true));
            }

            base.InitializeReport(param);
        }
    }
}

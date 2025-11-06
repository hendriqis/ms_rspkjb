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
    public partial class BPiutangInstansiDetail : BaseDailyPortraitRpt
    {
        List<PatientBill> lstEntityBill = null;
        String custType;

        public BPiutangInstansiDetail()
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

            lblLastUpdatedDate.Text = entityHealthcare.City + ", " + entityPayment.PaymentDate.ToString(Constant.FormatString.DATE_FORMAT) + " " + entityPayment.PaymentTime.ToString();

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

            lblTotalInstansi.Text = lstEntityBill.Sum(p => p.TotalPayerAmount).ToString("N2");
            lblDiskonInstansi.Text = lstEntityBill.Sum(p => p.PayerDiscountAmount).ToString("N2");
            lblGTInstansi.Text = (lstEntityBill.Sum(p => p.TotalPayerAmount - p.PayerDiscountAmount)).ToString("N2");

            //vPatientBill entityBill = BusinessLayer.GetvPatientBillList(string.Format("PaymentID = {0}", entityPayment.PaymentID))[0];
            PatientBillPayment entityBillPayment = BusinessLayer.GetPatientBillPaymentList(string.Format("PaymentID = {0}", entityPayment.PaymentID))[0];

            lblTerbilang.Text = string.Format("# {0} #", Function.NumberInWords(Convert.ToInt32(Math.Abs(entityBillPayment.PayerPaymentAmount)), true));

            base.InitializeReport(param);
        }
    }
}

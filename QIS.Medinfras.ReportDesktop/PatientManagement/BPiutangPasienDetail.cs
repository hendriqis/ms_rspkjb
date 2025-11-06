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
    public partial class BPiutangPasienDetail : BaseDailyPortraitRpt
    {
        List<PatientBill> lstEntityBill = null;
        String custType;

        public BPiutangPasienDetail()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = {0}",appSession.HealthcareID))[0];

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
            lblRegistrationNo.Text = entityReg.RegistrationNo;
            lblPatientName.Text = entityReg.PatientName;
            lblPayer.Text = entityReg.BusinessPartnerName;

            lstEntityBill = BusinessLayer.GetPatientBillList(string.Format("PatientBillingID IN (SELECT PatientBillingID FROM PatientBillPayment WHERE PaymentID = {0})", entityPayment.PaymentID));

            lblTotalPasien.Text = lstEntityBill.Sum(p => p.TotalPatientAmount).ToString("N2");
            lblDiskonPasien.Text = lstEntityBill.Sum(p => p.PatientDiscountAmount).ToString("N2");
            lblGTPasien.Text = (lstEntityBill.Sum(p => p.TotalPatientAmount - p.PatientDiscountAmount)).ToString("N2");

            //vPatientBill entityBill = BusinessLayer.GetvPatientBillList(string.Format("PaymentID = {0}", entityPayment.PaymentID))[0];
            PatientBillPayment entityBillPayment = BusinessLayer.GetPatientBillPaymentList(string.Format("PaymentID = {0}", entityPayment.PaymentID))[0];

            lblTerbilang.Text = string.Format("# {0} #", Function.NumberInWords(Convert.ToInt32(Math.Abs(entityBillPayment.PatientPaymentAmount)), true));
            
            base.InitializeReport(param);
        }
    }
}

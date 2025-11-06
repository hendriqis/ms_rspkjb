using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
namespace QIS.Medinfras.Report
{
    public partial class BPiutangInstansi_DM : BaseDailyPortraitRpt
    {
        public BPiutangInstansi_DM()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            //string filter = "";
            //if(param[0] != "") 
            //    filter = string.Format("{0} AND GCPaymentMethod = '{1}'", param[0], Constant.PaymentType.AR_PATIENT);

            //vPatientPaymentDt entityDt = BusinessLayer.GetvPatientPaymentDtList(string.Format("{0}", param[0]))[0];
            //lblTotalAmountString.Text = entityDt.PaymentAmountInString;
            //lblTotalAmount.Text = entityDt.PaymentAmount.ToString();

            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID))[0];
            lblPaymentDate2.Text = entityHealthcare.City + ", [PaymentDateInString]";

            vPatientPaymentHd entity = BusinessLayer.GetvPatientPaymentHdList(string.Format("{0}", param[0]))[0];
            lblTotalAmountString.Text = entity.TotalPaymentAmountInString;

            vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", entity.RegistrationID))[0];
            lblRegistrationNo.Text = entityReg.RegistrationNo;
            lblPatientName.Text = entityReg.PatientName;
            lblPatientName2.Text = entityReg.PatientName;
            lblBusinessPartnerCode.Text = entityReg.BusinessPartnerCode;
            lblBusinessPartnerName.Text = entityReg.BusinessPartnerName;
            lblContractNo.Text = entityReg.ContractNo;
            base.InitializeReport(param);
        }

        private void lblKeterangan_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string nama = lblPatientName.Text;
            ((XRLabel)sender).Text = Helper.GetTextFormatText(page, Constant.TextFormat.AR_PAYER_DESCRIPTION).Replace("[PatientName]", nama);
        }
    }
}

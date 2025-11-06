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
    public partial class BKembaliUangMuka_DM : BaseDailyPortraitRpt
    {
        public BKembaliUangMuka_DM()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string filter = "";
            if(param[0] != "") 
                filter = string.Format("{0} AND PaymentAmount < 0", param[0]);

            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID))[0];
            lblPaymentDate2.Text = entityHealthcare.City + ", [PaymentDateInString]";

            vPatientPaymentDt entityDt = BusinessLayer.GetvPatientPaymentDtList(filter)[0];
            lblTotalAmountString.Text = entityDt.PaymentAmountInString;
            lblTotalAmount.Text = Math.Abs(entityDt.PaymentAmount).ToString("N2");

            vPatientPaymentHd entity = BusinessLayer.GetvPatientPaymentHdList(string.Format("{0}", param[0]))[0];

            vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", entity.RegistrationID))[0];
            lblRegistrationNo.Text = entityReg.RegistrationNo;
            lblPatientName.Text = entityReg.PatientName;
            lblPatientName2.Text = entityReg.PatientName;
            
            base.InitializeReport(param);
        }

        private void lblKeterangan_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string nama = lblPatientName.Text;
            ((XRLabel)sender).Text = Helper.GetTextFormatText(page, Constant.TextFormat.REFUND_DOWN_PAYMENT_DESCRIPTION).Replace("[PatientName]", nama);
        }
    }
}

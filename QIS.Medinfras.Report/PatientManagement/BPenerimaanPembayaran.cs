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
    public partial class BPenerimaanPembayaran : BaseDailyPortraitRpt
    {
        public BPenerimaanPembayaran()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID))[0];
            lblCity.Text = entityHealthcare.City+",";

            vPatientPaymentHd entity = BusinessLayer.GetvPatientPaymentHdList(string.Format("{0}", param[0]))[0];
            lblTotalAmountString.Text = entity.TotalReceiveAmountInString;
            
            vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", entity.RegistrationID))[0];
            lblRegistrationNo.Text = entityReg.RegistrationNo;
            lblPatientName.Text = entityReg.PatientName;
            lblPatientName2.Text = entityReg.PatientName;
            
            base.InitializeReport(param);
        }

        private void lblKeterangan_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string nama = lblPatientName.Text;
            ((XRLabel)sender).Text = Helper.GetTextFormatText(page, Constant.TextFormat.PAYMENT_RECEIPT_DESCRIPTION).Replace("[PatientName]", nama);
        }
    }
}

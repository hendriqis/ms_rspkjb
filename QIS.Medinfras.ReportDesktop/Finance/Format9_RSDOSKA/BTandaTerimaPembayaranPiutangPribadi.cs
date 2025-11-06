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
    public partial class BTandaTerimaPembayaranPiutangPribadi : BaseA6Rpt
    {
        private string HealthcareName = "";
        private string Address = "";
        private string City = "";

        public BTandaTerimaPembayaranPiutangPribadi()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            String HealthcareID = AppSession.UserLogin.HealthcareID;
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", HealthcareID)).FirstOrDefault();

            HealthcareName = oHealthcare.HealthcareName;
            Address = oHealthcare.StreetName;
            City = oHealthcare.City;

            string[] temp = param[0].Split(';');
            vARReceivingHd entity = BusinessLayer.GetvARReceivingHdList(temp[0]).FirstOrDefault();
            lblPaymentNo.Text = entity.ARReceivingNo;
            lblPaymentDate.Text = entity.ReceivingDateInString;
            lblPatientName.Text = string.Format("({0}) {1}", entity.MedicalNo, entity.PatientName);
            xrLabel1.Text = entity.PatientName;
            lblPatientAddress.Text = entity.PatientAddress;
            lblTotalAmount.Text = string.Format("Rp. {0}", entity.cfTotalInvoiceAmountInString);
            lblTotalAmountString.Text = entity.cfTotalInvoiceAmountInStringInd;
            lblLastUpdatedBy.Text = entity.LastUpdatedByName;
            if (entity.LastUpdatedDate.ToString(Constant.FormatString.DATE_FORMAT_112) != "19000101")
            {
                lblLastUpdatedDate.Text = entity.LastUpdatedDate.ToString(Constant.FormatString.DATE_FORMAT);
            }
            else
            {
                lblLastUpdatedDate.Text = entity.CreatedDate.ToString(Constant.FormatString.DATE_FORMAT);
            }

            string[] invoice = new String[] { temp[1] };
            base.InitializeReport(invoice);
        }
        
    }
}

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
    public partial class BPenerimaanUangMukaA6 : BaseA6Rpt
    {

        public BPenerimaanUangMukaA6()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID)).FirstOrDefault();

            vPatientPaymentHd entityPayment = BusinessLayer.GetvPatientPaymentHdList(string.Format("{0}", param[0])).FirstOrDefault();
            lblLastUpdatedBy.Text = entityPayment.CreatedByUser;
            lblLastUpdatedDate.Text = entityHealthcare.City + ", " + entityPayment.PaymentDate.ToString(Constant.FormatString.DATE_FORMAT);
            lblTotalAmountString.Text = "# " + entityPayment.TotalPaymentAmountInString + " #";

            vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", entityPayment.RegistrationID)).FirstOrDefault();
            lblUnitPelayanan1.Text = entityReg.ServiceUnitName;
            lblPatientAddress.Text = entityReg.StreetName + " " + entityReg.County + " " + entityReg.District + " " + entityReg.City;


            base.InitializeReport(param);
        }
        
    }
}

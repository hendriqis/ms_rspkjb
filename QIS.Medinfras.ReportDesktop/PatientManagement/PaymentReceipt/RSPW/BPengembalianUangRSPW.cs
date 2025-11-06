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
    public partial class BPengembalianUangRSPW : BaseCustomA6Rpt
    {
        private int lineNumber = 0;
        private int detailID = 0;
        private int oldDetailID = 0;
        private decimal totalAmount = 0;

        public BPengembalianUangRSPW()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            lblReportTitle.Text = reportMaster.ReportTitle1;

            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();
            if (entityHealthcare != null)
            {
                lblHealthcare.Text = entityHealthcare.HealthcareName;
                lblAddress.Text = string.Format("{0} {1} {2}", entityHealthcare.StreetName, entityHealthcare.City, entityHealthcare.ZipCode);
                lblHealthcareInfoDetail.Text = string.Format("Telp/Fax : {0}/{1}", entityHealthcare.PhoneNo1, entityHealthcare.FaxNo1);
                lblHealthcareNameReceive.Text = entityHealthcare.HealthcareName;
            }

            List<vPatientPaymentHd> entityPayment = BusinessLayer.GetvPatientPaymentHdList(string.Format("{0}", param[0]));
            txtCityAndDate.Text = entityHealthcare.City + ", " + entityPayment.FirstOrDefault().PaymentDate.ToString(Constant.FormatString.DATE_FORMAT);
            lblTotalAmountString.Text = "# " + entityPayment.FirstOrDefault().TotalPaymentRefundAmountInString + " #";

            lblUnitPelayanan.Text = entityPayment.FirstOrDefault().ServiceUnitName;
            this.DataSource = entityPayment;

            //base.InitializeReport(param);
        }
    }
}

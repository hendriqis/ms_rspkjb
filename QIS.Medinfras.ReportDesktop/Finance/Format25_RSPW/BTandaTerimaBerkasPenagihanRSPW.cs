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
    public partial class BTandaTerimaBerkasPenagihanRSPW : BaseCustomDailyPotraitA5Rpt
    {
        private string HealthcareName = "";
        private string Address = "";
        private string City = "";

        public BTandaTerimaBerkasPenagihanRSPW()
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
            List<vARInvoiceHd1> lstEntity = BusinessLayer.GetvARInvoiceHd1List(temp[0]);
            lblReceivedFrom.Text = oHealthcare.HealthcareName;
            this.DataSource = lstEntity;

            base.InitializeReport(param);
        }

        private void lblTanggal_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string documentDate = Convert.ToDateTime(GetCurrentColumnValue("DocumentDate")).ToString(Constant.FormatString.DATE_FORMAT);
            string createdDate = Convert.ToDateTime(GetCurrentColumnValue("CreatedDate")).ToString(Constant.FormatString.DATE_FORMAT);

            if (documentDate != "01-Jan-1900")
            {
                lblTanggal.Text = string.Format("{0}, {1}", City, documentDate);
            }
            else
            {
                lblTanggal.Text = string.Format("{0}, {1}", City, createdDate);
            }
        }

    }
}

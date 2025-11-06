using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BKwitansiPenerimaanPiutangInstansiRSSBB : BaseRpt
    {
        private string HealthcareName = "";
        private string Address = "";
        private string City = "";

        public BKwitansiPenerimaanPiutangInstansiRSSBB()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            String HealthcareID = AppSession.UserLogin.HealthcareID;
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", HealthcareID)).FirstOrDefault();

            xrLogo.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/logo.png");

            HealthcareName = oHealthcare.HealthcareName;
            Address = oHealthcare.StreetName;
            City = oHealthcare.City;

            lblHealthcareName.Text = HealthcareName;
            lblAddress.Text = Address;
            lblCity.Text = City;

            lblNotes.Text = string.Format("Kwitansi ini sah bila ada tanda tangan Kasir, ada cap {0} / Bank, dan hanya terbit satu kali.", oHealthcare.HealthcareName);

            string[] temp = param[0].Split(';');
            List<vARReceivingHd> lstEntity = BusinessLayer.GetvARReceivingHdList(temp[0]);

            this.DataSource = lstEntity;
        }

        private void lblTanggal_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string receivingDate = Convert.ToDateTime(GetCurrentColumnValue("ReceivingDate")).ToString(Constant.FormatString.DATE_FORMAT);
            string createdDate = Convert.ToDateTime(GetCurrentColumnValue("CreatedDate")).ToString(Constant.FormatString.DATE_FORMAT);

            if (receivingDate != "01-Jan-1900")
            {
                lblTanggal.Text = string.Format("{0}, {1}", City, receivingDate);
            }
            else
            {
                lblTanggal.Text = string.Format("{0}, {1}", City, createdDate);
            }
        }
    }
}

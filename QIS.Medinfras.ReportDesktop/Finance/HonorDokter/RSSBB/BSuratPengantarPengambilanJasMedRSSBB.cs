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
    public partial class BSuratPengantarPengambilanJasMedRSSBB : BaseRpt
    {
        private string City = "";

        public BSuratPengantarPengambilanJasMedRSSBB()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            xrLogo.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/logo.png");

            String HealthcareID = AppSession.UserLogin.HealthcareID;
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", HealthcareID)).FirstOrDefault();

            lblHealthcareName.Text = oHealthcare.HealthcareName;
            lblAddress.Text = oHealthcare.StreetName;
            lblCity.Text = City = oHealthcare.City;

            SettingParameter setvarLeft = BusinessLayer.GetSettingParameter(Constant.SettingParameter.FN_KASIE_PENGELOLAAN_UTANG);
            lblSignLeft.Text = setvarLeft.ParameterValue;
            lblSignLeftCaption.Text = setvarLeft.ParameterName;
            
            List<vTransRevenueSharingHd> lstEntity = BusinessLayer.GetvTransRevenueSharingHdList(param[0]);
            this.DataSource = lstEntity;
        }

        private void lblTanggal_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string ProcessedDate = Convert.ToDateTime(GetCurrentColumnValue("ProcessedDate")).ToString(Constant.FormatString.DATE_FORMAT);
            string CreatedDate = Convert.ToDateTime(GetCurrentColumnValue("CreatedDate")).ToString(Constant.FormatString.DATE_FORMAT);

            if (ProcessedDate != "01-Jan-1900")
            {
                lblTanggal.Text = string.Format("{0}, {1}", City, ProcessedDate);
            }
            else
            {
                lblTanggal.Text = string.Format("{0}, {1}", City, CreatedDate);
            }
        }
    }
}

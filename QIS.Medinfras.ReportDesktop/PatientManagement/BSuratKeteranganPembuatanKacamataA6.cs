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
    public partial class BSuratKeteranganPembuatanKacamataA6 : BaseCustomA6Rpt
    {
        public BSuratKeteranganPembuatanKacamataA6()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", param[0]))[0];
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];

            xrLogo.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/logo.png");
            xrKacamata.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/Cetakan/kacamata.jpg");
            cHealthcareName.Text = oHealthcare.HealthcareName;
            cHealthcareAddress.Text = string.Format("{0} {1} {2}", oHealthcare.StreetName, oHealthcare.City, oHealthcare.ZipCode);

            txtPasien.Text = entity.cfPatientNameSalutation2;
            txtPasien.Font = new Font("Times New Roman", 9);
            if (!string.IsNullOrEmpty(entity.DiagnosisText))
            {
                txtDiagnosa.Text = entity.DiagnosisText;
            }
            else
            {
                txtDiagnosa.Text = "-";
            }
            lblDokter.Text = string.Format("( {0} )", entity.ParamedicName);

            base.InitializeReport(param);
        }

       
    }
}

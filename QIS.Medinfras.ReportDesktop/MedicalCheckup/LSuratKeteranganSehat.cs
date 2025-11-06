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
    public partial class LSuratKeteranganSehat : BaseCustomDailyPotraitRpt
    {
        public LSuratKeteranganSehat()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string toFollow = param[1];
            vRegistration entity = BusinessLayer.GetvRegistrationList(param[0]).FirstOrDefault();
                        
            lblMengikutiAtauMenjadi.Text = toFollow;

            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID)).FirstOrDefault();
            lblDateSign.Text = string.Format("{0}, {1}", oHealthcare.City, entity.RegistrationDateInString);

            String type = param[2];
            if (type == "0")
            {
                lblType.Text = "Tidak Buta Warna";
            }
            else
            {
                lblType.Text = "Buta Warna";
            }

            base.InitializeReport(param);
        }

    }
}

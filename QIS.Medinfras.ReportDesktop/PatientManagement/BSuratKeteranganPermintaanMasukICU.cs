using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using ThoughtWorks.QRCode.Codec;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BSuratKeteranganPermintaanMasukICU : BaseRpt
    {

        public BSuratKeteranganPermintaanMasukICU()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            #region Header 1 : Healthcare
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            if (oHealthcare != null)
            {
                xrLogo.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/logo.png");
                cHealthcareName.Text = oHealthcare.HealthcareName;
                cHealthcareAddress.Text = oHealthcare.StreetName;
                cHealthcareCityZipCodes.Text = string.Format("{0} {1}", oHealthcare.City, oHealthcare.ZipCode);
                cHealthcarePhone.Text = oHealthcare.PhoneNo1;
                cHealthcareFax.Text = oHealthcare.FaxNo1;
            }
            #endregion

            #region Header 2 : Patient Information
            vConsultVisit entity = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", param[0])).FirstOrDefault();
            vPatient entityP = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", entity.MRN))[0];
            vHealthcare entityH1 = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            ParamedicMaster entityPM = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", entity.ParamedicID))[0];
            Healthcare entityH2 = BusinessLayer.GetHealthcare(entityPM.HealthcareID);

            lblName.Text = entityP.PatientName;
            lblUmur.Text = string.Format("{0} / {1}", entityP.cfGender, entityP.DateOfBirthInString);
            lblAddress.Text = entityP.HomeAddress;
            lblUnit.Text = entity.ServiceUnitName;
            lblRM.Text = entityP.MedicalNo;
            lblKeterangan.Text = string.Format("Untuk itu saya BERSEDIA MENTAATI peraturan dan tata tertib yang berlaku di {0}, khususnya di Unit Perawatan Intensif (HCU/ICU/ICCU/PICU/NICU).", entityH2.HealthcareName);

            String jamPagi = param[1];
            String jamSore = param[2];

            lblPagi.Text = jamPagi;
            lblSore.Text = jamSore;

            lblDpjp.Text = entityPM.FullName;
            #endregion

            #region Footer
            cCityDate.Text = String.Format("{0}, {1}", oHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT)); // Surabaya, 13-Nov-2017
            //lblSignParamedicName.Text = entityCV.ParamedicName;
            ttdDokter.ImageUrl = string.Format("{0}{1}/Signature/{2}.png", AppConfigManager.QISPhysicalDirectory, AppConfigManager.QISParamedicImagePath, entity.ParamedicCode);
            ttdDokter.Visible = true;
            lblReportProperties.Text = string.Format("MEDINFRAS - {0}, Print Date/Time:{1}, User ID:{2}", reportMaster.ReportCode, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"), appSession.UserName);
            #endregion

        }
    }
}
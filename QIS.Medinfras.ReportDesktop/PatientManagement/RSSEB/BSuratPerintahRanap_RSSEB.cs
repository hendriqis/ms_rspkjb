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
    public partial class BSuratPerintahRanap_RSSEB : BaseRpt
    {

        public BSuratPerintahRanap_RSSEB()
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
            vHealthcareServiceUnitCustom entityHSU = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("HealthcareServiceUnitID = {0}", param[3]))[0];
            Room entityRoom = BusinessLayer.GetRoomList(string.Format("RoomID = {0}", param[4]))[0];
            Bed entityBed= BusinessLayer.GetBedList(string.Format("BedID = {0}", param[5]))[0];
            ParamedicMaster entityPM = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", entity.ParamedicID))[0];
            ParamedicMaster entityRMO = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", param[2]))[0];

            lblName.Text = entityP.PatientName;
            lblCity.Text = string.Format("{0}, {1}", entityH1.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));
            lblDiagnose.Text = param[1];
            lblParamedicName.Text = entityRMO.FullName;
            lblServiceUnit1.Text = string.Format("Bagian Informasi");
            lblServiceUnitName.Text = string.Format("{0} - {1}", entityHSU.ServiceUnitName, entityBed.BedCode);
            lblDOB.Text = entityP.DateOfBirthInString;
            lblGender.Text = entityP.cfGender;
            lblRM.Text = entityP.MedicalNo;
            lblUmur.Text = String.Format("{0} tahun {1} bulan {2} hari", entityP.AgeInYear, entityP.AgeInMonth, entityP.AgeInDay);
            #endregion

            #region Footer
            lblPrintedBy.Text = entity.ParamedicName;
            ttdDokter.ImageUrl = string.Format("{0}{1}/Signature/{2}.png", AppConfigManager.QISPhysicalDirectory, AppConfigManager.QISParamedicImagePath, entity.ParamedicCode);
            ttdDokter.Visible = true;
            lblReportProperties.Text = string.Format("MEDINFRAS - {0}, Print Date/Time:{1}, User ID:{2}", reportMaster.ReportCode, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"), appSession.UserName);
            #endregion
        }
    }
}
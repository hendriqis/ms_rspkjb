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
    public partial class BKeteranganKematianRSPKSB : BaseRpt
    {

        public BKeteranganKematianRSPKSB()
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
            ConsultVisit entity = BusinessLayer.GetConsultVisitList(string.Format("VisitID = {0}", param[0])).FirstOrDefault();
            Registration entityReg = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0}", entity.RegistrationID)).FirstOrDefault();
            vPatient entityP = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", entityReg.MRN))[0];
            vHealthcare entityH1 = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            ParamedicMaster entityPM = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", entity.ParamedicID))[0];
            Healthcare entityH2 = BusinessLayer.GetHealthcare(entityPM.HealthcareID);
            List<vPatientDiagnosis> entityMD = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND GCDiagnoseType = '{1}' AND IsDeleted = 0", param[0], Constant.DiagnoseType.MAIN_DIAGNOSIS));
            List<vPatientDiagnosis> entitySD = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND GCDiagnoseType = '{1}' AND IsDeleted = 0", param[0], Constant.DiagnoseType.COMPLICATION));
            //vChiefComplaint entityCC = BusinessLayer.GetvChiefComplaintList(string.Format("VisitID = {0}", entityCV.VisitID)).FirstOrDefault();

            cFullName.Text = entityP.PatientName;
            cSSN.Text = entityP.SSN;
            cGender.Text = entityP.Gender;
            cGenderEnglish.Text = entityP.GCGender == Constant.Gender.MALE ? "Male" : "Female";
            cDateOfBirth.Text = entityP.DateOfBirthInString;
            cReligion.Text = entityP.Religion;
            switch (entityP.GCReligion)
            {
                case "0006^CAT":
                    cReligionEnglish.Text = "Catholicism";
                    break;
                case "0006^CHR":
                    cReligionEnglish.Text = "Christianity";
                    break;
                case "0006^OTH":
                    cReligionEnglish.Text = "Other";
                    break;
                default:
                    xrTableCell37.Text = "";
                    break;
            }
            cPlaceOfBirth.Text = entityP.CityOfBirth;
            cAddress.Text = entityP.HomeAddress;
            string dateOfDeath = entity.DateOfDeath.ToString(Constant.FormatString.DATE_FORMAT);
            if (dateOfDeath != "01-Jan-1900")
            {
                cDeathDateTime.Text = string.Format("Tanggal: {0} Pukul: {1}", entity.DateOfDeath.ToString(Constant.FormatString.DATE_FORMAT), entity.TimeOfDeath);
                cDeathDateTimeEnglish.Text = string.Format("Date: {0} At: {1}", entity.DateOfDeath.ToString(Constant.FormatString.DATE_FORMAT), entity.TimeOfDeath);
            }
            else
            {
                cDeathDateTime.Text = string.Format("Tanggal: {0} Pukul: {1}:{2}", param[1], param[2], param[3]);
                cDeathDateTimeEnglish.Text = string.Format("Date: {0} At: {1}:{2}", param[1], param[2], param[3]);
            }
            DateTime deathDate = dateOfDeath != "01-Jan-1900" ? entity.DateOfDeath : DateTime.Parse(param[1]);
            DateTime birthDate = entityP.DateOfBirth;
            cAgeOfDeath.Text = string.Format("{0} tahun {1} bulan {2} hari", 
                Function.GetPatientAgeInYear(birthDate, deathDate).ToString(),
                Function.GetPatientAgeInMonth(birthDate, deathDate).ToString(),
                Function.GetPatientAgeInDay(birthDate, deathDate).ToString());
            cAgeOfDeathEnglish.Text = string.Format("{0} year(s) {1} month(s) {2} day(s)",
                Function.GetPatientAgeInYear(birthDate, deathDate).ToString(),
                Function.GetPatientAgeInMonth(birthDate, deathDate).ToString(),
                Function.GetPatientAgeInDay(birthDate, deathDate).ToString());
            cDoctor.Text = entityPM.FullName;

            if (param[5] == "filterDOA")
            {
                chkDeathOnArrival.Checked = true;
            }
            else if (param[5] == "filterIUFD")
            {
                chkIUFD.Checked = true;
            }
            else
            {
                chkOther.Checked = true;
                cOther.Text = param[6];
            }

            cFuneralPlan.Text = param[7];
            
            subMainDiagnosis.CanGrow = true;
            mrPatientMainDiagnosisRSPKSB.InitializeReport(entity.VisitID);

            subSecondaryDiagnosis.CanGrow = true;
            mrPatientSecondaryDiagnosisRSPKSB.InitializeReport(entity.VisitID);
            #endregion

            #region Footer
            cCityDate.Text = String.Format("{0}, {1}", oHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT)); // Surabaya, 13-Nov-2017
            //lblSignParamedicName.Text = entityCV.ParamedicName;

            lblReportProperties.Text = string.Format("MEDINFRAS - {0}, Print Date/Time:{1}, User ID:{2}", reportMaster.ReportCode, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"), appSession.UserName);
            #endregion

            #region Nomor Surat Kematian
            //SettingParameterDt entitySetPar = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, Constant.SettingParameter.SA0123);
            //List<vReportPrintLog> entityReportLog = BusinessLayer.GetvReportPrintLogList(string.Format("ReportCode = '{0}' AND PrintedDate = '{1}'", reportMaster.ReportCode, DateTime.Now.ToString()));

            //Int32 countReportLog = entityReportLog.Count + 1;

            //String date = DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112);

            //String noSuratKematian = string.Format("{0}/{1}/{2}", entitySetPar.ParameterValue, date, countReportLog);
            lblSuratKematian.Text = string.Format("NOMOR : {0}", param[4]);
            #endregion
        }
    }
}
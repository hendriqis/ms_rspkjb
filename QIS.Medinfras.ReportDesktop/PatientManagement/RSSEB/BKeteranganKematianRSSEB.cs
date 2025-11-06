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
    public partial class BKeteranganKematianRSSEB : BaseRpt
    {

        public BKeteranganKematianRSSEB()
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
            ParamedicMaster entityRMO = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", param[5]))[0];
            Healthcare entityH2 = BusinessLayer.GetHealthcare(entityPM.HealthcareID);

            lblName.Text = entityP.PatientName;
            lblDOB.Text = entityP.DateOfBirthInString;
            lblAddress.Text = entityP.HomeAddress;
            lblGender.Text = entityP.cfGender;
            lblStatus.Text = entityP.MaritalStatus;
            lblRM.Text = entityP.MedicalNo;
            lblNo.Text = entityP.SSN;
            lblRemarks.Text = param[4];

            DateTime Date = DateTime.Parse(param[1]);

            int DayNumber = (int)Date.DayOfWeek;
            string day = "";

            if (DayNumber == 1)
            {
                day = "Senin";
            }
            else if (DayNumber == 2)
            {
                day = "Selasa";
            }
            else if (DayNumber == 3)
            {
                day = "Rabu";
            }
            else if (DayNumber == 4)
            {
                day = "Kamis";
            }
            else if (DayNumber == 5)
            {
                day = "Jumat";
            }
            else if (DayNumber == 6)
            {
                day = "Sabtu";
            }
            else
            {
                day = "Minggu";
            }

            lblDayName.Text = string.Format("{0}", day);
            lblDeathOfDate.Text = string.Format("{0}", Date.ToString(Constant.FormatString.DATE_FORMAT));
            lblDeathOfTime.Text = string.Format("{0}:{1}", param[2], param[3]);

            lblPrintedBy.Text = entityRMO.FullName;

            Int16 AgeInYear = Convert.ToInt16(Function.GetPatientAgeInYear(entity.DateOfBirth, Date));
            Int16 AgeInMonth = Convert.ToInt16(Function.GetPatientAgeInMonth(entity.DateOfBirth, Date));
            Int16 AgeInDay = Convert.ToInt16(Function.GetPatientAgeInDay(entity.DateOfBirth, Date));

            lblUmur.Text = String.Format("{0} tahun {1} bulan {2} hari", AgeInYear, AgeInMonth, AgeInDay);

            lblKeterangan.Text = string.Format("Berdasarkan pemeriksaan medis yang dilakukan di {0} pada :", entityH1.HealthcareName);

            #endregion

            #region Footer
            lblCity.Text = string.Format("{0}, {1}", entityH1.City, Date.ToString(Constant.FormatString.DATE_FORMAT));
            ttdDokter.ImageUrl = string.Format("{0}{1}/Signature/{2}.png", AppConfigManager.QISPhysicalDirectory, AppConfigManager.QISParamedicImagePath, entityRMO.ParamedicCode);
            ttdDokter.Visible = true;

            lblReportProperties.Text = string.Format("MEDINFRAS - {0}, Print Date/Time:{1}, User ID:{2}", reportMaster.ReportCode, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"), appSession.UserName);
            #endregion

            #region Nomor Surat Kematian
            String year = DateTime.Now.ToString(Constant.FormatString.YEAR_FORMAT);
            String month = DateTime.Now.ToString(Constant.FormatString.MONTH_FORMAT);

            List<vReportPrintLog> entityReportLog = BusinessLayer.GetvReportPrintLogList(string.Format("ReportCode = '{0}' AND MONTH(PrintedDate) = '{1}' AND YEAR(PrintedDate) = '{2}'", reportMaster.ReportCode, month, year));

            Int32 countReportLog = entityReportLog.Count;
            String reportLogNumbering = string.Format("{0:00}", countReportLog);

            lblNumbering1.Text = year.Substring(1, 1);
            lblNumbering2.Text = year.Substring(2, 1);
            lblNumbering3.Text = year.Substring(3, 1);
            lblNumbering4.Text = month.Substring(0, 1);
            lblNumbering5.Text = month.Substring(1, 1);
            lblNumbering6.Text = reportLogNumbering.Substring(0, 1);
            lblNumbering7.Text = reportLogNumbering.Substring(1, 1);
            #endregion
        }
    }
}
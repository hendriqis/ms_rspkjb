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
    public partial class BSuratVisum_RSSEBS : BaseRpt
    {

        public BSuratVisum_RSSEBS()
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
            Healthcare entityH1 = BusinessLayer.GetHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            ParamedicMaster entityPM = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", entity.ParamedicID))[0];
            Healthcare entityH2 = BusinessLayer.GetHealthcare(entityPM.HealthcareID);

            lblName.Text = entityP.PatientName;
            lblDOB.Text = entityP.DateOfBirthInString;
            lblAddress.Text = entityP.HomeAddress;
            lblGender.Text = entityP.cfGender;
            lblStatus.Text = entityP.MaritalStatus;
            lblRM.Text = entityP.MedicalNo;
            lblNo.Text = entityP.SSN;
            lblNationalty.Text = entityP.Nationality;
            lblReligion.Text = entityP.Religion;
            lblRemarks.Text = param[6];

            xrRichText1.Text = param[5];

            DateTime Date = DateTime.Parse(param[1]);
            String year = DateTime.Now.ToString(Constant.FormatString.YEAR_FORMAT);
            String month = DateTime.Now.ToString(Constant.FormatString.MONTH_FORMAT);

            string monthInRomawi = "";
            if (month == "1")
            {
                monthInRomawi = "I";
            }
            else if (month == "2")
            {
                monthInRomawi = "II";
            }
            else if (month == "3")
            {
                monthInRomawi = "III";
            }
            else if (month == "4")
            {
                monthInRomawi = "IV";
            }
            else if (month == "5")
            {
                monthInRomawi = "V";
            }
            else if (month == "6")
            {
                monthInRomawi = "VI";
            }
            else if (month == "7")
            {
                monthInRomawi = "VII";
            }
            else if (month == "8")
            {
                monthInRomawi = "VIII";
            }
            else if (month == "9")
            {
                monthInRomawi = "IX";
            }
            else if (month == "10")
            {
                monthInRomawi = "X";
            }
            else if (month == "11")
            {
                monthInRomawi = "XI";
            }
            else if (month == "12")
            {
                monthInRomawi = "XII";
            }

            Int16 AgeInYear = Convert.ToInt16(Function.GetPatientAgeInYear(entity.DateOfBirth, Date));
            Int16 AgeInMonth = Convert.ToInt16(Function.GetPatientAgeInMonth(entity.DateOfBirth, Date));
            Int16 AgeInDay = Convert.ToInt16(Function.GetPatientAgeInDay(entity.DateOfBirth, Date));

            lblUmur.Text = String.Format("{0} tahun {1} bulan {2} hari", AgeInYear, AgeInMonth, AgeInDay);

            lblKeterangan.Text = string.Format("Berdasarkan surat yang bernomor : {0}/{1}/Reskrim, perihal permintaan Visum et Repertum Luka,",param[4],year);
            lblKeterangan1.Text = string.Format("pada tanggal {0} , {1}:{2} yang ditandatangani oleh Kepala Kepolisan", Date.ToString(Constant.FormatString.DATE_FORMAT), param[2], param[3]);

            #endregion

            #region Footer
            lblCity.Text = string.Format("{0}, {1}", oHealthcare.City, Date.ToString(Constant.FormatString.DATE_FORMAT));
            lblPrintedBy.Text = entity.ParamedicName;
            lblDirektur.Text = entityH1.DirectorName;
            ttdDokter.ImageUrl = string.Format("{0}{1}/Signature/{2}.png", AppConfigManager.QISPhysicalDirectory, AppConfigManager.QISParamedicImagePath, entity.ParamedicCode);
            ttdDokter.Visible = true;

            lblReportProperties.Text = string.Format("MEDINFRAS - {0}, Print Date/Time:{1}, User ID:{2}", reportMaster.ReportCode, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"), appSession.UserName);
            #endregion

            #region Nomor Surat Visum

            List<vReportPrintLog> entityReportLog = BusinessLayer.GetvReportPrintLogList(string.Format("ReportCode = '{0}' AND MONTH(PrintedDate) = '{1}' AND YEAR(PrintedDate) = '{2}'", reportMaster.ReportCode, month, year));

            Int32 countReportLog = entityReportLog.Count;
            String reportLogNumbering = string.Format("{0:000}", countReportLog);

            if (oHealthcare.Initial == "RSSEBS")
            {
                String healthcareName = "RSE-SEI LEKOP";
                lblSuratKematian.Text = string.Format("No. {0}/{1}/VISUM/{2}/{3}", reportLogNumbering, healthcareName, monthInRomawi, year);
            }
            else
            {
                String healthcareName = oHealthcare.HealthcareName;
                lblSuratKematian.Text = string.Format("No. {0}/{1}/VISUM/{2}/{3}", reportLogNumbering, healthcareName, monthInRomawi, year);
            }
            #endregion
        }
    }
}
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
    public partial class BKeteranganKematianRSPR : BaseRpt
    {

        public BKeteranganKematianRSPR()
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
            List<vPatientDiagnosis> lstPD = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", entity.VisitID));
            //vChiefComplaint entityCC = BusinessLayer.GetvChiefComplaintList(string.Format("VisitID = {0}", entityCV.VisitID)).FirstOrDefault();

            int count = lstPD.Count;
            string listID = "";
            List<vPatientDiagnosis> lstPatientDiagnosis = null;
            if (count > 0)
            {
                foreach (vPatientDiagnosis entityPD in lstPD)
                {
                    listID += entityPD.DiagnosisText + ",";
                }
                listID = listID.Substring(0, listID.Count() - 1);
                lstPatientDiagnosis = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", entity.VisitID));
            }
            else lstPatientDiagnosis = new List<vPatientDiagnosis>();


            lblDiagnose.Text = listID.ToString();
            lblName.Text = entityP.PatientName;
            lblDOB.Text = entityP.DateOfBirthInString;
            lblAddress.Text = entityP.HomeAddress;
            lblUmur.Text = String.Format("{0}tahun {1}bulan {2}hari", entity.AgeInYear, entity.AgeInMonth, entity.AgeInDay);
            lblGender.Text = entityP.cfGender;
            lblStatus.Text = entityP.MaritalStatus;
            lblRM.Text = entityP.MedicalNo;
            lblNo.Text = entityP.SSN;
            lbl1.Text = String.Format("Telah meninggal dunia di {0}", entityH1.HealthcareName);
            lbl2.Text = String.Format("Datang dalam keadaan sudah meninggal di {0}", entityH1.HealthcareName);

            DateTime Date = DateTime.Parse(param[1]);

            lblKematian.Text = String.Format("Tanggal {0} Pukul {1}:{2}", Date.ToString(Constant.FormatString.DATE_FORMAT) , param[2] , param[3]);

            //cPatientName.Text = entityPatient.FullName;
            //cMedicalNo.Text = entityPatient.MedicalNo;
            //cGender.Text = entityCV.Gender;
            #endregion

            #region Footer
            cCityDate.Text = String.Format("{0}, {1}", oHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT)); // Surabaya, 13-Nov-2017
            //lblSignParamedicName.Text = entityCV.ParamedicName;

            lblReportProperties.Text = string.Format("MEDINFRAS - {0}, Print Date/Time:{1}, User ID:{2}", reportMaster.ReportCode, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"), appSession.UserName);
            #endregion

            #region Nomor Surat Kematian
            SettingParameterDt entitySetPar = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, Constant.SettingParameter.SA0123);
            List<vReportPrintLog> entityReportLog = BusinessLayer.GetvReportPrintLogList(string.Format("ReportCode = '{0}' AND PrintedDate = '{1}'", reportMaster.ReportCode, DateTime.Now.ToString()));

            Int32 countReportLog = entityReportLog.Count + 1;

            String date = DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112);

            String noSuratKematian = string.Format("{0}/{1}/{2}", entitySetPar.ParameterValue, date, countReportLog);
            lblSuratKematian.Text = string.Format("NOMOR : {0}", noSuratKematian);
            #endregion
        }
    }
}
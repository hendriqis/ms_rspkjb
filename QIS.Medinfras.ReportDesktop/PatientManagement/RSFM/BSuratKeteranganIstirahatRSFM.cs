using System;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BSuratKeteranganIstirahatRSFM : BaseDailyPortrait2Rpt
    {
        public BSuratKeteranganIstirahatRSFM()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            base.InitializeReport(param);

            Registration entity = BusinessLayer.GetRegistrationList(string.Format(param[0]))[0];
            vPatient entityP = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", entity.MRN))[0];
            vHealthcare entityH1 = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", appSession.HealthcareID))[0];
            ConsultVisit entityCV = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", entity.RegistrationID))[0];
            ParamedicMaster entityPM = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", entityCV.ParamedicID))[0];

            lblName.Text = entityP.PatientName;
            lblDOB.Text = String.Format("{0} / {1}", entityP.DateOfBirthInString, entityP.AgeInYear);
            lblOccupation.Text = entityP.Occupation;
            lblAddress.Text = entityP.HomeAddress;
            lblGender.Text = entityP.cfGender;

            String days = GetDateAbsent(DateTime.Parse(param[1]).Date, DateTime.Parse(param[2]).Date);

            String startDate = param[3];
            String endDate = param[4];

            lblDate.Text = string.Format("Setelah dilakukan pemeriksaan dan oleh karena penyakitnya, maka diperlukan istirahat {0} hari.", days);
            lblKeterangan2.Text = string.Format("Terhitung mulai tanggal {0} s/d {1}.", startDate, endDate);

            lblPrintDate.Text = string.Format("{0}, {1}", entityH1.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));
            lblParamedic.Text = entityPM.FullName;

            #region Nomor Surat Kematian
            SettingParameterDt entitySetPar = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, Constant.SettingParameter.SA0123);
            List<vReportPrintLog> entityReportLog = BusinessLayer.GetvReportPrintLogList(string.Format("ReportCode = '{0}' AND PrintedDate = '{1}'", reportMaster.ReportCode, DateTime.Now.ToString()));

            Int32 countReportLog = entityReportLog.Count + 1;

            String date = DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112);

            String noSuratKematian = string.Format("{0}/{1}/{2}", entitySetPar.ParameterValue, date, countReportLog);
            lblSuratKematian.Text = string.Format("NOMOR : {0}", noSuratKematian);
            #endregion

            base.InitializeReport(param);
        }

        public String GetDateAbsent(DateTime date1, DateTime date2)
        {
            TimeSpan span = date2.AddDays(1) - date1; 
            int day = span.Days;

            return day.ToString();
        }
    }
}

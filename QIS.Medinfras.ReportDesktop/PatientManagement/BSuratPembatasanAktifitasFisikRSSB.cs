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
    public partial class BSuratPembatasanAktifitasFisikRSSB : BaseRpt
    {
        public BSuratPembatasanAktifitasFisikRSSB()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            #region Footer
            lblReportProperties.Text = string.Format("MEDINFRAS - {0}, Print Date/Time:{1}, User ID:{2}", reportMaster.ReportCode, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"), appSession.UserName);
            #endregion

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
                cTanggal.Text = string.Format("{0} , {1}", oHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));
            }
            #endregion

            //lblDate.Text = "Date : " + DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT);
            //lblTanggal.Text = "Tanggal : " + DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT);

            vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}",param[0])).FirstOrDefault();
            vConsultVisit entityCV = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0}", param[0])).FirstOrDefault();
            vPatient entityP = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", entityReg.MRN)).FirstOrDefault();
            List<TempData> lstTemp = new List<TempData>();
            TempData newEntity = new TempData();

            newEntity.Patient = entityP.PatientName;
            newEntity.DateFrom = param[3];
            newEntity.DateTo = param[4];
            newEntity.Days = GetDateAbsent(DateTime.Parse(param[1]).Date, DateTime.Parse(param[2]).Date);
            lblPatientName.Text = entityP.PatientName;
            lblPatientAge.Text = string.Format("{0} tahun {1} bulan {2} hari", entityReg.AgeInYear, entityReg.AgeInMonth, entityReg.AgeInDay);
            lblOccupation.Text = entityP.Occupation;

            lstTemp.Add(newEntity);

            this.DataSource = lstTemp;
        }

        public String GetDateAbsent(DateTime date1, DateTime date2)
        {
            TimeSpan span = date2 - date1;
            int day = span.Days;

            return day.ToString();
        }

        private class TempData
        {
            private string _Patient;

            public string Patient
            {
                get { return _Patient; }
                set { _Patient = value; }
            }

            private string _DateFrom;

            public string DateFrom
            {
                get { return _DateFrom; }
                set { _DateFrom = value; }
            }

            private string _DateTo;

            public string DateTo
            {
                get { return _DateTo; }
                set { _DateTo = value; }
            }

            private string _Days;

            public string Days
            {
                get { return _Days; }
                set { _Days = value; }
            }

        }

    }
}

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
    public partial class BMedicalSickRSRT : BaseRpt
    {
        public BMedicalSickRSRT()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            #region Footer
            lblReportProperties.Text = string.Format("MEDINFRAS - {0}, Print Date/Time:{1}, User ID:{2}", reportMaster.ReportCode, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"), appSession.UserName);
            #endregion
            Registration entityR = BusinessLayer.GetRegistration(Convert.ToInt32(param[0]));
            ConsultVisit entityCV = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", entityR.RegistrationID))[0];
            List<PatientDiagnosis> entityDiag = BusinessLayer.GetPatientDiagnosisList(string.Format("VisitID = {0}", entityCV.VisitID));
            Patient entityP = BusinessLayer.GetPatient(Convert.ToInt32(entityR.MRN));
            vAddress oddress = BusinessLayer.GetvAddressList(string.Format("AddressID = {0}", entityP.HomeAddressID)).FirstOrDefault();
            List<TempData> lstTemp = new List<TempData>();
            TempData newEntity = new TempData();

            string diagnose = "";
            foreach (PatientDiagnosis pd in entityDiag.Where(a => a.GCDiagnoseType == Constant.DiagnoseType.MAIN_DIAGNOSIS && a.IsDeleted == false))
            {
                if (string.IsNullOrEmpty(diagnose))
                {
                    diagnose = pd.DiagnosisText;
                }
                else
                {
                    diagnose += "; " + pd.DiagnosisText;
                }
            }
            newEntity.DOB = entityP.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT);
            newEntity.Patient = entityP.FullName;
            newEntity.Diagnose = diagnose;
            newEntity.DateFrom = param[3];
            newEntity.DateTo = param[4];
            newEntity.Days = GetDateAbsent(DateTime.Parse(param[1]).Date, DateTime.Parse(param[2]).Date);

            lstTemp.Add(newEntity);
            lblPatientName.Text = entityP.FullName;
            lblAddress.Text = oddress.cfFullHomeAddress;
            this.DataSource = lstTemp;
        }

        public String GetDateAbsent(DateTime date1, DateTime date2)
        {
            TimeSpan span = date2 - date1;
            int day = span.Days;

            if (day == 0)
            {
                day = 1;
            }
            else
            {
                day = day + 1;
            }

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

            private string _DOB;

            public string DOB
            {
                get { return _DOB; }
                set { _DOB = value; }
            }

            private string _Diagnose;

            public string Diagnose
            {
                get { return _Diagnose; }
                set { _Diagnose = value; }
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

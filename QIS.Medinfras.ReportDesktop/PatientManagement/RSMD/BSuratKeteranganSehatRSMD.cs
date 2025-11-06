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
    public partial class BSuratKeteranganSehatRSMD : BaseRpt
    {
        public BSuratKeteranganSehatRSMD()
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
            }
            #endregion

            //lblDate.Text = "Date : " + DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT);
            //lblTanggal.Text = "Tanggal : " + DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT);

            vRegistration entityR = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", param[0])).FirstOrDefault();
            ConsultVisit entityCV = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", entityR.RegistrationID))[0];
            List<PatientDiagnosis> entityDiag = BusinessLayer.GetPatientDiagnosisList(string.Format("VisitID = {0}", entityCV.VisitID));
            vPatient entityP = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", Convert.ToInt32(entityR.MRN))).FirstOrDefault();
            List<vVitalSignDt> entityVS = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID = {0} ORDER BY ID DESC", entityCV.VisitID));
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
            newEntity.DOB = string.Format("{0}",entityR.AgeInYear);
            newEntity.Occupation = string.Format("{0}", entityP.Occupation);
            newEntity.Address = string.Format("{0}", entityP.HomeAddress);
            newEntity.Patient = entityP.PatientName;
            newEntity.CM = entityVS.Where(a => a.VitalSignLabel == "HEIGHT").FirstOrDefault().VitalSignValue;
            newEntity.KG = entityVS.Where(a => a.VitalSignLabel == "WEIGHT").FirstOrDefault().VitalSignValue;
            newEntity.mmHg = string.Format("{0} / {1}", entityVS.Where(a => a.VitalSignLabel == "TDs").FirstOrDefault().VitalSignValue, entityVS.Where(a => a.VitalSignLabel == "TDd").FirstOrDefault().VitalSignValue);
            newEntity.PrintDate = string.Format("{0}, {1}", oHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT)); // Surabaya, 13-Nov-2017
            newEntity.ParamedicName = string.Format("{0}", entityR.ParamedicName);
            newEntity.DateFrom = param[1];
            newEntity.Diagnose = diagnose;
            
            lstTemp.Add(newEntity);

            this.DataSource = lstTemp;
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

            private string _Occupation;

            public string Occupation
            {
                get { return _Occupation; }
                set { _Occupation = value; }
            }

            private string _Address;

            public string Address
            {
                get { return _Address; }
                set { _Address = value; }
            }

            private string _CM;

            public string CM
            {
                get { return _CM; }
                set { _CM = value; }
            }

            private string _KG;

            public string KG
            {
                get { return _KG; }
                set { _KG = value; }
            }

            private string _mmHg;

            public string mmHg
            {
                get { return _mmHg; }
                set { _mmHg = value; }
            }

            private string _PrintDate;

            public string PrintDate
            {
                get { return _PrintDate; }
                set { _PrintDate = value; }
            }

            private string _Diagnose;

            public string Diagnose
            {
                get { return _Diagnose; }
                set { _Diagnose = value; }
            }

            private string _ParamedicName;
            
            public string ParamedicName
            {
                get { return _ParamedicName; }
                set { _ParamedicName = value; }
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

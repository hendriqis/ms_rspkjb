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
    public partial class BCoverMCUResult : BaseRpt
    {
        public BCoverMCUResult()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vConsultVisit1 cv = BusinessLayer.GetvConsultVisit1List(param[0].ToString()).FirstOrDefault();
            vPatient patient = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", cv.MRN)).FirstOrDefault();

            cPatientName.Text = cv.PatientName;

            if (cv.GCGender == Constant.Gender.FEMALE)
            {
                cGender.Text = "WANITA";
            }
            else if (cv.GCGender == Constant.Gender.MALE)
            {
                cGender.Text = "PRIA";
            }

//            cNIK.Text = patient.OfficeStreetName;
            cDateOfBirth.Text = cv.DateOfBirthInString;

            if (cv.DateOfBirth.ToString("dd-MM-yyyy") != Constant.ConstantDate.DEFAULT_NULL)
            {
                int ageInYear = Function.GetPatientAgeInYear(cv.DateOfBirth, DateTime.Now);
                int ageInMonth = Function.GetPatientAgeInMonth(cv.DateOfBirth, DateTime.Now);
                int ageInDay = Function.GetPatientAgeInDay(cv.DateOfBirth, DateTime.Now);

                cPatientAge.Text = string.Format("{0} Tahun {1} Bulan {2} Hari", ageInYear, ageInMonth, ageInDay); ;
            }
            else
            {
                cPatientAge.Text = "";
            }
            cCorporateAccountNo.Text = cv.CorporateAccountNo;
            cMedicalNo.Text = cv.MedicalNo;
            cRegistrationNo.Text = cv.RegistrationNo;
//            cPresentNo.Text = patient.OfficeStreetName;
            cPresentNo.Text = patient.StreetName;
            cHRUnit.Text = patient.Company;
            cActualVisitDate.Text = cv.ActualVisitDate.ToString(Constant.FormatString.DATE_FORMAT);

            base.InitializeReport(param);
        }
    }
}

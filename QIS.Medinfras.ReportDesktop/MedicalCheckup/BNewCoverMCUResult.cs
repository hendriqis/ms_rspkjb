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
    public partial class BNewCoverMCUResult : BaseRpt
    {
        public BNewCoverMCUResult()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vConsultVisit cv = BusinessLayer.GetvConsultVisitList(param[0].ToString()).FirstOrDefault();
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

            vTestOrderHd toh = BusinessLayer.GetvTestOrderHdList(string.Format("VisitID = {0}", cv.VisitID)).FirstOrDefault();
            vPatientChargesHd pch = BusinessLayer.GetvPatientChargesHdList(string.Format("VisitID = {0}", cv.VisitID)).FirstOrDefault();
            if (toh.CreatedDate != null)
            {
                cActualVisitDate.Text = toh.CreatedDate.ToString(Constant.FormatString.DATE_FORMAT);
            }
            else if (pch.CreatedDate != null)
            {
                cActualVisitDate.Text = pch.CreatedDate.ToString(Constant.FormatString.DATE_FORMAT);
            }
            else 
            {
                cActualVisitDate.Text = "-";
            }

            cMedicalNo.Text = cv.MedicalNo;
            cRegistrationNo.Text = cv.RegistrationNo;
//            cPresentNo.Text = patient.OfficeStreetName;
            cPresentNo.Text = patient.StreetName;
            cHRUnit.Text = cv.BusinessPartnerName;
            

            base.InitializeReport(param);
        }
    }
}

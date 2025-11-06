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
    public partial class BCoverMCUResultGranostic : BaseRpt
    {
        public BCoverMCUResultGranostic()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vConsultVisit cv = BusinessLayer.GetvConsultVisitList(param[0].ToString()).FirstOrDefault();
            vPatient patient = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", cv.MRN)).FirstOrDefault();

            cPatientName.Text = cv.PatientName;
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

            if (cv.MRN != 0 && cv.MRN != null)
            {
                xrPictureBox1.ImageUrl = cv.PatientImageUrl;
            }
            else
            {
                xrPictureBox1.ImageUrl = cv.GuestImageUrl;
            }

            cActualVisitDate.Text = cv.ActualVisitDate.ToString(Constant.FormatString.DATE_FORMAT);
            cRegistrationNo.Text = cv.RegistrationNo;

            base.InitializeReport(param);
        }
    }
}

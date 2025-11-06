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
    public partial class BNewMCUResultRSPW : BaseCustom2DailyPotraitRpt
    {
        public BNewMCUResultRSPW()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vConsultVisit1 cv = BusinessLayer.GetvConsultVisit1List(param[0].ToString()).FirstOrDefault();
            vPatient patient = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", cv.MRN)).FirstOrDefault();

            cMedicalNo.Text = cv.MedicalNo;
            cPatientName.Text = cv.PatientName;
            cGender.Text = patient.cfGender;
            cReligion.Text = patient.Religion;
            cMaritalStatus.Text = patient.MaritalStatus;
            cEducation.Text = patient.Education;
            cOccupation.Text = patient.Occupation;
            cStreetName.Text = patient.HomeAddress;

            if (cv.DateOfBirth.ToString("dd-MM-yyyy") != Constant.ConstantDate.DEFAULT_NULL)
            {
                int ageInYear = Function.GetPatientAgeInYear(cv.DateOfBirth, DateTime.Now);
                int ageInMonth = Function.GetPatientAgeInMonth(cv.DateOfBirth, DateTime.Now);
                int ageInDay = Function.GetPatientAgeInDay(cv.DateOfBirth, DateTime.Now);

                cDOB.Text = string.Format("{0} ( {1} yr {2} mth {3} day )", cv.DateOfBirthInString, ageInYear, ageInMonth, ageInDay);
            }
            else
            {
                cDOB.Text = "";
            }

            cRegistrationDate.Text = cv.ActualVisitDate.ToString(Constant.FormatString.DATE_FORMAT);

            #region MCU Result
            subMCUResult.CanGrow = true;
            BNewMCUResultResultDt1.InitializeReport(cv.VisitID);
            #endregion

            #region Test Order
            subTestOrder.CanGrow = true;
            NewMCULaboratoryResultDtNew1.InitializeReport(cv.VisitID);
            #endregion

            #region Test Order Imaging
            subImagingResult.CanGrow = true;
            NewMCUImagingResultDtNew.InitializeReport(cv.VisitID);
            #endregion

            #region Test Order Diagnostic
            subDiagnosticResult.CanGrow = true;
            NewMCUDiagnosticResultDtNew.InitializeReport(cv.VisitID);
            #endregion

            #region MCU Result
            //subMCUResult.CanGrow = true;
            //BMCUResultResultDt1.InitializeReport(cv.VisitID);
            #endregion

            #region Footer
            vHealthcare healthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", AppSession.UserLogin.HealthcareID)).FirstOrDefault();
            cFooterInfo.Text = string.Format("{0}, {1}", healthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));
            cFooterInfo1.Text = string.Format("Dokter Pemeriksa");
            cFooterInfo2.Text = string.Format("");
            //cParamedic.Text = cv.ParamedicName;
            #endregion

            base.InitializeReport(param);
        }

    }
}

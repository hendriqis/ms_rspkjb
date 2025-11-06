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
    public partial class BNew2MCUResult : BaseCustomDailyPotraitRpt
    {
        public BNew2MCUResult()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vConsultVisit cv = BusinessLayer.GetvConsultVisitList(param[0].ToString()).FirstOrDefault();
            string streetName = "";
            if (cv.MRN != 0)
            {
                vPatient patient = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", cv.MRN)).FirstOrDefault();
                streetName = patient.StreetName;
            }
            else
	        {
                vGuest guest = BusinessLayer.GetvGuestList(string.Format("GuestID = {0}", cv.GuestID)).FirstOrDefault();
                streetName = guest.StreetName;
	        }
            
            cMedicalNo.Text = cv.MedicalNo;
            cPatientName.Text = string.Format("{0} ({1})", cv.PatientName, cv.Gender);

            if (cv.DateOfBirth.ToString("dd-MM-yyyy") != Constant.ConstantDate.DEFAULT_NULL)
            {
                int ageInYear = Function.GetPatientAgeInYear(cv.DateOfBirth, DateTime.Now);
                int ageInMonth = Function.GetPatientAgeInMonth(cv.DateOfBirth, DateTime.Now);
                int ageInDay = Function.GetPatientAgeInDay(cv.DateOfBirth, DateTime.Now);

                cDOB.Text = string.Format("{0} ( {1} yr {2} mth {3} day )", cv.DateOfBirthInString, ageInYear, ageInMonth, ageInDay);
                cAge.Text = string.Format("{0} yr {1} mth {2} day", ageInYear, ageInMonth, ageInDay);
            }
            else
            {
                cDOB.Text = "";
                cAge.Text = "";
            }

            cRegistrationNo.Text = cv.RegistrationNo;
            cRegistrationDate.Text = cv.ActualVisitDate.ToString(Constant.FormatString.DATE_FORMAT);
            cRegistrationPhysican.Text = cv.ParamedicName;
            cHRUnit.Text = cv.BusinessPartnerName;
            cNIK.Text = streetName;

            #region MCU Result
            subMCUResult.CanGrow = true;
            BMCUResultDtNew1.InitializeReport(cv.VisitID);
            #endregion

            //#region Test Order
            //subTestOrder.CanGrow = true;
            //MCULaboratoryResultDtNew1.InitializeReport(cv.VisitID);
            //#endregion

            #region Footer
            vHealthcare healthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", AppSession.UserLogin.HealthcareID)).FirstOrDefault();
            cFooterInfo.Text = string.Format("{0}, {1}", healthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));
            cFooterInfo1.Text = string.Format("Dokter Pemeriksa");
            cFooterInfo2.Text = string.Format(" ");
            cParamedic.Text = cv.ParamedicName;
            #endregion

            base.InitializeReport(param);
        }

    }
}

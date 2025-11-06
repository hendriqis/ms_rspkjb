using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BCatatanBilling : BaseCustomDailyPotraitRpt
    {
        public BCatatanBilling()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vConsultVisit cv = BusinessLayer.GetvConsultVisitList(param[0])[0];
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = {0}", appSession.HealthcareID))[0];

            #region Header : Per Page
            cHeaderPatient.Text = string.Format("{0} | {1} | {2} yr {3} mnth {4} day",
                cv.PatientName, cv.Gender, cv.AgeInYear, cv.AgeInMonth, cv.AgeInDay);
            cHeaderRegistration.Text = cv.RegistrationNo;
            cHeaderMedicalNo.Text = cv.MedicalNo;
            #endregion

            cMedicalNo.Text = cv.MedicalNo;
            cPatientName.Text = cv.PatientName;
            cRegistrationNo.Text = cv.RegistrationNo;
            cRegisteredPhysician.Text = cv.ParamedicName;
            
            base.InitializeReport(param);
        }

    }
}

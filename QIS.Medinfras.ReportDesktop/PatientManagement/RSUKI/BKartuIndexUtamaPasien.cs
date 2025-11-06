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
    public partial class BKartuIndexUtamaPasien : BaseDailyPortrait2Rpt
    {
        public BKartuIndexUtamaPasien()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();
            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", param[0])).FirstOrDefault();
            vPatient entityP = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", entity.MRN)).FirstOrDefault();
            vConsultVisit entityVisit = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0}", entity.RegistrationID)).FirstOrDefault();

            lblMedicalNo.Text = entityP.MedicalNo;
            lblPatientName.Text = entityP.PatientName;
            lblDateOfBirth.Text = entityP.DateOfBirthInString;
            lblStreetName.Text = entityP.StreetName;
            lblTelpNo.Text = entityP.cfMobilePhoneNo;
            lblOccupation.Text = entityP.Occupation;
            lblEducation.Text = entityP.Education;
            lblReligion.Text = entityP.Religion;

            lblBusinessPartnerName.Text = entityVisit.BusinessPartnerName;
            lblServiceUnitName.Text = entityVisit.ServiceUnitName;
            lblParamedicName.Text = entityVisit.ParamedicName;
            lblRegistrationDate.Text = entity.RegistrationDateInString;

            txtEmail.Text = entityHealthcare.Email;
                     
            base.InitializeReport(param);
        }
    }
}

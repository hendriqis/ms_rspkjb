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
    public partial class BSuratKeteranganSehat : BaseCustomDailyPotraitRpt
    {
        public BSuratKeteranganSehat()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            Registration entity = BusinessLayer.GetRegistrationList(param[0])[0];
            vPatient entityP = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", entity.MRN))[0];
            Healthcare entityH1 = BusinessLayer.GetHealthcare(appSession.HealthcareID);
            ConsultVisit entityCV = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", entity.RegistrationID))[0];
            ParamedicMaster entityPM = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", entityCV.ParamedicID))[0];
            Healthcare entityH2 = BusinessLayer.GetHealthcare(entityPM.HealthcareID);

            lblName.Text = entityP.PatientName;
            lblNationality.Text = entityP.Nationality;
            lblDOB.Text = entityP.CityOfBirth + " / " + entityP.DateOfBirthInString;
            lblAddress.Text = entityP.StreetName;
            lblPhoneNo.Text = entityP.PhoneNo1 + " / " + entityP.MobilePhoneNo1;
            lblEmail.Text = entityP.EmailAddress;

            lblStatement.Text = entityH1.HealthcareName;
            lblDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT);
            lblDoctorsName.Text = entityPM.FullName;
            lblClinicName.Text = entityH2.HealthcareName;
            
            base.InitializeReport(param);
        }

    }
}

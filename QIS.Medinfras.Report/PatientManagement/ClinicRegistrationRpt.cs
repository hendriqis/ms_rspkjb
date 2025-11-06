using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Report
{
    public partial class ClinicRegistrationRpt : QIS.Medinfras.Report.BaseRpt
    {
        public ClinicRegistrationRpt()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            lblReportProperties.Text = string.Format("MEDINFRAS - {0}, Print Date/Time:{1}, User ID:{2}", reportMaster.ReportCode, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"), "admin");

            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", param[0]))[0];
            lblRegistrationNo.Text = string.Format("Registration No : {0}", entity.RegistrationNo);
            lblPatientName.Text = string.Format("{0} / {1}", entity.PatientName, entity.MedicalNo);
            lblAge.Text = Helper.GetPatientAge(words, entity.DateOfBirth);
            lblServiceUnit.Text = entity.ServiceUnitName;
            lblPhysician.Text = entity.ParamedicName;

            lblAgeLabel.Text = GetLabel("Age");
            lblClinicLabel.Text = GetLabel("Clinic");
            lblOutpatientLabel.Text = GetLabel("Outpatient");
            lblPhysicianLabel.Text = GetLabel("Physician");
        }
    }
}

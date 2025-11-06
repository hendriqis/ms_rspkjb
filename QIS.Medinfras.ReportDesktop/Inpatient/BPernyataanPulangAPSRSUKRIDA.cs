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
    public partial class BPernyataanPulangAPSRSUKRIDA : BaseCustomDailyPotraitRpt
    {
        public BPernyataanPulangAPSRSUKRIDA()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vRegistration entityReg = BusinessLayer.GetvRegistrationList(param[0])[0];
            vHealthcare entity = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            string MRN = string.Format("{0}", entityReg.MRN);
            vPatient entityPat = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", MRN))[0];

            int AgeYear = entityPat.AgeInYear;
            int AgeMonth = entityPat.AgeInMonth;
            int AgeDays = entityPat.AgeInDay;

            lblNama.Text = entityPat.PatientName;
            lblUmur.Text = string.Format("{0} Tahun {1} Bulan {2} Hari", AgeYear, AgeMonth, AgeDays);
            lblGender.Text = entityPat.cfGender;
            lblTgl.Text = entityPat.DateOfBirthInString;
            lblDate.Text = string.Format("{0}, {1}", entity.City, DateTime.Now.ToString("dd-MMM-yyyy"));
                 
            base.InitializeReport(param);
        }
    }
}

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
    public partial class BPernyataanPulangAPS : BaseCustomDailyPotraitRpt
    {
        public BPernyataanPulangAPS()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vRegistration entityReg = BusinessLayer.GetvRegistrationList(param[0])[0];
            string MRN = string.Format("{0}", entityReg.MRN);
            vPatient entityPat = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", MRN))[0];

            string FirstName = entityPat.FirstName;
            string MiddleName = entityPat.MiddleName;
            string LastName = entityPat.LastName;
            string Gender = entityPat.Gender;
            int AgeYear = entityPat.AgeInYear;
            int AgeMonth = entityPat.AgeInMonth;
            int AgeDays = entityPat.AgeInDay;

            
            lblNama.Text = string.Format("{0} {1} {2}", FirstName, MiddleName, LastName);
            lblUmur.Text = string.Format("{0} Tahun {1} Bulan {2} Hari", AgeYear, AgeMonth, AgeDays);
            lblGender.Text = Gender;
            lblTgl.Text = entityPat.cfDateOfBirth;
            lblDate.Text = string.Format("{0}", DateTime.Now.ToString("dd-MMM-yyyy"));
                 
            base.InitializeReport(param);
        }
    }
}

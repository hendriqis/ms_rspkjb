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
    public partial class BRencanaPulangPasien :BaseCustomDailyPotraitRpt
    {
        public BRencanaPulangPasien()
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

            lblNoReg.Text = entityReg.RegistrationNo;
            lblNama.Text = string.Format("{0} {1} {2}", FirstName, MiddleName, LastName);
            lblTgl.Text = entityPat.cfDateOfBirth;
            lblGender.Text = entityPat.Gender;
            lblHSU.Text = entityReg.ServiceUnitName;

            base.InitializeReport(param);
        }
    }
}

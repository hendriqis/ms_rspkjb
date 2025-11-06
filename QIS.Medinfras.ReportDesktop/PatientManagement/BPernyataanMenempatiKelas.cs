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
    public partial class BPernyataanMenempatiKelas : BaseCustomDailyPotraitRpt
    {
        public BPernyataanMenempatiKelas()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vRegistration entityReg = BusinessLayer.GetvRegistrationList(param[0])[0];
            string MRN = string.Format("{0}", entityReg.MRN);
            vPatient entityPat = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", MRN))[0];
            string RegistrationID = string.Format("{0}", entityReg.RegistrationID);
            vConsultVisit6 entitycv = BusinessLayer.GetvConsultVisit6List(string.Format("RegistrationID = {0}", RegistrationID))[0];


            string Classid = string.Format("{0}", entitycv.RequestClassID);
         

            string FirstName = entityPat.FirstName;
            string MiddleName = entityPat.MiddleName;
            string LastName = entityPat.LastName;
            string Gender = entityPat.Gender;
            string ClassName = entitycv.ClassName;
            int AgeYear = entityPat.AgeInYear;
            int AgeMonth = entityPat.AgeInMonth;
            int AgeDays = entityPat.AgeInDay;


            lblNama.Text = string.Format("{0} {1} {2}", FirstName, MiddleName, LastName);
            lblUmur.Text = string.Format("{0} Tahun {1} Bulan {2} Hari", AgeYear, AgeMonth, AgeDays);
            lblGender.Text = Gender;
            lblTgl.Text = entityPat.cfDateOfBirth;
            lblDate.Text = string.Format("{0}", DateTime.Now.ToString("dd-MMM-yyyy"));
            lblkelas.Text = ClassName;

            if (entitycv.Class != null)
            {
                lblrequest.Text = "-";
            }
            else
            {
                lblrequest.Text = entitycv.Class;
            

            }
        

            base.InitializeReport(param);
        }

    }
}

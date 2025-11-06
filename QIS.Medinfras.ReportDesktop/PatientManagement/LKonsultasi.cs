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
    public partial class LKonsultasi : BaseA6Rpt
    {
        public LKonsultasi()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();
            vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", param[0])).FirstOrDefault();

            string gender = "";
            if (entityReg.GCGender == Constant.Gender.MALE)
            {
                gender = "(L)";
            }
            else
            {
                gender = "(P)";
            }
            lblGender.Text = gender;
            lblNama.Text = entityReg.PatientName;
            cParamedicName.Text = entityReg.ReferrerParamedicName;
            lblAlamat.Text = string.Format("{0} {1}",entityReg.StreetName, entityReg.City);
            lblUmur.Text = string.Format("{0}Tahun {1}Bulan {2}Hari", entityReg.AgeInYear, entityReg.AgeInMonth, entityReg.AgeInDay);
            lblDiagnosa.Text = string.Format("{0} {1}", entityReg.DiagnoseName, entityReg.DiagnosisText);
            lblDate.Text = string.Format("{0}, {1}", entityHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));
            lblParamedic.Text = entityReg.ParamedicName;
            lblRM.Text = entityReg.MedicalNo;
            lblDiagnosa.Text = string.Format ("{0} {1}",entityReg.DiagnoseName, entityReg.DiagnosisText);
            base.InitializeReport(param);
        }

    }
}

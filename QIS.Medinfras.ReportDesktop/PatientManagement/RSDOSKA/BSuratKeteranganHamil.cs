using System;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BSuratKeteranganHamil: BaseDailyPortraitRpt
    {
        public BSuratKeteranganHamil()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            

            Registration entity = BusinessLayer.GetRegistrationList(string.Format(param[0]))[0];
            vPatient entityP = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", entity.MRN))[0];
            vHealthcare entityH1 = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", appSession.HealthcareID))[0];
            ConsultVisit entityCV = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", entity.RegistrationID))[0];
            ParamedicMaster entityPM = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", entityCV.ParamedicID))[0];

            lblName.Text = entityP.PatientName;
            lblDOB.Text = String.Format("{0} Umur {1} Tahun", entityP.DateOfBirthInString, entityP.AgeInYear);
            //lblGender.Text = entityP.Gender;
            lblOccupation.Text = entityP.Occupation;
            lblAddress.Text = entityP.StreetName;

           // String days = GetDateAbsent(DateTime.Parse(param[1]).Date, DateTime.Parse(param[2]).Date);

            String Kandungan = param[1];
            String Tanggal = param [2];
           // String endDate = param[4];
            txtKandungan.Text = string.Format("{0}", Kandungan);
            lblDate.Text = string.Format("{0}",Tanggal);

  //          lblPrintDate.Text = string.Format("{0}, {1}", entityH1.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));
            //lblParamedic.Text = entityPM.FullName;
            base.InitializeReport(param);
        }
    }
}

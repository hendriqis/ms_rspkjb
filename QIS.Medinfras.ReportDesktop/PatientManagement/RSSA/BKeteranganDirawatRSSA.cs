using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Linq;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BKeteranganDirawatRSSA : BaseCustomDailyPotraitRpt
    {
        public BKeteranganDirawatRSSA()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            Registration entity = BusinessLayer.GetRegistration(Convert.ToInt32(param[0]));
            vPatient entityP = BusinessLayer.GetvPatientList(String.Format("MRN = {0}", entity.MRN)).FirstOrDefault();
            ConsultVisit entityCV = BusinessLayer.GetConsultVisitList(String.Format("RegistrationID = {0}",entity.RegistrationID)).FirstOrDefault();
            ParamedicMaster entityPM = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", entityCV.ParamedicID)).FirstOrDefault();
            vHealthcare entityH2 = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID)).FirstOrDefault();

            lblName.Text = entityP.PatientName;
            lblDOB.Text = string.Format("{0} Thn999 / {1}", entityP.AgeInYear, entityP.cfGender);
            lblAddress.Text = entityP.StreetName + " " + entityP.County + " " + entityP.District + " " + entityP.City;
            lblPrintDate.Text = string.Format("{0} , {1}", entityH2.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));
            lblSub1.Text = string.Format("Yang bertanda tangan dibawah ini, Dokter {0}, menerangkan bahwa : ", entityH2.HealthcareName);
            lblPekerjaan.Text = entityP.Occupation;

            string[] tempDate = param[1].Split(';');
            String startDate = param[3];
            String endDate = param[4];
            if (endDate == null || endDate == "")
            {
                lblTgl.Text = string.Format("Penderita tersebut telah / sedang dirawat di {0} , sejak tanggal {1} s/d ", entityH2.HealthcareName, startDate);
            }
            else
            {
                lblTgl.Text = string.Format("Penderita tersebut telah / sedang dirawat di {0} , sejak tanggal {1} s/d {2}.", entityH2.HealthcareName, startDate, endDate);
            }

            base.InitializeReport(param);
        }
    }
}

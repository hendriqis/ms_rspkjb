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
    public partial class BKeteranganDirawat1 : BaseCustomDailyPotraitRpt
    {
        public BKeteranganDirawat1()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format ("RegistrationID ={0}", param[0])).FirstOrDefault();
            vPatient entityP = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", entity.MRN))[0];
            vHealthcare entityH1 = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            vConsultVisit entityCV = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0}", entity.RegistrationID))[0];
            ParamedicMaster entityPM = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", entityCV.ParamedicID))[0];
            Healthcare entityH2 = BusinessLayer.GetHealthcare(entityPM.HealthcareID);
            vChiefComplaint entityCC = BusinessLayer.GetvChiefComplaintList(string.Format("VisitID = {0}", entityCV.VisitID)).FirstOrDefault();

            lblName.Text = entityP.PatientName;
            lblDOB.Text = entityP.DateOfBirthInString;
            lblGender.Text = entityCV.cfGenderInitial2;
            lblAddress.Text = entityP.StreetName;
            lblPrintDate.Text = string.Format("{0}, {1}", entityH1.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));
            lblParamedic.Text = entityPM.FullName;
            lblTgl.Text = string.Format("{0} mulai tanggal {1} s/d tanggal {2}", entityH2.HealthcareName, entity.RegistrationDateInString, entityCV.DischargeDateInString);
            lblSub1.Text = string.Format("Yang bertanda tangan dibawah ini, Dokter {0}, menerangkan bahwa : ", entityH2.HealthcareName);

            if (entityCC == null)
                lblSub2.Text = string.Format("Dirawat di {0} karena {1}", entityH2.HealthcareName, "..........");
            else
                lblSub2.Text = string.Format("Dirawat di {0} karena {1}", entityH2.HealthcareName, entityCC.ChiefComplaintText);

            lblPekerjaan.Text = entityP.Occupation;

           
            base.InitializeReport(param);

        }
    }
}

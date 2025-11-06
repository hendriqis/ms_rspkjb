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
    public partial class BKeteranganDirawat : BaseCustomDailyPotraitRpt
    {
        public BKeteranganDirawat()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
         

            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID ={0}", param[0]))[0];
            vPatient entityP = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", entity.MRN))[0];
            vHealthcare entityH1 = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            vConsultVisit entityCV = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0}", entity.RegistrationID))[0];
            ParamedicMaster entityPM = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", entityCV.ParamedicID))[0];
            Healthcare entityH2 = BusinessLayer.GetHealthcare(entityPM.HealthcareID);

            lblName.Text = entityP.PatientName;
            lblDOB.Text = string.Format ("{0} / {1}Tahun", entityP.DateOfBirthInString, entity.AgeInYear);
            lblAddress.Text = entityP.StreetName;
            lblPrintDate.Text = string.Format("{0}, {1}", entityH1.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));
            lblParamedic.Text = entityPM.FullName;
            lblTgl.Text = string.Format("Betul-betul di rawat di {0} mulai tanggal {1}", entityH2.HealthcareName, entity.RegistrationDateInString);
            lblGender.Text = entityP.Gender;

           string[] tempDate = param[1].Split(';');
           // lblDate.Text = string.Format("{0} s/d {1}", Helper.YYYYMMDDToDate(tempDate[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(tempDate[1]).ToString(Constant.FormatString.DATE_FORMAT));
            String startDate = param[3];
            String endDate = param[4];
            lblDate.Text = string.Format("dan masih perlu istirahat di rumah selama terhitung mulai tanggal {0} s/d {1}.", startDate, endDate);
            base.InitializeReport(param);

        }
    }
}

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
    public partial class BPerintahRIRSUKRIDA : BaseA6Rpt
    {

        public BPerintahRIRSUKRIDA()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", param[0])).FirstOrDefault();
            vConsultVisit entityCV = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0}", entity.RegistrationID)).FirstOrDefault();
            vPatientDiagnosis entityPD = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND GCDiagnoseType = '{1}' AND IsDeleted = 0", entityCV.VisitID, Constant.DiagnoseType.MAIN_DIAGNOSIS)).FirstOrDefault();

            if (entityPD != null)
            {
                if (!String.IsNullOrEmpty(entityPD.DiagnosisText))
                {
                    lblDiagnosa.Text = string.Format("{0}", entityPD.DiagnosisText);
                }
                else
                {
                    lblDiagnosa.Text = string.Format("{0}", entityPD.DiagnoseName);
                }
            }
            else
            {
                lblDiagnosa.Text = "";
            }

            lblParamedicName.Text = entity.ParamedicName;
            lblAge.Text =string.Format ("/ {0} tahun", entity.AgeInYear);

            lblTglLahir.Text =  entity.cfDateOfBirth;

            lblParamedicVisit.Text = entity.ParamedicName;
            lblLastUpdatedDate.Text = string.Format("{0}, {1}", entityHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));
            lblGender.Text = string.Format("({0})", entity.cfGenderInitial);

            base.InitializeReport(param);
        }
        
    }
}

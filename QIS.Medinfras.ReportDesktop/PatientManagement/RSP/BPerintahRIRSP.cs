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
    public partial class BPerintahRIRSP : BaseA6Rpt
    {

        public BPerintahRIRSP()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", param[0])).FirstOrDefault();
            ConsultVisit cv = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = '{0}'", entity.RegistrationID)).FirstOrDefault();
            vPatientDiagnosis pdiagnos = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID={0} AND GCDiagnoseType='{1}' AND IsDeleted=0 ORDER BY ID DESC ", cv.VisitID, Constant.DiagnoseType.MAIN_DIAGNOSIS)).FirstOrDefault();
            
            lblDiagnosa.Text = "";
            if (pdiagnos != null) {
                if (!String.IsNullOrEmpty(pdiagnos.DiagnoseID))
                {
                    lblDiagnosa.Text = string.Format("{0}", pdiagnos.DiagnoseName);
                }
                else
                {
                    lblDiagnosa.Text = string.Format("{0}", pdiagnos.DiagnosisText);
                }
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

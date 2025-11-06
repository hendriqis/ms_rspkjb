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
    public partial class BSuratPengantarRawatInapIGD_RSRT : BaseA6Rpt
    {

        public BSuratPengantarRawatInapIGD_RSRT()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", param[0])).FirstOrDefault();

            if(entity.DiagnoseID == null && entity.DiagnoseID.ToString() == "")
                lblDiagnosa.Text = string.Format("{0}" , entity.DiagnosisText);
            else
                lblDiagnosa.Text = string.Format("{0}", entity.DiagnoseName);

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

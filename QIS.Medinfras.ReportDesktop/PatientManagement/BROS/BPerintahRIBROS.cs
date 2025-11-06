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
    public partial class BPerintahRIBROS : BaseA6Rpt
    {

        public BPerintahRIBROS()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", param[0])).FirstOrDefault();
            ConsultVisit entityVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", entity.RegistrationID)).FirstOrDefault();
            PatientDiagnosis entityDiagnose = BusinessLayer.GetPatientDiagnosisList(string.Format("VisitID = {0} AND GCDiagnoseType = '{1}'", entityVisit.VisitID, Constant.DiagnoseType.EARLY_DIAGNOSIS)).FirstOrDefault();

            if (entityDiagnose != null)
            {
                if (!String.IsNullOrEmpty(entity.DiagnoseID))
                {
                    lblDiagnosa.Text = string.Format("{0}", entity.DiagnoseName);
                }
                else
                {
                    lblDiagnosa.Text = string.Format("{0}", entity.DiagnosisText);
                }
            }

            lblIndikasi.Text = entityVisit.HospitalizationIndication;

            lblRoomType.Text = entity.RoomType;

            lblParamedicName.Text = entity.ParamedicName;
            lblAge.Text =string.Format ("/ {0} tahun", entity.AgeInYear);

            lblTglLahir.Text =  entity.cfDateOfBirth;

            string textJenisPelayanan = string.Empty;
            if (entityVisit.IsPreventiveCare)
            {
                textJenisPelayanan += "Preventif,";
            }
            if (entityVisit.IsCurativeCare)
            {
                textJenisPelayanan += "Kuratif,";
            }
            if (entityVisit.IsRehabilitationCare)
            {
                textJenisPelayanan += "Rehabilitatif,";
            }
            if (entityVisit.IsPalliativeCare)
            {
                textJenisPelayanan += "Paliatif,";
            }

            if (!string.IsNullOrEmpty(textJenisPelayanan))
            {
                textJenisPelayanan = textJenisPelayanan.Substring(0, textJenisPelayanan.Length - 1);
            }

            lblJenisPelayanan.Text = textJenisPelayanan;

            lblParamedicVisit.Text = entity.ParamedicName;
            lblLastUpdatedDate.Text = string.Format("{0}, {1}", entityHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));
            lblGender.Text = string.Format("({0})", entity.cfGenderInitial);

            ttdDokter.ImageUrl = string.Format("{0}{1}/Signature/{2}.png", AppConfigManager.QISPhysicalDirectory, AppConfigManager.QISParamedicImagePath, entity.ParamedicCode);
            ttdDokter.Visible = true;

            base.InitializeReport(param);
        }
        
    }
}

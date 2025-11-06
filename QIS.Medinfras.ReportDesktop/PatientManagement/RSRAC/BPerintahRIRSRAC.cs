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
    public partial class BPerintahRIRSRAC : BaseA6Rpt
    {
        public BPerintahRIRSRAC()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", param[0])).FirstOrDefault();
            vConsultVisit entityCV = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0}", entity.RegistrationID)).FirstOrDefault();
            ConsultVisit entityC = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", entity.RegistrationID)).FirstOrDefault();
            vPatientDiagnosis entityPD = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND GCDiagnoseType = '{1}' AND IsDeleted = 0", entityCV.VisitID, Constant.DiagnoseType.MAIN_DIAGNOSIS)).FirstOrDefault();
            ParamedicMaster entityPM = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}",entityCV.ReferralPhysicianID)).FirstOrDefault();
            
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

            if (entityPM != null)
            {
                lblParamedicName.Text = entityPM.FullName;
            }
            else 
            {
                lblParamedicName.Text = "";
            }
            lblAge.Text =string.Format ("/ {0} tahun", entity.AgeInYear);

            lblTglLahir.Text =  entity.cfDateOfBirth;
            lblKeterangan.Text = entityC.HospitalizationIndication;
            lblParamedicVisit.Text = entity.ParamedicName;
            lblLastUpdatedDate.Text = string.Format("{0}, {1}", entityHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));
            lblGender.Text = string.Format("({0})", entity.cfGenderInitial);
            if (entityC.GCRoomType != null)
            {
                if (entityC.GCRoomType == Constant.RoomType.ICU)
                {
                    xrICU.Checked = true;
                }
                else if (entityC.GCRoomType == Constant.RoomType.NICUPICU)
                {
                    xrPICU.Checked = true;
                }
                else if (entityC.GCRoomType == Constant.RoomType.VK)
                {
                    xrVK.Checked = true;
                }
                else if (entityC.GCRoomType == Constant.RoomType.ISOLASI)
                {
                    xrISOLASI.Checked = true;
                }
                else if (entityC.GCRoomType == Constant.RoomType.Ruangan)
                {
                    xrRuang.Checked = true;
                }
                else 
                {
                    xrICU.Checked = false;
                    xrPICU.Checked = false;
                    xrVK.Checked = false;
                    xrISOLASI.Checked = false;
                    xrRuang.Checked = false;
                }
            }
            if (entityPM != null)
            {
                ttdDokter.ImageUrl = string.Format("{0}{1}/Signature/{2}.png", AppConfigManager.QISPhysicalDirectory, AppConfigManager.QISParamedicImagePath, entityPM.ParamedicCode);
                ttdDokter.Visible = true;
            }
            else
            {
                ttdDokter.Visible = false;
            }
            base.InitializeReport(param);
        }
        
    }
}

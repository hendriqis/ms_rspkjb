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
    public partial class BPerintahRIRSPW : BaseA6Rpt
    {

        public BPerintahRIRSPW()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", param[0])).FirstOrDefault();

            if (!String.IsNullOrEmpty(entity.DiagnoseID))
            {
                lblDiagnosa.Text = string.Format("{0}", entity.DiagnoseName);
            }
            else
            {
                lblDiagnosa.Text = string.Format("{0}", entity.DiagnosisText);
            }

            lblAge.Text =string.Format ("/ {0} tahun", entity.AgeInYear);

            lblTglLahir.Text =  entity.cfDateOfBirth;

            lblLastUpdatedDate.Text = string.Format("{0}, {1}", entityHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));
            lblGender.Text = string.Format("({0})", entity.cfGenderInitial);

            if (entity.DepartmentID == Constant.Facility.INPATIENT)
            {
                lblParamedicName.Text = entity.ParamedicName;
                lblRoomName.Text = string.Format("{0} | {1}", entity.ServiceUnitName, entity.RoomName);
                lblBusinessPartners.Text = entity.BusinessPartnerName;
                if (entity.LinkedRegistrationID == null || entity.LinkedRegistrationID == 0)
                {
                    lblParamedicVisit.Text = entity.ParamedicName;
                    ttdDokter.ImageUrl = string.Format("{0}{1}/Signature/{2}.png", AppConfigManager.QISPhysicalDirectory, AppConfigManager.QISParamedicImagePath, entity.ParamedicCode);
                    ttdDokter.Visible = true;
                }
                else
                {
                    vRegistration entityLinkedFrom = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", entity.LinkedRegistrationID)).FirstOrDefault();
                    lblParamedicVisit.Text = entityLinkedFrom.ParamedicName;
                    ttdDokter.ImageUrl = string.Format("{0}{1}/Signature/{2}.png", AppConfigManager.QISPhysicalDirectory, AppConfigManager.QISParamedicImagePath, entityLinkedFrom.ParamedicCode);
                    ttdDokter.Visible = true;
                }
            }
            else
            {
                lblParamedicVisit.Text = entity.ParamedicName;
                ttdDokter.ImageUrl = string.Format("{0}{1}/Signature/{2}.png", AppConfigManager.QISPhysicalDirectory, AppConfigManager.QISParamedicImagePath, entity.ParamedicCode);
                ttdDokter.Visible = true;
                if (entity.LinkedToRegistrationID == null || entity.LinkedToRegistrationID == 0)
                {
                    lblParamedicName.Text = entity.ParamedicName;
                    lblRoomName.Text = entity.ServiceUnitName;
                    lblBusinessPartners.Text = entity.BusinessPartnerName;
                }
                else
                {
                    vRegistration entityLinkedTo = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", entity.LinkedToRegistrationID)).FirstOrDefault();
                    lblParamedicName.Text = entityLinkedTo.ParamedicName;
                    lblRoomName.Text = string.Format("{0} | {1}", entityLinkedTo.ServiceUnitName, entityLinkedTo.RoomName);
                    lblBusinessPartners.Text = entityLinkedTo.BusinessPartnerName;
                }
            }

            base.InitializeReport(param);
        }
        
    }
}

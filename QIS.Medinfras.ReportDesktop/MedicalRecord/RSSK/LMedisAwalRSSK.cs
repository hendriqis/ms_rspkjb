using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using ThoughtWorks.QRCode.Codec;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LMedisAwalRSSK : BaseDailyPortraitRpt
    {
        public LMedisAwalRSSK()
        {
           
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            #region Header 1 : Healthcare
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            vMedicalResumeEarlyRSSBB oMedicalResume = BusinessLayer.GetvMedicalResumeEarlyRSSBBList(string.Format("RegistrationID = '{0}'", param[0])).FirstOrDefault();
            vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = '{0}'", param[0])).FirstOrDefault();

            if (oMedicalResume.LinkedRegistrationID == 0 || oMedicalResume.LinkedRegistrationID == null)
            {
                lblDari.Visible = false;
                xrLabel10.Visible = false;
                xrLabel11.Visible = false;
            }
            else
            {
                if (oMedicalResume.DepartmentID == Constant.Facility.INPATIENT)
                {

                }
            }

            if (oHealthcare != null)
            {
                lblCity.Text = string.Format("{0}, {1}", oHealthcare.City, oMedicalResume.ActualVisitDate.ToString(Constant.FormatString.DATE_FORMAT));
            }

            lblDokterPemeriksa.Text = oMedicalResume.ParamedicName;

            if (oMedicalResume.DepartmentID == Constant.Facility.INPATIENT)
            {
                lblDokterPemeriksa.Text = oMedicalResume.ParamedicBefore;
                lblDokterMerawat.Text = oMedicalResume.ParamedicName;
            }
            else
            {
                if (!string.IsNullOrEmpty(oMedicalResume.ParamedicReferral))
                {
                    lblDokterMerawat.Text = oMedicalResume.ParamedicReferral;
                }
                else
                {
                    lblDokterMerawat.Text = "-";
                }
            }

            if (oMedicalResume.DepartmentID == Constant.Facility.EMERGENCY)
            {
                if (!string.IsNullOrEmpty(entityReg.GCTriage))
                {
                    lblTriage.Text = entityReg.Triage;
                }
                else
                {
                    xrLabel4.Visible = false;
                    xrLabel2.Visible = false;
                    lblTriage.Visible = false;
                }
            }
            else
            {
                xrLabel4.Visible = false;
                xrLabel2.Visible = false;
                lblTriage.Visible = false;
            }

            lblChiefComplaint.Text = oMedicalResume.ChiefComplaint;
            lblRPD.Text = oMedicalResume.RPD;
            lblROS.Text = oMedicalResume.ROSystem;
            lblDiagnosaUtama.Text = oMedicalResume.cfDiagnosaUtama;
            lblDiagnosaMasuk.Text = oMedicalResume.cfDiagnosaMasuk;
            lblDiagnosaSekunder.Text = oMedicalResume.cfDiagnosaSekunder;
            lblDiagnosaSebabLuar.Text = oMedicalResume.cfDiagnosaSebabLuar;
            lblDiagnosaBanding.Text = oMedicalResume.cfDiagnosaBanding;
            lblTreatment.Text = oMedicalResume.Treatment;
            lblIndication.Text = oMedicalResume.Indication;
            lblDokterPemeriksa2.Text = oMedicalResume.ParamedicName;
            lblConscious.Text = oMedicalResume.Conscious;
            lblNBPs.Text = string.Format("{0} / {1}", oMedicalResume.NBPs, oMedicalResume.NBPd);
            lblTemp.Text = oMedicalResume.Temp;
            lblHRPulse.Text = oMedicalResume.HRPulse;
            lblRespiration.Text = oMedicalResume.Respiration;
            lblPainScale.Text = string.Format("{0}{1}", oMedicalResume.PainScale, oMedicalResume.PainIndex);
            lblLab.Text = oMedicalResume.cfLabResult2;
            lblRad.Text = oMedicalResume.cfRadResult;

            #endregion
            
            base.InitializeReport(param);
        }
    }
}
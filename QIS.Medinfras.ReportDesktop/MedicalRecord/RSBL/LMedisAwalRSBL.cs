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
    public partial class LMedisAwalRSBL : BaseDailyPortraitRpt
    {
        public LMedisAwalRSBL()
        {
           
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            #region Header 1 : Healthcare
            string paramedicName = "";
            string licenseNo = "";
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            vMedicalResumeEarlyRSBL oMedicalResume = BusinessLayer.GetvMedicalResumeEarlyRSBLList(string.Format("RegistrationID = '{0}'", param[0])).FirstOrDefault();
            if (oHealthcare != null)
            {
                lblCity.Text = oHealthcare.City;
            }

            if (oMedicalResume.LinkedRegistrationID != 0)
            {
                ParamedicMaster entityParamedic = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = '{0}'", oMedicalResume.ParamedicIDBefore))[0];
                paramedicName = entityParamedic.FullName;
                licenseNo = entityParamedic.LicenseNo;
                lblDokterPemeriksa.Text = oMedicalResume.ParamedicBefore;
                lblDokterMerawat.Text = oMedicalResume.ParamedicName;
                lblChiefComplaint.Text = oMedicalResume.ChiefComplaintBefore;
                lblRPS.Text = oMedicalResume.RPSBefore;
                lblRPD.Text = oMedicalResume.RPDBefore;
                lblROS.Text = oMedicalResume.ROSystemBefore;
                lblDiagnosaUtama.Text = oMedicalResume.cfDiagnosaUtamaBefore;
                lblDiagnosaMasuk.Text = oMedicalResume.cfDiagnosaMasukBefore;
                lblDiagnosaSekunder.Text = oMedicalResume.cfDiagnosaSekunderBefore;
                lblDiagnosaSebabLuar.Text = oMedicalResume.cfDiagnosaSebabLuarBefore;
                lblDiagnosaBanding.Text = oMedicalResume.cfDiagnosaBandingBefore;
                lblTreatment.Text = oMedicalResume.TreatmentBefore;
                lblIndication.Text = oMedicalResume.IndicationBefore;
                lblDokterPemeriksa2.Text = oMedicalResume.ParamedicBefore;
                lblConscious.Text = oMedicalResume.ConsciousBefore;
                lblNBPs.Text = string.Format("{0} / {1}", oMedicalResume.NBPsBefore, oMedicalResume.NBPdBefore);
                lblTemp.Text = oMedicalResume.TempBefore;
                lblHRPulse.Text = oMedicalResume.HRPulseBefore;
                lblRespiration.Text = oMedicalResume.RespirationBefore;
                lblPainScale.Text = string.Format("{0}{1}", oMedicalResume.PainScaleBefore, oMedicalResume.PainIndexBefore);
                lblLab.Text = oMedicalResume.cfLabResultBefore2;
                lblRad.Text = oMedicalResume.cfRadResultBefore;
            }
            else
            {
                ParamedicMaster entityParamedic = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = '{0}'", oMedicalResume.ParamedicID))[0];
                paramedicName = entityParamedic.FullName;
                licenseNo = entityParamedic.LicenseNo;
                lblDokterPemeriksa.Text = oMedicalResume.ParamedicName;
                lblDokterMerawat.Text = "";
                lblChiefComplaint.Text = oMedicalResume.ChiefComplaint;
                lblRPS.Text = oMedicalResume.RPS;
                lblRPD.Text = oMedicalResume.RPD;
                lblROS.Text = oMedicalResume.ROSystem;
                lblDiagnosaUtama.Text = oMedicalResume.cfDiagnosaUtama;
                lblDiagnosaMasuk.Text = oMedicalResume.cfDiagnosaMasuk;
                lblDiagnosaSekunder.Text = oMedicalResume.cfDiagnosaSekunder;
                lblDiagnosaSebabLuar.Text = oMedicalResume.cfDiagnosaSebabLuar;
                lblDiagnosaBanding.Text = oMedicalResume.cfDiagnosaBanding;
                lblTreatment.Text = oMedicalResume.Treatment;
                lblIndication.Text = "";
                lblDokterPemeriksa2.Text = oMedicalResume.ParamedicName;
                lblConscious.Text = oMedicalResume.Conscious;
                lblNBPs.Text = string.Format("{0} / {1}", oMedicalResume.NBPs, oMedicalResume.NBPd);
                lblTemp.Text = oMedicalResume.Temp;
                lblHRPulse.Text = oMedicalResume.HRPulse;
                lblRespiration.Text = oMedicalResume.Respiration;
                lblPainScale.Text = string.Format("{0}{1}", oMedicalResume.PainScale, oMedicalResume.PainIndex);
                lblLab.Text = oMedicalResume.cfLabResult2;
                lblRad.Text = oMedicalResume.cfRadResult;
            }

            #endregion

            #region QR Codes Image
            string contents = string.Format(@"{0}\r\n{1}",
                paramedicName, licenseNo);

            QRCodeEncoder qRCodeEncoder = new QRCodeEncoder();
            qRCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qRCodeEncoder.QRCodeScale = 4;
            qRCodeEncoder.QRCodeVersion = 0;
            qRCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
            MemoryStream memoryStream = new MemoryStream();

            using (Bitmap bitMap = qRCodeEncoder.Encode(contents, System.Text.Encoding.UTF8))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    ttdDokter.Image = System.Drawing.Image.FromStream(ms, true, true);
                }
            }
            #endregion
            
            base.InitializeReport(param);
        }
    }
}
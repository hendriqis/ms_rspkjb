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
    public partial class BResumeMedisOld : BaseRpt
    {
        public BResumeMedisOld()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            #region Header 1 : Healthcare
            //vMedicalResume entity = BusinessLayer.GetvMedicalResumeList(param[0])[0];
            //vConsultVisit entityCV = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", entity.VisitID))[0];

            vConsultVisit entityCV = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", param[0])).FirstOrDefault();
            vMedicalResume entity = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID)).FirstOrDefault();
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID)).FirstOrDefault();

            if (oHealthcare != null)
            {
                xrLogo.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/logo.png");
                cHealthcareName.Text = oHealthcare.HealthcareName;
                cHealthcareAddress.Text = oHealthcare.StreetName;
                cHealthcareCityZipCodes.Text = string.Format("{0} {1}", oHealthcare.City, oHealthcare.ZipCode);
                cHealthcarePhone.Text = oHealthcare.PhoneNo1;
                cHealthcareFax.Text = oHealthcare.FaxNo1;
            }
            #endregion

            #region Header 2 : Patient Page
            cRegistrationNo.Text = entityCV.RegistrationNo;
            cMedicalNo.Text = entityCV.MedicalNo;
            cPatientName.Text = entityCV.cfPatientNameSalutation;
            cDateOfBirth.Text = entityCV.DateOfBirthInString;
            cVisitDate.Text = string.Format("{0}, {1}", entityCV.ActualVisitDateInString, entityCV.ActualVisitTime);
            cDischargeDate.Text = string.Format("{0}, {1}", entityCV.DischargeDateInString, entityCV.DischargeTime);
            #endregion

            #region Ringkasan Riwayat Penyakit
            List<vMedicalResume> lstMedicalResume = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

            if (lstMedicalResume.Count() > 0)
            {
                subSummary.CanGrow = true;
                mrSummary.InitializeReport(entityCV.VisitID, entity.ID);
            }
            else
            {
                subSummary.Visible = false;
            }
            #endregion

            #region Pemeriksaan Fisik
            if (entity != null)
            {
                List<vReviewOfSystemHd> lstHdReviewOfSystem = BusinessLayer.GetvReviewOfSystemHdList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

                if (lstHdReviewOfSystem.Count() > 0)
                {
                    subReviewOfSystem.CanGrow = true;
                    mrReviewOfSystem.InitializeReport(entityCV.VisitID, entity.ID);
                }
                else
                {
                    subReviewOfSystem.Visible = false;
                }
            }
            else
            {
                subReviewOfSystem.Visible = false;
            }
            #endregion
            
            #region Tanda Vital dan Indikator Lainnya
            if (entity != null)
            {
                List<vVitalSignHd> lstHdVitalSign = BusinessLayer.GetvVitalSignHdList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

                if (lstHdVitalSign.Count() > 0)
                {
                    subVitalSign.CanGrow = true;
                    mrVitalSignNew.InitializeReport(entityCV.VisitID, entity.ID);
                }
                else
                {
                    subVitalSign.Visible = false;
                }
            }
            else
            {
                subVitalSign.Visible = false;
            }
            #endregion

            #region Pemeriksaan Penunjang
            List<vMedicalResume> lstMedicalResume1 = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

            if (lstMedicalResume1.Count() > 0)
            {
                subDiagnosticExam.CanGrow = true;
                mrDiagnosticExamNew.InitializeReport(entityCV.VisitID, entity.ID);
            }
            else
            {
                subDiagnosticExam.Visible = false;
            }
            #endregion

            #region Ringkasan Terapi Pengobatan
            List<vMedicalResume> lstMedicalResume2 = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

            if (lstMedicalResume2.Count() > 0)
            {
                subTerapiPengobatan.CanGrow = true;
                mrTherapyPengobatan.InitializeReport(entityCV.VisitID, entity.ID);
            }
            else
            {
                subTerapiPengobatan.Visible = false;
            }
            #endregion

            #region Perkembangan Selama Perawatan
            List<vMedicalResume> lstMedicalResume3 = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

            if (lstMedicalResume3.Count() > 0)
            {
                subTreatment.CanGrow = true;
                mrTreatment.InitializeReport(entityCV.VisitID, entity.ID);
            }
            else
            {
                subTreatment.Visible = false;
            }
            #endregion

            #region Diagnosa
            if (entity != null)
            {
                List<vPatientDiagnosis> lstPatientDiagnosis = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

                if (lstPatientDiagnosis.Count() > 0)
                {
                    subDiagnose.CanGrow = true;
                    mrDiagnoseInformation.InitializeReport(entityCV.VisitID, entity.ID);
                }
                else
                {
                    subDiagnose.Visible = false;
                }
            }
            else
            {
                subDiagnose.Visible = false;
            }
            #endregion

            #region Tindakan dan Operasi
            List<vMedicalResume> lstMedicalResume4 = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

            if (lstMedicalResume4.Count() > 0)
            {
                subSurgery.CanGrow = true;
                mrSurgery.InitializeReport(entityCV.VisitID, entity.ID);
            }
            else
            {
                subSurgery.Visible = false;
            }
            #endregion

            #region Rencana Tindak Lanjut
            List<vMedicalResume> lstMedicalResume5 = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

            if (lstMedicalResume5.Count() > 0)
            {
                subInstruction.CanGrow = true;
                mrInstruction.InitializeReport(entityCV.VisitID, entity.ID);
            }
            else
            {
                subInstruction.Visible = false;
            }
           #endregion

            #region Footer
            lblTTDTanggal.Text = string.Format("{0}, {1}", oHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));
            ttdDokter.ImageUrl = string.Format("{0}{1}/Signature/{2}.png", AppConfigManager.QISPhysicalDirectory, AppConfigManager.QISParamedicImagePath, entityCV.ParamedicCode);
            ttdDokter.Visible = true;
            lblSignParamedicName.Text = entityCV.ParamedicName;

            lblReportProperties.Text = string.Format("MEDINFRAS - {0}, Print Date/Time:{1}, User ID:{2}", reportMaster.ReportCode, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"), appSession.UserName);
            #endregion
        }
    }
}

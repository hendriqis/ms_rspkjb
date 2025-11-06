using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.IO;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using ThoughtWorks.QRCode.Codec;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BResumeMedisNewRSDOSKA : BaseRpt
    {
        public BResumeMedisNewRSDOSKA()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            #region Header 1 : Healthcare
            //vMedicalResume entity = BusinessLayer.GetvMedicalResumeList(param[0])[0];
            //vConsultVisit entityCV = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", entity.VisitID))[0];

            vConsultVisit entityCV = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", param[0])).FirstOrDefault();
            Registration entityReg = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0}", entityCV.RegistrationID)).FirstOrDefault();
            vMedicalResume entity = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID)).FirstOrDefault();
            Patient entityPatient = BusinessLayer.GetPatientList(string.Format("MRN = {0}", entityCV.MRN)).FirstOrDefault();
            BusinessPartners entityBP = BusinessLayer.GetBusinessPartnersList(string.Format("BusinessPartnerID = {0}", entityCV.BusinessPartnerID)).FirstOrDefault();
            ParamedicMaster entityPM = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", entityCV.ParamedicID)).FirstOrDefault();
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
            cMedicalNo.Text = entityCV.MedicalNo;
            cPatientName.Text = entityCV.cfPatientNameSalutation;
            cDOB.Text = string.Format("{0} / {1}", entityCV.DateOfBirthInString, entityCV.cfGenderInitial2);
            cNIK.Text = entityPatient.SSN;
            cDPJP.Text = entityPM.FullName;
            if (entityCV.DepartmentID != Constant.Facility.INPATIENT)
            {
                xrLabel23.Text = string.Format("RINGKASAN PASIEN PULANG\n(DISCHARGE SUMMARY)");
            }
            else
            {
                xrLabel23.Text = string.Format("RINGKASAN PASIEN PULANG RAWAT INAP\n(INPATIENT DISCHARGE SUMMARY)");
            }

            if (entityReg.ReferrerParamedicID != null)
            {
                ParamedicMaster entityPMR = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", entityReg.ReferrerParamedicID)).FirstOrDefault();
                cDokterPengirim.Text = entityPMR.FullName;
            }
            else
            {
                cDokterPengirim.Text = "";
            }
            cBusinessPartners.Text = entityBP.BusinessPartnerName;

            cVisitDateTime.Text = string.Format("Tgl & Jam Masuk : {0}", entityCV.cfVisitDateTimeInString);

            if (entityCV.PhysicianDischargedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                if (entityCV.DischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
                {
                    cDischargeDateTime.Text = string.Format("Tgl & Jam Keluar** : {0}", entityCV.cfDischargeDateTimeInString);
                }
                else
                {
                    cDischargeDateTime.Text = "Tgl & Jam Keluar :";
                }
            }
            else
            {
                if (entityCV.DischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
                {
                    cDischargeDateTime.Text = string.Format("Tgl & Jam Keluar* : {0}", entityCV.cfPhysicianDischargedDateTimeOrderInString);
                }
                else
                {
                    cDischargeDateTime.Text = string.Format("Tgl & Jam Keluar** : {0}", entityCV.cfDischargeDateTimeInString);
                }
            }

            vPatientTransferResumeMedis entityPTR = BusinessLayer.GetvPatientTransferResumeMedisList(string.Format("RegistrationID = {0}", entityReg.RegistrationID)).FirstOrDefault();
            if (entityPTR != null)
            {
                cUnitVisit.Text = string.Format("Kamar/Bagian     : {0}", entityPTR.FromHSU);
            }
            else
            {
                cUnitVisit.Text = string.Format("Kamar/Bagian     : {0}", entityCV.ServiceUnitName);
            }

            if (entityCV.PhysicianDischargedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL || entityCV.DischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
            {
                cUnitDischarge.Text = string.Format("Kamar/Bagian     : {0}", entityCV.ServiceUnitName);
            }
            else
            {
                cUnitDischarge.Text = "Kamar/Bagian   :";
            }

            #endregion

            #region Riwayat Kesehatan / Riwayat Penyakit
            if (entity != null)
            {
                List<vMedicalResume> lstMedicalResume = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID = {0} AND IsDeleted = 0 AND ISNULL(GCMedicalResumeStatus,'') NOT IN ('{1}')", entityCV.VisitID, Constant.MedicalResumeStatus.REVISED));

                if (lstMedicalResume.Count() > 0)
                {
                    subSummary.CanGrow = true;
                    mrSummaryNew.InitializeReport(entityCV.VisitID, entity.ID);
                }
                else
                {
                    subSummary.Visible = false;
                }
            }
            else
            {
                subSummary.Visible = false;
            }
            #endregion

            #region Pemeriksaan Fisik
            if (entity != null)
            {
                List<vMedicalResume> lstMedicalResume6 = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

                if (lstMedicalResume6.Count() > 0)
                {
                    subReviewOfSystem.CanGrow = true;
                    mrReviewOfSystemNewRSDOSKA.InitializeReport(entityCV.VisitID, entity.ID);
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
                List<vMedicalResume> lstMedicalResume7 = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

                if (lstMedicalResume7.Count() > 0)
                {
                    subVitalSign.CanGrow = true;
                    mrVitalSignNewMRRSDOSKA.InitializeReport(entityCV.VisitID, entity.ID);
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
            if (entity != null)
            {
                List<vMedicalResume> lstMedicalResume1 = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID = {0} AND IsDeleted = 0 AND ISNULL(GCMedicalResumeStatus,'') NOT IN ('{1}')", entityCV.VisitID, Constant.MedicalResumeStatus.REVISED));

                if (lstMedicalResume1.Count() > 0)
                {
                    subDiagnosticExam.CanGrow = true;
                    mrDiagnosticExamNewMR.InitializeReport(entityCV.VisitID, entity.ID);
                }
                else
                {
                    subDiagnosticExam.Visible = false;
                }
            }
            else
            {
                subDiagnosticExam.Visible = false;
            }
            #endregion

            #region Diagnosa
            #region Diagnosa Saat Masuk Perawatan
            if (entity != null)
            {
                List<vPatientDiagnosis> lstPatientDiagnosis = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

                if (lstPatientDiagnosis.Count() > 0)
                {
                    subDiagnoseIn.CanGrow = true;
                    mrDiagnoseInformationNewInRSDOSKA.InitializeReport(entityCV.VisitID);
                }
                else
                {
                    subDiagnoseIn.Visible = false;
                }
            }
            else
            {
                subDiagnoseIn.Visible = false;
            }
            #endregion

            #region Diagnosa Saat Pulang
            if (entity != null)
            {
                List<vPatientDiagnosis> lstPatientDiagnosis2 = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

                if (lstPatientDiagnosis2.Count() > 0)
                {
                    subDiagnoseOut.CanGrow = true;
                    mrDiagnoseInformationNewOutRSDOSKA.InitializeReport(entityCV.VisitID);
                }
                else
                {
                    subDiagnoseOut.Visible = false;
                }
            }
            else
            {
                subDiagnoseOut.Visible = false;
            }
            #endregion
            #endregion

            #region Consult
            if (entity != null)
            {
                List<vPatientReferral> lstPatientReferral = BusinessLayer.GetvPatientReferralList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

                if (lstPatientReferral.Count() > 0)
                {
                    subReferral.CanGrow = true;
                    mrReferral.InitializeReport(entityCV.VisitID, entity.ID);
                }
                else
                {
                    subReferral.Visible = false;
                }
            }
            else
            {
                subReferral.Visible = false;
            }
            #endregion

            #region Ringkasan Terapi Pengobatan
            if (entity != null)
            {
                List<vMedicalResume> lstMedicalResume2 = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID = {0} AND IsDeleted = 0 AND ISNULL(GCMedicalResumeStatus,'') NOT IN ('{1}')", entityCV.VisitID, Constant.MedicalResumeStatus.REVISED));

                if (lstMedicalResume2.Count() > 0)
                {
                    subTerapiPengobatan.CanGrow = true;
                    mrTherapyPengobatanNew.InitializeReport(entityCV.VisitID, entity.ID);
                }
                else
                {
                    subTerapiPengobatan.Visible = false;
                }
            }
            else
            {
                subTerapiPengobatan.Visible = false;
            }
            #endregion

            #region Perkembangan Selama Perawatan
            if (entity != null)
            {
                List<vMedicalResume> lstMedicalResume3 = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID = {0} AND IsDeleted = 0 AND ISNULL(GCMedicalResumeStatus,'') NOT IN ('{1}')", entityCV.VisitID, Constant.MedicalResumeStatus.REVISED));

                if (lstMedicalResume3.Count() > 0)
                {
                    subTreatment.CanGrow = true;
                    mrTreatmentNew.InitializeReport(entityCV.VisitID, entity.ID);
                }
                else
                {
                    subTreatment.Visible = false;
                }
            }
            else
            {
                subTreatment.Visible = false;
            }
            #endregion

            #region Prosedur Terapi dan Tindakan
            if (entity != null)
            {
                List<vMedicalResume> lstMedicalResume4 = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID = {0} AND IsDeleted = 0  AND ISNULL(GCMedicalResumeStatus,'') NOT IN ('{1}')", entityCV.VisitID, Constant.MedicalResumeStatus.REVISED));

                if (lstMedicalResume4.Count() > 0)
                {
                    subSurgery.CanGrow = true;
                    mrSurgeryNew.InitializeReport(entityCV.VisitID, entity.ID);
                }
                else
                {
                    subSurgery.Visible = false;
                }
            }
            else
            {
                subSurgery.Visible = false;
            }
            #endregion

            #region Kondisi dan Cara Pulang
            if (entity != null)
            {
                List<vConsultVisit1> lstCV1 = BusinessLayer.GetvConsultVisit1List(string.Format("VisitID = {0}", entityCV.VisitID));

                if (lstCV1.Count() > 0)
                {
                    subDischarge.CanGrow = true;
                    mrDischarge.InitializeReport(entityCV.VisitID, entity.ID);
                }
                else
                {
                    subDischarge.Visible = false;
                }
            }
            else
            {
                subDischarge.Visible = false;
            }
            #endregion

            #region Instruksi dan Rencana Tindak Lanjut
            if (entity != null)
            {
                List<vMedicalResume> lstMedicalResume5 = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID = {0} AND IsDeleted = 0 AND ISNULL(GCMedicalResumeStatus,'') NOT IN ('{1}')", entityCV.VisitID, Constant.MedicalResumeStatus.REVISED));

                if (lstMedicalResume5.Count() > 0)
                {
                    subInstruction.CanGrow = true;
                    mrInstructionNew.InitializeReport(entityCV.VisitID, entity.ID);
                }
                else
                {
                    subInstruction.Visible = false;
                }
            }
            else
            {
                subInstruction.Visible = false;
            }
            #endregion

            #region Footer
            string filterExpressionResumeMedis = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.RESUME_MEDIS_TELEPHONE);
            string filterExpressionIGD = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IGD_TELEPHONE_ON_RESUME_MEDIS);
            SettingParameterDt ResumeMedisPhone = BusinessLayer.GetSettingParameterDtList(filterExpressionResumeMedis).FirstOrDefault();
            SettingParameterDt IGDPhone = BusinessLayer.GetSettingParameterDtList(filterExpressionIGD).FirstOrDefault();

            cResumeMedisPhone.Text = string.Format("√ Bila ada perubahan nomor telepon dan/handphone. Mohon segera menginformasikan ke nomor {0}", ResumeMedisPhone.ParameterValue);
            cIGDPhone.Text = string.Format("√ Dalam keadaan darurat, hubungi IGD {0} ke nomor {1}", oHealthcare.HealthcareName, IGDPhone.ParameterValue);

            if (entityCV.DischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                lblTTDTanggal.Text = string.Format("{0}, {1}", oHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_TIME_FORMAT_4));
            }
            else
            {
                lblTTDTanggal.Text = string.Format("{0}, {1} {2}", oHealthcare.City, entityCV.DischargeDateInString, entityCV.DischargeTime);
            }

            ttdDokter.ImageUrl = string.Format("{0}{1}/Signature/{2}.png", AppConfigManager.QISPhysicalDirectory, AppConfigManager.QISParamedicImagePath, entityCV.ParamedicCode);
            ttdDokter.Visible = true;

            lblSignParamedicName.Text = entityCV.ParamedicName;

            lblReportProperties.Text = string.Format("MEDINFRAS - {0}, Print Date/Time:{1}, User ID:{2}", reportMaster.ReportCode, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"), appSession.UserName);
            #endregion
        }
    }
}

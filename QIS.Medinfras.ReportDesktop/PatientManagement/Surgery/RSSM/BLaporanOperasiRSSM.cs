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
    public partial class BLaporanOperasiRSSM : BaseRpt
    {
        public BLaporanOperasiRSSM()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            #region Header 1 : Healthcare
            vConsultVisit entityCV = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", param[0])).FirstOrDefault();
            Registration entityReg = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0}", entityCV.RegistrationID)).FirstOrDefault();
            vPatientSurgery entity = BusinessLayer.GetvPatientSurgeryList(string.Format("VisitID = {0} AND PatientSurgeryID = '{1}' AND IsDeleted = 0", entityCV.VisitID, param[1])).FirstOrDefault();
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
                    cDischargeDateTime.Text = string.Format("Tgl & Jam Keluar* : {0}", entityCV.cfPhysicianDischargedDateTimeInString);
                }
                else
                {
                    cDischargeDateTime.Text = string.Format("Tgl & Jam Keluar* : {0}", entityCV.cfPhysicianDischargedDateTimeInString);
                }
            }

            vPatientTransferResumeMedis entityPTR = BusinessLayer.GetvPatientTransferResumeMedisList(string.Format("RegistrationID = {0}", entityReg.RegistrationID)).FirstOrDefault();
            if (entityPTR != null)
            {
                cUnitVisit.Text = string.Format("Kamar/Bagian     : {0}", entityPTR.FromHSU);
                cUnitDischarge.Text = string.Format("Kamar/Bagian     : {0}", entityPTR.ToHSU);
            }
            else
            {
                cUnitVisit.Text = "Kamar/Bagian   :";
                cUnitDischarge.Text = "Kamar/Bagian   :";
            }

            if (entity.IsCITO == true)
            {
                lblIsUrgent.Visible = true;
            }
            else
            {
                lblIsUrgent.Visible = false;
            }

            #region QR Codes Image
            string contents = string.Format(@"{0}\r\n{1}\r\n{2}\r\n{3}\r\n{4}\r\n{5}",
                entityCV.MedicalNo, entityCV.RegistrationNo, entityCV.FirstName, entityCV.MiddleName, entityCV.LastName, entityCV.cfPatientLocation);

            QRCodeEncoder qRCodeEncoder = new QRCodeEncoder();
            qRCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qRCodeEncoder.QRCodeScale = 4;
            qRCodeEncoder.QRCodeVersion = 0;
            qRCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
            MemoryStream memoryStream = new MemoryStream();
            //System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
            //imgBarCode.Height = 400;
            //imgBarCode.Width = 400;

            using (Bitmap bitMap = qRCodeEncoder.Encode(contents, System.Text.Encoding.UTF8))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    //byte[] byteImage = ms.ToArray();
                    //imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                    pictMRNQR.Image = System.Drawing.Image.FromStream(ms, true, true);
                }
            }
            #endregion

            #endregion

            #region Data Operasi
            if (entity != null)
            {
                List<vPatientSurgery> lstSurgery = BusinessLayer.GetvPatientSurgeryList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

                if (lstSurgery.Count() > 0)
                {
                    subSurgeryReport.CanGrow = true;
                    mrSurgeryReport.InitializeReport(entityCV.VisitID, Convert.ToInt32(param[1]));
                }
                else
                {
                    subSurgeryReport.Visible = false;
                }
            }
            else
            {
                subSurgeryReport.Visible = false;
            }
            #endregion

            #region Diagnosa dan Jenis Tindakan Operasi
            #region Diagnosa
            if (entity != null)
            {
                List<vPatientSurgery> lstSurgery1 = BusinessLayer.GetvPatientSurgeryList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

                if (lstSurgery1.Count() > 0)
                {
                    subDiagnose.CanGrow = true;
                    mrDiagnoseSurgery.InitializeReport(entity.VisitID, Convert.ToInt32(param[1]));
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

            #region Jenis Tindakan Operasi
            if (entity != null)
            {
                List<vPatientSurgeryProcedureGroup> lstProcedure = BusinessLayer.GetvPatientSurgeryProcedureGroupList(string.Format("PatientSurgeryID = {0} AND IsDeleted = 0", entity.PatientSurgeryID));

                if (lstProcedure.Count() > 0)
                {
                    subProcedure.CanGrow = true;
                    mrProcedure.InitializeReport(entity.VisitID, Convert.ToInt32(param[1]));
                }
                else
                {
                    subProcedure.Visible = false;
                }
            }
            else
            {
                subProcedure.Visible = false;
            }
            #endregion
            #endregion

            #region Team Pelaksana
            if (entity != null)
            {
                List<vPatientSurgeryTeam> lstSurgeryTeam = BusinessLayer.GetvPatientSurgeryTeamList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

                if (lstSurgeryTeam.Count() > 0)
                {
                    subSurgeryTeam.CanGrow = true;
                    mrSurgeryTeam.InitializeReport(entityCV.VisitID, Convert.ToInt32(param[1]));
                }
                else
                {
                    subSurgeryTeam.Visible = false;
                }
            }
            else
            {
                subSurgeryTeam.Visible = false;
            }
            #endregion

            #region Pemasangan Implant
            if (entity != null)
            {
                List<vPatientMedicalDevice> lstMedicalDevice = BusinessLayer.GetvPatientMedicalDeviceList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

                if (lstMedicalDevice.Count() > 0)
                {
                    subMedicalDevice.CanGrow = true;
                    mrMedicalDevice.InitializeReport(entityCV.VisitID, entity.TestOrderID);
                }
                else
                {
                    subMedicalDevice.Visible = false;
                }
            }
            else
            {
                subMedicalDevice.Visible = false;
            }
            #endregion

            #region Uraian Pembedahan
            if (entity != null)
            {
                List<vPatientSurgery> lstSurgery2 = BusinessLayer.GetvPatientSurgeryList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

                if (lstSurgery2.Count() > 0)
                {
                    subDiagnosisRemarks.CanGrow = true;
                    mrDiagnosisRemarks.InitializeReport(entityCV.VisitID, Convert.ToInt32(param[1]));
                }
                else
                {
                    subDiagnosisRemarks.Visible = false;
                }
            }
            else
            {
                subDiagnosisRemarks.Visible = false;
            }
            #endregion

            #region Footer
            if (entity != null)
            {
                vPatientSurgery entityPs = BusinessLayer.GetvPatientSurgeryList(string.Format("VisitID = {0} AND PatientSurgeryID = {1} AND IsDeleted = 0", entityCV.VisitID, param[1])).FirstOrDefault();
                if (entityPs != null)
                {
                    lblTTDTanggal.Text = entityPs.cfReportDate;
                }
                else
                {
                    lblTTDTanggal.Text = "";
                }

                vPatientSurgeryTeam entityPsTeam = BusinessLayer.GetvPatientSurgeryTeamList(string.Format("VisitID = {0} AND PatientSurgeryID = {1} AND GCParamedicRole = '{2}' AND IsDeleted = 0", entityCV.VisitID, entityPs.PatientSurgeryID, Constant.SurgeryTeamRole.OPERATOR)).LastOrDefault();

                if (entityPsTeam != null)
                {
                    ttdDokter.ImageUrl = string.Format("{0}{1}/Signature/{2}.png", AppConfigManager.QISPhysicalDirectory, AppConfigManager.QISParamedicImagePath, entityPsTeam.ParamedicCode);
                    ttdDokter.Visible = true;
                    lblSignParamedicName.Text = entityPsTeam.ParamedicName;
                }
                else
                {
                    ttdDokter.ImageUrl = string.Format("{0}{1}/Signature/{2}.png", AppConfigManager.QISPhysicalDirectory, AppConfigManager.QISParamedicImagePath, entityCV.ParamedicCode);
                    ttdDokter.Visible = true;
                    lblSignParamedicName.Text = entityCV.ParamedicName;
                }
            }

            lblReportProperties.Text = string.Format("MEDINFRAS - {0}, Print Date/Time:{1}, User ID:{2}", reportMaster.ReportCode, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"), appSession.UserName);
            #endregion
        }
    }
}
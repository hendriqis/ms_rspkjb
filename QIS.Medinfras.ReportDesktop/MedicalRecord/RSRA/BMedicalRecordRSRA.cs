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
    public partial class BMedicalRecordRSRA : BaseRpt
    {
        public BMedicalRecordRSRA()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID)).FirstOrDefault();

            #region Header 1 : Healthcare
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

            #region Header 2 : Per Page

            vConsultVisit entityCV = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", param[0])).FirstOrDefault();
            Registration entityReg = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0}", entityCV.RegistrationID)).FirstOrDefault();
            Patient entityPatient = BusinessLayer.GetPatientList(string.Format("MRN = {0}", entityCV.MRN)).FirstOrDefault();
            BusinessPartners entityBP = BusinessLayer.GetBusinessPartnersList(string.Format("BusinessPartnerID = {0}", entityCV.BusinessPartnerID)).FirstOrDefault();
            ParamedicMaster entityPM = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", entityCV.ParamedicID)).FirstOrDefault();

            cMedicalNo.Text = entityCV.MedicalNo;
            cPatientName.Text = entityCV.cfPatientNameSalutation;
            cDOB.Text = string.Format("{0} / {1}", entityCV.DateOfBirthInString, entityCV.cfGenderInitial2);
            cNIK.Text = entityPatient.SSN;
            cDPJP.Text = entityPM.FullName;
            if (entityCV.DepartmentID == Constant.Facility.INPATIENT)
            {
                xrLabel1.Text = string.Format("RINGKASAN PASIEN PULANG RAWAT INAP\n(INPATIENT DISCHARGE SUMMARY)");
            }
            else if (entityCV.DepartmentID == Constant.Facility.EMERGENCY)
            {
                xrLabel1.Text = string.Format("RINGKASAN PASIEN PULANG IGD\n(DISCHARGE SUMMARY)");
            }
            else
            {
                xrLabel1.Text = string.Format("RINGKASAN PASIEN PULANG RAWAT JALAN\n(DISCHARGE SUMMARY)");
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

            #region Anamnesis
            subAnamnesis.CanGrow = true;
            mrAnamnesis.InitializeReport(entityCV.VisitID);
            #endregion

            #region Pemeriksaan Fisik
            subPemeriksaanFisik.CanGrow = true;
            mrPhysicalExam.InitializeReport(entityCV.VisitID);
            #endregion

            #region Pemeriksaan Penunjang
            subPemeriksaanPenunjang.CanGrow = true;
            mrDiagnosticExam.InitializeReport(entityCV.VisitID);
            #endregion

            #region Diagnosa
            subDiagnosa.CanGrow = true;
            mrDiagnose.InitializeReport(entityCV.VisitID);
            #endregion

            #region Therapi
            subTerapi.CanGrow = true;
            mrTherapy.InitializeReport(entityCV.VisitID);
            #endregion

            #region Footer

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

            lblReportProperties.Text = string.Format("MEDINFRAS - {0}", reportMaster.ReportCode);
            PageFooter.Visible = reportMaster.IsShowFooter;
            #endregion

        }

    }
}

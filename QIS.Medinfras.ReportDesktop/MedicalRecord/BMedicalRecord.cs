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
    public partial class BMedicalRecord : BaseRpt
    {
        public BMedicalRecord()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            #region Header 1 : Healthcare
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            if (oHealthcare != null)
            {
                pictLogoHeader.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/LOGO_NH_TEXT.png");
            }
            #endregion

            #region Header 2 : Per Page

            vConsultVisit entityCV = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", param[0])).FirstOrDefault();

            cHeaderPatient.Text = string.Format("{0} | {1} | {2} yr {3} mnth {4} day",
                entityCV.PatientName, entityCV.Gender, entityCV.AgeInYear, entityCV.AgeInMonth, entityCV.AgeInDay);
            cHeaderRegistration.Text = entityCV.RegistrationNo;
            cHeaderMedicalNo.Text = entityCV.MedicalNo;

            #endregion

            #region Header 3 : Patient

            cMedicalRecord.Text = entityCV.MedicalNo;
            cPatientName.Text = entityCV.PatientName;
            cPatientAge.Text = string.Format("{0} yr {1} mnth {2} day", entityCV.AgeInYear, entityCV.AgeInMonth, entityCV.AgeInDay);
            cPatientSex.Text = entityCV.Gender;
            cPatientAddress.Text = entityCV.StreetName;

            #endregion

            #region Header 4 : Insurance

            cNameOfInsurance.Text = entityCV.BusinessPartnerName;
            cDateOfExamination.Text = entityCV.ActualVisitDateInString;
            cPhoneNo.Text = entityCV.PhoneNo1;

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
            xrLabel28.Text = string.Format("Dengan ini saya memberikan kuasa kepada dokter yang merawat di {0} untuk memberikan keterangan lengkap mengenai kondisi medis saya saat ini maupun lampau. Apabila dikemudian hari sejak diberikan keterangan ini terjadi Penyalahgunaan informasi medis tersebut, saya tidak akan melakukan tuntutan kepada dokter yang merawat dan {0} atas pemberian informasi kesehatan diri saya / suami / istri / anak / ayah / ibu saya.", oHealthcare.HealthcareName);
            xrLabel29.Text = string.Format("I, the undersigned authorize the attending doctor at {0} to revealed my past and present complete medical record. Should there be any misuse of the revealed medical record I will not hold the attending doctor and {0} responsible and will waive any lawsuit regarding the medical record revealed of myself / husband / wife / child / father / mother.", oHealthcare.HealthcareName);

            lblTTDTanggal.Text = string.Format("{0}, {1}", oHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));

            ttdDokter.ImageUrl = string.Format("{0}{1}/Signature/{2}.png", AppConfigManager.QISPhysicalDirectory, AppConfigManager.QISParamedicImagePath, entityCV.ParamedicCode);
            ttdDokter.Visible = true;

            lblTTDDoctor.Text = entityCV.ParamedicName;

            lblReportProperties.Text = string.Format("MEDINFRAS - {0}, Print Date/Time:{1}, User ID:{2}", reportMaster.ReportCode, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"), appSession.UserName);
            
            #endregion

        }
    }
}

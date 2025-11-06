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
    public partial class BFormulirTransferPasienAntarRuanganRSRAC : BaseRpt
    {
        public BFormulirTransferPasienAntarRuanganRSRAC()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            #region Header 1 : Healthcare
            vRegistrationLinkedTo entity = BusinessLayer.GetvRegistrationLinkedToList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            vPatientNurseTransfer entityPNT = BusinessLayer.GetvPatientNurseTransferList(string.Format("VisitID = {0}", entity.VisitID)).FirstOrDefault();
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
            cMedicalNo.Text = entity.MedicalNo;
            cPatientName.Text = entity.cfPatientNameSalutation;
            cDOB.Text = entity.DateOfBirthInString;
            cDPJP.Text = entity.ParamedicName;
            cDiagnose.Text = entity.PatientDiagnosis;
            cVisitDateTime.Text = entity.cfVisitDateTimeInString;
            cRoom.Text = entity.RoomName;
            if (entity.ToRegistrationDate.ToString("dd-MM-yyyy") == Constant.ConstantDate.DEFAULT_NULL)
            {
                cTransferDate.Text = string.Format(" ");//
            }
            else
            {
                cTransferDate.Text = entity.cfToRegistrationDateInString;
            }
            cIndikasiRawat.Text = string.Format("......................");


            cTransferRoom.Text = string.Format("{0} {1}" , entity.ToServiceUnitName , entity.ToRoomName);

            #region QR Codes Image
            string contents = string.Format(@"{0}\r\n{1}",
                entity.MedicalNo, entity.RegistrationNo);

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

            #region Riwayat Kesehatan / Riwayat Penyakit
            if (entity != null)
            {
                List<vNurseChiefComplaint> lstNurseChiefComplaint = BusinessLayer.GetvNurseChiefComplaintList(string.Format("VisitID = {0} AND IsDeleted = 0", entity.VisitID));

                if (lstNurseChiefComplaint.Count() > 0)
                {
                    subSummary.CanGrow = true;
                    medicalHistoryDt.InitializeReport(entity.VisitID);
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
                List<vReviewOfSystemHd> lstRosHd = BusinessLayer.GetvReviewOfSystemHdList(string.Format("VisitID = {0} AND IsDeleted = 0", entity.VisitID));

                if (lstRosHd.Count() > 0)
                {
                    subReviewOfSystem.CanGrow = true;
                    rosAntarRuangDt.InitializeReport(entity.VisitID);
                }
                else
                {
                    rosAntarRuangDt.Visible = false;
                }
            }
            else
            {
                rosAntarRuangDt.Visible = false;
            }
            #endregion

            #region Tanda Vital dan Indikator Lainnya
            if (entity != null)
            {
                List<vVitalSignHd> lstHdVitalSign = BusinessLayer.GetvVitalSignHdList(string.Format("VisitID = {0} AND IsDeleted = 0", entity.VisitID));

                if (lstHdVitalSign.Count() > 0)
                {
                    subVitalSign.CanGrow = true;
                    vitalSignAntarRuangDt.InitializeReport(entity.VisitID);
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
                List<vTestOrderHd> lstEntityHdTestOrder = BusinessLayer.GetvTestOrderHdList(string.Format("VisitID = {0} AND GCTransactionStatus != '{1}' AND TransactionCode IN ('{2}',{3})", entity.VisitID, Constant.TransactionStatus.VOID, Constant.TransactionCode.LABORATORY_TEST_ORDER, Constant.TransactionCode.IMAGING_TEST_ORDER));

                if (lstEntityHdTestOrder.Count() > 0)
                {
                    subDiagnosticExam.CanGrow = true;
                    mrDiagnosticExamNewMR.InitializeReport(entity.VisitID);
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

            #region Ringkasan Terapi Pengobatan
            if (entity != null)
            {
                List<vPrescriptionOrderHd> lstEntityHdPrescription = BusinessLayer.GetvPrescriptionOrderHdList(string.Format("VisitID = {0} AND GCTransactionStatus != '{1}'", entity.VisitID, Constant.TransactionStatus.VOID));

                if (lstEntityHdPrescription.Count() > 0)
                {
                    subTerapiPengobatan.CanGrow = true;
                    prescriptionSummaryOrderDt.InitializeReport(entity.VisitID);
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

            #region Prosedur Terapi dan Tindakan
            if (entity != null)
            {
                List<vPatientProcedure> lstPatientProcedures = BusinessLayer.GetvPatientProcedureList(string.Format("VisitID = {0} AND IsDeleted = 0", entity.VisitID));

                if (lstPatientProcedures.Count() > 0)
                {
                    subSurgery.CanGrow = true;
                    proceduresSurgeryDt.InitializeReport(entity.VisitID);
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
                List<vConsultVisit1> lstCV1 = BusinessLayer.GetvConsultVisit1List(string.Format("VisitID = {0}", entity.VisitID));

                if (lstCV1.Count() > 0)
                {
                    subDischarge.CanGrow = true;
                    dischargeSummaryDt.InitializeReport(entity.VisitID);
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

            #region Footer
            if (entityPNT != null)
            {
                lblTo.Text = entityPNT.ToNurseName;
                lblFrom.Text = entityPNT.FromNurseName;
            }
            else
            {
                lblTo.Text = "";
                lblFrom.Text = "";
            }
            lblReportProperties.Text = string.Format("MEDINFRAS - {0}, Print Date/Time:{1}, User ID:{2}", reportMaster.ReportCode, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"), appSession.UserName);
            #endregion
        }
    }
}

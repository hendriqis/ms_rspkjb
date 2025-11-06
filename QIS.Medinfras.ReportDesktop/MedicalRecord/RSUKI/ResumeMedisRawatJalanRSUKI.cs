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
    public partial class ResumeMedisRawatJalanRSUKI : BaseRpt
    {
        public ResumeMedisRawatJalanRSUKI()
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
            vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", entityCV.RegistrationID)).FirstOrDefault();
            Patient entityPatient = BusinessLayer.GetPatientList(string.Format("MRN = {0}", entityCV.MRN)).FirstOrDefault();
            ParamedicMaster entityPM = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", entityCV.ParamedicID)).FirstOrDefault();

            cRegistrationNo.Text = entityCV.RegistrationNo;
            cMedicalNo.Text = entityCV.MedicalNo;
            cPatientName.Text = entityCV.cfPatientNameSalutation;
            cDOB.Text = string.Format("{0}", entityCV.DateOfBirthInString);
            cDPJP.Text = entityPM.FullName;
            cGender.Text = entityCV.cfGenderInitial2;
            cServiceUnitName.Text = entityCV.ServiceUnitName;
            if (entityReg.ReferrerID != 0)
            {
                cRujukan.Text = entityReg.ReferrerName;
            }
            else if (entityReg.ReferrerParamedicID != 0)
            {
                ParamedicMaster pm = BusinessLayer.GetParamedicMaster(entityReg.ReferrerParamedicID);
                cRujukan.Text = pm.FullName;
            }
            else
            {
                cRujukan.Text = "";
            }

            lblReportTitle.Text = "Resume Medis Rawat Jalan";

            cActualVisitDateTime.Text = string.Format("{0}", entityCV.cfVisitDateTimeInString);

            if (entityCV.PhysicianDischargedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                if (entityCV.DischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
                {
                    cDischargeDateTime.Text = string.Format("{0}", entityCV.cfDischargeDateTimeInString);
                }
                else
                {
                    cDischargeDateTime.Text = "";
                }
            }
            else
            {
                if (entityCV.DischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
                {
                    cDischargeDateTime.Text = string.Format("{0}", entityCV.cfPhysicianDischargedDateTimeOrderInString);
                }
                else
                {
                    cDischargeDateTime.Text = string.Format("{0}", entityCV.cfDischargeDateTimeInString);
                }
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
            mrAnamnesisRSSEB.InitializeReport(entityCV.VisitID);
            #endregion

            #region Pemeriksaan Fisik
            subPemeriksaanFisik.CanGrow = true;
            mrPhysicalExamRSUKI.InitializeReport(entityCV.VisitID);
            #endregion

            #region Hasil Pemeriksaan Penunjang
            List<vTestOrderHd> lstEntityHdTestOrder = BusinessLayer.GetvTestOrderHdList(string.Format("VisitID = {0} AND GCTransactionStatus != '{1}'", entityCV.VisitID, Constant.TransactionStatus.VOID));

            if (lstEntityHdTestOrder.Count() > 0)
            {
                subTestOrder.CanGrow = true;
                mrTestOrderRSUKI.InitializeReport(entityCV.VisitID);
            }
            else
            {
                subTestOrder.Visible = false;
            }
            #endregion

            #region Diagnosa
            subDiagnosa.CanGrow = true;
            mrDiagnose.InitializeReport(entityCV.VisitID);
            #endregion

            #region Procedures
            List<vPatientProcedure> lstProcedures = BusinessLayer.GetvPatientProcedureList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

            if (lstProcedures.Count() > 0)
            {
                subTerapi.CanGrow = true;
                mrProceduresRSUKI.InitializeReport(entityCV.VisitID);
            }
            else
            {
                subTerapi.Visible = false;
            }
            #endregion

            #region PlanningNotes
            subAnamnesis.CanGrow = true;
            mrPlanningNotesRSUKI.InitializeReport(entityCV.VisitID);
            #endregion

            #region Tindakan Terapi
            List<vGetPrescriptionOrderDtRM> oPrescOrderDtHome = BusinessLayer.GetvGetPrescriptionOrderDtRMList(string.Format("VisitID = {0} AND GCItemType = '{1}' AND GCPrescriptionType = '{2}'", entityCV.VisitID, Constant.ItemType.OBAT_OBATAN, Constant.PrescriptionType.DISCHARGE_PRESCRIPTION));

            if (oPrescOrderDtHome.Count() > 0)
            {
                subPrescriptionDischarge.CanGrow = true;
                mRPrescriptionDischargeRSSEB.InitializeReport(oPrescOrderDtHome);
            }
            else
            {
                subPrescriptionDischarge.Visible = false;
            }
            #endregion

            #region Rencana Tindak Lanjut
            List<vPatientInstruction> lstPatientInstruction = BusinessLayer.GetvPatientInstructionList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

            if (lstPatientInstruction.Count() > 0)
            {
                subInstruction.CanGrow = true;
                mrInstructionRSUKI.InitializeReport(entityCV.VisitID);
            }
            else
            {
                subInstruction.Visible = false;
            }
            #endregion

            #region Kondisi dan Cara Pulang

            List<vConsultVisit1> lstCV1 = BusinessLayer.GetvConsultVisit1List(string.Format("VisitID = {0}", entityCV.VisitID));

            if (lstCV1.Count() > 0)
            {
                subDischarge.CanGrow = true;
                mrDischargeOutpatientRSUKI.InitializeReport(entityCV.VisitID);
            }
            else
            {
                subDischarge.Visible = false;
            }
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

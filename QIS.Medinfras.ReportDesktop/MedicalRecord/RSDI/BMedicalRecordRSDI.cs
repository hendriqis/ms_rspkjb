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
    public partial class BMedicalRecordRSDI : BaseRpt
    {
        public BMedicalRecordRSDI()
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
                cHealthcareAddress.Text = string.Format("{0} {1} {2}", oHealthcare.StreetName, oHealthcare.City, oHealthcare.ZipCode);
                cHealthcarePhone.Text = oHealthcare.PhoneNo1;
                cHealthcareFax.Text = oHealthcare.FaxNo1;
            }
            #endregion

            #region Header 2 : Per Page

            vConsultVisit entityCV = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", param[0])).FirstOrDefault();
            vConsultVisitTriage entityCVT = BusinessLayer.GetvConsultVisitTriageList(string.Format("VisitID = {0}", param[0])).FirstOrDefault();
            Registration entityReg = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0}", entityCV.RegistrationID)).FirstOrDefault();
            Patient entityPatient = BusinessLayer.GetPatientList(string.Format("MRN = {0}", entityCV.MRN)).FirstOrDefault();
            ParamedicMaster entityPM = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", entityCV.ParamedicID)).FirstOrDefault();

            cMedicalNo.Text = entityCV.MedicalNo;
            cPatientName.Text = entityCV.cfPatientNameSalutation;
            cDOB.Text = string.Format("{0}", entityCV.DateOfBirthInString);
            cDPJP.Text = entityPM.FullName;
            cGender.Text = entityCV.cfGenderInitial2;
            cDischargeMethod.Text = entityCV.DischargeMethod;
            if (entityCV.DepartmentID == Constant.Facility.EMERGENCY)
            {
                cTriage.Text = entityCVT.Triage;
            }
            else
            {
                cTriage.Visible = false;
                xrTableCell16.Visible = false;
                xrTableCell17.Visible = false;
            }

            if (entityCV.DepartmentID == Constant.Facility.EMERGENCY)
            {
                xrLabel1.Text = string.Format("RESUME MEDIS IGD");
            }
            else
            {
                xrLabel1.Text = string.Format("RESUME MEDIS RAWAT JALAN");
            }

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
            mrPhysicalExamRSDI.InitializeReport(entityCV.VisitID);
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
                mrProceduresRSSEB.InitializeReport(entityCV.VisitID);
            }
            else
            {
                subTerapi.Visible = false;
            }
            #endregion

            #region Tindakan Terapi
            List<vPrescriptionOrderDtRM> oPrescOrderDtHome = BusinessLayer.GetvPrescriptionOrderDtRMList(string.Format("VisitID = {0} AND GCItemType = '{1}' AND GCPrescriptionType = '{2}'", entityCV.VisitID, Constant.ItemType.OBAT_OBATAN, Constant.PrescriptionType.DISCHARGE_PRESCRIPTION));

            if (oPrescOrderDtHome.Count() > 0)
            {
                subPrescriptionDischarge.CanGrow = true;
                mRPrescriptionDischargeRSDI.InitializeReport(entityCV.VisitID);
            }
            else
            {
                subPrescriptionDischarge.Visible = false;
            }
            #endregion

            #region Rencana Tindak Lanjut
            //List<vPatientInstruction> lstPatientInstruction = BusinessLayer.GetvPatientInstructionList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

            //if (lstPatientInstruction.Count() > 0)
            //{
            //    subInstruction.CanGrow = true;
            //    mrInstructionRSSEB.InitializeReport(entityCV.VisitID);
            //}
            //else
            //{
            //    subInstruction.Visible = false;
            //}
            subInstruction.Visible = false;
            xrLabel6.Visible = false;
            #endregion

            #region Catatan Hasil Penunjang
            subMedicalDiagnosticResult.CanGrow = true;
            mrMedicalDiagnosticResultRSDI.InitializeReport(entityCV.VisitID);
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

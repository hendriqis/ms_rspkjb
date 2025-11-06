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
    public partial class BResumeMedisRSUKI : BaseRpt
    {

        public BResumeMedisRSUKI()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            #region Header 1 : Healthcare
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
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

            #region Header 2 : Patient

            vConsultVisit entityCV = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", param[0])).FirstOrDefault();
            Patient entityPatient = BusinessLayer.GetPatientList(string.Format("MRN = {0}", entityCV.MRN))[0];
            vPatientDiagnosis entityDiagnose = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", param[0])).FirstOrDefault();

            //info atas
            cMedicalNo.Text = entityPatient.MedicalNo;
            cPatientName.Text = entityPatient.FullName + " (" + entityCV.Gender + ")";
            cDOB.Text = entityPatient.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT);
            cAge.Text = string.Format("{0} yr {1} mnth {2} day", entityCV.AgeInYear, entityCV.AgeInMonth, entityCV.AgeInDay);
            cRegistrationNo.Text = entityCV.RegistrationNo;

            //info kiri
            cRegistrationDate.Text = entityCV.ActualVisitDate.ToString(Constant.FormatString.DATE_FORMAT);
            cServiceUnit.Text = entityCV.ServiceUnitName;
            cRoom.Text = string.Format("{0} {1}", entityCV.RoomName, entityCV.BedCode);
            if (entityDiagnose != null)
            {
                vPatientDiagnosis diagearly = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0 AND GCDiagnoseType = '{1}'", entityCV.VisitID, Constant.DiagnoseType.EARLY_DIAGNOSIS)).FirstOrDefault();
                vPatientDiagnosis diagmain = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0 AND GCDiagnoseType = '{1}'", entityCV.VisitID, Constant.DiagnoseType.MAIN_DIAGNOSIS)).FirstOrDefault();

                if (diagearly != null)
                {
                    cEarlyDiagnose.Text = string.Format("{0} - {1}", diagearly.DiagnoseID, diagearly.DiagnosisText);
                }
                else if (diagearly == null)
                {
                    if (diagmain != null)
                    {
                        cEarlyDiagnose.Text = string.Format("{0} - {1} (Diagnosa Utama)", diagmain.DiagnoseID, diagmain.DiagnoseName);
                    }
                    cEarlyDiagnose.Text = "";
                }
            }
            else if (entityDiagnose == null)
            {
                cEarlyDiagnose.Text = "";
            }

            //info kanan
            if (entityCV.PhysicianDischargeOrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                cDischargeDate.Text = string.Format("-");
            }
            else
            {
                cDischargeDate.Text = string.Format("{0}", entityCV.cfPhysicianDischargedDateOrderInString);
            }

            cPatientPJ.Text = entityCV.ParamedicName;

            vRegistration entityR = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", entityCV.RegistrationID)).FirstOrDefault();
            if (entityR.ReferrerParamedicName != "")
            {
                cReferer.Text = entityR.ReferrerGroup + ", " + entityR.ReferrerParamedicName;
            }
            else if (entityR.ReferrerName != "")
            {
                cReferer.Text = entityR.ReferrerGroup + ", " + entityR.ReferrerName;
            }
            else if (entityR.ReferrerName == "")
            {
                cReferer.Text = "";
            }

            #endregion

            #region Header 3 : Per Page

            cHeaderPatient.Text = string.Format("{0} | {1} | {2} yr {3} mnth {4} day",
                entityPatient.FullName, entityCV.Gender, entityCV.AgeInYear, entityCV.AgeInMonth, entityCV.AgeInDay);
            cHeaderRegistration.Text = entityCV.RegistrationNo;
            cHeaderMedicalNo.Text = entityPatient.MedicalNo;

            #endregion

            #region Ringkasan Penyakit
            List<vChiefComplaint> lstChiefComplaint = BusinessLayer.GetvChiefComplaintList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));
            if (lstChiefComplaint.Count() > 0)
            {
                subChiefComplaint.CanGrow = true;
                episodeSummaryChiefComplaintRpt.InitializeReport(entityCV.VisitID);
            }
            else
            {
                subChiefComplaint.Visible = false;
            }
            #endregion

            #region Pemeriksaan Fisik
            List<vReviewOfSystemHd> lstHdReviewOfSystem = BusinessLayer.GetvReviewOfSystemHdList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

            if (lstHdReviewOfSystem.Count() > 0)
            {
                subReviewOfSystem.CanGrow = true;
                episodeSummaryReviewOfSystemRpt.InitializeReport(entityCV.VisitID);
            }
            else
            {
                subReviewOfSystem.Visible = false;
            }
            #endregion

            #region Vital Sign
            List<VitalSignHd> lstVitalSignHd = BusinessLayer.GetVitalSignHdList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));
            if (lstVitalSignHd.Count() > 0)
            {
                subVitalSign.CanGrow = true;
                episodeSummaryVitalSignRpt.InitializeReport(entityCV.VisitID, 1);
            }
            else
            {
                xrLabel13.Visible = false;
                subVitalSign.Visible = false;
            }
            #endregion

            #region Hasil Pemeriksaan Penunjang
            List<vTestOrderHd> lstEntityHdTestOrder = BusinessLayer.GetvTestOrderHdList(string.Format("VisitID = {0} AND GCTransactionStatus != '{1}'", entityCV.VisitID, Constant.TransactionStatus.VOID));

            if (lstEntityHdTestOrder.Count() > 0)
            {
                subTestOrder.CanGrow = true;
                episodeSummaryTestOrderRpt.InitializeReport(entityCV.VisitID);
            }
            else
            {
                subTestOrder.Visible = false;
            }
            #endregion

            #region Terapi / Pengobatan
            List<vPrescriptionOrderHd> lstEntityHdPrescriptionAll = BusinessLayer.GetvPrescriptionOrderHdList(string.Format("VisitID = {0} AND GCTransactionStatus != '{1}'", entityCV.VisitID, Constant.TransactionStatus.VOID));

            if (lstEntityHdPrescriptionAll.Count() > 0)
            {
                subPrescription.CanGrow = true;
                episodeSummaryPrescriptionAllRpt.InitializeReport(entityCV.VisitID);
            }
            else
            {
                subPrescription.Visible = false;
            }
            #endregion

            #region Diagnosis
            subDiagnosis.CanGrow = true;
            episodeSummaryDiagnosisRptRSAJ1.InitializeReport(entityCV.VisitID);
            #endregion

            #region Allergy
            List<vPatientAllergy> lstAllergy = BusinessLayer.GetvPatientAllergyList(string.Format("MRN = {0} AND IsDeleted = 0", entityCV.MRN));
            if (lstAllergy.Count() > 0)
            {
                subPatientAllergies.CanGrow = true;
                episodeSummaryAllergyRpt.InitializeReport(entityCV.MRN);
            }
            else
            {
                subPatientAllergies.Visible = false;
            }
            #endregion

            #region Konsidi Pasien Keluar RS
            vConsultVisit lstEntity = BusinessLayer.GetvConsultVisitList(String.Format("VisitID = {0}", entityCV.VisitID)).FirstOrDefault();

            if (String.IsNullOrEmpty(lstEntity.DischargeCondition))
            {
                subDischarge.Visible = false;
            }
            else
            {
                subDischarge.CanGrow = true;
                episodeSummaryDischargeRpt1.InitializeReport(entityCV.VisitID);
            }
            #endregion

            #region Obat Dibawa Pulang
            List<vPrescriptionOrderHd> lstEntityHdPrescriptionDischarge = BusinessLayer.GetvPrescriptionOrderHdList(string.Format("VisitID = {0} AND GCTransactionStatus != '{1}'", entityCV.VisitID, Constant.TransactionStatus.VOID));

            if (lstEntityHdPrescriptionDischarge.Count() > 0)
            {
                subPrescription.CanGrow = true;
                episodeSummaryPrescriptionRpt1.InitializeReport(entityCV.VisitID);
            }
            else
            {
                subPrescription1.Visible = false;
            }
            #endregion

            #region Lanjutan Pengobatan
            vConsultVisit5 lstEntity2 = BusinessLayer.GetvConsultVisit5List(String.Format("VisitID = {0}", entityCV.VisitID)).FirstOrDefault();

            if (String.IsNullOrEmpty(lstEntity2.DischargeCondition))
            {
                subDischarge2.Visible = false;
            }
            else
            {
                subDischarge2.CanGrow = true;
                episodeSummaryDischargeRpt2.InitializeReport(entityCV.VisitID);
            }
            #endregion

            #region Footer

            if (entityCV.PhysicianDischargeOrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                lblDateTimeSign.Text = String.Format("{0}, {1}", oHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT)); // Jakarta, 29-Mar-2019
            }
            else
            {
                lblDateTimeSign.Text = String.Format("{0}, {1} {2}", oHealthcare.City, entityCV.cfPhysicianDischargedDateOrderInString, entityCV.PhysicianDischargeOrderTime); // Jakarta, 29-Mar-2019
            }
            
            lblSignParamedicName.Text = entityCV.ParamedicName;

            lblReportProperties.Text = string.Format("MEDINFRAS - {0}, Print Date/Time:{1}, User ID:{2}", reportMaster.ReportCode, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"), appSession.UserName);
            #endregion
        }
    }
}
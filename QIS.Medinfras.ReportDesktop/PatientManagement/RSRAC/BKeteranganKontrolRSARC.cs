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
    public partial class BKeteranganKontrolRSARC : BaseRpt
    {

        public BKeteranganKontrolRSARC()
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
                lblHealthcareName.Text = oHealthcare.HealthcareName;
                lblHealthcareAddress.Text = string.Format("{0}, {1} {2}", oHealthcare.StreetName, oHealthcare.City, oHealthcare.ZipCode);
                lblHealthcarePhone.Text = string.Format("Telp. {0} (Hunting) Fax. : {1}", oHealthcare.PhoneNo1, oHealthcare.FaxNo1);
            }
            #endregion

            #region Header 2 : Patient Information
            vConsultVisit entity = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", param[0])).FirstOrDefault();
            vPatient entityP = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", entity.MRN))[0];
            if (param[6] == null || param[6] == "null" || string.IsNullOrEmpty(param[6]))
            {
                xrLabel30.Visible = false;
                xrLabel31.Visible = false;
                lblServiceUnit.Visible = false;
            }
            else
            {
                vHealthcareServiceUnit entityH3 = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", param[6])).FirstOrDefault();
                lblServiceUnit.Text = entityH3.ServiceUnitName;
            }
            vHealthcare entityH1 = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            ParamedicMaster entityPM = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", entity.ParamedicID))[0];
            if (param[7] == null || param[7] == "null" || string.IsNullOrEmpty(param[7]))
            {
                xrLabel34.Visible = false;
                xrLabel35.Visible = false;
                lblParamedic.Visible = false;
            }
            else
            {
                ParamedicMaster entityPM1 = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", param[7]))[0];
                lblParamedic.Text = entityPM1.FullName;
            }
            Healthcare entityH2 = BusinessLayer.GetHealthcare(entityPM.HealthcareID);
            //vChiefComplaint entityCC = BusinessLayer.GetvChiefComplaintList(string.Format("VisitID = {0}", entityCV.VisitID)).FirstOrDefault();

            lblName.Text = entityP.PatientName;
            lblDOB.Text = entityP.DateOfBirthInString;
            lblAddress.Text = entityP.HomeAddress;
            lblUmur.Text = String.Format("{0}tahun {1}bulan {2}hari", entity.AgeInYear, entity.AgeInMonth, entity.AgeInDay);
            lblGender.Text = entityP.cfGender;
            lblStatus.Text = entityP.MaritalStatus;
            lblRM.Text = entityP.MedicalNo;
            lblNo.Text = entityP.SSN;

            DateTime Date = DateTime.Parse(param[2]);
            lblDay.Text = param[1];
            lblDate.Text = Date.ToString(Constant.FormatString.DATE_FORMAT);
            lblTime.Text = string.Format("{0}:{1} WIB", param[3], param[4]);
            lblDocument.Text = param[5];

            List<vPatientDiagnosis> entityDiag = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND GCDiagnoseType = '{1}' AND IsDeleted = 0", entity.VisitID, Constant.DiagnoseType.MAIN_DIAGNOSIS));
            if (entityDiag == null)
            {
                lblDiagnose.Text = "-";
            }
            else
            {
                List<vPatientDiagnosis> lstPatientDiagnosis = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND GCDiagnoseType = '{1}' AND IsDeleted = 0", entity.VisitID, Constant.DiagnoseType.MAIN_DIAGNOSIS));
                StringBuilder diagNotes = new StringBuilder();
                foreach (vPatientDiagnosis patientDiagnosis in lstPatientDiagnosis)
                {
                    if (diagNotes.ToString() != "")
                        diagNotes.Append(", ");
                    diagNotes.Append(patientDiagnosis.DiagnosisText);
                }
                lblDiagnose.Text = diagNotes.ToString();
            }
            PrescriptionOrderHd entityPo = BusinessLayer.GetPrescriptionOrderHdList(string.Format("VisitID = {0} AND GCPrescriptionType = '{1}'", entity.VisitID, Constant.PrescriptionType.DISCHARGE_PRESCRIPTION)).FirstOrDefault();
            if (entityPo == null)
            {
                lblTerapi.Text = " ";
            }
            else
            {
                List<PrescriptionOrderDt> lstPO = BusinessLayer.GetPrescriptionOrderDtList(string.Format("PrescriptionOrderID = {0}", entityPo.PrescriptionOrderID));
                StringBuilder presOrder = new StringBuilder();
                foreach (PrescriptionOrderDt po in lstPO)
                {
                    if (presOrder.ToString() != "")
                        presOrder.AppendLine(", ");
                    presOrder.AppendLine(po.DrugName);
                        
                }
                lblTerapi.Text = presOrder.ToString();
            }
            #endregion
            ttdDokter.ImageUrl = string.Format("{0}{1}/Signature/{2}.png", AppConfigManager.QISPhysicalDirectory, AppConfigManager.QISParamedicImagePath, entityPM.ParamedicCode);
            ttdDokter.Visible = true;
            #region Footer
            cCityDate.Text = String.Format("{0}, {1}", oHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT)); // Surabaya, 13-Nov-2017
            lblParamedicName.Text = entity.ParamedicName;
            #endregion
        }
    }
}
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
    public partial class BLembarJadwalRehabilitasiMedik : BaseRpt
    {

        public BLembarJadwalRehabilitasiMedik()
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

            #region Header 2 : Patient Information
            StringBuilder sbDiagnostic = new StringBuilder();
            vConsultVisit entity = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", param[0])).FirstOrDefault();
            vPatient entityP = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", entity.MRN))[0];
            vHealthcare entityH1 = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            ParamedicMaster entityPM = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", entity.ParamedicID))[0];
            Healthcare entityH2 = BusinessLayer.GetHealthcare(entityPM.HealthcareID);
            vChiefComplaint entityCC = BusinessLayer.GetvChiefComplaintList(string.Format("VisitID = {0}", entity.VisitID)).FirstOrDefault();
            vPatientVisitNote entityPVN = BusinessLayer.GetvPatientVisitNoteList(string.Format("VisitID = {0} AND GCPatientNoteType = 'X011^011'", entity.VisitID)).FirstOrDefault();
            vPatientDiagnosis entityDiagnose = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", entity.VisitID)).FirstOrDefault();
            vPatientProcedure entityProcedure = BusinessLayer.GetvPatientProcedureList(string.Format("VisitID = {0} AND IsDeleted = 0", entity.VisitID)).FirstOrDefault();

            lblMedicalNo.Text = entity.MedicalNo;
            lblPatientName.Text = entity.PatientName;
            lblDiagnose.Text = entityP.DateOfBirthInString;

            if (entityPVN != null)
            {
                lblPlanning.Text = string.Format("{0}{1} Instruksi : {2}{3}", entityPVN.PlanningText, Environment.NewLine, Environment.NewLine, entityPVN.InstructionText);
            }
            else
            {
                lblPlanning.Text = "";
            }

            if (entityDiagnose != null)
            {
                vPatientDiagnosis diagmain = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0 AND GCDiagnoseType = '{1}'", entity.VisitID, Constant.DiagnoseType.MAIN_DIAGNOSIS)).FirstOrDefault();

                if (diagmain != null)
                {
                    if (diagmain.DiagnoseID != "")
                    {
                        lblDiagnose.Text = string.Format("{0} - {1}", diagmain.DiagnoseID, diagmain.DiagnoseName);
                    }
                    else
                    {
                        lblDiagnose.Text = string.Format("{0}", diagmain.DiagnosisText);
                    }
                }
                else
                {
                    lblDiagnose.Text = "";
                }
            }
            else
            {
                lblDiagnose.Text = "";
            }
            #endregion

            #region Footer
            ttdDokter.ImageUrl = string.Format("{0}{1}/Signature/{2}.png", AppConfigManager.QISPhysicalDirectory, AppConfigManager.QISParamedicImagePath, entity.ParamedicCode);
            ttdDokter.Visible = true;
            lblSignParamedicName.Text = entity.ParamedicName;
            cCityDate.Text = String.Format("{0}, {1}", oHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));
            lblReportProperties.Text = string.Format("MEDINFRAS - {0}, Print Date/Time:{1}, User ID:{2}", reportMaster.ReportCode, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"), appSession.UserName);
            PageFooter.Visible = reportMaster.IsShowFooter;
            #endregion
        }
    }
}
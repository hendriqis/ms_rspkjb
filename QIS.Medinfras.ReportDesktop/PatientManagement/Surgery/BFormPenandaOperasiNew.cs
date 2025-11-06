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
    public partial class BFormPenandaOperasiNew : BaseRpt
    {

        public BFormPenandaOperasiNew()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            #region Header 1 : Healthcare
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

            #region Header 2 : Patient
            vPatientSurgery entity = BusinessLayer.GetvPatientSurgeryList(string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            vConsultVisit entityCV = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            Patient entityPatient = BusinessLayer.GetPatientList(string.Format("MRN = {0}", entityCV.MRN)).FirstOrDefault();
            vPatientDiagnosis entityDiagnose = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();

            cPatientName.Text = entityPatient.FullName;
            cMedicalNo.Text = entityPatient.MedicalNo;
            cDOB.Text = entityPatient.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT);
            cGender.Text = entityCV.Gender;
         
            #endregion

            if (entity != null)
            {
                vPatientSurgery entityPs = BusinessLayer.GetvPatientSurgeryList(string.Format("VisitID = {0} AND PatientSurgeryID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, param[0])).FirstOrDefault();

                #region Body Diagram
                List<vPatientBodyDiagramHd> lstHdBodyDiagram = BusinessLayer.GetvPatientBodyDiagramHdList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

                if (lstHdBodyDiagram.Count() > 0)
                {
                    subBodyDiagram.CanGrow = true;
                    mrBodyDiagramRpt.InitializeReport(entityCV.VisitID);
                }
                else
                {
                    xrLabel15.Visible = false;
                    mrBodyDiagramRpt.Visible = false;
                }
                #endregion

                #region Footer
                lblSignDate.Text = String.Format("{0}, {1}", oHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT)); // Surabaya, 13-Nov-2017

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

                ttdDokter.ImageUrl = string.Format("{0}{1}/Signature/{2}.png", AppConfigManager.QISPhysicalDirectory, AppConfigManager.QISParamedicImagePath, entityPs.ParamedicCode);
                ttdDokter.Visible = true;

                lblReportProperties.Text = string.Format("MEDINFRAS - {0}, Print Date/Time:{1}, User ID:{2}", reportMaster.ReportCode, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"), appSession.UserName);
                #endregion
            }
        }
    }
}
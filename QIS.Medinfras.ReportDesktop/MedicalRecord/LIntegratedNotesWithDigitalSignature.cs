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
    public partial class LIntegratedNotesWithDigitalSignature : BaseRpt
    {

        public LIntegratedNotesWithDigitalSignature()
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
            vConsultVisit entityCV = BusinessLayer.GetvConsultVisitList(String.Format(param[0].ToString())).FirstOrDefault();
            xrTableCell3.Text = entityCV.MedicalNo;
            xrTableCell5.Text = entityCV.PatientName;
            xrTableCell4.Text = entityCV.RegistrationNo;
            cRegisteredPhysician.Text = entityCV.ParamedicName;
            #endregion

            #region Header 3 : Per Page
            cHeaderPatient.Text = string.Format("{0} | {1} | {2} yr {3} mnth {4} day",
                entityCV.PatientName, entityCV.Gender, entityCV.AgeInYear, entityCV.AgeInMonth, entityCV.AgeInDay);
            cHeaderRegistration.Text = entityCV.RegistrationNo;
            cHeaderMedicalNo.Text = entityCV.MedicalNo;
            #endregion

            #region Integrated Notes With Digital Signature
            subIntegratedNotes.CanGrow = true;
            episodeSummaryIntegratedNotesRpt.InitializeReport(entityCV.VisitID);
            #endregion

            #region Footer
            lblReportProperties.Text = string.Format("MEDINFRAS - {0}, Print Date/Time:{1}, User ID:{2}", reportMaster.ReportCode, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"), appSession.UserName);
            #endregion
        }
    }
}

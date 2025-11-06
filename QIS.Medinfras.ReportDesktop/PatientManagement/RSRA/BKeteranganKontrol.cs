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
    public partial class BKeteranganKontrol : BaseRpt
    {

        public BKeteranganKontrol()
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
            vHealthcare entityH1 = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            ParamedicMaster entityPM = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", entity.ParamedicID))[0];
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
            #endregion

            #region Footer
            cCityDate.Text = String.Format("{0}, {1}", oHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT)); // Surabaya, 13-Nov-2017
            lblParamedicName.Text = entity.ParamedicName;
            #endregion
        }
    }
}
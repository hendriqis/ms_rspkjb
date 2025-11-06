using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Linq;


namespace QIS.Medinfras.ReportDesktop
{
    public partial class BFormKontrolPesertaMCU : BaseDailyPortrait2Rpt
    {
        public BFormKontrolPesertaMCU()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            vConsultVisit cv = BusinessLayer.GetvConsultVisitList(param[0].ToString()).FirstOrDefault();
            vConsultVisitItemPackage cvPackage = BusinessLayer.GetvConsultVisitItemPackageList(param[0].ToString()).FirstOrDefault();

            xrPictureBox1.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/logo.png");

            cPatientName.Text = cv.PatientName;

            pictPatient.ImageUrl = cv.PatientImageUrl;

            if (cv.GCGender == Constant.Gender.FEMALE)
            {
                cDateOfBirth.Text = string.Format("{0} - Wanita", cv.DateOfBirthInString);
            }
            else if (cv.GCGender == Constant.Gender.MALE)
            {
                cDateOfBirth.Text = string.Format("{0} - Pria", cv.DateOfBirthInString);
            }

            cPackage.Text = cvPackage.ItemName1;

            if (cv.SSN != null && cv.SSN != "")
            {
                cNIK.Text = cv.SSN;
            }
            else
            {
                cNIK.Text = "-";
            }
            cPatientName.Text = cv.PatientName;
            cRegistrationNo.Text = cv.RegistrationNo;

            cDepartment.Text = "-";
            cLocation.Text = "-";
            cCostCenter.Text = "-";

            if (cv.BusinessPartner != null && cv.BusinessPartner != "")
            {
                cSender.Text = cv.BusinessPartner;
            }
            else
            {
                cSender.Text = "-";
            }
            cRegistrationDate.Text = cv.ActualVisitDateInString;

            base.InitializeReport(param);
        }
    }
}

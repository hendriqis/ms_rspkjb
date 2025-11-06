using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using System.Text;
using System.IO;
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientInfoCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            vConsultVisitForPatientBanner oVisit = BusinessLayer.GetvConsultVisitForPatientBannerList(string.Format("RegistrationID = '{0}' AND IsMainVisit = 1", param)).First();
            if (oVisit != null)
            {
                hdnMRN.Value = oVisit.MRN.ToString();

                lblPatientName.InnerHtml = oVisit.PatientName;
                lblMRN.InnerHtml = oVisit.MedicalNo;
                lblMedicalNo.InnerHtml = oVisit.MedicalNo;
                lblSEPNo.InnerHtml = oVisit.NoSEP;
                lblIdentityNo.InnerHtml = oVisit.SSN;
                lblPatientName2.InnerHtml = oVisit.PatientName;
                lblPatientName3.InnerHtml = oVisit.PreferredName;
                lblGender.InnerHtml = oVisit.Gender;
                lblBloodType.InnerHtml = string.Format("{0} {1}", oVisit.BloodType, oVisit.BloodRhesus);
                lblReligion.InnerHtml = oVisit.Religion;
                lblNationality.InnerHtml = oVisit.Nationality;
                lblBirthPlace.InnerHtml = string.Format("{0}, {1}", oVisit.CityOfBirth, oVisit.DateOfBirthInString);
                lblAgeInYear.InnerHtml = oVisit.AgeInYear.ToString();
                lblAgeInMonth.InnerHtml = oVisit.AgeInMonth.ToString();
                lblAgeInDay.InnerHtml = oVisit.AgeInYear.ToString();
                lblEthnic.InnerHtml = oVisit.Ethnic;

                lblJenisPeserta.InnerHtml = oVisit.JenisPeserta;
                lblNHSRegistrationNo.InnerHtml = oVisit.NHSRegistrationNo;

                lblAddress.InnerHtml = oVisit.HomeAddress;
                lblPhoneNo.InnerHtml = string.Format("{0}{1}{2}", oVisit.PhoneNo1, !String.IsNullOrEmpty(oVisit.PhoneNo2) ? "," : "", oVisit.PhoneNo2);
                lblMobilePhoneNo.InnerHtml = string.Format("{0}{1}{2}", oVisit.MobilePhoneNo1, !String.IsNullOrEmpty(oVisit.MobilePhoneNo2) ? "," : "", oVisit.MobilePhoneNo2);
                lblEmail.InnerHtml = oVisit.EmailAddress;

                lblLastAcuteInitialAssessmentDate.InnerHtml = oVisit.cfLastAcuteInitialAssessmentDate;
                lblLastChronicInitialAssessmentDate.InnerHtml = oVisit.cfLastChronicInitialAssessmentDate;

                lblSpouseName.InnerHtml = oVisit.SpouseName;
                lblMotherName.InnerHtml = oVisit.MotherName;
                lblFatherName.InnerHtml = oVisit.FatherName;

                lblEducation.InnerHtml = oVisit.Education;
                lblWork.InnerHtml = oVisit.Occupation;
                lblPatientCategory.InnerHtml = oVisit.PatientCategory;

                #region Patient Image
                imgPatientImage.Src = HttpUtility.HtmlEncode(oVisit.cfPatientImageUrl);
                hdnPatientGender.Value = HttpUtility.HtmlEncode(oVisit.GCGender);
                #endregion

                BindGridView();
            }
        }

        private void BindGridView()
        {
            string filter = string.Format("MRN = {0} AND IsDeleted = 0", hdnMRN.Value);
            grdView.DataSource = BusinessLayer.GetvPatientNotesList(filter, int.MaxValue, 1, "ID");
            grdView.DataBind();
        }
    }
}
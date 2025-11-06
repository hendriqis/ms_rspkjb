using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class PatientDtBannerCtl : BaseUserControlCtl
    {
        public void InitializePatientBanner(vPatientBanner entity)
        {
            hdnPatientGender.Value = entity.GCSex;
            imgPatientProfilePicture.Src = entity.PatientImageUrl;
            lblPatientName.InnerHtml = entity.PatientName;

            lblMRN.InnerHtml = entity.MedicalNo;
            lblDOB.InnerHtml = entity.DateOfBirthInString;
            BasePage page = (BasePage)this.Page;
            lblPatientAge.InnerHtml = Helper.GetPatientAge(page.GetWords(), entity.DateOfBirth);
            lblGender.InnerHtml = entity.Sex;
            lblAllergy.InnerHtml = entity.PatientAllergy;
        }
    }
}
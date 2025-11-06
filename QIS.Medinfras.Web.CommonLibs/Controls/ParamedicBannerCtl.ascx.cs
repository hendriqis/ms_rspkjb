using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class ParamedicBannerCtl : BaseUserControlCtl
    {
        public void InitializePatientBanner(vParamedicMaster entity)
        {
            imgParamedicProfilePicture.Src = string.Format("{0}{1}{2}", AppConfigManager.QISVirtualDirectory, AppConfigManager.QISParamedicImagePath, entity.PictureFileName);
            hdnParamedicGender.Value = entity.GCGender;

            lblParamedicName.InnerHtml = entity.ParamedicName;
            lblParamedicCode.InnerHtml = entity.ParamedicCode;
            lblParamedicType.InnerHtml = entity.ParamedicMasterType;
            lblGender.InnerHtml = entity.Gender;
            lblSpecialty.InnerHtml = entity.SpecialtyName;
            lblEmploymentStatus.InnerHtml = entity.EmploymentStatus;
            lblInitial.InnerHtml = entity.Initial;

            if (entity.Notes.Length > 50)
            {
                lblNote.InnerHtml = entity.Notes.Substring(0, 50) + "...";
                lblNote.Attributes.Add("title", entity.Notes);
            }
            else
            {
                lblNote.InnerHtml = entity.Notes;
            }
        }
    }
}
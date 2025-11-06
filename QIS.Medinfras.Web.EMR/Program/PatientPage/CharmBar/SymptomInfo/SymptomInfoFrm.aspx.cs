using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class SymptomInfoFrm : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string imgFrontSrc = "";
                string imgBackSrc = "";
                string useMapFront = "";
                string useMapBack = "";
                Patient entity = BusinessLayer.GetPatient(AppSession.RegisteredPatient.MRN);
                if (entity.GCGender == Constant.Gender.MALE)
                {
                    imgFrontSrc = "male_front.png";
                    imgBackSrc = "male_back.png";
                    useMapFront = "#symptominfofrontmale";
                    useMapBack = "#symptominfobackmale";
                }
                else
                {
                    imgFrontSrc = "female_front.png";
                    imgBackSrc = "female_back.png";
                    useMapFront = "#symptominfofrontfemale";
                    useMapBack = "#symptominfobackfemale";
                }
                imgFront.Src = ResolveUrl("~/Libs/Images/Medical/BodyDiagram/") + imgFrontSrc;
                imgBack.Src = ResolveUrl("~/Libs/Images/Medical/BodyDiagram/") + imgBackSrc;
                imgFront.Attributes.Add("usemap", useMapFront);
                imgBack.Attributes.Add("usemap", useMapBack);
            }
        }
    }
}
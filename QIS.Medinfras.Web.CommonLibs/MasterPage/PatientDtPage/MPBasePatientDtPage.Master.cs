using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Web.UI.HtmlControls;
using QIS.Medinfras.Web.CommonLibs.Controls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class MPBasePatientDtPage : BaseMP
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!Page.IsPostBack)
            {
                if (AppSession.UserLogin == null)
                    Response.Redirect("~/../SystemSetup/Login.aspx");

                vPatientBanner entity = BusinessLayer.GetvPatientBannerList(string.Format("MRN = {0}", AppSession.PatientDetail.MRN))[0];
                EntityToControl(entity);
            }
        }

        private void EntityToControl(vPatientBanner entity)
        {
            ((PatientDtBannerCtl)ctlPatientBanner).InitializePatientBanner(entity);
        }
    }
}
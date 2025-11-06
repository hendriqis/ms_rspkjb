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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class MPBaseParamedicPage : BaseMP
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!Page.IsPostBack)
            {
                if (AppSession.UserLogin == null)
                    Response.Redirect("~/../SystemSetup/Login.aspx");

                vParamedicMaster entity = BusinessLayer.GetvParamedicMasterList(string.Format("ParamedicID = {0}", AppSession.ParamedicID))[0];
                EntityToControl(entity);
            }
        }

        private void EntityToControl(vParamedicMaster entity)
        {
            ctlParamedicBanner.InitializePatientBanner(entity);
        }
    }
}
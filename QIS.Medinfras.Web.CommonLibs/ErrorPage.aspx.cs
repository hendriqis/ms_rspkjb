using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxClasses;

namespace QIS.Medinfras.Web.CommonLibs
{
    public partial class ErrorPage : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            string errorMessage = ASPxWebControl.GetCallbackErrorMessage();
            if (!String.IsNullOrEmpty(errorMessage))
            {
                errorMessage = errorMessage.Replace("<", "");
                errorMessage = errorMessage.Replace(">", "");
                lblMessage.InnerHtml = errorMessage;
            }
        }
    }
}
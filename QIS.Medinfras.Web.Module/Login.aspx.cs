using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Text;

namespace QIS.Medinfras.Web.Module
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect(Page.ResolveUrl("~/Program/Outpatient/Registration.aspx"));

        }
    }
}
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
using System.Text;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class MedicalSummary1 : BasePage
    {
        protected int VisitID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            }
        }
    }
}
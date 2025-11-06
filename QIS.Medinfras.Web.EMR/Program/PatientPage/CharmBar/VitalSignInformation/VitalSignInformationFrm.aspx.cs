using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using System.Text;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class VitalSignInformationFrm : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                List<vVitalSignDtInformation> lst = BusinessLayer.GetvVitalSignDtInformationList(string.Format("MRN = {0}", AppSession.RegisteredPatient.MRN));
                rptView.DataSource = lst;
                rptView.DataBind();
            }
        }
    }
}
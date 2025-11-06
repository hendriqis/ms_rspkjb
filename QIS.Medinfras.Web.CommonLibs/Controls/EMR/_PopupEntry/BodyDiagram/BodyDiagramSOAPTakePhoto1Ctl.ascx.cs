using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class BodyDiagramSOAPTakePhoto1Ctl : UserControl
    {
        protected string MRN = AppSession.RegisteredPatient.MRN.ToString();
        protected string isUsingMRN = AppSession.RegisteredPatient.MRN > 0 ? "1" : "0";
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
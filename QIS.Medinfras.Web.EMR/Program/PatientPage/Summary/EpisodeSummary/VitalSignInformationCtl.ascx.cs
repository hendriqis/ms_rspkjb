using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class VitalSignInformationCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            List<vEpisodeVitalSignDtInformation> lst = BusinessLayer.GetvEpisodeVitalSignDtInformationList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID));
            rptView.DataSource = lst;
            rptView.DataBind();
        }
    }
}
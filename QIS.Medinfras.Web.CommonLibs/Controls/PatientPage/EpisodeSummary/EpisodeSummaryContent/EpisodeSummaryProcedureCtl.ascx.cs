using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class EpisodeSummaryProcedureCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string queryString)
        {
            string filter = string.Format("VisitID = {0} AND IsDeleted = 0", queryString);
            List<vPatientProcedure> lstPatientDiagnose = BusinessLayer.GetvPatientProcedureList(filter);
            rptDifferentDiagnosis.DataSource = lstPatientDiagnose;
            rptDifferentDiagnosis.DataBind();
        }
    }
}
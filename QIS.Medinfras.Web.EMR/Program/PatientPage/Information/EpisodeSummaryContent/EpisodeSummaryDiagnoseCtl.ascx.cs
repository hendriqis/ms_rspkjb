using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class EpisodeSummaryDiagnoseCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string queryString)
        {
            string filter = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);
            List<vPatientDiagnosis> lstPatientDiagnose = BusinessLayer.GetvPatientDiagnosisList(filter);
            rptDifferentDiagnosis.DataSource = lstPatientDiagnose.OrderBy(p => p.DiagnoseType);
            rptDifferentDiagnosis.DataBind();
        }
    }
}
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
    public partial class EpisodeSummaryDiagnoseCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string queryString)
        {
            BindGridView(Convert.ToInt32(queryString));
        }

        private void BindGridView(int visitID)
        {
            string filterExpression = string.Empty;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID = {0} AND IsDeleted = 0 ORDER BY GCDiagnoseType", visitID);

            List<vPatientDiagnosis> lstEntity = BusinessLayer.GetvPatientDiagnosisList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
    }
}
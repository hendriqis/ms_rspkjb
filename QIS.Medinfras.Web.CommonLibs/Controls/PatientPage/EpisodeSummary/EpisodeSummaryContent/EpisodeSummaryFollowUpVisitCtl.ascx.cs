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

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class EpisodeSummaryFollowUpVisitCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string queryString)
        {
            string filterExpression = string.Format("FromVisitID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}')", queryString, Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED);

            rptReviewOfSystem.DataSource = BusinessLayer.GetvAppointmentList(filterExpression);
            rptReviewOfSystem.DataBind();
        }
    }
}
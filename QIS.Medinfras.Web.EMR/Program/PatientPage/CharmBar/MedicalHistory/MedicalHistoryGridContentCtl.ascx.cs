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
    public partial class MedicalHistoryGridContentCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        public override void InitializeDataControl(string param)
        {
            string filterExpression = string.Format("MRN = {0} AND VisitID != {1}", AppSession.RegisteredPatient.MRN, AppSession.RegisteredPatient.VisitID);
            List<vConsultVisitCustom> lstEntity = BusinessLayer.GetvConsultVisitCustomList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, Convert.ToInt32(param), "VisitID DESC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
    }
}
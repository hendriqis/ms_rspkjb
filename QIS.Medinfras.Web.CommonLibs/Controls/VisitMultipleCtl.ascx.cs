using System.Collections.Generic;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class VisitMultipleCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            txtRegistrationNo.Text = param;
            BindGridView(param);
        }

        private void BindGridView(string param)
        {
            string filterExpression = string.Format("RegistrationNo = '{0}' AND GCVisitStatus != '{1}' AND GCRegistrationStatus != '{1}' ORDER BY VisitID", param, Constant.VisitStatus.CANCELLED);
            List<vConsultVisit7> lstEntity = BusinessLayer.GetvConsultVisit7List(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

    }
}
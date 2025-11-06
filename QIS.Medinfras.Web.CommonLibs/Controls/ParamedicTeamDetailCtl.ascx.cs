using System.Collections.Generic;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ParamedicTeamDetailCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            txtRegistrationNo.Text = param;
            BindGridView(param);
        }

        private void BindGridView(string param)
        {
            string filterExpression = string.Format("RegistrationNo = '{0}' AND GCParamedicRole != '{1}' AND IsDeleted = 0 ORDER BY GCParamedicRole", param, Constant.ParamedicRole.PEMERIKSA);
            List<vParamedicTeam> lstEntity = BusinessLayer.GetvParamedicTeamList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

    }
}
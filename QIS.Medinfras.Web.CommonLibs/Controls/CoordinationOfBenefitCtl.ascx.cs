using System.Collections.Generic;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class CoordinationOfBenefitCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] tempParam = param.Split('|');
            txtRegistrationNo.Text = tempParam[0];
            txtPatient.Text = "(" + tempParam[1] + ")" + tempParam[2];
            BindGridView(tempParam[0]);
        }

        private void BindGridView(string param)
        {
            string filterExpression = string.Format("RegistrationID = (SELECT RegistrationID FROM Registration WHERE RegistrationNo = '{0}') AND IsDeleted = 0", param);
            List<vRegistrationPayer> lstEntity = BusinessLayer.GetvRegistrationPayerList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

    }
}
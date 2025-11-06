using System.Collections.Generic;
using System.Linq;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientDepositBalanceCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] tempParam = param.Split('|');
            txtPatient.Text = "(" + tempParam[1] + ") " + tempParam[2];

            string filterExpression = string.Format("MRN = '{0}' Order by BalanceID DESC", tempParam[0]);
            List<vPatientDepositBalance> lstEntity = BusinessLayer.GetvPatientDepositBalanceList(filterExpression);
            txtBalanceEND.Text = lstEntity.FirstOrDefault().BalanceEND.ToString(Constant.FormatString.NUMERIC_2);

            BindGridView(tempParam[0]);
        }

        private void BindGridView(string param)
        {
            string filterExpression = string.Format("MRN = '{0}'", param);
            List<vPatientDepositBalance> lstEntity = BusinessLayer.GetvPatientDepositBalanceList(filterExpression, int.MaxValue, 1, "BalanceID DESC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

    }
}
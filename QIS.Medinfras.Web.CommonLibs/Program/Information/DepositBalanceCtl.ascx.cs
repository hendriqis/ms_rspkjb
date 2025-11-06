using System.Collections.Generic;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using System;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class DepositBalanceCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            //string[] tempParam = param.Split('|');
            //txtPatient.Text = "(" + tempParam[1] + ")" + tempParam[2];
            //BindGridView(tempParam[0]);

            hdnMRN.Value = param;

            Patient item = BusinessLayer.GetPatient(Convert.ToInt32(hdnMRN.Value));
            txtPatient.Text = String.Format("{0}", item.FullName);
            BindGridView(param);
        }

        private void BindGridView(string param)
        {
            string filterExpression = string.Format("MRN = '{0}'", param);
            List<vPatientDepositBalance> lstEntity = BusinessLayer.GetvPatientDepositBalanceList(filterExpression,int.MaxValue,1,"PaymentDate DESC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

    }
}
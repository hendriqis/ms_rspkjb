using System.Collections.Generic;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class InformationOrderOutstandingCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] mydata = param.Split('|');
            txtRegistrationNo.Text = mydata[1];
            txtPatient.Text = mydata[2] + " | " + mydata[3];
            BindGridView(mydata[0]);
        }

        private void BindGridView(string param)
        {
            string filterExpression = string.Format("VisitID = {0} AND GCTransactionStatus IN ('{1}','{2}')", param, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL);
            List<vRegistrationOutstandingDetail> lstEntity = BusinessLayer.GetvRegistrationOutstandingDetailList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

    }
}
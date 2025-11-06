using System;
using System.Collections.Generic;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class InfoPatientTransferCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnRegistrationID.Value = param;

            BindGridView();
        }

        protected void cbpViewInfoPatientTransfer_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = (string.Format("RegistrationID = {0} AND GCPatientTransferStatus = '{1}'", hdnRegistrationID.Value, Constant.PatientTransferStatus.TRANSFERRED));
            List<vPatientTransfer> lst = BusinessLayer.GetvPatientTransferList(filterExpression);
            lvwView.DataSource = lst;
            lvwView.DataBind();
        }
    }
}
using System;
using System.Collections.Generic;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class InfoHistoryJobBedRegistrationCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnRegistrationID.Value = param;

            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("RegistrationID = {0} AND ((IsJobBedClosedOLD <> IsJobBedClosedNEW) OR (IsJobBedReopenOLD <> IsJobBedReopenNEW))", Convert.ToInt32(hdnRegistrationID.Value));
            List<vRegistrationStatusLog> lstEntity = BusinessLayer.GetvRegistrationStatusLogList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }
    }
}
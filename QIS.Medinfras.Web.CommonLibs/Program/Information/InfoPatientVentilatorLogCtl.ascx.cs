using System;
using System.Collections.Generic;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class InfoPatientVentilatorLogCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnRegistrationID.Value = param;

            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("RegistrationID = {0} ORDER BY ID", Convert.ToInt32(hdnRegistrationID.Value));
            List<vPatientVentilatorLog> lstEntity = BusinessLayer.GetvPatientVentilatorLogList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }
    }
}
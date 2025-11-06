using System;
using System.Collections.Generic;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class InformationMultipleVisitCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            txtRegistrationNo.Text = paramInfo[0];
            txtNoSEP.Text = paramInfo[1];
            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("RegistrationNo = '{0}' AND GCVisitStatus != '{1}' AND GCRegistrationStatus != '{1}' ORDER BY VisitID", txtRegistrationNo.Text, Constant.VisitStatus.CANCELLED);
            List<vConsultVisit7> lstEntity = BusinessLayer.GetvConsultVisit7List(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxCallbackPanel;
using System.Data;
using Newtonsoft.Json;
using System.Text;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ExternalMedicationInfoCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            this.PopupTitle = "Patient Medication";
            string[] paramInfo = param.Split('|');
            BindGridView(paramInfo[1]);
        }

        private void BindGridView(string itemName)
        {
            string filterExpression = string.Format("VisitID = {0} AND DrugName LIKE '%{1}%' AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, itemName);
            List<vPastMedication> lstEntity = BusinessLayer.GetvPastMedicationList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
        }
    }
}
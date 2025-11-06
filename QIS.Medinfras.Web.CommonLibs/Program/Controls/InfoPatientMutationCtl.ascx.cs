using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class InfoPatientMutationCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        String filterExpression = "";

        public override void InitializeDataControl(string param)  
        {
            hdnVisitID.Value = param;
            vConsultVisit entityVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", hdnVisitID.Value))[0];
            txtPrintTotal.Text = entityVisit.RegistrationNo;
            hdnRegistrationID.Value = entityVisit.RegistrationID.ToString();

            BindGridDetail();
        }

        private void BindGridDetail()
        {
            filterExpression = (string.Format("RegistrationID = {0} AND GCPatientTransferStatus = '{1}' AND FromParamedicID = ToParamedicID", hdnRegistrationID.Value, Constant.PatientTransferStatus.TRANSFERRED));
            List<vPatientTransfer> lst = BusinessLayer.GetvPatientTransferList(filterExpression);
            lvwView.DataSource = lst;
            lvwView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridDetail();
        }
    }
}
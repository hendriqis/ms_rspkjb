using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ControlOrderDtCustomCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {
            string[] temp = param.Split('|');
            hdnVisitIDCtl.Value = temp[0];
            hdnRegistrationNoCtl.Value = temp[1];
            hdnPatientNameCtl.Value = temp[2];
            hdnItemIDCtl.Value = temp[3];
            hdnItemCodeCtl.Value = temp[4];
            hdnItemNameCtl.Value = temp[5];

            txtItemServiceName.Text = string.Format("{0} - {1}", hdnItemCodeCtl.Value, hdnItemNameCtl.Value);
            txtNoReg.Text = string.Format("{0} - {1}", hdnRegistrationNoCtl.Value, hdnPatientNameCtl.Value);
            
            BindGridView();
        }

        protected void cbpViewPopup_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string param = e.Parameter;
            string result = param + "|";

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView()
        {
            List<vPatientChargesDtAIOControl> lstEntity = BusinessLayer.GetvPatientChargesDtAIOControlList(string.Format(string.Format("VisitID = {0} ORDER BY ServiceUnitName, ItemName1", hdnVisitIDCtl.Value)));
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
    }
}
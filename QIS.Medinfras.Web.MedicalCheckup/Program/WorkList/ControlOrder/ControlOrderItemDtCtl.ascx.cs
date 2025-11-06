using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using System.Data;

namespace QIS.Medinfras.Web.MedicalCheckup.Program
{
    public partial class ControlOrderItemDtCtl : BaseViewPopupCtl
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
            BindGridView();
        }

        private void BindGridView()
        {
            List<vMCUOrderStatus> lstEntity = BusinessLayer.GetvMCUOrderStatusList(string.Format(string.Format("VisitID = {0} AND ItemPackageID = {1} ORDER BY ServiceUnitName ASC", hdnVisitIDCtl.Value, hdnItemIDCtl.Value)));
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
    }
}
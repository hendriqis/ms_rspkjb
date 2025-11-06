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
    public partial class GenerateOrderItemDtCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        protected string defaultParamedicName = string.Empty;
        protected string defaultParamedicID = string.Empty;

        public override void InitializeDataControl(string param)
        {
            hdnItemID.Value = param;
            vItemService vItemService = BusinessLayer.GetvItemServiceList(String.Format("ItemID = {0} AND IsDeleted = 0", Convert.ToInt32(param))).FirstOrDefault();
            txtItemServiceName.Text = string.Format("{0} - {1}", vItemService.ItemCode, vItemService.ItemName1);
            defaultParamedicID = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.MC_KODE_DEFAULT_DOKTER).ParameterValue;
            defaultParamedicName = BusinessLayer.GetParamedicMaster(Convert.ToInt32(defaultParamedicID)).FullName;
            BindGridView();
        }

        protected void cbpViewPopup_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }

        private void BindGridView()
        {
            grdView.DataSource = BusinessLayer.GetvItemServiceDtList(string.Format("ItemID = {0} AND IsDeleted = 0 ORDER BY ItemID ASC", hdnItemID.Value));
            grdView.DataBind();
        }
    }
}
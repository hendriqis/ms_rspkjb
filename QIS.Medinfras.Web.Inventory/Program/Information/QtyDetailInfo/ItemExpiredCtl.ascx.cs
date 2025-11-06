using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Inventory.Program.Information
{
    public partial class ItemExpiredCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                string[] temp = param.Split('|');
                hdnItemID.Value = temp[0];
                hdnLocationID.Value = temp[1];
                BindGridView();
            }
        }

        private void BindGridView()
        {
            grdView.DataSource = BusinessLayer.GetvItemDistributionDtExpiredInfoList(string.Format("ToLocationID = {0} AND ItemID = {1}", hdnLocationID.Value, hdnItemID.Value));
            grdView.DataBind();
        }
    }
}
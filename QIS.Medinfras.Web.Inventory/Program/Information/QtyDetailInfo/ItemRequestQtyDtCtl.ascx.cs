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
    public partial class ItemRequestQtyDtCtl : BaseViewPopupCtl
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
            grdView.DataSource = BusinessLayer.GetvItemRequestDtList(string.Format("ItemID = {0} AND ToLocationID = {3} AND IsDeleted = 0 AND GCItemDetailStatus NOT IN ('{1}','{2}') AND GCTransactionStatus NOT IN ('{1}','{2}')", hdnItemID.Value, Constant.TransactionStatus.VOID, Constant.TransactionStatus.CLOSED, hdnLocationID.Value));
            grdView.DataBind();
        }
    }
}
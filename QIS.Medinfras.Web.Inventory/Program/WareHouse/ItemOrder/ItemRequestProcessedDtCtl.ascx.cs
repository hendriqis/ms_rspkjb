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

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class ItemRequestProcessedDtCtl : BaseViewPopupCtl
    {
        decimal stock = 0;
        string itemUnit = "";
        public override void InitializeDataControl(string param)
        {
            string[] temp = param.Split('|');
            hdnOrderID.Value = temp[0];
            hdnItemID.Value = temp[1];

            stock = Convert.ToDecimal(temp[2]);
            itemUnit = temp[3];
            txtStock.Text = string.Format("{0} {1}", stock, itemUnit);
            ItemMaster item = BusinessLayer.GetItemMaster(Convert.ToInt32(temp[1]));
            txtItem.Text = string.Format("{0} ({1})", item.ItemName1, item.ItemCode);

            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("ItemRequestID != {0} AND ItemID = {1}", hdnOrderID.Value, hdnItemID.Value);

            List<vItemRequestDtRealizationPerItemPerOrder> lstEntity = BusinessLayer.GetvItemRequestDtRealizationPerItemPerOrderList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();

            decimal totalItemRequest = lstEntity.Sum(p => p.ItemRequestQuantity);
            decimal availableStock = stock - totalItemRequest;
            if (availableStock < 0)
                availableStock = 0;
            txtItemRequestTotal.Text = string.Format("{0} {1}", totalItemRequest, itemUnit);
            txtAvailableStock.Text = string.Format("{0} {1}", availableStock, itemUnit);
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }
    }
}
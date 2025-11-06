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
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class ApprovedConsignmentOrderDetailList : BasePageTrx
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.APPROVED_CONSIGNMENT_ORDER;
        }

        private GetUserMenuAccess menu;

        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = false;
        }

        protected override void InitializeDataControl()
        {
            MPTrx master = (MPTrx)Master;
            menu = ((MPMain)master.Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

            hdnOrderID.Value = Page.Request.QueryString["id"];
            vPurchaseOrderHd entityItemRequest = BusinessLayer.GetvPurchaseOrderHdList(String.Format("PurchaseOrderID = '{0}'", Convert.ToInt32(hdnOrderID.Value)))[0];
            EntityToControl(entityItemRequest);
        }

        private void EntityToControl(vPurchaseOrderHd entity)
        {
            hdnOrderID.Value = entity.PurchaseOrderID.ToString();
            txtOrderNo.Text = entity.PurchaseOrderNo;
            txtItemOrderDate.Text = entity.OrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtSupplierName.Text = entity.BusinessPartnerName;
            txtNotes.Text = entity.Remarks;
            txtExpiredDate.Text = entity.POExpiredDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPurchaseOrderType.Text = entity.PurchaseOrderType;
            txtTermCondition.Text = entity.TermName;
            txtFrancoRegion.Text = entity.FrancoRegion;
            txtCurrencyCode.Text = entity.CurrencyCode;
            txtCurrencyRate.Text = entity.CurrencyRate.ToString();
            txtDeliveryDate.Text = entity.DeliveryDateInString;
            txtTotalOrder.Text = entity.TransactionAmount.ToString("N");
            txtFinalDiscountInPercentage.Text = entity.FinalDiscount.ToString("N");
            txtFinalDiscount.Text = (entity.TransactionAmount * entity.FinalDiscount / 100).ToString("N");
            txtPPN.Text = ((entity.VATPercentage / 100) * (entity.TransactionAmount - Convert.ToDecimal(txtFinalDiscount.Text))).ToString("N");
            txtDP.Text = entity.DownPaymentAmount.ToString("N");
            txtTotalOrderSaldo.Text = (entity.TransactionAmount - Convert.ToDecimal(txtFinalDiscount.Text) + Convert.ToDecimal(txtPPN.Text) - entity.DownPaymentAmount).ToString("N");
            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnOrderID.Value != "")
                filterExpression = string.Format("PurchaseOrderID = {0} AND IsDeleted = 0", hdnOrderID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPurchaseOrderDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPurchaseOrderDt> lstEntity = BusinessLayer.GetvPurchaseOrderDtList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
            grdView.DataSource = lstEntity;
            grdView.DataBind();

        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}
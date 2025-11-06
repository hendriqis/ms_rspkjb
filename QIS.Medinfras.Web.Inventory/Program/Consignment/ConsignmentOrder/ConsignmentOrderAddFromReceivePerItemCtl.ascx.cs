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
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class ConsignmentOrderAddFromReceivePerItemCtl : BaseEntryPopupCtl
    {
        protected string filterExpressionPurchaseOrder = "";
        public override void InitializeDataControl(string param)
        {
            hdnIsUsedProductLineReceiveCtl.Value = AppSession.IsUsedProductLine;

            BindGridView();
        }

        public String GetPurchaseReceivePerItemFilterExpression()
        {
            Int32 orderID = ((ConsignmentOrder)Page).GetOrderID();
            Int32 oSupplierID = ((ConsignmentOrder)Page).GetSupplierID();
            Int32 oLocationID = ((ConsignmentOrder)Page).GetLocationID();
            Int32 oProductLineID = ((ConsignmentOrder)Page).GetProductLineID();

            string filterExpression = "IsDeleted = 0 AND ItemID IN (SELECT ItemID FROM ItemProduct WHERE IsConsigmentItem = 1) AND GCItemStatus != 'X181^999'";

            filterExpression += string.Format(" AND ItemID IN (SELECT ItemID FROM vPurchaseReceivePOCustom WHERE ISNULL(OrderQuantity,0) < (ISNULL(ReceivedQuantity,0) - ISNULL(ReturnQuantity,0)) AND (ISNULL(ReceivedQuantity,0) - ISNULL(ReturnQuantity,0) - ISNULL(OrderQuantity,0)) > 0 AND GCItemDetailStatus != '{0}'", Constant.TransactionStatus.VOID);

            filterExpression += string.Format(" AND LocationID = {0} AND BusinessPartnerID = {1}", oLocationID, oSupplierID); // masih lanjutan filter vPurchaseReceivePOCustom

            if (hdnIsUsedProductLineReceiveCtl.Value == "1")
            {
                filterExpression += string.Format(" AND ProductLineID = {0}", oProductLineID); // masih lanjutan filter vPurchaseReceivePOCustom
            }

            filterExpression += ")"; // penutup filter vPurchaseReceivePOCustom

            if (orderID != 0 && orderID != null) {
                filterExpression += " AND ItemID NOT IN (SELECT ItemID FROM PurchaseOrderDt WHERE PurchaseOrderID = " + orderID + " AND IsDeleted = 0)";
            }

            return filterExpression;
        }

        private void BindGridView()
        {
            Int32 oSupplierID = ((ConsignmentOrder)Page).GetSupplierID();

            string filterExpression = "1 = 0";
            if (hdnItemID.Value != "") 
            {
                int orderID = ((ConsignmentOrder)Page).GetOrderID();
                filterExpression = string.Format("ItemID = {0} AND ISNULL(OrderQuantity,0) < (ISNULL(ReceivedQuantity,0) - ISNULL(ReturnQuantity,0)) AND (ISNULL(ReceivedQuantity,0) - ISNULL(ReturnQuantity,0) - ISNULL(OrderQuantity,0)) > 0 AND GCItemDetailStatus != '{2}' AND BusinessPartnerID = '{3}' ORDER BY PurchaseReceiveNo, ItemName1 ASC",
                                        hdnItemID.Value, orderID, Constant.TransactionStatus.VOID, oSupplierID);

            }
            
            List<vPurchaseReceivePOCustom> lstEntity = BusinessLayer.GetvPurchaseReceivePOCustomList(filterExpression);
            grdPopupView.DataSource = lstEntity;
            grdPopupView.DataBind();
        }

        protected void grdPopupView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vPurchaseReceivePOCustom entity = e.Row.DataItem as vPurchaseReceivePOCustom;
                TextBox txtReceivedQty = e.Row.FindControl("txtReceivedQty") as TextBox;
                TextBox txtReturnQty = e.Row.FindControl("txtReturnQty") as TextBox;
                TextBox txtPOQty = e.Row.FindControl("txtPOQty") as TextBox;
                TextBox txtQuantity = e.Row.FindControl("txtQuantity") as TextBox;

                txtReceivedQty.Text = entity.ReceivedQuantity.ToString();
                txtReturnQty.Text = entity.ReturnQuantity.ToString();
                txtPOQty.Text = entity.OrderQuantity.ToString();
                txtQuantity.Text = (entity.RequiredOrderQuantity - entity.OrderQuantity).ToString();                
            }
        }

        protected void cbpViewPopup_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridView();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntityDt(PurchaseOrderDt entityDt, String[] data) 
        {
            entityDt.ItemID = Convert.ToInt32(data[1]);
            entityDt.Quantity = Convert.ToDecimal(data[2]);
            entityDt.GCPurchaseUnit = data[3];
            entityDt.GCBaseUnit = data[4];
            entityDt.ConversionFactor = Convert.ToDecimal(data[5]);
            entityDt.UnitPrice = Convert.ToDecimal(data[6]);

            entityDt.IsDiscountInPercentage1 = Convert.ToBoolean(data[7]);
            entityDt.IsDiscountInPercentage2 = Convert.ToBoolean(data[8]);
            entityDt.DiscountPercentage1 = Convert.ToDecimal(data[9]);
            entityDt.DiscountPercentage2 = Convert.ToDecimal(data[10]);
            entityDt.DiscountAmount1 = Convert.ToDecimal(data[11]);
            entityDt.DiscountAmount2 = Convert.ToDecimal(data[12]);

            entityDt.LineAmount = entityDt.CustomSubTotal;
            //entityDt.ReceivedQuantity = Convert.ToDecimal(data[10]);
            entityDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;

            //Simpan data draft
            entityDt.DraftQuantity = Convert.ToDecimal(data[2]);
            entityDt.DraftUnitPrice = Convert.ToDecimal(data[6]);
            entityDt.DraftDiscountPercentage1 = Convert.ToDecimal(data[9]);
            entityDt.DraftDiscountPercentage2 = Convert.ToDecimal(data[10]);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseReceivePODao purchaseReceivePODao = new PurchaseReceivePODao(ctx);
            PurchaseOrderDtDao purchaseOrderDtDao = new PurchaseOrderDtDao(ctx);
            PurchaseReceiveDtDao purchaseReceiveDtDao = new PurchaseReceiveDtDao(ctx);
            PurchaseOrderHdDao purchaseOrderHdDao = new PurchaseOrderHdDao(ctx);
            
            try
            {
                Int32 OrderID = ((ConsignmentOrder)Page).GetOrderID();
                Int32 OrderDtID = 0;

                ((ConsignmentOrder)Page).SavePurchaseOrderHd(ctx, ref OrderID);

                if (purchaseOrderHdDao.Get(OrderID).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    List<String> items = hdnLstItem.Value.Split('|').ToList();
                    items.Remove("");

                    PurchaseOrderDt entityDt = null;
                    PurchaseReceivePO entityPO = null;
                    PurchaseReceiveDt receiveDt = null;

                    foreach (String item in items)
                    {
                        String[] data = item.Split(';');
                        Int32 itemID = Convert.ToInt32(data[1]);

                        entityDt = new PurchaseOrderDt();
                        entityDt.PurchaseOrderID = OrderID;
                        ControlToEntityDt(entityDt, data);
                        entityDt.CreatedBy = AppSession.UserLogin.UserID;
                        OrderDtID = purchaseOrderDtDao.InsertReturnPrimaryKeyID(entityDt);

                        entityPO = new PurchaseReceivePO();
                        entityPO.ItemID = itemID;
                        entityPO.PurchaseOrderID = OrderID;
                        entityPO.PurchaseReceiveID = Convert.ToInt32(data[0]);
                        entityPO.ReceivedQuantity = Convert.ToDecimal(data[14]);
                        entityPO.OrderedQuantity = entityDt.Quantity * entityDt.ConversionFactor;
                        purchaseReceivePODao.Insert(entityPO);

                        receiveDt = BusinessLayer.GetPurchaseReceiveDtList(String.Format("PurchaseReceiveID = {0} AND ItemID = {1} AND GCItemDetailStatus != '{2}'", data[0], itemID, Constant.TransactionStatus.VOID), ctx).FirstOrDefault();
                        receiveDt.PurchaseOrderID = OrderID;
                        receiveDt.PurchaseOrderDtID = OrderDtID;
                        receiveDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        purchaseReceiveDtDao.Update(receiveDt);
                    }

                    retval = purchaseOrderHdDao.Get(OrderID).PurchaseOrderNo;
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = string.Format("Pemesanan konsinyasi {0} tidak dapat diubah. Harap refresh halaman ini.", purchaseOrderHdDao.Get(OrderID).PurchaseOrderNo);
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;

        }
    }
}
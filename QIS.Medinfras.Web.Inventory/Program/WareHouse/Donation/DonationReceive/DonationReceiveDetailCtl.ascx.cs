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
    public partial class DonationReceiveDetailCtl : BaseEntryPopupCtl
    {
        protected string filterExpressionPurchaseOrder = "";
        public override void InitializeDataControl(string param)
        {
            hdnSupplierID.Value = param;
            filterExpressionPurchaseOrder = string.Format("BusinessPartnerID = '{0}' AND GCTransactionStatus = '{1}' AND TransactionCode = '{2}'", hdnSupplierID.Value, Constant.TransactionStatus.APPROVED, Constant.TransactionCode.CONSIGNMENT_ORDER);
            BindGridView();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (grdView.Rows.Count < 1)
                BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = "1 = 0";
            if (hdnOrderID.Value != "")
                filterExpression = string.Format("PurchaseOrderID = {0} AND (Quantity - ReceivedQuantity) > 0 AND IsDeleted = 0 ORDER BY ItemName1 ASC", hdnOrderID.Value);
            
            List<vPurchaseOrderDt> lstEntity = BusinessLayer.GetvPurchaseOrderDtList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
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

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vPurchaseOrderDt entity = e.Row.DataItem as vPurchaseOrderDt;
                TextBox txtReceivedItem = e.Row.FindControl("txtReceivedItem") as TextBox;
                TextBox txtUnitPrice = e.Row.FindControl("txtUnitPrice") as TextBox;
                TextBox txtDiscountPercentage1 = e.Row.FindControl("txtDiscountPercentage1") as TextBox;
                TextBox txtDiscountPercentage2 = e.Row.FindControl("txtDiscountPercentage2") as TextBox;
                HtmlGenericControl lblPurchaseUnit = e.Row.FindControl("lblPurchaseUnit") as HtmlGenericControl;
                txtReceivedItem.Text = (entity.Quantity - entity.ReceivedQuantity).ToString();
                txtUnitPrice.Text = entity.UnitPrice.ToString();
                txtDiscountPercentage1.Text = entity.DiscountPercentage1.ToString();
                txtDiscountPercentage2.Text = entity.DiscountPercentage2.ToString();
                lblPurchaseUnit.InnerHtml = entity.PurchaseUnit;
            }
        }

        protected string DateTimeNowDatePicker()
        {
            return DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
        }

        protected class PRDtExpired 
        {
            public PurchaseReceiveDt purchaseReceiveDt { get; set; }
            public PurchaseReceiveDtExpired purchaseReceiveDtExpired { get; set; }
        }

        private void ControlToEntity(IDbContext ctx, List<PRDtExpired> lstPRDt)
        {
            PurchaseOrderDtDao entityPODtDao = new PurchaseOrderDtDao(ctx);
            foreach (GridViewRow row in grdView.Rows)
            {
                CheckBox chkIsSelected = row.FindControl("chkIsSelected") as CheckBox;

                if (chkIsSelected.Checked)
                {
                    HtmlInputHidden hdnPurchaseOrderDtID = (HtmlInputHidden)row.FindControl("keyField");
                    HtmlInputHidden hdnPurchaseOrderHdID = (HtmlInputHidden)row.FindControl("hdnPOHdID");
                    TextBox txtQtyReceive = (TextBox)row.FindControl("txtReceivedItem");
                    TextBox txtUnitPrice = (TextBox)row.FindControl("txtUnitPrice");
                    TextBox txtBatchNo = (TextBox)row.FindControl("txtBatchNo");
                    TextBox txtExpired = (TextBox)row.FindControl("txtExpired");
                    TextBox txtDiscountPercentage1 = (TextBox)row.FindControl("txtDiscountPercentage1");
                    TextBox txtDiscountPercentage2 = (TextBox)row.FindControl("txtDiscountPercentage2");
                    HtmlInputHidden hdnConversionFactor = (HtmlInputHidden)row.FindControl("hdnConversionFactor");
                    HtmlInputHidden hdnGCPurchaseUnit = (HtmlInputHidden)row.FindControl("hdnGCPurchaseUnit");
                    PurchaseOrderDt entityPODt = entityPODtDao.Get(Convert.ToInt32(hdnPurchaseOrderDtID.Value));
                    CheckBox chkIsAsset = row.FindControl("chkIsAsset") as CheckBox;
                    PurchaseReceiveDt entityPRDt = new PurchaseReceiveDt();
                    entityPRDt.PurchaseOrderID = Convert.ToInt32(hdnPurchaseOrderHdID.Value);
                    entityPRDt.ItemID = entityPODt.ItemID;
                    entityPRDt.Quantity = Convert.ToDecimal(Request.Form[txtQtyReceive.UniqueID]);
                    entityPRDt.GCItemUnit = hdnGCPurchaseUnit.Value;
                    entityPRDt.GCBaseUnit = entityPODt.GCBaseUnit;
                    entityPRDt.ConversionFactor = Convert.ToDecimal(hdnConversionFactor.Value);
                    entityPRDt.UnitPrice = Convert.ToDecimal(Request.Form[txtUnitPrice.UniqueID]);
                    entityPRDt.DiscountPercentage1 = Convert.ToDecimal(Request.Form[txtDiscountPercentage1.UniqueID]);
                    entityPRDt.DiscountPercentage2 = Convert.ToDecimal(Request.Form[txtDiscountPercentage2.UniqueID]);
                    entityPRDt.IsBonusItem = false;
                    entityPRDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                    entityPODt.ReceivedQuantity += entityPRDt.Quantity;
                    
                    PurchaseReceiveDtExpired entityExpiredDt = new PurchaseReceiveDtExpired();
                    entityExpiredDt.BatchNumber = Request.Form[txtBatchNo.UniqueID];
                    entityExpiredDt.Quantity = entityPRDt.Quantity;
                    entityExpiredDt.ExpiredDate = Helper.GetDatePickerValue(Request.Form[txtExpired.UniqueID]);

                    PRDtExpired entity = new PRDtExpired();
                    entity.purchaseReceiveDt = entityPRDt;
                    entity.purchaseReceiveDtExpired = entityExpiredDt;

                    lstPRDt.Add(entity);
                }

            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseReceiveDtDao entityPurchaseReceiveDtDao = new PurchaseReceiveDtDao(ctx); 
            PurchaseReceiveDtExpiredDao entityPurchaseReceiveDtExpiredDao = new PurchaseReceiveDtExpiredDao(ctx);

            try
            {
                List<PRDtExpired> lstPRDt = new List<PRDtExpired>();
                
                ControlToEntity(ctx, lstPRDt);
                int purchaseReceiveID = 0;
                string purchaseReceiveNo = "";
                ((ConsignmentReceive)Page).SavePurchaseReceiveHd(ctx, ref purchaseReceiveID, ref purchaseReceiveNo);

                foreach (PRDtExpired entityPRDt in lstPRDt)
                {
                    PurchaseReceiveDt entityDt = entityPRDt.purchaseReceiveDt;
                    entityDt.PurchaseReceiveID = purchaseReceiveID;
                    entityDt.CreatedBy = AppSession.UserLogin.UserID;
                    entityPurchaseReceiveDtDao.Insert(entityDt);
                    Int32 ID = BusinessLayer.GetPurchaseReceiveDtMaxID(ctx);

                    PurchaseReceiveDtExpired entityPRDtExpired = entityPRDt.purchaseReceiveDtExpired;
                    entityPRDtExpired.ID = ID;
                    entityPurchaseReceiveDtExpiredDao.Insert(entityPRDtExpired);
                }
                
                retval = purchaseReceiveNo;
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
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
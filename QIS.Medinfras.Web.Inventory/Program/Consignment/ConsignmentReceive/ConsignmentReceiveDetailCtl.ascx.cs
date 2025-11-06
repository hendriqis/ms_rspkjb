using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class ConsignmentReceiveDetailCtl : BaseEntryPopupCtl
    {
        protected string filterExpressionPurchaseOrder = "";
        public override void InitializeDataControl(string param)
        {
            hdnSupplierID.Value = param.Split('|')[0];
            hdnProductLineIDCtl.Value = param.Split('|')[1];
            hdnIsUsedProductLine.Value = AppSession.IsUsedProductLine;

            string filterSetvar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IM0131);
            List<SettingParameterDt> lstSetparDt = BusinessLayer.GetSettingParameterDtList(filterSetvar);
            hdnIM0131Ctl.Value = lstSetparDt.Where(t => t.ParameterCode == Constant.SettingParameter.IM0131).FirstOrDefault().ParameterValue;

            filterExpressionPurchaseOrder = string.Format("BusinessPartnerID = '{0}' AND GCTransactionStatus = '{1}' AND TransactionCode = '{2}'", hdnSupplierID.Value, Constant.TransactionStatus.APPROVED, Constant.TransactionCode.CONSIGNMENT_ORDER);

            if (hdnIsUsedProductLine.Value == "1")
            {
                if (hdnProductLineIDCtl.Value != "0")
                {
                    filterExpressionPurchaseOrder += string.Format(" AND ProductLineID = {0}", hdnProductLineIDCtl.Value);
                }
            }

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
            {
                filterExpression = string.Format("PurchaseOrderID = {0} AND (Quantity - ReceivedQuantity) > 0 AND IsDeleted = 0", hdnOrderID.Value);
            }

            if (hdnIsUsedProductLine.Value == "1")
            {
                filterExpression += string.Format(" AND ProductLineID = {0}", hdnProductLineIDCtl.Value);
            }

            filterExpression += " ORDER BY ItemName1 ASC";

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

        private void ControlToEntity(IDbContext ctx, List<PRDtExpired> lstPRDt, ref string errMessage)
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
                    TextBox txtDiscount1 = (TextBox)row.FindControl("txtDiscount1");
                    TextBox txtDiscountPercentage2 = (TextBox)row.FindControl("txtDiscountPercentage2");
                    TextBox txtDiscount2 = (TextBox)row.FindControl("txtDiscount2");
                    HtmlInputHidden hdnDefaultConversionFactor = (HtmlInputHidden)row.FindControl("hdnDefaultConversionFactor");
                    HtmlInputHidden hdnConversionFactor = (HtmlInputHidden)row.FindControl("hdnConversionFactor");
                    HtmlInputHidden hdnGCPurchaseUnit = (HtmlInputHidden)row.FindControl("hdnGCPurchaseUnit");
                    HtmlInputHidden hdnReceivedQuantity = (HtmlInputHidden)row.FindControl("hdnReceivedQuantity");
                    HtmlInputHidden hdnQuantity = (HtmlInputHidden)row.FindControl("hdnQuantity");
                    HtmlInputHidden hdnItemName1 = (HtmlInputHidden)row.FindControl("hdnItemName1");

                    CheckBox chkIsAsset = row.FindControl("chkIsAsset") as CheckBox;
                    Decimal defaultConversionFactor = Convert.ToDecimal(hdnDefaultConversionFactor.Value);

                    PurchaseOrderDt entityPODt = entityPODtDao.Get(Convert.ToInt32(hdnPurchaseOrderDtID.Value));

                    PurchaseReceiveDt entityPRDt = new PurchaseReceiveDt();
                    entityPRDt.PurchaseOrderID = Convert.ToInt32(hdnPurchaseOrderHdID.Value);
                    entityPRDt.PurchaseOrderDtID = Convert.ToInt32(hdnPurchaseOrderDtID.Value);
                    entityPRDt.ItemID = entityPODt.ItemID;
                    entityPRDt.Quantity = Convert.ToDecimal(Request.Form[txtQtyReceive.UniqueID]);
                    entityPRDt.GCItemUnit = hdnGCPurchaseUnit.Value;
                    entityPRDt.GCBaseUnit = entityPODt.GCBaseUnit;
                    entityPRDt.ConversionFactor = Convert.ToDecimal(hdnConversionFactor.Value);
                    entityPRDt.UnitPrice = Convert.ToDecimal(Request.Form[txtUnitPrice.UniqueID]);
                    entityPRDt.DiscountPercentage1 = Convert.ToDecimal(Request.Form[txtDiscountPercentage1.UniqueID]);
                    entityPRDt.DiscountAmount1 = Convert.ToDecimal(Request.Form[txtDiscount1.UniqueID]);
                    entityPRDt.DiscountPercentage2 = Convert.ToDecimal(Request.Form[txtDiscountPercentage2.UniqueID]);
                    entityPRDt.DiscountAmount2 = Convert.ToDecimal(Request.Form[txtDiscount2.UniqueID]);
                    entityPRDt.IsBonusItem = false;
                    entityPRDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                    entityPODt.ReceivedQuantity += entityPRDt.Quantity;
                    entityPRDt.LineAmount = entityPRDt.CustomSubTotal2;
                    if (hdnQuantity != null)
                    {
                        if ((Convert.ToDecimal(hdnQuantity.Value) - Convert.ToDecimal(hdnReceivedQuantity.Value)) < (entityPRDt.Quantity * entityPRDt.ConversionFactor / defaultConversionFactor))
                        {
                            errMessage = string.Format("Item {0} tidak dapat dilakukan penerimaan, karena melebihi jumlah pemesanan.", hdnItemName1.Value);
                            break;
                        }
                    }

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

                ControlToEntity(ctx, lstPRDt, ref errMessage);

                if (string.IsNullOrEmpty(errMessage))
                {
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
                else
                {
                    result = false;
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
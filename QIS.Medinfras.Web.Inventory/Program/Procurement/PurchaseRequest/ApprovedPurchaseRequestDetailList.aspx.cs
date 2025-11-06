using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using System.Data;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class ApprovedPurchaseRequestDetailList : BasePageTrx
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;
        private string[] lstDiscount1 = null;
        private string[] lstDiscount2 = null;
        private string[] lstPrice = null;
        private string[] lstQty = null;
        private string[] lstSupplierID = null;
        private string[] lstSupplierName = null;
        private string[] lstGCPurchaseUnit = null;
        private string[] lstPurchaseUnit = null;
        private string[] lstConversionFactor = null;
        private string[] lstTermID = null;
        private string[] lstSupplierItemName = null;

        protected string filterExpressionSupplier = "";

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.APPROVED_PURCHASE_REQUEST;
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
            filterExpressionSupplier = string.Format("GCBusinessPartnerType = '{0}' AND IsDeleted = 0", Constant.BusinessObjectType.SUPPLIER);
            hdnPurchaseRequestID.Value = Page.Request.QueryString["id"];
            vPurchaseRequestHd entityPurchaseRequest = BusinessLayer.GetvPurchaseRequestHdList(String.Format("PurchaseRequestID = '{0}'", Convert.ToInt32(hdnPurchaseRequestID.Value)))[0];
            EntityToControl(entityPurchaseRequest);

            List<StandardCode> listStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}','{2}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PURCHASE_ORDER_TYPE, Constant.StandardCode.FRANCO_REGION, Constant.StandardCode.CURRENCY_CODE));
            StandardCode scDefaultPurchaseOrderType = listStandardCode.FirstOrDefault(p => p.ParentID == Constant.StandardCode.PURCHASE_ORDER_TYPE && p.IsDefault);
            if (scDefaultPurchaseOrderType == null)
                scDefaultPurchaseOrderType = listStandardCode.FirstOrDefault(p => p.ParentID == Constant.StandardCode.PURCHASE_ORDER_TYPE);
            StandardCode scDefaultFrancoRegion = listStandardCode.FirstOrDefault(p => p.ParentID == Constant.StandardCode.FRANCO_REGION && p.IsDefault);
            if (scDefaultFrancoRegion == null)
                scDefaultFrancoRegion = listStandardCode.FirstOrDefault(p => p.ParentID == Constant.StandardCode.FRANCO_REGION);
            StandardCode scDefaultCurrencyCode = listStandardCode.FirstOrDefault(p => p.ParentID == Constant.StandardCode.CURRENCY_CODE && p.IsDefault);
            if (scDefaultCurrencyCode == null)
                scDefaultCurrencyCode = listStandardCode.FirstOrDefault(p => p.ParentID == Constant.StandardCode.CURRENCY_CODE);

            hdnDefaultPurchaseOrderType.Value = scDefaultPurchaseOrderType.StandardCodeID;
            hdnDefaultFrancoRegion.Value = scDefaultFrancoRegion.StandardCodeID;
            hdnDefaultCurrencyCode.Value = scDefaultCurrencyCode.StandardCodeID;
        }

        private void EntityToControl(vPurchaseRequestHd entity)
        {
            hdnPurchaseRequestID.Value = entity.PurchaseRequestID.ToString();
            txtOrderNo.Text = entity.PurchaseRequestNo;
            txtItemOrderDate.Text = entity.TransactionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtItemOrderTime.Text = entity.TransactionTime;
            hdnLocationIDFrom.Value = entity.FromLocationID.ToString();
            txtLocationCode.Text = entity.LocationCode;
            txtLocationName.Text = entity.LocationName;
            txtNotes.Text = entity.Remarks;
            
            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnPurchaseRequestID.Value != "")
                filterExpression = string.Format("PurchaseRequestID = {0} AND GCItemDetailStatus = '{1}' AND IsDeleted = 0", hdnPurchaseRequestID.Value, Constant.TransactionStatus.APPROVED);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPurchaseRequestDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }
            lstConversionFactor = hdnListConversionFactor.Value.Split('|');
            lstSelectedMember = hdnSelectedMember.Value.Split('|');
            lstDiscount1 = hdnDiscount1.Value.Split('|');
            lstDiscount2 = hdnDiscount2.Value.Split('|');
            lstPrice = hdnPrice.Value.Split('|');
            lstQty = hdnPurchaseOrderQty.Value.Split('|');
            lstSupplierID = hdnListSupplierID.Value.Split('|');
            lstSupplierName = hdnListSupplierName.Value.Split('|');
            lstGCPurchaseUnit = hdnListGCPurchaseUnit.Value.Split('|');
            lstPurchaseUnit = hdnListPurchaseUnit.Value.Split('|');
            lstTermID = hdnListTermID.Value.Split('|');
            lstSupplierItemName = hdnListSupplierItemName.Value.Split('|');
            List<vPurchaseRequestDtOutstanding> lstEntity = BusinessLayer.GetvPurchaseRequestDtOutstandingList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex," ItemName1 ASC");
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();

        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vPurchaseRequestDtOutstanding entity = e.Item.DataItem as vPurchaseRequestDtOutstanding;
                CheckBox chkIsSelected = (CheckBox)e.Item.FindControl("chkIsSelected");
                HtmlInputHidden hdnSupplierID = (HtmlInputHidden)e.Item.FindControl("hdnSupplierID");
                HtmlInputHidden hdnTermID = (HtmlInputHidden)e.Item.FindControl("hdnTermID");
                HtmlInputHidden hdnGCPurchaseUnit = (HtmlInputHidden)e.Item.FindControl("hdnGCPurchaseUnit");
                HtmlInputHidden hdnConversionFactor = (HtmlInputHidden)e.Item.FindControl("hdnConversionFactor");
                TextBox txtDiscount1 = (TextBox)e.Item.FindControl("txtDiscount1");
                TextBox txtDiscount2 = (TextBox)e.Item.FindControl("txtDiscount2");
                TextBox txtPurchaseQty = (TextBox)e.Item.FindControl("txtPurchaseQty");
                TextBox txtPrice = (TextBox)e.Item.FindControl("txtPrice");
                HtmlGenericControl lblSupplier = (HtmlGenericControl)e.Item.FindControl("lblSupplier");
                HtmlGenericControl lblPurchaseUnit = (HtmlGenericControl)e.Item.FindControl("lblPurchaseUnit");
                HtmlGenericControl lblPurchaseUnitPrice = (HtmlGenericControl)e.Item.FindControl("lblPurchaseUnitPrice");
                HtmlTableCell tdSupplierItemName = (HtmlTableCell)e.Item.FindControl("tdSupplierItemName");
                if (entity.BusinessPartnerID == 0)
                    lblSupplier.InnerHtml = "Pilih Supplier";
                else
                    lblSupplier.InnerHtml = entity.BusinessPartnerName;
                txtPurchaseQty.Text = entity.Quantity.ToString("#,##0.00");
                lblPurchaseUnit.InnerText = entity.PurchaseUnit;
                txtPrice.Text = entity.UnitPrice.ToString("N");
                txtDiscount1.Text = entity.DiscountPercentage.ToString("N");
                hdnSupplierID.Value = entity.BusinessPartnerID.ToString();
                hdnGCPurchaseUnit.Value = entity.GCPurchaseUnit;
                hdnConversionFactor.Value = entity.ConversionFactor.ToString();

                lblSupplier.Attributes.Remove("class");
                lblPurchaseUnit.Attributes.Remove("class");
                if (lstSelectedMember.Contains(entity.ID.ToString()))
                {
                    int idx = Array.IndexOf(lstSelectedMember, entity.ID.ToString());
                    chkIsSelected.Checked = true;
                    txtPrice.ReadOnly = false;
                    txtDiscount1.ReadOnly = false;
                    txtDiscount2.ReadOnly = false;
                    txtPurchaseQty.ReadOnly = false;
                    txtDiscount1.Text = lstDiscount1[idx];
                    txtDiscount2.Text = lstDiscount2[idx];
                    txtPrice.Text = lstPrice[idx];
                    txtPurchaseQty.Text = lstQty[idx];
                    hdnConversionFactor.Value = lstConversionFactor[idx];
                    hdnSupplierID.Value = lstSupplierID[idx];
                    hdnTermID.Value = lstTermID[idx];
                    lblSupplier.InnerHtml = lstSupplierName[idx];
                    hdnGCPurchaseUnit.Value = lstGCPurchaseUnit[idx];
                    lblPurchaseUnit.InnerHtml = lstPurchaseUnit[idx];
                    tdSupplierItemName.InnerHtml = lstSupplierItemName[idx];
                    lblSupplier.Attributes.Add("class", "lblSupplier lblLink");
                    lblPurchaseUnit.Attributes.Add("class", "lblPurchaseUnit lblLink");
                }
                else
                {
                    lblSupplier.Attributes.Add("class", "lblSupplier lblDisabled");
                    lblPurchaseUnit.Attributes.Add("class", "lblPurchaseUnit lblDisabled");
                }
                lblPurchaseUnitPrice.InnerHtml = lblPurchaseUnit.InnerHtml;
            }
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

        private void SavePurchaseOrderHd(IDbContext ctx, ref int purchaseOrderID, ref string retval, int? BusinessPartnerID, int TermID)
        {
            PurchaseOrderHdDao entityHdDao = new PurchaseOrderHdDao(ctx);
            PurchaseOrderHd entityHd = new PurchaseOrderHd();
            if (BusinessPartnerID == 0) BusinessPartnerID = null;
            entityHd.OrderDate = DateTime.Now;         
            entityHd.PurchaseOrderNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.PURCHASE_ORDER, entityHd.OrderDate, ctx);
            if (BusinessPartnerID != null)
            {
                retval += entityHd.PurchaseOrderNo + "^" + BusinessLayer.GetBusinessPartners((int)BusinessPartnerID).BusinessPartnerName + ";";
            }
            else retval += entityHd.PurchaseOrderNo + "^Undefined;";
            entityHd.DeliveryDate = Helper.GetDatePickerValue(txtItemOrderDate.Text);
            entityHd.POExpiredDate = Helper.GetDatePickerValue(txtItemOrderDate.Text);
            entityHd.GCPurchaseOrderType = hdnDefaultPurchaseOrderType.Value;
            entityHd.BusinessPartnerID = BusinessPartnerID;
            entityHd.TermID = TermID > 0 ? TermID : 1;
            entityHd.GCFrancoRegion = hdnDefaultFrancoRegion.Value;
            entityHd.GCCurrencyCode = hdnDefaultCurrencyCode.Value;
            entityHd.CurrencyRate = Convert.ToDecimal(1.00);
            entityHd.IsIncludeVAT = false;
            entityHd.FinalDiscount = Convert.ToDecimal(0.00);
            //entityHd.TaxAmount = Convert.ToDecimal(0.00);
            entityHd.LocationID = Convert.ToInt32(hdnLocationIDFrom.Value);
            entityHd.DownPaymentAmount = Convert.ToDecimal(0.00);
            entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
            ctx.CommandType = CommandType.Text;
            ctx.Command.Parameters.Clear();
            entityHd.CreatedBy = AppSession.UserLogin.UserID;
            entityHdDao.Insert(entityHd);
            purchaseOrderID = BusinessLayer.GetPurchaseOrderHdMaxID(ctx);
        }

        class CPurchaseRequest
        {
            public String ID { get; set; }
            public String Discount1 { get; set; }
            public String Discount2 { get; set; }
            public String Price { get; set; }
            public String QtyPO { get; set; }
            public String SupplierID { get; set; }
            public String GCPurchaseUnit { get; set; }
            public String ConversionFactor { get; set; }
            public String TermID { get; set; }
        }

        #region Getter
        public string GetSelectedMember() { return hdnSelectedMember.Value; }
        public string GetPurchaseOrderQty() { return hdnPurchaseOrderQty.Value; }
        public string GetPrice() { return hdnPrice.Value; }
        public string GetDiscount1() { return hdnDiscount1.Value; }
        public string GetDiscount2() { return hdnDiscount2.Value; }
        public string GetListSupplierID() { return hdnListSupplierID.Value; }
        public string GetListGCPurchaseUnit() { return hdnListGCPurchaseUnit.Value; }
        public string GetListConversionFactor() { return hdnListConversionFactor.Value; }
        public string GetListTermID() { return hdnListTermID.Value; }
        #endregion

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            retval = "";
            bool result = true;
            String[] paramID = hdnSelectedMember.Value.Substring(1).Split('|');
            String[] paramQuantityPO = hdnPurchaseOrderQty.Value.Substring(1).Split('|');
            String[] paramPrice = hdnPrice.Value.Substring(1).Split('|');
            String[] paramDiscount1 = hdnDiscount1.Value.Substring(1).Split('|');
            String[] paramDiscount2 = hdnDiscount2.Value.Substring(1).Split('|');
            String[] paramSupplierID = hdnListSupplierID.Value.Substring(1).Split('|');
            String[] paramGCPurchaseUnit = hdnListGCPurchaseUnit.Value.Substring(1).Split('|');
            String[] paramConversionFactor = hdnListConversionFactor.Value.Substring(1).Split('|');
            String[] paramTermID = hdnListTermID.Value.Substring(1).Split('|');

            List<CPurchaseRequest> listEntityTempPR = new List<CPurchaseRequest>();
            
            IDbContext ctx = DbFactory.Configure(true);
            int purchaseOrderID = 0;
            PurchaseOrderDtDao entityPurchaseOrderDtDao = new PurchaseOrderDtDao(ctx);
            PurchaseRequestDtDao entityPurchaseRequestDtDao = new PurchaseRequestDtDao(ctx);
            PurchaseRequestHdDao entityPurchaseRequestHdDao = new PurchaseRequestHdDao(ctx);
            PurchaseRequestPODao entityPRPODao = new PurchaseRequestPODao(ctx);
            try
            {
                List<PurchaseRequestDt> lstEntityPurchaseReqDt = BusinessLayer.GetPurchaseRequestDtList(string.Format("ID IN ({0})", hdnSelectedMember.Value.Substring(1).Replace('|', ',')));
                if (type == "approve")
                {
                    for (int i = 0; i < paramID.Length; i++)
                    {
                        CPurchaseRequest entityTempPR = new CPurchaseRequest();
                        entityTempPR.ID = paramID[i];
                        entityTempPR.QtyPO = paramQuantityPO[i];
                        entityTempPR.Discount1 = paramDiscount1[i];
                        entityTempPR.Discount2 = paramDiscount2[i];
                        entityTempPR.SupplierID = paramSupplierID[i];
                        entityTempPR.Price = paramPrice[i];
                        entityTempPR.GCPurchaseUnit = paramGCPurchaseUnit[i];
                        entityTempPR.ConversionFactor = paramConversionFactor[i];
                        entityTempPR.TermID = paramTermID[i];
                        listEntityTempPR.Add(entityTempPR);
                    }

                    var lstBusinessPartner = (from p in paramSupplierID
                                              select new { BusinessPartnerID = Convert.ToInt32(p) }).GroupBy(p => p).Select(p => p.First()).ToList();

                    for (int i = 0; i < lstBusinessPartner.Count; ++i)
                    {
                        List<CPurchaseRequest> lstCPRPerSupplier = listEntityTempPR.Where(p => p.SupplierID == lstBusinessPartner[i].BusinessPartnerID.ToString()).ToList();
                        List<PurchaseOrderDt> lstPurchaseOrderDt = new List<PurchaseOrderDt>();

                        SavePurchaseOrderHd(ctx, ref purchaseOrderID, ref retval, (int?)lstBusinessPartner[i].BusinessPartnerID, Convert.ToInt32(lstCPRPerSupplier[0].TermID));
                        foreach (CPurchaseRequest entityCPurchaseReqDt in lstCPRPerSupplier)
                        {
                            PurchaseRequestDt entityPurchaseReqDt = lstEntityPurchaseReqDt.Where(p => p.ID.ToString() == entityCPurchaseReqDt.ID).ToList()[0];
                            entityPurchaseReqDt.GCItemDetailStatus = Constant.TransactionStatus.PROCESSED;
                            PurchaseOrderDt entityPurchaseOrderDt = new PurchaseOrderDt();
                            //entityPurchaseOrderDt.PurchaseRequestID = entityPurchaseReqDt.PurchaseRequestID;
                            entityPurchaseOrderDt.ItemID = entityPurchaseReqDt.ItemID;
                            entityPurchaseOrderDt.Quantity = Convert.ToDecimal(entityCPurchaseReqDt.QtyPO);
                            entityPurchaseOrderDt.GCPurchaseUnit = entityCPurchaseReqDt.GCPurchaseUnit;
                            entityPurchaseOrderDt.GCBaseUnit = entityPurchaseReqDt.GCBaseUnit;
                            entityPurchaseOrderDt.ConversionFactor = Convert.ToDecimal(entityCPurchaseReqDt.ConversionFactor);
                            entityPurchaseOrderDt.UnitPrice = Convert.ToDecimal(entityCPurchaseReqDt.Price);
                            entityPurchaseOrderDt.DiscountPercentage1 = Convert.ToDecimal(entityCPurchaseReqDt.Discount1);
                            entityPurchaseOrderDt.DiscountPercentage2 = Convert.ToDecimal(entityCPurchaseReqDt.Discount2);
                            entityPurchaseOrderDt.IsBonusItem = false;

                            decimal lineAmount = entityPurchaseOrderDt.UnitPrice * entityPurchaseOrderDt.Quantity;
                            lineAmount = lineAmount * (100 - entityPurchaseOrderDt.DiscountPercentage1) / 100;
                            lineAmount = lineAmount * (100 - entityPurchaseOrderDt.DiscountPercentage2) / 100;
                            entityPurchaseOrderDt.LineAmount = lineAmount;
                            entityPurchaseOrderDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                            entityPurchaseOrderDt.CreatedBy = AppSession.UserLogin.UserID;
                            lstPurchaseOrderDt.Add(entityPurchaseOrderDt);
                            entityPurchaseReqDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityPurchaseRequestDtDao.Update(entityPurchaseReqDt);

                            entityPurchaseOrderDt.PurchaseOrderID = purchaseOrderID;
                            entityPurchaseOrderDtDao.Insert(entityPurchaseOrderDt);

                            PurchaseRequestPO entityPRPO = new PurchaseRequestPO();
                            entityPRPO.PurchaseOrderID = purchaseOrderID;
                            entityPRPO.ItemID = entityPurchaseOrderDt.ItemID;
                            entityPRPO.PurchaseRequestID = Convert.ToInt32(hdnPurchaseRequestID.Value);
                            entityPRPO.OrderQuantity = entityPurchaseOrderDt.Quantity;
                            entityPRPODao.Insert(entityPRPO);
                        }
                    }
                }
                else if (type == "decline")
                {
                    foreach (PurchaseRequestDt purchaseEntity in lstEntityPurchaseReqDt)
                    {
                        purchaseEntity.GCItemDetailStatus = Constant.TransactionStatus.VOID;
                        purchaseEntity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityPurchaseRequestDtDao.Update(purchaseEntity);
                    }
                }
                int count = BusinessLayer.GetPurchaseRequestDtRowCount(string.Format("PurchaseRequestID = {0} AND GCItemDetailStatus = '{1}' AND IsDeleted = 0", hdnPurchaseRequestID.Value, Constant.TransactionStatus.APPROVED), ctx);
                retval += "|" + count;
                if (count == 0)
                {
                    PurchaseRequestHd entityPurchaseRequestHd = entityPurchaseRequestHdDao.Get(Convert.ToInt32(hdnPurchaseRequestID.Value));
                    entityPurchaseRequestHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                    entityPurchaseRequestHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityPurchaseRequestHdDao.Update(entityPurchaseRequestHd);
                }
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                Helper.InsertErrorLog(ex);
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
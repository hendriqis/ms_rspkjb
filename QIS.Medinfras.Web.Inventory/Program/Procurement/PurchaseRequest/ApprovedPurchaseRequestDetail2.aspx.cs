using System;
using System.Collections.Generic;
using System.Data;
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
    public partial class ApprovedPurchaseRequestDetail2 : BasePageTrx
    {
        protected int PageCount = 0;
        private string[] lstSelectedMember = null;
        private string[] lstDiscount1 = null;
        private string[] lstDiscount2 = null;
        private string[] lstPrice = null;
        private string[] lstQty = null;
        private string[] lstSupplierID = null;
        private string[] lstSupplierName = null;
        private string[] lstGCPurchaseUnitORI = null;
        private string[] lstGCPurchaseUnit = null;
        private string[] lstPurchaseUnit = null;
        private string[] lstConversionFactorORI = null;
        private string[] lstConversionFactor = null;
        private string[] lstTermID = null;
        private string[] lstIsUrgent = null;
        //private string[] lstSupplierItemName = null;

        protected string filterExpressionLocation = "";

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
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            filterExpressionLocation = string.Format("{0};{1};{2};", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.TransactionCode.PURCHASE_ORDER);

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

            List<StandardCode> listStandardCodePO = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.PURCHASE_ORDER_TYPE));
            Methods.SetComboBoxField<StandardCode>(cboPurchaseOrderType, listStandardCodePO, "StandardCodeName", "StandardCodeID");
            cboPurchaseOrderType.SelectedIndex = 0;

            string filterExpression = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}')",
                                                            AppSession.UserLogin.HealthcareID, //0
                                                            Constant.SettingParameter.IM_REORDER_PR_BY_QTY_END, //1
                                                            Constant.SettingParameter.IM_IS_PURCHASE_REQUEST_SERVICE_UNIT, //2
                                                            Constant.SettingParameter.IM_IS_PO_QTY_CANNOT_OVER_PR_QTY, //3
                                                            Constant.SettingParameter.IM_PURCHASE_REQUEST_LIST_ORDER_BY, //4
                                                            Constant.SettingParameter.IM0126, //5
                                                            Constant.SettingParameter.IM0131 //6
                                                        );
            List<SettingParameterDt> lstSetvarDt = BusinessLayer.GetSettingParameterDtList(filterExpression);

            SettingParameterDt oParam = lstSetvarDt.Where(t => t.ParameterCode == Constant.SettingParameter.IM_REORDER_PR_BY_QTY_END).FirstOrDefault();
            hdnSortByQuantityEND.Value = oParam != null ? oParam.ParameterValue : "0";
            chkIsSortByQtyOnHand.Checked = hdnSortByQuantityEND.Value == "0" ? false : true;

            hdnIM0131.Value = lstSetvarDt.Where(t => t.ParameterCode == Constant.SettingParameter.IM0131).FirstOrDefault().ParameterValue;
            hdnIsUsedPurchaseOrderType.Value = lstSetvarDt.Where(t => t.ParameterCode == Constant.SettingParameter.IM_IS_PURCHASE_REQUEST_SERVICE_UNIT).FirstOrDefault().ParameterValue;
            hdnIsPOQtyCannotOverPRQty.Value = lstSetvarDt.Where(t => t.ParameterCode == Constant.SettingParameter.IM_IS_PO_QTY_CANNOT_OVER_PR_QTY).FirstOrDefault().ParameterValue;
            hdnPurchaseRequestListOrderBy.Value = lstSetvarDt.Where(t => t.ParameterCode == Constant.SettingParameter.IM_PURCHASE_REQUEST_LIST_ORDER_BY).FirstOrDefault().ParameterValue;
            hdnIsSortByItemAndPurchaseUnit.Value = lstSetvarDt.Where(t => t.ParameterCode == Constant.SettingParameter.IM0126).FirstOrDefault().ParameterValue;
            hdnIsUsedProductLine.Value = AppSession.IsUsedProductLine;

            if (hdnIsUsedProductLine.Value == "1")
            {
                trProductLine.Style.Remove("display");
                lblProductLine.Attributes.Add("class", "lblLink lblMandatory");
            }
            else
            {
                trProductLine.Style.Add("display", "none");
                lblProductLine.Attributes.Remove("class");
            }

            if (hdnIsUsedPurchaseOrderType.Value == "1")
            {
                trPurchaseOrderType.Style.Remove("display");
            }

            hdnDefaultPurchaseOrderType.Value = scDefaultPurchaseOrderType.StandardCodeID;
            hdnDefaultFrancoRegion.Value = scDefaultFrancoRegion.StandardCodeID;
            hdnDefaultCurrencyCode.Value = scDefaultCurrencyCode.StandardCodeID;

            SetControlEntrySetting(txtItemOrderDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnPurchaseRequestID.Value != "")
            {
                filterExpression = string.Format("PurchaseRequestID IN ({0}) AND GCItemDetailStatus = '{1}' AND IsDeleted = 0  AND (OrderedQuantity < Quantity)", hdnPurchaseRequestID.Value, Constant.TransactionStatus.APPROVED);

                if (chkIsNotProceedToPOOnly.Checked)
                {
                    filterExpression += " AND OrderedQuantity = 0";
                }

                if (hdnPurchaseRequestListOrderBy.Value == "1")
                {
                    filterExpression += chkIsSortByQtyOnHand.Checked == false ? " ORDER BY BusinessPartnerName, ItemName1" : " ORDER BY QtyOnHandAll ASC";
                }
                else if (hdnPurchaseRequestListOrderBy.Value == "2")
                {
                    filterExpression += chkIsSortByQtyOnHand.Checked == false ? " ORDER BY ItemName1, BusinessPartnerName" : " ORDER BY QtyOnHandAll ASC";
                }

                lstConversionFactorORI = hdnListConversionFactorORI.Value.Split('|');
                lstConversionFactor = hdnListConversionFactor.Value.Split('|');
                lstSelectedMember = hdnSelectedMember.Value.Split(',');
                lstDiscount1 = hdnDiscount1.Value.Split('|');
                lstDiscount2 = hdnDiscount2.Value.Split('|');
                lstPrice = hdnPrice.Value.Split('|');
                lstQty = hdnPurchaseOrderQty.Value.Split('|');
                lstSupplierID = hdnListSupplierID.Value.Split('|');
                lstSupplierName = hdnListSupplierName.Value.Split('|');
                lstGCPurchaseUnitORI = hdnListGCPurchaseUnitORI.Value.Split('|');
                lstGCPurchaseUnit = hdnListGCPurchaseUnit.Value.Split('|');
                lstPurchaseUnit = hdnListPurchaseUnit.Value.Split('|');
                lstTermID = hdnListTermID.Value.Split('|');
                lstIsUrgent = hdnListIsUrgent.Value.Split('|');

                List<vPurchaseRequestDtOutstanding> lstEntity = BusinessLayer.GetvPurchaseRequestDtOutstandingList(filterExpression);
                List<vPurchaseRequestDtOutstanding> lstEntity1 = new List<vPurchaseRequestDtOutstanding>();

                if (hdnIsSortByItemAndPurchaseUnit.Value == "0")
                {
                    lstEntity1 = (from bs in lstEntity
                                  group bs by bs.ItemID into g
                                  select new vPurchaseRequestDtOutstanding
                                  {
                                      ItemID = g.Key,
                                      ItemName1 = g.First().ItemName1,
                                      TransactionDate = g.First().TransactionDate,
                                      TransactionTime = g.First().TransactionTime,
                                      ApprovedDate = g.First().ApprovedDate,
                                      ApprovedByName = g.First().ApprovedByName,
                                      IsUrgent = g.First().IsUrgent,
                                      BusinessPartnerID = g.First().BusinessPartnerID,
                                      BusinessPartnerName = g.First().BusinessPartnerName,
                                      BusinessPartnerShortName = g.First().BusinessPartnerShortName,
                                      QuantityMAX = g.First().QuantityMAX,
                                      QuantityMIN = g.First().QuantityMIN,
                                      QtyOnHandAll = g.First().QtyOnHandAll,
                                      Quantity = g.Sum(x => x.cfRemainingQty),
                                      QtyOnOrder = g.First().QtyOnOrder,
                                      PurchaseUnit = g.First().PurchaseUnit,
                                      ConversionFactor = g.First().ConversionFactor,
                                      QuantityEND = g.First().QuantityEND,
                                      BaseUnit = g.First().BaseUnit,
                                      DefaultGCPurchaseUnit = g.First().DefaultGCPurchaseUnit,
                                      DefaultPurchaseUnit = g.First().DefaultPurchaseUnit,
                                      DefaultConversionFactor = g.First().DefaultConversionFactor,
                                      SupplierItemCode = g.First().SupplierItemCode,
                                      SupplierItemName = g.First().SupplierItemName,
                                      UnitPrice = g.First().UnitPrice,
                                      DiscountPercentage = g.First().DiscountPercentage,
                                      DiscountPercentage2 = g.First().DiscountPercentage2,
                                      GCPurchaseUnit = g.First().GCPurchaseUnit,
                                      GCBaseUnit = g.First().GCBaseUnit,
                                      TermID = g.First().TermID,
                                      Remarks = g.First().Remarks
                                  }).ToList();
                }
                else
                {
                    lstEntity1 = (from bs in lstEntity
                                  group bs by new { bs.ItemID, bs.GCPurchaseUnit } into g
                                  select new vPurchaseRequestDtOutstanding
                                  {
                                      ItemID = g.First().ItemID,
                                      ItemName1 = g.First().ItemName1,
                                      TransactionDate = g.First().TransactionDate,
                                      TransactionTime = g.First().TransactionTime,
                                      ApprovedDate = g.First().ApprovedDate,
                                      ApprovedByName = g.First().ApprovedByName,
                                      IsUrgent = g.First().IsUrgent,
                                      BusinessPartnerID = g.First().BusinessPartnerID,
                                      BusinessPartnerName = g.First().BusinessPartnerName,
                                      BusinessPartnerShortName = g.First().BusinessPartnerShortName,
                                      QuantityMAX = g.First().QuantityMAX,
                                      QuantityMIN = g.First().QuantityMIN,
                                      QtyOnHandAll = g.First().QtyOnHandAll,
                                      Quantity = g.Sum(x => x.cfRemainingQty),
                                      QtyOnOrder = g.First().QtyOnOrder,
                                      PurchaseUnit = g.First().PurchaseUnit,
                                      ConversionFactor = g.First().ConversionFactor,
                                      QuantityEND = g.First().QuantityEND,
                                      BaseUnit = g.First().BaseUnit,
                                      DefaultGCPurchaseUnit = g.First().DefaultGCPurchaseUnit,
                                      DefaultPurchaseUnit = g.First().DefaultPurchaseUnit,
                                      DefaultConversionFactor = g.First().DefaultConversionFactor,
                                      SupplierItemCode = g.First().SupplierItemCode,
                                      SupplierItemName = g.First().SupplierItemName,
                                      UnitPrice = g.First().UnitPrice,
                                      DiscountPercentage = g.First().DiscountPercentage,
                                      DiscountPercentage2 = g.First().DiscountPercentage2,
                                      GCPurchaseUnit = g.First().GCPurchaseUnit,
                                      GCBaseUnit = g.First().GCBaseUnit,
                                      TermID = g.First().TermID,
                                      Remarks = g.First().Remarks
                                  }).ToList();
                }

                lvwView.DataSource = lstEntity1;
                lvwView.DataBind();
            }
            else
            {
                List<vPurchaseRequestDtOutstanding> lstEntity1 = new List<vPurchaseRequestDtOutstanding>();
                lvwView.DataSource = lstEntity1;
                lvwView.DataBind();
            }
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vPurchaseRequestDtOutstanding entity = e.Item.DataItem as vPurchaseRequestDtOutstanding;
                CheckBox chkIsSelected = (CheckBox)e.Item.FindControl("chkIsSelected");
                HtmlInputHidden hdnSupplierID = (HtmlInputHidden)e.Item.FindControl("hdnSupplierID");
                HtmlInputHidden hdnTermID = (HtmlInputHidden)e.Item.FindControl("hdnTermID");
                HtmlInputHidden hdnIsUrgent = (HtmlInputHidden)e.Item.FindControl("hdnIsUrgent");
                HtmlInputHidden hdnGCPurchaseUnit = (HtmlInputHidden)e.Item.FindControl("hdnGCPurchaseUnit");
                HtmlInputHidden hdnGCPurchaseUnitORI = (HtmlInputHidden)e.Item.FindControl("hdnGCPurchaseUnitORI");
                HtmlInputHidden hdnConversionFactor = (HtmlInputHidden)e.Item.FindControl("hdnConversionFactor");
                HtmlInputHidden hdnConversionFactorORI = (HtmlInputHidden)e.Item.FindControl("hdnConversionFactorORI");
                TextBox txtDiscount1 = (TextBox)e.Item.FindControl("txtDiscount1");
                TextBox txtDiscount2 = (TextBox)e.Item.FindControl("txtDiscount2");
                TextBox txtPurchaseQty = (TextBox)e.Item.FindControl("txtPurchaseQty");
                TextBox txtPrice = (TextBox)e.Item.FindControl("txtPrice");
                HtmlGenericControl lblSupplier = (HtmlGenericControl)e.Item.FindControl("lblSupplier");
                HtmlGenericControl lblPurchaseUnit = (HtmlGenericControl)e.Item.FindControl("lblPurchaseUnit");

                hdnSupplierID.Value = entity.BusinessPartnerID.ToString();
                if (entity.BusinessPartnerID == 0)
                    lblSupplier.InnerHtml = "Pilih Supplier";
                else
                    lblSupplier.InnerHtml = string.IsNullOrEmpty(entity.BusinessPartnerName) ? entity.BusinessPartnerName : entity.BusinessPartnerName;
                txtPurchaseQty.Text = entity.cfRemainingQtyBaseUnit.ToString();
                lblPurchaseUnit.InnerText = entity.DefaultPurchaseUnit;
                txtPrice.Text = entity.cfUnitPriceBaseUnit.ToString("N2");
                txtDiscount1.Text = entity.DiscountPercentage.ToString("N2");
                txtDiscount2.Text = entity.DiscountPercentage2.ToString("N2");
                hdnGCPurchaseUnit.Value = entity.DefaultGCPurchaseUnit;
                hdnGCPurchaseUnitORI.Value = entity.GCPurchaseUnit;
                hdnConversionFactor.Value = entity.DefaultConversionFactor.ToString();
                hdnConversionFactorORI.Value = entity.ConversionFactor.ToString();

                lblSupplier.Attributes.Remove("class");
                lblPurchaseUnit.Attributes.Remove("class");
                if (lstSelectedMember.Contains(entity.ItemID.ToString()))
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
                    hdnConversionFactorORI.Value = lstConversionFactorORI[idx];
                    hdnConversionFactor.Value = lstConversionFactor[idx];
                    hdnSupplierID.Value = lstSupplierID[idx];
                    hdnTermID.Value = lstTermID[idx];
                    hdnIsUrgent.Value = lstIsUrgent[idx];
                    lblSupplier.InnerHtml = lstSupplierName[idx];
                    hdnGCPurchaseUnitORI.Value = lstGCPurchaseUnitORI[idx];
                    hdnGCPurchaseUnit.Value = lstGCPurchaseUnit[idx];
                    lblPurchaseUnit.InnerHtml = lstPurchaseUnit[idx];
                    lblSupplier.Attributes.Add("class", "lblSupplier lblLink");
                    lblPurchaseUnit.Attributes.Add("class", "lblPurchaseUnit lblLink");
                }
                else
                {
                    lblSupplier.Attributes.Add("class", "lblSupplier lblDisabled");
                    lblPurchaseUnit.Attributes.Add("class", "lblPurchaseUnit lblDisabled");
                }
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

        private void SavePurchaseOrderHd(IDbContext ctx, ref int purchaseOrderID, ref string retval, int? BusinessPartnerID, int TermID, String IsUrgent)
        {
            PurchaseOrderHdDao entityHdDao = new PurchaseOrderHdDao(ctx);
            PurchaseRequestHdDao entityPurchaseRequestHdDao = new PurchaseRequestHdDao(ctx);
            PurchaseOrderHd entityHd = new PurchaseOrderHd();

            if (BusinessPartnerID == 0) BusinessPartnerID = null;

            entityHd.OrderDate = Helper.GetDatePickerValue(txtItemOrderDate.Text);

            entityHd.TransactionCode = Constant.TransactionCode.PURCHASE_ORDER;
            entityHd.PurchaseOrderNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.PURCHASE_ORDER, entityHd.OrderDate, ctx);

            if (BusinessPartnerID == 0)
            {
                BusinessPartnerID = null;
            }

            if (BusinessPartnerID != null)
            {
                BusinessPartners bp = BusinessLayer.GetBusinessPartners((int)BusinessPartnerID);

                retval += entityHd.PurchaseOrderNo + "^" + bp.BusinessPartnerName + ";";

                entityHd.IsIncludeVAT = bp.IsTaxable;

                if (entityHd.IsIncludeVAT)
                {
                    string VATPercent = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.VAT_PERCENTAGE).ParameterValue;
                    if (VATPercent != "" && VATPercent != null)
                    {
                        entityHd.VATPercentage = Convert.ToDecimal(VATPercent);
                    }
                }
            }
            else
            {
                retval += entityHd.PurchaseOrderNo + "^Undefined;";
            }

            entityHd.DeliveryDate = DateTime.Now;
            entityHd.POExpiredDate = DateTime.Now;

            if (cboPurchaseOrderType.Value != null)
            {
                entityHd.GCPurchaseOrderType = Convert.ToString(cboPurchaseOrderType.Value);
            }
            else
            {
                entityHd.GCPurchaseOrderType = hdnDefaultPurchaseOrderType.Value;
            }

            entityHd.BusinessPartnerID = BusinessPartnerID;

            Term t = BusinessLayer.GetTermList("IsDeleted = 0").FirstOrDefault();
            entityHd.TermID = TermID > 0 ? TermID : t.TermID;

            if (IsUrgent == "True")
            {
                entityHd.IsUrgent = true;
            }
            else
            {
                entityHd.IsUrgent = false;
            }

            entityHd.GCFrancoRegion = hdnDefaultFrancoRegion.Value;
            entityHd.GCCurrencyCode = hdnDefaultCurrencyCode.Value;
            entityHd.CurrencyRate = Convert.ToDecimal(1.00);
            entityHd.FinalDiscount = Convert.ToDecimal(0.00);
            entityHd.LocationID = Convert.ToInt32(hdnLocationIDFrom.Value);
            entityHd.DownPaymentAmount = Convert.ToDecimal(0.00);
            entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;

            if (hdnIsUsedProductLine.Value == "1")
            {
                entityHd.ProductLineID = Convert.ToInt32(hdnProductLineID.Value);
            }
            entityHd.IsPPHInPercentage = true;

            ctx.CommandType = CommandType.Text;
            ctx.Command.Parameters.Clear();
            entityHd.CreatedBy = AppSession.UserLogin.UserID;
            purchaseOrderID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
        }

        class CPurchaseRequest
        {
            public String ItemID { get; set; }
            public String Discount1 { get; set; }
            public String Discount2 { get; set; }
            public String Price { get; set; }
            public String QtyPO { get; set; }
            public String SupplierID { get; set; }
            public String GCPurchaseUnit { get; set; }
            public String ConversionFactor { get; set; }
            public String TermID { get; set; }
            public String IsUrgent { get; set; }
            public Int32 ProductLineID { get; set; }
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
            String[] paramID = hdnSelectedMember.Value.Substring(1).Split(',');
            String[] paramQuantityPO = hdnPurchaseOrderQty.Value.Substring(1).Split('|');
            String[] paramPrice = hdnPrice.Value.Substring(1).Split('|');
            String[] paramDiscount1 = hdnDiscount1.Value.Substring(1).Split('|');
            String[] paramDiscount2 = hdnDiscount2.Value.Substring(1).Split('|');
            String[] paramSupplierID = hdnListSupplierID.Value.Substring(1).Split('|');
            String[] paramGCPurchaseUnit = hdnListGCPurchaseUnit.Value.Substring(1).Split('|');
            String[] paramConversionFactor = hdnListConversionFactor.Value.Substring(1).Split('|');
            String[] paramTermID = hdnListTermID.Value.Substring(1).Split('|');
            String[] paramIsUrgent = hdnListIsUrgent.Value.Substring(1).Split('|');

            List<CPurchaseRequest> listEntityTempPR = new List<CPurchaseRequest>();

            IDbContext ctx = DbFactory.Configure(true);
            int purchaseOrderID = 0;
            PurchaseOrderDtDao entityPurchaseOrderDtDao = new PurchaseOrderDtDao(ctx);
            PurchaseRequestDtDao entityPurchaseRequestDtDao = new PurchaseRequestDtDao(ctx);
            PurchaseRequestHdDao entityPurchaseRequestHdDao = new PurchaseRequestHdDao(ctx);
            PurchaseRequestPODao entityPRPODao = new PurchaseRequestPODao(ctx);
            try
            {
                List<PurchaseRequestDt> lstEntityPurchaseReqDt = BusinessLayer.GetPurchaseRequestDtList(string.Format(
                    "PurchaseRequestID IN ({0}) AND ItemID IN ({1}) AND IsDeleted = 0 AND GCItemDetailStatus = '{2}' AND (OrderedQuantity < Quantity)",
                    hdnPurchaseRequestID.Value, hdnSelectedMember.Value.Substring(1).Replace('|', ','), Constant.TransactionStatus.APPROVED));
                if (type == "approve")
                {
                    #region Approve

                    for (int i = 0; i < paramID.Length; i++)
                    {
                        CPurchaseRequest entityTempPR = new CPurchaseRequest();
                        entityTempPR.ItemID = paramID[i];
                        entityTempPR.QtyPO = paramQuantityPO[i];
                        entityTempPR.Discount1 = paramDiscount1[i];
                        entityTempPR.Discount2 = paramDiscount2[i];
                        entityTempPR.SupplierID = paramSupplierID[i];
                        entityTempPR.Price = paramPrice[i];
                        entityTempPR.GCPurchaseUnit = paramGCPurchaseUnit[i];
                        entityTempPR.ConversionFactor = paramConversionFactor[i];
                        entityTempPR.TermID = paramTermID[i];
                        entityTempPR.IsUrgent = paramIsUrgent[i];
                        listEntityTempPR.Add(entityTempPR);
                    }

                    var lstBusinessPartner = (from p in paramSupplierID
                                              select new { BusinessPartnerID = Convert.ToInt32(p) }).GroupBy(p => p).Select(p => p.First()).ToList();

                    if (hdnIsSortByItemAndPurchaseUnit.Value == "1")
                    {
                        lstBusinessPartner = (from p in paramSupplierID
                                              select new { BusinessPartnerID = Convert.ToInt32(p) }).ToList();
                    }

                    for (int i = 0; i < lstBusinessPartner.Count; ++i)
                    {
                        List<CPurchaseRequest> lstCPRPerSupplier = listEntityTempPR.Where(p => p.SupplierID == lstBusinessPartner[i].BusinessPartnerID.ToString()).ToList();
                        if (hdnIsSortByItemAndPurchaseUnit.Value == "1")
                        {
                            lstCPRPerSupplier = new List<CPurchaseRequest>();
                            lstCPRPerSupplier.Add(listEntityTempPR.Where(p => p.SupplierID == lstBusinessPartner[i].BusinessPartnerID.ToString()).ToList()[i]);
                        }
                        List<PurchaseOrderDt> lstPurchaseOrderDt = new List<PurchaseOrderDt>();

                        SavePurchaseOrderHd(ctx, ref purchaseOrderID, ref retval, (int?)lstBusinessPartner[i].BusinessPartnerID, Convert.ToInt32(lstCPRPerSupplier[0].TermID), lstCPRPerSupplier[0].IsUrgent);
                        foreach (CPurchaseRequest entityCPurchaseReqDt in lstCPRPerSupplier)
                        {

                            List<PurchaseRequestDt> lstEntityPurchaseReqDtPerItem = lstEntityPurchaseReqDt.Where(p => p.ItemID.ToString() == entityCPurchaseReqDt.ItemID).ToList();
                            if (hdnIsSortByItemAndPurchaseUnit.Value == "1")
                            {
                                lstEntityPurchaseReqDtPerItem = new List<PurchaseRequestDt>();
                                lstEntityPurchaseReqDtPerItem.Add(lstEntityPurchaseReqDt.Where(p => p.ItemID.ToString() == entityCPurchaseReqDt.ItemID).ToList()[i]);
                            }
                            PurchaseRequestDt entityPurchaseReqDt = lstEntityPurchaseReqDtPerItem[0];
                            PurchaseOrderDt entityPurchaseOrderDt = new PurchaseOrderDt();
                            entityPurchaseOrderDt.PurchaseRequestID = entityPurchaseReqDt.PurchaseRequestID;
                            entityPurchaseOrderDt.ItemID = entityPurchaseReqDt.ItemID;
                            entityPurchaseOrderDt.Quantity = Convert.ToDecimal(entityCPurchaseReqDt.QtyPO);
                            entityPurchaseOrderDt.GCPurchaseUnit = entityCPurchaseReqDt.GCPurchaseUnit;
                            entityPurchaseOrderDt.GCBaseUnit = entityPurchaseReqDt.GCBaseUnit;
                            entityPurchaseOrderDt.ConversionFactor = Convert.ToDecimal(entityCPurchaseReqDt.ConversionFactor);
                            entityPurchaseOrderDt.UnitPrice = Convert.ToDecimal(entityCPurchaseReqDt.Price);

                            entityPurchaseOrderDt.DiscountPercentage1 = Convert.ToDecimal(entityCPurchaseReqDt.Discount1);
                            entityPurchaseOrderDt.IsDiscountInPercentage1 = entityPurchaseOrderDt.DiscountPercentage1 > 0 ? true : false;
                            entityPurchaseOrderDt.DiscountAmount1 = Convert.ToDecimal((entityPurchaseOrderDt.Quantity * entityPurchaseOrderDt.UnitPrice) * entityPurchaseOrderDt.DiscountPercentage1 / 100);

                            entityPurchaseOrderDt.DiscountPercentage2 = Convert.ToDecimal(entityCPurchaseReqDt.Discount2);
                            entityPurchaseOrderDt.IsDiscountInPercentage2 = entityPurchaseOrderDt.DiscountPercentage2 > 0 ? true : false;
                            entityPurchaseOrderDt.DiscountAmount2 = Convert.ToDecimal(((entityPurchaseOrderDt.Quantity * entityPurchaseOrderDt.UnitPrice) - entityPurchaseOrderDt.DiscountAmount1) * entityPurchaseOrderDt.DiscountPercentage2 / 100);

                            entityPurchaseOrderDt.IsBonusItem = false;

                            decimal lineAmount = entityPurchaseOrderDt.UnitPrice * entityPurchaseOrderDt.Quantity;
                            entityPurchaseOrderDt.DiscountAmount1 = Convert.ToDecimal(entityCPurchaseReqDt.Discount1) * lineAmount / 100;
                            entityPurchaseOrderDt.DiscountAmount2 = Convert.ToDecimal(entityCPurchaseReqDt.Discount2) * (lineAmount - entityPurchaseOrderDt.DiscountAmount1) / 100;
                            lineAmount = lineAmount - entityPurchaseOrderDt.DiscountAmount1 - entityPurchaseOrderDt.DiscountAmount2;

                            entityPurchaseOrderDt.DraftQuantity = entityPurchaseOrderDt.Quantity;
                            entityPurchaseOrderDt.DraftUnitPrice = entityPurchaseOrderDt.UnitPrice;

                            entityPurchaseOrderDt.IsDraftDiscountInPercentage1 = entityPurchaseOrderDt.IsDiscountInPercentage1;
                            entityPurchaseOrderDt.DraftDiscountPercentage1 = entityPurchaseOrderDt.DiscountPercentage1;
                            entityPurchaseOrderDt.DraftDiscountAmount1 = entityPurchaseOrderDt.DiscountAmount1;

                            entityPurchaseOrderDt.IsDraftDiscountInPercentage2 = entityPurchaseOrderDt.IsDiscountInPercentage2;
                            entityPurchaseOrderDt.DraftDiscountPercentage2 = entityPurchaseOrderDt.DiscountPercentage2;
                            entityPurchaseOrderDt.DraftDiscountAmount2 = entityPurchaseOrderDt.DiscountAmount2;

                            entityPurchaseOrderDt.LineAmount = lineAmount;
                            entityPurchaseOrderDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                            entityPurchaseOrderDt.CreatedBy = AppSession.UserLogin.UserID;
                            lstPurchaseOrderDt.Add(entityPurchaseOrderDt);

                            entityPurchaseOrderDt.PurchaseOrderID = purchaseOrderID;
                            entityPurchaseOrderDtDao.Insert(entityPurchaseOrderDt);

                            int counterPRDt = 1;
                            int countDataPRDt = lstEntityPurchaseReqDt.Where(p => p.ItemID.ToString() == entityCPurchaseReqDt.ItemID).ToList().Count();

                            decimal orderQuantity = entityPurchaseOrderDt.Quantity;
                            foreach (PurchaseRequestDt purchaseRequestDt in lstEntityPurchaseReqDt.Where(p => p.ItemID.ToString() == entityCPurchaseReqDt.ItemID).ToList())
                            {
                                if (purchaseRequestDt.GCItemDetailStatus == Constant.TransactionStatus.APPROVED)
                                {
                                    decimal tempOrderQuantity = orderQuantity;
                                    decimal remainingOrderQuantity = (purchaseRequestDt.Quantity - purchaseRequestDt.OrderedQuantity);

                                    if (entityPurchaseOrderDt.ConversionFactor != entityPurchaseReqDt.ConversionFactor)
                                    {
                                        tempOrderQuantity = tempOrderQuantity * entityPurchaseOrderDt.ConversionFactor / entityPurchaseReqDt.ConversionFactor;
                                    }

                                    if (tempOrderQuantity > remainingOrderQuantity)
                                    {
                                        if (counterPRDt == countDataPRDt)
                                        {
                                            if (hdnIsPOQtyCannotOverPRQty.Value == "1")
                                            {
                                                tempOrderQuantity = remainingOrderQuantity;
                                            }
                                            else
                                            {
                                                tempOrderQuantity = purchaseRequestDt.Quantity;
                                            }
                                        }
                                        else
                                        {
                                            tempOrderQuantity = remainingOrderQuantity;
                                        }
                                    }

                                    PurchaseRequestPO entityPRPO = new PurchaseRequestPO();
                                    entityPRPO.PurchaseOrderID = purchaseOrderID;
                                    entityPRPO.ItemID = purchaseRequestDt.ItemID;
                                    entityPRPO.PurchaseRequestID = purchaseRequestDt.PurchaseRequestID;
                                    entityPRPO.OrderQuantity = tempOrderQuantity;

                                    purchaseRequestDt.OrderedQuantity = purchaseRequestDt.OrderedQuantity + tempOrderQuantity;

                                    string orderInformation = !string.IsNullOrEmpty(purchaseRequestDt.OrderInformation) ? purchaseRequestDt.OrderInformation + "|" : "|";
                                    purchaseRequestDt.OrderInformation = string.Format("{0}{1}", orderInformation, entityPurchaseOrderDt.PurchaseOrderID);

                                    purchaseRequestDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityPurchaseRequestDtDao.Update(purchaseRequestDt);

                                    orderQuantity -= tempOrderQuantity;

                                    entityPRPO.RequestQuantity = purchaseRequestDt.Quantity;
                                    entityPRPODao.Insert(entityPRPO);

                                    if (orderQuantity == 0) break;
                                }
                                else
                                {
                                    errMessage = "Detail permintaan pembelian ini tidak dapat diproses karena sudah diproses, harap refresh halaman ini.";
                                    result = false;
                                    break;
                                }

                                counterPRDt++;
                            }

                            if (!result)
                            {
                                break;
                            }
                        }
                    }

                    #endregion
                }
                else if (type == "decline")
                {
                    #region Decline

                    foreach (PurchaseRequestDt purchaseEntity in lstEntityPurchaseReqDt)
                    {
                        PurchaseRequestHd prHd = entityPurchaseRequestHdDao.Get(purchaseEntity.PurchaseRequestID);

                        if ((prHd.GLTransactionIDRequest != null && prHd.GLTransactionIDRequest != 0) || (prHd.GLTransactionDtIDRequest != null && prHd.GLTransactionDtIDRequest != 0))
                        {
                            result = false;
                            errMessage = "Permintaan pembelian dgn nomor <b>" + prHd.PurchaseRequestNo + "</b> tidak dapat diubah karena sudah diproses permintaan kas bon.";
                            break;
                        }
                        else
                        {
                            if (purchaseEntity.OrderedQuantity == 0)
                            {
                                if (purchaseEntity.GCItemDetailStatus == Constant.TransactionStatus.APPROVED)
                                {
                                    purchaseEntity.GCItemDetailStatus = Constant.TransactionStatus.VOID;
                                    purchaseEntity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityPurchaseRequestDtDao.Update(purchaseEntity);
                                }
                                else
                                {
                                    errMessage = "Detail permintaan pembelian ini tidak dapat diproses karena sudah diproses, harap refresh halaman ini.";
                                    result = false;
                                    break;
                                }
                            }
                            else
                            {
                                errMessage = "Detail permintaan pembelian ini tidak dapat dilakukan DECLINE karena sudah pernah diproses sebagai PEMESANAN BARANG, harap refresh halaman ini.";
                                result = false;
                                break;
                            }
                        }
                    }

                    #endregion
                }
                else if (type == "close")
                {
                    #region Close

                    foreach (PurchaseRequestDt purchaseEntity in lstEntityPurchaseReqDt)
                    {
                        if (purchaseEntity.GCItemDetailStatus == Constant.TransactionStatus.APPROVED)
                        {
                            purchaseEntity.GCItemDetailStatus = Constant.TransactionStatus.CLOSED;
                            purchaseEntity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityPurchaseRequestDtDao.Update(purchaseEntity);
                        }
                        else
                        {
                            errMessage = "Detail permintaan pembelian ini tidak dapat diproses karena sudah diproses, harap refresh halaman ini.";
                            result = false;
                            break;
                        }
                    }

                    #endregion
                }
                else if (type == "save")
                {
                    #region Save

                    for (int i = 0; i < paramID.Length; i++)
                    {
                        CPurchaseRequest entityTempPR = new CPurchaseRequest();
                        entityTempPR.ItemID = paramID[i];
                        entityTempPR.QtyPO = paramQuantityPO[i];
                        entityTempPR.Discount1 = paramDiscount1[i];
                        entityTempPR.Discount2 = paramDiscount2[i];
                        entityTempPR.SupplierID = paramSupplierID[i];
                        entityTempPR.Price = paramPrice[i];
                        entityTempPR.GCPurchaseUnit = paramGCPurchaseUnit[i];
                        entityTempPR.ConversionFactor = paramConversionFactor[i];
                        entityTempPR.TermID = paramTermID[i];
                        entityTempPR.IsUrgent = paramIsUrgent[i];
                        listEntityTempPR.Add(entityTempPR);
                    }

                    var lstBusinessPartner = (from p in paramSupplierID
                                              select new { BusinessPartnerID = Convert.ToInt32(p) }).GroupBy(p => p).Select(p => p.First()).ToList();

                    for (int i = 0; i < lstBusinessPartner.Count; ++i)
                    {
                        List<CPurchaseRequest> lstCPRPerSupplier = listEntityTempPR.Where(p => p.SupplierID == lstBusinessPartner[i].BusinessPartnerID.ToString()).ToList();

                        foreach (CPurchaseRequest entityCPurchaseReqDt in lstCPRPerSupplier)
                        {
                            List<PurchaseRequestDt> lstEntityPurchaseReqDtPerItem = lstEntityPurchaseReqDt.Where(p => p.ItemID.ToString() == entityCPurchaseReqDt.ItemID).ToList();
                            foreach (PurchaseRequestDt purchaseRequestDt in lstEntityPurchaseReqDtPerItem)
                            {
                                if (purchaseRequestDt.OrderedQuantity == 0)
                                {
                                    if (purchaseRequestDt.GCItemDetailStatus == Constant.TransactionStatus.APPROVED)
                                    {
                                        purchaseRequestDt.BusinessPartnerID = Convert.ToInt32(entityCPurchaseReqDt.SupplierID);
                                        purchaseRequestDt.Quantity = Convert.ToDecimal(entityCPurchaseReqDt.QtyPO);
                                        purchaseRequestDt.GCPurchaseUnit = entityCPurchaseReqDt.GCPurchaseUnit;
                                        purchaseRequestDt.ConversionFactor = Convert.ToDecimal(entityCPurchaseReqDt.ConversionFactor);
                                        purchaseRequestDt.UnitPrice = Convert.ToDecimal(entityCPurchaseReqDt.Price);
                                        purchaseRequestDt.DiscountPercentage = Convert.ToDecimal(entityCPurchaseReqDt.Discount1);
                                        purchaseRequestDt.DiscountPercentage2 = Convert.ToDecimal(entityCPurchaseReqDt.Discount2);
                                        entityPurchaseRequestDtDao.Update(purchaseRequestDt);
                                    }
                                    else
                                    {
                                        errMessage = "Detail permintaan pembelian ini tidak dapat diproses karena sudah diproses, harap refresh halaman ini.";
                                        result = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    errMessage = "Detail permintaan pembelian ini tidak dapat dilakukan SIMPAN karena sudah pernah diproses sebagai PEMESANAN BARANG, harap refresh halaman ini.";
                                    result = false;
                                    break;
                                }
                            }

                            if (!result)
                            {
                                break;
                            }
                        }
                    }

                    #endregion
                }

                if (result)
                {
                    string[] lstPurchaseRequestID = hdnPurchaseRequestID.Value.Split(',');
                    string tempPurchaseRequestID = "";
                    retval += "|";
                    foreach (string purchaseRequestID in lstPurchaseRequestID)
                    {
                        int countApproved = BusinessLayer.GetPurchaseRequestDtRowCount(string.Format(
                            "PurchaseRequestID = {0} AND GCItemDetailStatus = '{1}' AND IsDeleted = 0", purchaseRequestID, Constant.TransactionStatus.APPROVED), ctx);
                        int countNonDecline = BusinessLayer.GetPurchaseRequestDtRowCount(string.Format(
                            "PurchaseRequestID = {0} AND GCItemDetailStatus != '{1}' AND IsDeleted = 0", purchaseRequestID, Constant.TransactionStatus.VOID), ctx);
                        retval += ";" + countApproved;
                        if (countNonDecline == 0)
                        {
                            PurchaseRequestHd entityPurchaseRequestHd = entityPurchaseRequestHdDao.Get(Convert.ToInt32(purchaseRequestID));
                            entityPurchaseRequestHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                            entityPurchaseRequestHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityPurchaseRequestHdDao.Update(entityPurchaseRequestHd);
                        }
                        else
                        {
                            if (countApproved == 0)
                            {
                                PurchaseRequestHd entityPurchaseRequestHd = entityPurchaseRequestHdDao.Get(Convert.ToInt32(purchaseRequestID));
                                entityPurchaseRequestHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                                entityPurchaseRequestHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityPurchaseRequestHdDao.Update(entityPurchaseRequestHd);
                            }
                            else { tempPurchaseRequestID += purchaseRequestID + ","; }
                        }
                    }

                    ctx.CommitTransaction();

                    List<vPurchaseRequestHd> tempPurchase = new List<vPurchaseRequestHd>();
                    if (tempPurchaseRequestID != string.Empty)
                    {
                        tempPurchaseRequestID = tempPurchaseRequestID.Remove(tempPurchaseRequestID.Length - 1);
                        tempPurchase = BusinessLayer.GetvPurchaseRequestHdList(string.Format("PurchaseRequestID IN ({0})", tempPurchaseRequestID));
                    }
                    retval += "|" + tempPurchaseRequestID;
                    string tempPurchaseRequestNo = "";
                    foreach (vPurchaseRequestHd entTemp in tempPurchase)
                    {
                        tempPurchaseRequestNo += entTemp.PurchaseRequestNo + ",";
                    }
                    if (tempPurchaseRequestNo != string.Empty) tempPurchaseRequestNo = tempPurchaseRequestNo.Remove(tempPurchaseRequestNo.Length - 1);
                    retval += "|" + tempPurchaseRequestNo;
                    retval += "|" + hdnLocationIDFrom.Value;
                }
                else
                {
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

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public string OnGetFilterExpressionDetailInfo()
        {
            return string.Format("LocationID = {0}", hdnLocationIDFrom.Value);
        }
    }
}
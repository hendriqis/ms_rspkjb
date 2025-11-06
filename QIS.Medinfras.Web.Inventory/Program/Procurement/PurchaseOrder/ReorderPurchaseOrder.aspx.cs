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
    public partial class ReorderPurchaseOrder : BasePageTrx
    {
        protected string filterExpressionSupplier = "";
        protected string filterExpressionLocation = "";
        private string[] lstSelectedMember = null;
        private string[] lstQtyPurchaseOrder = null;
        private string[] lstSupplierID = null;

        protected int PageCount = 1;

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.REORDER_PURCHASE_ORDER;
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
            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                                                    "HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
                                                                                    AppSession.UserLogin.HealthcareID,
                                                                                    Constant.SettingParameter.IM0131
                                                                                ));

            hdnIM0131.Value = lstSettingParameterDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.IM0131).FirstOrDefault().ParameterValue;

            filterExpressionLocation = string.Format("{0};{1};{2};", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.TransactionCode.PURCHASE_ORDER);
            List<GetLocationUserList> lstLocation = BusinessLayer.GetLocationUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.TransactionCode.PURCHASE_ORDER, "");
            if (lstLocation.Count == 1)
            {
                trLocation.Style.Add("display", "none");
                hdnLocationID.Value = lstLocation[0].LocationID.ToString();
                txtLocationCode.Text = lstLocation[0].LocationCode;
                txtLocationName.Text = lstLocation[0].LocationName;
            }
            filterExpressionSupplier = string.Format("GCBusinessPartnerType = '{0}' AND IsDeleted = 0", Constant.BusinessObjectType.SUPPLIER);

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

            SetControlProperties();
            BindGridView(1, true, ref PageCount);
            hdnPageCount.Value = PageCount.ToString();
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> listStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}','{2}') AND IsDeleted = 0", Constant.StandardCode.PURCHASE_ORDER_TYPE, Constant.StandardCode.FRANCO_REGION, Constant.StandardCode.CURRENCY_CODE));
            List<Term> listTerm = BusinessLayer.GetTermList(string.Format("isDeleted = 0"));
            List<Variable> listFilter = new List<Variable>
            {
                new Variable(){Code="Semua", Value="1"},
                new Variable(){Code="Perlu di Order", Value="2"}, 
                new Variable(){Code="Tidak Perlu di Order", Value="3"}
            };

            Methods.SetComboBoxField<StandardCode>(cboPurchaseOrderType, listStandardCode.Where(p => p.ParentID == Constant.StandardCode.PURCHASE_ORDER_TYPE).ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboFrancoRegion, listStandardCode.Where(p => p.ParentID == Constant.StandardCode.FRANCO_REGION).ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboCurrency, listStandardCode.Where(p => p.ParentID == Constant.StandardCode.CURRENCY_CODE).ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<Variable>(cboFilter, listFilter, "Code", "Value");
            Methods.SetComboBoxField<Term>(cboTerm, listTerm, "TermName", "TermID");
            cboFilter.SelectedIndex = 0;
            cboPurchaseOrderType.SelectedIndex = 0;
            cboFrancoRegion.SelectedIndex = 0;
            cboCurrency.SelectedIndex = 0;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtItemOrderDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtItemOrderDeliveryDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtItemOrderExpiredDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(cboPurchaseOrderType, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(cboTerm, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(cboFrancoRegion, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(cboCurrency, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtKurs, new ControlEntrySetting(true, true, true, "1.00"));

            if (hdnIsUsedProductLine.Value == "1")
            {
                SetControlEntrySetting(lblProductLine, new ControlEntrySetting(true, false));
                SetControlEntrySetting(hdnProductLineID, new ControlEntrySetting(true, true, true));
                SetControlEntrySetting(hdnProductLineItemType, new ControlEntrySetting(true, true, true));
                SetControlEntrySetting(txtProductLineCode, new ControlEntrySetting(true, false, true));
                SetControlEntrySetting(txtProductLineName, new ControlEntrySetting(false, false, true));
            }
            else
            {
                SetControlEntrySetting(lblProductLine, new ControlEntrySetting(true, true));
                SetControlEntrySetting(hdnProductLineID, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(hdnProductLineItemType, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(txtProductLineCode, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(txtProductLineName, new ControlEntrySetting(false, false, false));
            }
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vItemBalanceReorderPO entity = e.Row.DataItem as vItemBalanceReorderPO;
                TextBox txtPurchaseOrder = e.Row.FindControl("txtPurchaseOrder") as TextBox;
                CheckBox chkIsSelected = (CheckBox)e.Row.FindControl("chkIsSelected");
                HtmlInputHidden hdnSupplierID = (HtmlInputHidden)e.Row.FindControl("hdnSupplierID");
                Decimal autoQty = (entity.QuantityMAX - entity.QuantityEND - entity.PurchaseOrderQtyOnOrder);
                if (autoQty < 0) autoQty = 0;
                txtPurchaseOrder.Text = autoQty.ToString("N");
                HtmlGenericControl lblSupplier = (HtmlGenericControl)e.Row.FindControl("lblSupplier");
                hdnSupplierID.Value = entity.BusinessPartnerID.ToString();
                if (entity.BusinessPartnerID == 0)
                    lblSupplier.InnerHtml = "Pilih Supplier";
                else
                    lblSupplier.InnerHtml = string.IsNullOrEmpty(entity.BusinessPartnerName) ? entity.BusinessPartnerName : entity.BusinessPartnerName;

                lblSupplier.Attributes.Remove("class");
                if (lstSelectedMember.Contains(entity.ID.ToString()))
                {
                    int idx = Array.IndexOf(lstSelectedMember, entity.ID.ToString());
                    chkIsSelected.Checked = true;
                    txtPurchaseOrder.ReadOnly = false;
                    txtPurchaseOrder.Text = lstQtyPurchaseOrder[idx];
                    hdnSupplierID.Value = lstSupplierID[idx];
                    lblSupplier.Attributes.Add("class", "lblSupplier lblLink");
                }
                else lblSupplier.Attributes.Add("class", "lblSupplier lblDisabled");
            }
        }

        public void SavePurchaseOrderHd(IDbContext ctx, ref int purchaseOrderID, ref string retval, int BusinessPartnerID)
        {
            PurchaseOrderHdDao entityHdDao = new PurchaseOrderHdDao(ctx);
            PurchaseOrderHd entityHd = new PurchaseOrderHd();
            entityHd.OrderDate = Helper.GetDatePickerValue(txtItemOrderDate.Text);
            entityHd.DeliveryDate = Helper.GetDatePickerValue(txtItemOrderDeliveryDate.Text);
            entityHd.POExpiredDate = Helper.GetDatePickerValue(txtItemOrderExpiredDate.Text);
            entityHd.GCPurchaseOrderType = cboPurchaseOrderType.Value.ToString();
            entityHd.TermID = Convert.ToInt32(cboTerm.Value.ToString());
            entityHd.BusinessPartnerID = BusinessPartnerID;
            entityHd.LocationID = Convert.ToInt32(hdnLocationID.Value);
            entityHd.GCFrancoRegion = cboFrancoRegion.Value.ToString();
            entityHd.GCCurrencyCode = cboCurrency.Value.ToString();
            entityHd.CurrencyRate = Convert.ToDecimal(txtKurs.Text);
            entityHd.IsIncludeVAT = false;
            entityHd.IsPPHInPercentage = true;
            //entityHd.TransactionAmount = 0;
            entityHd.TransactionCode = Constant.TransactionCode.PURCHASE_ORDER;
            entityHd.PurchaseOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.OrderDate, ctx);
            entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
            entityHd.FinalDiscount = Convert.ToDecimal(0.00);
            //entityHd.TaxAmount = Convert.ToDecimal(0.00);
            entityHd.DownPaymentAmount = Convert.ToDecimal(0.00);

            if (hdnIsUsedProductLine.Value == "1")
            {
                entityHd.ProductLineID = Convert.ToInt32(hdnProductLineID.Value);
            }

            ctx.CommandType = CommandType.Text;
            ctx.Command.Parameters.Clear();
            entityHd.CreatedBy = AppSession.UserLogin.UserID;
            purchaseOrderID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
            retval += entityHd.PurchaseOrderNo + "^" + BusinessLayer.GetBusinessPartners((int)BusinessPartnerID).BusinessPartnerName + ";";
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("LocationID = {0} AND (QuantityMIN > 0 AND QuantityMAX > 0) AND QuantityEND <= QuantityMIN AND IsDeleted = 0 AND GCItemStatus != '{1}'", hdnLocationID.Value, Constant.ItemStatus.IN_ACTIVE);

            if (cboFilter.Value.Equals("2"))
            {
                filterExpression = string.Format("LocationID = {0} AND (QuantityMIN > 0 AND QuantityMAX > 0) AND QuantityEND <= QuantityMIN AND (QuantityEND + PurchaseOrderQtyOnOrder) <= QuantityMIN AND IsDeleted = 0 AND GCItemStatus != '{1}'", hdnLocationID.Value, Constant.ItemStatus.IN_ACTIVE);
            }
            else if (cboFilter.Value.Equals("3"))
            {
                filterExpression = string.Format("LocationID = {0} AND (QuantityMIN > 0 AND QuantityMAX > 0) AND QuantityEND <= QuantityMIN AND (QuantityEND + PurchaseOrderQtyOnOrder) > QuantityMIN AND IsDeleted = 0 AND GCItemStatus != '{1}'", hdnLocationID.Value, Constant.ItemStatus.IN_ACTIVE);
            }

            if (hdnIsUsedProductLine.Value == "1")
            {
                if (hdnProductLineID.Value != "")
                {
                    filterExpression += string.Format(" AND ProductLineID = {0}", hdnProductLineID.Value);
                }
                else
                {
                    filterExpression += string.Format(" AND ProductLineID = 0");
                }
            }
            else
            {
                if (hdnGCLocationGroup.Value != "")
                {
                    if (hdnGCLocationGroup.Value == Constant.LocationGroup.DRUG_AND_MEDICAL_SUPPLIES)
                    {
                        filterExpression += string.Format(" AND GCItemType IN ('{0}','{1}')", Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS);
                    }
                    else if (hdnGCLocationGroup.Value == Constant.LocationGroup.LOGISTIC)
                    {
                        filterExpression += string.Format(" AND GCItemType IN ('{0}')", Constant.ItemType.BARANG_UMUM);
                    }
                    else
                    {
                        filterExpression += string.Format(" AND GCItemType IN ('{0}','{1}','{2}')",
                                Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, Constant.ItemType.BARANG_UMUM);
                    }
                }
                else
                {
                    filterExpression += string.Format(" AND GCItemType IN ('{0}','{1}','{2}')",
                            Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, Constant.ItemType.BARANG_UMUM);
                }
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvItemBalanceReorderPORowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            lstSelectedMember = hdnSelectedMember.Value.Split(',');
            lstQtyPurchaseOrder = hdnPurchaseOrder.Value.Split(',');
            lstSupplierID = hdnListSupplierID.Value.Split(',');

            List<vItemBalanceReorderPO> lstEntity = BusinessLayer.GetvItemBalanceReorderPOList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "BusinessPartnerName, ItemName1 ASC");
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

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            bool result = true;
            String[] paramID = hdnSelectedMember.Value.Substring(1).Split(',');
            String[] paramPurchaseOrder = hdnPurchaseOrder.Value.Substring(1).Split(',');
            String[] paramSupplierID = hdnListSupplierID.Value.Substring(1).Split(',');
            IDbContext ctx = DbFactory.Configure(true);
            int purchaseOrderID = 0;
            PurchaseOrderDtDao entityPurchaseOrderDtDao = new PurchaseOrderDtDao(ctx);
            PurchaseOrderHdDao entityPurchaseOrderHdDao = new PurchaseOrderHdDao(ctx);
            try
            {
                List<CPurchaseOrder> lstCPurchaseOrder = new List<CPurchaseOrder>();

                for (int i = 0; i < paramID.Length; i++)
                {
                    CPurchaseOrder entityTempPO = new CPurchaseOrder();
                    entityTempPO.ID = paramID[i];
                    entityTempPO.QtyPO = paramPurchaseOrder[i];
                    entityTempPO.SupplierID = paramSupplierID[i];
                    lstCPurchaseOrder.Add(entityTempPO);
                }

                var lstBusinessPartner = (from p in paramSupplierID
                                          select new { BusinessPartnerID = Convert.ToInt32(p) }).GroupBy(p => p).Select(p => p.First()).ToList();

                for (int i = 0; i < lstBusinessPartner.Count; ++i)
                {
                    SavePurchaseOrderHd(ctx, ref purchaseOrderID, ref retval, lstBusinessPartner[i].BusinessPartnerID);

                    PurchaseOrderHd entityHd = entityPurchaseOrderHdDao.Get(purchaseOrderID);

                    List<CPurchaseOrder> lstCPOPerSupplier = lstCPurchaseOrder.Where(p => p.SupplierID == lstBusinessPartner[i].BusinessPartnerID.ToString()).ToList();
                    foreach (CPurchaseOrder entityCPOPerSupplier in lstCPOPerSupplier)
                    {
                        vItemBalanceReorderPO entityItemBalance = BusinessLayer.GetvItemBalanceReorderPOList(string.Format("ID = {0} AND IsDeleted = 0", entityCPOPerSupplier.ID), ctx).FirstOrDefault();
                        PurchaseOrderDt entityPurchaseOrderDt = new PurchaseOrderDt();
                        entityPurchaseOrderDt.ItemID = entityItemBalance.ItemID;
                        entityPurchaseOrderDt.Quantity = Convert.ToDecimal(entityCPOPerSupplier.QtyPO);
                        entityPurchaseOrderDt.QtyENDLocation = entityItemBalance.QuantityEND;
                        entityPurchaseOrderDt.GCItemUnitQtyENDLocation = entityItemBalance.GCItemUnit;
                        entityPurchaseOrderDt.GCBaseUnit = entityItemBalance.GCItemUnit;
                        entityPurchaseOrderDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;

                        if (hdnIM0131.Value == "0")
                        {
                            List<GetItemMasterPurchase> impList = BusinessLayer.GetItemMasterPurchaseList(AppSession.UserLogin.HealthcareID, entityItemBalance.ItemID, lstBusinessPartner[i].BusinessPartnerID, ctx);
                            if (impList.Count > 0)
                            {
                                GetItemMasterPurchase imp = impList.FirstOrDefault();
                                entityPurchaseOrderDt.UnitPrice = imp.Price;
                                entityPurchaseOrderDt.GCPurchaseUnit = imp.ItemUnit;
                                entityPurchaseOrderDt.ConversionFactor = 1;
                                entityPurchaseOrderDt.IsDiscountInPercentage1 = true;
                                entityPurchaseOrderDt.DiscountPercentage1 = imp.Discount;
                                entityPurchaseOrderDt.IsDiscountInPercentage2 = true;
                                entityPurchaseOrderDt.DiscountPercentage2 = imp.Discount2;
                            }
                            else
                            {
                                entityPurchaseOrderDt.UnitPrice = 0;
                                entityPurchaseOrderDt.DiscountPercentage1 = 0;
                                entityPurchaseOrderDt.DiscountPercentage2 = 0;
                                entityPurchaseOrderDt.GCPurchaseUnit = entityItemBalance.GCItemUnit;
                                entityPurchaseOrderDt.ConversionFactor = 1;
                            }
                        }
                        else
                        {
                            List<GetItemMasterPurchaseWithDate> impList = BusinessLayer.GetItemMasterPurchaseWithDateList(AppSession.UserLogin.HealthcareID, entityItemBalance.ItemID, lstBusinessPartner[i].BusinessPartnerID, entityHd.OrderDate.ToString(Constant.FormatString.DATE_FORMAT_112), ctx);
                            if (impList.Count > 0)
                            {
                                GetItemMasterPurchaseWithDate imp = impList.FirstOrDefault();
                                entityPurchaseOrderDt.UnitPrice = imp.Price;
                                entityPurchaseOrderDt.GCPurchaseUnit = imp.ItemUnit;
                                entityPurchaseOrderDt.ConversionFactor = 1;
                                entityPurchaseOrderDt.IsDiscountInPercentage1 = true;
                                entityPurchaseOrderDt.DiscountPercentage1 = imp.Discount;
                                entityPurchaseOrderDt.IsDiscountInPercentage2 = true;
                                entityPurchaseOrderDt.DiscountPercentage2 = imp.Discount2;
                            }
                            else
                            {
                                entityPurchaseOrderDt.UnitPrice = 0;
                                entityPurchaseOrderDt.DiscountPercentage1 = 0;
                                entityPurchaseOrderDt.DiscountPercentage2 = 0;
                                entityPurchaseOrderDt.GCPurchaseUnit = entityItemBalance.GCItemUnit;
                                entityPurchaseOrderDt.ConversionFactor = 1;
                            }
                        }

                        decimal lineAmount = entityPurchaseOrderDt.UnitPrice * entityPurchaseOrderDt.Quantity;
                        decimal discAmount1 = 0, discAmount2 = 0;
                        if (entityPurchaseOrderDt.IsDiscountInPercentage1)
                        {
                            discAmount1 = lineAmount * entityPurchaseOrderDt.DiscountPercentage1 / 100;
                        }
                        else
                        {
                            discAmount1 = entityPurchaseOrderDt.DiscountAmount1;
                        }
                        if (entityPurchaseOrderDt.IsDiscountInPercentage2)
                        {
                            discAmount2 = (lineAmount - discAmount1) * entityPurchaseOrderDt.DiscountPercentage2 / 100;
                        }
                        else
                        {
                            discAmount2 = entityPurchaseOrderDt.DiscountAmount2;
                        }

                        lineAmount = lineAmount - discAmount1 - discAmount2;
                        entityPurchaseOrderDt.LineAmount = lineAmount;

                        //Simpan data draft
                        entityPurchaseOrderDt.DraftQuantity = entityPurchaseOrderDt.Quantity;
                        entityPurchaseOrderDt.DraftUnitPrice = entityPurchaseOrderDt.UnitPrice;
                        entityPurchaseOrderDt.IsDraftDiscountInPercentage1 = entityPurchaseOrderDt.IsDiscountInPercentage1;
                        entityPurchaseOrderDt.DraftDiscountPercentage1 = entityPurchaseOrderDt.DiscountPercentage1;
                        entityPurchaseOrderDt.DraftDiscountAmount1 = entityPurchaseOrderDt.DiscountAmount1;
                        entityPurchaseOrderDt.IsDraftDiscountInPercentage2 = entityPurchaseOrderDt.IsDiscountInPercentage2;
                        entityPurchaseOrderDt.DraftDiscountPercentage2 = entityPurchaseOrderDt.DiscountPercentage2;
                        entityPurchaseOrderDt.DraftDiscountAmount2 = entityPurchaseOrderDt.DiscountAmount2;

                        entityPurchaseOrderDt.CreatedBy = AppSession.UserLogin.UserID;
                        entityPurchaseOrderDt.PurchaseOrderID = purchaseOrderID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityPurchaseOrderDtDao.Insert(entityPurchaseOrderDt);
                    }
                }
                ctx.CommitTransaction();
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

        class CPurchaseOrder
        {
            public String ID { get; set; }
            public String QtyPO { get; set; }
            public String SupplierID { get; set; }
        }
    }
}
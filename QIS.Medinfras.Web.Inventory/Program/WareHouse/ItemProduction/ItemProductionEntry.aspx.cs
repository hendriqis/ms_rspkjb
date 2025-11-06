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
    public partial class ItemProductionEntry : BasePageTrx
    {

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.ITEM_PRODUCTION;
        }

        protected override void InitializeDataControl()
        {
            int count = BusinessLayer.GetLocationUserRowCount(string.Format("UserID = {0} AND IsDeleted = 0", AppSession.UserLogin.UserID));
            if (count > 0)
                hdnRecordFilterExpression.Value = string.Format("FromLocationID IN (SELECT LocationID FROM LocationUser WHERE UserID = {0} AND IsDeleted = 0)", AppSession.UserLogin.UserID);
            else
            {
                count = BusinessLayer.GetLocationUserRoleRowCount(string.Format("RoleID IN (SELECT RoleID FROM UserInRole WHERE UserID = {0} AND HealthcareID = '{1}') AND IsDeleted = 0", AppSession.UserLogin.UserID, AppSession.UserLogin.HealthcareID));
                if (count > 0)
                    hdnRecordFilterExpression.Value = string.Format("FromLocationID IN (SELECT LocationID FROM LocationUserRole WHERE RoleID IN (SELECT RoleID FROM UserInRole WHERE UserID = {0} AND HealthcareID = '{1}') AND IsDeleted = 0)", AppSession.UserLogin.UserID, AppSession.UserLogin.HealthcareID);
                else
                    hdnRecordFilterExpression.Value = "";
            }

            string PrinterType = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode = '{0}'", Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;

            if (PrinterType == Constant.PrinterType.DOT_MATRIX_FORMAT_1)
            {
                hdnIsDotMatrix.Value = "1";
            }
            else
            {
                hdnIsDotMatrix.Value = "0";
            }

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnProductionID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtProductionNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(hdnLocationIDFrom, new ControlEntrySetting(true, false, true, ""));
            SetControlEntrySetting(txtLocationCode, new ControlEntrySetting(true, false, true, ""));
            SetControlEntrySetting(txtLocationName, new ControlEntrySetting(false, false, true, ""));
            SetControlEntrySetting(hdnItemID, new ControlEntrySetting(true, false, true, ""));
            SetControlEntrySetting(txtItemCode, new ControlEntrySetting(true, false, true, ""));
            SetControlEntrySetting(txtItemName, new ControlEntrySetting(false, false, true, ""));
            SetControlEntrySetting(txtQuantity, new ControlEntrySetting(true, false, true, "0"));
            SetControlEntrySetting(chkIsFixedCost, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtUnitPrice, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtFixedCostAmount, new ControlEntrySetting(false, false, true));

            SetControlEntrySetting(txtProductionDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(hdnLocationIDTo, new ControlEntrySetting(true, false, true, ""));
            SetControlEntrySetting(txtLocationCodeTo, new ControlEntrySetting(true, false, true, ""));
            SetControlEntrySetting(txtLocationNameTo, new ControlEntrySetting(false, false, true, ""));
            SetControlEntrySetting(txtBatchNumber, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtReferenceNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(lblLocation, new ControlEntrySetting(true, false));
            SetControlEntrySetting(lblLocationTo, new ControlEntrySetting(true, false));
            SetControlEntrySetting(lblItem, new ControlEntrySetting(true, false));
        }

        #region Filter Expression Search Dialog
        protected string OnGetFilterExpressionFromLocation()
        {
            return string.Format("{0};{1};{2};", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.TransactionCode.PRODUCTION_PROCESS);
        }
        protected string OnGetFilterExpressionToLocation()
        {
            return string.Format("{0};{1};{2};", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.TransactionCode.PRODUCTION_PROCESS);
        }
        protected string OnGetFilterExpressionItemProduct()
        {
            return string.Format("GCItemType IN ('{0}','{1}','{2}') AND ItemID IN (SELECT ItemID FROM ItemProduct WHERE IsProductionItem = 1) AND IsDeleted = 0", Constant.ItemGroupMaster.DRUGS, Constant.ItemGroupMaster.SUPPLIES, Constant.ItemGroupMaster.LOGISTIC);
        }
        #endregion

        #region Load Entity
        protected string GetFilterExpression()
        {
            return hdnRecordFilterExpression.Value;
        }

        public override int OnGetRowCount()
        {
            string filterExpression = GetFilterExpression();
            return BusinessLayer.GetvItemProductionHdRowCount(filterExpression);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            vItemProductionHd entity = BusinessLayer.GetvItemProductionHd(filterExpression, PageIndex, "ProductionID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            PageIndex = BusinessLayer.GetvItemProductionHdRowIndex(filterExpression, keyValue, "ProductionID DESC");
            vItemProductionHd entity = BusinessLayer.GetvItemProductionHd(filterExpression, PageIndex, "ProductionID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }
        #endregion

        public override void OnAddRecord()
        {
            hdnIsEditable.Value = "1";
        }

        protected string IsEditable()
        {
            return hdnIsEditable.Value;
        }

        private void EntityToControl(vItemProductionHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                watermarkText = entity.TransactionStatusWatermark;
                hdnIsEditable.Value = "0";

                SetControlEntrySetting(txtQuantity, new ControlEntrySetting(true, false, true));
                SetControlEntrySetting(txtUnitPrice, new ControlEntrySetting(true, false, true));

                SetControlEntrySetting(txtBatchNumber, new ControlEntrySetting(true, false, false));
                SetControlEntrySetting(txtReferenceNo, new ControlEntrySetting(true, false, false));
                SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, false, false));
            }
            else
            {
                hdnIsEditable.Value = "1";
            }

            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN && entity.GCTransactionStatus != Constant.TransactionStatus.VOID)
            {
                hdnPrintStatus.Value = "true";
            }
            else
            {
                hdnPrintStatus.Value = "false";
            }

            hdnProductionID.Value = entity.ProductionID.ToString();
            txtProductionNo.Text = entity.ProductionNo;
            txtProductionDate.Text = entity.ProductionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnLocationIDFrom.Value = entity.FromLocationID.ToString();
            txtLocationCode.Text = entity.FromLocationCode;
            txtLocationName.Text = entity.FromLocationName;
            hdnLocationIDTo.Value = entity.ToLocationID.ToString();
            txtLocationCodeTo.Text = entity.ToLocationCode;
            txtLocationNameTo.Text = entity.ToLocationName;
            txtNotes.Text = entity.Remarks;
            txtReferenceNo.Text = entity.ReferenceNo;
            txtBatchNumber.Text = entity.BatchNumber;
            txtFixedCostAmount.Text = entity.FixedCostAmount.ToString("N2");
            hdnItemID.Value = entity.ItemID.ToString();
            txtItemCode.Text = entity.ItemCode;
            txtItemName.Text = entity.ItemName1;
            txtQuantity.Text = entity.Quantity.ToString();
            txtUnitPrice.Text = entity.UnitPrice.ToString("N2");
            chkIsFixedCost.Checked = (entity.FixedCostAmount != 0);

            divCreatedBy.InnerHtml = entity.CreatedByName;
            divCreatedDate.InnerHtml = entity.CreatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            divLastUpdatedBy.InnerHtml = entity.LastUpdatedByName;
            if (entity.LastUpdatedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                divLastUpdatedDate.InnerHtml = "";
            }
            else
            {
                divLastUpdatedDate.InnerHtml = entity.LastUpdatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }

            BindGridView();
        }

        private void BindGridView()
        {
            Qty = Convert.ToDecimal(txtQuantity.Text);

            string filterExpression = "1 = 0";
            string filterExpressionItemBalance = "1 = 0";
            if (hdnItemID.Value != "" && hdnItemID.Value != "0")
            {
                filterExpression = string.Format("ItemID = {0} AND IsDeleted = 0 ORDER BY SequenceNo, BillOfMaterialCode", hdnItemID.Value);
                filterExpressionItemBalance = string.Format("ItemID = {0} AND IsDeleted = 0", hdnItemID.Value);
            }

            string lstItemID = "";
            List<vItemBOM> lstEntity = BusinessLayer.GetvItemBOMList(filterExpression);
            foreach (vItemBOM itemBOM in lstEntity)
            {
                if (lstItemID != "")
                {
                    lstItemID += ",";
                }
                lstItemID += itemBOM.BillOfMaterialID.ToString();
            }

            if (lstItemID != "")
            {
                filterExpressionItemBalance = string.Format("HealthcareID = '{0}' AND ItemID IN ({1})", AppSession.UserLogin.HealthcareID, lstItemID);

                if (hdnLocationIDFrom.Value != "")
                {
                    filterExpressionItemBalance = string.Format("LocationID = {0} AND ItemID IN ({1}) AND IsDeleted = 0", hdnLocationIDFrom.Value, lstItemID);
                }
            }

            lstItemBalance = BusinessLayer.GetItemBalanceList(filterExpressionItemBalance);

            if (hdnProductionID.Value != "0" && hdnProductionID.Value != "")
            {
                string filterProductionDt = string.Format("ProductionID = {0} ORDER BY SequenceNo, BillOfMaterialCode", hdnProductionID.Value);
                List<vItemProductionDt> lstProductionDt = BusinessLayer.GetvItemProductionDtList(filterProductionDt);
                grdView.DataSource = lstProductionDt;
                grdView.DataBind();
            }
            else
            {
                grdView.DataSource = lstEntity;
                grdView.DataBind();
            }
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string productionID = hdnProductionID.Value;
            if (e.Parameter != null && e.Parameter != "")
            {
                if (e.Parameter == "print")
                {
                    if (hdnIsDotMatrix.Value != "1")
                    {
                        PrintLabelProduction(productionID);
                    }
                    else
                    {
                        PrintLabelProductionDotMatrix(productionID);
                    }

                }
            }
            else
            {
                BindGridView();
            }
        }

        private Decimal Qty = 0;
        private List<ItemBalance> lstItemBalance = null;
        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                if (hdnProductionID.Value != "0" && hdnProductionID.Value != "")
                {
                    vItemProductionDt entity = e.Row.DataItem as vItemProductionDt;

                    ItemBalance itemBalance = lstItemBalance.FirstOrDefault(p => p.ItemID == entity.BillOfMaterialID);
                    decimal qtyEnd = 0;
                    if (itemBalance != null)
                    {
                        qtyEnd = itemBalance.QuantityEND;
                        HtmlGenericControl divRemainingStock = (HtmlGenericControl)e.Row.FindControl("divRemainingStock");
                        divRemainingStock.InnerHtml = qtyEnd.ToString(Constant.FormatString.NUMERIC_2);
                    }
                    else
                    {
                        HtmlGenericControl divRemainingStock = (HtmlGenericControl)e.Row.FindControl("divRemainingStock");
                        divRemainingStock.InnerHtml = qtyEnd.ToString(Constant.FormatString.NUMERIC_2);
                    }

                    TextBox txtQuantityDt = e.Row.FindControl("txtQuantityDt") as TextBox;
                    TextBox txtCostAmount = e.Row.FindControl("txtCostAmount") as TextBox;

                    txtQuantityDt.Text = entity.BOMQuantity.ToString();
                    txtCostAmount.Text = entity.CostAmountNew.ToString();
                }
                else
                {
                    vItemBOM entity = e.Row.DataItem as vItemBOM;
                    ItemBalance itemBalance = lstItemBalance.FirstOrDefault(p => p.ItemID == entity.BillOfMaterialID);
                    decimal qtyEnd = 0;
                    if (itemBalance != null)
                    {
                        qtyEnd = itemBalance.QuantityEND;
                        HtmlGenericControl divRemainingStock = (HtmlGenericControl)e.Row.FindControl("divRemainingStock");
                        divRemainingStock.InnerHtml = qtyEnd.ToString(Constant.FormatString.NUMERIC_2);
                    }
                    else
                    {
                        HtmlGenericControl divRemainingStock = (HtmlGenericControl)e.Row.FindControl("divRemainingStock");
                        divRemainingStock.InnerHtml = qtyEnd.ToString(Constant.FormatString.NUMERIC_2);
                    }
                }
            }
        }

        #region Save & Edit Header
        private void ControlToEntity(ItemProductionHd entityHd)
        {
            entityHd.FromLocationID = Convert.ToInt32(hdnLocationIDFrom.Value);
            entityHd.ToLocationID = Convert.ToInt32(hdnLocationIDTo.Value);
            entityHd.ItemID = Convert.ToInt32(hdnItemID.Value);
            entityHd.Quantity = Convert.ToDecimal(txtQuantity.Text);
            entityHd.ProductionDate = Helper.GetDatePickerValue(txtProductionDate.Text);
            entityHd.Remarks = txtNotes.Text;
            entityHd.BatchNumber = txtBatchNumber.Text;
            entityHd.ReferenceNo = txtReferenceNo.Text;
            entityHd.UnitPrice = Convert.ToDecimal(txtUnitPrice.Text);
            entityHd.FixedCostAmount = Convert.ToDecimal(Request.Form[txtFixedCostAmount.UniqueID]);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ItemProductionHdDao entityHdDao = new ItemProductionHdDao(ctx);
            ItemProductionDtDao entityDtDao = new ItemProductionDtDao(ctx);
            try
            {
                ItemProductionHd entityHd = new ItemProductionHd();
                ControlToEntity(entityHd);
                entityHd.ProductionNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.PRODUCTION_PROCESS, entityHd.ProductionDate, ctx);
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                int oProductionID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);

                string filterExpression = string.Format("ItemID = {0} AND IsDeleted = 0", hdnItemID.Value);
                List<ItemBOM> lstEntity = BusinessLayer.GetItemBOMList(filterExpression, ctx);

                string lstItemID = "";
                foreach (ItemBOM itemBOM in lstEntity)
                {
                    if (lstItemID != "")
                        lstItemID += ",";
                    lstItemID += itemBOM.BillOfMaterialID.ToString();
                }
                if (lstItemID != "")
                    filterExpression = string.Format("HealthcareID = '{0}' AND ItemID IN ({1})", AppSession.UserLogin.HealthcareID, lstItemID);
                //lstItemCost = BusinessLayer.GetItemCostList(filterExpression, ctx);

                foreach (ItemBOM itemBOM in lstEntity)
                {
                    ItemProductionDt entityDt = new ItemProductionDt();
                    entityDt.ProductionID = oProductionID;
                    entityDt.ItemID = entityHd.ItemID;
                    entityDt.BillOfMaterialID = itemBOM.BillOfMaterialID;
                    entityDt.SequenceNo = itemBOM.SequenceNo;
                    entityDt.ConversionFactor = itemBOM.BOMQuantity / itemBOM.ItemQuantity;
                    entityDt.BOMQuantity = entityHd.Quantity * entityDt.ConversionFactor;

                    //ItemCost itemCost = lstItemCost.FirstOrDefault(p => p.ItemID == itemBOM.BillOfMaterialID);
                    //entityDt.CostAmount = (itemCost.TotalBurden + itemCost.TotalLabor + itemCost.TotalMaterial + itemCost.TotalOverhead + itemCost.TotalSubContract);

                    ItemPlanning itemPlanning = BusinessLayer.GetItemPlanningList(string.Format("ItemID = {0} AND IsDeleted = 0", entityDt.BillOfMaterialID), ctx).FirstOrDefault();
                    entityDt.CostAmount = entityDt.BOMQuantity * itemPlanning.AveragePrice;
                    entityDt.UnitPrice = itemPlanning.UnitPrice;

                    entityDtDao.Insert(entityDt);
                }

                retval = entityHd.ProductionID.ToString();
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

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ItemProductionHdDao entityHdDao = new ItemProductionHdDao(ctx);
            ItemProductionDtDao entityDtDao = new ItemProductionDtDao(ctx);
            try
            {
                ItemProductionHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnProductionID.Value));
                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    ControlToEntity(entityHd);
                    entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entityHd);

                    string filterExpression = string.Format("ItemID = {0} AND IsDeleted = 0", hdnItemID.Value);
                    List<ItemBOM> lstEntity = BusinessLayer.GetItemBOMList(filterExpression, ctx);

                    string lstItemID = "";
                    foreach (ItemBOM itemBOM in lstEntity)
                    {
                        if (lstItemID != "")
                            lstItemID += ",";
                        lstItemID += itemBOM.BillOfMaterialID.ToString();
                    }
                    if (lstItemID != "")
                        filterExpression = string.Format("HealthcareID = '{0}' AND ItemID IN ({1})", AppSession.UserLogin.HealthcareID, lstItemID);
                    //lstItemCost = BusinessLayer.GetItemCostList(filterExpression, ctx);

                    foreach (ItemBOM itemBOM in lstEntity)
                    {
                        ItemProductionDt entityDt = new ItemProductionDt();
                        entityDt.ProductionID = entityHd.ProductionID;
                        entityDt.ItemID = entityHd.ItemID;
                        entityDt.BillOfMaterialID = itemBOM.BillOfMaterialID;
                        entityDt.SequenceNo = itemBOM.SequenceNo;
                        entityDt.ConversionFactor = itemBOM.BOMQuantity / itemBOM.ItemQuantity;
                        entityDt.BOMQuantity = entityHd.Quantity * entityDt.ConversionFactor;

                        //ItemCost itemCost = lstItemCost.FirstOrDefault(p => p.ItemID == itemBOM.BillOfMaterialID);
                        //entityDt.CostAmount = (itemCost.TotalBurden + itemCost.TotalLabor + itemCost.TotalMaterial + itemCost.TotalOverhead + itemCost.TotalSubContract);

                        ItemPlanning itemPlanning = BusinessLayer.GetItemPlanningList(string.Format("ItemID = {0} AND IsDeleted = 0", entityDt.BillOfMaterialID), ctx).FirstOrDefault();
                        entityDt.CostAmount = entityDt.BOMQuantity * itemPlanning.AveragePrice;
                        entityDt.UnitPrice = itemPlanning.UnitPrice;

                        entityDtDao.Update(entityDt);
                    }
                    ctx.CommitTransaction();
                }
                else
                {
                    ctx.RollBackTransaction();
                    errMessage = "Produksi tidak dapat diubah. Harap refresh halaman ini.";
                    result = false;
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

        #region Approve Proposed Void Entity

        protected override bool OnApproveRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ItemProductionHdDao itemHdDao = new ItemProductionHdDao(ctx);
            ItemProductionDtDao entityDtDao = new ItemProductionDtDao(ctx);
            ItemPlanningDao itemPlanningDao = new ItemPlanningDao(ctx);
            try
            {
                ItemProductionHd itemHd = itemHdDao.Get(Convert.ToInt32(hdnProductionID.Value));
                ItemPlanning iplanning = BusinessLayer.GetItemPlanningList(string.Format("ItemID = {0} AND IsDeleted = 0", itemHd.ItemID), ctx).FirstOrDefault();

                decimal oOldAveragePrice = iplanning.AveragePrice;
                decimal oOldUnitPrice = iplanning.UnitPrice;
                decimal oOldPurchasePrice = iplanning.PurchaseUnitPrice;
                bool oOldIsPriceLastUpdatedBySystem = iplanning.IsPriceLastUpdatedBySystem;
                bool oOldIsDeleted = iplanning.IsDeleted;

                if (itemHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    ControlToEntity(itemHd);

                    string filterIB = "1 = 0";
                    filterIB = string.Format("ItemID IN ({0}) AND IsDeleted = 0", itemHd.ItemID);
                    lstItemBalance = BusinessLayer.GetItemBalanceList(filterIB, ctx);
                    decimal qtyEnd = lstItemBalance.Sum(p => p.QuantityEND);
                    decimal tempQty = (qtyEnd + itemHd.Quantity);
                    if (tempQty > 0)
                    {
                        iplanning.AveragePrice = ((iplanning.AveragePrice * qtyEnd) + (itemHd.UnitPrice)) / tempQty;
                    }

                    decimal ounitPrice = itemHd.UnitPrice / itemHd.Quantity;
                    if (iplanning.UnitPrice < ounitPrice)
                    {
                        iplanning.UnitPrice = ounitPrice;
                        iplanning.PurchaseUnitPrice = ounitPrice;
                    }

                    iplanning.LastPurchasePrice = ounitPrice;
                    iplanning.LastPurchaseUnitPrice = iplanning.PurchaseUnitPrice;

                    iplanning.IsPriceLastUpdatedBySystem = true;
                    iplanning.LastUpdatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    itemPlanningDao.Update(iplanning);

                    BusinessLayer.ProcessItemPlanningChangedInsertItemPriceHistory("PR", iplanning.ID, oOldAveragePrice, oOldUnitPrice, oOldPurchasePrice, oOldIsPriceLastUpdatedBySystem, oOldIsDeleted, ctx);

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    //string filterExpression = string.Format("ProductionID = {0}", itemHd.ProductionID);
                    //List<ItemProductionDt> lstItemProductionDt = BusinessLayer.GetItemProductionDtList(filterExpression, ctx);
                    //foreach (ItemProductionDt entityDt in lstItemProductionDt)
                    //{
                    //    entityDt.BOMQuantity = itemHd.Quantity * entityDt.ConversionFactor;
                    //    ItemPlanning itemPlanning = BusinessLayer.GetItemPlanningList(string.Format("ItemID = {0} AND IsDeleted = 0", entityDt.BillOfMaterialID), ctx).FirstOrDefault();
                    //    entityDt.CostAmount = entityDt.BOMQuantity * itemPlanning.AveragePrice;
                    //    ctx.CommandType = CommandType.Text;
                    //    ctx.Command.Parameters.Clear();
                    //    entityDtDao.Update(entityDt);
                    //}

                    string filterExpression = string.Format("ItemID = {0} AND IsDeleted = 0", hdnItemID.Value);
                    List<ItemBOM> lstEntity = BusinessLayer.GetItemBOMList(filterExpression, ctx);

                    string lstItemID = "";
                    foreach (ItemBOM itemBOM in lstEntity)
                    {
                        if (lstItemID != "")
                            lstItemID += ",";
                        lstItemID += itemBOM.BillOfMaterialID.ToString();
                    }
                    if (lstItemID != "")
                        filterExpression = string.Format("HealthcareID = '{0}' AND ItemID IN ({1})", AppSession.UserLogin.HealthcareID, lstItemID);
                    //lstItemCost = BusinessLayer.GetItemCostList(filterExpression, ctx);

                    foreach (ItemBOM itemBOM in lstEntity)
                    {
                        ItemProductionDt entityDt = new ItemProductionDt();
                        entityDt.ProductionID = itemHd.ProductionID;
                        entityDt.ItemID = itemHd.ItemID;
                        entityDt.BillOfMaterialID = itemBOM.BillOfMaterialID;
                        entityDt.SequenceNo = itemBOM.SequenceNo;
                        entityDt.ConversionFactor = itemBOM.BOMQuantity / itemBOM.ItemQuantity;
                        entityDt.BOMQuantity = itemHd.Quantity * entityDt.ConversionFactor;

                        ItemPlanning itemPlanning = BusinessLayer.GetItemPlanningList(string.Format("ItemID = {0} AND IsDeleted = 0", entityDt.BillOfMaterialID), ctx).FirstOrDefault();
                        entityDt.CostAmount = entityDt.BOMQuantity * itemPlanning.AveragePrice;
                        entityDt.UnitPrice = itemPlanning.UnitPrice;

                        entityDtDao.Update(entityDt);
                    }

                    itemHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                    itemHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    itemHdDao.Update(itemHd);

                    ctx.CommitTransaction();
                }
                else
                {
                    errMessage = "Produksi tidak dapat diubah. Harap refresh halaman ini.";
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

        protected override bool OnProposeRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ItemProductionHdDao itemHdDao = new ItemProductionHdDao(ctx);
            ItemProductionDtDao entityDtDao = new ItemProductionDtDao(ctx);
            try
            {
                ItemProductionHd itemHd = itemHdDao.Get(Convert.ToInt32(hdnProductionID.Value));
                if (itemHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    ControlToEntity(itemHd);

                    //string filterExpression = string.Format("ProductionID = {0}", itemHd.ProductionID);
                    //List<ItemProductionDt> lstItemProductionDt = BusinessLayer.GetItemProductionDtList(filterExpression, ctx);
                    //foreach (ItemProductionDt entityDt in lstItemProductionDt)
                    //{
                    //    entityDt.BOMQuantity = itemHd.Quantity * entityDt.ConversionFactor;
                    //    ItemPlanning itemPlanning = BusinessLayer.GetItemPlanningList(string.Format("ItemID = {0} AND IsDeleted = 0", entityDt.BillOfMaterialID), ctx).FirstOrDefault();
                    //    entityDt.CostAmount = entityDt.BOMQuantity * itemPlanning.AveragePrice;

                    //    entityDtDao.Update(entityDt);
                    //}

                    string filterExpression = string.Format("ItemID = {0} AND IsDeleted = 0", hdnItemID.Value);
                    List<ItemBOM> lstEntity = BusinessLayer.GetItemBOMList(filterExpression, ctx);

                    string lstItemID = "";
                    foreach (ItemBOM itemBOM in lstEntity)
                    {
                        if (lstItemID != "")
                            lstItemID += ",";
                        lstItemID += itemBOM.BillOfMaterialID.ToString();
                    }
                    if (lstItemID != "")
                        filterExpression = string.Format("HealthcareID = '{0}' AND ItemID IN ({1})", AppSession.UserLogin.HealthcareID, lstItemID);
                    //lstItemCost = BusinessLayer.GetItemCostList(filterExpression, ctx);

                    foreach (ItemBOM itemBOM in lstEntity)
                    {
                        ItemProductionDt entityDt = new ItemProductionDt();
                        entityDt.ProductionID = itemHd.ProductionID;
                        entityDt.ItemID = itemHd.ItemID;
                        entityDt.BillOfMaterialID = itemBOM.BillOfMaterialID;
                        entityDt.SequenceNo = itemBOM.SequenceNo;
                        entityDt.ConversionFactor = itemBOM.BOMQuantity / itemBOM.ItemQuantity;
                        entityDt.BOMQuantity = itemHd.Quantity * entityDt.ConversionFactor;

                        ItemPlanning itemPlanning = BusinessLayer.GetItemPlanningList(string.Format("ItemID = {0} AND IsDeleted = 0", entityDt.BillOfMaterialID), ctx).FirstOrDefault();
                        entityDt.CostAmount = entityDt.BOMQuantity * itemPlanning.AveragePrice;
                        entityDt.UnitPrice = itemPlanning.UnitPrice;

                        entityDtDao.Update(entityDt);
                    }

                    itemHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                    itemHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    itemHdDao.Update(itemHd);

                    ctx.CommitTransaction();
                }
                else
                {
                    errMessage = "Produksi tidak dapat diubah. Harap refresh halaman ini.";
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

        protected override bool OnVoidRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ItemProductionHdDao itemHdDao = new ItemProductionHdDao(ctx);
            ItemProductionDtDao entityDtDao = new ItemProductionDtDao(ctx);
            try
            {
                ItemProductionHd itemHd = itemHdDao.Get(Convert.ToInt32(hdnProductionID.Value));
                if (itemHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    string filterExpression = string.Format("ProductionID = {0}", itemHd.ProductionID);
                    List<ItemProductionDt> lstItemProductionDt = BusinessLayer.GetItemProductionDtList(filterExpression, ctx);
                    foreach (ItemProductionDt entityDt in lstItemProductionDt)
                    {
                        entityDt.BOMQuantity = itemHd.Quantity * entityDt.ConversionFactor;
                        ItemPlanning itemPlanning = BusinessLayer.GetItemPlanningList(string.Format("ItemID = {0} AND IsDeleted = 0", entityDt.BillOfMaterialID), ctx).FirstOrDefault();
                        entityDt.CostAmount = entityDt.BOMQuantity * itemPlanning.AveragePrice;

                        entityDtDao.Update(entityDt);
                    }

                    itemHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    itemHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    itemHdDao.Update(itemHd);

                    ctx.CommitTransaction();
                }
                else
                {
                    errMessage = "Produksi tidak dapat diubah. Harap refresh halaman ini.";
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

        #endregion

        #endregion

        private string PrintLabelProductionDotMatrix(string id)
        {
            string result = string.Empty;
            try
            {
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IM_FORMAT_CETAKAN_LABEL_PRODUKSI);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                if (lstParam != null)
                {
                    bool isBasedOnIPAddress = AppSession.IsPrinterLocationBasedOnIP;

                    if (isBasedOnIPAddress)
                    {
                        //Get Printer Address
                        string ipAddress = HttpContext.Current.Request.UserHostAddress;
                        filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}','{2}') AND IsDeleted=0",
                            ipAddress, Constant.DirectPrintType.LABEL_BARANG_PRODUKSI, Constant.DirectPrintType.LABEL_BARANG_PRODUKSI_LUAR);

                        List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);
                        if (lstPrinter.Count > 0)
                        {
                            string printerUrl1 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.LABEL_BARANG_PRODUKSI).FirstOrDefault().PrinterName;
                            string printerUrl2 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.LABEL_BARANG_PRODUKSI_LUAR).FirstOrDefault().PrinterName;

                            vItemProductionHd oHeader = BusinessLayer.GetvItemProductionHdList(string.Format("ProductionID = {0}", id)).FirstOrDefault();
                            ZebraPrinting.printLabelProductionDotMatrix(oHeader, printerUrl1, printerUrl2);
                        }
                        else
                        {
                            result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                        }
                    }
                    else
                    {
                        result = "Printer format has not been set yet";
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        private string PrintLabelProduction(string id)
        {
            string result = string.Empty;
            try
            {
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IM_FORMAT_CETAKAN_LABEL_PRODUKSI);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                if (lstParam != null)
                {
                    string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.IM_FORMAT_CETAKAN_LABEL_PRODUKSI)).FirstOrDefault().ParameterValue;

                    bool isBasedOnIPAddress = AppSession.IsPrinterLocationBasedOnIP;

                    if (isBasedOnIPAddress)
                    {
                        //Get Printer Address
                        string ipAddress = HttpContext.Current.Request.UserHostAddress;
                        filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}','{2}') AND IsDeleted=0",
                            ipAddress, Constant.DirectPrintType.LABEL_BARANG_PRODUKSI, Constant.DirectPrintType.LABEL_BARANG_PRODUKSI_LUAR);

                        List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);
                        if (lstPrinter.Count > 0)
                        {
                            string printerUrl1 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.LABEL_BARANG_PRODUKSI).FirstOrDefault().PrinterName;
                            string printerUrl2 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.LABEL_BARANG_PRODUKSI_LUAR).FirstOrDefault().PrinterName;
                            int labelCount = Convert.ToInt16(1);

                            vItemProductionHd oHeader = BusinessLayer.GetvItemProductionHdList(string.Format("ProductionID = {0}", id)).FirstOrDefault();
                            ZebraPrinting.PrintProductionLabel(oHeader, printerUrl1, printerUrl2, printFormat, labelCount);
                        }
                        else
                        {
                            result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                        }
                    }
                    else
                    {
                        result = "Printer format has not been set yet";
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

    }
}
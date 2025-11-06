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
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class StockAdjustmentEntry : BasePageTrx
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.ITEM_ADJUSTMENT;
        }

        public string GetAdjusmentTypeReceipts()
        {
            return Constant.AdjustmentType.RECEIPTS;
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString["id"] != null)
            {
                hdnRequestID.Value = Page.Request.QueryString["id"];
            }
            else
            {
                hdnRequestID.Value = "ALL";
            }

            SetControlProperties();
            hdnIsEditable.Value = "1";

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

            BindGridView(1, true, ref PageCount);

            //Helper.SetControlEntrySetting(txtQuantity, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtItemCode, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(cboItemUnit, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtConversion, new ControlEntrySetting(false, false, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(cboGCAdjustmentReason, new ControlEntrySetting(true, false, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(cboHealthcareUnit, new ControlEntrySetting(true, false, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtAdjustmentReason, new ControlEntrySetting(true, true, true), "mpTrxPopup");

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> listStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}','{2}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.ADJUSTMENT_REASON, Constant.StandardCode.ADJUSTMENT_TYPE, Constant.StandardCode.HEALTHCARE_UNIT));
            Methods.SetComboBoxField<StandardCode>(cboGCAdjustmentReason, listStandardCode.Where(p => p.ParentID == Constant.StandardCode.ADJUSTMENT_REASON).ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboHealthcareUnit, listStandardCode.Where(p => p.ParentID == Constant.StandardCode.HEALTHCARE_UNIT).ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");

            if (hdnRequestID.Value == "ISSUES")
            {
                Methods.SetComboBoxField<StandardCode>(cboGCAdjustmentType, listStandardCode.Where(p => p.ParentID == Constant.StandardCode.ADJUSTMENT_TYPE && p.StandardCodeID == Constant.AdjustmentType.ISSUES).ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");
            }
            else if (hdnRequestID.Value == "RECEIPTS")
            {
                Methods.SetComboBoxField<StandardCode>(cboGCAdjustmentType, listStandardCode.Where(p => p.ParentID == Constant.StandardCode.ADJUSTMENT_TYPE && p.StandardCodeID == Constant.AdjustmentType.RECEIPTS).ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");
            }
            else
            {
                Methods.SetComboBoxField<StandardCode>(cboGCAdjustmentType, listStandardCode.Where(p => p.ParentID == Constant.StandardCode.ADJUSTMENT_TYPE).ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnAdjustmentID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtAdjustmentNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtAdjustmentDate, new ControlEntrySetting(true, false, false, Constant.DefaultValueEntry.DATE_NOW));
            SetControlEntrySetting(txtAdjustmentTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(lblLocation, new ControlEntrySetting(true, false));
            SetControlEntrySetting(txtLocationCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtLocationName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(cboGCAdjustmentType, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(cboHealthcareUnit, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtReferenceNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
        }

        #region Load Entity
        public override void OnAddRecord()
        {
            hdnPageCount.Value = "0";
            hdnIsEditable.Value = "1";
        }

        protected string IsEditable()
        {
            return hdnIsEditable.Value;
        }

        protected string GetFilterExpression()
        {
            string filterExpression = String.Format("TransactionCode = '{0}'", Constant.TransactionCode.ITEM_ADJUSTMENT);
            if (hdnRecordFilterExpression.Value != "")
                filterExpression += string.Format(" AND {0}", hdnRecordFilterExpression.Value);
            return filterExpression;
        }

        public override int OnGetRowCount()
        {
            string filterExpression = GetFilterExpression();
            return BusinessLayer.GetvItemTransactionHdRowCount(filterExpression);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            vItemTransactionHd entity = BusinessLayer.GetvItemTransactionHd(filterExpression, PageIndex, "TransactionID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            PageIndex = BusinessLayer.GetvItemTransactionHdRowIndex(filterExpression, keyValue, "TransactionID DESC");
            vItemTransactionHd entity = BusinessLayer.GetvItemTransactionHd(filterExpression, PageIndex, "TransactionID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vItemTransactionHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                watermarkText = entity.TransactionStatusWatermark;
                hdnIsEditable.Value = "0";
                SetControlEntrySetting(txtReferenceNo, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(txtRemarks, new ControlEntrySetting(false, false, false));
            }
            else
            {
                hdnIsEditable.Value = "1";
            }

            if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
            {
                cboHealthcareUnit.Enabled = true;
            }
            else
            {
                cboHealthcareUnit.Enabled = false;
            }

            hdnAdjustmentID.Value = entity.TransactionID.ToString();
            txtAdjustmentNo.Text = entity.TransactionNo;
            txtAdjustmentDate.Text = entity.TransactionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtAdjustmentTime.Text = entity.TransactionTime;
            hdnLocationID.Value = entity.FromLocationID.ToString();
            txtLocationCode.Text = entity.FromLocationCode;
            txtLocationName.Text = entity.FromLocationName;

            cboGCAdjustmentType.Value = entity.GCAdjustmentType;
            cboHealthcareUnit.Value = entity.GCHealthcareUnit;
            ////if (entity.GCAdjustmentType == GetAdjusmentTypeReceipts())
            ////{
            ////    Helper.AddCssClass(txtQuantity, "min");
            ////    txtQuantity.Attributes.Add("min", "0");
            ////    txtQuantity.Attributes.Remove("max");
            ////}
            ////else 
            ////{
            ////    Helper.AddCssClass(txtQuantity, "max");
            ////    txtQuantity.Attributes.Add("max", "0");
            ////    txtQuantity.Attributes.Remove("min");
            ////}
            txtReferenceNo.Text = entity.ReferenceNo;
            txtRemarks.Text = entity.Remarks;

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

            BindGridView(1, true, ref PageCount);
            hdnPageCount.Value = PageCount.ToString();
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnAdjustmentID.Value != "")
                filterExpression = string.Format("TransactionID = {0} AND GCItemDetailStatus != '{1}'", hdnAdjustmentID.Value, Constant.TransactionStatus.VOID);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvItemTransactionDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vItemTransactionDt> lstEntity = BusinessLayer.GetvItemTransactionDtList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ItemName1 ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
        #endregion

        #region Filter Expression Search Dialog
        protected string OnGetFilterExpressionLocation()
        {
            return string.Format("{0};{1};{2};", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.TransactionCode.ITEM_ADJUSTMENT);
        }
        protected string OnGetFilterExpressionItemProduct()
        {
            return string.Format("GCItemType IN ('{0}','{1}','{2}','{3}') AND IsDeleted = 0", Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, Constant.ItemType.BARANG_UMUM, Constant.ItemType.BAHAN_MAKANAN);
        }
        #endregion

        #region Save Header
        public void SaveItemAdjustmentHd(IDbContext ctx, ref int AdjustmentID)
        {
            ItemTransactionHdDao entityHdDao = new ItemTransactionHdDao(ctx);
            if (hdnAdjustmentID.Value == "0")
            {
                ItemTransactionHd entityHd = new ItemTransactionHd();
                entityHd.TransactionDate = Helper.GetDatePickerValue(txtAdjustmentDate.Text);
                entityHd.TransactionTime = txtAdjustmentTime.Text;
                entityHd.FromLocationID = Convert.ToInt32(hdnLocationID.Value);
                entityHd.ToLocationID = null;
                entityHd.GCAdjustmentType = cboGCAdjustmentType.Value.ToString();
                entityHd.GCHealthcareUnit = cboHealthcareUnit.Value.ToString();
                entityHd.ReferenceNo = txtReferenceNo.Text;
                entityHd.Remarks = txtRemarks.Text;

                entityHd.TransactionCode = Constant.TransactionCode.ITEM_ADJUSTMENT;
                entityHd.TransactionNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.ITEM_ADJUSTMENT, entityHd.TransactionDate, ctx);
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                AdjustmentID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
            }
            else
            {
                AdjustmentID = Convert.ToInt32(hdnAdjustmentID.Value);
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                int OrderID = 0;
                SaveItemAdjustmentHd(ctx, ref OrderID);
                retval = OrderID.ToString();
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
            try
            {
                ItemTransactionHd entityHd = BusinessLayer.GetItemTransactionHd(Convert.ToInt32(hdnAdjustmentID.Value));
                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entityHd.TransactionDate = Helper.GetDatePickerValue(txtAdjustmentDate.Text);
                    entityHd.TransactionTime = txtAdjustmentTime.Text;
                    entityHd.FromLocationID = Convert.ToInt32(hdnLocationID.Value);
                    entityHd.ToLocationID = null;
                    entityHd.GCAdjustmentType = cboGCAdjustmentType.Value.ToString();
                    entityHd.GCHealthcareUnit = cboHealthcareUnit.Value.ToString();
                    entityHd.ReferenceNo = txtReferenceNo.Text;
                    entityHd.Remarks = txtRemarks.Text;
                    entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateItemTransactionHd(entityHd);
                }
                else
                {
                    errMessage = "Penyesuaian persediaan tidak dapat diubah. Harap refresh halaman ini.";
                    result = false;
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }

        protected override bool OnApproveRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ItemTransactionHdDao itemTransactionHdDao = new ItemTransactionHdDao(ctx);
            ItemTransactionDtDao itemTransactionDtDao = new ItemTransactionDtDao(ctx);
            try
            {
                ItemTransactionHd itemTransactionHd = itemTransactionHdDao.Get(Convert.ToInt32(hdnAdjustmentID.Value));
                if (itemTransactionHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    string filterExpressionPurchaseOrderHd = String.Format("TransactionID = {0} AND GCItemDetailStatus != '{1}'", hdnAdjustmentID.Value, Constant.TransactionStatus.VOID);
                    List<ItemTransactionDt> lstItemTransactionDt = BusinessLayer.GetItemTransactionDtList(filterExpressionPurchaseOrderHd, ctx);
                    foreach (ItemTransactionDt itemTransactionDt in lstItemTransactionDt)
                    {
                        itemTransactionDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                        itemTransactionDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        itemTransactionDtDao.Update(itemTransactionDt);
                    }

                    itemTransactionHd.ReferenceNo = txtReferenceNo.Text;
                    itemTransactionHd.Remarks = txtRemarks.Text;
                    itemTransactionHd.GCHealthcareUnit = cboHealthcareUnit.Value.ToString();
                    itemTransactionHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                    itemTransactionHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    itemTransactionHdDao.Update(itemTransactionHd);

                    ctx.CommitTransaction();
                }
                else
                {
                    errMessage = "Penyesuaian persediaan tidak dapat diubah. Harap refresh halaman ini.";
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
            try
            {
                ItemTransactionHd entity = BusinessLayer.GetItemTransactionHd(Convert.ToInt32(hdnAdjustmentID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    string filterExpressionPurchaseOrderHd = String.Format("TransactionID = {0} AND GCItemDetailStatus != '{1}'", hdnAdjustmentID.Value, Constant.TransactionStatus.VOID);
                    List<ItemTransactionDt> lstItemTransactionDt = BusinessLayer.GetItemTransactionDtList(filterExpressionPurchaseOrderHd);
                    foreach (ItemTransactionDt itemTransactionDt in lstItemTransactionDt)
                    {
                        itemTransactionDt.GCItemDetailStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                        itemTransactionDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        BusinessLayer.UpdateItemTransactionDt(itemTransactionDt);
                    }

                    entity.ReferenceNo = txtReferenceNo.Text;
                    entity.Remarks = txtRemarks.Text;
                    entity.GCHealthcareUnit = cboHealthcareUnit.Value.ToString();
                    entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateItemTransactionHd(entity);

                }
                else
                {
                    errMessage = "Penyesuaian persediaan tidak dapat diubah. Harap refresh halaman ini.";
                    result = false;
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }

        protected override bool OnVoidRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                ItemTransactionHd entity = BusinessLayer.GetItemTransactionHd(Convert.ToInt32(hdnAdjustmentID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    string filterExpressionPurchaseOrderHd = String.Format("TransactionID = {0} AND GCItemDetailStatus != '{1}'", hdnAdjustmentID.Value, Constant.TransactionStatus.VOID);
                    List<ItemTransactionDt> lstItemTransactionDt = BusinessLayer.GetItemTransactionDtList(filterExpressionPurchaseOrderHd);
                    foreach (ItemTransactionDt itemTransactionDt in lstItemTransactionDt)
                    {
                        itemTransactionDt.GCItemDetailStatus = Constant.TransactionStatus.VOID;
                        itemTransactionDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        BusinessLayer.UpdateItemTransactionDt(itemTransactionDt);
                    }

                    entity.GCHealthcareUnit = cboHealthcareUnit.Value.ToString();
                    entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateItemTransactionHd(entity);

                }
                else
                {
                    errMessage = "Penyesuaian persediaan tidak dapat diubah. Harap refresh halaman ini.";
                    result = false;
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }

        #endregion

        #region Process Detail
        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int adjustmentID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                if (hdnEntryID.Value.ToString() != "")
                {
                    adjustmentID = Convert.ToInt32(hdnAdjustmentID.Value);
                    if (OnSaveEditRecordEntityDt(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecordEntityDt(ref errMessage, ref adjustmentID))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "delete")
            {
                adjustmentID = Convert.ToInt32(hdnAdjustmentID.Value);
                if (OnDeleteEntityDt(ref errMessage, adjustmentID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpAdjustmentID"] = adjustmentID.ToString();
        }

        private void ControlToEntity(ItemTransactionDt entityDt)
        {
            entityDt.ItemID = Convert.ToInt32(hdnItemID.Value);
            entityDt.Quantity = Convert.ToDecimal(txtQuantity.Text);
            entityDt.GCItemUnit = cboItemUnit.Value.ToString();
            entityDt.GCBaseUnit = hdnGCBaseUnit.Value;
            entityDt.ConversionFactor = Convert.ToDecimal(hdnItemConversionFactor.Value);
            entityDt.BaseQuantity = entityDt.Quantity * entityDt.ConversionFactor;
            entityDt.GCAdjustmentReason = cboGCAdjustmentReason.Value.ToString();
            //entityDt.AdjustmentReason = txtAdjustmentReason.Text;
            entityDt.Remarks = txtNotesDt.Text;
            entityDt.CostAmount = Convert.ToDecimal(hdnCostAmount.Value);
        }

        private bool OnSaveAddRecordEntityDt(ref string errMessage, ref int AdjustmentID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ItemTransactionHdDao entityHdDao = new ItemTransactionHdDao(ctx);
            ItemTransactionDtDao entityDtDao = new ItemTransactionDtDao(ctx);
            try
            {
                if (cboGCAdjustmentType.Value == Constant.AdjustmentType.ISSUES && Convert.ToDecimal(txtQuantity.Text) > 0)
                {
                    result = false;
                    errMessage = "Penyesuaian persediaan dgn jenis PENGELUARAN, jumlah penyesuaian harus lebih kecil dari nol / dalam bentuk NEGATIF.";
                    ctx.RollBackTransaction();
                }
                else if (cboGCAdjustmentType.Value == Constant.AdjustmentType.RECEIPTS && Convert.ToDecimal(txtQuantity.Text) < 0)
                {
                    result = false;
                    errMessage = "Penyesuaian persediaan dgn jenis PENERIMAAN, jumlah penyesuaian harus lebih besar dari nol.";
                    ctx.RollBackTransaction();
                }
                else
                {
                    SaveItemAdjustmentHd(ctx, ref AdjustmentID);
                    if (entityHdDao.Get(AdjustmentID).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        ItemTransactionDt entityDt = new ItemTransactionDt();
                        ControlToEntity(entityDt);
                        entityDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                        entityDt.TransactionID = AdjustmentID;
                        entityDt.CreatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Insert(entityDt);
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Penyesuaian persediaan tidak dapat diubah. Harap refresh halaman ini.";
                        ctx.RollBackTransaction();
                    }
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

        private bool OnSaveEditRecordEntityDt(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ItemTransactionHdDao entityHdDao = new ItemTransactionHdDao(ctx);
            ItemTransactionDtDao entityDtDao = new ItemTransactionDtDao(ctx);
            try
            {
                if (cboGCAdjustmentType.Value == Constant.AdjustmentType.ISSUES && Convert.ToDecimal(txtQuantity.Text) > 0)
                {
                    result = false;
                    errMessage = "Penyesuaian persediaan dgn jenis PENGELUARAN, jumlah penyesuaian harus lebih kecil dari nol / dalam bentuk NEGATIF.";
                    ctx.RollBackTransaction();
                }
                else if (cboGCAdjustmentType.Value == Constant.AdjustmentType.RECEIPTS && Convert.ToDecimal(txtQuantity.Text) < 0)
                {
                    result = false;
                    errMessage = "Penyesuaian persediaan dgn jenis PENERIMAAN, jumlah penyesuaian harus lebih besar dari nol.";
                    ctx.RollBackTransaction();
                }
                else
                {
                    ItemTransactionDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                    if (entityHdDao.Get(entityDt.TransactionID).GCTransactionStatus == Constant.TransactionStatus.OPEN && entityDt.GCItemDetailStatus == Constant.TransactionStatus.OPEN)
                    {
                        ControlToEntity(entityDt);
                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(entityDt);
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Penyesuaian persediaan tidak dapat diubah. Harap refresh halaman ini.";
                        ctx.RollBackTransaction();
                    }
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

        private bool OnDeleteEntityDt(ref string errMessage, int ID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ItemTransactionHdDao entityHdDao = new ItemTransactionHdDao(ctx);
            ItemTransactionDtDao entityDtDao = new ItemTransactionDtDao(ctx);
            try
            {
                ItemTransactionDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                if (entityHdDao.Get(entityDt.TransactionID).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entityDt.GCItemDetailStatus = Constant.TransactionStatus.VOID;
                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Update(entityDt);
                    ctx.CommitTransaction();
                }
                else
                {
                    ctx.RollBackTransaction();
                    errMessage = "Penyesuaian persediaan tidak dapat diubah. Harap refresh halaman ini.";
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
        #endregion

        #region Callback
        protected void cboItemUnit_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            List<StandardCode> lst = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND (StandardCodeID IN (SELECT GCAlternateUnit FROM ItemAlternateUnit WHERE IsDeleted = 0 AND IsActive = 1 AND ItemID = {1}) OR StandardCodeID = (SELECT GCItemUnit FROM ItemMaster WHERE ItemID = {1}))", Constant.StandardCode.ITEM_UNIT, hdnItemID.Value));
            Methods.SetComboBoxField<StandardCode>(cboItemUnit, lst, "StandardCodeName", "StandardCodeID");
            cboItemUnit.SelectedIndex = -1;
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
        #endregion

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }
    }
}
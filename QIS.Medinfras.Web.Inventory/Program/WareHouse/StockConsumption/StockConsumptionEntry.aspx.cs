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
    public partial class StockConsumptionEntry : BasePageTrx
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.ITEM_CONSUMPTION;
        }

        protected override void InitializeDataControl()
        {
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

            Helper.SetControlEntrySetting(txtQuantity, new ControlEntrySetting(true, true, true), "mpTrx");
            Helper.SetControlEntrySetting(txtItemCode, new ControlEntrySetting(true, true, true), "mpTrx");
            Helper.SetControlEntrySetting(cboItemUnit, new ControlEntrySetting(true, true, true), "mpTrx");
            Helper.SetControlEntrySetting(txtConversion, new ControlEntrySetting(false, false, true), "mpTrx");

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> listStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.CONSUMPTION_TYPE, Constant.StandardCode.HEALTHCARE_UNIT));
            Methods.SetComboBoxField<StandardCode>(cboGCConsumptionType, listStandardCode.Where(p => p.ParentID == Constant.StandardCode.CONSUMPTION_TYPE).ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboHealthcareUnit, listStandardCode.Where(p => p.ParentID == Constant.StandardCode.HEALTHCARE_UNIT).ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnConsumptionID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtConsumptionNo, new ControlEntrySetting(false, false, false, ""));
            SetControlEntrySetting(txtConsumptionDate, new ControlEntrySetting(true, false, false, Constant.DefaultValueEntry.DATE_NOW));
            SetControlEntrySetting(txtConsumptionTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(lblLocation, new ControlEntrySetting(true, false));
            SetControlEntrySetting(txtLocationCode, new ControlEntrySetting(true, false, true, ""));
            SetControlEntrySetting(txtLocationName, new ControlEntrySetting(false, false, false, ""));

            SetControlEntrySetting(cboGCConsumptionType, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(cboHealthcareUnit, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false, ""));
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
            string filterExpression = String.Format("TransactionCode = '{0}' AND ServiceRequestID IS NULL", Constant.TransactionCode.ITEM_CONSUMPTION);
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
                SetControlEntrySetting(txtRemarks, new ControlEntrySetting(false, false, false));
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
            if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
            {
                cboHealthcareUnit.Enabled = true;
            }
            else
            {
                cboHealthcareUnit.Enabled = false;
            } 
            hdnConsumptionID.Value = entity.TransactionID.ToString();
            txtConsumptionNo.Text = entity.TransactionNo;
            txtConsumptionDate.Text = entity.TransactionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtConsumptionTime.Text = entity.TransactionTime;
            Location entityLoc = BusinessLayer.GetLocation(entity.FromLocationID);
            hdnGCLocationGroup.Value = entityLoc.GCLocationGroup;
            hdnLocationID.Value = entity.FromLocationID.ToString();
            txtLocationCode.Text = entity.FromLocationCode;
            txtLocationName.Text = entity.FromLocationName;

            if (entity.RegistrationID != 0 && entity.RegistrationID != null)
            {
                txtRegistrationNo.Text = string.Format("{0} | {1} | ({2}) {3}", entity.RegistrationNo, entity.ServiceUnitName, entity.MedicalNo, entity.PatientName);
            }
            else
            {
                txtRegistrationNo.Text = "";
            }
            cboGCConsumptionType.Value = entity.GCConsumptionType;
            cboHealthcareUnit.Value = entity.GCHealthcareUnit;
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
            if (hdnConsumptionID.Value != "")
                filterExpression = string.Format("TransactionID = {0} AND GCItemDetailStatus != '{1}'", hdnConsumptionID.Value, Constant.TransactionStatus.VOID);
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
            return string.Format("{0};{1};{2};", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.TransactionCode.ITEM_CONSUMPTION);
        }
        protected string OnGetFilterExpressionItemProduct()
        {
            return string.Format("GCItemType IN ('{0}','{1}','{2}') AND IsDeleted = 0", Constant.ItemGroupMaster.DRUGS, Constant.ItemGroupMaster.SUPPLIES, Constant.ItemGroupMaster.LOGISTIC);
        }
        #endregion

        #region Save Header
        public void SaveItemConsumptionHd(IDbContext ctx, ref int ConsumptionID)
        {
            ItemTransactionHdDao entityHdDao = new ItemTransactionHdDao(ctx);
            if (hdnConsumptionID.Value == "0")
            {
                ItemTransactionHd entityHd = new ItemTransactionHd();
                entityHd.TransactionDate = Helper.GetDatePickerValue(Request.Form[txtConsumptionDate.UniqueID]);
                entityHd.TransactionTime = txtConsumptionTime.Text;
                entityHd.FromLocationID = Convert.ToInt32(hdnLocationID.Value);
                entityHd.ToLocationID = null;
                entityHd.GCConsumptionType = cboGCConsumptionType.Value.ToString();
                entityHd.GCHealthcareUnit = cboHealthcareUnit.Value.ToString();
                entityHd.Remarks = txtRemarks.Text;
                entityHd.TransactionCode = Constant.TransactionCode.ITEM_CONSUMPTION;
                entityHd.TransactionNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.TransactionDate, ctx);
                if (hdnRegistrationID.Value != "" && hdnRegistrationID.Value != "0")
                {
                    entityHd.RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
                }
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                ConsumptionID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
            }
            else
            {
                ConsumptionID = Convert.ToInt32(hdnConsumptionID.Value);
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                int OrderID = 0;
                SaveItemConsumptionHd(ctx, ref OrderID);
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
                ItemTransactionHd entityHd = BusinessLayer.GetItemTransactionHd(Convert.ToInt32(hdnConsumptionID.Value));
                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entityHd.TransactionDate = Helper.GetDatePickerValue(Request.Form[txtConsumptionDate.UniqueID]);
                    entityHd.FromLocationID = Convert.ToInt32(hdnLocationID.Value);
                    entityHd.ToLocationID = null;
                    entityHd.GCConsumptionType = cboGCConsumptionType.Value.ToString();
                    entityHd.GCHealthcareUnit = cboHealthcareUnit.Value.ToString();
                    entityHd.Remarks = txtRemarks.Text;
                    entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateItemTransactionHd(entityHd);
                }
                else
                {
                    result = false;
                    errMessage = "Pemakaian barang " + entityHd.TransactionNo + " tidak dapat diubah. Harap refresh halaman ini.";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
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
            ItemRequestDtDao itemReqDtDao = new ItemRequestDtDao(ctx);
            try
            {
                ItemTransactionHd itemTransactionHd = itemTransactionHdDao.Get(Convert.ToInt32(hdnConsumptionID.Value));
                if (itemTransactionHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    string filterExpressionPurchaseOrderHd = String.Format("TransactionID = {0} AND GCItemDetailStatus != '{1}'", hdnConsumptionID.Value, Constant.TransactionStatus.VOID);
                    List<ItemTransactionDt> lstItemTransactionDt = BusinessLayer.GetItemTransactionDtList(filterExpressionPurchaseOrderHd, ctx);
                    if (lstItemTransactionDt.Count > 0)
                    {
                        List<ItemRequestDt> lstItemRequestDt = null;

                        if (itemTransactionHd.ItemRequestID != null && itemTransactionHd.ItemRequestID != 0)
                        {
                            string filterExpressionItemRequestHd = String.Format("ItemRequestID = {0} AND IsDeleted = 0 AND GCItemDetailStatus != '{1}'", itemTransactionHd.ItemRequestID, Constant.TransactionStatus.VOID);
                            lstItemRequestDt = BusinessLayer.GetItemRequestDtList(filterExpressionItemRequestHd);
                        }

                        foreach (ItemTransactionDt itemTransactionDt in lstItemTransactionDt)
                        {
                            itemTransactionDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                            itemTransactionDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            itemTransactionDtDao.Update(itemTransactionDt);

                            if (lstItemRequestDt != null)
                            {
                                int countItem = lstItemRequestDt.Where(a => a.ItemID == itemTransactionDt.ItemID).Count();
                                if (countItem > 0)
                                {
                                    ItemRequestDt iRequestDt = lstItemRequestDt.Where(a => a.ItemID == itemTransactionDt.ItemID).FirstOrDefault();
                                    iRequestDt.ApprovedConsumptionQty += itemTransactionDt.Quantity;
                                    iRequestDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    itemReqDtDao.Update(iRequestDt);
                                }
                            }
                        }

                        itemTransactionHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                        itemTransactionHd.GCHealthcareUnit = cboHealthcareUnit.Value.ToString();
                        itemTransactionHd.Remarks = txtRemarks.Text;
                        itemTransactionHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        itemTransactionHdDao.Update(itemTransactionHd);
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Pemakaian barang " + itemTransactionHd.TransactionNo + " belum memiliki Item";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Pemakaian barang " + itemTransactionHd.TransactionNo + " tidak dapat diubah. Harap refresh halaman ini.";
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

        protected override bool OnProposeRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                ItemTransactionHd entity = BusinessLayer.GetItemTransactionHd(Convert.ToInt32(hdnConsumptionID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entity.GCHealthcareUnit = cboHealthcareUnit.Value.ToString();
                    entity.Remarks = txtRemarks.Text;
                    entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateItemTransactionHd(entity);                
                }
                else
                {
                    result = false;
                    errMessage = "Pemakaian barang " + entity.TransactionNo + " tidak dapat diubah. Harap refresh halaman ini.";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
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
            IDbContext ctx = DbFactory.Configure(true);
            ItemTransactionHdDao itemTransactionHdDao = new ItemTransactionHdDao(ctx);
            ItemTransactionDtDao itemTransactionDtDao = new ItemTransactionDtDao(ctx);
            ItemRequestHdDao itemRequestHdDao = new ItemRequestHdDao(ctx);
            ItemRequestDtDao itemRequestDtDao = new ItemRequestDtDao(ctx);
            try
            {
                SettingParameterDt setvardt = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IM_ITEM_REQUEST_ALLOW_OUTSTANDING);
                string isAllowOutstanding = setvardt.ParameterValue == null ? "1" : setvardt.ParameterValue;

                ItemTransactionHd entity = itemTransactionHdDao.Get(Convert.ToInt32(hdnConsumptionID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entity.GCHealthcareUnit = cboHealthcareUnit.Value.ToString();
                    entity.Remarks = txtRemarks.Text;
                    entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    itemTransactionHdDao.Update(entity);

                    string filterExpressionItemTransactionHd = String.Format("TransactionID = {0} AND GCItemDetailStatus != '{1}'",
                                                                entity.TransactionID, Constant.TransactionStatus.VOID);
                    List<ItemTransactionDt> lstItemTransactionDt = BusinessLayer.GetItemTransactionDtList(filterExpressionItemTransactionHd, ctx);
                    foreach (ItemTransactionDt itemDt in lstItemTransactionDt)
                    {
                        itemDt.GCItemDetailStatus = Constant.DistributionStatus.VOID;
                        itemDt.LastUpdatedBy = AppSession.UserLogin.UserID;

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        itemTransactionDtDao.Update(itemDt);

                        if (entity.ItemRequestID != null && entity.ItemRequestID != 0)
                        {
                            string filterItemRequestDt = string.Format("ItemRequestID = {0} AND ItemID = {1} AND GCItemDetailStatus IN ('{2}','{3}')",
                                                                    entity.ItemRequestID, itemDt.ItemID, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED);
                            ItemRequestDt irequestDt = BusinessLayer.GetItemRequestDtList(filterItemRequestDt, ctx).FirstOrDefault();
                            if (irequestDt != null)
                            {
                                irequestDt.ConsumptionQty = irequestDt.ConsumptionQty - itemDt.Quantity;

                                if (isAllowOutstanding == "1")
                                {
                                    if (irequestDt.GCItemRequestType == Constant.ItemRequestType.DISTRIBUTION)
                                    {
                                        if (irequestDt.PurchaseRequestQty == 0 && irequestDt.DistributionQty == 0)
                                        {
                                            irequestDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                                        }

                                        if (isAllowOutstanding == "1")
                                        {
                                            if (irequestDt.GCItemDetailStatus == Constant.TransactionStatus.CLOSED)
                                            {
                                                irequestDt.GCItemDetailStatus = Constant.TransactionStatus.PROCESSED;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (irequestDt.PurchaseRequestQty == 0 && irequestDt.ConsumptionQty == 0)
                                        {
                                            irequestDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                                        }

                                        if (irequestDt.GCItemDetailStatus == Constant.TransactionStatus.CLOSED)
                                        {
                                            irequestDt.GCItemDetailStatus = Constant.TransactionStatus.PROCESSED;
                                        }
                                    }
                                }
                                irequestDt.LastUpdatedBy = AppSession.UserLogin.UserID;

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                itemRequestDtDao.Update(irequestDt);
                            }
                        }
                    }

                    if (isAllowOutstanding == "1")
                    {
                        if (entity.ItemRequestID != null && entity.ItemRequestID != 0)
                        {
                            string filterItemRequestHd = string.Format("ItemRequestID = {0}", entity.ItemRequestID);
                            ItemRequestHd irequestHd = BusinessLayer.GetItemRequestHdList(filterItemRequestHd, ctx).FirstOrDefault();

                            string filterItemRequestDtLstApp = string.Format("ItemRequestID = {0} AND GCItemDetailStatus = '{1}'",
                                                                    entity.ItemRequestID, Constant.TransactionStatus.APPROVED);
                            string filterItemRequestDtLstProc = string.Format("ItemRequestID = {0} AND GCItemDetailStatus = '{1}'",
                                                                    entity.ItemRequestID, Constant.TransactionStatus.PROCESSED);
                            string filterItemRequestDtLstALL = string.Format("ItemRequestID = {0} AND GCItemDetailStatus != '{1}'",
                                                                    entity.ItemRequestID, Constant.TransactionStatus.VOID);
                            int jumlahItemRequestDtApproved = BusinessLayer.GetItemRequestDtRowCount(filterItemRequestDtLstApp, ctx);
                            int jumlahItemRequestDtProcessed = BusinessLayer.GetItemRequestDtRowCount(filterItemRequestDtLstProc, ctx);
                            int jumlahItemRequestDtALL = BusinessLayer.GetItemRequestDtRowCount(filterItemRequestDtLstALL, ctx);

                            if (jumlahItemRequestDtApproved == jumlahItemRequestDtALL)
                            {
                                irequestHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                            }

                            if (jumlahItemRequestDtProcessed == jumlahItemRequestDtALL)
                            {
                                irequestHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                            }

                            irequestHd.LastUpdatedBy = AppSession.UserLogin.UserID;

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            itemRequestHdDao.Update(irequestHd);
                        }
                    }

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Pemakaian barang " + entity.TransactionNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
                    adjustmentID = Convert.ToInt32(hdnConsumptionID.Value);
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
                adjustmentID = Convert.ToInt32(hdnConsumptionID.Value);
                if (OnDeleteEntityDt(ref errMessage, adjustmentID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpConsumptionID"] = adjustmentID.ToString();
        }

        private void ControlToEntity(ItemTransactionDt entityDt)
        {
            entityDt.ItemID = Convert.ToInt32(hdnItemID.Value);
            entityDt.Quantity = Convert.ToDecimal(txtQuantity.Text);
            entityDt.GCItemUnit = cboItemUnit.Value.ToString();
            entityDt.GCBaseUnit = hdnGCBaseUnit.Value;
            entityDt.ConversionFactor = Convert.ToDecimal(hdnItemConversionFactor.Value);
            entityDt.BaseQuantity = entityDt.Quantity * entityDt.ConversionFactor;
            //entityDt.GCConsumptionReason = cboGCConsumptionReason.Value.ToString();
            //entityDt.ConsumptionReason = txtConsumptionReason.Text;
            entityDt.CostAmount = Convert.ToDecimal(hdnCostAmount.Value);
            entityDt.Remarks = txtNotesDt.Text;
        }

        private bool OnSaveAddRecordEntityDt(ref string errMessage, ref int ConsumptionID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ItemTransactionHdDao entityHdDao = new ItemTransactionHdDao(ctx);
            ItemTransactionDtDao entityDtDao = new ItemTransactionDtDao(ctx);
            try
            {
                SaveItemConsumptionHd(ctx, ref ConsumptionID);
                if (entityHdDao.Get(ConsumptionID).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    ItemTransactionDt entityDt = new ItemTransactionDt();
                    ControlToEntity(entityDt);
                    entityDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                    entityDt.TransactionID = ConsumptionID;
                    entityDt.CreatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Insert(entityDt);
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Pemakaian barang " + entityHdDao.Get(ConsumptionID).TransactionNo + " tidak dapat diubah. Harap refresh halaman ini.";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
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
                    errMessage = "Pemakaian barang " + entityHdDao.Get(entityDt.TransactionID).TransactionNo + " tidak dapat diubah. Harap refresh halaman ini.";
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

        private bool OnDeleteEntityDt(ref string errMessage, int ID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ItemTransactionHdDao entityHdDao = new ItemTransactionHdDao(ctx);
            ItemTransactionDtDao entityDtDao = new ItemTransactionDtDao(ctx);
            ItemRequestHdDao itemRequestHdDao = new ItemRequestHdDao(ctx);
            ItemRequestDtDao itemRequestDtDao = new ItemRequestDtDao(ctx);
            try
            {
                SettingParameterDt setvardt = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IM_ITEM_REQUEST_ALLOW_OUTSTANDING);
                string isAllowOutstanding = setvardt.ParameterValue == null ? "1" : setvardt.ParameterValue;

                ItemTransactionDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                if (entityHdDao.Get(entityDt.TransactionID).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entityDt.GCItemDetailStatus = Constant.TransactionStatus.VOID;
                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Update(entityDt);

                    if (entityHdDao.Get(entityDt.TransactionID).ItemRequestID != null && entityHdDao.Get(entityDt.TransactionID).ItemRequestID != 0)
                    {
                        string filterItemRequestDt = string.Format("ItemRequestID = {0} AND ItemID = {1} AND GCItemDetailStatus IN ('{2}','{3}')",
                                                                entityHdDao.Get(entityDt.TransactionID).ItemRequestID, entityDt.ItemID, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED);
                        ItemRequestDt irequestDt = BusinessLayer.GetItemRequestDtList(filterItemRequestDt).FirstOrDefault();
                        if (irequestDt != null)
                        {
                            irequestDt.ConsumptionQty = irequestDt.ConsumptionQty - entityDt.Quantity;
                            if (isAllowOutstanding == "1")
                            {
                                if (irequestDt.GCItemRequestType == Constant.ItemRequestType.DISTRIBUTION)
                                {
                                    if (irequestDt.PurchaseRequestQty == 0 && irequestDt.DistributionQty == 0)
                                    {
                                        irequestDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                                    }

                                    if (irequestDt.GCItemDetailStatus == Constant.TransactionStatus.CLOSED)
                                    {
                                        irequestDt.GCItemDetailStatus = Constant.TransactionStatus.PROCESSED;
                                    }
                                }
                                else
                                {
                                    if (irequestDt.PurchaseRequestQty == 0 && irequestDt.ConsumptionQty == 0)
                                    {
                                        irequestDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                                    }

                                    if (irequestDt.GCItemDetailStatus == Constant.TransactionStatus.CLOSED)
                                    {
                                        irequestDt.GCItemDetailStatus = Constant.TransactionStatus.PROCESSED;
                                    }
                                }
                            }
                            irequestDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            itemRequestDtDao.Update(irequestDt);

                            if (isAllowOutstanding == "1")
                            {
                                string filterItemRequestHd = string.Format("ItemRequestID = {0}", irequestDt.ItemRequestID);
                                ItemRequestHd irequestHd = BusinessLayer.GetItemRequestHdList(filterItemRequestHd).FirstOrDefault();
                                if (irequestDt.ConsumptionQty == 0)
                                {
                                    string filterItemRequestDtLst = string.Format("ItemRequestID = {0} AND GCItemDetailStatus = '{1}'",
                                                                            irequestDt.ItemRequestID, Constant.TransactionStatus.APPROVED);
                                    string filterItemRequestDtLstALL = string.Format("ItemRequestID = {0} AND GCItemDetailStatus != '{1}'",
                                                                            irequestDt.ItemRequestID, Constant.TransactionStatus.VOID);
                                    int jumlahItemRequestDtApproved = BusinessLayer.GetItemRequestDtRowCount(filterItemRequestDtLst, ctx);
                                    int jumlahItemRequestDtALL = BusinessLayer.GetItemRequestDtRowCount(filterItemRequestDtLstALL, ctx);

                                    if (jumlahItemRequestDtApproved == jumlahItemRequestDtALL)
                                    {
                                        irequestHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                                    }
                                }
                                if (irequestDt.ConsumptionQty < irequestDt.Quantity)
                                {
                                    irequestHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                                }

                                irequestHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                itemRequestHdDao.Update(irequestHd);
                            }
                        }
                    }

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Pemakaian barang " + entityHdDao.Get(entityDt.TransactionID).TransactionNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
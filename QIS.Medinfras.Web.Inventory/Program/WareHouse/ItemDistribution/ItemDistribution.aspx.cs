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
    public partial class ItemDistribution : BasePageTrx
    {
        protected string filterExpressionItemProduct = "";
        protected string filterExpressionLocation = "";
        protected string filterExpressionLocationTo = "";
        protected int PageCount = 1;

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.ITEM_DISTRIBUTION;
        }

        protected override void InitializeDataControl()
        {
            filterExpressionItemProduct = string.Format("GCItemType IN ('{0}','{1}','{2}','{3}') AND IsDeleted = 0", Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, Constant.ItemType.BARANG_UMUM, Constant.ItemType.BAHAN_MAKANAN);
            filterExpressionLocation = string.Format("{0};{1};{2};", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.TransactionCode.ITEM_DISTRIBUTION);
            filterExpressionLocationTo = string.Format("{0};0;{1};", AppSession.UserLogin.HealthcareID, Constant.TransactionCode.ITEM_REQUEST);

            int count = BusinessLayer.GetLocationUserRowCount(string.Format("UserID = {0} AND IsDeleted = 0", AppSession.UserLogin.UserID));
            if (count > 0)
                hdnRecordFilterExpression.Value = string.Format("ServiceRequestID IS NULL AND FromLocationID IN (SELECT LocationID FROM LocationUser WHERE UserID = {0} AND IsDeleted = 0)", AppSession.UserLogin.UserID);
            else
            {
                count = BusinessLayer.GetLocationUserRoleRowCount(string.Format("RoleID IN (SELECT RoleID FROM UserInRole WHERE UserID = {0} AND HealthcareID = '{1}') AND IsDeleted = 0", AppSession.UserLogin.UserID, AppSession.UserLogin.HealthcareID));
                if (count > 0)
                    hdnRecordFilterExpression.Value = string.Format("ServiceRequestID IS NULL AND FromLocationID IN (SELECT LocationID FROM LocationUserRole WHERE RoleID IN (SELECT RoleID FROM UserInRole WHERE UserID = {0} AND HealthcareID = '{1}') AND IsDeleted = 0)", AppSession.UserLogin.UserID, AppSession.UserLogin.HealthcareID);
                else
                    hdnRecordFilterExpression.Value = "ServiceRequestID IS NULL";
            }

            hdnIsAutoReceived.Value = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IM_KONFIRMASI_PENERIMAAN_DISTRIBUSI_BARANG).ParameterValue == "1" ? "0" : "1";

            BindGridView(1, true, ref PageCount);
            Helper.SetControlEntrySetting(txtQuantity, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtItemCode, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(cboItemUnit, new ControlEntrySetting(true, true, true), "mpTrxPopup");

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            
            string filterSetVar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
                                                        AppSession.UserLogin.HealthcareID,
                                                        Constant.SettingParameter.IM_IS_QTY_DISTRIBUTION_CANNOT_OVER_REQUEST,
                                                        Constant.SettingParameter.IM_DISTRIBUTION_ALLOWED_WITHOUT_REQUEST
                                                    );
            List<SettingParameterDt> setvarList = BusinessLayer.GetSettingParameterDtList(filterSetVar);
            hdnIsDistributionQty.Value = setvarList.Where(a => a.ParameterCode == Constant.SettingParameter.IM_IS_QTY_DISTRIBUTION_CANNOT_OVER_REQUEST).FirstOrDefault().ParameterValue;
            hdnIsDistributionAllowedWithoutRequest.Value = setvarList.Where(a => a.ParameterCode == Constant.SettingParameter.IM_DISTRIBUTION_ALLOWED_WITHOUT_REQUEST).FirstOrDefault().ParameterValue;

        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnDistributionID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtDistributionNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblLocation, new ControlEntrySetting(true, false));
            SetControlEntrySetting(txtLocationCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtLocationName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(hdnLocationItemGroupID, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(txtItemTransactionDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtItemTransactionTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(lblLocationTo, new ControlEntrySetting(true, false));
            SetControlEntrySetting(txtLocationCodeTo, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtLocationNameTo, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false));
        }

        #region Load Entity
        protected string GetFilterExpression()
        {
            return hdnRecordFilterExpression.Value;
        }

        public override int OnGetRowCount()
        {
            string filterExpression = GetFilterExpression();
            return BusinessLayer.GetvItemDistributionHdRowCount(filterExpression);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText, ref string retval)
        {
            string filterExpression = GetFilterExpression();
            vItemDistributionHd entity = new vItemDistributionHd();
            if (!string.IsNullOrEmpty(filterExpression))
            {
                if (!string.IsNullOrEmpty(retval))
                {
                    filterExpression += string.Format(" AND DistributionID = {0}", retval);
                    entity = BusinessLayer.GetvItemDistributionHdList(filterExpression).FirstOrDefault();
                }
                else
                {
                    entity = BusinessLayer.GetvItemDistributionHd(filterExpression, PageIndex, "DistributionID DESC");
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(retval))
                {
                    filterExpression = string.Format("DistributionID = {0}", retval);
                    entity = BusinessLayer.GetvItemDistributionHdList(filterExpression).FirstOrDefault();
                }
                else
                {
                    entity = BusinessLayer.GetvItemDistributionHd(filterExpression, PageIndex, "DistributionID DESC");
                }
            }
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            vItemDistributionHd entity = BusinessLayer.GetvItemDistributionHd(filterExpression, PageIndex, "DistributionID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            PageIndex = BusinessLayer.GetvItemDistributionHdRowIndex(filterExpression, keyValue, "DistributionID DESC");
            vItemDistributionHd entity = BusinessLayer.GetvItemDistributionHd(filterExpression, PageIndex, "DistributionID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }
        #endregion

        public override void OnAddRecord()
        {
            hdnPageCount.Value = "0";
            hdnIsEditable.Value = "1";
        }

        protected string IsEditable()
        {
            return hdnIsEditable.Value;
        }

        private void EntityToControl(vItemDistributionHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity.GCDistributionStatus != Constant.DistributionStatus.OPEN)
            {
                isShowWatermark = true;
                watermarkText = entity.DistributionStatusWatermark;
                hdnIsEditable.Value = "0";
                SetControlEntrySetting(txtNotes, new ControlEntrySetting(false, false, false));
            }
            else
                hdnIsEditable.Value = "1";

            if (entity.GCDistributionStatus != Constant.DistributionStatus.OPEN && entity.GCDistributionStatus != Constant.DistributionStatus.VOID)
                hdnPrintStatus.Value = "true";
            else
                hdnPrintStatus.Value = "false";

            hdnDistributionID.Value = entity.DistributionID.ToString();
            txtDistributionNo.Text = entity.DistributionNo;
            txtItemTransactionDate.Text = entity.TransactionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtItemTransactionTime.Text = entity.TransactionTime;
            hdnGCLocationGroup.Value = entity.FromGCLocationGroup;
            hdnLocationIDFrom.Value = entity.FromLocationID.ToString();
            txtLocationCode.Text = entity.FromLocationCode;
            txtLocationName.Text = entity.FromLocationName;
            hdnLocationIDTo.Value = entity.ToLocationID.ToString();
            txtLocationCodeTo.Text = entity.ToLocationCode;
            txtLocationNameTo.Text = entity.ToLocationName;
            hdnLocationItemGroupID.Value = entity.FromLocationItemGroupID.ToString();
            txtNotes.Text = entity.DeliveryRemarks;

            if (entity.RegistrationID != 0 && entity.RegistrationID != null)
            {
                txtRegistrationNo.Text = string.Format("{0} | {1} | ({2}) {3}", entity.RegistrationNo, entity.ServiceUnitName, entity.MedicalNo, entity.PatientName);
            }
            else
            {
                txtRegistrationNo.Text = "";
            }

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
        }


        #region Save & Edit Header

        private void ControlToEntityHd(ItemDistributionHd entityHd)
        {
            entityHd.FromLocationID = Convert.ToInt32(hdnLocationIDFrom.Value);
            entityHd.ToLocationID = Convert.ToInt32(hdnLocationIDTo.Value);
        }

        public void SaveItemDistributionHd(IDbContext ctx, ref int distributionID)
        {
            ItemDistributionHdDao entityHdDao = new ItemDistributionHdDao(ctx);
            if (hdnDistributionID.Value == "0")
            {
                ItemDistributionHd entityHd = new ItemDistributionHd();
                ControlToEntityHd(entityHd);
                entityHd.TransactionDate = DateTime.Now;
                entityHd.TransactionTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                entityHd.DistributionNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.ITEM_DISTRIBUTION, entityHd.TransactionDate, ctx);
                entityHd.GCDistributionStatus = Constant.DistributionStatus.OPEN;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                distributionID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
            }
            else
            {
                distributionID = Convert.ToInt32(hdnDistributionID.Value);
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                int distributionID = 0;
                SaveItemDistributionHd(ctx, ref distributionID);
                retval = distributionID.ToString();
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
                ItemDistributionHd entity = BusinessLayer.GetItemDistributionHd(Convert.ToInt32(hdnDistributionID.Value));
                if (entity.GCDistributionStatus == Constant.DistributionStatus.OPEN)
                {
                    entity.DeliveryRemarks = txtNotes.Text;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateItemDistributionHd(entity);
                }
                else
                {
                    result = false;
                    errMessage = "Distribusi " + entity.DistributionNo + " tidak dapat diubah. Harap refresh halaman ini.";
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

        #region Approve Proposed Void Entity
        protected override bool OnApproveRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ItemDistributionHdDao itemHdDao = new ItemDistributionHdDao(ctx);
            ItemDistributionDtDao itemDtDao = new ItemDistributionDtDao(ctx);
            ItemRequestDtDao itemReqDtDao = new ItemRequestDtDao(ctx);
            try
            {
                string GCDistributionStatus = Constant.DistributionStatus.ON_DELIVERY;
                if (hdnIsAutoReceived.Value == "1")
                {
                    GCDistributionStatus = Constant.DistributionStatus.RECEIVED;
                }

                ItemDistributionHd itemHd = itemHdDao.Get(Convert.ToInt32(hdnDistributionID.Value));
                retval = hdnDistributionID.Value; 
                if (itemHd.GCDistributionStatus == Constant.DistributionStatus.OPEN)
                {
                    ControlToEntityHd(itemHd);
                    itemHd.GCDistributionStatus = GCDistributionStatus;
                    itemHd.DeliveryDate = DateTime.Now;
                    itemHd.DeliveryTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                    itemHd.DeliveryRemarks = txtNotes.Text;
                    itemHd.DeliveredBy = BusinessLayer.GetUser(AppSession.UserLogin.UserID).UserName;
                    itemHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    itemHdDao.Update(itemHd);

                    List<ItemRequestDt> lstItemRequestDt = null;
                    if (itemHd.ItemRequestID != null && itemHd.ItemRequestID != 0)
                    {
                        string filterExpressionItemRequestHd = String.Format("ItemRequestID = {0} AND IsDeleted = 0 AND GCItemDetailStatus != '{1}'", itemHd.ItemRequestID, Constant.TransactionStatus.VOID);
                        lstItemRequestDt = BusinessLayer.GetItemRequestDtList(filterExpressionItemRequestHd, ctx);
                    }

                    string filterExpressionItemDistributionHd = String.Format("DistributionID = {0} AND IsDeleted = 0", hdnDistributionID.Value);
                    List<ItemDistributionDt> lstItemDistributionDt = BusinessLayer.GetItemDistributionDtList(filterExpressionItemDistributionHd, ctx);
                    foreach (ItemDistributionDt itemDt in lstItemDistributionDt)
                    {
                        itemDt.GCItemDetailStatus = GCDistributionStatus;
                        itemDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        itemDtDao.Update(itemDt);

                        if (lstItemRequestDt != null)
                        {
                            int countItem = lstItemRequestDt.Where(a => a.ItemID == itemDt.ItemID).Count();
                            if (countItem > 0)
                            {
                                ItemRequestDt iRequestDt = lstItemRequestDt.Where(a => a.ItemID == itemDt.ItemID).FirstOrDefault();
                                iRequestDt.ApprovedDistributionQty += itemDt.Quantity;
                                iRequestDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                itemReqDtDao.Update(iRequestDt);
                            }
                        }
                    }

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Distribusi " + itemHd.DistributionNo + " tidak dapat diubah. Harap refresh halaman ini.";
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

        protected override bool OnProposeRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            try
            {
                ItemDistributionHd entity = BusinessLayer.GetItemDistributionHd(Convert.ToInt32(hdnDistributionID.Value));
                if (entity.GCDistributionStatus == Constant.DistributionStatus.OPEN)
                {
                    ControlToEntityHd(entity);
                    entity.DeliveryRemarks = txtNotes.Text;
                    entity.GCDistributionStatus = Constant.DistributionStatus.WAIT_FOR_APPROVAL;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateItemDistributionHd(entity);

                    string filterExpressionItemDistributionHd = String.Format("DistributionID = {0} AND IsDeleted = 0", hdnDistributionID.Value);
                    List<ItemDistributionDt> lstItemDistributionDt = BusinessLayer.GetItemDistributionDtList(filterExpressionItemDistributionHd);

                    foreach (ItemDistributionDt itemDt in lstItemDistributionDt)
                    {
                        itemDt.GCItemDetailStatus = Constant.DistributionStatus.WAIT_FOR_APPROVAL;
                        itemDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        BusinessLayer.UpdateItemDistributionDt(itemDt);
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Distribusi " + entity.DistributionNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
            ItemDistributionHdDao itemDistributionHdDao = new ItemDistributionHdDao(ctx);
            ItemDistributionDtDao itemDistributionDtDao = new ItemDistributionDtDao(ctx);
            ItemRequestHdDao itemRequestHdDao = new ItemRequestHdDao(ctx);
            ItemRequestDtDao itemRequestDtDao = new ItemRequestDtDao(ctx);
            try
            {
                SettingParameterDt setvardt = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IM_ITEM_REQUEST_ALLOW_OUTSTANDING);
                string isAllowOutstanding = setvardt.ParameterValue == null ? "1" : setvardt.ParameterValue;

                ItemDistributionHd entity = itemDistributionHdDao.Get(Convert.ToInt32(hdnDistributionID.Value));
                if (entity.GCDistributionStatus == Constant.DistributionStatus.OPEN)
                {
                    entity.DeliveryRemarks = txtNotes.Text;
                    entity.GCDistributionStatus = Constant.DistributionStatus.VOID;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    itemDistributionHdDao.Update(entity);

                    string filterExpressionItemDistributionHd = String.Format("DistributionID = {0} AND IsDeleted = 0", hdnDistributionID.Value);
                    List<ItemDistributionDt> lstItemDistributionDt = BusinessLayer.GetItemDistributionDtList(filterExpressionItemDistributionHd, ctx);
                    foreach (ItemDistributionDt itemDt in lstItemDistributionDt)
                    {
                        itemDt.GCItemDetailStatus = Constant.DistributionStatus.VOID;
                        itemDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        itemDistributionDtDao.Update(itemDt);

                        if (entity.ItemRequestID != null && entity.ItemRequestID != 0)
                        {
                            string filterItemRequestDt = string.Format("ItemRequestID = {0} AND ItemID = {1} AND GCItemDetailStatus IN ('{2}','{3}')",
                                                                    entity.ItemRequestID, itemDt.ItemID, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED);
                            ItemRequestDt irequestDt = BusinessLayer.GetItemRequestDtList(filterItemRequestDt, ctx).FirstOrDefault();
                            if (irequestDt != null)
                            {
                                irequestDt.DistributionQty = irequestDt.DistributionQty - itemDt.Quantity;
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
                            ItemRequestHd irequestHd = BusinessLayer.GetItemRequestHdList(filterItemRequestHd).FirstOrDefault();

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
                    errMessage = "Distribusi " + entity.DistributionNo + " tidak dapat diubah. Harap refresh halaman ini.";
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

        #endregion

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnDistributionID.Value != "")
                filterExpression = string.Format("DistributionID = {0} AND IsDeleted = 0", hdnDistributionID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvItemDistributionDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vItemDistributionDt> lstEntity = BusinessLayer.GetvItemDistributionDtList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ItemName1 ASC");
            hdnPageCount.Value = pageCount.ToString();
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        #region callBack Trigger
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

        #region Process Detail
        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int distributionID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                if (hdnEntryID.Value.ToString() != "")
                {
                    distributionID = Convert.ToInt32(hdnDistributionID.Value);
                    if (OnSaveEditRecordEntityDt(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecordEntityDt(ref errMessage, ref distributionID))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "delete")
            {
                distributionID = Convert.ToInt32(hdnDistributionID.Value);
                if (OnDeleteEntityDt(ref errMessage, distributionID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpOrderID"] = distributionID.ToString();
        }

        private void ControlToEntity(ItemDistributionDt entityDt)
        {
            entityDt.ItemID = Convert.ToInt32(hdnItemID.Value);
            entityDt.Quantity = Convert.ToDecimal(txtQuantity.Text);
            entityDt.GCItemUnit = cboItemUnit.Value.ToString();
            entityDt.GCBaseUnit = hdnGCBaseUnit.Value;
            entityDt.ConversionFactor = Convert.ToDecimal(hdnItemUnitValue.Value);
            entityDt.GCItemDetailStatus = Constant.DistributionStatus.OPEN;

            ItemPlanning entityPlanning = BusinessLayer.GetItemPlanningList(string.Format("ItemID = {0}", Convert.ToInt32(hdnItemID.Value))).FirstOrDefault();
            if (entityPlanning != null)
            {
                entityDt.AveragePrice = entityPlanning.AveragePrice;
            }
            else
            {
                entityDt.AveragePrice = 0;
            }

        }

        private bool OnSaveAddRecordEntityDt(ref string errMessage, ref int distributionID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ItemDistributionHdDao entityHdDao = new ItemDistributionHdDao(ctx);
            ItemDistributionDtDao entityDtDao = new ItemDistributionDtDao(ctx);
            try
            {
                SaveItemDistributionHd(ctx, ref distributionID);
                if (entityHdDao.Get(distributionID).GCDistributionStatus == Constant.DistributionStatus.OPEN)
                {
                    ItemDistributionDt entityDt = new ItemDistributionDt();
                    ControlToEntity(entityDt);
                    entityDt.DistributionID = distributionID;
                    entityDt.CreatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Insert(entityDt);
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Distribusi " + entityHdDao.Get(distributionID).DistributionNo + " tidak dapat diubah. Harap refresh halaman ini.";
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

        private bool OnSaveEditRecordEntityDt(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ItemDistributionHdDao entityHdDao = new ItemDistributionHdDao(ctx);
            ItemDistributionDtDao entityDtDao = new ItemDistributionDtDao(ctx);
            ItemRequestHdDao RequestHdDao = new ItemRequestHdDao(ctx);
            ItemRequestDtDao RequestDtDao = new ItemRequestDtDao(ctx);

            try
            {
                ItemDistributionDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                ItemDistributionHd entityHd = entityHdDao.Get(entityDt.DistributionID);
                if (hdnIsDistributionQty.Value == "1")
                {
                    if (entityHd.GCDistributionStatus == Constant.DistributionStatus.OPEN && entityDt.IsDeleted == false && entityDt.GCItemDetailStatus == Constant.DistributionStatus.OPEN)
                    {
                        if (entityHd.ItemRequestID != 0 && entityHd.ItemRequestID != null)
                        {
                            ItemRequestHd RequestHd = RequestHdDao.Get(Convert.ToInt32(entityHd.ItemRequestID));
                            
                            String filterExpression1 = string.Format("ItemRequestID = {0} AND ItemID = {1} AND Isdeleted = 0 AND GCItemDetailStatus NOT IN ('{2}')", entityHd.ItemRequestID, entityDt.ItemID, Constant.TransactionStatus.VOID);
                            ItemRequestDt RequestDt = BusinessLayer.GetItemRequestDtList(filterExpression1, ctx).FirstOrDefault();

                            decimal RequestQty = 0;
                            if (RequestDt.GCBaseUnit != RequestDt.GCItemUnit)
                            {
                                RequestQty = RequestDt.Quantity * RequestDt.ConversionFactor;
                            }
                            else
                            {
                                RequestQty = RequestDt.Quantity;
                            }

                            decimal DistributionOtherQty = 0;

                            String filterExpression2 = string.Format("ItemRequestID = {0} AND DistributionID != {1} AND GCDistributionStatus NOT IN ('{2}')", entityHd.ItemRequestID, entityHd.DistributionID, Constant.DistributionStatus.VOID);
                            List<ItemDistributionHd> lstDistributionHdOther = BusinessLayer.GetItemDistributionHdList(filterExpression2, ctx);
                            if (lstDistributionHdOther.Count > 0)
                            {
                                foreach (ItemDistributionHd e in lstDistributionHdOther)
                                {
                                    String filterExpression3 = string.Format("DistributionID = {0} AND ItemID = {1} AND IsDeleted = 0 AND GCItemDetailStatus NOT IN ('{2}')", e.DistributionID, entityDt.ItemID, Constant.DistributionStatus.VOID);
                                    ItemDistributionDt distributionDtOther = BusinessLayer.GetItemDistributionDtList(filterExpression3, ctx).FirstOrDefault();
                                    if (distributionDtOther != null)
                                    {
                                        if (distributionDtOther.GCBaseUnit != distributionDtOther.GCItemUnit)
                                        {
                                            decimal qty = distributionDtOther.Quantity * distributionDtOther.ConversionFactor;
                                            DistributionOtherQty = DistributionOtherQty + qty;
                                        }
                                        else
                                        {
                                            DistributionOtherQty = DistributionOtherQty + distributionDtOther.Quantity;
                                        }
                                    }
                                }
                            }

                            decimal qtyValid = RequestQty + DistributionOtherQty;

                            ControlToEntity(entityDt);
                            entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityDtDao.Update(entityDt);

                            decimal qtyInput = 0;
                            if (entityDt.GCItemUnit != entityDt.GCBaseUnit)
                            {
                                qtyInput = entityDt.Quantity * entityDt.ConversionFactor;
                            }
                            else
                            {
                                qtyInput = entityDt.Quantity;
                            }

                            if (qtyInput <= qtyValid)
                            {
                                ctx.CommitTransaction();
                            }
                            else
                            {
                                result = false;
                                errMessage = "Quantity tidak boleh lebih besar dari Permintaan Barang";
                                Exception ex = new Exception(errMessage);
                                Helper.InsertErrorLog(ex);
                                ctx.RollBackTransaction();
                            }
                        }
                        else
                        {
                            ControlToEntity(entityDt);
                            entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entityDt);
                            ctx.CommitTransaction();
                        }
                    }
                    else
                    {
                        result = false;
                        errMessage = "Distribusi " + entityHdDao.Get(entityDt.DistributionID).DistributionNo + " tidak dapat diubah. Harap refresh halaman ini.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    ControlToEntity(entityDt);
                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Update(entityDt);
                    ctx.CommitTransaction();
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
            ItemDistributionHdDao entityHdDao = new ItemDistributionHdDao(ctx);
            ItemDistributionDtDao entityDtDao = new ItemDistributionDtDao(ctx);
            ItemRequestHdDao itemRequestHdDao = new ItemRequestHdDao(ctx);
            ItemRequestDtDao itemRequestDtDao = new ItemRequestDtDao(ctx);
            try
            {
                SettingParameterDt setvardt = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IM_ITEM_REQUEST_ALLOW_OUTSTANDING);
                string isAllowOutstanding = setvardt.ParameterValue == null ? "1" : setvardt.ParameterValue;

                ItemDistributionDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                if (entityHdDao.Get(entityDt.DistributionID).GCDistributionStatus == Constant.DistributionStatus.OPEN)
                {
                    entityDt.IsDeleted = true;
                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityDtDao.Update(entityDt);

                    if (entityHdDao.Get(entityDt.DistributionID).ItemRequestID != null && entityHdDao.Get(entityDt.DistributionID).ItemRequestID != 0)
                    {
                        string filterItemRequestDt = string.Format("ItemRequestID = {0} AND ItemID = {1} AND GCItemDetailStatus IN ('{2}','{3}')",
                                                                entityHdDao.Get(entityDt.DistributionID).ItemRequestID, entityDt.ItemID, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED);
                        ItemRequestDt irequestDt = BusinessLayer.GetItemRequestDtList(filterItemRequestDt, ctx).FirstOrDefault();
                        if (irequestDt != null)
                        {
                            irequestDt.DistributionQty = irequestDt.DistributionQty - entityDt.Quantity;
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
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            itemRequestDtDao.Update(irequestDt);

                            if (isAllowOutstanding == "1")
                            {
                                string filterItemRequestHd = string.Format("ItemRequestID = {0}", irequestDt.ItemRequestID);
                                ItemRequestHd irequestHd = BusinessLayer.GetItemRequestHdList(filterItemRequestHd, ctx).FirstOrDefault();
                                if (irequestDt.DistributionQty == 0)
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
                                if (irequestDt.DistributionQty < irequestDt.Quantity)
                                {
                                    irequestHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                                }

                                irequestHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                itemRequestHdDao.Update(irequestHd);
                            }
                        }
                    }

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Distribusi " + entityHdDao.Get(entityDt.DistributionID).DistributionNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
    }
}
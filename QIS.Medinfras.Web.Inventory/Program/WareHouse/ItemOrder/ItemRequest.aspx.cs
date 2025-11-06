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
    public partial class ItemRequest : BasePageTrx
    {
        protected string filterExpressionItemProduct = "";
        protected string filterExpressionLocation = "";
        protected string filterExpressionLocationTo = "";
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.ITEM_REQUEST;
        }

        protected override void InitializeDataControl()
        {
            #region SettingParameter

            GetSettingParameter();

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
            #endregion

            filterExpressionItemProduct = string.Format("GCItemType IN ('{0}','{1}','{2}','{3}') AND IsDeleted = 0", Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, Constant.ItemType.BARANG_UMUM, Constant.ItemType.BAHAN_MAKANAN);
            filterExpressionLocation = string.Format("{0};{1};{2};", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.TransactionCode.ITEM_REQUEST);
            filterExpressionLocationTo = string.Format("{0};0;{1};", AppSession.UserLogin.HealthcareID, Constant.TransactionCode.ITEM_DISTRIBUTION);

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

            string filterExpression = string.Format("ParentID IN ('{0}') AND IsDeleted = 0", Constant.StandardCode.TRANSACTION_TYPE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboTransactionType, lstStandardCode, "StandardCodeName", "StandardCodeID");
            cboTransactionType.SelectedIndex = 0;

            BindGridView(1, true, ref PageCount);
            Helper.SetControlEntrySetting(txtQuantity, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtItemCode, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(cboItemUnit, new ControlEntrySetting(true, true, true), "mpTrxPopup");

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        private void GetSettingParameter()
        {
            string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')", AppSession.UserLogin.HealthcareID,
                                                                    Constant.SettingParameter.IM_PERMINTAAN_BARANG_DENGAN_SATUAN_KECIL,
                                                                    Constant.SettingParameter.IM_ITEM_REQUEST_GET_DESTINATION_LOCATION,
                                                                    Constant.SettingParameter.SA_IS_USED_PRODUCT_LINE);
            List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterExp);
            if (lstParam != null)
            {
                hdnIsAllowUsingAlternateUnit.Value = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.IM_PERMINTAAN_BARANG_DENGAN_SATUAN_KECIL).FirstOrDefault().ParameterValue;
                hdnIsAllowGetFromDestinationLocation.Value = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.IM_ITEM_REQUEST_GET_DESTINATION_LOCATION).FirstOrDefault().ParameterValue;
                hdnIsUsedProductLine.Value = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.SA_IS_USED_PRODUCT_LINE).FirstOrDefault().ParameterValue;
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnOrderID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtOrderNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblLocation, new ControlEntrySetting(true, false));
            SetControlEntrySetting(txtLocationCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtLocationName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(hdnFromLocationItemGroupID, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(hdnRegistrationID, new ControlEntrySetting(true, false));
            SetControlEntrySetting(txtRegistrationNo, new ControlEntrySetting(false, false, true));

            SetControlEntrySetting(txtItemOrderDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtItemOrderTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(lblLocationTo, new ControlEntrySetting(true, false));

            SetControlEntrySetting(lblRegistrationNo, new ControlEntrySetting(true, false));

            SetControlEntrySetting(txtLocationCodeTo, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtLocationNameTo, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(hdnToLocationItemGroupID, new ControlEntrySetting(false, false, false));

            if (hdnIsAllowGetFromDestinationLocation.Value != "1")
            {
                lblFilterItem.Visible = false;
                rbFilterItemLocation.Visible = false;
            }
            else
            {
                lblFilterItem.Visible = true;
                rbFilterItemLocation.Visible = true;
            }

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

        #region Load Entity
        protected string GetFilterExpression()
        {
            //if (!chkIsDisplayToItemOnly.Checked)
            //    filterExpressionItemProduct = string.Format("GCItemType IN ('{0}','{1}','{2}') AND IsDeleted = 0", Constant.ItemGroupMaster.DRUGS, Constant.ItemGroupMaster.SUPPLIES, Constant.ItemGroupMaster.LOGISTIC);
            //else
            //    filterExpressionItemProduct = string.Format("GCItemType IN ('{0}','{1}','{2}') AND IsDeleted = 0 AND ItemID IN (SELECT ItemID FROM ItemBalance WHERE LocationID = {3})", Constant.ItemGroupMaster.DRUGS, Constant.ItemGroupMaster.SUPPLIES, Constant.ItemGroupMaster.LOGISTIC, hdnLocationIDTo.Value);   

            return hdnRecordFilterExpression.Value;
        }

        public override int OnGetRowCount()
        {
            string filterExpression = GetFilterExpression();
            return BusinessLayer.GetvItemRequestHdRowCount(filterExpression);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            vItemRequestHd entity = BusinessLayer.GetvItemRequestHd(filterExpression, PageIndex, "ItemRequestID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            PageIndex = BusinessLayer.GetvItemRequestHdRowIndex(filterExpression, keyValue, "ItemRequestID DESC");
            vItemRequestHd entity = BusinessLayer.GetvItemRequestHd(filterExpression, PageIndex, "ItemRequestID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vItemRequestHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                hdnIsEditable.Value = "0";
                isShowWatermark = true;
                watermarkText = entity.TransactionStatusWatermark;
                SetControlEntrySetting(txtNotes, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(chkIsFilterQtyOnHand, new ControlEntrySetting(true, false, false));
            }
            else
                hdnIsEditable.Value = "1";

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
                rbFilterItemLocation.Enabled = true;
            }
            else
            {
                rbFilterItemLocation.Enabled = false;
            }
            hdnOrderID.Value = entity.ItemRequestID.ToString();
            txtOrderNo.Text = entity.ItemRequestNo;
            txtItemOrderDate.Text = entity.TransactionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtItemOrderTime.Text = entity.TransactionTime;
            hdnGCTransactionStatus.Value = entity.GCTransactionStatus;

            hdnRegistrationID.Value = entity.RegistrationID.ToString();
            if (entity.RegistrationID != 0 && entity.RegistrationID != null)
            {
                txtRegistrationNo.Text = string.Format("{0} | {1} | ({2}) {3}", entity.RegistrationNo, entity.ServiceUnitName, entity.MedicalNo, entity.PatientName);
            }
            else
            {
                txtRegistrationNo.Text = "";
            }


            hdnLocationIDFrom.Value = entity.FromLocationID.ToString();
            txtLocationCode.Text = entity.FromLocationCode;
            txtLocationName.Text = entity.FromLocationName;
            hdnFromLocationItemGroupID.Value = entity.FromLocationItemGroupID.ToString();
            hdnGCLocationGroupFrom.Value = entity.FromGCLocationGroup;

            hdnLocationIDTo.Value = entity.ToLocationID.ToString();
            txtLocationCodeTo.Text = entity.ToLocationCode;
            txtLocationNameTo.Text = entity.ToLocationName;
            hdnToLocationItemGroupID.Value = entity.ToLocationItemGroupID.ToString();
            hdnGCLocationGroupTo.Value = entity.ToGCLocationGroup;

            txtNotes.Text = entity.Remarks;

            hdnProductLineID.Value = entity.ProductLineID.ToString();
            txtProductLineCode.Text = entity.ProductLineCode;
            txtProductLineName.Text = entity.ProductLineName;
            hdnProductLineItemType.Value = entity.GCItemType;

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

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            hdnQtyMaxID.Value = "";
            string filterExpression = "1 = 0";
            if (hdnOrderID.Value != "")
                filterExpression = string.Format("ItemRequestID = {0} AND IsDeleted = 0", hdnOrderID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvItemRequestDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vItemRequestDt> lstEntity = BusinessLayer.GetvItemRequestDtList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ItemName1 ASC");

            if (lstEntity.Count > 0)
            {
                for (int i = 0; i < lstEntity.Count; i++)
                {
                    hdnQtyMaxID.Value += lstEntity[i].ID.ToString() + "|";
                }
            }

            hdnPageCount.Value = pageCount.ToString();
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
        #endregion

        public override void OnAddRecord()
        {
            hdnPageCount.Value = "0";
            hdnIsEditable.Value = "1";
            hdnLocationIDFrom.Value = "";
            hdnLocationIDTo.Value = "";
        }

        protected string IsEditable()
        {
            return hdnIsEditable.Value;
        }

        #region Save & Edit Header
        private void ControlToEntityHd(ItemRequestHd entityHd)
        {
            entityHd.FromLocationID = Convert.ToInt32(hdnLocationIDFrom.Value);
            entityHd.ToLocationID = Convert.ToInt32(hdnLocationIDTo.Value);
            entityHd.TransactionDate = Helper.GetDatePickerValue(txtItemOrderDate.Text);
            entityHd.TransactionTime = txtItemOrderTime.Text;

            if (hdnRegistrationID.Value != null && hdnRegistrationID.Value != "")
            {
                entityHd.RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
            }


            entityHd.Remarks = txtNotes.Text;

            if (hdnIsUsedProductLine.Value == "1")
            {
                entityHd.ProductLineID = Convert.ToInt32(hdnProductLineID.Value);
            }

        }

        public void SaveItemRequestHd(IDbContext ctx, ref int OrderID)
        {
            ItemRequestHdDao entityHdDao = new ItemRequestHdDao(ctx);
            if (hdnOrderID.Value == "0")
            {
                ItemRequestHd entityHd = new ItemRequestHd();
                ControlToEntityHd(entityHd);
                entityHd.ItemRequestNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.ITEM_REQUEST, entityHd.TransactionDate, ctx);
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                OrderID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
            }
            else
            {
                OrderID = Convert.ToInt32(hdnOrderID.Value);
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                int OrderID = 0;
                SaveItemRequestHd(ctx, ref OrderID);
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
                ItemRequestHd entity = BusinessLayer.GetItemRequestHd(Convert.ToInt32(hdnOrderID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entity.Remarks = txtNotes.Text;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateItemRequestHd(entity);
                }
                else
                {
                    result = false;
                    errMessage = "Permintaan barang " + entity.ItemRequestNo + " tidak dapat diubah. Harap refresh halaman ini.";
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

        #region Approved Proposed Void Entity
        protected override bool OnApproveRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ItemRequestHdDao itemHdDao = new ItemRequestHdDao(ctx);
            ItemRequestDtDao itemDtDao = new ItemRequestDtDao(ctx);
            try
            {
                string filterExpressionItemRequestHd = String.Format("ItemRequestID = {0} AND IsDeleted = 0", hdnOrderID.Value);
                ItemRequestHd itemHd = itemHdDao.Get(Convert.ToInt32(hdnOrderID.Value));
                List<ItemRequestDt> lstItemRequestDt = BusinessLayer.GetItemRequestDtList(filterExpressionItemRequestHd, ctx);

                if (itemHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    if (lstItemRequestDt.Count > 0)
                    {
                        foreach (ItemRequestDt itemDt in lstItemRequestDt)
                        {
                            itemDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                            itemDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            itemDtDao.Update(itemDt);
                        }

                        ControlToEntityHd(itemHd);
                        itemHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                        itemHd.ApprovedDate = DateTime.Now;
                        itemHd.ApprovedBy = AppSession.UserLogin.UserID;
                        itemHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        itemHdDao.Update(itemHd);
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "No Permintaan yang anda pilih belum memiliki Item";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Permintaan barang " + itemHd.ItemRequestNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
                ItemRequestHd entity = BusinessLayer.GetItemRequestHd(Convert.ToInt32(hdnOrderID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    string filterExpressionItemRequestHd = String.Format("ItemRequestID = {0} AND IsDeleted = 0", hdnOrderID.Value);
                    List<ItemRequestDt> lstItemRequestDt = BusinessLayer.GetItemRequestDtList(filterExpressionItemRequestHd);
                    foreach (ItemRequestDt itemDt in lstItemRequestDt)
                    {
                        itemDt.GCItemDetailStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                        itemDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        BusinessLayer.UpdateItemRequestDt(itemDt);
                    }

                    ControlToEntityHd(entity);
                    entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateItemRequestHd(entity);
                }
                else
                {
                    result = false;
                    errMessage = "Permintaan barang " + entity.ItemRequestNo + " tidak dapat diubah. Harap refresh halaman ini.";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                result = false;
            }
            return result;
        }

        protected override bool OnVoidRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                ItemRequestHd entity = BusinessLayer.GetItemRequestHd(Convert.ToInt32(hdnOrderID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    string filterExpressionItemRequestHd = String.Format("ItemRequestID = {0} AND IsDeleted = 0", hdnOrderID.Value);
                    List<ItemRequestDt> lstItemRequestDt = BusinessLayer.GetItemRequestDtList(filterExpressionItemRequestHd);
                    foreach (ItemRequestDt itemDt in lstItemRequestDt)
                    {
                        itemDt.GCItemDetailStatus = Constant.TransactionStatus.VOID;
                        itemDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        BusinessLayer.UpdateItemRequestDt(itemDt);
                    }

                    entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateItemRequestHd(entity);
                }
                else
                {
                    result = false;
                    errMessage = "Permintaan barang " + entity.ItemRequestNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
        #endregion

        #endregion

        #region callBack Trigger
        protected void cboItemUnit_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string filterExp = string.Format("ParentID = '{0}' AND (StandardCodeID IN (SELECT GCAlternateUnit FROM ItemAlternateUnit WHERE IsDeleted = 0 AND IsActive = 1 AND ItemID = {1}) OR StandardCodeID = (SELECT GCItemUnit FROM ItemMaster WHERE ItemID = {1}))", Constant.StandardCode.ITEM_UNIT, hdnItemID.Value);
            if (hdnIsAllowUsingAlternateUnit.Value == "1")
            {
                filterExp = string.Format("StandardCodeID = (SELECT GCItemUnit FROM ItemMaster WHERE ItemID = {0})", hdnItemID.Value);
            }
            List<StandardCode> lst = BusinessLayer.GetStandardCodeList(filterExp);
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
            int OrderID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                if (hdnEntryID.Value.ToString() != "")
                {
                    OrderID = Convert.ToInt32(hdnOrderID.Value);
                    if (OnSaveEditRecordEntityDt(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecordEntityDt(ref errMessage, ref OrderID))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "delete")
            {
                OrderID = Convert.ToInt32(hdnOrderID.Value);
                if (OnDeleteEntityDt(ref errMessage, OrderID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpOrderID"] = OrderID.ToString();
        }

        private void ControlToEntity(ItemRequestDt entityDt)
        {
            entityDt.ItemID = Convert.ToInt32(hdnItemID.Value);
            entityDt.Quantity = Convert.ToDecimal(txtQuantity.Text);
            entityDt.GCItemUnit = cboItemUnit.Value.ToString();
            entityDt.GCBaseUnit = hdnGCBaseUnit.Value;
            entityDt.ConversionFactor = Convert.ToDecimal(hdnItemUnitValue.Value);
            entityDt.GCItemRequestType = cboTransactionType.Value.ToString();
            entityDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
            entityDt.Remarks = Convert.ToString(txtNotes2.Text);
        }

        private bool OnSaveAddRecordEntityDt(ref string errMessage, ref int OrderID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ItemRequestHdDao entityHdDao = new ItemRequestHdDao(ctx);
            ItemRequestDtDao entityDtDao = new ItemRequestDtDao(ctx);
            try
            {
                SaveItemRequestHd(ctx, ref OrderID);
                if (entityHdDao.Get(OrderID).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    //ditutup AG 20210707 issue 202107050000001
                    //ItemBalance entityItemBalance = null;

                    //if (hdnItemLocationIs.Value == "from")
                    //{
                    //    entityItemBalance = BusinessLayer.GetItemBalanceList(string.Format("ItemID IN ({0}) AND LocationID = {1} AND IsDeleted = 0", hdnItemID.Value, entityHdDao.Get(OrderID).FromLocationID), ctx).FirstOrDefault();
                    //}
                    //else
                    //{
                    //    entityItemBalance = BusinessLayer.GetItemBalanceList(string.Format("ItemID IN ({0}) AND LocationID = {1} AND IsDeleted = 0", hdnItemID.Value, entityHdDao.Get(OrderID).ToLocationID), ctx).FirstOrDefault();
                    //}

                    ItemRequestDt entityDt = new ItemRequestDt();
                    ControlToEntity(entityDt);
                    //entityDt.GCItemRequestType = entityItemBalance.GCItemRequestType;
                    entityDt.ItemRequestID = OrderID;
                    entityDt.CreatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Insert(entityDt);
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Permintaan barang " + entityHdDao.Get(OrderID).ItemRequestNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
            ItemRequestHdDao entityHdDao = new ItemRequestHdDao(ctx);
            ItemRequestDtDao entityDtDao = new ItemRequestDtDao(ctx);
            try
            {
                ItemRequestDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                if (entityHdDao.Get(entityDt.ItemRequestID).GCTransactionStatus == Constant.TransactionStatus.OPEN && entityDt.IsDeleted == false && entityDt.GCItemDetailStatus == Constant.TransactionStatus.OPEN)
                {
                    //ditutup AG 20210707 issue 202107050000001
                    //ItemBalance entityItemBalance = null;

                    //if (hdnItemLocationIs.Value == "from")
                    //{
                    //    entityItemBalance = BusinessLayer.GetItemBalanceList(string.Format("ItemID IN ({0}) AND LocationID = {1}", hdnItemID.Value, entityHdDao.Get(entityDt.ItemRequestID).FromLocationID), ctx).FirstOrDefault();
                    //}
                    //else
                    //{
                    //    entityItemBalance = BusinessLayer.GetItemBalanceList(string.Format("ItemID IN ({0}) AND LocationID = {1}", hdnItemID.Value, entityHdDao.Get(entityDt.ItemRequestID).ToLocationID), ctx).FirstOrDefault();
                    //}

                    ControlToEntity(entityDt);
                    //entityDt.GCItemRequestType = entityItemBalance.GCItemRequestType;
                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Update(entityDt);
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Permintaan barang " + entityHdDao.Get(entityDt.ItemRequestID).ItemRequestNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
            ItemRequestHdDao entityHdDao = new ItemRequestHdDao(ctx);
            ItemRequestDtDao entityDtDao = new ItemRequestDtDao(ctx);
            try
            {
                ItemRequestDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                if (entityHdDao.Get(entityDt.ItemRequestID).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entityDt.IsDeleted = true;
                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Update(entityDt);
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Permintaan barang " + entityHdDao.Get(entityDt.ItemRequestID).ItemRequestNo + " tidak dapat diubah. Harap refresh halaman ini.";
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

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class PurchaseRequest : BasePageTrx
    {
        protected string filterExpressionItemProduct = "";
        protected string filterExpressionSupplier = "";
        protected string filterExpressionLocation = "";
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.PURCHASE_REQUEST;
        }

        protected override void InitializeDataControl()
        {
            #region SettingParameter
            string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
                            AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IM_KONTROL_CETAK_BUKTI_PERMINTAAN_PEMBELIAN, Constant.SettingParameter.IM0131);
            List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterExp);

            SettingParameterDt setvardt = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IM_IS_PURCHASE_REQUEST_SERVICE_UNIT);
            hdnIsUsedPurchaseOrderType.Value = setvardt.ParameterValue;

            hdnIM0131.Value = lstParam.Where(t => t.ParameterCode == Constant.SettingParameter.IM0131).FirstOrDefault().ParameterValue;

            if (lstParam != null)
            {
                hdnIsControlPrint.Value = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.IM_KONTROL_CETAK_BUKTI_PERMINTAAN_PEMBELIAN).FirstOrDefault().ParameterValue;
            }
            else
            {
                hdnIsControlPrint.Value = "0";
            }

            hdnIsUsedProductLine.Value = AppSession.IsUsedProductLine;
            #endregion

            List<StandardCode> listStandardCodePO = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.PURCHASE_ORDER_TYPE));
            //listStandardCodePO.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboPurchaseOrderType, listStandardCodePO, "StandardCodeName", "StandardCodeID");
            cboPurchaseOrderType.SelectedIndex = 0;

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
            else
            {
                trPurchaseOrderType.Style.Add("display", "none");
            }

            filterExpressionItemProduct = string.Format("GCItemType IN ('{0}','{1}','{2}','{3}') AND IsDeleted = 0",
                                                Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, Constant.ItemType.BARANG_UMUM, Constant.ItemType.BAHAN_MAKANAN);
            filterExpressionSupplier = string.Format("GCBusinessPartnerType = '{0}' AND IsActive = 1 AND IsDeleted = 0 AND IsBlackList = 0", Constant.BusinessObjectType.SUPPLIER);
            filterExpressionLocation = string.Format("{0};{1};{2};", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.TransactionCode.PURCHASE_REQUEST);

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
            Helper.SetControlEntrySetting(txtQuantity, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtItemCode, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(cboItemUnit, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtPrice, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtDiscount, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtDiscount2, new ControlEntrySetting(true, true, true), "mpTrxPopup");

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            List<SettingParameterDt> lstSettingParameter = BusinessLayer.GetSettingParameterDtList(string.Format(
                        "HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
                                    AppSession.UserLogin.HealthcareID, //0
                                    Constant.SettingParameter.IM_SHOW_TOTAL_PRICE_IN_PURCHASE_REQUEST //1
                                ));

            hdnIsShowTotalPrice.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IM_SHOW_TOTAL_PRICE_IN_PURCHASE_REQUEST).ParameterValue;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnRequestID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtOrderNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblLocation, new ControlEntrySetting(true, false));
            SetControlEntrySetting(hdnLocationIDFrom, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtLocationCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtLocationName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(hdnLocationItemGroupID, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(cboPurchaseOrderType, new ControlEntrySetting(true, true, true));

            SetControlEntrySetting(txtItemOrderDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtItemOrderTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsUrgent, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(txtItemRequestNo, new ControlEntrySetting(false, false, false));

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
            return hdnRecordFilterExpression.Value;
        }

        public override int OnGetRowCount()
        {
            string filterExpression = GetFilterExpression();
            return BusinessLayer.GetvPurchaseRequestHdRowCount(filterExpression);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText, ref string retval)
        {
            string filterExpression = GetFilterExpression();
            vPurchaseRequestHd entity = new vPurchaseRequestHd();
            if (!string.IsNullOrEmpty(filterExpression))
            {
                if (!string.IsNullOrEmpty(retval))
                {
                    filterExpression += string.Format(" AND PurchaseRequestID = {0}", retval);
                    entity = BusinessLayer.GetvPurchaseRequestHdList(filterExpression).FirstOrDefault();
                }
                else
                {
                    entity = BusinessLayer.GetvPurchaseRequestHd(filterExpression, PageIndex, "PurchaseRequestID DESC");
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(retval))
                {
                    filterExpression = string.Format("PurchaseRequestID = {0}", retval);
                    entity = BusinessLayer.GetvPurchaseRequestHdList(filterExpression).FirstOrDefault();
                }
                else
                {
                    entity = BusinessLayer.GetvPurchaseRequestHd(filterExpression, PageIndex, "PurchaseRequestID DESC");
                }
            }
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }


        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            vPurchaseRequestHd entity = BusinessLayer.GetvPurchaseRequestHd(filterExpression, PageIndex, "PurchaseRequestID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            PageIndex = BusinessLayer.GetvPurchaseRequestHdRowIndex(filterExpression, keyValue, "PurchaseRequestID DESC");
            vPurchaseRequestHd entity = BusinessLayer.GetvPurchaseRequestHd(filterExpression, PageIndex, "PurchaseRequestID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vPurchaseRequestHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                hdnIsEditable.Value = "0";
                isShowWatermark = true;
                watermarkText = entity.TransactionStatusWatermark;
                SetControlEntrySetting(chkIsUrgent, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(txtNotes, new ControlEntrySetting(false, false, false));
            }
            else
            {
                hdnIsEditable.Value = "1";
            }

            if (hdnIsControlPrint.Value == "1")
            {
                if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN && entity.GCTransactionStatus != Constant.TransactionStatus.VOID)
                    hdnPrintStatus.Value = "true";
                else
                    hdnPrintStatus.Value = "false";
            }
            else
            {
                hdnPrintStatus.Value = "true";
            }
            txtItemRequestNo.Text = entity.ItemRequestNo;
            hdnRequestID.Value = entity.PurchaseRequestID.ToString();
            txtOrderNo.Text = entity.PurchaseRequestNo;
            txtItemOrderDate.Text = entity.TransactionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtItemOrderTime.Text = entity.TransactionTime;
            hdnLocationIDFrom.Value = entity.FromLocationID.ToString();
            txtLocationCode.Text = entity.LocationCode;
            txtLocationName.Text = entity.LocationName;
            hdnLocationItemGroupID.Value = entity.LocationItemGroupID.ToString();
            chkIsUrgent.Checked = entity.IsUrgent;
            txtNotes.Text = entity.Remarks;
            hdnGCTransactionStatus.Value = entity.GCTransactionStatus;

            hdnProductLineID.Value = entity.ProductLineID.ToString();
            txtProductLineCode.Text = entity.ProductLineCode;
            txtProductLineName.Text = entity.ProductLineName;
            hdnProductLineItemType.Value = entity.GCItemType;
            cboPurchaseOrderType.Value = entity.GCPurchaseOrderType;
            divCreatedBy.InnerHtml = entity.CreatedByName;
            divCreatedDate.InnerHtml = entity.CreatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            divLastUpdatedBy.InnerHtml = entity.LastUpdatedByName;
            divApprovedBy.InnerHtml = entity.ApprovedByName;
            if (entity.ApprovedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                divApprovedDate.InnerHtml = "";
            }
            else
            {
                divApprovedDate.InnerHtml = entity.ApprovedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }
            if (entity.LastUpdatedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                divLastUpdatedDate.InnerHtml = "";
            }
            else
            {
                divLastUpdatedDate.InnerHtml = entity.LastUpdatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }

            if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
            {
                cboPurchaseOrderType.Enabled = true;
            }
            else
            {
                cboPurchaseOrderType.Enabled = false;
            }

            BindGridView(1, true, ref PageCount);
            hdnPageCount.Value = PageCount.ToString();
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnRequestID.Value != "")
                filterExpression = string.Format("PurchaseRequestID = {0} AND IsDeleted = 0 AND GCItemDetailStatus != '{1}'", hdnRequestID.Value, Constant.TransactionStatus.VOID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPurchaseRequestDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPurchaseRequestDt1> lstEntity = BusinessLayer.GetvPurchaseRequestDt1List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ItemName1 ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
        #endregion

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vPurchaseRequestDt1 entity = e.Row.DataItem as vPurchaseRequestDt1;

                if (hdnIsShowTotalPrice.Value == "0") // utk tutup kolom SubTotal saat setvar IM0099 = 0
                {
                    // Disc1(%)
                    grdView.Columns[11].HeaderStyle.CssClass = "classDisplayNone";
                    grdView.Columns[11].ItemStyle.CssClass = "classDisplayNone";

                    // Disc2(%)
                    grdView.Columns[12].HeaderStyle.CssClass = "classDisplayNone";
                    grdView.Columns[12].ItemStyle.CssClass = "classDisplayNone";

                    // Sub Total(%)
                    grdView.Columns[13].HeaderStyle.CssClass = "classDisplayNone";
                    grdView.Columns[13].ItemStyle.CssClass = "classDisplayNone";
                }
                else
                {
                    // Disc1(%)
                    grdView.Columns[11].HeaderStyle.CssClass = "classDisplay";
                    grdView.Columns[11].ItemStyle.CssClass = "classDisplay";

                    // Disc2(%)
                    grdView.Columns[12].HeaderStyle.CssClass = "classDisplay";
                    grdView.Columns[12].ItemStyle.CssClass = "classDisplay";

                    // Sub Total(%)
                    grdView.Columns[13].HeaderStyle.CssClass = "classDisplay";
                    grdView.Columns[13].ItemStyle.CssClass = "classDisplay";
                }
            }
        }

        public override void OnAddRecord()
        {
            hdnPageCount.Value = "0";
            hdnIsEditable.Value = "1";
        }

        protected string IsEditable()
        {
            return hdnIsEditable.Value;
        }

        #region Save Edit Header
        private void ControlToEntityHd(PurchaseRequestHd entityHd)
        {
            entityHd.FromLocationID = Convert.ToInt32(hdnLocationIDFrom.Value);
            entityHd.TransactionDate = Helper.GetDatePickerValue(txtItemOrderDate.Text);
            entityHd.TransactionTime = txtItemOrderTime.Text;
            entityHd.IsUrgent = chkIsUrgent.Checked;
            entityHd.Remarks = txtNotes.Text;
            if (hdnIsUsedProductLine.Value == "1")
            {
                entityHd.ProductLineID = Convert.ToInt32(hdnProductLineID.Value);
            }
            if (hdnIsUsedPurchaseOrderType.Value == "1")
            {
                entityHd.GCPurchaseOrderType = Convert.ToString(cboPurchaseOrderType.Value);
            }
        }

        public void SavePurchaseRequestHd(IDbContext ctx, ref int OrderID)
        {
            PurchaseRequestHdDao entityHdDao = new PurchaseRequestHdDao(ctx);
            if (hdnRequestID.Value == "0")
            {
                PurchaseRequestHd entityHd = new PurchaseRequestHd();
                ControlToEntityHd(entityHd);
                entityHd.PurchaseRequestNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.PURCHASE_REQUEST, entityHd.TransactionDate, ctx);
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();

                entityHd.CreatedBy = AppSession.UserLogin.UserID;

                entityHdDao.Insert(entityHd);

                OrderID = BusinessLayer.GetPurchaseRequestHdMaxID(ctx);
            }
            else
            {
                OrderID = Convert.ToInt32(hdnRequestID.Value);
            }
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            if (cboPurchaseOrderType.Value.ToString() != "" && cboPurchaseOrderType.Value != null)
            {
                return true;
            }
            else
            {
                errMessage = Helper.GetErrorMessageText(this, "Mohon isi *Jenis Permintaan* terlebih dahulu.");
                return false;
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                int OrderID = 0;
                SavePurchaseRequestHd(ctx, ref OrderID);
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
                PurchaseRequestHd entity = BusinessLayer.GetPurchaseRequestHd(Convert.ToInt32(hdnRequestID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entity.Remarks = txtNotes.Text;
                    if (hdnIsUsedPurchaseOrderType.Value == "1")
                    {
                        entity.GCPurchaseOrderType = cboPurchaseOrderType.Value.ToString();
                    }
                    if (hdnIsUsedProductLine.Value == "1")
                    {
                        entity.ProductLineID = Convert.ToInt32(hdnProductLineID.Value);
                    }
                    entity.IsUrgent = chkIsUrgent.Checked;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdatePurchaseRequestHd(entity);
                }
                else
                {
                    result = false;
                    errMessage = "Permintaan pembelian " + entity.PurchaseRequestNo + " tidak dapat diubah. Harap refresh halaman ini.";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
            return result;
        }

        protected override bool OnApproveRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseRequestHdDao purchaseHdDao = new PurchaseRequestHdDao(ctx);
            PurchaseRequestDtDao purchaseDtDao = new PurchaseRequestDtDao(ctx);
            try
            {
                PurchaseRequestHd purchaseHd = purchaseHdDao.Get(Convert.ToInt32(hdnRequestID.Value));
                if (purchaseHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    ControlToEntityHd(purchaseHd);
                    purchaseHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                    purchaseHd.ApprovedBy = AppSession.UserLogin.UserID;
                    purchaseHd.ApprovedDate = DateTime.Now;
                    purchaseHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    purchaseHdDao.Update(purchaseHd);

                    string filterExpressionPurchaseRequestHd = String.Format("PurchaseRequestID = {0} AND IsDeleted = 0", hdnRequestID.Value);
                    List<PurchaseRequestDt> lstPurchaseRequestDt = BusinessLayer.GetPurchaseRequestDtList(filterExpressionPurchaseRequestHd, ctx);
                    foreach (PurchaseRequestDt purchaseDt in lstPurchaseRequestDt)
                    {
                        purchaseDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                        purchaseDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        purchaseDtDao.Update(purchaseDt);
                    }
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Permintaan pembelian " + purchaseHd.PurchaseRequestNo + " tidak dapat diubah. Harap refresh halaman ini.";
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

        protected override bool OnApproveRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseRequestHdDao purchaseHdDao = new PurchaseRequestHdDao(ctx);
            PurchaseRequestDtDao purchaseDtDao = new PurchaseRequestDtDao(ctx);
            try
            {
                PurchaseRequestHd purchaseHd = purchaseHdDao.Get(Convert.ToInt32(hdnRequestID.Value));
                retval = hdnRequestID.Value;
                if (purchaseHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    ControlToEntityHd(purchaseHd);
                    purchaseHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                    purchaseHd.ApprovedBy = AppSession.UserLogin.UserID;
                    purchaseHd.ApprovedDate = DateTime.Now;
                    purchaseHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    purchaseHdDao.Update(purchaseHd);

                    string filterExpressionPurchaseRequestHd = String.Format("PurchaseRequestID = {0} AND IsDeleted = 0", hdnRequestID.Value);
                    List<PurchaseRequestDt> lstPurchaseRequestDt = BusinessLayer.GetPurchaseRequestDtList(filterExpressionPurchaseRequestHd, ctx);
                    foreach (PurchaseRequestDt purchaseDt in lstPurchaseRequestDt)
                    {
                        purchaseDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                        purchaseDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        purchaseDtDao.Update(purchaseDt);
                    }
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Permintaan pembelian " + purchaseHd.PurchaseRequestNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseRequestHdDao purchaseHdDao = new PurchaseRequestHdDao(ctx);
            PurchaseRequestDtDao purchaseDtDao = new PurchaseRequestDtDao(ctx);
            try
            {
                PurchaseRequestHd purchaseHd = purchaseHdDao.Get(Convert.ToInt32(hdnRequestID.Value));
                retval = hdnRequestID.Value;
                if (purchaseHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    ControlToEntityHd(purchaseHd);
                    purchaseHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                    purchaseHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    purchaseHdDao.Update(purchaseHd);

                    string filterExpressionPurchaseRequestHd = String.Format("PurchaseRequestID = {0} AND IsDeleted = 0", hdnRequestID.Value);
                    List<PurchaseRequestDt> lstPurchaseRequestDt = BusinessLayer.GetPurchaseRequestDtList(filterExpressionPurchaseRequestHd, ctx);
                    foreach (PurchaseRequestDt purchaseDt in lstPurchaseRequestDt)
                    {
                        purchaseDt.GCItemDetailStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                        purchaseDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        purchaseDtDao.Update(purchaseDt);
                    }
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Permintaan pembelian " + purchaseHd.PurchaseRequestNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseRequestHdDao purchaseHdDao = new PurchaseRequestHdDao(ctx);
            PurchaseRequestDtDao purchaseDtDao = new PurchaseRequestDtDao(ctx);
            try
            {
                PurchaseRequestHd purchaseHd = purchaseHdDao.Get(Convert.ToInt32(hdnRequestID.Value));
                if (purchaseHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    ControlToEntityHd(purchaseHd);
                    purchaseHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                    purchaseHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    purchaseHdDao.Update(purchaseHd);

                    string filterExpressionPurchaseRequestHd = String.Format("PurchaseRequestID = {0} AND IsDeleted = 0", hdnRequestID.Value);
                    List<PurchaseRequestDt> lstPurchaseRequestDt = BusinessLayer.GetPurchaseRequestDtList(filterExpressionPurchaseRequestHd, ctx);
                    foreach (PurchaseRequestDt purchaseDt in lstPurchaseRequestDt)
                    {
                        purchaseDt.GCItemDetailStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                        purchaseDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        purchaseDtDao.Update(purchaseDt);
                    }
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Permintaan pembelian " + purchaseHd.PurchaseRequestNo + " tidak dapat diubah. Harap refresh halaman ini.";
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

        protected override bool OnVoidRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseRequestHdDao purchaseHdDao = new PurchaseRequestHdDao(ctx);
            PurchaseRequestDtDao purchaseDtDao = new PurchaseRequestDtDao(ctx);
            ItemRequestHdDao itemRequestHdDao = new ItemRequestHdDao(ctx);
            ItemRequestDtDao itemRequestDtDao = new ItemRequestDtDao(ctx);
            try
            {
                PurchaseRequestHd purchaseHd = purchaseHdDao.Get(Convert.ToInt32(hdnRequestID.Value));
                if (purchaseHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    ControlToEntityHd(purchaseHd);
                    purchaseHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    purchaseHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    purchaseHdDao.Update(purchaseHd);

                    string filterExpressionPurchaseRequestHd = String.Format("PurchaseRequestID = {0} AND IsDeleted = 0", hdnRequestID.Value);
                    List<PurchaseRequestDt> lstPurchaseRequestDt = BusinessLayer.GetPurchaseRequestDtList(filterExpressionPurchaseRequestHd, ctx);
                    foreach (PurchaseRequestDt purchaseDt in lstPurchaseRequestDt)
                    {
                        purchaseDt.GCItemDetailStatus = Constant.TransactionStatus.VOID;
                        purchaseDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        purchaseDtDao.Update(purchaseDt);

                        if (purchaseHd.ItemRequestID != null && purchaseHd.ItemRequestID != 0)
                        {
                            string filterItemRequestDt = string.Format("ItemRequestID = {0} AND ItemID = {1} AND GCItemDetailStatus IN ('{2}','{3}')",
                                                                    purchaseHd.ItemRequestID, purchaseDt.ItemID, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED);
                            ItemRequestDt irequestDt = BusinessLayer.GetItemRequestDtList(filterItemRequestDt, ctx).FirstOrDefault();
                            if (irequestDt != null)
                            {
                                irequestDt.PurchaseRequestQty = irequestDt.PurchaseRequestQty - purchaseDt.Quantity;
                                if (irequestDt.GCItemRequestType == Constant.ItemRequestType.DISTRIBUTION)
                                {
                                    if (irequestDt.PurchaseRequestQty == 0 && irequestDt.DistributionQty == 0)
                                    {
                                        irequestDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                                    }
                                }
                                else
                                {
                                    if (irequestDt.PurchaseRequestQty == 0 && irequestDt.ConsumptionQty == 0)
                                    {
                                        irequestDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                                    }
                                }
                                irequestDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                itemRequestDtDao.Update(irequestDt);
                            }
                        }
                    }

                    if (purchaseHd.ItemRequestID != null && purchaseHd.ItemRequestID != 0)
                    {
                        string filterItemRequestDtLst = string.Format("ItemRequestID = {0} AND GCItemDetailStatus = '{1}'",
                                                                purchaseHd.ItemRequestID, Constant.TransactionStatus.APPROVED);
                        string filterItemRequestDtLstALL = string.Format("ItemRequestID = {0} AND GCItemDetailStatus != '{1}'",
                                                                purchaseHd.ItemRequestID, Constant.TransactionStatus.VOID);
                        int jumlahItemRequestDtApproved = BusinessLayer.GetItemRequestDtRowCount(filterItemRequestDtLst, ctx);
                        int jumlahItemRequestDtALL = BusinessLayer.GetItemRequestDtRowCount(filterItemRequestDtLstALL, ctx);

                        if (jumlahItemRequestDtApproved == jumlahItemRequestDtALL)
                        {
                            string filterItemRequestHd = string.Format("ItemRequestID = {0}", purchaseHd.ItemRequestID);
                            ItemRequestHd irequestHd = BusinessLayer.GetItemRequestHdList(filterItemRequestHd, ctx).FirstOrDefault();
                            irequestHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
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
                    errMessage = "Permintaan pembelian " + purchaseHd.PurchaseRequestNo + " tidak dapat diubah. Harap refresh halaman ini.";
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

        #region callBack Trigger
        protected void cboItemUnit_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            bool isUsingCatalogSupplier = false;
            String supplierID = hdnSupplierID.Value;
            if (String.IsNullOrEmpty(hdnSupplierID.Value))
            {
                supplierID = "0";
            }


            string filterIP = string.Format(string.Format("ItemID = {0} AND IsDeleted = 0 AND HealthcareID = '{1}'", hdnItemID.Value, AppSession.UserLogin.HealthcareID));
            ItemPlanning ip = BusinessLayer.GetItemPlanningList(filterIP).FirstOrDefault();
            if (ip != null)
            {
                isUsingCatalogSupplier = ip.IsUsingSupplierCatalog;
            }

            if (isUsingCatalogSupplier)
            {
                string filterSC = string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1 AND (StandardCodeID IN (SELECT GCPurchaseUnit FROM SupplierItem WITH(NOLOCK) WHERE IsDeleted = 0 AND ItemID = {1} AND BusinessPartnerID = {2}) OR StandardCodeID IN (SELECT vaiu.GCAlternateUnit FROM vItemAlternateItemUnit vaiu WITH(NOLOCK) WHERE vaiu.ItemID = {1} AND vaiu.IsDeleted = 0 AND vaiu.IsActive = 1))",
                                                    Constant.StandardCode.ITEM_UNIT, hdnItemID.Value, supplierID);
                List<StandardCode> lst = BusinessLayer.GetStandardCodeList(filterSC);
                Methods.SetComboBoxField<StandardCode>(cboItemUnit, lst, "StandardCodeName", "StandardCodeID");
                cboItemUnit.SelectedIndex = -1;
            }
            else
            {
                string filterSC = string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1 AND (StandardCodeID IN (SELECT vaiu.GCAlternateUnit FROM vItemAlternateItemUnit vaiu WITH(NOLOCK) WHERE vaiu.ItemID = {1} AND vaiu.IsDeleted = 0 AND vaiu.IsActive = 1))",
                                                    Constant.StandardCode.ITEM_UNIT, hdnItemID.Value);
                List<StandardCode> lst = BusinessLayer.GetStandardCodeList(filterSC);
                Methods.SetComboBoxField<StandardCode>(cboItemUnit, lst, "StandardCodeName", "StandardCodeID");
                cboItemUnit.SelectedIndex = 0;
            }

            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "edit")
                {
                    result = "edit";
                }
                else if (param[0] == "addItem")
                {
                    result = "addItem";
                }
            }
            cboItemUnit.JSProperties["cpResult"] = result;
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            int pageCount = 1;
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
                    OrderID = Convert.ToInt32(hdnRequestID.Value);
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
                OrderID = Convert.ToInt32(hdnRequestID.Value);
                if (OnDeleteEntityDt(ref errMessage, OrderID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpOrderID"] = OrderID.ToString();
        }

        private void ControlToEntity(PurchaseRequestDt entityDt)
        {
            entityDt.ItemID = Convert.ToInt32(hdnItemID.Value);
            entityDt.Quantity = Convert.ToDecimal(txtQuantity.Text);

            if (hdnQtyEndLocation.Value != "0" && hdnQtyEndLocation.Value != "" && hdnQtyEndLocation.Value != null)
            {
                entityDt.QtyENDLocation = Convert.ToDecimal(hdnQtyEndLocation.Value);
                entityDt.GCItemUnitQtyENDLocation = hdnGCItemUnitQtyEndLocation.Value;
            }
            else
            {
                entityDt.QtyENDLocation = 0;
                entityDt.GCItemUnitQtyENDLocation = null;
            }

            entityDt.GCPurchaseUnit = cboItemUnit.Value.ToString();
            entityDt.GCBaseUnit = hdnGCBaseUnit.Value;
            entityDt.ConversionFactor = Convert.ToDecimal(hdnItemUnitValue.Value);
            entityDt.Remarks = txtNotesDt.Text;
            if (hdnSupplierID.Value != "" && hdnSupplierID.Value != "0") { entityDt.BusinessPartnerID = Convert.ToInt32(hdnSupplierID.Value); }
            else entityDt.BusinessPartnerID = null;
            entityDt.UnitPrice = Convert.ToDecimal(txtPrice.Text);

            if (txtDiscount.Text == "")
            {
                txtDiscount.Text = "0";
            }
            entityDt.DiscountPercentage = Convert.ToDecimal(txtDiscount.Text);
            if (txtDiscount2.Text == "")
            {
                txtDiscount2.Text = "0";
            }
            entityDt.DiscountPercentage2 = Convert.ToDecimal(txtDiscount2.Text);
            entityDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
        }

        private bool OnSaveAddRecordEntityDt(ref string errMessage, ref int OrderID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseRequestHdDao entityHdDao = new PurchaseRequestHdDao(ctx);
            PurchaseRequestDtDao entityDtDao = new PurchaseRequestDtDao(ctx);
            try
            {
                SavePurchaseRequestHd(ctx, ref OrderID);
                if (entityHdDao.Get(OrderID).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    PurchaseRequestDt entityDt = new PurchaseRequestDt();
                    ControlToEntity(entityDt);
                    entityDt.PurchaseRequestID = OrderID;
                    entityDt.CreatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Insert(entityDt);
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Permintaan pembelian " + entityHdDao.Get(OrderID).PurchaseRequestNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
            PurchaseRequestHdDao entityHdDao = new PurchaseRequestHdDao(ctx);
            PurchaseRequestDtDao entityDtDao = new PurchaseRequestDtDao(ctx);
            try
            {
                PurchaseRequestDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                if (entityHdDao.Get(entityDt.PurchaseRequestID).GCTransactionStatus == Constant.TransactionStatus.OPEN && entityDt.IsDeleted == false && entityDt.GCItemDetailStatus == Constant.TransactionStatus.OPEN)
                {
                    ControlToEntity(entityDt);
                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityDtDao.Update(entityDt);
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Permintaan pembelian " + entityHdDao.Get(entityDt.PurchaseRequestID).PurchaseRequestNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
            PurchaseRequestHdDao entityHdDao = new PurchaseRequestHdDao(ctx);
            PurchaseRequestDtDao entityDtDao = new PurchaseRequestDtDao(ctx);
            ItemRequestHdDao itemRequestHdDao = new ItemRequestHdDao(ctx);
            ItemRequestDtDao itemRequestDtDao = new ItemRequestDtDao(ctx);
            try
            {
                PurchaseRequestDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                if (entityHdDao.Get(entityDt.PurchaseRequestID).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entityDt.IsDeleted = true;
                    entityDt.GCItemDetailStatus = Constant.TransactionStatus.VOID;
                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityDtDao.Update(entityDt);

                    if (entityHdDao.Get(entityDt.PurchaseRequestID).ItemRequestID != null && entityHdDao.Get(entityDt.PurchaseRequestID).ItemRequestID != 0)
                    {
                        string filterItemRequestDt = string.Format("ItemRequestID = {0} AND ItemID = {1} AND GCItemDetailStatus IN ('{2}','{3}')",
                                                                entityHdDao.Get(entityDt.PurchaseRequestID).ItemRequestID, entityDt.ItemID, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED);
                        ItemRequestDt irequestDt = BusinessLayer.GetItemRequestDtList(filterItemRequestDt, ctx).FirstOrDefault();
                        if (irequestDt != null)
                        {
                            irequestDt.PurchaseRequestQty = irequestDt.PurchaseRequestQty - entityDt.Quantity;
                            if (irequestDt.GCItemRequestType == Constant.ItemRequestType.DISTRIBUTION)
                            {
                                if (irequestDt.PurchaseRequestQty == 0 && irequestDt.DistributionQty == 0)
                                {
                                    irequestDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                                }
                            }
                            else
                            {
                                if (irequestDt.PurchaseRequestQty == 0 && irequestDt.ConsumptionQty == 0)
                                {
                                    irequestDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                                }
                            }
                            irequestDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            itemRequestDtDao.Update(irequestDt);

                            if (irequestDt.PurchaseRequestQty == 0)
                            {
                                string filterItemRequestDtLst = string.Format("ItemRequestID = {0} AND GCItemDetailStatus = '{1}'",
                                                                        irequestDt.ItemRequestID, Constant.TransactionStatus.APPROVED);
                                string filterItemRequestDtLstALL = string.Format("ItemRequestID = {0} AND GCItemDetailStatus != '{1}'",
                                                                        irequestDt.ItemRequestID, Constant.TransactionStatus.VOID);
                                int jumlahItemRequestDtApproved = BusinessLayer.GetItemRequestDtRowCount(filterItemRequestDtLst, ctx);
                                int jumlahItemRequestDtALL = BusinessLayer.GetItemRequestDtRowCount(filterItemRequestDtLstALL, ctx);

                                if (jumlahItemRequestDtApproved == jumlahItemRequestDtALL)
                                {
                                    string filterItemRequestHd = string.Format("ItemRequestID = {0}", irequestDt.ItemRequestID);
                                    ItemRequestHd irequestHd = BusinessLayer.GetItemRequestHdList(filterItemRequestHd, ctx).FirstOrDefault();
                                    irequestHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                                    irequestHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    itemRequestHdDao.Update(irequestHd);
                                }
                            }
                        }
                    }

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Permintaan pembelian " + entityHdDao.Get(entityDt.PurchaseRequestID).PurchaseRequestNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
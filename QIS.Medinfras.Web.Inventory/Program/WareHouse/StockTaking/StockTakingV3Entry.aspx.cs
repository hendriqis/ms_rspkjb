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
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class StockTakingV3Entry : BasePageTrx
    {
        protected int PageCount = 1;
        protected List<DataTempStok> lstTempStok = new List<DataTempStok>();
        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.STOCK_TAKING_V3;
        }

        public string GetTransactionApprove()
        {
            return Constant.TransactionStatus.APPROVED;
        }

        protected string DateTimeNowDatePicker()
        {
            return DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
        }

        protected override void InitializeDataControl()
        {
            hdnDefaultCycleCountType.Value = BusinessLayer.GetSettingParameter(Constant.SettingParameter.DEFAULT_CYCLE_COUNT_TYPE).ParameterValue;
            hdnInputAll.Value = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IM_STOCK_OPNAME_INPUT_ALL).ParameterValue;
            hdnInputQtyFisik.Value = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IM_STOCK_OPNAME_INPUT_QTY_FISIK).ParameterValue;
            hdnInputQtySelisih.Value = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IM_STOCK_OPNAME_INPUT_QTY_SELISIH).ParameterValue;

            btnStartCalculate.Attributes.Add("enabled", "false");

            int count = BusinessLayer.GetLocationUserRowCount(string.Format("UserID = {0} AND IsDeleted = 0", AppSession.UserLogin.UserID));
            if (count > 0)
                hdnRecordFilterExpression.Value = string.Format("LocationID IN (SELECT LocationID FROM LocationUser WHERE UserID = {0} AND IsDeleted = 0)", AppSession.UserLogin.UserID);
            else
            {
                count = BusinessLayer.GetLocationUserRoleRowCount(string.Format("RoleID IN (SELECT RoleID FROM UserInRole WHERE UserID = {0} AND HealthcareID = '{1}') AND IsDeleted = 0", AppSession.UserLogin.UserID, AppSession.UserLogin.HealthcareID));
                if (count > 0)
                    hdnRecordFilterExpression.Value = string.Format("LocationID IN (SELECT LocationID FROM LocationUserRole WHERE RoleID IN (SELECT RoleID FROM UserInRole WHERE UserID = {0} AND HealthcareID = '{1}') AND IsDeleted = 0)", AppSession.UserLogin.UserID, AppSession.UserLogin.HealthcareID);
                else
                    hdnRecordFilterExpression.Value = "";
            }

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

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            SettingParameterDt setpar = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IM_IS_QTY_STOCK_OPNAME_OTOMATIS_TERISI);
            hdnIsAutoFillQtyEnd.Value = setpar.ParameterValue;
        }

        protected override void SetControlProperties()
        {
            string filterExpression1 = string.Format("ParentID IN ('{0}') AND IsDeleted = 0", Constant.StandardCode.ABC_CLASS);
            List<StandardCode> lstStandardCode1 = BusinessLayer.GetStandardCodeList(filterExpression1);
            lstStandardCode1.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "Semua" });
            Methods.SetComboBoxField<StandardCode>(cboABCClass, lstStandardCode1, "StandardCodeName", "StandardCodeID");

            string filterExpression2 = string.Format("ParentID IN ('{0}') AND IsDeleted = 0", Constant.StandardCode.ITEM_TYPE);
            List<StandardCode> lstStandardCode2 = BusinessLayer.GetStandardCodeList(filterExpression2);
            Methods.SetComboBoxField<StandardCode>(cboItemType, lstStandardCode2.Where(sc => sc.StandardCodeID == Constant.ItemType.OBAT_OBATAN || sc.StandardCodeID == Constant.ItemType.BARANG_MEDIS || sc.StandardCodeID == Constant.ItemType.BARANG_UMUM || sc.StandardCodeID == Constant.ItemType.BAHAN_MAKANAN).ToList(), "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnStockTakingID, new ControlEntrySetting(true, true, false, "0"));
            SetControlEntrySetting(txtStockTakingNo, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtFormDate, new ControlEntrySetting(true, false, true, Constant.DefaultValueEntry.DATE_NOW));
            SetControlEntrySetting(hdnLocationID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtLocationCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtLocationName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboABCClass, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(cboItemType, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(cboBinLocation, new ControlEntrySetting(true, false, true));

            SetControlEntrySetting(lblLocation, new ControlEntrySetting(true, false));

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

        #region Filter Expression Search Dialog
        protected string GetLocationFilterExpression()
        {
            return string.Format("{0};{1};{2};", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.TransactionCode.STOCK_TAKING);
        }
        #endregion

        public override void OnAddRecord()
        {
            hdnPageCount.Value = "0";
            hndIsChangeRbl.Value = "0";
            btnStartCalculate.Attributes.Add("enabled", "false");

            divCreatedBy.InnerHtml = string.Empty;
            divCreatedDate.InnerHtml = string.Empty;
            divProposedBy.InnerHtml = string.Empty;
            divProposedDate.InnerHtml = string.Empty;
            divApprovedBy.InnerHtml = string.Empty;
            divApprovedDate.InnerHtml = string.Empty;
            divVoidBy.InnerHtml = string.Empty;
            divVoidDate.InnerHtml = string.Empty;
            divLastUpdatedBy.InnerHtml = string.Empty;
            divLastUpdatedDate.InnerHtml = string.Empty;
            trProposedBy.Style.Add("display", "none");
            trProposedDate.Style.Add("display", "none");
            trApprovedBy.Style.Add("display", "none");
            trApprovedDate.Style.Add("display", "none");
            trVoidBy.Style.Add("display", "none");
            trVoidDate.Style.Add("display", "none");
        }

        #region Load Entity
        protected string GetFilterExpression()
        {
            string filterExp = hdnRecordFilterExpression.Value;

            if (!String.IsNullOrEmpty(filterExp))
            {
                filterExp += string.Format(" AND BinLocationID IS NOT NULL");
            }
            else
            {
                filterExp = string.Format("BinLocationID IS NOT NULL");
            }
            return filterExp;
        }

        public override int OnGetRowCount()
        {
            string filterExpression = GetFilterExpression();
            return BusinessLayer.GetvStockTakingHdRowCount(filterExpression);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            vStockTakingHd entity = BusinessLayer.GetvStockTakingHd(filterExpression, PageIndex, "StockTakingID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            PageIndex = BusinessLayer.GetvStockTakingHdRowIndex(filterExpression, keyValue, "StockTakingID DESC");
            vStockTakingHd entity = BusinessLayer.GetvStockTakingHd(filterExpression, PageIndex, "StockTakingID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private bool IsEditable = true;
        private void EntityToControl(vStockTakingHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            hdnFilterExpression.Value = "";
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                watermarkText = entity.TransactionStatusWatermark;
                IsEditable = false;
                SetControlEntrySetting(txtSearchView, new ControlEntrySetting(false, false, false));
            }
            else
                IsEditable = true;

            if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
            {
                btnSavePerPagging.Style.Remove("display");
                txtRemarks.Enabled = true;
                cboBinLocation.Enabled = true;
            }
            else
            {
                btnSavePerPagging.Style.Add("display", "none");
                txtRemarks.Enabled = false;
                cboBinLocation.Enabled = false;
            }

            hdnStockTakingID.Value = entity.StockTakingID.ToString();
            txtStockTakingNo.Text = entity.StockTakingNo;
            txtFormDate.Text = entity.FormDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnLocationID.Value = entity.LocationID.ToString();
            txtLocationCode.Text = entity.LocationCode;
            txtLocationName.Text = entity.LocationName;
            txtRemarks.Text = entity.Remarks;
            cboABCClass.Value = entity.GCABCClass;
            cboItemType.Value = entity.GCItemType;

            hdnProductLineID.Value = entity.ProductLineID.ToString();
            txtProductLineCode.Text = entity.ProductLineCode;
            txtProductLineName.Text = entity.ProductLineName;
            hdnProductLineItemType.Value = entity.GCItemType;

            List<StockTakingDt> lst = BusinessLayer.GetStockTakingDtList(String.Format(
                "StockTakingID = {0} AND GCItemDetailStatus != '{1}'", entity.StockTakingID, Constant.TransactionStatus.VOID));
            decimal totalItem = lst.Count();
            decimal totalDifference = lst.Where(a => a.QuantityAdjustment != 0).Count();
            decimal totalAccurate = totalItem - totalDifference;
            decimal accuracy = 0;
            if (totalItem != 0)
            {
                accuracy = totalAccurate / totalItem * 100;
            }

            txtAccuracy.Text = accuracy.ToString("N2") + "%";

            if (entity.BinLocationID != null && entity.BinLocationID != 0)
            {
                string filterExpression2 = string.Format("BinLocationID = {0}", entity.BinLocationID);
                List<BinLocation> lstStandardCode2 = BusinessLayer.GetBinLocationList(filterExpression2);
                Methods.SetComboBoxField<BinLocation>(cboBinLocation, lstStandardCode2, "BinLocationName", "BinLocationID");
                cboBinLocation.SelectedIndex = 0;
            }

            divCreatedBy.InnerHtml = entity.CreatedByName;
            divCreatedDate.InnerHtml = entity.CreatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            divLastUpdatedBy.InnerHtml = entity.LastUpdatedByName;
            if (entity.LastUpdatedDate != null && entity.LastUpdatedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
            {
                divLastUpdatedDate.InnerHtml = entity.LastUpdatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }

            if (entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
            {
                divProposedBy.InnerHtml = entity.ProposedByName;
                if (entity.ProposedDate != null && entity.ProposedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
                {
                    divProposedDate.InnerHtml = entity.ProposedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
                }

                trProposedBy.Style.Remove("display");
                trProposedDate.Style.Remove("display");
            }
            else
            {
                trProposedBy.Style.Add("display", "none");
                trProposedDate.Style.Add("display", "none");
            }

            if (entity.GCTransactionStatus == Constant.TransactionStatus.APPROVED)
            {
                if (entity.ProposedDate != null && entity.ProposedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
                {
                    divProposedBy.InnerHtml = entity.ProposedByName;
                    divProposedDate.InnerHtml = entity.ProposedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
                    trProposedBy.Style.Remove("display");
                    trProposedDate.Style.Remove("display");
                }
                else
                {
                    trProposedBy.Style.Add("display", "none");
                    trProposedDate.Style.Add("display", "none");
                }
                divApprovedBy.InnerHtml = entity.ApprovedByName;
                if (entity.ApprovedDate != null && entity.ApprovedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
                {
                    divApprovedDate.InnerHtml = entity.ApprovedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
                }

                trApprovedBy.Style.Remove("display");
                trApprovedDate.Style.Remove("display");
            }
            else
            {
                trApprovedBy.Style.Add("display", "none");
                trApprovedDate.Style.Add("display", "none");
            }

            if (entity.GCTransactionStatus == Constant.TransactionStatus.VOID)
            {
                divVoidBy.InnerHtml = entity.VoidByName;
                if (entity.VoidDate != null && entity.VoidDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
                {
                    divVoidDate.InnerHtml = entity.VoidDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
                }

                string voidReason = "";

                if (entity.GCVoidReason == Constant.DeleteReason.OTHER)
                {
                    voidReason = entity.VoidReasonWatermark + " ( " + entity.VoidReason + " )";
                }
                else
                {
                    voidReason = entity.VoidReasonWatermark;
                }

                trVoidBy.Style.Remove("display");
                trVoidDate.Style.Remove("display");
            }
            else
            {
                trVoidBy.Style.Add("display", "none");
                trVoidDate.Style.Add("display", "none");
            }

            BindGridView(1, true, ref PageCount);
            hdnPageCount.Value = PageCount.ToString();
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnStockTakingID.Value != "")
                filterExpression = string.Format("StockTakingID = {0} AND GCItemDetailStatus != '{1}'", hdnStockTakingID.Value, Constant.TransactionStatus.VOID);
            if (hdnFilterExpression.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpression.Value);

            if (Convert.ToInt32(cboBinLocation.Value) != 0)
            {
                filterExpression += string.Format(" AND BinLocationID = {0}", Convert.ToInt32(cboBinLocation.Value));
            }

            //if (isCountPageCount)
            //{
            //    int rowCount = BusinessLayer.GetvStockTakingDtRowCount(filterExpression);
            //    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            //}

            lstCheckCountType = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.CHECK_COUNT_TYPE));

            filterExpression += string.Format(" ORDER BY ItemName1 ASC");
            List<vStockTakingDt> lstEntity = BusinessLayer.GetvStockTakingDtList(filterExpression);

            var result = lstEntity.GroupBy(test => test.PageIndex).Select(grp => grp.First()).ToList().OrderBy(x => x.PageIndex);
            Methods.SetRadioButtonListField<vStockTakingDt>(rblPagging, result.ToList(), "PageIndex", "PageIndex");

            if (hndIsChangeRbl.Value == "0")
            {
                rblPagging.SelectedIndex = 0;
                lvwView.DataSource = lstEntity.Where(t => t.PageIndex == 1).ToList();
            }
            else
            {
                lvwView.DataSource = lstEntity.Where(t => t.PageIndex == Convert.ToInt32(hdnPageIndexSelected.Value)).ToList();
            }

            lvwView.DataBind();

            if (lstEntity.Count > 0)
                btnStartCalculate.Attributes.Add("enabled", "false");
            else
                btnStartCalculate.Attributes.Remove("enabled");
        }

        List<StandardCode> lstCheckCountType = null;
        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vStockTakingDt entity = e.Item.DataItem as vStockTakingDt;
                ASPxComboBox cboCheckCountType = e.Item.FindControl("cboCheckCountType") as ASPxComboBox;
                HtmlInputButton btnSave = e.Item.FindControl("btnSave") as HtmlInputButton;
                cboCheckCountType.ClientInstanceName = string.Format("cboCheckCountType{0}", e.Item.DataItemIndex);
                Methods.SetComboBoxField<StandardCode>(cboCheckCountType, lstCheckCountType, "StandardCodeName", "StandardCodeID");
                cboCheckCountType.Value = entity.GCCheckCountType;
                HtmlGenericControl lblExpiredDate = e.Item.FindControl("lblExpiredDate") as HtmlGenericControl;
                HtmlInputText txtAdjustment = e.Item.FindControl("txtAdjustment") as HtmlInputText;
                HtmlInputText txtQuantityEND = e.Item.FindControl("txtQuantityEND") as HtmlInputText;
                HtmlInputText txtRemarksDt = e.Item.FindControl("txtRemarksDt") as HtmlInputText;

                if (hdnIsAutoFillQtyEnd.Value == "1")
                {
                    if (entity.QuantityEND == 0)
                    {
                        txtQuantityEND.Value = entity.QuantityBSO.ToString();
                    }
                    else
                    {
                        txtQuantityEND.Value = entity.QuantityEND.ToString();
                    }
                }
                else
                {
                    txtQuantityEND.Value = entity.QuantityEND.ToString();
                }

                if (!entity.IsControlExpired || entity.QuantityEND > 0) lblExpiredDate.Attributes.Add("class", "lblDisabled");
                if (entity.PurchaseUnit == "")
                    entity.PurchaseUnit = entity.ItemUnit;
                if (entity.PurchaseUnit != entity.ItemUnit)
                {
                    HtmlGenericControl divPurchaseUnit = e.Item.FindControl("divPurchaseUnit") as HtmlGenericControl;
                    HtmlGenericControl divConversionFactor = e.Item.FindControl("divConversionFactor") as HtmlGenericControl;

                    divPurchaseUnit.InnerHtml = entity.PurchaseUnit;
                    divConversionFactor.InnerHtml = string.Format("1 {0} = {1} {2}", entity.PurchaseUnit, entity.ConversionFactor.ToString("N2"), entity.ItemUnit);
                }

                if (!IsEditable)
                {
                    txtQuantityEND.Attributes.Add("readonly", "readonly");
                    txtAdjustment.Attributes.Add("readonly", "readonly");
                    txtRemarksDt.Attributes.Add("readonly", "readonly");
                    cboCheckCountType.ClientEnabled = false;
                    btnSave.Attributes.Add("enabled", "false");
                }
                else
                {
                    if (hdnInputAll.Value == "0")
                    {
                        txtQuantityEND.Attributes.Remove("readonly");
                        txtAdjustment.Attributes.Remove("readonly");
                    }
                    else
                    {
                        if (hdnInputQtyFisik.Value == "1" && hdnInputQtySelisih.Value == "1")
                        {
                            txtQuantityEND.Attributes.Remove("readonly");
                            txtAdjustment.Attributes.Remove("readonly");
                        }
                        else if (hdnInputQtyFisik.Value == "1" && hdnInputQtySelisih.Value == "0")
                        {
                            txtQuantityEND.Attributes.Remove("readonly");
                            txtAdjustment.Attributes.Add("readonly", "readonly");
                        }
                        else if (hdnInputQtyFisik.Value == "0" && hdnInputQtySelisih.Value == "1")
                        {
                            txtQuantityEND.Attributes.Add("readonly", "readonly");
                            txtAdjustment.Attributes.Remove("readonly");
                        }
                        else
                        {
                            txtQuantityEND.Attributes.Add("readonly", "readonly");
                            txtAdjustment.Attributes.Add("readonly", "readonly");
                        }
                    }

                    txtRemarksDt.Attributes.Remove("readonly");

                    if (hdnIsAutoFillQtyEnd.Value == "1")
                    {
                        cboCheckCountType.ClientEnabled = true;
                        btnSave.Attributes.Remove("enabled");
                    }
                    else
                    {
                        cboCheckCountType.ClientEnabled = false;
                        btnSave.Attributes.Add("enabled", "false");
                    }
                }
            }
        }

        #endregion

        #region Save
        private void ControlToEntity(StockTakingHd entity)
        {
            entity.FormDate = Helper.GetDatePickerValue(txtFormDate);
            entity.LocationID = Convert.ToInt32(hdnLocationID.Value);
            entity.Remarks = txtRemarks.Text;
            if (cboABCClass.Value != null && cboABCClass.Value.ToString() != "")
            {
                entity.GCABCClass = cboABCClass.Value.ToString();
            }
            if (cboItemType.Value != null && cboItemType.Value.ToString() != "")
            {
                entity.GCItemType = cboItemType.Value.ToString();
            }
            if (hdnIsUsedProductLine.Value == "1")
            {
                entity.ProductLineID = Convert.ToInt32(hdnProductLineID.Value);
            }
            entity.BinLocationID = Convert.ToInt32(cboBinLocation.Value);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            try
            {
                StockTakingHd entity = new StockTakingHd();
                ControlToEntity(entity);
                entity.StockTakingNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.STOCK_TAKING, entity.FormDate);
                entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertStockTakingHd(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            try
            {
                StockTakingHd entity = BusinessLayer.GetStockTakingHd(Convert.ToInt32(hdnStockTakingID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    ControlToEntity(entity);
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateStockTakingHd(entity);
                    return true;
                }
                else
                {
                    errMessage = "Stock opname tidak dapat diubah. Harap refresh halaman ini.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";

            if (param[0] == "calculate")
            {
                if (FillStockTakingDt(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else
            {
                if (UpdateStockTakingDt(param, ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool FillStockTakingDt(ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            StockTakingHdDao stockTakingHdDao = new StockTakingHdDao(ctx);
            StockTakingDtDao stockTakingDtDao = new StockTakingDtDao(ctx);
            try
            {
                int stockTakingID = Convert.ToInt32(hdnStockTakingID.Value);
                int locationID = Convert.ToInt32(hdnLocationID.Value);
                string abcClass = "0";
                if (cboABCClass.Value != null && cboABCClass.Value.ToString() != "")
                {
                    abcClass = cboABCClass.Value.ToString();
                }
                string itemType = cboItemType.Value.ToString();

                BusinessLayer.FillStockTakingDt1(stockTakingID, locationID, abcClass, itemType, Convert.ToInt32(hdnProductLineID.Value), DateTime.Now, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT), AppSession.UserLogin.UserID, Convert.ToInt32(cboBinLocation.Value.ToString()));

                #region update PageIndex & RowIndex
                string filterExp = string.Format("StockTakingID = {0}", stockTakingID);
                List<StockTakingDt> lstDetail = BusinessLayer.GetStockTakingDtList(filterExp, ctx);
                int RowIndex = 1;
                int PageIndex = 1;
                foreach (StockTakingDt e in lstDetail)
                {
                    e.RowIndex = RowIndex;
                    e.PageIndex = PageIndex;
                    stockTakingDtDao.Update(e);
                    RowIndex = RowIndex + 1;

                    if (RowIndex == 26)
                    {
                        RowIndex = 1;
                        PageIndex = PageIndex + 1;
                    }
                }
                #endregion

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
            finally
            {
                ctx.Close();
            }
            return true;
        }

        private bool UpdateStockTakingDt(string[] param, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            StockTakingHdDao stockTakingHdDao = new StockTakingHdDao(ctx);
            //StockTakingDtDao stockTakingDtDao = new StockTakingDtDao(ctx);
            //StockTakingDtExpiredDao stockTakingDtExpiredDao = new StockTakingDtExpiredDao(ctx);            

            try
            {
                StockTakingHd stockTakingHd = stockTakingHdDao.Get(Convert.ToInt32(hdnStockTakingID.Value));
                if (stockTakingHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    Int32 itemID = Convert.ToInt32(param[1]);
                    Decimal adjustment = Convert.ToDecimal(param[2]);
                    Decimal quantityEND = Convert.ToDecimal(param[3]);
                    String GCCheckCountType = param[4];
                    String remarks = param[5];

                    StockTakingDt entity = BusinessLayer.GetStockTakingDt(Convert.ToInt32(hdnStockTakingID.Value), itemID);
                    entity.QuantityAdjustment = adjustment;
                    entity.QuantityEND = quantityEND;
                    entity.GCCheckCountType = GCCheckCountType;
                    entity.Remarks = remarks;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateStockTakingDt(entity);

                    return true;
                }
                else
                {
                    errMessage = "Stock opname tidak dapat diubah. Harap refresh halaman ini.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private bool UpdateStockTakingDtAll(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            StockTakingHdDao stockTakingHdDao = new StockTakingHdDao(ctx);
            StockTakingDtDao stockTakingDtDao = new StockTakingDtDao(ctx);
            //StockTakingDtExpiredDao stockTakingDtExpiredDao = new StockTakingDtExpiredDao(ctx);            
            try
            {
                string jsonData = hdnTempJsonData.Value;
                if (string.IsNullOrEmpty(jsonData))
                {
                    errMessage = "Silahkan dilakukan pengisian terlebih dahulu dimasing masing item.";
                    result = false;
                }

                StockTakingHd stockTakingHd = stockTakingHdDao.Get(Convert.ToInt32(hdnStockTakingID.Value));
                lstTempStok = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DataTempStok>>(jsonData);
                if (lstTempStok.Count > 0)
                {
                    if (stockTakingHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        foreach (DataTempStok data in lstTempStok)
                        {
                            StockTakingDt entity = BusinessLayer.GetStockTakingDt(Convert.ToInt32(hdnStockTakingID.Value), data.ItemID);
                            entity.QuantityAdjustment = data.QtyAdjustment;
                            entity.QuantityEND = data.QtyEnd;
                            entity.GCCheckCountType = data.CountType;
                            entity.Remarks = data.Remarks;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ////BusinessLayer.UpdateStockTakingDt(entity);
                            stockTakingDtDao.Update(entity);
                        }

                        lstTempStok = null;
                        hdnTempJsonData.Value = string.Empty;
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        ctx.RollBackTransaction();
                        errMessage = "Stock opname tidak dapat diubah. Harap refresh halaman ini.";
                        result = false;
                    }
                }
                else
                {
                    ctx.RollBackTransaction();
                    errMessage = "Silahkan dilakukan pengisian terlebih dahulu dimasing masing item.";
                    result = false;
                }

            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                ctx.RollBackTransaction();
                result = false;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        protected override bool OnApproveRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            StockTakingHdDao stockTakingHdDao = new StockTakingHdDao(ctx);
            StockTakingDtDao stockTakingDtDao = new StockTakingDtDao(ctx);
            try
            {
                StockTakingHd stockTakingHd = stockTakingHdDao.Get(Convert.ToInt32(hdnStockTakingID.Value));
                if (stockTakingHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    string filterExpression = String.Format("StockTakingID = {0} AND GCItemDetailStatus != '{1}' AND QuantityAdjustment != 0", hdnStockTakingID.Value, Constant.TransactionStatus.VOID);
                    List<StockTakingDt> lstStockTakingDt = BusinessLayer.GetStockTakingDtList(filterExpression, ctx);

                    int countOutOfStockMinus = lstStockTakingDt.Where(a => a.QuantityEND < 0).Count();
                    if (countOutOfStockMinus == 0)
                    {
                        foreach (StockTakingDt stockTakingDt in lstStockTakingDt)
                        {
                            stockTakingDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                            stockTakingDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            stockTakingDtDao.Update(stockTakingDt);
                        }
                        stockTakingHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                        stockTakingHd.ApprovedBy = AppSession.UserLogin.UserID;
                        stockTakingHd.ApprovedDate = DateTime.Now;
                        stockTakingHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        stockTakingHdDao.Update(stockTakingHd);

                        decimal totalItem = lstStockTakingDt.Count();
                        decimal totalDifference = lstStockTakingDt.Where(a => a.QuantityAdjustment != 0).Count();
                        decimal totalAccurate = totalItem - totalDifference;
                        decimal accuracy = 0;
                        if (totalItem != 0)
                        {
                            accuracy = totalAccurate / totalItem * 100;
                        }

                        txtAccuracy.Text = accuracy.ToString(Constant.FormatString.NUMERIC_2) + "%";

                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Stock Opname dgn nomor <b>" + stockTakingHd.StockTakingNo + "</b> tidak dapat di-approve karena ada " + countOutOfStockMinus.ToString() + " detail item stok yang minus.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Stock opname tidak dapat diubah. Harap refresh halaman ini.";
                    ctx.RollBackTransaction();
                }
            }
            catch (Exception ex)
            {
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

        protected override bool OnProposeRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            StockTakingHdDao stockTakingHdDao = new StockTakingHdDao(ctx);
            StockTakingDtDao stockTakingDtDao = new StockTakingDtDao(ctx);
            try
            {
                StockTakingHd stockTakingHd = stockTakingHdDao.Get(Convert.ToInt32(hdnStockTakingID.Value));
                if (stockTakingHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    stockTakingHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                    stockTakingHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    stockTakingHdDao.Update(stockTakingHd);

                    string filterExpression = String.Format("StockTakingID = {0} AND GCItemDetailStatus != '{1}'", hdnStockTakingID.Value, Constant.TransactionStatus.VOID);
                    List<StockTakingDt> lstStockTakingDt = BusinessLayer.GetStockTakingDtList(filterExpression, ctx);
                    if (lstStockTakingDt.Count > 0)
                    {
                        foreach (StockTakingDt stockTakingDt in lstStockTakingDt)
                        {
                            //if (stockTakingDt.QuantityEND == 0)
                            //{
                            //    stockTakingDt.QuantityAdjustment = stockTakingDt.QuantityBSO * -1;
                            //    stockTakingDt.QuantityEND = 0;
                            //}
                            //stockTakingDt.GCCheckCountType = hdnDefaultCycleCountType.Value.ToString();
                            //stockTakingDt.IsProposed = true;
                            stockTakingDt.GCItemDetailStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                            stockTakingDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            stockTakingDtDao.Update(stockTakingDt);
                        }

                        decimal totalItem = lstStockTakingDt.Count();
                        decimal totalDifference = lstStockTakingDt.Where(a => a.QuantityAdjustment != 0 || a.QuantityAdjustment < 0).Count();
                        decimal totalAccurate = totalItem - totalDifference;
                        decimal accuracy = 0;
                        if (totalItem != 0)
                        {
                            accuracy = totalAccurate / totalItem * 100;
                        }

                        txtAccuracy.Text = accuracy.ToString("N2") + "%";

                        ctx.CommitTransaction();
                    }
                    else
                    {
                        errMessage = "Harap hitung stok terlebih dahulu sebelum propose / approve";
                        result = false;
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    errMessage = "Stock opname tidak dapat diubah. Harap refresh halaman ini.";
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
            try
            {
                StockTakingHd entity = BusinessLayer.GetStockTakingHd(Convert.ToInt32(hdnStockTakingID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    entity.VoidBy = AppSession.UserLogin.UserID;
                    entity.VoidDate = DateTime.Now;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateStockTakingHd(entity);
                    return true;
                }
                else
                {
                    errMessage = "Stock opname tidak dapat diubah. Harap refresh halaman ini.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
        #endregion

        #region Callback
        protected void cboBinLocation_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string filterExpression2 = string.Format("LocationID = '{0}' AND IsDeleted = 0", hdnLocationID.Value);
            List<BinLocation> lstStandardCode2 = BusinessLayer.GetBinLocationList(filterExpression2);
            Methods.SetComboBoxField<BinLocation>(cboBinLocation, lstStandardCode2, "BinLocationName", "BinLocationID");
            cboBinLocation.SelectedIndex = 0;
        }

        protected void cbpPaggingNew_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            BindGridView(1, true, ref pageCount);
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
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
                else if (param.Length > 1)
                {
                    if (param[1] == "calculate")
                    {
                        BindGridView(1, true, ref pageCount);
                        result = "calculate|" + pageCount;
                    }
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

        protected void cbpSetDataProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "setData")
                {
                    // var param = 'setData|' + itemID + '|' + adjustment + '|' + quantityEND + '|' + checkCountType + '|' + remarks;
                    int ItemID = Convert.ToInt32(param[1]);
                    Decimal Adjustment = Convert.ToDecimal(param[2]);
                    Decimal QuantiyEnd = Convert.ToDecimal(param[3]);
                    string CheckCountType = param[4];
                    string Remarks = param[5];

                    string data = hdnTempJsonData.Value;

                    if (string.IsNullOrEmpty(data))
                    {
                        DataTempStok oData = new DataTempStok();
                        oData.ItemID = ItemID;
                        oData.QtyAdjustment = Adjustment;
                        oData.QtyEnd = QuantiyEnd;
                        oData.CountType = CheckCountType;
                        oData.Remarks = Remarks;
                        lstTempStok.Add(oData);

                        string toJson = Newtonsoft.Json.JsonConvert.SerializeObject(lstTempStok);
                        hdnTempJsonData.Value = toJson;
                    }
                    else
                    {
                        lstTempStok = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DataTempStok>>(data);
                        DataTempStok oData = lstTempStok.Where(p => p.ItemID == ItemID).FirstOrDefault();
                        if (oData != null)
                        {
                            lstTempStok.Remove(oData);

                            oData.ItemID = ItemID;
                            oData.QtyAdjustment = Adjustment;
                            oData.QtyEnd = QuantiyEnd;
                            oData.CountType = CheckCountType;
                            oData.Remarks = Remarks;

                            lstTempStok.Add(oData);
                            string toJson = Newtonsoft.Json.JsonConvert.SerializeObject(lstTempStok);
                            hdnTempJsonData.Value = toJson;
                        }
                        else
                        {
                            DataTempStok newData = new DataTempStok();
                            newData.ItemID = ItemID;
                            newData.QtyAdjustment = Adjustment;
                            newData.QtyEnd = QuantiyEnd;
                            newData.CountType = CheckCountType;
                            newData.Remarks = Remarks;

                            lstTempStok.Add(newData);
                            string toJson = Newtonsoft.Json.JsonConvert.SerializeObject(lstTempStok);
                            hdnTempJsonData.Value = toJson;
                        }

                    }
                    result = "setData";
                }
                else if (param[0] == "save")
                {
                    result = "save|";
                    if (UpdateStockTakingDtAll(ref errMessage))
                    {
                        result += "success";
                    }
                    else
                    {
                        result += "fail|" + errMessage;
                    }
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion
    }
    public class DataTempStok
    {

        public int ItemID { get; set; }
        public Decimal QtyEnd { get; set; }
        public Decimal QtyAdjustment { get; set; }
        public string CountType { get; set; }
        public string Remarks { get; set; }
    }
}

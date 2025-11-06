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
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class CSSDHandoverConfirmationDetailList : BasePageTrx
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.CSSD_HANDOVER_CONFIRMATION;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            #region MDServiceRequest

            hdnServiceRequestID.Value = Page.Request.QueryString["id"];
            vMDServiceRequestHd entity = BusinessLayer.GetvMDServiceRequestHdList(string.Format("RequestID = {0}", hdnServiceRequestID.Value)).FirstOrDefault();
            hdnServiceRequestNo.Value = entity.RequestNo;
            hdnServiceRequestStatus.Value = entity.GCServiceStatus;

            EntityToControl(entity);

            #endregion

            #region StockConsumption
            
            string filterCons = String.Format("TransactionCode = '{0}' AND ServiceRequestID = {1} AND GCTransactionStatus != '{2}'", Constant.TransactionCode.SERVICE_CONSUMPTION, hdnServiceRequestID.Value, Constant.TransactionStatus.VOID);
            List<ItemTransactionHd> lstConsumptionHd = BusinessLayer.GetItemTransactionHdList(filterCons);

            if (lstConsumptionHd.Count > 0)
            {
                hdnIsAllowCreateConsumption.Value = "0";
            }
            else
            {
                hdnIsAllowCreateConsumption.Value = "1";
            }

            if (hdnIsAllowCreateConsumption.Value == "1")
            {
                hdnIsEditable.Value = "1";
            }

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

            hdnLocationConsumptionID.Value = entity.ToLocationID.ToString();
            hdnLocationConsumptionUnitID.Value = entity.FromLocationID.ToString();

            Helper.SetControlEntrySetting(txtQuantity, new ControlEntrySetting(true, true, true), "mpTrx");
            Helper.SetControlEntrySetting(txtItemCode, new ControlEntrySetting(true, true, true), "mpTrx");
            Helper.SetControlEntrySetting(cboItemUnit, new ControlEntrySetting(true, true, true), "mpTrx");
            Helper.SetControlEntrySetting(txtConversion, new ControlEntrySetting(false, false, true), "mpTrx");

            BindGridViewConsumption(1, true, ref PageCount);

            #endregion
        }

        #region StockConsumption

        protected override void SetControlProperties()
        {
            List<StandardCode> listStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.CONSUMPTION_TYPE, Constant.StandardCode.HEALTHCARE_UNIT));
            Methods.SetComboBoxField<StandardCode>(cboGCConsumptionType, listStandardCode.Where(p => p.ParentID == Constant.StandardCode.CONSUMPTION_TYPE).ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboHealthcareUnit, listStandardCode.Where(p => p.ParentID == Constant.StandardCode.HEALTHCARE_UNIT).ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");

            Location oLocation = BusinessLayer.GetLocation(Convert.ToInt32(hdnLocationConsumptionID.Value));
            txtLocationConsumptionCode.Text = oLocation.LocationCode;
            txtLocationConsumptionName.Text = oLocation.LocationName;
            hdnLocationConsumptionItemGroupID.Value = oLocation.ItemGroupID.ToString();
            hdnLocationConsumptionGCLocationGroup.Value = oLocation.GCLocationGroup;

            Location oUnit = BusinessLayer.GetLocation(Convert.ToInt32(hdnLocationConsumptionUnitID.Value));
            cboHealthcareUnit.Value = oUnit.GCHealthcareUnit;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnConsumptionID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtConsumptionNo, new ControlEntrySetting(false, false, false, ""));
            SetControlEntrySetting(txtConsumptionDate, new ControlEntrySetting(true, false, false, Constant.DefaultValueEntry.DATE_NOW));
            SetControlEntrySetting(txtConsumptionTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(txtLocationConsumptionCode, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtLocationConsumptionName, new ControlEntrySetting(false, false, true));

            SetControlEntrySetting(cboGCConsumptionType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboHealthcareUnit, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false, ""));
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            if (hdnIsAllowCreateConsumption.Value == "0")
            {
                IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
            }
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            if (hdnIsAllowCreateConsumption.Value == "0")
            {
                IsAllowAdd = false;
                IsAllowSave = false;
            }
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
            string filterExpression = String.Format("TransactionCode = '{0}' AND ServiceRequestID = {1}", Constant.TransactionCode.SERVICE_CONSUMPTION, hdnServiceRequestID.Value);
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
            EntityToControlConsumption(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            PageIndex = BusinessLayer.GetvItemTransactionHdRowIndex(filterExpression, keyValue, "TransactionID DESC");
            vItemTransactionHd entity = BusinessLayer.GetvItemTransactionHd(filterExpression, PageIndex, "TransactionID DESC");
            EntityToControlConsumption(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControlConsumption(vItemTransactionHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            //if (entity.TransactionID != null && entity.TransactionID != 0)
            //{
            //    hdnIsAllowCreateConsumption.Value = "0";
            //}

            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                watermarkText = entity.TransactionStatusWatermark;
                hdnIsEditable.Value = "0";
            }
            else
            {
                hdnIsEditable.Value = "1";
                hdnIsAllowCreateConsumption.Value = "1";
            }

            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN && entity.GCTransactionStatus != Constant.TransactionStatus.VOID)
            {
                hdnPrintStatus.Value = "true";
            }
            else
            {
                hdnPrintStatus.Value = "false";
            }

            hdnConsumptionID.Value = entity.TransactionID.ToString();
            txtConsumptionNo.Text = entity.TransactionNo;
            txtConsumptionDate.Text = entity.TransactionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtConsumptionTime.Text = entity.TransactionTime;

            Location entityLoc = BusinessLayer.GetLocation(entity.FromLocationID);
            hdnGCLocationGroupConsumption.Value = entityLoc.GCLocationGroup;
            hdnLocationConsumptionID.Value = entity.FromLocationID.ToString();
            txtLocationConsumptionCode.Text = entity.FromLocationCode;
            txtLocationConsumptionName.Text = entity.FromLocationName;

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

            BindGridViewConsumption(1, true, ref PageCount);
            hdnPageCount.Value = PageCount.ToString();
        }

        private void BindGridViewConsumption(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnConsumptionID.Value != "")
            {
                filterExpression = string.Format("TransactionID = {0} AND GCItemDetailStatus != '{1}'", hdnConsumptionID.Value, Constant.TransactionStatus.VOID);
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvItemTransactionDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MATRIX);
            }

            List<vItemTransactionDt> lstEntity = BusinessLayer.GetvItemTransactionDtList(filterExpression, Constant.GridViewPageSize.GRID_MATRIX, pageIndex, "ItemName1 ASC");
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
                entityHd.ServiceRequestID = Convert.ToInt32(hdnServiceRequestID.Value);
                entityHd.TransactionDate = Helper.GetDatePickerValue(Request.Form[txtConsumptionDate.UniqueID]);
                entityHd.TransactionTime = txtConsumptionTime.Text;
                entityHd.FromLocationID = Convert.ToInt32(hdnLocationConsumptionID.Value);
                entityHd.ToLocationID = null;
                entityHd.GCConsumptionType = cboGCConsumptionType.Value.ToString();
                entityHd.GCHealthcareUnit = cboHealthcareUnit.Value.ToString();
                entityHd.Remarks = "Pemakaian CSSD dari nomor " + hdnServiceRequestNo.Value + "|" + txtRemarks.Text;
                entityHd.TransactionCode = Constant.TransactionCode.SERVICE_CONSUMPTION;
                entityHd.TransactionNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.TransactionDate, ctx);
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                entityHdDao.Insert(entityHd);
                ConsumptionID = BusinessLayer.GetItemTransactionHdMaxID(ctx);
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
                string filterCons = String.Format("TransactionCode = '{0}' AND ServiceRequestID = {1} AND GCTransactionStatus != '{2}'", Constant.TransactionCode.SERVICE_CONSUMPTION, hdnServiceRequestID.Value, Constant.TransactionStatus.VOID);
                List<ItemTransactionHd> lstConsumptionHd = BusinessLayer.GetItemTransactionHdList(filterCons);

                if (lstConsumptionHd.Count > 0)
                {
                    result = false;
                    errMessage = "Tidak bisa menyimpan pemakaian CSSD karena sudah ada proses pemakaian di nomor <b>" + lstConsumptionHd.FirstOrDefault().TransactionNo + "</b>";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }
                else
                {
                    int OrderID = 0;
                    SaveItemConsumptionHd(ctx, ref OrderID);
                    retval = OrderID.ToString();
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

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            try
            {
                ItemTransactionHd entityHd = BusinessLayer.GetItemTransactionHd(Convert.ToInt32(hdnConsumptionID.Value));
                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entityHd.TransactionDate = Helper.GetDatePickerValue(Request.Form[txtConsumptionDate.UniqueID]);
                    entityHd.FromLocationID = Convert.ToInt32(hdnLocationConsumptionID.Value);
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
            ItemDistributionHdDao distHdDao = new ItemDistributionHdDao(ctx);
            ItemDistributionDtDao distDtDao = new ItemDistributionDtDao(ctx);
            MDServiceRequestHdDao reqHdDao = new MDServiceRequestHdDao(ctx);
            MDServiceRequestDtDao reqDtDao = new MDServiceRequestDtDao(ctx);

            try
            {
                ItemTransactionHd itemTransactionHd = itemTransactionHdDao.Get(Convert.ToInt32(hdnConsumptionID.Value));

                MDServiceRequestHd reqHd = reqHdDao.Get(Convert.ToInt32(itemTransactionHd.ServiceRequestID));

                string filterlstReqDt = string.Format("RequestID = {0} AND IsDeleted = 0", reqHd.RequestID);
                List<MDServiceRequestDt> lstReqDt = BusinessLayer.GetMDServiceRequestDtList(filterlstReqDt);

                if (itemTransactionHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    string filterExpressionPurchaseOrderHd = String.Format("TransactionID = {0} AND GCItemDetailStatus != '{1}'", hdnConsumptionID.Value, Constant.TransactionStatus.VOID);
                    List<ItemTransactionDt> lstItemTransactionDt = BusinessLayer.GetItemTransactionDtList(filterExpressionPurchaseOrderHd, ctx);
                    if (lstItemTransactionDt.Count > 0)
                    {
                        #region Consumption

                        foreach (ItemTransactionDt itemTransactionDt in lstItemTransactionDt)
                        {
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            itemTransactionDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                            itemTransactionDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            itemTransactionDtDao.Update(itemTransactionDt);
                        }

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        itemTransactionHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                        itemTransactionHd.GCConsumptionType = cboGCConsumptionType.Value.ToString();
                        itemTransactionHd.GCHealthcareUnit = cboHealthcareUnit.Value.ToString();
                        itemTransactionHd.Remarks = txtRemarks.Text;
                        itemTransactionHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        itemTransactionHdDao.Update(itemTransactionHd);
                        
                        #endregion

                        //#region Distribution

                        //string filterDistHd = string.Format("ServiceRequestID = '{0}' AND GCDistributionStatus = '{1}'", hdnServiceRequestID.Value, Constant.DistributionStatus.ON_DELIVERY);
                        //List<ItemDistributionHd> distHdLst = BusinessLayer.GetItemDistributionHdList(filterDistHd, ctx);
                        //if (distHdLst.Count > 0)
                        //{
                        //    foreach (ItemDistributionHd distHd in distHdLst)
                        //    {
                        //        string filterDistDt = string.Format("DistributionID = {0} AND IsDeleted = 0 AND GCItemDetailStatus = '{1}'", distHd.DistributionID, Constant.DistributionStatus.ON_DELIVERY);
                        //        List<ItemDistributionDt> distDtLst = BusinessLayer.GetItemDistributionDtList(filterDistDt, ctx);
                        //        foreach (ItemDistributionDt distDt in distDtLst)
                        //        {
                        //            ctx.CommandType = CommandType.Text;
                        //            ctx.Command.Parameters.Clear();
                        //            distDt.GCItemDetailStatus = Constant.DistributionStatus.RECEIVED;
                        //            distDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        //            distDtDao.Update(distDt);
                        //        }

                        //        ctx.CommandType = CommandType.Text;
                        //        ctx.Command.Parameters.Clear();
                        //        distHd.GCDistributionStatus = Constant.DistributionStatus.RECEIVED;
                        //        distHd.ReceivedBy = BusinessLayer.GetUser(AppSession.UserLogin.UserID).UserName;
                        //        distHd.ReceivedDate = DateTime.Now;
                        //        distHd.ReceivedTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                        //        distHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        //        distHdDao.Update(distHd);
                        //    }
                        //}

                        //#endregion

                        #region MDServiceRequest

                        foreach (MDServiceRequestDt reqDt in lstReqDt)
                        {
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            reqDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            reqDtDao.Update(reqDt);
                        }

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        reqHd.GCServiceStatus = Constant.ServiceStatus.PROCESSED;
                        //reqHd.ReceivedBy = AppSession.UserLogin.UserID;
                        //reqHd.ReceivedDate = DateTime.Now;
                        reqHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        reqHdDao.Update(reqHd);

                        #endregion

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
                    entity.GCConsumptionType = cboGCConsumptionType.Value.ToString();
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

                ItemTransactionHd entity = BusinessLayer.GetItemTransactionHd(Convert.ToInt32(hdnConsumptionID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entity.GCConsumptionType = cboGCConsumptionType.Value.ToString();
                    entity.GCHealthcareUnit = cboHealthcareUnit.Value.ToString();
                    entity.Remarks = txtRemarks.Text;
                    entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    itemTransactionHdDao.Update(entity);

                    string filterExpressionItemTransactionHd = String.Format("TransactionID = {0} AND GCItemDetailStatus != '{1}'",
                                                                entity.TransactionID, Constant.TransactionStatus.VOID);
                    List<ItemTransactionDt> lstItemTransactionDt = BusinessLayer.GetItemTransactionDtList(filterExpressionItemTransactionHd);
                    foreach (ItemTransactionDt itemDt in lstItemTransactionDt)
                    {
                        itemDt.GCItemDetailStatus = Constant.DistributionStatus.VOID;
                        itemDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        itemTransactionDtDao.Update(itemDt);
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
            int consumptionID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                if (hdnEntryID.Value.ToString() != "")
                {
                    consumptionID = Convert.ToInt32(hdnConsumptionID.Value);
                    if (OnSaveEditRecordEntityDt(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecordEntityDt(ref errMessage, ref consumptionID))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "delete")
            {
                consumptionID = Convert.ToInt32(hdnConsumptionID.Value);
                if (OnDeleteEntityDt(ref errMessage, consumptionID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpConsumptionID"] = consumptionID.ToString();
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
                string filterCons = String.Format("TransactionCode = '{0}' AND ServiceRequestID = {1} AND GCTransactionStatus != '{2}' AND TransactionID != {3}", Constant.TransactionCode.SERVICE_CONSUMPTION, hdnServiceRequestID.Value, Constant.TransactionStatus.VOID, hdnConsumptionID.Value);
                List<ItemTransactionHd> lstConsumptionHd = BusinessLayer.GetItemTransactionHdList(filterCons);

                if (lstConsumptionHd.Count > 0)
                {
                    result = false;
                    errMessage = "Tidak bisa menyimpan pemakaian CSSD karena sudah ada proses pemakaian di nomor <b>" + lstConsumptionHd.FirstOrDefault().TransactionNo + "</b>";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }
                else
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
            List<StandardCode> lst = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND (StandardCodeID IN (SELECT GCAlternateUnit FROM ItemAlternateUnit WHERE ItemID = {1}) OR StandardCodeID = (SELECT GCItemUnit FROM ItemMaster WHERE ItemID = {1}))", Constant.StandardCode.ITEM_UNIT, hdnItemID.Value));
            Methods.SetComboBoxField<StandardCode>(cboItemUnit, lst, "StandardCodeName", "StandardCodeID");
            cboItemUnit.SelectedIndex = -1;
        }

        protected void cbpViewConsumption_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewConsumption(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewConsumption(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        #endregion

        #region MDServiceRequest

        private void EntityToControl(vMDServiceRequestHd entity)
        {
            hdnServiceRequestID.Value = entity.RequestID.ToString();
            txtRequestNo.Text = entity.RequestNo;
            txtRequestDate.Text = entity.RequestDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtRequestTime.Text = entity.RequestTime;

            hdnLocationIDFrom.Value = entity.FromLocationID.ToString();
            txtLocationCode.Text = entity.FromLocationCode;
            txtLocationName.Text = entity.FromLocationName;
            hdnLocationIDTo.Value = entity.ToLocationID.ToString();
            txtLocationCodeTo.Text = entity.ToLocationCode;
            txtLocationNameTo.Text = entity.ToLocationName;

            txtServiceType.Text = entity.ServiceType;
            hdnPackageID.Value = entity.PackageID.ToString();
            txtPackageCode.Text = entity.PackageCode;
            txtPackageName.Text = entity.PackageName;

            txtSentBy.Text = entity.SentByName;
            txtSentDate.Text = entity.SentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtSentTime.Text = entity.SentDate.ToString(Constant.FormatString.TIME_FORMAT);

            txtReceivedBy.Text = entity.ReceivedByName;
            txtReceivedDate.Text = entity.ReceivedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtReceivedTime.Text = entity.ReceivedDate.ToString(Constant.FormatString.TIME_FORMAT);

            txtNotes.Text = entity.Remarks;

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";

            if (hdnServiceRequestID.Value != "")
            {
                filterExpression = string.Format("RequestID = {0} AND IsDeleted = 0", hdnServiceRequestID.Value);
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvMDServiceRequestDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_FIVE);
            }

            List<vMDServiceRequestDt> lstEntity = BusinessLayer.GetvMDServiceRequestDtList(filterExpression, Constant.GridViewPageSize.GRID_FIVE, pageIndex, "IsConsumption, ItemName1 ASC");
            grdViewRequest.DataSource = lstEntity;
            grdViewRequest.DataBind();
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

    }
}
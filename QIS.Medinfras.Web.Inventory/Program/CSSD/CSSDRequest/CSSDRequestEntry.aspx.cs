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
    public partial class CSSDRequestEntry : BasePageTrx
    {
        protected string filterExpressionItemProduct = "";
        protected string filterExpressionLocation = "";
        protected string filterExpressionLocationTo = "";
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.CSSD_REQUEST;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
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

        protected string IsFromPackage()
        {
            if (hdnPackageID.Value != "0" && hdnPackageID.Value != "")
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }

        protected override void InitializeDataControl()
        {
            GetSettingParameter();
            
            filterExpressionItemProduct = string.Format("GCItemType IN ('{0}','{1}','{2}') AND IsDeleted = 0", Constant.ItemGroupMaster.DRUGS, Constant.ItemGroupMaster.SUPPLIES, Constant.ItemGroupMaster.LOGISTIC);
            filterExpressionLocation = string.Format("{0};{1};{2};", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.TransactionCode.SERVICE_REQUEST);

            SettingParameterDt locTo = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IM_DEFAULT_LOCATION_CSSD);
            filterExpressionLocationTo = string.Format("LocationID = {0}", locTo.ParameterValue);

            int count = BusinessLayer.GetLocationUserRowCount(string.Format("UserID = {0} AND IsDeleted = 0", AppSession.UserLogin.UserID));
            if (count > 0)
            {
                hdnRecordFilterExpression.Value = string.Format("FromLocationID IN (SELECT LocationID FROM LocationUser WHERE UserID = {0} AND IsDeleted = 0)", AppSession.UserLogin.UserID);
            }
            else
            {
                count = BusinessLayer.GetLocationUserRoleRowCount(string.Format("RoleID IN (SELECT RoleID FROM UserInRole WHERE UserID = {0} AND HealthcareID = '{1}') AND IsDeleted = 0", AppSession.UserLogin.UserID, AppSession.UserLogin.HealthcareID));
                if (count > 0)
                    hdnRecordFilterExpression.Value = string.Format("FromLocationID IN (SELECT LocationID FROM LocationUserRole WHERE RoleID IN (SELECT RoleID FROM UserInRole WHERE UserID = {0} AND HealthcareID = '{1}') AND IsDeleted = 0)", AppSession.UserLogin.UserID, AppSession.UserLogin.HealthcareID);
                else
                    hdnRecordFilterExpression.Value = "";
            }


            string filterServiceType = string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.SERVICE_TYPE);
            List<StandardCode> lst = BusinessLayer.GetStandardCodeList(filterServiceType);
            Methods.SetComboBoxField<StandardCode>(cboServiceType, lst, "StandardCodeName", "StandardCodeID");
            cboServiceType.SelectedIndex = 1;

            Helper.SetControlEntrySetting(txtQuantity, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtItemCode, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(cboItemUnit, new ControlEntrySetting(true, true, true), "mpTrxPopup");

            BindGridView(1, true, ref PageCount);

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        private void GetSettingParameter()
        {
            string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')", AppSession.UserLogin.HealthcareID,
                                                                    Constant.SettingParameter.IM_PERMINTAAN_BARANG_DENGAN_SATUAN_KECIL);
            List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterExp);
            if (lstParam != null)
            {
                hdnIsAllowUsingAlternateUnit.Value = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.IM_PERMINTAAN_BARANG_DENGAN_SATUAN_KECIL).FirstOrDefault().ParameterValue;
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnRequestID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtRequestNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(hdnLocationIDFrom, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblLocation, new ControlEntrySetting(true, false));
            SetControlEntrySetting(txtLocationCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtLocationName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(hdnFromLocationItemGroupID, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(cboServiceType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(txtRequestDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtRequestTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));

            SetControlEntrySetting(hdnLocationIDTo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblLocationTo, new ControlEntrySetting(true, false));
            SetControlEntrySetting(txtLocationCodeTo, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtLocationNameTo, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(hdnToLocationItemGroupID, new ControlEntrySetting(false, false, false));


            SetControlEntrySetting(hdnPackageID, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblCSSDPackage, new ControlEntrySetting(true, false));
            SetControlEntrySetting(txtPackageCode, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtPackageName, new ControlEntrySetting(false, false, false));

        }

        #region Load Entity
        protected string GetFilterExpression()
        {
            return hdnRecordFilterExpression.Value;
        }

        public override int OnGetRowCount()
        {
            string filterExpression = GetFilterExpression();
            return BusinessLayer.GetvMDServiceRequestHdRowCount(filterExpression);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            vMDServiceRequestHd entity = BusinessLayer.GetvMDServiceRequestHd(filterExpression, PageIndex, "RequestID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            PageIndex = BusinessLayer.GetvMDServiceRequestHdRowIndex(filterExpression, keyValue, "RequestID DESC");
            vMDServiceRequestHd entity = BusinessLayer.GetvMDServiceRequestHd(filterExpression, PageIndex, "RequestID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vMDServiceRequestHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity.GCServiceStatus != Constant.ServiceStatus.OPEN)
            {
                hdnIsEditable.Value = "0";
                isShowWatermark = true;
                watermarkText = entity.ServiceStatus;
            }
            else
            {
                hdnIsEditable.Value = "1";
            }

            if (entity.GCServiceStatus != Constant.ServiceStatus.OPEN && entity.GCServiceStatus != Constant.ServiceStatus.VOID)
                hdnPrintStatus.Value = "true";
            else
                hdnPrintStatus.Value = "false";

            if (entity.GCServiceStatus == Constant.ServiceStatus.OPEN)
            {
                txtNotes.Enabled = true;
                cboServiceType.Enabled = true;
            }
            else
            {
                txtNotes.Enabled = false;
                cboServiceType.Enabled = false;
            } 

            hdnRequestID.Value = entity.RequestID.ToString();
            txtRequestNo.Text = entity.RequestNo;
            txtRequestDate.Text = entity.RequestDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtRequestTime.Text = entity.RequestTime;

            hdnLocationIDFrom.Value = entity.FromLocationID.ToString();
            txtLocationCode.Text = entity.FromLocationCode;
            txtLocationName.Text = entity.FromLocationName;
            hdnFromLocationItemGroupID.Value = entity.FromLocationItemGroupID.ToString();
            hdnGCLocationGroupFrom.Value = entity.FromLocationGCLocationGroup;

            hdnLocationIDTo.Value = entity.ToLocationID.ToString();
            txtLocationCodeTo.Text = entity.ToLocationCode;
            txtLocationNameTo.Text = entity.ToLocationName;
            hdnToLocationItemGroupID.Value = entity.ToLocationItemGroupID.ToString();
            hdnGCLocationGroupTo.Value = entity.ToLocationGCLocationGroup;

            cboServiceType.Value = entity.GCServiceType;

            hdnPackageID.Value = entity.PackageID.ToString();
            txtPackageCode.Text = entity.PackageCode;
            txtPackageName.Text = entity.PackageName;

            txtNotes.Text = entity.Remarks;

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
            string filterExpression = "1 = 0";
            if (hdnRequestID.Value != "")
                filterExpression = string.Format("RequestID = {0} AND IsDeleted = 0", hdnRequestID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvMDServiceRequestDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vMDServiceRequestDt> lstEntity = BusinessLayer.GetvMDServiceRequestDtList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "IsConsumption, ItemName1 ASC");
            hdnPageCount.Value = pageCount.ToString();
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
        #endregion

        #region Save & Edit Header
        private void ControlToEntityHd(MDServiceRequestHd entityHd)
        {
            entityHd.FromLocationID = Convert.ToInt32(hdnLocationIDFrom.Value);
            entityHd.ToLocationID = Convert.ToInt32(hdnLocationIDTo.Value);
            entityHd.RequestDate = Helper.GetDatePickerValue(txtRequestDate.Text);
            entityHd.RequestTime = txtRequestTime.Text;
            if (hdnPackageID.Value != "" && hdnPackageID.Value != "0")
            {
                entityHd.PackageID = Convert.ToInt32(hdnPackageID.Value);
            }
            entityHd.GCServiceType = cboServiceType.Value.ToString();
            entityHd.Remarks = txtNotes.Text;
        }

        public void SaveMDServiceRequestHd(IDbContext ctx, ref int RequestID)
        {
            MDServiceRequestHdDao entityHdDao = new MDServiceRequestHdDao(ctx);
            if (hdnRequestID.Value == "0")
            {
                MDServiceRequestHd entityHd = new MDServiceRequestHd();
                ControlToEntityHd(entityHd);
                entityHd.TransactionCode = Constant.TransactionCode.SERVICE_REQUEST;
                entityHd.GCServiceStatus = Constant.ServiceStatus.OPEN;
                entityHd.CreatedBy = AppSession.UserLogin.UserID;

                entityHd.RequestNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.RequestDate, ctx);
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                RequestID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
            }
            else
            {
                RequestID = Convert.ToInt32(hdnRequestID.Value);
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ItemDistributionHdDao distHdDao = new ItemDistributionHdDao(ctx);

            try
            {
                int RequestID = 0;
                SaveMDServiceRequestHd(ctx, ref RequestID);
                retval = RequestID.ToString();

                ItemDistributionHd distHd = new ItemDistributionHd();
                distHd.TransactionCode = Constant.TransactionCode.SERVICE_DISTRIBUTION;
                distHd.ServiceRequestID = RequestID;
                distHd.FromLocationID = Convert.ToInt32(hdnLocationIDFrom.Value);
                distHd.ToLocationID = Convert.ToInt32(hdnLocationIDTo.Value);
                distHd.DeliveryDate = DateTime.Now;
                distHd.DeliveryTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                distHd.GCDistributionStatus = Constant.DistributionStatus.OPEN;
                distHd.DistributionNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.SERVICE_DISTRIBUTION, distHd.DeliveryDate, ctx);
                distHd.CreatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                int oDistributionID =  distHdDao.InsertReturnPrimaryKeyID(distHd);

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
                MDServiceRequestHd entity = BusinessLayer.GetMDServiceRequestHd(Convert.ToInt32(hdnRequestID.Value));
                if (entity.GCServiceStatus == Constant.ServiceStatus.OPEN)
                {
                    entity.Remarks = txtNotes.Text;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateMDServiceRequestHd(entity);                 
                }
                else
                {
                    result = false;
                    errMessage = "Permintaan CSSD dengan nomor " + entity.RequestNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
            MDServiceRequestHdDao itemHdDao = new MDServiceRequestHdDao(ctx);
            MDServiceRequestDtDao itemDtDao = new MDServiceRequestDtDao(ctx);
            ItemDistributionHdDao distHdDao = new ItemDistributionHdDao(ctx);
            ItemDistributionDtDao distDtDao = new ItemDistributionDtDao(ctx);

            try
            {
                MDServiceRequestHd itemHd = itemHdDao.Get(Convert.ToInt32(hdnRequestID.Value));

                string filterDistHd = string.Format("ServiceRequestID = {0} AND TransactionCode = '{1}'", itemHd.RequestID, Constant.TransactionCode.SERVICE_DISTRIBUTION);
                ItemDistributionHd distHd = BusinessLayer.GetItemDistributionHdList(filterDistHd, ctx).FirstOrDefault();

                string filterRequestDt = String.Format("RequestID = {0} AND IsDeleted = 0", hdnRequestID.Value);
                List<MDServiceRequestDt> lstMDServiceRequestDt = BusinessLayer.GetMDServiceRequestDtList(filterRequestDt, ctx);

                if (itemHd.GCServiceStatus == Constant.ServiceStatus.OPEN)
                {
                    if (lstMDServiceRequestDt.Count > 0)
                    {
                        foreach (MDServiceRequestDt itemDt in lstMDServiceRequestDt)
                        {
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();

                            itemDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            itemDtDao.Update(itemDt);

                            if (!itemDt.IsConsumption)
                            {
                                string filterDistDt = string.Format("DistributionID = {0} AND ItemID = {1} AND IsDeleted = 0", distHd.DistributionID, itemDt.ItemID);
                                ItemDistributionDt distDt = BusinessLayer.GetItemDistributionDtList(filterDistDt, ctx).FirstOrDefault();
                                distDt.GCItemDetailStatus = Constant.DistributionStatus.ON_DELIVERY;
                                distDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                distDtDao.Update(distDt);
                            }
                        }

                        ControlToEntityHd(itemHd);
                        itemHd.GCServiceStatus = Constant.ServiceStatus.ON_DELIVERY;
                        itemHd.SentBy = AppSession.UserLogin.UserID;
                        itemHd.SentDate = DateTime.Now;
                        itemHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        itemHdDao.Update(itemHd);

                        distHd.GCDistributionStatus = Constant.DistributionStatus.ON_DELIVERY;
                        distHd.DeliveredBy = BusinessLayer.GetUser(AppSession.UserLogin.UserID).UserName;
                        distHd.DeliveryDate = DateTime.Now;
                        distHd.DeliveryTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                        distHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        distHdDao.Update(distHd);

                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Permintaan CSSD ini tidak memiliki detail item";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Permintaan CSSD dengan nomor " + itemHd.RequestNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
            MDServiceRequestHdDao itemHdDao = new MDServiceRequestHdDao(ctx);
            MDServiceRequestDtDao itemDtDao = new MDServiceRequestDtDao(ctx);
            ItemDistributionHdDao distHdDao = new ItemDistributionHdDao(ctx);
            ItemDistributionDtDao distDtDao = new ItemDistributionDtDao(ctx);

            try
            {
                MDServiceRequestHd entity = itemHdDao.Get(Convert.ToInt32(hdnRequestID.Value));

                string filterDistHd = string.Format("ServiceRequestID = {0} AND TransactionCode = '{1}'", entity.RequestID, Constant.TransactionCode.SERVICE_DISTRIBUTION);
                ItemDistributionHd distHd = BusinessLayer.GetItemDistributionHdList(filterDistHd, ctx).FirstOrDefault();

                if (entity.GCServiceStatus == Constant.ServiceStatus.OPEN)
                {
                    string filterRequestDt = String.Format("RequestID = {0} AND IsDeleted = 0", hdnRequestID.Value);
                    List<MDServiceRequestDt> lstMDServiceRequestDt = BusinessLayer.GetMDServiceRequestDtList(filterRequestDt);
                    foreach (MDServiceRequestDt itemDt in lstMDServiceRequestDt)
                    {
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();

                        itemDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        itemDtDao.Update(itemDt);

                        if (!itemDt.IsConsumption)
                        {
                            string filterDistDt = string.Format("DistributionID = {0} AND ItemID = {1} AND IsDeleted = 0", distHd.DistributionID, itemDt.ItemID);
                            ItemDistributionDt distDt = BusinessLayer.GetItemDistributionDtList(filterDistDt, ctx).FirstOrDefault();
                            distDt.GCItemDetailStatus = Constant.DistributionStatus.WAIT_FOR_APPROVAL;
                            distDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            distDtDao.Update(distDt);
                        }
                    }

                    ControlToEntityHd(entity);
                    entity.GCServiceStatus = Constant.ServiceStatus.PROPOSED;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    itemHdDao.Update(entity);

                    distHd.GCDistributionStatus = Constant.DistributionStatus.WAIT_FOR_APPROVAL;
                    distHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    distHdDao.Update(distHd);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Permintaan CSSD dengan nomor " + entity.RequestNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
            MDServiceRequestHdDao itemHdDao = new MDServiceRequestHdDao(ctx);
            MDServiceRequestDtDao itemDtDao = new MDServiceRequestDtDao(ctx);
            ItemDistributionHdDao distHdDao = new ItemDistributionHdDao(ctx);
            ItemDistributionDtDao distDtDao = new ItemDistributionDtDao(ctx);
            ItemTransactionHdDao consHdDao = new ItemTransactionHdDao(ctx);
            ItemTransactionDtDao consDtDao = new ItemTransactionDtDao(ctx);

            try
            {
                MDServiceRequestHd entity = itemHdDao.Get(Convert.ToInt32(hdnRequestID.Value));

                string filterDistHd = string.Format("ServiceRequestID = {0} AND TransactionCode = '{1}'", entity.RequestID, Constant.TransactionCode.SERVICE_DISTRIBUTION);
                ItemDistributionHd distHd = BusinessLayer.GetItemDistributionHdList(filterDistHd, ctx).FirstOrDefault();

                if (entity.GCServiceStatus == Constant.ServiceStatus.OPEN)
                {
                    string filterRequestDt = String.Format("RequestID = {0} AND IsDeleted = 0", hdnRequestID.Value);
                    List<MDServiceRequestDt> lstMDServiceRequestDt = BusinessLayer.GetMDServiceRequestDtList(filterRequestDt);
                    foreach (MDServiceRequestDt itemDt in lstMDServiceRequestDt)
                    {
                        itemDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        itemDtDao.Update(itemDt);

                        if (!itemDt.IsConsumption)
                        {
                            string filterDistDt = string.Format("DistributionID = {0} AND ItemID = {1} AND IsDeleted = 0", distHd.DistributionID, itemDt.ItemID);
                            ItemDistributionDt distDt = BusinessLayer.GetItemDistributionDtList(filterDistDt, ctx).FirstOrDefault();
                            distDt.GCItemDetailStatus = Constant.DistributionStatus.VOID;
                            distDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            distDtDao.Update(distDt);
                        }

                        if (itemDt.IsConsumption)
                        {
                            string filterCons = string.Format("TransactionID = (SELECT TransactionID FROM ItemTransactionHd WHERE ServiceRequestID = {0} AND TransactionCode = '{1}') AND ItemID = {2} AND GCItemDetailStatus != '{3}'", itemDt.RequestID, Constant.TransactionCode.SERVICE_CONSUMPTION, itemDt.ItemID, Constant.TransactionStatus.VOID);
                            ItemTransactionDt consDt = BusinessLayer.GetItemTransactionDtList(filterCons).FirstOrDefault();
                            consDt.GCItemDetailStatus = Constant.TransactionStatus.VOID;
                            consDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            consDtDao.Update(consDt);
                        }
                    }

                    entity.GCServiceStatus = Constant.ServiceStatus.VOID;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    itemHdDao.Update(entity);

                    distHd.GCDistributionStatus = Constant.DistributionStatus.VOID;
                    distHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    distHdDao.Update(distHd);

                    if (entity.PackageID != null && entity.PackageID != 0)
                    {
                        string filterConsHd = string.Format("ServiceRequestID = {0} AND TransactionCode = '{1}'", entity.RequestID, Constant.TransactionCode.SERVICE_CONSUMPTION);
                        ItemTransactionHd consHd = BusinessLayer.GetItemTransactionHdList(filterConsHd, ctx).FirstOrDefault();
                        consHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                        consHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        consHdDao.Update(consHd);
                    }

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Permintaan CSSD dengan nomor " + entity.RequestNo + " tidak dapat diubah. Harap refresh halaman ini.";
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

        #region callBack Trigger
        protected void cboItemUnit_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string filterExp = string.Format("ParentID = '{0}' AND (StandardCodeID IN (SELECT GCAlternateUnit FROM ItemAlternateUnit WHERE IsDeleted = 0 AND ItemID = {1}) OR StandardCodeID = (SELECT GCItemUnit FROM ItemMaster WHERE ItemID = {1}))", Constant.StandardCode.ITEM_UNIT, hdnItemID.Value);
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
            int RequestID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                if (hdnEntryID.Value.ToString() != "")
                {
                    RequestID = Convert.ToInt32(hdnRequestID.Value);
                    if (OnSaveEditRecordEntityDt(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecordEntityDt(ref errMessage, ref RequestID))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "delete")
            {
                RequestID = Convert.ToInt32(hdnRequestID.Value);
                if (OnDeleteEntityDt(ref errMessage, RequestID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRequestID"] = RequestID.ToString();
        }

        private void ControlToEntity(MDServiceRequestDt entityDt)
        {
            entityDt.ItemID = Convert.ToInt32(hdnItemID.Value);
            entityDt.Quantity = Convert.ToDecimal(txtQuantity.Text);
            entityDt.GCItemUnit = cboItemUnit.Value.ToString();
            entityDt.GCBaseUnit = hdnGCBaseUnit.Value;
            entityDt.ConversionFactor = Convert.ToDecimal(hdnItemUnitValue.Value);
        }

        private bool OnSaveAddRecordEntityDt(ref string errMessage, ref int RequestID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            MDServiceRequestHdDao entityHdDao = new MDServiceRequestHdDao(ctx);
            MDServiceRequestDtDao entityDtDao = new MDServiceRequestDtDao(ctx);
            ItemDistributionHdDao distHdDao = new ItemDistributionHdDao(ctx);
            ItemDistributionDtDao distDtDao = new ItemDistributionDtDao(ctx);
            try
            {
                SaveMDServiceRequestHd(ctx, ref RequestID);

                int oDistributionID = 0;
                string filterDistHd = string.Format("ServiceRequestID = {0} AND TransactionCode = '{1}'", RequestID, Constant.TransactionCode.SERVICE_DISTRIBUTION);
                List<ItemDistributionHd> distHdLst = BusinessLayer.GetItemDistributionHdList(filterDistHd, ctx);

                if (distHdLst.Count == 0)
                {
                    ItemDistributionHd distHd = new ItemDistributionHd();
                    distHd.TransactionCode = Constant.TransactionCode.SERVICE_DISTRIBUTION;
                    distHd.ServiceRequestID = RequestID;
                    distHd.FromLocationID = Convert.ToInt32(hdnLocationIDFrom.Value);
                    distHd.ToLocationID = Convert.ToInt32(hdnLocationIDTo.Value);
                    distHd.DeliveryDate = DateTime.Now;
                    distHd.DeliveryTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                    distHd.DeliveryRemarks = "Distribusi dari permintaan CSSD di nomor " + entityHdDao.Get(RequestID).RequestNo;
                    distHd.GCDistributionStatus = Constant.DistributionStatus.OPEN;
                    distHd.DistributionNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.SERVICE_DISTRIBUTION, distHd.DeliveryDate, ctx);
                    distHd.CreatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    oDistributionID = distHdDao.InsertReturnPrimaryKeyID(distHd);
                }
                else
                {
                    oDistributionID = distHdLst.FirstOrDefault().DistributionID;
                }

                if (entityHdDao.Get(RequestID).GCServiceStatus == Constant.ServiceStatus.OPEN)
                {
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();

                    MDServiceRequestDt entityDt = new MDServiceRequestDt();
                    ControlToEntity(entityDt);
                    entityDt.BaseQuantity = entityDt.Quantity;
                    entityDt.RequestID = RequestID;
                    entityDt.CreatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Insert(entityDt);
                     
                    ItemDistributionDt distDt = new ItemDistributionDt();
                    distDt.DistributionID = oDistributionID;
                    distDt.ItemID = entityDt.ItemID;
                    distDt.Quantity = entityDt.Quantity;
                    distDt.GCItemUnit = entityDt.GCItemUnit;
                    distDt.GCBaseUnit = entityDt.GCBaseUnit;
                    distDt.ConversionFactor = entityDt.ConversionFactor;
                    distDt.GCItemDetailStatus = Constant.DistributionStatus.OPEN;
                    distDt.CreatedBy = AppSession.UserLogin.UserID;

                    ItemPlanning entityPlanning = BusinessLayer.GetItemPlanningList(string.Format("ItemID = {0}", distDt.ItemID), ctx).FirstOrDefault();
                    if (entityPlanning != null)
                    {
                        distDt.AveragePrice = entityPlanning.AveragePrice;
                    }
                    else
                    {
                        distDt.AveragePrice = 0;
                    }

                    distDtDao.Insert(distDt);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Permintaan CSSD dengan nomor " + entityHdDao.Get(RequestID).RequestNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
            MDServiceRequestHdDao entityHdDao = new MDServiceRequestHdDao(ctx);
            MDServiceRequestDtDao entityDtDao = new MDServiceRequestDtDao(ctx);
            ItemDistributionDtDao distDtDao = new ItemDistributionDtDao(ctx);
            ItemTransactionDtDao consDtDao = new ItemTransactionDtDao(ctx);

            try
            {
                MDServiceRequestDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));

                string filterDist = string.Format("DistributionID = (SELECT DistributionID FROM ItemDistributionHd WHERE ServiceRequestID = {0} AND TransactionCode = '{1}') AND ItemID = {2} AND IsDeleted = 0", entityDt.RequestID, Constant.TransactionCode.SERVICE_DISTRIBUTION, entityDt.ItemID);
                ItemDistributionDt distDt = BusinessLayer.GetItemDistributionDtList(filterDist).FirstOrDefault();

                string filterCons = string.Format("TransactionID = (SELECT TransactionID FROM ItemTransactionHd WHERE ServiceRequestID = {0} AND TransactionCode = '{1}') AND ItemID = {2} AND IsDeleted = 0", entityDt.RequestID, Constant.TransactionCode.SERVICE_CONSUMPTION, entityDt.ItemID);
                ItemTransactionDt consDt = BusinessLayer.GetItemTransactionDtList(filterCons).FirstOrDefault();

                if (entityHdDao.Get(entityDt.RequestID).GCServiceStatus == Constant.ServiceStatus.OPEN && entityDt.IsDeleted == false)
                {
                    ControlToEntity(entityDt);
                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Update(entityDt);

                    if (!entityDt.IsConsumption)
                    {
                        distDt.ItemID = entityDt.ItemID;
                        distDt.Quantity = entityDt.Quantity;
                        distDt.GCItemUnit = entityDt.GCItemUnit;
                        distDt.GCBaseUnit = entityDt.GCBaseUnit;
                        distDt.ConversionFactor = entityDt.ConversionFactor;
                        distDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        distDtDao.Update(distDt);
                    }

                    if (entityDt.IsConsumption)
                    {
                        consDt.ItemID = entityDt.ItemID;
                        consDt.Quantity = entityDt.Quantity;
                        consDt.GCItemUnit = entityDt.GCItemUnit;
                        consDt.GCBaseUnit = entityDt.GCBaseUnit;
                        consDt.ConversionFactor = entityDt.ConversionFactor;
                        consDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        consDtDao.Update(consDt);
                    }

                    ctx.CommitTransaction();
                }
                else 
                {
                    result = false;
                    errMessage = "Permintaan CSSD dengan nomor " + entityHdDao.Get(Convert.ToInt32(hdnEntryID.Value)).RequestNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
            MDServiceRequestHdDao entityHdDao = new MDServiceRequestHdDao(ctx);
            MDServiceRequestDtDao entityDtDao = new MDServiceRequestDtDao(ctx);
            ItemDistributionDtDao distDtDao = new ItemDistributionDtDao(ctx);
            ItemTransactionDtDao consDtDao = new ItemTransactionDtDao(ctx);

            try
            {
                MDServiceRequestDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));

                string filterDist = string.Format("DistributionID = (SELECT DistributionID FROM ItemDistributionHd WHERE ServiceRequestID = {0} AND TransactionCode = '{1}') AND ItemID = {2} AND IsDeleted = 0", entityDt.RequestID, Constant.TransactionCode.SERVICE_DISTRIBUTION, entityDt.ItemID);
                ItemDistributionDt distDt = BusinessLayer.GetItemDistributionDtList(filterDist).FirstOrDefault();

                string filterCons = string.Format("TransactionID = (SELECT TransactionID FROM ItemTransactionHd WHERE ServiceRequestID = {0} AND TransactionCode = '{1}') AND ItemID = {2} AND GCItemDetailStatus != '{3}'", entityDt.RequestID, Constant.TransactionCode.SERVICE_CONSUMPTION, entityDt.ItemID, Constant.TransactionStatus.VOID);
                ItemTransactionDt consDt = BusinessLayer.GetItemTransactionDtList(filterCons).FirstOrDefault();

                if (entityHdDao.Get(entityDt.RequestID).GCServiceStatus == Constant.ServiceStatus.OPEN)
                {
                    entityDt.IsDeleted = true;
                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Update(entityDt);

                    if (!entityDt.IsConsumption)
                    {
                        distDt.IsDeleted = true;
                        distDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        distDtDao.Update(distDt);
                    }

                    if (entityDt.IsConsumption)
                    {
                        consDt.GCItemDetailStatus = Constant.TransactionStatus.VOID;
                        consDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        consDtDao.Update(consDt);
                    }

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Permintaan CSSD dengan nomor " + entityHdDao.Get(Convert.ToInt32(hdnEntryID.Value)).RequestNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
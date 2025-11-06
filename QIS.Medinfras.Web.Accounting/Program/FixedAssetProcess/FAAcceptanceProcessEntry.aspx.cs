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

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class FAAcceptanceProcessEntry : BasePageTrx
    {
        protected int PageCount = 1;
        protected int total = 0;

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.FA_ACCEPTANCE;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            hdnIsUsedProductLine.Value = AppSession.IsUsedProductLine;

            hdnMenuCode.Value = Constant.MenuCode.Accounting.FA_ACCEPTANCE;

            string filterSetvar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
                                                        AppSession.UserLogin.HealthcareID,
                                                        Constant.SettingParameter.FADEPRECIATION_FROM_APPROVE_FAACCEPTANCE,
                                                        Constant.SettingParameter.IS_APPROVED_FAACCEPTANCE_REPLACE_DEPRECIATIONSTARTDATE
                                                    );
            List<SettingParameterDt> lstSetvar = BusinessLayer.GetSettingParameterDtList(filterSetvar);

            hdnIsProcessDepreciationFromFAAcceptance.Value = lstSetvar.Where(a => a.ParameterCode == Constant.SettingParameter.FADEPRECIATION_FROM_APPROVE_FAACCEPTANCE).FirstOrDefault().ParameterValue;
            hdnIsApprovedFAAcceptanceReplaceDepreciationStartDate.Value = lstSetvar.Where(a => a.ParameterCode == Constant.SettingParameter.IS_APPROVED_FAACCEPTANCE_REPLACE_DEPRECIATIONSTARTDATE).FirstOrDefault().ParameterValue;

            SetControlProperties();

            BindGridView(1, true, ref PageCount);
        }

        protected override void SetControlProperties()
        {
            trProductLine.Style.Add("display", "none");
            lblProductLine.Attributes.Remove("class");

            trApprovedBy.Style.Add("display", "none");
            trApprovedDate.Style.Add("display", "none");

            divApprovedBy.InnerHtml = "";
            divApprovedDate.InnerHtml = "";

            trVoidBy.Style.Add("display", "none");
            trVoidDate.Style.Add("display", "none");

            divVoidBy.InnerHtml = "";
            divVoidDate.InnerHtml = "";

            if (isNewOrRefreshClick == false)
            {
                if (Page.Request.QueryString.Count > 0)
                {
                    string[] reqID = Page.Request.QueryString["id"].Split('|');

                    if (reqID[0] != null && reqID[0] != "")
                    {
                        FALocation faLoc = BusinessLayer.GetFALocation(Convert.ToInt32(reqID[0]));
                        hdnFALocationID.Value = faLoc.FALocationID.ToString();
                        txtFALocationCode.Text = faLoc.FALocationCode;
                        txtFALocationName.Text = faLoc.FALocationName;
                    }

                    if (reqID[1] != null && reqID[1] != "")
                    {
                        FAGroup faGroup = BusinessLayer.GetFAGroup(Convert.ToInt32(reqID[1]));
                        hdnFAGroupID.Value = faGroup.FAGroupID.ToString();
                        txtFAGroupCode.Text = faGroup.FAGroupCode;
                        txtFAGroupName.Text = faGroup.FAGroupName;
                    }

                    if (reqID[2] != null && reqID[2] != "")
                    {
                        FAItem faItem = BusinessLayer.GetFAItem(Convert.ToInt32(reqID[2]));
                        txtRemarks.Text = faItem.FixedAssetCode + " - " + faItem.FixedAssetName;
                    }
                }
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnFAAcceptanceID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtFAAcceptanceNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtAcceptanceDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtFAAcceptanceSubject, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(hdnGCTransactionStatus, new ControlEntrySetting(false, false, false, ""));

            SetControlEntrySetting(lblProductLine, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnAcceptanceProductLineID, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(hdnAcceptanceProductLineItemType, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtAcceptanceProductLineCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtAcceptanceProductLineName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblFAGroup, new ControlEntrySetting(true, false));
            SetControlEntrySetting(hdnFAGroupID, new ControlEntrySetting(true, false));
            SetControlEntrySetting(txtFAGroupCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtFAGroupName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(lblFALocation, new ControlEntrySetting(true, false));
            SetControlEntrySetting(txtFALocationCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtFALocationName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(hdnFALocationID, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
        }

        #region Load Entity
        public override void OnAddRecord()
        {
            hdnPageCount.Value = "0";
            hdnIsEditable.Value = "1";
            hdnGCTransactionStatus.Value = "";
            hdnFAGroupID.Value = "";
            hdnFALocationID.Value = "";
            txtRemarks.Text = "";
        }

        protected string GetFilterExpression()
        {
            return hdnRecordFilterExpression.Value;
        }

        protected string IsEditable()
        {
            return hdnIsEditable.Value;
        }

        public override int OnGetRowCount()
        {
            string filterExpression = GetFilterExpression();
            return BusinessLayer.GetvFAAcceptanceHdRowCount(filterExpression);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            vFAAcceptanceHd entity = BusinessLayer.GetvFAAcceptanceHd(filterExpression, PageIndex, "FAAcceptanceID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            PageIndex = BusinessLayer.GetvFAAcceptanceHdRowIndex(filterExpression, keyValue, "FAAcceptanceID DESC");
            vFAAcceptanceHd entity = BusinessLayer.GetvFAAcceptanceHd(filterExpression, PageIndex, "FAAcceptanceID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vFAAcceptanceHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                watermarkText = entity.TransactionStatus;
                hdnIsEditable.Value = "0";
            }
            else
                hdnIsEditable.Value = "1";

            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN && entity.GCTransactionStatus != Constant.TransactionStatus.VOID)
                hdnPrintStatus.Value = "true";
            else
                hdnPrintStatus.Value = "false";

            hdnGCTransactionStatus.Value = entity.GCTransactionStatus;
            hdnFAAcceptanceID.Value = entity.FAAcceptanceID.ToString();
            txtFAAcceptanceNo.Text = entity.FAAcceptanceNo;
            txtAcceptanceDate.Text = entity.AcceptanceDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            if (entity.AcceptanceProductLineID != null && entity.AcceptanceProductLineID != 0)
            {
                hdnAcceptanceProductLineID.Value = entity.AcceptanceProductLineID.ToString();
                txtAcceptanceProductLineCode.Text = entity.AcceptanceProductLineCode;
                txtAcceptanceProductLineName.Text = entity.AcceptanceProductLineName;
            }
            else
            {
                hdnAcceptanceProductLineID.Value = null;
                txtAcceptanceProductLineCode.Text = null;
                txtAcceptanceProductLineName.Text = null;
            }

            if (entity.FAGroupID != null && entity.FAGroupID != 0)
            {
                hdnFAGroupID.Value = entity.FAGroupID.ToString();
                txtFAGroupCode.Text = entity.FAGroupCode;
                txtFAGroupName.Text = entity.FAGroupName;
            }
            else
            {
                hdnFAGroupID.Value = null;
                txtFAGroupCode.Text = null;
                txtFAGroupName.Text = null;
            }

            hdnFALocationID.Value = entity.ToFALocationID.ToString();
            txtFALocationCode.Text = entity.ToFALocationCode;
            txtFALocationName.Text = entity.ToFALocationName;

            txtFAAcceptanceSubject.Text = entity.FAAcceptanceSubject;
            txtRemarks.Text = entity.Remarks;

            divCreatedBy.InnerHtml = entity.CreatedByName;
            divCreatedDate.InnerHtml = entity.CreatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);

            if (entity.ApprovedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                trApprovedBy.Style.Add("display", "none");
                trApprovedDate.Style.Add("display", "none");

                divApprovedBy.InnerHtml = "";
                divApprovedDate.InnerHtml = "";
            }
            else
            {
                trApprovedBy.Style.Remove("display");
                trApprovedDate.Style.Remove("display");

                divApprovedBy.InnerHtml = entity.ApprovedByName;
                divApprovedDate.InnerHtml = entity.ApprovedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }

            if (entity.VoidDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                trVoidBy.Style.Add("display", "none");
                trVoidDate.Style.Add("display", "none");

                divVoidBy.InnerHtml = "";
                divVoidDate.InnerHtml = "";
            }
            else
            {
                trVoidBy.Style.Remove("display");
                trVoidDate.Style.Remove("display");

                divVoidBy.InnerHtml = entity.VoidByName;
                divVoidDate.InnerHtml = entity.VoidDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }

            if (entity.LastUpdatedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                divLastUpdatedBy.InnerHtml = "";
                divLastUpdatedDate.InnerHtml = "";
            }
            else
            {
                divLastUpdatedBy.InnerHtml = entity.LastUpdatedByName;
                divLastUpdatedDate.InnerHtml = entity.LastUpdatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }

            BindGridView(1, true, ref PageCount);
            hdnPageCount.Value = PageCount.ToString();
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnFAAcceptanceID.Value != "" && hdnFAAcceptanceID.Value != "0")
            {
                filterExpression = string.Format("FAAcceptanceID = {0} AND ISNULL(GCProcurementStatusDetail,'') != '{1}' AND IsDeleted = 0", hdnFAAcceptanceID.Value, Constant.TransactionStatus.VOID);
            }
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvFAAcceptanceDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }
            List<vFAAcceptanceDt> lstEntity = BusinessLayer.GetvFAAcceptanceDtList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ProcurementQuantity, FixedAssetCode");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
        #endregion

        #region Save Edit Header
        private void ControlToEntity(FAAcceptanceHd entityHd)
        {
            entityHd.AcceptanceDate = Helper.GetDatePickerValue(txtAcceptanceDate.Text);
            if (hdnIsUsedProductLine.Value == "1")
            {
                if (hdnAcceptanceProductLineID.Value != null && hdnAcceptanceProductLineID.Value != "" && hdnAcceptanceProductLineID.Value != "0")
                {
                    entityHd.AcceptanceProductLineID = Convert.ToInt32(hdnAcceptanceProductLineID.Value);
                }
            }
            if (hdnFAGroupID.Value != null && hdnFAGroupID.Value != "" && hdnFAGroupID.Value != "0")
            {
                entityHd.FAGroupID = Convert.ToInt32(hdnFAGroupID.Value);
            }
            entityHd.ToFALocationID = Convert.ToInt32(hdnFALocationID.Value);
            entityHd.FAAcceptanceSubject = txtFAAcceptanceSubject.Text;
            entityHd.Remarks = txtRemarks.Text;
        }

        public void SaveFAAcceptanceHd(IDbContext ctx, ref int FAAcceptanceID, ref string FAAcceptanceNo)
        {
            FAAcceptanceHdDao entityHdDao = new FAAcceptanceHdDao(ctx);
            if (hdnFAAcceptanceID.Value == "0")
            {
                FAAcceptanceHd entityHd = new FAAcceptanceHd();
                ControlToEntity(entityHd);
                entityHd.FAAcceptanceNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.FIXED_ASSET_FA_ACCEPTANCE, entityHd.AcceptanceDate, ctx);
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                FAAcceptanceID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
                FAAcceptanceNo = entityHd.FAAcceptanceNo;
            }
            else
            {
                FAAcceptanceID = Convert.ToInt32(hdnFAAcceptanceID.Value);
                FAAcceptanceNo = txtFAAcceptanceNo.Text;
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                int FAAcceptanceID = 0;
                string FAAcceptanceNo = "";
                SaveFAAcceptanceHd(ctx, ref FAAcceptanceID, ref FAAcceptanceNo);
                retval = Convert.ToString(FAAcceptanceID);
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
                FAAcceptanceHd entity = BusinessLayer.GetFAAcceptanceHd(Convert.ToInt32(hdnFAAcceptanceID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    ControlToEntity(entity);
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    retval = Convert.ToString(entity.FAAcceptanceID);
                    BusinessLayer.UpdateFAAcceptanceHd(entity);
                }
                else
                {
                    errMessage = "Pembelian tidak dapat diubah. Harap refresh halaman ini.";
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
            FAItemDao faItemDao = new FAItemDao(ctx);
            FAAcceptanceHdDao faAcceptanceHdDao = new FAAcceptanceHdDao(ctx);
            FAAcceptanceDtDao faAcceptanceDtDao = new FAAcceptanceDtDao(ctx);

            try
            {
                FAAcceptanceHd entity = faAcceptanceHdDao.Get(Convert.ToInt32(hdnFAAcceptanceID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    ControlToEntity(entity);
                    entity.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                    entity.ApprovedBy = AppSession.UserLogin.UserID;
                    entity.ApprovedDate = DateTime.Now;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    faAcceptanceHdDao.Update(entity);

                    List<FAAcceptanceDt> lstEntity = BusinessLayer.GetFAAcceptanceDtList(string.Format("FAAcceptanceID = {0} AND IsDeleted = 0", hdnFAAcceptanceID.Value, ctx));
                    foreach (FAAcceptanceDt entityDt in lstEntity)
                    {
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();

                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        faAcceptanceDtDao.Update(entityDt);

                        if (hdnIsApprovedFAAcceptanceReplaceDepreciationStartDate.Value == "1")
                        {
                            FAItem faItem = faItemDao.Get(entityDt.FixedAssetID);
                            faItem.DepreciationStartDate = entity.ApprovedDate;
                            faItem.LastUpdatedBy = AppSession.UserLogin.UserID;
                            faItemDao.Update(faItem);
                        }

                        if (hdnIsProcessDepreciationFromFAAcceptance.Value == "1")
                        {
                            BusinessLayer.GenerateFADepreciation(entityDt.FixedAssetID, AppSession.UserLogin.UserID, ctx);
                        }
                    }

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

        #endregion

        #region CallBack Trigger
        protected void cboItemUnit_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
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
                    result = string.Format("refresh|{0}", pageCount);
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
            int FAAcceptanceID = 0;
            string FAAcceptanceNo = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                if (hdnEntryID.Value.ToString() != "")
                {
                    FAAcceptanceID = Convert.ToInt32(hdnFAAcceptanceID.Value);
                    if (OnSaveEditRecordEntityDt(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecordEntityDt(ref errMessage, ref FAAcceptanceID, ref FAAcceptanceNo))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "delete")
            {
                FAAcceptanceID = Convert.ToInt32(hdnFAAcceptanceID.Value);
                if (OnDeleteEntityDt(ref errMessage, ref FAAcceptanceID, ref FAAcceptanceNo))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpFAAcceptanceID"] = FAAcceptanceNo;
        }

        private void ControlToEntity(FAAcceptanceDt entityDt)
        {
            entityDt.FixedAssetID = Convert.ToInt32(hdnItemID.Value);
        }

        private bool OnSaveAddRecordEntityDt(ref string errMessage, ref int FAAcceptanceID, ref string FAAcceptanceNo)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            FAAcceptanceHdDao entityHdDao = new FAAcceptanceHdDao(ctx);
            FAAcceptanceDtDao entityDtDao = new FAAcceptanceDtDao(ctx);
            try
            {
                SaveFAAcceptanceHd(ctx, ref FAAcceptanceID, ref FAAcceptanceNo);
                if (entityHdDao.Get(FAAcceptanceID).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    FAAcceptanceDt entityDt = new FAAcceptanceDt();
                    ControlToEntity(entityDt);
                    entityDt.FAAcceptanceID = FAAcceptanceID;
                    entityDt.CreatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Insert(entityDt);
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Pembelian tidak dapat diubah. Harap refresh halaman ini.";
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
            FAAcceptanceHdDao entityHdDao = new FAAcceptanceHdDao(ctx);
            FAAcceptanceDtDao entityDtDao = new FAAcceptanceDtDao(ctx);
            try
            {
                FAAcceptanceDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                //if (entityDt.GCItemDetailStatus == Constant.TransactionStatus.OPEN)
                //{
                ControlToEntity(entityDt);
                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDtDao.Update(entityDt);
                ctx.CommitTransaction();
                //}
                //else
                //{
                //    result = false;
                //    errMessage = "Pembelian tidak dapat diubah. Harap refresh halaman ini.";
                //    ctx.RollBackTransaction();
                //}
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

        private bool OnDeleteEntityDt(ref string errMessage, ref int ID, ref string FAAcceptanceNo)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            FAAcceptanceHdDao entityHdDao = new FAAcceptanceHdDao(ctx);
            FAAcceptanceDtDao entityDtDao = new FAAcceptanceDtDao(ctx);
            try
            {

                FAAcceptanceDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                FAAcceptanceNo = entityHdDao.Get(entityDt.FAAcceptanceID).FAAcceptanceNo;
                //if (entityDt.GCItemDetailStatus == Constant.TransactionStatus.OPEN)
                //{
                //entityDt.GCItemDetailStatus = Constant.TransactionStatus.VOID;
                entityDt.IsDeleted = true;
                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDtDao.Update(entityDt);
                ctx.CommitTransaction();
                //}
                //else
                //{
                //    ctx.RollBackTransaction();
                //    errMessage = "Pembelian tidak dapat diubah. Harap refresh halaman ini.";
                //    result = false;
                //}
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                result = false;
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
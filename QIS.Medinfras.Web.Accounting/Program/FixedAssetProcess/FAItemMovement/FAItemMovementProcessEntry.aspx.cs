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

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class FAItemMovementProcessEntry : BasePageTrx
    {
        protected int PageCount = 1;

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.FA_COMBINED_ITEM_MOVEMENT;
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

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            string filterSC = string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.FA_ITEM_MOVEMENT_TYPE);
            List<StandardCode> lstSC = BusinessLayer.GetStandardCodeList(filterSC);
            Methods.SetComboBoxField(cboMovementType, lstSC, "StandardCodeName", "StandardCodeID");

            SetControlProperties();

            BindGridView(1, true, ref PageCount);
        }

        protected override void SetControlProperties()
        {
            trApprovedBy.Style.Add("display", "none");
            trApprovedDate.Style.Add("display", "none");

            divApprovedBy.InnerHtml = "";
            divApprovedDate.InnerHtml = "";

            trVoidBy.Style.Add("display", "none");
            trVoidDate.Style.Add("display", "none");

            divVoidBy.InnerHtml = "";
            divVoidDate.InnerHtml = "";
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnMovementID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtMovementNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtMovementDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(hdnFALocationIDFrom, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(lblFALocationFrom, new ControlEntrySetting(true, false));
            SetControlEntrySetting(txtFALocationCodeFrom, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtFALocationNameFrom, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(hdnFALocationIDTo, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(lblFALocationTo, new ControlEntrySetting(true, false));
            SetControlEntrySetting(txtFALocationCodeTo, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtFALocationNameTo, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(cboMovementType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
        }

        #region Load Entity
        protected string GetFilterExpression()
        {
            string filterExpression = "1 = 1";

            return filterExpression;
        }

        public override int OnGetRowCount()
        {
            string filterExpression = GetFilterExpression();
            return BusinessLayer.GetvFAItemMovementHdRowCount(filterExpression);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            vFAItemMovementHd entity = BusinessLayer.GetvFAItemMovementHd(filterExpression, PageIndex, "MovementID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();

            PageIndex = BusinessLayer.GetvFAItemMovementHdRowIndex(filterExpression, keyValue, "MovementID DESC");

            vFAItemMovementHd entity = BusinessLayer.GetvFAItemMovementHd(filterExpression, PageIndex, "MovementID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }
        #endregion

        private void EntityToControl(vFAItemMovementHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                watermarkText = entity.TransactionStatus;
                hdnIsEditable.Value = "0";

                SetControlEntrySetting(cboMovementType, new ControlEntrySetting(false, false, true));
                SetControlEntrySetting(txtRemarks, new ControlEntrySetting(false, false, false));
            }
            else
            {
                hdnIsEditable.Value = "1";
            }

            hdnMovementID.Value = entity.MovementID.ToString();
            txtMovementNo.Text = entity.MovementNo;
            txtMovementDate.Text = entity.MovementDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnFALocationIDFrom.Value = entity.FromFALocationID.ToString();
            txtFALocationCodeFrom.Text = entity.FromFALocationCode;
            txtFALocationNameFrom.Text = entity.FromFALocationName;
            hdnFALocationIDTo.Value = entity.ToFALocationID.ToString();
            txtFALocationCodeTo.Text = entity.ToFALocationCode;
            txtFALocationNameTo.Text = entity.ToFALocationName;
            cboMovementType.Value = entity.GCMovementType;
            txtRemarks.Text = entity.Remarks;
            hdnGCTransactionStatus.Value = entity.GCTransactionStatus;

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

            if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
            {
                cboMovementType.Enabled = true;
            }
            else
            {
                cboMovementType.Enabled = false;
            }

            BindGridView(1, true, ref PageCount);
        }


        #region Process Header

        private void ControlToEntityHd(FAItemMovementHd entityHd)
        {
            entityHd.FromFALocationID = Convert.ToInt32(hdnFALocationIDFrom.Value);
            entityHd.ToFALocationID = Convert.ToInt32(hdnFALocationIDTo.Value);
            entityHd.GCMovementType = cboMovementType.Value.ToString();
            entityHd.Remarks = txtRemarks.Text;
        }

        public void SaveFAItemMovementHd(IDbContext ctx, ref int movementID, ref int fromFALocationID)
        {
            FAItemMovementHdDao entityHdDao = new FAItemMovementHdDao(ctx);

            if (hdnMovementID.Value == "0")
            {
                FAItemMovementHd entityHd = new FAItemMovementHd();
                ControlToEntityHd(entityHd);
                entityHd.MovementDate = Helper.GetDatePickerValue(txtMovementDate.Text);
                entityHd.MovementNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.FIXED_ASSET_ITEM_MOVEMENT_HD, entityHd.MovementDate, ctx);
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();

                movementID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
                fromFALocationID = entityHd.FromFALocationID;
            }
            else
            {
                movementID = Convert.ToInt32(hdnMovementID.Value);
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);

            try
            {
                int movementID = 0, fromFALocationID = 0;
                SaveFAItemMovementHd(ctx, ref movementID, ref fromFALocationID);
                
                retval = movementID.ToString();

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
            FAItemMovementHdDao entityHdDao = new FAItemMovementHdDao(ctx);

            try
            {
                FAItemMovementHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnMovementID.Value));
                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entityHd.GCMovementType = cboMovementType.Value.ToString();
                    entityHd.Remarks = txtRemarks.Text;
                    entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entityHd);

                    retval = entityHd.MovementID.ToString();
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Mutasi " + entityHd.MovementNo + " tidak dapat diubah. Harap refresh halaman ini.";
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

        protected override bool OnApproveRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            FAItemMovementHdDao entityHdDao = new FAItemMovementHdDao(ctx);
            FAItemDao faitemDao = new FAItemDao(ctx);
            FAItemMovementDao movementDao = new FAItemMovementDao(ctx);

            try
            {
                FAItemMovementHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnMovementID.Value));
                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    ControlToEntityHd(entityHd);
                    entityHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                    entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHd.ApprovedDate = DateTime.Now;
                    entityHd.ApprovedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entityHd);

                    string filterDt = string.Format("MovementID = {0} AND IsDeleted = 0", entityHd.MovementID);
                    List<FAItemMovementDt> lstMovementDt = BusinessLayer.GetFAItemMovementDtList(filterDt, ctx);
                    foreach (FAItemMovementDt entityDt in lstMovementDt)
                    {
                        FAItem faitem = faitemDao.Get(entityDt.FixedAssetID);
                        faitem.FALocationID = entityHd.ToFALocationID;
                        faitem.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        faitemDao.Update(faitem);

                        FAItemMovement movement = new FAItemMovement();
                        movement.FixedAssetID = entityDt.FixedAssetID;
                        movement.FromFALocationID = entityHd.FromFALocationID;
                        movement.ToFALocationID = entityHd.ToFALocationID;
                        movement.MovementDate = entityHd.MovementDate;
                        movement.MovementNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.FIXED_ASSET_ITEM_MOVEMENT, movement.MovementDate, ctx);
                        movement.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                        movement.CreatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        movementDao.Insert(movement);
                    }

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Mutasi " + entityHd.MovementNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
            FAItemMovementHdDao entityHdDao = new FAItemMovementHdDao(ctx);

            try
            {
                FAItemMovementHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnMovementID.Value));
                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    ControlToEntityHd(entityHd);
                    entityHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                    entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entityHd);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Mutasi " + entityHd.MovementNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
            FAItemMovementHdDao entityHdDao = new FAItemMovementHdDao(ctx);

            try
            {
                FAItemMovementHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnMovementID.Value));
                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entityHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHd.VoidDate = DateTime.Now;
                    entityHd.VoidBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entityHd);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Mutasi " + entityHd.MovementNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
        
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            
            if (hdnMovementID.Value != "" && hdnMovementID.Value != "0")
            {
                filterExpression = string.Format("MovementID = {0} AND IsDeleted = 0", hdnMovementID.Value);
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvFAItemMovementDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MATRIX);
            }

            hdnPageCount.Value = pageCount.ToString();
            List<vFAItemMovementDt> lstEntity = BusinessLayer.GetvFAItemMovementDtList(filterExpression, Constant.GridViewPageSize.GRID_MATRIX, pageIndex, "FixedAssetName ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
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

        #region Process Detail
        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string[] param = e.Parameter.Split('|');
            int movementID = 0, fromFALocationID = 0;

            result = param[0] + "|";
            movementID = Convert.ToInt32(hdnMovementID.Value);
            fromFALocationID = Convert.ToInt32(hdnFALocationIDFrom.Value);

            if (param[0] == "save")
            {
                if (hdnID.Value.ToString() != "")
                {
                    if (OnSaveEditRecordEntityDt(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecordEntityDt(ref errMessage, ref movementID, ref fromFALocationID))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "delete")
            {
                if (OnDeleteEntityDt(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpOrderID"] = movementID.ToString();
        }

        private bool OnSaveAddRecordEntityDt(ref string errMessage, ref int movementID, ref int fromFALocationID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            FAItemMovementHdDao entityHdDao = new FAItemMovementHdDao(ctx);
            FAItemMovementDtDao entityDtDao = new FAItemMovementDtDao(ctx);
            try
            {
                SaveFAItemMovementHd(ctx, ref movementID, ref fromFALocationID);
                if (entityHdDao.Get(movementID).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    FAItemMovementDt entityDt = new FAItemMovementDt();
                    entityDt.MovementID = movementID;
                    entityDt.FixedAssetID = Convert.ToInt32(hdnFAItemID.Value);
                    entityDt.ReferenceNo = txtReferenceNo.Text;
                    entityDt.CreatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Insert(entityDt);
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Mutasi " + entityHdDao.Get(movementID).MovementNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
            FAItemMovementHdDao entityHdDao = new FAItemMovementHdDao(ctx);
            FAItemMovementDtDao entityDtDao = new FAItemMovementDtDao(ctx);

            try
            {
                FAItemMovementHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnMovementID.Value));
                FAItemMovementDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnID.Value));

                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    if (!entityDt.IsDeleted)
                    {
                        entityDt.ReferenceNo = txtReferenceNo.Text;
                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(entityDt);

                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Mutasi " + entityHd.MovementNo + " tidak dapat diubah. Harap refresh halaman ini.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Mutasi " + entityHd.MovementNo + " tidak dapat diubah. Harap refresh halaman ini.";
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

        private bool OnDeleteEntityDt(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            FAItemMovementHdDao entityHdDao = new FAItemMovementHdDao(ctx);
            FAItemMovementDtDao entityDtDao = new FAItemMovementDtDao(ctx);

            try
            {
                FAItemMovementHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnMovementID.Value));
                FAItemMovementDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnID.Value));

                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    if (!entityDt.IsDeleted)
                    {
                        entityDt.IsDeleted = true;
                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(entityDt);

                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Mutasi " + entityHd.MovementNo + " tidak dapat diubah. Harap refresh halaman ini.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Mutasi " + entityHd.MovementNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
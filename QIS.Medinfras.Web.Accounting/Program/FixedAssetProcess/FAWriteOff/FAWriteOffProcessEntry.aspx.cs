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
    public partial class FAWriteOffProcessEntry : BasePageTrx
    {
        protected int PageCount = 1;

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.FA_COMBINED_WRITE_OFF;
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

            string filterExpression = String.Format("ParentID IN ('{0}','{1}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.ASSET_SALES_TYPE, Constant.StandardCode.TIPE_PEMUSNAHAN);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);

            Methods.SetComboBoxField(cboAssetWriteOffType, lstStandardCode.Where(x => x.ParentID == Constant.StandardCode.TIPE_PEMUSNAHAN).ToList(), "StandardCodeName", "StandardCodeID");
            cboAssetWriteOffType.SelectedIndex = 0;

            Methods.SetComboBoxField(cboAssetSalesType, lstStandardCode.Where(x => x.ParentID == Constant.StandardCode.ASSET_SALES_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            cboAssetSalesType.SelectedIndex = 0;

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
            SetControlEntrySetting(hdnFAWriteOffHdID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtFAWriteOffHdNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtFAWriteOffHdDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtRemarksHd, new ControlEntrySetting(true, true, false));

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
            return BusinessLayer.GetvFAWriteOffHdRowCount(filterExpression);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            vFAWriteOffHd entity = BusinessLayer.GetvFAWriteOffHd(filterExpression, PageIndex, "FAWriteOffHdID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();

            PageIndex = BusinessLayer.GetvFAWriteOffHdRowIndex(filterExpression, keyValue, "FAWriteOffHdID DESC");

            vFAWriteOffHd entity = BusinessLayer.GetvFAWriteOffHd(filterExpression, PageIndex, "FAWriteOffHdID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }
        #endregion

        private void EntityToControl(vFAWriteOffHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                watermarkText = entity.TransactionStatus;
                hdnIsEditable.Value = "0";

                SetControlEntrySetting(txtRemarksHd, new ControlEntrySetting(false, false, false));
            }
            else
            {
                hdnIsEditable.Value = "1";
            }

            hdnFAWriteOffHdID.Value = entity.FAWriteOffHdID.ToString();
            txtFAWriteOffHdNo.Text = entity.FAWriteOffHdNo;
            txtFAWriteOffHdDate.Text = entity.FAWriteOffHdDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtRemarksHd.Text = entity.Remarks;
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

            BindGridView(1, true, ref PageCount);
        }


        #region Process Header

        public void SaveFAWriteOffHd(IDbContext ctx, ref int FAWriteOffHdID)
        {
            FAWriteOffHdDao entityHdDao = new FAWriteOffHdDao(ctx);

            if (hdnFAWriteOffHdID.Value == "0")
            {
                FAWriteOffHd entityHd = new FAWriteOffHd();
                entityHd.FAWriteOffHdDate = Helper.GetDatePickerValue(txtFAWriteOffHdDate.Text);
                entityHd.FAWriteOffHdNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.FIXED_ASSET_WRITE_OFF_HD, entityHd.FAWriteOffHdDate, ctx);
                entityHd.Remarks = txtRemarksHd.Text;
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();

                FAWriteOffHdID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
            }
            else
            {
                FAWriteOffHdID = Convert.ToInt32(hdnFAWriteOffHdID.Value);
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);

            try
            {
                int FAWriteOffHdID = 0;
                SaveFAWriteOffHd(ctx, ref FAWriteOffHdID);

                retval = FAWriteOffHdID.ToString();

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
            FAWriteOffHdDao entityHdDao = new FAWriteOffHdDao(ctx);

            try
            {
                FAWriteOffHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnFAWriteOffHdID.Value));
                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entityHd.Remarks = txtRemarksHd.Text;
                    entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entityHd);

                    retval = entityHd.FAWriteOffHdID.ToString();
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Pemusnahan dengan nomor " + entityHd.FAWriteOffHdNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
            FAWriteOffHdDao entityHdDao = new FAWriteOffHdDao(ctx);
            FAItemDao faitemDao = new FAItemDao(ctx);
            FAWriteOffDao writeOffDao = new FAWriteOffDao(ctx);

            try
            {
                FAWriteOffHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnFAWriteOffHdID.Value));
                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entityHd.Remarks = txtRemarksHd.Text;
                    entityHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                    entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHd.ApprovedDate = DateTime.Now;
                    entityHd.ApprovedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entityHd);

                    string filterDt = string.Format("FAWriteOffHdID = {0} AND IsDeleted = 0", entityHd.FAWriteOffHdID);
                    List<FAWriteOffDt> lstMovementDt = BusinessLayer.GetFAWriteOffDtList(filterDt, ctx);
                    foreach (FAWriteOffDt entityDt in lstMovementDt)
                    {
                        FAItem faitem = faitemDao.Get(entityDt.FixedAssetID);
                        faitem.GCItemStatus = Constant.ItemStatus.IN_ACTIVE;
                        faitem.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        faitemDao.Update(faitem);

                        FAWriteOff writeOff = new FAWriteOff();
                        writeOff.FAWriteOffDtID = entityDt.ID;
                        writeOff.FixedAssetID = entityDt.FixedAssetID;
                        writeOff.FAWriteOffNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.FIXED_ASSET_WRITE_OFF, entityHd.FAWriteOffHdDate, ctx);
                        writeOff.GCAssetWriteOffType = entityDt.GCAssetWriteOffType;
                        writeOff.GCAssetSalesType = entityDt.GCAssetSalesType;
                        writeOff.AssetValue = entityDt.AssetValue;
                        writeOff.WriteOffAmount = entityDt.WriteOffAmount;
                        writeOff.Remarks = entityDt.Remarks;
                        writeOff.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                        writeOff.CreatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        writeOffDao.Insert(writeOff);
                    }

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Pemusnahan dengan nomor " + entityHd.FAWriteOffHdNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
            FAWriteOffHdDao entityHdDao = new FAWriteOffHdDao(ctx);

            try
            {
                FAWriteOffHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnFAWriteOffHdID.Value));
                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entityHd.Remarks = txtRemarksHd.Text;
                    entityHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                    entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entityHd);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Pemusnahan dengan nomor " + entityHd.FAWriteOffHdNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
            FAWriteOffHdDao entityHdDao = new FAWriteOffHdDao(ctx);

            try
            {
                FAWriteOffHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnFAWriteOffHdID.Value));
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
                    errMessage = "Pemusnahan dengan nomor " + entityHd.FAWriteOffHdNo + " tidak dapat diubah. Harap refresh halaman ini.";
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

            if (hdnFAWriteOffHdID.Value != "" && hdnFAWriteOffHdID.Value != "0")
            {
                filterExpression = string.Format("FAWriteOffHdID = {0} AND IsDeleted = 0", hdnFAWriteOffHdID.Value);
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvFAWriteOffDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MATRIX);
            }

            hdnPageCount.Value = pageCount.ToString();
            List<vFAWriteOffDt> lstEntity = BusinessLayer.GetvFAWriteOffDtList(filterExpression, Constant.GridViewPageSize.GRID_MATRIX, pageIndex, "FixedAssetCode ASC");
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
            int FAWriteOffHdID = 0;

            result = param[0] + "|";
            FAWriteOffHdID = Convert.ToInt32(hdnFAWriteOffHdID.Value);

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
                    if (OnSaveAddRecordEntityDt(ref errMessage, ref FAWriteOffHdID))
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
            panel.JSProperties["cpOrderID"] = FAWriteOffHdID.ToString();
        }

        private bool OnSaveAddRecordEntityDt(ref string errMessage, ref int FAWriteOffHdID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            FAWriteOffHdDao entityHdDao = new FAWriteOffHdDao(ctx);
            FAWriteOffDtDao entityDtDao = new FAWriteOffDtDao(ctx);
            try
            {
                SaveFAWriteOffHd(ctx, ref FAWriteOffHdID);
                if (entityHdDao.Get(FAWriteOffHdID).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    FAWriteOffDt entityDt = new FAWriteOffDt();
                    entityDt.FAWriteOffHdID = FAWriteOffHdID;
                    entityDt.FixedAssetID = Convert.ToInt32(hdnFAItemID.Value);
                    entityDt.GCAssetWriteOffType = cboAssetWriteOffType.Value.ToString();
                    entityDt.GCAssetSalesType = cboAssetSalesType.Value.ToString();
                    entityDt.AssetValue = Convert.ToDecimal(Request.Form[txtProcurementAmount.UniqueID]);
                    entityDt.WriteOffAmount = Convert.ToDecimal(txtWriteOffAmount.Text);
                    entityDt.Remarks = txtRemarks.Text;
                    entityDt.CreatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Insert(entityDt);
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Pemusnahan dengan nomor " + entityHdDao.Get(FAWriteOffHdID).FAWriteOffHdNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
            FAWriteOffHdDao entityHdDao = new FAWriteOffHdDao(ctx);
            FAWriteOffDtDao entityDtDao = new FAWriteOffDtDao(ctx);

            try
            {
                FAWriteOffHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnFAWriteOffHdID.Value));
                FAWriteOffDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnID.Value));

                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    if (!entityDt.IsDeleted)
                    {
                        entityDt.GCAssetWriteOffType = cboAssetWriteOffType.Value.ToString();
                        entityDt.GCAssetSalesType = cboAssetSalesType.Value.ToString();
                        entityDt.WriteOffAmount = Convert.ToDecimal(txtWriteOffAmount.Text);
                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(entityDt);

                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Pemusnahan dengan nomor " + entityHd.FAWriteOffHdNo + " tidak dapat diubah. Harap refresh halaman ini.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Pemusnahan dengan nomor " + entityHd.FAWriteOffHdNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
            FAWriteOffHdDao entityHdDao = new FAWriteOffHdDao(ctx);
            FAWriteOffDtDao entityDtDao = new FAWriteOffDtDao(ctx);

            try
            {
                FAWriteOffHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnFAWriteOffHdID.Value));
                FAWriteOffDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnID.Value));

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
                        errMessage = "Pemusnahan dengan nomor " + entityHd.FAWriteOffHdNo + " tidak dapat diubah. Harap refresh halaman ini.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Pemusnahan dengan nomor " + entityHd.FAWriteOffHdNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
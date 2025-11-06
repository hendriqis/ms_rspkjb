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
    public partial class BudgetingMasterEntry : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.BUDGETING_MASTER;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public Int32 GetDisplayCount()
        {
            return Convert.ToInt32(hdnDisplayCount.Value) - 1;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            hdnIsEditable.Value = "1";

            BindGridView();
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnBudgetHdID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(hdnDefaultYear, new ControlEntrySetting(false, false, false, DateTime.Now.ToString(Constant.FormatString.YEAR_FORMAT)));
            SetControlEntrySetting(txtBudgetYear, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.YEAR_FORMAT)));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, true));

            Helper.SetControlEntrySetting(hdnGLAccountID, new ControlEntrySetting(true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtGLAccountCode, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtGLAccountName, new ControlEntrySetting(false, false, false), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtBudgetAmount, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtRemarksDt, new ControlEntrySetting(true, true, false), "mpTrxPopup");
        }

        protected override void SetControlProperties()
        {
        }

        #region Load Entity
        public override void OnAddRecord()
        {
            hdnIsEditable.Value = "1";
        }

        protected string IsEditable()
        {
            return hdnIsEditable.Value;
        }

        protected string GetFilterExpression()
        {
            return "BudgetHdID IS NOT NULL";
        }

        public override int OnGetRowCount()
        {
            string filterExpression = GetFilterExpression();
            return BusinessLayer.GetvBudgetHdRowCount(filterExpression);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            vBudgetHd entity = BusinessLayer.GetvBudgetHd(filterExpression, PageIndex, "BudgetHdID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            PageIndex = BusinessLayer.GetvBudgetHdRowIndex(filterExpression, keyValue, "BudgetHdID DESC");
            vBudgetHd entity = BusinessLayer.GetvBudgetHd(filterExpression, PageIndex, "BudgetHdID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vBudgetHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                hdnIsEditable.Value = "0";
                isShowWatermark = true;
                watermarkText = entity.TransactionStatus;
            }
            else
            {
                hdnIsEditable.Value = "1";
            }

            hdnBudgetHdID.Value = entity.BudgetHdID.ToString();
            txtBudgetYear.Text = entity.BudgetYear;
            txtRemarks.Text = entity.Remarks;

            txtTotalBudgetAmount.Text = "0";

            hdnGCTransactionStatus.Value = entity.GCTransactionStatus;

            BindGridView();

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
        }

        private void BindGridView()
        {
            string filterExpression = "1 = 0";
            if (hdnBudgetHdID.Value != "")
            {
                filterExpression = string.Format("BudgetHdID = {0} AND IsDeleted = 0 ORDER BY BudgetHdID, BudgetDtID", hdnBudgetHdID.Value);
            }

            List<vBudgetDt> lstEntity = BusinessLayer.GetvBudgetDtList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();

            txtTotalBudgetAmount.Text = lstEntity.Sum(a => a.BudgetAmount).ToString(Constant.FormatString.NUMERIC_2);
        }
        #endregion

        #region Save Header
        public void SaveBudgetHd(IDbContext ctx, ref int BudgetID)
        {
            BudgetID = 0;
            BudgetHdDao entityHdDao = new BudgetHdDao(ctx);

            if (hdnBudgetHdID.Value == "" || hdnBudgetHdID.Value == "0")
            {
                if (txtBudgetYear.Text.Length == 4)
                {
                    BudgetHd entityHd = new BudgetHd();
                    entityHd.BudgetYear = txtBudgetYear.Text;
                    entityHd.Remarks = txtRemarks.Text;
                    entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                    entityHd.CreatedBy = AppSession.UserLogin.UserID;

                    BudgetID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);

                    hdnBudgetHdID.Value = BudgetID.ToString();
                }
            }
            else
            {
                BudgetID = Convert.ToInt32(hdnBudgetHdID.Value);
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                int oBudgetHdID = 0;
                SaveBudgetHd(ctx, ref oBudgetHdID);

                retval = oBudgetHdID.ToString();

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
            BudgetHdDao budgetHdDao = new BudgetHdDao(ctx);

            try
            {
                BudgetHd entity = budgetHdDao.Get(Convert.ToInt32(hdnBudgetHdID.Value));
                entity.Remarks = txtRemarks.Text;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                budgetHdDao.Update(entity);

                ctx.CommitTransaction();
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

        protected override bool OnApproveRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            BudgetHdDao budgetHdDao = new BudgetHdDao(ctx);

            try
            {
                BudgetHd entity = budgetHdDao.Get(Convert.ToInt32(hdnBudgetHdID.Value));
                entity.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                budgetHdDao.Update(entity);

                ctx.CommitTransaction();
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

        protected override bool OnVoidRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            BudgetHdDao budgetHdDao = new BudgetHdDao(ctx);

            try
            {
                BudgetHd entity = budgetHdDao.Get(Convert.ToInt32(hdnBudgetHdID.Value));
                entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                budgetHdDao.Update(entity);

                ctx.CommitTransaction();
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

        #endregion

        #region Process Detail
        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int BudgetID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                if (hdnBudgetDtID.Value.ToString() != "" && hdnBudgetDtID.Value.ToString() != "0")
                {
                    BudgetID = Convert.ToInt32(hdnBudgetHdID.Value);
                    if (OnSaveEditRecordEntityDt(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecordEntityDt(ref errMessage, ref BudgetID))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "delete")
            {
                BudgetID = Convert.ToInt32(hdnBudgetHdID.Value);
                if (OnDeleteEntityDt(ref errMessage, BudgetID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else if (param[0] == "reopen")
            {
                BudgetID = Convert.ToInt32(hdnBudgetHdID.Value);
                if (OnReOpen(ref errMessage, BudgetID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpBudgetID"] = BudgetID.ToString();
        }

        private bool OnSaveAddRecordEntityDt(ref string errMessage, ref int BudgetID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            BudgetDtDao entityDtDao = new BudgetDtDao(ctx);
            try
            {
                SaveBudgetHd(ctx, ref BudgetID);

                BudgetDt entityDt = new BudgetDt();
                entityDt.BudgetHdID = BudgetID;
                entityDt.GLAccountID = Convert.ToInt32(hdnGLAccountID.Value);
                entityDt.BudgetAmount = Convert.ToDecimal(txtBudgetAmount.Text);
                entityDt.Remarks = txtRemarksDt.Text;
                entityDt.CreatedBy = AppSession.UserLogin.UserID;
                entityDtDao.Insert(entityDt);

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

        private bool OnSaveEditRecordEntityDt(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            BudgetDtDao entityDtDao = new BudgetDtDao(ctx);
            try
            {
                if (hdnBudgetDtID.Value != null && hdnBudgetDtID.Value != "" && hdnBudgetDtID.Value != "0")
                {
                    BudgetDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnBudgetDtID.Value));
                    entityDt.GLAccountID = Convert.ToInt32(hdnGLAccountID.Value);
                    entityDt.BudgetAmount = Convert.ToDecimal(txtBudgetAmount.Text);
                    entityDt.Remarks = txtRemarksDt.Text;
                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Update(entityDt);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Proses gagal, silahkan refresh ulang halaman.";
                    Exception ex = new Exception(errMessage);
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
            BudgetDtDao entityDtDao = new BudgetDtDao(ctx);
            try
            {
                if (hdnBudgetDtID.Value != null && hdnBudgetDtID.Value != "" && hdnBudgetDtID.Value != "0")
                {
                    BudgetDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnBudgetDtID.Value));
                    entityDt.IsDeleted = true;
                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Update(entityDt);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Proses gagal, silahkan refresh ulang halaman.";
                    Exception ex = new Exception(errMessage);
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

        private bool OnReOpen(ref string errMessage, int ID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            BudgetHdDao entityHdDao = new BudgetHdDao(ctx);
            try
            {
                if (hdnBudgetHdID.Value != null && hdnBudgetHdID.Value != "" && hdnBudgetHdID.Value != "0")
                {
                    BudgetHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnBudgetHdID.Value));
                    entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                    entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entityHd);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Proses gagal, silahkan refresh ulang halaman.";
                    Exception ex = new Exception(errMessage);
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
        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";

            BindGridView();

            result = string.Format("refresh");

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion
    }
}
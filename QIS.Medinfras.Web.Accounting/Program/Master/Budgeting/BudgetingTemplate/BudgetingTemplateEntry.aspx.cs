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
    public partial class BudgetingTemplateEntry : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.BUDGETING_TEMPLATE;
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
            SetControlEntrySetting(hdnBudgetTemplateID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtBudgetTemplateCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtBudgetTemplateName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));

            Helper.SetControlEntrySetting(hdnGLAccountID, new ControlEntrySetting(true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtGLAccountCode, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtGLAccountName, new ControlEntrySetting(false, false, false), "mpTrxPopup");
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
            return "BudgetTemplateID IS NOT NULL AND IsDeleted = 0";
        }

        public override int OnGetRowCount()
        {
            string filterExpression = GetFilterExpression();
            return BusinessLayer.GetvBudgetTemplateHdRowCount(filterExpression);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            vBudgetTemplateHd entity = BusinessLayer.GetvBudgetTemplateHd(filterExpression, PageIndex, "BudgetTemplateID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            PageIndex = BusinessLayer.GetvBudgetTemplateHdRowIndex(filterExpression, keyValue, "BudgetTemplateID DESC");
            vBudgetTemplateHd entity = BusinessLayer.GetvBudgetTemplateHd(filterExpression, PageIndex, "BudgetTemplateID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vBudgetTemplateHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            hdnBudgetTemplateID.Value = entity.BudgetTemplateID.ToString();
            txtBudgetTemplateCode.Text = entity.BudgetTemplateCode;
            txtBudgetTemplateName.Text = entity.BudgetTemplateName;
            txtRemarks.Text = entity.Remarks;

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
            if (hdnBudgetTemplateID.Value != "")
            {
                filterExpression = string.Format("BudgetTemplateID = {0} AND IsDeletedHd = 0 AND IsDeletedDt = 0 ORDER BY BudgetTemplateID", hdnBudgetTemplateID.Value);
            }

            List<vBudgetTemplateDt> lstEntity = BusinessLayer.GetvBudgetTemplateDtList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
        #endregion

        #region Save Header
        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("BudgetTemplateCode = '{0}' AND IsDeleted = 0", txtBudgetTemplateCode.Text);
            List<BudgetTemplateHd> lst = BusinessLayer.GetBudgetTemplateHdList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Template With Code " + txtBudgetTemplateCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        public void SaveBudgetHd(IDbContext ctx, ref int BudgetTemplateID)
        {
            BudgetTemplateID = 0;
            BudgetTemplateHdDao entityHdDao = new BudgetTemplateHdDao(ctx);

            if (hdnBudgetTemplateID.Value == "" || hdnBudgetTemplateID.Value == "0")
            {
                BudgetTemplateHd entityHd = new BudgetTemplateHd();
                entityHd.BudgetTemplateCode = txtBudgetTemplateCode.Text;
                entityHd.BudgetTemplateName = txtBudgetTemplateName.Text;
                entityHd.Remarks = txtRemarks.Text;
                entityHd.CreatedBy = AppSession.UserLogin.UserID;

                BudgetTemplateID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);

                hdnBudgetTemplateID.Value = BudgetTemplateID.ToString();
            }
            else
            {
                BudgetTemplateID = Convert.ToInt32(hdnBudgetTemplateID.Value);
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
            BudgetTemplateHdDao budgetTemplateHdDao = new BudgetTemplateHdDao(ctx);

            try
            {
                BudgetTemplateHd entity = budgetTemplateHdDao.Get(Convert.ToInt32(hdnBudgetTemplateID.Value));
                entity.BudgetTemplateName = txtBudgetTemplateName.Text;
                entity.Remarks = txtRemarks.Text;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                budgetTemplateHdDao.Update(entity);

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
            BudgetTemplateHdDao budgetTemplateHdDao = new BudgetTemplateHdDao(ctx);
            try
            {
                BudgetTemplateHd entity = budgetTemplateHdDao.Get(Convert.ToInt32(hdnBudgetTemplateID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                budgetTemplateHdDao.Update(entity);

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
            int BudgetTemplateID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                if (hdnID.Value.ToString() != "" && hdnID.Value.ToString() != "0")
                {
                    BudgetTemplateID = Convert.ToInt32(hdnBudgetTemplateID.Value);
                    if (OnSaveEditRecordEntityDt(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecordEntityDt(ref errMessage, ref BudgetTemplateID))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "delete")
            {
                BudgetTemplateID = Convert.ToInt32(hdnBudgetTemplateID.Value);
                if (OnDeleteEntityDt(ref errMessage, BudgetTemplateID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpBudgetID"] = BudgetTemplateID.ToString();
        }

        private bool OnSaveAddRecordEntityDt(ref string errMessage, ref int BudgetID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            BudgetTemplateDtDao entityDtDao = new BudgetTemplateDtDao(ctx);
            try
            {
                SaveBudgetHd(ctx, ref BudgetID);

                BudgetTemplateDt entityDt = new BudgetTemplateDt();
                entityDt.BudgetTemplateID = BudgetID;
                entityDt.GLAccountID = Convert.ToInt32(hdnGLAccountID.Value);
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
            BudgetTemplateDtDao entityDtDao = new BudgetTemplateDtDao(ctx);
            try
            {
                if (hdnID.Value != null && hdnID.Value != "" && hdnID.Value != "0")
                {
                    BudgetTemplateDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnID.Value));
                    entityDt.GLAccountID = Convert.ToInt32(hdnGLAccountID.Value);
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
            BudgetTemplateDtDao entityDtDao = new BudgetTemplateDtDao(ctx);
            try
            {
                if (hdnID.Value != null && hdnID.Value != "" && hdnID.Value != "0")
                {
                    BudgetTemplateDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnID.Value));
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
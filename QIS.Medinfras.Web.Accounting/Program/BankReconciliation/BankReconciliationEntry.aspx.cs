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
    public partial class BankReconciliationEntry : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.BANK_RECONCILIATION_ENTRY;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            BindGridView();

            hdnIsEditable.Value = "1";
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnBankReconciliationID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtBankReconciliationNo, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtBankReconciliationDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(cboCurrency, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtCurrentAccountBalance, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtCalculationBalance, new ControlEntrySetting(false, false, true, "0"));
            SetControlEntrySetting(hdnGLAccountID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtGLAccountNo, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtGLAccountName, new ControlEntrySetting(false, false, true));
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> listStandardCode = BusinessLayer.GetStandardCodeList(string.Format(
                                                                    "ParentID IN ('{0}') AND IsDeleted = 0 AND IsActive = 1",
                                                                    Constant.StandardCode.CURRENCY_CODE));
            Methods.SetComboBoxField<StandardCode>(cboCurrency, listStandardCode.Where(p => p.ParentID == Constant.StandardCode.CURRENCY_CODE).ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");
            cboCurrency.SelectedIndex = 0;

        }

        #region Load Entity
        public override void OnAddRecord()
        {
            hdnBankReconciliationID.Value = "0";
            hdnIsEditable.Value = "1";
            hdnDisplayCount.Value = "1";
        }

        protected string IsEditable()
        {
            return hdnIsEditable.Value;
        }

        protected string GetFilterExpression()
        {
            return "1 = 1";
        }

        public override int OnGetRowCount()
        {
            string filterExpression = GetFilterExpression();
            return BusinessLayer.GetvBankReconciliationHdRowCount(filterExpression);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            vBankReconciliationHd entity = BusinessLayer.GetvBankReconciliationHd(filterExpression, PageIndex, "BankReconciliationID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
            hdnGCTransactionStatus.Value = entity.GCTransactionStatus;
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            PageIndex = BusinessLayer.GetvBankReconciliationHdRowIndex(filterExpression, keyValue, "BankReconciliationID DESC");
            vBankReconciliationHd entity = BusinessLayer.GetvBankReconciliationHd(filterExpression, PageIndex, "BankReconciliationID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
            hdnGCTransactionStatus.Value = entity.GCTransactionStatus;
        }

        private void EntityToControl(vBankReconciliationHd entity, ref bool isShowWatermark, ref string watermarkText)
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

            hdnBankReconciliationID.Value = entity.BankReconciliationID.ToString();
            txtBankReconciliationNo.Text = entity.BankReconciliationNo;
            txtBankReconciliationDate.Text = entity.BankReconciliationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            cboCurrency.Value = entity.GCCurrencyCode;
            txtCurrentAccountBalance.Text = entity.CurrentAccountBalance.ToString();
            txtCalculationBalance.Text = entity.CalculationBalance.ToString();
            txtDifferentBalance.Text = (entity.CurrentAccountBalance - entity.CalculationBalance).ToString();
            hdnGLAccountID.Value = entity.GLAccountID.ToString();
            txtGLAccountNo.Text = entity.GLAccountNo;
            txtGLAccountName.Text = entity.GLAccountName;

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

            BindGridView();
        }

        private void BindGridView()
        {
            string oBankReconciliationID = hdnBankReconciliationID.Value == "" ? "0" : hdnBankReconciliationID.Value;

            string filterExpression = string.Format("BankReconciliationID IS NOT NULL");
            filterExpression += string.Format(" AND BankReconciliationID = {0}", oBankReconciliationID);
            filterExpression += " ORDER BY JournalDate, JournalNo";

            List<vBankReconciliationDt> lstEntity = BusinessLayer.GetvBankReconciliationDtList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
        #endregion

        #region Save Header
        public void SaveBankReconciliationHd(IDbContext ctx, ref int BankReconciliationID, ref string BankReconciliationNo)
        {
            BankReconciliationHdDao entityHdDao = new BankReconciliationHdDao(ctx);
            if (hdnBankReconciliationID.Value == "" || hdnBankReconciliationID.Value == "0")
            {
                BankReconciliationHd entityHd = new BankReconciliationHd();
                entityHd.BankReconciliationDate = Helper.GetDatePickerValue(Request.Form[txtBankReconciliationDate.UniqueID]);
                entityHd.GCCurrencyCode = cboCurrency.Value.ToString();
                entityHd.CurrentAccountBalance = Convert.ToDecimal(txtCurrentAccountBalance.Text);
                entityHd.CalculationBalance = Convert.ToDecimal(txtCalculationBalance.Text);
                entityHd.GLAccountID = Convert.ToInt32(hdnGLAccountID.Value);
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                entityHd.BankReconciliationNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.BANK_RECONCILIATION, entityHd.BankReconciliationDate, ctx);

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                entityHd.CreatedBy = AppSession.UserLogin.UserID;

                BankReconciliationID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
                BankReconciliationNo = entityHd.BankReconciliationNo;

                hdnBankReconciliationID.Value = BankReconciliationID.ToString();
            }
            else
            {
                BankReconciliationID = Convert.ToInt32(hdnBankReconciliationID.Value);
                BankReconciliationNo = txtBankReconciliationNo.Text;
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);

            try
            {
                int BankReconciliationID = 0;
                string BankReconciliationNo = "";

                SaveBankReconciliationHd(ctx, ref BankReconciliationID, ref BankReconciliationNo);
                if (BankReconciliationID != 0)
                {
                    retval = BankReconciliationID.ToString();
                }
                else
                {
                    errMessage = "Proses Gagal.";
                    result = false;
                }

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
            BankReconciliationHdDao bankReconciliationHdDao = new BankReconciliationHdDao(ctx);

            try
            {
                BankReconciliationHd entityHd = bankReconciliationHdDao.Get(Convert.ToInt32(hdnBankReconciliationID.Value));
                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entityHd.GCCurrencyCode = cboCurrency.Value.ToString();
                    entityHd.CurrentAccountBalance = Convert.ToDecimal(txtCurrentAccountBalance.Text);
                    //entityHd.CalculationBalance = Convert.ToDecimal(txtCalculationBalance.Text);
                    entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    bankReconciliationHdDao.Update(entityHd);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
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
            BankReconciliationHdDao bankReconciliationHdDao = new BankReconciliationHdDao(ctx);
            GLTransactionDtDao entityDtDao = new GLTransactionDtDao(ctx);

            try
            {
                BankReconciliationHd entityHd = bankReconciliationHdDao.Get(Convert.ToInt32(hdnBankReconciliationID.Value));
                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entityHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                    entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    bankReconciliationHdDao.Update(entityHd);

                    string filterExpression = string.Format("BankReconciliationID = {0}", entityHd.BankReconciliationID);
                    List<GLTransactionDt> lstEntityDt = BusinessLayer.GetGLTransactionDtList(filterExpression, ctx);
                    foreach (GLTransactionDt entity in lstEntityDt)
                    {
                        entity.IsReconciled = true;
                        entity.LastReconciledBy = AppSession.UserLogin.UserID;
                        entity.LastReconciledDate = DateTime.Now;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(entity);
                    }
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
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
            BankReconciliationHdDao bankReconciliationHdDao = new BankReconciliationHdDao(ctx);
            GLTransactionDtDao entityDtDao = new GLTransactionDtDao(ctx);

            try
            {
                BankReconciliationHd entityHd = bankReconciliationHdDao.Get(Convert.ToInt32(hdnBankReconciliationID.Value));
                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entityHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    bankReconciliationHdDao.Update(entityHd);

                    string filterExpression = string.Format("BankReconciliationID = {0}", entityHd.BankReconciliationID);
                    List<GLTransactionDt> lstEntityDt = BusinessLayer.GetGLTransactionDtList(filterExpression, ctx);
                    foreach (GLTransactionDt entity in lstEntityDt)
                    {
                        entity.BankReconciliationID = null;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(entity);
                    }

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
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

        #region Process Detail
        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int BankReconciliationID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";

            if (param[0] == "delete")
            {
                BankReconciliationID = Convert.ToInt32(hdnBankReconciliationID.Value);
                if (OnDeleteEntityDt(ref errMessage, BankReconciliationID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpBankReconciliationID"] = BankReconciliationID.ToString();
        }

        private bool OnDeleteEntityDt(ref string errMessage, int ID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            BankReconciliationHdDao bankReconciliationHdDao = new BankReconciliationHdDao(ctx);
            GLTransactionDtDao entityDtDao = new GLTransactionDtDao(ctx);

            try
            {
                BankReconciliationHd entityHd = bankReconciliationHdDao.Get(Convert.ToInt32(hdnBankReconciliationID.Value));
                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    decimal debitAmount = 0;
                    decimal creditAmount = 0;
                    if (hdnGLTransactionDtID.Value != "" && hdnGLTransactionDtID.Value != null)
                    {
                        GLTransactionDt entity = entityDtDao.Get(Convert.ToInt32(hdnGLTransactionDtID.Value));

                        debitAmount = entity.DebitAmount;
                        creditAmount = entity.CreditAmount;

                        entity.BankReconciliationID = null;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(entity);
                    }
                    else
                    {
                        result = false;
                        errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }

                    //entityHd.CalculationBalance -= (debitAmount - creditAmount);
                    //entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    //bankReconciliationHdDao.Update(entityHd);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
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
        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";

            BindGridView();

            result = string.Format("refresh|");

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion
    }
}
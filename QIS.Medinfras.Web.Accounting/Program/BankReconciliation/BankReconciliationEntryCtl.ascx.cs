using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class BankReconciliationEntryCtl : BaseEntryPopupCtl
    {
        private BankReconciliationEntry DetailPage
        {
            get { return (BankReconciliationEntry)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            string[] lstParam = param.Split('|');
            hdnBankReconciliationIDEntryCtl.Value = lstParam[0];
            hdnGLAccountIDEntryCtl.Value = lstParam[1];

            txtJournalDateFrom.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtJournalDateTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            BindGridView();
        }

        #region Bind Grid
        private void BindGridView()
        {
            string filterExpression = string.Format("BankReconciliationID IS NULL AND IsReconciled = 0 AND LastReconciledBy IS NULL");
            filterExpression += string.Format(" AND GLAccount = {0}", hdnGLAccountIDEntryCtl.Value);

            filterExpression += string.Format(" AND JournalDate BETWEEN '{0}' AND '{1}'",
                                                    Helper.GetDatePickerValue(txtJournalDateFrom.Text).ToString(Constant.FormatString.DATE_FORMAT_112),
                                                    Helper.GetDatePickerValue(txtJournalDateTo.Text).ToString(Constant.FormatString.DATE_FORMAT_112));

            filterExpression += " ORDER BY JournalDate, JournalNo";

            List<vBankReconciliationDt> lstEntity = BusinessLayer.GetvBankReconciliationDtList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        private void BindGridViewDetail()
        {
            string filterExpression = "1 = 0";

            if (hdnExpandID.Value != "")
            {
                filterExpression = string.Format("GLTransactionID = {0} AND GCItemDetailStatus != '{1}' AND IsDeleted = 0 ORDER BY DisplayOrder ASC",
                                            hdnExpandID.Value, Constant.TransactionStatus.VOID);
            }

            List<vGLTransactionDt> lstEntity = BusinessLayer.GetvGLTransactionDtList(filterExpression);
            grdDetail.DataSource = lstEntity;
            grdDetail.DataBind();
        }

        protected void cbpProcessDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridView();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridViewDetail();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        #region Save Entity
        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            BankReconciliationHdDao entityHdDao = new BankReconciliationHdDao(ctx);
            GLTransactionDtDao entityDtDao = new GLTransactionDtDao(ctx);

            int BankReconciliationID = 0;
            string BankReconciliationNo = "";

            try
            {
                DetailPage.SaveBankReconciliationHd(ctx, ref BankReconciliationID, ref BankReconciliationNo);
                BankReconciliationHd entityHd = entityHdDao.Get(BankReconciliationID);
                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    string filterExpression = string.Format("TransactionDtID IN ({0})", hdnSelectedTransactionDtID.Value.Substring(1));
                    List<GLTransactionDt> lstEntityDt = BusinessLayer.GetGLTransactionDtList(filterExpression, ctx);
                    foreach (GLTransactionDt entity in lstEntityDt)
                    {
                        entity.BankReconciliationID = BankReconciliationID;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(entity);
                    }

                    retval = BankReconciliationNo;

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
    }
}
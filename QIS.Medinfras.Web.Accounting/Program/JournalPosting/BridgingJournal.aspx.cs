using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxCallbackPanel;
using System.Data;

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class BridgingJournal : BasePageTrx
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.BRIDGING_JOURNAL;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowNextPrev = IsAllowSave = IsAllowVoid = false;
        }

        protected int minDate = -1;

        protected override void InitializeDataControl()
        {
            txtFromJournalDate.Text = DateTime.Today.AddDays(-7).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtToJournalDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnIsUsingSummaryJournal.Value = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.AC_IS_BRIDGING_USING_SUMMARY).ParameterValue;

            if (hdnIsUsingSummaryJournal.Value == "1")
            {
                vGLTransactionHd entity = BusinessLayer.GetvGLTransactionHd(string.Format("TransactionCode = '{0}'", Constant.TransactionCode.JOURNAL_MEMORIAL_IKHTISAR), 0, "JournalDate DESC");
                if (entity != null)
                {
                    hdnLastPostingDate.Value = entity.JournalDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    minDate = (DateTime.Now - entity.JournalDate).Days - 1;
                }
            }
            else
            {
                vGLTransactionSummaryHd entity = BusinessLayer.GetvGLTransactionSummaryHd(string.Format("TransactionCode = '{0}'", Constant.TransactionCode.JOURNAL_MEMORIAL_IKHTISAR), 0, "JournalDate DESC");
                if (entity != null)
                {
                    hdnLastPostingDate.Value = entity.JournalDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    minDate = (DateTime.Now - entity.JournalDate).Days - 1;
                }
            }

            BindGridView(CurrPage, true, ref PageCount);

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        private string GetFilterExpression()
        {
            string filterExpression = string.Format("(JournalDate BETWEEN '{0}' AND '{1}') AND FlagStaging = 0 AND GCTransactionStatus = '{2}'", 
                        Helper.GetDatePickerValue(txtFromJournalDate).ToString("yyyyMMdd"), Helper.GetDatePickerValue(txtToJournalDate).ToString("yyyyMMdd"), Constant.TransactionStatus.APPROVED);
            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvGLTransactionHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }
            if (hdnIsUsingSummaryJournal.Value == "1")
            {
                List<vGLTransactionSummaryHd> lstEntity = BusinessLayer.GetvGLTransactionSummaryHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "JournalNo");
                grdView.DataSource = lstEntity;
            }
            else
            {
                List<vGLTransactionHd> lstEntity = BusinessLayer.GetvGLTransactionHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "JournalNo");
                grdView.DataSource = lstEntity;
            }
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

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            try
            {
                string transactionCode = hdnID.Value;
                DateTime fromJournalDate =  Helper.GetDatePickerValue(txtFromJournalDate);
                DateTime toJournalDate =  Helper.GetDatePickerValue(txtToJournalDate);
                BusinessLayer.InsertDataToStaging(fromJournalDate, toJournalDate);
                return true;
            }
            catch (Exception ex)
            {
                Helper.InsertErrorLog(ex);
                errMessage = ex.Message;
                return false;
            }
        }
    }
}
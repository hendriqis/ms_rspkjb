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
    public partial class InterfaceJournalProcessByUser : BasePageTrx
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.INTERFACE_JOURNAL_PROCESS_BY_USER;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowNextPrev = IsAllowSave = IsAllowVoid = false;
        }

        protected int minDate = -1;
        protected override void InitializeDataControl()
        {
            //txtFromJournalDate.Text = DateTime.Today.AddDays(-7).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtFromJournalDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            //txtToJournalDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            vGLTransactionHd entity = BusinessLayer.GetvGLTransactionHd(string.Format(
                        "TransactionCode = '{0}' AND GCTransactionStatus != '{1}'",
                        Constant.TransactionCode.JOURNAL_MEMORIAL_IKHTISAR, Constant.TransactionStatus.VOID), 0, "JournalDate DESC");
            if (entity != null)
            {
                hdnLastPostingDate.Value = entity.JournalDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                minDate = (DateTime.Now - entity.JournalDate).Days - 1;
            }

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(String.Format("ParentID = '{0}' AND StandardCodeID != '{1}' AND IsActive = 1 AND IsDeleted = 0",
                                                    Constant.StandardCode.JOURNAL_GROUP, Constant.JournalGroup.MEMORIAL));
            Methods.SetComboBoxField<StandardCode>(cboGCJournalGroup, lstStandardCode, "StandardCodeName", "StandardCodeID");
            cboGCJournalGroup.SelectedIndex = 0;

            BindGridView(CurrPage, true, ref PageCount);

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "";
            string GCJournalGroup = cboGCJournalGroup.Value.ToString();
            switch (GCJournalGroup)
            {
                case Constant.JournalGroup.PENDAPATAN_PENERIMAAN: filterExpression = string.Format("TransactionCode BETWEEN '7200' AND '7220'"); break;
                case Constant.JournalGroup.HUTANG_PIUTANG: filterExpression = string.Format("TransactionCode BETWEEN '7221' AND '7240'"); break;
                case Constant.JournalGroup.INVENTORY: filterExpression = string.Format("TransactionCode BETWEEN '7241' AND '7260'"); break;
                case Constant.JournalGroup.PHARMACY: filterExpression = string.Format("TransactionCode BETWEEN '7261' AND '7270'"); break;
                case Constant.JournalGroup.FIXED_ASSET: filterExpression = string.Format("TransactionCode BETWEEN '7271' AND '7280'"); break;
            }

            filterExpression += " AND IsProcessBySystem = 0 ";

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvTransactionJournalProcessRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vTransactionJournalProcess> lstEntity = BusinessLayer.GetvTransactionJournalProcessList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
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
                //DateTime toJournalDate =  Helper.GetDatePickerValue(txtToJournalDate);
                DateTime toJournalDate = Helper.GetDatePickerValue(txtFromJournalDate);
                bool isAllowSave = true;
                if (hdnLastPostingDate.Value != "")
                {
                    DateTime lastPostingDate = Helper.GetDatePickerValue(hdnLastPostingDate.Value);
                    if (fromJournalDate <= lastPostingDate || toJournalDate <= lastPostingDate)
                        isAllowSave = false;
                }
                if (isAllowSave)
                {
                    string journalDate = string.Format("{0}|{1}", fromJournalDate.ToString("yyyyMMdd"), toJournalDate.ToString("yyyyMMdd"));
                    retval = BusinessLayer.ProcessInterfaceJournal(AppSession.UserLogin.HealthcareID, journalDate, transactionCode, AppSession.UserLogin.UserID);
                    return true;
                }
                else
                {
                    errMessage = "Journal Pada Periode ini Telah Diposting";
                    return false;
                }
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
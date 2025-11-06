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
    public partial class GLProcessAutomaticInformation : BasePageTrx
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.INFORMATION_JOURNAL_PROCESS;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowNextPrev = IsAllowSave = IsAllowVoid = false;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            txtFromDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtToDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            List<StandardCode> lstSC = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0",
                                                                    Constant.StandardCode.JOURNAL_PROCESS_STATUS));
            Methods.SetComboBoxField<StandardCode>(cboStatusJournal, lstSC, "StandardCodeName", "StandardCodeID");
            cboStatusJournal.SelectedIndex = 0;

            BindGridViewSummary(CurrPage, true, ref PageCount);
            BindGridViewDetail(CurrPage, true, ref PageCount);
        }

        private void BindGridViewSummary(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("{0}|{1}",
                    Helper.GetDatePickerValue(txtFromDate.Text).ToString(Constant.FormatString.DATE_FORMAT_112),
                    Helper.GetDatePickerValue(txtToDate.Text).ToString(Constant.FormatString.DATE_FORMAT_112));

            List<GetJournalProcessLogCountSummary> lstEntity = BusinessLayer.GetJournalProcessLogCountSummary(filterExpression);
            grdViewSummary.DataSource = lstEntity;
            grdViewSummary.DataBind();
        }

        private void BindGridViewDetail(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("ProcessDateTime BETWEEN '{0}' AND '{1}' AND GCJournalProcessStatus = '{2}'",
                    Helper.GetDatePickerValue(txtFromDate.Text).ToString(Constant.FormatString.DATE_FORMAT_112),
                    Helper.GetDatePickerValue(txtToDate.Text).ToString(Constant.FormatString.DATE_FORMAT_112),
                    cboStatusJournal.Value);

            List<vJournalProcessLog> lstEntity = BusinessLayer.GetvJournalProcessLogList(filterExpression);
            grdViewDetail.DataSource = lstEntity;
            grdViewDetail.DataBind();
        }

        protected void cbpViewSummary_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewSummary(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewSummary(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDetail(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewDetail(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}
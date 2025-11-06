using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class JournalClosingEntry : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.JOURNAL_CLOSING;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void SetControlProperties()
        {
            string filterExpression = String.Format("TransactionCode = '{0}' AND GCTransactionStatus = '{1}'",
                                    Constant.TransactionCode.JOURNAL_MEMORIAL_IKHTISAR, Constant.TransactionStatus.CLOSED);
            vGLTransactionHd entity = BusinessLayer.GetvGLTransactionHd(filterExpression, 0, "JournalDate DESC");
            if (entity != null)
            {
                divJournalNo.InnerHtml = entity.JournalNo;
                divJournalDate.InnerHtml = entity.JournalDate.ToString(Constant.FormatString.DATE_FORMAT);
                divCreatedBy.InnerHtml = entity.CreatedByName;
                divCreatedDate.InnerHtml = entity.CreatedDate.ToString(Constant.FormatString.DATE_FORMAT);
                divLastUpdatedBy.InnerHtml = entity.LastUpdatedByName;
                divLastUpdatedDate.InnerHtml = entity.LastUpdatedDate.ToString(Constant.FormatString.DATE_FORMAT);

                txtPeriod.Text = entity.JournalDate.AddMonths(1).ToString("MMM-yyyy");
                hdnPeriodNo.Value = entity.JournalDate.AddMonths(1).ToString("yyyyMM");
            }
            else
            {
                divJournalNo.InnerHtml = "-";
                divJournalDate.InnerHtml = "-";
                divCreatedBy.InnerHtml = "-";
                divCreatedDate.InnerHtml = "-";
                divLastUpdatedBy.InnerHtml = "-";
                divLastUpdatedDate.InnerHtml = "-";

                filterExpression = String.Format("TransactionCode = '{0}' AND GCTransactionStatus = '{1}'",
                                    Constant.TransactionCode.JOURNAL_MEMORIAL_IKHTISAR, Constant.TransactionStatus.APPROVED);
                entity = BusinessLayer.GetvGLTransactionHd(filterExpression, 0, "JournalDate ASC");
                if (entity != null)
                {
                    txtPeriod.Text = entity.JournalDate.ToString("MMM-yyyy");
                    hdnPeriodNo.Value = entity.JournalDate.ToString("yyyyMM");
                }
            }
        }

        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "closing")
            {
                if (ClosingJournal(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool ClosingJournal(ref string errMessage)
        {
            try
            {
                bool result = BusinessLayer.ClosingJournal(AppSession.UserLogin.HealthcareID, hdnPeriodNo.Value, AppSession.UserLogin.UserID);
                if (result)
                {
                    return true;
                }
                errMessage = "Maaf, proses Closing Gagal";
                return false;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
    }
}
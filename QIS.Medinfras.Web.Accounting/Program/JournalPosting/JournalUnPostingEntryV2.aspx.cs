using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class JournalUnPostingEntryV2 : BasePageTrx
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.JOURNAL_UNPOSTINGV2;
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
                    Constant.TransactionCode.JOURNAL_MEMORIAL_IKHTISAR, Constant.TransactionStatus.VOID);
            vGLTransactionHd entity = BusinessLayer.GetvGLTransactionHd(filterExpression, 0, "LastUpdatedDate DESC");
            if (entity != null)
            {
                divJournalNo.InnerHtml = entity.JournalNo;
                divJournalDate.InnerHtml = entity.JournalDate.ToString(Constant.FormatString.DATE_FORMAT);
                divCreatedBy.InnerHtml = entity.CreatedByName;
                divCreatedDate.InnerHtml = entity.CreatedDate.ToString(Constant.FormatString.DATE_FORMAT);
                divLastUpdatedBy.InnerHtml = entity.LastUpdatedByName;
                divLastUpdatedDate.InnerHtml = entity.LastUpdatedDate.ToString(Constant.FormatString.DATE_FORMAT);
            }
            else
            {
                divJournalNo.InnerHtml = "-";
                divJournalDate.InnerHtml = "-";
                divCreatedBy.InnerHtml = "-";
                divCreatedDate.InnerHtml = "-";
                divLastUpdatedBy.InnerHtml = "-";
                divLastUpdatedDate.InnerHtml = "-";
            }

            BindGridView(CurrPage, true, ref PageCount);
        }

        private string GetFilterExpression()
        {
            string filterExpression = "", filterDefault = "";
            string JournalNo = "";

            filterDefault = string.Format("TransactionCode = '{0}' AND GCTransactionStatus IN ('{1}','{2}')",
                                    Constant.TransactionCode.JOURNAL_MEMORIAL_IKHTISAR, Constant.TransactionStatus.APPROVED, Constant.TransactionStatus.PROCESSED);
            List<vGLTransactionHd> lstHD = BusinessLayer.GetvGLTransactionHdList(filterDefault, Constant.GridViewPageSize.GRID_MASTER, 1, "LastUpdatedDate DESC");
            if (lstHD.Count != 0)
            {
                JournalNo = lstHD.FirstOrDefault().JournalNo;
                filterExpression += string.Format("JournalNo = '{0}'", JournalNo);
            }
            else
            {
                filterExpression += string.Format("{0}", filterDefault);
            }

            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();

            List<vGLTransactionHd> lstEntity = BusinessLayer.GetvGLTransactionHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "JournalDate, JournalGroup, JournalNo");
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

        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "unposting")
            {
                if (UnpostingJournal(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool UnpostingJournal(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            GLTransactionHdDao glTransactionhdDao = new GLTransactionHdDao(ctx);
            try
            {
                GLTransactionHd entityHD = glTransactionhdDao.Get(Convert.ToInt32(hdnID.Value));
                if ((entityHD.GCTransactionStatus == Constant.TransactionStatus.APPROVED || entityHD.GCTransactionStatus == Constant.TransactionStatus.PROCESSED) && entityHD.TransactionCode == Constant.TransactionCode.JOURNAL_MEMORIAL_IKHTISAR && !entityHD.FlagStaging)
                {
                    bool resultDt = BusinessLayer.UnpostingJournalv2(AppSession.UserLogin.HealthcareID, entityHD.JournalNo, AppSession.UserLogin.UserID);
                    if (resultDt)
                    {
                        result = true;
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        errMessage = "GAGAL UNPOSTING";
                        result = false;
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    errMessage = "GAGAL UNPOSTING";
                    result = false;
                    ctx.RollBackTransaction();
                }
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
    }
}
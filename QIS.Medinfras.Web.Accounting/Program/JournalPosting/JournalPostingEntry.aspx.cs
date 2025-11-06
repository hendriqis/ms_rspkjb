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
    public partial class JournalPostingEntry : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.JOURNAL_POSTING;
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

            string filterVT = String.Format("ParameterCode = '{0}'", Constant.SettingParameter.PENDING_HUTANG);
            SettingParameterDt setvarVT = BusinessLayer.GetSettingParameterDtList(filterVT).FirstOrDefault();
            hdnSisaJurnal.Value = setvarVT.ParameterValue;

            if (hdnSisaJurnal.Value == "0")
            {
                trHutangSupplier.Style.Add("display", "none");
            }
        }

        protected override void SetControlProperties()
        {
            string filterExpression = String.Format("TransactionCode = '{0}' AND GCTransactionStatus != '{1}'",
                    Constant.TransactionCode.JOURNAL_MEMORIAL_IKHTISAR, Constant.TransactionStatus.VOID);
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

                filterExpression = String.Format("GCTransactionStatus != '{0}'", Constant.TransactionStatus.VOID);
                entity = BusinessLayer.GetvGLTransactionHd(filterExpression, 0, "JournalDate ASC");
                if (entity != null)
                {
                    txtPeriod.Text = entity.JournalDate.ToString("MMM-yyyy");
                    hdnPeriodNo.Value = entity.JournalDate.ToString("yyyyMM");
                }
            }

            BindGridView();
            BindGridViewHutang();
        }
        
        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "posting")
            {
                if (PostingJournal(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool PostingJournal(ref string errMessage)
        {
            try
            {
                if (hdnIsAllowPosting.Value == "1")
                {
                    bool result = BusinessLayer.PostingJournal(AppSession.UserLogin.HealthcareID, hdnPeriodNo.Value, AppSession.UserLogin.UserID);
                    //bool result = BusinessLayer.PostingJournalStep1(hdnPeriodNo.Value, AppSession.UserLogin.UserID, true);

                    if (result)
                    {
                        return true;
                    }
                    else
                    {
                        errMessage = "Maaf, masih ada Junal yang belum seimbang atau statusnya masih open (trigger)";
                        return false;
                    }
                }
                else
                {
                    errMessage = "Maaf, masih ada Junal yang belum seimbang atau statusnya masih open";
                    return false;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("PeriodNo = '{0}'", hdnPeriodNo.Value);

            List<vBeforeJournalPostingInformation> lstEntity = BusinessLayer.GetvBeforeJournalPostingInformationList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();

            if (lstEntity.Count == 0)
            {
                hdnIsAllowPosting.Value = "1";
            }
        }

        private void BindGridViewHutang()
        {
            List<GetPurchaseInvoicePending> lstEntity = BusinessLayer.GetPurchaseInvoicePending(hdnPeriodNo.Value);
            grdViewHutang.DataSource = lstEntity;
            grdViewHutang.DataBind();

            if (lstEntity.Count == 0)
            {
                hdnIsAllowPosting.Value = "1";
            }
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            BindGridView();
            result = string.Format("refresh|");

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewHutang_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            BindGridViewHutang();
            result = string.Format("refresh|");

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}
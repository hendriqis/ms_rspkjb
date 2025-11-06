using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using System.Data;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class ARInvoicePayerProcessEntry2 : BasePageTrx
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.AR_INVOICE_PAYER_PROCESS;
        }
        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
        }

        protected string OnGetCustomerFilterExpression()
        {
            return string.Format("IsDeleted = 0");
        }

        protected override void InitializeDataControl()
        {
            List<Bank> lstBank = BusinessLayer.GetBankList("IsDeleted = 0");
            Methods.SetComboBoxField<Bank>(cboBank, lstBank, "BankName", "BankID");
            cboBank.SelectedIndex = 0;
            txtInvoiceDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            List<Term> lstTerm = BusinessLayer.GetTermList(string.Format("IsDeleted = 0"));
            Methods.SetComboBoxField<Term>(cboTerm, lstTerm, "TermName", "TermID");
            cboTerm.SelectedIndex = 0;
            BindGridView(1, true, ref PageCount);
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnARInvoiceID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtARInvoiceNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtInvoiceDate, new ControlEntrySetting(true, true, true, Constant.DefaultValueEntry.DATE_NOW));
            SetControlEntrySetting(cboTerm, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboBank, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
        }

        protected string onGetARInvoiceFilterExpression()
        {
            return string.Format("BusinessPartnerID = {0} AND GCTransactionStatus NOT IN ('{1}','{2}') ", AppSession.BusinessPartnerID, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
        }

        protected string GetFilterExpression()
        {
            return onGetARInvoiceFilterExpression();
        }

        public override int OnGetRowCount()
        {
            string filterExpression = GetFilterExpression();
            return BusinessLayer.GetvARInvoiceHdRowCount(filterExpression);
        }

        public override void OnAddRecord()
        {
            hdnIsEditable.Value = "1";
            hdnPageCount.Value = "0";
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            vARInvoiceHd entity = BusinessLayer.GetvARInvoiceHdList(filterExpression, PageIndex, " ARInvoiceID DESC")[0];
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            PageIndex = BusinessLayer.GetvARInvoiceHdRowIndex(filterExpression, keyValue, "ARInvoiceID DESC");
            vARInvoiceHd entity = BusinessLayer.GetvARInvoiceHdList(filterExpression, PageIndex, "ARInvoiceID DESC")[0];
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vARInvoiceHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                watermarkText = entity.TransactionStatusWatermark;
                hdnIsEditable.Value = "0";
            }
            else
                hdnIsEditable.Value = "1";
            hdnARInvoiceID.Value = entity.ARInvoiceID.ToString();
            txtARInvoiceNo.Text = entity.ARInvoiceNo;
            txtInvoiceDate.Text = entity.ARInvoiceDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            cboBank.Value = entity.BankID.ToString();
            cboTerm.Value = entity.TermID.ToString();
            txtRemarks.Text = entity.Remarks;

            BindGridView(1, true, ref PageCount);
            hdnPageCount.Value = PageCount.ToString();
        }
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnARInvoiceID.Value != "")
                filterExpression = string.Format("ARInvoiceID = {0}", hdnARInvoiceID.Value);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvARInvoiceDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vARInvoiceDt> lstInvoiceDt = BusinessLayer.GetvARInvoiceDtList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ARInvoiceID ASC");
            grdView.DataSource = lstInvoiceDt;
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

        public void SaveARInvoiceHd(IDbContext ctx, ref int ARInvoiceID)
        {
            if (hdnARInvoiceID.Value == "0")
            {
                ARInvoiceHdDao entityHdDao = new ARInvoiceHdDao(ctx);
                TermDao termDao = new TermDao(ctx);

                ARInvoiceHd entityHd = new ARInvoiceHd();
                entityHd.ARInvoiceDate = Helper.GetDatePickerValue(txtInvoiceDate);
                entityHd.BankID = Convert.ToInt32(cboBank.Value);
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                entityHd.MRN = null;
                entityHd.BusinessPartnerID = AppSession.BusinessPartnerID;
                entityHd.Remarks = txtRemarks.Text;
                entityHd.TermID = Convert.ToInt32(cboTerm.Value);
                Term term = termDao.Get(entityHd.TermID);
                entityHd.DueDate = entityHd.ARInvoiceDate.AddDays(term.TermDay);
                entityHd.ARInvoiceNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.AR_INVOICE_PAYER, entityHd.ARInvoiceDate, ctx);
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                entityHdDao.Insert(entityHd);
                entityHd.ARInvoiceID = BusinessLayer.GetARInvoiceHdMaxID(ctx);

                ARInvoiceID = entityHd.ARInvoiceID;
            }
            else
                ARInvoiceID = Convert.ToInt32(hdnARInvoiceID.Value);
        }
    }
}
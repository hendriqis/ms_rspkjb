using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Utils;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class UnbalanceJournalInformation : BasePageTrx
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.UNBALANCE_JOURNAL;
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

            String filterExpressionTransactionType = String.Format("TransactionCode LIKE '{0}'",Constant.TransactionCode.JOURNAL);
            List<TransactionType> lstTransactionType = BusinessLayer.GetTransactionTypeList(filterExpressionTransactionType);
            Methods.SetComboBoxField(cboDataSource, lstTransactionType, "TransactionName", "TransactionCode");
            
            BindGridView(1, true, ref PageCount);
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref PageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridView(1, true, ref PageCount);
                    result = "refresh|" + PageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (cboDataSource.Value != null) 
            {
                filterExpression = String.Format("GCTransactionStatus != '{0}' AND TransactionCode = '{1}' AND (DebitAmount - CreditAmount) != 0", Constant.TransactionStatus.VOID, cboDataSource.Value);
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvGLTransactionHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_ITEM);
            }

            List<vGLTransactionHd> lstEntity = BusinessLayer.GetvGLTransactionHdList(filterExpression, Constant.GridViewPageSize.GRID_ITEM, pageIndex, "JournalNo");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }


    }
}
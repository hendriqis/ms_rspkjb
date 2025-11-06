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
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class SupplierCreditNoteVoidList : BasePageList
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.CREDIT_NOTE_VOID;
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "Supplier Name" };
            fieldListValue = new string[] { "BusinessPartnerName" };
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            BindGridView(1, true, ref PageCount);
        }

        private string GetFilterExpression()
        {
            string filterExpression = String.Format("CreditNoteID NOT IN (SELECT CreditNoteID FROM PurchaseInvoiceDt WITH(NOLOCK) WHERE CreditNoteID IS NOT NULL AND IsDeleted = 0) AND GCTransactionStatus = '{0}'", Constant.TransactionStatus.APPROVED);
            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += GetFilterExpression();

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvSupplierCreditNoteRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vSupplierCreditNote> lstEntity = BusinessLayer.GetvSupplierCreditNoteList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "CreditNoteNo DESC");
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

        protected override bool OnCustomButtonClick(string type, ref string retval, ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            SupplierCreditNoteDao entityDao = new SupplierCreditNoteDao(ctx);
            try
            {
                if (type == "decline")
                {
                    string[] listParam = hdnParam.Value.Split(',');
                    foreach (string param in listParam)
                    {
                        int CreditNoteID = Convert.ToInt32(param);
                        SupplierCreditNote entity = entityDao.Get(CreditNoteID);
                        if (entity.GCTransactionStatus == Constant.TransactionStatus.APPROVED)
                        {
                            entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDao.Update(entity);
                        }

                    }
                    ctx.CommitTransaction();
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
    }
}
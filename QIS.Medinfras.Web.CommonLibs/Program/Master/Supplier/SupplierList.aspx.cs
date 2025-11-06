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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class SupplierList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        private const string DEFAULT_GRDVIEW_FILTER = "BusinessPartnerID > 1 AND GCBusinessPartnerType = '{0}' AND (HealthcareID = '{1}' OR HealthcareID IS NULL) AND IsDeleted = 0";

        public override string OnGetMenuCode()
        {
            String id = Request.QueryString["id"];

            if (id == "FN")
            {
                return Constant.MenuCode.Finance.SUPPLIER;
            }
            else
            {
                return Constant.MenuCode.Inventory.SUPPLIER;
            }
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            hdnFilterExpression.Value = filterExpression;
            hdnID.Value = keyValue;
            filterExpression = GetFilterExpression();
            if (keyValue != "")
            {
                int row = BusinessLayer.GetBusinessPartnersRowIndex(filterExpression, keyValue) + 1;
                CurrPage = Helper.GetPageCount(row, Constant.GridViewPageSize.GRID_MASTER);
            }
            else
                CurrPage = 1;

            BindGridView(CurrPage, true, ref PageCount);
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "Supplier Name", "Supplier Code", "Short Name" };
            fieldListValue = new string[] { "BusinessPartnerName", "BusinessPartnerCode", "ShortName" };
        }

        private string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += String.Format(DEFAULT_GRDVIEW_FILTER, Constant.BusinessObjectType.SUPPLIER, AppSession.UserLogin.HealthcareID);
            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetBusinessPartnersRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<BusinessPartners> lstEntity = BusinessLayer.GetBusinessPartnersList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, " BusinessPartnerCode ASC");
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

        protected override bool OnAddRecord(ref string url, ref string errMessage)
        {
            String MenuID = Request.QueryString["id"];
            url = ResolveUrl(string.Format("~/Libs/Program/Master/Supplier/SupplierEntry.aspx?menu={0}", MenuID));
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                String MenuID = Request.QueryString["id"];
                url = ResolveUrl(string.Format("~/Libs/Program/Master/Supplier/SupplierEntry.aspx?menu={0}&id={1}", MenuID, hdnID.Value));
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            BusinessPartnersDao bpDao = new BusinessPartnersDao(ctx);
            try
            {
                if (hdnID.Value != "" && hdnID.Value != "0")
                {
                    List<PurchaseOrderHd> porder = BusinessLayer.GetPurchaseOrderHdList(string.Format("BusinessPartnerID = {0} AND GCTransactionStatus != '{1}'",
                                hdnID.Value, Constant.TransactionStatus.VOID), ctx);
                    List<PurchaseReceiveHd> preceive = BusinessLayer.GetPurchaseReceiveHdList(string.Format("BusinessPartnerID = {0} AND GCTransactionStatus != '{1}'",
                                hdnID.Value, Constant.TransactionStatus.VOID), ctx);
                    List<PurchaseReturnHd> preturn = BusinessLayer.GetPurchaseReturnHdList(string.Format("BusinessPartnerID = {0} AND GCTransactionStatus != '{1}'",
                                hdnID.Value, Constant.TransactionStatus.VOID), ctx);
                    List<SupplierCreditNote> scn = BusinessLayer.GetSupplierCreditNoteList(string.Format("BusinessPartnerID = {0} AND GCTransactionStatus != '{1}'",
                                hdnID.Value, Constant.TransactionStatus.VOID), ctx);
                    List<PurchaseInvoiceHd> pinvoice = BusinessLayer.GetPurchaseInvoiceHdList(string.Format("BusinessPartnerID = {0} AND GCTransactionStatus != '{1}'",
                                hdnID.Value, Constant.TransactionStatus.VOID), ctx);
                    List<SupplierPaymentHd> sph = BusinessLayer.GetSupplierPaymentHdList(string.Format("BusinessPartnerID = {0} AND GCTransactionStatus != '{1}'",
                                hdnID.Value, Constant.TransactionStatus.VOID), ctx);
                    List<DirectPurchaseHd> dpurchase = BusinessLayer.GetDirectPurchaseHdList(string.Format("BusinessPartnerID = {0} AND GCTransactionStatus != '{1}'",
                                hdnID.Value, Constant.TransactionStatus.VOID), ctx);
                    List<DirectPurchaseReturnHd> dpurchasereturn = BusinessLayer.GetDirectPurchaseReturnHdList(string.Format("BusinessPartnerID = {0} AND GCTransactionStatus != '{1}'",
                                hdnID.Value, Constant.TransactionStatus.VOID), ctx);

                    if (porder.Count == 0 && preceive.Count == 0 && preturn.Count == 0 && scn.Count == 0 && pinvoice.Count == 0 && sph.Count == 0 && dpurchase.Count == 0 && dpurchasereturn.Count == 0)
                    {
                        BusinessPartners entity = bpDao.Get(Convert.ToInt32(hdnID.Value));
                        entity.IsDeleted = true;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        bpDao.Update(entity);

                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Supplier tidak dapat dihapus karena sudah digunakan untuk transaksi.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Supplier tidak dapat dihapus karena sudah digunakan untuk transaksi.";
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
    }
}
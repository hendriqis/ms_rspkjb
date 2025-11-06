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

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class ARInvoicePayerList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        private const string DEFAULT_GRDVIEW_FILTER = "BusinessPartnerID > 1 AND GCBusinessPartnerType = '{0}' AND (HealthcareID = '{1}' OR HealthcareID IS NULL) AND IsDeleted = 0";
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.AR_INVOICE_PAYER;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowDelete = IsAllowEdit = false;
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
            {
                CurrPage = 1;
            }

            BindGridView(CurrPage, true, ref PageCount);
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "Customer Name", "Customer Code", "Short Name" };
            fieldListValue = new string[] { "BusinessPartnerName", "BusinessPartnerCode", "ShortName" };
        }

        private string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
            {
                filterExpression += " AND ";
            }
            filterExpression += string.Format("((IsActive = 1 AND IsBlacklist = 0) OR (BusinessPartnerID IN (SELECT ah.BusinessPartnerID FROM ARInvoiceHd ah WITH(NOLOCK) WHERE ah.GCTransactionStatus <> '{0}')))", Constant.TransactionStatus.VOID);

            if (filterExpression != "")
            {
                filterExpression += " AND ";
            }
            filterExpression += String.Format(DEFAULT_GRDVIEW_FILTER, Constant.BusinessObjectType.CUSTOMER, AppSession.UserLogin.HealthcareID);
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

            List<BusinessPartners> lstEntity = BusinessLayer.GetBusinessPartnersList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "BusinessPartnerName");
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

        protected void btnOpenTransactionDt_Click(object sender, EventArgs e)
        {
            if (hdnID.Value.ToString() != "")
            {
                string filterCust = string.Format("BusinessPartnerID = {0}", hdnID.Value);
                vCustomer entity = BusinessLayer.GetvCustomerList(filterCust).FirstOrDefault();
                AppSession.BusinessPartnerID = entity.BusinessPartnerID;
                AppSession.BusinessPartnerName = entity.BusinessPartnerName;
                AppSession.CustomerGroupID = entity.CustomerGroupID;

                List<GetUserMenuAccess> lstMenu = BusinessLayer.GetUserMenuAccess(Constant.Module.FINANCE, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("ParentCode = '{0}'", OnGetMenuCode())).OrderBy(p => p.MenuIndex).ToList();
                GetUserMenuAccess menu = lstMenu.OrderBy(p => p.MenuIndex).FirstOrDefault();
                Response.Redirect(Page.ResolveUrl(menu.MenuUrl));
            }
        }
    }
}
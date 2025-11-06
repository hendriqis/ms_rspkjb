using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class APSupplierList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.SUPPLIER_LIST;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            hdnFilterExpression.Value = filterExpression;
            hdnID.Value = keyValue;
            filterExpression = GetFilterExpression();
            if (keyValue != "")
            {
                int row = BusinessLayer.GetvAPInvoiceSupplierRowIndex(filterExpression, keyValue) + 1;
                CurrPage = Helper.GetPageCount(row, Constant.GridViewPageSize.GRID_MASTER);
            }
            else
                CurrPage = 1;

            BindGridView(CurrPage, true, ref PageCount);
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "BusinessPartnerName", "BusinessPartnerCode", "BusinessPartnerType", "MasterType" };
            fieldListValue = new string[] { "BusinessPartnerName", "BusinessPartnerCode", "BusinessPartnerType", "MasterType" };
        }

        private string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;
            return filterExpression;

        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvAPInvoiceSupplierRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vAPInvoiceSupplier> lstEntity = BusinessLayer.GetvAPInvoiceSupplierList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "BusinessPartnerName ASC");
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
                BusinessPartners entity = BusinessLayer.GetBusinessPartners(Convert.ToInt32(hdnID.Value));
                AppSession.BusinessPartnerID = entity.BusinessPartnerID;
                AppSession.BusinessPartnerName = entity.BusinessPartnerName;

                List<GetUserMenuAccess> lstMenu = BusinessLayer.GetUserMenuAccess(Constant.Module.FINANCE, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("ParentCode = '{0}'", OnGetMenuCode())).OrderBy(p => p.MenuIndex).ToList();
                GetUserMenuAccess menu = lstMenu.OrderBy(p => p.MenuIndex).FirstOrDefault();
                Response.Redirect(Page.ResolveUrl(menu.MenuUrl));
            }
        }
    }
}
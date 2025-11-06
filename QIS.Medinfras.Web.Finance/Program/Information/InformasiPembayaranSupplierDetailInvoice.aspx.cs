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

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class InformasiPembayaranSupplierDetailInvoice : BasePageTrx
    {
        protected int PageCount = 0;
        protected string filterExpressionSupplier = "";

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.INFORMASI_PEMBAYARAN_HUTANG_PER_DETAIL_INVOICE;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            filterExpressionSupplier = string.Format("GCBusinessPartnerType = '{0}' AND IsDeleted = 0", Constant.BusinessObjectType.SUPPLIER);

        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string[] param = e.Parameter.Split('|');
            string result = param[0] + "|";

            if (e.Parameter != null && e.Parameter != "")
            {
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

        protected string GetFilterExpression()
        {
            string filterExpression = "";

            if (txtSupplierCode.Text != "")
            {
                filterExpression += string.Format("BusinessPartnerCode = '{0}'", txtSupplierCode.Text);
            }

            string SearchBy = rblDataSource.Text;
            if (SearchBy == "1")
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression += string.Format("PurchaseInvoiceNo LIKE '%{0}%'", txtSearchNo.Text);
            }
            else if (SearchBy == "2")
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression += string.Format("PurchaseReceiveNo LIKE '%{0}%'", txtSearchNo.Text);
            }
            else if (SearchBy == "3")
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression += string.Format("ReferenceNo LIKE '%{0}%'", txtSearchNo.Text);
            }

            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvInformationSupplierPaymentHeaderRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_ITEM);
            }
            List<vInformationSupplierPaymentPerDetail> lstEntity = BusinessLayer.GetvInformationSupplierPaymentPerDetailList(filterExpression, Constant.GridViewPageSize.GRID_ITEM, pageIndex, "PurchaseInvoiceID, ID");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
    }
}
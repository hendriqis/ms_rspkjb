using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class APSupplierPaymentVerificationDtCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;

        public override void InitializeDataControl(string param)
        {
            hdnSupplierPaymentIDDtCtl.Value = param;

            string filterEntity = string.Format("SupplierPaymentID = {0}", hdnSupplierPaymentIDDtCtl.Value);
            vSupplierPaymentHd entity = BusinessLayer.GetvSupplierPaymentHdList(filterEntity).FirstOrDefault();
            txtSupplierPaymentNoDtCtl.Text = entity.SupplierPaymentNo;
            txtStampAmountDtCtl.Text = entity.StampAmount.ToString(Constant.FormatString.NUMERIC_2);
            txtPaymentAmountDtCtl.Text = entity.TotalPaymentAmount.ToString(Constant.FormatString.NUMERIC_2);

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("SupplierPaymentID = {0}", hdnSupplierPaymentIDDtCtl.Value);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvSupplierPaymentDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MATRIX);
            }

            List<vSupplierPaymentDt> lstSupplierPayment = BusinessLayer.GetvSupplierPaymentDtList(filterExpression, Constant.GridViewPageSize.GRID_MATRIX, pageIndex, "PurchaseInvoiceID ASC");
            lvwView.DataSource = lstSupplierPayment;
            lvwView.DataBind();
        }

        protected void cbpPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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
    }
}
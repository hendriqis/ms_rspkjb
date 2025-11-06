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
    public partial class ARInvoiceReceivingInformationPerReceiveCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;

        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                hdnParam.Value = param;
                txtARReceiveNo.Text = BusinessLayer.GetARReceivingHd(Convert.ToInt32(hdnParam.Value)).ARReceivingNo;
                BindGridView(1, true, ref PageCount);
            }
        }

        #region Binding Source
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("ARReceivingID = {0}", hdnParam.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvARReceivingPerARInvoiceRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MATRIX);
            }
            List<vARReceivingPerARInvoice> lstEntity = BusinessLayer.GetvARReceivingPerARInvoiceList(filterExpression, Constant.GridViewPageSize.GRID_MATRIX, pageIndex, "ARReceivingNo, AlocationDate, ARInvoiceNo");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpViewCtl_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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
            result += "|" + pageCount;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion
    }
}
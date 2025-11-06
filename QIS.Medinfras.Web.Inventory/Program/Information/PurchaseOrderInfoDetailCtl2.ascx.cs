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
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Inventory.Program;

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class PurchaseOrderInfoDetailCtl2 : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;

        private PurchaseReceiveInfoPerItem DetailPage
        {
            get { return (PurchaseReceiveInfoPerItem)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            String[] lstParam = param.Split('|');
            //hdnLocationID.Value = lstParam[0];
            hdnPurchaseReceiveID.Value = lstParam[2];
            //String[] lstDate = lstParam[1].Split(';');
            //hdnDateFrom.Value = Helper.GetDatePickerValue(lstDate[0]).ToString();
            //hdnDateTo.Value = Helper.GetDatePickerValue(lstDate[1]).ToString();

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {

            String filterExpression = string.Format("PurchaseReceiveID = {0}", hdnPurchaseReceiveID.Value);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPurchaseReceiveDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 10);
            }

            List<vPurchaseReceiveDt> lstPurchaseReceiveDt = BusinessLayer.GetvPurchaseReceiveDtList(filterExpression, 10, pageIndex);
            grdPopupView.DataSource = lstPurchaseReceiveDt;
            grdPopupView.DataBind();
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
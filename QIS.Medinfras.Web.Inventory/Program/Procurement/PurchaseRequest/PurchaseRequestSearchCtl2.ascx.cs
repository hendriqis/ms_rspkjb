using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class PurchaseRequestSearchCtl2 : BaseViewPopupCtl
    {
        private ApprovedPurchaseRequestDetail2 DetailPage
        {
            get { return (ApprovedPurchaseRequestDetail2)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            string[] temp = param.Split('|');

            hdnLocationParam.Value = temp[0];
            hdnProductLineIDCtl.Value = temp[1];
            hdnPurchaseOrderType.Value = temp[2];

            BindGridView();

        }

        protected void cbpProcessDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                BindGridView();
                result = "refresh";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private string GetFilterExpression()
        {
            string prDT = "";
            string filterDT = string.Format("GCItemDetailStatus = '{0}' AND OrderedQuantity < Quantity", Constant.TransactionStatus.APPROVED);
            List<PurchaseRequestDt> lstDt = BusinessLayer.GetPurchaseRequestDtList(filterDT);
            foreach (PurchaseRequestDt dt in lstDt)
            {
                prDT += dt.PurchaseRequestID + ",";
            }

            if (prDT != "")
            {
                prDT = prDT.Substring(0, prDT.Length - 1);
            }
            else {
                prDT = "0"; //jika kosong akan error saat manggil view, karna PurchaseRequestID IN ()
            }

            string filterExpression;
            if (hdnProductLineIDCtl.Value != "0")
            {
                filterExpression = string.Format(
                    "FromLocationID LIKE '%{0}%' AND GCTransactionStatus = '{1}' AND ProductLineID = {2} AND PurchaseRequestID IN ({3})",
                    hdnLocationParam.Value, Constant.TransactionStatus.APPROVED, hdnProductLineIDCtl.Value, prDT);
            }
            else
            {
                filterExpression = string.Format(
                    "FromLocationID LIKE '%{0}%' AND GCTransactionStatus = '{1}' AND ProductLineID IS NULL AND PurchaseRequestID IN ({2})",
                    hdnLocationParam.Value, Constant.TransactionStatus.APPROVED, prDT);
            }

            if (hdnPurchaseOrderType.Value != "null" && hdnPurchaseOrderType.Value != "")
            {
                filterExpression += string.Format(" AND GCPurchaseOrderType = '{0}'", hdnPurchaseOrderType.Value);
            }

            return filterExpression;
        }

        private void BindGridView()
        {
            string filterExpression = GetFilterExpression();

            List<vPurchaseRequestHd> lstEntity = BusinessLayer.GetvPurchaseRequestHdList(filterExpression, int.MaxValue, 1, "PurchaseRequestNo ASC");
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }
    }
}
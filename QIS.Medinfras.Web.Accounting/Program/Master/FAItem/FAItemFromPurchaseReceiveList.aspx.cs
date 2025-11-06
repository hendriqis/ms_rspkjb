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

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class FAItemFromPurchaseReceiveList : BasePageList
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.FA_ITEM_FROM_PURCHASE_RECEIVE;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            txtReceivedDateFrom.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtReceivedDateTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            BindGridView();
        }

        private void BindGridView()
        {
            string oReceivedDate = string.Format("{0}|{1}",
                                        Helper.GetDatePickerValue(txtReceivedDateFrom.Text).ToString(Constant.FormatString.DATE_FORMAT_112),
                                        Helper.GetDatePickerValue(txtReceivedDateTo.Text).ToString(Constant.FormatString.DATE_FORMAT_112));
            int oBusinessPartnerID = (hdnSupplierID.Value != "" && hdnSupplierID.Value != "0") ? Convert.ToInt32(hdnSupplierID.Value) : 0;
            int oItemID = (hdnItemID.Value != "" && hdnItemID.Value != "0") ? Convert.ToInt32(hdnItemID.Value) : 0;
            int oProductLineID = (hdnProductLineID.Value != "" && hdnProductLineID.Value != "0") ? Convert.ToInt32(hdnProductLineID.Value) : 0;

            List<GetPurchaseReceiveDtFixedAsset> lstEntity = BusinessLayer.GetPurchaseReceiveDtFixedAssetList(oReceivedDate, oBusinessPartnerID, oItemID, oProductLineID);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                string[] oParam = hdnID.Value.Split('|');
                url = ResolveUrl(string.Format("~/Program/Master/FAItem/FAItemEntry.aspx?id={0}|{1}", oParam[0], oParam[1]));

                return true;
            }
            return false;
        }
    }
}
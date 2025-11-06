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
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Inventory.Program.Information
{
    public partial class OutstandingPurchaseReceiveCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                string[] temp = param.Split('|');
                hdnItemID.Value = temp[0];
                hdnFromLocationID.Value = temp[1];

                string[] date = temp[2].Split(';');
                hdnStartDate.Value = date[0];
                hdnEndDate.Value = date[1];

                BindGridView();
            }
        }

        private void BindGridView()
        {
            String startDate = Helper.GetDatePickerValue(hdnStartDate.Value).ToString("yyyy-MM-dd");
            String EndDate = Helper.GetDatePickerValue(hdnEndDate.Value).ToString("yyyy-MM-dd");
            string filterExpression = "";
            filterExpression = string.Format("ReceivedDate BETWEEN '{0}' AND '{1}' AND ItemID = {2}", startDate, EndDate, hdnItemID.Value);
            if (hdnFromLocationID.Value != "")
            {
                filterExpression += string.Format(" AND LocationID = {0}", hdnFromLocationID.Value);
            }
            vOutstandingPurchaseReceive purchaseReceiveID = BusinessLayer.GetvOutstandingPurchaseReceiveList(filterExpression).FirstOrDefault();

            grdView.DataSource = BusinessLayer.GetvPurchaseReceiveDtList(string.Format("PurchaseReceiveID IN ({0}) AND ItemID = {1}", purchaseReceiveID.PurchaseReceiveID, hdnItemID.Value));
            grdView.DataBind();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class LocationItemExpiredDateInfoCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        public override void InitializeDataControl(string param)
        {
            String[] lstParam = param.Split('|');
            hdnID.Value = lstParam[0];
            hdnLocationID.Value = lstParam[1];
            hdnItemID.Value = lstParam[2];

            BindGridView();
            BindGridView2();
        }

        protected void cbpPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = "1 = 0";
            if (hdnID.Value != "")
                filterExpression = string.Format("ID = {0}", hdnID.Value);

            List<ItemBalanceDtExpired> lstEntity = BusinessLayer.GetItemBalanceDtExpiredList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpPopupView2_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView2();
        }

        private void BindGridView2()
        {
            int count = Convert.ToInt32(txtLastPOR.Text);
            int itemID = Convert.ToInt32(hdnItemID.Value);
            int locationID = Convert.ToInt32(hdnLocationID.Value);

            List<GetPurchaseReceiveDtExpired> lstEntity = BusinessLayer.GetPurchaseReceiveDtExpiredList(count, itemID, locationID);
            grdView2.DataSource = lstEntity;
            grdView2.DataBind();
        }
    }
}
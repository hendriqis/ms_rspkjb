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

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class PurchaseReceiveExpiredDtCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnID.Value = param;
            BindGridView();
        }

        protected override void OnLoad(EventArgs e)
        {
            //base.OnLoad(e);
            //if (grdView.Rows.Count < 1)
            //    BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = "1 = 0";
            if (hdnID.Value != "")
                filterExpression = string.Format("ID = {0}", hdnID.Value);
            
            List<PurchaseReceiveDtExpired> lstEntity = BusinessLayer.GetPurchaseReceiveDtExpiredList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }
    }
}
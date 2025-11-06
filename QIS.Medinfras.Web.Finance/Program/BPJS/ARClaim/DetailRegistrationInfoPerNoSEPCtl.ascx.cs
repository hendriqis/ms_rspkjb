using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class DetailRegistrationInfoPerNoSEPCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {
            hdnNoSEPCtl.Value = param;

            txtNoSEP.Text = param;

            BindGridView();
        }

        #region Bind Grid
        private void BindGridView()
        {
            string filter = string.Format("NoSEP = '{0}'", txtNoSEP.Text);
            List<vRegistrationPaymentPerNoSEPDetail> lstEntity = BusinessLayer.GetvRegistrationPaymentPerNoSEPDetailList(filter);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vRegistrationPaymentPerNoSEPDetail entity = e.Item.DataItem as vRegistrationPaymentPerNoSEPDetail;

            }
        }
        #endregion

        protected void cbpProcessDetailInfo_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string param = e.Parameter;
            string result = param + "|";
            string retval = "";

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = retval;
        }
    }
}
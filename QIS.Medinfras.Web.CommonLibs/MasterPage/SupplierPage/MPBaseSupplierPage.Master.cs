using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Web.UI.HtmlControls;
using QIS.Medinfras.Web.CommonLibs.Controls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class MPBaseSupplierPage : BaseMP
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!Page.IsPostBack)
            {
                if (AppSession.UserLogin == null)
                    Response.Redirect("~/../SystemSetup/Login.aspx");

                vAPInvoiceSupplierForBanner entity = BusinessLayer.GetvAPInvoiceSupplierForBannerList(string.Format("BusinessPartnerID = {0}", AppSession.BusinessPartnerID))[0];
                EntityToControl(entity);
            }
        }

        private void EntityToControl(vAPInvoiceSupplierForBanner entity)
        {
            ((SupplierBannerCtl)ctlSupplierBanner).InitializeSupplierBanner(entity);
        }
    }
}
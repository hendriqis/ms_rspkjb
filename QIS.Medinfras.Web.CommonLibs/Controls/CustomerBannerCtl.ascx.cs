using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class CustomerBannerCtl : BaseUserControlCtl
    {
        public void InitializeCustomerBanner(vCustomer entityC)
        {
            string imagePath = string.Format("{0}{1}", AppConfigManager.QISVirtualDirectory, AppConfigManager.QISBusinessPartnerLogoPath).Replace("#BusinessPartnerCode", entityC.BusinessPartnerCode);
            imgBusinessPartnerLogo.Src = string.Format(@"{0}\{1}", imagePath, entityC.LogoFileName);

            lblCustomerCode.InnerHtml = entityC.BusinessPartnerCode;
            lblCustomerName.InnerHtml = entityC.BusinessPartnerName;
            lblCustomerGroupCode.InnerHtml = entityC.CustomerGroupCode;
            lblCustomerGroupName.InnerHtml = entityC.CustomerGroupName;
            lblCustomerType.InnerHtml = entityC.CustomerType;
            lblCustomerLineName.InnerHtml = entityC.CustomerLineName;
            lblTariffScheme.InnerHtml = entityC.TariffScheme;
            lblBillToAddressInfo.InnerHtml = string.Format("{0} {1}", entityC.BusinessPartnersBillingAddress, entityC.BusinessPartnersBillingZipCode);
            lblBillToPhone.InnerHtml = entityC.BusinessPartnersBillingBillingPhoneNo1;
            lblBillToFax.InnerHtml = entityC.BusinessPartnersBillingBillingFaxNo1;
            lblBillToEmail.InnerHtml = entityC.BusinessPartnersBillingBillingEmail;
        }
    }
}
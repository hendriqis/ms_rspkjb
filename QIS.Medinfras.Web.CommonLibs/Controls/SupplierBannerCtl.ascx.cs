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
    public partial class SupplierBannerCtl : BaseUserControlCtl
    {
        public void InitializeSupplierBanner(vAPInvoiceSupplierForBanner entity)
        {
            string imagePath = string.Format("{0}{1}", AppConfigManager.QISVirtualDirectory, AppConfigManager.QISBusinessPartnerLogoPath).Replace("#BusinessPartnerCode", entity.BusinessPartnerCode);
            imgBusinessPartnerLogo.Src = string.Format(@"{0}\{1}", imagePath, entity.LogoFileName);

            lblBusinessPartnerCode.InnerHtml = entity.BusinessPartnerCode;
            lblBusinessPartnerName.InnerHtml = entity.BusinessPartnerName;
            lblAddress.InnerHtml = entity.cfAddress;
            lblPhoneNo.InnerHtml = entity.cfPhoneNo;
            lblEmail.InnerHtml = entity.Email;
            lblContactPerson.InnerHtml = entity.ContactPerson;
            lblContactPersonMobileNo.InnerHtml = entity.ContactPersonMobileNo;
            lblContactPersonEmail.InnerHtml = entity.ContactPersonEmail;
            lblPartnerType.InnerHtml = entity.cfPartnerMasterType;
            lblSupplierLine.InnerHtml = entity.SupplierLineName;
        }
    }
}
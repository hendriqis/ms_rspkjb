using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class PurchaseOrderSupplierInfoCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;

        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                BusinessPartners entity = BusinessLayer.GetBusinessPartners(Convert.ToInt32(param));
                vAddress entityAddress = BusinessLayer.GetvAddressList(String.Format("AddressID = {0}", entity.AddressID)).FirstOrDefault();
                EntityToControl(entity,entityAddress);
            }
        }

        private void EntityToControl(BusinessPartners entity, vAddress entityAddress)
        {
            #region General Information
            txtSupplierCode.Text = entity.BusinessPartnerCode;
            txtSupplierName.Text = entity.BusinessPartnerName;
            #endregion

            #region Contact Person
            txtContactPersonName.Text = entity.ContactPerson;
            txtContactPersonPhoneNumber.Text = entity.ContactPersonMobileNo;
            txtContactPersonEmail.Text = entity.ContactPersonEmail;
            #endregion

            #region Address
            txtAddress.Text = entityAddress.StreetName;
            txtCounty.Text = entityAddress.County; // Desa
            txtDistrict.Text = entityAddress.District; //Kabupaten
            txtCity.Text = entityAddress.City;
            if (entityAddress.GCState != "")
                txtProvinceCode.Text = entityAddress.GCState.Split('^')[1];
            else
                txtProvinceCode.Text = "";
            txtProvinceName.Text = entityAddress.State;
            txtZipCode.Text = entityAddress.ZipCode;
            txtTelephoneNo.Text = entityAddress.PhoneNo1;
            txtFaxNo.Text = entityAddress.FaxNo1;
            txtEmail.Text = entityAddress.Email;
            #endregion
        }
    }
}
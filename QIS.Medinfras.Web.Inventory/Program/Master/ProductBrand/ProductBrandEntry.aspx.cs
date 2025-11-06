using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class ProductBrandEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.PRODUCT_BRAND;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                vProductBrand entity = BusinessLayer.GetvProductBrandList(string.Format("ProductBrandID = {0}",ID))[0];
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
            }
            txtProductBrandCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtProductBrandCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtProductBrandName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(hdnManufacturerID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtManufacturerCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtManufacturerName, new ControlEntrySetting(false, false, true));
        }

        private void EntityToControl(vProductBrand entity)
        {
            txtProductBrandCode.Text = entity.ProductBrandCode;
            txtProductBrandName.Text = entity.ProductBrandName;
            hdnManufacturerID.Value = entity.ManufacturerID.ToString();
            txtManufacturerCode.Text = entity.ManufacturerCode;
            txtManufacturerName.Text = entity.ManufacturerName;
        }

        private void ControlToEntity(ProductBrand entity)
        {
            entity.ProductBrandName = txtProductBrandName.Text;
            entity.ManufacturerID = Convert.ToInt32(hdnManufacturerID.Value);
        }

        //protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        //{
        //    errMessage = string.Empty;

        //    string FilterExpression = string.Format("ProductBrandCode = '{0}'", txtProductBrandCode.Text);
        //    List<ProductBrand> lst = BusinessLayer.GetProductBrandList(FilterExpression);

        //    if (lst.Count > 0)
        //        errMessage = " Product Brand with Code " + txtProductBrandCode.Text + " is already exist!";

        //    return (errMessage == string.Empty);
        //}

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("ProductBrandCode = '{0}' AND ProductBrandID != {1}", txtProductBrandCode.Text, hdnID.Value);
            List<ProductBrand> lst = BusinessLayer.GetProductBrandList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Product Brand Class with Code " + txtProductBrandCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            ProductBrandDao entityDao = new ProductBrandDao(ctx);
            bool result = false;
            try
            {
                ProductBrand entity = new ProductBrand();
                ControlToEntity(entity);
                entity.ProductBrandCode = Helper.GenerateProductBrandCode(ctx, entity.ProductBrandName);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetProductBrandMaxID(ctx).ToString();
                ctx.CommitTransaction();
                result = true;
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                result = false;
                errMessage = ex.Message;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                ProductBrand entity = BusinessLayer.GetProductBrand(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateProductBrand(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
    }
}
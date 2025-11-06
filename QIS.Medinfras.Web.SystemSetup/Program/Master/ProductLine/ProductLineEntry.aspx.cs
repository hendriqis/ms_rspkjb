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

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class ProductLineEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.PRODUCT_LINE;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                ProductLine entity = BusinessLayer.GetProductLine(Convert.ToInt32(ID));
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
            }
            txtProductLineCode.Focus();
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtProductLineCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtProductLineName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtInventory, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCOGS, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtPurchase, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtPurchaseDiscount, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtPurchaseReturn, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtSales, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtSalesReturn, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtSalesDiscount, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(ProductLine entity)
        {
            txtProductLineCode.Text = entity.ProductLineCode;
            txtProductLineName.Text = entity.ProductLineName;
            //txtInventory.Text = entity.Inventory;
            //txtCOGS.Text = entity.COGS;
            //txtPurchase.Text = entity.Purchase;
            //txtPurchaseDiscount.Text = entity.PurchaseDiscount;
            //txtPurchaseReturn.Text = entity.PurchaseReturn;
            //txtSales.Text = entity.Sales;
            //txtSalesReturn.Text = entity.SalesReturn;
            //txtSalesDiscount.Text = entity.SalesDiscount;
        }

        private void ControlToEntity(ProductLine entity)
        {
            entity.ProductLineCode = txtProductLineCode.Text;
            entity.ProductLineName = txtProductLineName.Text;
            //entity.Inventory = txtInventory.Text;
            //entity.COGS = txtCOGS.Text;
            //entity.Purchase = txtPurchase.Text;
            //entity.PurchaseDiscount = txtPurchaseDiscount.Text;
            //entity.PurchaseReturn = txtPurchaseReturn.Text;
            //entity.Sales = txtSales.Text;
            //entity.SalesReturn = txtSalesReturn.Text;
            //entity.SalesDiscount = txtSalesDiscount.Text;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("ProductLineCode = '{0}'", txtProductLineCode.Text);
            List<ProductLine> lst = BusinessLayer.GetProductLineList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Product Line with Code " + txtProductLineCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("ProductLineCode = '{0}' AND ProductLineID != {1}", txtProductLineCode.Text, hdnID.Value);
            List<ProductLine> lst = BusinessLayer.GetProductLineList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Product Line Class with Code " + txtProductLineCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            ProductLineDao entityDao = new ProductLineDao(ctx);
            bool result = false;
            try
            {
                ProductLine entity = new ProductLine();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetProductLineMaxID(ctx).ToString();
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
                ProductLine entity = BusinessLayer.GetProductLine(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateProductLine(entity);
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
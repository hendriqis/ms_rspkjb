using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ProductLineEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.PRODUCT_LINE;
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
                int ID = Convert.ToInt32(Request.QueryString["id"]);
                hdnID.Value = ID.ToString();
                ProductLine entity = BusinessLayer.GetProductLine(ID);
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
            }

            SetControlProperties();
            txtProductLineCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void SetControlProperties()
        {
            String filterExpression = string.Format("ParentID IN ('{0}')", Constant.StandardCode.ITEM_TYPE);
            List<StandardCode> lstScItemUnit = BusinessLayer.GetStandardCodeList(filterExpression);
            lstScItemUnit.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboGCItemType, lstScItemUnit, "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtProductLineCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtProductLineName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboGCItemType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(chkIsInventoryItem, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsFixedAsset, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsConsigmentItem, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(ProductLine entity)
        {
            txtProductLineCode.Text = entity.ProductLineCode;
            txtProductLineName.Text = entity.ProductLineName;
            if (entity.GCItemType != null)
            {
                cboGCItemType.Value = entity.GCItemType;
            }

            chkIsInventoryItem.Checked = entity.IsInventoryItem;
            chkIsFixedAsset.Checked = entity.IsFixedAsset;
            chkIsConsigmentItem.Checked = entity.IsConsigmentItem;
        }

        private void ControlToEntity(ProductLine entity)
        {
            entity.ProductLineCode = txtProductLineCode.Text;
            entity.ProductLineName = txtProductLineName.Text;
            if (cboGCItemType.Value == null)
            {
                entity.GCItemType = "";
            }
            else
            {
                entity.GCItemType = cboGCItemType.Value.ToString();
            }

            entity.IsInventoryItem = chkIsInventoryItem.Checked;
            entity.IsFixedAsset = chkIsFixedAsset.Checked;
            entity.IsConsigmentItem = chkIsConsigmentItem.Checked;
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
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
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
                Helper.InsertErrorLog(ex);
                return false;
            }
        }
    }
}
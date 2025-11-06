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

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class ItemMasterRevenueSharingEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.ITEM_MASTER_REVENUE_SHARING;
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

                vItemMasterRevenueSharing entity = BusinessLayer.GetvItemMasterRevenueSharingList(string.Format("ID = {0}", ID)).FirstOrDefault();

                SetControlProperties();
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void SetControlProperties()
        {
            String filterExpression = string.Format("ParentID IN ('{0}','{1}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.ITEM_TYPE, Constant.StandardCode.CUSTOMER_TYPE);
            List<StandardCode> listSC = BusinessLayer.GetStandardCodeList(filterExpression);

            Methods.SetComboBoxField<StandardCode>(cboItemType, listSC.Where(a => a.ParentID == Constant.StandardCode.ITEM_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            cboItemType.SelectedIndex = 0;

            Methods.SetComboBoxField<StandardCode>(cboCustomerType, listSC.Where(a => a.ParentID == Constant.StandardCode.CUSTOMER_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            cboCustomerType.Value = Constant.CustomerType.PERSONAL;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnItemID, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtItemCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtItemName, new ControlEntrySetting(false, false, true));

            SetControlEntrySetting(cboCustomerType, new ControlEntrySetting(true, true, true));

            SetControlEntrySetting(hdnRevenueSharingID, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRevenueSharingCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRevenueSharingName, new ControlEntrySetting(false, false, true));
        }

        private void EntityToControl(vItemMasterRevenueSharing entity)
        {
            hdnItemID.Value = entity.ItemID.ToString();
            txtItemCode.Text = entity.ItemCode;
            txtItemName.Text = entity.ItemName1;

            cboCustomerType.Value = entity.GCCustomerType;

            hdnRevenueSharingID.Value = entity.RevenueSharingID.ToString();
            txtRevenueSharingCode.Text = entity.RevenueSharingCode;
            txtRevenueSharingName.Text = entity.RevenueSharingName;
        }

        private void ControlToEntity(ItemMasterRevenueSharing entity)
        {
            entity.ItemID = Convert.ToInt32(hdnItemID.Value);
            entity.RevenueSharingID = Convert.ToInt32(hdnRevenueSharingID.Value);
            entity.GCCustomerType = cboCustomerType.Value.ToString();
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("ItemID = {0} AND GCCustomerType = '{1}' AND IsDeleted = 0", hdnItemID.Value, hdnRevenueSharingID.Value);
            List<ItemMasterRevenueSharing> lst = BusinessLayer.GetItemMasterRevenueSharingList(FilterExpression);

            StandardCode entitySC = BusinessLayer.GetStandardCode(cboCustomerType.Value.ToString());

            if (lst.Count > 0)
            {
                errMessage = "Mapping formula honor dokter dari item <b>" + txtItemName.Text + "</b> dan tipe pembayar <b>" + entitySC.StandardCodeName + "</b> sudah tersedia.";
            }

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("ItemID = {0} AND GCCustomerType = '{1}' AND IsDeleted = 0", hdnItemID.Value, hdnRevenueSharingID.Value);
            List<ItemMasterRevenueSharing> lst = BusinessLayer.GetItemMasterRevenueSharingList(FilterExpression);

            StandardCode entitySC = BusinessLayer.GetStandardCode(cboCustomerType.Value.ToString());

            if (lst.Count > 0)
            {
                errMessage = "Mapping formula honor dokter dari item <b>" + txtItemName.Text + "</b> dan tipe pembayar <b>" + entitySC.StandardCodeName + "</b> sudah tersedia.";
            }

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ItemMasterRevenueSharingDao entityDao = new ItemMasterRevenueSharingDao(ctx);
            try
            {
                ItemMasterRevenueSharing entity = new ItemMasterRevenueSharing();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                retval = entityDao.InsertReturnPrimaryKeyID(entity).ToString();

                ctx.CommitTransaction();
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
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ItemMasterRevenueSharingDao entityDao = new ItemMasterRevenueSharingDao(ctx);
            try
            {
                ItemMasterRevenueSharing entity = new ItemMasterRevenueSharing();
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);

                ctx.CommitTransaction();
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
    }
}
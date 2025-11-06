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
    public partial class ItemGroupMasterEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.ITEM_GROUP_MASTER;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                SetControlProperties();
                vItemGroupMaster entity = BusinessLayer.GetvItemGroupMasterList(string.Format("ItemGroupID = {0}", ID))[0];
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            txtRevenueSharingCode.Focus();
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.ITEM_TYPE));
            Methods.SetComboBoxField<StandardCode>(cboItemType, lstSc, "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtItemGroupCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtItemGroupName1, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtItemGroupName2, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboItemType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtPrintOrder, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(hdnRevenueSharingID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtRevenueSharingCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtRevenueSharingName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(hdnParentID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtParentCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtParentName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(chkIsHeader, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(txtCITOAmount, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(chkIsCITOInPercentage, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtComplicationAmount, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(chkIsComplicationInPercentage, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(vItemGroupMaster entity)
        {
            txtItemGroupCode.Text = entity.ItemGroupCode;
            txtItemGroupName1.Text = entity.ItemGroupName1;
            txtItemGroupName2.Text = entity.ItemGroupName2;
            cboItemType.Value = entity.GCItemType;
            txtPrintOrder.Text = entity.PrintOrder.ToString();
            hdnRevenueSharingID.Value = entity.RevenueSharingID.ToString();
            txtRevenueSharingCode.Text = entity.RevenueSharingCode;
            txtRevenueSharingName.Text = entity.RevenueSharingName;
            hdnParentID.Value = entity.ParentID.ToString();
            txtParentCode.Text = entity.ParentCode;
            txtParentName.Text = entity.ParentName;
            chkIsHeader.Checked = entity.IsHeader;

            txtCITOAmount.Text = entity.CitoAmount.ToString();
            chkIsCITOInPercentage.Checked = entity.IsCitoInPercentage;
            txtComplicationAmount.Text = entity.ComplicationAmount.ToString();
            chkIsComplicationInPercentage.Checked = entity.IsComplicationInPercentage;
        }

        private void ControlToEntity(ItemGroupMaster entity)
        {
            entity.ItemGroupCode = txtItemGroupCode.Text;
            entity.ItemGroupName1 = txtItemGroupName1.Text;
            entity.ItemGroupName2 = txtItemGroupName2.Text;
            entity.GCItemType = cboItemType.Value.ToString();
            entity.PrintOrder = Convert.ToInt16(txtPrintOrder.Text);
            if (hdnRevenueSharingID.Value == "" || hdnRevenueSharingID.Value == "0")
                entity.RevenueSharingID = null;
            else
                entity.RevenueSharingID = Convert.ToInt32(hdnRevenueSharingID.Value);
            if (hdnParentID.Value == "" || hdnParentID.Value == "0")
                entity.ParentID = null;
            else
                entity.ParentID = Convert.ToInt32(hdnParentID.Value);
            entity.IsHeader = chkIsHeader.Checked;

            entity.CitoAmount = Convert.ToDecimal(txtCITOAmount.Text);
            entity.IsCitoInPercentage = chkIsCITOInPercentage.Checked;
            entity.ComplicationAmount = Convert.ToDecimal(txtComplicationAmount.Text);
            entity.IsComplicationInPercentage = chkIsComplicationInPercentage.Checked;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("ItemGroupCode = '{0}'", txtItemGroupCode.Text);
            List<ItemGroupMaster> lst = BusinessLayer.GetItemGroupMasterList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Item Group with Code " + txtItemGroupCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("ItemGroupCode = '{0}' AND ItemGroupID != {1}", txtItemGroupCode.Text, hdnID.Value);
            List<ItemGroupMaster> lst = BusinessLayer.GetItemGroupMasterList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Item Group with Code " + txtItemGroupCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            ItemGroupMasterDao entityDao = new ItemGroupMasterDao(ctx);
            bool result = false;
            try
            {
                ItemGroupMaster entity = new ItemGroupMaster();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetItemGroupMasterMaxID(ctx).ToString();
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
                ItemGroupMaster entity = BusinessLayer.GetItemGroupMaster(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateItemGroupMaster(entity);
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
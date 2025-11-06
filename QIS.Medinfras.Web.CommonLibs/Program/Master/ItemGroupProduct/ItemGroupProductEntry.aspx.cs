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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ItemGroupProductEntry : BasePageEntry
    {
        private String filterExpress = "";
        public override string OnGetMenuCode()
        {
            String TypeItem = hdnTypeItem.Value;
            switch (TypeItem)
            {
                case Constant.ItemGroupMaster.DIAGNOSTIC: return Constant.MenuCode.MedicalDiagnostic.ITEM_GROUP;
                case Constant.ItemGroupMaster.SERVICE: return Constant.MenuCode.Finance.ITEM_GROUP;
                case Constant.ItemGroupMaster.RADIOLOGY: return Constant.MenuCode.Imaging.ITEM_GROUP;
                case Constant.ItemGroupMaster.LABORATORY: return Constant.MenuCode.Laboratory.ITEM_GROUP;
                case Constant.ItemGroupMaster.MEDICAL_CHECKUP: return Constant.MenuCode.MedicalCheckup.ITEM_GROUP;
                case Constant.ItemGroupMaster.NUTRITION: return Constant.MenuCode.Nutrition.ITEM_GROUP;
                default: return Constant.MenuCode.Inventory.ITEM_GROUP;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected string OnGetParentFilterExpression()
        {
            string filterExpression = "";
            switch (hdnTypeItem.Value)
            {
                //case Constant.ItemGroup.DIAGNOSTIC: { filterExpression = string.Format("GCItemType = '{0}'", Constant.ItemGroupMaster.DIAGNOSTIC); break; }
                //case Constant.ItemGroup.FINANCE: filterExpression = string.Format("GCItemType = '{0}'", Constant.ItemGroupMaster.SERVICE); break;
                //case Constant.ItemGroup.IMAGING: { filterExpression = string.Format("GCItemType = '{0}'", Constant.ItemGroupMaster.RADIOLOGY); break; }
                //case Constant.ItemGroup.LABORATORY: { filterExpression = string.Format("GCItemType = '{0}'", Constant.ItemGroupMaster.LABORATORY); break; }
                //case Constant.ItemGroup.MCU: filterExpression = string.Format("GCItemType = '{0}'", Constant.ItemGroupMaster.MEDICAL_CHECKUP); break;
                //case Constant.ItemGroup.NUTRIENT: filterExpression = string.Format("GCItemType = '{0}'", Constant.ItemGroupMaster.NUTRITION); break;
                //default: filterExpression = String.Format("GCItemType IN ('{0}','{1}','{2}')", Constant.ItemGroupMaster.DRUGS, Constant.ItemGroupMaster.SUPPLIES, Constant.ItemGroupMaster.LOGISTIC); break;

                case Constant.ItemGroupMaster.DIAGNOSTIC: { filterExpression = string.Format("GCItemType = '{0}'", Constant.ItemGroupMaster.DIAGNOSTIC); break; }
                case Constant.ItemGroupMaster.SERVICE: filterExpression = string.Format("GCItemType = '{0}'", Constant.ItemGroupMaster.SERVICE); break;
                case Constant.ItemGroupMaster.RADIOLOGY: { filterExpression = string.Format("GCItemType = '{0}'", Constant.ItemGroupMaster.RADIOLOGY); break; }
                case Constant.ItemGroupMaster.LABORATORY: { filterExpression = string.Format("GCItemType = '{0}'", Constant.ItemGroupMaster.LABORATORY); break; }
                case Constant.ItemGroupMaster.MEDICAL_CHECKUP: filterExpression = string.Format("GCItemType = '{0}'", Constant.ItemGroupMaster.MEDICAL_CHECKUP); break;
                case Constant.ItemGroupMaster.NUTRITION: filterExpression = string.Format("GCItemType = '{0}'", Constant.ItemGroupMaster.NUTRITION); break;
                default: filterExpression = String.Format("GCItemType IN ('{0}','{1}','{2}')", Constant.ItemGroupMaster.DRUGS, Constant.ItemGroupMaster.SUPPLIES, Constant.ItemGroupMaster.LOGISTIC); break;
            }
            filterExpression += " AND IsHeader = 1 AND IsDeleted = 0";
            return filterExpression;
        }

        protected override void InitializeDataControl()
        {
            String[] param = Request.QueryString["id"].Split('|');
            hdnTypeItem.Value = param[0];
            if (param.Length > 1)
            {
                IsAdd = false;
                hdnID.Value = param[1];
                SetControlProperties();
                vItemGroupMaster entity = BusinessLayer.GetvItemGroupMasterList(string.Format("ItemGroupID = {0}", hdnID.Value))[0];
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            txtRevenueSharingCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void SetControlProperties()
        {
            switch (hdnTypeItem.Value)
            {
                case Constant.ItemGroupMaster.DIAGNOSTIC: { filterExpress = ""; hdnItemGroup.Value = Constant.ItemGroupMaster.DIAGNOSTIC; break; }
                case Constant.ItemGroupMaster.SERVICE: filterExpress = ""; hdnItemGroup.Value = Constant.ItemGroupMaster.SERVICE; break;
                case Constant.ItemGroupMaster.RADIOLOGY: { filterExpress = ""; hdnItemGroup.Value = Constant.ItemGroupMaster.RADIOLOGY; break; }
                case Constant.ItemGroupMaster.LABORATORY: { filterExpress = ""; hdnItemGroup.Value = Constant.ItemGroupMaster.LABORATORY; break; }
                case Constant.ItemGroupMaster.MEDICAL_CHECKUP: filterExpress = ""; hdnItemGroup.Value = Constant.ItemGroupMaster.MEDICAL_CHECKUP; break;
                case Constant.ItemGroupMaster.NUTRITION: filterExpress = ""; hdnItemGroup.Value = Constant.ItemGroupMaster.NUTRITION; break;
                default: filterExpress = String.Format("StandardCodeID IN ('{0}','{1}','{2}')", Constant.ItemGroupMaster.DRUGS, Constant.ItemGroupMaster.SUPPLIES, Constant.ItemGroupMaster.LOGISTIC); break;
            }

            if (filterExpress == "") trItemtype.Style.Add("display", "none");
            else
            {
                List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("{0} AND IsDeleted = 0", filterExpress));
                Methods.SetComboBoxField<StandardCode>(cboItemType, lstSc, "StandardCodeName", "StandardCodeID");
            }
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
            if (hdnItemGroup.Value == "") entity.GCItemType = cboItemType.Value.ToString();
            else entity.GCItemType = hdnItemGroup.Value;
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
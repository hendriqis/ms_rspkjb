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
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class ItemLaboratoryEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.LABORATORY;
        }

        protected override void InitializeDataControl()
        {
            hdnGCItemType.Value = Constant.ItemGroupMaster.LABORATORY;
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;

                SetControlProperties();
                vItemMaster entity = BusinessLayer.GetvItemMasterList(string.Format("ItemID = {0}", ID))[0];
                vItemService entityService = BusinessLayer.GetvItemServiceList(string.Format("ItemID = {0}", ID))[0];
                ItemTagField entityTagField = BusinessLayer.GetItemTagField(entity.ItemID);
                ItemLaboratory entityLaboratory = BusinessLayer.GetItemLaboratory(entity.ItemID);
                EntityToControl(entity, entityLaboratory, entityService, entityTagField);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            txtItemCode.Focus();
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}') AND IsDeleted = 0", Constant.StandardCode.ITEM_UNIT, Constant.StandardCode.LABORATORY_TEST_CATEGORY));
            Methods.SetComboBoxField<StandardCode>(cboItemUnit, lstSc.Where(p => p.ParentID == Constant.StandardCode.ITEM_UNIT).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboLabTestCategory, lstSc.Where(p => p.ParentID == Constant.StandardCode.LABORATORY_TEST_CATEGORY).ToList(), "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtItemCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtItemName1, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtItemName2, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboItemUnit, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(hdnItemGroupID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtItemGroupCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtItemGroupName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(hdnProductLineID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtProductLineCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtProductLineName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(hdnRevenueSharingID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtRevenueSharingCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtRevenueSharingName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(cboLabTestCategory, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDuration, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtPreconditionNotes, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(chkIsAllowCITO, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsAllowDiscount, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsAllowVariable, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsIncludeInAdminCalculation, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsPrintWithDoctorName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsSubContractItem, new ControlEntrySetting(true, true, false));
        }

        protected override void OnReInitControl()
        {
            #region Custom Attribute
            foreach (RepeaterItem item in rptCustomAttribute.Items)
            {
                TextBox txt = (TextBox)item.FindControl("txtTagField");
                txt.Text = "";
            }
            #endregion
        }

        private void EntityToControl(vItemMaster entity, ItemLaboratory entityLaboratory, vItemService entityService, ItemTagField entityTagField)
        {
            txtItemCode.Text = entity.ItemCode;
            txtItemName1.Text = entity.ItemName1;
            txtItemName2.Text = entity.ItemName2;
            cboItemUnit.Value = entity.GCItemUnit;
            hdnItemGroupID.Value = entity.ItemGroupID.ToString();
            txtItemGroupCode.Text = entity.ItemGroupCode;
            txtItemGroupName.Text = entity.ItemGroupName1;
            hdnProductLineID.Value = entity.ProductLineID.ToString();
            txtProductLineCode.Text = entity.ProductLineCode;
            txtProductLineName.Text = entity.ProductLineName;
            hdnRevenueSharingID.Value = entityService.RevenueSharingID.ToString();
            txtRevenueSharingCode.Text = entityService.RevenueSharingCode;
            txtRevenueSharingName.Text = entityService.RevenueSharingName;
            txtRemarks.Text = entity.Remarks; 
            cboLabTestCategory.Value = entityLaboratory.GCLabTestCategory;
            txtDuration.Text = entityLaboratory.Duration.ToString();
            txtPreconditionNotes.Text = entityLaboratory.PreconditionNotes;

            chkIsAllowCITO.Checked = entityService.IsAllowCito;
            chkIsAllowDiscount.Checked = entityService.IsAllowDiscount;
            chkIsAllowVariable.Checked = entityService.IsAllowVariable;
            chkIsIncludeInAdminCalculation.Checked = entity.IsIncludeInAdminCalculation;
            chkIsPrintWithDoctorName.Checked = entityService.IsPrintWithDoctorName;
            chkIsSubContractItem.Checked = entityService.IsSubContractItem;

            #region Custom Attribute
            foreach (RepeaterItem item in rptCustomAttribute.Items)
            {
                TextBox txt = (TextBox)item.FindControl("txtTagField");
                HtmlInputHidden hdn = (HtmlInputHidden)item.FindControl("hdnTagFieldCode");
                txt.Text = entityTagField.GetType().GetProperty("TagField" + hdn.Value).GetValue(entityTagField, null).ToString();
            }
            #endregion
        }

        private void ControlToEntity(ItemMaster entity, ItemLaboratory entityLaboratory, ItemService entityService, ItemTagField entityTagField)
        {
            entity.ItemCode = txtItemCode.Text;
            entity.ItemName1 = txtItemName1.Text;
            entity.ItemName2 = txtItemName2.Text;
            entity.GCItemUnit = cboItemUnit.Value.ToString();
            entity.ItemGroupID = Convert.ToInt32(hdnItemGroupID.Value);
            entity.ProductLineID = Convert.ToInt32(hdnProductLineID.Value);
            if (hdnRevenueSharingID.Value == "" || hdnRevenueSharingID.Value == "0")
                entityService.RevenueSharingID = null;
            else
                entityService.RevenueSharingID = Convert.ToInt32(hdnRevenueSharingID.Value);
            entity.Remarks = txtRemarks.Text;
            entityLaboratory.GCLabTestCategory = cboLabTestCategory.Value.ToString();
            entityLaboratory.Duration = Convert.ToByte(txtDuration.Text);
            entityLaboratory.PreconditionNotes = txtPreconditionNotes.Text;

            entityService.IsAllowCito = chkIsAllowCITO.Checked;
            entityService.IsAllowDiscount = chkIsAllowDiscount.Checked;
            entityService.IsAllowVariable = chkIsAllowVariable.Checked;
            entity.IsIncludeInAdminCalculation = chkIsIncludeInAdminCalculation.Checked;
            entityService.IsPrintWithDoctorName = chkIsPrintWithDoctorName.Checked;
            entityService.IsSubContractItem = chkIsSubContractItem.Checked;

            #region Custom Attribute
            foreach (RepeaterItem item in rptCustomAttribute.Items)
            {
                TextBox txt = (TextBox)item.FindControl("txtTagField");
                HtmlInputHidden hdn = (HtmlInputHidden)item.FindControl("hdnTagFieldCode");
                entityTagField.GetType().GetProperty("TagField" + hdn.Value).SetValue(entityTagField, txt.Text, null);
            }
            #endregion
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("ItemCode = '{0}'", txtItemCode.Text);
            List<ItemMaster> lst = BusinessLayer.GetItemMasterList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Item with Code " + txtItemCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("ItemCode = '{0}' AND ItemID != {1}", txtItemCode.Text, hdnID.Value);
            List<ItemMaster> lst = BusinessLayer.GetItemMasterList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Item with Code " + txtItemCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ItemMasterDao entityDao = new ItemMasterDao(ctx);
            ItemServiceDao entityServiceDao = new ItemServiceDao(ctx);
            ItemLaboratoryDao entityLaboratoryDao = new ItemLaboratoryDao(ctx);
            ItemCostDao entityCostDao = new ItemCostDao(ctx);
            ItemTagFieldDao entityTagFieldDao = new ItemTagFieldDao(ctx);
            try
            {
                ItemMaster entity = new ItemMaster();
                ItemService entityService = new ItemService();
                ItemLaboratory entityLaboratory = new ItemLaboratory();
                ItemTagField entityTagField = new ItemTagField();
                ControlToEntity(entity, entityLaboratory, entityService, entityTagField);

                entity.GCItemType = hdnGCItemType.Value;

                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);

                entityService.ItemID = BusinessLayer.GetItemMasterMaxID(ctx);
                entityServiceDao.Insert(entityService);

                entityLaboratory.ItemID = entityService.ItemID;
                entityLaboratoryDao.Insert(entityLaboratory);

                entityTagField.ItemID = entityService.ItemID;
                entityTagFieldDao.Insert(entityTagField);

                List<Healthcare> lstHealthcare = BusinessLayer.GetHealthcareList("", ctx);
                foreach (Healthcare healthcare in lstHealthcare)
                {
                    ItemCost ic = new ItemCost();
                    ic.ItemID = entityService.ItemID;
                    ic.HealthcareID = healthcare.HealthcareID;
                    ic.CreatedBy = AppSession.UserLogin.UserID;
                    entityCostDao.Insert(ic);
                }
                retval = entityService.ItemID.ToString();
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
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
            ItemMasterDao entityDao = new ItemMasterDao(ctx);
            ItemServiceDao entityServiceDao = new ItemServiceDao(ctx);
            ItemLaboratoryDao entityLaboratoryDao = new ItemLaboratoryDao(ctx);
            ItemTagFieldDao entityTagFieldDao = new ItemTagFieldDao(ctx);
            try
            {
                ItemMaster entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                ItemService entityService = entityServiceDao.Get(entity.ItemID);
                ItemLaboratory entityLaboratory = entityLaboratoryDao.Get(entity.ItemID);
                ItemTagField entityTagField = entityTagFieldDao.Get(entity.ItemID);

                ControlToEntity(entity, entityLaboratory, entityService, entityTagField);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityService.LastUpdatedBy = AppSession.UserLogin.UserID;

                entityDao.Update(entity);
                entityServiceDao.Update(entityService);
                entityLaboratoryDao.Update(entityLaboratory);
                entityTagFieldDao.Update(entityTagField);

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (pnlCustomAttribute.Visible)
            {
                List<Variable> ListCustomAttribute = initListCustomAttribute();
                if (ListCustomAttribute.Count == 0)
                    pnlCustomAttribute.Visible = false;
                else
                {
                    rptCustomAttribute.DataSource = ListCustomAttribute;
                    rptCustomAttribute.DataBind();
                }
            }
        }

        private List<Variable> initListCustomAttribute()
        {
            List<Variable> ListCustomAttribute = new List<Variable>();
            TagField tagField = BusinessLayer.GetTagField(Constant.BusinessObjectType.ITEM);
            if (tagField != null)
            {
                if (tagField.TagField1 != "") { ListCustomAttribute.Add(new Variable { Code = "1", Value = tagField.TagField1 }); }
                if (tagField.TagField2 != "") { ListCustomAttribute.Add(new Variable { Code = "2", Value = tagField.TagField2 }); }
                if (tagField.TagField3 != "") { ListCustomAttribute.Add(new Variable { Code = "3", Value = tagField.TagField3 }); }
                if (tagField.TagField4 != "") { ListCustomAttribute.Add(new Variable { Code = "4", Value = tagField.TagField4 }); }
                if (tagField.TagField5 != "") { ListCustomAttribute.Add(new Variable { Code = "5", Value = tagField.TagField5 }); }
                if (tagField.TagField6 != "") { ListCustomAttribute.Add(new Variable { Code = "6", Value = tagField.TagField6 }); }
                if (tagField.TagField7 != "") { ListCustomAttribute.Add(new Variable { Code = "7", Value = tagField.TagField7 }); }
                if (tagField.TagField8 != "") { ListCustomAttribute.Add(new Variable { Code = "8", Value = tagField.TagField8 }); }
                if (tagField.TagField9 != "") { ListCustomAttribute.Add(new Variable { Code = "9", Value = tagField.TagField9 }); }
                if (tagField.TagField10 != "") { ListCustomAttribute.Add(new Variable { Code = "10", Value = tagField.TagField10 }); }
                if (tagField.TagField11 != "") { ListCustomAttribute.Add(new Variable { Code = "11", Value = tagField.TagField11 }); }
                if (tagField.TagField12 != "") { ListCustomAttribute.Add(new Variable { Code = "12", Value = tagField.TagField12 }); }
                if (tagField.TagField13 != "") { ListCustomAttribute.Add(new Variable { Code = "13", Value = tagField.TagField13 }); }
                if (tagField.TagField14 != "") { ListCustomAttribute.Add(new Variable { Code = "14", Value = tagField.TagField14 }); }
                if (tagField.TagField15 != "") { ListCustomAttribute.Add(new Variable { Code = "15", Value = tagField.TagField15 }); }
                if (tagField.TagField16 != "") { ListCustomAttribute.Add(new Variable { Code = "16", Value = tagField.TagField16 }); }
                if (tagField.TagField17 != "") { ListCustomAttribute.Add(new Variable { Code = "17", Value = tagField.TagField17 }); }
                if (tagField.TagField18 != "") { ListCustomAttribute.Add(new Variable { Code = "18", Value = tagField.TagField18 }); }
                if (tagField.TagField19 != "") { ListCustomAttribute.Add(new Variable { Code = "19", Value = tagField.TagField19 }); }
                if (tagField.TagField20 != "") { ListCustomAttribute.Add(new Variable { Code = "20", Value = tagField.TagField20 }); }
            }
            return ListCustomAttribute;
        }
    }
}
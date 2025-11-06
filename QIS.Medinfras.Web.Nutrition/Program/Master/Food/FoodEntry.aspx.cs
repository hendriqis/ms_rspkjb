using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.Nutrition.Program
{
    public partial class FoodEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Nutrition.FOOD;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            hdnGCItemType.Value = Constant.ItemGroupMaster.NUTRITION;

            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                vItemFood entity = BusinessLayer.GetvItemFoodList(string.Format("ItemID = {0}", ID))[0];
                ItemTagField entityTagField = BusinessLayer.GetItemTagField(Convert.ToInt32(ID));

                SetControlProperties();
                EntityToControl(entity, entityTagField);
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
            string filterExpression = string.Format("ParentID IN ('{0}','{1}') AND IsDeleted = 0", Constant.StandardCode.ITEM_UNIT, Constant.StandardCode.FOOD_GROUP);

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);

            Methods.SetComboBoxField<StandardCode>(cboItemUnit, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ITEM_UNIT && sc.TagProperty.ToString().Contains("URT")).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboItemAlternateUnit, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ITEM_UNIT && sc.TagProperty.ToString().Contains("URT")).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGCFoodGroup, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.FOOD_GROUP).ToList(), "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            #region Item Information
            SetControlEntrySetting(txtItemCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtItemName1, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtItemName2, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(hdnItemGroupID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtItemGroupCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtItemGroupName, new ControlEntrySetting(false, false, true));
            #endregion

            #region Inventory Information
            SetControlEntrySetting(cboItemUnit, new ControlEntrySetting(true, true, true));
            #endregion

            #region Other Information
            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false));
            #endregion

            #region Food
            SetControlEntrySetting(cboGCFoodGroup, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboItemAlternateUnit, new ControlEntrySetting(true, true, true));
            #endregion
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

        private void EntityToControl(vItemFood entity, ItemTagField entityTagField)
        {
            #region Item Information
            txtItemCode.Text = entity.ItemCode;
            txtItemName1.Text = entity.ItemName1;
            txtItemName2.Text = entity.ItemName2;
            hdnItemGroupID.Value = entity.ItemGroupID.ToString();
            txtItemGroupCode.Text = entity.ItemGroupCode;
            txtItemGroupName.Text = entity.ItemGroupName1;
            #endregion

            #region Inventory Information
            cboItemUnit.Value = entity.GCItemUnit;
            txtItemUnit1Name.Text = entity.ItemUnit;
            #endregion

            #region Other Information
            txtNotes.Text = entity.Remarks;
            #endregion

            #region Custom Attribute
            foreach (RepeaterItem item in rptCustomAttribute.Items)
            {
                TextBox txt = (TextBox)item.FindControl("txtTagField");
                HtmlInputHidden hdn = (HtmlInputHidden)item.FindControl("hdnTagFieldCode");
                txt.Text = entityTagField.GetType().GetProperty("TagField" + hdn.Value).GetValue(entityTagField, null).ToString();
            }
            #endregion

            #region Food
            decimal itemUnit1Qty = 1;
            decimal itemUnit2Qty = 1;
            if (!string.IsNullOrEmpty(entity.ConversionFactorLabel))
            {
                string[] factor = entity.ConversionFactorLabel.Split('|');
                itemUnit1Qty = Convert.ToDecimal(factor[0]);
                itemUnit2Qty = Convert.ToDecimal(factor[1]);
            }
            cboGCFoodGroup.Value = entity.GCFoodGroup;
            txtItemUnit1Quantity.Text = itemUnit1Qty.ToString();
            cboItemAlternateUnit.Value = entity.GCAlternateUnit;
            txtItemUnit2Quantity.Text = itemUnit2Qty.ToString();
            txtItemUnit2Name.Text = entity.AlternateUnit;
            #endregion
        }

        private void ControlToEntity(ItemMaster entity, ItemProduct entityProduct, ItemTagField entityTagField, Food entityFood)
        {
            #region Item Information
            entity.ItemName1 = txtItemName1.Text;
            entity.ItemName2 = txtItemName2.Text;
            entity.ItemGroupID = Convert.ToInt32(hdnItemGroupID.Value);
            #endregion

            #region Inventory Information
            entity.GCItemUnit = cboItemUnit.Value.ToString();
            #endregion

            #region Other Information
            entity.Remarks = txtNotes.Text;
            #endregion

            #region Custom Attribute
            foreach (RepeaterItem item in rptCustomAttribute.Items)
            {
                TextBox txt = (TextBox)item.FindControl("txtTagField");
                HtmlInputHidden hdn = (HtmlInputHidden)item.FindControl("hdnTagFieldCode");
                entityTagField.GetType().GetProperty("TagField" + hdn.Value).SetValue(entityTagField, txt.Text, null);
            }
            #endregion

            #region Food
            entityFood.GCFoodGroup = cboGCFoodGroup.Value.ToString();
            entityFood.GCAlternateUnit = cboItemAlternateUnit.Value.ToString();

            decimal itemunit1Qty = Convert.ToDecimal(txtItemUnit1Quantity.Text);
            decimal itemunit2Qty = Convert.ToDecimal(txtItemUnit2Quantity.Text);
            entityFood.ConversionFactor = Math.Round(itemunit2Qty / itemunit1Qty, 4);
            entityFood.ConversionFactorLabel = string.Format("{0}|{1}", txtItemUnit1Quantity.Text, txtItemUnit2Quantity.Text);
            #endregion
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ItemMasterDao entityDao = new ItemMasterDao(ctx);
            ItemProductDao entityProductDao = new ItemProductDao(ctx);
            DrugInfoDao entityDrugDao = new DrugInfoDao(ctx);
            ItemTagFieldDao entityTagFieldDao = new ItemTagFieldDao(ctx);
            FoodDao entityFoodDao = new FoodDao(ctx);
            ItemCostDao entityCostDao = new ItemCostDao(ctx);
            ItemPlanningDao entityPlanningDao = new ItemPlanningDao(ctx);
            try
            {
                ItemMaster entity = new ItemMaster();
                ItemProduct entityProduct = new ItemProduct();
                ItemTagField entityTagField = new ItemTagField();
                Food entityFood = new Food();
                ControlToEntity(entity, entityProduct, entityTagField, entityFood);
                entity.ItemCode = Helper.GenerateItemCode(ctx, entity.ItemName1);
                entity.GCItemType = hdnGCItemType.Value;

                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);

                entityProduct.ItemID = BusinessLayer.GetItemMasterMaxID(ctx);
                entityProductDao.Insert(entityProduct);

                entityTagField.ItemID = entityProduct.ItemID;
                entityTagFieldDao.Insert(entityTagField);

                entityFood.ItemID = entityProduct.ItemID;
                entityFoodDao.Insert(entityFood);

                List<Healthcare> lstHealthcare = BusinessLayer.GetHealthcareList("", ctx);
                foreach (Healthcare healthcare in lstHealthcare)
                {
                    ItemCost ic = new ItemCost();
                    ic.ItemID = entityProduct.ItemID;
                    ic.HealthcareID = healthcare.HealthcareID;
                    ic.CreatedBy = AppSession.UserLogin.UserID;
                    entityCostDao.Insert(ic);

                    ItemPlanning ip = new ItemPlanning();
                    ip.BusinessPartnerID = null;
                    ip.ItemID = entityProduct.ItemID;
                    ip.HealthcareID = healthcare.HealthcareID;
                    ip.IsUsingSupplierCatalog = false;
                    ip.IsUsingDynamicROP = false;
                    ip.BackwardDays = 0;
                    ip.ForwardDays = 0;
                    ip.CreatedBy = AppSession.UserLogin.UserID;
                    ip.IsPriceLastUpdatedBySystem = false;
                    entityPlanningDao.Insert(ip);
                }
                retval = entityProduct.ItemID.ToString();
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
            ItemMasterDao entityDao = new ItemMasterDao(ctx);
            ItemProductDao entityProductDao = new ItemProductDao(ctx);
            FoodDao entityFoodDao = new FoodDao(ctx);
            DrugInfoDao entityDrugDao = new DrugInfoDao(ctx);
            ItemTagFieldDao entityTagFieldDao = new ItemTagFieldDao(ctx);
            try
            {
                Int32 ItemID = Convert.ToInt32(hdnID.Value);
                ItemMaster entity = entityDao.Get(ItemID);
                ItemProduct entityProduct = entityProductDao.Get(ItemID);
                ItemTagField entityTagField = entityTagFieldDao.Get(ItemID);
                Food entityFood = entityFoodDao.Get(ItemID);
                ControlToEntity(entity, entityProduct, entityTagField, entityFood);

                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityProduct.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityTagField.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityFood.LastUpdatedBy = AppSession.UserLogin.UserID;

                entityDao.Update(entity);
                entityProductDao.Update(entityProduct);
                entityTagFieldDao.Update(entityTagField);
                entityFoodDao.Update(entityFood);

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
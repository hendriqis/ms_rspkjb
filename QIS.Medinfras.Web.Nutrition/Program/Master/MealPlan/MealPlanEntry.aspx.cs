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
    public partial class MealPlanEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Nutrition.MEAL_PLAN;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                MealPlan entity = BusinessLayer.GetMealPlan(Convert.ToInt32(ID));
                SetControlProperties();
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            txtMealPlanCode.Focus();
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format(
                                                                                    "ParentID IN ('{0}','{1}','{2}') AND IsActive = 1 AND IsDeleted = 0",
                                                                                    Constant.StandardCode.DIET_TYPE,
                                                                                    Constant.StandardCode.MEAL_FORM,
                                                                                    Constant.StandardCode.MEAL_PLAN_CATEGORY));
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboDietType, lstStandardCode.Where(x => x.ParentID == Constant.StandardCode.DIET_TYPE || x.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboMealForm, lstStandardCode.Where(x => x.ParentID == Constant.StandardCode.MEAL_FORM || x.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboMealPlanCategory, lstStandardCode.Where(x => x.ParentID == Constant.StandardCode.MEAL_PLAN_CATEGORY || x.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

            cboDietType.SelectedIndex = 0;
            cboMealForm.SelectedIndex = 0;
            cboMealPlanCategory.SelectedIndex = 0;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtMealPlanCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtMealPlanName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRemark, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(MealPlan entity)
        {
            txtMealPlanCode.Text = entity.MealPlanCode;
            txtMealPlanName.Text = entity.MealPlanName;
            if (!string.IsNullOrEmpty(entity.GCDietType))
                cboDietType.Value = entity.GCDietType;
            if (!string.IsNullOrEmpty(entity.GCMealForm))
                cboMealForm.Value = entity.GCMealForm;
            txtRemark.Text = entity.Remarks;
            chkIsStapleFood.Checked = entity.IsStapleFood;
            if (!string.IsNullOrEmpty(entity.GCMealPlanCategory))
            {
                cboMealPlanCategory.Value = entity.GCDietType;
            }
        }

        private void ControlToEntity(MealPlan entity)
        {
            entity.MealPlanName = txtMealPlanName.Text;
            if (cboDietType.Value != null && cboDietType.Value.ToString() != "")
                entity.GCDietType = cboDietType.Value.ToString();
            if (cboMealForm.Value != null && cboMealForm.Value.ToString() != "")
                entity.GCMealForm = cboMealForm.Value.ToString();
            entity.Remarks = txtRemark.Text;
            entity.IsStapleFood = chkIsStapleFood.Checked;
            if (cboMealPlanCategory.Value != null && cboMealPlanCategory.Value.ToString() != "")
                entity.GCMealPlanCategory = cboMealPlanCategory.Value.ToString();
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("MealPlanCode = '{0}' AND IsDeleted = 0", txtMealPlanCode.Text);
            List<MealPlan> lst = BusinessLayer.GetMealPlanList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Meal Plan With Code " + txtMealPlanCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("MealPlanCode = '{0}' AND MealPlanID != {1} AND IsDeleted = 0", txtMealPlanCode.Text, hdnID.Value);
            List<MealPlan> lst = BusinessLayer.GetMealPlanList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Meal Plan With Code " + txtMealPlanCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            MealPlanDao entityDao = new MealPlanDao(ctx);
            bool result = false;
            try
            {
                MealPlan entity = new MealPlan();
                ControlToEntity(entity);
                entity.MealPlanCode = GeneratePanelCode(ctx, entity.MealPlanName);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                retval = entityDao.InsertReturnPrimaryKeyID(entity).ToString();
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
                MealPlan entity = BusinessLayer.GetMealPlan(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateMealPlan(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        public String GeneratePanelCode(IDbContext ctx, String ItemName)
        {
            string itemName2Char = ItemName.Trim().Substring(0, 3).ToUpper();
            MealPlan im = BusinessLayer.GetMealPlanList(string.Format("MealPlanCode LIKE '{0}%'", itemName2Char), 1, 1, "MealPlanCode DESC", ctx).FirstOrDefault();
            int newNumber = 1;
            if (im != null)
                newNumber = Convert.ToInt32(im.MealPlanCode.Substring(itemName2Char.Length)) + 1;
            return string.Format("{0}{1}", itemName2Char, newNumber.ToString().PadLeft(5, '0'));
        }
    }
}
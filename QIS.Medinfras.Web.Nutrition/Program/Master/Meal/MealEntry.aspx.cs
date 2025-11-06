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
    public partial class MealEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Nutrition.MEAL;
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
                Meal entity = BusinessLayer.GetMeal(Convert.ToInt32(ID));
                SetControlProperties();
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            txtMealCode.Focus();
        }

        protected override void SetControlProperties()
        {
            //List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND ISActive = 1 AND IsDeleted = 0", Constant.StandardCode.NUTRIENT_UNIT));
            //Methods.SetComboBoxField<StandardCode>(cboNutrientUnit, lstStandardCode, "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtMealCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtMealName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtStandardPortion, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtServingLabel, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtRemark, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(Meal entity)
        {
            txtMealCode.Text = entity.MealCode;
            txtMealName.Text = entity.MealName;
            txtServingLabel.Text = entity.ServingLabel;
            txtRemark.Text = entity.Remarks;
            txtStandardPortion.Text = entity.StandardPortion.ToString();
        }

        private void ControlToEntity(Meal entity)
        {
            entity.MealName = txtMealName.Text;
            entity.ServingLabel = txtServingLabel.Text;
            entity.Remarks = txtRemark.Text;
            entity.StandardPortion = Convert.ToDecimal(txtStandardPortion.Text);
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("MealCode = '{0}' AND IsDeleted = 0", txtMealCode.Text);
            List<Meal> lst = BusinessLayer.GetMealList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Meal With Code " + txtMealCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("MealCode = '{0}' AND MealID != {1} AND IsDeleted = 0", txtMealCode.Text, hdnID.Value);
            List<Meal> lst = BusinessLayer.GetMealList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Meal With Code " + txtMealCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            MealDao entityDao = new MealDao(ctx);
            bool result = false;
            try
            {
                Meal entity = new Meal();
                ControlToEntity(entity);
                entity.MealCode = GenerateMealCode(ctx, entity.MealName);
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
                Meal entity = BusinessLayer.GetMeal(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateMeal(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        public String GenerateMealCode(IDbContext ctx, String ItemName)
        {
            string itemName2Char = ItemName.Trim().Substring(0, 3).ToUpper();
            Meal im = BusinessLayer.GetMealList(string.Format("MealCode LIKE '{0}%'", itemName2Char), 1, 1, "MealCode DESC", ctx).FirstOrDefault();
            int newNumber = 1;
            if (im != null)
                newNumber = Convert.ToInt32(im.MealCode.Substring(itemName2Char.Length)) + 1;
            return string.Format("{0}{1}", itemName2Char, newNumber.ToString().PadLeft(5, '0'));
        }
    }
}
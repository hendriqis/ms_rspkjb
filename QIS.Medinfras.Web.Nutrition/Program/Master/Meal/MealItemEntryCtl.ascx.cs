using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.Nutrition.Program
{
    public partial class MealItemEntryCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnMealID.Value = param;
            Meal entity = BusinessLayer.GetMeal(Convert.ToInt32(hdnMealID.Value));
            txtMealCode.Text = entity.MealCode;
            txtMealName.Text = entity.MealName;
            txtStandardPortion.Text = entity.StandardPortion.ToString();

            SetControlProperties();

            BindGridView();

            txtItemCode.Attributes.Add("validationgroup", "mpEntryPopup");
            txtItemAmount.Attributes.Add("validationgroup", "mpEntryPopup");
            cboItemUnit.Attributes.Add("validationgroup", "mpEntryPopup");
        }

        private void SetControlProperties()
        {
            SetItemUnitDropDownList();
        }

        private void SetItemUnitDropDownList()
        {
            string filterExpression = string.Format("ParentID = '{0}' AND TagProperty LIKE '%,URT%' AND IsDeleted = 0",Constant.StandardCode.ITEM_UNIT);
            if (hdnItemID.Value != string.Empty)
                filterExpression = string.Format("ParentID = '{0}' AND (StandardCodeID IN (SELECT GCAlternateUnit FROM ItemAlternateUnit WHERE ItemID = {1} AND IsDeleted = 0) OR StandardCodeID = (SELECT GCItemUnit FROM ItemMaster WHERE ItemID = {1}))", Constant.StandardCode.ITEM_UNIT, hdnItemID.Value);

            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboItemUnit, lstSc, "StandardCodeName", "StandardCodeID");
            cboItemUnit.SelectedIndex = 0;
        }

        protected string OnGetItemFilterExpression()
        {
            return string.Format("ItemID NOT IN (SELECT ItemID FROM MealDt WHERE MealID = {0} AND IsDeleted = 0) AND IsDeleted = 0 AND GCItemType= '{1}' ", hdnMealID.Value, Constant.ItemGroupMaster.NUTRITION);
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("MealID = {0} AND IsDeleted = 0", hdnMealID.Value);
            List<vMealDt> lstEntity = BusinessLayer.GetvMealDtList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }
            
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string[] param = e.Parameter.Split('|');

            string result = param[0] + "|";
            string errMessage = "";
            if (param[0] == "save")
            {
                if (hdnIsAdd.Value.ToString() == "0")
                {
                    if (OnSaveEditRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "delete")
            {
                if (OnDeleteRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            BindGridView();
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(MealDt entity)
        {
            entity.MealID = Convert.ToInt32(hdnMealID.Value);
            entity.ItemID = Convert.ToInt32(hdnItemID.Value);
            entity.FoodQuantity = Convert.ToDecimal(txtItemAmount.Text);
            entity.GCItemUnit = cboItemUnit.Value.ToString();
            if (txtNotes.Text == "")
            {
                entity.AmountPerServingLabel = "";
            }
            else 
            entity.AmountPerServingLabel = txtNotes.Text;
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                MealDt entity = new MealDt();
                ControlToEntity(entity);
                entity.MealID = Convert.ToInt32(hdnMealID.Value);
                entity.IsDeleted = false;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertMealDt(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                MealDt entity = BusinessLayer.GetMealDt(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateMealDt(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            try
            {
                MealDt entity = BusinessLayer.GetMealDt(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateMealDt(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        protected void cboItemUnit_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            SetItemUnitDropDownList();
        }
       
    }
}
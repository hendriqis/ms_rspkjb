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
    public partial class FoodNutrifactEntryCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnItemID.Value = param;
            ItemMaster entity = BusinessLayer.GetItemMaster(Convert.ToInt32(hdnItemID.Value));
            txtItemCode.Text = entity.ItemCode;
            txtItemName.Text = entity.ItemName1;

            SetControlProperties();

            BindGridView();

            txtNutrientCode.Attributes.Add("validationgroup", "mpEntryPopup");
            txtNutrientAmount.Attributes.Add("validationgroup", "mpEntryPopup");
            cboGCNutrientUnit.Attributes.Add("validationgroup", "mpEntryPopup");
        }

        private void SetControlProperties()
        {
            List<StandardCode> list = BusinessLayer.GetStandardCodeList(String.Format("ParentID = '{0}' AND IsDeleted = 0",Constant.StandardCode.NUTRIENT_UNIT));
            Methods.SetComboBoxField<StandardCode>(cboGCNutrientUnit, list, "StandardCodeName", "StandardCodeID");

        }

        protected string OnGetNutrientFilterExpression()
        {
            return string.Format("NutrientID NOT IN (SELECT NutrientID FROM FoodNutrifact WHERE ItemID = {0} AND IsDeleted = 0) AND IsDeleted = 0", hdnItemID.Value);
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("FoodID = {0} AND IsDeleted = 0", hdnItemID.Value);
            List<vFoodNutrifact> lstEntity = BusinessLayer.GetvFoodNutrifactList(filterExpression);
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

        private void ControlToEntity(FoodNutrifact entity)
        {
            entity.NutrientID = Convert.ToInt32(hdnNutrientID.Value);
            entity.NutrientAmount = Convert.ToDecimal(txtNutrientAmount.Text);
            entity.GCNutrientUnit = cboGCNutrientUnit.Value.ToString();
            entity.Remarks = txtNotes.Text;
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                FoodNutrifact entity = new FoodNutrifact();
                ControlToEntity(entity);
                entity.ItemID = Convert.ToInt32(hdnItemID.Value);
                entity.IsDeleted = false;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertFoodNutrifact(entity);
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
                FoodNutrifact entity = BusinessLayer.GetFoodNutrifact(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateFoodNutrifact(entity);
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
                FoodNutrifact entity = BusinessLayer.GetFoodNutrifact(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateFoodNutrifact(entity);
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
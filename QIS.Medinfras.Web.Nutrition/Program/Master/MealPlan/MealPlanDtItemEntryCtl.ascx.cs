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
    public partial class MealPlanDtItemEntryCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnMealPlanDtID.Value = param;
            vMealPlanDt entity = BusinessLayer.GetvMealPlanDtList(String.Format("MealPlanDtID = {0}", Convert.ToInt32(hdnMealPlanDtID.Value))).FirstOrDefault();
            txtMealPlanCode.Text = entity.MealPlanCode;
            txtMealPlanName.Text = entity.MealPlanName;
            txtMealTime.Text = entity.MealTime;
            txtMealPlanCodeTemplate.Text = entity.MealPlanCode;
            txtMealPlanNameTemplate.Text = entity.MealPlanName;

            SetControlProperties();

            BindGridView();

            txtMealCode.Attributes.Add("validationgroup", "mpEntryPopup");
        }

        private void SetControlProperties()
        {
            List<ClassCare> listClass = BusinessLayer.GetClassCareList("IsUsedInChargeClass = 1 AND IsDeleted = 0");
            Methods.SetComboBoxField<ClassCare>(cboClass, listClass, "ClassName", "ClassID");
            cboClass.SelectedIndex = 0;
            Methods.SetComboBoxField<ClassCare>(cboClassTemplate, listClass, "ClassName", "ClassID");
            cboClassTemplate.SelectedIndex = 0;

            string filterExpression = string.Format("ParentID IN ('{0}','{1}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.MEAL_DATE, Constant.StandardCode.MEAL_TIME);
              
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField(cboDay, lstSc.Where(p => p.ParentID == Constant.StandardCode.MEAL_DATE).ToList(), "StandardCodeName", "StandardCodeID");
            cboDay.SelectedIndex = 0;
            Methods.SetComboBoxField(cboDayTemplate, lstSc.Where(p => p.ParentID == Constant.StandardCode.MEAL_DATE).ToList(), "StandardCodeName", "StandardCodeID");
            cboDayTemplate.SelectedIndex = 0;
            Methods.SetComboBoxField<StandardCode>(cboMealTimeTemplate, lstSc.Where(x => x.ParentID == Constant.StandardCode.MEAL_TIME).ToList(), "StandardCodeName", "StandardCodeID");
        }
        
        private void BindGridView()
        {
            string filterExpression = string.Format("MealPlanDtID = {0} AND ClassID = {1} AND GCMealDay = '{2}' AND IsDeleted = 0", hdnMealPlanDtID.Value,Convert.ToInt32(cboClass.Value),cboDay.Value);
            List<vMealPlanDtItem> lstEntity = BusinessLayer.GetvMealPlanDtItemList(filterExpression);
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
            else if(param[0] == "import")
            {
                ImportTemplate(ref errMessage);
            }
            BindGridView();
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(MealPlanDtItem entity)
        {
            entity.MealID = Convert.ToInt32(hdnMealID.Value);            
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                MealPlanDtItem entity = new MealPlanDtItem();
                ControlToEntity(entity);
                entity.MealPlanDtID = Convert.ToInt32(hdnMealPlanDtID.Value);
                entity.ClassID = Convert.ToInt32(cboClass.Value);
                entity.GCMealDay = Convert.ToString(cboDay.Value);
                entity.IsDeleted = false;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertMealPlanDtItem(entity);
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
                MealPlanDtItem entity = BusinessLayer.GetMealPlanDtItem(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateMealPlanDtItem(entity);
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
                MealPlanDtItem entity = BusinessLayer.GetMealPlanDtItem(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateMealPlanDtItem(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private bool ImportTemplate(ref string errMessage)
        {
            try
            {
                string filterExpression = string.Format("MealPlanDtID = {0} AND ClassID = {1} AND GCMealDay = '{2}' AND GCMealTime = '{3}' AND MealPlanID = '{4}' AND IsDeleted = 0", hdnMealPlanDtIDTemplate.Value, Convert.ToInt32(cboClassTemplate.Value), cboDayTemplate.Value, cboMealTimeTemplate.Value, hdnMealPlanIDTemplate.Value);
                List<vMealPlanDtItem> lstEntity = BusinessLayer.GetvMealPlanDtItemList(filterExpression);

                foreach (vMealPlanDtItem str in lstEntity)
                {
                    MealPlanDtItem entity = new MealPlanDtItem();
                    entity.MealPlanDtID = Convert.ToInt32(hdnMealPlanDtID.Value);
                    entity.ClassID = Convert.ToInt32(cboClass.Value);
                    entity.GCMealDay = Convert.ToString(cboDay.Value);
                    entity.IsDeleted = false;
                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    entity.MealID = str.MealID;
                    BusinessLayer.InsertMealPlanDtItem(entity);
                }


                //MealPlanDtItem entity = new MealPlanDtItem();
                //ControlToEntity(entity);
                //entity.MealPlanDtID = Convert.ToInt32(hdnMealPlanDtID.Value);
                //entity.ClassID = Convert.ToInt32(cboClass.Value);
                //entity.GCMealDay = Convert.ToString(cboDay.Value);
                //entity.IsDeleted = false;
                //entity.CreatedBy = AppSession.UserLogin.UserID;
                //BusinessLayer.InsertMealPlanDtItem(entity);


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
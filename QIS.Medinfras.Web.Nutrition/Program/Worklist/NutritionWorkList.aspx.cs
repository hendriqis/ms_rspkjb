using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;
using System.Text;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Nutrition.Program
{
    public partial class NutritionWorkList : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Nutrition.WORK_LIST;
            
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected String GetItemDetailStatus()
        {
            return Constant.TransactionStatus.PROCESSED;
        }

        protected override void InitializeDataControl() 
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            List<Variable> lstViewType = new List<Variable>();
            lstViewType.Add(new Variable { Code = "1", Value = GetLabel("Berdasarkan Panel Menu Makanan") });
            lstViewType.Add(new Variable { Code = "2", Value = GetLabel("Berdasarkan Menu Makanan") });

            Methods.SetRadioButtonListField<Variable>(rblViewType, lstViewType, "Value", "Code");
            rblViewType.SelectedIndex = 0;

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.MEAL_TIME));
            Methods.SetComboBoxField<StandardCode>(cboMealTime, lstStandardCode, "StandardCodeName", "StandardCodeID");
            cboMealTime.SelectedIndex = 0;

            List<ServiceUnitMaster> lstServiceUnit = BusinessLayer.GetServiceUnitMasterList(string.Format("IsDeleted = 0"));

            txtDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            BindGridView();
            BindGridViewMenu();
        }
        
        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }

        protected void cbpViewMenu_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridViewMenu();
        }

        private void BindGridView()
        {
            string filterExpression = GetFilterExpression();

            List<vNutritionOrderDtWorkList1> lstEntity = BusinessLayer.GetvNutritionOrderDtWorkList1List(filterExpression + String.Format(" ORDER BY MealPlanCode"));
            
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        private void BindGridViewMenu()
        {
            string filterExpression = GetFilterExpression();

            List<vNutritionOrderDtMealPlan> lstEntity2 = BusinessLayer.GetvNutritionOrderDtMealPlanList(filterExpression + String.Format(" ORDER BY MealName"));
            grdViewMenu.DataSource = lstEntity2;
            grdViewMenu.DataBind();
        }

        private string GetFilterExpression()
        {
            string filterExpression = "";
            filterExpression = string.Format("ScheduleDate = '{0}' AND GCMealTime = '{1}'", Helper.GetDatePickerValue(txtDate.Text).ToString("yyyyMMdd"), cboMealTime.Value);
            return filterExpression;
        }

        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                if (ChangeWorkListStatus(ref errMessage)) 
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else if (param[0] == "cancel")
            {
                if (OnChangePreviousWorkListStatus(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        public bool ChangeWorkListStatus(ref string errMessage) 
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            NutritionOrderDtDao entityDtDao = new NutritionOrderDtDao(ctx);
            try
            {
                String filterExpression = String.Format("GCMealTime = '{0}' AND NutritionOrderHdID IN (SELECT NutritionOrderHdID FROM NutritionOrderHd WHERE ScheduleDate = '{1}') AND MealPlanID IN ({2})", cboMealTime.Value, Helper.GetDatePickerValue(txtDate.Text).ToString("yyyyMMdd"), hdnMealPlanID.Value);
                List<NutritionOrderDt> lstEntity = BusinessLayer.GetNutritionOrderDtList(filterExpression,ctx);
                foreach (NutritionOrderDt obj in lstEntity) 
                {
                    if (obj.GCItemDetailStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                    {
                        obj.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                    }
                    else if (obj.GCItemDetailStatus == Constant.TransactionStatus.APPROVED)
                    {
                        obj.GCItemDetailStatus = Constant.TransactionStatus.PROCESSED;
                    }
                    obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Update(obj);
                }
                ctx.CommitTransaction();
            }
            catch(Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        public bool OnChangePreviousWorkListStatus(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            NutritionOrderDtDao entityDtDao = new NutritionOrderDtDao(ctx);
            try
            {
                String filterExpression = String.Format("GCMealTime = '{0}' AND NutritionOrderHdID IN (SELECT NutritionOrderHdID FROM NutritionOrderHd WHERE ScheduleDate = '{1}') AND MealPlanID IN ({2})", cboMealTime.Value, Helper.GetDatePickerValue(txtDate.Text).ToString("yyyyMMdd"), hdnMealPlanID.Value);
                List<NutritionOrderDt> lstEntity = BusinessLayer.GetNutritionOrderDtList(filterExpression, ctx);
                foreach (NutritionOrderDt obj in lstEntity)
                {
                    if (obj.GCItemDetailStatus == Constant.TransactionStatus.PROCESSED)
                    {
                        obj.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                    }
                    else if (obj.GCItemDetailStatus == Constant.TransactionStatus.APPROVED)
                    {
                        obj.GCItemDetailStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                    }
                    obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Update(obj);
                }
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
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
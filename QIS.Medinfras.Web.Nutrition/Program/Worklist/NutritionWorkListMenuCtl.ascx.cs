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
    public partial class NutritionWorkListMenuCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');

            hdnScheduleDate.Value = paramInfo[0];
            hdnMealPlanID.Value = paramInfo[1];
            txtMealPlanCode.Text = paramInfo[2];
            txtMealPlanName.Text = paramInfo[3];
            txtMealTime.Text = paramInfo[4];

            SetControlProperties();

            BindGridView();
        }

        private void SetControlProperties()
        {
            //List<ClassCare> listClass = BusinessLayer.GetClassCareList("IsUsedInChargeClass = 1 AND IsDeleted = 0");
            //Methods.SetComboBoxField<ClassCare>(cboClass, listClass, "ClassName", "ClassID");
            //cboClass.SelectedIndex = 0;

            //string filterExpression = string.Format("ParentID IN ('{0}','{1}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.MEAL_DATE, Constant.StandardCode.MEAL_TIME);
              
            //List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(filterExpression);
            //Methods.SetComboBoxField(cboDay, lstSc.Where(p => p.ParentID == Constant.StandardCode.MEAL_DATE).ToList(), "StandardCodeName", "StandardCodeID");
            //cboDay.SelectedIndex = 0;
        }
        
        private void BindGridView()
        {
            string filterExpression = string.Format("ScheduleDate = '{0}' AND MealPlanID = {1} AND MealTime = '{2}'", hdnScheduleDate.Value, hdnMealPlanID.Value, txtMealTime.Text);
            List<vNutritionOrderDtMealPlan> lstEntity = BusinessLayer.GetvNutritionOrderDtMealPlanList(filterExpression);
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
            string errMessage = string.Empty;

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpPrintJobOrder_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string[] param = e.Parameter.Split('|');
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            try
            {
                string scheduleDate = hdnScheduleDate.Value;
                int id = Convert.ToInt32(hdnMealPlanID.Value);
                string mealTime = txtMealTime.Text;
                string result = string.Empty;
                result = PrintNutritionMealPlanMenuList(scheduleDate, id, mealTime);
                panel.JSProperties["cpZebraPrinting"] = result;
            }
            catch (Exception ex)
            {
                Helper.InsertErrorLog(ex);
                panel.JSProperties["cpZebraPrinting"] = "An error occured while sending command to printer";
            }
        }

        private string PrintNutritionMealPlanMenuList(string scheduleDate, int id, string mealTime)
        {
            //Get Printer Address
            string ipAddress = HttpContext.Current.Request.UserHostAddress;

            string filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                ipAddress, Constant.DirectPrintType.GIZI_JOB_ORDER_MENU_DETAIL);

            List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);

            if (lstPrinter.Count > 0)
                ZebraPrinting.PrintKitchenJobOrderByMealPlan1(scheduleDate, id, mealTime, lstPrinter[0].PrinterName);
            else
                return string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);

            return string.Empty;
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            return true;
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            return true;
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            return true;
        }
    }
}
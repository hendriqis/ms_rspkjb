using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using System.Data;

namespace QIS.Medinfras.Web.Nutrition.Program
{
    public partial class NutritionWorkListCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {
            List<String> lstParam = param.Split('|').ToList();
            hdnMealPlanID.Value = lstParam[0];
            hdnGCMealTime.Value = lstParam[1];
            hdnGCMealDay.Value = lstParam[2];
            hdnDateParam.Value = lstParam[3];
            txtDate.Text = lstParam[3];
            hdnGCMealStatus.Value = lstParam[4];
            hdnIsHasRemarks.Value = lstParam[5];
            List<StandardCode> lst = BusinessLayer.GetStandardCodeList(String.Format("StandardCodeID IN ('{0}','{1}')",lstParam[1],lstParam[2]));
            txtMealTime.Text = lst.First(x => x.StandardCodeID == lstParam[1]).StandardCodeName;
            txtMealDay.Text = lst.First(x => x.StandardCodeID == lstParam[2]).StandardCodeName;

            List<vHealthcareServiceUnit> lstServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("DepartmentID = 'INPATIENT' AND IsDeleted = 0"));
            lstServiceUnit.Insert(0, new vHealthcareServiceUnit { HealthcareServiceUnitID = 0, ServiceUnitName = "" });
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
            cboServiceUnit.SelectedIndex = 0;

            BindGridView(1, true, ref PageCount);
        }

        protected void cbpViewPopup_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref PageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridView(1, true, ref PageCount);
                    result = "refresh|" + PageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            String filterExpression = String.Format("MealPlanID = {0} AND GCMealTime = '{1}' AND GCMealDay = '{2}' AND ScheduleDate = '{3}' AND GCMealStatus = '{4}'", hdnMealPlanID.Value, hdnGCMealTime.Value, hdnGCMealDay.Value, Helper.GetDatePickerValue(hdnDateParam.Value.ToString()), hdnGCMealStatus.Value);
            
            if (hdnIsHasRemarks.Value == "True")
                filterExpression += string.Format(" AND IsHasRemarks = 1");

            object cbo = cboServiceUnit.Value;
            if (cbo != null && cboServiceUnit.Value.ToString() != "0")
            {
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", cboServiceUnit.Value.ToString());
            }

            List<vNutritionOrderDt1> lstEntity = BusinessLayer.GetvNutritionOrderDt1List(filterExpression);
            txtPatientTotal.Text = lstEntity.Count.ToString();
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ReorderMealPlanListX : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            string type = string.Empty;
            string[] page = Page.Request.QueryString["id"].Split('|');
            type = page[0];

            switch (type)
            {
                case "NT":
                    return Constant.MenuCode.Nutrition.NUTRITION_REORDER_MEAL_PLAN;
                case "IP":
                    return Constant.MenuCode.Inpatient.NUTRITION_REORDER_MEAL_PLAN;
                default:
                    return Constant.MenuCode.Nutrition.NUTRITION_REORDER_MEAL_PLAN;
            }

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

            string filterCboDepartment = string.Empty;
            if (Page.Request.QueryString["id"].Split('|')[0] == "IP")
            {
                string filterHSU = string.Format("DepartmentID = '{0}' AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM ServiceUnitUserRole WHERE RoleID IN (SELECT RoleID FROM UserInRole WHERE UserID = {1}) AND IsDeleted = 0)",Constant.Facility.INPATIENT, AppSession.UserLogin.UserID);
                List<vHealthcareServiceUnit> lstCheck = BusinessLayer.GetvHealthcareServiceUnitList(string.Format(filterHSU));
                if (lstCheck.Count > 0)
                {
                    filterCboDepartment += filterHSU;
                }
                else
                {
                    filterCboDepartment += string.Format("DepartmentID = '{0}' AND IsDeleted = 0", Constant.Facility.INPATIENT);
                }
            }
            else
            {
                filterCboDepartment += string.Format("DepartmentID = '{0}' AND IsDeleted = 0", Constant.Facility.INPATIENT);
            }

            List<vHealthcareServiceUnit> lstServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format(filterCboDepartment));
            lstServiceUnit = lstServiceUnit.OrderBy(unit => unit.ServiceUnitName).ToList();
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "ServiceUnitCode");
            cboServiceUnit.SelectedIndex = 0;

            txtScheduleDate.Text = DateTime.Now.AddDays(1).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtScheduleDate.Attributes.Add("readonly", "readonly");
            DateTime date = Convert.ToDateTime(DateTime.Now.AddDays(1));
            hdnMealDay.Value = GetMealDay(date.Day);
            BindGridView();
        }

        private string GetMealDay(int day)
        {
            string result = string.Empty;
            if (day == 31)
                result = Constant.StandardCode.MEAL_DATE + "^011";
            else if (day % 10 == 0)
                result = Constant.StandardCode.MEAL_DATE + "^010";
            else
                result = Constant.StandardCode.MEAL_DATE + String.Format("^00{0}", (day % 10).ToString());
            return result;
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = GetFilterExpression();

            List<vReorderMealPlanPatient> lstEntity = BusinessLayer.GetvReorderMealPlanPatientList(filterExpression + String.Format(" ORDER BY BedCode"));

            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        private string GetFilterExpression()
        {
            string filterExpression = "";
            filterExpression = string.Format("GCMealTime = '{0}' AND ServiceUnitCode = '{1}' AND IsDischarge = 0", cboMealTime.Value, cboServiceUnit.Value);
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
                if (OnReorderNutritionOrder(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        public bool OnReorderNutritionOrder(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            NutritionOrderHdDao entityHdDao = new NutritionOrderHdDao(ctx);
            NutritionOrderDtDao entityDtDao = new NutritionOrderDtDao(ctx);
            NutritionOrderHdDietTypeDao entityDietDao = new NutritionOrderHdDietTypeDao(ctx);
            ConsultVisitDao entityCVDao = new ConsultVisitDao(ctx);

            int orderHdId = 0;
            try
            {
                List<NutritionOrderHd> lstEntityHd = BusinessLayer.GetNutritionOrderHdList(string.Format("NutritionOrderHdID IN ({0})", hdnNutritionOrderHdID.Value), ctx);
                foreach (NutritionOrderHd obj in lstEntityHd)
                {
                    ConsultVisit cv = entityCVDao.Get(obj.VisitID);
           
                    NutritionOrderHd entityHd = new NutritionOrderHd();
                    entityHd.NutritionOrderNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.IP_NUTRITION_ORDER, DateTime.Now, ctx);
                    entityHd.NutritionOrderDate = DateTime.Now;
                    entityHd.ScheduleTime = entityHd.NutritionOrderTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                    entityHd.ScheduleDate = DateTime.Now.AddDays(1);
                    entityHd.ReferenceNo = obj.NutritionOrderNo;
                    entityHd.VisitID = obj.VisitID;
                    entityHd.HealthcareServiceUnitID = obj.HealthcareServiceUnitID;
                    entityHd.VisitHealthcareServiceUnitID = cv.HealthcareServiceUnitID;
                    entityHd.ParamedicID = obj.ParamedicID;
                    entityHd.GCDietType = obj.GCDietType;
                    entityHd.NumberOfCalories = obj.NumberOfCalories;
                    entityHd.Remarks = obj.Remarks;
                    entityHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                    entityHd.CreatedDate = DateTime.Now;
                    entityHd.CreatedBy = AppSession.UserLogin.UserID;

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();

                    orderHdId = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
                    
                    List<NutritionOrderHdDietType> lstEntityDiet = BusinessLayer.GetNutritionOrderHdDietTypeList(string.Format("NutritionOrderHdID = {0}", obj.NutritionOrderHdID));
                    if (lstEntityDiet.Count > 0)
                    {
                        foreach (NutritionOrderHdDietType objDiet in lstEntityDiet)
                        {
                            NutritionOrderHdDietType entityDiet = new NutritionOrderHdDietType();
                            entityDiet.NutritionOrderHdID = orderHdId;
                            entityDiet.GCDietType = objDiet.GCDietType;
                            entityDietDao.Insert(entityDiet);
                        }
                    }

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();

                    List<NutritionOrderDt> lstEntityDt = BusinessLayer.GetNutritionOrderDtList(string.Format("NutritionOrderHdId = {0} AND GCItemDetailStatus NOT IN ('{1}')", obj.NutritionOrderHdID, Constant.TransactionStatus.VOID), ctx);
                    foreach (NutritionOrderDt objDt in lstEntityDt)
                    {
                        if (cboMealTime.Value.ToString() == objDt.GCMealTime)
                        {
                            NutritionOrderDt entityDt = new NutritionOrderDt();
                            entityDt.NutritionOrderHdID = orderHdId;
                            entityDt.ParamedicID = objDt.ParamedicID;
                            entityDt.GCMealTime = objDt.GCMealTime;
                            entityDt.GCMealDay = hdnMealDay.Value;
                            entityDt.ClassID = objDt.ClassID;
                            entityDt.MealPlanID = objDt.MealPlanID;
                            entityDt.Remarks = objDt.Remarks;
                            entityDt.GCMealStatus = objDt.GCMealStatus;
                            entityDt.GCItemDetailStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                            entityDt.CreatedBy = AppSession.UserLogin.UserID;
                            entityDt.CreatedDate = DateTime.Now;
                            entityDtDao.InsertReturnPrimaryKeyID(entityDt);
                        }
                    }
                }
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
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ReorderMealPlanList : BasePageTrx
    {
        protected int PageCount = 1;
        protected string filterExpressionLocation = "";
        protected string filterExpressionLocationTo = "";

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

        private GetUserMenuAccess menu;

        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }

        public override bool IsShowRightPanel()
        {
            return false;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = false;
            IsAllowSave = false;
        }
        protected override void InitializeDataControl()
        {
            MPTrx master = (MPTrx)Master;
            menu = ((MPMain)master.Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

            filterExpressionLocation = string.Format("{0};0;{1};", AppSession.UserLogin.HealthcareID, Constant.TransactionCode.ITEM_REQUEST);
            filterExpressionLocationTo = string.Format("{0};{1};{2};", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.TransactionCode.ITEM_DISTRIBUTION);

            int count = BusinessLayer.GetLocationUserRowCount(string.Format("UserID = {0} AND IsDeleted = 0", AppSession.UserLogin.UserID));
            if (count > 0)
                hdnRecordFilterExpression.Value = string.Format("ToLocationID IN (SELECT LocationID FROM LocationUser WHERE UserID = {0} AND IsDeleted = 0)", AppSession.UserLogin.UserID);
            else
            {
                count = BusinessLayer.GetLocationUserRoleRowCount(string.Format("RoleID IN (SELECT RoleID FROM UserInRole WHERE UserID = {0} AND HealthcareID = '{1}') AND IsDeleted = 0", AppSession.UserLogin.UserID, AppSession.UserLogin.HealthcareID));
                if (count > 0)
                    hdnRecordFilterExpression.Value = string.Format("ToLocationID IN (SELECT LocationID FROM LocationUserRole WHERE RoleID IN (SELECT RoleID FROM UserInRole WHERE UserID = {0} AND HealthcareID = '{1}') AND IsDeleted = 0)", AppSession.UserLogin.UserID, AppSession.UserLogin.HealthcareID);
                else
                    hdnRecordFilterExpression.Value = "";
            }

            string filterHSU = string.Format("DepartmentID = '{0}' AND IsDeleted = 0", Constant.Facility.INPATIENT);
            List<ServiceUnitMaster> lstServiceUnit = BusinessLayer.GetServiceUnitMasterList(filterHSU);
            lstServiceUnit = lstServiceUnit.OrderBy(unit => unit.ServiceUnitName).ToList();
            Methods.SetComboBoxField<ServiceUnitMaster>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "ServiceUnitCode");
            Methods.SetComboBoxField<ServiceUnitMaster>(cboServiceUnitFilter, lstServiceUnit, "ServiceUnitName", "ServiceUnitCode");
            cboServiceUnit.SelectedIndex = 0;
            cboServiceUnitFilter.SelectedIndex = 0;

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.MEAL_TIME));
            Methods.SetComboBoxField<StandardCode>(cboMealTime, lstStandardCode, "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboMealTimeCopy, lstStandardCode, "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboMealTimeGenerate, lstStandardCode, "StandardCodeName", "StandardCodeID");
            cboMealTime.SelectedIndex = 0;
            cboMealTimeCopy.SelectedIndex = 0;
            cboMealTimeGenerate.SelectedIndex = 0;

            //txtScheduleDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtScheduleDate1.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtScheduleDateCopy.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtScheduleDateGenerate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtScheduleDateGenerate.Attributes.Add("readonly", "readonly");

            //22-07-2019
            string scheduleDateYear = txtScheduleDateGenerate.Text.Substring(6, 4);
            string scheduleDateMonth = txtScheduleDateGenerate.Text.Substring(3, 2);
            string scheduleDateDay = txtScheduleDateGenerate.Text.Substring(0, 2);
            string scheduleDateFormat = string.Format("{0}/{1}/{2}", scheduleDateDay, scheduleDateMonth, scheduleDateYear);
            DateTime scheduleDate = DateTime.ParseExact(scheduleDateFormat, "dd/MM/yyyy", null);
            hdnGCMealDay.Value = GetMealDay(scheduleDate.Day);

            GetSettingParameter();

            BindGridView(1, true, ref PageCount);
        }

        private void GetSettingParameter()
        {
            List<SettingParameterDt> lstSetParDt = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.NT_REORDER_MEAL_BY_MEAL_TIME_PERIOD));
            hdnReorderByMealTimePeriod.Value = lstSetParDt.Where(t => t.ParameterCode == Constant.SettingParameter.NT_REORDER_MEAL_BY_MEAL_TIME_PERIOD).FirstOrDefault().ParameterValue;
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

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;

            if (filterExpression != "")
            {
                filterExpression += " AND ";
            }

            filterExpression += String.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}', '{3}') AND DepartmentID IN ('{4}') AND ServiceUnitCode = '{5}' AND DischargeDate IS NULL", Constant.VisitStatus.OPEN, Constant.VisitStatus.CLOSED, Constant.VisitStatus.DISCHARGED, Constant.VisitStatus.CANCELLED, Constant.Facility.INPATIENT, cboServiceUnit.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvConsultVisit9RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_PATIENT_LIST);
            }

            List<vConsultVisit9> lstEntity = BusinessLayer.GetvConsultVisit9List(filterExpression, Constant.GridViewPageSize.GRID_PATIENT_LIST, pageIndex, "BedCode ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else if (param[0] == "reorder")
                {
                    string errMessage = string.Empty;
                    string orderID = ",";
                    bool resultOrder = true;
                    //22-07-2019
                    string orderDateYear = txtScheduleDateCopy.Text.Substring(6, 4);
                    string orderDateMonth = txtScheduleDateCopy.Text.Substring(3, 2);
                    string orderDateDay = txtScheduleDateCopy.Text.Substring(0, 2);
                    string orderDateFormat = string.Format("{0}/{1}/{2}", orderDateDay, orderDateMonth, orderDateYear);
                    DateTime orderDate = DateTime.ParseExact(orderDateFormat, "dd/MM/yyyy", null);
                    if (!string.IsNullOrEmpty(param[1]))
                    {
                        List<ConsultVisit> lstCV = BusinessLayer.GetConsultVisitList(string.Format("VisitID IN ({0})", param[1]));
                        if (lstCV.Count > 0)
                        {
                            if (hdnReorderByMealTimePeriod.Value == "0")
                            {
                                foreach (ConsultVisit cv in lstCV)
                                {
                                    List<NutritionOrderHd> lstEntityHd = BusinessLayer.GetNutritionOrderHdList(string.Format("VisitID = {0} AND ScheduleDate = '{1}' AND NutritionOrderHdID IN (SELECT NutritionOrderHdID FROM NutritionOrderDt WITH(NOLOCK) WHERE GCMealTime = '{2}' AND GCItemDetailStatus != '{3}')", cv.VisitID, orderDate, cboMealTimeCopy.Value, Constant.TransactionStatus.VOID)).GroupBy(g => g.NutritionOrderHdID).Select(s => s.FirstOrDefault()).ToList();
                                    foreach (NutritionOrderHd hd in lstEntityHd)
                                    {
                                        orderID += hd.NutritionOrderHdID + ",";
                                    }
                                }
                                if (orderID != ",")
                                {
                                    string id = orderID.Remove(orderID.Length - 1, 1);
                                    OnReorderNutritionOrder(ref errMessage, id.Substring(1), ref resultOrder);
                                    if (resultOrder)
                                    {
                                        BindGridViewDt(1, true, ref pageCount);
                                        result = "success|" + pageCount;
                                    }
                                    else
                                    {
                                        result = "failed";
                                    }
                                }
                                else
                                {
                                    result = "copyfailed|" + "Tidak ada data yang dapat diproses.";
                                }
                            }
                            else
                            {
                                foreach (ConsultVisit cv in lstCV)
                                {
                                    NutritionOrderHd entityHd = BusinessLayer.GetNutritionOrderHdList(string.Format("VisitID = {0} AND ScheduleDate = '{1}' ORDER BY NutritionOrderHdID DESC", cv.VisitID, orderDate)).FirstOrDefault();
                                    if (entityHd != null)
                                    {
                                        orderID += entityHd.NutritionOrderHdID.ToString() + ",";
                                    }
                                }
                                string id = orderID.Remove(orderID.Length - 1, 1);
                                if (!string.IsNullOrEmpty(id))
                                {
                                    OnReorderNutritionOrder(ref errMessage, id.Substring(1), ref resultOrder);
                                    if (resultOrder)
                                    {
                                        BindGridViewDt(1, true, ref pageCount);
                                        result = "success|" + pageCount;
                                    }
                                    else
                                    {
                                        result = "copyfailed|" + errMessage;
                                    }
                                }
                                else
                                {
                                    result = "copyfailed|" + errMessage;
                                }
                            }
                        }
                    }
                    else
                    {
                        result = "failed";
                    }
                }
                else // refresh
                {

                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        public bool OnReorderNutritionOrder(ref string errMessage, string lstOrderID, ref bool result)
        {
            IDbContext ctx = DbFactory.Configure(true);
            NutritionOrderHdDao entityHdDao = new NutritionOrderHdDao(ctx);
            NutritionOrderDtDao entityDtDao = new NutritionOrderDtDao(ctx);
            NutritionOrderHdDietTypeDao entityDietDao = new NutritionOrderHdDietTypeDao(ctx);
            ConsultVisitDao entityCVDao = new ConsultVisitDao(ctx);

            //22-07-2019
            string scheduleDateYear = txtScheduleDateGenerate.Text.Substring(6, 4);
            string scheduleDateMonth = txtScheduleDateGenerate.Text.Substring(3, 2);
            string scheduleDateDay = txtScheduleDateGenerate.Text.Substring(0, 2);
            string scheduleDateFormat = string.Format("{0}/{1}/{2}", scheduleDateDay, scheduleDateMonth, scheduleDateYear);
            DateTime scheduleDate = DateTime.ParseExact(scheduleDateFormat, "dd/MM/yyyy", null);
            int orderHdId = 0;
            try
            {
                if (hdnReorderByMealTimePeriod.Value != "1")
                {
                    if (lstOrderID != string.Empty)
                    {
                        List<NutritionOrderHd> lstEntityHd = BusinessLayer.GetNutritionOrderHdList(string.Format("NutritionOrderHdID IN ({0})", lstOrderID), ctx);
                        foreach (NutritionOrderHd obj in lstEntityHd)
                        {
                            ConsultVisit cv = entityCVDao.Get(obj.VisitID);

                            NutritionOrderHd entityHd = new NutritionOrderHd();
                            entityHd.NutritionOrderNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.IP_NUTRITION_ORDER, scheduleDate, ctx);
                            entityHd.NutritionOrderDate = DateTime.Now;
                            entityHd.ScheduleTime = entityHd.NutritionOrderTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                            entityHd.ScheduleDate = scheduleDate;
                            entityHd.ReferenceNo = obj.NutritionOrderNo;
                            entityHd.VisitID = obj.VisitID;
                            entityHd.HealthcareServiceUnitID = obj.HealthcareServiceUnitID;
                            entityHd.VisitHealthcareServiceUnitID = cv.HealthcareServiceUnitID;
                            entityHd.ParamedicID = obj.ParamedicID;
                            entityHd.GCDietType = obj.GCDietType;
                            entityHd.NumberOfCalories = obj.NumberOfCalories;
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

                            List<NutritionOrderDt> lstEntityDt = BusinessLayer.GetNutritionOrderDtList(string.Format("NutritionOrderHdId = {0} AND GCMealTime = '{1}'", obj.NutritionOrderHdID, cboMealTimeCopy.Value.ToString()), ctx);
                            foreach (NutritionOrderDt objDt in lstEntityDt)
                            {
                                NutritionOrderDt entityDt = new NutritionOrderDt();
                                entityDt.NutritionOrderHdID = orderHdId;
                                entityDt.ParamedicID = objDt.ParamedicID;
                                entityDt.GCMealTime = cboMealTimeGenerate.Value.ToString();
                                entityDt.GCMealDay = hdnGCMealDay.Value;
                                entityDt.ClassID = objDt.ClassID;
                                entityDt.MealPlanID = objDt.MealPlanID;
                                entityDt.GCMealStatus = objDt.GCMealStatus;
                                entityDt.GCItemDetailStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                entityDt.CreatedBy = AppSession.UserLogin.UserID;
                                entityDt.CreatedDate = DateTime.Now;
                                entityDtDao.InsertReturnPrimaryKeyID(entityDt);
                            }
                        }
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Tidak ada order yang akan di copy";
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    if (lstOrderID != string.Empty)
                    {
                        List<NutritionOrderHd> lstEntityHd = BusinessLayer.GetNutritionOrderHdList(string.Format("NutritionOrderHdID IN (SELECT NutritionOrderHdID FROM NutritionOrderDt WITH(NOLOCK) WHERE NutritionOrderHdID IN ({0}) AND GCMealTime = '{1}')", lstOrderID, cboMealTimeCopy.Value.ToString()), ctx);
                        if (lstEntityHd.Count > 0)
                        {
                            foreach (NutritionOrderHd obj in lstEntityHd)
                            {
                                ConsultVisit cv = entityCVDao.Get(obj.VisitID);

                                NutritionOrderHd entityHd = new NutritionOrderHd();
                                entityHd.NutritionOrderNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.IP_NUTRITION_ORDER, scheduleDate, ctx);
                                entityHd.NutritionOrderDate = DateTime.Now;
                                entityHd.ScheduleTime = entityHd.NutritionOrderTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                                entityHd.ScheduleDate = scheduleDate;
                                entityHd.ReferenceNo = obj.NutritionOrderNo;
                                entityHd.VisitID = obj.VisitID;
                                entityHd.HealthcareServiceUnitID = obj.HealthcareServiceUnitID;
                                entityHd.VisitHealthcareServiceUnitID = cv.HealthcareServiceUnitID;
                                entityHd.ParamedicID = obj.ParamedicID;
                                entityHd.GCDietType = obj.GCDietType;
                                entityHd.NumberOfCalories = obj.NumberOfCalories;
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

                                List<NutritionOrderDt> lstEntityDt = BusinessLayer.GetNutritionOrderDtList(string.Format("NutritionOrderHdId = {0} AND GCMealTime = '{1}'", obj.NutritionOrderHdID, cboMealTimeCopy.Value.ToString()), ctx);
                                foreach (NutritionOrderDt objDt in lstEntityDt)
                                {
                                    NutritionOrderDt entityDt = new NutritionOrderDt();
                                    entityDt.NutritionOrderHdID = orderHdId;
                                    entityDt.ParamedicID = objDt.ParamedicID;
                                    entityDt.GCMealTime = cboMealTimeGenerate.Value.ToString();
                                    entityDt.GCMealDay = hdnGCMealDay.Value;
                                    entityDt.ClassID = objDt.ClassID;
                                    entityDt.MealPlanID = objDt.MealPlanID;
                                    entityDt.GCMealStatus = objDt.GCMealStatus;
                                    entityDt.GCItemDetailStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                    entityDt.CreatedBy = AppSession.UserLogin.UserID;
                                    entityDt.CreatedDate = DateTime.Now;
                                    entityDtDao.InsertReturnPrimaryKeyID(entityDt);
                                }
                            }
                            ctx.CommitTransaction();
                        }
                        else
                        {
                            result = false;
                            errMessage = "Tidak ada order untuk jadwal makan ini";
                            ctx.RollBackTransaction();
                        }
                    }
                    else
                    {
                        result = false;
                        errMessage = "Tidak ada order yang akan di copy";
                        ctx.RollBackTransaction();
                    }
                }
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

        #region Item Request Dt
        private void BindGridViewDt(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnID.Value != "")
            {
                //22-07-2019
                string orderDateYear = txtScheduleDate1.Text.Substring(6, 4);
                string orderDateMonth = txtScheduleDate1.Text.Substring(3, 2);
                string orderDateDay = txtScheduleDate1.Text.Substring(0, 2);
                string orderDateFormat = string.Format("{0}/{1}/{2}", orderDateDay, orderDateMonth, orderDateYear);
                DateTime orderDate = DateTime.ParseExact(orderDateFormat, "dd/MM/yyyy", null);
                filterExpression = string.Format("ScheduleDate = '{0}' AND GCMealTime = '{1}' AND ServiceUnitCode = '{2}'", orderDate, cboMealTime.Value, cboServiceUnitFilter.Value);

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvReorderMealPlanPatientRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }
            }
            List<vReorderMealPlanPatient> lstEntity = BusinessLayer.GetvReorderMealPlanPatientList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "BedCode ASC");
            grdViewDt.DataSource = lstEntity;
            grdViewDt.DataBind();
        }
        protected void cbpViewDt_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDt(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion
    }
}
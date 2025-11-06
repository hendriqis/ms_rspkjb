using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class NutritionOrderQuickPicksCtl : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;
        private string[] lstSelectedMemberMealTime = null;

        public override void InitializeDataControl(string param)
        {
            hdnParam.Value = param;
            string[] paramList = hdnParam.Value.Split('|');
            hdnNutritionOrderHdID.Value = paramList[0];
            hdnMealDayCtlQP.Value = paramList[1];
            hdnOrderDate.Value = paramList[2];
            hdnOrderTime.Value = paramList[3];
            hdnScheduleDate.Value = paramList[4];
            hdnScheduleTime.Value = paramList[5];
            hdnVisitID.Value = paramList[6];
            hdnParamedicID.Value = paramList[7];
            hdnClassID.Value = paramList[8];
            hdnDiagnoseID.Value = paramList[9];
            hdnNumberOfCalories.Value = paramList[10];
            hdnDietType.Value = paramList[11];
            hdnNumberOfProtein.Value = paramList[12];
            hdnRemarksHd.Value = paramList[13];

            #region Set Meal Day
            if (hdnMealDayCtlQP.Value == "1" || hdnMealDayCtlQP.Value == "11" || hdnMealDayCtlQP.Value == "21")
            {
                hdnGCMealDay.Value = Constant.StandardCode.MEAL_DATE + "^001";
            }
            else if (hdnMealDayCtlQP.Value == "2" || hdnMealDayCtlQP.Value == "12" || hdnMealDayCtlQP.Value == "22")
            {
                hdnGCMealDay.Value = Constant.StandardCode.MEAL_DATE + "^002";
            }
            else if (hdnMealDayCtlQP.Value == "3" || hdnMealDayCtlQP.Value == "13" || hdnMealDayCtlQP.Value == "23")
            {
                hdnGCMealDay.Value = Constant.StandardCode.MEAL_DATE + "^003";
            }
            else if (hdnMealDayCtlQP.Value == "4" || hdnMealDayCtlQP.Value == "14" || hdnMealDayCtlQP.Value == "24")
            {
                hdnGCMealDay.Value = Constant.StandardCode.MEAL_DATE + "^004";
            }
            else if (hdnMealDayCtlQP.Value == "5" || hdnMealDayCtlQP.Value == "15" || hdnMealDayCtlQP.Value == "25")
            {
                hdnGCMealDay.Value = Constant.StandardCode.MEAL_DATE + "^005";
            }
            else if (hdnMealDayCtlQP.Value == "6" || hdnMealDayCtlQP.Value == "16" || hdnMealDayCtlQP.Value == "26")
            {
                hdnGCMealDay.Value = Constant.StandardCode.MEAL_DATE + "^006";
            }
            else if (hdnMealDayCtlQP.Value == "7" || hdnMealDayCtlQP.Value == "17" || hdnMealDayCtlQP.Value == "27")
            {
                hdnGCMealDay.Value = Constant.StandardCode.MEAL_DATE + "^007";
            }
            else if (hdnMealDayCtlQP.Value == "8" || hdnMealDayCtlQP.Value == "18" || hdnMealDayCtlQP.Value == "28")
            {
                hdnGCMealDay.Value = Constant.StandardCode.MEAL_DATE + "^008";
            }
            else if (hdnMealDayCtlQP.Value == "9" || hdnMealDayCtlQP.Value == "19" || hdnMealDayCtlQP.Value == "29")
            {
                hdnGCMealDay.Value = Constant.StandardCode.MEAL_DATE + "^009";
            }
            else if (hdnMealDayCtlQP.Value == "10" || hdnMealDayCtlQP.Value == "20" || hdnMealDayCtlQP.Value == "30")
            {
                hdnGCMealDay.Value = Constant.StandardCode.MEAL_DATE + "^010";
            }
            else
            {
                hdnGCMealDay.Value = Constant.StandardCode.MEAL_DATE + "^011";
            }
            #endregion

            hdnHealthcareServiceUnitID.Value = AppSession.RegisteredPatient.HealthcareServiceUnitID.ToString();

            List<SettingParameterDt> lstSettingParameter = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                                    "HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
                                                                    AppSession.UserLogin.HealthcareID,
                                                                    Constant.SettingParameter.NT_PANEL_MAKAN_BY_DAY
                                                                    ));

            hdnIsMealPlanByDayQPCtl.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.NT_PANEL_MAKAN_BY_DAY).ParameterValue;

            hdnGCItemType.Value = Constant.ItemType.BAHAN_MAKANAN;
            SetControlProperties();
            BindGridView(1, true, ref PageCount);
        }

        private void SetControlProperties()
        {
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(String.Format("ParentID IN ('{0}', '{1}') AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.MEAL_TIME, Constant.StandardCode.MEAL_STATUS));
            Methods.SetComboBoxField<StandardCode>(cboMealTime, lstStandardCode.Where(x => x.ParentID == Constant.StandardCode.MEAL_TIME).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboMealStatus, lstStandardCode.Where(x => x.ParentID == Constant.StandardCode.MEAL_STATUS).ToList(), "StandardCodeName", "StandardCodeID");
            cboMealTime.SelectedIndex = 0;
            cboMealStatus.SelectedIndex = 0;
        }

        protected void cbpViewPopup_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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
                else // refresh
                {
                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vMealPlan1 entity = e.Row.DataItem as vMealPlan1;
                CheckBox chkIsSelected = e.Row.FindControl("chkIsSelected") as CheckBox;
                if (lstSelectedMember.Contains(entity.MealPlanDtID.ToString()))
                    chkIsSelected.Checked = true;
            }
        }

        private string GetFilterExpression()
        {
            string filterExpression = "IsDeletedDetail = 0 AND IsDeletedHd = 0";

            if (hdnFilterItem.Value != string.Empty)
            {
                filterExpression += string.Format(" AND MealPlanName LIKE '%{0}%'", hdnFilterItem.Value);
            }

            if (cboMealTime.Value.ToString() != string.Empty)
            {
                filterExpression += string.Format(" AND GCMealTime = '{0}'", cboMealTime.Value.ToString());
            }

            if (hdnIsMealPlanByDayQPCtl.Value.ToString() == "1")
            {
                filterExpression += string.Format(" AND MealPlanDtID IN (SELECT a.MealPlanDtID FROM MealPlanDtItem a WITH(NOLOCK) WHERE a.IsDeleted = 0 AND a.GCMealDay = '{0}')", hdnGCMealDay.Value);
            }

            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();

            if (hdnNutritionOrderHdID.Value != "0" && hdnNutritionOrderHdID.Value != "")
            {
                
                List<vNutritionOrderDt> lstItemID = BusinessLayer.GetvNutritionOrderDtList(string.Format("NutritionOrderHdID = {0} AND GCItemDetailStatus NOT IN ('{1}', '{2}')", hdnNutritionOrderHdID.Value, Constant.TransactionStatus.VOID, Constant.TransactionStatus.CLOSED));
                string lstSelectedID = "";
                foreach (vNutritionOrderDt itm in lstItemID)
                {
                    lstSelectedID += "," + itm.MealPlanID;
                }

                if (lstItemID.Count == 0)
                {
                    filterExpression += string.Format(" AND IsDeletedHd = 0 AND IsDeletedDetail = 0");
                }
                else
                {
                    filterExpression += string.Format(" AND MealPlanID NOT IN ({0}) AND IsDeletedHd = 0", lstSelectedID.Substring(1));
                }
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvMealPlan1RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MATRIX);
            }

            lstSelectedMember = hdnSelectedMember.Value.Split(',');
            lstSelectedMemberMealTime = hdnSelectedMemberMealTime.Value.Split(',');

            List<vMealPlan1> lstEntity = BusinessLayer.GetvMealPlan1List(filterExpression, Constant.GridViewPageSize.GRID_MATRIX, pageIndex, "MealPlanName ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            NutritionOrderHdDao entityHdDao = new NutritionOrderHdDao(ctx);
            NutritionOrderDtDao entityDtDao = new NutritionOrderDtDao(ctx);
            NutritionOrderHdDietTypeDao entityDiet = new NutritionOrderHdDietTypeDao(ctx);
            bool result = true;
            lstSelectedMember = hdnSelectedMember.Value.Split(',');
            lstSelectedMemberMealTime = hdnSelectedMemberMealTime.Value.Split(',');
            int nutritionOrderHdID = 0;
            int nutritionOrderDtID = 0;
            int index = 0;

            string orderDateYear = hdnOrderDate.Value.Substring(6, 4);
            string orderDateMonth = hdnOrderDate.Value.Substring(3, 2);
            string orderDateDay = hdnOrderDate.Value.Substring(0, 2);
            string orderDateFormat = string.Format("{0}/{1}/{2}", orderDateDay, orderDateMonth, orderDateYear);

            try
            {
                NutritionOrderHd entityHd = null;
                //string[] param = hdnParam.Value.Split('|');
                DateTime orderDate = DateTime.ParseExact(orderDateFormat, "dd/MM/yyyy", null);

                if (hdnNutritionOrderHdID.Value == "" || hdnNutritionOrderHdID.Value == "0")
                {
                    entityHd = new NutritionOrderHd();
                    entityHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                    entityHd.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                    entityHd.VisitHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                    entityHd.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
                    entityHd.NutritionOrderDate = orderDate;
                    entityHd.NutritionOrderTime = hdnOrderTime.Value;
                    entityHd.NutritionOrderNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.IP_NUTRITION_ORDER, entityHd.NutritionOrderDate);
                    entityHd.ScheduleDate = Convert.ToDateTime(hdnScheduleDate.Value);
                    entityHd.ScheduleTime = hdnScheduleTime.Value;
                    entityHd.Remarks = hdnRemarksHd.Value;
                    entityHd.DiagnoseID = hdnDiagnoseID.Value;
                    if (hdnDietType.Value != "")
                    {
                        entityHd.GCDietType = hdnDietType.Value;
                    }
                    entityHd.NumberOfCalories = Convert.ToDecimal(hdnNumberOfCalories.Value);
                    entityHd.NumberOfProtein = Convert.ToDecimal(hdnNumberOfProtein.Value);
                    entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                    entityHd.CreatedBy = AppSession.UserLogin.UserID;
                    entityHd.CreatedDate = DateTime.Now;
                    nutritionOrderHdID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
                }
                else
                {
                    entityHd = entityHdDao.Get(Convert.ToInt32(hdnNutritionOrderHdID.Value));
                }
                retval = entityHd.NutritionOrderNo;

                #region Multi Diet
                List<NutritionOrderHd> entityOrderHd = BusinessLayer.GetNutritionOrderHdList(string.Format("VisitID = {0} AND GCTransactionStatus != '{1}' ORDER BY NutritionOrderHdID DESC", AppSession.RegisteredPatient.VisitID, Constant.TransactionStatus.VOID), ctx);
                if (entityOrderHd.Count > 1)
                {
                    NutritionOrderHd orderHd = BusinessLayer.GetNutritionOrderHdList(string.Format("VisitID = {0} AND GCTransactionStatus != '{1}' ORDER BY NutritionOrderHdID DESC", AppSession.RegisteredPatient.VisitID, Constant.TransactionStatus.VOID), ctx)[1];
                    List<NutritionOrderHdDietType> lstEntityDiet = BusinessLayer.GetNutritionOrderHdDietTypeList(string.Format("NutritionOrderHdID = {0}", orderHd.NutritionOrderHdID), ctx);
                    if (lstEntityDiet.Count > 0)
                    {
                        foreach (NutritionOrderHdDietType obj in lstEntityDiet)
                        {
                            if (hdnNutritionOrderHdID.Value == "" || hdnNutritionOrderHdID.Value == "0")
                            {
                                NutritionOrderHdDietType checkDiet = BusinessLayer.GetNutritionOrderHdDietTypeList(string.Format("NutritionOrderHdID = {0} AND GCDietType = '{1}'", nutritionOrderHdID, obj.GCDietType), ctx).FirstOrDefault();
                                if (checkDiet == null)
                                {
                                    NutritionOrderHdDietType oDiet = new NutritionOrderHdDietType();
                                    oDiet.NutritionOrderHdID = nutritionOrderHdID;
                                    oDiet.GCDietType = obj.GCDietType;
                                    entityDiet.Insert(oDiet);
                                }
                            }
                            else
                            {
                                NutritionOrderHdDietType checkDiet = BusinessLayer.GetNutritionOrderHdDietTypeList(string.Format("NutritionOrderHdID = {0} AND GCDietType = '{1}'", entityHd.NutritionOrderHdID, obj.GCDietType), ctx).FirstOrDefault();
                                if (checkDiet == null)
                                {
                                    NutritionOrderHdDietType oDiet = new NutritionOrderHdDietType();
                                    oDiet.NutritionOrderHdID = entityHd.NutritionOrderHdID;
                                    oDiet.GCDietType = obj.GCDietType;
                                    entityDiet.Insert(oDiet);
                                }
                            }
                        }
                    }
                }
                else
                {
                    List<NutritionOrderHdDietType> lstEntityDiet = BusinessLayer.GetNutritionOrderHdDietTypeList(string.Format("NutritionOrderHdID = {0}", entityOrderHd.FirstOrDefault().NutritionOrderHdID), ctx);
                    if (lstEntityDiet.Count > 0)
                    {
                        foreach (NutritionOrderHdDietType obj in lstEntityDiet)
                        {
                            if (hdnNutritionOrderHdID.Value == "" || hdnNutritionOrderHdID.Value == "0")
                            {
                                NutritionOrderHdDietType checkDiet = BusinessLayer.GetNutritionOrderHdDietTypeList(string.Format("NutritionOrderHdID = {0} AND GCDietType = '{1}'", nutritionOrderHdID, obj.GCDietType), ctx).FirstOrDefault();
                                if (checkDiet == null)
                                {
                                    NutritionOrderHdDietType oDiet = new NutritionOrderHdDietType();
                                    oDiet.NutritionOrderHdID = nutritionOrderHdID;
                                    oDiet.GCDietType = obj.GCDietType;
                                    entityDiet.Insert(oDiet);
                                }
                            }
                            else
                            {
                                NutritionOrderHdDietType checkDiet = BusinessLayer.GetNutritionOrderHdDietTypeList(string.Format("NutritionOrderHdID = {0} AND GCDietType = '{1}'", entityHd.NutritionOrderHdID, obj.GCDietType), ctx).FirstOrDefault();
                                if (checkDiet == null)
                                {
                                    NutritionOrderHdDietType oDiet = new NutritionOrderHdDietType();
                                    oDiet.NutritionOrderHdID = entityHd.NutritionOrderHdID;
                                    oDiet.GCDietType = obj.GCDietType;
                                    entityDiet.Insert(oDiet);
                                }
                            }
                        }
                    }
                }
                #endregion

                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    foreach (String id in lstSelectedMember)
                    {
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();

                        MealPlanDt entityMP = BusinessLayer.GetMealPlanDtList(string.Format("MealPlanDtID = {0}", id)).FirstOrDefault();

                        NutritionOrderDt entity = new NutritionOrderDt();
                        if (hdnNutritionOrderHdID.Value == "" || hdnNutritionOrderHdID.Value == "0")
                        {
                            entity.NutritionOrderHdID = nutritionOrderHdID;
                        }
                        else
                        {
                            entity.NutritionOrderHdID = entityHd.NutritionOrderHdID;
                        }
                        entity.ParamedicID = AppSession.RegisteredPatient.ParamedicID;
                        //entity.GCMealTime = cboMealTime.Value.ToString();
                        entity.GCMealTime = lstSelectedMemberMealTime[index];
                        entity.GCMealDay = hdnGCMealDay.Value;
                        entity.ClassID = Convert.ToInt32(hdnClassID.Value);
                        if (entityMP != null)
                        {
                            entity.MealPlanID = entityMP.MealPlanID;
                        }
                        entity.GCMealStatus = cboMealStatus.Value.ToString();
                        entity.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                        entity.IsNotForPatient = chkIsNotForPatient.Checked;
                        entity.CreatedBy = AppSession.UserLogin.UserID;
                        entity.CreatedDate = DateTime.Now;

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();

                        nutritionOrderDtID = entityDtDao.InsertReturnPrimaryKeyID(entity);
                        index++;
                        

                    }
                }
                else
                {
                    result = false;
                    errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
                    ctx.RollBackTransaction();
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
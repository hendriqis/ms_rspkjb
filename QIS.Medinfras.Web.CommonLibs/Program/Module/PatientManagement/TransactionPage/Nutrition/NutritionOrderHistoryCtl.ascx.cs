using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class NutritionOrderHistoryCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnParam.Value = param;

            string[] temp = hdnParam.Value.Split('|');
            hdnRegistrationID.Value = temp[0];
            hdnVisitID.Value = temp[1];
            hdnNutritionOrderHdID.Value = temp[2];
            hdnParamedicID.Value = temp[3];
            hdnClassID.Value = temp[4];
            hdnOrderDate.Value = temp[5];
            hdnOrderTime.Value = temp[6];
            hdnHealthcareServiceUnitID.Value = temp[7];
            hdnScheduleDate.Value = temp[8];
            hdnScheduleTime.Value = temp[9];
            hdnGCDietType.Value = temp[10];

            DateTime date = Convert.ToDateTime(hdnScheduleDate.Value);
            string dateDay = date.Day.ToString();

            if (dateDay == "1" || dateDay == "11" || dateDay == "21")
            {
                hdnGCMealDayCtlOH.Value = Constant.StandardCode.MEAL_DATE + "^001";
            }
            else if (dateDay == "2" || dateDay == "12" || dateDay == "22")
            {
                hdnGCMealDayCtlOH.Value = Constant.StandardCode.MEAL_DATE + "^002";
            }
            else if (dateDay == "3" || dateDay == "13" || dateDay == "23")
            {
                hdnGCMealDayCtlOH.Value = Constant.StandardCode.MEAL_DATE + "^003";
            }
            else if (dateDay == "4" || dateDay == "14" || dateDay == "24")
            {
                hdnGCMealDayCtlOH.Value = Constant.StandardCode.MEAL_DATE + "^004";
            }
            else if (dateDay == "5" || dateDay == "15" || dateDay == "25")
            {
                hdnGCMealDayCtlOH.Value = Constant.StandardCode.MEAL_DATE + "^005";
            }
            else if (dateDay == "6" || dateDay == "16" || dateDay == "26")
            {
                hdnGCMealDayCtlOH.Value = Constant.StandardCode.MEAL_DATE + "^006";
            }
            else if (dateDay == "7" || dateDay == "17" || dateDay == "27")
            {
                hdnGCMealDayCtlOH.Value = Constant.StandardCode.MEAL_DATE + "^007";
            }
            else if (dateDay == "8" || dateDay == "18" || dateDay == "28")
            {
                hdnGCMealDayCtlOH.Value = Constant.StandardCode.MEAL_DATE + "^008";
            }
            else if (dateDay == "9" || dateDay == "19" || dateDay == "29")
            {
                hdnGCMealDayCtlOH.Value = Constant.StandardCode.MEAL_DATE + "^009";
            }
            else if (dateDay == "10" || dateDay == "20" || dateDay == "30")
            {
                hdnGCMealDayCtlOH.Value = Constant.StandardCode.MEAL_DATE + "^010";
            }
            else
            {
                hdnGCMealDayCtlOH.Value = Constant.StandardCode.MEAL_DATE + "^011";
            }

            hdnMRN.Value = AppSession.RegisteredPatient.MRN.ToString();

            SetControlProperties();
            BindGridView();
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(String.Format("ParentID IN ('{0}','{1}') AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.MEAL_TIME, Constant.StandardCode.MEAL_DATE));
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboMealTimeHistory, lstStandardCode.Where(x => x.ParentID == Constant.StandardCode.MEAL_TIME).ToList(), "StandardCodeName", "StandardCodeID");
            cboMealTimeHistory.SelectedIndex = 0;

            List<StandardCode> lstMealDay = BusinessLayer.GetStandardCodeList(String.Format("ParentID IN ('{0}','{1}') AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.MEAL_TIME, Constant.StandardCode.MEAL_DATE));
            lstMealDay.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboMealDayHistory, lstMealDay.Where(x => x.ParentID == Constant.StandardCode.MEAL_DATE).ToList(), "StandardCodeName", "StandardCodeID");
            cboMealDayHistory.SelectedIndex = 0;
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("VisitID = {0} AND GCTransactionStatus IN ('{1}')", hdnVisitID.Value, Constant.TransactionStatus.WAIT_FOR_APPROVAL);
            string nutritionOrderDtID = string.Empty;
            string filterListID = string.Empty;
            string filterExpressionDt = string.Empty;
            bool isCheck = chkUseScheduleDate.Checked;
            bool isIgnore = chkIgnoreMealTime.Checked;

            List<string> lstOrderHdID = new List<string>();

            List<vNutritionOrderHd> lstNutritionOrderHd = BusinessLayer.GetvNutritionOrderHdList(filterExpression);

            if (lstNutritionOrderHd.Count == 1)
            {
                foreach (vNutritionOrderHd orderHdID in lstNutritionOrderHd)
                {
                    nutritionOrderDtID += string.Format("{0}", orderHdID.NutritionOrderHdID);
                }
                filterListID = nutritionOrderDtID;
            }
            else if (lstNutritionOrderHd.Count > 1)
            {
                foreach (vNutritionOrderHd orderHdID in lstNutritionOrderHd)
                {
                    nutritionOrderDtID += string.Format("{0},", orderHdID.NutritionOrderHdID);
                }
                filterListID = nutritionOrderDtID.Remove(nutritionOrderDtID.Length - 1);
            }
            else
            {
                filterListID = string.Empty;
            }

            if (!isCheck)
            {
                if (lstNutritionOrderHd.Count != 0)
                {
                    if (!isIgnore)
                    {
                        filterExpressionDt += string.Format("GCMealTime = '{0}' AND GCMealDay = '{1}' AND NutritionOrderHdID IN ({2}) ORDER BY ScheduleDate DESC", cboMealTimeHistory.Value.ToString(), cboMealDayHistory.Value.ToString(), filterListID);
                    }
                    else
                    {
                        if (!isCheck)
                        {
                            filterExpressionDt += string.Format("GCMealDay = '{0}' AND NutritionOrderHdID IN ({1}) ORDER BY ScheduleDate DESC", cboMealDayHistory.Value.ToString(), filterListID);
                        }
                        else
                        {
                            filterExpressionDt += string.Format("GCMealTime = '{0}' AND ScheduleDate = '{1}' AND NutritionOrderHdID IN ({2}) ORDER BY ScheduleDate DESC", cboMealTimeHistory.Value.ToString(), hdnScheduleDate.Value, filterListID);
                        }
                    }
                }
                else
                {
                    filterExpressionDt += string.Format("NutritionOrderHdID IS NULL");
                }
            }
            else
            {
                if (lstNutritionOrderHd.Count != 0)
                {
                    if (!isIgnore)
                    {
                        filterExpressionDt += string.Format("GCMealTime = '{0}' AND ScheduleDate = '{1}' AND NutritionOrderHdID IN ({2}) ORDER BY ScheduleDate DESC", cboMealTimeHistory.Value.ToString(), hdnScheduleDate.Value, filterListID);
                    }
                    else
                    {
                        if (txtScheduleDate.Text == "")
                        {
                            filterExpressionDt += string.Format("NutritionOrderHdID IN ({1}) ORDER BY ScheduleDate DESC", hdnScheduleDate.Value, filterListID);
                        }
                        else
                        {
                            filterExpressionDt += string.Format("NutritionOrderHdID IN ({0}) AND ScheduleDate = '{1}' ORDER BY ScheduleDate DESC", filterListID, hdnScheduleDate.Value);
                        }
                    }
                }
                else
                {
                    filterExpressionDt += string.Format("NutritionOrderHdID IS NULL");
                }
            }

            List<vNutritionOrderDtHistory> lstNutritionOrderDt = BusinessLayer.GetvNutritionOrderDtHistoryList(filterExpressionDt);
            lstNutritionOrderDt = lstNutritionOrderDt.GroupBy(x => x.MealPlanID).Select(x => x.Last()).ToList();

            grdView.DataSource = lstNutritionOrderDt;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vNutritionOrderDt entity = e.Row.DataItem as vNutritionOrderDt;
            }
        }

        protected void cbpViewMealOrderHistory_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            errMessage = string.Empty;
            IDbContext ctx = DbFactory.Configure(true);
            NutritionOrderHdDao orderHdDao = new NutritionOrderHdDao(ctx);
            NutritionOrderDtDao orderDtDao = new NutritionOrderDtDao(ctx);
            NutritionOrderHdDietTypeDao orderDietDao = new NutritionOrderHdDietTypeDao(ctx);

            ItemMasterDao itemDao = new ItemMasterDao(ctx);
            //22-07-2019
            string orderDateYear = hdnOrderDate.Value.Substring(6, 4);
            string orderDateMonth = hdnOrderDate.Value.Substring(3, 2);
            string orderDateDay = hdnOrderDate.Value.Substring(0, 2);
            string orderDateFormat = string.Format("{0}/{1}/{2}", orderDateDay, orderDateMonth, orderDateYear);
            DateTime orderDate = DateTime.ParseExact(orderDateFormat, "dd/MM/yyyy", null);

            try
            {
                //List<PrescriptionOrderDt> lstPrescriptionOrderDt = new List<PrescriptionOrderDt>();
                List<NutritionOrderDt> lstNutritionOrderDt = new List<NutritionOrderDt>();

                int registrationID = Convert.ToInt32(hdnRegistrationID.Value);
                int visitID = Convert.ToInt32(hdnVisitID.Value);

                string[] lstSelectedMember = hdnSelectedMember.Value.Split(',');
                string[] lstSelectedMemberHd = hdnSelectedMemberHd.Value.Split(',');

                bool isValidQty = true;

                if (isValidQty)
                {
                    int dt = 0;
                    foreach (String nutritionOrderDtID in lstSelectedMember)
                    {
                        #region NutritionOrderDt

                        NutritionOrderDt entityOrderDt = BusinessLayer.GetNutritionOrderDtList(string.Format("NutritionOrderDtID = {0}", nutritionOrderDtID)).FirstOrDefault();

                        NutritionOrderDt orderDt = new NutritionOrderDt();

                        string tempItemID = BusinessLayer.GetNutritionOrderDt(Convert.ToInt32(nutritionOrderDtID)).MealPlanID.ToString();
                        string tempGCMealTime = BusinessLayer.GetNutritionOrderDt(Convert.ToInt32(nutritionOrderDtID)).GCMealTime;

                        orderDt.MealPlanID = Convert.ToInt32(entityOrderDt.MealPlanID);
                        //orderDt.MealPlanID = Convert.ToInt32(tempItemID);
                        //orderDt.MealPlanID = Convert.ToInt32(nutritionOrderDtID);
                        orderDt.ParamedicID = AppSession.RegisteredPatient.ParamedicID;
                        //orderDt.GCMealTime = cboMealTimeHistory.Value.ToString();
                        orderDt.GCMealTime = entityOrderDt.GCMealTime;
                        //orderDt.GCMealTime = tempGCMealTime;
                        orderDt.GCMealDay = hdnGCMealDayCtlOH.Value;
                        orderDt.ClassID = Convert.ToInt32(hdnClassID.Value);
                        orderDt.Remarks = string.Empty;
                        orderDt.GCMealStatus = entityOrderDt.GCMealStatus;
                        orderDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                        orderDt.IsNotForPatient = false;
                        orderDt.GCCarbohydrate = string.Empty;
                        orderDt.GCAnimalDish = string.Empty;
                        orderDt.GCVegetableDish = string.Empty;
                        orderDt.GCVegetables = string.Empty;
                        orderDt.GCFruits = string.Empty;
                        orderDt.GCFluid = string.Empty;
                        orderDt.CreatedBy = AppSession.UserLogin.UserID;
                        orderDt.CreatedDate = DateTime.Now;

                        lstNutritionOrderDt.Add(orderDt);

                        #endregion
                        dt++;
                    }

                    #region NutritionOrderHd
                    int hd = 0;
                    if (hdnNutritionOrderHdID.Value == "" || hdnNutritionOrderHdID.Value == "0")
                    {
                        #region NutritionOrderHd
                        NutritionOrderHd entityHd = new NutritionOrderHd();
                        entityHd.NutritionOrderDate = Convert.ToDateTime(orderDate);
                        entityHd.NutritionOrderTime = hdnOrderTime.Value;
                        entityHd.NutritionOrderNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.IP_NUTRITION_ORDER, entityHd.NutritionOrderDate, ctx);
                        entityHd.ScheduleDate = Convert.ToDateTime(hdnScheduleDate.Value);
                        entityHd.ScheduleTime = hdnScheduleTime.Value;
                        if (hdnGCDietType.Value != "")
                        {
                            entityHd.GCDietType = hdnGCDietType.Value;
                        }
                        entityHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                        entityHd.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                        entityHd.VisitHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                        entityHd.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
                        entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        entityHd.CreatedBy = AppSession.UserLogin.UserID;
                        entityHd.CreatedDate = DateTime.Now;

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        hd = orderHdDao.InsertReturnPrimaryKeyID(entityHd);

                        NutritionOrderHd entityOrderHd = BusinessLayer.GetNutritionOrderHdList(string.Format("VisitID = {0} AND GCTransactionStatus != '{1}' ORDER BY NutritionOrderHdID DESC", AppSession.RegisteredPatient.VisitID, Constant.TransactionStatus.VOID), ctx)[1];
                        List<NutritionOrderHdDietType> lstEntityDiet = BusinessLayer.GetNutritionOrderHdDietTypeList(string.Format("NutritionOrderHdID = {0}", entityOrderHd.NutritionOrderHdID), ctx);
                        if (lstEntityDiet.Count > 0)
                        {
                            foreach (NutritionOrderHdDietType obj in lstEntityDiet)
                            {
                                NutritionOrderHdDietType checkDiet = BusinessLayer.GetNutritionOrderHdDietTypeList(string.Format("NutritionOrderHdID = {0} AND GCDietType = '{1}'", hd, obj.GCDietType), ctx).FirstOrDefault();
                                if (checkDiet == null)
                                {
                                    NutritionOrderHdDietType oDiet = new NutritionOrderHdDietType();
                                    oDiet.NutritionOrderHdID = hd;
                                    oDiet.GCDietType = obj.GCDietType;
                                    orderDietDao.Insert(oDiet);
                                }
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        hd = Convert.ToInt32(hdnNutritionOrderHdID.Value);
                    }

                    if (orderHdDao.Get(hd).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        retval = orderHdDao.Get(hd).NutritionOrderNo;
                        for (int ctr = 0; ctr < lstNutritionOrderDt.Count(); ctr++)
                        {
                            lstNutritionOrderDt[ctr].NutritionOrderHdID = hd;
                            orderDtDao.Insert(lstNutritionOrderDt[ctr]);
                        }

                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
                        ctx.RollBackTransaction();
                    }
                    #endregion
                }
                else
                {
                    result = false;
                    errMessage = "Quantity Resep tidak boleh kurang dari atau sama dengan 0 !";
                    ctx.RollBackTransaction();
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
    }
}
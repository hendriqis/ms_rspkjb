using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;
using System.Data;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class NutritionOrderEditCtl : BaseViewPopupCtl
    {
        protected bool IsEditable = true;

        public override void InitializeDataControl(string param)
        {
            hdnNutritionOrderID.Value = param.ToString();
            String filterExpression = String.Format("NutritionOrderHdID = {0}", hdnNutritionOrderID.Value);
            vNutritionOrderHd entity = BusinessLayer.GetvNutritionOrderHdList(filterExpression)[0];
            txtNutritionOrderNo.Text = entity.NutritionOrderNo;
            txtDiagnoseID.Text = entity.DiagnoseID;
            txtDiagnoseName.Text = entity.DiagnoseName;
            txtOrderDate.Text = entity.NutritionOrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtOrderTime.Text = entity.NutritionOrderTime;
            hdnScheduleDate.Value = entity.ScheduleDate.ToString(Constant.FormatString.DATE_FORMAT);

            vConsultVisit entityVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", entity.VisitID))[0];
            txtReligion.Text = entityVisit.Religion;
            hdnParamedicIDDt.Value = entityVisit.ParamedicID.ToString();
            hdnParamedicCodeDt.Value = entityVisit.ParamedicCode;
            hdnParamedicNameDt.Value = entityVisit.ParamedicName;

            List<StandardCode> lstMealTime = BusinessLayer.GetStandardCodeList(String.Format("ParentID IN ('{0}','{1}') AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.MEAL_TIME, Constant.StandardCode.MEAL_DATE));
            Methods.SetComboBoxField<StandardCode>(cboMealTime, lstMealTime.Where(x => x.ParentID == Constant.StandardCode.MEAL_TIME).ToList(), "StandardCodeName", "StandardCodeID");

            List<StandardCode> lstMealStatus = BusinessLayer.GetStandardCodeList(String.Format("ParentID IN ('{0}','{1}') AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.MEAL_STATUS, Constant.StandardCode.MEAL_DATE));
            Methods.SetComboBoxField<StandardCode>(cboMealStatus, lstMealStatus.Where(x => x.ParentID == Constant.StandardCode.MEAL_STATUS).ToList(), "StandardCodeName", "StandardCodeID");

            txtMealDay.Attributes.Add("readonly", "readonly");

            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = "1 = 0";
            if (hdnNutritionOrderID.Value != "" && hdnNutritionOrderID.Value != "0")
                filterExpression = string.Format("NutritionOrderHdID = {0} AND GCItemDetailStatus != '{1}'", hdnNutritionOrderID.Value, Constant.TransactionStatus.VOID);
            List<vNutritionOrderDt> lstEntity = BusinessLayer.GetvNutritionOrderDtList(filterExpression);
            grdPopupView.DataSource = lstEntity;
            grdPopupView.DataBind();
        }

        #region Process
        protected void cbpPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridView();
                result = "refresh";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpPopupProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int OrderID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                if (hdnEntryID.Value.ToString() != "")
                {
                    if (OnSaveEditRecordEntityDt(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnAddRecordEntityDt(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "delete")
            {
                if (OnDeleteRecordEntityDt(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpNutritionOrderID"] = OrderID.ToString();
        }
        #endregion

        #region Process Detail
        private void ControlToEntity(NutritionOrderDt entityDt)
        {
            entityDt.GCMealTime = cboMealTime.Value.ToString();
            if (cboMealStatus.Value != null)
            {
                entityDt.GCMealStatus = cboMealStatus.Value.ToString();
            }
            entityDt.MealPlanID = Convert.ToInt32(hdnMealPlanID.Value);
            entityDt.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
            entityDt.Remarks = txtRemarks.Text;
        }

        private bool OnAddRecordEntityDt(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            NutritionOrderDtDao entityDtDao = new NutritionOrderDtDao(ctx);
            try
            {
                NutritionOrderHd entityHd = BusinessLayer.GetNutritionOrderHd(Convert.ToInt32(hdnNutritionOrderID.Value));
                ConsultVisit entityCV = BusinessLayer.GetConsultVisit(entityHd.VisitID);

                NutritionOrderDt entityDt = new NutritionOrderDt();
                ControlToEntity(entityDt);
                entityDt.NutritionOrderHdID = Convert.ToInt32(hdnNutritionOrderID.Value);
                if (txtMealDay.Text.Length != 2)
                {
                    entityDt.GCMealDay = Constant.StandardCode.MEAL_DATE + "^00" + txtMealDay.Text;
                }
                else
                {
                    entityDt.GCMealDay = Constant.StandardCode.MEAL_DATE + "^0" + txtMealDay.Text;
                }
                entityDt.ClassID = Convert.ToInt32(entityCV.ChargeClassID);
                entityDt.GCItemDetailStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                entityDt.CreatedBy = AppSession.UserLogin.UserID;
                entityDt.CreatedDate = DateTime.Now;
                entityDtDao.InsertReturnPrimaryKeyID(entityDt);
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

        private bool OnSaveEditRecordEntityDt(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            NutritionOrderDtDao entityDtDao = new NutritionOrderDtDao(ctx);
            try
            {
                NutritionOrderDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                ControlToEntity(entityDt);
                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDtDao.Update(entityDt);
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

        private bool OnDeleteRecordEntityDt(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            NutritionOrderDtDao entityDao = new NutritionOrderDtDao(ctx);
            try
            {
                NutritionOrderDt entityDt = entityDao.Get(Convert.ToInt32(hdnEntryID.Value));
                entityDt.GCItemDetailStatus = Constant.TransactionStatus.VOID;
                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entityDt);

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
        #endregion
    }
}
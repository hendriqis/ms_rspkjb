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

namespace QIS.Medinfras.Web.Nutrition.Program
{
    public partial class NutritionOrderEditCtlLink : BaseViewPopupCtl
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
            txtReligion.Text = entity.Religion;

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(String.Format("ParentID IN ('{0}','{1}','{2}') AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.MEAL_TIME, Constant.StandardCode.MEAL_DATE, Constant.StandardCode.MEAL_STATUS));
            Methods.SetComboBoxField<StandardCode>(cboMealTime, lstStandardCode.Where(x => x.ParentID == Constant.StandardCode.MEAL_TIME).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboMealStatus, lstStandardCode.Where(x => x.ParentID == Constant.StandardCode.MEAL_STATUS).ToList(), "StandardCodeName", "StandardCodeID");
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
            }
            else if (param[0] == "delete")
            {
                OrderID = Convert.ToInt32(hdnEntryID.Value);
                if (OnDeleteEntityDt(ref errMessage, OrderID))
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
            entityDt.GCMealStatus = cboMealStatus.Value.ToString();
            entityDt.MealPlanID = Convert.ToInt32(hdnMealPlanID.Value);
            entityDt.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
            entityDt.Remarks = txtRemarks.Text;
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
        private bool OnDeleteEntityDt(ref string errMessage, int ID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            NutritionOrderDtDao entityDtDao = new NutritionOrderDtDao(ctx);
            try
            {
                NutritionOrderDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                entityDt.GCItemDetailStatus = Constant.TransactionStatus.VOID;
                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDtDao.Update(entityDt);
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                result = false;
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
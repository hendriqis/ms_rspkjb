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
using QIS.Data.Core.Dal;
using System.Data;
using QIS.Medinfras.Web.CommonLibs.Program;

namespace QIS.Medinfras.Web.Nutrition.Program
{
    public partial class NutritionOrderEntry : BasePageTrx
    {
        protected int PageCount = 1;
        protected bool IsEditable = true;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Nutrition.NUTRITION_ORDER_INPATIENT;
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString.Count > 0)
            {
                hdnVisitID.Value = Page.Request.QueryString["id"];
                vConsultVisit2 entity = BusinessLayer.GetvConsultVisit2List(string.Format("VisitID = {0}", hdnVisitID.Value))[0];
                hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
                hdnDefaultChargeClassID.Value = entity.ChargeClassID.ToString();
                hdnParamedicID.Value = entity.ParamedicID.ToString();
                txtParamedicCode.Text = entity.ParamedicCode;
                txtParamedicName.Text = entity.ParamedicName;
                txtAgama.Text = entity.Religion;
                ctlPatientBanner.InitializePatientBanner(entity);

                if (OnGetRowCount() > 0)
                    IsLoadFirstRecord = true;
                else
                {
                    IsLoadFirstRecord = false;
                    OnAddRecord();
                }


                int day = DateTime.Now.Day;
                //hdnMealDay.Value = day.ToString();
                if (day == 31)
                    hdnMealDay.Value = Constant.StandardCode.MEAL_DATE + "^011";
                else if (day % 10 == 0)
                    hdnMealDay.Value = Constant.StandardCode.MEAL_DATE + "^010";
                else
                    hdnMealDay.Value = Constant.StandardCode.MEAL_DATE + String.Format("^00{0}", (day % 10).ToString());


                Helper.SetControlEntrySetting(cboMealTime, new ControlEntrySetting(true, true, true), "mpTrx");
                Helper.SetControlEntrySetting(cboMealDay, new ControlEntrySetting(true, true, true), "mpTrx");
                Helper.SetControlEntrySetting(txtMealPlanCode, new ControlEntrySetting(true, true, true), "mpTrx");
                Helper.SetControlEntrySetting(txtParamedicCode, new ControlEntrySetting(true, true, true), "mpTrx");
            }
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(String.Format("ParentID IN ('{0}','{1}') AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.MEAL_TIME, Constant.StandardCode.MEAL_DATE));
            Methods.SetComboBoxField<StandardCode>(cboMealTime, lstStandardCode.Where(x => x.ParentID == Constant.StandardCode.MEAL_TIME).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboMealDay, lstStandardCode.Where(x => x.ParentID == Constant.StandardCode.MEAL_DATE).ToList(), "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnNutritionOrderID, new ControlEntrySetting(false, false, false, "0"));

            SetControlEntrySetting(txtNutritionOrderNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtDiagnoseID, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDiagnoseName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtOrderDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtOrderTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
        }

        #region Load Entity
        public override void OnAddRecord()
        {
            string filterExpression = "1 = 0";
            List<vNutritionOrderDt> lstEntity = BusinessLayer.GetvNutritionOrderDtList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        private string GetFilterExpression()
        {
            string filterExpression = "";
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID = {0}", hdnVisitID.Value);
            return filterExpression;
        }
        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            vNutritionOrderHd entity = BusinessLayer.GetvNutritionOrderHd(filterExpression, PageIndex, " NutritionOrderHdID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();

            PageIndex = BusinessLayer.GetvNutritionOrderHdRowIndex(filterExpression, keyValue, "NutritionOrderHdID DESC");
            vNutritionOrderHd entity = BusinessLayer.GetvNutritionOrderHd(filterExpression, PageIndex, "NutritionOrderHdID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        public override int OnGetRowCount()
        {
            string filterExpression = GetFilterExpression();
            return BusinessLayer.GetvNutritionOrderHdRowCount(filterExpression);
        }

        private void EntityToControl(vNutritionOrderHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                watermarkText = entity.TransactionStatusWatermark;
            }
            IsEditable = (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN);
            hdnNutritionOrderID.Value = entity.NutritionOrderHdID.ToString();
            txtNutritionOrderNo.Text = entity.NutritionOrderNo;
            txtOrderDate.Text = entity.NutritionOrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtOrderTime.Text = entity.NutritionOrderTime;
            txtDiagnoseID.Text = entity.DiagnoseID;
            txtDiagnoseName.Text = entity.DiagnoseName;
            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = "1 = 0";
            if (hdnNutritionOrderID.Value != "" && hdnNutritionOrderID.Value != "0")
                filterExpression = string.Format("NutritionOrderHdID = {0} AND GCItemDetailStatus != '{1}'", hdnNutritionOrderID.Value, Constant.TransactionStatus.VOID);
            List<vNutritionOrderDt> lstEntity = BusinessLayer.GetvNutritionOrderDtList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
        #endregion

        #region Save & Edit Header
        public void SaveOrderHd(IDbContext ctx, ref int NutritionOrderID)
        {
            NutritionOrderHdDao entityHdDao = new NutritionOrderHdDao(ctx);
            if (hdnNutritionOrderID.Value == "" || hdnNutritionOrderID.Value == "0")
            {
                NutritionOrderHd entityHd = new NutritionOrderHd();
                entityHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                entityHd.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                entityHd.NutritionOrderDate = Helper.GetDatePickerValue(Request.Form[txtOrderDate.UniqueID]);
                entityHd.NutritionOrderTime = Request.Form[txtOrderTime.UniqueID];
                entityHd.ReferenceNo = "";
                entityHd.LinkField = "";
                entityHd.DiagnoseID = txtDiagnoseID.Text;
                entityHd.NutritionOrderNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.IP_NUTRITION_ORDER, entityHd.NutritionOrderDate, ctx);
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();

                entityHd.CreatedBy = AppSession.UserLogin.UserID;

                entityHdDao.Insert(entityHd);

                NutritionOrderID = BusinessLayer.GetNutritionOrderHdMaxID(ctx);
            }
            else
            {
                NutritionOrderID = Convert.ToInt32(hdnNutritionOrderID.Value);
            }
        }
        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                int NutritionOrderID = 0;
                SaveOrderHd(ctx, ref NutritionOrderID);
                retval = NutritionOrderID.ToString();
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

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            try
            {
                NutritionOrderHd entity = BusinessLayer.GetNutritionOrderHd(Convert.ToInt32(hdnNutritionOrderID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entity.DiagnoseID = txtDiagnoseID.Text;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateNutritionOrderHd(entity);
                    result = true;
                }
                else
                {
                    errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
                }
                return result;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        #region Propose Entity
        protected override bool OnProposeRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            NutritionOrderHdDao entityHdDao = new NutritionOrderHdDao(ctx);
            NutritionOrderDtDao entityDtDao = new NutritionOrderDtDao(ctx);
            try
            {
                Int32 NutritionOrderID = Convert.ToInt32(hdnNutritionOrderID.Value);
                NutritionOrderHd entity = entityHdDao.Get(NutritionOrderID);
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    List<NutritionOrderDt> lstDt = BusinessLayer.GetNutritionOrderDtList(String.Format("NutritionOrderHdID = {0}", entity.NutritionOrderHdID), ctx);
                    foreach (NutritionOrderDt obj in lstDt)
                    {
                        if (obj.GCItemDetailStatus != Constant.TransactionStatus.VOID)
                            obj.GCItemDetailStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                        obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(obj);
                    }
                    entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entity);
                    ctx.CommitTransaction();
                }
                else 
                {
                    result = false;
                    errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
                    ctx.RollBackTransaction();                
                }
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

        #region Void Entity
        protected override bool OnVoidRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            NutritionOrderHdDao entityHdDao = new NutritionOrderHdDao(ctx);
            NutritionOrderDtDao entityDtDao = new NutritionOrderDtDao(ctx);
            try
            {
                Int32 NutritionOrderID = Convert.ToInt32(hdnNutritionOrderID.Value);
                NutritionOrderHd entity = entityHdDao.Get(NutritionOrderID);
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    List<NutritionOrderDt> lstDt = BusinessLayer.GetNutritionOrderDtList(String.Format("NutritionOrderHdID = {0}", entity.NutritionOrderHdID), ctx);
                    foreach (NutritionOrderDt obj in lstDt)
                    {
                        if (obj.GCItemDetailStatus == Constant.TransactionStatus.OPEN)
                        {
                            obj.GCItemDetailStatus = Constant.TransactionStatus.VOID;
                            obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(obj);
                        }
                    }
                    entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entity);
                    ctx.CommitTransaction();
                }
                else 
                {
                    result = false;
                    errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
                    ctx.RollBackTransaction();                
                }
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
        #endregion

        #region Process
        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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

        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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
                    OrderID = Convert.ToInt32(hdnNutritionOrderID.Value);
                    if (OnSaveEditRecordEntityDt(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecordEntityDt(ref errMessage, ref OrderID))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "delete")
            {
                OrderID = Convert.ToInt32(hdnNutritionOrderID.Value);
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
            entityDt.GCMealDay = cboMealDay.Value.ToString();
            entityDt.MealPlanID = Convert.ToInt32(hdnMealPlanID.Value);
            entityDt.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
            entityDt.ClassID = Convert.ToInt32(hdnDefaultChargeClassID.Value);
            entityDt.Remarks = txtRemarks.Text;
        }
        private bool OnSaveAddRecordEntityDt(ref string errMessage, ref int NutritionOrderID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            NutritionOrderDtDao entityDtDao = new NutritionOrderDtDao(ctx);
            NutritionOrderDt entityDt = new NutritionOrderDt();
            try
            {
                if (hdnNutritionOrderID.Value.ToString() == "" || hdnNutritionOrderID.Value.ToString() == "0")
                {
                    SaveOrderHd(ctx, ref NutritionOrderID);
                    ControlToEntity(entityDt);
                    entityDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                    entityDt.NutritionOrderHdID = NutritionOrderID;
                    entityDt.CreatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Insert(entityDt);
                    ctx.CommitTransaction();
                }
                else if (hdnNutritionOrderID.Value.ToString() != "" || hdnNutritionOrderID.Value.ToString() != "0")
                {
                    if (BusinessLayer.GetNutritionOrderHd(Convert.ToInt32(hdnNutritionOrderID.Value.ToString())).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        SaveOrderHd(ctx, ref NutritionOrderID);
                        ControlToEntity(entityDt);
                        entityDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                        entityDt.NutritionOrderHdID = NutritionOrderID;
                        entityDt.CreatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Insert(entityDt);
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
                        ctx.RollBackTransaction();
                    }
                }
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
                if (BusinessLayer.GetNutritionOrderHd(entityDt.NutritionOrderHdID).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    if (entityDt.GCItemDetailStatus == Constant.TransactionStatus.OPEN)
                    {
                        ControlToEntity(entityDt);
                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(entityDt);
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
                    ctx.RollBackTransaction();
                }

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
                if (BusinessLayer.GetNutritionOrderHd(entityDt.NutritionOrderHdID).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    if (entityDt.GCItemDetailStatus == Constant.TransactionStatus.OPEN)
                    {
                        entityDt.GCItemDetailStatus = Constant.TransactionStatus.VOID;
                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(entityDt);
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
                    ctx.RollBackTransaction();
                }
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
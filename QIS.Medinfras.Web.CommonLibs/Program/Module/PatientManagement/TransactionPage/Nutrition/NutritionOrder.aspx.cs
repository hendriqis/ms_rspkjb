using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
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
    public partial class NutritionOrder : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            string type = string.Empty;
            string[] param = Page.Request.QueryString["id"].Split('|');
            type = param[0];

            switch (type)
            {
                case Constant.Facility.INPATIENT:
                    return Constant.MenuCode.Inpatient.PATIENT_TRANSACTION_NUTRITION_ORDER;
                case Constant.Facility.DIAGNOSTIC:
                    return Constant.MenuCode.MedicalDiagnostic.DIAGNOSTIC_NUTRITION_ORDER;
                case Constant.Facility.EMERGENCY:
                    return Constant.MenuCode.EmergencyCare.PATIENT_TRANSACTION_NUTRITION_ORDER;
                case Constant.Facility.OUTPATIENT:
                    return Constant.MenuCode.Outpatient.PATIENT_TRANSACTION_NUTRITION_ORDER;
                default:
                    return Constant.MenuCode.Nutrition.NUTRITION_ORDER;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected string GetServiceUnitFilterFilterExpression()
        {
            return string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND HealthcareServiceUnitID != {2} AND IsDeleted = 0 AND IsUsingRegistration = 1", AppSession.UserLogin.HealthcareID, Constant.Facility.DIAGNOSTIC, AppSession.RegisteredPatient.HealthcareServiceUnitID);
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = (hdnGCRegistrationStatus.Value != Constant.VisitStatus.CLOSED && !AppSession.RegisteredPatient.IsLockDown);
            IsAllowSave = !AppSession.RegisteredPatient.IsLockDown;
            IsAllowVoid = !AppSession.RegisteredPatient.IsLockDown;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();

            hdnDepartmentID.Value = AppSession.RegisteredPatient.DepartmentID;
            hdnGCRegistrationStatus.Value = AppSession.RegisteredPatient.GCRegistrationStatus;
            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnClassID.Value = AppSession.RegisteredPatient.ClassID.ToString();
            hdnHealthcareServiceUnitID.Value = AppSession.RegisteredPatient.HealthcareServiceUnitID.ToString();

            List<SettingParameterDt> lstSettingParameter = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                                    "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
                                                                    AppSession.UserLogin.HealthcareID,
                                                                    Constant.SettingParameter.NT_PANEL_MAKAN_BY_DAY,
                                                                    Constant.SettingParameter.NT_DEFAULT_ORDER_DATE
                                                                    ));

            hdnIsMealPlanByDay.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.NT_PANEL_MAKAN_BY_DAY).ParameterValue;
            hdnIsDefaultOrderDate.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.NT_DEFAULT_ORDER_DATE).ParameterValue;

            SetControlProperties();

            if (AppSession.UserLogin.ParamedicID != null)
            {
                ParamedicMaster oParamedic = BusinessLayer.GetParamedicMaster(Convert.ToInt32(AppSession.UserLogin.ParamedicID));
                if (oParamedic != null)
                {
                    hdnDefaultParamedicID.Value = oParamedic.ParamedicID.ToString();
                    txtParamedicCode.Text = oParamedic.ParamedicCode;
                    hdnDefaultParamedicCode.Value = oParamedic.ParamedicCode;
                    txtParamedicName.Text = oParamedic.FullName;
                    hdnDefaultParamedicName.Value = oParamedic.FullName;
                }

            }
            IsEditable = AppSession.RegisteredPatient.IsLockDown ? false : true;

            Helper.SetControlEntrySetting(txtItemCode, new ControlEntrySetting(true, true, true), "mpTrxPopup");

            txtOrderDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtOrderTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            if (hdnIsDefaultOrderDate.Value == "1")
            {
                txtScheduleDate.Text = DateTime.Now.Date.AddDays(1).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            }
            else
            {
                txtScheduleDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            }
            txtScheduleTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            List<NutritionSGA> entitySGA = BusinessLayer.GetNutritionSGAList(string.Format("VisitID = {0} AND IsDeleted = 0 ORDER BY ID DESC", hdnVisitID.Value));
            if (entitySGA.Count != 0)
            {
                cboDietType.Value = entitySGA.FirstOrDefault().GCDietType;
                hdnItemIndex.Value = cboDietType.Value.ToString();
            }
            else
            {
                cboDietType.SelectedIndex = 0;
                cboDietType.Value = string.Empty;
            }

            NutritionNotes obj = BusinessLayer.GetNutritionNotesList(string.Format("VisitID = {0} AND IsDeleted = 0 ORDER BY ID DESC", hdnVisitID.Value)).FirstOrDefault();
            if (obj != null)
            {
                hdnDefaultInterventionNotes.Value = string.Format("Catatan Intervensi Gizi : {0}{1}", Environment.NewLine, obj.OtherRemarks);
            }
            else
            {
                hdnDefaultInterventionNotes.Value = string.Empty;
            }

            txtNumberOfCalories.Text = "0.00";
            txtNumberOfProtein.Text = "0.00";

            string moduleName = Helper.GetModuleName();
            string ModuleID = Helper.GetModuleID(moduleName);
            GetUserMenuAccess menu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault();
            string CRUDMode = menu.CRUDMode;
            hdnIsAllowVoid.Value = CRUDMode.Contains("X") ? "1" : "0";

            BindGridView();

            IsLoadFirstRecord = (OnGetRowCount() > 0);
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

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vNutritionOrderDt entity = e.Row.DataItem as vNutritionOrderDt;
                if (entity.IsNotForPatient)
                {
                    e.Row.BackColor = Color.Pink;
                }
                else
                {
                    e.Row.BackColor = Color.Transparent;
                }
            }
        }

        private void BindGridView()
        {
            string filterExpression = "1 = 0";
            if (hdnOrderID.Value != "" && hdnOrderID.Value != "0")
                filterExpression = string.Format("NutritionOrderHdID = {0} AND GCItemDetailStatus != 'X121^999' ORDER BY NutritionOrderDtID ASC", hdnOrderID.Value);
            List<vNutritionOrderDt> lstEntity = BusinessLayer.GetvNutritionOrderDtList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

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

        protected override void SetControlProperties()
        {
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(String.Format("ParentID IN ('{0}','{1}', '{2}') AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.MEAL_TIME, Constant.StandardCode.MEAL_DATE, Constant.StandardCode.MEAL_STATUS));
            Methods.SetComboBoxField<StandardCode>(cboMealTime, lstStandardCode.Where(x => x.ParentID == Constant.StandardCode.MEAL_TIME).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboMealStatus, lstStandardCode.Where(x => x.ParentID == Constant.StandardCode.MEAL_STATUS).ToList(), "StandardCodeName", "StandardCodeID");
            //Methods.SetComboBoxField<StandardCode>(cboMealDay, lstStandardCode.Where(x => x.ParentID == Constant.StandardCode.MEAL_DATE).ToList(), "StandardCodeName", "StandardCodeID");

            List<StandardCode> lstDietType = BusinessLayer.GetStandardCodeList(String.Format("ParentID IN ('{0}') AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.DIET_TYPE));
            lstDietType.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboDietType, lstDietType.Where(x => x.ParentID == Constant.StandardCode.DIET_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            cboDietType.SelectedIndex = 0;

        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnOrderID, new ControlEntrySetting(false, false, false, "0"));

            SetControlEntrySetting(txtOrderNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtOrderDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtOrderTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            if (hdnIsDefaultOrderDate.Value == "1")
            {
                SetControlEntrySetting(txtScheduleDate, new ControlEntrySetting(true, false, true, DateTime.Now.AddDays(1).ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            }
            else
            {
                SetControlEntrySetting(txtScheduleDate, new ControlEntrySetting(true, false, true, DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            }
            SetControlEntrySetting(txtScheduleTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));

            SetControlEntrySetting(hdnParamedicID, new ControlEntrySetting(true, false, false, hdnDefaultParamedicID.Value));
            SetControlEntrySetting(txtParamedicCode, new ControlEntrySetting(true, false, true, hdnDefaultParamedicCode.Value));
            SetControlEntrySetting(txtParamedicName, new ControlEntrySetting(false, false, false, hdnDefaultParamedicName.Value));
            SetControlEntrySetting(txtDiagnoseID, new ControlEntrySetting(true, false, false, ""));
            SetControlEntrySetting(txtDiagnoseName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(cboDietType, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(lblParamedic, new ControlEntrySetting(true, false));
            SetControlEntrySetting(txtNumberOfCalories, new ControlEntrySetting(true, true, false, "0.00"));
            SetControlEntrySetting(txtNumberOfProtein, new ControlEntrySetting(true, true, false, "0.00"));
        }

        public override void OnAddRecord()
        {
            hdnGCTransactionStatus.Value = Constant.TransactionStatus.OPEN;
            IsEditable = true;
            string filterExpression = "1 = 0";
            List<vNutritionOrderDt> lstEntity = BusinessLayer.GetvNutritionOrderDtList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();

            txtNotes.Text = hdnDefaultInterventionNotes.Value;

            divCreatedBy.InnerHtml = string.Empty;
            divCreatedDate.InnerHtml = string.Empty;
            divProposedBy.InnerHtml = string.Empty;
            divProposedDate.InnerHtml = string.Empty;
            divVoidBy.InnerHtml = string.Empty;
            divVoidDate.InnerHtml = string.Empty;
            divLastUpdatedBy.InnerHtml = string.Empty;
            divLastUpdatedDate.InnerHtml = string.Empty;
            divVoidReason.InnerHtml = string.Empty;
            trProposedBy.Style.Add("display", "none");
            trProposedDate.Style.Add("display", "none");
            trVoidBy.Style.Add("display", "none");
            trVoidDate.Style.Add("display", "none");
            trVoidReason.Style.Add("display", "none");
        }

        public override int OnGetRowCount()
        {
            string filterExpression = string.Format("VisitID = {0}", hdnVisitID.Value);
            return BusinessLayer.GetvNutritionOrderHdRowCount(filterExpression);
        }

        #region Load Entity
        protected bool IsEditable = true;
        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = string.Format("VisitID = {0}", hdnVisitID.Value);
            vNutritionOrderHd entity = BusinessLayer.GetvNutritionOrderHd(filterExpression, PageIndex, " NutritionOrderHdID DESC");
            if (entity != null)
            {
                EntityToControl(entity, ref isShowWatermark, ref watermarkText);
            }
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = string.Format("VisitID = {0}", hdnVisitID.Value);
            PageIndex = BusinessLayer.GetvNutritionOrderHdRowIndex(filterExpression, keyValue, "NutritionOrderHdID DESC");
            vNutritionOrderHd entity = BusinessLayer.GetvNutritionOrderHd(filterExpression, PageIndex, " NutritionOrderHdID DESC");
            if (entity != null)
            {
                EntityToControl(entity, ref isShowWatermark, ref watermarkText);
            }
        }

        private void EntityToControl(vNutritionOrderHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                watermarkText = entity.TransactionStatusWatermark;
            }
            IsEditable = (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN);
            Registration entityRegistration = BusinessLayer.GetRegistration(Convert.ToInt32(hdnRegistrationID.Value));
            IsEditable = entityRegistration.IsLockDown ? false : IsEditable;
            hdnGCTransactionStatus.Value = entity.GCTransactionStatus;
            hdnOrderID.Value = entity.NutritionOrderHdID.ToString();
            txtOrderNo.Text = entity.NutritionOrderNo;
            txtOrderDate.Text = entity.NutritionOrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtOrderTime.Text = entity.NutritionOrderTime;
            txtScheduleDate.Text = entity.ScheduleDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtScheduleTime.Text = entity.ScheduleTime;
            hdnParamedicID.Value = entity.ParamedicID.ToString();
            txtParamedicCode.Text = entity.ParamedicCode;
            txtParamedicName.Text = entity.ParamedicName;
            txtDiagnoseID.Text = entity.DiagnoseID;
            txtDiagnoseName.Text = entity.DiagnoseName;
            txtNotes.Text = entity.Remarks;
            cboDietType.Value = entity.GCDietType;
            txtNumberOfCalories.Text = entity.NumberOfCalories.ToString();
            txtNumberOfProtein.Text = entity.NumberOfProtein.ToString();

            divCreatedBy.InnerHtml = entity.CreatedBy;
            divCreatedDate.InnerHtml = entity.CreatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            divLastUpdatedBy.InnerHtml = entity.LastUpdateBy;
            if (entity.LastUpdatedDate != null && entity.LastUpdatedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
            {
                divLastUpdatedDate.InnerHtml = entity.LastUpdatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }

            if (entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
            {
                divProposedBy.InnerHtml = entity.ProposedBy;
                if (entity.ProposedDate != null && entity.ProposedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
                {
                    divProposedDate.InnerHtml = entity.ProposedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
                }

                trProposedBy.Style.Remove("display");
                trProposedDate.Style.Remove("display");
            }
            else
            {
                trProposedBy.Style.Add("display", "none");
                trProposedDate.Style.Add("display", "none");
            }

            if (entity.GCTransactionStatus == Constant.TransactionStatus.VOID)
            {
                divVoidBy.InnerHtml = entity.VoidBy;
                if (entity.VoidDate != null && entity.VoidDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
                {
                    divVoidDate.InnerHtml = entity.VoidDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
                }

                string voidReason = "";

                if (entity.GCVoidReason == Constant.DeleteReason.OTHER)
                {
                    voidReason = entity.VoidReasonWatermark + " ( " + entity.VoidReason + " )";
                }
                else
                {
                    voidReason = entity.VoidReasonWatermark;
                }

                trVoidBy.Style.Remove("display");
                trVoidDate.Style.Remove("display");
                divVoidReason.InnerHtml = voidReason;
                trVoidReason.Style.Remove("display");
            }
            else
            {
                trVoidBy.Style.Add("display", "none");
                trVoidDate.Style.Add("display", "none");
                trVoidReason.Style.Add("display", "none");
            }

            BindGridView();
        }
        #endregion

        #region Save Entity
        public void SaveNutrionOrderHd(IDbContext ctx, ref int orderID)
        {
            object cbo = cboDietType.Value;
            NutritionOrderHdDao entityHdDao = new NutritionOrderHdDao(ctx);
            NutritionOrderHdDietTypeDao entityHddtDao = new NutritionOrderHdDietTypeDao(ctx);

            if (hdnOrderID.Value == "0" || hdnOrderID.Value == "" || hdnOrderID.Value == null)
            {
                NutritionOrderHd entityHd = new NutritionOrderHd();
                NutritionOrderHdDietType entityHddt = new NutritionOrderHdDietType();
                entityHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                entityHd.NutritionOrderDate = Helper.GetDatePickerValue(Request.Form[txtOrderDate.UniqueID]);
                entityHd.NutritionOrderTime = Request.Form[txtOrderTime.UniqueID];
                entityHd.ScheduleDate = Helper.GetDatePickerValue(Request.Form[txtScheduleDate.UniqueID]);
                entityHd.ScheduleTime = Request.Form[txtScheduleTime.UniqueID];
                entityHd.HealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                entityHd.VisitHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                entityHd.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
                entityHd.ReferenceNo = "";
                entityHd.LinkField = "";
                entityHd.Remarks = txtNotes.Text;

                if (Request.Form[txtDiagnoseID.UniqueID] != null && Request.Form[txtDiagnoseID.UniqueID] != "")
                {
                    entityHd.DiagnoseID = Request.Form[txtDiagnoseID.UniqueID];
                }
                else
                {
                    entityHd.DiagnoseID = txtDiagnoseID.Text;
                }

                if (Request.Form[txtDiagnoseName.UniqueID] != null && Request.Form[txtDiagnoseName.UniqueID] != "")
                {
                    entityHd.DiagnoseText = Request.Form[txtDiagnoseName.UniqueID];
                }
                else
                {
                    entityHd.DiagnoseText = txtDiagnoseName.Text;
                }

                if (cboDietType.Value != null && cboDietType.Value != "")
                {
                    entityHd.GCDietType = cboDietType.Value.ToString();
                }
                else
                {
                    entityHd.GCDietType = null;
                }

                entityHd.NutritionOrderNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.IP_NUTRITION_ORDER, entityHd.NutritionOrderDate, ctx);
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                entityHd.NumberOfCalories = Convert.ToDecimal(Request.Form[txtNumberOfCalories.UniqueID]);
                entityHd.NumberOfProtein = Convert.ToDecimal(Request.Form[txtNumberOfProtein.UniqueID]);

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();

                entityHd.CreatedBy = AppSession.UserLogin.UserID;

                orderID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);

                if (cboDietType.Value != null && cboDietType.Value != "")
                {
                    entityHddt.NutritionOrderHdID = orderID;
                    entityHddt.GCDietType = cboDietType.Value.ToString();
                    entityHddtDao.Insert(entityHddt);
                }
            }
            else
            {
                orderID = Convert.ToInt32(hdnOrderID.Value);
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                int orderID = 0;
                SaveNutrionOrderHd(ctx, ref orderID);

                retval = orderID.ToString();
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

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            NutritionOrderHdDao entityHdDao = new NutritionOrderHdDao(ctx);
            NutritionOrderHdDietTypeDao entityHdDietTypeDao = new NutritionOrderHdDietTypeDao(ctx);
            try
            {
                NutritionOrderHd entity = entityHdDao.Get(Convert.ToInt32(hdnOrderID.Value));
                NutritionOrderHdDietType entityHddt = new NutritionOrderHdDietType();
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    if (cboDietType.Value != null && cboDietType.Value != "")
                    {
                        String tempDietType = String.Empty;
                        if (entity.GCDietType == cboDietType.Value.ToString())
                            tempDietType = entity.GCDietType;
                        else if (entity.GCDietType != cboDietType.Value.ToString())
                            tempDietType = cboDietType.Value.ToString();

                        NutritionOrderHdDietType entityDietType = BusinessLayer.GetNutritionOrderHdDietType(Convert.ToInt32(hdnOrderID.Value), tempDietType);
                        if (entityDietType == null)
                        {
                            tempDietType = entity.GCDietType;
                            NutritionOrderHdDietType entityTemp = BusinessLayer.GetNutritionOrderHdDietType(Convert.ToInt32(hdnOrderID.Value), tempDietType);
                            if (entityTemp == null)
                            {
                                entityHddt.NutritionOrderHdID = entity.NutritionOrderHdID;
                                entityHddt.GCDietType = cboDietType.Value.ToString();
                                entityHdDietTypeDao.Insert(entityHddt);
                            }
                            else
                            {
                                tempDietType = entity.GCDietType;
                                BusinessLayer.DeleteNutritionOrderHdDietType(Convert.ToInt32(hdnOrderID.Value), tempDietType);
                                entityHddt.NutritionOrderHdID = entity.NutritionOrderHdID;
                                entityHddt.GCDietType = cboDietType.Value.ToString();
                                entityHdDietTypeDao.Insert(entityHddt);
                            }
                        }
                        else
                        {
                            if (entityDietType.GCDietType == entity.GCDietType)
                            {
                                tempDietType = entity.GCDietType;
                                BusinessLayer.DeleteNutritionOrderHdDietType(Convert.ToInt32(hdnOrderID.Value), tempDietType);
                                entityHddt.NutritionOrderHdID = entity.NutritionOrderHdID;
                                entityHddt.GCDietType = cboDietType.Value.ToString();
                                entityHdDietTypeDao.Insert(entityHddt);
                            }
                        }

                        entity.GCDietType = cboDietType.Value.ToString();
                    }

                    entity.DiagnoseID = Request.Form[txtDiagnoseID.UniqueID];
                    entity.DiagnoseText = Request.Form[txtDiagnoseName.UniqueID];
                    entity.NumberOfCalories = Convert.ToDecimal(txtNumberOfCalories.Text);
                    entity.NumberOfProtein = Convert.ToDecimal(txtNumberOfProtein.Text);
                    entityHdDao.Update(entity);
                }
                else
                {
                    errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
                    result = false;
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
        #endregion

        #region Process Detail
        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int orderID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                if (hdnEntryID.Value.ToString() != "")
                {
                    orderID = Convert.ToInt32(hdnOrderID.Value);
                    if (OnSaveEditRecordEntityDt(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecordEntityDt(ref errMessage, ref orderID))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "delete")
            {
                orderID = Convert.ToInt32(hdnOrderID.Value);
                if (OnDeleteEntityDt(ref errMessage, orderID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpOrderID"] = orderID.ToString();
        }

        private void ControlToEntity(NutritionOrderDt entityDt)
        {
            entityDt.GCMealTime = cboMealTime.Value.ToString();
            if (hdnMealDay.Value == "1")
            {
                entityDt.GCMealDay = Constant.StandardCode.MEAL_DATE + "^001";
            }
            else if (hdnMealDay.Value == "2")
            {
                entityDt.GCMealDay = Constant.StandardCode.MEAL_DATE + "^002";
            }
            else if (hdnMealDay.Value == "3")
            {
                entityDt.GCMealDay = Constant.StandardCode.MEAL_DATE + "^003";
            }
            else if (hdnMealDay.Value == "4")
            {
                entityDt.GCMealDay = Constant.StandardCode.MEAL_DATE + "^004";
            }
            else if (hdnMealDay.Value == "5")
            {
                entityDt.GCMealDay = Constant.StandardCode.MEAL_DATE + "^005";
            }
            else if (hdnMealDay.Value == "6")
            {
                entityDt.GCMealDay = Constant.StandardCode.MEAL_DATE + "^006";
            }
            else if (hdnMealDay.Value == "7")
            {
                entityDt.GCMealDay = Constant.StandardCode.MEAL_DATE + "^007";
            }
            else if (hdnMealDay.Value == "8")
            {
                entityDt.GCMealDay = Constant.StandardCode.MEAL_DATE + "^008";
            }
            else if (hdnMealDay.Value == "9")
            {
                entityDt.GCMealDay = Constant.StandardCode.MEAL_DATE + "^009";
            }
            else if (hdnMealDay.Value == "10")
            {
                entityDt.GCMealDay = Constant.StandardCode.MEAL_DATE + "^010";
            }
            else
            {
                entityDt.GCMealDay = Constant.StandardCode.MEAL_DATE + "^011";
            }
            entityDt.MealPlanID = Convert.ToInt32(hdnItemID.Value);
            entityDt.ParamedicID = AppSession.RegisteredPatient.ParamedicID;
            entityDt.ClassID = AppSession.RegisteredPatient.ChargeClassID;
            entityDt.Remarks = txtRemarks.Text;
            entityDt.IsNotForPatient = chkIsNotForPatient.Checked;
            entityDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
            if (cboMealStatus.Value != null)
            {
                entityDt.GCMealStatus = cboMealStatus.Value.ToString();
            }
            else
            {
                entityDt.GCMealStatus = null;
            }
            entityDt.IsNewPatient = chkIsNewPatient.Checked;
        }

        private bool OnSaveAddRecordEntityDt(ref string errMessage, ref int orderID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            NutritionOrderHdDao entityHdDao = new NutritionOrderHdDao(ctx);
            NutritionOrderDtDao entityDtDao = new NutritionOrderDtDao(ctx);
            try
            {
                SaveNutrionOrderHd(ctx, ref orderID);
                NutritionOrderHd entityHd = entityHdDao.Get(orderID);
                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    NutritionOrderDt entityDt = new NutritionOrderDt();
                    ControlToEntity(entityDt);
                    entityDt.NutritionOrderHdID = orderID;
                    entityDt.CreatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Insert(entityDt);
                }
                else
                {
                    errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
                    result = false;
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

        private bool OnSaveEditRecordEntityDt(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            NutritionOrderHdDao entityHdDao = new NutritionOrderHdDao(ctx);
            NutritionOrderDtDao entityDtDao = new NutritionOrderDtDao(ctx);
            try
            {
                NutritionOrderHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnOrderID.Value));
                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    NutritionOrderDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                    ControlToEntity(entityDt);
                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Update(entityDt);
                }
                else
                {
                    errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
                    result = false;
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

        #region Void Entity
        //protected override bool OnVoidRecord(ref string errMessage)
        //{
        //    bool result = true;
        //    IDbContext ctx = DbFactory.Configure(true);
        //    NutritionOrderHdDao entityHdDao = new NutritionOrderHdDao(ctx);
        //    NutritionOrderDtDao entityDtDao = new NutritionOrderDtDao(ctx);
        //    try
        //    {
        //        Int32 NutritionOrderID = Convert.ToInt32(hdnOrderID.Value);
        //        NutritionOrderHd entity = entityHdDao.Get(NutritionOrderID);
        //        if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
        //        {
        //            entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
        //            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
        //            entity.VoidDate = DateTime.Now;
        //            entity.VoidBy = AppSession.UserLogin.UserID;
        //            entityHdDao.Update(entity);

        //            List<NutritionOrderDt> lstDt = BusinessLayer.GetNutritionOrderDtList(string.Format("NutritionOrderHdID = {0}", NutritionOrderID));
        //            foreach (NutritionOrderDt dt in lstDt)
        //            {
        //                NutritionOrderDt entityDt = entityDtDao.Get(dt.NutritionOrderDtID);
        //                entityDt.GCItemDetailStatus = Constant.TransactionStatus.VOID;
        //                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
        //                entityDtDao.Update(entityDt);
        //            }
        //            ctx.CommitTransaction();
        //        }
        //        else
        //        {
        //            errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
        //            result = false;
        //            ctx.RollBackTransaction();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result = false;
        //        errMessage = ex.Message;
        //        Helper.InsertErrorLog(ex);
        //        ctx.RollBackTransaction();
        //    }
        //    finally
        //    {
        //        ctx.Close();
        //    }
        //    return result;
        //}

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            string[] param = type.Split(';');
            string gcDeleteReason = param[1];
            string reason = param[2];
            bool result = true;

            if (param[0] == "void")
            {
                IDbContext ctx = DbFactory.Configure(true);
                NutritionOrderHdDao entityHdDao = new NutritionOrderHdDao(ctx);
                NutritionOrderDtDao entityDtDao = new NutritionOrderDtDao(ctx);
                try
                {
                    Int32 NutritionOrderID = Convert.ToInt32(hdnOrderID.Value);
                    NutritionOrderHd entity = entityHdDao.Get(NutritionOrderID);
                    if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entity.GCVoidReason = gcDeleteReason;
                        if (gcDeleteReason == Constant.DeleteReason.OTHER)
                        {
                            entity.VoidReason = reason;
                        }
                        entity.VoidBy = AppSession.UserLogin.UserID;
                        entity.VoidDate = DateTime.Now;
                        entityHdDao.Update(entity);

                        List<NutritionOrderDt> lstDt = BusinessLayer.GetNutritionOrderDtList(string.Format("NutritionOrderHdID = {0}", NutritionOrderID));
                        foreach (NutritionOrderDt dt in lstDt)
                        {
                            NutritionOrderDt entityDt = entityDtDao.Get(dt.NutritionOrderDtID);
                            entityDt.GCItemDetailStatus = Constant.TransactionStatus.VOID;
                            entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entityDt);
                        }
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
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
            }
            return result;
        }
        #endregion

        #region Proposed Entity
        protected override bool OnProposeRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            NutritionOrderHdDao entityHdDao = new NutritionOrderHdDao(ctx);
            NutritionOrderDtDao entityDtDao = new NutritionOrderDtDao(ctx);
            NutritionOrderHdDietTypeDao entityDietDao = new NutritionOrderHdDietTypeDao(ctx);

            //string diet = cboDietType.Value.ToString();
            string diet = string.Empty;
            if (!string.IsNullOrEmpty(cboDietType.Text))
            {
                diet = cboDietType.Value.ToString();
            }

            try
            {
                Int32 orderID = Convert.ToInt32(hdnOrderID.Value);
                NutritionOrderHd entity = entityHdDao.Get(orderID);
                NutritionOrderHdDietType entityDiet = null;
                if (!string.IsNullOrEmpty(diet))
                {
                    entityDiet = BusinessLayer.GetNutritionOrderHdDietTypeList(string.Format("NutritionOrderHdID = {0} AND GCDietType = '{1}'", orderID, cboDietType.Value.ToString())).FirstOrDefault();
                }
                else
                {
                    entityDiet = null;
                }

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
                    entity.ScheduleDate = Helper.GetDatePickerValue(Request.Form[txtScheduleDate.UniqueID]);
                    entity.ScheduleTime = Request.Form[txtScheduleTime.UniqueID];
                    if (!string.IsNullOrEmpty(diet))
                    {
                        entity.GCDietType = cboDietType.Value.ToString();
                    }
                    else
                    {
                        entity.GCDietType = string.Empty;
                    }

                    if (entityDiet == null)
                    {
                        if (!string.IsNullOrEmpty(diet))
                        {
                            NutritionOrderHdDietType entityHdDiet = new NutritionOrderHdDietType();
                            entityHdDiet.NutritionOrderHdID = orderID;
                            entityHdDiet.GCDietType = cboDietType.Value.ToString();
                            entityDietDao.Insert(entityHdDiet);
                        }
                    }
                    entity.DiagnoseID = Request.Form[txtDiagnoseID.UniqueID];
                    entity.DiagnoseText = Request.Form[txtDiagnoseName.UniqueID];
                    entity.Remarks = txtNotes.Text;
                    entity.NumberOfCalories = Convert.ToDecimal(Request.Form[txtNumberOfCalories.UniqueID]);
                    entity.NumberOfProtein = Convert.ToDecimal(Request.Form[txtNumberOfProtein.UniqueID]);
                    entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                    entity.ProposedBy = AppSession.UserLogin.UserID;
                    entity.ProposedDate = DateTime.Now;
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

        private void SendNotification(TestOrderHd order)
        {
            StringBuilder sbMessage = new StringBuilder();
            sbMessage.AppendLine(string.Format("No  : {0}", order.TestOrderNo));
            sbMessage.AppendLine(string.Format("Fr  : {0}", string.Format("{0} ({1})", AppSession.RegisteredPatient.ServiceUnitName, txtParamedicName.Text)));
            sbMessage.AppendLine(string.Format("Px  : {0}", AppSession.RegisteredPatient.PatientName));
            sbMessage.AppendLine(string.Format("PDx :    "));
            sbMessage.AppendLine(string.Format("{0}", order.Remarks));
            TcpClient client = new TcpClient();
            client.Connect(IPAddress.Parse(hdnIPAddress.Value), Convert.ToInt16(hdnPort.Value));
            NetworkStream stream = client.GetStream();
            using (BinaryWriter w = new BinaryWriter(stream))
            {
                using (BinaryReader r = new BinaryReader(stream))
                {
                    w.Write(string.Format(@"{0}", sbMessage.ToString()).ToCharArray());
                }
            }
        }
        #endregion

        private string PrintOrderReceipt(int healthcareServiceUnitID, int orderID, string printFormat)
        {
            string result = string.Empty;
            try
            {
                List<vTestOrderDt> lstOrder = BusinessLayer.GetvTestOrderDtList(String.Format("TestOrderID = {0} AND IsDeleted = 0 ORDER BY ItemName1", orderID));
                Registration oRegistration = BusinessLayer.GetRegistration(AppSession.RegisteredPatient.RegistrationID);
                if (oRegistration != null)
                {
                    vConsultVisit oVisit = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", oRegistration.RegistrationID)).FirstOrDefault();
                    if (oVisit != null)
                    {
                        string printerUrl = BusinessLayer.GetHealthcareServiceUnit(healthcareServiceUnitID).Printer1Url;
                        ZebraPrinting.PrintBuktiOrderPenunjang(oVisit, lstOrder, printerUrl);
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }
    }
}
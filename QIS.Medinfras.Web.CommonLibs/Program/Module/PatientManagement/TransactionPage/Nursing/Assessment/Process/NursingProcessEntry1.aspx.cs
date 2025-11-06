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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class NursingProcessEntry1 : BasePageTrx
    {
        string menuType = string.Empty;
        string deptType = string.Empty;
        protected int PageCount = 1;
        protected bool IsEditable = true;
        protected string errMessage, retval;
        private string lastDiagnoseText = "";

        public override string OnGetMenuCode()
        {
            if (menuType == "fo")
            {
                #region Follow-up Pasien Pulang
                switch (deptType)
                {
                    case Constant.Module.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.FOLLOWUP_NURSING_ASSESSMENT_PROCESS;
                    case Constant.Module.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.FOLLOWUP_NURSING_ASSESSMENT_PROCESS;
                    case Constant.Module.INPATIENT:
                        return Constant.MenuCode.Inpatient.FOLLOWUP_NURSING_ASSESSMENT_PROCESS;
                    case Constant.Module.MEDICAL_DIAGNOSTIC:
                        return Constant.MenuCode.MedicalDiagnostic.FOLLOWUP_NURSING_ASSESSMENT_PROCESS;
                    default:
                        return Constant.MenuCode.Inpatient.FOLLOWUP_NURSING_ASSESSMENT_PROCESS;
                }
                #endregion
            }
            else if (menuType == "dp")
            {
                #region Data Pemeriksaan Pasien
                switch (deptType)
                {
                    case Constant.Module.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.DATA_PATIENT_NURSING_ASSESSMENT_PROCESS;
                    case Constant.Module.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.DATA_PATIENT_NURSING_ASSESSMENT_PROCESS;
                    default:
                        return Constant.MenuCode.Outpatient.DATA_PATIENT_NURSING_ASSESSMENT_PROCESS;
                }
                #endregion
            }
            else
            {
                #region Pasien Dalam Perawatan
                switch (deptType)
                {
                    case Constant.Module.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.NURSING_ASSESSMENT_PROCESS;
                    case Constant.Module.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.NURSING_ASSESSMENT_PROCESS;
                    case Constant.Module.INPATIENT:
                        return Constant.MenuCode.Inpatient.NURSING_ASSESSMENT_PROCESS;
                    case Constant.Module.MEDICAL_DIAGNOSTIC:
                        return Constant.MenuCode.MedicalDiagnostic.NURSING_ASSESSMENT_PROCESS;
                    case Constant.Module.LABORATORY:
                        return Constant.MenuCode.Laboratory.NURSING_ASSESSMENT_PROCESS;
                    case Constant.Module.IMAGING:
                        return Constant.MenuCode.Imaging.NURSING_ASSESSMENT_PROCESS;
                    case Constant.Module.RADIOTHERAPHY:
                        return Constant.MenuCode.Radiotheraphy.PATIENT_PAGE_RT_NURSING_ASSESSMENT_PROCESS;
                    default:
                        return Constant.MenuCode.Inpatient.NURSING_ASSESSMENT_PROCESS;
                }
                #endregion
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }


        protected override void InitializeDataControl()
        {
            string[] param = Page.Request.QueryString["id"].Split('|');
            if (param.Count() > 1)
            {
                deptType = param[0];
                menuType = param[1];
            }
            else
            {
                deptType = param[0];
            }

            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                                        "HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
                                                                        AppSession.UserLogin.HealthcareID,
                                                                        Constant.SettingParameter.LOAD_TRANSACTION_NO_WHEN_OPEN_NURSING_TRANSACTION_MENU
                                                                    ));

            hdnLoadFirstRecord.Value = lstSettingParameterDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.LOAD_TRANSACTION_NO_WHEN_OPEN_NURSING_TRANSACTION_MENU).FirstOrDefault().ParameterValue;

            if (Page.Request.QueryString.Count > 0)
            {
                hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
                hdnLinkedRegistrationID.Value = AppSession.RegisteredPatient.LinkedRegistrationID.ToString();
                hdnUserID.Value = AppSession.UserLogin.UserID.ToString();
                hdnHealthcareServiceUnitID.Value = AppSession.RegisteredPatient.HealthcareServiceUnitID.ToString();
                hdnDefaultChargeClassID.Value = AppSession.RegisteredPatient.ChargeClassID.ToString();
                hdnDefaultParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();

                hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

                if (hdnLoadFirstRecord.Value == "1")
                {
                    if (OnGetRowCount() > 0)
                    {
                        IsLoadFirstRecord = true;
                    }
                    else
                    {
                        IsLoadFirstRecord = false;
                        OnAddRecord();
                    }
                }
                else
                {
                    IsLoadFirstRecord = false;
                    //OnAddRecord();
                }

                hdnNursingDiagnoseID.Value = "0";
            }

            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;
            List<ParamedicMaster> lstParamedic = BusinessLayer.GetParamedicMasterList(string.Format("GCParamedicMasterType NOT IN ('{0}') OR ParamedicID IN ({1})",
                                                                                            Constant.ParamedicType.Physician, paramedicID));
            Methods.SetComboBoxField<ParamedicMaster>(cboParamedicID, lstParamedic, "FullName", "ParamedicID");

            if (AppSession.UserLogin.ParamedicID != 0 && AppSession.UserLogin.ParamedicID != null)
            {
                //cboParamedicID.Value = lstParamedic.Where(a => a.ParamedicID == AppSession.UserLogin.ParamedicID).FirstOrDefault().ParamedicName;
                cboParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
                cboParamedicID.Enabled = false;
            }
            else
            {
                cboParamedicID.SelectedIndex = 0;
            }

            hdnParamedicID.Value = cboParamedicID.ToString();
            hdnDefaultParamedicID.Value = cboParamedicID.ToString();
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnTransactionID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(hdnNursingDiagnoseID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtTransactionNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtDiagnoseText, new ControlEntrySetting(true, true, false, lastDiagnoseText));
            SetControlEntrySetting(txtProblemCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtProblemName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtDiagnoseCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtDiagnoseName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblNursingDiagnose, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtTransactionDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtTransactionTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
        }

        protected string GetFilterExpression()
        {
            //string filterExpression = String.Empty;
            string filterExpression = String.Format("(VisitID = {0} OR VisitID IN (SELECT VisitID FROM ConsultVisit WHERE RegistrationID = {1}))", hdnVisitID.Value, hdnLinkedRegistrationID.Value);
            if (filterExpression != null)
            {
                cboParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
            }
            return filterExpression;
        }

        public override int OnGetRowCount()
        {
            return BusinessLayer.GetvNursingTransactionHdRowCount(GetFilterExpression());
        }

        public string GetInterventionFilterExpression()
        {
            string diagnoseID = hdnNursingDiagnoseID.Value;
            string filterExpression = string.Format("NurseInterventionID NOT IN (SELECT NursingInterventionID FROM NursingTransactionInterventionHD WHERE TransactionID = {0})", hdnTransactionID.Value);

            if (diagnoseID != string.Empty && diagnoseID != "0")
            {
                filterExpression = string.Format("NurseDiagnoseID = {0} AND NurseInterventionID NOT IN (SELECT NursingInterventionID FROM NursingTransactionInterventionHD WHERE TransactionID = {1})", diagnoseID, hdnTransactionID.Value);
            }

            return filterExpression;
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            PageIndex = BusinessLayer.GetvNursingTransactionHdRowIndex(filterExpression, keyValue, "TransactionID DESC");
            vNursingTransactionHd entity = BusinessLayer.GetvNursingTransactionHd(filterExpression, PageIndex, "TransactionID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            //PageIndex = BusinessLayer.GetvNursingTransactionHdRowIndex(filterExpression, "TransactionID DESC");
            vNursingTransactionHd entity = BusinessLayer.GetvNursingTransactionHd(filterExpression, PageIndex, "TransactionID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vNursingTransactionHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                watermarkText = entity.TransactionStatus;
            }
            hdnTransactionID.Value = entity.TransactionID.ToString();
            txtTransactionNo.Text = entity.TransactionNo;
            txtTransactionDate.Text = entity.TransactionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTransactionTime.Text = entity.TransactionTime;
            hdnTransactionStatus.Value = entity.GCTransactionStatus;

            hdnNursingPatientProblemID.Value = entity.NursingPatientProblemID.ToString();
            txtProblemCode.Text = entity.ProblemCode;
            txtProblemName.Text = entity.ProblemName;

            hdnNursingDiagnoseID.Value = entity.NursingDiagnoseID.ToString();
            txtDiagnoseCode.Text = entity.NurseDiagnoseCode;
            txtDiagnoseName.Text = entity.NurseDiagnoseName;
            txtDiagnoseText.Text = entity.DiagnoseText;

            if (entity.ParamedicID != 0)
            {
                hdnParamedicID.Value = entity.ParamedicID.ToString();
                cboParamedicID.Value = entity.ParamedicID.ToString();
                cboParamedicID.Enabled = false;
            }
            else
            {
                hdnParamedicID.Value = "0";
                cboParamedicID.Value = "";
                cboParamedicID.Enabled = false;
            }

            //divSubjectiveMayor.InnerHtml = entity.PercentageSubjectiveMayor.ToString("N0");
            //divObjectiveMayor.InnerHtml = entity.PercentageObjectiveMayor.ToString("N0");
            //divSubjectiveMinor.InnerHtml = entity.PercentageSubjectiveMinor.ToString("N0");
            //divObjectiveMinor.InnerHtml = entity.PercentageObjectiveMinor.ToString("N0");

            ctlDiagnoseItem.LoadGridViewByHeaderEntity(entity.TransactionID);
            ctlOutcome.LoadGridViewByHeaderEntity(entity.TransactionID);
            ctlIntervention.LoadGridViewByHeaderEntity(entity.TransactionID, entity.NursingDiagnoseID);
            ctlImplementation.LoadGridViewByHeaderEntity(entity.TransactionID);
            ctlEvaluation.LoadGridViewByHeaderEntity(entity.TransactionID);
        }

        public override void OnAddRecord()
        {
            hdnTransactionID.Value = "0";
            txtTransactionNo.Text = String.Empty;
            txtTransactionDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTransactionTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            txtDiagnoseCode.Text = string.Empty;
            txtDiagnoseName.Text = string.Empty;

            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;
            List<ParamedicMaster> lstParamedic = BusinessLayer.GetParamedicMasterList(string.Format("GCParamedicMasterType NOT IN ('{0}') OR ParamedicID IN ({1})",
                                                                                           Constant.ParamedicType.Physician, paramedicID));
            if (AppSession.UserLogin.ParamedicID != 0 && AppSession.UserLogin.ParamedicID != null)
            {
                //cboParamedicID.Value = lstParamedic.Where(a => a.ParamedicID == AppSession.UserLogin.ParamedicID).FirstOrDefault();
                cboParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
                cboParamedicID.Enabled = false;
            }
            else
            {
                cboParamedicID.SelectedIndex = 0;
            }
            NursingTransactionHd entityLast = BusinessLayer.GetNursingTransactionHdList(String.Format("VisitID = {0} ORDER BY TransactionID DESC", hdnVisitID.Value.Trim())).FirstOrDefault();
            if (entityLast != null)
                lastDiagnoseText = entityLast.DiagnoseText;
            else
                lastDiagnoseText = String.Empty;
            txtDiagnoseText.Text = lastDiagnoseText;
            hdnNursingDiagnoseID.Value = "0";

            ctlDiagnoseItem.LoadGridViewByHeaderEntity(0);
            ctlOutcome.LoadGridViewByHeaderEntity(0);
            ctlIntervention.LoadGridViewByHeaderEntity(0, 0);
            ctlEvaluation.LoadGridViewByHeaderEntity(0);

            divSubjectiveMayor.InnerHtml = "0";
            divObjectiveMayor.InnerHtml = "0";
            divSubjectiveMinor.InnerHtml = "0";
            divObjectiveMinor.InnerHtml = "0";
        }

        #region Public Function
        public string GetNursingDiagnoseID()
        {
            return hdnNursingDiagnoseID.Value;
        }

        public string GetNursingTransactionID()
        {
            return hdnTransactionID.Value;
        }

        public string GetVisitID()
        {
            return hdnVisitID.Value;
        }

        public string GetHealthcareServiceUnitID()
        {
            return hdnHealthcareServiceUnitID.Value;
        }

        public string GetDefaultParamedicID()
        {
            return hdnDefaultParamedicID.Value;
        }

        public string GetUserID()
        {
            return AppSession.UserLogin.UserID.ToString();
        }

        public void SaveHeaderFromUserControl()
        {
            int transactionID = 0;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                SaveNursingTransactionHd(ctx, ref transactionID);
                ctx.CommitTransaction();
            }
            catch
            {
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
        }

        public void SaveEvaluationFromUserControl(IDbContext ctx)
        {
            string errMsg = "";
            bool isEvaluation = false;
            if (hdnContainerActive.Value == "containerOutcome")
            {
                isEvaluation = false;
            }
            else if (hdnContainerActive.Value == "ctlEvaluation")
            {
                isEvaluation = true;
            }
            ctlEvaluation.SaveNursingTransactionDt(ctx, Convert.ToInt32(hdnTransactionID.Value), isEvaluation, ref errMessage);
        }

        public void SetChildrenControlHiddenField()
        {
            ctlIntervention.SetHiddenField(hdnTransactionID.Value, hdnNursingDiagnoseID.Value);
        }

        public string LoadNursingEvaluationFromNursingDiagnoseItem(IDbContext ctx)
        {
            return ctlEvaluation.SetHiddenFieldWithinTransaction(Convert.ToInt32(hdnTransactionID.Value), ctx);
        }
        #endregion

        protected string OnGetDiagnoseFilterExpression()
        {
            if (!string.IsNullOrEmpty(txtProblemCode.Text))
            {
                string filterExpression = string.Format("NurseDiagnoseID IN (SELECT NurseDiagnoseID FROM vNursingProblemDiagnose WITH (NOLOCK) WHERE ProblemCode = '{0}')", txtProblemCode.Text);
                return string.Format("{0} AND IsDeleted = 0", filterExpression);
            }
            else
            {
                return String.Format("IsDeleted = 0");
            }
        }

        protected string GetTransactionFilterExpression()
        {
            //return String.Format("(VisitID = {0} OR VisitID IN (SELECT VisitID FROM ConsultVisit WHERE RegistrationID = {1}))", hdnVisitID.Value, hdnLinkedRegistrationID.Value);
            return String.Format("RegistrationID IN ({0},{1})", AppSession.RegisteredPatient.RegistrationID, hdnLinkedRegistrationID.Value);
        }

        #region Save Header
        public void SaveNursingTransactionHd(IDbContext ctx, ref int TransactionID)
        {
            NursingTransactionHdDao entityHdDao = new NursingTransactionHdDao(ctx);
            //menuType = Page.Request.QueryString["id"].Split('|')[0];
            if (hdnTransactionID.Value == "0")
            {
                NursingTransactionHd entityHd = new NursingTransactionHd();
                entityHd.TransactionDate = Helper.GetDatePickerValue(Request.Form[txtTransactionDate.UniqueID]);
                entityHd.TransactionTime = txtTransactionTime.Text;
                if (!string.IsNullOrEmpty(txtProblemCode.Text))
                    entityHd.NursingPatientProblemID = Convert.ToInt32(hdnNursingPatientProblemID.Value);
                entityHd.NursingDiagnoseID = Convert.ToInt32(hdnNursingDiagnoseID.Value);
                entityHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                entityHd.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                if (AppSession.UserLogin.ParamedicID != 0 && AppSession.UserLogin.ParamedicID != null)
                {
                    entityHd.ParamedicID = AppSession.UserLogin.ParamedicID;
                }
                else
                {
                    if (cboParamedicID.Value != null)
                        entityHd.ParamedicID = Convert.ToInt32(cboParamedicID.Value.ToString());
                }
                entityHd.TransactionNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.NURSING_TRANSACTION, entityHd.TransactionDate, ctx);
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                entityHd.DiagnoseText = txtDiagnoseText.Text;
                entityHd.LinkField = String.Empty;

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                int transactionID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
                TransactionID = transactionID;
                hdnTransactionID.Value = transactionID.ToString();
            }
            else
            {
                TransactionID = Convert.ToInt32(hdnTransactionID.Value);
                NursingTransactionHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnTransactionID.Value));
                entityHd.NursingDiagnoseID = Convert.ToInt32(hdnNursingDiagnoseID.Value);
                entityHd.DiagnoseText = txtDiagnoseText.Text;
                entityHd.TransactionDate = Helper.GetDatePickerValue(Request.Form[txtTransactionDate.UniqueID]);
                entityHd.TransactionTime = txtTransactionTime.Text;
                entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityHdDao.Update(entityHd);
            }

            SetChildrenControlHiddenField();
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            if (hdnTransactionID.Value == "" || hdnTransactionID.Value == "0")
            {
                bool result = true;
                IDbContext ctx = DbFactory.Configure(true);
                NursingTransactionHdDao entityDao = new NursingTransactionHdDao(ctx);
                bool isValid = true;
                try
                {
                    int TransactionID = 0;

                    if (cboParamedicID.Value == null)
                    {
                        errMessage = "Field Perawat yang membuat Asuhan Keperawatan tidak boleh kosong.";
                        isValid = false;
                    }

                    if (isValid)
                    {
                        SaveNursingTransactionHd(ctx, ref TransactionID);
                        retval = TransactionID.ToString();

                        bool isEvaluation = false;
                        if (hdnContainerActive.Value == "ctlEvaluation")
                        {
                            isEvaluation = true;
                        }
                        else
                        {
                            isEvaluation = false;
                        }

                        ctlDiagnoseItem.SaveNursingTransactionDt(ctx, TransactionID, ref errMessage);
                        ctlOutcome.SaveNursingTransactionDt(ctx, TransactionID, ref errMessage);
                        ctlIntervention.SaveNursingTransactionDt(ctx, TransactionID, hdnNursingDiagnoseID.Value, ref errMessage);
                        ctlEvaluation.SaveNursingTransactionDt(ctx, TransactionID, isEvaluation, ref errMessage);
                        ctx.CommitTransaction();
                        hdnTransactionID.Value = TransactionID.ToString();
                    }
                    else
                    {
                        ctx.RollBackTransaction();
                        result = false;
                        hdnTransactionID.Value = "0";
                    }
                }
                catch (Exception ex)
                {
                    ctx.RollBackTransaction();
                    errMessage = ex.Message;
                    result = false;
                    hdnTransactionID.Value = "0";
                }
                finally
                {
                    ctx.Close();
                }

                //if (isValid)
                //{
                //    #region Diagnosa Pelayanan
                //    ctx = DbFactory.Configure(true);
                //    try
                //    {
                //        ctlDiagnoseItem.SaveNursingTransactionDt(ctx, Convert.ToInt32(retval), ref errMessage);
                //        ctx.CommitTransaction();
                //    }
                //    catch (Exception ex)
                //    {
                //        ctx.RollBackTransaction();
                //        errMessage = ex.Message;
                //        result = false;
                //    }
                //    finally
                //    {
                //        ctx.Close();
                //    }
                //    #endregion
                //    #region NOC
                //    ctx = DbFactory.Configure(true);
                //    try
                //    {
                //        ctlOutcome.SaveNursingTransactionDt(ctx, Convert.ToInt32(retval), ref errMessage);
                //        ctx.CommitTransaction();
                //    }
                //    catch (Exception ex)
                //    {
                //        ctx.RollBackTransaction();
                //        errMessage = ex.Message;
                //        result = false;
                //    }
                //    finally
                //    {
                //        ctx.Close();
                //    }
                //    #endregion 
                //}

                return result;
            }
            else
                return OnSaveEditRecord(ref errMessage, ref retval);
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            NursingTransactionHdDao entityHdDao = new NursingTransactionHdDao(ctx);
            bool isValid = true;
            int transactionID = Convert.ToInt32(hdnTransactionID.Value);
            try
            {

                if (cboParamedicID.Value == null)
                {
                    errMessage = "Field Perawat yang membuat Asuhan Keperawatan tidak boleh kosong.";
                    isValid = false;
                }

                if (isValid)
                {
                    bool isEvaluation = false;
                    if (hdnContainerActive.Value == "containerOutcome")
                    {
                        isEvaluation = false;
                    }
                    else if (hdnContainerActive.Value == "ctlEvaluation")
                    {
                        isEvaluation = true;
                    }
                    else
                    {
                        isEvaluation = false;
                    }

                    SaveNursingTransactionHd(ctx, ref transactionID);
                    ctlDiagnoseItem.SaveNursingTransactionDt(ctx, transactionID, ref errMessage);
                    ctlOutcome.SaveNursingTransactionDt(ctx, transactionID, ref errMessage);
                    ctlIntervention.SaveNursingTransactionDt(ctx, transactionID, hdnNursingDiagnoseID.Value, ref errMessage);
                    ctlEvaluation.SaveNursingTransactionDt(ctx, transactionID, isEvaluation, ref errMessage);
                    ctx.CommitTransaction();
                    retval = transactionID.ToString();
                }
                else
                {
                    ctx.RollBackTransaction();
                    result = false;
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

            //#region Diagnosa Pelayanan
            //ctx = DbFactory.Configure(true);
            //try 
            //{
            //    ctlDiagnoseItem.SaveNursingTransactionDt(ctx, Convert.ToInt32(retval), ref errMessage);
            //    ctx.CommitTransaction();
            //}
            //catch (Exception ex)
            //{
            //    ctx.RollBackTransaction();
            //    errMessage = ex.Message;
            //    result = false;
            //}
            //finally
            //{
            //    ctx.Close();
            //}
            //#endregion
            //#region NOC
            //ctx = DbFactory.Configure(true);
            //try
            //{
            //    ctlOutcome.SaveNursingTransactionDt(ctx, Convert.ToInt32(retval), ref errMessage);
            //    ctx.CommitTransaction();
            //}
            //catch (Exception ex)
            //{
            //    ctx.RollBackTransaction();
            //    errMessage = ex.Message;
            //    result = false;
            //}
            //finally
            //{
            //    ctx.Close();
            //}
            //#endregion

            return result;
        }

        protected override bool OnApproveRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            NursingTransactionHdDao entityHdDao = new NursingTransactionHdDao(ctx);
            try
            {
                NursingTransactionHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnTransactionID.Value));
                entityHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;


                bool isEvaluation = false;
                if (hdnContainerActive.Value == "ctlEvaluation")
                {
                    isEvaluation = true;
                }
                else
                {
                    isEvaluation = false;
                }

                ctlDiagnoseItem.SaveNursingTransactionDt(ctx, entityHd.TransactionID, ref errMessage);
                ctlOutcome.SaveNursingTransactionDt(ctx, entityHd.TransactionID, ref errMessage);
                ctlIntervention.SaveNursingTransactionDt(ctx, entityHd.TransactionID, hdnNursingDiagnoseID.Value, ref errMessage);
                ctlEvaluation.SaveNursingTransactionDt(ctx, entityHd.TransactionID, isEvaluation, ref errMessage);

                entityHdDao.Update(entityHd);

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }



        protected override bool OnVoidRecord(ref string errMessage)
        {
            try
            {
                NursingTransactionHd entity = BusinessLayer.GetNursingTransactionHd(Convert.ToInt32(hdnTransactionID.Value));
                entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateNursingTransactionHd(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
        #endregion


        protected void cbpImportProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');

                if (param[0] == "import")
                {
                    result = ImportNursingTransaction();
                }
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private string ImportNursingTransaction()
        {
            string result = "";
            IDbContext ctx = DbFactory.Configure(true);
            NursingTransactionHdDao entityDao = new NursingTransactionHdDao(ctx);
            try
            {
                int templateTransactionID = Convert.ToInt32(hdnTemplateTransactionID.Value);
                int TransactionID = 0;
                ImportNursingTransactionHd(ctx, templateTransactionID, ref TransactionID);
                ImportNursingTransactionDt(ctx, templateTransactionID, TransactionID);
                ImportNursingOutcomeDt(ctx, templateTransactionID, TransactionID);
                ImportNursingIntervention(ctx, templateTransactionID, TransactionID);
                NursingTransactionHd entity = BusinessLayer.GetNursingTransactionHdList(String.Format("TransactionID = {0}", TransactionID), ctx).FirstOrDefault();

                result = "success|" + entity.TransactionNo;
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                result = "failed";
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private void ImportNursingTransactionHd(IDbContext ctx, int templateTransactionID, ref int TransactionID)
        {
            NursingTransactionHdDao entityHdDao = new NursingTransactionHdDao(ctx);
            NursingTransactionHd entityTemplate = BusinessLayer.GetNursingTransactionHdList(String.Format("TransactionID = {0}", templateTransactionID), ctx).FirstOrDefault();
            if (entityTemplate != null)
            {
                NursingTransactionHd entityHd = new NursingTransactionHd();
                entityHd.TransactionDate = Helper.GetDatePickerValue(Request.Form[txtTransactionDate.UniqueID]);
                entityHd.TransactionTime = txtTransactionTime.Text;
                entityHd.NursingPatientProblemID = entityTemplate.NursingPatientProblemID;
                entityHd.NursingDiagnoseID = entityTemplate.NursingDiagnoseID;
                entityHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                entityHd.HealthcareServiceUnitID = 0;
                entityHd.SubjectiveText = String.Empty;
                entityHd.ObjectiveText = String.Empty;
                entityHd.AssessmentText = String.Empty;
                entityHd.PlanningText = String.Empty;
                entityHd.TransactionNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.NURSING_TRANSACTION, entityHd.TransactionDate, ctx);
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                entityHd.LinkField = String.Format("{0}|{1}", hdnVisitID.Value, hdnHealthcareServiceUnitID.Value);
                entityHd.NOCInterval = entityTemplate.NOCInterval;
                entityHd.ParamedicID = AppSession.UserLogin.ParamedicID;

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                entityHdDao.Insert(entityHd);
                int transactionID = BusinessLayer.GetNursingTransactionHdMaxID(ctx);
                TransactionID = transactionID;
                hdnTransactionID.Value = transactionID.ToString();
            }
        }

        private void ImportNursingTransactionDt(IDbContext ctx, int templateTransactionID, int TransactionID)
        {
            NursingTransactionDtDao entityDao = new NursingTransactionDtDao(ctx);
            List<NursingTransactionDt> lstEntityTemplate = BusinessLayer.GetNursingTransactionDtList(String.Format("TransactionID = {0}", templateTransactionID), ctx);

            if (lstEntityTemplate.Count > 0)
            {
                foreach (NursingTransactionDt entityTemplate in lstEntityTemplate)
                {
                    vNursingDiagnoseItem entityDiagItem = BusinessLayer.GetvNursingDiagnoseItemList(String.Format("NursingDiagnoseItemID = {0}", entityTemplate.NursingDiagnoseItemID), ctx).FirstOrDefault();
                    if (entityDiagItem.GCNursingEvaluation == Constant.NursingEvaluation.SUBJECTIVE || entityDiagItem.GCNursingEvaluation == Constant.NursingEvaluation.SUBJECTIVE)
                    {
                        NursingTransactionEvaluationDt entityEval = BusinessLayer.GetNursingTransactionEvaluationDtList(String.Format("TransactionID = {0} AND NursingDiagnoseItemID = {1}", templateTransactionID, entityTemplate.NursingDiagnoseItemID), ctx).FirstOrDefault();
                        if (entityEval != null)
                        {
                            NursingTransactionDt entity = new NursingTransactionDt();
                            entity.TransactionID = TransactionID;
                            entity.NursingDiagnoseItemID = entityTemplate.NursingDiagnoseItemID;
                            entity.NursingItemText = entityTemplate.NursingItemText;
                            entity.IsEditedByUser = entityTemplate.IsEditedByUser;

                            entity.CreatedBy = AppSession.UserLogin.UserID;

                            entityDao.Insert(entity);
                        }
                    }
                    else
                    {
                        NursingTransactionDt entity = new NursingTransactionDt();
                        entity.TransactionID = TransactionID;
                        entity.NursingDiagnoseItemID = entityTemplate.NursingDiagnoseItemID;
                        entity.NursingItemText = entityTemplate.NursingItemText;
                        entity.IsEditedByUser = entityTemplate.IsEditedByUser;

                        entity.CreatedBy = AppSession.UserLogin.UserID;

                        entityDao.Insert(entity);
                    }

                }
            }

        }

        private void ImportNursingOutcomeDt(IDbContext ctx, int templateTransactionID, int TransactionID)
        {
            NursingTransactionOutcomeDtDao entityDao = new NursingTransactionOutcomeDtDao(ctx);
            List<NursingTransactionOutcomeDt> lstEntityTemplate = BusinessLayer.GetNursingTransactionOutcomeDtList(String.Format("TransactionID = {0}", templateTransactionID), ctx);
            if (lstEntityTemplate.Count > 0)
            {
                foreach (NursingTransactionOutcomeDt entityTemplate in lstEntityTemplate)
                {
                    NursingTransactionOutcomeDt entity = new NursingTransactionOutcomeDt();
                    entity.TransactionID = TransactionID;
                    entity.NursingDiagnoseItemID = entityTemplate.NursingDiagnoseItemID;
                    entity.NursingDiagnoseItemIndicatorID = entityTemplate.NursingDiagnoseItemIndicatorID;
                    entity.ScaleScore = 0;
                    entity.Remarks = entityTemplate.Remarks;

                    entity.CreatedBy = AppSession.UserLogin.UserID;

                    entityDao.Insert(entity);
                }
            }
        }

        private void ImportNursingIntervention(IDbContext ctx, int templateTransactionID, int TransactionID)
        {
            NursingTransactionInterventionHdDao entityDao = new NursingTransactionInterventionHdDao(ctx);
            NursingTransactionInterventionDtDao entityDtDao = new NursingTransactionInterventionDtDao(ctx);
            List<NursingTransactionInterventionHd> lstEntityTemplate = BusinessLayer.GetNursingTransactionInterventionHdList(String.Format("TransactionID = {0}", templateTransactionID), ctx);
            if (lstEntityTemplate.Count > 0)
            {
                foreach (NursingTransactionInterventionHd entityTemplate in lstEntityTemplate)
                {
                    NursingTransactionInterventionHd entity = new NursingTransactionInterventionHd();
                    entity.TransactionID = TransactionID;
                    entity.NursingInterventionID = entityTemplate.NursingInterventionID;
                    entity.CreatedBy = AppSession.UserLogin.UserID;

                    int transactionHdID = entityDao.InsertReturnPrimaryKeyID(entity);

                    List<NursingTransactionInterventionDt> lstEntityDtTemplate = BusinessLayer.GetNursingTransactionInterventionDtList(String.Format("NursingTransactionInterventionHdID = {0}", entityTemplate.ID), ctx);
                    if (lstEntityDtTemplate.Count > 0)
                    {
                        foreach (NursingTransactionInterventionDt entityDtTemplate in lstEntityDtTemplate)
                        {
                            //if (entityDtTemplate.IsContinued)
                            //{
                            NursingTransactionInterventionDt entityDt = new NursingTransactionInterventionDt();
                            entityDt.NursingTransactionInterventionHdID = transactionHdID;
                            entityDt.NursingInterventionItemID = entityDtTemplate.NursingInterventionItemID;
                            entityDt.IsEditedByUser = entityDtTemplate.IsEditedByUser;
                            entityDt.NursingItemText = entityDtTemplate.NursingItemText;
                            entityDt.InterventionImplementation = entityDtTemplate.InterventionImplementation;
                            entityDt.CreatedBy = AppSession.UserLogin.UserID;

                            entityDtDao.Insert(entityDt);
                            //}
                        }
                    }

                }
            }

        }
    }
}




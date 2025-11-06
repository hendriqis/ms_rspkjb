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

namespace QIS.Medinfras.Web.Nursing.Program
{
    public partial class NursingTransactionEntry : BasePageTrx
    {
        protected int PageCount = 1;
        protected bool IsEditable = true;
        protected string errMessage, retval;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Nursing.NURSING_TRANSACTION;
        }
        private string lastDiagnoseText = "";

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString.Count > 0)
            {
                hdnVisitID.Value = Page.Request.QueryString["id"].Split('|')[0];

                vConsultVisit2 entity = BusinessLayer.GetvConsultVisit2List(string.Format("VisitID = {0}", hdnVisitID.Value))[0];
                hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
                hdnDefaultChargeClassID.Value = entity.ChargeClassID.ToString();
                hdnDefaultParamedicID.Value = entity.ParamedicID.ToString();

                ctlPatientBanner.InitializePatientBanner(entity);

                if (OnGetRowCount() > 0)
                    IsLoadFirstRecord = true;
                else
                {
                    IsLoadFirstRecord = false;
                    OnAddRecord();
                }

                hdnNursingDiagnoseID.Value = "0";
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnTransactionID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(hdnNursingDiagnoseID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtTransactionNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtDiagnoseText, new ControlEntrySetting(true, true, false, lastDiagnoseText));
            SetControlEntrySetting(txtDiagnoseCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtDiagnoseName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblNursingDiagnose, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtTransactionDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtTransactionTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
        }

        protected string GetFilterExpression()
        {
            string filterExpression = String.Empty;
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
            hdnNursingDiagnoseID.Value = entity.NursingDiagnoseID.ToString();
            txtDiagnoseCode.Text = entity.NurseDiagnoseCode;
            txtDiagnoseName.Text = entity.NurseDiagnoseName;
            txtDiagnoseText.Text = entity.DiagnoseText;

            ctlDiagnoseItem.LoadGridViewByHeaderEntity(entity.TransactionID);
            ctlOutcome.LoadGridViewByHeaderEntity(entity.TransactionID);
            ctlIntervention.LoadGridViewByHeaderEntity(entity.TransactionID);
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

            vNursingTransactionHd entityLast = BusinessLayer.GetvNursingTransactionHdList(String.Format("VisitID = {0} ORDER BY TransactionID DESC", hdnVisitID.Value.Trim())).FirstOrDefault();
            if (entityLast != null)
                lastDiagnoseText = entityLast.DiagnoseText;
            else
                lastDiagnoseText = String.Empty;
            txtDiagnoseText.Text = lastDiagnoseText;
            hdnNursingDiagnoseID.Value = "0";

            ctlDiagnoseItem.LoadGridViewByHeaderEntity(0);
            ctlOutcome.LoadGridViewByHeaderEntity(0);
            ctlIntervention.LoadGridViewByHeaderEntity(0);
            ctlEvaluation.LoadGridViewByHeaderEntity(0);
        }

        #region Public Function
        public string GetNursingDiagnoseID()
        {
            //return Request.Form[hdnNursingDiagnoseID.UniqueID];
            return hdnNursingDiagnoseID.Value;
        }

        public string GetNursingTransactionID()
        {
            //return Request.Form[hdnNursingDiagnoseID.UniqueID];
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

        public void SaveHeaderFromUserControl()
        {
            //if (hdnTransactionID.Value != "" && hdnTransactionID.Value != "0")
            //    return OnSaveEditRecord(ref errMessage, ref retval);
            //else
            //    return OnSaveAddRecord(ref errMessage, ref retval);
            int transactionID = 0;
            IDbContext ctx = DbFactory.Configure(true);
            try 
            { 
                SaveNursingTransactionHd(ctx,ref transactionID);
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
            ctlEvaluation.SaveNursingTransactionDt(ctx,Convert.ToInt32(hdnTransactionID.Value),ref errMessage);
        }

        public string LoadNursingEvaluationFromNursingDiagnoseItem(IDbContext ctx)
        {
            return ctlEvaluation.SetHiddenFieldWithinTransaction(Convert.ToInt32(hdnTransactionID.Value),ctx);
        }
        #endregion

        protected string OnGetDiagnoseFilterExpression()
        {
            return String.Format("IsDeleted = 0");
        }

        protected string GetTransactionFilterExpression()
        {
            return String.Format("VisitID = {0}", hdnVisitID.Value);
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
                entityHd.NursingDiagnoseID = Convert.ToInt32(hdnNursingDiagnoseID.Value);
                entityHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                entityHd.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                entityHd.TransactionNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.NURSING_TRANSACTION, entityHd.TransactionDate, ctx);
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                entityHd.DiagnoseText = txtDiagnoseText.Text;
                entityHd.LinkField = String.Empty;

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                entityHdDao.Insert(entityHd);
                int transactionID = BusinessLayer.GetNursingTransactionHdMaxID(ctx);
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
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            if (hdnTransactionID.Value == "" || hdnTransactionID.Value == "0")
            {
                bool result = true;
                IDbContext ctx = DbFactory.Configure(true);
                NursingTransactionHdDao entityDao = new NursingTransactionHdDao(ctx);
                try
                {
                    int TransactionID = 0;
                    SaveNursingTransactionHd(ctx, ref TransactionID);
                    retval = TransactionID.ToString();

                    ctlIntervention.SaveNursingTransactionDt(ctx, TransactionID, ref errMessage);
                    ctlEvaluation.SaveNursingTransactionDt(ctx, TransactionID, ref errMessage);
                    ctx.CommitTransaction();
                    hdnTransactionID.Value = TransactionID.ToString();
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

                #region Diagnosa Pelayanan
                ctx = DbFactory.Configure(true);
                try
                {
                    ctlDiagnoseItem.SaveNursingTransactionDt(ctx, Convert.ToInt32(retval), ref errMessage);
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
                #endregion
                #region NOC
                ctx = DbFactory.Configure(true);
                try
                {
                    ctlOutcome.SaveNursingTransactionDt(ctx, Convert.ToInt32(retval), ref errMessage);
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
                #endregion
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
            int transactionID = Convert.ToInt32(hdnTransactionID.Value);
            try
            {
                SaveNursingTransactionHd(ctx,ref transactionID);


                ctlOutcome.SaveNursingTransactionDt(ctx, transactionID, ref errMessage);
                ctlIntervention.SaveNursingTransactionDt(ctx, transactionID, ref errMessage);
                ctlEvaluation.SaveNursingTransactionDt(ctx, transactionID, ref errMessage);
                ctx.CommitTransaction();
                retval = transactionID.ToString();
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

            #region Diagnosa Pelayanan
            ctx = DbFactory.Configure(true);
            try 
            {
                ctlDiagnoseItem.SaveNursingTransactionDt(ctx, Convert.ToInt32(retval), ref errMessage);
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
            #endregion
            #region NOC
            ctx = DbFactory.Configure(true);
            try
            {
                ctlOutcome.SaveNursingTransactionDt(ctx, Convert.ToInt32(retval), ref errMessage);
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
            #endregion

            //if(!result)
            //    throw new Exception(errMessage);
            return result;
        }

        protected override bool OnProposeRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            NursingTransactionHdDao entityHdDao = new NursingTransactionHdDao(ctx);
            try
            {
                NursingTransactionHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnTransactionID.Value));
                entityHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityHdDao.Update(entityHd);

                ctlDiagnoseItem.SaveNursingTransactionDt(ctx, entityHd.TransactionID, ref errMessage);
                ctlOutcome.SaveNursingTransactionDt(ctx, entityHd.TransactionID, ref errMessage);
                ctlIntervention.SaveNursingTransactionDt(ctx, entityHd.TransactionID, ref errMessage);
                ctlEvaluation.SaveNursingTransactionDt(ctx, entityHd.TransactionID, ref errMessage);
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
                ImportNursingTransactionDt(ctx,templateTransactionID, TransactionID);
                ImportNursingOutcomeDt(ctx, templateTransactionID, TransactionID);
                ImportNursingIntervention(ctx, templateTransactionID, TransactionID);
                NursingTransactionHd entity = BusinessLayer.GetNursingTransactionHdList(String.Format("TransactionID = {0}", TransactionID),ctx).FirstOrDefault();
               
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
            NursingTransactionHd entityTemplate = BusinessLayer.GetNursingTransactionHdList(String.Format("TransactionID = {0}",templateTransactionID),ctx).FirstOrDefault();
            if (entityTemplate != null)
            {
                NursingTransactionHd entityHd = new NursingTransactionHd();
                entityHd.TransactionDate = Helper.GetDatePickerValue(Request.Form[txtTransactionDate.UniqueID]);
                entityHd.TransactionTime = txtTransactionTime.Text;
                entityHd.NursingDiagnoseID = entityTemplate.NursingDiagnoseID;
                entityHd.VisitID = Convert.ToInt32(0);
                entityHd.HealthcareServiceUnitID = 0;
                entityHd.SubjectiveText = String.Empty;
                entityHd.ObjectiveText = String.Empty;
                entityHd.AssessmentText = String.Empty;
                entityHd.PlanningText = String.Empty;
                entityHd.TransactionNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.NURSING_TRANSACTION, entityHd.TransactionDate, ctx);
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                entityHd.LinkField = String.Format("{0}|{1}", hdnVisitID.Value, hdnHealthcareServiceUnitID.Value);
                entityHd.NOCInterval = entityTemplate.NOCInterval;

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
                    vNursingDiagnoseItem entityDiagItem = BusinessLayer.GetvNursingDiagnoseItemList(String.Format("NursingDiagnoseItemID = {0}",entityTemplate.NursingDiagnoseItemID),ctx).FirstOrDefault();
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
                foreach(NursingTransactionOutcomeDt entityTemplate in lstEntityTemplate)
                {
                    NursingTransactionOutcomeDt entity = new NursingTransactionOutcomeDt();
                    entity.TransactionID = TransactionID;
                    entity.NursingDiagnoseItemID = entityTemplate.NursingDiagnoseItemID;
                    entity.NursingDiagnoseItemIndicatorID = entity.NursingDiagnoseItemIndicatorID;
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
            List<NursingTransactionInterventionHd> lstEntityTemplate = BusinessLayer.GetNursingTransactionInterventionHdList(String.Format("TransactionID = {0}", templateTransactionID),ctx);
            if (lstEntityTemplate.Count > 0)
            {
                foreach (NursingTransactionInterventionHd entityTemplate in lstEntityTemplate)
                {
                    NursingTransactionInterventionHd entity = new NursingTransactionInterventionHd();
                    entity.TransactionID = TransactionID;
                    entity.NursingInterventionID = entityTemplate.NursingInterventionID;
                    entity.CreatedBy = AppSession.UserLogin.UserID;

                    entityDao.Insert(entity);
                    int transactionHdID = BusinessLayer.GetNursingTransactionInterventionHdMaxID(ctx);

                    List<NursingTransactionInterventionDt> lstEntityDtTemplate = BusinessLayer.GetNursingTransactionInterventionDtList(String.Format("NursingTransactionInterventionHdID = {0}", entityTemplate.ID), ctx);
                    if (lstEntityDtTemplate.Count > 0)
                    {
                        foreach (NursingTransactionInterventionDt entityDtTemplate in lstEntityDtTemplate)
                        {
                            if (entityDtTemplate.IsContinued)
                            {
                                NursingTransactionInterventionDt entityDt = new NursingTransactionInterventionDt();
                                entityDt.NursingTransactionInterventionHdID = transactionHdID;
                                entityDt.NursingInterventionItemID = entityDtTemplate.NursingInterventionItemID;
                                entityDt.IsEditedByUser = entityDtTemplate.IsEditedByUser;
                                entityDt.NursingItemText = entityDtTemplate.NursingItemText;
                                entityDt.InterventionImplementation = entityDtTemplate.InterventionImplementation;
                                entityDt.CreatedBy = AppSession.UserLogin.UserID;

                                entityDtDao.Insert(entityDt);
                            }
                        }
                    }

                }
            }

        }
    }
}




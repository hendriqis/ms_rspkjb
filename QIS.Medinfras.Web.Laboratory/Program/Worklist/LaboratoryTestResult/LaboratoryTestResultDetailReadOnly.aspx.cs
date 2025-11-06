using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Laboratory.Program
{
    public partial class LaboratoryTestResultDetailReadOnly : BasePageTrx
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Laboratory.LAB_RESULT_V2;
        }

        protected string GetMainParamedicRole()
        {
            return Constant.ParamedicRole.PELAKSANA;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
         //   IsAllowVoid = IsAllowNextPrev = false;
         /////   btnImportResult.Visible = false;

         //   if (hdnIsBridgingLIS.Value == "1")
         //   {
         //       string filterExpression = string.Format("TransactionNo = '{0}'", hdnTransactionNo.Value);
         //       BridgingStatus oStatus = BusinessLayer.GetBridgingStatusList(filterExpression).FirstOrDefault();
         //       if (oStatus != null)
         //       {
         //           btnImportResult.Visible = oStatus.IsResultAvailable && hdnIsResultExists.Value == "0";
         //           btnVoidResult.Visible = !btnImportResult.Visible && hdnIsResultExists.Value == "1";
         //       }
         //   }
            //btnReopen.Visible = hdnIsResultExists.Value == "0";
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString.Count > 0)
            {
                string[] param = Page.Request.QueryString["id"].Split('|');
                hdnType.Value = param[0];
                String transactionID = param[1];
                hdnTransactionHdID.Value = transactionID;

                PatientChargesHd entityChargesHd = BusinessLayer.GetPatientChargesHd(Convert.ToInt32(hdnTransactionHdID.Value));
                txtTransactionNo.Text = entityChargesHd.TransactionNo;
                hdnTransactionNo.Value = entityChargesHd.TransactionNo;
                hdnTestOrderID.Value = entityChargesHd.TestOrderID.ToString();
                hdnVisitID.Value = entityChargesHd.VisitID.ToString();
                hdnHealthcareServiceUnitID.Value = entityChargesHd.HealthcareServiceUnitID.ToString();
                hdnDepartmentID.Value = Constant.Facility.DIAGNOSTIC;

                if (entityChargesHd != null)
                {
                    PatientChargesHdInfo entityChargesHdInfo = BusinessLayer.GetPatientChargesHdInfo(Convert.ToInt32(entityChargesHd.TransactionID));
                    if (entityChargesHdInfo != null)
                        hdnIsPathologicalAnatomyTest.Value = entityChargesHdInfo.IsPathologicalAnatomyTest ? "1" :  "0";
                }

                if (hdnTestOrderID.Value != "0" && hdnTestOrderID.Value != "")
                {
                    vTestOrderHd entityTestOrderHd = BusinessLayer.GetvTestOrderHdList(string.Format("TestOrderID = {0}", hdnTestOrderID.Value)).FirstOrDefault();
                    txtOrderBy.Text = entityTestOrderHd.ParamedicName;
                    txtOrderDate.Text = entityTestOrderHd.TestOrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtOrderTime.Text = entityTestOrderHd.TestOrderTime;
                    txtOrderNumber.Text = entityTestOrderHd.TestOrderNo;
                    if (hdnIsPathologicalAnatomyTest.Value == "0")
                        hdnIsPathologicalAnatomyTest.Value = entityTestOrderHd.IsPathologicalAnatomyTest ? "1" : "0"; 
                }

                vConsultVisit2 entity = BusinessLayer.GetvConsultVisit2List(string.Format("VisitID = {0}", hdnVisitID.Value)).FirstOrDefault();
                ctlPatientBanner.InitializePatientBanner(entity);

                hdnGCRegistrationStatus.Value = entity.GCVisitStatus;
                hdnRegistrationID.Value = entity.RegistrationID.ToString();
                hdnPhysicianID.Value = entity.ParamedicID.ToString();
                hdnPhysicianCode.Value = entity.ParamedicCode;
                hdnKdGudang.Value = entity.LocationID.ToString();
                hdnBusinessPartnerID.Value = entity.BusinessPartnerID.ToString();
                hdnClassID.Value = entity.ClassID.ToString();

                hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

                IsLoadFirstRecord = true;

                string filterExpression = String.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.LB_PRINT_HASIL_SETELAH_VERIFIKASI, Constant.SettingParameter.LB_BRIDGING_LIS);
                List<SettingParameterDt> lstParameter = BusinessLayer.GetSettingParameterDtList(filterExpression);

                hdnIsAllowByPassPrint.Value = lstParameter.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.LB_PRINT_HASIL_SETELAH_VERIFIKASI)).FirstOrDefault().ParameterValue;
                hdnIsBridgingLIS.Value = lstParameter.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.LB_BRIDGING_LIS)).FirstOrDefault().ParameterValue;

                Helper.SetControlEntrySetting(txtReferenceNo, new ControlEntrySetting(true, true, false), "mpEntry");
                Helper.SetControlEntrySetting(txtResultDate, new ControlEntrySetting(true, true, true), "mpEntry");
                Helper.SetControlEntrySetting(txtResultTime, new ControlEntrySetting(true, true, true), "mpEntry");
            }
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
        }

        protected void cbpViewHasil_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }

        #region Load Entity
        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            LaboratoryResultHd entityLabResult = BusinessLayer.GetLaboratoryResultHdList(string.Format("ChargeTransactionID = {0} AND IsDeleted = 0", hdnTransactionHdID.Value)).FirstOrDefault();
            if (entityLabResult != null)
            {
                EntityToControl(entityLabResult, ref isShowWatermark, ref watermarkText);
                hdnIsVerified.Value = entityLabResult.cfIsVerified ? "1" : "0";
                txtResultDate.Enabled = false;
                txtResultTime.Enabled = false;
            }
            else
            {
                txtReferenceNo.Text = "";
                txtResultDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtResultTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            }

            BindGridView();
        }

        public override int OnGetRowCount()
        {
            return 1;
        }

        private void EntityToControl(LaboratoryResultHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
            {
                hdnIsStatusOpen.Value = "1";
            }
            else
            {
                if (entity.GCTransactionStatus != Constant.TransactionStatus.PROCESSED)
                    hdnWatermarkText.Value = BusinessLayer.GetStandardCode(entity.GCTransactionStatus).TagProperty;
                else
                    hdnWatermarkText.Value = "VERIFIED";

                txtReferenceNo.Enabled = false;
                txtNotes.Enabled = false;
                hdnIsStatusOpen.Value = "0";
            }
            hdnResultGCTransactionStatus.Value = entity.GCTransactionStatus;
            hdnLabResultID.Value = entity.ID.ToString();
            txtReferenceNo.Text = entity.ReferenceNo;
            txtResultDate.Text = entity.ResultDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtResultTime.Text = entity.ResultTime;
            hdnResultAttachment.Value = entity.ResultAttachment;
            hdnTransactionHdID.Value = entity.ChargeTransactionID.ToString();
        }
        #endregion

        #region Save Entity
        public void SaveLaboratoryResultHd(IDbContext ctx, ref int labResultID, ref string referenceNo)
        {
            LaboratoryResultHdDao entityHdDao = new LaboratoryResultHdDao(ctx);
            if (hdnLabResultID.Value == "0")
            {
                LaboratoryResultHd entityHd = new LaboratoryResultHd();
                entityHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                entityHd.Remarks = txtNotes.Text;
                entityHd.ResultDate = Helper.GetDatePickerValue(txtResultDate.Text);
                entityHd.ResultTime = txtResultTime.Text;
                entityHd.ChargeTransactionID = Convert.ToInt32(hdnTransactionHdID.Value);
                if (hdnTestOrderID.Value == "0" || hdnTestOrderID.Value == "")
                    entityHd.TestOrderID = null;
                else
                    entityHd.TestOrderID = Convert.ToInt32(hdnTestOrderID.Value);


                string reffNo = txtReferenceNo.Text;
                if (AppSession.LB0034 == "1" && hdnIsPathologicalAnatomyTest.Value == "1")
                {
                    reffNo = BusinessLayer.GeneratePALaboratoryResultReferenceNo(AppSession.LB0035, entityHd.ResultDate, ctx);                 
                }

                entityHd.ReferenceNo = txtReferenceNo.Text = reffNo;
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();

                entityHd.CreatedBy = AppSession.UserLogin.UserID;

                labResultID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
                referenceNo = reffNo;
            }
            else
            {
                labResultID = Convert.ToInt32(hdnLabResultID.Value);
                referenceNo = txtReferenceNo.Text;
            }
        }

        private string GetFilterExpression()
        {
            string filterExpression = "";

            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("TransactionID = {0} AND IsTestItem = 1 AND IsDeleted = 0", hdnTransactionHdID.Value);
            return filterExpression;
        }

        private void BindGridView()
        {
            string filterExpression = GetFilterExpression();

            List<vPatientChargesDtLaboratoryResult> lstEntity = BusinessLayer.GetvPatientChargesDtLaboratoryResultList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();

            if (hdnLabResultID.Value == "")
                hdnLabResultID.Value = "0";

            //string orderBy = "PrintOrder, DisplayOrder";
            //if (AppSession.LIS_BRIDGING_PROTOCOL == Constant.LIS_PROVIDER.HCLAB)
            //{
            //    orderBy = "DisplayOrderSequence";
            //}
            string orderBy = "FractionDisplayOrder";
            List<vLaboratoryResultDt> lstLabEntity = BusinessLayer.GetvLaboratoryResultDtList(string.Format("ID = {0} ORDER BY {1}", hdnLabResultID.Value, orderBy));

            grdView2.DataSource = lstLabEntity;
            grdView2.DataBind();

            hdnIsResultExists.Value = lstLabEntity.Count > 0 ? "1" : "0";
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                int labResultID = 0;
                string referenceNo = string.Empty;
                SaveLaboratoryResultHd(ctx, ref labResultID, ref referenceNo);
                retval = labResultID.ToString() + "|" + referenceNo;
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
            LaboratoryResultHdDao entityDao = new LaboratoryResultHdDao(ctx);
            try
            {
                LaboratoryResultHd entity = entityDao.Get(Convert.ToInt32(hdnLabResultID.Value));
                entity.ReferenceNo = txtReferenceNo.Text;
                entity.Remarks = txtNotes.Text;
                entityDao.Update(entity);

                retval = hdnLabResultID.Value;
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

        #region Proposed Entity
        protected override bool OnProposeRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            LaboratoryResultHdDao entityHdDao = new LaboratoryResultHdDao(ctx);
            try
            {
                if (hdnLabResultID.Value != null && hdnLabResultID.Value != "")
                {
                    string filterLaboratoryHd = string.Format("ChargeTransactionID = {0} AND ID = {1} AND GCTransactionStatus = '{2}' AND IsDeleted = 0",
                                                                                                        hdnTransactionHdID.Value, hdnLabResultID.Value, Constant.TransactionStatus.OPEN);
                    LaboratoryResultHd entityResult = BusinessLayer.GetLaboratoryResultHdList(filterLaboratoryHd, ctx).FirstOrDefault();
                    if (entityResult != null)
                    {
                        entityResult.ReferenceNo = txtReferenceNo.Text;
                        entityResult.Remarks = txtNotes.Text;
                        entityResult.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                        entityResult.ProposedBy = AppSession.UserLogin.UserID;
                        entityResult.ProposedDate = DateTime.Now;
                        entityResult.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityHdDao.Update(entityResult);
                    }
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Hasil pemeriksaan tidak ditemukan.";
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
            return result;
        }
        #endregion

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;

            if (type.Contains(";"))
            {
                #region Void/Delete Result

                string[] param = type.Split(';');
                string gcDeleteReason = param[1];
                string reason = param[2];

                if (param[0] == "delete_result")
                {
                    IDbContext ctx = DbFactory.Configure(true);
                    PatientChargesDtDao patientChargesDtDao = new PatientChargesDtDao(ctx);
                    LaboratoryResultHdDao entityHdDao = new LaboratoryResultHdDao(ctx);
                    LaboratoryResultDtDao entityDtDao = new LaboratoryResultDtDao(ctx);
                    try
                    {
                        if (hdnLabResultID.Value != null && hdnLabResultID.Value != "")
                        {
                            string filterLaboratoryHd = string.Format("ChargeTransactionID = {0} AND ID = {1} AND GCTransactionStatus = '{2}' AND IsDeleted = 0",
                                                                                                                hdnTransactionHdID.Value, hdnLabResultID.Value, Constant.TransactionStatus.OPEN);
                            LaboratoryResultHd entityResult = BusinessLayer.GetLaboratoryResultHdList(filterLaboratoryHd, ctx).FirstOrDefault();
                            if (entityResult != null)
                            {
                                List<LaboratoryResultDt> lstEntityDt = BusinessLayer.GetLaboratoryResultDtList(string.Format("ID = {0} AND IsDeleted = 0", entityResult.ID), ctx);
                                foreach (LaboratoryResultDt entityDt in lstEntityDt)
                                {
                                    List<PatientChargesDt> lstChargesDt = BusinessLayer.GetPatientChargesDtList(string.Format(
                                                                                            "TransactionID = {0} AND ItemID = {1} AND IsDeleted = 0 AND ISNULL(GCTransactionDetailStatus,'') != '{2}'",
                                                                                            entityResult.ChargeTransactionID, entityDt.ItemID, Constant.TransactionStatus.VOID), ctx);
                                    foreach (PatientChargesDt chargesDt in lstChargesDt)
                                    {
                                        chargesDt.IsHasTestResult = false;
                                        chargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        chargesDt.LastUpdatedDate = DateTime.Now;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        patientChargesDtDao.Update(chargesDt);
                                    }

                                    entityDt.IsDeleted = true;
                                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    entityDtDao.Update(entityDt);
                                }

                                entityResult.IsDeleted = true;
                                entityResult.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                entityResult.GCDeleteReason = gcDeleteReason;
                                if (gcDeleteReason == Constant.DeleteReason.OTHER)
                                {
                                    entityResult.DeleteReason = reason;
                                }
                                entityResult.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                entityHdDao.Update(entityResult);
                                ctx.CommitTransaction();
                            }
                        }
                        else
                        {
                            result = false;
                            errMessage = "Hasil pemeriksaan tidak ditemukan.";
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

                #endregion
            }
            else
            {
                if (type == "saveparamedic")
                {
                    result = OnSaveEditParamedic(ref errMessage);
                    BindGridView();
                }
                else if (type == "reopen_result")
                {
                    #region Reopen Result

                    IDbContext ctx = DbFactory.Configure(true);
                    LaboratoryResultHdDao entityHdDao = new LaboratoryResultHdDao(ctx);
                    try
                    {
                        if (hdnLabResultID.Value != null && hdnLabResultID.Value != "")
                        {
                            string filterLaboratoryHd = string.Format("ChargeTransactionID = {0} AND ID = {1} AND GCTransactionStatus = '{2}' AND IsDeleted = 0",
                                                                                                                hdnTransactionHdID.Value, hdnLabResultID.Value, Constant.TransactionStatus.OPEN);
                            LaboratoryResultHd entityResult = BusinessLayer.GetLaboratoryResultHdList(filterLaboratoryHd, ctx).FirstOrDefault();
                            if (entityResult != null)
                            {
                                entityResult.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                                entityResult.ProposedBy = null;
                                entityResult.ProposedDate = null;
                                entityResult.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                entityHdDao.Update(entityResult);
                            }
                            ctx.CommitTransaction();
                        }
                        else
                        {
                            result = false;
                            errMessage = "Hasil pemeriksaan tidak ditemukan.";
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

                    #endregion
                }
            }
            return result;
        }

        protected bool OnSaveEditParamedic(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesDtDao entityDao = new PatientChargesDtDao(ctx);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            ItemMasterDao itemMasterDao = new ItemMasterDao(ctx);
            try
            {
                if (hdnChargesDtID.Value != "0" && hdnChargesDtID.Value != "")
                {
                    PatientChargesDt entity = entityDao.Get(Convert.ToInt32(hdnChargesDtID.Value));
                    PatientChargesHd entityHd = entityHdDao.Get(entity.TransactionID);

                    string filterRevDt = string.Format("PatientChargesDtID = {0} AND PatientChargesID = {1} AND IsDeleted = 0 AND RSTransactionID IN (SELECT hd.RSTransactionID FROM TransRevenueSharingHd hd WITH(NOLOCK) WHERE GCTransactionStatus != '{2}')",
                                                            entity.ID, entity.TransactionID, Constant.TransactionStatus.VOID);
                    List<TransRevenueSharingDt> lstTransRevDt = BusinessLayer.GetTransRevenueSharingDtList(filterRevDt);
                    if (lstTransRevDt.Count == 0)
                    {
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        ItemMaster im = itemMasterDao.Get(entity.ItemID);
                        GetItemRevenueSharing rv = BusinessLayer.GetItemRevenueSharing(im.ItemCode, Convert.ToInt32(hdnParamedicID.Value), entity.ChargeClassID, GetMainParamedicRole(), Convert.ToInt32(hdnVisitID.Value), entityHd.HealthcareServiceUnitID, entityHd.TransactionDate, entityHd.TransactionTime, ctx).FirstOrDefault();
                        entity.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
                        if (rv.RevenueSharingID != null && rv.RevenueSharingID != 0)
                        {
                            entity.RevenueSharingID = rv.RevenueSharingID;
                        }
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityDao.Update(entity);

                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Dokter/Paramedis tidak bisa diubah karena transaksi ini sudah proses honor dokter.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
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

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }
    }
}
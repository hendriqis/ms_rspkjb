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
using QIS.Medinfras.Web.CommonLibs.Controls;
using QIS.Medinfras.Web.CommonLibs.Program;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class MDTestResultDetail : BasePageTrx
    {
        protected bool IsEditable = true;

        public override string OnGetMenuCode()
        {
            if (hdnType.Value == "hs")
            {
                if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                    return Constant.MenuCode.Imaging.IMAGING_RESULT_HISTORY;
                return Constant.MenuCode.MedicalDiagnostic.MD_RESULT_HISTORY;
            }
            else
            {
                if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                    return Constant.MenuCode.Imaging.IMAGING_RESULT;
                else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                    return Constant.MenuCode.Radiotheraphy.RADIOTERAPHY_RESULT;
                return Constant.MenuCode.MedicalDiagnostic.MD_RESULT;
            }
        }

        public int GetVisitID()
        {
            return Convert.ToInt32(Request.Form[hdnVisitID.UniqueID]);
        }

        protected string GetMainParamedicRole()
        {
            return Constant.ParamedicRole.PELAKSANA;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowVoid = IsAllowNextPrev = false;
        }

        protected override void InitializeDataControl()
        {
            string TransactionID;
            if (Page.Request.QueryString.Count > 0)
            {
                GetSettingParameter();

                string[] param = Page.Request.QueryString["id"].Split('|');
                hdnType.Value = param[0];
                TransactionID = param[1];
                hdnIsImagingResult.Value = AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging ? "1" : "0";
                hdnIsBridgingToRIS.Value = AppSession.IsBridgingToRIS ? "1" : "0";

                vPatientChargesHd entityPCHD = BusinessLayer.GetvPatientChargesHdList(string.Format("TransactionID = {0}", TransactionID)).FirstOrDefault();
                hdnTransactionHdID.Value = TransactionID;
                hdnVisitID.Value = Convert.ToString(entityPCHD.VisitID);
                hdnTestOrderID.Value = entityPCHD.TestOrderID.ToString();
                txtTransactionNo.Text = entityPCHD.TransactionNo;

                if (hdnTestOrderID.Value != "0" && hdnTestOrderID.Value != "")
                {
                    TestOrderHd entityTestOrderHd = BusinessLayer.GetTestOrderHdList(string.Format("TestOrderID = {0}", hdnTestOrderID.Value)).FirstOrDefault();

                    if (entityTestOrderHd.ParamedicID != null && entityTestOrderHd.ParamedicID != 0)
                    {
                        ParamedicMaster entityPM = BusinessLayer.GetParamedicMaster(entityTestOrderHd.ParamedicID);
                        txtOrderBy.Text = entityPM.FullName;
                    }
                    txtDiagnosa.Text = entityTestOrderHd.Remarks;
                    txtOrderDate.Text = entityTestOrderHd.TestOrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtOrderTime.Text = entityTestOrderHd.TestOrderTime;
                    txtOrderNo.Text = entityTestOrderHd.TestOrderNo;
                }

                vConsultVisit2 entity = BusinessLayer.GetvConsultVisit2List(string.Format("VisitID = {0}", hdnVisitID.Value)).FirstOrDefault();
                ((PatientBannerCtl)ctlPatientBanner).InitializePatientBanner(entity);

                hdnGCRegistrationStatus.Value = entity.GCVisitStatus;
                hdnRegistrationID.Value = entity.RegistrationID.ToString();
                hdnPhysicianID.Value = entity.ParamedicID.ToString();
                hdnPhysicianCode.Value = entity.ParamedicCode;
                hdnKdGudang.Value = entity.LocationID.ToString();
                hdnBusinessPartnerID.Value = entity.BusinessPartnerID.ToString();
                hdnClassID.Value = entity.ClassID.ToString();

                hdnHealthcareServiceUnitID.Value = entityPCHD.HealthcareServiceUnitID.ToString();
                hdnDepartmentID.Value = Constant.Facility.DIAGNOSTIC;
                hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

                IsLoadFirstRecord = true;

                string filterSC = string.Format("ParentID IN ('{0}') AND IsActive = 1 AND IsDeleted = 0",
                                                            Constant.StandardCode.RESULT_DELIVERY_PLAN //0
                                                        );
                List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterSC);
                lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });

                Methods.SetComboBoxField<StandardCode>(cboResultDeliveryPlan, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.RESULT_DELIVERY_PLAN || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

                Registration reg = BusinessLayer.GetRegistration(entity.RegistrationID);
                if (reg.GCResultDeliveryPlan != null)
                {
                    cboResultDeliveryPlan.Value = reg.GCResultDeliveryPlan;
                    if (reg.ResultDeliveryPlanOthers != null)
                    {
                        txtResultDeliveryPlanOthers.Text = reg.ResultDeliveryPlanOthers;
                        txtResultDeliveryPlanOthers.Attributes.Remove("readonly");
                    }
                    else
                    {
                        txtResultDeliveryPlanOthers.Text = "";
                    }
                }
                else
                {
                    cboResultDeliveryPlan.Value = "";
                    txtResultDeliveryPlanOthers.Text = "";
                }

                Helper.SetControlEntrySetting(txtReferenceNo, new ControlEntrySetting(true, true, false), "mpEntry");
                Helper.SetControlEntrySetting(txtResultDate, new ControlEntrySetting(true, true, true), "mpEntry");
                Helper.SetControlEntrySetting(txtResultTime, new ControlEntrySetting(true, true, true), "mpEntry");
                Helper.SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false), "mpEntry");
                Helper.SetControlEntrySetting(txtParamedicCode, new ControlEntrySetting(true, true, true), "mpTrxPopup");
                Helper.SetControlEntrySetting(txtParamedicName, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            }
        }

        private void GetSettingParameter()
        {
            //Healthcare Setting Parameter
            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                                                "HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
                                                                                AppSession.UserLogin.HealthcareID,
                                                                                Constant.SettingParameter.IS_PRINT_HASIL_SETELAH_VERIFIKASI));

            hdnIsAllowPrintAfterVerified.Value = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.IS_PRINT_HASIL_SETELAH_VERIFIKASI).FirstOrDefault().ParameterValue;
        }

        #region Load Entity
        public override int OnGetRowCount()
        {
            return 1;
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            ImagingResultHd entityResult = BusinessLayer.GetImagingResultHdList(string.Format("ChargeTransactionID = {0} AND IsDeleted = 0", hdnTransactionHdID.Value)).FirstOrDefault();

            if (entityResult != null)
            {
                EntityToControl(entityResult, ref isShowWatermark, ref watermarkText);
                IsEditable = (entityResult.GCTransactionStatus == Constant.TransactionStatus.OPEN);
                txtResultDate.Enabled = false;
                txtResultTime.Enabled = false;
            }
            else
            {
                IsEditable = true;
                txtReferenceNo.Text = "";
                txtResultDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtResultTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                txtNotes.Text = "";
                string filterExpression = string.Format("TransactionID = {0} AND IsDeleted = 0", hdnTransactionHdID.Value);
                if (hdnID.Value == "")
                    hdnID.Value = "0";
                BindGridView();
            }
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            ImagingResultHd entityResult = BusinessLayer.GetImagingResultHdList(string.Format("ChargeTransactionID = {0} AND IsDeleted = 0", hdnTransactionHdID.Value)).FirstOrDefault();

            if (entityResult != null)
            {
                EntityToControl(entityResult, ref isShowWatermark, ref watermarkText);
                IsEditable = (entityResult.GCTransactionStatus == Constant.TransactionStatus.OPEN);
                txtResultDate.Enabled = false;
                txtResultTime.Enabled = false;
            }
            else
            {
                IsEditable = true;
                txtReferenceNo.Text = "";
                txtResultDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtResultTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                txtNotes.Text = "";
                string filterExpression = string.Format("TransactionID = {0} AND IsDeleted = 0", hdnTransactionHdID.Value);
                if (hdnID.Value == "")
                    hdnID.Value = "0";
                BindGridView();
            }
        }

        private void EntityToControl(ImagingResultHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity != null)
            {
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    IsEditable = true;
                }
                else
                {
                    IsEditable = false;
                }
            }
            else
            {
                IsEditable = true;
            }

            if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
            {
                hdnIsStatusOpen.Value = "1";
            }
            else
            {
                hdnWatermarkText.Value = BusinessLayer.GetStandardCode(entity.GCTransactionStatus).TagProperty;
                txtReferenceNo.Enabled = false;
                txtNotes.Enabled = false;
                hdnIsStatusOpen.Value = "0";
            }

            hdnID.Value = entity.ID.ToString();
            txtReferenceNo.Text = entity.ReferenceNo;
            txtResultDate.Text = entity.ResultDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtResultTime.Text = entity.ResultTime;
            txtNotes.Text = entity.Remarks;

            if (entity.GCResultDeliveryPlan != null && entity.GCResultDeliveryPlan != "")
            {
                cboResultDeliveryPlan.Value = entity.GCResultDeliveryPlan;
                if (entity.GCResultDeliveryPlan == Constant.ResultDeliveryPlan.OTHERS)
                {
                    txtResultDeliveryPlanOthers.Text = entity.ResultDeliveryPlanOthers;
                    txtResultDeliveryPlanOthers.Attributes.Remove("readonly");
                }
                else
                {
                    txtResultDeliveryPlanOthers.Text = "";
                }
            }
            else
            {
                cboResultDeliveryPlan.Value = "";
                txtResultDeliveryPlanOthers.Text = "";
            }

            hdnResultGCTransactionStatus.Value = entity.GCTransactionStatus;
            hdnTransactionHdID.Value = entity.ChargeTransactionID.ToString();
            hdnIsResultVerified.Value = entity.cfIsVerified ? "1" : "0";

            PatientChargesHd chargesHd = BusinessLayer.GetPatientChargesHd(entity.ChargeTransactionID);
            txtTransactionNo.Text = chargesHd.TransactionNo;
            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("TransactionID = {0} AND IsDeleted = 0 AND IsTestItem = 1", hdnTransactionHdID.Value);
            if (hdnID.Value == "")
                hdnID.Value = "0";
            List<vPatientChargesDtImagingResult> lstEntity = BusinessLayer.GetvPatientChargesDtImagingResultList(filterExpression, int.MaxValue, 1, "ItemCode");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
        #endregion

        #region Save Entity
        private void ControlToEntity(ImagingResultHd entity)
        {
            entity.ReferenceNo = txtReferenceNo.Text;
            entity.ResultDate = Helper.GetDatePickerValue(txtResultDate.Text);
            entity.ResultTime = txtResultTime.Text;
            entity.Remarks = txtNotes.Text;
            entity.IsInternal = true;
            entity.IsDeleted = false;
            entity.VisitID = Convert.ToInt32(hdnVisitID.Value);
            entity.ChargeTransactionID = Convert.ToInt32(hdnTransactionHdID.Value);

            if (cboResultDeliveryPlan.Value != null)
            {
                if (cboResultDeliveryPlan.Value.ToString() != "")
                {
                    entity.GCResultDeliveryPlan = cboResultDeliveryPlan.Value.ToString();
                    if (cboResultDeliveryPlan.Value.ToString() == Constant.ResultDeliveryPlan.OTHERS)
                    {
                        entity.ResultDeliveryPlanOthers = Request.Form[txtResultDeliveryPlanOthers.UniqueID];
                    }
                    else
                    {
                        entity.ResultDeliveryPlanOthers = null;
                    }
                }
                else
                {
                    entity.GCResultDeliveryPlan = null;
                    entity.ResultDeliveryPlanOthers = null;
                }
            }
            else
            {
                entity.GCResultDeliveryPlan = null;
                entity.ResultDeliveryPlanOthers = null;
            }

            hdnResultGCTransactionStatus.Value = entity.GCTransactionStatus;

            if (hdnTestOrderID.Value != "" && hdnTestOrderID.Value != "0")
                entity.TestOrderID = Convert.ToInt32(hdnTestOrderID.Value);
            else
                entity.TestOrderID = null;
        }

        public void SaveImagingResultHd(IDbContext ctx, ref int ImagingHdID)
        {
            ImagingResultHdDao entityHdDao = new ImagingResultHdDao(ctx);
            string filterImagingHd = string.Format("ChargeTransactionID = {0} AND ID = {1} AND GCTransactionStatus = '{2}' AND IsDeleted = 0",
                                                                                                hdnTransactionHdID.Value, hdnID.Value, Constant.TransactionStatus.OPEN);
            ImagingResultHd entityResult = BusinessLayer.GetImagingResultHdList(filterImagingHd, ctx).FirstOrDefault();
            if (entityResult != null)
            {
                hdnID.Value = Convert.ToString(entityResult.ID);
                ImagingHdID = Convert.ToInt32(hdnID.Value);
            }
            else
            {
                ImagingResultHd entityHd = new ImagingResultHd();
                ControlToEntity(entityHd);
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                ImagingHdID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
            }
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ImagingResultHdDao entityDao = new ImagingResultHdDao(ctx);
            TestOrderHdDao entityOrderDao = new TestOrderHdDao(ctx);
            try
            {
                if (hdnID.Value != "0")
                {
                    ImagingResultHd entity = BusinessLayer.GetImagingResultHd(Convert.ToInt32(hdnID.Value));
                    ControlToEntity(entity);
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDao.Update(entity);

                    TestOrderHd entityOrder = BusinessLayer.GetTestOrderHd(Convert.ToInt32(hdnTestOrderID.Value));
                    if (entityOrder != null)
                    {
                        entityOrder.Remarks = txtDiagnosa.Text;
                        entityOrder.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityOrderDao.Update(entityOrder);
                    }

                    retval = hdnID.Value;
                }
                else
                {
                    ImagingResultHd entity = new ImagingResultHd();
                    ControlToEntity(entity);
                    entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                    entity.CreatedBy = AppSession.UserLogin.UserID;

                    TestOrderHd entityOrder = BusinessLayer.GetTestOrderHd(Convert.ToInt32(hdnTestOrderID.Value));
                    if (entityOrder != null)
                    {
                        entityOrder.Remarks = txtDiagnosa.Text;
                        entityOrder.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityOrderDao.Update(entityOrder);
                    }

                    retval = entityDao.InsertReturnPrimaryKeyID(entity).ToString();
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
            string retval = "";
            string errMessage = "";
            if (OnSaveHdRecord(ref errMessage, ref retval))
                result += "success";
            else
                result += string.Format("fail|{0}", errMessage);
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = retval;
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

        private bool OnSaveHdRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                int ImagingHdID = 0;
                SaveImagingResultHd(ctx, ref ImagingHdID);
                retval = ImagingHdID.ToString();
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
            ImagingResultHdDao entityHdDao = new ImagingResultHdDao(ctx);
            try
            {
                if (hdnID.Value != null && hdnID.Value != "")
                {
                    string filterImagingHd = string.Format("ChargeTransactionID = {0} AND ID = {1} AND GCTransactionStatus = '{2}' AND IsDeleted = 0",
                                                                                                        hdnTransactionHdID.Value, hdnID.Value, Constant.TransactionStatus.OPEN);
                    ImagingResultHd entityResult = BusinessLayer.GetImagingResultHdList(filterImagingHd, ctx).FirstOrDefault();
                    if (entityResult != null)
                    {
                        if (cboResultDeliveryPlan.Value != null)
                        {
                            if (cboResultDeliveryPlan.Value.ToString() != "")
                            {
                                entityResult.GCResultDeliveryPlan = cboResultDeliveryPlan.Value.ToString();
                                if (cboResultDeliveryPlan.Value.ToString() == Constant.ResultDeliveryPlan.OTHERS)
                                {
                                    entityResult.ResultDeliveryPlanOthers = Request.Form[txtResultDeliveryPlanOthers.UniqueID];
                                }
                                else
                                {
                                    entityResult.ResultDeliveryPlanOthers = null;
                                }
                            }
                            else
                            {
                                entityResult.GCResultDeliveryPlan = null;
                                entityResult.ResultDeliveryPlanOthers = null;
                            }
                        }
                        else
                        {
                            entityResult.GCResultDeliveryPlan = null;
                            entityResult.ResultDeliveryPlanOthers = null;
                        }

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

        #region Custom Button
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
                    ImagingResultHdDao entityHdDao = new ImagingResultHdDao(ctx);
                    ImagingResultDtDao entityDtDao = new ImagingResultDtDao(ctx);
                    try
                    {
                        if (hdnID.Value != null && hdnID.Value != "")
                        {
                            string filterImagingHd = string.Format("ChargeTransactionID = {0} AND ID = {1} AND GCTransactionStatus = '{2}' AND IsDeleted = 0",
                                                                                                                hdnTransactionHdID.Value, hdnID.Value, Constant.TransactionStatus.OPEN);
                            ImagingResultHd entityResult = BusinessLayer.GetImagingResultHdList(filterImagingHd, ctx).FirstOrDefault();
                            if (entityResult != null)
                            {
                                List<ImagingResultDt> lstEntityDt = BusinessLayer.GetImagingResultDtList(string.Format("ID = {0} AND IsDeleted = 0", entityResult.ID), ctx);
                                foreach (ImagingResultDt entityDt in lstEntityDt)
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
                    ImagingResultHdDao entityHdDao = new ImagingResultHdDao(ctx);
                    try
                    {
                        if (hdnID.Value != null && hdnID.Value != "")
                        {
                            string filterImagingHd = string.Format("ChargeTransactionID = {0} AND ID = {1} AND GCTransactionStatus = '{2}' AND IsDeleted = 0",
                                                                                                                hdnTransactionHdID.Value, hdnID.Value, Constant.TransactionStatus.WAIT_FOR_APPROVAL);
                            ImagingResultHd entityResult = BusinessLayer.GetImagingResultHdList(filterImagingHd, ctx).FirstOrDefault();
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
        #endregion

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }
    }
}
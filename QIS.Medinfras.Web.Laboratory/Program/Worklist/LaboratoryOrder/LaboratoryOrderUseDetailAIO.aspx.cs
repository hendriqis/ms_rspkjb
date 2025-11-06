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
using QIS.Medinfras.Web.CommonLibs.Service;

namespace QIS.Medinfras.Web.Laboratory.Program
{
    public partial class LaboratoryOrderUseDetailAIO : BasePageTrxPatientManagement
    {
        protected int testOrderOpenCount = 0;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Laboratory.WORK_LIST_AIO;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = (hdnGCRegistrationStatus.Value != Constant.VisitStatus.CLOSED);
        }

        public override string GetGCCustomerType()
        {
            return hdnGCCustomerType.Value;
        }

        public override string GetGCTransactionStatus()
        {
            return hdnGCTransactionStatus.Value;
        }

        public override string GetRemarksHd()
        {
            return "";
        }

        public override string GetReferenceNoHd()
        {
            return "";
        }

        public override bool GetIsCorrectionTransactionHd()
        {
            return false;
        }

        public override bool GetIsAIOTransactionHd()
        {
            return false;
        }

        public override int GetClassID()
        {
            return Convert.ToInt32(hdnClassID.Value);
        }
        public override int GetRegistrationPhysicianID()
        {
            return Convert.ToInt32(hdnRegistrationPhysicianID.Value);
        }
        public override int GetLocationID()
        {
            return Convert.ToInt32(hdnLocationID.Value);
        }
        public override int GetLogisticLocationID()
        {
            return Convert.ToInt32(hdnLogisticLocationID.Value);
        }
        public override int GetHealthcareServiceUnitID()
        {
            return Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
        }
        public override string GetTransactionHdID()
        {
            return hdnTransactionHdID.Value;
        }
        public override void SetTransactionHdID(string value)
        {
            hdnTransactionHdID.Value = value;
        }
        public override string GetDepartmentID()
        {
            return hdnDepartmentID.Value;
        }
        public override string GetTransactionDate()
        {
            return Request.Form[txtTransactionDateLab.UniqueID];
        }
        public override string GetTransactionTime()
        {
            return Request.Form[txtTransactionTime.UniqueID];
        }

        public override String GetTransactionDate2()
        {
            return txtTransactionDateLab.Text;
        }
        public override String GetTransactionTime2()
        {
            return txtTransactionTime.Text;
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString.Count > 0)
            {
                string[] param = Page.Request.QueryString["id"].Split('|');
                if (param[0] == "to")
                {
                    hdnVisitID.Value = param[1];
                    //hdnDefaultTestOrderID.Value = 
                    hdnTestOrderID.Value = param[2];
                    testOrderOpenCount = BusinessLayer.GetTestOrderDtList(string.Format("TestOrderID = {0} AND GCTestOrderStatus = '{1}'", param[2], Constant.TestOrderStatus.OPEN)).Count;
                    hdnHealthcareServiceUnitID.Value = AppSession.MedicalDiagnostic.HealthcareServiceUnitID.ToString();
                }
                else
                {
                    hdnVisitID.Value = param[0];
                    hdnHealthcareServiceUnitID.Value = param[1];
                    hdnTestOrderID.Value = "0";
                    btnClinicTransactionTestOrder.Style.Add("display", "none");
                }
                vConsultVisit2 entity = BusinessLayer.GetvConsultVisit2List(string.Format("VisitID = {0}", hdnVisitID.Value))[0];

                List<vPatientVisitNote> lstEntity = BusinessLayer.GetvPatientVisitNoteList(string.Format("VisitID = {0} AND IsDeleted = 0", hdnVisitID.Value));
                if (lstEntity.Count == 0)
                {
                    divVisitNote.Attributes.Add("style", "display:none");
                }

                ctlPatientBanner.InitializePatientBanner(entity);
                hdnGCRegistrationStatus.Value = entity.GCVisitStatus;
                hdnRegistrationID.Value = entity.RegistrationID.ToString();
                hdnRegistrationPhysicianID.Value = hdnPhysicianID.Value = entity.ParamedicID.ToString();
                hdnPhysicianCode.Value = entity.ParamedicCode;
                hdnPhysicianName.Value = entity.ParamedicName;
                vHealthcareServiceUnit hsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", hdnHealthcareServiceUnitID.Value))[0];
                hdnDepartmentID.Value = hsu.DepartmentID;
                hdnLocationID.Value = hsu.LocationID.ToString();
                hdnLogisticLocationID.Value = hsu.LogisticLocationID.ToString();
                //hdnLocationID.Value = entity.LocationID.ToString();
                //hdnLogisticLocationID.Value = entity.LogisticLocationID.ToString();
                //hdnDepartmentID.Value = Constant.Facility.DIAGNOSTIC;

                hdnBusinessPartnerID.Value = entity.BusinessPartnerID.ToString();
                hdnGCCustomerType.Value = entity.GCCustomerType;
                hdnClassID.Value = entity.ChargeClassID.ToString();

                IsLoadFirstRecord = (OnGetRowCount() > 0);
                if (IsLoadFirstRecord)
                {
                    if (hdnTestOrderID.Value != "0")
                    {
                        string filterExpression = string.Format("VisitID = {0} AND HealthcareServiceUnitID = {1} AND TestOrderID = {2} AND GCTransactionStatus != '{3}'", hdnVisitID.Value, hdnHealthcareServiceUnitID.Value, hdnTestOrderID.Value, Constant.TransactionStatus.VOID);
                        if (BusinessLayer.GetvPatientChargesHd2RowCount(filterExpression) <= 0) IsLoadFirstRecord = false;
                    }
                }
                hdnTransactionHdID.Value = "0";

                List<ParamedicMaster> lstParamedic = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {0}) AND IsDeleted = 0", hdnHealthcareServiceUnitID.Value));
                if (lstParamedic.Count == 1)
                {
                    ParamedicMaster paramedic = lstParamedic.FirstOrDefault();
                    hdnPhysicianID.Value = paramedic.ParamedicID.ToString();
                    hdnPhysicianCode.Value = paramedic.ParamedicCode;
                    hdnPhysicianName.Value = paramedic.FullName;
                }
                else
                {
                    hdnPhysicianID.Value = hdnPhysicianCode.Value = hdnPhysicianName.Value = "";
                }

                int count = BusinessLayer.GetServiceUnitParamedicRowCount(string.Format("HealthcareServiceUnitID = {0}", hdnHealthcareServiceUnitID.Value));
                if (count > 0)
                    hdnIsHealthcareServiceUnitHasParamedic.Value = "1";
                else
                    hdnIsHealthcareServiceUnitHasParamedic.Value = "0";

                ((PatientUseDetailServiceCtl)ctlService).OnAddRecord();
                ((PatientUseDetailDrugMSCtl)ctlDrugMS).OnAddRecord();
                ((PatientUseDetailDrugMSReturnCtl)ctlDrugMSReturn).OnAddRecord();
                ((PatientUseDetailLogisticCtl)ctlLogistic).OnAddRecord();
                ((PatientUseDetailLogisticReturnCtl)ctlLogisticReturn).OnAddRecord();

                hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

                GetSettingParameter();
            }

            string moduleName = Helper.GetModuleName();
            string ModuleID = Helper.GetModuleID(moduleName);
            GetUserMenuAccess menu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault();
            string CRUDMode = menu.CRUDMode;
            hdnIsAllowVoid.Value = CRUDMode.Contains("X") ? "1" : "0";
        }

        private void GetSettingParameter()
        {
            List<SettingParameterDt> lstParameter = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_PROPOSE_VALIDASI_TARIF_0));
            if (lstParameter.Count > 0)
            {
                hdnValidateTariffOnPropose.Value = lstParameter.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.FN_PROPOSE_VALIDASI_TARIF_0)).FirstOrDefault().ParameterValue;
            }
        }

        protected override void SetControlProperties()
        {
            ((PatientUseDetailServiceCtl)ctlService).SetControlProperties();
            ((PatientUseDetailDrugMSCtl)ctlDrugMS).SetControlProperties();
            ((PatientUseDetailDrugMSReturnCtl)ctlDrugMSReturn).SetControlProperties();
            ((PatientUseDetailLogisticCtl)ctlLogistic).SetControlProperties();
            ((PatientUseDetailLogisticReturnCtl)ctlLogisticReturn).SetControlProperties();
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnTransactionHdID, new ControlEntrySetting(false, false, false, "0"));

            SetControlEntrySetting(txtTransactionNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtTransactionDateLab, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtTransactionTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(txtReferenceNo, new ControlEntrySetting(true, true, false));
        }

        public override void OnAddRecord()
        {
            string[] param = Page.Request.QueryString["id"].Split('|');
            //if (param[0] == "to")
            //    hdnTestOrderID.Value = param[2];
            hdnTestOrderID.Value = "0";
            hdnGCTransactionStatus.Value = Constant.TransactionStatus.OPEN;
            ((PatientUseDetailServiceCtl)ctlService).OnAddRecord();
            ((PatientUseDetailDrugMSCtl)ctlDrugMS).OnAddRecord();
            ((PatientUseDetailDrugMSReturnCtl)ctlDrugMSReturn).OnAddRecord();
            ((PatientUseDetailLogisticCtl)ctlLogistic).OnAddRecord();
            ((PatientUseDetailLogisticReturnCtl)ctlLogisticReturn).OnAddRecord();
        }

        public override int OnGetRowCount()
        {
            string filterExpression = string.Format("VisitID = {0} AND HealthcareServiceUnitID = {1}", hdnVisitID.Value, hdnHealthcareServiceUnitID.Value);
            return BusinessLayer.GetvPatientChargesHd2RowCount(filterExpression);
        }

        #region Load Entity
        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = string.Empty;
            /*if (hdnTestOrderID.Value != "" && hdnTestOrderID.Value != "0")
                filterExpression = string.Format("VisitID = {0} AND HealthcareServiceUnitID = {1} AND TestOrderID = {2}", hdnVisitID.Value, hdnHealthcareServiceUnitID.Value, hdnTestOrderID.Value);
            else*/
            filterExpression = string.Format("VisitID = {0} AND HealthcareServiceUnitID = {1}", hdnVisitID.Value, hdnHealthcareServiceUnitID.Value);
            vPatientChargesHd2 entity = BusinessLayer.GetvPatientChargesHd2(filterExpression, PageIndex, " TransactionID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {

            string filterExpression = string.Format("VisitID = {0} AND HealthcareServiceUnitID = {1}", hdnVisitID.Value, hdnHealthcareServiceUnitID.Value);
            PageIndex = BusinessLayer.GetvPatientChargesHd2RowIndex(filterExpression, keyValue, "TransactionID DESC");
            vPatientChargesHd2 entity = BusinessLayer.GetvPatientChargesHd2(filterExpression, PageIndex, "TransactionID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vPatientChargesHd2 entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                watermarkText = entity.TransactionStatusWatermark;
            }
            hdnGCTransactionStatus.Value = entity.GCTransactionStatus;
            hdnTestOrderID.Value = entity.TestOrderID.ToString();
            hdnTransactionHdID.Value = entity.TransactionID.ToString();
            txtTransactionNo.Text = entity.TransactionNo;
            txtReferenceNo.Text = entity.ReferenceNo;
            txtTestOrderInfo.Text = entity.TestOrderInfo;
            txtTransactionDateLab.Text = entity.TransactionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTransactionTime.Text = entity.TransactionTime;
            bool flagHaveCharges = false;
            if (entity != null) flagHaveCharges = true;
            ((PatientUseDetailServiceCtl)ctlService).InitializeTransactionControl(flagHaveCharges);
            ((PatientUseDetailDrugMSCtl)ctlDrugMS).InitializeTransactionControl(flagHaveCharges);
            ((PatientUseDetailDrugMSReturnCtl)ctlDrugMSReturn).InitializeTransactionControl(flagHaveCharges);
            ((PatientUseDetailLogisticCtl)ctlLogistic).InitializeTransactionControl(flagHaveCharges);
            ((PatientUseDetailLogisticReturnCtl)ctlLogisticReturn).InitializeTransactionControl(flagHaveCharges);
        }
        #endregion

        #region Save Entity
        public override void SaveTransactionHeader(IDbContext ctx, ref int transactionID)
        {
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            if (hdnTransactionHdID.Value == "0")
            {
                PatientChargesHd entityHd = new PatientChargesHd();
                entityHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                entityHd.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                entityHd.TransactionCode = Constant.TransactionCode.LABORATORY_CHARGES;
                //entityHd.TransactionDate = Helper.GetDatePickerValue(txtTransactionDateLab.Text);
                entityHd.TransactionDate = Helper.GetDatePickerValue(GetTransactionDate());
                entityHd.TransactionTime = Request.Form[txtTransactionTime.UniqueID];
                entityHd.PatientBillingID = null;
                entityHd.ReferenceNo = txtReferenceNo.Text;
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                entityHd.GCVoidReason = null;
                entityHd.TransactionNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.TransactionDate, ctx);
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                transactionID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
            }
            else
            {
                transactionID = Convert.ToInt32(hdnTransactionHdID.Value);
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            try
            {
                PatientChargesHd entityHd = new PatientChargesHd();
                entityHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                entityHd.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                entityHd.TransactionCode = Constant.TransactionCode.LABORATORY_CHARGES;

                entityHd.TransactionDate = Helper.GetDatePickerValue(GetTransactionDate());
                entityHd.TransactionTime = Request.Form[txtTransactionTime.UniqueID];
                entityHd.PatientBillingID = null;
                entityHd.ReferenceNo = txtReferenceNo.Text;
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                entityHd.GCVoidReason = null;
                entityHd.TotalPatientAmount = 0;
                entityHd.TotalPayerAmount = 0;
                entityHd.TotalAmount = 0;
                entityHd.TransactionNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.TransactionDate, ctx);
                entityHd.CreatedBy = AppSession.UserLogin.UserID; 
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                retval = entityHdDao.InsertReturnPrimaryKeyID(entityHd).ToString();
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
            try
            {
                PatientChargesHd entity = BusinessLayer.GetPatientChargesHd(Convert.ToInt32(hdnTransactionHdID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entity.ReferenceNo = txtReferenceNo.Text;
                    entity.TransactionDate = Helper.GetDatePickerValue(GetTransactionDate());
                    entity.TransactionTime = Request.Form[txtTransactionTime.UniqueID];
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdatePatientChargesHd(entity);
                    return true;
                }
                else
                {
                    errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat diubah lagi.", entity.TransactionNo);
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    return false;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }
        #endregion

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            string[] param = type.Split(';');
            string gcDeleteReason = param[1];
            string reason = param[2];
            bool result = true;

            if (param[0] == "void")
            {
                IDbContext ctx = DbFactory.Configure(true);
                TestOrderHdDao orderHdDao = new TestOrderHdDao(ctx);
                TestOrderDtDao orderDtDao = new TestOrderDtDao(ctx);
                PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
                try
                {
                    Int32 TransactionID = Convert.ToInt32(hdnTransactionHdID.Value);
                    PatientChargesHd entity = entityHdDao.Get(TransactionID);
                    if (AppSession.IsAdminCanCancelAllTransaction)
                    {
                        if (entity.PatientBillingID == null && (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN || entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL))
                        {
                            ((PatientUseDetailServiceCtl)ctlService).OnVoidAllChargesDt(ctx, TransactionID);
                            ((PatientUseDetailDrugMSCtl)ctlDrugMS).OnVoidAllChargesDt(ctx, TransactionID);
                            ((PatientUseDetailDrugMSReturnCtl)ctlDrugMSReturn).OnVoidAllChargesDt(ctx, TransactionID);
                            ((PatientUseDetailLogisticCtl)ctlLogistic).OnVoidAllChargesDt(ctx, TransactionID);
                            ((PatientUseDetailLogisticReturnCtl)ctlLogisticReturn).OnVoidAllChargesDt(ctx, TransactionID);

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

                            //Update Status TestOrderHd
                            if (entity.TestOrderID != null)
                            {
                                TestOrderHd orderHd = orderHdDao.Get((int)entity.TestOrderID);
                                if (orderHd.GCTransactionStatus != Constant.TransactionStatus.VOID)
                                {
                                    orderHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                    orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    orderHdDao.Update(orderHd);

                                    List<TestOrderDt> lstTestOrderDt = BusinessLayer.GetTestOrderDtList(string.Format("TestOrderID = {0} AND IsDeleted = 0", entity.TestOrderID.ToString()), ctx);
                                    foreach (TestOrderDt orderDt in lstTestOrderDt)
                                    {
                                        if (orderDt != null && orderDt.GCTestOrderStatus != Constant.TestOrderStatus.CANCELLED)
                                        {
                                            if (!orderDt.IsDeleted)
                                            {
                                                string filterDt = String.Format("TransactionID = '{0}' AND ItemID = '{1}' AND IsDeleted = 0", entity.TransactionID, orderDt.ItemID);
                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();
                                                List<PatientChargesDt> lstChargesDt = BusinessLayer.GetPatientChargesDtList(filterDt, ctx);
                                                foreach (PatientChargesDt e in lstChargesDt)
                                                {
                                                    orderDt.ChargeAIOQty = orderDt.ChargeAIOQty - e.ChargedQuantity;
                                                }

                                                if (orderDt.ChargeAIOQty < orderDt.ItemQty)
                                                {
                                                    orderDt.GCTestOrderStatus = Constant.TestOrderStatus.OPEN;
                                                }
                                                orderDt.BusinessPartnerID = null;
                                                orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                orderDtDao.Update(orderDt);
                                            }
                                        }
                                    }
                                }
                            }
                            ctx.CommitTransaction();
                        }
                        else
                        {
                            result = false;
                            errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat dibatalkan lagi.", entity.TransactionNo);
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                    }
                    else
                    {
                        if (entity.PatientBillingID == null && entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                        {
                            ((PatientUseDetailServiceCtl)ctlService).OnVoidAllChargesDt(ctx, TransactionID);
                            ((PatientUseDetailDrugMSCtl)ctlDrugMS).OnVoidAllChargesDt(ctx, TransactionID);
                            ((PatientUseDetailLogisticCtl)ctlLogistic).OnVoidAllChargesDt(ctx, TransactionID);

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

                            //Update Status TestOrderHd
                            if (entity.TestOrderID != null)
                            {
                                TestOrderHd orderHd = orderHdDao.Get((int)entity.TestOrderID);
                                if (orderHd.GCTransactionStatus != Constant.TransactionStatus.VOID)
                                {
                                    orderHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                    orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    orderHdDao.Update(orderHd);

                                    List<TestOrderDt> lstTestOrderDt = BusinessLayer.GetTestOrderDtList(string.Format("TestOrderID = {0} AND IsDeleted = 0", entity.TestOrderID.ToString()), ctx);
                                    foreach (TestOrderDt orderDt in lstTestOrderDt)
                                    {
                                        if (orderDt != null && orderDt.GCTestOrderStatus != Constant.TestOrderStatus.CANCELLED)
                                        {
                                            if (!orderDt.IsDeleted)
                                            {
                                                string filterDt = String.Format("TransactionID = '{0}' AND ItemID = '{1}' AND IsDeleted = 0", entity.TransactionID, orderDt.ItemID);
                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();
                                                List<PatientChargesDt> lstChargesDt = BusinessLayer.GetPatientChargesDtList(filterDt, ctx);
                                                foreach (PatientChargesDt e in lstChargesDt)
                                                {
                                                    orderDt.ChargeAIOQty = orderDt.ChargeAIOQty - e.ChargedQuantity;
                                                }

                                                if (orderDt.ChargeAIOQty < orderDt.ItemQty)
                                                {
                                                    orderDt.GCTestOrderStatus = Constant.TestOrderStatus.OPEN;
                                                }
                                                orderDt.BusinessPartnerID = null;
                                                orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                orderDtDao.Update(orderDt);
                                            }
                                        }
                                    }
                                }
                            }
                            ctx.CommitTransaction();
                        }
                        else
                        {
                            result = false;
                            errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat dibatalkan lagi.", entity.TransactionNo);
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
            }
            return result;
        }

        #region Proposed Entity
        protected override bool OnProposeRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao chargesDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            TestOrderHdDao testOrderHdDao = new TestOrderHdDao(ctx);
            TestOrderDtDao testOrderDtDao = new TestOrderDtDao(ctx);
            try
            {
                if (hdnValidateTariffOnPropose.Value == "1")
                {
                    string filterExpression = String.Format("TransactionID = {0} AND Tariff = 0 AND IsDeleted = 0", hdnTransactionHdID.Value);
                    List<PatientChargesDt> lstDetail = BusinessLayer.GetPatientChargesDtList(filterExpression);
                    if (lstDetail.Count > 0)
                    {
                        errMessage = "Masih ada item yang belum memiliki tariff, proses tidak bisa dilanjutkan!";
                        result = false;
                    }
                }

                Int32 transactionID = Convert.ToInt32(hdnTransactionHdID.Value);
                PatientChargesHd entity = chargesDao.Get(transactionID);
                if (entity.PatientBillingID == null && entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    chargesDao.Update(entity);

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    string filterChargesDt = string.Format(string.Format("TransactionID = {0}  AND IsDeleted = 0 AND GCTransactionDetailStatus <> '{1}'", entity.TransactionID, Constant.TransactionStatus.VOID));
                    List<PatientChargesDt> lstPatientChargesDt = BusinessLayer.GetPatientChargesDtList(filterChargesDt, ctx); // AND LocationID IS NOT NULL AND IsApproved = 0
                    foreach (PatientChargesDt patientChargesDt in lstPatientChargesDt)
                    {
                        if ((patientChargesDt.LocationID != null && patientChargesDt.LocationID != 0) && !patientChargesDt.IsApproved)
                        {
                            patientChargesDt.IsApproved = true;
                        }
                        patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                        patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(patientChargesDt);

                        if (hdnTestOrderID.Value != "" && hdnTestOrderID.Value != "0")
                        {
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            string filterDt = String.Format("TestOrderID = '{0}' AND ItemID = '{1}' AND IsDeleted = 0", hdnTestOrderID.Value, patientChargesDt.ItemID);
                            TestOrderDt orderDt = BusinessLayer.GetTestOrderDtList(filterDt, ctx).FirstOrDefault();
                            //orderDt.ChargeAIOQty += patientChargesDt.ChargedQuantity;
                            //orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;

                            if (orderDt.ChargeAIOQty > orderDt.ItemQty)
                            {
                                errMessage = "Jumlah Qty Sudah Melebihi Qty Paket AIO Yang Di Perbolehkan. Harap Sesuaikan Qty.";
                                result = false;
                            }
                            else
                            {
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                testOrderDtDao.Update(orderDt);
                            }
                        }
                    }

                    if (AppSession.IsBridgingToQueue)
                    {
                        //If Bridging to Queue - Send Information
                        if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT || hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC || hdnDepartmentID.Value == Constant.Facility.LABORATORY)
                        {
                            try
                            {
                                OrderDTO oData = new OrderDTO();
                                vConsultVisit oVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", hdnVisitID.Value))[0];
                                oData = ConvertOrderToDTO(entity, oVisit);
                                QueueService oService = new QueueService();
                                string apiResult = oService.SendOrderInformation(oData);
                                string[] apiResultInfo = apiResult.Split('|');
                                if (apiResultInfo[0] == "0")
                                {
                                    Exception ex = new Exception(apiResultInfo[1]);
                                    Helper.InsertErrorLog(ex);
                                }
                            }
                            catch (Exception ex)
                            {
                                Helper.InsertErrorLog(ex);
                            }
                        }
                    }
                    ctx.CommitTransaction();
                }
                else
                {
                    errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat di-proposed lagi.", entity.TransactionNo);
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    result = false;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                result = false;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
        #endregion

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        private OrderDTO ConvertOrderToDTO(PatientChargesHd chargesHd, vConsultVisit oVisit)
        {
            OrderDTO oData = new OrderDTO();

            oData.RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
            oData.RegistrationNo = oVisit.RegistrationNo;
            oData.PatientID = AppSession.RegisteredPatient.MRN;
            oData.PatientInfo = new PatientData() { PatientID = oData.PatientID, MedicalNo = AppSession.RegisteredPatient.MedicalNo, FullName = AppSession.RegisteredPatient.PatientName };
            oData.VisitID = AppSession.RegisteredPatient.VisitID;
            if (oVisit != null)
            {
                oData.VisitInformation = new VisitInfo()
                {
                    VisitID = oVisit.VisitID,
                    VisitDate = oVisit.VisitDate.ToString(Constant.FormatString.DATE_FORMAT_112),
                    VisitTime = oVisit.VisitTime,
                    PhysicianID = oVisit.ParamedicID,
                    PhysicianCode = oVisit.ParamedicCode,
                    PhysicianName = oVisit.ParamedicName
                };
            }
            else
            {
                oData.VisitInformation = new VisitInfo()
                {
                    VisitID = AppSession.RegisteredPatient.VisitID,
                    VisitDate = AppSession.RegisteredPatient.VisitDate.ToString(Constant.FormatString.DATE_FORMAT_112)
                };
            }
            oData.TransactionNo = chargesHd.TransactionNo;
            oData.TransactionDate = chargesHd.TransactionDate.ToString(Constant.FormatString.DATE_FORMAT_112);
            oData.TransactionTime = chargesHd.TransactionTime;
            oData.OrderType = "LABORATORY";
            oData.IsCompound = false;

            List<OrderDt> lstOrderDt = new List<OrderDt>();
            List<vPatientChargesDt> lstChargesDt = BusinessLayer.GetvPatientChargesDtList(string.Format("TransactionID = {0} AND IsDeleted = 0", chargesHd.TransactionID));
            foreach (vPatientChargesDt item in lstChargesDt)
            {
                OrderDt orderDt = new OrderDt() { itemCode = item.ItemCode, itemName = item.ItemName1 };
                lstOrderDt.Add(orderDt);
            }
            oData.DetailList = lstOrderDt;
            return oData;
        }
    }
}
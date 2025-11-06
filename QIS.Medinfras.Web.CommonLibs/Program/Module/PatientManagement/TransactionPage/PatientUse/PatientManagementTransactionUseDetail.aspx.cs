using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.UI;
using Newtonsoft.Json;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.API.Model;
using QIS.Medinfras.Web.CommonLibs.Controls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientManagementTransactionUseDetail : BasePageTrxPatientManagement
    {
        public override string OnGetMenuCode()
        {
            string[] param = Page.Request.QueryString["id"].Split('|');
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.EMERGENCY:
                    return Constant.MenuCode.EmergencyCare.SERVICE_ORDER_TRANS;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.WORK_LIST;
                    else
                        return Constant.MenuCode.MedicalDiagnostic.WORK_LIST;
                default:
                    return Constant.MenuCode.Outpatient.SERVICE_ORDER_TRANS;
            }

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
            return txtRemarks.Text;
        }

        public override string GetReferenceNoHd()
        {
            return txtReferenceNo.Text;
        }

        public override bool GetIsCorrectionTransactionHd()
        {
            return false;
        }

        public override bool GetIsAIOTransactionHd()
        {
            return false;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = (hdnGCRegistrationStatus.Value != Constant.VisitStatus.CLOSED);
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
            return Request.Form[txtTransactionDate.UniqueID];
        }
        public override string GetTransactionTime()
        {
            return Request.Form[txtTransactionTime.UniqueID];
        }
        public override string GetTransactionDate2()
        {
            return txtTransactionDate.Text;
        }
        public override string GetTransactionTime2()
        {
            return txtTransactionTime.Text;
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString.Count > 0)
            {
                string[] param = Page.Request.QueryString["id"].Split('|');
                hdnParam.Value = param[0];
                string transactionNo = string.Empty;
                if (param[0] == "to")
                {
                    hdnVisitID.Value = param[2];
                    hdnDefaultTestOrderID.Value = hdnTestOrderID.Value = param[1];
                    TestOrderHd toHD = BusinessLayer.GetTestOrderHd(Convert.ToInt32(hdnTestOrderID.Value));
                    hdnOrderGCTransactionStatus.Value = toHD.GCTransactionStatus;
                    btnTransactionServiceOrder.Style.Add("display", "none");
                    PatientChargesHd entityHd = BusinessLayer.GetPatientChargesHdList(string.Format("TestOrderID = '{0}' AND GCTransactionStatus <> '{1}'", hdnTestOrderID.Value, Constant.TransactionStatus.VOID)).FirstOrDefault();
                    if (entityHd != null) transactionNo = entityHd.TransactionNo;
                }
                else if (param[0] == "soer")
                {
                    hdnVisitID.Value = param[2];
                    hdnServiceOrderID.Value = param[1];
                    btnClinicTransactionTestOrder.Style.Add("display", "none");
                    PatientChargesHd entityHd = BusinessLayer.GetPatientChargesHdList(string.Format("ServiceOrderID = '{0}' AND GCTransactionStatus <> '{1}'", hdnServiceOrderID.Value, Constant.TransactionStatus.VOID)).FirstOrDefault();
                    if (entityHd != null) transactionNo = entityHd.TransactionNo;
                    hdnDepartmentID.Value = Constant.Facility.EMERGENCY;

                }
                else if (param[0] == "soop")
                {
                    hdnVisitID.Value = param[2];
                    hdnServiceOrderID.Value = param[1];
                    PatientChargesHd entityHd = BusinessLayer.GetPatientChargesHdList(string.Format("ServiceOrderID = '{0}' AND GCTransactionStatus <> '{1}'", hdnServiceOrderID.Value, Constant.TransactionStatus.VOID)).FirstOrDefault();
                    if (entityHd != null) transactionNo = entityHd.TransactionNo;
                    btnClinicTransactionTestOrder.Style.Add("display", "none");
                    hdnDepartmentID.Value = Constant.Facility.OUTPATIENT;
                }
                else
                {
                    hdnParam.Value = "";
                    hdnVisitID.Value = param[0];
                    hdnTestOrderID.Value = "";
                    hdnServiceOrderID.Value = "";
                    btnClinicTransactionTestOrder.Style.Add("display", "none");
                    btnTransactionServiceOrder.Style.Add("display", "none");
                }
                vConsultVisit2 entity = BusinessLayer.GetvConsultVisit2List(string.Format("VisitID = {0}", hdnVisitID.Value))[0];

                ((PatientBannerCtl)ctlPatientBanner).InitializePatientBanner(entity);
                hdnGCRegistrationStatus.Value = entity.GCVisitStatus;
                hdnRegistrationID.Value = entity.RegistrationID.ToString();
                hdnRegistrationPhysicianID.Value = hdnPhysicianID.Value = entity.ParamedicID.ToString();
                hdnLinkedRegistrationID.Value = entity.LinkedRegistrationID.ToString();
                hdnPhysicianCode.Value = entity.ParamedicCode;
                hdnPhysicianName.Value = entity.ParamedicName;

                hdnBusinessPartnerID.Value = entity.BusinessPartnerID.ToString();
                hdnGCCustomerType.Value = entity.GCCustomerType;
                hdnClassID.Value = entity.ChargeClassID.ToString();

                if (param.Count() < 2)
                {
                    hdnLocationID.Value = entity.LocationID.ToString();
                    hdnLogisticLocationID.Value = entity.LogisticLocationID.ToString();
                    hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
                    hdnDepartmentID.Value = entity.DepartmentID;
                    IsLoadFirstRecord = true;
                }
                else
                {
                    if (hdnTestOrderID.Value != "")
                    {
                        TestOrderHd testOrderHd = BusinessLayer.GetTestOrderHd(Convert.ToInt32(hdnTestOrderID.Value));
                        hdnHealthcareServiceUnitID.Value = testOrderHd.HealthcareServiceUnitID.ToString();
                    }
                    else if (hdnServiceOrderID.Value != "")
                    {
                        ServiceOrderHd serviceOrderHd = BusinessLayer.GetServiceOrderHd(Convert.ToInt32(hdnServiceOrderID.Value));
                        hdnHealthcareServiceUnitID.Value = serviceOrderHd.HealthcareServiceUnitID.ToString();
                    }
                    else
                    {
                        hdnHealthcareServiceUnitID.Value = param[1];
                    }

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

                    vHealthcareServiceUnit hsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", hdnHealthcareServiceUnitID.Value))[0];
                    hdnDepartmentID.Value = hsu.DepartmentID;
                    hdnLocationID.Value = hsu.LocationID.ToString();
                    hdnLogisticLocationID.Value = hsu.LogisticLocationID.ToString();
                    string filterExpression = GetFilterExpression();
                    //IsLoadFirstRecord = (OnGetRowCount() > 0);
                    hdnTransactionHdID.Value = "0";
                    ((PatientUseDetailServiceCtl)ctlService).OnAddRecord();
                    ((PatientUseDetailDrugMSCtl)ctlDrugMS).OnAddRecord();
                    ((PatientUseDetailDrugMSReturnCtl)ctlDrugMSReturn).OnAddRecord();
                    ((PatientUseDetailLogisticCtl)ctlLogistic).OnAddRecord();
                    ((PatientUseDetailLogisticReturnCtl)ctlLogisticReturn).OnAddRecord();
                    if (!string.IsNullOrEmpty(transactionNo))
                    {
                        IsLoadFirstRecord = true;
                        pageIndexFirstLoad = BusinessLayer.GetvPatientChargesHdRowIndex(filterExpression, transactionNo, "TransactionID DESC");
                    }
                }

                int count = BusinessLayer.GetServiceUnitParamedicRowCount(string.Format("HealthcareServiceUnitID = {0}", hdnHealthcareServiceUnitID.Value));
                if (count > 0)
                    hdnIsHealthcareServiceUnitHasParamedic.Value = "1";
                else
                    hdnIsHealthcareServiceUnitHasParamedic.Value = "0";

                hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            }

            string moduleName = Helper.GetModuleName();
            string ModuleID = Helper.GetModuleID(moduleName);
            GetUserMenuAccess menu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault();
            string CRUDMode = menu.CRUDMode;
            hdnIsAllowVoid.Value = CRUDMode.Contains("X") ? "1" : "0";

            SettingParameterDt setvarDt = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IS_AUTOMATICALLY_SEND_TO_BRIDGING);
            hdnIsAutomaticallySendToRIS.Value = setvarDt.ParameterValue;
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
            SetControlEntrySetting(txtTransactionDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtTransactionTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(txtTestOrderInfo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtReferenceNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
        }

        public override void OnAddRecord()
        {
            string[] param = Page.Request.QueryString["id"].Split('|');
            if (param[0] == "to")
                hdnTestOrderID.Value = param[2];
            else if (param[0] == "so")
                hdnServiceOrderID.Value = param[2];

            hdnGCTransactionStatus.Value = Constant.TransactionStatus.OPEN;

            ((PatientUseDetailServiceCtl)ctlService).OnAddRecord();
            ((PatientUseDetailDrugMSCtl)ctlDrugMS).OnAddRecord();
            ((PatientUseDetailDrugMSReturnCtl)ctlDrugMSReturn).OnAddRecord();
            ((PatientUseDetailLogisticCtl)ctlLogistic).OnAddRecord();
            ((PatientUseDetailLogisticReturnCtl)ctlLogisticReturn).OnAddRecord();
        }

        #region Load Entity
        protected string GetFilterExpression()
        {
            string filterExpression = "";
            if (hdnLinkedRegistrationID.Value != "" && hdnLinkedRegistrationID.Value != "0")
                filterExpression = string.Format("(VisitID = {0} OR (RegistrationID = {1} AND IsChargesTransfered = 1))", hdnVisitID.Value, hdnLinkedRegistrationID.Value);
            else
                filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);
            filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", hdnHealthcareServiceUnitID.Value);
            return filterExpression;
        }

        public override int OnGetRowCount()
        {
            string filterExpression = GetFilterExpression();
            return BusinessLayer.GetvPatientChargesHdRowCount(filterExpression);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            vPatientChargesHd entity = BusinessLayer.GetvPatientChargesHd(filterExpression, PageIndex, " TransactionID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            PageIndex = BusinessLayer.GetvPatientChargesHdRowIndex(filterExpression, keyValue, "TransactionID DESC");
            vPatientChargesHd entity = BusinessLayer.GetvPatientChargesHd(filterExpression, PageIndex, "TransactionID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vPatientChargesHd entity, ref bool isShowWatermark, ref string watermarkText)
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
            txtTestOrderInfo.Text = entity.TestOrderInfo;
            txtReferenceNo.Text = entity.ReferenceNo;
            txtTransactionDate.Text = entity.TransactionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTransactionTime.Text = entity.TransactionTime;
            //hdnTotalPatient.Value = entity.TotalPatientAmount.ToString();
            //hdnTotalPayer.Value = entity.TotalPayerAmount.ToString();
            txtRemarks.Text = entity.Remarks;
            bool flagHaveCharges = false;
            if (entity != null) flagHaveCharges = true;
            ((PatientUseDetailServiceCtl)ctlService).InitializeTransactionControl(flagHaveCharges);
            ((PatientUseDetailDrugMSCtl)ctlDrugMS).InitializeTransactionControl(flagHaveCharges);
            ((PatientUseDetailDrugMSReturnCtl)ctlDrugMSReturn).InitializeTransactionControl(flagHaveCharges);
            ((PatientUseDetailLogisticCtl)ctlLogistic).InitializeTransactionControl(flagHaveCharges);
            ((PatientUseDetailLogisticReturnCtl)ctlLogisticReturn).InitializeTransactionControl(flagHaveCharges);

            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN && entity.GCTransactionStatus != Constant.TransactionStatus.VOID)
                hdnIsAllowPrint.Value = "1";
            else
                hdnIsAllowPrint.Value = "0";
        }
        #endregion

        #region Save Entity
        public override void SaveTransactionHeader(IDbContext ctx, ref int transactionID)
        {
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            ServiceOrderHdDao serviceOrderHdDao = new ServiceOrderHdDao(ctx);
            TestOrderHdDao testOrderHdDao = new TestOrderHdDao(ctx);

            if (hdnTransactionHdID.Value == "0")
            {
                PatientChargesHd entityHd = new PatientChargesHd();
                entityHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                entityHd.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                switch (hdnDepartmentID.Value)
                {
                    case Constant.Facility.INPATIENT: entityHd.TransactionCode = Constant.TransactionCode.IP_CHARGES; break;
                    case Constant.Facility.EMERGENCY: entityHd.TransactionCode = Constant.TransactionCode.ER_CHARGES; break;
                    case Constant.Facility.DIAGNOSTIC:
                        if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                            entityHd.TransactionCode = Constant.TransactionCode.IMAGING_CHARGES;
                        else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                            entityHd.TransactionCode = Constant.TransactionCode.LABORATORY_CHARGES;
                        else
                            entityHd.TransactionCode = Constant.TransactionCode.OTHER_DIAGNOSTIC_CHARGES; break;
                    default: entityHd.TransactionCode = Constant.TransactionCode.OP_CHARGES; break;
                }
                entityHd.TransactionDate = Helper.GetDatePickerValue(Request.Form[txtTransactionDate.UniqueID]);
                entityHd.TransactionTime = Request.Form[txtTransactionTime.UniqueID];
                entityHd.PatientBillingID = null;
                entityHd.ReferenceNo = txtReferenceNo.Text;
                entityHd.Remarks = txtRemarks.Text;
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                entityHd.GCVoidReason = null;
                entityHd.TransactionNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.TransactionDate, ctx);
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                transactionID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);

                if (hdnServiceOrderID.Value != "" && hdnServiceOrderID.Value != "0")
                {
                    ServiceOrderHd serviceOrderHd = serviceOrderHdDao.Get(Convert.ToInt32(hdnServiceOrderID.Value));
                    if (serviceOrderHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                    {
                        serviceOrderHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                        serviceOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        serviceOrderHdDao.Update(serviceOrderHd);
                    }
                }

                if (hdnTestOrderID.Value != "" && hdnTestOrderID.Value != "0")
                {
                    TestOrderHd testOrderHd = testOrderHdDao.Get(Convert.ToInt32(hdnTestOrderID.Value));
                    if (testOrderHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                    {
                        testOrderHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                        testOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        testOrderHdDao.Update(testOrderHd);
                    }
                }
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
            ServiceOrderHdDao serviceOrderHdDao = new ServiceOrderHdDao(ctx);
            TestOrderHdDao testOrderHdDao = new TestOrderHdDao(ctx);
            try
            {
                PatientChargesHd entityHd = new PatientChargesHd();
                entityHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                entityHd.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                switch (hdnDepartmentID.Value)
                {
                    case Constant.Facility.INPATIENT: entityHd.TransactionCode = Constant.TransactionCode.IP_CHARGES; break;
                    case Constant.Facility.EMERGENCY: entityHd.TransactionCode = Constant.TransactionCode.ER_CHARGES; break;
                    case Constant.Facility.DIAGNOSTIC:
                        if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                            entityHd.TransactionCode = Constant.TransactionCode.IMAGING_CHARGES;
                        else
                            entityHd.TransactionCode = Constant.TransactionCode.OTHER_DIAGNOSTIC_CHARGES; break;
                    default: entityHd.TransactionCode = Constant.TransactionCode.OP_CHARGES; break;
                }
                entityHd.TransactionDate = Helper.GetDatePickerValue(Request.Form[txtTransactionDate.UniqueID]);
                entityHd.TransactionTime = Request.Form[txtTransactionTime.UniqueID];
                entityHd.PatientBillingID = null;
                entityHd.ReferenceNo = txtReferenceNo.Text;
                entityHd.Remarks = txtRemarks.Text;
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

                if (hdnServiceOrderID.Value != "")
                {
                    ServiceOrderHd serviceOrderHd = serviceOrderHdDao.Get(Convert.ToInt32(hdnServiceOrderID.Value));
                    if (serviceOrderHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                    {
                        serviceOrderHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                        serviceOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        serviceOrderHdDao.Update(serviceOrderHd);
                    }
                }

                if (hdnTestOrderID.Value != "")
                {
                    TestOrderHd testOrderHd = testOrderHdDao.Get(Convert.ToInt32(hdnTestOrderID.Value));
                    if (testOrderHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                    {
                        testOrderHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                        testOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        testOrderHdDao.Update(testOrderHd);
                    }
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

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            try
            {
                PatientChargesHd entity = BusinessLayer.GetPatientChargesHd(Convert.ToInt32(hdnTransactionHdID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entity.ReferenceNo = txtReferenceNo.Text;
                    entity.Remarks = txtRemarks.Text;
                    entity.TransactionDate = Helper.GetDatePickerValue(Request.Form[txtTransactionDate.UniqueID]);
                    entity.TransactionTime = Request.Form[txtTransactionTime.UniqueID];
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entity);
                }
                else
                {
                    errMessage = "Transaksi " + entity.TransactionNo + " Sudah Diproses. Tidak Bisa Diubah";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
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

        //#region Void Entity
        //protected override bool OnVoidRecord(ref string errMessage)
        //{
        //    bool result = true;
        //    IDbContext ctx = DbFactory.Configure(true);
        //    PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
        //    try
        //    {
        //        Int32 TransactionID = Convert.ToInt32(hdnTransactionHdID.Value);
        //        PatientChargesHd entity = entityHdDao.Get(TransactionID);
        //        if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN || entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
        //        {
        //            entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
        //            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
        //            entityHdDao.Update(entity);

        //            ((PatientManagementTransactionDetailServiceCtl)ctlService).OnVoidAllChargesDt(ctx, TransactionID);
        //            ((PatientManagementTransactionDetailDrugMSCtl)ctlDrugMS).OnVoidAllChargesDt(ctx, TransactionID);
        //            ((PatientManagementTransactionDetailDrugMSReturnCtl)ctlDrugMSReturn).OnVoidAllChargesDt(ctx, TransactionID);
        //            ((PatientManagementTransactionDetailLogisticCtl)ctlLogistic).OnVoidAllChargesDt(ctx, TransactionID);
        //        }
        //        else
        //        {
        //            errMessage = "Transaksi Sudah Diproses. Tidak Bisa dibatalkan";
        //            result = false;
        //        }
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
        //    return result;
        //}
        //#endregion

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            string[] param = type.Split(';');
            string gcDeleteReason = param[1];
            string reason = param[2];
            bool result = true;

            if (param[0] == "void")
            {
                IDbContext ctx = DbFactory.Configure(true);
                PatientChargesHdDao chargesDao = new PatientChargesHdDao(ctx);
                PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
                TestOrderHdDao orderHdDao = new TestOrderHdDao(ctx);
                TestOrderDtDao orderDtDao = new TestOrderDtDao(ctx);
                ServiceOrderHdDao serviceHdDao = new ServiceOrderHdDao(ctx);
                ServiceOrderDtDao serviceDtDao = new ServiceOrderDtDao(ctx);
                //ChargesStatusLogDao logDao = new ChargesStatusLogDao(ctx);
                try
                {
                    //ChargesStatusLog log = new ChargesStatusLog();
                    //string statusOld = "", statusNew = "";
                    Int32 TransactionID = Convert.ToInt32(hdnTransactionHdID.Value);
                    PatientChargesHd entity = chargesDao.Get(TransactionID);
                    //statusOld = entity.GCTransactionStatus;
                    if (entity.PatientBillingID == null && entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        ((PatientUseDetailServiceCtl)ctlService).OnVoidAllChargesDt(ctx, TransactionID);
                        ((PatientUseDetailDrugMSCtl)ctlDrugMS).OnVoidAllChargesDt(ctx, TransactionID);
                        ((PatientUseDetailDrugMSReturnCtl)ctlDrugMSReturn).OnVoidAllChargesDt(ctx, TransactionID);
                        ((PatientUseDetailLogisticCtl)ctlLogistic).OnVoidAllChargesDt(ctx, TransactionID);
                        ((PatientUseDetailLogisticReturnCtl)ctlLogisticReturn).OnVoidAllChargesDt(ctx, TransactionID);

                        entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                        entity.GCVoidReason = gcDeleteReason;
                        if (gcDeleteReason == Constant.DeleteReason.OTHER)
                        {
                            entity.VoidReason = reason;
                        }
                        entity.VoidBy = AppSession.UserLogin.UserID;
                        entity.VoidDate = DateTime.Now;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        chargesDao.Update(entity);

                        List<PatientChargesDt> lstPatientChargesDt = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID = {0} AND IsDeleted = 0 AND GCTransactionDetailStatus <> '{1}'", entity.TransactionID, Constant.TransactionStatus.VOID), ctx);
                        foreach (PatientChargesDt patientChargesDt in lstPatientChargesDt)
                        {
                            //PatientChargesDt entity = entityDtDao.Get(patientChargesDt.ID);
                            patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                            patientChargesDt.IsApproved = false;
                            patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(patientChargesDt);
                        }

                        //statusNew = entity.GCTransactionStatus;

                        //log.VisitID = entity.VisitID;
                        //log.TransactionID = entity.TransactionID;
                        //log.LogDate = DateTime.Now;
                        //log.UserID = AppSession.UserLogin.UserID;
                        //log.GCTransactionStatusOLD = statusOld;
                        //log.GCTransactionStatusNEW = statusNew;
                        //logDao.Insert(log);

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
                                            orderDt.GCTestOrderStatus = Constant.TestOrderStatus.OPEN;
                                            orderDt.BusinessPartnerID = null;
                                            orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            orderDtDao.Update(orderDt);
                                        }
                                    }
                                }
                            }
                        }

                        //Update Status ServiceOrder
                        if (entity.ServiceOrderID != null)
                        {
                            ServiceOrderHd orderHd = serviceHdDao.Get((int)entity.ServiceOrderID);
                            if (orderHd.GCTransactionStatus != Constant.TransactionStatus.VOID)
                            {
                                orderHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                serviceHdDao.Update(orderHd);

                                List<ServiceOrderDt> lstDt = BusinessLayer.GetServiceOrderDtList(string.Format("ServiceOrderID = {0} AND IsDeleted = 0", entity.ServiceOrderID.ToString()), ctx);
                                foreach (ServiceOrderDt orderDt in lstDt)
                                {
                                    if (orderDt != null && orderDt.GCServiceOrderStatus != Constant.TestOrderStatus.CANCELLED)
                                    {
                                        if (!orderDt.IsDeleted)
                                        {
                                            orderDt.GCServiceOrderStatus = Constant.TestOrderStatus.OPEN;
                                            orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            serviceDtDao.Update(orderDt);
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
                        errMessage = "Transaksi " + entity.TransactionNo + " Sudah Diproses. Tidak Bisa Diubah";
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

        #region Proposed Entity
        protected override bool OnProposeRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao chargesDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            //ChargesStatusLogDao logDao = new ChargesStatusLogDao(ctx);
            try
            {
                //ChargesStatusLog log = new ChargesStatusLog();
                //string statusOld = "", statusNew = "";
                PatientChargesHd entity = chargesDao.Get(Convert.ToInt32(hdnTransactionHdID.Value));
                //statusOld = entity.GCTransactionStatus;
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    if (hdnTestOrderID.Value != "" && hdnTestOrderID.Value != "0")
                    {
                        TestOrderHd testOrderHd = BusinessLayer.GetTestOrderHd(Convert.ToInt32(hdnTestOrderID.Value));
                        if (testOrderHd.GCTransactionStatus != Constant.TransactionStatus.PROCESSED && testOrderHd.GCTransactionStatus != Constant.TransactionStatus.VOID)
                        {
                            errMessage = "Masih Ada Test Order Yang Belum Diproses. Silakan approve / decline terlebih dahulu";
                            result = false;
                        }
                    }

                    if (hdnServiceOrderID.Value != "" && hdnServiceOrderID.Value != "0")
                    {
                        ServiceOrderHd serviceOrderHd = BusinessLayer.GetServiceOrderHd(Convert.ToInt32(hdnServiceOrderID.Value));
                        if (serviceOrderHd.GCTransactionStatus != Constant.TransactionStatus.PROCESSED && serviceOrderHd.GCTransactionStatus != Constant.TransactionStatus.VOID)
                        {
                            errMessage = "Masih Ada Service Order Yang Belum Diproses. Silakan approve / decline terlebih dahulu";
                            result = false;
                        }
                    }

                    string filterExp = string.Format("TransactionID = '{0}' AND GCItemType IN ('{1}','{2}','{3}') AND IsDeleted = 0 AND IsApproved = 0", hdnTransactionHdID.Value, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, Constant.ItemType.BARANG_UMUM);
                    List<vPatientChargesDt> lstOpenItemCharges = BusinessLayer.GetvPatientChargesDtList(filterExp);
                    if (lstOpenItemCharges.Count > 0)
                    {
                        errMessage = "Masih ada transaksi penggunaaan obat, alkes dan barang umum yang belum diposting. Silakan posting terlebih dahulu";
                        result = false;
                    }

                    bool isProcessCharges = true;
                    string referenceNo = string.Empty;
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                    {
                        if (AppSession.IsBridgingToRIS)
                        {
                            if (hdnIsAutomaticallySendToRIS.Value == "1")
                            {
                                if (AppSession.BRIDGING_TOOLS == "1")
                                {
                                    string[] resultInfo;

                                    switch (AppSession.RIS_BRIDGING_PROTOCOL)
                                    {
                                        case Constant.RIS_Bridging_Protocol.WEB_API:
                                            var result1 = SendOrderToRIS(Convert.ToInt32(hdnTestOrderID.Value), Convert.ToInt32(hdnTransactionHdID.Value));
                                            resultInfo = ((string)result1).Split('|');
                                            break;
                                        case Constant.RIS_Bridging_Protocol.HL7:
                                            var result2 = SendHL7OrderToRIS(Convert.ToInt32(hdnTestOrderID.Value), Convert.ToInt32(hdnTransactionHdID.Value));
                                            resultInfo = ((string)result2).Split('|');
                                            break;
                                        case Constant.RIS_Bridging_Protocol.LINK_DB:
                                            var result3 = SendOrderToRISLinkDB(Convert.ToInt32(hdnTestOrderID.Value), Convert.ToInt32(hdnTransactionHdID.Value));
                                            resultInfo = ((string)result3).Split('|');
                                            break;
                                        default:
                                            resultInfo = "0|Unknown Protocol".Split('|');
                                            break;
                                    }

                                    isProcessCharges = resultInfo[0] == "1";
                                    referenceNo = resultInfo[1];
                                    if (!isProcessCharges)
                                        errMessage = resultInfo[1];
                                }
                            }
                        }
                    }

                    if (result)
                    {
                        if (isProcessCharges)
                        {
                            if (!string.IsNullOrEmpty(referenceNo))
                                entity.ReferenceNo = referenceNo;
                            entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                            entity.ProposedBy = AppSession.UserLogin.UserID;
                            entity.ProposedDate = DateTime.Now;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            chargesDao.Update(entity);

                            //statusNew = entity.GCTransactionStatus;

                            //log.VisitID = entity.VisitID;
                            //log.TransactionID = entity.TransactionID;
                            //log.LogDate = DateTime.Now;
                            //log.UserID = AppSession.UserLogin.UserID;
                            //log.GCTransactionStatusOLD = statusOld;
                            //log.GCTransactionStatusNEW = statusNew;
                            //logDao.Insert(log);

                            List<PatientChargesDt> lstPatientChargesDt = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID = {0}  AND IsDeleted = 0 AND GCTransactionDetailStatus <> '{1}'", entity.TransactionID, Constant.TransactionStatus.VOID), ctx); // AND LocationID IS NOT NULL AND IsApproved = 0
                            foreach (PatientChargesDt patientChargesDt in lstPatientChargesDt)
                            {
                                if ((patientChargesDt.LocationID != null && patientChargesDt.LocationID != 0) && !patientChargesDt.IsApproved)
                                {
                                    patientChargesDt.IsApproved = true;
                                }
                                patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Update(patientChargesDt);
                            }
                        }
                        else
                        {
                            result = false;
                        }
                    }
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Transaksi " + entity.TransactionNo + " Sudah Diproses. Tidak Bisa Diubah";
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

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        #region RIS - Imaging Services
        public object SendOrderToRIS(int testOrderID, int transactionID)
        {
            string result = "";
            try
            {
                string url = AppSession.RIS_WEB_API_URL;
                #region Convert into DTO Objects
                bool isfromOrder = testOrderID > 0;

                string filterExpression = string.Format("TransactionID = {0}", transactionID);
                vPatientChargesHd oHeader = BusinessLayer.GetvPatientChargesHdList(filterExpression).FirstOrDefault();
                if (oHeader != null)
                {
                    vConsultVisit oVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", oHeader.VisitID)).FirstOrDefault();
                    List<vPatientChargesDt> oList = BusinessLayer.GetvPatientChargesDtList(filterExpression);
                    TestOrderDTO oData = new TestOrderDTO();
                    if (oList.Count > 0)
                    {
                        string orderPriority = "NORMAL";
                        string orderParamedicCode = oVisit.ParamedicCode;
                        string orderParamedicName = oVisit.ParamedicName;
                        DateTime orderDate = DateTime.Now.Date;
                        string orderTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                        if (testOrderID > 0)
                        {
                            vTestOrderHd oOrderHd = BusinessLayer.GetvTestOrderHdList(string.Format("TestOrderID = {0}", testOrderID)).FirstOrDefault();
                            orderPriority = oOrderHd != null ? (oOrderHd.IsCITO ? "HIGH" : "NORMAL") : "NORMAL";
                            orderParamedicCode = oOrderHd != null ? oOrderHd.ParamedicCode : "";
                            orderParamedicName = oOrderHd != null ? oOrderHd.ParamedicName : "";
                            orderDate = oOrderHd.TestOrderDate;
                            orderTime = oOrderHd.TestOrderTime;
                        }

                        oData.placerOrderNumber = oHeader.TransactionNo;
                        oData.visitNumber = oVisit.RegistrationNo;
                        oData.pointOfCare = oHeader.ServiceUnitName;
                        oData.room = oVisit.RoomName;
                        oData.bed = oVisit.BedCode;
                        oData.orderDateTime = string.Format("{0} {1}:00", orderDate.ToString("yyyy-MM-dd"), orderTime);
                        oData.imagingOrderPriority = orderPriority;
                        oData.reportingPriority = orderPriority;

                        List<TestOrderDtDTO> lstDetail = new List<TestOrderDtDTO>();

                        foreach (vPatientChargesDt item in oList)
                        {
                            TestOrderDtDTO oDetail = new TestOrderDtDTO();
                            string modality = String.IsNullOrEmpty(item.GCModality) ? "CT" : item.GCModality.Substring(5);
                            procedure oProcedure = new procedure() { procedureCode = item.ItemCode, procedureName = item.ItemName1, modalityCode = modality, procedureFee = 0 };
                            readingPhysician oPhysician = new readingPhysician() { radStaffCode = item.ParamedicCode, radStaffName = item.ParamedicName };
                            List<readingPhysician> lst = new List<readingPhysician>();
                            lst.Add(oPhysician);

                            oDetail.procedure = oProcedure;
                            oDetail.readingPhysician = lst;
                            lstDetail.Add(oDetail);
                        }
                        oData.orderDetail = lstDetail;

                        patient oPatient = new patient();

                        oPatient.patientID = oVisit.MRN.ToString();
                        oPatient.mrn = oVisit.MedicalNo;
                        oPatient.patientName = oVisit.PatientName;
                        oPatient.sex = oVisit.GCGender.Substring(5);
                        oPatient.address = oVisit.HomeAddress;
                        oPatient.dateOfBirth = oVisit.DateOfBirth.ToString("yyyy-MM-dd");
                        oPatient.size = "0";
                        oPatient.weight = "0";
                        oPatient.maritalStatus = string.IsNullOrEmpty(oVisit.GCMaritalStatus) ? "U" : oVisit.GCMaritalStatus.Substring(5);

                        oData.patient = oPatient;

                        List<referringPhysician> lstReferringPhysician = new List<referringPhysician>();

                        if (testOrderID > 0)
                        {
                            lstReferringPhysician.Add(new referringPhysician() { refPhyCode = orderParamedicCode, refPhyName = orderParamedicName });
                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(oVisit.ReferralPhysicianCode))
                                lstReferringPhysician.Add(new referringPhysician() { refPhyCode = oVisit.ReferralPhysicianCode, refPhyName = oVisit.ReferralPhysicianName });
                            else
                                lstReferringPhysician.Add(new referringPhysician() { refPhyCode = oVisit.ParamedicCode, refPhyName = oVisit.ParamedicName });
                        }

                        oData.referringPhysician = lstReferringPhysician;

                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/inputOrder/", url));
                        request.Method = "POST";
                        request.ContentType = "application/json";
                        Methods.SetRequestHeader(request);

                        var json = JsonConvert.SerializeObject(oData);
                        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                        {
                            streamWriter.Write(json);
                        }

                        WebResponse response = (WebResponse)request.GetResponse();
                        string responseMsg = string.Empty;
                        using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                        {
                            responseMsg = sr.ReadToEnd();
                        };

                        APIResponse respInfo = JsonConvert.DeserializeObject<APIResponse>(responseMsg);

                        if (!string.IsNullOrEmpty(respInfo.Data))
                        {
                            result = string.Format("{0}|{1}", "1", respInfo.Data);
                        }
                        else
                        {
                            result = string.Format("{0}|{1}", "0", respInfo.Remark);
                        }
                    }
                    else
                    {
                        result = string.Format("{0}|{1}", "0", "There is no order to be sent to RIS");
                    }
                #endregion
                }
                return result;
            }
            catch (WebException ex)
            {
                //switch (ex.Status)
                //{
                //    case WebExceptionStatus.ProtocolError:
                //        result = string.Format("{0}|{1}", "0", "Method not found");
                //        break;
                //    default:
                //        result = string.Format("{0}|{1}", "0", string.Format("{0} ({1})", ex.Status.ToString()));
                //        break;
                //}
                result = string.Format("{0}|{1}", "0", string.Format("{0} ({1})", ex.Status.ToString()));
                return result;
            }
        }

        public object SendHL7OrderToRIS(int testOrderID, int transactionID)
        {
            string result = "";
            try
            {
                string url = AppSession.RIS_WEB_API_URL;
                #region Convert into DTO Objects
                bool isfromOrder = testOrderID > 0;

                string filterExpression = string.Format("TransactionID = {0}", transactionID);
                vPatientChargesHd oHeader = BusinessLayer.GetvPatientChargesHdList(filterExpression).FirstOrDefault();
                if (oHeader != null)
                {
                    vConsultVisit oVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", oHeader.VisitID)).FirstOrDefault();
                    List<vPatientChargesDt> oList = BusinessLayer.GetvPatientChargesDtList(filterExpression);
                    ImagingOrderDTO oData = new ImagingOrderDTO();
                    if (oList.Count > 0)
                    {
                        string healthcareID = AppSession.UserLogin.HealthcareID;
                        string orderNo = string.Empty;
                        string orderPriority = "NORMAL";
                        string orderParamedicCode = oVisit.ParamedicCode.Trim();
                        string orderParamedicName = oVisit.ParamedicName.Trim();
                        DateTime orderDate = DateTime.Now.Date;
                        string orderTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                        if (testOrderID > 0)
                        {
                            vTestOrderHd oOrderHd = BusinessLayer.GetvTestOrderHdList(string.Format("TestOrderID = {0}", testOrderID)).FirstOrDefault();
                            orderNo = oOrderHd.TestOrderNo;
                            orderPriority = oOrderHd != null ? (oOrderHd.IsCITO ? "HIGH" : "NORMAL") : "NORMAL";
                            orderParamedicCode = oOrderHd != null ? oOrderHd.ParamedicCode : "";
                            orderParamedicName = oOrderHd != null ? oOrderHd.ParamedicName : "";
                            orderDate = oOrderHd.TestOrderDate;
                            orderTime = oOrderHd.TestOrderTime;
                        }

                        oData.HealthcareID = healthcareID.Trim();
                        oData.TestOrderID = transactionID;
                        oData.TestOrderNo = oHeader.TransactionNo.Trim();
                        oData.TestOrderDate = oHeader.TransactionDate.ToString(Constant.FormatString.DATE_FORMAT_112).Trim();
                        oData.TestOrderTime = oHeader.TransactionTime.Trim();
                        if (isfromOrder)
                        {
                            oData.PhysicianCode = orderParamedicCode.Trim();
                            oData.PhysicianName = orderParamedicName.Trim();
                        }
                        else
                        {
                            oData.PhysicianCode = AppSession.RegisteredPatient.ParamedicCode.Trim();
                            oData.PhysicianName = AppSession.RegisteredPatient.ParamedicName.Trim();
                        }

                        oData.PatientID = oVisit.MRN;

                        PatientInfo oPatientInfo = new PatientInfo();

                        oPatientInfo.PatientID = oVisit.MRN;
                        oPatientInfo.MedicalNo = oVisit.MedicalNo.Trim();
                        oPatientInfo.FirstName = oVisit.FirstName.Trim();
                        oPatientInfo.MiddleName = oVisit.MiddleName.Trim();
                        oPatientInfo.LastName = oVisit.LastName.Trim();
                        oPatientInfo.PrefferedName = oVisit.PreferredName.Trim();
                        oPatientInfo.Gender = oVisit.Gender.Trim();
                        oPatientInfo.Religion = oVisit.Religion.Trim();
                        oPatientInfo.MaritalStatus = oVisit.MaritalStatus.Trim();
                        oPatientInfo.DateOfBirth = oVisit.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT_112).Trim();
                        oPatientInfo.CityOfBirth = oVisit.CityOfBirth.Trim();
                        oPatientInfo.HomeAddress = oVisit.HomeAddress.Trim();
                        oPatientInfo.HomeZipCode = oVisit.ZipCode.Trim();
                        oPatientInfo.HomePhoneNo1 = oVisit.PhoneNo1.Trim();
                        oPatientInfo.HomePhoneNo2 = oVisit.PhoneNo2.Trim();
                        oPatientInfo.MobileNo1 = oVisit.MobilePhoneNo1.Trim();
                        oPatientInfo.MobileNo2 = oVisit.MobilePhoneNo2.Trim();

                        oData.PatientInfo = oPatientInfo;

                        oData.VisitID = oVisit.VisitID;
                        oData.RegistrationID = oVisit.RegistrationID;
                        oData.RegistrationNo = oVisit.RegistrationNo.Trim();

                        CompactVisitInfo oVisitInfo = new CompactVisitInfo();
                        oVisit.VisitDate = oVisit.VisitDate;
                        oVisit.VisitTime = oVisit.VisitTime;
                        oVisitInfo.DepartmentID = oVisit.DepartmentID.Trim();
                        oVisitInfo.RegistrationNo = oVisit.RegistrationNo.Trim();
                        oVisitInfo.ServiceUnitName = oVisit.ServiceUnitName.Trim();

                        oData.VisitInformation = oVisitInfo;

                        List<TestOrderDetailInfo> lstDetail = new List<TestOrderDetailInfo>();

                        foreach (vPatientChargesDt item in oList)
                        {
                            TestOrderDetailInfo oDetail = new TestOrderDetailInfo();
                            string modality = String.IsNullOrEmpty(item.GCModality) ? "CT" : item.GCModality.Substring(5);

                            oDetail.ItemCode = item.ItemCode.Trim();
                            oDetail.ItemName = item.ItemName1.Trim();
                            oDetail.IsCITO = item.IsCITO;
                            oDetail.ModalityType = modality.Trim();
                            oDetail.DiagnoseName = item.DiagnoseTestOrder.Trim();
                            oDetail.Remarks = string.Empty;

                            lstDetail.Add(oDetail);
                        }

                        oData.OrderItemList = lstDetail;

                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/imaging/order/sendOrderHL7/", url));
                        request.Method = "POST";
                        request.ContentType = "application/json";
                        Methods.SetRequestHeader(request);

                        var json = JsonConvert.SerializeObject(oData);
                        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                        {
                            streamWriter.Write(json);
                        }

                        WebResponse response = (WebResponse)request.GetResponse();
                        string responseMsg = string.Empty;
                        using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                        {
                            responseMsg = sr.ReadToEnd();
                        };

                        MedinfrasAPIResponse respInfo = JsonConvert.DeserializeObject<MedinfrasAPIResponse>(responseMsg);

                        if (!string.IsNullOrEmpty(respInfo.Data))
                        {
                            result = string.Format("{0}|{1}", "1", respInfo.Data);
                        }
                        else
                        {
                            result = string.Format("{0}|{1}", "0", respInfo.Remarks);
                        }
                    }
                    else
                    {
                        result = string.Format("{0}|{1}", "0", "There is no order to be sent to RIS");
                    }
                #endregion
                }
                return result;
            }
            catch (WebException ex)
            {
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        result = string.Format("{0}|{1}", "0", "Method not found");
                        break;
                    default:
                        result = string.Format("{0}|{1}", "0", string.Format("{0} ({1})", ex.Status.ToString()));
                        break;
                }
                return result;
            }
        }

        public object SendOrderToRISLinkDB(int testOrderID, int transactionID)
        {
            string result = "";
            try
            {
                string url = AppSession.RIS_WEB_API_URL;
                #region Convert into DTO Objects
                bool isfromOrder = testOrderID > 0;

                string filterExpression = string.Format("TransactionID = {0}", transactionID);
                vPatientChargesHd oHeader = BusinessLayer.GetvPatientChargesHdList(filterExpression).FirstOrDefault();
                if (oHeader != null)
                {
                    vConsultVisit oVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", oHeader.VisitID)).FirstOrDefault();
                    List<vPatientChargesDt> oList = BusinessLayer.GetvPatientChargesDtList(filterExpression);
                    ImagingOrderDTO oData = new ImagingOrderDTO();
                    if (oList.Count > 0)
                    {
                        string healthcareID = AppSession.UserLogin.HealthcareID;
                        string orderNo = string.Empty;
                        string orderPriority = "NORMAL";
                        string orderParamedicCode = oVisit.ParamedicCode.Trim();
                        string orderParamedicName = oVisit.ParamedicName.Trim();
                        DateTime orderDate = DateTime.Now.Date;
                        string orderTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                        if (testOrderID > 0)
                        {
                            vTestOrderHd oOrderHd = BusinessLayer.GetvTestOrderHdList(string.Format("TestOrderID = {0}", testOrderID)).FirstOrDefault();
                            orderNo = oOrderHd.TestOrderNo;
                            orderPriority = oOrderHd != null ? (oOrderHd.IsCITO ? "HIGH" : "NORMAL") : "NORMAL";
                            orderParamedicCode = oOrderHd != null ? oOrderHd.ParamedicCode : "";
                            orderParamedicName = oOrderHd != null ? oOrderHd.ParamedicName : "";
                            orderDate = oOrderHd.TestOrderDate;
                            orderTime = oOrderHd.TestOrderTime;
                        }

                        oData.HealthcareID = healthcareID.Trim();
                        oData.TestOrderID = transactionID;
                        oData.TestOrderNo = oHeader.TransactionNo.Trim();
                        oData.TestOrderDate = oHeader.TransactionDate.ToString(Constant.FormatString.DATE_FORMAT_112).Trim();
                        oData.TestOrderTime = oHeader.TransactionTime.Trim();
                        if (isfromOrder)
                        {
                            oData.PhysicianCode = orderParamedicCode.Trim();
                            oData.PhysicianName = orderParamedicName.Trim();
                        }
                        else
                        {
                            oData.PhysicianCode = AppSession.RegisteredPatient.ParamedicCode.Trim();
                            oData.PhysicianName = AppSession.RegisteredPatient.ParamedicName.Trim();
                        }

                        oData.PatientID = oVisit.MRN;

                        PatientInfo oPatientInfo = new PatientInfo();

                        oPatientInfo.PatientID = oVisit.MRN;
                        oPatientInfo.MedicalNo = oVisit.MedicalNo.Trim();
                        oPatientInfo.FirstName = oVisit.FirstName.Trim();
                        oPatientInfo.MiddleName = oVisit.MiddleName.Trim();
                        oPatientInfo.LastName = oVisit.LastName.Trim();
                        oPatientInfo.PrefferedName = oVisit.PreferredName.Trim();
                        oPatientInfo.Gender = oVisit.Gender.Trim();
                        oPatientInfo.Religion = oVisit.Religion.Trim();
                        oPatientInfo.MaritalStatus = oVisit.MaritalStatus.Trim();
                        oPatientInfo.DateOfBirth = oVisit.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT_112).Trim();
                        oPatientInfo.CityOfBirth = oVisit.CityOfBirth.Trim();
                        oPatientInfo.HomeAddress = oVisit.HomeAddress.Trim();
                        oPatientInfo.HomeZipCode = oVisit.ZipCode.Trim();
                        oPatientInfo.HomePhoneNo1 = oVisit.PhoneNo1.Trim();
                        oPatientInfo.HomePhoneNo2 = oVisit.PhoneNo2.Trim();
                        oPatientInfo.MobileNo1 = oVisit.MobilePhoneNo1.Trim();
                        oPatientInfo.MobileNo2 = oVisit.MobilePhoneNo2.Trim();

                        oData.PatientInfo = oPatientInfo;

                        oData.VisitID = oVisit.VisitID;
                        oData.RegistrationID = oVisit.RegistrationID;
                        oData.RegistrationNo = oVisit.RegistrationNo.Trim();

                        CompactVisitInfo oVisitInfo = new CompactVisitInfo();
                        oVisit.VisitDate = oVisit.VisitDate;
                        oVisit.VisitTime = oVisit.VisitTime;
                        oVisitInfo.DepartmentID = oVisit.DepartmentID.Trim();
                        oVisitInfo.RegistrationNo = oVisit.RegistrationNo.Trim();
                        oVisitInfo.ServiceUnitName = oVisit.ServiceUnitName.Trim();

                        oData.VisitInformation = oVisitInfo;

                        List<TestOrderDetailInfo> lstDetail = new List<TestOrderDetailInfo>();

                        foreach (vPatientChargesDt item in oList)
                        {
                            TestOrderDetailInfo oDetail = new TestOrderDetailInfo();
                            string modality = String.IsNullOrEmpty(item.GCModality) ? "CT" : item.GCModality.Substring(5);

                            oDetail.ItemCode = item.ItemCode.Trim();
                            oDetail.ItemName = item.ItemName1.Trim();
                            oDetail.RequestedPhysicianName = item.ParamedicName;
                            oDetail.IsCITO = item.IsCITO;
                            oDetail.ModalityType = modality.Trim();
                            oDetail.ModalityCode = item.ModalityCode;
                            oDetail.ModalityAETitle = item.ModalityAETitle;
                            oDetail.Remarks = string.Empty;

                            lstDetail.Add(oDetail);
                        }

                        oData.OrderItemList = lstDetail;

                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/imaging/order/sendOrderToRISLinkDB/", url));
                        request.Method = "POST";
                        request.ContentType = "application/json";
                        Methods.SetRequestHeader(request);

                        var json = JsonConvert.SerializeObject(oData);
                        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                        {
                            streamWriter.Write(json);
                        }

                        WebResponse response = (WebResponse)request.GetResponse();
                        string responseMsg = string.Empty;
                        using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                        {
                            responseMsg = sr.ReadToEnd();
                        };

                        MedinfrasAPIResponse respInfo = JsonConvert.DeserializeObject<MedinfrasAPIResponse>(responseMsg);

                        if (!string.IsNullOrEmpty(respInfo.Data))
                        {
                            result = string.Format("{0}|{1}", "1", respInfo.Data);
                        }
                        else
                        {
                            result = string.Format("{0}|{1}", "0", respInfo.Remarks);
                        }
                    }
                    else
                    {
                        result = string.Format("{0}|{1}", "0", "There is no order to be sent to RIS");
                    }
                #endregion
                }
                return result;
            }
            catch (WebException ex)
            {
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        result = string.Format("{0}|{1}", "0", "Method not found");
                        break;
                    default:
                        result = string.Format("{0}|{1}", "0", string.Format("{0} ({1})", ex.Status.ToString()));
                        break;
                }
                return result;
            }
        }
        #endregion
    }
}
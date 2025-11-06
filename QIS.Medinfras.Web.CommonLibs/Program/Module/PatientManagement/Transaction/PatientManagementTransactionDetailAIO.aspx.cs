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
using QIS.Medinfras.Common;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.API.Model;
using QIS.Medinfras.Web.CommonLibs.Controls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientManagementTransactionDetailAIO : BasePageTrxPatientManagement
    {
        public override string OnGetMenuCode()
        {
            string[] param = Page.Request.QueryString["id"].Split('|');
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.EMERGENCY:
                    return Constant.MenuCode.EmergencyCare.SERVICE_ORDER_TRANS_AIO;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.WORK_LIST_AIO;
                    //else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Nutrition)
                    //    return Constant.MenuCode.Nutrition.NUTRITION_ASSESSMENT_WORKLIST;
                    else
                        return Constant.MenuCode.MedicalDiagnostic.WORK_LIST_AIO;
                default:
                    return Constant.MenuCode.Outpatient.SERVICE_ORDER_TRANS_AIO;
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

                    SettingParameterDt setvar = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.MD_SERVICE_UNIT_OPERATING_THEATRE);
                    HealthcareServiceUnit hsuSetvar = BusinessLayer.GetHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", setvar.ParameterValue)).FirstOrDefault();

                    if (toHD.HealthcareServiceUnitID == hsuSetvar.HealthcareServiceUnitID && (toHD.ProcedureGroupID != null && toHD.ProcedureGroupID != 0))
                    {
                        hdnIsOperatingRoomOrderProcedure.Value = "1";
                    }

                    btnTransactionServiceOrder.Style.Add("display", "none");

                    PatientChargesHd entityHd = BusinessLayer.GetPatientChargesHdList(string.Format("TestOrderID = '{0}' AND GCTransactionStatus <> '{1}'", hdnTestOrderID.Value, Constant.TransactionStatus.VOID)).FirstOrDefault();
                    if (entityHd != null)
                    {
                        transactionNo = entityHd.TransactionNo;
                    }
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

                vConsultVisit2 entity = BusinessLayer.GetvConsultVisit2List(string.Format("VisitID = {0}", hdnVisitID.Value)).FirstOrDefault();

                List<vPatientVisitNote> lstEntity = BusinessLayer.GetvPatientVisitNoteList(string.Format("VisitID = {0} AND IsDeleted = 0", hdnVisitID.Value));
                if (lstEntity.Count == 0)
                {
                    divVisitNote.Attributes.Add("style", "display:none");
                }

                if (entity.DischargeDate != null && entity.DischargeDate.ToString(Constant.FormatString.DATE_FORMAT) != "01-Jan-1900")
                {
                    hdnIsDischarges.Value = "1";
                }
                else
                {
                    hdnIsDischarges.Value = "0";
                }

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
                    hdnTransactionHdID.Value = "0";
                    ((PatientManagementTransactionDetailServiceCtl)ctlService).OnAddRecord();
                    ((PatientManagementTransactionDetailDrugMSCtl)ctlDrugMS).OnAddRecord();
                    ((PatientManagementTransactionDetailDrugMSReturnCtl)ctlDrugMSReturn).OnAddRecord();
                    ((PatientManagementTransactionDetailLogisticCtl)ctlLogistic).OnAddRecord();
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

            hdnIsPregnant.Value = BusinessLayer.GetPatientBirthRecordRowCount(string.Format("MotherVisitID = {0}", hdnVisitID.Value)).ToString();
        }

        protected override void SetControlProperties()
        {
            ((PatientManagementTransactionDetailServiceCtl)ctlService).SetControlProperties();
            ((PatientManagementTransactionDetailDrugMSCtl)ctlDrugMS).SetControlProperties();
            ((PatientManagementTransactionDetailLogisticCtl)ctlLogistic).SetControlProperties();
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
                hdnTestOrderID.Value = param[1];
            else if (param[0] == "so")
                hdnServiceOrderID.Value = param[1];

            hdnGCTransactionStatus.Value = Constant.TransactionStatus.OPEN;

            imgIsAIOTransaction.Style.Add("display", "none");

            ((PatientManagementTransactionDetailServiceCtl)ctlService).OnAddRecord();
            ((PatientManagementTransactionDetailDrugMSCtl)ctlDrugMS).OnAddRecord();
            ((PatientManagementTransactionDetailDrugMSReturnCtl)ctlDrugMSReturn).OnAddRecord();
            ((PatientManagementTransactionDetailLogisticCtl)ctlLogistic).OnAddRecord();
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
            vPatientChargesHd2 entity = BusinessLayer.GetvPatientChargesHd2(filterExpression, PageIndex, " TransactionID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
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
            txtTestOrderInfo.Text = entity.TestOrderInfo;
            hdnProcedureGroupID.Value = entity.ProcedureGroupID.ToString();
            txtProcedureOrderInfo.Text = string.Format("{0} - {1}", entity.ProcedureGroupCode, entity.ProcedureGroupName);
            txtReferenceNo.Text = entity.ReferenceNo;
            txtTransactionDate.Text = entity.TransactionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTransactionTime.Text = entity.TransactionTime;
            hdnTotalPatient.Value = entity.TotalPatientAmount.ToString();
            hdnTotalPayer.Value = entity.TotalPayerAmount.ToString();
            txtRemarks.Text = entity.Remarks;
            bool flagHaveCharges = false;
            if (entity != null) flagHaveCharges = true;

            if (entity.IsAIOTransaction)
            {
                imgIsAIOTransaction.Src = string.Format("{0}", "~/libs/Images/Status/has_aio_package.png");
                imgIsAIOTransaction.Style.Remove("display");
            }
            else
            {
                imgIsAIOTransaction.Style.Add("display", "none");
            }

            ((PatientManagementTransactionDetailServiceCtl)ctlService).InitializeTransactionControl(flagHaveCharges);
            ((PatientManagementTransactionDetailDrugMSCtl)ctlDrugMS).InitializeTransactionControl(flagHaveCharges);
            ((PatientManagementTransactionDetailDrugMSReturnCtl)ctlDrugMSReturn).InitializeTransactionControl(flagHaveCharges);
            ((PatientManagementTransactionDetailLogisticCtl)ctlLogistic).InitializeTransactionControl(flagHaveCharges);

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
                try
                {
                    Int32 TransactionID = Convert.ToInt32(hdnTransactionHdID.Value);
                    PatientChargesHd entity = chargesDao.Get(TransactionID);
                    if (entity.PatientBillingID == null && entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        ((PatientManagementTransactionDetailServiceCtl)ctlService).OnVoidAllChargesDt(ctx, TransactionID);
                        ((PatientManagementTransactionDetailDrugMSCtl)ctlDrugMS).OnVoidAllChargesDt(ctx, TransactionID);
                        ((PatientManagementTransactionDetailDrugMSReturnCtl)ctlDrugMSReturn).OnVoidAllChargesDt(ctx, TransactionID);
                        ((PatientManagementTransactionDetailLogisticCtl)ctlLogistic).OnVoidAllChargesDt(ctx, TransactionID);

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
                                                orderDt.GCServiceOrderStatus = Constant.TestOrderStatus.OPEN;
                                            }
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
            TestOrderHdDao testOrderHdDao = new TestOrderHdDao(ctx);
            TestOrderDtDao testOrderDtDao = new TestOrderDtDao(ctx);
            ServiceOrderHdDao serviceOrderHdDao = new ServiceOrderHdDao(ctx);
            ServiceOrderDtDao serviceOrderDtDao = new ServiceOrderDtDao(ctx);
            try
            {
                PatientChargesHd entity = chargesDao.Get(Convert.ToInt32(hdnTransactionHdID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
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
                                            if (AppSession.RIS_HL7_MESSAGE_FORMAT == Constant.RIS_HL7MessageFormat.MEDAVIS)
                                            {
                                                var result2 = SendMedavisHL7OrderToRIS(Convert.ToInt32(hdnTestOrderID.Value), Convert.ToInt32(hdnTransactionHdID.Value));
                                                resultInfo = ((string)result2).Split('|');
                                            }
                                            else
                                            {
                                                var result2 = SendHL7OrderToRIS(Convert.ToInt32(hdnTestOrderID.Value), Convert.ToInt32(hdnTransactionHdID.Value));
                                                resultInfo = ((string)result2).Split('|');
                                            }
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
                            entity.Remarks = txtRemarks.Text;
                            entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                            entity.ProposedBy = AppSession.UserLogin.UserID;
                            entity.ProposedDate = DateTime.Now;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            chargesDao.Update(entity);

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
                                else if (hdnServiceOrderID.Value != "" && hdnServiceOrderID.Value != "0")
                                {
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    string filterDt = String.Format("ServiceOrderID = '{0}' AND ItemID = '{1}' AND IsDeleted = 0", hdnServiceOrderID.Value, patientChargesDt.ItemID);
                                    ServiceOrderDt orderDt = BusinessLayer.GetServiceOrderDtList(filterDt, ctx).FirstOrDefault();
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
                                        serviceOrderDtDao.Update(orderDt);
                                    }
                                }
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

                        APIMessageLog entityAPILog = new APIMessageLog()
                        {
                            MessageDateTime = DateTime.Now,
                            Recipient = "RIS",
                            Sender = "MEDINFRAS",
                            MessageText = JsonConvert.SerializeObject(oData)
                        };

                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/inputOrder/", url));
                        request.Method = "POST";
                        request.ContentType = "application/json";
                        Web.Common.Methods.SetRequestHeader(request);

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

                            entityAPILog.IsSuccess = true;
                            BusinessLayer.InsertAPIMessageLog(entityAPILog);
                        }
                        else
                        {
                            result = string.Format("{0}|{1}", "0", respInfo.Remark);

                            entityAPILog.IsSuccess = false;
                            entityAPILog.ErrorMessage = respInfo.Remark;
                            BusinessLayer.InsertAPIMessageLog(entityAPILog);
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
                result = string.Format("{0}|{1}", "0", ex.Status.ToString());
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

                            orderParamedicCode = oOrderHd != null ? oOrderHd.ParamedicCode : oVisit.ParamedicCode;
                            orderParamedicName = oOrderHd != null ? oOrderHd.ParamedicName : oVisit.ParamedicName;

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
                        oVisitInfo.ServiceUnitCode = oVisit.ServiceUnitCode;
                        oVisitInfo.ServiceUnitName = oVisit.ServiceUnitName;
                        oVisitInfo.RoomCode = oVisit.RoomCode;
                        oVisitInfo.BedCode = oVisit.BedCode;

                        oData.VisitInformation = oVisitInfo;

                        List<TestOrderDetailInfo> lstDetail = new List<TestOrderDetailInfo>();

                        foreach (vPatientChargesDt item in oList)
                        {
                            TestOrderDetailInfo oDetail = new TestOrderDetailInfo();
                            string modality = String.IsNullOrEmpty(item.GCModality) ? "CT" : item.GCModality.Substring(5);

                            ParamedicMaster oParamedic = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicCode = '{0}'", item.ParamedicCode)).FirstOrDefault();
                            string requestedPhysicianName = string.Format("{0}^{1}^^^^", item.ParamedicCode, item.ParamedicName);
                            if (oParamedic != null)
                            {
                                requestedPhysicianName = string.Format("{0}^{1}^^^^", oParamedic.ParamedicCode, oParamedic.FullName);
                            }

                            oDetail.ItemCode = item.ItemCode.Trim();
                            oDetail.ItemName = item.ItemName1.Trim();

                            oDetail.RequestedPhysicianName = requestedPhysicianName;
                            oDetail.IsCITO = item.IsCITO;
                            oDetail.ModalityType = modality.Trim();
                            oDetail.ModalityCode = item.ModalityCode;
                            oDetail.ModalityAETitle = item.ModalityAETitle;
                            oDetail.Remarks = string.Empty;

                            lstDetail.Add(oDetail);
                        }

                        oData.OrderItemList = lstDetail;

                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/", url));
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
                //switch (ex.Status)
                //{
                //    case WebExceptionStatus.ProtocolError:
                //        result = string.Format("{0}|{1}", "0", "Method not found");
                //        break;
                //    default:
                //        result = string.Format("{0}|{1}", "0", string.Format("{0} ({1})", ex.Status.ToString()));
                //        break;
                //}
                result = string.Format("{0}|{1}", "0", ex.Status.ToString());
                return result;
            }
        }

        public object SendMedavisHL7OrderToRIS(int testOrderID, int transactionID)
        {
            string result = "";
            try
            {
                string url = AppSession.RIS_WEB_API_URL;
                bool isfromOrder = testOrderID > 0;

                #region Convert into DTO Objects
                string filterExpression = string.Format("TransactionID = {0}", transactionID);
                vPatientChargesHd oHeader = BusinessLayer.GetvPatientChargesHdList(filterExpression).FirstOrDefault();
                if (oHeader != null)
                {
                    vConsultVisit oVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", oHeader.VisitID)).FirstOrDefault();
                    filterExpression += " AND IsDeleted = 0";
                    List<vPatientChargesDt> oList = BusinessLayer.GetvPatientChargesDtList(filterExpression);
                    ImagingOrderDTO oData = new ImagingOrderDTO();
                    if (oList.Count > 0)
                    {
                        string ipaddress, port = string.Empty;
                        SettingParameterDt oParam = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode ='{1}'", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IS_RIS_HL7_BROKER)).FirstOrDefault();

                        HL7MessageText hl7Message = new HL7MessageText();

                        string healthcareID = AppSession.UserLogin.HealthcareID;
                        string orderNo = string.Empty;
                        string orderPriority = "NORMAL";
                        string orderParamedicCode = oVisit.ParamedicCode.Trim();
                        string orderParamedicName = oVisit.ParamedicName.Trim();
                        DateTime orderDate = DateTime.Now.Date;
                        string orderTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                        string remarks = "RADIOLOGY";
                        if (testOrderID > 0)
                        {
                            vTestOrderHd oOrderHd = BusinessLayer.GetvTestOrderHdList(string.Format("TestOrderID = {0}", testOrderID)).FirstOrDefault();
                            orderNo = oOrderHd.TestOrderNo;
                            orderPriority = oOrderHd != null ? (oOrderHd.IsCITO ? "HIGH" : "NORMAL") : "NORMAL";

                            orderParamedicCode = oOrderHd != null ? oOrderHd.ParamedicCode : oVisit.ParamedicCode;
                            orderParamedicName = oOrderHd != null ? oOrderHd.ParamedicName : oVisit.ParamedicName;

                            orderDate = oOrderHd.TestOrderDate;
                            orderTime = oOrderHd.TestOrderTime;
                            remarks = oOrderHd.Remarks;
                        }

                        string orderDateTime = string.Format("{0}{1}00", orderDate.ToString(Constant.FormatString.DATE_FORMAT_112), orderTime.Replace(":", ""));

                        int sequenceNo = 1;
                        int errorNo = 0;
                        foreach (vPatientChargesDt item in oList)
                        {
                            string detailID = item.ID.ToString();
                            string datetimeStamp = string.Format("{0}{1}{2}", DateTime.Now.Date.ToString(Constant.FormatString.DATE_FORMAT_112), DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT_2), sequenceNo.ToString());
                            string modality = String.IsNullOrEmpty(item.GCModality) ? "CT" : item.GCModality.Substring(5);
                            string accessionNo = string.Format("{0}{1}", modality, datetimeStamp);
                            string orderStatus = "NW";
                            string priority = item.IsCITO ? "S" : "R";
                            string diagnoseName = item.DiagnoseTestOrder == null ? "-" : item.DiagnoseTestOrder.Trim();

                            ParamedicMaster oParamedic = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicCode = '{0}'", item.ParamedicCode)).FirstOrDefault();
                            string requestedPhysicianName = string.Format("{0}^{1}^^^^", item.ParamedicCode, item.ParamedicName);
                            if (oParamedic != null)
                            {
                                requestedPhysicianName = string.Format("{0}^{1}^^^^", oParamedic.ParamedicCode, oParamedic.FullName);
                            }

                            #region MSH
                            HL7Segment msh = new HL7Segment();
                            msh.Field(0, "MSH");
                            msh.Field(1, ""); //will be ignored
                            msh.Field(2, @"^~\&");
                            msh.Field(3, "MEDINFRAS-API_RIS");
                            msh.Field(4, AppSession.UserLogin.HealthcareID);
                            msh.Field(5, CommonConstant.HL7_MEDAVIS_MSG.IDENTIFICATION_1);
                            msh.Field(6, CommonConstant.HL7_MEDAVIS_MSG.IDENTIFICATION_2);
                            msh.Field(7, orderDateTime);
                            msh.Field(8, string.Empty);
                            msh.Field(9, "ORM^O01");
                            msh.Field(10, orderNo);
                            msh.Field(11, "P");
                            msh.Field(12, CommonConstant.HL7_MEDAVIS_MSG.HL7_VERSION);
                            msh.Field(13, string.Empty);
                            msh.Field(14, string.Empty);
                            msh.Field(15, "ER");
                            msh.Field(16, "ER");
                            msh.Field(17, string.Empty);
                            msh.Field(18, "8859/1");

                            hl7Message.Add(msh);
                            #endregion

                            #region PID
                            string patientName = string.Format("{0}^{1}^{2}^^{3}^^^", oVisit.LastName, oVisit.FirstName, oVisit.MiddleName, oVisit.Salutation);
                            string dateofBirth = string.Format("{0}000000", oVisit.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT_112).Trim());
                            string gender = oVisit.GenderCodeSuffix;
                            string patientAddress = oVisit.HomeAddress == null ? string.Empty : string.Format("{0}^^{1}^", oVisit.StreetName.Replace("\n", " ").Replace("\t", " ").Replace(Environment.NewLine, " ").TrimEnd(), oVisit.City.TrimEnd());
                            string phoneNo = oVisit.PhoneNo1 == null ? string.Empty : oVisit.PhoneNo1.Trim();

                            HL7Segment pid = new HL7Segment();
                            pid.Field(0, "PID");
                            pid.Field(1, "1");
                            pid.Field(2, oVisit.MedicalNo);
                            pid.Field(3, oVisit.MedicalNo);
                            pid.Field(4, string.Empty);
                            pid.Field(5, patientName);
                            pid.Field(6, string.Empty);
                            pid.Field(7, dateofBirth);
                            pid.Field(8, gender);
                            pid.Field(9, string.Empty);
                            pid.Field(10, string.Empty);
                            pid.Field(11, patientAddress);
                            pid.Field(12, string.Empty);
                            pid.Field(13, phoneNo);

                            hl7Message.Add(pid);
                            #endregion

                            #region PV1
                            string pv1_Param1 = "I";
                            string serviceUnitName = oVisit.ServiceUnitName == null ? string.Empty : oVisit.ServiceUnitName.Trim();
                            string bedCode = oVisit.BedCode;
                            string patientLocation = string.Format("{0}{1}", serviceUnitName, !string.IsNullOrEmpty(bedCode) ? " - " + bedCode : string.Empty);

                            switch (oVisit.DepartmentID)
                            {
                                case Constant.Facility.INPATIENT:
                                    pv1_Param1 = "I";
                                    break;
                                case Constant.Facility.EMERGENCY:
                                    pv1_Param1 = "E";
                                    break;
                                default:
                                    pv1_Param1 = "O";
                                    break;
                            }

                            int noOfItem = oList.Count;
                            //string requestedPhysicianName = noOfItem > 1 ?
                            string orderPhysicianName = string.Format("{0}^^^^", orderParamedicName);

                            HL7Segment pv1 = new HL7Segment();
                            pv1.Field(0, "PV1");
                            pv1.Field(1, string.Empty);
                            pv1.Field(2, pv1_Param1);
                            pv1.Field(3, patientLocation);
                            pv1.Field(4, string.Empty);
                            pv1.Field(5, string.Empty);
                            pv1.Field(6, string.Empty);
                            pv1.Field(7, string.Empty);
                            pv1.Field(8, orderPhysicianName);
                            pv1.Field(9, string.Empty);
                            pv1.Field(10, string.Empty);
                            pv1.Field(11, string.Empty);
                            pv1.Field(12, string.Empty);
                            pv1.Field(13, string.Empty);
                            pv1.Field(14, string.Empty);
                            pv1.Field(15, string.Empty);
                            pv1.Field(16, string.Empty);
                            pv1.Field(17, string.Empty);
                            pv1.Field(18, string.Empty);
                            pv1.Field(19, oVisit.RegistrationNo);

                            hl7Message.Add(pv1);
                            #endregion

                            #region ORC
                            HL7Segment orc = new HL7Segment();
                            orc.Field(0, "ORC");
                            orc.Field(1, orderStatus);
                            orc.Field(2, orderNo);
                            orc.Field(3, string.Empty);
                            orc.Field(4, detailID);
                            orc.Field(5, string.Empty);
                            orc.Field(6, string.Empty);
                            orc.Field(7, string.Format("{0}^^5^^^{1}", item.ChargedQuantity.ToString(), priority));
                            orc.Field(8, string.Empty);
                            orc.Field(9, orderDateTime);
                            orc.Field(10, string.Empty);
                            orc.Field(11, string.Empty);
                            orc.Field(12, requestedPhysicianName);
                            orc.Field(13, string.Empty);
                            orc.Field(14, string.Empty);
                            orc.Field(15, string.Empty);
                            orc.Field(16, string.Empty);
                            orc.Field(17, string.Empty);
                            orc.Field(18, string.Empty);
                            orc.Field(19, string.Empty);
                            orc.Field(20, string.Empty);
                            orc.Field(21, string.Empty);

                            hl7Message.Add(orc);
                            #endregion

                            #region OBR
                            HL7Segment obr = new HL7Segment();
                            obr.Field(0, "OBR");
                            obr.Field(1, string.Empty);
                            obr.Field(2, orderNo);
                            obr.Field(3, string.Empty);
                            obr.Field(4, string.Format("{0}^{1}", item.ItemCode, item.AlternateItemName));
                            obr.Field(5, priority);
                            obr.Field(6, orderDateTime);
                            obr.Field(7, string.Empty);
                            obr.Field(8, string.Empty);
                            obr.Field(9, string.Empty);
                            obr.Field(10, "A");
                            obr.Field(11, string.Empty);
                            obr.Field(12, string.Empty);
                            obr.Field(13, string.Empty);
                            obr.Field(14, string.Empty);
                            obr.Field(15, string.Empty);
                            obr.Field(16, string.Empty);
                            obr.Field(17, accessionNo);
                            obr.Field(18, string.Empty);
                            obr.Field(19, string.Empty);
                            obr.Field(20, string.Empty);
                            obr.Field(21, string.Empty);
                            obr.Field(22, string.Empty);
                            obr.Field(23, modality);
                            obr.Field(24, string.Empty);
                            obr.Field(25, string.Empty);
                            obr.Field(26, "^^5");
                            obr.Field(27, string.Empty);
                            obr.Field(28, string.Empty);
                            obr.Field(29, string.Empty);
                            obr.Field(30, string.Empty);
                            obr.Field(31, string.Empty);
                            obr.Field(32, string.Empty);
                            obr.Field(33, string.Empty);
                            obr.Field(34, string.Empty);
                            obr.Field(35, string.Empty);
                            obr.Field(36, string.Empty);
                            obr.Field(37, string.Empty);
                            obr.Field(38, string.Empty);
                            obr.Field(39, string.Empty);
                            obr.Field(40, string.Empty);
                            obr.Field(41, string.Empty);
                            obr.Field(42, string.Empty);
                            obr.Field(43, string.Format("{0}^{1}^^^{1}", item.ItemCode, item.ItemName1));

                            hl7Message.Add(obr);
                            #endregion

                            #region NTE
                            if (!string.IsNullOrEmpty(oHeader.Remarks))
                                remarks = oHeader.Remarks.Substring(0, 500);

                            HL7Segment nte = new HL7Segment();
                            nte.Field(0, "NTE");
                            nte.Field(1, string.Empty);
                            nte.Field(2, string.Empty);
                            nte.Field(3, remarks);

                            hl7Message.Add(nte);
                            #endregion

                            #region Send To RIS Broker Service
                            if (oParam != null)
                            {
                                string[] paramInfo = oParam.ParameterValue.Split(':');
                                ipaddress = paramInfo[0];
                                port = !string.IsNullOrEmpty(paramInfo[1]) ? paramInfo[1] : "6000";

                                result = CommonMethods.SendMessageToListener(ipaddress, port, (char)0x0B + hl7Message.Serialize() + (char)0x1C + (char)0x0D);
                                string[] resultInfo = result.Split('|');
                                if (resultInfo[0] == "0")
                                    errorNo += 1;
                            }
                            else
                            {
                                result = string.Format("{0}|{1}", "0", "Invalid Configuration for RIS HL7 Broker IP Address");
                                break;
                            }
                            #endregion

                            sequenceNo += 1;
                        }
                        if (errorNo > 0)
                            if (errorNo < oList.Count)
                                result = string.Format("{0}|{1}", "1", "There are {0} item(s) is rejected by the RIS");
                            else
                                result = string.Format("{0}|{1}", "0", "The order is rejected by the RIS..Please check the log message");
                        else
                            result = string.Format("{0}|{1}", "1", string.Empty);
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
                result = string.Format("{0}|{1}", "0", ex.Status.ToString());
                return result;
            }
        }

        /// <summary>
        /// Used to send imaging order via Medinfras API into RIS Interface Database
        /// </summary>
        /// <param name="testOrderID"></param>
        /// <param name="transactionID"></param>
        /// <returns></returns>
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
                            oDetail.DiagnoseName = item.DiagnoseTestOrder.Trim();
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
                result = string.Format("{0}|{1}", "0", ex.Status.ToString());
                return result;
            }
        }
        #endregion
    }
}
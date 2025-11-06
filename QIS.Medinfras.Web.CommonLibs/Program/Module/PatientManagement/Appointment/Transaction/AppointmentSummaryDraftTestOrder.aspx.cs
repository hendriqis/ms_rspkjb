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
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class AppointmentSummaryDraftTestOrder : AppointmentBasePageTrxPatientManagement
    {
        vConsultVisit2 entityCV = null;

        public override string OnGetMenuCode()
        {
            string id = Page.Request.QueryString["id"];
            switch (hdnAppointmentDepartmentID.Value)
            {
                case Constant.Facility.OUTPATIENT:
                    switch (id)
                    {
                        case "lb": return Constant.MenuCode.Outpatient.BILL_SUMMARY_LABORATORY;
                        case "is": return Constant.MenuCode.Outpatient.DRAFT_CHARGES_IMAGING_ENTRY;
                        case "md": return Constant.MenuCode.Outpatient.BILL_SUMMARY_MEDICAL_DIAGNOSTIC;
                        default: return Constant.MenuCode.Outpatient.BILL_SUMMARY_CHARGES;
                    }
                case Constant.Facility.MEDICAL_CHECKUP:
                    switch (id)
                    {
                        case "lb": return Constant.MenuCode.MedicalCheckup.BILL_SUMMARY_LABORATORY;
                        case "is": return Constant.MenuCode.MedicalCheckup.BILL_SUMMARY_IMAGING;
                        case "md": return Constant.MenuCode.MedicalCheckup.BILL_SUMMARY_MEDICAL_DIAGNOSTIC;
                        default: return Constant.MenuCode.MedicalCheckup.BILL_SUMMARY_CHARGES;
                    }
                case Constant.Facility.EMERGENCY:
                    switch (id)
                    {
                        case "lb": return Constant.MenuCode.EmergencyCare.BILL_SUMMARY_LABORATORY;
                        case "is": return Constant.MenuCode.EmergencyCare.BILL_SUMMARY_IMAGING;
                        case "md": return Constant.MenuCode.EmergencyCare.BILL_SUMMARY_MEDICAL_DIAGNOSTIC;
                        default: return Constant.MenuCode.EmergencyCare.BILL_SUMMARY_CHARGES;
                    }
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                    {
                        switch (id)
                        {
                            case "lb": return Constant.MenuCode.Imaging.BILL_SUMMARY_LABORATORY;
                            case "md": return Constant.MenuCode.Imaging.BILL_SUMMARY_MEDICAL_DIAGNOSTIC;
                            default: return Constant.MenuCode.Imaging.BILL_SUMMARY_CHARGES;
                        }
                    }
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                    {
                        switch (id)
                        {
                            case "is": return Constant.MenuCode.Laboratory.BILL_SUMMARY_IMAGING;
                            case "md": return Constant.MenuCode.Laboratory.BILL_SUMMARY_MEDICAL_DIAGNOSTIC;
                            default: return Constant.MenuCode.Laboratory.BILL_SUMMARY_CHARGES;
                        }
                    }
                    switch (id)
                    {
                        case "lb": return Constant.MenuCode.MedicalDiagnostic.BILL_SUMMARY_LABORATORY;
                        case "is": return Constant.MenuCode.MedicalDiagnostic.BILL_SUMMARY_IMAGING;
                        case "md": return Constant.MenuCode.MedicalDiagnostic.BILL_SUMMARY_MEDICAL_DIAGNOSTIC;
                        default: return Constant.MenuCode.MedicalDiagnostic.BILL_SUMMARY_CHARGES;
                    }
                default:
                    switch (id)
                    {
                        case "lb": return Constant.MenuCode.Outpatient.BILL_SUMMARY_LABORATORY;
                        case "is": return Constant.MenuCode.Outpatient.BILL_SUMMARY_IMAGING;
                        case "md": return Constant.MenuCode.Outpatient.BILL_SUMMARY_MEDICAL_DIAGNOSTIC;
                        default: return Constant.MenuCode.Outpatient.BILL_SUMMARY_CHARGES;
                    }
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = (hdnAppointmentStatus.Value == Constant.AppointmentStatus.STARTED);
            //IsAllowSave = !entityCV.IsLockDown;
            //IsAllowVoid = !entityCV.IsLockDown;
        }

        public override string GetGCCustomerType()
        {
            return hdnGCCustomerType.Value;
        }

        public override string GetAppointmentStatus()
        {
            return hdnAppointmentStatus.Value;
        }

        public override int GetAppointmentID()
        {
            return Convert.ToInt32(hdnAppointmentID.Value);
        }

        public override int GetClassID()
        {
            return Convert.ToInt32(hdnClassID.Value);
        }

        public override int GetAppointmentPhysicianID()
        {
            return Convert.ToInt32(hdnAppointmentPhysicianID.Value);
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

        protected override void InitializeDataControl()
        {
            vAppointment entity = BusinessLayer.GetvAppointmentList(string.Format("AppointmentID = {0}", Request.QueryString["appid"]))[0];
            hdnAppointmentPhysicianID.Value = entity.ParamedicID.ToString();
            hdnAppointmentStatus.Value = entity.GCAppointmentStatus;
            hdnAppointmentID.Value = entity.AppointmentID.ToString();

            hdnLocationID.Value = entity.LocationID.ToString();
            hdnLogisticLocationID.Value = entity.LogisticLocationID.ToString();
            //hdnBusinessPartnerID.Value = entityCV.BusinessPartnerID.ToString();
            //hdnGCCustomerType.Value = entityCV.GCCustomerType;
            //hdnClassID.Value = entityCV.ChargeClassID.ToString();
            hdnAppointmentDepartmentID.Value = entity.DepartmentID;
            hdnDepartmentID.Value = Constant.Facility.DIAGNOSTIC;

            string serviceUnitID = "";
            if (Page.Request.QueryString["id"] == "lb")
            {
                SettingParameter settingParameter = BusinessLayer.GetSettingParameter(Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID);
                serviceUnitID = settingParameter.ParameterValue;
                vHealthcareServiceUnit hsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = {0} AND ServiceUnitID IN ({1})", AppSession.UserLogin.HealthcareID, serviceUnitID)).FirstOrDefault();
                hdnHealthcareServiceUnitID.Value = hsu.HealthcareServiceUnitID.ToString();
            }
            else if (Page.Request.QueryString["id"] == "is")
            {
                SettingParameter settingParameter = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID);
                serviceUnitID = settingParameter.ParameterValue;
                vHealthcareServiceUnit hsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = {0} AND ServiceUnitID IN ({1})", AppSession.UserLogin.HealthcareID, serviceUnitID)).FirstOrDefault();
                hdnHealthcareServiceUnitID.Value = hsu.HealthcareServiceUnitID.ToString();
            }
            else if (Page.Request.QueryString["id"] == "md")
            {
                SettingParameter settingParameterLB = BusinessLayer.GetSettingParameter(Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID);
                SettingParameter settingParameterIS = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID);
                serviceUnitID = string.Format("{0}, {1}", settingParameterLB.ParameterValue, settingParameterIS.ParameterValue);
                vHealthcareServiceUnit hsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format(
                    "HealthcareID = {0} AND DepartmentID = '{1}' AND ServiceUnitID NOT IN ({2})",
                    AppSession.UserLogin.HealthcareID, hdnDepartmentID.Value, serviceUnitID)).FirstOrDefault();
                hdnHealthcareServiceUnitID.Value = hsu.HealthcareServiceUnitID.ToString();
            }

            IsLoadFirstRecord = (OnGetRowCount() > 0);
            hdnTransactionHdID.Value = "0";
            int count = BusinessLayer.GetServiceUnitParamedicRowCount(string.Format("HealthcareServiceUnitID = {0}", hdnHealthcareServiceUnitID.Value));
            if (count > 0)
                hdnIsHealthcareServiceUnitHasParamedic.Value = "1";
            else
                hdnIsHealthcareServiceUnitHasParamedic.Value = "0";

            //List<ParamedicMaster> lstParamedic = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {0}) AND IsDeleted = 0", hdnHealthcareServiceUnitID.Value));
            //if (lstParamedic.Count == 1)
            //{
            //    ParamedicMaster paramedic = lstParamedic.FirstOrDefault();
            //    hdnPhysicianID.Value = paramedic.ParamedicID.ToString();
            //    hdnPhysicianCode.Value = paramedic.ParamedicCode;
            //    hdnPhysicianName.Value = paramedic.FullName;
            //}
            //else
            //{
            //    hdnPhysicianID.Value = hdnPhysicianCode.Value = hdnPhysicianName.Value = "";
            //}

            ((AppointmentDraftTransactionDetailServiceCtl)ctlService).OnAddRecord();
            ((AppointmentDraftTransactionDetailDrugMSCtl)ctlDrugMS).OnAddRecord();
            ((AppointmentDraftTransactionDetailLogisticCtl)ctlLogistic).OnAddRecord();

            string moduleName = Helper.GetModuleName();
            string ModuleID = Helper.GetModuleID(moduleName);
            GetUserMenuAccess menu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault();
            string CRUDMode = menu.CRUDMode;
            hdnIsAllowVoid.Value = CRUDMode.Contains("X") ? "1" : "0";
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void SetControlProperties()
        {
            ((AppointmentDraftTransactionDetailServiceCtl)ctlService).SetControlProperties();
            ((AppointmentDraftTransactionDetailDrugMSCtl)ctlDrugMS).SetControlProperties();
            ((AppointmentDraftTransactionDetailLogisticCtl)ctlLogistic).SetControlProperties();

            string serviceUnitID = "";
            if (Page.Request.QueryString["id"] == "lb")
            {
                SettingParameter settingParameter = BusinessLayer.GetSettingParameter(Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID);
                serviceUnitID = settingParameter.ParameterValue;
                vHealthcareServiceUnit hsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = {0} AND ServiceUnitID IN ({1})", AppSession.UserLogin.HealthcareID, serviceUnitID)).FirstOrDefault();
                hdnHealthcareServiceUnitID.Value = hsu.HealthcareServiceUnitID.ToString();
                txtServiceUnitCode.Text = hsu.ServiceUnitCode;
                txtServiceUnitName.Text = hsu.ServiceUnitName;
            }
            else if (Page.Request.QueryString["id"] == "is")
            {
                SettingParameter settingParameter = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID);
                serviceUnitID = settingParameter.ParameterValue;
                vHealthcareServiceUnit hsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = {0} AND ServiceUnitID IN ({1})", AppSession.UserLogin.HealthcareID, serviceUnitID)).FirstOrDefault();
                hdnHealthcareServiceUnitID.Value = hsu.HealthcareServiceUnitID.ToString();
                txtServiceUnitCode.Text = hsu.ServiceUnitCode;
                txtServiceUnitName.Text = hsu.ServiceUnitName;
            }
            else if (Page.Request.QueryString["id"] == "md")
            {
                SettingParameter settingParameterLB = BusinessLayer.GetSettingParameter(Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID);
                SettingParameter settingParameterIS = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID);
                serviceUnitID = string.Format("{0}, {1}", settingParameterLB.ParameterValue, settingParameterIS.ParameterValue);
                vHealthcareServiceUnit hsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format(
                    "HealthcareID = {0} AND DepartmentID = '{1}' AND ServiceUnitID NOT IN ({2})",
                    AppSession.UserLogin.HealthcareID, hdnDepartmentID.Value, serviceUnitID)).FirstOrDefault();
                hdnHealthcareServiceUnitID.Value = hsu.HealthcareServiceUnitID.ToString();
                txtServiceUnitCode.Text = hsu.ServiceUnitCode;
                txtServiceUnitName.Text = hsu.ServiceUnitName;
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnTransactionHdID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(chkIsCorrectionTransaction, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtTransactionNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtTransactionDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtTransactionTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(txtReferenceNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(lblServiceUnit, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtServiceUnitCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtServiceUnitName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
        }

        public override void OnAddRecord()
        {
            hdnAppointmentStatus.Value = Constant.TransactionStatus.OPEN;
            ((AppointmentDraftTransactionDetailServiceCtl)ctlService).OnAddRecord();
            ((AppointmentDraftTransactionDetailDrugMSCtl)ctlDrugMS).OnAddRecord();
            ((AppointmentDraftTransactionDetailLogisticCtl)ctlLogistic).OnAddRecord();
        }

        protected string GetFilterExpression()
        {
            string serviceUnitID = "", hsuID = "";
            if (Page.Request.QueryString["id"] == "lb")
            {
                SettingParameter settingParameter = BusinessLayer.GetSettingParameter(Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID);
                serviceUnitID = settingParameter.ParameterValue;
                vHealthcareServiceUnit hsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = {0} AND ServiceUnitID IN ({1})", AppSession.UserLogin.HealthcareID, serviceUnitID)).FirstOrDefault();
                hsuID = hsu.HealthcareServiceUnitID.ToString();
            }
            else if (Page.Request.QueryString["id"] == "is")
            {
                SettingParameter settingParameter = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID);
                serviceUnitID = settingParameter.ParameterValue;
                vHealthcareServiceUnit hsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = {0} AND ServiceUnitID IN ({1})", AppSession.UserLogin.HealthcareID, serviceUnitID)).FirstOrDefault();
                hsuID = hsu.HealthcareServiceUnitID.ToString();
            }
            else if (Page.Request.QueryString["id"] == "md")
            {
                SettingParameter settingParameterLB = BusinessLayer.GetSettingParameter(Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID);
                SettingParameter settingParameterIS = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID);
                serviceUnitID = string.Format("{0}, {1}", settingParameterLB.ParameterValue, settingParameterIS.ParameterValue);
                List<vHealthcareServiceUnit> lsthsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format(
                    "HealthcareID = {0} AND DepartmentID = '{1}' AND ServiceUnitID NOT IN ({2})",
                    AppSession.UserLogin.HealthcareID, hdnDepartmentID.Value, serviceUnitID));
                foreach (vHealthcareServiceUnit hsu in lsthsu)
                {
                    hsuID += "," + hsu.HealthcareServiceUnitID;
                }
                hsuID = hsuID.Substring(1, hsuID.Length - 1);
            }

            string filterExpression = "";
            filterExpression = string.Format("AppointmentID = {0}", hdnAppointmentID.Value);
            filterExpression += string.Format(" AND HealthcareServiceUnitID IN ({0})", hsuID);
            return filterExpression;
        }

        protected string GetServiceUnitFilterFilterExpression()
        {
            SettingParameter settingParameterLB = BusinessLayer.GetSettingParameter(Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID);
            SettingParameter settingParameterIS = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID);
            string serviceUnitID = string.Format("{0}, {1}", settingParameterLB.ParameterValue, settingParameterIS.ParameterValue);

            if (Page.Request.QueryString["id"] == "lb")
            {
                return string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND ServiceUnitID IN ({2}) AND IsDeleted = 0",
                    AppSession.UserLogin.HealthcareID, Constant.Facility.DIAGNOSTIC, settingParameterLB.ParameterValue);
            }
            else if (Page.Request.QueryString["id"] == "is")
            {
                return string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND ServiceUnitID IN ({2}) AND IsDeleted = 0",
                    AppSession.UserLogin.HealthcareID, Constant.Facility.DIAGNOSTIC, settingParameterIS.ParameterValue);
            }
            else
            {
                return string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND ServiceUnitID NOT IN ({2}) AND IsDeleted = 0",
                    AppSession.UserLogin.HealthcareID, Constant.Facility.DIAGNOSTIC, serviceUnitID);
            }

        }

        public override int OnGetRowCount()
        {
            string filterExpression = GetFilterExpression();
            return BusinessLayer.GetvDraftPatientChargesHdRowCount(filterExpression);
        }

        #region Load Entity
        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            vDraftPatientChargesHd entity = BusinessLayer.GetvDraftPatientChargesHd(filterExpression, PageIndex, " TransactionID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            PageIndex = BusinessLayer.GetvDraftPatientChargesHdRowIndex(filterExpression, keyValue, "TransactionID DESC");
            vDraftPatientChargesHd entity = BusinessLayer.GetvDraftPatientChargesHd(filterExpression, PageIndex, "TransactionID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected bool IsEditable = true;
        private void EntityToControl(vDraftPatientChargesHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                watermarkText = entity.TransactionStatusWatermark;
                IsEditable = false;
                IsAllowProposed = false;
            }
            else
            {
                isShowWatermark = false;
                IsEditable = true;
                IsAllowProposed = true;
            }
            //if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            //{
            //    isShowWatermark = true;
            //    watermarkText = entity.TransactionStatusWatermark;
            //}
            //else
            //{
            //    isShowWatermark = false;
            //}
            hdnAppointmentStatus.Value = entity.GCTransactionStatus;
            hdnTransactionHdID.Value = entity.TransactionID.ToString();
            txtTransactionNo.Text = entity.DraftTransactionNo;
            txtReferenceNo.Text = entity.ReferenceNo;
            chkIsCorrectionTransaction.Checked = entity.IsCorrectionTransaction;
            txtServiceUnitCode.Text = entity.ServiceUnitCode;
            txtServiceUnitName.Text = entity.ServiceUnitName;
            txtRemarks.Text = entity.Remarks;
            txtTransactionDate.Text = entity.DraftTransactionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTransactionTime.Text = entity.DraftTransactionTime;
            bool flagHaveCharges = false;
            if (entity != null) flagHaveCharges = true;
            ((AppointmentDraftTransactionDetailServiceCtl)ctlService).InitializeTransactionControl(flagHaveCharges);
            ((AppointmentDraftTransactionDetailDrugMSCtl)ctlDrugMS).InitializeTransactionControl(flagHaveCharges);
            ((AppointmentDraftTransactionDetailLogisticCtl)ctlLogistic).InitializeTransactionControl(flagHaveCharges);
        }
        #endregion

        #region Save Entity
        public override void SaveTransactionHeader(IDbContext ctx, ref int transactionID)
        {
            DraftPatientChargesHdDao entityHdDao = new DraftPatientChargesHdDao(ctx);
            if (hdnTransactionHdID.Value == "0")
            {
                DraftPatientChargesHd entityHd = new DraftPatientChargesHd();
                entityHd.AppointmentID = Convert.ToInt32(hdnAppointmentID.Value);
                entityHd.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                switch (hdnDepartmentID.Value)
                {
                    case Constant.Facility.INPATIENT: entityHd.DraftTransactionCode = Constant.TransactionCode.DRAFT_IP_CHARGES; break;
                    case Constant.Facility.EMERGENCY: entityHd.DraftTransactionCode = Constant.TransactionCode.DRAFT_ER_CHARGES; break;
                    case Constant.Facility.DIAGNOSTIC:
                        if (Page.Request.QueryString["id"] == "is")
                            entityHd.DraftTransactionCode = Constant.TransactionCode.DRAFT_IMAGING_CHARGES;
                        else
                        {
                            if (hdnAppointmentDepartmentID.Value == Constant.Facility.DIAGNOSTIC && AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                                entityHd.DraftTransactionCode = Constant.TransactionCode.DRAFT_IMAGING_CHARGES;
                            else
                                entityHd.DraftTransactionCode = Constant.TransactionCode.DRAFT_OTHER_DIAGNOSTIC_CHARGES;
                        }; break;
                    default: entityHd.DraftTransactionCode = Constant.TransactionCode.DRAFT_OP_CHARGES; break;
                }

                entityHd.DraftTransactionDate = Helper.GetDatePickerValue(Request.Form[txtTransactionDate.UniqueID]);
                entityHd.DraftTransactionTime = Request.Form[txtTransactionTime.UniqueID];
                entityHd.IsCorrectionTransaction = chkIsCorrectionTransaction.Checked;
                entityHd.Remarks = txtRemarks.Text;
                entityHd.PatientBillingID = null;
                entityHd.ReferenceNo = txtReferenceNo.Text;
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                entityHd.GCVoidReason = null;
                entityHd.DraftTransactionNo = BusinessLayer.GenerateTransactionNo(entityHd.DraftTransactionCode, entityHd.DraftTransactionDate, ctx);
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                entityHd.TransactionID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
                transactionID = entityHd.TransactionID;
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
            DraftPatientChargesHdDao entityHdDao = new DraftPatientChargesHdDao(ctx);
            try
            {
                DraftPatientChargesHd entityHd = new DraftPatientChargesHd();
                entityHd.AppointmentID = Convert.ToInt32(hdnAppointmentID.Value);
                entityHd.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                entityHd.DraftTransactionCode = Constant.TransactionCode.DRAFT_LABORATORY_CHARGES;
                entityHd.DraftTransactionDate = Helper.GetDatePickerValue(Request.Form[txtTransactionDate.UniqueID]);
                entityHd.DraftTransactionTime = Request.Form[txtTransactionTime.UniqueID];
                entityHd.PatientBillingID = null;
                entityHd.IsCorrectionTransaction = chkIsCorrectionTransaction.Checked;
                entityHd.ReferenceNo = txtReferenceNo.Text;
                entityHd.Remarks = txtRemarks.Text;
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                entityHd.GCVoidReason = null;
                entityHd.TotalPatientAmount = 0;
                entityHd.TotalPayerAmount = 0;
                entityHd.TotalAmount = 0;
                entityHd.DraftTransactionNo = BusinessLayer.GenerateTransactionNo(entityHd.DraftTransactionCode, entityHd.DraftTransactionDate, ctx);
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
                DraftPatientChargesHd entity = BusinessLayer.GetDraftPatientChargesHd(Convert.ToInt32(hdnTransactionHdID.Value));
                if (AppSession.IsAdminCanCancelAllTransaction)
                {
                    if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN || entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                    {
                        entity.ReferenceNo = txtReferenceNo.Text;
                        entity.Remarks = txtRemarks.Text;
                        entity.DraftTransactionDate = Helper.GetDatePickerValue(Request.Form[txtTransactionDate.UniqueID]);
                        entity.DraftTransactionTime = Request.Form[txtTransactionTime.UniqueID];
                        BusinessLayer.UpdateDraftPatientChargesHd(entity);
                        return true;
                    }
                    else
                    {
                        errMessage = "Draft Transaksi " + entity.DraftTransactionNo + " Sudah Diproses. Tidak Bisa Diubah";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        return false;
                    }
                }
                else
                {
                    if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        entity.ReferenceNo = txtReferenceNo.Text;
                        entity.Remarks = txtRemarks.Text;
                        entity.DraftTransactionDate = Helper.GetDatePickerValue(Request.Form[txtTransactionDate.UniqueID]);
                        entity.DraftTransactionTime = Request.Form[txtTransactionTime.UniqueID];
                        BusinessLayer.UpdateDraftPatientChargesHd(entity);
                        return true;
                    }
                    else
                    {
                        errMessage = "Draft Transaksi " + entity.DraftTransactionNo + " Sudah Diproses. Tidak Bisa Diubah";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        return false;
                    }
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
        //        if (entity.PatientBillingID == null && (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN || entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL))
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

        #region Proposed Entity
        protected override bool OnProposeRecord(ref string errMessage)
        {
            bool result = true;
            Int32 TransactionID = Convert.ToInt32(hdnTransactionHdID.Value);
            IDbContext ctx = DbFactory.Configure(true);
            DraftPatientChargesHdDao entityHdDao = new DraftPatientChargesHdDao(ctx);
            DraftPatientChargesDtDao entityDtDao = new DraftPatientChargesDtDao(ctx);
            //ChargesStatusLogDao logDao = new ChargesStatusLogDao(ctx);

            try
            {
                //ChargesStatusLog log = new ChargesStatusLog();
                //string statusOld = "", statusNew = "";
                DraftPatientChargesHd entity = entityHdDao.Get(TransactionID);
                if (entity.PatientBillingID == null && entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {

                    bool isProcessCharges = true;
                    string referenceNo = string.Empty;
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                    {
                        //if (AppSession.IsBridgingToRIS)
                        //{
                        //    var processResult = SendOrderToRIS(0, Convert.ToInt32(hdnTransactionHdID.Value));

                        //    string[] resultInfo = ((string)processResult).Split('|');
                        //    isProcessCharges = resultInfo[0] == "1";
                        //    referenceNo = resultInfo[1];
                        //    if (!isProcessCharges)
                        //        errMessage = resultInfo[1];
                        //}
                    }

                    if (isProcessCharges)
                    {
                        Int32 transactionID = Convert.ToInt32(hdnTransactionHdID.Value);

                        DraftPatientChargesHd oChargesHD = BusinessLayer.GetDraftPatientChargesHd(transactionID);
                        //statusOld = oChargesHD.GCTransactionStatus;
                        if (!string.IsNullOrEmpty(referenceNo))
                            oChargesHD.ReferenceNo = referenceNo;
                        oChargesHD.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                        oChargesHD.LastUpdatedBy = AppSession.UserLogin.UserID;
                        //statusNew = oChargesHD.GCTransactionStatus;
                        entityHdDao.Update(oChargesHD);

                        //statusNew = entity.GCTransactionStatus;

                        //log.VisitID = entity.VisitID;
                        //log.TransactionID = entity.TransactionID;
                        //log.LogDate = DateTime.Now;
                        //log.UserID = AppSession.UserLogin.UserID;
                        //log.GCTransactionStatusOLD = statusOld;
                        //log.GCTransactionStatusNEW = statusNew;
                        //logDao.Insert(log);

                        List<DraftPatientChargesDt> lstPatientChargesDt = BusinessLayer.GetDraftPatientChargesDtList(string.Format("TransactionID = {0}  AND IsDeleted = 0 AND GCTransactionDetailStatus <> '{1}'", oChargesHD.TransactionID, Constant.TransactionStatus.VOID), ctx); // AND LocationID IS NOT NULL AND IsApproved = 0
                        foreach (DraftPatientChargesDt patientChargesDt in lstPatientChargesDt)
                        {
                            if ((patientChargesDt.LocationID != null && patientChargesDt.LocationID != 0) && !patientChargesDt.IsApproved)
                            {
                                patientChargesDt.IsApproved = true;
                            }
                            patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                            patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(patientChargesDt);
                        }
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Draft Transaksi " + entity.DraftTransactionNo + " Sudah Diproses. Tidak Bisa Diubah";
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
            string[] param = type.Split(';');
            string gcDeleteReason = param[1];
            string reason = param[2];
            bool result = true;

            if (param[0] == "void")
            {
                IDbContext ctx = DbFactory.Configure(true);
                DraftTestOrderHdDao orderHdDao = new DraftTestOrderHdDao(ctx);
                DraftTestOrderDtDao orderDtDao = new DraftTestOrderDtDao(ctx);
                DraftPatientChargesHdDao entityHdDao = new DraftPatientChargesHdDao(ctx);
                DraftPatientChargesDtDao entityDtDao = new DraftPatientChargesDtDao(ctx);
                //ChargesStatusLogDao logDao = new ChargesStatusLogDao(ctx);
                try
                {
                    //ChargesStatusLog log = new ChargesStatusLog();
                    //string statusOld = "", statusNew = "";
                    Int32 TransactionID = Convert.ToInt32(hdnTransactionHdID.Value);
                    DraftPatientChargesHd entity = entityHdDao.Get(TransactionID);
                    //statusOld = entity.GCTransactionStatus;
                    if (AppSession.IsAdminCanCancelAllTransaction)
                    {
                        if (entity.PatientBillingID == null && (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN || entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL))
                        {
                            ((AppointmentDraftTransactionDetailServiceCtl)ctlService).OnVoidAllChargesDt(ctx, TransactionID);
                            ((AppointmentDraftTransactionDetailDrugMSCtl)ctlDrugMS).OnVoidAllChargesDt(ctx, TransactionID);
                            ((AppointmentDraftTransactionDetailLogisticCtl)ctlLogistic).OnVoidAllChargesDt(ctx, TransactionID);

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

                            List<DraftPatientChargesDt> lstPatientChargesDt = BusinessLayer.GetDraftPatientChargesDtList(string.Format("TransactionID = {0} AND IsDeleted = 0 AND GCTransactionDetailStatus <> '{1}'", entity.TransactionID, Constant.TransactionStatus.VOID), ctx);
                            foreach (DraftPatientChargesDt patientChargesDt in lstPatientChargesDt)
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
                            if (entity.DraftTestOrderID != null)
                            {
                                DraftTestOrderHd orderHd = orderHdDao.Get((int)entity.DraftTestOrderID);
                                if (orderHd.GCTransactionStatus != Constant.TransactionStatus.VOID)
                                {
                                    if (hdnIsDischarges.Value == "1")
                                    {
                                        orderHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                        orderHd.GCVoidReason = Constant.DeleteReason.OTHER;
                                        orderHd.VoidReason = "Linked transaction was deleted";
                                    }
                                    else
                                    {
                                        orderHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                    }
                                    orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    orderHdDao.Update(orderHd);

                                    List<DraftTestOrderDt> lstTestOrderDt = BusinessLayer.GetDraftTestOrderDtList(string.Format("DraftTestOrderID = {0} AND IsDeleted = 0", entity.DraftTestOrderID.ToString()), ctx);
                                    foreach (DraftTestOrderDt orderDt in lstTestOrderDt)
                                    {
                                        if (orderDt != null && orderDt.GCDraftTestOrderStatus != Constant.TestOrderStatus.CANCELLED)
                                        {
                                            if (!orderDt.IsDeleted)
                                            {
                                                if (hdnIsDischarges.Value == "1")
                                                {
                                                    orderDt.GCDraftTestOrderStatus = Constant.TestOrderStatus.CANCELLED;
                                                    orderDt.GCVoidReason = Constant.DeleteReason.OTHER;
                                                    orderDt.VoidReason = "Linked transaction was deleted";
                                                }
                                                else
                                                {
                                                    orderDt.GCDraftTestOrderStatus = Constant.TestOrderStatus.OPEN;
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
                            errMessage = "Draft Transaksi " + entity.DraftTransactionNo + " Sudah Diproses. Tidak Bisa Diubah";
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                    }
                    else
                    {
                        if (entity.PatientBillingID == null && entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                        {
                            ((AppointmentDraftTransactionDetailServiceCtl)ctlService).OnVoidAllChargesDt(ctx, TransactionID);
                            ((AppointmentDraftTransactionDetailDrugMSCtl)ctlDrugMS).OnVoidAllChargesDt(ctx, TransactionID);
                            ((AppointmentDraftTransactionDetailLogisticCtl)ctlLogistic).OnVoidAllChargesDt(ctx, TransactionID);

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

                            //statusNew = entity.GCTransactionStatus;

                            //log.VisitID = entity.VisitID;
                            //log.TransactionID = entity.TransactionID;
                            //log.LogDate = DateTime.Now;
                            //log.UserID = AppSession.UserLogin.UserID;
                            //log.GCTransactionStatusOLD = statusOld;
                            //log.GCTransactionStatusNEW = statusNew;
                            //logDao.Insert(log);

                            //Update Status TestOrderHd
                            if (entity.DraftTestOrderID != null)
                            {
                                DraftTestOrderHd orderHd = orderHdDao.Get((int)entity.DraftTestOrderID);
                                if (orderHd.GCTransactionStatus != Constant.TransactionStatus.VOID)
                                {
                                    if (hdnIsDischarges.Value == "1")
                                    {
                                        orderHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                        orderHd.GCVoidReason = Constant.DeleteReason.OTHER;
                                        orderHd.VoidReason = "Linked transaction was deleted";
                                    }
                                    else
                                    {
                                        orderHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                    }
                                    orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    orderHdDao.Update(orderHd);

                                    List<DraftTestOrderDt> lstTestOrderDt = BusinessLayer.GetDraftTestOrderDtList(string.Format("DraftTestOrderID = {0} AND IsDeleted = 0", entity.DraftTestOrderID.ToString()), ctx);
                                    foreach (DraftTestOrderDt orderDt in lstTestOrderDt)
                                    {
                                        if (orderDt != null && orderDt.GCDraftTestOrderStatus != Constant.TestOrderStatus.CANCELLED)
                                        {
                                            if (!orderDt.IsDeleted)
                                            {
                                                if (hdnIsDischarges.Value == "1")
                                                {
                                                    orderDt.GCDraftTestOrderStatus = Constant.TestOrderStatus.CANCELLED;
                                                    orderDt.GCVoidReason = Constant.DeleteReason.OTHER;
                                                    orderDt.VoidReason = "Linked transaction was deleted";
                                                }
                                                else
                                                {
                                                    orderDt.GCDraftTestOrderStatus = Constant.TestOrderStatus.OPEN;
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
                            errMessage = "Draft Transaksi " + entity.DraftTransactionNo + " Sudah Diproses. Tidak Bisa Diubah";
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
    }
}
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
    public partial class AppointmentSummaryDraftCharges : AppointmentBasePageTrxPatientManagement
    {
        public override string OnGetMenuCode()
        {
            string id = Page.Request.QueryString["id"];
            switch (id)
            {
                case "er": return Constant.MenuCode.Inpatient.BILL_SUMMARY_CHARGES_EMERGENCY;
                case "op": return Constant.MenuCode.Outpatient.DRAFT_CHARGES_ENTRY;
                case "is": return Constant.MenuCode.Inpatient.BILL_SUMMARY_IMAGING;
                case "md": return Constant.MenuCode.Inpatient.BILL_SUMMARY_MEDICAL_DIAGNOSTIC;
                case "bs": return Constant.MenuCode.BillingManagement.BILL_SUMMARY_CHARGES;
                default: return Constant.MenuCode.Inpatient.BILL_SUMMARY_CHARGES;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            if (hdnGCAppointmentStatus.Value != Constant.AppointmentStatus.DELETED && hdnGCAppointmentStatus.Value != Constant.AppointmentStatus.COMPLETE)
            {
                IsAllowAdd = true;
            }
            else {
                IsAllowAdd = false;
            }
        }

        public override string GetGCCustomerType()
        {
            return hdnGCCustomerType.Value;
        }

        public override string GetAppointmentStatus()
        {
            return hdnGCAppointmentStatus.Value;
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
            if (hdnHealthcareServiceUnitID.Value == "")
                return 0;
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

        protected string GetServiceUnitLabel()
        {
            if (Page.Request.QueryString["id"] == "md")
                return GetLabel("Penunjang Medis");
            else if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
                return GetLabel("Ruang Perawatan");
            return GetLabel("Klinik");
        }


        protected string GetServiceUnitFilterFilterExpression()
        {
            if (Page.Request.QueryString["id"] == "md")
                return string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND HealthcareServiceUnitID NOT IN ({2},{3},{4}) AND IsDeleted = 0 AND IsUsingRegistration = 1", AppSession.UserLogin.HealthcareID, Constant.Facility.DIAGNOSTIC, hdnHSUImagingID.Value, hdnHSULaboratoryID.Value, hdnHealthcareServiceUnitAppointmentID.Value);
            vAppointment entity = BusinessLayer.GetvAppointmentList(string.Format("AppointmentID = {0}", Request.QueryString["appid"]))[0];
            return string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsDeleted = 0 AND IsUsingRegistration = 1", AppSession.UserLogin.HealthcareID, Constant.Facility.OUTPATIENT);
        }

        protected override void InitializeDataControl()
        {
            vAppointment entity = BusinessLayer.GetvAppointmentList(string.Format("AppointmentID = {0}", Request.QueryString["appid"]))[0];
            hdnHealthcareServiceUnitAppointmentID.Value = Convert.ToString(entity.HealthcareServiceUnitID);
            hdnAppointmentPhysicianID.Value = entity.ParamedicID.ToString();
            hdnGCAppointmentStatus.Value = entity.GCAppointmentStatus;
            hdnAppointmentID.Value = entity.AppointmentID.ToString();
            hdnHealthcareID.Value = AppSession.UserLogin.HealthcareID;
            hdnLocationID.Value = entity.LocationID.ToString();
            hdnLogisticLocationID.Value = entity.LogisticLocationID.ToString();
            //hdnBusinessPartnerID.Value = entity.BusinessPartnerID.ToString();
            //hdnGCCustomerType.Value = entity.GCCustomerType;
            //hdnClassID.Value = entity.ChargeClassID.ToString();
            hdnVisitDepartmentID.Value = hdnDepartmentID.Value = entity.DepartmentID;

            if (Page.Request.QueryString["id"] == "er")
            {
                vHealthcareServiceUnit hsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = {0} AND DepartmentID = '{1}' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, Constant.Facility.EMERGENCY)).FirstOrDefault();
                hdnDefaultHealthcareServiceUnitID.Value = hdnHealthcareServiceUnitID.Value = hsu.HealthcareServiceUnitID.ToString();
                hdnDefaultLocationID.Value = hdnLocationID.Value = hsu.LocationID.ToString();
                hdnDefaultLogisticLocationID.Value = hdnLogisticLocationID.Value = hsu.LogisticLocationID.ToString();
                hdnDepartmentID.Value = Constant.Facility.EMERGENCY;
                IsLoadFirstRecord = (OnGetRowCount() > 0);
                hdnTransactionHdID.Value = "0";
                ((AppointmentDraftTransactionDetailServiceCtl)ctlService).OnAddRecord();
                ((AppointmentDraftTransactionDetailDrugMSCtl)ctlDrugMS).OnAddRecord();
                ((AppointmentDraftTransactionDetailLogisticCtl)ctlLogistic).OnAddRecord();
            }
            else if (Page.Request.QueryString["id"] == "op")
            {
                vHealthcareServiceUnit hsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = {0} AND DepartmentID = '{1}' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, Constant.Facility.OUTPATIENT)).FirstOrDefault();
                hdnDefaultHealthcareServiceUnitID.Value = hdnHealthcareServiceUnitID.Value = hsu.HealthcareServiceUnitID.ToString();
                hdnDefaultLocationID.Value = hdnLocationID.Value = "0";
                hdnDefaultLogisticLocationID.Value = hdnLogisticLocationID.Value = "0";
                trServiceUnit.Attributes.Remove("style");
                hdnDepartmentID.Value = Constant.Facility.OUTPATIENT;
                IsLoadFirstRecord = (OnGetRowCount() > 0);
                hdnTransactionHdID.Value = "0";
                ((AppointmentDraftTransactionDetailServiceCtl)ctlService).OnAddRecord();
                ((AppointmentDraftTransactionDetailDrugMSCtl)ctlDrugMS).OnAddRecord();
                ((AppointmentDraftTransactionDetailLogisticCtl)ctlLogistic).OnAddRecord();
            }
            else if (Page.Request.QueryString["id"] == "is")
            {
                SettingParameter settingParameter = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID);
                string imagingServiceUnitID = settingParameter.ParameterValue;
                vHealthcareServiceUnit hsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = {0} AND ServiceUnitID = {1}", AppSession.UserLogin.HealthcareID, imagingServiceUnitID)).FirstOrDefault();
                hdnDefaultHealthcareServiceUnitID.Value = hdnHealthcareServiceUnitID.Value = hsu.HealthcareServiceUnitID.ToString();
                hdnDefaultLocationID.Value = hdnLocationID.Value = hsu.LocationID.ToString();
                hdnDefaultLogisticLocationID.Value = hdnLogisticLocationID.Value = hsu.LogisticLocationID.ToString();
                hdnDepartmentID.Value = Constant.Facility.DIAGNOSTIC;
                IsLoadFirstRecord = (OnGetRowCount() > 0);
                hdnTransactionHdID.Value = "0";
                ((AppointmentDraftTransactionDetailServiceCtl)ctlService).OnAddRecord();
                ((AppointmentDraftTransactionDetailDrugMSCtl)ctlDrugMS).OnAddRecord();
                ((AppointmentDraftTransactionDetailLogisticCtl)ctlLogistic).OnAddRecord();
            }
            else if (Page.Request.QueryString["id"] == "md")
            {
                List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID, Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID));
                string laboratoryID = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).ParameterValue;
                string imagingID = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID).ParameterValue;
                List<vHealthcareServiceUnit> lstHealthcareServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND ServiceUnitID IN ({1},{2}) AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, laboratoryID, imagingID));
                hdnHSUImagingID.Value = lstHealthcareServiceUnit.FirstOrDefault(p => p.ServiceUnitID == Convert.ToInt32(imagingID)).HealthcareServiceUnitID.ToString();
                hdnHSULaboratoryID.Value = lstHealthcareServiceUnit.FirstOrDefault(p => p.ServiceUnitID == Convert.ToInt32(laboratoryID)).HealthcareServiceUnitID.ToString();
                hdnDefaultLocationID.Value = hdnLocationID.Value = "0";
                hdnDefaultLogisticLocationID.Value = hdnLogisticLocationID.Value = "0";
                trServiceUnit.Attributes.Remove("style");
                hdnDepartmentID.Value = Constant.Facility.DIAGNOSTIC;
                IsLoadFirstRecord = (OnGetRowCount() > 0);
                hdnTransactionHdID.Value = "0";

                ((AppointmentDraftTransactionDetailServiceCtl)ctlService).OnAddRecord();
                ((AppointmentDraftTransactionDetailDrugMSCtl)ctlDrugMS).OnAddRecord();
                ((AppointmentDraftTransactionDetailLogisticCtl)ctlLogistic).OnAddRecord();
            }
            else
            {
                //hdnDefaultLocationID.Value = hdnLocationID.Value = entity.LocationID.ToString();
                //hdnDefaultLogisticLocationID.Value = hdnLogisticLocationID.Value = entity.LogisticLocationID.ToString();
                hdnDefaultHealthcareServiceUnitID.Value = hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
                //hdnPhysicianID.Value = entity.ParamedicID.ToString();
                //hdnPhysicianCode.Value = entity.ParamedicCode;
                //hdnPhysicianName.Value = entity.ParamedicName;
                IsLoadFirstRecord = (OnGetRowCount() > 0);
                hdnTransactionHdID.Value = "0";

                ((AppointmentDraftTransactionDetailServiceCtl)ctlService).OnAddRecord();
                ((AppointmentDraftTransactionDetailDrugMSCtl)ctlDrugMS).OnAddRecord();
                ((AppointmentDraftTransactionDetailLogisticCtl)ctlLogistic).OnAddRecord();
            }

            //if (Page.Request.QueryString["id"] != null && hdnHealthcareServiceUnitID.Value != "")
            //{
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
            //}

            if (hdnHealthcareServiceUnitID.Value != "")
            {
                int count = BusinessLayer.GetServiceUnitParamedicRowCount(string.Format("HealthcareServiceUnitID = {0}", hdnHealthcareServiceUnitID.Value));
                if (count > 0)
                    hdnIsHealthcareServiceUnitHasParamedic.Value = "1";
                else
                    hdnIsHealthcareServiceUnitHasParamedic.Value = "0";
            }

            if (hdnDepartmentID.Value == Constant.Facility.INPATIENT || hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
            {
                trServiceUnit.Attributes.Remove("style");
                hdnTransactionHdID.Value = "0";
                ((AppointmentDraftTransactionDetailServiceCtl)ctlService).OnAddRecord();
                ((AppointmentDraftTransactionDetailDrugMSCtl)ctlDrugMS).OnAddRecord();
                ((AppointmentDraftTransactionDetailLogisticCtl)ctlLogistic).OnAddRecord();
            }

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
            SettingParameter settingParameterLB = BusinessLayer.GetSettingParameter(Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID);
            SettingParameter settingParameterIS = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID);
            serviceUnitID = string.Format("{0}, {1}", settingParameterLB.ParameterValue, settingParameterIS.ParameterValue);
            string filterHSU = string.Format("HealthcareID = {0} AND DepartmentID = '{1}' AND ServiceUnitID NOT IN ({2})",
                                    AppSession.UserLogin.HealthcareID, hdnDepartmentID.Value, serviceUnitID);
            if (hdnHealthcareServiceUnitID.Value != "" && hdnHealthcareServiceUnitID.Value != "0")
            {
                filterHSU += string.Format(" AND HealthcareServiceUnitID = {0}", hdnHealthcareServiceUnitID.Value);
            }
            vHealthcareServiceUnit hsu = BusinessLayer.GetvHealthcareServiceUnitList(filterHSU).FirstOrDefault();
            hdnHealthcareServiceUnitID.Value = hsu.HealthcareServiceUnitID.ToString();
            txtServiceUnitCode.Text = hsu.ServiceUnitCode;
            txtServiceUnitName.Text = hsu.ServiceUnitName;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnTransactionHdID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtTransactionNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtTransactionDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtTransactionTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(txtReferenceNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(hdnHealthcareServiceUnitID, new ControlEntrySetting(false, false, false, hdnDefaultHealthcareServiceUnitID.Value));
            SetControlEntrySetting(hdnLocationID, new ControlEntrySetting(false, false, false, hdnDefaultLocationID.Value));
            SetControlEntrySetting(hdnLogisticLocationID, new ControlEntrySetting(false, false, false, hdnDefaultLogisticLocationID.Value));
            SetControlEntrySetting(txtServiceUnitCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtServiceUnitName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblServiceUnit, new ControlEntrySetting(true, false));
        }

        public override void OnAddRecord()
        {
            string id = Page.Request.QueryString["id"];
            if (id == "md" || id == "op" || hdnDepartmentID.Value == Constant.Facility.INPATIENT || hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
                trServiceUnit.Attributes.Remove("style");

            lblServiceUnit.Attributes.Remove("class");
            lblServiceUnit.Attributes.Add("class", "lblLink lblMandatory");
            hdnGCAppointmentStatus.Value = Constant.TransactionStatus.OPEN;
            ((AppointmentDraftTransactionDetailServiceCtl)ctlService).OnAddRecord();
            ((AppointmentDraftTransactionDetailDrugMSCtl)ctlDrugMS).OnAddRecord();
            ((AppointmentDraftTransactionDetailLogisticCtl)ctlLogistic).OnAddRecord();
        }

        #region Load Entity
        protected string GetFilterExpression()
        {
            string filterExpression = "";
            string id = Page.Request.QueryString["id"];

            filterExpression = string.Format("AppointmentID = {0}", hdnAppointmentID.Value);
            
            if (id == "md")
                filterExpression += string.Format(" AND DepartmentID = '{0}' AND HealthcareServiceUnitID NOT IN ({1},{2},{3})", Constant.Facility.DIAGNOSTIC, hdnHSUImagingID.Value, hdnHSULaboratoryID.Value, hdnHealthcareServiceUnitAppointmentID.Value);
            else if (id == "op" || hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
                filterExpression += string.Format(" AND DepartmentID = '{0}'", Constant.Facility.OUTPATIENT);
            else if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
                filterExpression += string.Format(" AND DepartmentID = '{0}'", Constant.Facility.INPATIENT);
            else
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", hdnHealthcareServiceUnitID.Value);
            return filterExpression;
        }

        public override int OnGetRowCount()
        {
            string filterExpression = GetFilterExpression();
            return BusinessLayer.GetvDraftPatientChargesHdRowCount(filterExpression);
        }

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
            if (AppSession.IsAdminCanCancelAllTransaction)
            {
                if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN && entity.GCTransactionStatus != Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                {
                    isShowWatermark = true;
                    watermarkText = entity.TransactionStatusWatermark;
                    IsEditable = false;
                }
                else
                {
                    isShowWatermark = false;
                    IsEditable = true;
                }

                if (entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                {
                    IsAllowProposed = false;
                }
                else if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    IsAllowProposed = true;
                }
                else
                {
                    IsAllowProposed = false;
                }
            }
            else
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
            lblServiceUnit.Attributes.Remove("class");
            lblServiceUnit.Attributes.Add("class", "lblDisabled");
            hdnGCAppointmentStatus.Value = entity.GCTransactionStatus;
            hdnTransactionHdID.Value = entity.TransactionID.ToString();
            txtTransactionNo.Text = entity.DraftTransactionNo;
            txtReferenceNo.Text = entity.ReferenceNo;
            txtRemarks.Text = entity.Remarks;
            txtTransactionDate.Text = entity.DraftTransactionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTransactionTime.Text = entity.DraftTransactionTime;
            hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
            string id = Page.Request.QueryString["id"];
            if (id == "md" || id == "op" || hdnDepartmentID.Value == Constant.Facility.INPATIENT || hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
            {
                trServiceUnit.Attributes.Remove("style");
                txtServiceUnitCode.Text = entity.ServiceUnitCode;
                txtServiceUnitName.Text = entity.ServiceUnitName;
                hdnLocationID.Value = entity.LocationID.ToString();
                hdnLogisticLocationID.Value = entity.LogisticLocationID.ToString();
            }
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
                            if (hdnVisitDepartmentID.Value == Constant.Facility.DIAGNOSTIC && AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                                entityHd.DraftTransactionCode = Constant.TransactionCode.DRAFT_IMAGING_CHARGES;
                            else
                                entityHd.DraftTransactionCode = Constant.TransactionCode.DRAFT_OTHER_DIAGNOSTIC_CHARGES;
                        }; break;
                    default: entityHd.DraftTransactionCode = Constant.TransactionCode.DRAFT_OP_CHARGES; break;
                }
                entityHd.DraftTransactionDate = Helper.GetDatePickerValue(Request.Form[txtTransactionDate.UniqueID]);
                entityHd.DraftTransactionTime = Request.Form[txtTransactionTime.UniqueID];
                entityHd.PatientBillingID = null;
                entityHd.Remarks = txtRemarks.Text;
                entityHd.ReferenceNo = txtReferenceNo.Text;
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                entityHd.GCVoidReason = null;
                entityHd.DraftTransactionNo = BusinessLayer.GenerateTransactionNo(entityHd.DraftTransactionCode, entityHd.DraftTransactionDate, ctx);
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
            DraftPatientChargesHdDao entityHdDao = new DraftPatientChargesHdDao(ctx);
            try
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
                            if (hdnVisitDepartmentID.Value == Constant.Facility.DIAGNOSTIC && AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                                entityHd.DraftTransactionCode = Constant.TransactionCode.DRAFT_IMAGING_CHARGES;
                            else
                                entityHd.DraftTransactionCode = Constant.TransactionCode.DRAFT_OTHER_DIAGNOSTIC_CHARGES;
                        }; break;
                    default: entityHd.DraftTransactionCode = Constant.TransactionCode.DRAFT_OP_CHARGES; break;
                }
                entityHd.DraftTransactionDate = Helper.GetDatePickerValue(Request.Form[txtTransactionDate.UniqueID]);
                entityHd.DraftTransactionTime = Request.Form[txtTransactionTime.UniqueID];
                entityHd.PatientBillingID = null;
                entityHd.Remarks = txtRemarks.Text;
                entityHd.ReferenceNo = txtReferenceNo.Text;
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
                        errMessage = "Transaksi " + entity.DraftTransactionNo + " Sudah Diproses. Tidak Bisa Diubah";
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
        //            errMessage = "Transaksi Sudah Diproses. Tidak Bisa Dibatalkan";
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

            try
            {
                DraftPatientChargesHd entity = entityHdDao.Get(TransactionID);
                if (entity.PatientBillingID == null && entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {

                    bool isProcessCharges = true;
                    string referenceNo = string.Empty;
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

                        List<DraftPatientChargesDt> lstDraftPatientChargesDt = BusinessLayer.GetDraftPatientChargesDtList(string.Format("TransactionID = {0}  AND IsDeleted = 0 AND GCTransactionDetailStatus <> '{1}'", oChargesHD.TransactionID, Constant.TransactionStatus.VOID), ctx); // AND LocationID IS NOT NULL AND IsApproved = 0
                        foreach (DraftPatientChargesDt patientChargesDt in lstDraftPatientChargesDt)
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
                    errMessage = "Transaksi " + entity.DraftTransactionNo + " Sudah Diproses. Tidak Bisa Diubah";
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
                DraftServiceOrderHdDao serviceHdDao = new DraftServiceOrderHdDao(ctx);
                DraftServiceOrderDtDao serviceDtDao = new DraftServiceOrderDtDao(ctx);
                DraftPatientChargesHdDao entityHdDao = new DraftPatientChargesHdDao(ctx);
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

                            //statusNew = entity.GCTransactionStatus;

                            //log.VisitID = entity.VisitID;
                            //log.TransactionID = entity.TransactionID;
                            //log.LogDate = DateTime.Now;
                            //log.UserID = AppSession.UserLogin.UserID;
                            //log.GCTransactionStatusOLD = statusOld;
                            //log.GCTransactionStatusNEW = statusNew;
                            //logDao.Insert(log);

                            //Update Status ServiceOrder
                            if (entity.DraftServiceOrderID != null)
                            {
                                DraftServiceOrderHd orderHd = serviceHdDao.Get((int)entity.DraftServiceOrderID);
                                if (orderHd.GCTransactionStatus != Constant.TransactionStatus.VOID)
                                {
                                    if (hdnIsDischarges.Value == "1")
                                    {
                                        orderHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                    }
                                    else
                                    {
                                        orderHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                    }
                                    orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    serviceHdDao.Update(orderHd);

                                    List<DraftServiceOrderDt> lstDt = BusinessLayer.GetDraftServiceOrderDtList(string.Format("DraftServiceOrderID = {0} AND IsDeleted = 0", entity.DraftServiceOrderID.ToString()), ctx);
                                    foreach (DraftServiceOrderDt orderDt in lstDt)
                                    {
                                        if (orderDt != null && orderDt.GCDraftServiceOrderStatus != Constant.TestOrderStatus.CANCELLED)
                                        {
                                            if (!orderDt.IsDeleted)
                                            {
                                                if (hdnIsDischarges.Value == "1")
                                                {
                                                    orderDt.GCDraftServiceOrderStatus = Constant.TestOrderStatus.CANCELLED;
                                                    orderDt.GCVoidReason = Constant.DeleteReason.OTHER;
                                                    orderDt.VoidReason = "Linked transaction was deleted";
                                                }
                                                else
                                                {
                                                    orderDt.GCDraftServiceOrderStatus = Constant.TestOrderStatus.OPEN;
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
                            errMessage = "Transaksi " + entity.DraftTransactionNo + " Sudah Diproses. Tidak Bisa Diubah";
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

                            //Update Status ServiceOrder
                            if (entity.DraftServiceOrderID != null)
                            {
                                DraftServiceOrderHd orderHd = serviceHdDao.Get((int)entity.DraftServiceOrderID);
                                if (orderHd.GCTransactionStatus != Constant.TransactionStatus.VOID)
                                {
                                    if (hdnIsDischarges.Value == "1")
                                    {
                                        orderHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                    }
                                    else
                                    {
                                        orderHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                    }
                                    orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    serviceHdDao.Update(orderHd);

                                    List<DraftServiceOrderDt> lstDt = BusinessLayer.GetDraftServiceOrderDtList(string.Format("DraftServiceOrderID = {0} AND IsDeleted = 0", entity.DraftServiceOrderID.ToString()), ctx);
                                    foreach (DraftServiceOrderDt orderDt in lstDt)
                                    {
                                        if (orderDt != null && orderDt.GCDraftServiceOrderStatus != Constant.TestOrderStatus.CANCELLED)
                                        {
                                            if (!orderDt.IsDeleted)
                                            {
                                                if (hdnIsDischarges.Value == "1")
                                                {
                                                    orderDt.GCDraftServiceOrderStatus = Constant.TestOrderStatus.CANCELLED;
                                                    orderDt.GCVoidReason = Constant.DeleteReason.OTHER;
                                                    orderDt.VoidReason = "Linked transaction was deleted";
                                                }
                                                else
                                                {
                                                    orderDt.GCDraftServiceOrderStatus = Constant.TestOrderStatus.OPEN;
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
                            errMessage = "Transaksi " + entity.DraftTransactionNo + " Sudah Diproses. Tidak Bisa Diubah";
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
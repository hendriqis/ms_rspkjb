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
    public partial class PatientBillSummaryTestOrder : BasePageTrxPatientManagement
    {
        vConsultVisit2 entityCV = null;

        public override string OnGetMenuCode()
        {
            string id = Page.Request.QueryString["id"];
            switch (hdnVisitDepartmentID.Value)
            {
                case Constant.Facility.INPATIENT:
                    switch (id)
                    {
                        case "lb": return Constant.MenuCode.Inpatient.BILL_SUMMARY_LABORATORY;
                        case "is": return Constant.MenuCode.Inpatient.BILL_SUMMARY_IMAGING;
                        case "md": return Constant.MenuCode.Inpatient.BILL_SUMMARY_MEDICAL_DIAGNOSTIC;
                        default: return Constant.MenuCode.Inpatient.BILL_SUMMARY_CHARGES;
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
            //////IsAllowAdd = (hdnGCRegistrationStatus.Value != Constant.VisitStatus.CLOSED); // ditutup oleh RN 20191205 : untuk proses reopen billing

            if (AppSession.RegisteredPatient.IsBillingReopen && AppSession.IsUsedReopenBilling)
            {
                IsAllowAdd = (hdnGCRegistrationStatus.Value != Constant.VisitStatus.CANCELLED);
            }
            else
            {
                IsAllowAdd = (hdnGCRegistrationStatus.Value.ToString() != Constant.VisitStatus.CLOSED);
            }

            IsAllowSave = !entityCV.IsLockDown;
            IsAllowVoid = !entityCV.IsLockDown;
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
            if (hdnIsCheckedTransactionCorrection.Value == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool GetIsAIOTransactionHd()
        {
            if (hdnIsCheckedAIOTransaction.Value == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool IsPatientBillSummaryPage()
        {
            return true;
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
            hdnIsAdminCanCancelAllTransaction.Value = AppSession.IsAdminCanCancelAllTransaction ? "1" : "0";
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();

            entityCV = BusinessLayer.GetvConsultVisit2List(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();

            if (entityCV.DischargeDate != null && entityCV.DischargeDate.ToString(Constant.FormatString.DATE_FORMAT) != "01-Jan-1900")
            {
                hdnIsDischarges.Value = "1";
            }
            else
            {
                hdnIsDischarges.Value = "0";
            }

            hdnGCRegistrationStatus.Value = entityCV.GCVisitStatus;
            hdnRegistrationID.Value = entityCV.RegistrationID.ToString();
            hdnRegistrationPhysicianID.Value = hdnPhysicianID.Value = entityCV.ParamedicID.ToString();
            hdnPhysicianCode.Value = entityCV.ParamedicCode;
            hdnPhysicianName.Value = entityCV.ParamedicName;
            hdnLocationID.Value = entityCV.LocationID.ToString();
            hdnLogisticLocationID.Value = entityCV.LogisticLocationID.ToString();
            hdnBusinessPartnerID.Value = entityCV.BusinessPartnerID.ToString();
            hdnGCCustomerType.Value = entityCV.GCCustomerType;
            hdnClassID.Value = entityCV.ChargeClassID.ToString();
            hdnVisitDepartmentID.Value = entityCV.DepartmentID;
            hdnLinkedRegistrationID.Value = entityCV.LinkedRegistrationID.ToString();
            hdnDepartmentID.Value = Constant.Facility.DIAGNOSTIC;

            hdnIsHasAIOPackage.Value = entityCV.IsHasAIOPackage ? "1" : "0";

            if (hdnIsHasAIOPackage.Value == "1")
            {
                trIsAIOTransaction.Attributes.Remove("style");
            }
            else
            {
                trIsAIOTransaction.Attributes.Add("style", "display:none");
            }

            string serviceUnitID = "";
            string requestID = Page.Request.QueryString["id"];
            hdnRequestID.Value = requestID;

            if (requestID == "lb")
            {
                SettingParameter settingParameter = BusinessLayer.GetSettingParameter(Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID);
                serviceUnitID = settingParameter.ParameterValue;
                vHealthcareServiceUnit hsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = {0} AND ServiceUnitID IN ({1})", AppSession.UserLogin.HealthcareID, serviceUnitID)).FirstOrDefault();
                hdnHealthcareServiceUnitID.Value = hsu.HealthcareServiceUnitID.ToString();
                hdnLocationID.Value = hsu.LocationID.ToString();
                hdnLogisticLocationID.Value = hsu.LogisticLocationID.ToString();

                hdnIsLaboratoryUnit.Value = "1";
                trPATest.Style.Remove("display");
            }
            else if (requestID == "is")
            {
                SettingParameter settingParameter = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID);
                serviceUnitID = settingParameter.ParameterValue;
                vHealthcareServiceUnit hsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = {0} AND ServiceUnitID IN ({1})", AppSession.UserLogin.HealthcareID, serviceUnitID)).FirstOrDefault();
                hdnHealthcareServiceUnitID.Value = hsu.HealthcareServiceUnitID.ToString();
                hdnLocationID.Value = hsu.LocationID.ToString();
                hdnLogisticLocationID.Value = hsu.LogisticLocationID.ToString();

                hdnIsLaboratoryUnit.Value = "0";
                trPATest.Style.Add("display", "none");
            }
            else if (requestID == "md")
            {
                SettingParameter settingParameterLB = BusinessLayer.GetSettingParameter(Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID);
                SettingParameter settingParameterIS = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID);
                serviceUnitID = string.Format("{0}, {1}", settingParameterLB.ParameterValue, settingParameterIS.ParameterValue);
                vHealthcareServiceUnit hsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format(
                    "HealthcareID = {0} AND DepartmentID = '{1}' AND ServiceUnitID NOT IN ({2}) AND IsLaboratoryUnit = 0",
                    AppSession.UserLogin.HealthcareID, hdnDepartmentID.Value, serviceUnitID)).FirstOrDefault();
                hdnHealthcareServiceUnitID.Value = hsu.HealthcareServiceUnitID.ToString();
                hdnLocationID.Value = hsu.LocationID.ToString();
                hdnLogisticLocationID.Value = hsu.LogisticLocationID.ToString();

                hdnIsLaboratoryUnit.Value = "0";
                trPATest.Style.Add("display", "none");
            }

            IsLoadFirstRecord = (OnGetRowCount() > 0);

            hdnTransactionHdID.Value = "0";
            int count = BusinessLayer.GetServiceUnitParamedicRowCount(string.Format("HealthcareServiceUnitID = {0}", hdnHealthcareServiceUnitID.Value));
            if (count > 0)
                hdnIsHealthcareServiceUnitHasParamedic.Value = "1";
            else
                hdnIsHealthcareServiceUnitHasParamedic.Value = "0";

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

            ((PatientManagementTransactionDetailServiceCtl)ctlService).OnAddRecord();
            ((PatientManagementTransactionDetailDrugMSCtl)ctlDrugMS).OnAddRecord();
            ((PatientManagementTransactionDetailDrugMSReturnCtl)ctlDrugMSReturn).OnAddRecord();
            ((PatientManagementTransactionDetailLogisticCtl)ctlLogistic).OnAddRecord();

            string moduleName = Helper.GetModuleName();
            string ModuleID = Helper.GetModuleID(moduleName);
            GetUserMenuAccess menu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault();
            string CRUDMode = menu.CRUDMode;
            hdnIsAllowVoid.Value = CRUDMode.Contains("X") ? "1" : "0";
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void SetControlProperties()
        {
            ((PatientManagementTransactionDetailServiceCtl)ctlService).SetControlProperties();
            ((PatientManagementTransactionDetailDrugMSCtl)ctlDrugMS).SetControlProperties();
            ((PatientManagementTransactionDetailLogisticCtl)ctlLogistic).SetControlProperties();

            string serviceUnitID = "";
            if (Page.Request.QueryString["id"] == "lb")
            {
                SettingParameter settingParameter = BusinessLayer.GetSettingParameter(Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID);
                serviceUnitID = settingParameter.ParameterValue;
                vHealthcareServiceUnit hsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = {0} AND IsLaboratoryUnit = 1", AppSession.UserLogin.HealthcareID)).FirstOrDefault();
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
                    "HealthcareID = {0} AND DepartmentID = '{1}' AND ServiceUnitID NOT IN ({2}) AND IsLaboratoryUnit = 0",
                    AppSession.UserLogin.HealthcareID, hdnDepartmentID.Value, serviceUnitID)).FirstOrDefault();
                hdnHealthcareServiceUnitID.Value = hsu.HealthcareServiceUnitID.ToString();
                txtServiceUnitCode.Text = hsu.ServiceUnitCode;
                txtServiceUnitName.Text = hsu.ServiceUnitName;
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnTransactionHdID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(chkIsCorrectionTransaction, new ControlEntrySetting(true, false, false, false));
            chkIsCorrectionTransaction.Checked = false;
            SetControlEntrySetting(hdnIsCheckedTransactionCorrection, new ControlEntrySetting(true, false, false, "0"));
            SetControlEntrySetting(chkIsAutoTransaction, new ControlEntrySetting(false, false, false, false));
            chkIsAutoTransaction.Checked = false;
            SetControlEntrySetting(chkIsChargesGenerateMCU, new ControlEntrySetting(false, false, false, false));
            chkIsChargesGenerateMCU.Checked = false;
            SetControlEntrySetting(chkIsAIOTransaction, new ControlEntrySetting(true, false, false, false));
            chkIsAIOTransaction.Checked = false;
            if (hdnIsHasAIOPackage.Value == "1")
            {
                trIsAIOTransaction.Attributes.Remove("style");
            }
            else
            {
                trIsAIOTransaction.Attributes.Add("style", "display:none");
            }
            SetControlEntrySetting(hdnIsCheckedAIOTransaction, new ControlEntrySetting(true, false, false, "0"));

            SetControlEntrySetting(txtTransactionNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtTransactionDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtTransactionTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(txtReferenceNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(lblServiceUnit, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtServiceUnitCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtServiceUnitName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsPATest, new ControlEntrySetting(true, true, false));
        }

        public override void OnAddRecord()
        {
            hdnGCTransactionStatus.Value = Constant.TransactionStatus.OPEN;

            trIsAutoTransaction.Attributes.Add("style", "display:none");
            trIsChargesGenerateMCU.Attributes.Add("style", "display:none");

            if (hdnIsHasAIOPackage.Value == "1")
            {
                trIsAIOTransaction.Attributes.Remove("style");
            }
            else
            {
                trIsAIOTransaction.Attributes.Add("style", "display:none");
            }

            ((PatientManagementTransactionDetailServiceCtl)ctlService).OnAddRecord();
            ((PatientManagementTransactionDetailDrugMSCtl)ctlDrugMS).OnAddRecord();
            ((PatientManagementTransactionDetailDrugMSReturnCtl)ctlDrugMSReturn).OnAddRecord();
            ((PatientManagementTransactionDetailLogisticCtl)ctlLogistic).OnAddRecord();

            divCreatedBy.InnerHtml = string.Empty;
            divCreatedDate.InnerHtml = string.Empty;
            divLastUpdatedBy.InnerHtml = string.Empty;
            divLastUpdatedDate.InnerHtml = string.Empty;
            divProposedBy.InnerHtml = string.Empty;
            divProposedDate.InnerHtml = string.Empty;
            trProposedBy.Style.Add("display", "none");
            trProposedDate.Style.Add("display", "none");
            divVoidBy.InnerHtml = string.Empty;
            divVoidDate.InnerHtml = string.Empty;
            divVoidReason.InnerHtml = string.Empty;
            trVoidBy.Style.Add("display", "none");
            trVoidDate.Style.Add("display", "none");
            trVoidReason.Style.Add("display", "none");
        }

        protected string GetFilterExpression()
        {
            string serviceUnitID = "", hsuID = "";

            string requestID = Page.Request.QueryString["id"];
            if (requestID == "lb")
            {
                SettingParameter settingParameter = BusinessLayer.GetSettingParameter(Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID);
                serviceUnitID = settingParameter.ParameterValue;

                List<vHealthcareServiceUnit> lsthsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format(
                    "HealthcareID = {0} AND DepartmentID = '{1}' AND IsLaboratoryUnit = 1",
                    AppSession.UserLogin.HealthcareID, hdnDepartmentID.Value, serviceUnitID));
                foreach (vHealthcareServiceUnit hsu in lsthsu)
                {
                    hsuID += "," + hsu.HealthcareServiceUnitID;
                }
                hsuID = hsuID.Substring(1, hsuID.Length - 1);
            }
            else if (requestID == "is")
            {
                SettingParameter settingParameter = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID);
                serviceUnitID = settingParameter.ParameterValue;
                vHealthcareServiceUnit hsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = {0} AND ServiceUnitID IN ({1})", AppSession.UserLogin.HealthcareID, serviceUnitID)).FirstOrDefault();
                hsuID = hsu.HealthcareServiceUnitID.ToString();
            }
            else if (requestID == "md")
            {
                SettingParameter settingParameterLB = BusinessLayer.GetSettingParameter(Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID);
                SettingParameter settingParameterIS = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID);
                serviceUnitID = string.Format("{0}, {1}", settingParameterLB.ParameterValue, settingParameterIS.ParameterValue);
                List<vHealthcareServiceUnit> lsthsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format(
                    "HealthcareID = {0} AND DepartmentID = '{1}' AND ServiceUnitID NOT IN ({2}) AND IsLaboratoryUnit = 0",
                    AppSession.UserLogin.HealthcareID, hdnDepartmentID.Value, serviceUnitID));
                foreach (vHealthcareServiceUnit hsu in lsthsu)
                {
                    hsuID += "," + hsu.HealthcareServiceUnitID;
                }
                hsuID = hsuID.Substring(1, hsuID.Length - 1);
            }

            string filterExpression = "";

            ////if (hdnLinkedRegistrationID.Value != "" && hdnLinkedRegistrationID.Value != "0")
            ////    filterExpression = string.Format("(VisitID = {0} OR (RegistrationID = {1} AND IsChargesTransfered = 1))", hdnVisitID.Value, hdnLinkedRegistrationID.Value);
            ////else
            ////    filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);

            filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);

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
                return string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsDeleted = 0 AND IsLaboratoryUnit = 1",
                    AppSession.UserLogin.HealthcareID, Constant.Facility.DIAGNOSTIC);
            }
            else if (Page.Request.QueryString["id"] == "is")
            {
                return string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND ServiceUnitID IN ({2}) AND IsDeleted = 0",
                    AppSession.UserLogin.HealthcareID, Constant.Facility.DIAGNOSTIC, settingParameterIS.ParameterValue);
            }
            else
            {
                return string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND ServiceUnitID NOT IN ({2}) AND IsDeleted = 0 AND IsLaboratoryUnit = 0",
                    AppSession.UserLogin.HealthcareID, Constant.Facility.DIAGNOSTIC, serviceUnitID);
            }

        }

        public override int OnGetRowCount()
        {
            int registrationID = Convert.ToInt32(hdnRegistrationID.Value);
            int registrationHealthcareServiceUnitID = 0;
            string reqID = Page.Request.QueryString["id"];
            string requestID = "";

            if (reqID == "md")
            {
                requestID = "md";
            }
            else if (reqID == "is")
            {
                requestID = "is";
            }
            else if (reqID == "lb")
            {
                requestID = "lb";
            }
            else if (reqID == "op")
            {
                requestID = "op";
            }
            else if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
            {
                requestID = "ip";
            }
            else if (hdnDepartmentID.Value == Constant.Facility.EMERGENCY)
            {
                requestID = "er";
            }
            else if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
            {
                requestID = "op";
            }
            else if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
            {
                if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                {
                    requestID = "is";
                }
                else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                {
                    requestID = "lb";
                }
                else
                {
                    requestID = "md";
                }
            }

            int hdRowCount = BusinessLayer.GetPatientChargesHdPerRegistrationPerRequestIDRowCount(registrationID, requestID, registrationHealthcareServiceUnitID);
            hdnChargesHdRowCount.Value = hdRowCount.ToString();

            return hdRowCount;
        }

        #region Load Entity
        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            vPatientChargesHd6 entity = BusinessLayer.GetvPatientChargesHd6(filterExpression, PageIndex, " TransactionID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            PageIndex = BusinessLayer.GetvPatientChargesHd6RowIndex(filterExpression, keyValue, "TransactionID DESC");
            vPatientChargesHd6 entity = BusinessLayer.GetvPatientChargesHd6(filterExpression, PageIndex, "TransactionID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected bool IsEditable = true;
        private void EntityToControl(vPatientChargesHd6 entity, ref bool isShowWatermark, ref string watermarkText)
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

                    SetControlEntrySetting(chkIsPATest, new ControlEntrySetting(false, false, false));
                }
                else
                {
                    isShowWatermark = false;
                    IsEditable = true;
                    IsAllowProposed = true;

                    SetControlEntrySetting(chkIsPATest, new ControlEntrySetting(true, true, false));
                }
            }

            hdnGCTransactionStatus.Value = entity.GCTransactionStatus;
            hdnTransactionHdID.Value = entity.TransactionID.ToString();
            txtTransactionNo.Text = entity.TransactionNo;
            txtReferenceNo.Text = entity.ReferenceNo;
            chkIsCorrectionTransaction.Checked = entity.IsCorrectionTransaction;
            hdnIsCheckedTransactionCorrection.Value = entity.IsCorrectionTransaction ? "1" : "0";
            txtServiceUnitCode.Text = entity.ServiceUnitCode;
            txtServiceUnitName.Text = entity.ServiceUnitName;
            txtRemarks.Text = entity.Remarks;
            txtTransactionDate.Text = entity.TransactionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTransactionTime.Text = entity.TransactionTime;
            hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
            chkIsPATest.Checked = entity.IsPathologicalAnatomyTest;

            bool flagHaveCharges = false;
            if (entity != null) flagHaveCharges = true;

            chkIsAutoTransaction.Checked = entity.IsAutoTransaction;
            if (entity.IsAutoTransaction)
            {
                trIsAutoTransaction.Attributes.Remove("style");
            }
            else
            {
                trIsAutoTransaction.Attributes.Add("style", "display:none");
            }

            if (entity.ConsultVisitItemPackageID != null && entity.ConsultVisitItemPackageID != 0)
            {
                chkIsChargesGenerateMCU.Checked = true;
                trIsChargesGenerateMCU.Attributes.Remove("style");
            }
            else
            {
                chkIsChargesGenerateMCU.Checked = false;
                trIsChargesGenerateMCU.Attributes.Add("style", "display:none");
            }

            chkIsAIOTransaction.Checked = entity.IsAIOTransaction;
            hdnIsCheckedAIOTransaction.Value = entity.IsAIOTransaction ? "1" : "0";
            if (hdnIsHasAIOPackage.Value == "1")
            {
                trIsAIOTransaction.Attributes.Remove("style");
            }
            else
            {
                trIsAIOTransaction.Attributes.Add("style", "display:none");
            }

            ((PatientManagementTransactionDetailServiceCtl)ctlService).InitializeTransactionControl(flagHaveCharges);
            ((PatientManagementTransactionDetailDrugMSCtl)ctlDrugMS).InitializeTransactionControl(flagHaveCharges);
            ((PatientManagementTransactionDetailDrugMSReturnCtl)ctlDrugMSReturn).InitializeTransactionControl(flagHaveCharges);
            ((PatientManagementTransactionDetailLogisticCtl)ctlLogistic).InitializeTransactionControl(flagHaveCharges);

            divCreatedBy.InnerHtml = entity.CreatedByName;
            divCreatedDate.InnerHtml = entity.CreatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            divLastUpdatedBy.InnerHtml = entity.LastUpdatedByName;
            if (entity.LastUpdatedDate != null && entity.LastUpdatedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
            {
                divLastUpdatedDate.InnerHtml = entity.LastUpdatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }

            if (entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
            {
                divProposedBy.InnerHtml = entity.ProposedByName;
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
                divVoidBy.InnerHtml = entity.VoidByName;
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

        }
        #endregion

        #region Save Entity
        public override void SaveTransactionHeader(IDbContext ctx, ref int transactionID)
        {
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesHdInfoDao entityHdInfoDao = new PatientChargesHdInfoDao(ctx);
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
                        if (Page.Request.QueryString["id"] == "is"){
                            entityHd.TransactionCode = Constant.TransactionCode.IMAGING_CHARGES;
                        }
                        else if (Page.Request.QueryString["id"] == "lb"){
                            entityHd.TransactionCode = Constant.TransactionCode.LABORATORY_CHARGES;
                        }
                        else
                        {
                            entityHd.TransactionCode = Constant.TransactionCode.OTHER_DIAGNOSTIC_CHARGES;
                        }; break;
                    default: entityHd.TransactionCode = Constant.TransactionCode.OP_CHARGES; break;
                }

                entityHd.TransactionDate = Helper.GetDatePickerValue(Request.Form[txtTransactionDate.UniqueID]);
                entityHd.TransactionTime = Request.Form[txtTransactionTime.UniqueID];
                entityHd.IsCorrectionTransaction = hdnIsCheckedTransactionCorrection.Value == "1" ? true : false;
                entityHd.IsAIOTransaction = hdnIsCheckedAIOTransaction.Value == "1" ? true : false;
                entityHd.Remarks = txtRemarks.Text;
                entityHd.PatientBillingID = null;
                entityHd.ReferenceNo = txtReferenceNo.Text;
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                entityHd.GCVoidReason = null;
                entityHd.TransactionNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.TransactionDate, ctx);
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                entityHd.TransactionID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
                transactionID = entityHd.TransactionID;

                if (hdnIsLaboratoryUnit.Value == "1")
                {
                    PatientChargesHdInfo hdInfo = entityHdInfoDao.Get(transactionID);
                    hdInfo.IsPathologicalAnatomyTest = chkIsPATest.Checked;
                    hdInfo.LastUpdatedPAInfoBy = AppSession.UserLogin.UserID;
                    hdInfo.LastUpdatedPAInfoDate = DateTime.Now;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityHdInfoDao.Update(hdInfo);
                }
            }
            else
            {
                transactionID = Convert.ToInt32(hdnTransactionHdID.Value);

                if (hdnIsLaboratoryUnit.Value == "1")
                {
                    PatientChargesHdInfo hdInfo = entityHdInfoDao.Get(transactionID);
                    hdInfo.IsPathologicalAnatomyTest = chkIsPATest.Checked;
                    hdInfo.LastUpdatedPAInfoBy = AppSession.UserLogin.UserID;
                    hdInfo.LastUpdatedPAInfoDate = DateTime.Now;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityHdInfoDao.Update(hdInfo);
                }
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesHdInfoDao entityHdInfoDao = new PatientChargesHdInfoDao(ctx);
            try
            {
                PatientChargesHd entityHd = new PatientChargesHd();
                entityHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                entityHd.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                entityHd.TransactionCode = Constant.TransactionCode.LABORATORY_CHARGES;
                entityHd.TransactionDate = Helper.GetDatePickerValue(Request.Form[txtTransactionDate.UniqueID]);
                entityHd.TransactionTime = Request.Form[txtTransactionTime.UniqueID];
                entityHd.PatientBillingID = null;
                entityHd.IsCorrectionTransaction = hdnIsCheckedTransactionCorrection.Value == "1" ? true : false;
                entityHd.IsAIOTransaction = hdnIsCheckedAIOTransaction.Value == "1" ? true : false;
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

                if (hdnIsLaboratoryUnit.Value == "1")
                {
                    PatientChargesHdInfo hdInfo = entityHdInfoDao.Get(Convert.ToInt32(retval));
                    hdInfo.IsPathologicalAnatomyTest = chkIsPATest.Checked;
                    hdInfo.LastUpdatedPAInfoBy = AppSession.UserLogin.UserID;
                    hdInfo.LastUpdatedPAInfoDate = DateTime.Now;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityHdInfoDao.Update(hdInfo);
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
            try
            {
                PatientChargesHd entity = BusinessLayer.GetPatientChargesHd(Convert.ToInt32(hdnTransactionHdID.Value));
                if (AppSession.IsAdminCanCancelAllTransaction)
                {
                    if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN || entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                    {
                        entity.ReferenceNo = txtReferenceNo.Text;
                        entity.Remarks = txtRemarks.Text;
                        entity.TransactionDate = Helper.GetDatePickerValue(Request.Form[txtTransactionDate.UniqueID]);
                        entity.TransactionTime = Request.Form[txtTransactionTime.UniqueID];
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        BusinessLayer.UpdatePatientChargesHd(entity);

                        if (hdnIsLaboratoryUnit.Value == "1")
                        {
                            PatientChargesHdInfo hdInfo = BusinessLayer.GetPatientChargesHdInfo(entity.TransactionID);
                            hdInfo.IsPathologicalAnatomyTest = chkIsPATest.Checked;
                            hdInfo.LastUpdatedPAInfoBy = AppSession.UserLogin.UserID;
                            hdInfo.LastUpdatedPAInfoDate = DateTime.Now;
                            BusinessLayer.UpdatePatientChargesHdInfo(hdInfo);
                        }
                        return true;
                    }
                    else
                    {
                        errMessage = "Transaksi " + entity.TransactionNo + " Sudah Diproses. Tidak Bisa Diubah";
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
                        entity.TransactionDate = Helper.GetDatePickerValue(Request.Form[txtTransactionDate.UniqueID]);
                        entity.TransactionTime = Request.Form[txtTransactionTime.UniqueID];
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        BusinessLayer.UpdatePatientChargesHd(entity);

                        if (hdnIsLaboratoryUnit.Value == "1")
                        {
                            PatientChargesHdInfo hdInfo = BusinessLayer.GetPatientChargesHdInfo(entity.TransactionID);
                            hdInfo.IsPathologicalAnatomyTest = chkIsPATest.Checked;
                            hdInfo.LastUpdatedPAInfoBy = AppSession.UserLogin.UserID;
                            hdInfo.LastUpdatedPAInfoDate = DateTime.Now;
                            BusinessLayer.UpdatePatientChargesHdInfo(hdInfo);
                        }
                        return true;
                    }
                    else
                    {
                        errMessage = "Transaksi " + entity.TransactionNo + " Sudah Diproses. Tidak Bisa Diubah";
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
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            TestOrderHdDao testOrderHdDao = new TestOrderHdDao(ctx);
            TestOrderDtDao testOrderDtDao = new TestOrderDtDao(ctx);
            ServiceOrderHdDao serviceOrderHdDao = new ServiceOrderHdDao(ctx);
            ServiceOrderDtDao serviceOrderDtDao = new ServiceOrderDtDao(ctx);

            try
            {
                PatientChargesHd entity = entityHdDao.Get(TransactionID);
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

                    if (entity.TestOrderID != null && entity.TestOrderID != 0)
                    {
                        TestOrderHd testOrderHd = testOrderHdDao.Get(Convert.ToInt32(entity.TestOrderID));
                        if (testOrderHd.GCTransactionStatus != Constant.TransactionStatus.PROCESSED && testOrderHd.GCTransactionStatus != Constant.TransactionStatus.VOID & testOrderHd.GCTransactionStatus != Constant.TransactionStatus.CLOSED)
                        {
                            errMessage = "Masih ada Test Order yang Belum Diproses. Silakan approve / decline terlebih dahulu.";
                            result = false;
                        }
                    }

                    if (entity.ServiceOrderID != null && entity.ServiceOrderID != 0)
                    {
                        ServiceOrderHd serviceOrderHd = serviceOrderHdDao.Get(Convert.ToInt32(entity.ServiceOrderID));
                        if (serviceOrderHd.GCTransactionStatus != Constant.TransactionStatus.PROCESSED && serviceOrderHd.GCTransactionStatus != Constant.TransactionStatus.VOID)
                        {
                            errMessage = "Masih ada Service Order yang Belum Diproses. Silakan approve / decline terlebih dahulu.";
                            result = false;
                        }
                    }

                    if (result)
                    {
                        if (isProcessCharges)
                        {
                            Int32 transactionID = Convert.ToInt32(hdnTransactionHdID.Value);

                            if (!string.IsNullOrEmpty(referenceNo))
                                entity.ReferenceNo = referenceNo;
                            
                            List<PatientChargesDt> lstPatientChargesDt = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID = {0}  AND IsDeleted = 0 AND GCTransactionDetailStatus <> '{1}'", entity.TransactionID, Constant.TransactionStatus.VOID), ctx); // AND LocationID IS NOT NULL AND IsApproved = 0
                            foreach (PatientChargesDt patientChargesDt in lstPatientChargesDt)
                            {
                                if ((patientChargesDt.LocationID != null && patientChargesDt.LocationID != 0) && !patientChargesDt.IsApproved)
                                {
                                    patientChargesDt.IsApproved = true;
                                }
                                patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;

                                if (entity.TestOrderID != null && patientChargesDt.ReferenceDtID == null && (entity.TransactionCode == Constant.TransactionCode.LABORATORY_CHARGES || entity.TransactionCode == Constant.TransactionCode.IMAGING_CHARGES || entity.TransactionCode == Constant.TransactionCode.OTHER_DIAGNOSTIC_CHARGES))
                                {
                                    TestOrderDt orderDt = new TestOrderDt();
                                    orderDt.TestOrderID = Convert.ToInt32(entity.TestOrderID);
                                    orderDt.ItemID = patientChargesDt.ItemID;
                                    orderDt.ParamedicID = patientChargesDt.ParamedicID;
                                    orderDt.GCTestOrderStatus = Constant.TestOrderStatus.IN_PROGRESS;
                                    orderDt.ItemQty = patientChargesDt.ChargedQuantity;
                                    orderDt.IsCITO = patientChargesDt.IsCITO;
                                    orderDt.ItemUnit = patientChargesDt.GCItemUnit;
                                    orderDt.IsCreatedFromOrder = false;
                                    orderDt.CreatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    int oReferenceDtID = testOrderDtDao.InsertReturnPrimaryKeyID(orderDt);

                                    if (oReferenceDtID != null && oReferenceDtID != 0)
                                    {
                                        patientChargesDt.ReferenceDtID = oReferenceDtID;
                                    }
                                }

                                patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Update(patientChargesDt);
                            }

                            int oTestOrderID;
                            if ((entity.TransactionCode == Constant.TransactionCode.LABORATORY_CHARGES || entity.TransactionCode == Constant.TransactionCode.IMAGING_CHARGES || entity.TransactionCode == Constant.TransactionCode.OTHER_DIAGNOSTIC_CHARGES) && entity.TestOrderID == null)
                            {
                                #region Generate Test Order Hd + Dt

                                TestOrderHd orderHd = new TestOrderHd();
                                orderHd.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
                                orderHd.FromHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                                orderHd.VisitHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                                orderHd.ParamedicID = AppSession.RegisteredPatient.ParamedicID;
                                orderHd.VisitID = entity.VisitID;
                                orderHd.TestOrderDate = entity.TransactionDate;
                                orderHd.TestOrderTime = entity.TransactionTime;
                                orderHd.GCToBePerformed = Constant.ToBePerformed.CURRENT_EPISODE;
                                orderHd.ScheduledDate = entity.TransactionDate;
                                orderHd.ScheduledTime = entity.TransactionTime;
                                orderHd.ReferenceNo = txtReferenceNo.Text;
                                orderHd.Remarks = txtRemarks.Text;
                                orderHd.IsCITO = false;

                                if (entity.TransactionCode == Constant.TransactionCode.IMAGING_CHARGES)
                                {
                                    orderHd.TransactionCode = Constant.TransactionCode.IMAGING_TEST_ORDER;
                                }
                                else if (entity.TransactionCode == Constant.TransactionCode.LABORATORY_CHARGES)
                                {
                                    orderHd.TransactionCode = Constant.TransactionCode.LABORATORY_TEST_ORDER;
                                }
                                else
                                {
                                    orderHd.TransactionCode = Constant.TransactionCode.OTHER_TEST_ORDER;
                                }

                                orderHd.TestOrderNo = BusinessLayer.GenerateTransactionNo(orderHd.TransactionCode, orderHd.TestOrderDate, ctx);
                                orderHd.GCOrderStatus = Constant.TestOrderStatus.RECEIVED;
                                orderHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                                orderHd.IsCreatedBySystem = true;
                                orderHd.CreatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                oTestOrderID = Convert.ToInt32(testOrderHdDao.InsertReturnPrimaryKeyID(orderHd));

                                foreach (PatientChargesDt patientChargesDt in lstPatientChargesDt)
                                {
                                    TestOrderDt orderDt = new TestOrderDt();
                                    orderDt.TestOrderID = oTestOrderID;
                                    orderDt.ItemID = patientChargesDt.ItemID;
                                    orderDt.ParamedicID = patientChargesDt.ParamedicID;
                                    orderDt.GCTestOrderStatus = Constant.TestOrderStatus.IN_PROGRESS;
                                    orderDt.ItemQty = patientChargesDt.ChargedQuantity;
                                    orderDt.IsCITO = patientChargesDt.IsCITO;
                                    orderDt.ItemUnit = patientChargesDt.GCItemUnit;
                                    orderDt.IsCreatedFromOrder = false;
                                    orderDt.CreatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    testOrderDtDao.Insert(orderDt);
                                }

                                #endregion

                                entity.TestOrderID = oTestOrderID;
                            }

                            entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                            entity.ProposedBy = AppSession.UserLogin.UserID;
                            entity.ProposedDate = DateTime.Now;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityHdDao.Update(entity);

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
                PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
                //ChargesStatusLogDao logDao = new ChargesStatusLogDao(ctx);
                try
                {
                    //ChargesStatusLog log = new ChargesStatusLog();
                    //string statusOld = "", statusNew = "";
                    Int32 TransactionID = Convert.ToInt32(hdnTransactionHdID.Value);
                    PatientChargesHd entity = entityHdDao.Get(TransactionID);
                    //statusOld = entity.GCTransactionStatus;
                    if (AppSession.IsAdminCanCancelAllTransaction)
                    {
                        if (entity.PatientBillingID == null && (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN || entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL))
                        {
                            ((PatientManagementTransactionDetailServiceCtl)ctlService).OnVoidAllChargesDt(ctx, TransactionID);
                            ((PatientManagementTransactionDetailDrugMSCtl)ctlDrugMS).OnVoidAllChargesDt(ctx, TransactionID);
                            //((PatientManagementTransactionDetailDrugMSReturnCtl)ctlDrugMSReturn).OnVoidAllChargesDt(ctx, TransactionID);
                            ((PatientManagementTransactionDetailLogisticCtl)ctlLogistic).OnVoidAllChargesDt(ctx, TransactionID);

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

                            //List<PatientChargesDt> lstPatientChargesDt = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID = {0} AND IsDeleted = 0 AND GCTransactionDetailStatus <> '{1}'", entity.TransactionID, Constant.TransactionStatus.VOID), ctx);
                            //foreach (PatientChargesDt patientChargesDt in lstPatientChargesDt)
                            //{
                            //    //PatientChargesDt entity = entityDtDao.Get(patientChargesDt.ID);
                            //    patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                            //    patientChargesDt.IsApproved = false;
                            //    patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            //    entityDtDao.Update(patientChargesDt);
                            //}

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

                                    List<TestOrderDt> lstTestOrderDt = BusinessLayer.GetTestOrderDtList(string.Format("TestOrderID = {0} AND IsDeleted = 0", entity.TestOrderID.ToString()), ctx);
                                    foreach (TestOrderDt orderDt in lstTestOrderDt)
                                    {
                                        if (orderDt != null && orderDt.GCTestOrderStatus != Constant.TestOrderStatus.CANCELLED)
                                        {
                                            if (!orderDt.IsDeleted)
                                            {
                                                if (hdnIsDischarges.Value == "1")
                                                {
                                                    orderDt.GCTestOrderStatus = Constant.TestOrderStatus.CANCELLED;
                                                    orderDt.GCVoidReason = Constant.DeleteReason.OTHER;
                                                    orderDt.VoidReason = "Linked transaction was deleted";
                                                }
                                                else
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
                            errMessage = "Transaksi " + entity.TransactionNo + " Sudah Diproses. Tidak Bisa Diubah";
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                    }
                    else
                    {
                        if (entity.PatientBillingID == null && entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                        {
                            ((PatientManagementTransactionDetailServiceCtl)ctlService).OnVoidAllChargesDt(ctx, TransactionID);
                            ((PatientManagementTransactionDetailDrugMSCtl)ctlDrugMS).OnVoidAllChargesDt(ctx, TransactionID);
                            //((PatientManagementTransactionDetailDrugMSReturnCtl)ctlDrugMSReturn).OnVoidAllChargesDt(ctx, TransactionID);
                            ((PatientManagementTransactionDetailLogisticCtl)ctlLogistic).OnVoidAllChargesDt(ctx, TransactionID);

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

                            //List<PatientChargesDt> lstPatientChargesDt = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID = {0} AND IsDeleted = 0 AND GCTransactionDetailStatus <> '{1}'", entity.TransactionID, Constant.TransactionStatus.VOID), ctx);
                            //foreach (PatientChargesDt patientChargesDt in lstPatientChargesDt)
                            //{
                            //    //PatientChargesDt entity = entityDtDao.Get(patientChargesDt.ID);
                            //    patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                            //    patientChargesDt.IsApproved = false;
                            //    patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            //    entityDtDao.Update(patientChargesDt);
                            //}

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

                                    List<TestOrderDt> lstTestOrderDt = BusinessLayer.GetTestOrderDtList(string.Format("TestOrderID = {0} AND IsDeleted = 0", entity.TestOrderID.ToString()), ctx);
                                    foreach (TestOrderDt orderDt in lstTestOrderDt)
                                    {
                                        if (orderDt != null && orderDt.GCTestOrderStatus != Constant.TestOrderStatus.CANCELLED)
                                        {
                                            if (!orderDt.IsDeleted)
                                            {
                                                if (hdnIsDischarges.Value == "1")
                                                {
                                                    orderDt.GCTestOrderStatus = Constant.TestOrderStatus.CANCELLED;
                                                    orderDt.GCVoidReason = Constant.DeleteReason.OTHER;
                                                    orderDt.VoidReason = "Linked transaction was deleted";
                                                }
                                                else
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
                            errMessage = "Transaksi " + entity.TransactionNo + " Sudah Diproses. Tidak Bisa Diubah";
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
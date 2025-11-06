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
    public partial class PatientBillSummaryCharges : BasePageTrxPatientManagement
    {
        public override string OnGetMenuCode()
        {
            string id = Page.Request.QueryString["id"];
            switch (hdnVisitDepartmentID.Value)
            {
                case Constant.Facility.INPATIENT:
                    switch (id)
                    {
                        case "er": return Constant.MenuCode.Inpatient.BILL_SUMMARY_CHARGES_EMERGENCY;
                        case "op": return Constant.MenuCode.Inpatient.BILL_SUMMARY_CHARGES_OUTPATIENT;
                        case "is": return Constant.MenuCode.Inpatient.BILL_SUMMARY_IMAGING;
                        case "md": return Constant.MenuCode.Inpatient.BILL_SUMMARY_MEDICAL_DIAGNOSTIC;
                        case "bs": return Constant.MenuCode.BillingManagement.BILL_SUMMARY_CHARGES;
                        default: return Constant.MenuCode.Inpatient.BILL_SUMMARY_CHARGES;
                    }
                case Constant.Facility.MEDICAL_CHECKUP:
                    switch (id)
                    {
                        case "er": return Constant.MenuCode.MedicalCheckup.BILL_SUMMARY_CHARGES_EMERGENCY;
                        case "op": return Constant.MenuCode.MedicalCheckup.BILL_SUMMARY_CHARGES_OUTPATIENT;
                        case "is": return Constant.MenuCode.MedicalCheckup.BILL_SUMMARY_IMAGING;
                        case "md": return Constant.MenuCode.MedicalCheckup.BILL_SUMMARY_MEDICAL_DIAGNOSTIC;
                        case "bs": return Constant.MenuCode.BillingManagement.BILL_SUMMARY_CHARGES;
                        default: return Constant.MenuCode.MedicalCheckup.BILL_SUMMARY_CHARGES;
                    }
                case Constant.Facility.EMERGENCY:
                    switch (id)
                    {
                        case "op": return Constant.MenuCode.EmergencyCare.BILL_SUMMARY_CHARGES_OUTPATIENT;
                        case "is": return Constant.MenuCode.EmergencyCare.BILL_SUMMARY_IMAGING;
                        case "md": return Constant.MenuCode.EmergencyCare.BILL_SUMMARY_MEDICAL_DIAGNOSTIC;
                        case "bs": return Constant.MenuCode.BillingManagement.BILL_SUMMARY_CHARGES;
                        default: return Constant.MenuCode.EmergencyCare.BILL_SUMMARY_CHARGES;
                    }
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                    {
                        switch (id)
                        {
                            case "op": return Constant.MenuCode.Imaging.BILL_SUMMARY_CHARGES_OUTPATIENT;
                            case "er": return Constant.MenuCode.Imaging.BILL_SUMMARY_CHARGES_EMERGENCY;
                            case "md": return Constant.MenuCode.Imaging.BILL_SUMMARY_MEDICAL_DIAGNOSTIC;
                            case "bs": return Constant.MenuCode.BillingManagement.BILL_SUMMARY_CHARGES;
                            default: return Constant.MenuCode.Imaging.BILL_SUMMARY_CHARGES;
                        }
                    }
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                    {
                        switch (id)
                        {
                            case "op": return Constant.MenuCode.Laboratory.BILL_SUMMARY_CHARGES_OUTPATIENT;
                            case "er": return Constant.MenuCode.Laboratory.BILL_SUMMARY_CHARGES_EMERGENCY;
                            case "md": return Constant.MenuCode.Laboratory.BILL_SUMMARY_MEDICAL_DIAGNOSTIC;
                            case "bs": return Constant.MenuCode.BillingManagement.BILL_SUMMARY_CHARGES;
                            default: return Constant.MenuCode.Laboratory.BILL_SUMMARY_IMAGING;
                        }
                    }
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                    {
                        switch (id)
                        {
                            case "op": return Constant.MenuCode.Radiotheraphy.BILL_SUMMARY_CHARGES_OUTPATIENT;
                            case "er": return Constant.MenuCode.Radiotheraphy.BILL_SUMMARY_CHARGES_EMERGENCY;
                            case "md": return Constant.MenuCode.MedicalDiagnostic.BILL_SUMMARY_MEDICAL_DIAGNOSTIC;
                            case "is": return Constant.MenuCode.MedicalDiagnostic.BILL_SUMMARY_IMAGING;
                            case "bs": return Constant.MenuCode.BillingManagement.BILL_SUMMARY_CHARGES;
                            default: return Constant.MenuCode.Radiotheraphy.BILL_SUMMARY_CHARGES;
                        }
                    }
                    switch (id)
                    {
                        case "op": return Constant.MenuCode.MedicalDiagnostic.BILL_SUMMARY_CHARGES_OUTPATIENT;
                        case "er": return Constant.MenuCode.MedicalDiagnostic.BILL_SUMMARY_CHARGES_EMERGENCY;
                        case "md": return Constant.MenuCode.MedicalDiagnostic.BILL_SUMMARY_MEDICAL_DIAGNOSTIC;
                        case "is": return Constant.MenuCode.MedicalDiagnostic.BILL_SUMMARY_IMAGING;
                        case "bs": return Constant.MenuCode.BillingManagement.BILL_SUMMARY_CHARGES;
                        default: return Constant.MenuCode.MedicalDiagnostic.BILL_SUMMARY_CHARGES;
                    }
                default:
                    switch (id)
                    {
                        case "er": return Constant.MenuCode.Outpatient.BILL_SUMMARY_CHARGES_EMERGENCY;
                        case "is": return Constant.MenuCode.Outpatient.BILL_SUMMARY_IMAGING;
                        case "md": return Constant.MenuCode.Outpatient.BILL_SUMMARY_MEDICAL_DIAGNOSTIC;
                        case "bs": return Constant.MenuCode.BillingManagement.BILL_SUMMARY_CHARGES;
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

            IsAllowProposed = true;
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
        public override string GetTransactionDate2()
        {
            return txtTransactionDate.Text;
        }
        public override string GetTransactionTime2()
        {
            return txtTransactionTime.Text;
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
                return string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND HealthcareServiceUnitID NOT IN ({2},{3},{4}) AND IsDeleted = 0 AND IsUsingRegistration = 1", AppSession.UserLogin.HealthcareID, Constant.Facility.DIAGNOSTIC, hdnHSUImagingID.Value, hdnHSULaboratoryID.Value, AppSession.RegisteredPatient.HealthcareServiceUnitID);
            ConsultVisit entity = BusinessLayer.GetConsultVisitList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID))[0];
            if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
            {
                string hsuID = "";
                List<PatientTransfer> entityPT = BusinessLayer.GetPatientTransferList(string.Format("RegistrationID = {0}", entity.RegistrationID));
                if (entityPT.Count > 0)
                {
                    foreach (PatientTransfer pt in entityPT)
                    {
                        hsuID += pt.FromHealthcareServiceUnitID.ToString() + "," + pt.ToHealthcareServiceUnitID.ToString() + ",";
                    }
                    hsuID = hsuID.Substring(0, hsuID.Length - 1);
                    //return string.Format("HealthcareID = '{0}' AND HealthcareServiceUnitID IN ({1}) AND IsDeleted = 0 AND IsUsingRegistration = 1", AppSession.UserLogin.HealthcareID, entityPT[entityPT.Count - 1].FromHealthcareServiceUnitID, entityPT[entityPT.Count - 1].ToHealthcareServiceUnitID);
                    return string.Format("HealthcareID = '{0}' AND HealthcareServiceUnitID IN ({1}) AND IsDeleted = 0 AND IsUsingRegistration = 1", AppSession.UserLogin.HealthcareID, hsuID);
                }
                else
                {
                    return string.Format("HealthcareID = '{0}' AND HealthcareServiceUnitID = {1} AND IsDeleted = 0 AND IsUsingRegistration = 1", AppSession.UserLogin.HealthcareID, entity.HealthcareServiceUnitID);
                }
            }
            return string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsDeleted = 0 AND IsUsingRegistration = 1", AppSession.UserLogin.HealthcareID, Constant.Facility.OUTPATIENT);
        }

        protected override void InitializeDataControl()
        {
            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format(
                          "HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
                          AppSession.UserLogin.HealthcareID, //0
                          Constant.SettingParameter.SA0168 //1
                          ));
            hdnIsShowItemNotificationWhenProposed.Value = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.SA0168).FirstOrDefault().ParameterValue;

            hdnIsAdminCanCancelAllTransaction.Value = AppSession.IsAdminCanCancelAllTransaction ? "1" : "0";

            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            
            vConsultVisit2 entity = BusinessLayer.GetvConsultVisit2List(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();

            if (entity.DischargeDate != null && entity.DischargeDate.ToString(Constant.FormatString.DATE_FORMAT) != "01-Jan-1900")
            {
                hdnIsDischarges.Value = "1";
            }
            else
            {
                hdnIsDischarges.Value = "0";
            }

            hdnGCRegistrationStatus.Value = entity.GCVisitStatus;
            hdnRegistrationID.Value = entity.RegistrationID.ToString();
            hdnRegistrationPhysicianID.Value = entity.ParamedicID.ToString();
            hdnHealthcareID.Value = AppSession.UserLogin.HealthcareID;
            hdnBusinessPartnerID.Value = entity.BusinessPartnerID.ToString();
            hdnGCCustomerType.Value = entity.GCCustomerType;
            hdnClassID.Value = entity.ChargeClassID.ToString();
            hdnVisitDepartmentID.Value = hdnDepartmentID.Value = entity.DepartmentID;
            hdnLinkedRegistrationID.Value = entity.LinkedRegistrationID.ToString();

            hdnIsHasAIOPackage.Value = entity.IsHasAIOPackage ? "1" : "0";

            if (hdnIsHasAIOPackage.Value == "1")
            {
                trIsAIOTransaction.Attributes.Remove("style");
            }
            else
            {
                trIsAIOTransaction.Attributes.Add("style", "display:none");
            }

            string requestID = Page.Request.QueryString["id"];
            if (requestID == "er")
            {
                vHealthcareServiceUnit hsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = {0} AND DepartmentID = '{1}' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, Constant.Facility.EMERGENCY)).FirstOrDefault();
                hdnDefaultHealthcareServiceUnitID.Value = hdnHealthcareServiceUnitID.Value = hsu.HealthcareServiceUnitID.ToString();
                hdnDefaultLocationID.Value = hdnLocationID.Value = hsu.LocationID.ToString();
                hdnDefaultLogisticLocationID.Value = hdnLogisticLocationID.Value = hsu.LogisticLocationID.ToString();
                hdnDepartmentID.Value = Constant.Facility.EMERGENCY;
            }
            else if (requestID == "op")
            {
                vHealthcareServiceUnit hsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = {0} AND DepartmentID = '{1}' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, Constant.Facility.OUTPATIENT)).FirstOrDefault();
                hdnDefaultHealthcareServiceUnitID.Value = hdnHealthcareServiceUnitID.Value = hsu.HealthcareServiceUnitID.ToString();
                hdnDefaultLocationID.Value = hdnLocationID.Value = hsu.LocationID.ToString();
                hdnDefaultLogisticLocationID.Value = hdnLogisticLocationID.Value = hsu.LogisticLocationID.ToString();
                trServiceUnit.Attributes.Remove("style");
                hdnDepartmentID.Value = Constant.Facility.OUTPATIENT;
            }
            else if (requestID == "is")
            {
                SettingParameter settingParameter = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID);
                string imagingServiceUnitID = settingParameter.ParameterValue;
                vHealthcareServiceUnit hsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = {0} AND ServiceUnitID = {1}", AppSession.UserLogin.HealthcareID, imagingServiceUnitID)).FirstOrDefault();
                hdnDefaultHealthcareServiceUnitID.Value = hdnHealthcareServiceUnitID.Value = hsu.HealthcareServiceUnitID.ToString();
                hdnDefaultLocationID.Value = hdnLocationID.Value = hsu.LocationID.ToString();
                hdnDefaultLogisticLocationID.Value = hdnLogisticLocationID.Value = hsu.LogisticLocationID.ToString();
                hdnDepartmentID.Value = Constant.Facility.DIAGNOSTIC;
            }
            else if (requestID == "md")
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
            }
            else
            {
                hdnDefaultLocationID.Value = hdnLocationID.Value = entity.LocationID.ToString();
                hdnDefaultLogisticLocationID.Value = hdnLogisticLocationID.Value = entity.LogisticLocationID.ToString();
                hdnDefaultHealthcareServiceUnitID.Value = hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
                hdnPhysicianID.Value = entity.ParamedicID.ToString();
                hdnPhysicianCode.Value = entity.ParamedicCode;
                hdnPhysicianName.Value = entity.ParamedicName;
            }

            IsLoadFirstRecord = (OnGetRowCount() > 0);

            hdnTransactionHdID.Value = "0";
            ((PatientManagementTransactionDetailServiceCtl)ctlService).OnAddRecord();
            ((PatientManagementTransactionDetailDrugMSCtl)ctlDrugMS).OnAddRecord();
            ((PatientManagementTransactionDetailDrugMSReturnCtl)ctlDrugMSReturn).OnAddRecord();
            ((PatientManagementTransactionDetailLogisticCtl)ctlLogistic).OnAddRecord();
            ((PatientManagementTransactionDetailLogisticReturnCtl)ctlLogisticReturn).OnAddRecord();

            if (requestID != null && requestID != "" && hdnHealthcareServiceUnitID.Value != "")
            {
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
            }

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
                ((PatientManagementTransactionDetailServiceCtl)ctlService).OnAddRecord();
                ((PatientManagementTransactionDetailDrugMSCtl)ctlDrugMS).OnAddRecord();
                ((PatientManagementTransactionDetailLogisticCtl)ctlLogistic).OnAddRecord();
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
            ((PatientManagementTransactionDetailServiceCtl)ctlService).SetControlProperties();
            ((PatientManagementTransactionDetailDrugMSCtl)ctlDrugMS).SetControlProperties();
            ((PatientManagementTransactionDetailLogisticCtl)ctlLogistic).SetControlProperties();

            string serviceUnitID = "";
            SettingParameter settingParameterLB = BusinessLayer.GetSettingParameter(Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID);
            SettingParameter settingParameterIS = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID);
            serviceUnitID = string.Format("{0}, {1}", settingParameterLB.ParameterValue, settingParameterIS.ParameterValue);
            string filterHSU = string.Format("HealthcareID = {0} AND DepartmentID = '{1}' AND ServiceUnitID NOT IN ({2}) AND IsLaboratoryUnit = 0",
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
            ((PatientManagementTransactionDetailLogisticReturnCtl)ctlLogisticReturn).OnAddRecord();

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

        #region Load Entity
        protected string GetFilterExpression()
        {
            string filterExpression = "";
            string id = Page.Request.QueryString["id"];

            //filterExpression = string.Format("(RegistrationID = {0} OR (LinkedToRegistrationID = {0} AND IsChargesTransfered = 1))", hdnRegistrationID.Value);

            filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);

            if (id == "md")
                filterExpression += string.Format(" AND DepartmentID = '{0}' AND HealthcareServiceUnitID NOT IN ({1},{2},{3})", Constant.Facility.DIAGNOSTIC, hdnHSUImagingID.Value, hdnHSULaboratoryID.Value, AppSession.RegisteredPatient.HealthcareServiceUnitID);
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
            int registrationID = Convert.ToInt32(hdnRegistrationID.Value);
            int registrationHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
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
            else if (hdnDepartmentID.Value == Constant.Facility.MEDICAL_CHECKUP)
            {
                requestID = "mc";
            }

            int hdRowCount = BusinessLayer.GetPatientChargesHdPerRegistrationPerRequestIDRowCount(registrationID, requestID, registrationHealthcareServiceUnitID);
            hdnChargesHdRowCount.Value = hdRowCount.ToString();

            return hdRowCount;
        }

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
                }
                else
                {
                    isShowWatermark = false;
                    IsEditable = true;
                    IsAllowProposed = true;
                }
            }

            lblServiceUnit.Attributes.Remove("class");
            lblServiceUnit.Attributes.Add("class", "lblDisabled");
            hdnGCTransactionStatus.Value = entity.GCTransactionStatus;
            hdnTransactionHdID.Value = entity.TransactionID.ToString();
            txtTransactionNo.Text = entity.TransactionNo;
            txtReferenceNo.Text = entity.ReferenceNo;
            txtRemarks.Text = entity.Remarks;
            txtTransactionDate.Text = entity.TransactionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTransactionTime.Text = entity.TransactionTime;
            hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
            chkIsCorrectionTransaction.Checked = entity.IsCorrectionTransaction;
            hdnIsCheckedTransactionCorrection.Value = entity.IsCorrectionTransaction ? "1" : "0";

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
            ((PatientManagementTransactionDetailLogisticReturnCtl)ctlLogisticReturn).InitializeTransactionControl(flagHaveCharges);

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
                        if (Page.Request.QueryString["id"] == "is")
                            entityHd.TransactionCode = Constant.TransactionCode.IMAGING_CHARGES;
                        else
                        {
                            if (hdnVisitDepartmentID.Value == Constant.Facility.DIAGNOSTIC && AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                                entityHd.TransactionCode = Constant.TransactionCode.IMAGING_CHARGES;
                            else
                                entityHd.TransactionCode = Constant.TransactionCode.OTHER_DIAGNOSTIC_CHARGES;
                        }; break;
                    default: entityHd.TransactionCode = Constant.TransactionCode.OP_CHARGES; break;
                }
                entityHd.TransactionDate = Helper.GetDatePickerValue(Request.Form[txtTransactionDate.UniqueID]);
                entityHd.TransactionTime = Request.Form[txtTransactionTime.UniqueID];
                entityHd.IsCorrectionTransaction = hdnIsCheckedTransactionCorrection.Value == "1" ? true : false;
                entityHd.IsAIOTransaction = hdnIsCheckedAIOTransaction.Value == "1" ? true : false;
                entityHd.PatientBillingID = null;
                entityHd.Remarks = txtRemarks.Text;
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
                switch (hdnDepartmentID.Value)
                {
                    case Constant.Facility.INPATIENT: entityHd.TransactionCode = Constant.TransactionCode.IP_CHARGES; break;
                    case Constant.Facility.EMERGENCY: entityHd.TransactionCode = Constant.TransactionCode.ER_CHARGES; break;
                    case Constant.Facility.DIAGNOSTIC:
                        if (Page.Request.QueryString["id"] == "is")
                            entityHd.TransactionCode = Constant.TransactionCode.IMAGING_CHARGES;
                        else
                        {
                            if (hdnVisitDepartmentID.Value == Constant.Facility.DIAGNOSTIC && AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                                entityHd.TransactionCode = Constant.TransactionCode.IMAGING_CHARGES;
                            else
                                entityHd.TransactionCode = Constant.TransactionCode.OTHER_DIAGNOSTIC_CHARGES;
                        }; break;
                    default: entityHd.TransactionCode = Constant.TransactionCode.OP_CHARGES; break;
                }
                entityHd.TransactionDate = Helper.GetDatePickerValue(Request.Form[txtTransactionDate.UniqueID]);
                entityHd.TransactionTime = Request.Form[txtTransactionTime.UniqueID];
                entityHd.PatientBillingID = null;
                entityHd.IsCorrectionTransaction = hdnIsCheckedTransactionCorrection.Value == "1" ? true : false;
                entityHd.IsAIOTransaction = hdnIsCheckedAIOTransaction.Value == "1" ? true : false;
                entityHd.Remarks = txtRemarks.Text;
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
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            TestOrderHdDao testOrderHdDao = new TestOrderHdDao(ctx);
            TestOrderDtDao testOrderDtDao = new TestOrderDtDao(ctx);
            ServiceOrderHdDao serviceOrderHdDao = new ServiceOrderHdDao(ctx);
            ServiceOrderDtDao serviceOrderDtDao = new ServiceOrderDtDao(ctx);
            ItemServiceDao itsDao = new ItemServiceDao(ctx);
            ItemMasterDao masterDao = new ItemMasterDao(ctx);
            PatientChargesDtInfoDao entityDtInfoDao = new PatientChargesDtInfoDao(ctx);
            VisitPackageBalanceHdDao packageBalanceHdDao = new VisitPackageBalanceHdDao(ctx);
            VisitPackageBalanceDtDao packageBalanceDtDao = new VisitPackageBalanceDtDao(ctx);
            PatientDao patientDao = new PatientDao(ctx);

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
                            entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                            entity.ProposedBy = AppSession.UserLogin.UserID;
                            entity.ProposedDate = DateTime.Now;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityHdDao.Update(entity);

                            List<PatientChargesDt> lstPatientChargesDt = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID = {0}  AND IsDeleted = 0 AND GCTransactionDetailStatus <> '{1}'", entity.TransactionID, Constant.TransactionStatus.VOID), ctx); // AND LocationID IS NOT NULL AND IsApproved = 0
                            foreach (PatientChargesDt patientChargesDt in lstPatientChargesDt)
                            {
                                if ((patientChargesDt.LocationID != null && patientChargesDt.LocationID != 0) && !patientChargesDt.IsApproved)
                                {
                                    patientChargesDt.IsApproved = true;
                                }

                                ItemService its = itsDao.Get(patientChargesDt.ItemID);
                                if (its != null)
                                {
                                    if (its.IsPackageItem)
                                    {
                                        patientChargesDt.IsApproved = true;
                                    }
                                }

                                patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Update(patientChargesDt);

                                #region Patient Package
                                PatientChargesDtInfo info = entityDtInfoDao.Get(patientChargesDt.ID);
                                if (its != null && its.IsPackageBalanceItem)
                                {
                                    int mrn = AppSession.RegisteredPatient.MRN;

                                    int ID;
                                    if (info.VisitPackageBalanceTransactionID == null)
                                    {
                                        string filterPackage = string.Format("MRN = {0} AND ItemID = {1} AND Quantity > 0", mrn, patientChargesDt.ItemID);
                                        VisitPackageBalanceHd entityPackageHdCek = BusinessLayer.GetVisitPackageBalanceHdList(filterPackage, ctx).FirstOrDefault();
                                        if (entityPackageHdCek != null)
                                        {
                                            ID = entityPackageHdCek.TransactionID;
                                            VisitPackageBalanceDt entityPackageDt = new VisitPackageBalanceDt();
                                            entityPackageDt.TransactionID = entityPackageHdCek.TransactionID;
                                            entityPackageDt.PatientChargesDtID = patientChargesDt.ID;
                                            entityPackageDt.VisitID = entity.VisitID;
                                            entityPackageDt.QuantityBEGIN = entityPackageHdCek.Quantity;
                                            entityPackageDt.QuantityIN = Convert.ToDecimal(its.DefaultPackageBalanceQty);
                                            entityPackageDt.QuantityOUT = 0;
                                            entityPackageDt.QuantityEND = entityPackageDt.QuantityBEGIN + entityPackageDt.QuantityIN - entityPackageDt.QuantityOUT;
                                            entityPackageDt.CreatedBy = AppSession.UserLogin.UserID;
                                            entityPackageDt.CreatedDate = DateTime.Now;
                                            packageBalanceDtDao.Insert(entityPackageDt);
                                            entityPackageHdCek.Quantity = entityPackageDt.QuantityEND;
                                            entityPackageHdCek.QuantityBegin = entityPackageHdCek.QuantityBegin + Convert.ToDecimal(its.DefaultPackageBalanceQty);
                                            entityPackageHdCek.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            entityPackageHdCek.LastUpdatedDate = DateTime.Now;
                                            packageBalanceHdDao.Update(entityPackageHdCek);
                                        }
                                        else
                                        {
                                            VisitPackageBalanceHd entityPackageHd = new VisitPackageBalanceHd();
                                            //entityPackageHd.GuestID = guestID;
                                            entityPackageHd.MRN = mrn;
                                            entityPackageHd.ItemID = patientChargesDt.ItemID;
                                            entityPackageHd.QuantityBegin = Convert.ToDecimal(its.DefaultPackageBalanceQty);
                                            entityPackageHd.ExpiredDate = info.PackageExpiredDate;
                                            entityPackageHd.Quantity = Convert.ToDecimal(its.DefaultPackageBalanceQty);
                                            entityPackageHd.LineAmount = patientChargesDt.LineAmount;
                                            entityPackageHd.CreatedBy = AppSession.UserLogin.UserID;
                                            entityPackageHd.CreatedDate = DateTime.Now;
                                            ID = packageBalanceHdDao.InsertReturnPrimaryKeyID(entityPackageHd);

                                            VisitPackageBalanceDt entityPackageDt = new VisitPackageBalanceDt();
                                            entityPackageDt.TransactionID = ID;
                                            entityPackageDt.PatientChargesDtID = patientChargesDt.ID;
                                            entityPackageDt.VisitID = entity.VisitID;
                                            entityPackageDt.QuantityBEGIN = 0;
                                            entityPackageDt.QuantityIN = Convert.ToDecimal(its.DefaultPackageBalanceQty);
                                            entityPackageDt.QuantityOUT = 0;
                                            entityPackageDt.QuantityEND = entityPackageDt.QuantityBEGIN + entityPackageDt.QuantityIN - entityPackageDt.QuantityOUT;
                                            entityPackageDt.CreatedBy = AppSession.UserLogin.UserID;
                                            entityPackageDt.CreatedDate = DateTime.Now;
                                            packageBalanceDtDao.Insert(entityPackageDt);
                                        }

                                        if (info.PackageBalanceQtyTaken > 0)
                                        {
                                            VisitPackageBalanceHd hd = packageBalanceHdDao.Get(ID);
                                            VisitPackageBalanceDt entityPackageDt1 = new VisitPackageBalanceDt();
                                            entityPackageDt1.TransactionID = ID;
                                            entityPackageDt1.PatientChargesDtID = patientChargesDt.ID;
                                            entityPackageDt1.VisitID = entity.VisitID;
                                            entityPackageDt1.QuantityBEGIN = hd.Quantity;
                                            entityPackageDt1.QuantityIN = 0;
                                            entityPackageDt1.QuantityOUT = info.PackageBalanceQtyTaken;
                                            entityPackageDt1.QuantityEND = entityPackageDt1.QuantityBEGIN + entityPackageDt1.QuantityIN - entityPackageDt1.QuantityOUT;
                                            entityPackageDt1.CreatedBy = AppSession.UserLogin.UserID;
                                            entityPackageDt1.CreatedDate = DateTime.Now;

                                            if (entityPackageDt1.QuantityEND >= 0)
                                            {
                                                packageBalanceDtDao.Insert(entityPackageDt1);

                                                hd.Quantity = entityPackageDt1.QuantityEND;
                                                hd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                hd.LastUpdatedDate = DateTime.Now;
                                                packageBalanceHdDao.Update(hd);
                                            }
                                            else
                                            {
                                                result = false;
                                                ItemMaster item = masterDao.Get(patientChargesDt.ItemID);
                                                if (!String.IsNullOrEmpty(errMessage))
                                                {
                                                    errMessage += string.Format("Sisa Paket Kunjungan untuk item {0} tidak mencukupi", item.ItemName1);
                                                }
                                                else
                                                {
                                                    errMessage = string.Format("Sisa Paket Kunjungan untuk item {0} tidak mencukupi", item.ItemName1);
                                                }
                                            }
                                        }

                                        info.VisitPackageBalanceTransactionID = ID;
                                        //info.IsFirstPackageBalance = true;
                                        info.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        info.LastUpdatedDate = DateTime.Now;
                                        entityDtInfoDao.Update(info);
                                    }
                                    else
                                    {
                                        if (info.PackageBalanceQtyTaken > 0)
                                        {
                                            VisitPackageBalanceHd hd1 = packageBalanceHdDao.Get(Convert.ToInt32(info.VisitPackageBalanceTransactionID));
                                            VisitPackageBalanceDt entityPackageDt1 = new VisitPackageBalanceDt();
                                            entityPackageDt1.TransactionID = hd1.TransactionID;
                                            entityPackageDt1.PatientChargesDtID = patientChargesDt.ID;
                                            entityPackageDt1.VisitID = entity.VisitID;
                                            entityPackageDt1.QuantityBEGIN = hd1.Quantity;
                                            entityPackageDt1.QuantityIN = 0;
                                            entityPackageDt1.QuantityOUT = info.PackageBalanceQtyTaken;
                                            entityPackageDt1.QuantityEND = entityPackageDt1.QuantityBEGIN + entityPackageDt1.QuantityIN - entityPackageDt1.QuantityOUT;
                                            entityPackageDt1.CreatedBy = AppSession.UserLogin.UserID;
                                            entityPackageDt1.CreatedDate = DateTime.Now;

                                            if (entityPackageDt1.QuantityEND >= 0)
                                            {
                                                packageBalanceDtDao.Insert(entityPackageDt1);

                                                hd1.Quantity = entityPackageDt1.QuantityEND;
                                                hd1.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                hd1.LastUpdatedDate = DateTime.Now;
                                                packageBalanceHdDao.Update(hd1);
                                            }
                                            else
                                            {
                                                result = false;
                                                ItemMaster item = masterDao.Get(patientChargesDt.ItemID);
                                                if (!String.IsNullOrEmpty(errMessage))
                                                {
                                                    errMessage += string.Format("Sisa Paket Kunjungan untuk item {0} tidak mencukupi", item.ItemName1);
                                                }
                                                else
                                                {
                                                    errMessage = string.Format("Sisa Paket Kunjungan untuk item {0} tidak mencukupi", item.ItemName1);
                                                }
                                            }
                                        }
                                        //info.IsFirstPackageBalance = false;
                                        info.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        info.LastUpdatedDate = DateTime.Now;
                                        entityDtInfoDao.Update(info);
                                    }
                                }
                                #endregion
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

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            string[] param = type.Split(';');
            string gcDeleteReason = param[1];
            string reason = param[2];
            bool result = true;

            if (param[0] == "void")
            {
                IDbContext ctx = DbFactory.Configure(true);
                ServiceOrderHdDao serviceHdDao = new ServiceOrderHdDao(ctx);
                ServiceOrderDtDao serviceDtDao = new ServiceOrderDtDao(ctx);
                PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
                RegistrationBedChargesDao bedChargesDao = new RegistrationBedChargesDao(ctx);

                try
                {
                    Int32 TransactionID = Convert.ToInt32(hdnTransactionHdID.Value);
                    PatientChargesHd entity = entityHdDao.Get(TransactionID);
                    if (AppSession.IsAdminCanCancelAllTransaction)
                    {
                        if (entity.PatientBillingID == null && (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN || entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL))
                        {
                            ((PatientManagementTransactionDetailServiceCtl)ctlService).OnVoidAllChargesDt(ctx, TransactionID);
                            ((PatientManagementTransactionDetailDrugMSCtl)ctlDrugMS).OnVoidAllChargesDt(ctx, TransactionID);
                            ((PatientManagementTransactionDetailDrugMSReturnCtl)ctlDrugMSReturn).OnVoidAllChargesDt(ctx, TransactionID);
                            ((PatientManagementTransactionDetailLogisticCtl)ctlLogistic).OnVoidAllChargesDt(ctx, TransactionID);
                            ((PatientManagementTransactionDetailLogisticReturnCtl)ctlLogisticReturn).OnVoidAllChargesDt(ctx, TransactionID);

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

                            #region Update Service Order
                            if (entity.ServiceOrderID != null)
                            {
                                ServiceOrderHd orderHd = serviceHdDao.Get((int)entity.ServiceOrderID);
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

                                    List<ServiceOrderDt> lstDt = BusinessLayer.GetServiceOrderDtList(string.Format("ServiceOrderID = {0} AND IsDeleted = 0", entity.ServiceOrderID.ToString()), ctx);
                                    foreach (ServiceOrderDt orderDt in lstDt)
                                    {
                                        if (orderDt != null && orderDt.GCServiceOrderStatus != Constant.TestOrderStatus.CANCELLED)
                                        {
                                            if (!orderDt.IsDeleted)
                                            {
                                                if (hdnIsDischarges.Value == "1")
                                                {
                                                    orderDt.GCServiceOrderStatus = Constant.TestOrderStatus.CANCELLED;
                                                    orderDt.GCVoidReason = Constant.DeleteReason.OTHER;
                                                    orderDt.VoidReason = "Linked transaction was deleted";
                                                }
                                                else
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
                            #endregion

                            #region Bed Charges RSDOSKA

                            if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
                            {
                                string filterBedCharges = string.Format("TransactionID = {0}", entity.TransactionID);
                                RegistrationBedCharges regBedCharges = BusinessLayer.GetRegistrationBedChargesList(filterBedCharges, ctx).FirstOrDefault();
                                if (regBedCharges != null)
                                {
                                    regBedCharges.TransactionID = null;
                                    regBedCharges.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    bedChargesDao.Update(regBedCharges);
                                }
                            }

                            #endregion

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
                            ((PatientManagementTransactionDetailDrugMSReturnCtl)ctlDrugMSReturn).OnVoidAllChargesDt(ctx, TransactionID);
                            ((PatientManagementTransactionDetailLogisticCtl)ctlLogistic).OnVoidAllChargesDt(ctx, TransactionID);
                            ((PatientManagementTransactionDetailLogisticReturnCtl)ctlLogisticReturn).OnVoidAllChargesDt(ctx, TransactionID);

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

                            #region Update Service Order
                            if (entity.ServiceOrderID != null)
                            {
                                ServiceOrderHd orderHd = serviceHdDao.Get((int)entity.ServiceOrderID);
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

                                    List<ServiceOrderDt> lstDt = BusinessLayer.GetServiceOrderDtList(string.Format("ServiceOrderID = {0} AND IsDeleted = 0", entity.ServiceOrderID.ToString()), ctx);
                                    foreach (ServiceOrderDt orderDt in lstDt)
                                    {
                                        if (orderDt != null && orderDt.GCServiceOrderStatus != Constant.TestOrderStatus.CANCELLED)
                                        {
                                            if (!orderDt.IsDeleted)
                                            {
                                                if (hdnIsDischarges.Value == "1")
                                                {
                                                    orderDt.GCServiceOrderStatus = Constant.TestOrderStatus.CANCELLED;
                                                    orderDt.GCVoidReason = Constant.DeleteReason.OTHER;
                                                    orderDt.VoidReason = "Linked transaction was deleted";
                                                }
                                                else
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
                            #endregion

                            #region Bed Charges RSDOSKA

                            if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
                            {
                                string filterBedCharges = string.Format("TransactionID = {0}", entity.TransactionID);
                                List<RegistrationBedCharges> lstRegBedCharges = BusinessLayer.GetRegistrationBedChargesList(filterBedCharges, ctx);
                                if (lstRegBedCharges.Count > 0)
                                {
                                    RegistrationBedCharges regBedCharges = lstRegBedCharges.FirstOrDefault();
                                    regBedCharges.TransactionID = null;
                                    regBedCharges.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    bedChargesDao.Update(regBedCharges);
                                }
                            }

                            #endregion

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
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.API.Model;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class TransactionPageCharges : BasePageTrxPatientManagement
    {
        //vConsultVisit entityCV = null;

        public override string OnGetMenuCode()
        {
            string id = Page.Request.QueryString["id"];
            string menuCode = Constant.MenuCode.Outpatient.PATIENT_PAGE_CHARGES;
            switch (hdnVisitDepartmentID.Value)
            {
                case Constant.Facility.INPATIENT: menuCode = Constant.MenuCode.Inpatient.PATIENT_TRANSACTION_PAGE_CHARGES; break;
                case Constant.Facility.EMERGENCY: menuCode = Constant.MenuCode.EmergencyCare.PATIENT_TRANSACTION_PAGE_CHARGES; break;
                case Constant.Facility.OUTPATIENT: menuCode = Constant.MenuCode.Outpatient.PATIENT_PAGE_CHARGES; break;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        menuCode = Constant.MenuCode.Imaging.PATIENT_TRANSACTION_PAGE_CHARGES;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        menuCode = Constant.MenuCode.Laboratory.PATIENT_TRANSACTION_PAGE_CHARGES;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                        menuCode = Constant.MenuCode.Radiotheraphy.PATIENT_TRANSACTION_PAGE_CHARGES;
                    else
                        menuCode = Constant.MenuCode.MedicalDiagnostic.PATIENT_TRANSACTION_PAGE_CHARGES;
                    break;
                default: menuCode = Constant.MenuCode.Outpatient.PATIENT_PAGE_CHARGES; break;
            }
            if (hdnPageTitle.Value == string.Empty)
                hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", menuCode)).FirstOrDefault().MenuCaption;
            return menuCode;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = (hdnGCRegistrationStatus.Value != Constant.VisitStatus.CLOSED && !AppSession.RegisteredPatient.IsLockDown);
            IsAllowSave = !AppSession.RegisteredPatient.IsLockDown;
            IsAllowVoid = !AppSession.RegisteredPatient.IsLockDown;
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
            if (hdnIsCheckedAIOTransaction.Value == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
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
            return GetLabel("Ruang Perawatan");
        }

        protected string GetServiceUnitFilterFilterExpression()
        {
            if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
            {
                string hsuID = "";
                List<PatientTransfer> entityPT = BusinessLayer.GetPatientTransferList(string.Format("RegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID));
                if (entityPT.Count > 0)
                {
                    foreach (PatientTransfer pt in entityPT)
                    {
                        hsuID += pt.FromHealthcareServiceUnitID.ToString() + "," + pt.ToHealthcareServiceUnitID.ToString() + ",";
                    }
                    hsuID = hsuID.Substring(0, hsuID.Length - 1);
                    return string.Format("HealthcareID = '{0}' AND HealthcareServiceUnitID IN ({1}) AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, hsuID);
                }
                else
                {
                    return string.Format("HealthcareID = '{0}' AND HealthcareServiceUnitID = {1} AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, AppSession.RegisteredPatient.HealthcareServiceUnitID);
                }
            }
            return string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND HealthcareServiceUnitID NOT IN ({2},{3}) AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, Constant.Facility.DIAGNOSTIC, hdnHSUImagingID.Value, hdnHSULaboratoryID.Value);
        }

        protected override void InitializeDataControl()
        {
            List<SettingParameterDt> lstSettingParameter = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                                                                                                        AppSession.UserLogin.HealthcareID,
                                                                                                        Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID,
                                                                                                        Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID,
                                                                                                        Constant.SettingParameter.MD_IS_USING_MULTIVISIT_SCHEDULE,
                                                                                                        Constant.SettingParameter.RT0001));
            string laboratoryID = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).ParameterValue;
            string imagingID = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID).ParameterValue;
            hdnIsServiceUnitMultiVisitSchedule.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.MD_IS_USING_MULTIVISIT_SCHEDULE).ParameterValue;

            List<vHealthcareServiceUnitCustom> lstHealthcareServiceUnit = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("HealthcareID = '{0}' AND ServiceUnitID IN ({1},{2}) AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, laboratoryID, imagingID));
            hdnHSUImagingID.Value = lstHealthcareServiceUnit.FirstOrDefault(p => p.ServiceUnitID == Convert.ToInt32(imagingID)).HealthcareServiceUnitID.ToString();
            hdnHSULaboratoryID.Value = lstHealthcareServiceUnit.FirstOrDefault(p => p.ServiceUnitID == Convert.ToInt32(laboratoryID)).HealthcareServiceUnitID.ToString();

            hdnRadioteraphyUnitID.Value = AppSession.RT0001;

            vConsultVisit9 entity = BusinessLayer.GetvConsultVisit9List(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();

            if (hdnIsServiceUnitMultiVisitSchedule.Value == "1")
            {
                vHealthcareServiceUnitCustom hsu = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("HealthcareServiceUnitID = {0}", entity.HealthcareServiceUnitID)).FirstOrDefault();
                if (hsu.IsAllowMultiVisitSchedule)
                {
                    trIsCopyMultiVisitSchedule.Style.Remove("display");
                }
            }

            if (entity.IsLaboratoryUnit)
            {
                hdnIsLaboratoryUnit.Value = "1";
                trPATest.Style.Remove("display");
            }
            else
            {
                hdnIsLaboratoryUnit.Value = "0";
                trPATest.Style.Add("display", "none");
            }
            hdnRegistrationID.Value = entity.RegistrationID.ToString();
            hdnVisitID.Value = entity.VisitID.ToString();
            hdnMRN.Value = entity.MRN.ToString();
            hdnGCRegistrationStatus.Value = entity.GCVisitStatus;
            hdnAppointmentID.Value = entity.AppointmentID.ToString();
            hdnRegistrationPhysicianID.Value = hdnPhysicianID.Value = entity.ParamedicID.ToString();
            hdnPhysicianCode.Value = entity.ParamedicCode;
            hdnPhysicianName.Value = entity.ParamedicName;
            hdnIsParturition.Value = Convert.ToString(entity.IsParturition);
            hdnBusinessPartnerID.Value = entity.BusinessPartnerID.ToString();
            hdnGCCustomerType.Value = entity.GCCustomerType;
            hdnClassID.Value = entity.ChargeClassID.ToString();
            hdnVisitDepartmentID.Value = hdnDepartmentID.Value = entity.DepartmentID;
            hdnDefaultLocationID.Value = hdnLocationID.Value = entity.LocationID.ToString();
            hdnDefaultLogisticLocationID.Value = hdnLogisticLocationID.Value = entity.LogisticLocationID.ToString();
            hdnDefaultHealthcareServiceUnitID.Value = hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
            if (hdnHealthcareServiceUnitID.Value == hdnHSUImagingID.Value || hdnHealthcareServiceUnitID.Value == hdnHSULaboratoryID.Value)
            {
                btnGoBillSummary.Style.Remove("display");
            }

            hdnReferrerParamedicID.Value = entity.ReferrerParamedicID != null ? entity.ReferrerParamedicID.ToString() : "";

            hdnIsHasAIOPackage.Value = entity.IsHasAIOPackage ? "1" : "0";

            if (hdnIsHasAIOPackage.Value == "1")
            {
                trIsAIOTransaction.Attributes.Remove("style");
            }
            else
            {
                trIsAIOTransaction.Attributes.Add("style", "display:none");
            }

            string transactionNo = string.Empty;

            if (Page.Request.QueryString.Count > 0)
            {
                hdnTransactionHdID.Value = Page.Request.QueryString["id"];
                PatientChargesHd entityHd = BusinessLayer.GetPatientChargesHd(Convert.ToInt32(hdnTransactionHdID.Value));
                transactionNo = entityHd != null ? entityHd.TransactionNo : string.Empty;
            }
            else
            {
                hdnTransactionHdID.Value = "0";
            }

            if (!string.IsNullOrEmpty(transactionNo))
            {
                IsLoadFirstRecord = true;
                string filterExpression = GetFilterExpression();
                pageIndexFirstLoad = BusinessLayer.GetPatientChargesHdRowIndex(filterExpression, transactionNo, "TransactionID DESC");
            }
            else
            {
                IsLoadFirstRecord = (OnGetRowCount() > 0);
            }

            if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
                trServiceUnit.Attributes.Remove("style");

            string moduleName = Helper.GetModuleName();
            string ModuleID = Helper.GetModuleID(moduleName);
            GetUserMenuAccess menu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault();
            string CRUDMode = menu.CRUDMode;
            hdnIsAllowVoid.Value = CRUDMode.Contains("X") ? "1" : "0";

            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format(
                          "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
                          AppSession.UserLogin.HealthcareID, //0
                          Constant.SettingParameter.IS_AUTOMATICALLY_SEND_TO_BRIDGING, //1
                          Constant.SettingParameter.SA0168 //2
                          ));
            hdnIsAutomaticallySendToRIS.Value = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.IS_AUTOMATICALLY_SEND_TO_BRIDGING).FirstOrDefault().ParameterValue;
            hdnIsShowItemNotificationWhenProposed.Value = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.SA0168).FirstOrDefault().ParameterValue;

            hdnTransactionHdID.Value = "0";
            ((PatientManagementTransactionDetailServiceCtl)ctlService).OnAddRecord();
            ((PatientManagementTransactionDetailDrugMSCtl)ctlDrugMS).OnAddRecord();
            ((PatientManagementTransactionDetailDrugMSReturnCtl)ctlDrugMSReturn).OnAddRecord();
            ((PatientManagementTransactionDetailLogisticCtl)ctlLogistic).OnAddRecord();
            ((PatientManagementTransactionDetailLogisticReturnCtl)ctlLogisticReturn).OnAddRecord();
            //txtServiceCode.Attributes.Add("validationgroup", "mpTrxService");
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
            SetControlEntrySetting(txtReferenceNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsPATest, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(chkIsAutoTransaction, new ControlEntrySetting(false, false, false, false));
            chkIsAutoTransaction.Checked = false;
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
            hdnGCTransactionStatus.Value = Constant.TransactionStatus.OPEN;
            if (Page.Request.QueryString["id"] == "md" || hdnDepartmentID.Value == Constant.Facility.INPATIENT)
                trServiceUnit.Attributes.Remove("style");

            lblServiceUnit.Attributes.Remove("class");
            lblServiceUnit.Attributes.Add("class", "lblLink lblMandatory");

            trIsAutoTransaction.Attributes.Add("style", "display:none");

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
            divProposedBy.InnerHtml = string.Empty;
            divProposedDate.InnerHtml = string.Empty;
            divVoidBy.InnerHtml = string.Empty;
            divVoidDate.InnerHtml = string.Empty;
            divLastUpdatedBy.InnerHtml = string.Empty;
            divLastUpdatedDate.InnerHtml = string.Empty;
            divVoidReason.InnerHtml = string.Empty;
            trProposedBy.Style.Add("display", "none");
            trProposedDate.Style.Add("display", "none");
            trVoidBy.Style.Add("display", "none");
            trVoidDate.Style.Add("display", "none");
            trVoidReason.Style.Add("display", "none");

            if (hdnIsServiceUnitMultiVisitSchedule.Value == "1")
            {
                vHealthcareServiceUnitCustom hsu = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("HealthcareServiceUnitID = {0}", AppSession.RegisteredPatient.HealthcareServiceUnitID)).FirstOrDefault();
                if (hsu.IsAllowMultiVisitSchedule)
                {
                    trIsCopyMultiVisitSchedule.Style.Remove("display");
                    chkIsCopyMultiVisitSchedule.Checked = false;
                }
            }
        }

        #region Load Entity
        protected string GetFilterExpression()
        {
            string filterExpression = "";
            if (Page.Request.QueryString["id"] == "md")
                filterExpression = string.Format("VisitID = {0} AND DepartmentID = '{1}' AND HealthcareServiceUnitID NOT IN ({2},{3})", hdnVisitID.Value, Constant.Facility.DIAGNOSTIC, hdnHSUImagingID.Value, hdnHSULaboratoryID.Value);
            else if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
                filterExpression += string.Format("VisitID = {0} AND DepartmentID = '{1}'", hdnVisitID.Value, Constant.Facility.INPATIENT);
            else
                filterExpression = string.Format("VisitID = {0} AND HealthcareServiceUnitID = {1}", hdnVisitID.Value, hdnHealthcareServiceUnitID.Value);

            filterExpression += string.Format(" AND TransactionCode != '{0}'", Constant.TransactionCode.IP_PATIENT_ACCOMPANY_CHARGES);
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

                SetControlEntrySetting(txtReferenceNo, new ControlEntrySetting(true, false, false));
                SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, false, false));
                SetControlEntrySetting(chkIsPATest, new ControlEntrySetting(false, false, false));
            }
            else
            {
                SetControlEntrySetting(txtReferenceNo, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(chkIsPATest, new ControlEntrySetting(true, true, false));
            }

            lblServiceUnit.Attributes.Remove("class");
            lblServiceUnit.Attributes.Add("class", "lblDisabled");
            hdnGCTransactionStatus.Value = entity.GCTransactionStatus;
            hdnTransactionHdID.Value = entity.TransactionID.ToString();
            txtTransactionNo.Text = entity.TransactionNo;
            txtReferenceNo.Text = entity.ReferenceNo;
            txtTransactionDate.Text = entity.TransactionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTransactionTime.Text = entity.TransactionTime;
            hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
            txtRemarks.Text = entity.Remarks;

            chkIsPATest.Checked = entity.IsPathologicalAnatomyTest;

            if (Page.Request.QueryString["id"] == "md" || hdnDepartmentID.Value == Constant.Facility.INPATIENT)
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

            if (hdnIsServiceUnitMultiVisitSchedule.Value == "1")
            {
                vHealthcareServiceUnitCustom hsu = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("HealthcareServiceUnitID = {0}", entity.HealthcareServiceUnitID)).FirstOrDefault();
                if (hsu.IsAllowMultiVisitSchedule)
                {
                    if (!string.IsNullOrEmpty(entity.TransactionNo))
                    {
                        trIsCopyMultiVisitSchedule.Style.Add("display", "none");
                    }
                    else
                    {
                        trIsCopyMultiVisitSchedule.Style.Remove("display");
                    }
                }
            }


            ((PatientManagementTransactionDetailServiceCtl)ctlService).InitializeTransactionControl(flagHaveCharges, hdnIsServiceUnitMultiVisitSchedule.Value);
            ((PatientManagementTransactionDetailDrugMSCtl)ctlDrugMS).InitializeTransactionControl(flagHaveCharges);
            ((PatientManagementTransactionDetailDrugMSReturnCtl)ctlDrugMSReturn).InitializeTransactionControl(flagHaveCharges);
            ((PatientManagementTransactionDetailLogisticCtl)ctlLogistic).InitializeTransactionControl(flagHaveCharges);
            ((PatientManagementTransactionDetailLogisticReturnCtl)ctlLogisticReturn).InitializeTransactionControl(flagHaveCharges);

            divCreatedBy.InnerHtml = entity.CreatedByName;
            divCreatedDate.InnerHtml = entity.CreatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            divLastUpdatedBy.InnerHtml = entity.LastUpdatedByName;
            if (entity.LastUpdatedDate != null && entity.LastUpdatedDate.ToString(Constant.FormatString.DATE_TIME_FORMAT) != "01 January 1900 00:00:00")
            {
                divLastUpdatedDate.InnerHtml = entity.LastUpdatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }
            else
            {
                divLastUpdatedDate.InnerHtml = string.Empty;
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
                        if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                            entityHd.TransactionCode = Constant.TransactionCode.IMAGING_CHARGES;
                        else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                            entityHd.TransactionCode = Constant.TransactionCode.LABORATORY_CHARGES;
                        else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                            entityHd.TransactionCode = Constant.TransactionCode.RADIOTHERAPHY_CHARGES;
                        else
                            entityHd.TransactionCode = Constant.TransactionCode.OTHER_DIAGNOSTIC_CHARGES; break;
                    default: entityHd.TransactionCode = Constant.TransactionCode.OP_CHARGES; break;
                }
                entityHd.TransactionDate = Helper.GetDatePickerValue(Request.Form[txtTransactionDate.UniqueID]);
                entityHd.TransactionTime = Request.Form[txtTransactionTime.UniqueID];
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
                transactionID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);

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
                switch (hdnDepartmentID.Value)
                {
                    case Constant.Facility.INPATIENT: entityHd.TransactionCode = Constant.TransactionCode.IP_CHARGES; break;
                    case Constant.Facility.EMERGENCY: entityHd.TransactionCode = Constant.TransactionCode.ER_CHARGES; break;
                    case Constant.Facility.DIAGNOSTIC:
                        if (entityHd.HealthcareServiceUnitID != Convert.ToInt32(hdnHSUImagingID.Value))
                            entityHd.TransactionCode = Constant.TransactionCode.OTHER_DIAGNOSTIC_CHARGES;
                        else
                            entityHd.TransactionCode = Constant.TransactionCode.IMAGING_CHARGES; break;
                    default: entityHd.TransactionCode = Constant.TransactionCode.OP_CHARGES; break;
                }
                entityHd.TransactionDate = Helper.GetDatePickerValue(Request.Form[txtTransactionDate.UniqueID]);
                entityHd.TransactionTime = Request.Form[txtTransactionTime.UniqueID];
                entityHd.IsAIOTransaction = hdnIsCheckedAIOTransaction.Value == "1" ? true : false;
                entityHd.Remarks = txtRemarks.Text;
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
        //            //((PatientManagementTransactionDetailDrugMSReturnCtl)ctlDrugMSReturn).OnVoidAllChargesDt(ctx, TransactionID);
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
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            TestOrderHdDao orderHdDao = new TestOrderHdDao(ctx);
            TestOrderDtDao orderDtDao = new TestOrderDtDao(ctx);
            ItemServiceDao itsDao = new ItemServiceDao(ctx);
            ItemMasterDao masterDao = new ItemMasterDao(ctx);
            PatientChargesHdInfoDao entityHdInfoDao = new PatientChargesHdInfoDao(ctx);
            PatientChargesDtInfoDao entityDtInfoDao = new PatientChargesDtInfoDao(ctx);
            VisitPackageBalanceHdDao packageBalanceHdDao = new VisitPackageBalanceHdDao(ctx);
            VisitPackageBalanceDtDao packageBalanceDtDao = new VisitPackageBalanceDtDao(ctx);
            PatientDao patientDao = new PatientDao(ctx);

            try
            {
                PatientChargesHd entity = entityHdDao.Get(Convert.ToInt32(hdnTransactionHdID.Value));
                PatientChargesHdInfo hdInfo = entityHdInfoDao.Get(entity.TransactionID);
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
                                            var result1 = SendOrderToRIS(0, Convert.ToInt32(hdnTransactionHdID.Value));
                                            resultInfo = ((string)result1).Split('|');
                                            break;
                                        case Constant.RIS_Bridging_Protocol.HL7:
                                            var result2 = SendHL7OrderToRIS(0, Convert.ToInt32(hdnTransactionHdID.Value));
                                            resultInfo = ((string)result2).Split('|');
                                            break;
                                        case Constant.RIS_Bridging_Protocol.LINK_DB:
                                            var result3 = SendOrderToRISLinkDB(0, Convert.ToInt32(hdnTransactionHdID.Value));
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

                            int oTestOrderID;
                            if ((entity.TransactionCode == Constant.TransactionCode.LABORATORY_CHARGES || entity.TransactionCode == Constant.TransactionCode.IMAGING_CHARGES || entity.TransactionCode == Constant.TransactionCode.OTHER_DIAGNOSTIC_CHARGES) && entity.TestOrderID == null)
                            {
                                TestOrderHd orderHd = new TestOrderHd();
                                orderHd.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
                                
                                if (!string.IsNullOrEmpty(AppSession.HealthcareServiceUnitID) && AppSession.HealthcareServiceUnitID != "0")
                                {
                                    orderHd.FromHealthcareServiceUnitID = Convert.ToInt32(AppSession.HealthcareServiceUnitID);
                                }
                                else
                                {
                                    orderHd.FromHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                                }

                                if (hdnReferrerParamedicID.Value != null && hdnReferrerParamedicID.Value != "" && hdnReferrerParamedicID.Value != "0")
                                {
                                    orderHd.ParamedicID = Convert.ToInt32(hdnReferrerParamedicID.Value);
                                }
                                else
                                {
                                    orderHd.ParamedicID = Convert.ToInt32(hdnRegistrationPhysicianID.Value);
                                }

                                orderHd.VisitHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                                orderHd.VisitID = entity.VisitID;
                                orderHd.TestOrderDate = entity.TransactionDate;
                                orderHd.TestOrderTime = entity.TransactionTime;
                                orderHd.GCToBePerformed = Constant.ToBePerformed.CURRENT_EPISODE;
                                orderHd.ScheduledDate = entity.TransactionDate;
                                orderHd.ScheduledTime = entity.TransactionTime;
                                orderHd.IsPathologicalAnatomyTest = hdInfo.IsPathologicalAnatomyTest;
                                string remarks = txtRemarks.Text;
                                if (!string.IsNullOrEmpty(remarks))
                                {
                                    string[] strData = remarks.Split('|');
                                    if (strData.Length == 2)
                                    { //format | catatan klinis
                                        remarks = string.Format("{0}", strData[1]);
                                    }
                                }
                                orderHd.Remarks = remarks;
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
                                oTestOrderID = Convert.ToInt32(orderHdDao.InsertReturnPrimaryKeyID(orderHd));

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
                                    orderDtDao.Insert(orderDt);
                                }

                                entity.TestOrderID = oTestOrderID;
                            }

                            if (!string.IsNullOrEmpty(referenceNo))
                            {
                                entity.ReferenceNo = referenceNo;
                            }
                            entity.Remarks = txtRemarks.Text;
                            entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                            entity.ProposedBy = AppSession.UserLogin.UserID;
                            entity.ProposedDate = DateTime.Now;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
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
                PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
                try
                {
                    Int32 TransactionID = Convert.ToInt32(hdnTransactionHdID.Value);
                    PatientChargesHd entity = entityHdDao.Get(TransactionID);
                    if (entity.PatientBillingID == null && entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        ((PatientManagementTransactionDetailServiceCtl)ctlService).OnVoidAllChargesDt(ctx, TransactionID);
                        ((PatientManagementTransactionDetailDrugMSCtl)ctlDrugMS).OnVoidAllChargesDt(ctx, TransactionID);
                        //Waktu Void Obat, Obat-obat Retur udah ikut ke void jadi tidak perlu lagi yang ini.
                        //((PatientManagementTransactionDetailDrugMSReturnCtl)ctlDrugMSReturn).OnVoidAllChargesDt(ctx, TransactionID);
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
                                lstReferringPhysician.Add(new referringPhysician() { refPhyCode = "", refPhyName = "" });
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
        #endregion
    }
}
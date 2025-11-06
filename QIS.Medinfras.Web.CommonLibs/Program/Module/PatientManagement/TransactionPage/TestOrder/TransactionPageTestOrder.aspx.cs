using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.CommonLibs.Service;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class TransactionPageTestOrder : BasePageTrx
    {
        string menuType = string.Empty;

        public override string OnGetMenuCode()
        {
            string menuCode = string.Empty;
            if (string.IsNullOrEmpty(menuType))
            {
                switch (hdnDepartmentID.Value)
                {
                    case Constant.Facility.INPATIENT: menuCode = Constant.MenuCode.Inpatient.PATIENT_TRANSACTION_TEST_ORDER; break;
                    case Constant.Facility.EMERGENCY: menuCode = Constant.MenuCode.EmergencyCare.PATIENT_TRANSACTION_TEST_ORDER; break;
                    case Constant.Facility.DIAGNOSTIC:
                        if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                            menuCode = Constant.MenuCode.Imaging.PATIENT_TRANSACTION_TEST_ORDER;
                        else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                            menuCode = Constant.MenuCode.Laboratory.PATIENT_TRANSACTION_TEST_ORDER;
                        else menuCode = Constant.MenuCode.MedicalDiagnostic.PATIENT_TRANSACTION_TEST_ORDER; break;
                    default: menuCode = Constant.MenuCode.Outpatient.PATIENT_TRANSACTION_TEST_ORDER; break;
                }
            }
            else
            {
                switch (menuType)
                {
                    case "md": menuCode = Constant.MenuCode.MedicalDiagnostic.PATIENT_TRANSACTION_DIAGNOSTIC_ORDER; break;
                    case "er": menuCode = Constant.MenuCode.EmergencyCare.PATIENT_TRANSACTION_DIAGNOSTIC_ORDER; break;
                    case Constant.Module.RADIOTHERAPHY: menuCode = Constant.MenuCode.Radiotheraphy.PATIENT_PAGE_RT_DIAGNOSTIC_ORDER; break;
                    default:
                        break;
                }
            }
            return menuCode;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected string GetServiceUnitFilterFilterExpression()
        {
            return string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND HealthcareServiceUnitID NOT IN ({2},{3},{4}) AND IsDeleted = 0 AND IsUsingRegistration = 1 AND IsUsingJobOrder = 1",
                                            AppSession.UserLogin.HealthcareID,
                                            Constant.Facility.DIAGNOSTIC,
                                            hdnOperatingRoomID.Value,
                                            hdnBloodBankHSUID.Value,
                                            AppSession.RegisteredPatient.HealthcareServiceUnitID
                                        );
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = (hdnGCRegistrationStatus.Value != Constant.VisitStatus.CLOSED && !AppSession.RegisteredPatient.IsLockDown);
            IsAllowSave = !AppSession.RegisteredPatient.IsLockDown;
            IsAllowVoid = !AppSession.RegisteredPatient.IsLockDown;
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString["id"] != null)
            {
                menuType = Page.Request.QueryString["id"];
            }

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            List<SettingParameterDt> lstSettingParameter = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                                    "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}')",
                                                                    AppSession.UserLogin.HealthcareID, //0
                                                                    Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI, //1
                                                                    Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM, //2
                                                                    Constant.SettingParameter.MD_SERVICE_UNIT_OPERATING_THEATRE, //3
                                                                    Constant.SettingParameter.LB_PEMERIKSAAN_LABORATORIUM_HANYA_BPJS, //4
                                                                    Constant.SettingParameter.NOTES_TEST_ORDER_COPY_DIAGNOSE, //5
                                                                    Constant.SettingParameter.MD0018, //6
                                                                    Constant.SettingParameter.TEST_ORDER_HANYA_UNTUK_ITEM_PEMERIKSAAN, //7
                                                                    Constant.SettingParameter.OP0040, //8
                                                                    Constant.SettingParameter.FN_IS_DISPLAY_PRICE_IN_ORDER_AND_SERVICES_QUICKPICKS_NURSE, //9
                                                                    Constant.SettingParameter.EM0075, //10
                                                                    Constant.SettingParameter.MD_IS_USING_MULTIVISIT_SCHEDULE,
                                                                    Constant.SettingParameter.RT0001
                                                                ));

            hdnImagingServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).ParameterValue;
            hdnLaboratoryServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).ParameterValue;
            hdnOperatingRoomID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.MD_SERVICE_UNIT_OPERATING_THEATRE).ParameterValue;
            hdnBloodBankHSUID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.MD0018).ParameterValue;
            hdnIsOnlyBPJSItem.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB_PEMERIKSAAN_LABORATORIUM_HANYA_BPJS).ParameterValue;
            hdnIsNotesCopyDiagnose.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.NOTES_TEST_ORDER_COPY_DIAGNOSE).ParameterValue;
            hdnOrderHanyaItemPemeriksaan.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.TEST_ORDER_HANYA_UNTUK_ITEM_PEMERIKSAAN).ParameterValue;
            hdnPenjaminDisamakanRegistrasi.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.OP0040).ParameterValue;
            hdnIsDisplayPrice.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_IS_DISPLAY_PRICE_IN_ORDER_AND_SERVICES_QUICKPICKS_NURSE).ParameterValue;
            hdnIsQuickPicksUsingForm.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.EM0075).ParameterValue;
            hdnIsUsingMultiVisitScheduleOrder.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.MD_IS_USING_MULTIVISIT_SCHEDULE).ParameterValue;
            hdnRadiotheraphyServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.RT0001).ParameterValue;

            hdnEM0079.Value = AppSession.EM0079;

            if (hdnOperatingRoomID.Value == null || hdnOperatingRoomID.Value == "")
            {
                hdnOperatingRoomID.Value = "0";
            }
            if (hdnBloodBankHSUID.Value == null || hdnBloodBankHSUID.Value == "")
            {
                hdnBloodBankHSUID.Value = "0";
            }
            if (hdnRadiotheraphyServiceUnitID.Value == null || hdnRadiotheraphyServiceUnitID.Value == "")
            {
                hdnRadiotheraphyServiceUnitID.Value = "0";
            }

            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnDepartmentID.Value = AppSession.RegisteredPatient.DepartmentID;
            hdnGCRegistrationStatus.Value = AppSession.RegisteredPatient.GCRegistrationStatus;
            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnClassID.Value = AppSession.RegisteredPatient.ClassID.ToString();
            hdnPatientInformation.Value = string.Format("{0};{1};{2}", AppSession.RegisteredPatient.MedicalNo, AppSession.RegisteredPatient.PatientName, AppSession.RegisteredPatient.ServiceUnitName);

            hdnDefaultVisitParamedicID.Value = AppSession.RegisteredPatient.ParamedicID.ToString();
            hdnDefaultVisitParamedicCode.Value = AppSession.RegisteredPatient.ParamedicCode;
            hdnDefaultVisitParamedicName.Value = AppSession.RegisteredPatient.ParamedicName;

            Registration entityRegistration = BusinessLayer.GetRegistration(Convert.ToInt32(hdnRegistrationID.Value));
            hdnIsBPJSRegistration.Value = entityRegistration.GCCustomerType == Constant.CustomerType.BPJS ? "1" : "0";

            cboToBePerformed.SelectedIndex = 0;

            if (hdnIsNotesCopyDiagnose.Value == "1")
            {
                string filterDiag = string.Format("VisitID = {0} AND ParamedicID = {1} AND IsDeleted = 0 ORDER BY GCDiagnoseType", hdnVisitID.Value, hdnDefaultVisitParamedicID.Value);
                List<vPatientDiagnosis> lstDiagnosis = BusinessLayer.GetvPatientDiagnosisList(filterDiag);

                //Create Diagnosis Summary for : CPOE Clinical Notes
                StringBuilder strDiagnosis = new StringBuilder();
                foreach (var item in lstDiagnosis)
                {
                    if (item.GCDifferentialStatus != Constant.DifferentialDiagnosisStatus.RULED_OUT)
                    {
                        strDiagnosis.AppendLine(string.Format("{0}", item.cfDiagnosisText));
                    }
                }
                hdnDefaultDiagnosa.Value = strDiagnosis.ToString();
            }
            else
            {
                hdnDefaultDiagnosa.Value = "";
            }

            if (hdnImagingServiceUnitID.Value == hdnHealthcareServiceUnitID.Value && AppSession.EM0079 == "0")
            {
                hdnDefaultDiagnosa.Value = hdnRemarks.Value = string.Empty;
            }
            else
            {
                hdnRemarks.Value = hdnDefaultDiagnosa.Value;
            }

            txtRemarksHd.Text = hdnDefaultDiagnosa.Value;

            IsEditable = AppSession.RegisteredPatient.IsLockDown ? false : true;
            BindGridView();

            string moduleName = Helper.GetModuleName();
            string ModuleID = Helper.GetModuleID(moduleName);
            GetUserMenuAccess menu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault();
            string CRUDMode = menu.CRUDMode;
            hdnIsAllowVoid.Value = CRUDMode.Contains("X") ? "1" : "0";

            lblPhysician2.Attributes.Add("style", "display:none");

            Helper.SetControlEntrySetting(txtItemCode, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtPerformDate, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            IsLoadFirstRecord = (OnGetRowCount() > 0);

            hdnDatePickerToday.Value = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnTimeToday.Value = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
        }

        private void BindGridView()
        {
            string filterExpression = "1 = 0";
            if (hdnTestOrderID.Value != "")
                filterExpression = string.Format("TestOrderID = {0} AND IsDeleted = 0 ORDER BY ID DESC", hdnTestOrderID.Value);
            List<vTestOrderDt> lstEntity = BusinessLayer.GetvTestOrderDtList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridView();
                result = "refresh";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected override void SetControlProperties()
        {
            string filterExpression;
            if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
            {
                filterExpression = string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1 AND StandardCodeID != '{1}' ORDER BY StandardCodeID", Constant.StandardCode.TO_BE_PERFORMED, Constant.ToBePerformed.PRIOR_TO_NEXT_VISIT);
            }
            else
            {
                filterExpression = string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1 AND StandardCodeID != '{1}' ORDER BY StandardCodeID", Constant.StandardCode.TO_BE_PERFORMED, Constant.ToBePerformed.SCHEDULLED);
            }

            List<StandardCode> lstToBePerformed = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboToBePerformed, lstToBePerformed, "StandardCodeName", "StandardCodeID");
            cboToBePerformed.SelectedIndex = 0;
        }

        protected override void OnControlEntrySetting()
        {
            hdnRemarks.Value = hdnDefaultDiagnosa.Value;
            SetControlEntrySetting(hdnTestOrderID, new ControlEntrySetting(false, false, false, "0"));

            hdnTestOrderID.Value = "0";

            SetControlEntrySetting(txtTestOderNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtTestOrderDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtTestOrderTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(txtScheduledDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtScheduledTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));

            SetControlEntrySetting(hdnHealthcareServiceUnitID, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtServiceUnitCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtServiceUnitName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnProcedureGroupID, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtProcedureGroupCode, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtProcedureGroupName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnPhysicianID, new ControlEntrySetting(true, false, false, hdnDefaultVisitParamedicID.Value));
            SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, false, true, hdnDefaultVisitParamedicCode.Value));
            SetControlEntrySetting(txtPhysicianName, new ControlEntrySetting(false, false, false, hdnDefaultVisitParamedicName.Value));

            if (hdnTestOrderID.Value == "0")
            {
                SetControlEntrySetting(txtRemarksHd, new ControlEntrySetting(true, false, true, hdnDefaultDiagnosa.Value));
                SetControlEntrySetting(hdnRemarks, new ControlEntrySetting(true, false, true, hdnDefaultDiagnosa.Value));

                txtRemarksHd.Text = hdnDefaultDiagnosa.Value;
                hdnRemarks.Value = hdnDefaultDiagnosa.Value;
            }
            else
            {
                SetControlEntrySetting(txtRemarksHd, new ControlEntrySetting(true, false, true));
                SetControlEntrySetting(hdnRemarks, new ControlEntrySetting(true, false, true));
            }

            SetControlEntrySetting(txtRemarksDt, new ControlEntrySetting(true, true, false, ""));
            SetControlEntrySetting(txtItemQty, new ControlEntrySetting(true, true, true, "1"));
            SetControlEntrySetting(chkIsCITO, new ControlEntrySetting(true, false, false, false));
            SetControlEntrySetting(hdnIsCITOHd, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(chkIsMultiVisitScheduleOrder, new ControlEntrySetting(true, false, false, false));

            SetControlEntrySetting(lblPhysician, new ControlEntrySetting(true, true));
            SetControlEntrySetting(lblPhysician2, new ControlEntrySetting(true, true));
            SetControlEntrySetting(lblServiceUnit, new ControlEntrySetting(true, false));
            SetControlEntrySetting(lblProcedureGroup, new ControlEntrySetting(true, false));

            SetControlEntrySetting(cboToBePerformed, new ControlEntrySetting(true, false, false));
            if (cboToBePerformed.Items.Count > 0)
            {
                cboToBePerformed.SelectedIndex = 0;
            }
        }

        public override void OnAddRecord()
        {
            hdnGCTransactionStatus.Value = Constant.TransactionStatus.OPEN;
            IsEditable = true;
            string filterExpression = "1 = 0";
            List<vTestOrderDt> lstEntity = BusinessLayer.GetvTestOrderDtList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();

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
        }

        public override int OnGetRowCount()
        {
            string filterExpression = string.Format("VisitID = {0} AND HealthcareServiceUnitID NOT IN ({1},{2},{3})",
                                                        hdnVisitID.Value,
                                                        hdnOperatingRoomID.Value,
                                                        hdnBloodBankHSUID.Value,
                                                        AppSession.RegisteredPatient.HealthcareServiceUnitID
                                                    );
            return BusinessLayer.GetvTestOrderHdRowCount(filterExpression);
        }

        #region Load Entity
        protected bool IsEditable = true;
        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = string.Format("VisitID = {0} AND HealthcareServiceUnitID NOT IN ({1},{2})",
                                                        hdnVisitID.Value,
                                                        hdnOperatingRoomID.Value,
                                                        hdnBloodBankHSUID.Value
                                                    );
            vTestOrderHd entity = BusinessLayer.GetvTestOrderHd(filterExpression, PageIndex, " TestOrderID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = string.Format("VisitID = {0} AND HealthcareServiceUnitID NOT IN ({1},{2})",
                                                        hdnVisitID.Value,
                                                        hdnOperatingRoomID.Value,
                                                        hdnBloodBankHSUID.Value
                                                    );
            PageIndex = BusinessLayer.GetvTestOrderHdRowIndex(filterExpression, keyValue, "TestOrderID DESC");
            vTestOrderHd entity = BusinessLayer.GetvTestOrderHd(filterExpression, PageIndex, "TestOrderID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vTestOrderHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                watermarkText = entity.TransactionStatusWatermark;

                SetControlEntrySetting(txtRemarksHd, new ControlEntrySetting(false, false, true));
                SetControlEntrySetting(hdnRemarks, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(lblPhysician, new ControlEntrySetting(false, false));
                SetControlEntrySetting(lblPhysician2, new ControlEntrySetting(false, false));
                SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(false, false, true));
            }
            IsEditable = (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN);

            lblPhysician.Attributes.Remove("class");
            lblServiceUnit.Attributes.Remove("class");

            //Registration entityRegistration = BusinessLayer.GetRegistration(Convert.ToInt32(hdnRegistrationID.Value));
            //IsEditable = entityRegistration.IsLockDown ? false : IsEditable;

            hdnGCTransactionStatus.Value = entity.GCTransactionStatus;
            hdnTestOrderID.Value = entity.TestOrderID.ToString();
            txtTestOderNo.Text = entity.TestOrderNo;
            txtTestOrderDate.Text = entity.TestOrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTestOrderTime.Text = entity.TestOrderTime;
            cboToBePerformed.Value = entity.GCToBePerformed;
            txtScheduledDate.Text = entity.ScheduledDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtScheduledTime.Text = entity.ScheduledTime;
            hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
            if (entity.IsLaboratoryUnit)
            {
                hdnIsLaboratoryUnit.Value = "1";
            }
            else
            {
                hdnIsLaboratoryUnit.Value = "0";
            }
            txtServiceUnitCode.Text = entity.ServiceUnitCode;
            txtServiceUnitName.Text = entity.ServiceUnitName;

            if (entity.HealthcareServiceUnitID == Convert.ToInt32(hdnOperatingRoomID.Value))
            {
                trProcedureGroup.Attributes.Remove("style");
            }
            else
            {
                trProcedureGroup.Attributes.Add("style", "display:none");
            }

            if (entity.IsLaboratoryUnit)
            {
                trLaboratoryOrder.Attributes.Remove("style");
                chkIsPATest.Checked = entity.IsPathologicalAnatomyTest;
                chkIsAdditionalTest.Checked = entity.IsAdditionalOrder;
            }
            else
            {
                trLaboratoryOrder.Attributes.Add("style", "display:none");
                chkIsPATest.Checked = false;
                chkIsAdditionalTest.Checked = false;
            }

            hdnProcedureGroupID.Value = entity.ProcedureGroupID.ToString();
            txtProcedureGroupCode.Text = entity.ProcedureGroupCode;
            txtProcedureGroupName.Text = entity.ProcedureGroupName;

            if (hdnProcedureGroupID.Value != "" && hdnProcedureGroupID.Value != "0")
            {
                IsEditable = false;
            }

            hdnPhysicianID.Value = entity.ParamedicID.ToString();
            txtPhysicianCode.Text = entity.ParamedicCode;
            txtPhysicianName.Text = entity.ParamedicName;
            chkIsCITO.Checked = entity.IsCITO;
            hdnIsCITOHd.Value = entity.IsCITO ? "1" : "0";
            txtRemarksHd.Text = entity.Remarks;
            hdnRemarks.Value = entity.Remarks;
            if (hdnIsUsingMultiVisitScheduleOrder.Value == "1")
            {
                vHealthcareServiceUnitCustom hsu = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("HealthcareServiceUnitID = {0}", entity.HealthcareServiceUnitID)).FirstOrDefault();
                if (hsu.IsAllowMultiVisitSchedule)
                {
                    chkIsMultiVisitScheduleOrder.Checked = entity.IsMultiVisitScheduleOrder;
                    trMultiVisitScheduleOrder.Attributes.Remove("style");
                }
                else
                {
                    trMultiVisitScheduleOrder.Attributes.Add("style", "display:none");
                }
            }
            else
            {
                trMultiVisitScheduleOrder.Attributes.Add("style", "display:none");
            }

            divCreatedBy.InnerHtml = entity.CreatedBy;
            divCreatedDate.InnerHtml = entity.CreatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            divLastUpdatedBy.InnerHtml = entity.LastUpdateBy;
            if (entity.LastUpdatedDate != null && entity.LastUpdatedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
            {
                divLastUpdatedDate.InnerHtml = entity.LastUpdatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }

            if (entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
            {
                divProposedBy.InnerHtml = entity.ProposedBy;
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
                divVoidBy.InnerHtml = entity.VoidBy;
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

            if (entity.GCToBePerformed == Constant.ToBePerformed.PRIOR_TO_NEXT_VISIT)
            {
                lblPhysician.Attributes.Add("style", "display:none");
                lblPhysician2.Attributes.Remove("style");
            }
            else
            {
                lblPhysician.Attributes.Remove("style");
                lblPhysician2.Attributes.Add("style", "display:none");
            }

            BindGridView();
        }
        #endregion

        #region Save Entity
        public void SaveTestOrderHd(IDbContext ctx, ref int testOrderID)
        {
            TestOrderHdDao entityHdDao = new TestOrderHdDao(ctx);
            if (hdnTestOrderID.Value == "0")
            {
                TestOrderHd entityHd = new TestOrderHd();
                entityHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                entityHd.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                if (!string.IsNullOrEmpty(AppSession.HealthcareServiceUnitID))
                {
                    entityHd.FromHealthcareServiceUnitID = Convert.ToInt32(AppSession.HealthcareServiceUnitID);
                }
                else
                {
                    entityHd.FromHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                }
                entityHd.VisitHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                entityHd.TestOrderDate = Helper.GetDatePickerValue(Request.Form[txtTestOrderDate.UniqueID]);
                entityHd.TestOrderTime = Request.Form[txtTestOrderTime.UniqueID];
                entityHd.GCToBePerformed = cboToBePerformed.Value.ToString();
                entityHd.ScheduledDate = Helper.GetDatePickerValue(Request.Form[txtScheduledDate.UniqueID]);
                entityHd.ScheduledTime = Request.Form[txtScheduledTime.UniqueID];
                entityHd.IsCITO = hdnIsCITOHd.Value == "1" ? true : false;
                entityHd.IsMultiVisitScheduleOrder = chkIsMultiVisitScheduleOrder.Checked;
                entityHd.Remarks = hdnRemarks.Value;
                if (hdnProcedureGroupID.Value != "" && hdnProcedureGroupID.Value != "0")
                {
                    entityHd.ProcedureGroupID = Convert.ToInt32(hdnProcedureGroupID.Value);
                }
                entityHd.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);

                if (hdnHealthcareServiceUnitID.Value == hdnImagingServiceUnitID.Value)
                {
                    entityHd.TransactionCode = Constant.TransactionCode.IMAGING_TEST_ORDER;
                }
                else if (hdnIsLaboratoryUnit.Value == "1")
                {
                    entityHd.TransactionCode = Constant.TransactionCode.LABORATORY_TEST_ORDER;
                    entityHd.IsPathologicalAnatomyTest = chkIsPATest.Checked;
                    entityHd.IsAdditionalOrder = chkIsAdditionalTest.Checked;
                }
                else if (hdnHealthcareServiceUnitID.Value == hdnRadiotheraphyServiceUnitID.Value)
                {
                    entityHd.TransactionCode = Constant.TransactionCode.RADIOTHERAPHY_TEST_ORDER;
                    entityHd.IsPathologicalAnatomyTest = false;
                    entityHd.IsAdditionalOrder = false;
                }
                else
                {
                    entityHd.TransactionCode = Constant.TransactionCode.OTHER_TEST_ORDER;
                    entityHd.IsPathologicalAnatomyTest = false;
                    entityHd.IsAdditionalOrder = false;
                }

                entityHd.TestOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.TestOrderDate, ctx);
                entityHd.GCOrderStatus = Constant.OrderStatus.OPEN;
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                entityHd.IsCreatedBySystem = false;

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();

                entityHd.CreatedBy = AppSession.UserLogin.UserID;

                testOrderID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
            }
            else
            {
                testOrderID = Convert.ToInt32(hdnTestOrderID.Value);
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                int testOrderID = 0;
                SaveTestOrderHd(ctx, ref testOrderID);
                ctx.CommitTransaction();
                retval = testOrderID.ToString();
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
            TestOrderHdDao entityHdDao = new TestOrderHdDao(ctx);
            try
            {
                TestOrderHd entity = entityHdDao.Get(Convert.ToInt32(hdnTestOrderID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    if (hdnProcedureGroupID.Value != "" && hdnProcedureGroupID.Value != null && hdnProcedureGroupID.Value != "0")
                    {
                        entity.ProcedureGroupID = Convert.ToInt32(hdnProcedureGroupID.Value);
                    }
                    entity.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
                    entity.IsCITO = hdnIsCITOHd.Value == "1" ? true : false;
                    if (hdnHealthcareServiceUnitID.Value == AppSession.LaboratoryServiceUnitID)
                    {
                        entity.IsPathologicalAnatomyTest = chkIsPATest.Checked;
                        entity.IsAdditionalOrder = chkIsAdditionalTest.Checked;
                    }
                    //entity.Remarks = txtRemarksHd.Text;
                    entity.Remarks = hdnRemarks.Value;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entity);
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
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

        #region Process Detail
        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int testOrderID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                if (hdnEntryID.Value.ToString() != "")
                {
                    testOrderID = Convert.ToInt32(hdnTestOrderID.Value);
                    if (OnSaveEditRecordEntityDt(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecordEntityDt(ref errMessage, ref testOrderID))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "delete")
            {
                testOrderID = Convert.ToInt32(hdnTestOrderID.Value);
                if (OnDeleteEntityDt(ref errMessage, testOrderID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTestOrderID"] = testOrderID.ToString();
        }

        private void ControlToEntity(TestOrderDt entityDt)
        {
            entityDt.ItemID = Convert.ToInt32(hdnItemID.Value);
            entityDt.DiagnoseID = txtDiagnoseID.Text;

            if (hdnParamedicDetailID.Value != "" && hdnParamedicDetailID.Value != "0")
            {
                entityDt.ParamedicID = Convert.ToInt32(hdnParamedicDetailID.Value);
            }

            entityDt.IsCITO = chkIsCITO_Detail.Checked;
            entityDt.Remarks = txtRemarksDt.Text;
            entityDt.ItemQty = Convert.ToDecimal(txtItemQty.Text);
            entityDt.IsCreatedFromOrder = true;
        }

        private bool OnSaveAddRecordEntityDt(ref string errMessage, ref int testOrderID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TestOrderHdDao entityHdDao = new TestOrderHdDao(ctx);
            TestOrderDtDao entityDtDao = new TestOrderDtDao(ctx);
            try
            {
                bool allow = true;
                ItemService iservice = BusinessLayer.GetItemService(Convert.ToInt32(hdnItemID.Value));

                if (hdnIsBPJSRegistration.Value == "1")
                {
                    if (hdnIsOnlyBPJSItem.Value == "1")
                    {
                        if (iservice.IsBPJS)
                        {
                            allow = true;
                        }
                        else
                        {
                            allow = false;
                        }
                    }
                }

                if (allow)
                {
                    SaveTestOrderHd(ctx, ref testOrderID);
                    TestOrderHd entityHd = entityHdDao.Get(testOrderID);
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        TestOrderDt entityDt = new TestOrderDt();
                        ControlToEntity(entityDt);
                        entityDt.TestOrderID = testOrderID;
                        entityDt.CreatedBy = AppSession.UserLogin.UserID;
                        entityDt.GCTestOrderStatus = Constant.TestOrderStatus.OPEN;
                        entityDt.ItemUnit = hdnGCItemUnit.Value;
                        entityDt.Remarks = txtRemarksDt.Text;
                        entityDt.CreatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Insert(entityDt);
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Item tersebut bukan untuk BPJS.";
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

        private bool OnSaveEditRecordEntityDt(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TestOrderHdDao entityHdDao = new TestOrderHdDao(ctx);
            TestOrderDtDao entityDtDao = new TestOrderDtDao(ctx);
            try
            {
                TestOrderHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnTestOrderID.Value));
                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    TestOrderDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                    if (!entityDt.IsDeleted)
                    {
                        ControlToEntity(entityDt);
                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(entityDt);
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
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

        private bool OnDeleteEntityDt(ref string errMessage, int ID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TestOrderHdDao entityHdDao = new TestOrderHdDao(ctx);
            TestOrderDtDao entityDtDao = new TestOrderDtDao(ctx);
            try
            {
                TestOrderHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnTestOrderID.Value));
                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    TestOrderDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                    if (!entityDt.IsDeleted)
                    {
                        entityDt.IsDeleted = true;
                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(entityDt);
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
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

        #region Void Entity
        protected override bool OnVoidRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TestOrderHdDao entityHdDao = new TestOrderHdDao(ctx);
            TestOrderDtDao entityDtDao = new TestOrderDtDao(ctx);
            try
            {
                Int32 TestOrderID = Convert.ToInt32(hdnTestOrderID.Value);
                TestOrderHd entity = entityHdDao.Get(TestOrderID);
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    List<TestOrderDt> lstDt = BusinessLayer.GetTestOrderDtList(string.Format("TestOrderID = {0}", TestOrderID), ctx);
                    foreach (TestOrderDt dt in lstDt)
                    {
                        TestOrderDt entityDt = entityDtDao.Get(dt.ID);
                        entityDt.GCTestOrderStatus = Constant.TestOrderStatus.CANCELLED;
                        entityDt.GCVoidReason = Constant.DeleteReason.OTHER;
                        entityDt.VoidReason = "HEADER IS CANCELLED";
                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(entityDt);
                    }

                    entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    entity.GCOrderStatus = Constant.TestOrderStatus.CANCELLED;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entity.VoidDate = DateTime.Now;
                    entity.VoidBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entity);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
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

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            string[] param = type.Split(';');
            string gcDeleteReason = param[1];
            string reason = param[2];
            bool result = true;

            if (param[0] == "void")
            {
                IDbContext ctx = DbFactory.Configure(true);
                TestOrderHdDao entityHdDao = new TestOrderHdDao(ctx);
                TestOrderDtDao entityDtDao = new TestOrderDtDao(ctx);
                try
                {
                    Int32 TestOrderID = Convert.ToInt32(hdnTestOrderID.Value);
                    TestOrderHd entity = entityHdDao.Get(TestOrderID);
                    if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                        entity.GCOrderStatus = Constant.TestOrderStatus.CANCELLED;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entity.GCVoidReason = gcDeleteReason;
                        if (gcDeleteReason == Constant.DeleteReason.OTHER)
                        {
                            entity.VoidReason = reason;
                        }
                        entity.VoidBy = AppSession.UserLogin.UserID;
                        entity.VoidDate = DateTime.Now;
                        entityHdDao.Update(entity);

                        List<TestOrderDt> lstDt = BusinessLayer.GetTestOrderDtList(string.Format("TestOrderID = {0}", TestOrderID));
                        foreach (TestOrderDt dt in lstDt)
                        {
                            TestOrderDt entityDt = entityDtDao.Get(dt.ID);
                            entityDt.GCTestOrderStatus = Constant.TestOrderStatus.CANCELLED;
                            entityDt.GCVoidReason = Constant.DeleteReason.OTHER;
                            entityDt.VoidReason = "HEADER IS CANCELLED";
                            entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entityDt);
                        }
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
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
        #endregion

        #region Proposed Entity
        protected override bool OnProposeRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TestOrderHdDao entityHdDao = new TestOrderHdDao(ctx);
            TestOrderDtDao entityDtDao = new TestOrderDtDao(ctx);
            try
            {
                string isCetakBukti = "0";
                string printFormat = string.Empty;
                string isSendNotification = "0";
                string isPrintWhenPropose = "0";

                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}')",
                            AppSession.UserLogin.HealthcareID, Constant.SettingParameter.OTOMATIS_CETAK_BUKTI_ORDER_PENUNJANG,
                            Constant.SettingParameter.SA_SISTEM_NOTIFIKASI_ORDER, Constant.SettingParameter.RM_FORMAT_BUKTI_ORDER_PENUNJANG,
                            Constant.SettingParameter.LB_PRINT_AFTER_PROPOSE_ORDER);
                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);

                if (lstParam != null)
                {
                    isCetakBukti = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.OTOMATIS_CETAK_BUKTI_ORDER_PENUNJANG)).FirstOrDefault().ParameterValue;
                    printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_BUKTI_ORDER_PENUNJANG)).FirstOrDefault().ParameterValue;
                    isSendNotification = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.SA_SISTEM_NOTIFIKASI_ORDER)).FirstOrDefault().ParameterValue;
                    isPrintWhenPropose = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.LB_PRINT_AFTER_PROPOSE_ORDER)).FirstOrDefault().ParameterValue;
                }

                //cek apakah catatan klinis/diagnosa kosong
                //if(string.IsNullOrEmpty(txtRemarksHd.Text)){
                if (string.IsNullOrEmpty(hdnRemarks.Value))
                {
                    errMessage = "Catatan Klinis / Diagnosa Tidak Boleh Kosong!";
                    return false;
                }

                Int32 testOrderID = Convert.ToInt32(hdnTestOrderID.Value);
                TestOrderHd entity = entityHdDao.Get(testOrderID);
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    String TestOrderID = Convert.ToString(testOrderID);
                    List<TestOrderDt> lstDt = BusinessLayer.GetTestOrderDtList(string.Format("TestOrderID = {0}", TestOrderID));

                    if (lstDt.Count() == 0)
                    {
                        entity.IsCITO = hdnIsCITOHd.Value == "1" ? true : false;
                        //entity.Remarks = txtRemarksHd.Text;
                        entity.Remarks = hdnRemarks.Value;
                        entity.SendOrderBy = AppSession.UserLogin.UserID;
                        entity.SendOrderDateTime = DateTime.Now;
                        if (hdnIsUsingMultiVisitScheduleOrder.Value == "0")
                        {
                            entity.GCOrderStatus = Constant.TestOrderStatus.RECEIVED;
                            entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                        }
                        else
                        {
                            if (entity.IsMultiVisitScheduleOrder)
                            {
                                entity.GCOrderStatus = Constant.TestOrderStatus.CLOSED;
                                entity.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                            }
                            else
                            {
                                entity.GCOrderStatus = Constant.TestOrderStatus.RECEIVED;
                                entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                            }
                        }
                        entity.ProposedBy = AppSession.UserLogin.UserID;
                        entity.ProposedDate = DateTime.Now;
                        entity.IsPathologicalAnatomyTest = chkIsPATest.Checked;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityHdDao.Update(entity);

                        if (isCetakBukti == "1")
                            PrintOrderReceipt(entity.HealthcareServiceUnitID, testOrderID, printFormat);

                        if (isSendNotification == "1")
                        {
                            try
                            {
                                HealthcareServiceUnit hsu = BusinessLayer.GetHealthcareServiceUnit(Convert.ToInt32(hdnHealthcareServiceUnitID.Value));
                                hdnIPAddress.Value = hsu.IPAddress == null ? string.Empty : hsu.IPAddress;

                                if (!String.IsNullOrEmpty(hdnIPAddress.Value))
                                {
                                    SendNotification(entity);
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                        if (isPrintWhenPropose == "1")
                        {
                            vJobOrderLab entityJob = BusinessLayer.GetvJobOrderLabList(string.Format("TestOrderID = {0}", testOrderID)).FirstOrDefault();
                            if (entityJob != null)
                            {
                                PrintJobOrderLaboratory(entityJob.TestOrderID);
                            }
                        }
                    }
                    else
                    {
                        entity.IsCITO = hdnIsCITOHd.Value == "1" ? true : false;
                        //entity.Remarks = txtRemarksHd.Text;
                        entity.Remarks = hdnRemarks.Value;
                        entity.SendOrderBy = AppSession.UserLogin.UserID;
                        entity.SendOrderDateTime = DateTime.Now;
                        if (hdnIsUsingMultiVisitScheduleOrder.Value == "1")
                        {
                            if (entity.IsMultiVisitScheduleOrder)
                            {
                                entity.GCOrderStatus = Constant.TestOrderStatus.CLOSED;
                                entity.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                            }
                            else
                            {
                                entity.GCOrderStatus = Constant.TestOrderStatus.RECEIVED;
                                entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                            }
                        }
                        else
                        {
                            entity.GCOrderStatus = Constant.TestOrderStatus.RECEIVED;
                            entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                        }
                        entity.IsPathologicalAnatomyTest = chkIsPATest.Checked;
                        entity.ProposedBy = AppSession.UserLogin.UserID;
                        entity.ProposedDate = DateTime.Now;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                        //jika semua testOrderDt IsCito 1 dan testOrderHD IsCito 0 maka testOrderHd IsCito 1
                        int entityDtCitoCount = lstDt.Where(t => !t.IsDeleted && t.GCTestOrderStatus != Constant.TestOrderStatus.CANCELLED && t.IsCITO).ToList().Count;
                        int entityDtValidCount = lstDt.Where(t => !t.IsDeleted && t.GCTestOrderStatus != Constant.TestOrderStatus.CANCELLED).ToList().Count;

                        if (entityDtCitoCount == entityDtValidCount)
                        {
                            entity.IsCITO = true;
                        }

                        foreach (TestOrderDt e in lstDt)
                        {
                            if (!e.IsDeleted && e.GCTestOrderStatus != Constant.TestOrderStatus.CANCELLED)
                            {
                                //cek untuk setiap testOrderDt jika testOrderHD IsCito 1 maka testOrderDt IsCito 1
                                if (entity.IsCITO == true)
                                {
                                    e.IsCITO = true;
                                }

                                //bila remarks di testorderDt blm ada isinya, ambil data dari testorderHd
                                if (e.Remarks == "" || e.Remarks == null)
                                {
                                    //e.Remarks = txtRemarksHd.Text;
                                    e.Remarks = hdnRemarks.Value;
                                }
                                e.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Update(e);
                            }
                        }

                        entityHdDao.Update(entity);

                        if (isCetakBukti == "1")
                            PrintOrderReceipt(entity.HealthcareServiceUnitID, testOrderID, printFormat);

                        if (isSendNotification == "1")
                        {
                            try
                            {
                                HealthcareServiceUnit hsu = BusinessLayer.GetHealthcareServiceUnit(Convert.ToInt32(hdnHealthcareServiceUnitID.Value));
                                hdnIPAddress.Value = hsu.IPAddress == null ? string.Empty : hsu.IPAddress;

                                if (!String.IsNullOrEmpty(hdnIPAddress.Value))
                                {
                                    SendNotification(entity);
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                        if (isPrintWhenPropose == "1")
                        {
                            vJobOrderLab entityJob = BusinessLayer.GetvJobOrderLabList(string.Format("TestOrderID = {0}", testOrderID)).FirstOrDefault();
                            if (entityJob != null)
                            {
                                PrintJobOrderLaboratory(entityJob.TestOrderID);
                            }
                        }
                    }

                    string resultCheckTestOrderGCToBePerformed = onCheckTestOrderGCToBePerformed(testOrderID, ctx);
                    if (String.IsNullOrEmpty(resultCheckTestOrderGCToBePerformed))
                    {
                        ctx.CommitTransaction();

                        if (AppSession.SA0137 == "1")
                        {
                            if (AppSession.SA0133 == Constant.CenterBackConsumerAPI.MEDINFRAS_EMR_V1)
                            {
                                BridgingToMedinfrasV1(1, entity);
                            }
                        }
                    }
                    else
                    {
                        result = false;
                        errMessage = resultCheckTestOrderGCToBePerformed;
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
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

        private void SendNotification(TestOrderHd order)
        {
            StringBuilder sbMessage = new StringBuilder();
            sbMessage.AppendLine(string.Format("No  : {0}", order.TestOrderNo));
            sbMessage.AppendLine(string.Format("Fr  : {0}", string.Format("{0} ({1})", AppSession.RegisteredPatient.ServiceUnitName, txtPhysicianName.Text)));
            sbMessage.AppendLine(string.Format("Px  : {0}", AppSession.RegisteredPatient.PatientName));
            sbMessage.AppendLine(string.Format("PDx :    "));
            sbMessage.AppendLine(string.Format("{0}", order.Remarks));
            TcpClient client = new TcpClient();
            client.Connect(IPAddress.Parse(hdnIPAddress.Value), Convert.ToInt16(hdnPort.Value));
            NetworkStream stream = client.GetStream();
            using (BinaryWriter w = new BinaryWriter(stream))
            {
                using (BinaryReader r = new BinaryReader(stream))
                {
                    w.Write(string.Format(@"{0}", sbMessage.ToString()).ToCharArray());
                }
            }
        }
        #endregion

        private string PrintOrderReceipt(int healthcareServiceUnitID, int orderID, string printFormat)
        {
            string result = string.Empty;
            try
            {
                List<vTestOrderDt> lstOrder = BusinessLayer.GetvTestOrderDtList(String.Format("TestOrderID = {0} AND IsDeleted = 0 ORDER BY ItemName1", orderID));
                Registration oRegistration = BusinessLayer.GetRegistration(AppSession.RegisteredPatient.RegistrationID);
                if (oRegistration != null)
                {
                    vConsultVisit oVisit = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", oRegistration.RegistrationID)).FirstOrDefault();
                    if (oVisit != null)
                    {
                        string printerUrl = BusinessLayer.GetHealthcareServiceUnit(healthcareServiceUnitID).Printer1Url;
                        ZebraPrinting.PrintBuktiOrderPenunjang(oVisit, lstOrder, printerUrl);
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        #region PrintJobOrderLaboratory
        private string PrintJobOrderLaboratory(int id)
        {
            string result = string.Empty;
            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_FORMAT_BUKTI_PENDAFTARAN, Constant.SettingParameter.RM_ALAMAT_PRINTER_BUKTI_PENDAFTARAN, Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                if (lstParam != null)
                {
                    string printerType = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;

                    List<vJobOrderLab> oTestOrderDt = BusinessLayer.GetvJobOrderLabList(string.Format("TestOrderID = {0}", id));
                    if (oTestOrderDt != null)
                    {
                        switch (printerType)
                        {
                            case Constant.PrinterType.THERMAL_RECEIPT_PRINTER:
                                //Get Printer Address
                                string ipAddress = HttpContext.Current.Request.UserHostAddress;

                                filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                    ipAddress, Constant.DirectPrintType.JOB_ORDER_LABORATORIUM);

                                List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);

                                if (lstPrinter.Count > 0)
                                    ZebraPrinting.PrintJobOrderLaboratory(oTestOrderDt, lstPrinter[0].PrinterName);
                                else
                                    result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }
        #endregion

        protected string onCheckTestOrderGCToBePerformed(int orderID, IDbContext ctx)
        {
            string result = "";
            //IDbContext ctx = DbFactory.Configure(true);
            //ctx.CommandType = CommandType.Text;
            //ctx.Command.Parameters.Clear();

            TestOrderHdDao entityHdDao = new TestOrderHdDao(ctx);
            TestOrderDtDao entityDtDao = new TestOrderDtDao(ctx);

            AppointmentDao entityAppointmentDao = new AppointmentDao(ctx);
            ParamedicScheduleDao ParamedicScheduleDao = new ParamedicScheduleDao(ctx);
            ParamedicScheduleDateDao ParamedicScheduleDateDao = new ParamedicScheduleDateDao(ctx);
            ParamedicLeaveScheduleDao ParamedicLeaveScheduleDao = new ParamedicLeaveScheduleDao(ctx);

            DraftTestOrderHdDao DraftTestOrderHdDao = new DraftTestOrderHdDao(ctx);
            DraftTestOrderDtDao DraftTestOrderDtDao = new DraftTestOrderDtDao(ctx);

            Appointment appointment = new Appointment();
            DateTime stAppo = DateTime.Now;
            DateTime stAppoValid = DateTime.Now;
            int hour = 0;
            int minute = 0;
            string startTimeCheck = "";
            string endTimeCheck = "";

            try
            {
                TestOrderHd entityHd = entityHdDao.Get(orderID);
                if (entityHd.GCToBePerformed == Constant.ToBePerformed.PRIOR_TO_NEXT_VISIT)
                {
                    entityHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                    entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entityHd);

                    int registrationID = Convert.ToInt32(hdnRegistrationID.Value);
                    int visitID = Convert.ToInt32(hdnVisitID.Value);
                    int paramedicID = Convert.ToInt32(hdnPhysicianID.Value);
                    int healthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                    DateTime scheduleDate = Helper.GetDatePickerValue(Request.Form[txtScheduledDate.UniqueID]);
                    bool isValid = true;

                    appointment.StartDate = appointment.EndDate = scheduleDate;

                    Int16 daynumber = (Int16)scheduleDate.DayOfWeek;
                    if (daynumber == 0)
                    {
                        daynumber = 7;
                    }

                    #region validate paramedic Schedule
                    vParamedicSchedule obj = BusinessLayer.GetvParamedicScheduleList(string.Format(
                                                                "HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND DayNumber = {2}",
                                                                healthcareServiceUnitID, paramedicID, daynumber), ctx).FirstOrDefault();

                    vParamedicScheduleDate objSchDate = BusinessLayer.GetvParamedicScheduleDateList(string.Format(
                                                                                    "HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND ScheduleDate = '{2}'",
                                                                                    healthcareServiceUnitID, paramedicID, scheduleDate), ctx).FirstOrDefault();
                    List<ParamedicLeaveSchedule> objLeave = BusinessLayer.GetParamedicLeaveScheduleList(string.Format("ParamedicID = {0} AND '{1}' BETWEEN CONVERT(VARCHAR, StartDate, 112) AND CONVERT(VARCHAR, EndDate, 112) AND IsDeleted = 0", paramedicID, scheduleDate.ToString(Constant.FormatString.DATE_FORMAT_112)), ctx);

                    if (objLeave.Count > 0)
                    {
                        result = "Maaf dokter sedang cuti";
                        ctx.RollBackTransaction();
                        ctx.Close();
                        return result;
                    }

                    ValidateParamedicScSchedule(obj, objSchDate);
                    #endregion

                    #region validate Visit Type
                    int visitDuration = 0;

                    List<ParamedicVisitType> lstVisitTypeParamedic = BusinessLayer.GetParamedicVisitTypeList(string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1}", healthcareServiceUnitID, paramedicID));
                    vHealthcareServiceUnitCustom VisitTypeHealthcare = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("HealthcareServiceUnitID = {0}", healthcareServiceUnitID)).FirstOrDefault();
                    if (lstVisitTypeParamedic.Count > 0)
                    {
                        visitDuration = lstVisitTypeParamedic.FirstOrDefault().VisitDuration;
                        appointment.VisitDuration = Convert.ToInt16(visitDuration);
                        appointment.VisitTypeID = lstVisitTypeParamedic.FirstOrDefault().VisitTypeID;
                    }
                    else
                    {
                        if (VisitTypeHealthcare.IsHasVisitType)
                        {
                            List<vServiceUnitVisitType> lstServiceUnitVisitType = BusinessLayer.GetvServiceUnitVisitTypeList(string.Format("HealthcareServiceUnitID = {0}", healthcareServiceUnitID));
                            visitDuration = lstServiceUnitVisitType.FirstOrDefault().VisitDuration;
                            appointment.VisitDuration = Convert.ToInt16(visitDuration);
                            appointment.VisitTypeID = lstServiceUnitVisitType.FirstOrDefault().VisitTypeID;
                        }
                        else
                        {
                            List<VisitType> lstVisitType = BusinessLayer.GetVisitTypeList(string.Format("IsDeleted = 0"));
                            visitDuration = 15;
                            appointment.VisitDuration = Convert.ToInt16(visitDuration);
                            appointment.VisitTypeID = lstVisitType.FirstOrDefault().VisitTypeID;
                        }
                    }
                    #endregion

                    #region Save Appointment
                    int session = 1;
                    if (objSchDate != null)
                    {
                        DateTime objSchStart1 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime1);
                        DateTime objSchStart2 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime2);
                        DateTime objSchStart3 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime3);
                        DateTime objSchStart4 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime4);
                        DateTime objSchStart5 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime5);

                        DateTime objSchEnd1 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime1);
                        DateTime objSchEnd2 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime2);
                        DateTime objSchEnd3 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime3);
                        DateTime objSchEnd4 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime4);
                        DateTime objSchEnd5 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime5);

                        string filterExpression;
                        List<Appointment> lstAppointment;
                        DateTime startAppo, endAppo;

                        if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 != "" && objSchDate.StartTime4 != "" && objSchDate.StartTime5 != "")
                        {
                            #region check slot time 1
                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime1, objSchDate.EndTime1, hdnPhysicianID.Value);
                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                            if (lstAppointment.Count > 0)
                            {
                                //set jam mulai dan jam selesai Appointment
                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                //end

                                appointment.StartTime = stAppo.ToString("HH:mm");
                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                session = 1;
                            }
                            else
                            {
                                int hourTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(0, 2));
                                int minuteTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(3));

                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                appointment.StartTime = objSchDate.StartTime1;
                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                session = 1;
                            }

                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                            if (!objSchDate.IsAppointmentByTimeSlot1)
                            {
                                if (startAppo.TimeOfDay < objSchStart1.TimeOfDay || endAppo.TimeOfDay > objSchEnd1.TimeOfDay)
                                {
                                    if (objSchDate.IsAllowWaitingList1)
                                    {
                                        appointment.StartTime = objSchDate.StartTime1;
                                        appointment.EndTime = objSchDate.StartTime1;
                                        appointment.IsWaitingList = true;
                                        startTimeCheck = objSchDate.StartTime1;
                                        endTimeCheck = objSchDate.EndTime1;

                                        session = 1;
                                    }
                                    else
                                    {
                                        isValid = false;
                                    }
                                }
                                else
                                {
                                    appointment.IsWaitingList = false;
                                    startTimeCheck = objSchDate.StartTime1;
                                    endTimeCheck = objSchDate.EndTime1;

                                    session = 1;
                                }
                            }
                            else
                            {
                                isValid = false;
                            }
                            #endregion

                            #region check slot time 2
                            if (!isValid)
                            {
                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime2, objSchDate.EndTime2, hdnPhysicianID.Value);
                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                if (lstAppointment.Count > 0)
                                {
                                    //set jam mulai dan jam selesai Appointment
                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                    //end

                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                    session = 2;
                                }
                                else
                                {
                                    int hourTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(0, 2));
                                    int minuteTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(3));

                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                    appointment.StartTime = objSchDate.StartTime2;
                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                    session = 2;
                                }

                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                if (!objSchDate.IsAppointmentByTimeSlot2)
                                {
                                    if (startAppo.TimeOfDay < objSchStart2.TimeOfDay || endAppo.TimeOfDay > objSchEnd2.TimeOfDay)
                                    {
                                        if (objSchDate.IsAllowWaitingList2)
                                        {
                                            appointment.StartTime = objSchDate.StartTime2;
                                            appointment.EndTime = objSchDate.StartTime2;
                                            appointment.IsWaitingList = true;
                                            startTimeCheck = objSchDate.StartTime2;
                                            endTimeCheck = objSchDate.EndTime2;

                                            session = 2;
                                        }
                                        else
                                        {
                                            isValid = false;
                                        }
                                    }
                                    else
                                    {
                                        appointment.IsWaitingList = false;
                                        startTimeCheck = objSchDate.StartTime2;
                                        endTimeCheck = objSchDate.EndTime2;

                                        session = 2;
                                    }
                                }
                                else
                                {
                                    isValid = false;
                                }
                            }
                            #endregion

                            #region check slot time 3
                            if (!isValid)
                            {
                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime3, objSchDate.EndTime3, hdnPhysicianID.Value);
                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                if (lstAppointment.Count > 0)
                                {
                                    //set jam mulai dan jam selesai Appointment
                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                    //end

                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                    session = 3;
                                }
                                else
                                {
                                    int hourTemp = Convert.ToInt32(objSchDate.StartTime3.Substring(0, 2));
                                    int minuteTemp = Convert.ToInt32(objSchDate.StartTime3.Substring(3));

                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                    appointment.StartTime = objSchDate.StartTime3;
                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                    session = 3;
                                }

                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                if (!objSchDate.IsAppointmentByTimeSlot3)
                                {
                                    if (startAppo.TimeOfDay < objSchStart3.TimeOfDay || endAppo.TimeOfDay > objSchEnd3.TimeOfDay)
                                    {
                                        if (objSchDate.IsAllowWaitingList3)
                                        {
                                            appointment.StartTime = objSchDate.StartTime3;
                                            appointment.EndTime = objSchDate.StartTime3;
                                            appointment.IsWaitingList = true;
                                            startTimeCheck = objSchDate.StartTime3;
                                            endTimeCheck = objSchDate.EndTime3;

                                            session = 3;
                                        }
                                        else
                                        {
                                            isValid = false;
                                        }
                                    }
                                    else
                                    {
                                        appointment.IsWaitingList = false;
                                        startTimeCheck = objSchDate.StartTime3;
                                        endTimeCheck = objSchDate.EndTime3;

                                        session = 3;
                                    }
                                }
                                else
                                {
                                    isValid = false;
                                }
                            }
                            #endregion

                            #region check slot time 4
                            if (!isValid)
                            {
                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime4, objSchDate.EndTime4, hdnPhysicianID.Value);
                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                if (lstAppointment.Count > 0)
                                {
                                    //set jam mulai dan jam selesai Appointment
                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                    //end

                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                    session = 4;
                                }
                                else
                                {
                                    int hourTemp = Convert.ToInt32(objSchDate.StartTime4.Substring(0, 2));
                                    int minuteTemp = Convert.ToInt32(objSchDate.StartTime4.Substring(3));

                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                    appointment.StartTime = objSchDate.StartTime4;
                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                    session = 4;
                                }

                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                if (!objSchDate.IsAppointmentByTimeSlot4)
                                {
                                    if (startAppo.TimeOfDay < objSchStart4.TimeOfDay || endAppo.TimeOfDay > objSchEnd4.TimeOfDay)
                                    {
                                        if (objSchDate.IsAllowWaitingList4)
                                        {
                                            appointment.StartTime = objSchDate.StartTime4;
                                            appointment.EndTime = objSchDate.StartTime4;
                                            appointment.IsWaitingList = true;
                                            startTimeCheck = objSchDate.StartTime4;
                                            endTimeCheck = objSchDate.EndTime4;

                                            session = 4;
                                        }
                                        else
                                        {
                                            isValid = false;
                                        }
                                    }
                                    else
                                    {
                                        appointment.IsWaitingList = false;
                                        startTimeCheck = objSchDate.StartTime4;
                                        endTimeCheck = objSchDate.EndTime4;

                                        session = 4;
                                    }
                                }
                                else
                                {
                                    isValid = false;
                                }
                            }
                            #endregion

                            #region check slot time 5
                            if (!isValid)
                            {
                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime5, objSchDate.EndTime5, hdnPhysicianID.Value);
                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                if (lstAppointment.Count > 0)
                                {
                                    //set jam mulai dan jam selesai Appointment
                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                    //end

                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                    session = 5;
                                }
                                else
                                {
                                    int hourTemp = Convert.ToInt32(objSchDate.StartTime5.Substring(0, 2));
                                    int minuteTemp = Convert.ToInt32(objSchDate.StartTime5.Substring(3));

                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                    appointment.StartTime = objSchDate.StartTime5;
                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                    session = 5;
                                }

                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                if (!objSchDate.IsAppointmentByTimeSlot5)
                                {
                                    if (startAppo.TimeOfDay < objSchStart5.TimeOfDay || endAppo.TimeOfDay > objSchEnd5.TimeOfDay)
                                    {
                                        if (objSchDate.IsAllowWaitingList5)
                                        {
                                            appointment.StartTime = objSchDate.StartTime5;
                                            appointment.EndTime = objSchDate.StartTime5;
                                            appointment.IsWaitingList = true;
                                            startTimeCheck = objSchDate.StartTime5;
                                            endTimeCheck = objSchDate.EndTime5;

                                            session = 5;
                                        }
                                        else
                                        {
                                            isValid = false;
                                        }
                                    }
                                    else
                                    {
                                        appointment.IsWaitingList = false;
                                        startTimeCheck = objSchDate.StartTime5;
                                        endTimeCheck = objSchDate.EndTime5;

                                        session = 5;
                                    }
                                }
                                else
                                {
                                    isValid = false;
                                }
                            }
                            #endregion
                        }
                        else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 != "" && objSchDate.StartTime4 != "" && objSchDate.StartTime5 == "")
                        {
                            #region check slot time 1
                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime1, objSchDate.EndTime1, hdnPhysicianID.Value);
                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                            if (lstAppointment.Count > 0)
                            {
                                //set jam mulai dan jam selesai Appointment
                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                //end

                                appointment.StartTime = stAppo.ToString("HH:mm");
                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                session = 1;
                            }
                            else
                            {
                                int hourTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(0, 2));
                                int minuteTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(3));

                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                appointment.StartTime = objSchDate.StartTime1;
                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                session = 1;
                            }

                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                            if (!objSchDate.IsAppointmentByTimeSlot1)
                            {
                                if (startAppo.TimeOfDay < objSchStart1.TimeOfDay || endAppo.TimeOfDay > objSchEnd1.TimeOfDay)
                                {
                                    if (objSchDate.IsAllowWaitingList1)
                                    {
                                        appointment.StartTime = objSchDate.StartTime1;
                                        appointment.EndTime = objSchDate.StartTime1;
                                        appointment.IsWaitingList = true;
                                        startTimeCheck = objSchDate.StartTime1;
                                        endTimeCheck = objSchDate.EndTime1;

                                        session = 1;
                                    }
                                    else
                                    {
                                        isValid = false;
                                    }
                                }
                                else
                                {
                                    appointment.IsWaitingList = false;
                                    startTimeCheck = objSchDate.StartTime1;
                                    endTimeCheck = objSchDate.EndTime1;

                                    session = 1;
                                }
                            }
                            else
                            {
                                isValid = false;
                            }
                            #endregion

                            #region check slot time 2
                            if (!isValid)
                            {
                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime2, objSchDate.EndTime2, hdnPhysicianID.Value);
                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                if (lstAppointment.Count > 0)
                                {
                                    //set jam mulai dan jam selesai Appointment
                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                    //end

                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                    session = 2;
                                }
                                else
                                {
                                    int hourTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(0, 2));
                                    int minuteTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(3));

                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                    appointment.StartTime = objSchDate.StartTime2;
                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                    session = 2;
                                }

                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                if (!objSchDate.IsAppointmentByTimeSlot2)
                                {
                                    if (startAppo.TimeOfDay < objSchStart2.TimeOfDay || endAppo.TimeOfDay > objSchEnd2.TimeOfDay)
                                    {
                                        if (objSchDate.IsAllowWaitingList2)
                                        {
                                            appointment.StartTime = objSchDate.StartTime2;
                                            appointment.EndTime = objSchDate.StartTime2;
                                            appointment.IsWaitingList = true;
                                            startTimeCheck = objSchDate.StartTime2;
                                            endTimeCheck = objSchDate.EndTime2;

                                            session = 2;
                                        }
                                        else
                                        {
                                            isValid = false;
                                        }
                                    }
                                    else
                                    {
                                        appointment.IsWaitingList = false;
                                        startTimeCheck = objSchDate.StartTime2;
                                        endTimeCheck = objSchDate.EndTime2;

                                        session = 2;
                                    }
                                }
                                else
                                {
                                    isValid = false;
                                }
                            }
                            #endregion

                            #region check slot time 3
                            if (!isValid)
                            {
                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime3, objSchDate.EndTime3, hdnPhysicianID.Value);
                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                if (lstAppointment.Count > 0)
                                {
                                    //set jam mulai dan jam selesai Appointment
                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                    //end

                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                    session = 3;
                                }
                                else
                                {
                                    int hourTemp = Convert.ToInt32(objSchDate.StartTime3.Substring(0, 2));
                                    int minuteTemp = Convert.ToInt32(objSchDate.StartTime3.Substring(3));

                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                    appointment.StartTime = objSchDate.StartTime3;
                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                    session = 3;
                                }

                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                if (!objSchDate.IsAppointmentByTimeSlot3)
                                {
                                    if (startAppo.TimeOfDay < objSchStart3.TimeOfDay || endAppo.TimeOfDay > objSchEnd3.TimeOfDay)
                                    {
                                        if (objSchDate.IsAllowWaitingList3)
                                        {
                                            appointment.StartTime = objSchDate.StartTime3;
                                            appointment.EndTime = objSchDate.StartTime3;
                                            appointment.IsWaitingList = true;
                                            startTimeCheck = objSchDate.StartTime3;
                                            endTimeCheck = objSchDate.EndTime3;

                                            session = 3;
                                        }
                                        else
                                        {
                                            isValid = false;
                                        }
                                    }
                                    else
                                    {
                                        appointment.IsWaitingList = false;
                                        startTimeCheck = objSchDate.StartTime3;
                                        endTimeCheck = objSchDate.EndTime3;

                                        session = 3;
                                    }
                                }
                                else
                                {
                                    isValid = false;
                                }
                            }
                            #endregion

                            #region check slot time 4
                            if (!isValid)
                            {
                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime4, objSchDate.EndTime4, hdnPhysicianID.Value);
                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                if (lstAppointment.Count > 0)
                                {
                                    //set jam mulai dan jam selesai Appointment
                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                    //end

                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                    session = 4;
                                }
                                else
                                {
                                    int hourTemp = Convert.ToInt32(objSchDate.StartTime4.Substring(0, 2));
                                    int minuteTemp = Convert.ToInt32(objSchDate.StartTime4.Substring(3));

                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                    appointment.StartTime = objSchDate.StartTime4;
                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                    session = 4;
                                }

                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                if (!objSchDate.IsAppointmentByTimeSlot4)
                                {
                                    if (startAppo.TimeOfDay < objSchStart4.TimeOfDay || endAppo.TimeOfDay > objSchEnd4.TimeOfDay)
                                    {
                                        if (objSchDate.IsAllowWaitingList4)
                                        {
                                            appointment.StartTime = objSchDate.StartTime4;
                                            appointment.EndTime = objSchDate.StartTime4;
                                            appointment.IsWaitingList = true;
                                            startTimeCheck = objSchDate.StartTime4;
                                            endTimeCheck = objSchDate.EndTime4;

                                            session = 4;
                                        }
                                        else
                                        {
                                            isValid = false;
                                        }
                                    }
                                    else
                                    {
                                        appointment.IsWaitingList = false;
                                        startTimeCheck = objSchDate.StartTime4;
                                        endTimeCheck = objSchDate.EndTime4;

                                        session = 4;
                                    }
                                }
                                else
                                {
                                    isValid = false;
                                }
                            }
                            #endregion
                        }
                        else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 != "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                        {
                            #region check slot time 1
                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime1, objSchDate.EndTime1, hdnPhysicianID.Value);
                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                            if (lstAppointment.Count > 0)
                            {
                                //set jam mulai dan jam selesai Appointment
                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                //end

                                appointment.StartTime = stAppo.ToString("HH:mm");
                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                session = 1;
                            }
                            else
                            {
                                int hourTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(0, 2));
                                int minuteTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(3));

                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                appointment.StartTime = objSchDate.StartTime1;
                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                session = 1;
                            }

                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                            if (!objSchDate.IsAppointmentByTimeSlot1)
                            {
                                if (startAppo.TimeOfDay < objSchStart1.TimeOfDay || endAppo.TimeOfDay > objSchEnd1.TimeOfDay)
                                {
                                    if (objSchDate.IsAllowWaitingList1)
                                    {
                                        appointment.StartTime = objSchDate.StartTime1;
                                        appointment.EndTime = objSchDate.StartTime1;
                                        appointment.IsWaitingList = true;
                                        startTimeCheck = objSchDate.StartTime1;
                                        endTimeCheck = objSchDate.EndTime1;

                                        session = 1;
                                    }
                                    else
                                    {
                                        isValid = false;
                                    }
                                }
                                else
                                {
                                    appointment.IsWaitingList = false;
                                    startTimeCheck = objSchDate.StartTime1;
                                    endTimeCheck = objSchDate.EndTime1;

                                    session = 1;
                                }
                            }
                            else
                            {
                                isValid = false;
                            }
                            #endregion

                            #region check slot time 2
                            if (!isValid)
                            {
                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime2, objSchDate.EndTime2, hdnPhysicianID.Value);
                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                if (lstAppointment.Count > 0)
                                {
                                    //set jam mulai dan jam selesai Appointment
                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                    //end

                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                    session = 2;
                                }
                                else
                                {
                                    int hourTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(0, 2));
                                    int minuteTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(3));

                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                    appointment.StartTime = objSchDate.StartTime2;
                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                    session = 2;
                                }

                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                if (!objSchDate.IsAppointmentByTimeSlot2)
                                {
                                    if (startAppo.TimeOfDay < objSchStart2.TimeOfDay || endAppo.TimeOfDay > objSchEnd2.TimeOfDay)
                                    {
                                        if (objSchDate.IsAllowWaitingList2)
                                        {
                                            appointment.StartTime = objSchDate.StartTime2;
                                            appointment.EndTime = objSchDate.StartTime2;
                                            appointment.IsWaitingList = true;
                                            startTimeCheck = objSchDate.StartTime2;
                                            endTimeCheck = objSchDate.EndTime2;

                                            session = 2;
                                        }
                                        else
                                        {
                                            isValid = false;
                                        }
                                    }
                                    else
                                    {
                                        appointment.IsWaitingList = false;
                                        startTimeCheck = objSchDate.StartTime2;
                                        endTimeCheck = objSchDate.EndTime2;

                                        session = 2;
                                    }
                                }
                                else
                                {
                                    isValid = false;
                                }
                            }
                            #endregion

                            #region check slot time 3
                            if (!isValid)
                            {
                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime3, objSchDate.EndTime3, hdnPhysicianID.Value);
                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                if (lstAppointment.Count > 0)
                                {
                                    //set jam mulai dan jam selesai Appointment
                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                    //end

                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                    session = 3;
                                }
                                else
                                {
                                    int hourTemp = Convert.ToInt32(objSchDate.StartTime3.Substring(0, 2));
                                    int minuteTemp = Convert.ToInt32(objSchDate.StartTime3.Substring(3));

                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                    appointment.StartTime = objSchDate.StartTime3;
                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                    session = 3;
                                }

                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                if (!objSchDate.IsAppointmentByTimeSlot3)
                                {
                                    if (startAppo.TimeOfDay < objSchStart3.TimeOfDay || endAppo.TimeOfDay > objSchEnd3.TimeOfDay)
                                    {
                                        if (objSchDate.IsAllowWaitingList3)
                                        {
                                            appointment.StartTime = objSchDate.StartTime3;
                                            appointment.EndTime = objSchDate.StartTime3;
                                            appointment.IsWaitingList = true;
                                            startTimeCheck = objSchDate.StartTime3;
                                            endTimeCheck = objSchDate.EndTime3;

                                            session = 3;
                                        }
                                        else
                                        {
                                            isValid = false;
                                        }
                                    }
                                    else
                                    {
                                        appointment.IsWaitingList = false;
                                        startTimeCheck = objSchDate.StartTime3;
                                        endTimeCheck = objSchDate.EndTime3;

                                        session = 3;
                                    }
                                }
                                else
                                {
                                    isValid = false;
                                }
                            }
                            #endregion
                        }
                        else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                        {
                            #region check slot time 1
                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime1, objSchDate.EndTime1, hdnPhysicianID.Value);
                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                            if (lstAppointment.Count > 0)
                            {
                                //set jam mulai dan jam selesai Appointment
                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                //end

                                appointment.StartTime = stAppo.ToString("HH:mm");
                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                session = 1;
                            }
                            else
                            {
                                int hourTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(0, 2));
                                int minuteTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(3));

                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                appointment.StartTime = objSchDate.StartTime1;
                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                session = 1;
                            }

                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                            if (!objSchDate.IsAppointmentByTimeSlot1)
                            {
                                if (startAppo.TimeOfDay < objSchStart1.TimeOfDay || endAppo.TimeOfDay > objSchEnd1.TimeOfDay)
                                {
                                    if (objSchDate.IsAllowWaitingList1)
                                    {
                                        appointment.StartTime = objSchDate.StartTime1;
                                        appointment.EndTime = objSchDate.StartTime1;
                                        appointment.IsWaitingList = true;
                                        startTimeCheck = objSchDate.StartTime1;
                                        endTimeCheck = objSchDate.EndTime1;

                                        session = 1;
                                    }
                                    else
                                    {
                                        isValid = false;
                                    }
                                }
                                else
                                {
                                    appointment.IsWaitingList = false;
                                    startTimeCheck = objSchDate.StartTime1;
                                    endTimeCheck = objSchDate.EndTime1;

                                    session = 1;
                                }
                            }
                            else
                            {
                                isValid = false;
                            }
                            #endregion

                            #region check slot time 2
                            if (!isValid)
                            {
                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime2, objSchDate.EndTime2, hdnPhysicianID.Value);
                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                if (lstAppointment.Count > 0)
                                {
                                    //set jam mulai dan jam selesai Appointment
                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                    //end

                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                    session = 2;
                                }
                                else
                                {
                                    int hourTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(0, 2));
                                    int minuteTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(3));

                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                    appointment.StartTime = objSchDate.StartTime2;
                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                    session = 2;
                                }

                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                if (!objSchDate.IsAppointmentByTimeSlot2)
                                {
                                    if (startAppo.TimeOfDay < objSchStart2.TimeOfDay || endAppo.TimeOfDay > objSchEnd2.TimeOfDay)
                                    {
                                        if (objSchDate.IsAllowWaitingList2)
                                        {
                                            appointment.StartTime = objSchDate.StartTime2;
                                            appointment.EndTime = objSchDate.StartTime2;
                                            appointment.IsWaitingList = true;
                                            startTimeCheck = objSchDate.StartTime2;
                                            endTimeCheck = objSchDate.EndTime2;

                                            session = 2;
                                        }
                                        else
                                        {
                                            isValid = false;
                                        }
                                    }
                                    else
                                    {
                                        appointment.IsWaitingList = false;
                                        startTimeCheck = objSchDate.StartTime2;
                                        endTimeCheck = objSchDate.EndTime2;

                                        session = 2;
                                    }
                                }
                                else
                                {
                                    isValid = false;
                                }
                            }
                            #endregion
                        }
                        else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 == "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                        {
                            #region check slot time 1
                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime1, objSchDate.EndTime1, hdnPhysicianID.Value);
                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                            if (lstAppointment.Count > 0)
                            {
                                //set jam mulai dan jam selesai Appointment
                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                //end

                                appointment.StartTime = stAppo.ToString("HH:mm");
                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                session = 1;
                            }
                            else
                            {
                                int hourTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(0, 2));
                                int minuteTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(3));

                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                appointment.StartTime = objSchDate.StartTime1;
                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                session = 1;
                            }

                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                            if (!objSchDate.IsAppointmentByTimeSlot1)
                            {
                                if (startAppo.TimeOfDay < objSchStart1.TimeOfDay || endAppo.TimeOfDay > objSchEnd1.TimeOfDay)
                                {
                                    if (objSchDate.IsAllowWaitingList1)
                                    {
                                        appointment.StartTime = objSchDate.StartTime1;
                                        appointment.EndTime = objSchDate.StartTime1;
                                        appointment.IsWaitingList = true;
                                        startTimeCheck = objSchDate.StartTime1;
                                        endTimeCheck = objSchDate.EndTime1;

                                        session = 1;
                                    }
                                    else
                                    {
                                        isValid = false;
                                    }
                                }
                                else
                                {
                                    appointment.IsWaitingList = false;
                                    startTimeCheck = objSchDate.StartTime1;
                                    endTimeCheck = objSchDate.EndTime1;

                                    session = 1;
                                }
                            }
                            else
                            {
                                isValid = false;
                            }
                            #endregion
                        }
                    }
                    else if (obj != null && objSchDate == null)
                    {
                        DateTime objStart1 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime1);
                        DateTime objStart2 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime2);
                        DateTime objStart3 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime3);
                        DateTime objStart4 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime4);
                        DateTime objStart5 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime5);

                        DateTime objEnd1 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime1);
                        DateTime objEnd2 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime2);
                        DateTime objEnd3 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime3);
                        DateTime objEnd4 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime4);
                        DateTime objEnd5 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime5);

                        string filterExpression;
                        List<Appointment> lstAppointment;
                        DateTime startAppo, endAppo;

                        if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 != "" && obj.StartTime4 != "" && obj.StartTime5 != "")
                        {
                            #region check slot time 1
                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime1, obj.EndTime1, hdnPhysicianID.Value);
                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                            if (lstAppointment.Count > 0)
                            {
                                //set jam mulai dan jam selesai Appointment
                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                //end

                                appointment.StartTime = stAppo.ToString("HH:mm");
                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                session = 1;
                            }
                            else
                            {
                                int hourTemp = Convert.ToInt32(obj.StartTime1.Substring(0, 2));
                                int minuteTemp = Convert.ToInt32(obj.StartTime1.Substring(3));

                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                appointment.StartTime = obj.StartTime1;
                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                session = 1;
                            }

                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                            if (!obj.IsAppointmentByTimeSlot1)
                            {
                                if (startAppo.TimeOfDay < objStart1.TimeOfDay || endAppo.TimeOfDay > objEnd1.TimeOfDay)
                                {
                                    if (obj.IsAllowWaitingList1)
                                    {
                                        appointment.StartTime = obj.StartTime1;
                                        appointment.EndTime = obj.StartTime1;
                                        appointment.IsWaitingList = true;
                                        startTimeCheck = obj.StartTime1;
                                        endTimeCheck = obj.EndTime1;

                                        session = 1;
                                    }
                                    else
                                    {
                                        isValid = false;
                                    }
                                }
                                else
                                {
                                    appointment.IsWaitingList = false;
                                    startTimeCheck = obj.StartTime1;
                                    endTimeCheck = obj.EndTime1;

                                    session = 1;
                                }
                            }
                            else
                            {
                                isValid = false;
                            }
                            #endregion

                            #region check slot time 2
                            if (!isValid)
                            {
                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime2, obj.EndTime2, hdnPhysicianID.Value);
                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                if (lstAppointment.Count > 0)
                                {
                                    //set jam mulai dan jam selesai Appointment
                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                    //end

                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                    session = 2;
                                }
                                else
                                {
                                    int hourTemp = Convert.ToInt32(obj.StartTime2.Substring(0, 2));
                                    int minuteTemp = Convert.ToInt32(obj.StartTime2.Substring(3));

                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                    appointment.StartTime = obj.StartTime2;
                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                    session = 2;
                                }

                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                if (!obj.IsAppointmentByTimeSlot2)
                                {
                                    if (startAppo.TimeOfDay < objStart2.TimeOfDay || endAppo.TimeOfDay > objEnd2.TimeOfDay)
                                    {
                                        if (obj.IsAllowWaitingList2)
                                        {
                                            appointment.StartTime = obj.StartTime2;
                                            appointment.EndTime = obj.StartTime2;
                                            appointment.IsWaitingList = true;
                                            startTimeCheck = obj.StartTime2;
                                            endTimeCheck = obj.EndTime2;

                                            session = 2;
                                        }
                                        else
                                        {
                                            isValid = false;
                                        }
                                    }
                                    else
                                    {
                                        appointment.IsWaitingList = false;
                                        startTimeCheck = obj.StartTime2;
                                        endTimeCheck = obj.EndTime2;

                                        session = 2;
                                    }
                                }
                                else
                                {
                                    isValid = false;
                                }
                            }
                            #endregion

                            #region check slot time 3
                            if (!isValid)
                            {
                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime3, obj.EndTime3, hdnPhysicianID.Value);
                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                if (lstAppointment.Count > 0)
                                {
                                    //set jam mulai dan jam selesai Appointment
                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                    //end

                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                    session = 3;
                                }
                                else
                                {
                                    int hourTemp = Convert.ToInt32(obj.StartTime3.Substring(0, 2));
                                    int minuteTemp = Convert.ToInt32(obj.StartTime3.Substring(3));

                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                    appointment.StartTime = obj.StartTime3;
                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                    session = 3;
                                }

                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                if (!obj.IsAppointmentByTimeSlot3)
                                {
                                    if (startAppo.TimeOfDay < objStart3.TimeOfDay || endAppo.TimeOfDay > objEnd3.TimeOfDay)
                                    {
                                        if (obj.IsAllowWaitingList3)
                                        {
                                            appointment.StartTime = obj.StartTime3;
                                            appointment.EndTime = obj.StartTime3;
                                            appointment.IsWaitingList = true;
                                            startTimeCheck = obj.StartTime3;
                                            endTimeCheck = obj.EndTime3;

                                            session = 3;
                                        }
                                        else
                                        {
                                            isValid = false;
                                        }
                                    }
                                    else
                                    {
                                        appointment.IsWaitingList = false;
                                        startTimeCheck = obj.StartTime3;
                                        endTimeCheck = obj.EndTime3;

                                        session = 3;
                                    }
                                }
                                else
                                {
                                    isValid = false;
                                }
                            }
                            #endregion

                            #region check slot time 4
                            if (!isValid)
                            {
                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime4, obj.EndTime4, hdnPhysicianID.Value);
                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                if (lstAppointment.Count > 0)
                                {
                                    //set jam mulai dan jam selesai Appointment
                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                    //end

                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                    session = 4;
                                }
                                else
                                {
                                    int hourTemp = Convert.ToInt32(obj.StartTime4.Substring(0, 2));
                                    int minuteTemp = Convert.ToInt32(obj.StartTime4.Substring(3));

                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                    appointment.StartTime = obj.StartTime4;
                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                    session = 4;
                                }

                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                if (!obj.IsAppointmentByTimeSlot4)
                                {
                                    if (startAppo.TimeOfDay < objStart4.TimeOfDay || endAppo.TimeOfDay > objEnd4.TimeOfDay)
                                    {
                                        if (obj.IsAllowWaitingList4)
                                        {
                                            appointment.StartTime = obj.StartTime4;
                                            appointment.EndTime = obj.StartTime4;
                                            appointment.IsWaitingList = true;
                                            startTimeCheck = obj.StartTime4;
                                            endTimeCheck = obj.EndTime4;

                                            session = 4;
                                        }
                                        else
                                        {
                                            isValid = false;
                                        }
                                    }
                                    else
                                    {
                                        appointment.IsWaitingList = false;
                                        startTimeCheck = obj.StartTime4;
                                        endTimeCheck = obj.EndTime4;

                                        session = 4;
                                    }
                                }
                                else
                                {
                                    isValid = false;
                                }
                            }
                            #endregion

                            #region check slot time 5
                            if (!isValid)
                            {
                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime5, obj.EndTime5, hdnPhysicianID.Value);
                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                if (lstAppointment.Count > 0)
                                {
                                    //set jam mulai dan jam selesai Appointment
                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                    //end

                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                    session = 5;
                                }
                                else
                                {
                                    int hourTemp = Convert.ToInt32(obj.StartTime5.Substring(0, 2));
                                    int minuteTemp = Convert.ToInt32(obj.StartTime5.Substring(3));

                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                    appointment.StartTime = obj.StartTime5;
                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                    session = 5;
                                }

                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                if (!obj.IsAppointmentByTimeSlot5)
                                {
                                    if (startAppo.TimeOfDay < objStart5.TimeOfDay || endAppo.TimeOfDay > objEnd5.TimeOfDay)
                                    {
                                        if (obj.IsAllowWaitingList5)
                                        {
                                            appointment.StartTime = obj.StartTime5;
                                            appointment.EndTime = obj.StartTime5;
                                            appointment.IsWaitingList = true;
                                            startTimeCheck = obj.StartTime5;
                                            endTimeCheck = obj.EndTime5;

                                            session = 5;
                                        }
                                        else
                                        {
                                            isValid = false;
                                        }
                                    }
                                    else
                                    {
                                        appointment.IsWaitingList = false;
                                        startTimeCheck = obj.StartTime5;
                                        endTimeCheck = obj.EndTime5;

                                        session = 5;
                                    }
                                }
                                else
                                {
                                    isValid = false;
                                }
                            }
                            #endregion
                        }
                        else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 != "" && obj.StartTime4 != "" && obj.StartTime5 == "")
                        {
                            #region check slot time 1
                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime1, obj.EndTime1, hdnPhysicianID.Value);
                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                            if (lstAppointment.Count > 0)
                            {
                                //set jam mulai dan jam selesai Appointment
                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                //end

                                appointment.StartTime = stAppo.ToString("HH:mm");
                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                session = 1;
                            }
                            else
                            {
                                int hourTemp = Convert.ToInt32(obj.StartTime1.Substring(0, 2));
                                int minuteTemp = Convert.ToInt32(obj.StartTime1.Substring(3));

                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                appointment.StartTime = obj.StartTime1;
                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                session = 1;
                            }

                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                            if (!obj.IsAppointmentByTimeSlot1)
                            {
                                if (startAppo.TimeOfDay < objStart1.TimeOfDay || endAppo.TimeOfDay > objEnd1.TimeOfDay)
                                {
                                    if (obj.IsAllowWaitingList1)
                                    {
                                        appointment.StartTime = obj.StartTime1;
                                        appointment.EndTime = obj.StartTime1;
                                        appointment.IsWaitingList = true;
                                        startTimeCheck = obj.StartTime1;
                                        endTimeCheck = obj.EndTime1;

                                        session = 1;
                                    }
                                    else
                                    {
                                        isValid = false;
                                    }
                                }
                                else
                                {
                                    appointment.IsWaitingList = false;
                                    startTimeCheck = obj.StartTime1;
                                    endTimeCheck = obj.EndTime1;

                                    session = 1;
                                }
                            }
                            else
                            {
                                isValid = false;
                            }
                            #endregion

                            #region check slot time 2
                            if (!isValid)
                            {
                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime2, obj.EndTime2, hdnPhysicianID.Value);
                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                if (lstAppointment.Count > 0)
                                {
                                    //set jam mulai dan jam selesai Appointment
                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                    //end

                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                    session = 2;
                                }
                                else
                                {
                                    int hourTemp = Convert.ToInt32(obj.StartTime2.Substring(0, 2));
                                    int minuteTemp = Convert.ToInt32(obj.StartTime2.Substring(3));

                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                    appointment.StartTime = obj.StartTime2;
                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                    session = 2;
                                }

                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                if (!obj.IsAppointmentByTimeSlot2)
                                {
                                    if (startAppo.TimeOfDay < objStart2.TimeOfDay || endAppo.TimeOfDay > objEnd2.TimeOfDay)
                                    {
                                        if (obj.IsAllowWaitingList2)
                                        {
                                            appointment.StartTime = obj.StartTime2;
                                            appointment.EndTime = obj.StartTime2;
                                            appointment.IsWaitingList = true;
                                            startTimeCheck = obj.StartTime2;
                                            endTimeCheck = obj.EndTime2;

                                            session = 2;
                                        }
                                        else
                                        {
                                            isValid = false;
                                        }
                                    }
                                    else
                                    {
                                        appointment.IsWaitingList = false;
                                        startTimeCheck = obj.StartTime2;
                                        endTimeCheck = obj.EndTime2;

                                        session = 2;
                                    }
                                }
                                else
                                {
                                    isValid = false;
                                }
                            }
                            #endregion

                            #region check slot time 3
                            if (!isValid)
                            {
                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime3, obj.EndTime3, hdnPhysicianID.Value);
                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                if (lstAppointment.Count > 0)
                                {
                                    //set jam mulai dan jam selesai Appointment
                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                    //end

                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                    session = 3;
                                }
                                else
                                {
                                    int hourTemp = Convert.ToInt32(obj.StartTime3.Substring(0, 2));
                                    int minuteTemp = Convert.ToInt32(obj.StartTime3.Substring(3));

                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                    appointment.StartTime = obj.StartTime3;
                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                    session = 3;
                                }

                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                if (!obj.IsAppointmentByTimeSlot3)
                                {
                                    if (startAppo.TimeOfDay < objStart3.TimeOfDay || endAppo.TimeOfDay > objEnd3.TimeOfDay)
                                    {
                                        if (obj.IsAllowWaitingList3)
                                        {
                                            appointment.StartTime = obj.StartTime3;
                                            appointment.EndTime = obj.StartTime3;
                                            appointment.IsWaitingList = true;
                                            startTimeCheck = obj.StartTime3;
                                            endTimeCheck = obj.EndTime3;

                                            session = 3;
                                        }
                                        else
                                        {
                                            isValid = false;
                                        }
                                    }
                                    else
                                    {
                                        appointment.IsWaitingList = false;
                                        startTimeCheck = obj.StartTime3;
                                        endTimeCheck = obj.EndTime3;

                                        session = 3;
                                    }
                                }
                                else
                                {
                                    isValid = false;
                                }
                            }
                            #endregion

                            #region check slot time 4
                            if (!isValid)
                            {
                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime4, obj.EndTime4, hdnPhysicianID.Value);
                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                if (lstAppointment.Count > 0)
                                {
                                    //set jam mulai dan jam selesai Appointment
                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                    //end

                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                    session = 4;
                                }
                                else
                                {
                                    int hourTemp = Convert.ToInt32(obj.StartTime4.Substring(0, 2));
                                    int minuteTemp = Convert.ToInt32(obj.StartTime4.Substring(3));

                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                    appointment.StartTime = obj.StartTime4;
                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                    session = 4;
                                }

                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                if (!obj.IsAppointmentByTimeSlot4)
                                {
                                    if (startAppo.TimeOfDay < objStart4.TimeOfDay || endAppo.TimeOfDay > objEnd4.TimeOfDay)
                                    {
                                        if (obj.IsAllowWaitingList4)
                                        {
                                            appointment.StartTime = obj.StartTime4;
                                            appointment.EndTime = obj.StartTime4;
                                            appointment.IsWaitingList = true;
                                            startTimeCheck = obj.StartTime4;
                                            endTimeCheck = obj.EndTime4;

                                            session = 4;
                                        }
                                        else
                                        {
                                            isValid = false;
                                        }
                                    }
                                    else
                                    {
                                        appointment.IsWaitingList = false;
                                        startTimeCheck = obj.StartTime4;
                                        endTimeCheck = obj.EndTime4;

                                        session = 4;
                                    }
                                }
                                else
                                {
                                    isValid = false;
                                }
                            }
                            #endregion
                        }
                        else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 != "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                        {
                            #region check slot time 1
                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime1, obj.EndTime1, hdnPhysicianID.Value);
                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                            if (lstAppointment.Count > 0)
                            {
                                //set jam mulai dan jam selesai Appointment
                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                //end

                                appointment.StartTime = stAppo.ToString("HH:mm");
                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                session = 1;
                            }
                            else
                            {
                                int hourTemp = Convert.ToInt32(obj.StartTime1.Substring(0, 2));
                                int minuteTemp = Convert.ToInt32(obj.StartTime1.Substring(3));

                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                appointment.StartTime = obj.StartTime1;
                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                session = 1;
                            }

                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                            if (!obj.IsAppointmentByTimeSlot1)
                            {
                                if (startAppo.TimeOfDay < objStart1.TimeOfDay || endAppo.TimeOfDay > objEnd1.TimeOfDay)
                                {
                                    if (obj.IsAllowWaitingList1)
                                    {
                                        appointment.StartTime = obj.StartTime1;
                                        appointment.EndTime = obj.StartTime1;
                                        appointment.IsWaitingList = true;
                                        startTimeCheck = obj.StartTime1;
                                        endTimeCheck = obj.EndTime1;

                                        session = 1;
                                    }
                                    else
                                    {
                                        isValid = false;
                                    }
                                }
                                else
                                {
                                    appointment.IsWaitingList = false;
                                    startTimeCheck = obj.StartTime1;
                                    endTimeCheck = obj.EndTime1;

                                    session = 1;
                                }
                            }
                            else
                            {
                                isValid = false;
                            }
                            #endregion

                            #region check slot time 2
                            if (!isValid)
                            {
                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime2, obj.EndTime2, hdnPhysicianID.Value);
                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                if (lstAppointment.Count > 0)
                                {
                                    //set jam mulai dan jam selesai Appointment
                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                    //end

                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                    session = 2;
                                }
                                else
                                {
                                    int hourTemp = Convert.ToInt32(obj.StartTime2.Substring(0, 2));
                                    int minuteTemp = Convert.ToInt32(obj.StartTime2.Substring(3));

                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                    appointment.StartTime = obj.StartTime2;
                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                    session = 2;
                                }

                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                if (!obj.IsAppointmentByTimeSlot2)
                                {
                                    if (startAppo.TimeOfDay < objStart2.TimeOfDay || endAppo.TimeOfDay > objEnd2.TimeOfDay)
                                    {
                                        if (obj.IsAllowWaitingList2)
                                        {
                                            appointment.StartTime = obj.StartTime2;
                                            appointment.EndTime = obj.StartTime2;
                                            appointment.IsWaitingList = true;
                                            startTimeCheck = obj.StartTime2;
                                            endTimeCheck = obj.EndTime2;

                                            session = 2;
                                        }
                                        else
                                        {
                                            isValid = false;
                                        }
                                    }
                                    else
                                    {
                                        appointment.IsWaitingList = false;
                                        startTimeCheck = obj.StartTime2;
                                        endTimeCheck = obj.EndTime2;

                                        session = 2;
                                    }
                                }
                                else
                                {
                                    isValid = false;
                                }
                            }
                            #endregion

                            #region check slot time 3
                            if (!isValid)
                            {
                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime3, obj.EndTime3, hdnPhysicianID.Value);
                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                if (lstAppointment.Count > 0)
                                {
                                    //set jam mulai dan jam selesai Appointment
                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                    //end

                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                    session = 3;
                                }
                                else
                                {
                                    int hourTemp = Convert.ToInt32(obj.StartTime3.Substring(0, 2));
                                    int minuteTemp = Convert.ToInt32(obj.StartTime3.Substring(3));

                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                    appointment.StartTime = obj.StartTime3;
                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                    session = 3;
                                }

                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                if (!obj.IsAppointmentByTimeSlot3)
                                {
                                    if (startAppo.TimeOfDay < objStart3.TimeOfDay || endAppo.TimeOfDay > objEnd3.TimeOfDay)
                                    {
                                        if (obj.IsAllowWaitingList3)
                                        {
                                            appointment.StartTime = obj.StartTime3;
                                            appointment.EndTime = obj.StartTime3;
                                            appointment.IsWaitingList = true;
                                            startTimeCheck = obj.StartTime3;
                                            endTimeCheck = obj.EndTime3;

                                            session = 3;
                                        }
                                        else
                                        {
                                            isValid = false;
                                        }
                                    }
                                    else
                                    {
                                        appointment.IsWaitingList = false;
                                        startTimeCheck = obj.StartTime3;
                                        endTimeCheck = obj.EndTime3;

                                        session = 3;
                                    }
                                }
                                else
                                {
                                    isValid = false;
                                }
                            }
                            #endregion
                        }
                        else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                        {
                            #region check slot time 1
                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime1, obj.EndTime1, hdnPhysicianID.Value);
                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                            if (lstAppointment.Count > 0)
                            {
                                //set jam mulai dan jam selesai Appointment
                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                //end

                                appointment.StartTime = stAppo.ToString("HH:mm");
                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                session = 1;
                            }
                            else
                            {
                                int hourTemp = Convert.ToInt32(obj.StartTime1.Substring(0, 2));
                                int minuteTemp = Convert.ToInt32(obj.StartTime1.Substring(3));

                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                appointment.StartTime = obj.StartTime1;
                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                session = 1;
                            }

                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                            if (!obj.IsAppointmentByTimeSlot1)
                            {
                                if (startAppo.TimeOfDay < objStart1.TimeOfDay || endAppo.TimeOfDay > objEnd1.TimeOfDay)
                                {
                                    if (obj.IsAllowWaitingList1)
                                    {
                                        appointment.StartTime = obj.StartTime1;
                                        appointment.EndTime = obj.StartTime1;
                                        appointment.IsWaitingList = true;
                                        startTimeCheck = obj.StartTime1;
                                        endTimeCheck = obj.EndTime1;

                                        session = 1;
                                    }
                                    else
                                    {
                                        isValid = false;
                                    }
                                }
                                else
                                {
                                    appointment.IsWaitingList = false;
                                    startTimeCheck = obj.StartTime1;
                                    endTimeCheck = obj.EndTime1;

                                    session = 1;
                                }
                            }
                            else
                            {
                                isValid = false;
                            }
                            #endregion

                            #region check slot time 2
                            if (!isValid)
                            {
                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime2, obj.EndTime2, hdnPhysicianID.Value);
                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                if (lstAppointment.Count > 0)
                                {
                                    //set jam mulai dan jam selesai Appointment
                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                    //end

                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                    session = 2;
                                }
                                else
                                {
                                    int hourTemp = Convert.ToInt32(obj.StartTime2.Substring(0, 2));
                                    int minuteTemp = Convert.ToInt32(obj.StartTime2.Substring(3));

                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                    appointment.StartTime = obj.StartTime2;
                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                    session = 2;
                                }

                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                if (!obj.IsAppointmentByTimeSlot2)
                                {
                                    if (startAppo.TimeOfDay < objStart2.TimeOfDay || endAppo.TimeOfDay > objEnd2.TimeOfDay)
                                    {
                                        if (obj.IsAllowWaitingList2)
                                        {
                                            appointment.StartTime = obj.StartTime2;
                                            appointment.EndTime = obj.StartTime2;
                                            appointment.IsWaitingList = true;
                                            startTimeCheck = obj.StartTime2;
                                            endTimeCheck = obj.EndTime2;

                                            session = 2;
                                        }
                                        else
                                        {
                                            isValid = false;
                                        }
                                    }
                                    else
                                    {
                                        appointment.IsWaitingList = false;
                                        startTimeCheck = obj.StartTime2;
                                        endTimeCheck = obj.EndTime2;

                                        session = 2;
                                    }
                                }
                                else
                                {
                                    isValid = false;
                                }
                            }
                            #endregion
                        }
                        else if (obj.StartTime1 != "" && obj.StartTime2 == "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                        {
                            #region check slot time 1
                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime1, obj.EndTime1, hdnPhysicianID.Value);
                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                            if (lstAppointment.Count > 0)
                            {
                                //set jam mulai dan jam selesai Appointment
                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                //end

                                appointment.StartTime = stAppo.ToString("HH:mm");
                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                session = 1;
                            }
                            else
                            {
                                int hourTemp = Convert.ToInt32(obj.StartTime1.Substring(0, 2));
                                int minuteTemp = Convert.ToInt32(obj.StartTime1.Substring(3));

                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                appointment.StartTime = obj.StartTime1;
                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                session = 1;
                            }

                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                            if (!obj.IsAppointmentByTimeSlot1)
                            {
                                if (startAppo.TimeOfDay < objStart1.TimeOfDay || endAppo.TimeOfDay > objEnd1.TimeOfDay)
                                {
                                    if (obj.IsAllowWaitingList1)
                                    {
                                        appointment.StartTime = obj.StartTime1;
                                        appointment.EndTime = obj.StartTime1;
                                        appointment.IsWaitingList = true;
                                        startTimeCheck = obj.StartTime1;
                                        endTimeCheck = obj.EndTime1;

                                        session = 1;
                                    }
                                    else
                                    {
                                        isValid = false;
                                    }
                                }
                                else
                                {
                                    appointment.IsWaitingList = false;
                                    startTimeCheck = obj.StartTime1;
                                    endTimeCheck = obj.EndTime1;

                                    session = 1;
                                }
                            }
                            else
                            {
                                isValid = false;
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        isValid = false;
                    }

                    #region finalisasi appointment
                    int AppointmentID = 0;
                    if (isValid)
                    {
                        vRegistration reg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", registrationID)).FirstOrDefault();

                        if (reg.MRN == 0 && reg.GuestID != 0)
                        {
                            appointment.IsNewPatient = true;
                            appointment.Name = reg.PatientName;
                            appointment.GCGender = reg.GCGender;
                            appointment.StreetName = reg.StreetName;
                            appointment.PhoneNo = reg.PhoneNo1;
                            appointment.MobilePhoneNo = reg.MobilePhoneNo1;
                            appointment.GCSalutation = reg.GCSalutation;
                        }
                        else if (reg.MRN != 0 && reg.GuestID == 0)
                        {
                            appointment.IsNewPatient = false;
                            appointment.MRN = reg.MRN;
                        }

                        appointment.Notes = string.Format("From Registration {0}", reg.RegistrationNo);
                        appointment.HealthcareServiceUnitID = healthcareServiceUnitID;
                        appointment.ParamedicID = paramedicID;
                        appointment.FromVisitID = visitID;
                        if (hdnPenjaminDisamakanRegistrasi.Value == "1")
                        {
                            if (reg.BusinessPartnerID != 1)
                            {
                                appointment.GCCustomerType = reg.GCCustomerType;
                                appointment.BusinessPartnerID = reg.BusinessPartnerID;
                                appointment.ContractID = reg.ContractID;
                                appointment.CoverageTypeID = reg.CoverageTypeID;
                                appointment.CoverageLimitAmount = reg.CoverageLimitAmount;
                                appointment.IsCoverageLimitPerDay = reg.IsCoverageLimitPerDay;
                                appointment.GCTariffScheme = reg.GCTariffScheme;
                                appointment.IsControlClassCare = reg.IsControlClassCare;
                                appointment.ControlClassID = reg.ControlClassID;
                            }
                            else
                            {
                                appointment.GCCustomerType = Constant.CustomerType.PERSONAL;
                                appointment.BusinessPartnerID = 1;
                                appointment.ContractID = null;
                                appointment.CoverageTypeID = null;
                                appointment.CoverageLimitAmount = 0;
                                appointment.IsCoverageLimitPerDay = false;
                                appointment.GCTariffScheme = null;
                                appointment.IsControlClassCare = false;
                                appointment.ControlClassID = null;
                            }
                        }
                        else
                        {
                            appointment.GCCustomerType = Constant.CustomerType.PERSONAL;
                            appointment.BusinessPartnerID = 1;
                            appointment.ContractID = null;
                            appointment.CoverageTypeID = null;
                            appointment.CoverageLimitAmount = 0;
                            appointment.IsCoverageLimitPerDay = false;
                            appointment.GCTariffScheme = null;
                            appointment.IsControlClassCare = false;
                            appointment.ControlClassID = null;
                        }
                        appointment.EmployeeID = null;
                        appointment.GCAppointmentStatus = Constant.AppointmentStatus.STARTED;
                        appointment.TransactionCode = Constant.TransactionCode.OP_APPOINTMENT;
                        appointment.AppointmentNo = BusinessLayer.GenerateTransactionNo(appointment.TransactionCode, appointment.StartDate);
                        appointment.Session = session;
                        appointment.GCReferrerGroup = Constant.Referrer.DOKTER_RS;
                        appointment.ReferrerParamedicID = AppSession.RegisteredPatient.ParamedicID;

                        //if (appointment.IsWaitingList)
                        //{
                        //    appointment.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNoAppointment(appointment.HealthcareServiceUnitID, Convert.ToInt32(appointment.ParamedicID), appointment.StartDate, startTimeCheck, endTimeCheck, 1) + 1);
                        //}
                        //else
                        //{
                        //    appointment.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNoAppointment(appointment.HealthcareServiceUnitID, Convert.ToInt32(appointment.ParamedicID), appointment.StartDate, startTimeCheck, endTimeCheck, 0) + 1);
                        //}
                        //                    appointment.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(appointment.HealthcareServiceUnitID, Convert.ToInt32(appointment.ParamedicID), appointment.StartDate) + 1);
                        appointment.GCAppointmentMethod = Constant.AppointmentMethod.CALLCENTER;
                        appointment.CreatedBy = AppSession.UserLogin.UserID;

                        //cek jika sudah ada appointment di periode ini blok
                        string filterExpresion = string.Format("ParamedicID = {0} AND StartDate = '{1}' AND GCAppointmentStatus != '{2}' AND StartTime >= '{3}' AND EndTime <= '{4}' AND FromVisitID = {5}", appointment.ParamedicID, appointment.StartDate, Constant.AppointmentStatus.DELETED, startTimeCheck, endTimeCheck, appointment.FromVisitID);
                        List<Appointment> lstAppointmentCheck = BusinessLayer.GetAppointmentList(filterExpresion);

                        if (lstAppointmentCheck.Count < 1)
                        {
                            bool isBPJS = false;
                            if (appointment.GCCustomerType == Constant.CustomerType.BPJS)
                            {
                                isBPJS = true;
                            }
                            appointment.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(appointment.HealthcareServiceUnitID, Convert.ToInt32(appointment.ParamedicID), appointment.StartDate, Convert.ToInt32(appointment.Session), false, isBPJS, 0, ctx));
                            //appointment.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(appointment.HealthcareServiceUnitID, Convert.ToInt32(appointment.ParamedicID), appointment.StartDate, session, 1));
                            AppointmentID = onSaveAppointment(appointment);
                        }
                        else
                        {
                            AppointmentID = lstAppointmentCheck.FirstOrDefault().AppointmentID;
                        }
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                    }
                    #endregion
                    #endregion

                    string filterExpressionOrder = string.Format("TestOrderID = {0} AND IsDeleted = 0 AND GCTestOrderStatus != '{1}'", orderID, Constant.TestOrderStatus.CANCELLED);
                    List<TestOrderDt> lstEntityDt = BusinessLayer.GetTestOrderDtList(filterExpressionOrder, ctx);

                    if (AppointmentID != 0)
                    {
                        DraftTestOrderHd entityDraftHd = new DraftTestOrderHd();
                        entityDraftHd.AppointmentID = AppointmentID;
                        entityDraftHd.HealthcareServiceUnitID = appointment.HealthcareServiceUnitID;
                        entityDraftHd.ParamedicID = appointment.ParamedicID;

                        if (entityDraftHd.HealthcareServiceUnitID == Convert.ToInt32(hdnImagingServiceUnitID.Value))
                        {
                            entityDraftHd.TransactionCode = Constant.TransactionCode.DRAFT_IMAGING_TEST_ORDER;
                        }
                        else if (entityDraftHd.HealthcareServiceUnitID == Convert.ToInt32(hdnLaboratoryServiceUnitID.Value))
                        {
                            entityDraftHd.TransactionCode = Constant.TransactionCode.DRAFT_LABORATORY_TEST_ORDER;
                        }
                        else
                        {
                            entityDraftHd.TransactionCode = Constant.TransactionCode.DRAFT_OTHER_TEST_ORDER;
                        }

                        entityDraftHd.DraftTestOrderDate = appointment.StartDate;
                        entityDraftHd.DraftTestOrderTime = appointment.StartTime;
                        entityDraftHd.GCToBePerformed = Constant.ToBePerformed.CURRENT_EPISODE;
                        entityDraftHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        entityDraftHd.GCVoidReason = null;
                        entityDraftHd.ScheduledDate = appointment.StartDate;
                        entityDraftHd.ScheduledTime = appointment.StartTime;
                        entityDraftHd.DraftTestOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityDraftHd.DraftTestOrderDate, ctx);
                        entityDraftHd.Remarks = string.Format("From Test Order {0} (Date : {1} {2})", Request.Form[txtTestOderNo.UniqueID], Request.Form[txtTestOrderDate.UniqueID], Request.Form[txtTestOrderTime.UniqueID]);
                        entityDraftHd.CreatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        int DraftTestOrderID = DraftTestOrderHdDao.InsertReturnPrimaryKeyID(entityDraftHd);
                        string info = String.Format("Appointment No : {0} Dengan Dokter {1} Pada Tanggal {2} {3}", appointment.AppointmentNo, Request.Form[txtPhysicianName.UniqueID], appointment.StartDate.ToString(Constant.FormatString.DATE_FORMAT), appointment.StartTime);

                        foreach (TestOrderDt e in lstEntityDt)
                        {
                            DraftTestOrderDt draftTestOrderDt = new DraftTestOrderDt();
                            draftTestOrderDt.DraftTestOrderID = DraftTestOrderID;
                            draftTestOrderDt.ItemPackageID = e.ItemPackageID;
                            draftTestOrderDt.ItemID = e.ItemID;
                            draftTestOrderDt.DiagnoseID = e.DiagnoseID;
                            draftTestOrderDt.ParamedicID = e.ParamedicID;
                            draftTestOrderDt.GCDraftTestOrderStatus = Constant.TestOrderStatus.OPEN;
                            draftTestOrderDt.BusinessPartnerID = e.BusinessPartnerID;
                            draftTestOrderDt.ItemQty = e.ItemQty;
                            draftTestOrderDt.ItemUnit = e.ItemUnit;
                            draftTestOrderDt.Remarks = e.Remarks;
                            draftTestOrderDt.IsCITO = e.IsCITO;
                            draftTestOrderDt.IsDeleted = false;
                            draftTestOrderDt.CreatedBy = AppSession.UserLogin.UserID;
                            DraftTestOrderDtDao.Insert(draftTestOrderDt);

                            TestOrderDt testDt = entityDtDao.Get(Convert.ToInt32(e.ID));
                            testDt.Remarks = testDt.Remarks + " / " + string.Format("{0} | ReferenceNo : {1}", info, entityDraftHd.DraftTestOrderNo);
                            entityDtDao.Update(testDt);
                        }
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = "Maaf Tidak Ada Jadwal Praktek Dokter Untuk Tanggal Yang Dipilih";
                        ctx.RollBackTransaction();
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private int onSaveAppointment(Appointment app)
        {
            IDbContext ctx = DbFactory.Configure(true);
            AppointmentDao AppointmentDao = new AppointmentDao(ctx);
            ctx.CommandType = CommandType.Text;
            ctx.Command.Parameters.Clear();
            int retval = AppointmentDao.InsertReturnPrimaryKeyID(app);
            ctx.CommitTransaction();
            return retval;
        }

        private void ValidateParamedicScSchedule(vParamedicSchedule obj, vParamedicScheduleDate objSchDate)
        {
            Int32 ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
            DateTime selectedDate = Helper.GetDatePickerValue(txtScheduledDate.Text);
            List<GetParamedicLeaveScheduleCompare> objLeave = BusinessLayer.GetParamedicLeaveScheduleCompareList(selectedDate.ToString(Constant.FormatString.DATE_FORMAT_112), ParamedicID);

            #region validate time slot
            #region if leave in period
            if (objLeave.FirstOrDefault().DayNumber != 0 && objLeave.Count() > 1)
            {
                #region set time slot Paramedic Schedule
                if (obj != null)
                {
                    if (obj.DayNumber == objLeave.FirstOrDefault().DayNumber && objLeave.FirstOrDefault().Date == selectedDate)
                    {
                        DateTime startTimeDefault = DateTime.Parse("2012-01-28" + ' ' + objLeave.FirstOrDefault().StartTime);

                        DateTime objStart1 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime1);
                        DateTime objStart2 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime2);
                        DateTime objStart3 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime3);
                        DateTime objStart4 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime4);
                        DateTime objStart5 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime5);

                        DateTime objEnd1 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime1);
                        DateTime objEnd2 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime2);
                        DateTime objEnd3 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime3);
                        DateTime objEnd4 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime4);
                        DateTime objEnd5 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime5);

                        if (obj.StartTime5 != "")
                        {

                            if (startTimeDefault.TimeOfDay >= objStart5.TimeOfDay)
                            {
                                obj.EndTime5 = startTimeDefault.ToString("HH:mm");
                            }
                            else if (startTimeDefault.TimeOfDay >= objStart4.TimeOfDay && startTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                            {
                                obj.EndTime4 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime5 = "";
                                obj.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && startTimeDefault.TimeOfDay <= objStart5.TimeOfDay)
                            {
                                obj.StartTime5 = "";
                                obj.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay)
                            {
                                obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                                obj.StartTime5 = "";
                                obj.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && startTimeDefault.TimeOfDay <= objStart4.TimeOfDay)
                            {
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                                obj.StartTime5 = "";
                                obj.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay)
                            {
                                obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                                obj.StartTime5 = "";
                                obj.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart3.TimeOfDay)
                            {
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                                obj.StartTime5 = "";
                                obj.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
                            {
                                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                                obj.StartTime5 = "";
                                obj.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay <= objStart2.TimeOfDay)
                            {
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                                obj.StartTime5 = "";
                                obj.EndTime5 = "";
                            }
                        }
                        else if (obj.StartTime4 != "" && obj.StartTime5 == "")
                        {
                            if (startTimeDefault.TimeOfDay >= objStart4.TimeOfDay && startTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                            {
                                obj.EndTime4 = startTimeDefault.ToString("HH:mm");
                            }
                            else if (startTimeDefault.TimeOfDay >= objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay)
                            {
                                obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && startTimeDefault.TimeOfDay <= objStart4.TimeOfDay)
                            {
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay)
                            {
                                obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart3.TimeOfDay)
                            {
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
                            {
                                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay <= objStart2.TimeOfDay)
                            {
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                            }
                        }
                        else if (obj.StartTime3 != "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                        {
                            if (startTimeDefault.TimeOfDay >= objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay)
                            {
                                obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                            }
                            else if (startTimeDefault.TimeOfDay <= objStart3.TimeOfDay && startTimeDefault.TimeOfDay >= objEnd3.TimeOfDay)
                            {
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objStart3.TimeOfDay && startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay)
                            {
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay)
                            {
                                obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objStart2.TimeOfDay && startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay)
                            {
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objStart2.TimeOfDay && startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay)
                            {
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
                            {
                                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                            }
                        }
                        else if (obj.StartTime2 != "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                        {
                            if (startTimeDefault.TimeOfDay >= objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay)
                            {
                                obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objStart2.TimeOfDay && startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay)
                            {
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objStart2.TimeOfDay && startTimeDefault.TimeOfDay >= objStart1.TimeOfDay)
                            {
                                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objStart2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart1.TimeOfDay)
                            {
                                obj.StartTime1 = "";
                                obj.EndTime2 = "";
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
                            {
                                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                            }
                        }
                        else if (obj.StartTime1 != "" && obj.StartTime2 == "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                        {
                            if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
                            {
                                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            }
                            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay)
                            {
                                obj.StartTime1 = "";
                            }
                        }
                    }
                    else if (obj.DayNumber == objLeave.LastOrDefault().DayNumber && objLeave.LastOrDefault().Date == selectedDate)
                    {
                        DateTime endTime = DateTime.Parse(String.Format("2012-01-28 {0}:15", objLeave.LastOrDefault().EndTime));
                        endTime = endTime.AddMinutes(15);

                        DateTime endTimeDefault = DateTime.Parse("2012-01-28" + ' ' + objLeave.LastOrDefault().EndTime);
                        DateTime objStart1 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime1);
                        DateTime objStart2 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime2);
                        DateTime objStart3 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime3);
                        DateTime objStart4 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime4);
                        DateTime objStart5 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime5);

                        DateTime objEnd1 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime1);
                        DateTime objEnd2 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime2);
                        DateTime objEnd3 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime3);
                        DateTime objEnd4 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime4);
                        DateTime objEnd5 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime5);

                        if (obj.StartTime5 != "")
                        {
                            if (endTimeDefault.TimeOfDay >= objStart5.TimeOfDay)
                            {
                                obj.StartTime5 = endTime.ToString("HH:mm");
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objStart4.TimeOfDay)
                            {
                                obj.StartTime4 = endTime.ToString("HH:mm");
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objStart3.TimeOfDay)
                            {
                                obj.StartTime3 = endTime.ToString("HH:mm");
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objStart2.TimeOfDay)
                            {
                                obj.StartTime2 = endTime.ToString("HH:mm");
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objStart1.TimeOfDay)
                            {
                                obj.StartTime1 = endTime.ToString("HH:mm");
                            }
                        }
                        else if (obj.StartTime4 != "" && obj.StartTime5 == "")
                        {
                            if (endTimeDefault.TimeOfDay >= objStart4.TimeOfDay)
                            {
                                obj.StartTime4 = endTime.ToString("HH:mm");
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objStart3.TimeOfDay)
                            {
                                obj.StartTime3 = endTime.ToString("HH:mm");
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objStart2.TimeOfDay)
                            {
                                obj.StartTime2 = endTime.ToString("HH:mm");
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objStart1.TimeOfDay)
                            {
                                obj.StartTime1 = endTime.ToString("HH:mm");
                            }
                        }
                        else if (obj.StartTime3 != "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                        {
                            if (endTimeDefault.TimeOfDay >= objStart3.TimeOfDay)
                            {
                                obj.StartTime3 = endTime.ToString("HH:mm");
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objStart2.TimeOfDay)
                            {
                                obj.StartTime2 = endTime.ToString("HH:mm");
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
                            {
                                obj.StartTime1 = endTime.ToString("HH:mm");
                            }
                            else if (endTimeDefault.TimeOfDay >= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay)
                            {
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                        }
                        else if (obj.StartTime2 != "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                        {
                            if (endTimeDefault.TimeOfDay >= objStart2.TimeOfDay)
                            {
                                obj.StartTime2 = endTime.ToString("HH:mm");
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objStart1.TimeOfDay)
                            {
                                obj.StartTime1 = endTime.ToString("HH:mm");
                            }
                        }
                        else if (obj.StartTime1 != "" && obj.StartTime2 == "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                        {
                            if (objStart1.TimeOfDay <= endTimeDefault.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
                            {
                                obj.StartTime1 = endTime.ToString("HH:mm");
                            }
                            else if (endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay)
                            {
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                        }
                    }
                    else
                    {
                        List<GetParamedicLeaveScheduleCompare> c = objLeave.Where(t => t.Date == selectedDate).ToList();
                        if (c.Count > 0)
                        {
                            obj.StartTime1 = "";
                            obj.StartTime2 = "";
                            obj.StartTime3 = "";
                            obj.StartTime4 = "";
                            obj.StartTime5 = "";

                            obj.EndTime1 = "";
                            obj.EndTime2 = "";
                            obj.EndTime3 = "";
                            obj.EndTime4 = "";
                            obj.EndTime5 = "";
                        }
                    }
                }
                #endregion

                #region set time slot Paramedic Schedule Date
                if (objSchDate != null)
                {
                    DateTime objSchStart1 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime1);
                    DateTime objSchStart2 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime2);
                    DateTime objSchStart3 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime3);
                    DateTime objSchStart4 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime4);
                    DateTime objSchStart5 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime5);

                    DateTime objSchEnd1 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime1);
                    DateTime objSchEnd2 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime2);
                    DateTime objSchEnd3 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime3);
                    DateTime objSchEnd4 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime4);
                    DateTime objSchEnd5 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime5);

                    if (objSchDate.ScheduleDate == objLeave.FirstOrDefault().Date)
                    {
                        DateTime startTimeDefault = DateTime.Parse(String.Format("{0} {1}:00", objSchDate.cfScheduleDateInString, objLeave.FirstOrDefault().StartTime));
                        if (objSchDate.StartTime5 != "")
                        {

                            if (startTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay)
                            {
                                objSchDate.EndTime5 = startTimeDefault.ToString("HH:mm");
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                            {
                                objSchDate.EndTime4 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime5 = "";
                                objSchDate.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart5.TimeOfDay)
                            {
                                objSchDate.StartTime5 = "";
                                objSchDate.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay)
                            {
                                objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                                objSchDate.StartTime5 = "";
                                objSchDate.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart4.TimeOfDay)
                            {
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                                objSchDate.StartTime5 = "";
                                objSchDate.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay)
                            {
                                objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                                objSchDate.StartTime5 = "";
                                objSchDate.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay)
                            {
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                                objSchDate.StartTime5 = "";
                                objSchDate.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay)
                            {
                                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                                objSchDate.StartTime5 = "";
                                objSchDate.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart2.TimeOfDay)
                            {
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                                objSchDate.StartTime5 = "";
                                objSchDate.EndTime5 = "";
                            }
                        }
                        else if (objSchDate.StartTime4 != "" && objSchDate.StartTime5 == "")
                        {
                            if (startTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                            {
                                objSchDate.EndTime4 = startTimeDefault.ToString("HH:mm");
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay)
                            {
                                objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart4.TimeOfDay)
                            {
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay)
                            {
                                objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay)
                            {
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay)
                            {
                                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart2.TimeOfDay)
                            {
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                            }
                        }
                        else if (objSchDate.StartTime3 != "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                        {
                            if (startTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay)
                            {
                                objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                            }
                            else if (startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay)
                            {
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay)
                            {
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay)
                            {
                                objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay)
                            {
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay)
                            {
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay)
                            {
                                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                            }
                        }
                        else if (objSchDate.StartTime2 != "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                        {
                            if (startTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay)
                            {
                                objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            }
                            else if (startTimeDefault.TimeOfDay <= objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay)
                            {
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay)
                            {
                                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay)
                            {
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay)
                            {
                                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                            }
                        }
                        else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 == "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                        {
                            if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay)
                            {
                                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay)
                            {
                                objSchDate.StartTime1 = "";
                            }
                        }
                    }
                    else if (objSchDate.ScheduleDate == objLeave.LastOrDefault().Date)
                    {
                        DateTime endTime = DateTime.Parse(String.Format("{0} {1}:00", objSchDate.cfScheduleDateInString, objLeave.LastOrDefault().EndTime));
                        endTime = endTime.AddMinutes(15);

                        DateTime endTimeDefault = DateTime.Parse("2012-01-28" + ' ' + objLeave.LastOrDefault().EndTime);

                        if (objSchDate.StartTime5 != "")
                        {

                            if (endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay)
                            {
                                objSchDate.StartTime5 = endTime.ToString("HH:mm");
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay)
                            {
                                objSchDate.StartTime4 = endTime.ToString("HH:mm");
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay)
                            {
                                objSchDate.StartTime3 = endTime.ToString("HH:mm");
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay)
                            {
                                objSchDate.StartTime2 = endTime.ToString("HH:mm");
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay)
                            {
                                objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            }
                        }
                        else if (objSchDate.StartTime4 != "" && objSchDate.StartTime5 == "")
                        {
                            if (endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay)
                            {
                                objSchDate.StartTime4 = endTime.ToString("HH:mm");
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay)
                            {
                                objSchDate.StartTime3 = endTime.ToString("HH:mm");
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay)
                            {
                                objSchDate.StartTime2 = endTime.ToString("HH:mm");
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay)
                            {
                                objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            }
                        }
                        else if (objSchDate.StartTime3 != "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                        {
                            if (endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay)
                            {
                                objSchDate.StartTime3 = endTime.ToString("HH:mm");
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay)
                            {
                                objSchDate.StartTime2 = endTime.ToString("HH:mm");
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay)
                            {
                                objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            }
                        }
                        else if (objSchDate.StartTime2 != "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                        {
                            if (endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay)
                            {
                                objSchDate.StartTime2 = endTime.ToString("HH:mm");
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay)
                            {
                                objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            }
                        }
                        else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 == "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                        {
                            if (objSchStart1.TimeOfDay <= endTimeDefault.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay)
                            {
                                objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            }
                            else if (endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay)
                            {
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime1 = "";
                            }
                        }
                    }
                    else
                    {
                        List<GetParamedicLeaveScheduleCompare> c = objLeave.Where(t => t.Date == selectedDate).ToList();
                        if (c.Count > 0)
                        {
                            objSchDate.StartTime1 = "";
                            objSchDate.StartTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.StartTime5 = "";

                            objSchDate.EndTime1 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.EndTime5 = "";
                        }
                    }
                }
                #endregion
            }
            #endregion
            #region if leave only in one day
            else if (objLeave.FirstOrDefault().DayNumber != 0 && objLeave.Count() == 1)
            {
                #region set time slot Paramedic Schedule
                if (obj != null)
                {
                    DateTime startTimeDefault = DateTime.Parse("2012-01-28" + ' ' + objLeave.FirstOrDefault().StartTime);
                    DateTime endTimeDefault = DateTime.Parse("2012-01-28" + ' ' + objLeave.FirstOrDefault().EndTime);

                    DateTime startTime = startTimeDefault.AddMinutes(15);
                    DateTime endTime = endTimeDefault.AddMinutes(15);

                    DateTime objStart1 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime1);
                    DateTime objStart2 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime2);
                    DateTime objStart3 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime3);
                    DateTime objStart4 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime4);
                    DateTime objStart5 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime5);

                    DateTime objEnd1 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime1);
                    DateTime objEnd2 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime2);
                    DateTime objEnd3 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime3);
                    DateTime objEnd4 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime4);
                    DateTime objEnd5 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime5);

                    if (obj.StartTime1 != "" && obj.StartTime2 == "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                    {
                        if (startTimeDefault.TimeOfDay < objStart1.TimeOfDay && startTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1
                        {
                            obj.StartTime1 = startTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1/2
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay) //1/2
                        {
                            //obj = null;
                            obj.StartTime1 = "";
                            obj.EndTime1 = "";
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay) //1/2
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //3
                        {
                            obj.EndTime2 = obj.EndTime1;
                            obj.EndTime1 = objLeave.FirstOrDefault().StartTime;
                            obj.StartTime2 = endTime.ToString("HH:mm");

                            obj.IsAllowWaitingList2 = obj.IsAllowWaitingList1;
                            obj.MaximumWaitingList2 = obj.MaximumWaitingList1;

                            obj.IsAppointmentByTimeSlot2 = obj.IsAppointmentByTimeSlot1;
                            obj.MaximumAppointment2 = obj.MaximumAppointment1;
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay) //4
                        {
                            obj.EndTime1 = objLeave.FirstOrDefault().StartTime;
                        }
                    }
                    else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                    {
                        if (startTimeDefault.TimeOfDay < objStart1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1
                        {
                            obj.StartTime1 = startTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1/2
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay) //2 modif
                        {
                            //obj = null;
                            obj.StartTime1 = "";
                            obj.EndTime1 = "";
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay) //2 modif
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objStart2.TimeOfDay) //2 modif
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //3
                        {
                            obj.StartTime3 = obj.StartTime2;
                            obj.EndTime3 = obj.EndTime2;
                            obj.EndTime2 = obj.EndTime1;
                            obj.EndTime1 = objLeave.FirstOrDefault().StartTime;
                            obj.StartTime2 = endTime.ToString("HH:mm");

                            obj.IsAllowWaitingList3 = obj.IsAllowWaitingList2;
                            obj.MaximumWaitingList3 = obj.MaximumWaitingList2;

                            obj.IsAppointmentByTimeSlot3 = obj.IsAppointmentByTimeSlot2;
                            obj.MaximumAppointment3 = obj.MaximumAppointment2;
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay) //4
                        {
                            obj.EndTime1 = objLeave.FirstOrDefault().StartTime;
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //5
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay)  //6
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objStart2.TimeOfDay) //7
                        {
                            DateTime start2 = objStart2.AddMinutes(15);
                            obj.StartTime1 = start2.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //8
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay) //9
                        {
                            //obj = null;
                            obj.StartTime1 = "";
                            obj.EndTime1 = "";
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //10
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay) //11
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //12
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay) //13
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay == objStart2.TimeOfDay) //14
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //15
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime2;
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay) //16
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //17
                        {
                            obj.EndTime3 = obj.EndTime2;
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = endTime.ToString("HH:mm");

                            obj.IsAllowWaitingList3 = obj.IsAllowWaitingList2;
                            obj.MaximumWaitingList3 = obj.MaximumWaitingList2;

                            obj.IsAppointmentByTimeSlot3 = obj.IsAppointmentByTimeSlot2;
                            obj.MaximumAppointment3 = obj.MaximumAppointment2;
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //18
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay) //19
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay) //20
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                    }
                    else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 != "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                    {
                        if (startTimeDefault.TimeOfDay < objStart1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1
                        {
                            obj.StartTime1 = startTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1/2
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay < objStart1.TimeOfDay && endTimeDefault.TimeOfDay == objStart1.TimeOfDay)
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay == objEnd1.TimeOfDay) //2
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = obj.StartTime3;
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay)
                        {
                            //obj = null;
                            obj.StartTime1 = "";
                            obj.EndTime1 = "";
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //3
                        {
                            obj.StartTime4 = obj.StartTime3;
                            obj.EndTime4 = obj.EndTime3;
                            obj.StartTime3 = obj.StartTime2;
                            obj.EndTime3 = obj.EndTime2;
                            obj.EndTime2 = obj.EndTime1;
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");

                            obj.IsAllowWaitingList4 = obj.IsAllowWaitingList3;
                            obj.MaximumWaitingList4 = obj.MaximumWaitingList3;

                            obj.IsAppointmentByTimeSlot4 = obj.IsAppointmentByTimeSlot3;
                            obj.MaximumAppointment4 = obj.MaximumAppointment3;
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //4
                        {
                            obj.EndTime1 = objLeave.FirstOrDefault().StartTime;
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //5
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay)  //6
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objStart2.TimeOfDay) //7
                        {
                            DateTime start2 = objStart2.AddMinutes(15);
                            obj.StartTime1 = start2.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //8
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //9
                        {
                            obj.StartTime1 = obj.StartTime3;
                            obj.EndTime1 = obj.EndTime3;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //10
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay) //11
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //12
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //13
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay == objStart2.TimeOfDay) //14
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //15
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime2;
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //16
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //17
                        {
                            obj.StartTime4 = obj.StartTime3;
                            obj.EndTime4 = obj.EndTime3;
                            obj.EndTime3 = obj.EndTime2;
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = endTime.ToString("HH:mm");

                            obj.IsAllowWaitingList4 = obj.IsAllowWaitingList3;
                            obj.MaximumWaitingList4 = obj.MaximumWaitingList3;

                            obj.IsAppointmentByTimeSlot4 = obj.IsAppointmentByTimeSlot3;
                            obj.MaximumAppointment4 = obj.MaximumAppointment3;
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //18
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //19
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //20
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay) //21
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime3;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay) //22
                        {
                            //obj = null;
                            obj.StartTime1 = "";
                            obj.EndTime1 = "";
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd3.TimeOfDay) //23
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay) //24
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd3.TimeOfDay) //25
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay) //26
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay) //27
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay) //28
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay) //29
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay) //30
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay) //31
                        {
                            obj.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay >= objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay) //32
                        {
                            obj.EndTime4 = obj.EndTime3;
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime4 = endTime.ToString("HH:mm");

                            obj.IsAllowWaitingList4 = obj.IsAllowWaitingList3;
                            obj.MaximumWaitingList4 = obj.MaximumWaitingList3;

                            obj.IsAppointmentByTimeSlot4 = obj.IsAppointmentByTimeSlot3;
                            obj.MaximumAppointment4 = obj.MaximumAppointment3;
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay == objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay) //33
                        {
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay) //34
                        {
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay)
                        {
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                    }
                    else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 != "" && obj.StartTime4 != "" && obj.StartTime5 == "")
                    {
                        if (startTimeDefault.TimeOfDay < objStart1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1
                        {
                            obj.StartTime1 = startTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1/2
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay < objStart1.TimeOfDay && endTimeDefault.TimeOfDay == objStart1.TimeOfDay)
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay == objEnd1.TimeOfDay) //2
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = obj.StartTime3;
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
                        {
                            //obj = null;
                            obj.StartTime1 = "";
                            obj.EndTime1 = "";
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay == objEnd3.TimeOfDay)
                        {
                            obj.StartTime1 = obj.StartTime4;
                            obj.EndTime1 = obj.EndTime4;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime1 = obj.StartTime4;
                            obj.EndTime1 = obj.EndTime4;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd4.TimeOfDay)
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime4;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //3
                        {
                            obj.StartTime4 = obj.StartTime3;
                            obj.EndTime4 = obj.EndTime3;
                            obj.StartTime3 = obj.StartTime2;
                            obj.EndTime3 = obj.EndTime2;
                            obj.EndTime2 = obj.EndTime1;
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //4
                        {
                            obj.EndTime1 = objLeave.FirstOrDefault().StartTime;
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //5
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay)  //6
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objStart2.TimeOfDay) //7
                        {
                            DateTime start2 = objStart2.AddMinutes(15);
                            obj.StartTime1 = start2.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //8
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //9
                        {
                            obj.StartTime1 = obj.StartTime3;
                            obj.EndTime1 = obj.EndTime3;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //10
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay) //11
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //12
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //13
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay == objStart2.TimeOfDay) //14
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //15
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime2;
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //16
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //17
                        {
                            obj.StartTime4 = obj.StartTime3;
                            obj.EndTime4 = obj.EndTime3;
                            obj.EndTime3 = obj.EndTime2;
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //18
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //19
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //20
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay) //21
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime3;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay) //22
                        {
                            //obj = null;
                            obj.StartTime1 = "";
                            obj.EndTime1 = "";
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd3.TimeOfDay) //23
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay) //24
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = obj.StartTime4;
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd3.TimeOfDay) //25
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = obj.StartTime4;
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd4.TimeOfDay) //26
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd4.TimeOfDay)
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay) //27
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay) //28
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay) //29
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay) //30
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = obj.StartTime4;
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = endTime.ToString("HH:mm");
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay) //31
                        {
                            obj.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay >= objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay) //32
                        {
                            obj.StartTime5 = obj.StartTime4;
                            obj.EndTime5 = obj.EndTime4;
                            obj.EndTime4 = obj.EndTime3;
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime4 = endTime.ToString("HH:mm");

                            obj.IsAllowWaitingList5 = obj.IsAllowWaitingList4;
                            obj.MaximumWaitingList5 = obj.MaximumWaitingList4;

                            obj.IsAppointmentByTimeSlot5 = obj.IsAppointmentByTimeSlot4;
                            obj.MaximumAppointment5 = obj.MaximumAppointment4;
                        }
                        else if (startTimeDefault.TimeOfDay == objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay) //33
                        {
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay == objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime3 = endTime.ToString("HH:mm");
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay) //34
                        {
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
                        {
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
                        {
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime3 = endTime.ToString("HH:mm");
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime2 = obj.StartTime4;
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime3 = obj.StartTime4;
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objEnd3.TimeOfDay && endTimeDefault.TimeOfDay == objStart4.TimeOfDay)
                        {
                            obj.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
                        {
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
                        {
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart4.TimeOfDay && endTimeDefault.TimeOfDay > objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
                        {
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart4.TimeOfDay && startTimeDefault.TimeOfDay < objEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.EndTime5 = obj.EndTime4;
                            obj.EndTime4 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime5 = endTime.ToString("HH:mm");

                            obj.IsAllowWaitingList5 = obj.IsAllowWaitingList4;
                            obj.MaximumWaitingList5 = obj.MaximumWaitingList4;

                            obj.IsAppointmentByTimeSlot5 = obj.IsAppointmentByTimeSlot4;
                            obj.MaximumAppointment5 = obj.MaximumAppointment4;
                        }
                        else if (startTimeDefault.TimeOfDay > objStart4.TimeOfDay && startTimeDefault.TimeOfDay < objEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
                        {
                            obj.EndTime4 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime2 = obj.StartTime4;
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime3 = obj.StartTime4;
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime3 = obj.StartTime4;
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                    }
                    else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 != "" && obj.StartTime4 != "" && obj.StartTime5 != "")
                    {
                        if (startTimeDefault.TimeOfDay < objStart1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1
                        {
                            obj.StartTime1 = startTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1/2
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay < objStart1.TimeOfDay && endTimeDefault.TimeOfDay == objStart1.TimeOfDay)
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay == objEnd1.TimeOfDay) //2
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = obj.StartTime3;
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            //obj = null;
                            obj.StartTime1 = "";
                            obj.EndTime1 = "";
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objStart5.TimeOfDay)
                        {
                            obj.StartTime1 = obj.StartTime5;
                            obj.EndTime1 = obj.EndTime5;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay == objEnd3.TimeOfDay)
                        {
                            obj.StartTime1 = obj.StartTime4;
                            obj.EndTime1 = obj.EndTime4;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime1 = obj.StartTime4;
                            obj.EndTime1 = obj.EndTime4;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd4.TimeOfDay)
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime4;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //3
                        {
                            obj.StartTime4 = obj.StartTime3;
                            obj.EndTime4 = obj.EndTime3;
                            obj.StartTime3 = obj.StartTime2;
                            obj.EndTime3 = obj.EndTime2;
                            obj.EndTime2 = obj.EndTime1;
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //4
                        {
                            obj.EndTime1 = objLeave.FirstOrDefault().StartTime;
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //5
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay)  //6
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objStart2.TimeOfDay) //7
                        {
                            DateTime start2 = objStart2.AddMinutes(15);
                            obj.StartTime1 = start2.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //8
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //9
                        {
                            obj.StartTime1 = obj.StartTime3;
                            obj.EndTime1 = obj.EndTime3;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //10
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay) //11
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //12
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //13
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay == objStart2.TimeOfDay) //14
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //15
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime2;
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //16
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //17
                        {
                            obj.StartTime4 = obj.StartTime3;
                            obj.EndTime4 = obj.EndTime3;
                            obj.EndTime3 = obj.EndTime2;
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //18
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //19
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //20
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay) //21
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime3;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd3.TimeOfDay) //23
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay) //24
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = obj.StartTime4;
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd3.TimeOfDay) //25
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = obj.StartTime4;
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay) //26
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd4.TimeOfDay)
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay) //27
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay) //28
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay) //29
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay) //30
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = obj.StartTime4;
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = endTime.ToString("HH:mm");
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay) //31
                        {
                            obj.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay >= objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay) //32
                        {
                            obj.StartTime5 = obj.StartTime4;
                            obj.EndTime5 = obj.EndTime4;
                            obj.EndTime4 = obj.EndTime3;
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay) //33
                        {
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay == objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime3 = endTime.ToString("HH:mm");
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay) //34
                        {
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime3 = endTime.ToString("HH:mm");
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime2 = obj.StartTime4;
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime3 = obj.StartTime4;
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objEnd3.TimeOfDay && endTimeDefault.TimeOfDay == objStart4.TimeOfDay)
                        {
                            obj.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart4.TimeOfDay && endTimeDefault.TimeOfDay > objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart4.TimeOfDay && startTimeDefault.TimeOfDay < objEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.EndTime5 = obj.EndTime4;
                            obj.EndTime4 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime5 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart4.TimeOfDay && startTimeDefault.TimeOfDay < objEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.EndTime4 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime2 = obj.StartTime4;
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime3 = obj.StartTime4;
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime3 = obj.StartTime4;
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd4.TimeOfDay && endTimeDefault.TimeOfDay == objStart5.TimeOfDay)
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime5;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime5;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime5;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objStart5.TimeOfDay)
                        {
                            obj.StartTime2 = obj.StartTime5;
                            obj.EndTime2 = obj.EndTime5;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime5;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime5;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = endTime.ToString("HH:mm");
                            obj.EndTime3 = obj.EndTime5;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.StartTime3 = endTime.ToString("HH:mm");
                            obj.EndTime3 = obj.EndTime5;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objStart5.TimeOfDay)
                        {
                            obj.StartTime3 = obj.StartTime5;
                            obj.EndTime3 = obj.EndTime5;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objStart5.TimeOfDay)
                        {
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime4 = obj.StartTime5;
                            obj.EndTime4 = obj.EndTime5;
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime4 = endTime.ToString("HH:mm");
                            obj.EndTime4 = obj.EndTime5;
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objStart5.TimeOfDay)
                        {
                            obj.StartTime4 = obj.StartTime5;
                            obj.EndTime4 = obj.EndTime5;
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.StartTime4 = endTime.ToString("HH:mm");
                            obj.EndTime4 = obj.EndTime5;
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objStart5.TimeOfDay)
                        {
                            obj.StartTime4 = obj.StartTime5;
                            obj.EndTime4 = obj.EndTime5;
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.StartTime4 = endTime.ToString("HH:mm");
                            obj.EndTime4 = obj.EndTime5;
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objStart5.TimeOfDay)
                        {
                            obj.StartTime4 = obj.StartTime5;
                            obj.EndTime4 = obj.EndTime5;
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.StartTime4 = endTime.ToString("HH:mm");
                            obj.EndTime4 = obj.EndTime5;
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart4.TimeOfDay && startTimeDefault.TimeOfDay < objEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objStart5.TimeOfDay)
                        {
                            obj.EndTime4 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart4.TimeOfDay && startTimeDefault.TimeOfDay < objEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.EndTime4 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime5 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && startTimeDefault.TimeOfDay < objStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.StartTime5 = endTime.ToString("HH:mm");
                            obj.EndTime5 = obj.EndTime5;
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && startTimeDefault.TimeOfDay < objStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart5.TimeOfDay && endTimeDefault.TimeOfDay > objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.StartTime5 = startTime.ToString("HH:mm");
                            obj.EndTime5 = endTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                    }
                }
                #endregion

                #region set time slot Paramedic Schedule Date
                if (objSchDate != null)
                {
                    DateTime startTimeDefault = DateTime.Parse("2012-01-28" + ' ' + objLeave.FirstOrDefault().StartTime);
                    DateTime endTimeDefault = DateTime.Parse("2012-01-28" + ' ' + objLeave.FirstOrDefault().EndTime);

                    DateTime startTime = startTimeDefault.AddMinutes(15);
                    DateTime endTime = endTimeDefault.AddMinutes(15);

                    DateTime objSchStart1 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime1);
                    DateTime objSchStart2 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime2);
                    DateTime objSchStart3 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime3);
                    DateTime objSchStart4 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime4);
                    DateTime objSchStart5 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime5);

                    DateTime objSchEnd1 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime1);
                    DateTime objSchEnd2 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime2);
                    DateTime objSchEnd3 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime3);
                    DateTime objSchEnd4 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime4);
                    DateTime objSchEnd5 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime5);

                    if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 == "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                    {
                        if (startTimeDefault.TimeOfDay < objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1
                        {
                            objSchDate.StartTime1 = startTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1/2
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay) //1/2
                        {
                            //objSchDate = null;
                            objSchDate.StartTime1 = "";
                            objSchDate.EndTime1 = "";
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay) //1/2
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //3
                        {
                            objSchDate.EndTime2 = objSchDate.EndTime1;
                            objSchDate.EndTime1 = objLeave.FirstOrDefault().StartTime;
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");

                            objSchDate.IsAllowWaitingList2 = objSchDate.IsAllowWaitingList1;
                            objSchDate.MaximumWaitingList2 = objSchDate.MaximumWaitingList1;

                            objSchDate.IsAppointmentByTimeSlot2 = objSchDate.IsAppointmentByTimeSlot1;
                            objSchDate.MaximumAppointment2 = objSchDate.MaximumAppointment1;
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay) //4
                        {
                            objSchDate.EndTime1 = objLeave.FirstOrDefault().StartTime;
                        }
                    }
                    else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                    {
                        if (startTimeDefault.TimeOfDay < objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1
                        {
                            objSchDate.StartTime1 = startTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1/2
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay) //2 modif
                        {
                            //objSchDate = null;
                            objSchDate.StartTime1 = "";
                            objSchDate.EndTime1 = "";
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay) //2 modif
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart2.TimeOfDay) //2 modif
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //3
                        {
                            objSchDate.StartTime3 = objSchDate.StartTime2;
                            objSchDate.EndTime3 = objSchDate.EndTime2;
                            objSchDate.EndTime2 = objSchDate.EndTime1;
                            objSchDate.EndTime1 = objLeave.FirstOrDefault().StartTime;
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");

                            objSchDate.IsAllowWaitingList3 = objSchDate.IsAllowWaitingList2;
                            objSchDate.MaximumWaitingList3 = objSchDate.MaximumWaitingList2;

                            objSchDate.IsAppointmentByTimeSlot3 = objSchDate.IsAppointmentByTimeSlot2;
                            objSchDate.MaximumAppointment3 = objSchDate.MaximumAppointment2;
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay) //4
                        {
                            objSchDate.EndTime1 = objLeave.FirstOrDefault().StartTime;
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //5
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay)  //6
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart2.TimeOfDay) //7
                        {
                            DateTime start2 = objSchStart2.AddMinutes(15);
                            objSchDate.StartTime1 = start2.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //8
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay) //9
                        {
                            //objSchDate = null;
                            objSchDate.StartTime1 = "";
                            objSchDate.EndTime1 = "";
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //10
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay) //11
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //12
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay) //13
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart2.TimeOfDay) //14
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //15
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime2;
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay) //16
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //17
                        {
                            objSchDate.EndTime3 = objSchDate.EndTime2;
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");

                            objSchDate.IsAllowWaitingList3 = objSchDate.IsAllowWaitingList2;
                            objSchDate.MaximumWaitingList3 = objSchDate.MaximumWaitingList2;

                            objSchDate.IsAppointmentByTimeSlot3 = objSchDate.IsAppointmentByTimeSlot2;
                            objSchDate.MaximumAppointment3 = objSchDate.MaximumAppointment2;
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //18
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay) //19
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay) //20
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                    }
                    else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 != "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                    {
                        if (startTimeDefault.TimeOfDay < objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1
                        {
                            objSchDate.StartTime1 = startTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1/2
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay < objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart1.TimeOfDay)
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay == objSchEnd1.TimeOfDay) //2
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = objSchDate.StartTime3;
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay)
                        {
                            //objSchDate = null;
                            objSchDate.StartTime1 = "";
                            objSchDate.EndTime1 = "";
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //3
                        {
                            objSchDate.StartTime4 = objSchDate.StartTime3;
                            objSchDate.EndTime4 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = objSchDate.StartTime2;
                            objSchDate.EndTime3 = objSchDate.EndTime2;
                            objSchDate.EndTime2 = objSchDate.EndTime1;
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");

                            objSchDate.IsAllowWaitingList4 = objSchDate.IsAllowWaitingList3;
                            objSchDate.MaximumWaitingList4 = objSchDate.MaximumWaitingList3;

                            objSchDate.IsAppointmentByTimeSlot4 = objSchDate.IsAppointmentByTimeSlot3;
                            objSchDate.MaximumAppointment4 = objSchDate.MaximumAppointment3;
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //4
                        {
                            objSchDate.EndTime1 = objLeave.FirstOrDefault().StartTime;
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //5
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay)  //6
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart2.TimeOfDay) //7
                        {
                            DateTime start2 = objSchStart2.AddMinutes(15);
                            objSchDate.StartTime1 = start2.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //8
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //9
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime3;
                            objSchDate.EndTime1 = objSchDate.EndTime3;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //10
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay) //11
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //12
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //13
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart2.TimeOfDay) //14
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //15
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime2;
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //16
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //17
                        {
                            objSchDate.StartTime4 = objSchDate.StartTime3;
                            objSchDate.EndTime4 = objSchDate.EndTime3;
                            objSchDate.EndTime3 = objSchDate.EndTime2;
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");

                            objSchDate.IsAllowWaitingList4 = objSchDate.IsAllowWaitingList3;
                            objSchDate.MaximumWaitingList4 = objSchDate.MaximumWaitingList3;

                            objSchDate.IsAppointmentByTimeSlot4 = objSchDate.IsAppointmentByTimeSlot3;
                            objSchDate.MaximumAppointment4 = objSchDate.MaximumAppointment3;
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //18
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //19
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //20
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay) //21
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime3;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay) //22
                        {
                            //objSchDate = null;
                            objSchDate.StartTime1 = "";
                            objSchDate.EndTime1 = "";
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay)
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd3.TimeOfDay) //23
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay) //24
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd3.TimeOfDay) //25
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay) //26
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay) //27
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay) //28
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay) //29
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay) //30
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay) //31
                        {
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay) //32
                        {
                            objSchDate.EndTime4 = objSchDate.EndTime3;
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");

                            objSchDate.IsAllowWaitingList4 = objSchDate.IsAllowWaitingList3;
                            objSchDate.MaximumWaitingList4 = objSchDate.MaximumWaitingList3;

                            objSchDate.IsAppointmentByTimeSlot4 = objSchDate.IsAppointmentByTimeSlot3;
                            objSchDate.MaximumAppointment4 = objSchDate.MaximumAppointment3;
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay) //33
                        {
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay) //34
                        {
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay)
                        {
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                    }
                    else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 != "" && objSchDate.StartTime4 != "" && objSchDate.StartTime5 == "")
                    {
                        if (startTimeDefault.TimeOfDay < objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1
                        {
                            objSchDate.StartTime1 = startTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1/2
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay < objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart1.TimeOfDay)
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay == objSchEnd1.TimeOfDay) //2
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = objSchDate.StartTime3;
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
                        {
                            //objSchDate = null;
                            objSchDate.StartTime1 = "";
                            objSchDate.EndTime1 = "";
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay == objSchEnd3.TimeOfDay)
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime4;
                            objSchDate.EndTime1 = objSchDate.EndTime4;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime4;
                            objSchDate.EndTime1 = objSchDate.EndTime4;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime4;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //3
                        {
                            objSchDate.StartTime4 = objSchDate.StartTime3;
                            objSchDate.EndTime4 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = objSchDate.StartTime2;
                            objSchDate.EndTime3 = objSchDate.EndTime2;
                            objSchDate.EndTime2 = objSchDate.EndTime1;
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //4
                        {
                            objSchDate.EndTime1 = objLeave.FirstOrDefault().StartTime;
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //5
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay)  //6
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart2.TimeOfDay) //7
                        {
                            DateTime start2 = objSchStart2.AddMinutes(15);
                            objSchDate.StartTime1 = start2.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //8
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //9
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime3;
                            objSchDate.EndTime1 = objSchDate.EndTime3;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //10
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay) //11
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //12
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //13
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart2.TimeOfDay) //14
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //15
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime2;
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //16
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //17
                        {
                            objSchDate.StartTime4 = objSchDate.StartTime3;
                            objSchDate.EndTime4 = objSchDate.EndTime3;
                            objSchDate.EndTime3 = objSchDate.EndTime2;
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //18
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //19
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //20
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay) //21
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime3;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay) //22
                        {
                            //objSchDate = null;
                            objSchDate.StartTime1 = "";
                            objSchDate.EndTime1 = "";
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay)
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd3.TimeOfDay) //23
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay) //24
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = objSchDate.StartTime4;
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd3.TimeOfDay) //25
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = objSchDate.StartTime4;
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd4.TimeOfDay) //26
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay) //26
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay) //27
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay) //28
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay) //29
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay) //30
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = objSchDate.StartTime4;
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay) //31
                        {
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay) //32
                        {
                            objSchDate.StartTime5 = objSchDate.StartTime4;
                            objSchDate.EndTime5 = objSchDate.EndTime4;
                            objSchDate.EndTime4 = objSchDate.EndTime3;
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");

                            objSchDate.IsAllowWaitingList5 = objSchDate.IsAllowWaitingList4;
                            objSchDate.MaximumWaitingList5 = objSchDate.MaximumWaitingList4;

                            objSchDate.IsAppointmentByTimeSlot5 = objSchDate.IsAppointmentByTimeSlot4;
                            objSchDate.MaximumAppointment5 = objSchDate.MaximumAppointment4;
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay) //33
                        {
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay) //34
                        {
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime2 = objSchDate.StartTime4;
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime3 = objSchDate.StartTime4;
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime5 = objSchDate.EndTime4;
                            objSchDate.EndTime4 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime5 = endTime.ToString("HH:mm");

                            objSchDate.IsAllowWaitingList5 = objSchDate.IsAllowWaitingList4;
                            objSchDate.MaximumWaitingList5 = objSchDate.MaximumWaitingList4;

                            objSchDate.IsAppointmentByTimeSlot5 = objSchDate.IsAppointmentByTimeSlot4;
                            objSchDate.MaximumAppointment5 = objSchDate.MaximumAppointment4;
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime4 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime2 = objSchDate.StartTime4;
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime3 = objSchDate.StartTime4;
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime3 = objSchDate.StartTime4;
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                    }
                    else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 != "" && objSchDate.StartTime4 != "" && objSchDate.StartTime5 != "")
                    {
                        if (startTimeDefault.TimeOfDay < objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1
                        {
                            objSchDate.StartTime1 = startTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1/2
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay < objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart1.TimeOfDay)
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay == objSchEnd1.TimeOfDay) //2
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = objSchDate.StartTime3;
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            //objSchDate = null;
                            objSchDate.StartTime1 = "";
                            objSchDate.EndTime1 = "";
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart5.TimeOfDay)
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime5;
                            objSchDate.EndTime1 = objSchDate.EndTime5;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime4;
                            objSchDate.EndTime1 = objSchDate.EndTime4;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime4;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //3
                        {
                            objSchDate.StartTime4 = objSchDate.StartTime3;
                            objSchDate.EndTime4 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = objSchDate.StartTime2;
                            objSchDate.EndTime3 = objSchDate.EndTime2;
                            objSchDate.EndTime2 = objSchDate.EndTime1;
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //4
                        {
                            objSchDate.EndTime1 = objLeave.FirstOrDefault().StartTime;
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //5
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay)  //6
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart2.TimeOfDay) //7
                        {
                            DateTime start2 = objSchStart2.AddMinutes(15);
                            objSchDate.StartTime1 = start2.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //8
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //9
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime3;
                            objSchDate.EndTime1 = objSchDate.EndTime3;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //10
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay) //11
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //12
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //13
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart2.TimeOfDay) //14
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //15
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime2;
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //16
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //17
                        {
                            objSchDate.StartTime4 = objSchDate.StartTime3;
                            objSchDate.EndTime4 = objSchDate.EndTime3;
                            objSchDate.EndTime3 = objSchDate.EndTime2;
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //18
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //19
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //20
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay) //21
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime3;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay)
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd3.TimeOfDay) //23
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay) //24
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = objSchDate.StartTime4;
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd3.TimeOfDay) //25
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = objSchDate.StartTime4;
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay) //26
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay) //26
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay) //27
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay) //28
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay) //29
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay) //30
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = objSchDate.StartTime4;
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay) //31
                        {
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay) //32
                        {
                            objSchDate.StartTime5 = objSchDate.StartTime4;
                            objSchDate.EndTime5 = objSchDate.EndTime4;
                            objSchDate.EndTime4 = objSchDate.EndTime3;
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay) //33
                        {
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay) //34
                        {
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime2 = objSchDate.StartTime4;
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime3 = objSchDate.StartTime4;
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime5 = objSchDate.EndTime4;
                            objSchDate.EndTime4 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime5 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.EndTime4 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime2 = objSchDate.StartTime4;
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime3 = objSchDate.StartTime4;
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime3 = objSchDate.StartTime4;
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart5.TimeOfDay)
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime5;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime5;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime5;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart5.TimeOfDay)
                        {
                            objSchDate.StartTime2 = objSchDate.StartTime5;
                            objSchDate.EndTime2 = objSchDate.EndTime5;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime5;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime5;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                            objSchDate.EndTime3 = objSchDate.EndTime5;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                            objSchDate.EndTime3 = objSchDate.EndTime5;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart5.TimeOfDay)
                        {
                            objSchDate.StartTime3 = objSchDate.StartTime5;
                            objSchDate.EndTime3 = objSchDate.EndTime5;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart5.TimeOfDay)
                        {
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime4 = objSchDate.StartTime5;
                            objSchDate.EndTime4 = objSchDate.EndTime5;
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                            objSchDate.EndTime4 = objSchDate.EndTime5;
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart5.TimeOfDay)
                        {
                            objSchDate.StartTime4 = objSchDate.StartTime5;
                            objSchDate.EndTime4 = objSchDate.EndTime5;
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                            objSchDate.EndTime4 = objSchDate.EndTime5;
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart5.TimeOfDay)
                        {
                            objSchDate.StartTime4 = objSchDate.StartTime5;
                            objSchDate.EndTime4 = objSchDate.EndTime5;
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                            objSchDate.EndTime4 = objSchDate.EndTime5;
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart5.TimeOfDay)
                        {
                            objSchDate.StartTime4 = objSchDate.StartTime5;
                            objSchDate.EndTime4 = objSchDate.EndTime5;
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                            objSchDate.EndTime4 = objSchDate.EndTime5;
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart5.TimeOfDay)
                        {
                            objSchDate.EndTime4 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.EndTime4 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime5 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime5 = endTime.ToString("HH:mm");
                            objSchDate.EndTime5 = objSchDate.EndTime5;
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime5 = startTime.ToString("HH:mm");
                            objSchDate.EndTime5 = endTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                    }
                }
                #endregion
            }
            #endregion
            #endregion
        }

        private void BridgingToMedinfrasV1(int ProcessType, TestOrderHd entity)
        {
            APIMessageLog apiLog = new APIMessageLog();
            apiLog.MessageDateTime = DateTime.Now;
            apiLog.Sender = Constant.BridgingVendor.HIS;
            apiLog.Recipient = Constant.BridgingVendor.MEDINFRAS_API;

            MedinfrasV1Service oService = new MedinfrasV1Service();
            string serviceResult = oService.OnSendOrderMedicalDiagnosticServices(ProcessType, entity, null, null);
            string[] serviceResultInfo = serviceResult.Split('|');
            if (serviceResultInfo[0] == "1")
            {
                apiLog.IsSuccess = true;
                apiLog.MessageText = serviceResultInfo[1];
                apiLog.Response = serviceResultInfo[2];
            }
            else
            {
                apiLog.IsSuccess = false;
                apiLog.MessageText = serviceResultInfo[1];
                apiLog.Response = serviceResultInfo[2];
                apiLog.ErrorMessage = serviceResultInfo[2];
            }
            BusinessLayer.InsertAPIMessageLog(apiLog);
        }
    }
}
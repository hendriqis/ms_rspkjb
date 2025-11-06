using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class UDDMedicationOrderEntry : BasePageTrx
    {
        vConsultVisit4 entityCV = null;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Pharmacy.UDD_MEDICATION_ORDER_ENTRY;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = (hdnGCRegistrationStatus.Value != Constant.VisitStatus.CLOSED && !entityCV.IsLockDown);
            IsAllowSave = !entityCV.IsLockDown;
            IsAllowVoid = !entityCV.IsLockDown;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            string moduleName = Helper.GetModuleName();
            string ModuleID = Helper.GetModuleID(moduleName);
            GetUserMenuAccess menu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault();
            string CRUDMode = menu.CRUDMode;
            hdnIsAllowVoid.Value = CRUDMode.Contains("X") ? "1" : "0";

            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            entityCV = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", hdnVisitID.Value))[0];

            hdnDepartmentID.Value = entityCV.DepartmentID;
            hdnGCRegistrationStatus.Value = entityCV.GCVisitStatus;
            hdnRegistrationID.Value = entityCV.RegistrationID.ToString();
            hdnClassID.Value = entityCV.ClassID.ToString();

            hdnDefaultVisitParamedicID.Value = entityCV.ParamedicID.ToString();
            hdnDefaultVisitParamedicCode.Value = entityCV.ParamedicCode;
            hdnDefaultVisitParamedicName.Value = entityCV.ParamedicName;

            _isEditable = entityCV.IsLockDown ? false : true;
            BindGridView();
            List<vHealthcareServiceUnit> lstHealthcareServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.HealthcareServiceUnitID));
            hdnDefaultDispensaryServiceUnitID.Value = lstHealthcareServiceUnit.FirstOrDefault().DispensaryServiceUnitID.ToString();

            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                                "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}')",
                                                                AppSession.UserLogin.HealthcareID, //0
                                                                Constant.SettingParameter.PRESCRIPTION_FEE_AMOUNT, //1
                                                                Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_100, //2
                                                                Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1, //3
                                                                Constant.SettingParameter.PH_IS_REVIEW_PRESCRIPTION_MANDATORY_FOR_PROPOSED_TRANSACTION, //4
                                                                Constant.SettingParameter.IS_USING_DRUG_ALERT //5
                                                            ));

            hdnPrescriptionFeeAmount.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PRESCRIPTION_FEE_AMOUNT).ParameterValue;
            hdnIsEndingAmountRoundingTo100.Value = lstSettingParameterDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_100).FirstOrDefault().ParameterValue;
            hdnIsEndingAmountRoundingTo1.Value = lstSettingParameterDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1).FirstOrDefault().ParameterValue;
            hdnIsReviewPrescriptionMandatoryForProposedTransaction.Value = lstSettingParameterDt.Where(a => a.ParameterCode == Constant.SettingParameter.PH_IS_REVIEW_PRESCRIPTION_MANDATORY_FOR_PROPOSED_TRANSACTION).FirstOrDefault().ParameterValue;
            hdnIsUsingDrugAlertMain.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_USING_DRUG_ALERT).ParameterValue;

            if (Page.Request.QueryString.Count > 0)
            {
                string[] param = Page.Request.QueryString["id"].Split('|');
                string prescriptionOrderID = param[2];

                if (!string.IsNullOrEmpty(prescriptionOrderID))
                {
                    IsLoadFirstRecord = true;
                    string filterExpression = string.Format("VisitID = {0}", hdnVisitID.Value);
                    PrescriptionOrderHd orderHd = BusinessLayer.GetPrescriptionOrderHd(Convert.ToInt32(prescriptionOrderID));
                    pageIndexFirstLoad = BusinessLayer.GetvPrescriptionOrderHd3RowIndex(filterExpression, orderHd.PrescriptionOrderNo, "PrescriptionOrderID DESC");
                    hdnPrescriptionOrderID.Value = prescriptionOrderID;
                    hdnSelectedOrderID.Value = prescriptionOrderID;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(hdnPrescriptionOrderID.Value))
                {
                    IsLoadFirstRecord = (OnGetRowCount() > 0);
                    hdnSelectedOrderID.Value = string.Empty;
                }
                else
                {
                    IsLoadFirstRecord = true;
                    hdnSelectedOrderID.Value = hdnPrescriptionOrderID.Value;
                }
            }

            //LoadShortcutInformation();
        }

        private void LoadShortcutInformation()
        {
            string filterExpression = string.Format("VisitID IN ({0}) AND IsCompleted = 0 AND IsDeleted = 0 ORDER BY InstructionDate, InstructionTime, PatientInstructionID DESC", AppSession.RegisteredPatient.VisitID);

            List<vPatientInstruction> lstEntity = BusinessLayer.GetvPatientInstructionList(filterExpression);
            rptPhysicianInstruction.DataSource = lstEntity;
            rptPhysicianInstruction.DataBind();
        }

        protected override void SetControlProperties()
        {
            List<vHealthcareServiceUnitCustom> lstHealthcareServiceUnit = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("IsDeleted = 0 AND IsInpatientDispensary=1"));
            Methods.SetComboBoxField<vHealthcareServiceUnitCustom>(cboDispensaryUnit, lstHealthcareServiceUnit.Where(x => x.DepartmentID == "PHARMACY").ToList(), "ServiceUnitName", "HealthcareServiceUnitID");
            //txtStartTime1.Attributes.Add("validationgroup", "mpTrx");
            //txtStartTime2.Attributes.Add("validationgroup", "mpTrx");
            //txtStartTime3.Attributes.Add("validationgroup", "mpTrx");
            //txtStartTime4.Attributes.Add("validationgroup", "mpTrx");
            //txtStartTime5.Attributes.Add("validationgroup", "mpTrx");
            //txtStartTime6.Attributes.Add("validationgroup", "mpTrx");
            if (hdnDefaultDispensaryServiceUnitID.Value == "0") cboDispensaryUnit.SelectedIndex = 0;
            else
                cboDispensaryUnit.Value = hdnDefaultDispensaryServiceUnitID.Value;

            String filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}') AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.DRUG_FORM, Constant.StandardCode.DOSING_FREQUENCY, Constant.StandardCode.MEDICATION_ROUTE, Constant.StandardCode.ITEM_UNIT, Constant.StandardCode.REFILL_INSTRUCTION, Constant.StandardCode.COENAM_RULE, Constant.StandardCode.PRESCRIPTION_TYPE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboForm, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DRUG_FORM).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboFrequencyTimeline, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DOSING_FREQUENCY).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDosingUnit, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ITEM_UNIT && (sc.TagProperty.Contains("PRE") || sc.TagProperty.Contains("1"))).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboMedicationRoute, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.MEDICATION_ROUTE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboCoenamRule, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.COENAM_RULE || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

            if (!AppSession.IsUsedInpatientPrescriptionTypeFilter)
            {
                Methods.SetComboBoxField<StandardCode>(cboPrescriptionTypeMainPage, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.PRESCRIPTION_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            }
            else
            {
                if (!string.IsNullOrEmpty(AppSession.InpatientPrescriptionTypeFilter))
                {
                    string[] prescriptionType = AppSession.InpatientPrescriptionTypeFilter.Split(',');
                    Methods.SetComboBoxField<StandardCode>(cboPrescriptionTypeMainPage, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.PRESCRIPTION_TYPE).Where(x => !prescriptionType.Contains(x.StandardCodeID)).ToList(), "StandardCodeName", "StandardCodeID");
                }
                else
                {
                    Methods.SetComboBoxField<StandardCode>(cboPrescriptionTypeMainPage, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.PRESCRIPTION_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
                }
            }

            if (txtPrescriptionOrderNo.Text == string.Empty)
                cboPrescriptionTypeMainPage.SelectedIndex = 0;

            cboForm.SelectedIndex = 0;
            cboFrequencyTimeline.SelectedIndex = 0;
            cboDosingUnit.SelectedIndex = 0;
            cboMedicationRoute.SelectedIndex = 0;
            cboCoenamRule.SelectedIndex = 0;

            txtStartDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtStartTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            BindCboLocation();
        }

        protected void cboLocation_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindCboLocation();
        }

        private void BindCboLocation()
        {
            Location location = BusinessLayer.GetLocationList(string.Format("LocationID IN (SELECT LocationID FROM HealthcareServiceUnit WHERE HealthcareServiceUnitID = {0})", cboDispensaryUnit.Value)).FirstOrDefault();

            if (location != null)
            {
                int locationID = location.LocationID;
                Location loc = BusinessLayer.GetLocation(locationID);
                List<Location> lstLocation = null;
                if (loc.IsHeader)
                    lstLocation = BusinessLayer.GetLocationList(string.Format("ParentID = {0}", loc.LocationID));
                else
                {
                    lstLocation = new List<Location>();
                    lstLocation.Add(loc);
                }
                hdnIsAllowOverIssued.Value = loc.IsAllowOverIssued ? "1" : "0";
                Methods.SetComboBoxField<Location>(cboLocation, lstLocation, "LocationName", "LocationID");
                cboLocation.SelectedIndex = 0;
            }
        }

        protected void cboDosingUnit_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            if (hdnDrugID.Value != null && hdnDrugID.Value.ToString() != "")
            {
                List<StandardCode> lst = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND (TagProperty LIKE '%PRE%' OR TagProperty LIKE '%1%') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.ITEM_UNIT, hdnDrugID.Value));
                Methods.SetComboBoxField<StandardCode>(cboDosingUnit, lst, "StandardCodeName", "StandardCodeID");
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnPrescriptionOrderID, new ControlEntrySetting(false, false, false, "0"));

            SetControlEntrySetting(txtPrescriptionOrderNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtPrescriptionDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtPrescriptionTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));

            SetControlEntrySetting(hdnPhysicianID, new ControlEntrySetting(true, false, false, hdnDefaultVisitParamedicID.Value));
            SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, false, true, hdnDefaultVisitParamedicCode.Value));
            SetControlEntrySetting(txtPhysicianName, new ControlEntrySetting(false, false, false, hdnDefaultVisitParamedicName.Value));
            SetControlEntrySetting(cboDispensaryUnit, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(cboLocation, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(cboPrescriptionTypeMainPage, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(false, false, false, string.Empty));

            SetControlEntrySetting(lblPhysician, new ControlEntrySetting(true, false));

            SetControlEntrySetting(chkIsCorrectPatient, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsCorrectMedication, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsCorrectStrength, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsCorrectFrequency, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsCorrectDosage, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsCorrectRoute, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsHasDrugInteraction, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsHasDuplication, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsADChecked, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsFARChecked, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsKLNChecked, new ControlEntrySetting(true, true, false));

            Helper.SetControlEntrySetting(txtDrugCode, new ControlEntrySetting(true, true, true), "mpTrx");
            Helper.SetControlEntrySetting(txtDrugName, new ControlEntrySetting(false, false, true), "mpTrx");
            Helper.SetControlEntrySetting(txtGenericName, new ControlEntrySetting(false, false, false), "mpTrx");
            Helper.SetControlEntrySetting(txtSignaLabel, new ControlEntrySetting(true, true, false), "mpTrx");
            Helper.SetControlEntrySetting(txtSignaName1, new ControlEntrySetting(false, false, false), "mpTrx");
            Helper.SetControlEntrySetting(txtStrengthAmount, new ControlEntrySetting(false, false, false), "mpTrx");
            Helper.SetControlEntrySetting(txtStrengthUnit, new ControlEntrySetting(false, false, false), "mpTrx");
            Helper.SetControlEntrySetting(cboForm, new ControlEntrySetting(false, false, false), "mpTrx");
            Helper.SetControlEntrySetting(cboCoenamRule, new ControlEntrySetting(true, true, false), "mpTrx");
            Helper.SetControlEntrySetting(txtPurposeOfMedication, new ControlEntrySetting(true, true, false), "mpTrx");

            Helper.SetControlEntrySetting(txtFrequencyNumber, new ControlEntrySetting(true, true, true), "mpTrx");
            Helper.SetControlEntrySetting(cboFrequencyTimeline, new ControlEntrySetting(true, true, true), "mpTrx");
            Helper.SetControlEntrySetting(txtDosingDose, new ControlEntrySetting(true, true, true), "mpTrx");
            Helper.SetControlEntrySetting(cboDosingUnit, new ControlEntrySetting(true, true, true), "mpTrx");
            Helper.SetControlEntrySetting(cboMedicationRoute, new ControlEntrySetting(true, true, true), "mpTrx");
            Helper.SetControlEntrySetting(txtStartDate, new ControlEntrySetting(true, true, true), "mpTrx");
            Helper.SetControlEntrySetting(txtStartTime, new ControlEntrySetting(true, true, true), "mpTrx");
            Helper.SetControlEntrySetting(txtDosingDuration, new ControlEntrySetting(true, true, true), "mpTrx");
            Helper.SetControlEntrySetting(txtDosingDurationTimeline, new ControlEntrySetting(false, false, false), "mpTrx");
            Helper.SetControlEntrySetting(txtDispenseQty, new ControlEntrySetting(true, true, true), "mpTrx");
            Helper.SetControlEntrySetting(txtMedicationAdministration, new ControlEntrySetting(true, true, false), "mpTrx");
        }

        protected bool _isEditable = true;

        public override void OnAddRecord()
        {
            tdImageDrugAlertInfo.Style.Add("display", "none");
            hdnGCOrderStatus.Value = "";
            hdnGCTransactionStatus.Value = "";

            if (hdnDefaultDispensaryServiceUnitID.Value == "0")
            {
                cboDispensaryUnit.SelectedIndex = 0;
            }
            else
            {
                cboDispensaryUnit.Value = hdnDefaultDispensaryServiceUnitID.Value;
            }

            _isEditable = true;
            hdnIsEditable.Value = "1";

            divCreatedBy.InnerHtml = string.Empty;
            divCreatedDate.InnerHtml = string.Empty;
            divLastUpdatedBy.InnerHtml = string.Empty;
            divLastUpdatedDate.InnerHtml = string.Empty;

            cboPrescriptionTypeMainPage.Enabled = true;
            BindGridView();
        }

        #region Load Entity
        public override int OnGetRowCount()
        {
            string filterExpression = string.Format("VisitID = {0}", hdnVisitID.Value);
            return BusinessLayer.GetvPrescriptionOrderHd3RowCount(filterExpression);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = string.Format("VisitID = {0}", hdnVisitID.Value);
            //if (!string.IsNullOrEmpty(hdnSelectedOrderID.Value) && hdnSelectedOrderID.Value != "0")
            //{
            //    filterExpression += string.Format(" AND PrescriptionOrderID = {0}", hdnSelectedOrderID.Value);
            //    PageIndex = 0;
            //}
            vPrescriptionOrderHd3 entity = BusinessLayer.GetvPrescriptionOrderHd3(filterExpression, PageIndex, "PrescriptionOrderID DESC");
            if (entity != null)
            {
                EntityToControl(entity, ref isShowWatermark, ref watermarkText);
            }
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = string.Format("VisitID = {0}", hdnVisitID.Value);
            //hdnSelectedOrderID.Value = string.Empty;
            PageIndex = BusinessLayer.GetvPrescriptionOrderHd3RowIndex(filterExpression, keyValue, "PrescriptionOrderID DESC");
            vPrescriptionOrderHd3 entity = BusinessLayer.GetvPrescriptionOrderHd3(filterExpression, PageIndex, "PrescriptionOrderID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vPrescriptionOrderHd3 entity, ref bool isShowWatermark, ref string watermarkText)
        {
            _isEditable = (entity.GCTransactionStatus != Constant.TransactionStatus.VOID && entity.GCTransactionStatus != Constant.TransactionStatus.PROCESSED && entity.GCTransactionStatus != Constant.TransactionStatus.CLOSED);
            Registration entityRegistration = BusinessLayer.GetRegistration(Convert.ToInt32(hdnRegistrationID.Value));
            _isEditable = entityRegistration.IsLockDown ? false : _isEditable;

            if (!_isEditable)
            {
                isShowWatermark = true;
                watermarkText = entity.TransactionStatusWatermark;
            }

            hdnPrescriptionOrderID.Value = entity.PrescriptionOrderID.ToString();
            txtPrescriptionOrderNo.Text = entity.PrescriptionOrderNo;
            txtPrescriptionDate.Text = entity.PrescriptionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPrescriptionTime.Text = entity.PrescriptionTime;
            hdnPhysicianID.Value = entity.ParamedicID.ToString();
            txtPhysicianCode.Text = entity.ParamedicCode;
            txtPhysicianName.Text = entity.ParamedicName;
            chkIsCorrectPatient.Checked = entity.IsCorrectPatient;
            chkIsCorrectMedication.Checked = entity.IsCorrectMedication;
            chkIsCorrectStrength.Checked = entity.IsCorrectStrength;
            chkIsCorrectFrequency.Checked = entity.IsCorrectFrequency;
            chkIsCorrectDosage.Checked = entity.IsCorrectDosage;
            chkIsCorrectRoute.Checked = entity.IsCorrectRoute;
            chkIsHasDrugInteraction.Checked = entity.IsHasDrugInteraction;
            chkIsHasDuplication.Checked = entity.IsHasDuplication;
            chkIsADChecked.Checked = entity.IsADChecked;
            chkIsFARChecked.Checked = entity.IsFARChecked;
            chkIsKLNChecked.Checked = entity.IsKLNChecked;
            txtRemarks.Text = entity.Remarks;
            cboDispensaryUnit.Value = entity.DispensaryServiceUnitID.ToString();
            BindCboLocation();
            cboLocation.Value = entity.LocationID.ToString();

            cboPrescriptionTypeMainPage.Value = entity.GCPrescriptionType.ToString();
            hdnGCOrderStatus.Value = entity.GCOrderStatus;
            hdnGCTransactionStatus.Value = entity.GCTransactionStatus;
            hdnGCPrescriptionType.Value = entity.GCPrescriptionType;

            divCreatedBy.InnerHtml = entity.CreatedBy;
            divCreatedDate.InnerHtml = entity.CreatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            divLastUpdatedBy.InnerHtml = entity.LastUpdateBy;
            if (entity.LastUpdatedDate != null && entity.LastUpdatedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
            {
                divLastUpdatedDate.InnerHtml = entity.LastUpdatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }

            if (hdnIsUsingDrugAlertMain.Value == "1")
            {
                PrescriptionOrderHdInfo info = BusinessLayer.GetPrescriptionOrderHdInfo(entity.PrescriptionOrderID);
                if (info != null)
                {
                    if (!String.IsNullOrEmpty(info.DrugAlertResultInfo) || !String.IsNullOrEmpty(info.DrugAlertResultInfo1))
                    {
                        string toFound = "No results found. Absence of interaction result should in no way be interpreted as safe. Clinical judgment should be exercised";
                        if (info.DrugAlertResultInfo.Contains(toFound) && info.DrugAlertResultInfo1.Contains(toFound))
                        {
                            tdImageDrugAlertInfo.Style.Add("display", "none");
                        }
                        else
                        {
                            tdImageDrugAlertInfo.Style.Remove("display");
                        }
                    }
                    else
                    {
                        tdImageDrugAlertInfo.Style.Add("display", "none");
                    }
                }
                else
                {
                    tdImageDrugAlertInfo.Style.Add("display", "none");
                }
            }
            else
            {
                tdImageDrugAlertInfo.Style.Add("display", "none");
            }

            hdnIsHasPPRAItem.Value = entity.IsHasPPRAItem ? "1" : "0";

            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = "1 = 0";
            if (hdnPrescriptionOrderID.Value != "" && hdnPrescriptionOrderID.Value != "0")
                filterExpression = string.Format("PrescriptionOrderID = {0} AND ParentID IS NULL AND OrderIsDeleted = 0 ORDER BY PrescriptionOrderDetailID", hdnPrescriptionOrderID.Value);
            List<vPrescriptionOrderDt1> lstEntity = BusinessLayer.GetvPrescriptionOrderDt1List(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                if (hdnIsRefreshPageWithID.Value == "1")
                {
                    LoadHeaderInformation();
                    BindGridView();
                }
                else
                {
                    BindGridView();
                }
                result = "refresh";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void LoadHeaderInformation()
        {
            if (!string.IsNullOrEmpty(hdnPrescriptionOrderID.Value))
            {
                string filterExpression = string.Format("VisitID = {0}", hdnVisitID.Value);
                vPrescriptionOrderHd3 entity = BusinessLayer.GetvPrescriptionOrderHd3List(filterExpression).FirstOrDefault();
                if (entity != null)
                {
                    cboPrescriptionTypeMainPage.Value = entity.GCPrescriptionType;
                    chkIsCorrectPatient.Checked = entity.IsCorrectPatient;
                    chkIsCorrectMedication.Checked = entity.IsCorrectMedication;
                    chkIsCorrectStrength.Checked = entity.IsCorrectStrength;
                    chkIsCorrectFrequency.Checked = entity.IsCorrectFrequency;
                    chkIsCorrectDosage.Checked = entity.IsCorrectDosage;
                    chkIsCorrectRoute.Checked = entity.IsCorrectRoute;
                    chkIsHasDrugInteraction.Checked = entity.IsHasDrugInteraction;
                    chkIsHasDuplication.Checked = entity.IsHasDuplication;
                    chkIsADChecked.Checked = entity.IsADChecked;
                    chkIsFARChecked.Checked = entity.IsFARChecked;
                    chkIsKLNChecked.Checked = entity.IsKLNChecked;
                    hdnIsHasPPRAItem.Value = entity.IsHasPPRAItem ? "1" : "0";
                }
            }
        }
        #endregion

        #region Save Entity
        public void SavePrescriptionOrderHd(IDbContext ctx, ref int prescriptionOrderID, string prescType = "")
        {
            PrescriptionOrderHdDao entityHdDao = new PrescriptionOrderHdDao(ctx);
            PrescriptionOrderDtDao entityDtDao = new PrescriptionOrderDtDao(ctx);
            PrescriptionOrderHd entityHd = null;
            if (hdnPrescriptionOrderID.Value == "0")
            {
                DateTime sendOrderDateTime = DateTime.ParseExact(string.Format("{0} {1}", Helper.GetDatePickerValue(Request.Form[txtPrescriptionDate.UniqueID]).ToString(Constant.FormatString.DATE_FORMAT_112), Request.Form[txtPrescriptionTime.UniqueID]), "yyyyMMdd HH:mm", CultureInfo.InvariantCulture);

                entityHd = new PrescriptionOrderHd();
                entityHd.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
                entityHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                entityHd.VisitHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                entityHd.PrescriptionDate = Helper.GetDatePickerValue(Request.Form[txtPrescriptionDate.UniqueID]);
                entityHd.PrescriptionTime = Request.Form[txtPrescriptionTime.UniqueID];
                entityHd.ClassID = Convert.ToInt32(hdnClassID.Value);
                entityHd.DispensaryServiceUnitID = Convert.ToInt32(cboDispensaryUnit.Value);
                entityHd.LocationID = Convert.ToInt32(cboLocation.Value);
                entityHd.GCPrescriptionType = !string.IsNullOrEmpty(prescType) ? prescType : cboPrescriptionTypeMainPage.Value.ToString();
                entityHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                entityHd.GCOrderStatus = Constant.OrderStatus.RECEIVED;
                entityHd.TransactionCode = Constant.TransactionCode.IP_MEDICATION_ORDER;
                entityHd.IsCorrectPatient = chkIsCorrectPatient.Checked;
                entityHd.IsCorrectMedication = chkIsCorrectMedication.Checked;
                entityHd.IsCorrectStrength = chkIsCorrectStrength.Checked;
                entityHd.IsCorrectFrequency = chkIsCorrectFrequency.Checked;
                entityHd.IsCorrectDosage = chkIsCorrectDosage.Checked;
                entityHd.IsCorrectRoute = chkIsCorrectRoute.Checked;
                entityHd.IsHasDrugInteraction = chkIsHasDrugInteraction.Checked;
                entityHd.IsHasDuplication = chkIsHasDuplication.Checked;
                entityHd.IsADChecked = chkIsADChecked.Checked;
                entityHd.IsFARChecked = chkIsFARChecked.Checked;
                entityHd.IsKLNChecked = chkIsKLNChecked.Checked;
                entityHd.IsCreatedBySystem = true;
                entityHd.IsOrderedByPhysician = false;
                entityHd.PrescriptionOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.PrescriptionDate, ctx);
                entityHd.Remarks = txtRemarks.Text;
                entityHd.SendOrderBy = AppSession.UserLogin.UserID;
                entityHd.SendOrderDateTime = sendOrderDateTime;
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                prescriptionOrderID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
            }
            else
            {
                prescriptionOrderID = Convert.ToInt32(hdnPrescriptionOrderID.Value);
            }

        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                int PrescriptionOrderID = 0;
                SavePrescriptionOrderHd(ctx, ref PrescriptionOrderID);

                retval = PrescriptionOrderID.ToString();
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
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

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            try
            {
                Int32 prescriptionOrderID = Convert.ToInt32(hdnPrescriptionOrderID.Value);
                PrescriptionOrderHd orderHd = BusinessLayer.GetPrescriptionOrderHd(prescriptionOrderID);
                if (orderHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                {
                    if (cboPrescriptionTypeMainPage.Value.ToString() == Constant.PrescriptionType.DISCHARGE_PRESCRIPTION)
                    {
                        string filterExpression = string.Format("PrescriptionOrderID = {0} AND IsUsingUDD = 1", hdnPrescriptionOrderID.Value);
                        List<PrescriptionOrderDt> lstEntity = BusinessLayer.GetPrescriptionOrderDtList(filterExpression);
                        if (lstEntity.Count() == 0)
                        {
                            orderHd.GCPrescriptionType = cboPrescriptionTypeMainPage.Value.ToString();
                        }
                    }
                    else
                    {
                        orderHd.GCPrescriptionType = cboPrescriptionTypeMainPage.Value.ToString();
                    }

                    if (orderHd.GCOrderStatus == Constant.OrderStatus.OPEN)
                    {
                        orderHd.GCOrderStatus = Constant.OrderStatus.RECEIVED;
                    }
                    orderHd.IsCorrectPatient = chkIsCorrectPatient.Checked;
                    orderHd.IsCorrectMedication = chkIsCorrectMedication.Checked;
                    orderHd.IsCorrectStrength = chkIsCorrectStrength.Checked;
                    orderHd.IsCorrectFrequency = chkIsCorrectFrequency.Checked;
                    orderHd.IsCorrectDosage = chkIsCorrectDosage.Checked;
                    orderHd.IsCorrectRoute = chkIsCorrectRoute.Checked;
                    orderHd.IsHasDrugInteraction = chkIsHasDrugInteraction.Checked;
                    orderHd.IsHasDuplication = chkIsHasDuplication.Checked;
                    orderHd.IsADChecked = chkIsADChecked.Checked;
                    orderHd.IsFARChecked = chkIsFARChecked.Checked;
                    orderHd.IsKLNChecked = chkIsKLNChecked.Checked;
                    orderHd.Remarks = txtRemarks.Text;
                    orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    orderHd.LastUpdatedDate = DateTime.Now;
                    BusinessLayer.UpdatePrescriptionOrderHd(orderHd);
                }
                else
                {
                    errMessage = "Order Sudah Di Proses Oleh User lain, Silahkan refresh halaman ini.";
                    result = false;
                }

                retval = prescriptionOrderID.ToString();
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                result = false;
            }
            return result;
        }
        #endregion

        #region Void Entity
        protected override bool OnVoidRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderHdDao entityHdDao = new PrescriptionOrderHdDao(ctx);
            PrescriptionOrderDtDao entityDtDao = new PrescriptionOrderDtDao(ctx);

            try
            {
                Int32 PrescriptionOrderID = Convert.ToInt32(hdnPrescriptionOrderID.Value);
                PrescriptionOrderHd entity = entityHdDao.Get(PrescriptionOrderID);
                if (entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                {
                    entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entity);

                    List<PrescriptionOrderDt> lstOrderDt = BusinessLayer.GetPrescriptionOrderDtList(string.Format("PrescriptionOrderID = {0} AND IsDeleted = 0", entity.PrescriptionOrderID), ctx);
                    foreach (PrescriptionOrderDt orderDt in lstOrderDt)
                    {
                        orderDt.GCPrescriptionOrderStatus = Constant.OrderStatus.CANCELLED;
                        orderDt.GCVoidReason = Constant.DeleteReason.OTHER;
                        orderDt.VoidReason = "Proses void dari farmasi (Medication Order)";
                        orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(orderDt);
                    }

                    #region Log PrescriptionTaskOrder
                    Helper.InsertPrescriptionOrderTaskLog(ctx, entity.PrescriptionOrderID, Constant.PrescriptionTaskLogStatus.Void, AppSession.UserLogin.UserID, false);
                    #endregion

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = string.Format("Maaf, order nomor {0} sudah diproses. Harap refresh halaman ini.", entity.PrescriptionOrderNo);
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
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

        #region Proposed Entity
        protected override bool OnProposeRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderHdDao prescriptionOrderHdDao = new PrescriptionOrderHdDao(ctx);
            PrescriptionOrderDtDao prescriptionOrderDtDao = new PrescriptionOrderDtDao(ctx);
            MedicationScheduleDao entityMedicationScheduleDao = new MedicationScheduleDao(ctx);
            try
            {
                PrescriptionOrderHd orderHd = prescriptionOrderHdDao.Get(Convert.ToInt32(hdnPrescriptionOrderID.Value));

                if (orderHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL || orderHd.GCTransactionStatus == Constant.TransactionStatus.APPROVED)
                {
                    if (hdnIsReviewPrescriptionMandatoryForProposedTransaction.Value == "1")
                    {
                        if (orderHd.IsCorrectPatient || orderHd.IsCorrectMedication || orderHd.IsCorrectStrength || orderHd.IsCorrectFrequency || orderHd.IsCorrectDosage || orderHd.IsCorrectRoute || orderHd.IsHasDrugInteraction || orderHd.IsHasDuplication || orderHd.IsCorrectTimeOfGiving || orderHd.IsADChecked || orderHd.IsFARChecked || orderHd.IsKLNChecked)
                        {
                            result = true;
                        }
                        else
                        {
                            result = false;
                        }
                    }

                    if (result)
                    {
                        #region PrescriptionOrderHd
                        String filterExpression = string.Format("PrescriptionOrderID = {0} AND IsDeleted = 0 ORDER BY PrescriptionOrderDetailID", hdnPrescriptionOrderID.Value);
                        List<PrescriptionOrderDt> lstEntityDt = BusinessLayer.GetPrescriptionOrderDtList(filterExpression, ctx);
                        orderHd.LocationID = Convert.ToInt32(cboLocation.Value);
                        orderHd.GCOrderStatus = Constant.OrderStatus.IN_PROGRESS;
                        orderHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                        orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        #endregion

                        #region Split Selected Items into UDD and Non-UDD staging list
                        int ct = 0;
                        List<PrescriptionOrderDt> lstProcessAsUDD = new List<PrescriptionOrderDt>();
                        List<PrescriptionOrderDt> lstProcessToCharges = new List<PrescriptionOrderDt>();

                        foreach (PrescriptionOrderDt objDt in lstEntityDt)
                        {
                            if (objDt.GCPrescriptionOrderStatus == Constant.OrderStatus.RECEIVED)
                            {
                                if (objDt.IsCompound)
                                {
                                    lstProcessToCharges.Add(objDt);
                                    if (objDt.IsRFlag)
                                        ct += 1;
                                }
                                else
                                {
                                    if (objDt.IsUsingUDD)
                                    {
                                        lstProcessAsUDD.Add(objDt);
                                        ct += 1;
                                    }
                                    else
                                    {
                                        lstProcessToCharges.Add(objDt);
                                    }
                                }
                            }
                        }
                        #endregion

                        #region Process As UDD
                        if (lstProcessAsUDD.Count > 0)
                        {
                            foreach (PrescriptionOrderDt orderDt in lstProcessAsUDD)
                            {
                                orderDt.GCPrescriptionOrderStatus = Constant.OrderStatus.RECEIVED;
                                orderDt.IsUsingUDD = true;
                                orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                prescriptionOrderDtDao.Update(orderDt);
                            }
                        }
                        #endregion

                        #region Process As Charges
                        if (lstProcessToCharges.Count > 0)
                        {
                            CreateCharges(ctx, orderHd, lstProcessToCharges, ref errMessage);
                        }

                        #endregion

                        #region PrescriptionOrderHd
                        prescriptionOrderHdDao.Update(orderHd);
                        //retval = orderHd.PrescriptionOrderNo;
                        #endregion

                        #region Create Medication Schedule
                        if ((cboPrescriptionTypeMainPage.Value.ToString() == Constant.PrescriptionType.MEDICATION_ORDER) || (cboPrescriptionTypeMainPage.Value.ToString() == Constant.PrescriptionType.TERAPI_BARU) || (cboPrescriptionTypeMainPage.Value.ToString() == Constant.PrescriptionType.PASIEN_BARU || (cboPrescriptionTypeMainPage.Value.ToString() == Constant.PrescriptionType.CITO)))
                        {
                            foreach (PrescriptionOrderDt orderDt in lstProcessAsUDD)
                            {
                                CreateMedicationSchedule(orderDt, entityMedicationScheduleDao);
                            }
                            foreach (PrescriptionOrderDt orderDt in lstProcessToCharges)
                            {
                                CreateMedicationSchedule(orderDt, entityMedicationScheduleDao);
                            }
                        }
                        #endregion

                        if (orderHd.IsOrderedByPhysician) // ini dari order
                        {
                            #region Log PrescriptionTaskOrder
                            Helper.InsertPrescriptionOrderTaskLog(ctx, orderHd.PrescriptionOrderID, Constant.PrescriptionTaskLogStatus.Received, AppSession.UserLogin.UserID, false);
                            #endregion
                        }
                        else
                        {
                            #region Log PrescriptionTaskOrder
                            Helper.InsertPrescriptionOrderTaskLog(ctx, orderHd.PrescriptionOrderID, Constant.PrescriptionTaskLogStatus.Sent, AppSession.UserLogin.UserID, false);
                            Helper.InsertPrescriptionOrderTaskLog(ctx, orderHd.PrescriptionOrderID, Constant.PrescriptionTaskLogStatus.Received, AppSession.UserLogin.UserID, false);
                            #endregion
                        }
                        
                    }
                    else
                    {
                        result = false;
                        errMessage = string.Format("Harap isi telaah resep terlebih dahulu.");
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                    }
                }
                else
                {
                    result = false;
                    errMessage = string.Format("Maaf, order nomor {0} sudah diproses. Harap refresh halaman ini.", orderHd.PrescriptionOrderNo);
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                }

                if (result)
                {
                    ctx.CommitTransaction();
                }
                else
                {
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

        private void CreateMedicationSchedule(PrescriptionOrderDt orderDt, MedicationScheduleDao entityMedicationScheduleDao)
        {
            if (orderDt.IsRFlag)
            {
                decimal duration = 1;

                if (!orderDt.IsAsRequired)
                    duration = orderDt.DosingDuration;
                else
                    duration = 1;

                DateTime StartDate = orderDt.StartDate;
                for (int i = 0; i < duration; i++)
                {
                    DateTime currentDate = StartDate.AddDays(i);
                    int sequenceNo = 1;
                    for (int j = 0; j < orderDt.Frequency; j++)
                    {
                        MedicationSchedule oSchedule = new MedicationSchedule();
                        oSchedule.VisitID = AppSession.RegisteredPatient.VisitID;
                        oSchedule.PrescriptionOrderID = orderDt.PrescriptionOrderID;
                        oSchedule.PrescriptionOrderDetailID = orderDt.PrescriptionOrderDetailID;
                        oSchedule.ItemID = orderDt.ItemID;
                        if (orderDt.IsCompound)
                            oSchedule.ItemName = !string.IsNullOrEmpty(orderDt.CompoundDrugname) ? orderDt.CompoundDrugname : "OBAT RACIKAN";
                        else
                            oSchedule.ItemName = orderDt.DrugName;
                        oSchedule.MedicationDate = currentDate;
                        oSchedule.SequenceNo = sequenceNo.ToString();
                        oSchedule.MedicationTime = GetSequenceMedicationTime(j + 1, orderDt);
                        oSchedule.NumberOfDosage = orderDt.NumberOfDosage;
                        oSchedule.NumberOfDosageInString = orderDt.NumberOfDosageInString;
                        oSchedule.GCDosingUnit = orderDt.GCDosingUnit;
                        oSchedule.ConversionFactor = orderDt.ConversionFactor;
                        oSchedule.ResultQuantity = orderDt.ResultQty;
                        oSchedule.ChargeQuantity = orderDt.ChargeQty;
                        oSchedule.IsAsRequired = orderDt.IsAsRequired;
                        oSchedule.IsIMM = orderDt.IsIMM;
                        oSchedule.IsUsingUDD = orderDt.IsUsingUDD;
                        oSchedule.GCRoute = orderDt.GCRoute;
                        oSchedule.GCCoenamRule = orderDt.GCCoenamRule;
                        oSchedule.MedicationAdministration = orderDt.MedicationAdministration;
                        if (orderDt.IsCompound || !orderDt.IsUsingUDD)
                            oSchedule.GCMedicationStatus = Constant.MedicationStatus.DIPROSES_FARMASI;
                        else
                            oSchedule.GCMedicationStatus = Constant.MedicationStatus.OPEN;
                        oSchedule.IsInternalMedication = true;
                        oSchedule.CreatedBy = AppSession.UserLogin.UserID;
                        sequenceNo++;
                        entityMedicationScheduleDao.Insert(oSchedule);
                    }
                }
            }
        }

        private string GetSequenceMedicationTime(int sequence, PrescriptionOrderDt orderDt)
        {
            string medicationTime = "";
            switch (sequence)
            {
                case 1:
                    medicationTime = orderDt.Sequence1Time;
                    //if (orderDt.Frequency == 1)
                    //{
                    //    if (orderDt.Sequence1Time == "" || orderDt.Sequence1Time == "-")
                    //    {
                    //        if (orderDt.Sequence2Time == "" || orderDt.Sequence2Time == "-")
                    //        {

                    //        }
                    //        else if (orderDt.Sequence3Time == "" || orderDt.Sequence3Time == "-")
                    //        {

                    //        }
                    //        else if (orderDt.Sequence4Time == "" || orderDt.Sequence4Time == "-")
                    //        {

                    //        }
                    //        else if (orderDt.Sequence5Time == "" || orderDt.Sequence6Time == "-")
                    //        {

                    //        }
                    //        else if (orderDt.Sequence6Time == "" || orderDt.Sequence6Time == "-")
                    //        {

                    //        }
                    //    }
                    //    else
                    //    {
                    //        medicationTime = orderDt.Sequence1Time;
                    //    }
                    //}
                    break;
                case 2:
                    medicationTime = orderDt.Sequence2Time;
                    break;
                case 3:
                    medicationTime = orderDt.Sequence3Time;
                    break;
                case 4:
                    medicationTime = orderDt.Sequence4Time;
                    break;
                case 5:
                    medicationTime = orderDt.Sequence5Time;
                    break;
                case 6:
                    medicationTime = orderDt.Sequence6Time;
                    break;
                default:
                    medicationTime = orderDt.StartTime;
                    break;
            }
            return medicationTime;
        }

        private string GetMedicationTime(int sequence, int frequency)
        {
            string medicationTime = "";
            switch (sequence)
            {
                case 1:
                    if (frequency <= 3)
                        medicationTime = "08:00";
                    else if (frequency == 4 || frequency == 5)
                    {
                        medicationTime = "06:00";
                    }
                    else
                    {
                        medicationTime = "02:00";
                    }
                    break;
                case 2:
                    switch (frequency)
                    {
                        case 2:
                            medicationTime = "20:00";
                            break;
                        case 3:
                            medicationTime = "16:00";
                            break;
                        case 4:
                            medicationTime = "12:00";
                            break;
                        case 5:
                            medicationTime = "09:00";
                            break;
                        case 6:
                            medicationTime = "06:00";
                            break;
                        default:
                            medicationTime = "00:00";
                            break;
                    }
                    break;
                case 3:
                    switch (frequency)
                    {
                        case 3:
                            medicationTime = "23:59";
                            break;
                        case 4:
                            medicationTime = "18:00";
                            break;
                        case 5:
                            medicationTime = "12:00";
                            break;
                        case 6:
                            medicationTime = "10:00";
                            break;
                        default:
                            medicationTime = "00:00";
                            break;
                    }
                    break;
                case 4:
                    switch (frequency)
                    {
                        case 4:
                            medicationTime = "23:59";
                            break;
                        case 5:
                            medicationTime = "15:00";
                            break;
                        case 6:
                            medicationTime = "14:00";
                            break;
                        default:
                            medicationTime = "00:00";
                            break;
                    }
                    break;
                case 5:
                    switch (frequency)
                    {
                        case 5:
                            medicationTime = "15:00";
                            break;
                        case 6:
                            medicationTime = "18:00";
                            break;
                        default:
                            medicationTime = "00:00";
                            break;
                    }
                    break;
                case 6:
                    medicationTime = "22:00";
                    break;
                default:
                    medicationTime = "00:00";
                    break;
            }
            return medicationTime;
        }

        private bool CreateCharges(IDbContext ctx, PrescriptionOrderHd orderHd, List<PrescriptionOrderDt> lstSelectedOrderDt, ref string errMessage)
        {
            bool result = true;
            String filterExpression = "1 = 0";
            DateTime transactionDate = Helper.GetDatePickerValue(txtPrescriptionDate.Text);
            string transactionNo = string.Empty;

            PatientChargesDtDao chargesDtDao = new PatientChargesDtDao(ctx);
            PatientChargesHdDao chargesHdDao = new PatientChargesHdDao(ctx);
            PrescriptionOrderDtDao orderDtDao = new PrescriptionOrderDtDao(ctx);
            PrescriptionOrderHdDao orderHdDao = new PrescriptionOrderHdDao(ctx);
            ItemProductDao iProductDao = new ItemProductDao(ctx);

            #region PatientChargesHd
            PatientChargesHd chargesHd = new PatientChargesHd();

            chargesHd.VisitID = orderHd.VisitID;
            chargesHd.TransactionDate = DateTime.Now.Date;
            chargesHd.TransactionTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            chargesHd.PrescriptionOrderID = orderHd.PrescriptionOrderID;
            chargesHd.HealthcareServiceUnitID = orderHd.DispensaryServiceUnitID;
            chargesHd.CreatedBy = AppSession.UserLogin.UserID;
            chargesHd.TransactionCode = Constant.TransactionCode.PRESCRIPTION_INPATIENT;
            #endregion

            List<PatientChargesDt> lstChargesDt = new List<PatientChargesDt>();
            List<PrescriptionOrderDt> lstOrderDt = new List<PrescriptionOrderDt>();

            foreach (PrescriptionOrderDt orderDt in lstSelectedOrderDt)
            {
                if (orderDt.GCPrescriptionOrderStatus == Constant.TestOrderStatus.CANCELLED || orderDt.GCPrescriptionOrderStatus == Constant.TestOrderStatus.IN_PROGRESS)
                {
                    result = false;
                    errMessage = "Order Sudah Di Proses Oleh User lain, Silahkan Merefresh halaman ini.";
                    break;
                }

                PatientChargesDt chargesDt = new PatientChargesDt();

                filterExpression = string.Format("ItemID = {0}", orderDt.ItemID);
                vItemProduct item = BusinessLayer.GetvItemProductList(filterExpression, ctx).FirstOrDefault();

                if (orderDt == null)
                {
                    result = false;
                    errMessage = string.Format("Cannot found item product information for item {0}", orderDt.DrugName);
                    break;
                }

                #region PatientChargesDt
                chargesDt.PrescriptionOrderDetailID = orderDt.PrescriptionOrderDetailID;
                chargesDt.ItemID = (int)orderDt.ItemID;
                chargesDt.LocationID = Convert.ToInt32(cboLocation.Value);
                chargesDt.ChargeClassID = AppSession.RegisteredPatient.ChargeClassID;

                List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(AppSession.RegisteredPatient.RegistrationID, AppSession.RegisteredPatient.VisitID, chargesDt.ChargeClassID, (int)orderDt.ItemID, 2, transactionDate, ctx);

                decimal basePrice = 0;
                decimal basePriceComp1 = 0;
                decimal basePriceComp2 = 0;
                decimal basePriceComp3 = 0;
                decimal price = 0;
                decimal priceComp1 = 0;
                decimal priceComp2 = 0;
                decimal priceComp3 = 0;
                bool isDiscountUsedComp = false;
                decimal discountAmount = 0;
                decimal discountAmountComp1 = 0;
                decimal discountAmountComp2 = 0;
                decimal discountAmountComp3 = 0;
                decimal coverageAmount = 0;
                bool isDiscountInPercentage = false;
                bool isDiscountInPercentageComp1 = false;
                bool isDiscountInPercentageComp2 = false;
                bool isDiscountInPercentageComp3 = false;
                bool isCoverageInPercentage = false;
                decimal costAmount = 0;

                if (list.Count > 0)
                {
                    GetCurrentItemTariff obj = list[0];
                    basePrice = obj.BasePrice;
                    basePriceComp1 = obj.BasePriceComp1;
                    basePriceComp2 = obj.BasePriceComp2;
                    basePriceComp3 = obj.BasePriceComp3;
                    price = obj.Price;
                    priceComp1 = obj.PriceComp1;
                    priceComp2 = obj.PriceComp2;
                    priceComp3 = obj.PriceComp3;
                    isDiscountUsedComp = obj.IsDiscountUsedComp;
                    discountAmount = obj.DiscountAmount;
                    discountAmountComp1 = obj.DiscountAmountComp1;
                    discountAmountComp2 = obj.DiscountAmountComp2;
                    discountAmountComp3 = obj.DiscountAmountComp3;
                    coverageAmount = obj.CoverageAmount;
                    isDiscountInPercentage = obj.IsDiscountInPercentage;
                    isDiscountInPercentageComp1 = obj.IsDiscountInPercentageComp1;
                    isDiscountInPercentageComp2 = obj.IsDiscountInPercentageComp2;
                    isDiscountInPercentageComp3 = obj.IsDiscountInPercentageComp3;
                    isCoverageInPercentage = obj.IsCoverageInPercentage;
                    costAmount = obj.CostAmount;
                }

                chargesDt.BaseTariff = basePrice;
                chargesDt.Tariff = price;
                chargesDt.BaseComp1 = basePriceComp1;
                chargesDt.BaseComp2 = basePriceComp2;
                chargesDt.BaseComp3 = basePriceComp3;
                chargesDt.TariffComp1 = priceComp1;
                chargesDt.TariffComp2 = priceComp2;
                chargesDt.TariffComp3 = priceComp3;
                chargesDt.CostAmount = costAmount;

                chargesDt.GCBaseUnit = item.GCItemUnit;
                chargesDt.GCItemUnit = item.GCItemUnit;
                chargesDt.ParamedicID = Convert.ToInt32(orderHd.ParamedicID);

                chargesDt.IsVariable = false;
                chargesDt.IsUnbilledItem = false;

                #region dispense full since it is not UDD

                decimal dosageQty = orderDt.NumberOfDosage;
                decimal compoundQty = orderDt.CompoundQty;
                decimal resultQty = orderDt.ResultQty;
                decimal chargeQty = orderDt.ChargeQty;
                decimal baseQty = orderDt.ResultQty;

                if (item.GCConsumptionDeductionType == Constant.InventoryRoundingStatus.ROUND_UP)
                    resultQty = Math.Ceiling(resultQty);

                if (item.GCStockDeductionType == Constant.InventoryRoundingStatus.ROUND_UP)
                    chargeQty = Math.Ceiling(chargeQty);

                baseQty = resultQty;

                chargesDt.UsedQuantity = resultQty;
                chargesDt.BaseQuantity = baseQty;
                chargesDt.ChargedQuantity = chargeQty;

                if (chargesDt.ChargedQuantity > 0)
                {
                    chargesDt.DiscountAmount = discountAmount;
                    chargesDt.DiscountComp1 = chargesDt.DiscountAmount / chargesDt.ChargedQuantity;
                    chargesDt.PrescriptionFeeAmount = Convert.ToDecimal(hdnPrescriptionFeeAmount.Value);

                    if (orderDt.EmbalaceID != null && orderDt.IsRFlag)
                    {
                        EmbalaceHd embalaceHd = BusinessLayer.GetEmbalaceHdList(string.Format("EmbalaceID = {0}", orderDt.EmbalaceID)).FirstOrDefault();
                        if (embalaceHd != null)
                        {
                            if (embalaceHd.IsUsingRangePricing)
                            {
                                EmbalaceDt embalaceDt = BusinessLayer.GetEmbalaceDtList(string.Format("EmbalaceID = {0} AND StartingQty <= {1} AND EndingQty >= {1}", orderDt.EmbalaceID, orderDt.EmbalaceQty)).FirstOrDefault();
                                if (embalaceDt != null)
                                    chargesDt.EmbalaceAmount = Convert.ToDecimal(embalaceDt.Tariff * orderDt.EmbalaceQty);
                                else
                                    chargesDt.EmbalaceAmount = 0;
                            }
                            else
                            {
                                chargesDt.EmbalaceAmount = Convert.ToDecimal(embalaceHd.Tariff * orderDt.EmbalaceQty);
                            }
                        }

                    }
                    else
                    {
                        chargesDt.EmbalaceAmount = 0;
                    }
                }
                else
                {
                    chargesDt.PrescriptionFeeAmount = 0;
                    chargesDt.EmbalaceAmount = 0;
                }
                #endregion

                decimal grossLineAmount = (chargesDt.ChargedQuantity * price) + chargesDt.EmbalaceAmount + chargesDt.PrescriptionFeeAmount;

                decimal totalDiscountAmount = 0;
                decimal totalDiscountAmount1 = 0;
                decimal totalDiscountAmount2 = 0;
                decimal totalDiscountAmount3 = 0;

                if (isDiscountInPercentage || isDiscountInPercentageComp1 || isDiscountInPercentageComp2 || isDiscountInPercentageComp3)
                {
                    if (isDiscountUsedComp)
                    {
                        if (priceComp1 > 0)
                        {
                            if (isDiscountInPercentageComp1)
                            {
                                totalDiscountAmount1 = priceComp1 * discountAmountComp1 / 100;
                                chargesDt.DiscountPercentageComp1 = discountAmountComp1;
                            }
                            else
                            {
                                totalDiscountAmount1 = discountAmountComp1;
                            }
                        }

                        if (priceComp2 > 0)
                        {
                            if (isDiscountInPercentageComp2)
                            {
                                totalDiscountAmount2 = priceComp2 * discountAmountComp2 / 100;
                                chargesDt.DiscountPercentageComp2 = discountAmountComp2;
                            }
                            else
                            {
                                totalDiscountAmount2 = discountAmountComp2;
                            }
                        }

                        if (priceComp3 > 0)
                        {
                            if (isDiscountInPercentageComp3)
                            {
                                totalDiscountAmount3 = priceComp3 * discountAmountComp3 / 100;
                                chargesDt.DiscountPercentageComp3 = discountAmountComp3;
                            }
                            else
                            {
                                totalDiscountAmount3 = discountAmountComp3;
                            }
                        }
                    }
                    else
                    {
                        if (priceComp1 > 0)
                        {
                            totalDiscountAmount1 = priceComp1 * discountAmount / 100;
                            chargesDt.DiscountPercentageComp1 = discountAmount;
                        }

                        if (priceComp2 > 0)
                        {
                            totalDiscountAmount2 = priceComp2 * discountAmount / 100;
                            chargesDt.DiscountPercentageComp2 = discountAmount;
                        }

                        if (priceComp3 > 0)
                        {
                            totalDiscountAmount3 = priceComp3 * discountAmount / 100;
                            chargesDt.DiscountPercentageComp3 = discountAmount;
                        }
                    }

                    if (chargesDt.DiscountPercentageComp1 > 0)
                    {
                        chargesDt.IsDiscountInPercentageComp1 = true;
                    }

                    if (chargesDt.DiscountPercentageComp2 > 0)
                    {
                        chargesDt.IsDiscountInPercentageComp2 = true;
                    }

                    if (chargesDt.DiscountPercentageComp3 > 0)
                    {
                        chargesDt.IsDiscountInPercentageComp3 = true;
                    }
                }
                else
                {
                    if (isDiscountUsedComp)
                    {
                        if (priceComp1 > 0)
                            totalDiscountAmount1 = discountAmountComp1;
                        if (priceComp2 > 0)
                            totalDiscountAmount2 = discountAmountComp2;
                        if (priceComp3 > 0)
                            totalDiscountAmount3 = discountAmountComp3;
                    }
                    else
                    {
                        if (priceComp1 > 0)
                            totalDiscountAmount1 = discountAmount;
                        if (priceComp2 > 0)
                            totalDiscountAmount2 = discountAmount;
                        if (priceComp3 > 0)
                            totalDiscountAmount3 = discountAmount;
                    }
                }

                totalDiscountAmount = (totalDiscountAmount1 + totalDiscountAmount2 + totalDiscountAmount3) * (chargesDt.ChargedQuantity);

                if (grossLineAmount > 0)
                {
                    if (totalDiscountAmount > grossLineAmount)
                    {
                        totalDiscountAmount = grossLineAmount;
                    }
                }

                decimal total = grossLineAmount - totalDiscountAmount;
                decimal totalPayer = 0;
                if (isCoverageInPercentage)
                {
                    totalPayer = total * coverageAmount / 100;
                }
                else
                {
                    totalPayer = coverageAmount * chargesDt.ChargedQuantity;
                }

                if (total == 0)
                {
                    totalPayer = total;
                }
                else
                {
                    if (totalPayer < 0 && totalPayer < total)
                    {
                        totalPayer = total;
                    }
                    else if (totalPayer > 0 & totalPayer > total)
                    {
                        totalPayer = total;
                    }
                }

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                ItemPlanning iPlanning = BusinessLayer.GetItemPlanningList(string.Format("HealthcareID = '{0}' AND ItemID = {1} AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, chargesDt.ItemID), ctx).FirstOrDefault();
                chargesDt.AveragePrice = iPlanning.AveragePrice;
                chargesDt.CostAmount = iPlanning.UnitPrice;

                if (chargesDt.ItemID != null && chargesDt.ItemID != 0)
                {
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    ItemProduct iProduct = iProductDao.Get(chargesDt.ItemID);
                    chargesDt.HETAmount = iProduct.HETAmount;
                }

                chargesDt.ConversionFactor = orderDt.ConversionFactor;
                chargesDt.IsCITO = false;
                chargesDt.CITOAmount = 0;
                chargesDt.IsComplication = false;
                chargesDt.ComplicationAmount = 0;

                chargesDt.IsDiscount = totalDiscountAmount != 0 ? true : false;
                chargesDt.DiscountAmount = totalDiscountAmount;
                chargesDt.DiscountComp1 = totalDiscountAmount1;
                chargesDt.DiscountComp2 = totalDiscountAmount2;
                chargesDt.DiscountComp3 = totalDiscountAmount3;

                decimal oPatientAmount = total - totalPayer;
                decimal oPayerAmount = totalPayer;
                decimal oLineAmount = total;

                if (hdnIsEndingAmountRoundingTo1.Value == "1")
                {
                    decimal upPatientAmount = Math.Ceiling(oPatientAmount);
                    decimal diffUpPatientAmount = upPatientAmount - oPatientAmount;
                    if (diffUpPatientAmount >= Convert.ToDecimal("0.5"))
                    {
                        oPatientAmount = Math.Floor(oPatientAmount);
                    }
                    else
                    {
                        oPatientAmount = Math.Ceiling(oPatientAmount);
                    }

                    decimal upPayerAmount = Math.Ceiling(oPayerAmount);
                    decimal diffUpPayerAmount = upPayerAmount - oPayerAmount;
                    if (diffUpPayerAmount >= Convert.ToDecimal("0.5"))
                    {
                        oPayerAmount = Math.Floor(oPayerAmount);
                    }
                    else
                    {
                        oPayerAmount = Math.Ceiling(oPayerAmount);
                    }

                    oLineAmount = oPatientAmount + oPayerAmount;
                }

                if (hdnIsEndingAmountRoundingTo100.Value == "1")
                {
                    oPatientAmount = Math.Ceiling(oPatientAmount / 100) * 100;
                    oPayerAmount = Math.Ceiling(oPayerAmount / 100) * 100;
                    oLineAmount = oPatientAmount + oPayerAmount;
                }

                chargesDt.PatientAmount = oPatientAmount;
                chargesDt.PayerAmount = oPayerAmount;
                chargesDt.LineAmount = oLineAmount;

                chargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.OPEN;
                chargesDt.CreatedBy = AppSession.UserLogin.UserID;
                lstChargesDt.Add(chargesDt);

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                PrescriptionOrderDt entityOrderDt = orderDtDao.Get(Convert.ToInt32(orderDt.PrescriptionOrderDetailID));
                entityOrderDt.GCPrescriptionOrderStatus = Constant.OrderStatus.IN_PROGRESS;
                lstOrderDt.Add(entityOrderDt);

                #endregion
            }

            #region Commit to Database
            chargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(chargesHd.TransactionCode, chargesHd.TransactionDate, ctx);
            chargesHd.GCTransactionStatus = Constant.TransactionStatus.OPEN; //OPEN
            chargesHd.CreatedBy = AppSession.UserLogin.UserID;
            ctx.CommandType = CommandType.Text;
            ctx.Command.Parameters.Clear();
            int transactionID = chargesHdDao.InsertReturnPrimaryKeyID(chargesHd);
            transactionNo = chargesHd.TransactionNo;

            foreach (PatientChargesDt item in lstChargesDt)
            {
                item.TransactionID = transactionID;
                item.IsApproved = false;
                item.CreatedBy = AppSession.UserLogin.UserID;
                chargesDtDao.Insert(item);
            }

            foreach (PrescriptionOrderDt orderDt in lstOrderDt)
            {
                orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                orderDtDao.Update(orderDt);
            }
            #endregion

            return result;
        }

        private void PrintOrder(int orderID)
        {

        }

        #endregion

        #region Process Detail
        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string lstRecordID = hdnSelectedID.Value;
            string lstFrequency = hdnSelectedFrequency.Value;
            string lstStartDate = hdnSelectedStartDate.Value;
            string lstSelectedTime1 = hdnSelectedTime1.Value;
            string lstSelectedTime2 = hdnSelectedTime2.Value;
            string lstSelectedTime3 = hdnSelectedTime3.Value;
            string lstSelectedTime4 = hdnSelectedTime4.Value;
            string lstSelectedTime5 = hdnSelectedTime5.Value;
            string lstSelectedTime6 = hdnSelectedTime6.Value;
            string result = "";
            string errMessage = "";
            string isAllow = "notConfirm";
            int prescriptionOrderID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            string errorMessage = string.Empty;

            if (param[0] == "save")
            {
                if (!IsValidated(lstFrequency, lstStartDate, lstSelectedTime1, lstSelectedTime2, lstSelectedTime3, lstSelectedTime4, lstSelectedTime5, lstSelectedTime6, ref errorMessage))
                {
                    result += string.Format("fail|0|{0}", "Waktu pemberian obat sequence harus sesuai dengan format jam (00:00 s/d 23:59)");
                }
                else
                {
                    if (hdnEntryID.Value.ToString() != "")
                    {
                        prescriptionOrderID = Convert.ToInt32(hdnPrescriptionOrderID.Value);
                        if (OnSaveEditRecordEntityDt(ref errMessage, ref isAllow))
                            result += "success";
                        else
                            result += string.Format("fail|{0}|{1}", isAllow, errMessage);
                    }
                    else
                    {
                        if (OnSaveAddRecordEntityDt(ref errMessage, ref prescriptionOrderID, ref isAllow))
                            result += "success";
                        else
                            result += string.Format("fail|{0}|{1}", isAllow, errMessage);
                    }
                }
            }
            else if (param[0] == "delete")
            {
                prescriptionOrderID = Convert.ToInt32(hdnEntryID.Value);
                if (OnDeleteEntityDt(ref errMessage, prescriptionOrderID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpPrescriptionOrderID"] = prescriptionOrderID.ToString();
        }

        private void ControlToEntity(PrescriptionOrderDt entity)
        {
            entity.IsRFlag = true;
            if (hdnDrugID.Value.ToString() != "")
                entity.ItemID = Convert.ToInt32(hdnDrugID.Value);
            else
                entity.ItemID = null;
            entity.DrugName = Request.Form[txtDrugName.UniqueID];
            entity.GenericName = txtGenericName.Text;
            if (hdnSignaID.Value == "" || hdnSignaID.Value == "0")
                entity.SignaID = null;
            else
                entity.SignaID = Convert.ToInt32(hdnSignaID.Value);
            entity.GCDrugForm = cboForm.Value.ToString();
            if (cboCoenamRule.Value != null && cboCoenamRule.Value.ToString() != "")
                entity.GCCoenamRule = cboCoenamRule.Value.ToString();
            else
                entity.GCCoenamRule = null;
            if (hdnGCDoseUnit.Value != "")
            {
                entity.Dose = Convert.ToDecimal(Request.Form[txtStrengthAmount.UniqueID]);
                entity.GCDoseUnit = hdnGCDoseUnit.Value;
            }
            else
            {
                entity.Dose = null;
                entity.GCDoseUnit = null;
            }
            entity.GCDosingFrequency = cboFrequencyTimeline.Value.ToString();
            entity.Frequency = Convert.ToInt16(txtFrequencyNumber.Text.Replace(",", "."));
            entity.NumberOfDosage = Convert.ToDecimal(txtDosingDose.Text.Replace(",", "."));
            entity.ConversionFactor = 1;
            if (hdnGCItemUnit.Value != cboDosingUnit.Value.ToString())
            {
                if (entity.Dose > 0 && (cboDosingUnit.Value.ToString() == hdnGCDoseUnit.Value))
                {
                    entity.ConversionFactor = 1 / Convert.ToDecimal(entity.Dose);
                }
            }
            entity.GCDosingUnit = cboDosingUnit.Value.ToString();
            entity.GCRoute = cboMedicationRoute.Value.ToString();
            entity.StartDate = Helper.GetDatePickerValue(txtStartDate);
            entity.StartTime = txtStartTime.Text;
            entity.Sequence1Time = txtStartTime1.Text;
            entity.Sequence2Time = txtStartTime2.Text;
            entity.Sequence3Time = txtStartTime3.Text;
            entity.Sequence4Time = txtStartTime4.Text;
            entity.Sequence5Time = txtStartTime5.Text;
            entity.Sequence6Time = txtStartTime6.Text;
            entity.IsAsRequired = chkIsAsRequired.Checked ? true : false;
            entity.IsIMM = chkIsIMM.Checked ? true : false;
            entity.IsUsingUDD = chkIsAsRequired.Checked ? false : chkIsUsingUDD.Checked;
            entity.MedicationPurpose = txtPurposeOfMedication.Text;
            entity.MedicationAdministration = txtMedicationAdministration.Text;
            entity.DosingDuration = Convert.ToDecimal(txtDosingDuration.Text.Replace(",", "."));
            if (!entity.IsUsingUDD)
            {
                entity.DispenseQty = Convert.ToDecimal(txtDispenseQty.Text.Replace(",", "."));
                entity.TakenQty = Convert.ToDecimal(txtTakenQty.Text.Replace(",", "."));
                entity.ChargeQty = entity.TakenQty;
                entity.ResultQty = entity.TakenQty;
            }
            else
            {
                entity.DispenseQty = 0;
                entity.TakenQty = 0;
                entity.ChargeQty = 0;
                entity.ResultQty = 0;
            }
            entity.GCPrescriptionOrderStatus = Constant.OrderStatus.RECEIVED;
        }

        private bool OnSaveAddRecordEntityDt(ref string errMessage, ref int prescriptionOrderID, ref string isAllow)
        {
            bool result = true;
            errMessage = string.Empty;
            result = IsPrescriptionItemValid(ref errMessage, ref isAllow, "add");
            if (result || isAllow.Equals("confirm"))
            {
                IDbContext ctx = DbFactory.Configure(true);
                PrescriptionOrderDtDao entityDtDao = new PrescriptionOrderDtDao(ctx);
                PrescriptionOrderHdDao prescriptionOrderHdDao = new PrescriptionOrderHdDao(ctx);
                try
                {
                    SavePrescriptionOrderHd(ctx, ref prescriptionOrderID);
                    if (prescriptionOrderHdDao.Get(prescriptionOrderID).GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                    {
                        PrescriptionOrderDt entityDt = new PrescriptionOrderDt();
                        ControlToEntity(entityDt);
                        entityDt.PrescriptionOrderID = prescriptionOrderID;
                        entityDt.CreatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Insert(entityDt);
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Order Sudah Di Proses Oleh User lain, Silahkan refresh halaman ini.";
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

        private bool OnSaveEditRecordEntityDt(ref string errMessage, ref string isAllow)
        {
            bool result = true;
            errMessage = string.Empty;
            result = IsPrescriptionItemValid(ref errMessage, ref isAllow, "edit");
            if (result || isAllow.Equals("confirm"))
            {
                IDbContext ctx = DbFactory.Configure(true);
                PrescriptionOrderDtDao entityDtDao = new PrescriptionOrderDtDao(ctx);
                PrescriptionOrderHdDao prescriptionOrderHdDao = new PrescriptionOrderHdDao(ctx);
                try
                {
                    if (prescriptionOrderHdDao.Get(Convert.ToInt32(hdnPrescriptionOrderID.Value)).GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                    {
                        PrescriptionOrderDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                        if (!entityDt.IsDeleted)
                        {
                            ControlToEntity(entityDt);
                            entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDt.LastUpdatedDate = DateTime.Now;
                            entityDtDao.Update(entityDt);
                            ctx.CommitTransaction();
                        }
                    }
                    else
                    {
                        result = false;
                        errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
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

        private bool OnDeleteEntityDt(ref string errMessage, int ID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderDtDao entityDtDao = new PrescriptionOrderDtDao(ctx);
            PrescriptionOrderHdDao prescriptionOrderHdDao = new PrescriptionOrderHdDao(ctx);
            try
            {
                if (prescriptionOrderHdDao.Get(Convert.ToInt32(hdnPrescriptionOrderID.Value)).GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                {
                    PrescriptionOrderDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                    if (!entityDt.IsDeleted)
                    {
                        entityDt.IsDeleted = true;
                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(entityDt);
                        ctx.CommitTransaction();
                    }
                }
                else
                {
                    ctx.RollBackTransaction();
                    errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
                    result = false;
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

        private bool IsValidated(string lstFrequency, string lstStartDate, string lstTime1, string lstTime2, string lstTime3, string lstTime4, string lstTime5, string lstTime6, ref string result)
        {
            StringBuilder message = new StringBuilder();

            if (string.IsNullOrEmpty(txtPrescriptionTime.Text))
            {
                message.AppendLine("Jam Order harus sesuai dengan format jam (00:00 s/d 23:59)|");
            }
            else
            {
                if (!Methods.ValidateTimeFormat(txtPrescriptionTime.Text))
                    message.AppendLine("Format Jam Order Operasi tidak sesuai format (HH:MM)|");
                else
                {
                    DateTime startDateTime = DateTime.ParseExact(string.Format("{0} {1}", txtPrescriptionDate.Text, txtPrescriptionTime.Text), Common.Constant.FormatString.DATE_TIME_FORMAT_4, CultureInfo.InvariantCulture, DateTimeStyles.None);

                    if (startDateTime.Date > DateTime.Now.Date)
                    {
                        message.AppendLine("Tanggal Order harus lebih kecil atau sama dengan tanggal hari ini.|");
                    }
                }
            }

            #region Validate Medication Time
            string[] lstMedicationTime = lstTime1.Split(',');
            foreach (string time in lstMedicationTime)
            {
                if (time != "-")
                {
                    if (!string.IsNullOrEmpty(time) || time == "__:__")
                    {
                        Regex reg = new Regex(@"^(?:[01][0-9]|2[0-3]):[0-5][0-9]$");
                        if (!reg.IsMatch(time))
                        {
                            message.AppendLine("Waktu pemberian obat sequence #1 harus sesuai dengan format jam (00:00 s/d 23:59)|");
                        }
                    }
                }
            }

            lstMedicationTime = lstTime2.Split(',');
            foreach (string time in lstMedicationTime)
            {
                if (time != "-")
                {
                    if (!string.IsNullOrEmpty(time) || time == "__:__")
                    {
                        Regex reg = new Regex(@"^(?:[01][0-9]|2[0-3]):[0-5][0-9]$");
                        if (!reg.IsMatch(time))
                        {
                            message.AppendLine("Waktu pemberian obat sequence #2 harus sesuai dengan format jam (00:00 s/d 23:59)|");
                        }
                    }
                }
            }

            lstMedicationTime = lstTime3.Split(',');
            foreach (string time in lstMedicationTime)
            {
                if (time != "-")
                {
                    if (!string.IsNullOrEmpty(time) || time == "__:__")
                    {
                        Regex reg = new Regex(@"^(?:[01][0-9]|2[0-3]):[0-5][0-9]$");
                        if (!reg.IsMatch(time))
                        {
                            message.AppendLine("Waktu pemberian obat sequence #3 harus sesuai dengan format jam (00:00 s/d 23:59)|");
                        }
                    }
                }
            }

            lstMedicationTime = lstTime4.Split(',');
            foreach (string time in lstMedicationTime)
            {
                if (time != "-")
                {
                    if (!string.IsNullOrEmpty(time) || time == "__:__")
                    {
                        Regex reg = new Regex(@"^(?:[01][0-9]|2[0-3]):[0-5][0-9]$");
                        if (!reg.IsMatch(time))
                        {
                            message.AppendLine("Waktu pemberian obat sequence #4 harus sesuai dengan format jam (00:00 s/d 23:59)|");
                        }
                    }
                }
            }

            lstMedicationTime = lstTime5.Split(',');
            foreach (string time in lstMedicationTime)
            {
                if (time != "-")
                {
                    if (!string.IsNullOrEmpty(time) || time == "__:__")
                    {
                        Regex reg = new Regex(@"^(?:[01][0-9]|2[0-3]):[0-5][0-9]$");
                        if (!reg.IsMatch(time))
                        {
                            message.AppendLine("Waktu pemberian obat sequence #5 harus sesuai dengan format jam (00:00 s/d 23:59)|");
                        }
                    }
                }
            }

            lstMedicationTime = lstTime6.Split(',');
            foreach (string time in lstMedicationTime)
            {
                if (time != "-")
                {
                    if (!string.IsNullOrEmpty(time) || time == "__:__")
                    {
                        Regex reg = new Regex(@"^(?:[01][0-9]|2[0-3]):[0-5][0-9]$");
                        if (!reg.IsMatch(time))
                        {
                            message.AppendLine("Waktu pemberian obat sequence #6 harus sesuai dengan format jam (00:00 s/d 23:59)|");
                        }
                    }
                }
            }
            #endregion

            if (!string.IsNullOrEmpty(message.ToString()))
            {
                result = message.ToString().Replace(@"|", "<br />");
            }
            return result == string.Empty;
        }
        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            string[] param = type.Split(';');
            string gcDeleteReason = param[1];
            string reason = param[2];
            bool result = true;

            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderHdDao orderHdDao = new PrescriptionOrderHdDao(ctx);
            PrescriptionOrderDtDao orderDtDao = new PrescriptionOrderDtDao(ctx);
            try
            {
                if (param[0] == "void")
                {
                    Int32 transactionID = Convert.ToInt32(hdnPrescriptionOrderID.Value);

                    //Update Status PrescriptionOrderHd
                    PrescriptionOrderHd orderHd = orderHdDao.Get(transactionID);
                    if (orderHd != null)
                    {
                        List<PrescriptionOrderDt> lstOrderDt = BusinessLayer.GetPrescriptionOrderDtList(string.Format("PrescriptionOrderID = {0} AND IsDeleted = 0", hdnPrescriptionOrderID.Value), ctx);
                        foreach (PrescriptionOrderDt orderDt in lstOrderDt)
                        {
                            if (!orderDt.IsDeleted)
                            {
                                orderDt.GCPrescriptionOrderStatus = Constant.OrderStatus.CANCELLED;
                                orderDt.GCVoidReason = gcDeleteReason;
                                orderDt.VoidReason = reason;
                                orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                orderDtDao.Update(orderDt);
                            }
                        }

                        orderHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                        orderHd.GCOrderStatus = Constant.OrderStatus.CANCELLED;
                        orderHd.GCVoidReason = gcDeleteReason;
                        orderHd.VoidReason = reason;
                        orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        orderHdDao.Update(orderHd);

                        ctx.CommitTransaction();
                    }
                    else
                    {
                        errMessage = "Pembatalan transaksi tidak dapat dilakukan karena header Transaksi tidak ditemukan.";
                        result = false;
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

        private bool IsPrescriptionItemValid(ref string errMessage, ref string isAllow, string flag)
        {
            #region Check For Allergy
            string filterExpression = string.Format("MRN = {0} AND GCAllergenType = '{1}' AND IsDeleted = 0", AppSession.RegisteredPatient.MRN, Constant.AllergenType.DRUG);
            string allergenName = string.Empty;
            vDrugInfo entityItem = null;

            List<PatientAllergy> lstPatientAllergy = null;
            if (hdnDrugID.Value != "")
            {
                entityItem = BusinessLayer.GetvDrugInfoList(string.Format("ItemID = {0}", hdnDrugID.Value))[0];
                if (entityItem.GenericName != string.Empty)
                {
                    filterExpression += string.Format(" AND (Allergen LIKE '%{0}%' OR Allergen LIKE '%{1}%')", entityItem.GenericName, entityItem.ItemName1);
                }
                else filterExpression += string.Format(" AND Allergen LIKE '%{0}%'", entityItem.ItemName1);
                lstPatientAllergy = BusinessLayer.GetPatientAllergyList(filterExpression);
            }

            if (lstPatientAllergy.Count > 0)
            {
                errMessage = string.Format("Pasien memiliki alergi {0} ({1})", txtGenericName.Text, txtDrugName.Text).Replace("()", "");
                return false;
            }
            else
            {
                filterExpression = string.Format("ItemID = {0}", hdnDrugID.Value);
                List<DrugContent> contents = BusinessLayer.GetDrugContentList(filterExpression);
                foreach (DrugContent item in contents)
                {
                    if (item.Keyword != "")
                    {
                        filterExpression = string.Format("MRN = {0} AND GCAllergenType = '{1}' AND Allergen LIKE '%{2}%' AND IsDeleted = 0", AppSession.RegisteredPatient.MRN, Constant.AllergenType.DRUG, item.Keyword);
                        lstPatientAllergy = BusinessLayer.GetPatientAllergyList(filterExpression);
                        if (lstPatientAllergy.Count > 0)
                        {
                            errMessage = string.Format("Pasien memiliki alergi {0} ({1})", item.ContentText, txtDrugName.Text).Replace("()", "");
                            return false;
                        }
                    }
                }
            }
            #endregion

            List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}','{2}')", Constant.SettingParameter.FM_KONTROL_ADVERSE_REACTION, Constant.SettingParameter.FM_MAKSIMUM_DURASI_NARKOTIKA, Constant.SettingParameter.FM_KONTROL_DUPLIKASI_TERAPI));
            SettingParameter setParMaxDurasiNarkotika = lstSettingParameter.Where(t => t.ParameterCode == Constant.SettingParameter.FM_MAKSIMUM_DURASI_NARKOTIKA).FirstOrDefault();
            SettingParameter setParControlAdverseReaction = lstSettingParameter.Where(t => t.ParameterCode == Constant.SettingParameter.FM_KONTROL_ADVERSE_REACTION).FirstOrDefault();
            bool isControlTheraphyDuplication = lstSettingParameter.Where(t => t.ParameterCode == Constant.SettingParameter.FM_KONTROL_DUPLIKASI_TERAPI).FirstOrDefault().ParameterValue == "0" ? false : true;

            string prescriptionOrderId = hdnPrescriptionOrderID.Value;

            #region Duplicate Theraphy

            if (isControlTheraphyDuplication)
            {
                if (prescriptionOrderId != "0")
                {
                    string filterExp = string.Format("PrescriptionOrderID = {0} AND IsDeleted = 0", prescriptionOrderId);
                    List<vPrescriptionOrderDt> itemlist = BusinessLayer.GetvPrescriptionOrderDtList(filterExp);
                    foreach (var item in itemlist)
                    {
                        if (flag == "edit" && item.ItemID == Convert.ToInt32(hdnDrugID.Value)) continue;

                        //Generic Name
                        if ((item.GenericName.ToLower().TrimEnd() == txtGenericName.Text.ToLower().TrimEnd()) && !string.IsNullOrEmpty(item.GenericName))
                        {
                            errMessage = string.Format("Duplikasi obat dengan nama generik {0} yang sama ({1})", item.GenericName.TrimEnd(), item.ItemName1.TrimEnd());
                            return false;
                        }
                        vDrugInfo drugInfo = BusinessLayer.GetvDrugInfoList(string.Format("ItemCode = '{0}'", item.ItemCode)).FirstOrDefault();
                        if (drugInfo != null)
                        {
                            //ATC Class
                            if (drugInfo.ATCClassCode == entityItem.ATCClassCode && !string.IsNullOrEmpty(entityItem.ATCClassCode))
                            {
                                errMessage = string.Format("Duplikasi obat dengan Kelompok/Kelas ATC {0} yang sama ({1})", drugInfo.ATCClassName.TrimEnd(), item.ItemName1.TrimEnd());
                                return false;
                            }
                            //Kelompok Theraphy
                            if (drugInfo.MIMSClassCode.ToLower().TrimEnd() == entityItem.MIMSClassCode.ToLower().TrimEnd() && !string.IsNullOrEmpty(entityItem.MIMSClassCode))
                            {
                                errMessage = string.Format("Duplikasi obat dengan Kelompok Terapi {0} yang sama ({1})", drugInfo.MIMSClassName.TrimEnd(), item.ItemName1.TrimEnd());
                                return false;
                            }
                        }
                    }
                }
            }
            #endregion

            #region psikotropika & narkotika
            if (entityItem.GCDrugClass == Constant.DrugClass.MORPHIN || entityItem.GCDrugClass == Constant.DrugClass.NARKOTIKA || entityItem.GCDrugClass == Constant.DrugClass.PSIKOTROPIKA)
            {
                int frekuensiDay = 0;
                frekuensiDay = Convert.ToInt32(txtFrequencyNumber.Text);
                if (cboFrequencyTimeline.Value.ToString() == Constant.DosingFrequency.WEEK) frekuensiDay = frekuensiDay * 7;
                else if (cboFrequencyTimeline.Value.ToString() == Constant.DosingFrequency.HOUR) frekuensiDay = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(frekuensiDay) / 24.0));
                if (Convert.ToInt32(frekuensiDay) > Convert.ToInt32(setParMaxDurasiNarkotika.ParameterValue))
                {
                    errMessage = string.Format("Obat {0} Mengandung Narkotika, pemakaian tidak boleh lebih dari {1} hari", entityItem.ItemName1.TrimEnd(), setParMaxDurasiNarkotika.ParameterValue);
                    return false;
                }
            }
            #endregion

            if (setParControlAdverseReaction.ParameterValue == "0")
            {
                isAllow = "confirm";
            }

            #region Adverse Reaction
            prescriptionOrderId = hdnPrescriptionOrderID.Value;
            if (prescriptionOrderId != "0")
            {
                filterExpression = string.Format("ItemID = {0}", hdnDrugID.Value);
                List<DrugReaction> reactions = BusinessLayer.GetDrugReactionList(filterExpression);
                foreach (DrugReaction advReaction in reactions)
                {
                    string filterExp = string.Format("PrescriptionOrderID = {0} AND IsDeleted = 0", prescriptionOrderId);
                    List<vPrescriptionOrderDt> itemlist = BusinessLayer.GetvPrescriptionOrderDtList(filterExp);
                    foreach (var item in itemlist)
                    {
                        vDrugInfo drugInfo = BusinessLayer.GetvDrugInfoList(string.Format("ItemCode = '{0}'", item.ItemCode)).FirstOrDefault();
                        if (drugInfo != null)
                        {
                            //Generic Name
                            if (drugInfo.GenericName.ToLower().TrimEnd().Contains(advReaction.AdverseReactionText1.ToLower().TrimEnd())
                                || advReaction.AdverseReactionText1.ToLower().TrimEnd().Contains(drugInfo.GenericName.ToLower().TrimEnd()))
                            {
                                errMessage = string.Format("Terjadi interaksi obat dengan {0} ({1}) \n Catatan Interaksi Obat: \n {2}", item.ItemName1.TrimEnd(), drugInfo.GenericName, advReaction.AdverseReactionText2);
                                return false;
                            }
                        }
                    }
                }
            }
            #endregion


            return (errMessage == string.Empty);
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected bool IsEditable()
        {
            return _isEditable;
        }
    }
}
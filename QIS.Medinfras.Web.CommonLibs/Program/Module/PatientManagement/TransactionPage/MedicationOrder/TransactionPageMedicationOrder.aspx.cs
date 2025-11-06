using System;
using System.Collections.Generic;
using System.Globalization;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;
using Newtonsoft.Json;
using System.Reflection;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class TransactionPageMedicationOrder : BasePageTrx
    {
        vConsultVisit entityCV = null;
        string menuType = string.Empty;

        public override string OnGetMenuCode()
        {
            if (menuType == "md")
            {
                return Constant.MenuCode.MedicalDiagnostic.PATIENT_TRANSACTION_MEDICATION_ORDER_LIST;
            }
            else if (menuType == "er")
            {
                return Constant.MenuCode.EmergencyCare.PATIENT_TRANSACTION_MEDICATION_ORDER_LIST;
            }
            else if (menuType == "al")
            {
                hdnMenuType.Value = menuType;
                switch (hdnDepartmentID.Value)
                {
                    case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.PATIENT_TRANSACTION_MEDICATION_ORDER_ALKES;
                    case Constant.Facility.OUTPATIENT: return Constant.MenuCode.Outpatient.PATIENT_TRANSACTION_MEDICATION_ORDER_ALKES;
                    case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.PATIENT_TRANSACTION_MEDICATION_ORDER_ALKES;
                    default: return Constant.MenuCode.Inpatient.PATIENT_TRANSACTION_MEDICATION_ORDER_ALKES;
                }
            }
            else
            {
                switch (hdnDepartmentID.Value)
                {
                    case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.PATIENT_TRANSACTION_MEDICATION_ORDER;
                    case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.PATIENT_TRANSACTION_MEDICATION_ORDER;
                    case Constant.Facility.DIAGNOSTIC:
                        if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                            return Constant.MenuCode.Imaging.PATIENT_TRANSACTION_MEDICATION_ORDER;
                        if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                            return Constant.MenuCode.Laboratory.PATIENT_TRANSACTION_MEDICATION_ORDER;
                        return Constant.MenuCode.MedicalDiagnostic.PATIENT_TRANSACTION_MEDICATION_ORDER;
                    default: return Constant.MenuCode.Outpatient.PATIENT_TRANSACTION_MEDICATION_ORDER;
                }
            }
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = (hdnGCRegistrationStatus.Value != Constant.VisitStatus.CLOSED && !entityCV.IsLockDown);
            IsAllowSave = !entityCV.IsLockDown;
            IsAllowVoid = !entityCV.IsLockDown;
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString["id"] != null)
            {
                menuType = Page.Request.QueryString["id"];
            }
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            entityCV = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", hdnVisitID.Value))[0];
            hdnChargeClassID.Value = entityCV.ChargeClassID.ToString();
            hdnDepartmentID.Value = entityCV.DepartmentID;
            hdnGCRegistrationStatus.Value = entityCV.GCVisitStatus;
            hdnRegistrationID.Value = entityCV.RegistrationID.ToString();
            hdnClassID.Value = entityCV.ClassID.ToString();

            hdnDefaultVisitParamedicID.Value = entityCV.ParamedicID.ToString();
            hdnDefaultVisitParamedicCode.Value = entityCV.ParamedicCode;
            hdnDefaultVisitParamedicName.Value = entityCV.ParamedicName;
            
            //txtServiceCode.Attributes.Add("validationgroup", "mpTrxService");
            IsLoadFirstRecord = (OnGetRowCount() > 0);
            IsEditable = entityCV.IsLockDown ? false : true;

            string moduleName = Helper.GetModuleName();
            string ModuleID = Helper.GetModuleID(moduleName);
            GetUserMenuAccess menu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault();
            string CRUDMode = menu.CRUDMode;
            hdnIsAllowVoid.Value = CRUDMode.Contains("X") ? "1" : "0";

            BindGridView();
            List<vHealthcareServiceUnit> lstHealthcareServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.HealthcareServiceUnitID));
            hdnDefaultDispensaryServiceUnitID.Value = lstHealthcareServiceUnit.FirstOrDefault().DispensaryServiceUnitID.ToString();
            cboDispensaryUnit.Value = hdnDefaultDispensaryServiceUnitID.Value;

            SettingParameterDt setvarDt = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IM_TRANSAKSI_OBAT_HANYA_TIPE_DISTRIBUSI);
            hdnIsDrugChargesJustDistribution.Value = setvarDt.ParameterValue;
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

        protected override void SetControlProperties()
        {
            List<vHealthcareServiceUnit> lstHealthcareServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("IsDeleted = 0 AND IsUsingRegistration = 1"));
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboDispensaryUnit, lstHealthcareServiceUnit.Where(x => x.DepartmentID == "PHARMACY").ToList(), "ServiceUnitName", "HealthcareServiceUnitID");
            if (hdnDefaultDispensaryServiceUnitID.Value == "0") cboDispensaryUnit.SelectedIndex = 0;
            else
                cboDispensaryUnit.Value = hdnDefaultDispensaryServiceUnitID.Value;

            String filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}') AND IsDeleted = 0", Constant.StandardCode.DRUG_FORM, Constant.StandardCode.DOSING_FREQUENCY, Constant.StandardCode.MEDICATION_ROUTE, Constant.StandardCode.ITEM_UNIT, Constant.StandardCode.REFILL_INSTRUCTION, Constant.StandardCode.COENAM_RULE, Constant.StandardCode.PRESCRIPTION_TYPE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboForm, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DRUG_FORM).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboFrequencyTimeline, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DOSING_FREQUENCY).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDosingUnit, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ITEM_UNIT).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboMedicationRoute, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.MEDICATION_ROUTE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboCoenamRule, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.COENAM_RULE || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboPrescriptionType, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.PRESCRIPTION_TYPE).ToList(), "StandardCodeName", "StandardCodeID");

            if (txtPrescriptionOrderNo.Text == string.Empty)
                cboPrescriptionType.SelectedIndex = 0;

            cboForm.SelectedIndex = 0;
            cboFrequencyTimeline.SelectedIndex = 0;
            cboDosingUnit.SelectedIndex = 0;
            cboMedicationRoute.SelectedIndex = 0;
            cboCoenamRule.SelectedIndex = 0;

            txtStartDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtStartTime.Text = DateTime.Now.ToString("HH:mm");

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
                List<StandardCode> lst = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND StandardCodeID IN (SELECT GCDoseUnit FROM vDrugInfo WHERE ItemID = {1}) OR StandardCodeID IN (SELECT GCItemUnit FROM vDrugInfo WHERE ItemID = {1}) AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.ITEM_UNIT, hdnDrugID.Value));
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
            SetControlEntrySetting(cboPrescriptionType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false, string.Empty));

            SetControlEntrySetting(lblPhysician, new ControlEntrySetting(true, false));
        }

        protected bool IsEditable = true;
        public override void OnAddRecord()
        {
            hdnGCTransactionStatus.Value = Constant.TransactionStatus.OPEN;
            if (hdnDefaultDispensaryServiceUnitID.Value == "0") cboDispensaryUnit.SelectedIndex = 0;
            else
                cboDispensaryUnit.Value = hdnDefaultDispensaryServiceUnitID.Value;
            IsEditable = true;

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
            vPrescriptionOrderHd3 entity = BusinessLayer.GetvPrescriptionOrderHd3(filterExpression, PageIndex, " PrescriptionOrderID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = string.Format("VisitID = {0}", hdnVisitID.Value);
            PageIndex = BusinessLayer.GetvPrescriptionOrderHd3RowIndex(filterExpression, keyValue, "PrescriptionOrderID DESC");
            vPrescriptionOrderHd3 entity = BusinessLayer.GetvPrescriptionOrderHd3(filterExpression, PageIndex, "PrescriptionOrderID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vPrescriptionOrderHd3 entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                watermarkText = entity.TransactionStatusWatermark;
            }
            IsEditable = (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN);
            Registration entityRegistration = BusinessLayer.GetRegistration(Convert.ToInt32(hdnRegistrationID.Value));
            IsEditable = entityRegistration.IsLockDown ? false : IsEditable;
            hdnGCTransactionStatus.Value = entity.GCTransactionStatus;
            hdnPrescriptionOrderID.Value = entity.PrescriptionOrderID.ToString();
            txtPrescriptionOrderNo.Text = entity.PrescriptionOrderNo;
            txtPrescriptionDate.Text = entity.PrescriptionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPrescriptionTime.Text = entity.PrescriptionTime;
            hdnPhysicianID.Value = entity.ParamedicID.ToString();
            txtPhysicianCode.Text = entity.ParamedicCode;
            txtPhysicianName.Text = entity.ParamedicName;
            txtRemarks.Text = entity.Remarks;
            cboDispensaryUnit.Value = entity.DispensaryServiceUnitID.ToString();
            BindCboLocation();
            cboLocation.Value = entity.LocationID.ToString();
            cboPrescriptionType.Value = entity.GCPrescriptionType.ToString();

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

            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = "1 = 0";
            if (hdnPrescriptionOrderID.Value != "")
                filterExpression = string.Format("ParentID IS NULL AND PrescriptionOrderID = {0} AND OrderIsDeleted = 0 ORDER BY PrescriptionOrderDetailID DESC", hdnPrescriptionOrderID.Value);
            List<vPrescriptionOrderDt3> lstEntity = BusinessLayer.GetvPrescriptionOrderDt3List(filterExpression);
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
        #endregion

        #region Save Entity
        public void SavePrescriptionOrderHd(IDbContext ctx, ref int prescriptionOrderID)
        {
            PrescriptionOrderHdDao entityHdDao = new PrescriptionOrderHdDao(ctx);
            PrescriptionOrderDtDao entityDtDao = new PrescriptionOrderDtDao(ctx);
            PrescriptionOrderHd entityHd = null;
            if (hdnPrescriptionOrderID.Value == "0")
            {
                entityHd = new PrescriptionOrderHd();
                entityHd.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
                entityHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                entityHd.VisitHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                entityHd.PrescriptionDate = Helper.GetDatePickerValue(Request.Form[txtPrescriptionDate.UniqueID]);
                entityHd.PrescriptionTime = Request.Form[txtPrescriptionTime.UniqueID];
                entityHd.ClassID = Convert.ToInt32(hdnClassID.Value);
                entityHd.DispensaryServiceUnitID = Convert.ToInt32(cboDispensaryUnit.Value);
                entityHd.LocationID = Convert.ToInt32(cboLocation.Value);
                entityHd.GCPrescriptionType = cboPrescriptionType.Value.ToString();
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
                    entityHd.TransactionCode = Constant.TransactionCode.IP_MEDICATION_ORDER;
                else if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
                    entityHd.TransactionCode = Constant.TransactionCode.OP_MEDICATION_ORDER;
                else
                    entityHd.TransactionCode = Constant.TransactionCode.ER_MEDICATION_ORDER;
                entityHd.PrescriptionOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.PrescriptionDate, ctx);
                entityHd.Remarks = txtRemarks.Text;
                entityHd.GCOrderStatus = Constant.TestOrderStatus.OPEN;
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
                ctx.CommitTransaction();
                retval = PrescriptionOrderID.ToString();
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
            try
            {
                Int32 prescriptionOrderID = Convert.ToInt32(hdnPrescriptionOrderID.Value);
                PrescriptionOrderHd orderHd = BusinessLayer.GetPrescriptionOrderHd(prescriptionOrderID);
                if (orderHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    orderHd.GCPrescriptionType = cboPrescriptionType.Value.ToString();
                    orderHd.Remarks = txtRemarks.Text;
                    orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    orderHd.LastUpdatedDate = DateTime.Now;
                    BusinessLayer.UpdatePrescriptionOrderHd(orderHd);
                }
                else
                {
                    errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
                    result = false;
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
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
            PrescriptionOrderHdOriginalDao entityOrderHdOriginalDao = new PrescriptionOrderHdOriginalDao(ctx);
            PrescriptionOrderDtOriginalDao entityOrderDtOriginalDao = new PrescriptionOrderDtOriginalDao(ctx);

            try
            {
                Int32 PrescriptionOrderID = Convert.ToInt32(hdnPrescriptionOrderID.Value);
                PrescriptionOrderHd entity = entityHdDao.Get(PrescriptionOrderID);
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    List<PrescriptionOrderDt> lstDt = BusinessLayer.GetPrescriptionOrderDtList(string.Format("PrescriptionOrderID = {0}", PrescriptionOrderID), ctx);
                    foreach (PrescriptionOrderDt dt in lstDt)
                    {
                        PrescriptionOrderDt entityDt = entityDtDao.Get(dt.PrescriptionOrderDetailID);
                        entityDt.GCPrescriptionOrderStatus = Constant.OrderStatus.CANCELLED;
                        entityDt.GCVoidReason = Constant.DeleteReason.OTHER;
                        entityDt.VoidReason = "HEADER IS CANCELLED";
                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(entityDt);
                    }

                    entity.GCOrderStatus = Constant.TestOrderStatus.CANCELLED;
                    entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entity.VoidDate = DateTime.Now;
                    entity.VoidBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entity);

                    if (entity.IsOrderedByPhysician == true) {

                        string filterHdOri = string.Format("PrescriptionOrderID = {0} AND GCTransactionStatus <> '{1}'", entity.PrescriptionOrderID, Constant.TransactionStatus.VOID);
                        List<PrescriptionOrderHdOriginal> lstHdOri = BusinessLayer.GetPrescriptionOrderHdOriginalList(filterHdOri, ctx);
                        foreach (PrescriptionOrderHdOriginal hdOri in lstHdOri)
                        {
                            hdOri.GCTransactionStatus = Constant.TransactionStatus.VOID;
                            hdOri.GCOrderStatus = Constant.OrderStatus.CANCELLED;
                            hdOri.GCVoidReason = Constant.DeleteReason.OTHER;
                            hdOri.VoidReason = "HEADER IS CANCELLED";
                            hdOri.VoidBy = AppSession.UserLogin.UserID;
                            hdOri.VoidDate = DateTime.Now;
                            hdOri.LastUpdatedBy = AppSession.UserLogin.UserID;
                            hdOri.LastUpdatedDate = DateTime.Now;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityOrderHdOriginalDao.Update(hdOri);

                            string filterDtOri = string.Format("PrescriptionOrderID = {0} AND GCPrescriptionOrderStatus <> '{1}'", hdOri.PrescriptionOrderID, Constant.OrderStatus.CANCELLED);
                            List<PrescriptionOrderDtOriginal> lstDtOri = BusinessLayer.GetPrescriptionOrderDtOriginalList(filterDtOri, ctx);
                            foreach (PrescriptionOrderDtOriginal dtOri in lstDtOri)
                            {
                                dtOri.IsDeleted = true;
                                dtOri.GCPrescriptionOrderStatus = Constant.OrderStatus.CANCELLED;
                                dtOri.GCVoidReason = Constant.DeleteReason.OTHER;
                                dtOri.VoidReason = "HEADER IS CANCELLED";
                                dtOri.VoidBy = AppSession.UserLogin.UserID;
                                dtOri.VoidDateTime = DateTime.Now;
                                dtOri.LastUpdatedBy = AppSession.UserLogin.UserID;
                                dtOri.LastUpdatedDate = DateTime.Now;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                entityOrderDtOriginalDao.Update(dtOri);
                            }
                        }
                    }
                    #region Log PrescriptionTaskOrder
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    Helper.InsertPrescriptionOrderTaskLog(ctx, entity.PrescriptionOrderID, Constant.PrescriptionTaskLogStatus.Void, AppSession.UserLogin.UserID, false);
                    #endregion

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Order Resep " + entity.PrescriptionOrderNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
                PrescriptionOrderHdDao entityHdDao = new PrescriptionOrderHdDao(ctx);
                PrescriptionOrderDtDao entityDtDao = new PrescriptionOrderDtDao(ctx);
                try
                {
                    Int32 PrescriptionOrderID = Convert.ToInt32(hdnPrescriptionOrderID.Value);
                    PrescriptionOrderHd entity = entityHdDao.Get(PrescriptionOrderID);
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

                        List<PrescriptionOrderDt> lstDt = BusinessLayer.GetPrescriptionOrderDtList(string.Format("PrescriptionOrderID = {0}", PrescriptionOrderID));
                        foreach (PrescriptionOrderDt dt in lstDt)
                        {
                            PrescriptionOrderDt entityDt = entityDtDao.Get(dt.PrescriptionOrderDetailID);
                            entityDt.GCPrescriptionOrderStatus = Constant.OrderStatus.CANCELLED;
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
            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderHdDao prescriptionOrderHdDao = new PrescriptionOrderHdDao(ctx);
            PrescriptionOrderHdOriginalDao entityOrderHdOriginalDao = new PrescriptionOrderHdOriginalDao(ctx);
            PrescriptionOrderDtOriginalDao entityOrderDtOriginalDao = new PrescriptionOrderDtOriginalDao(ctx);
            PrescriptionOrderDtDao prescriptionOrderDtDao = new PrescriptionOrderDtDao(ctx);
            try
            {

                Int32 prescriptionOrderID = Convert.ToInt32(hdnPrescriptionOrderID.Value);
                Healthcare entityHSU = BusinessLayer.GetHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID)).FirstOrDefault();
                PrescriptionOrderHd entity = BusinessLayer.GetPrescriptionOrderHd(prescriptionOrderID);
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                   
                    entity.GCOrderStatus = Constant.TestOrderStatus.RECEIVED;
                    entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                    entity.ProposedBy = AppSession.UserLogin.UserID;
                    entity.ProposedDate = DateTime.Now;
                    entity.SendOrderDateTime = DateTime.Now;
                    entity.SendOrderBy = AppSession.UserLogin.UserID;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    //BusinessLayer.UpdatePrescriptionOrderHd(entity);
                    prescriptionOrderHdDao.Update(entity);

                     //Log : Copy of Current Prescription Order
                    int historyID  = 0;
                    if(entity.IsOrderedByPhysician){
                        #region Log Header
                        PrescriptionOrderHdOriginal originalHd = new PrescriptionOrderHdOriginal();
                        CopyHeaderObject(entity, ref originalHd);
                        historyID = entityOrderHdOriginalDao.InsertReturnPrimaryKeyID(originalHd);
                        #endregion
                    }
                    List<PrescriptionOrderDtOriginal> lstOriginalDt = new List<PrescriptionOrderDtOriginal>();

                    string filterDt = string.Format("PrescriptionOrderID = {0} AND IsDeleted = 0", entity.PrescriptionOrderID);
                    List<PrescriptionOrderDt> lstEntityDt = BusinessLayer.GetPrescriptionOrderDtList(filterDt);
                    foreach (PrescriptionOrderDt entityDt in lstEntityDt)
                    {
                        entityDt.GCPrescriptionOrderStatus = Constant.TestOrderStatus.RECEIVED;
                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        prescriptionOrderDtDao.Update(entityDt);

                        if(historyID > 0){
                            PrescriptionOrderDtOriginal originalDt = new PrescriptionOrderDtOriginal();
                            CopyDetailObject(entityDt, ref originalDt);
                            originalDt.HistoryHeaderID = historyID;
                            lstOriginalDt.Add(originalDt);
                        }

                           
                    }

                     #region Log Detail
                     if(lstOriginalDt.Count > 0 )
                     {
                         foreach (PrescriptionOrderDtOriginal originalDt in lstOriginalDt)
                         {
                             entityOrderDtOriginalDao.Insert(originalDt);
                         }
                     }
                     #endregion


                    #region Log PrescriptionTaskOrder
                    Helper.InsertPrescriptionOrderTaskLog(ctx, entity.PrescriptionOrderID, Constant.PrescriptionTaskLogStatus.Sent, AppSession.UserLogin.UserID, false);
                    #endregion

                    ctx.CommitTransaction();

                    //TODO Print Prescription Order
                    try
                    {
                        if (entityHSU.Initial == "BROS")
                        {
                            PrintOrderTracer(entity.PrescriptionOrderID);
                        }
                        else
                        {
                            PrintOrder(prescriptionOrderID);
                        }
                    }
                    catch (Exception)
                    {
                    }

                    return true;
                }
                else
                {
                    errMessage = "Order Resep " + entity.PrescriptionOrderNo + " tidak dapat diubah. Harap refresh halaman ini.";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    return false;
                }
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
            finally
            {
                ctx.Close();
            }
        }

        private void PrintOrder(int orderID)
        {

        }

        private string PrintOrderTracer(int prescriptionOrderID)
        {
            string result = string.Empty;
            Healthcare entityHSU = BusinessLayer.GetHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID)).FirstOrDefault();
            string ipAddress = HttpContext.Current.Request.UserHostAddress;
            string filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0", ipAddress, Constant.DirectPrintType.ORDER_FARMASI);
            List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);
            string printerUrl1 = "";
            if (lstPrinter.Count > 0)
            {
                printerUrl1 = lstPrinter.FirstOrDefault().PrinterName;
            }

            if (entityHSU.Initial == "BROS")
            {
                List<SettingParameterDt> lstOSetPar = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.PH0071, Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN));

                SettingParameterDt oSetPar = lstOSetPar.Where(t => t.ParameterCode == Constant.SettingParameter.PH0071).FirstOrDefault();
                string[] printUrl = oSetPar.ParameterValue.Split('|');
                string bpjsID = lstOSetPar.Where(t => t.ParameterCode == Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN).FirstOrDefault().ParameterValue;
                Customer bp = BusinessLayer.GetCustomer(AppSession.RegisteredPatient.BusinessPartnerID);
                printerUrl1 = printUrl[0];

                if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT)
                {
                    printerUrl1 = printUrl[2]; // Ranap
                }
                else if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.EMERGENCY)
                {
                    printerUrl1 = printUrl[1]; // Umum
                }
                else
                {
                    if (bp.GCCustomerType == Constant.CustomerType.BPJS)
                    {
                        printerUrl1 = printUrl[0]; // BPJS
                    }
                    else
                    {
                        printerUrl1 = printUrl[1];
                    }
                }
                ZebraPrinting.PrintOrderFarmasi(prescriptionOrderID, printerUrl1);
            }
            else
            {
                //if (entityHSU.Initial == "DEMO")
                //{
                //    ZebraPrinting.PrintOrderFarmasi(prescriptionOrderID, printerUrl1);
                //}
                ZebraPrinting.PrintOrderFarmasi(prescriptionOrderID, printerUrl1);
            }
            return result;
        }

        #endregion

        #region Process Detail
        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string isAllow = "notConfirm";
            int prescriptionOrderID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
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
            entity.IsRFlag = chkIsRx.Checked;
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
            entity.Frequency = Convert.ToInt16(txtFrequencyNumber.Text);
            entity.NumberOfDosage = Convert.ToDecimal(txtDosingDose.Text);
            entity.ConversionFactor = 1;
            if (hdnGCItemUnit.Value != cboDosingUnit.Value.ToString())
            {
                if (entity.Dose != null)
                {
                    entity.ConversionFactor = 1 / Convert.ToDecimal(entity.Dose);
                }
            }
            entity.GCDosingUnit = cboDosingUnit.Value.ToString();
            entity.GCRoute = cboMedicationRoute.Value.ToString();
            entity.StartDate = Helper.GetDatePickerValue(txtStartDate);
            //entity.StartTime = txtStartTime.Text;
            entity.MedicationPurpose = txtPurposeOfMedication.Text;
            entity.MedicationAdministration = txtMedicationAdministration.Text;
            entity.DosingDuration = Convert.ToDecimal(txtDosingDuration.Text);
            entity.DispenseQty = Convert.ToDecimal(txtDispenseQty.Text);
            //entity.TakenQty = Convert.ToDecimal(hdnTakenQty.Value) == 0 ? entity.DispenseQty : Convert.ToDecimal(hdnTakenQty.Value);
            entity.TakenQty = entity.DispenseQty;
            entity.ResultQty = entity.TakenQty;
            entity.ChargeQty = entity.TakenQty;
            entity.IsCreatedFromOrder = true;
            entity.GCPrescriptionOrderStatus = Constant.TestOrderStatus.OPEN;

            string[] medicationTime = Methods.GetMedicationSequenceTime(entity.Frequency).Split('|');
            entity.Sequence1Time = medicationTime[0];
            entity.Sequence2Time = medicationTime[1];
            entity.Sequence3Time = medicationTime[2];
            entity.Sequence4Time = medicationTime[3];
            entity.Sequence5Time = medicationTime[4];
            entity.Sequence6Time = medicationTime[5];
            if (medicationTime[0] != "-")
            {
                entity.StartTime = medicationTime[0];
            }
            else
            {
                entity.StartTime = "00:00";
                entity.Sequence1Time = "00:00";
            }
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
                    PrescriptionOrderHd entityHd = prescriptionOrderHdDao.Get(prescriptionOrderID);
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
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
                        errMessage = "Order Resep tidak dapat diubah. Harap refresh halaman ini.";
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
                PrescriptionOrderDtLogDao orderLogDao = new PrescriptionOrderDtLogDao(ctx);
                try
                {
                    if (prescriptionOrderHdDao.Get(Convert.ToInt32(hdnPrescriptionOrderID.Value)).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        PrescriptionOrderDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                        hdnSelectedItem.Value = JsonConvert.SerializeObject(entityDt);

                        if (!entityDt.IsDeleted)
                        {
                            ControlToEntity(entityDt);
                            entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDt.LastUpdatedDate = DateTime.Now;
                            entityDtDao.Update(entityDt);

                            //PrescriptionOrderDt oOldItem = JsonConvert.DeserializeObject<PrescriptionOrderDt>(hdnSelectedItem.Value);
                            PrescriptionOrderDtLog orderDtLog = new PrescriptionOrderDtLog();
                            orderDtLog.LogDate = DateTime.Now;
                            orderDtLog.PrescriptionOrderDetailID = entityDt.PrescriptionOrderDetailID;
                            orderDtLog.OldValues = hdnSelectedItem.Value;
                            orderDtLog.NewValues = JsonConvert.SerializeObject(entityDt);
                            orderDtLog.UserID = AppSession.UserLogin.UserID;
                            orderLogDao.Insert(orderDtLog);

                            ctx.CommitTransaction();
                        }
                    }
                    else
                    {
                        result = false;
                        errMessage = "Order Resep " + prescriptionOrderHdDao.Get(Convert.ToInt32(hdnPrescriptionOrderID.Value)).PrescriptionOrderNo + " tidak dapat diubah. Harap refresh halaman ini.";
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

        private bool OnDeleteEntityDt(ref string errMessage, int ID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderDtDao entityDtDao = new PrescriptionOrderDtDao(ctx);
            PrescriptionOrderHdDao prescriptionOrderHdDao = new PrescriptionOrderHdDao(ctx);
            try
            {
                if (prescriptionOrderHdDao.Get(Convert.ToInt32(hdnPrescriptionOrderID.Value)).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    PrescriptionOrderDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                    if (entityDt.IsCompound != true)
                    {
                        if (!entityDt.IsDeleted)
                        {
                            entityDt.GCPrescriptionOrderStatus = Constant.TestOrderStatus.CANCELLED;
                            entityDt.IsDeleted = true;
                            entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entityDt);
                            ctx.CommitTransaction();
                        }
                    }
                    else
                    {
                        List<PrescriptionOrderDt> lstChild = BusinessLayer.GetPrescriptionOrderDtList(string.Format(" ParentID = {0} ", hdnEntryID.Value));
                        if (!entityDt.IsDeleted)
                        {
                            entityDt.GCPrescriptionOrderStatus = Constant.TestOrderStatus.CANCELLED;
                            entityDt.IsDeleted = true;
                            entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entityDt);
                        }
                        foreach (PrescriptionOrderDt dt in lstChild)
                        {
                            if (!dt.IsDeleted)
                            {
                                dt.GCPrescriptionOrderStatus = Constant.TestOrderStatus.CANCELLED;
                                dt.IsDeleted = true;
                                dt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Update(dt);
                            }
                        }
                        ctx.CommitTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Order Resep " + prescriptionOrderHdDao.Get(Convert.ToInt32(hdnPrescriptionOrderID.Value)).PrescriptionOrderNo + " tidak dapat diubah. Harap refresh halaman ini.";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                result = false;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
        #endregion

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
                    filterExpression = string.Format("MRN = {0} AND GCAllergenType = '{1}' AND Allergen LIKE '%{2}%' AND IsDeleted = 0", AppSession.RegisteredPatient.MRN, Constant.AllergenType.DRUG, item.Keyword);
                    lstPatientAllergy = BusinessLayer.GetPatientAllergyList(filterExpression);
                    if (lstPatientAllergy.Count > 0)
                    {
                        errMessage = string.Format("Pasien memiliki alergi {0} ({1})", item.ContentText, txtDrugName.Text).Replace("()", "");
                        return false;
                    }
                }
            }
            #endregion

            #region Duplicate Theraphy
            string prescriptionOrderId = hdnPrescriptionOrderID.Value;
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
                    //vDrugInfo drugInfo = BusinessLayer.GetvDrugInfoList(string.Format("ItemCode = '{0}'", item.ItemCode)).FirstOrDefault();
                    //if (drugInfo != null)
                    //{
                        //ATC Class
                        //di comment berkaitan dengan issue RSMD Jika ada order resep farmasi dengan kelompok/Kelas ATC yang sama di satu order tetap bisa dilakukan
                        //if (drugInfo.ATCClassCode == entityItem.ATCClassCode && !string.IsNullOrEmpty(entityItem.ATCClassCode))
                        //{
                        //    errMessage = string.Format("Duplikasi obat dengan Kelompok/Kelas ATC {0} yang sama ({1})", drugInfo.ATCClassName.TrimEnd(), item.ItemName1.TrimEnd());
                        //    return false;
                        //}
                        //Kelompok Theraphy
                        //di comment berkaitan dengan issue RSMD Jika ada order resep farmasi dengan kelompok terapi yang sama di satu order tetap bisa dilakukan
                        //if (drugInfo.MIMSClassCode.ToLower().TrimEnd() == entityItem.MIMSClassCode.ToLower().TrimEnd() && !string.IsNullOrEmpty(entityItem.MIMSClassCode))
                        //{
                        //    errMessage = string.Format("Duplikasi obat dengan Kelompok Terapi {0} yang sama ({1})", drugInfo.MIMSClassName.TrimEnd(), item.ItemName1.TrimEnd());
                        //    return false;
                        //}
                    //}
                }
            }
            #endregion

            List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.FM_KONTROL_ADVERSE_REACTION, Constant.SettingParameter.FM_MAKSIMUM_DURASI_NARKOTIKA));
            SettingParameter setParMaxDurasiNarkotika = lstSettingParameter.Where(t => t.ParameterCode == Constant.SettingParameter.FM_MAKSIMUM_DURASI_NARKOTIKA).FirstOrDefault();
            SettingParameter setParControlAdverseReaction = lstSettingParameter.Where(t => t.ParameterCode == Constant.SettingParameter.FM_KONTROL_ADVERSE_REACTION).FirstOrDefault();

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

        private void CopyHeaderObject(PrescriptionOrderHd source, ref PrescriptionOrderHdOriginal destination)
        {
            var fields = source.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                var value = field.GetValue(source);
                destination.GetType().GetProperty(field.Name.Replace("_", "")).SetValue(destination, value, null);
            }
        }

        private void CopyDetailObject(PrescriptionOrderDt source, ref PrescriptionOrderDtOriginal destination)
        {
            var fields = source.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                var value = field.GetValue(source);
                destination.GetType().GetProperty(field.Name.Replace("_", "")).SetValue(destination, value, null);
            }
        }


    }
}
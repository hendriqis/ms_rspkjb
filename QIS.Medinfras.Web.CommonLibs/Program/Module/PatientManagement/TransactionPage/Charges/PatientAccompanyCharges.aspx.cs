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
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientAccompanyCharges : BasePageTrxPatientManagement
    {
        vConsultVisit entityCV = null;

        public override string OnGetMenuCode()
        {
            string id = Page.Request.QueryString["id"];
            string menuCode = Constant.MenuCode.Inpatient.PATIENT_TRANSACTION_PAGE_PATIENT_ACCOMPANY;
            if (hdnPageTitle.Value == string.Empty)
                hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", menuCode)).FirstOrDefault().MenuCaption;
            return menuCode;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = (hdnGCRegistrationStatus.Value != Constant.VisitStatus.CLOSED && !entityCV.IsLockDown);
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
            return false;
        }

        public override bool GetIsAIOTransactionHd()
        {
            return false;
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
            return GetLabel("Ruang Perawatan");
        }

        protected string GetServiceUnitFilterFilterExpression()
        {
            ConsultVisit entity = BusinessLayer.GetConsultVisitList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            
            List<PatientAccompany> lstAccompany = BusinessLayer.GetPatientAccompanyList(string.Format("RegistrationID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.RegistrationID));
            string hsuAccID = "0";

            foreach (PatientAccompany pa in lstAccompany)
            {
                hsuAccID += pa.HealthcareServiceUnitID + ",";
            }

            if (hsuAccID != "0")
            {
                hsuAccID = hsuAccID.Substring(0, hsuAccID.Length - 1);
            }

            string filter = string.Format(
                        "HealthcareID = '{0}' AND IsDeleted = 0 AND IsUsingRegistration = 1 AND DepartmentID = '{1}' AND HealthcareServiceUnitID IN ({2},{3})",
                                    AppSession.UserLogin.HealthcareID, Constant.Facility.INPATIENT, hsuAccID, entity.HealthcareServiceUnitID);

            return filter;

            //if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
            //{
            //    List<PatientTransfer> entityPT = BusinessLayer.GetPatientTransferList(string.Format("RegistrationID = {0}", entity.RegistrationID));
            //    if (entityPT.Count > 0)
            //    {
            //        return string.Format("HealthcareID = '{0}' AND HealthcareServiceUnitID IN ({1}, {2}) AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, entityPT[entityPT.Count - 1].FromHealthcareServiceUnitID, entityPT[entityPT.Count - 1].ToHealthcareServiceUnitID);
            //    }
            //    else
            //    {
            //        return string.Format("HealthcareID = '{0}' AND HealthcareServiceUnitID = {1} AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, entity.HealthcareServiceUnitID);
            //    }
            //}
            //return string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND HealthcareServiceUnitID NOT IN ({2},{3}) AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, Constant.Facility.DIAGNOSTIC, hdnHSUImagingID.Value, hdnHSULaboratoryID.Value);
        }

        protected override void InitializeDataControl()
        {
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            entityCV = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID))[0];
            hdnMRN.Value = Convert.ToString(entityCV.MRN);
            hdnGCRegistrationStatus.Value = entityCV.GCVisitStatus;
            hdnRegistrationID.Value = entityCV.RegistrationID.ToString();
            hdnRegistrationPhysicianID.Value = hdnPhysicianID.Value = entityCV.ParamedicID.ToString();
            hdnPhysicianCode.Value = entityCV.ParamedicCode;
            hdnPhysicianName.Value = entityCV.ParamedicName;
            List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID, Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID));
            string laboratoryID = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).ParameterValue;
            string imagingID = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID).ParameterValue;
            List<vHealthcareServiceUnit> lstHealthcareServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND ServiceUnitID IN ({1},{2}) AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, laboratoryID, imagingID));
            hdnHSUImagingID.Value = lstHealthcareServiceUnit.FirstOrDefault(p => p.ServiceUnitID == Convert.ToInt32(imagingID)).HealthcareServiceUnitID.ToString();
            hdnHSULaboratoryID.Value = lstHealthcareServiceUnit.FirstOrDefault(p => p.ServiceUnitID == Convert.ToInt32(laboratoryID)).HealthcareServiceUnitID.ToString();
            hdnBusinessPartnerID.Value = entityCV.BusinessPartnerID.ToString();
            hdnGCCustomerType.Value = entityCV.GCCustomerType;
            hdnClassID.Value = entityCV.ChargeClassID.ToString();
            hdnVisitDepartmentID.Value = hdnDepartmentID.Value = entityCV.DepartmentID;
            hdnDefaultLocationID.Value = hdnLocationID.Value = entityCV.LocationID.ToString();
            hdnDefaultLogisticLocationID.Value = hdnLogisticLocationID.Value = entityCV.LogisticLocationID.ToString();
            hdnDefaultHealthcareServiceUnitID.Value = hdnHealthcareServiceUnitID.Value = entityCV.HealthcareServiceUnitID.ToString();
            if (hdnHealthcareServiceUnitID.Value == hdnHSUImagingID.Value || hdnHealthcareServiceUnitID.Value == hdnHSULaboratoryID.Value)
            {
                btnGoBillSummary.Style.Remove("display");
            }
            if (OnGetRowCount() > 0)
                IsLoadFirstRecord = true;
            else
                IsLoadFirstRecord = false;

            if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
                trServiceUnit.Attributes.Remove("style");
            
            string moduleName = Helper.GetModuleName();
            string ModuleID = Helper.GetModuleID(moduleName);
            GetUserMenuAccess menu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault();
            string CRUDMode = menu.CRUDMode;
            hdnIsAllowVoid.Value = CRUDMode.Contains("X") ? "1" : "0";

            SettingParameterDt setvarDt = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IS_AUTOMATICALLY_SEND_TO_BRIDGING);
            hdnIsAutomaticallySendToRIS.Value = setvarDt.ParameterValue;

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

            ((PatientManagementTransactionDetailServiceCtl)ctlService).OnAddRecord();
            ((PatientManagementTransactionDetailDrugMSCtl)ctlDrugMS).OnAddRecord();
            ((PatientManagementTransactionDetailDrugMSReturnCtl)ctlDrugMSReturn).OnAddRecord();
            ((PatientManagementTransactionDetailLogisticCtl)ctlLogistic).OnAddRecord();
            ((PatientManagementTransactionDetailLogisticReturnCtl)ctlLogisticReturn).OnAddRecord();

            divCreatedBy.InnerHtml = string.Empty;
            divCreatedDate.InnerHtml = string.Empty;
            divLastUpdatedBy.InnerHtml = string.Empty;
            divLastUpdatedDate.InnerHtml = string.Empty;
            divVoidReason.InnerHtml = string.Empty;
            trVoidReason.Style.Add("display", "none");
        }

        #region Load Entity
        protected string GetFilterExpression()
        {
            List<PatientAccompany> lstAccompany = BusinessLayer.GetPatientAccompanyList(string.Format("RegistrationID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.RegistrationID));
            string hsuAccID = "";

            foreach (PatientAccompany pa in lstAccompany)
            {
                hsuAccID += pa.HealthcareServiceUnitID + ",";
            }

            string filterExpression = string.Format(
                    "VisitID = {0} AND HealthcareServiceUnitID IN ({1}) AND TransactionCode = '{2}'",
                    hdnVisitID.Value, hdnHealthcareServiceUnitID.Value, Constant.TransactionCode.IP_PATIENT_ACCOMPANY_CHARGES);

            if (hsuAccID != "")
            {
                hsuAccID = hsuAccID.Substring(0, hsuAccID.Length - 1);
                filterExpression = string.Format(
                    "VisitID = {0} AND HealthcareServiceUnitID IN ({1},{2}) AND TransactionCode = '{3}'",
                    hdnVisitID.Value, hdnHealthcareServiceUnitID.Value, hsuAccID, Constant.TransactionCode.IP_PATIENT_ACCOMPANY_CHARGES);
            }


            //if (Page.Request.QueryString["id"] == "md")
            //    filterExpression = string.Format("VisitID = {0} AND DepartmentID = '{1}' AND HealthcareServiceUnitID NOT IN ({2},{3})", hdnVisitID.Value, Constant.Facility.DIAGNOSTIC, hdnHSUImagingID.Value, hdnHSULaboratoryID.Value);
            //else if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
            //    filterExpression += string.Format("VisitID = {0} AND DepartmentID = '{1}' AND TransactionCode = '{2}'", hdnVisitID.Value, Constant.Facility.INPATIENT,Constant.TransactionCode.IP_PATIENT_ACCOMPANY_CHARGES);
            //else
            //    filterExpression = string.Format("VisitID = {0} AND HealthcareServiceUnitID = {1}", hdnVisitID.Value, hdnHealthcareServiceUnitID.Value);

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
            ((PatientManagementTransactionDetailServiceCtl)ctlService).InitializeTransactionControl(flagHaveCharges);
            ((PatientManagementTransactionDetailDrugMSCtl)ctlDrugMS).InitializeTransactionControl(flagHaveCharges);
            ((PatientManagementTransactionDetailDrugMSReturnCtl)ctlDrugMSReturn).InitializeTransactionControl(flagHaveCharges);
            ((PatientManagementTransactionDetailLogisticCtl)ctlLogistic).InitializeTransactionControl(flagHaveCharges);
            ((PatientManagementTransactionDetailLogisticReturnCtl)ctlLogisticReturn).InitializeTransactionControl(flagHaveCharges);

            divCreatedBy.InnerHtml = entity.CreatedByName;
            divCreatedDate.InnerHtml = entity.CreatedDate.ToString(Constant.FormatString.DATE_TIME_FORMAT);
            divLastUpdatedBy.InnerHtml = entity.LastUpdatedByName;
            if (entity.LastUpdatedDate != null && entity.LastUpdatedDate.ToString(Constant.FormatString.DATE_TIME_FORMAT) != "01 January 1900 00:00:00")
            {
                divLastUpdatedDate.InnerHtml = entity.LastUpdatedDate.ToString(Constant.FormatString.DATE_TIME_FORMAT);
            }
            else
            {
                divLastUpdatedDate.InnerHtml = string.Empty;
            }

            if (entity.GCTransactionStatus == Constant.TransactionStatus.VOID)
            {
                string voidReason = "";

                if (entity.GCVoidReason == Constant.DeleteReason.OTHER)
                {
                    voidReason = entity.VoidReasonWatermark + " ( " + entity.VoidReason + " )";
                }
                else
                {
                    voidReason = entity.VoidReasonWatermark;
                }

                divVoidReason.InnerHtml = voidReason;
                trVoidReason.Style.Remove("display");
            }
            else
            {
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
                entityHd.TransactionCode = Constant.TransactionCode.IP_PATIENT_ACCOMPANY_CHARGES;
                entityHd.TransactionDate = Helper.GetDatePickerValue(Request.Form[txtTransactionDate.UniqueID]);
                entityHd.TransactionTime = Request.Form[txtTransactionTime.UniqueID];
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
                entityHd.TransactionCode = Constant.TransactionCode.IP_PATIENT_ACCOMPANY_CHARGES;
                entityHd.TransactionDate = Helper.GetDatePickerValue(Request.Form[txtTransactionDate.UniqueID]);
                entityHd.TransactionTime = Request.Form[txtTransactionTime.UniqueID];
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

        #region Void Entity
        protected override bool OnVoidRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            try
            {
                Int32 TransactionID = Convert.ToInt32(hdnTransactionHdID.Value);
                PatientChargesHd entity = entityHdDao.Get(TransactionID);
                if (entity.PatientBillingID == null && (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN || entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL))
                {
                    entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    entity.VoidBy = AppSession.UserLogin.UserID;
                    entity.VoidDate = DateTime.Now;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entity);

                    ((PatientManagementTransactionDetailServiceCtl)ctlService).OnVoidAllChargesDt(ctx, TransactionID);
                    ((PatientManagementTransactionDetailDrugMSCtl)ctlDrugMS).OnVoidAllChargesDt(ctx, TransactionID);
                    //((PatientManagementTransactionDetailDrugMSReturnCtl)ctlDrugMSReturn).OnVoidAllChargesDt(ctx, TransactionID);
                    ((PatientManagementTransactionDetailLogisticCtl)ctlLogistic).OnVoidAllChargesDt(ctx, TransactionID);
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

        #region Proposed Entity
        protected override bool OnProposeRecord(ref string errMessage)
        {
            bool result = true;
            Int32 TransactionID = Convert.ToInt32(hdnTransactionHdID.Value);
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            TestOrderHdDao orderHdDao = new TestOrderHdDao(ctx);
            TestOrderDtDao orderDtDao = new TestOrderDtDao(ctx);

            try
            {
                PatientChargesHd entity = entityHdDao.Get(TransactionID);
                if (entity.PatientBillingID == null && entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {

                    bool isProcessCharges = true;
                    string referenceNo = string.Empty;
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                    {
                        if (AppSession.IsBridgingToRIS)
                        {
                            if (hdnIsAutomaticallySendToRIS.Value == "1")
                            {
                                var processResult = SendOrderToRIS(0, Convert.ToInt32(hdnTransactionHdID.Value));

                                string[] resultInfo = ((string)processResult).Split('|');
                                isProcessCharges = resultInfo[0] == "1";
                                referenceNo = resultInfo[1];
                                if (!isProcessCharges)
                                    errMessage = resultInfo[1];
                            }
                        }
                    }

                    if (isProcessCharges)
                    {
                        Int32 transactionID = Convert.ToInt32(hdnTransactionHdID.Value);

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

                        int oTestOrderID;
                        if ((entity.TransactionCode == Constant.TransactionCode.LABORATORY_CHARGES || entity.TransactionCode == Constant.TransactionCode.IMAGING_CHARGES || entity.TransactionCode == Constant.TransactionCode.OTHER_DIAGNOSTIC_CHARGES) && entity.TestOrderID == null)
                        {
                            TestOrderHd orderHd = new TestOrderHd();
                            orderHd.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
                            if (!string.IsNullOrEmpty(AppSession.HealthcareServiceUnitID))
                            {
                                orderHd.FromHealthcareServiceUnitID = Convert.ToInt32(AppSession.HealthcareServiceUnitID);
                            }
                            else
                            {
                                orderHd.FromHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                            }
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
                            entity.ReferenceNo = referenceNo;
                        entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                        entity.ProposedBy = AppSession.UserLogin.UserID;
                        entity.ProposedDate = DateTime.Now;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityHdDao.Update(entity);

                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    errMessage = "Transaksi " + entity.TransactionNo + " Sudah Diproses. Tidak Bisa Diubah";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
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
                //ChargesStatusLogDao logDao = new ChargesStatusLogDao(ctx);
                try
                {
                    //ChargesStatusLog log = new ChargesStatusLog();
                    //string statusOld = "", statusNew = "";
                    Int32 TransactionID = Convert.ToInt32(hdnTransactionHdID.Value);
                    PatientChargesHd entity = entityHdDao.Get(TransactionID);
                    //statusOld = entity.GCTransactionStatus;
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
                        //statusNew = entity.GCTransactionStatus;

                        //log.VisitID = entity.VisitID;
                        //log.TransactionID = entity.TransactionID;
                        //log.LogDate = DateTime.Now;
                        //log.UserID = AppSession.UserLogin.UserID;
                        //log.GCTransactionStatusOLD = statusOld;
                        //log.GCTransactionStatusNEW = statusNew;
                        //logDao.Insert(log);
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
                result = string.Format("{0}|{1}", "0", string.Format("{0} ({1})", ex.Status.ToString()));
                return result;
            }
        }
        #endregion
    }
}
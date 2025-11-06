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
    public partial class PatientBillSummaryLab : BasePageTrxPatientManagement
    {
        vConsultVisit2 entityCV = null;

        public override string OnGetMenuCode()
        {
            if (hdnVisitDepartmentID.Value == Constant.Facility.PHARMACY)
                return Constant.MenuCode.Pharmacy.BILL_SUMMARY_SERVICE;
            switch (hdnVisitDepartmentID.Value)
            {
                case Constant.Facility.MEDICAL_CHECKUP: return Constant.MenuCode.MedicalCheckup.BILL_SUMMARY_LABORATORY;
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.BILL_SUMMARY_LABORATORY;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.BILL_SUMMARY_LABORATORY;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.BILL_SUMMARY_LABORATORY;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.BILL_SUMMARY_CHARGES;
                    return Constant.MenuCode.MedicalDiagnostic.BILL_SUMMARY_LABORATORY;
                default: return Constant.MenuCode.Outpatient.BILL_SUMMARY_LABORATORY;
            }
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
            return false;
        }

        public override bool GetIsAIOTransactionHd()
        {
            return false;
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
            entityCV = BusinessLayer.GetvConsultVisit2List(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID))[0];

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

            if (entityCV.DepartmentID == Constant.Facility.PHARMACY)
            {
                hdnHealthcareServiceUnitID.Value = entityCV.HealthcareServiceUnitID.ToString();
            }
            else
            {
                SettingParameter settingParameter = BusinessLayer.GetSettingParameter(Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID);
                string labServiceUnitID = settingParameter.ParameterValue;

                List<vHealthcareServiceUnit> lstHealthcareServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = {0} AND IsLaboratoryUnit = 1", AppSession.UserLogin.HealthcareID));
                hdnHealthcareServiceUnitID.Value = lstHealthcareServiceUnit.FirstOrDefault(p => p.ServiceUnitID == Convert.ToInt32(labServiceUnitID)).HealthcareServiceUnitID.ToString();
            }

            hdnVisitDepartmentID.Value = entityCV.DepartmentID;
            hdnLinkedRegistrationID.Value = entityCV.LinkedRegistrationID.ToString();
            hdnDepartmentID.Value = Constant.Facility.DIAGNOSTIC;
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
            //txtServiceCode.Attributes.Add("validationgroup", "mpTrxService");

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
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnTransactionHdID, new ControlEntrySetting(false, false, false, "0"));

            SetControlEntrySetting(txtTransactionNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtTransactionDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtTransactionTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(txtReferenceNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
        }

        public override void OnAddRecord()
        {
            hdnGCTransactionStatus.Value = Constant.TransactionStatus.OPEN;

            imgIsAIOTransaction.Style.Add("display", "none");

            ((PatientManagementTransactionDetailServiceCtl)ctlService).OnAddRecord();
            ((PatientManagementTransactionDetailDrugMSCtl)ctlDrugMS).OnAddRecord();
            ((PatientManagementTransactionDetailDrugMSReturnCtl)ctlDrugMSReturn).OnAddRecord();
            ((PatientManagementTransactionDetailLogisticCtl)ctlLogistic).OnAddRecord();
        }

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

        #region Load Entity
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
            if (AppSession.IsAdminCanCancelAllTransaction)
            {
                if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN && entity.GCTransactionStatus != Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                {
                    isShowWatermark = true;
                    watermarkText = entity.TransactionStatusWatermark;
                }
                else
                {
                    isShowWatermark = false;
                }
            }
            else
            {
                if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
                {
                    isShowWatermark = true;
                    watermarkText = entity.TransactionStatusWatermark;
                }
                else
                {
                    isShowWatermark = false;
                }
            }
            hdnGCTransactionStatus.Value = entity.GCTransactionStatus;
            hdnTransactionHdID.Value = entity.TransactionID.ToString();
            txtTransactionNo.Text = entity.TransactionNo;
            txtReferenceNo.Text = entity.ReferenceNo;
            txtRemarks.Text = entity.Remarks;
            txtTransactionDate.Text = entity.TransactionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTransactionTime.Text = entity.TransactionTime;
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
                entityHd.TransactionCode = Constant.TransactionCode.LABORATORY_CHARGES;
                
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
                entityHd.TransactionCode = Constant.TransactionCode.LABORATORY_CHARGES;
                
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
                        errMessage = "Transaksi Sudah Diproses. Tidak Bisa Diubah";
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
                        errMessage = "Transaksi Sudah Diproses. Tidak Bisa Diubah";
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

        #region Proposed Entity
        protected override bool OnProposeRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            //ChargesStatusLogDao logDao = new ChargesStatusLogDao(ctx);
            try
            {
                //ChargesStatusLog log = new ChargesStatusLog();
                //string statusOld = "", statusNew = "";

                Int32 TransactionID = Convert.ToInt32(hdnTransactionHdID.Value);
                PatientChargesHd entity = BusinessLayer.GetPatientChargesHd(TransactionID);
                //statusOld = entity.GCTransactionStatus;
                if (AppSession.IsAdminCanCancelAllTransaction)
                {
                    if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN || entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                    {
                        entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                        entity.ProposedBy = AppSession.UserLogin.UserID;
                        entity.ProposedDate = DateTime.Now;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityHdDao.Update(entity);

                        //statusNew = entity.GCTransactionStatus;

                        //log.VisitID = entity.VisitID;
                        //log.TransactionID = entity.TransactionID;
                        //log.LogDate = DateTime.Now;
                        //log.UserID = AppSession.UserLogin.UserID;
                        //log.GCTransactionStatusOLD = statusOld;
                        //log.GCTransactionStatusNEW = statusNew;
                        //logDao.Insert(log); 

                        result = true;
                    }
                    else
                    {
                        errMessage = "Transaksi Sudah Diproses. Tidak Bisa Dipropose";
                        result = false;
                    }
                }
                else
                {
                    if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                        entity.ProposedBy = AppSession.UserLogin.UserID;
                        entity.ProposedDate = DateTime.Now;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityHdDao.Update(entity);
                        //statusNew = entity.GCTransactionStatus;
                        //log.VisitID = entity.VisitID;
                        //log.TransactionID = entity.TransactionID;
                        //log.LogDate = DateTime.Now;
                        //log.UserID = AppSession.UserLogin.UserID;
                        //log.GCTransactionStatusOLD = statusOld;
                        //log.GCTransactionStatusNEW = statusNew;
                        //logDao.Insert(log); 
                        result = true;
                    }
                    else
                    {
                        errMessage = "Transaksi Sudah Diproses. Tidak Bisa Dipropose";
                        result = false;
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
        #endregion

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }
    }
}
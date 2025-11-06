using System;
using System.Collections.Generic;
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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientBillSummaryPharmacy : BasePageTrxPatientManagement
    {
        protected bool IsShowSwitchIcon = false;

        public override string OnGetMenuCode()
        {
            string id = Page.Request.QueryString["id"];
            switch (hdnVisitDepartmentID.Value)
            {
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.BILL_SUMMARY_PHARMACY;
                case Constant.Facility.MEDICAL_CHECKUP: return Constant.MenuCode.MedicalCheckup.BILL_SUMMARY_PHARMACY;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.BILL_SUMMARY_PHARMACY;
                case Constant.Facility.PHARMACY: return Constant.MenuCode.Pharmacy.BILL_SUMMARY_PHARMACY;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.BILL_SUMMARY_PHARMACY;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.BILL_SUMMARY_PHARMACY;
                    return Constant.MenuCode.MedicalDiagnostic.BILL_SUMMARY_PHARMACY;
                default:
                    return Constant.MenuCode.Outpatient.BILL_SUMMARY_PHARMACY;
            }
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = false;
            IsAllowSave = false;
            //IsAllowAdd = (hdnGCRegistrationStatus.Value != Constant.RegistrationStatus.CLOSED);
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
            return "";
        }

        public override string GetReferenceNoHd()
        {
            return "";
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
            if (hdnHealthcareServiceUnitID.Value == "")
                return 0;
            return Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
        }
        public override string GetTransactionHdID()
        {
            return hdnPrescriptionOrderID.Value;
        }
        public override void SetTransactionHdID(string value)
        {
            hdnPrescriptionOrderID.Value = value;
        }
        public override string GetDepartmentID()
        {
            return hdnDepartmentID.Value;
        }
        public override string GetTransactionDate()
        {
            return Request.Form[txtPrescriptionDate.UniqueID];
        }
        public override string GetTransactionTime()
        {
            return Request.Form[txtPrescriptionTime.UniqueID];
        }
        public override string GetTransactionDate2()
        {
            return txtPrescriptionDate.Text;
        }
        public override string GetTransactionTime2()
        {
            return txtPrescriptionTime.Text;
        }

        protected override void InitializeDataControl()
        {
            hdnIsAdminCanCancelAllTransaction.Value = AppSession.IsAdminCanCancelAllTransaction ? "1" : "0";
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();

            vConsultVisit2 entity = BusinessLayer.GetvConsultVisit2List(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();

            IsShowSwitchIcon = entity.GCCustomerType != Constant.CustomerType.PERSONAL;

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
            hdnVisitID.Value = entity.VisitID.ToString();
            hdnVisitDepartmentID.Value = hdnDepartmentID.Value = entity.DepartmentID;
            hdnLinkedRegistrationID.Value = entity.LinkedRegistrationID.ToString();
            List<ClassCare> lstClassCare = BusinessLayer.GetClassCareList("IsDeleted = 0 AND IsUsedInChargeClass = 1");
            Methods.SetComboBoxField(cboChargeClass, lstClassCare, "ClassName", "ClassID");
            IsLoadFirstRecord = (OnGetRowCount() > 0);
            string moduleName = Helper.GetModuleName();
            string ModuleID = Helper.GetModuleID(moduleName);
            GetUserMenuAccess menu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault();
            string CRUDMode = menu.CRUDMode;
            hdnIsAllowVoid.Value = CRUDMode.Contains("X") ? "1" : "0";

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }

        protected override void SetControlProperties()
        {
            //((PatientManagementTransactionDetailServiceCtl)ctlService).SetControlProperties();
            //((PatientManagementTransactionDetailDrugMSCtl)ctlDrugMS).SetControlProperties();
            //((PatientManagementTransactionDetailLogisticCtl)ctlLogistic).SetControlProperties();
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnTransactionID, new ControlEntrySetting(false, false, false, ""));
            SetControlEntrySetting(hdnPrescriptionOrderID, new ControlEntrySetting(false, false, false, ""));
            SetControlEntrySetting(txtPrescriptionOrderNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtPrescriptionDate, new ControlEntrySetting(false, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtPrescriptionTime, new ControlEntrySetting(false, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(txtDispenseryUnitName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(hdnHealthcareServiceUnitID, new ControlEntrySetting(false, false, false, hdnDefaultHealthcareServiceUnitID.Value));
            SetControlEntrySetting(hdnLocationID, new ControlEntrySetting(false, false, false, hdnDefaultLocationID.Value));
        }

        #region Load Entity
        public override void OnAddRecord()
        {
            //hdnTransactionStatus.Value = "";
            BindGridView();

            imgIsAIOTransaction.Style.Add("display", "none");

            divCreatedBy.InnerHtml = string.Empty;
            divCreatedDate.InnerHtml = string.Empty;
            divProposedBy.InnerHtml = string.Empty;
            divProposedDate.InnerHtml = string.Empty;
            trProposedBy.Style.Add("display", "none");
            trProposedDate.Style.Add("display", "none");
            divLastUpdatedBy.InnerHtml = string.Empty;
            divLastUpdatedDate.InnerHtml = string.Empty;
            divVoidReason.InnerHtml = string.Empty;
            trVoidReason.Style.Add("display", "none");
        }

        protected string GetFilterExpression()
        {
            String filterExpression = "";
            if (hdnLinkedRegistrationID.Value != "" && hdnLinkedRegistrationID.Value != "0")
                filterExpression = string.Format("(VisitID = {0} OR (RegistrationID = {1} AND IsChargesTransfered = 1))", hdnVisitID.Value, hdnLinkedRegistrationID.Value);
            else
                filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);
            filterExpression += string.Format(" AND DepartmentID = '{0}'", Constant.Facility.PHARMACY);
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

        protected bool IsEditable = true;
        private void EntityToControl(vPatientChargesHd entity, ref bool isShowWatermark, ref string watermarkText)
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

            hdnGCTransactionStatus.Value = entity.GCTransactionStatus;
            hdnTransactionID.Value = entity.TransactionID.ToString();
            hdnPrescriptionOrderID.Value = entity.PrescriptionOrderID.ToString();
            txtPrescriptionOrderNo.Text = entity.TransactionNo;
            txtPrescriptionDate.Text = entity.TransactionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPrescriptionTime.Text = entity.TransactionTime.ToString();
            txtDispenseryUnitName.Text = entity.ServiceUnitName;
            hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();

            if (entity.IsAIOTransaction)
            {
                imgIsAIOTransaction.Src = string.Format("{0}", "~/libs/Images/Status/has_aio_package.png");
                imgIsAIOTransaction.Style.Remove("display");
            }
            else
            {
                imgIsAIOTransaction.Style.Add("display", "none");
            }

            BindGridView();

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

        private void BindGridView()
        {
            IsShowSwitchIcon = hdnGCCustomerType.Value != Constant.CustomerType.PERSONAL;

            string filterExpression = "1 = 0";

            if (hdnPrescriptionOrderID.Value != "")
                filterExpression = string.Format("TransactionID = {0} AND ID IS NOT NULL AND IsDeleted = 0 ORDER BY ID DESC", hdnTransactionID.Value);
            List<vPatientChargesDt> lstEntity = BusinessLayer.GetvPatientChargesDtList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();

            if (lstEntity.Count > 0)
            {
                ((HtmlGenericControl)lvwView.FindControl("tdTotalAllPayer")).InnerHtml = lstEntity.Sum(x => x.PayerAmount).ToString("N");
                ((HtmlGenericControl)lvwView.FindControl("tdTotalAllPatient")).InnerHtml = lstEntity.Sum(x => x.PatientAmount).ToString("N");
                ((HtmlGenericControl)lvwView.FindControl("tdTotalAll")).InnerHtml = lstEntity.Sum(x => x.LineAmount).ToString("N");
            }
        }
        #endregion

        #region Save Entity
        public override void SaveTransactionHeader(IDbContext ctx, ref int transactionID)
        {
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            return false;
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            return false;
        }
        #endregion

        //#region Void Entity
        //protected override bool OnVoidRecord(ref string errMessage)
        //{
        //    bool result = true;
        //    IDbContext ctx = DbFactory.Configure(true);
        //    PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
        //    PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
        //    try
        //    {
        //        PatientChargesHd entity = entityHdDao.Get(Convert.ToInt32(hdnTransactionID.Value));
        //        if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN || entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
        //        {
        //            entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
        //            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
        //            entityHdDao.Update(entity);

        //            List<PatientChargesDt> lstPatientChargesDt = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID = {0} AND IsDeleted = 0", hdnTransactionID.Value), ctx);
        //            foreach (PatientChargesDt patientChargesDt in lstPatientChargesDt)
        //            {
        //                patientChargesDt.IsApproved = false;
        //                patientChargesDt.IsDeleted = true;
        //                patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
        //                entityDtDao.Update(patientChargesDt);
        //            }
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
        //        Helper.InsertErrorLog(ex);
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

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            string[] param = type.Split(';');
            string gcDeleteReason = param[1];
            string reason = param[2];
            bool result = true;

            if (param[0] == "void")
            {
                IDbContext ctx = DbFactory.Configure(true);
                PrescriptionOrderHdDao orderHdDao = new PrescriptionOrderHdDao(ctx);
                PrescriptionOrderDtDao orderDtDao = new PrescriptionOrderDtDao(ctx);
                PatientChargesHdDao chargesDao = new PatientChargesHdDao(ctx);
                PatientChargesDtDao chargesDtDao = new PatientChargesDtDao(ctx);
                //ChargesStatusLogDao logDao = new ChargesStatusLogDao(ctx);
                try
                {
                    //ChargesStatusLog log = new ChargesStatusLog();
                    //string statusOld = "", statusNew = "";
                    Int32 TransactionID = Convert.ToInt32(hdnTransactionID.Value);
                    PatientChargesHd entity = chargesDao.Get(TransactionID);
                    //statusOld = entity.GCTransactionStatus;
                    if (AppSession.IsAdminCanCancelAllTransaction)
                    {
                        if (entity.PatientBillingID == null && entity.GCTransactionStatus == Constant.TransactionStatus.OPEN || entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                        {
                            entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                            entity.GCVoidReason = gcDeleteReason;
                            if (gcDeleteReason == Constant.DeleteReason.OTHER)
                            {
                                entity.VoidReason = reason;
                            }
                            entity.VoidBy = AppSession.UserLogin.UserID;
                            entity.VoidDate = DateTime.Now;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            chargesDao.Update(entity);

                            //statusNew = entity.GCTransactionStatus;

                            //log.VisitID = entity.VisitID;
                            //log.TransactionID = entity.TransactionID;
                            //log.LogDate = DateTime.Now;
                            //log.UserID = AppSession.UserLogin.UserID;
                            //log.GCTransactionStatusOLD = statusOld;
                            //log.GCTransactionStatusNEW = statusNew;
                            //logDao.Insert(log);

                            List<PatientChargesDt> lstPatientChargesDt = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID = {0} AND IsDeleted = 0", hdnTransactionID.Value), ctx);
                            foreach (PatientChargesDt patientChargesDt in lstPatientChargesDt)
                            {
                                patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                                patientChargesDt.IsApproved = false;
                                patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                chargesDtDao.Update(patientChargesDt);

                                if (entity.PrescriptionOrderID != null) //Not UDD Transaction
                                {
                                    PrescriptionOrderDt orderDt = orderDtDao.Get((int)patientChargesDt.PrescriptionOrderDetailID);
                                    if (orderDt != null)
                                    {
                                        if (!orderDt.IsDeleted)
                                        {
                                            if (orderDt.ItemID.ToString() == BusinessLayer.GetSettingParameter(Constant.SettingParameter.NON_MASTER_ITEM).ParameterValue)
                                            {
                                                orderDt.GCPrescriptionOrderStatus = Constant.TestOrderStatus.CANCELLED;
                                                orderDt.GCVoidReason = Constant.DeleteReason.OTHER;
                                                orderDt.VoidReason = "Linked transaction was deleted";
                                            }
                                            else
                                            {
                                                PrescriptionOrderHd orderHd = orderHdDao.Get((int)entity.PrescriptionOrderID);
                                                if (!orderHd.IsCreatedBySystem)
                                                {
                                                    if (hdnIsDischarges.Value == "1")
                                                    {
                                                        orderDt.GCPrescriptionOrderStatus = Constant.TestOrderStatus.CANCELLED;
                                                        orderDt.GCVoidReason = Constant.DeleteReason.OTHER;
                                                        orderDt.VoidReason = "Linked transaction was deleted";
                                                    }
                                                    else
                                                    {
                                                        orderDt.GCPrescriptionOrderStatus = Constant.TestOrderStatus.RECEIVED;
                                                    }
                                                }
                                                else
                                                {
                                                    orderDt.GCPrescriptionOrderStatus = Constant.TestOrderStatus.CANCELLED;
                                                    orderDt.GCVoidReason = Constant.DeleteReason.OTHER;
                                                    orderDt.VoidReason = "Linked transaction was deleted";
                                                }
                                            }
                                            orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            orderDtDao.Update(orderDt);
                                        }
                                    }
                                }
                            }

                            //Update Status PrescriptionOrderHd
                            if (entity.PrescriptionOrderID != null)
                            {
                                PrescriptionOrderHd orderHd = orderHdDao.Get((int)entity.PrescriptionOrderID);
                                if (orderHd != null)
                                {
                                    if (!orderHd.IsCreatedBySystem)
                                    {
                                        if (hdnIsDischarges.Value == "1")
                                        {
                                            orderHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                            orderHd.GCVoidReason = Constant.DeleteReason.OTHER;
                                            orderHd.VoidReason = "Linked transaction was deleted";
                                            orderHd.GCOrderStatus = Constant.TestOrderStatus.CANCELLED;
                                        }
                                        else
                                        {
                                            orderHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                            orderHd.GCOrderStatus = Constant.TestOrderStatus.RECEIVED;
                                        }
                                        orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        orderHdDao.Update(orderHd);
                                    }
                                    else
                                    {
                                        orderHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                        orderHd.GCOrderStatus = Constant.TestOrderStatus.CANCELLED;
                                        orderHd.GCVoidReason = Constant.DeleteReason.OTHER;
                                        orderHd.VoidReason = "Linked transaction was deleted";
                                        orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        orderHdDao.Update(orderHd);
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
                            entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                            entity.GCVoidReason = gcDeleteReason;
                            if (gcDeleteReason == Constant.DeleteReason.OTHER)
                            {
                                entity.VoidReason = reason;
                            }
                            entity.VoidBy = AppSession.UserLogin.UserID;
                            entity.VoidDate = DateTime.Now;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            chargesDao.Update(entity);

                            //statusNew = entity.GCTransactionStatus;

                            //log.VisitID = entity.VisitID;
                            //log.TransactionID = entity.TransactionID;
                            //log.LogDate = DateTime.Now;
                            //log.UserID = AppSession.UserLogin.UserID;
                            //log.GCTransactionStatusOLD = statusOld;
                            //log.GCTransactionStatusNEW = statusNew;
                            //logDao.Insert(log);

                            List<PatientChargesDt> lstPatientChargesDt = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID = {0} AND IsDeleted = 0", hdnTransactionID.Value), ctx);
                            foreach (PatientChargesDt patientChargesDt in lstPatientChargesDt)
                            {
                                patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                                patientChargesDt.IsApproved = false;
                                patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                chargesDtDao.Update(patientChargesDt);

                                if (entity.PrescriptionOrderID != null) //Not UDD Transaction
                                {
                                    //        PrescriptionOrderDt orderDt = orderDtDao.Get((int)patientChargesDt.PrescriptionOrderDetailID);
                                    //        if (orderDt != null)
                                    //        {
                                    //            if (!orderDt.IsDeleted)
                                    //            {
                                    //                if (hdnIsDischarges.Value == "1")
                                    //                {
                                    //                    orderDt.GCPrescriptionOrderStatus = Constant.TestOrderStatus.CANCELLED;
                                    //                    orderDt.GCVoidReason = Constant.DeleteReason.OTHER;
                                    //                    orderDt.VoidReason = "Linked transaction was deleted";
                                    //                }
                                    //                else
                                    //                {
                                    //                    orderDt.GCPrescriptionOrderStatus = Constant.TestOrderStatus.RECEIVED;
                                    //                }
                                    //            }
                                    //        } 
                                    //    }
                                    //}

                                    PrescriptionOrderDt orderDt = orderDtDao.Get((int)patientChargesDt.PrescriptionOrderDetailID);
                                    if (orderDt != null)
                                    {
                                        if (!orderDt.IsDeleted)
                                        {
                                            if (orderDt.ItemID.ToString() == BusinessLayer.GetSettingParameter(Constant.SettingParameter.NON_MASTER_ITEM).ParameterValue)
                                            {
                                                orderDt.GCPrescriptionOrderStatus = Constant.TestOrderStatus.CANCELLED;
                                                orderDt.GCVoidReason = Constant.DeleteReason.OTHER;
                                                orderDt.VoidReason = "Linked transaction was deleted";
                                            }
                                            else
                                            {
                                                PrescriptionOrderHd orderHd = orderHdDao.Get((int)entity.PrescriptionOrderID);
                                                if (!orderHd.IsCreatedBySystem)
                                                {
                                                    if (hdnIsDischarges.Value == "1")
                                                    {
                                                        orderDt.GCPrescriptionOrderStatus = Constant.TestOrderStatus.CANCELLED;
                                                        orderDt.GCVoidReason = Constant.DeleteReason.OTHER;
                                                        orderDt.VoidReason = "Linked transaction was deleted";
                                                    }
                                                    else
                                                    {
                                                        orderDt.GCPrescriptionOrderStatus = Constant.TestOrderStatus.RECEIVED;
                                                    }
                                                }
                                                else
                                                {
                                                    orderDt.GCPrescriptionOrderStatus = Constant.TestOrderStatus.CANCELLED;
                                                    orderDt.GCVoidReason = Constant.DeleteReason.OTHER;
                                                    orderDt.VoidReason = "Linked transaction was deleted";
                                                }
                                            }
                                            orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            orderDtDao.Update(orderDt);
                                        }
                                    }
                                }
                            }

                            //Update Status PrescriptionOrderHd
                            if (entity.PrescriptionOrderID != null)
                            {
                                PrescriptionOrderHd orderHd = orderHdDao.Get((int)entity.PrescriptionOrderID);
                                if (orderHd != null)
                                {
                                    if (!orderHd.IsCreatedBySystem)
                                    {
                                        int dtAllCount = BusinessLayer.GetPrescriptionOrderDtRowCount(string.Format("PrescriptionOrderID = {0} AND IsRFlag = 1 AND IsDeleted = 0", hdnPrescriptionOrderID.Value), ctx);
                                        int dtOpenCount = BusinessLayer.GetPrescriptionOrderDtRowCount(string.Format("PrescriptionOrderID = {0} AND IsRFlag = 1 AND GCPrescriptionOrderStatus = '{1}' AND IsDeleted = 0", hdnPrescriptionOrderID.Value, Constant.OrderStatus.RECEIVED), ctx);
                                        int dtProcessedCount = BusinessLayer.GetPrescriptionOrderDtRowCount(string.Format("PrescriptionOrderID = {0} AND IsRFlag = 1 AND GCPrescriptionOrderStatus = '{1}' AND IsDeleted = 0", hdnPrescriptionOrderID.Value, Constant.OrderStatus.IN_PROGRESS), ctx);
                                        int dtVoidCount = BusinessLayer.GetPrescriptionOrderDtRowCount(string.Format("PrescriptionOrderID = {0} AND IsRFlag = 1 AND GCPrescriptionOrderStatus = '{1}' AND IsDeleted = 0", hdnPrescriptionOrderID.Value, Constant.OrderStatus.CANCELLED), ctx);

                                        if (dtVoidCount == dtAllCount)
                                        {
                                            orderHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                            orderHd.GCVoidReason = Constant.DeleteReason.OTHER;
                                            orderHd.VoidReason = "Linked transaction was deleted";
                                            orderHd.GCOrderStatus = Constant.TestOrderStatus.CANCELLED;
                                            orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            orderHdDao.Update(orderHd);
                                        }
                                        else
                                        {
                                            orderHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                            orderHd.GCOrderStatus = Constant.TestOrderStatus.RECEIVED;
                                            orderHd.StartDate = DateTime.Now.Date;
                                            orderHd.StartTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                                            orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            orderHdDao.Update(orderHd);
                                        }
                                    }
                                    else
                                    {
                                        orderHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                        orderHd.GCOrderStatus = Constant.TestOrderStatus.CANCELLED;
                                        orderHd.GCVoidReason = Constant.DeleteReason.OTHER;
                                        orderHd.VoidReason = "Linked transaction was deleted";
                                        orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        orderHdDao.Update(orderHd);
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

        #region Process Detail
        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int TransactionID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                if (OnSaveEditRecordEntityDt(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else if (param[0] == "switch")
            {
                int ID = Convert.ToInt32(param[1]);
                if (OnSwitchDt(ref errMessage, ID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else if (param[0] == "delete")
            {
                if (OnDeleteEntityDt(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTransactionID"] = TransactionID.ToString();
        }

        private bool OnSwitchDt(ref string errMessage, int ID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            try
            {
                PatientChargesHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnTransactionID.Value));
                if (AppSession.IsAdminCanCancelAllTransaction)
                {
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || entityHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                    {
                        PatientChargesDt entity = entityDtDao.Get(ID);
                        if (!entity.IsDeleted)
                        {
                            decimal temp = entity.PayerAmount;
                            entity.PayerAmount = entity.PatientAmount;
                            entity.PatientAmount = temp;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entity);
                        }
                    }
                    else
                    {
                        errMessage = "Transaksi " + entityHd.TransactionNo + " Sudah Diproses. Tidak Bisa Diubah";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        result = false;
                    }
                }
                else
                {
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        PatientChargesDt entity = entityDtDao.Get(ID);
                        if (!entity.IsDeleted)
                        {
                            decimal temp = entity.PayerAmount;
                            entity.PayerAmount = entity.PatientAmount;
                            entity.PatientAmount = temp;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entity);
                        }
                    }
                    else
                    {
                        errMessage = "Transaksi " + entityHd.TransactionNo + " Sudah Diproses. Tidak Bisa Diubah";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
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

        private bool OnSaveEditRecordEntityDt(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            try
            {
                PatientChargesHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnTransactionID.Value));
                if (AppSession.IsAdminCanCancelAllTransaction)
                {
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || entityHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                    {
                        PatientChargesDt entityChargesDt = BusinessLayer.GetPatientChargesDt(Convert.ToInt32(hdnEntryID.Value));
                        if (!entityChargesDt.IsDeleted)
                        {
                            entityChargesDt.ChargeClassID = Convert.ToInt32(cboChargeClass.Value);
                            entityChargesDt.BaseTariff = Convert.ToDecimal(hdnBasePrice.Value);
                            entityChargesDt.TariffComp1 = entityChargesDt.Tariff = Convert.ToDecimal(Request.Form[txtItemUnitPrice.UniqueID]);
                            entityChargesDt.PatientAmount = Convert.ToDecimal(Request.Form[txtPatientAmount.UniqueID]);
                            entityChargesDt.PayerAmount = Convert.ToDecimal(Request.Form[txtPayerAmount.UniqueID]);
                            entityChargesDt.LineAmount = Convert.ToDecimal(Request.Form[txtLineAmount.UniqueID]);
                            entityChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entityChargesDt);
                        }
                    }
                    else
                    {
                        errMessage = "Transaksi " + entityHd.TransactionNo + " Sudah Diproses. Tidak Bisa Diubah";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        result = false;
                    }
                }
                else
                {
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        PatientChargesDt entityChargesDt = BusinessLayer.GetPatientChargesDt(Convert.ToInt32(hdnEntryID.Value));
                        if (!entityChargesDt.IsDeleted)
                        {
                            entityChargesDt.ChargeClassID = Convert.ToInt32(cboChargeClass.Value);
                            entityChargesDt.BaseTariff = Convert.ToDecimal(hdnBasePrice.Value);
                            entityChargesDt.TariffComp1 = entityChargesDt.Tariff = Convert.ToDecimal(Request.Form[txtItemUnitPrice.UniqueID]);
                            entityChargesDt.PatientAmount = Convert.ToDecimal(Request.Form[txtPatientAmount.UniqueID]);
                            entityChargesDt.PayerAmount = Convert.ToDecimal(Request.Form[txtPayerAmount.UniqueID]);
                            entityChargesDt.LineAmount = Convert.ToDecimal(Request.Form[txtLineAmount.UniqueID]);
                            entityChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entityChargesDt);
                        }
                    }
                    else
                    {
                        errMessage = "Transaksi " + entityHd.TransactionNo + " Sudah Diproses. Tidak Bisa Diubah";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
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
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private bool OnDeleteEntityDt(ref string errMessage)
        {
            bool result = true;
            return result;
        }
        #endregion

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }
    }
}
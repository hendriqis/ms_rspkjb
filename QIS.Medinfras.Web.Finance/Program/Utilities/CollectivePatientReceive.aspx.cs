using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Service;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class CollectivePatientReceive : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.FN_COLLECTIVE_PATIENT_RECEIVE;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            txtPeriodFrom.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPeriodTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPaymentDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            hdnIsFinalisasiKlaimAfterARInvoice.Value = AppSession.IsClaimFinalAfterARInvoice ? "1" : "0";
            hdnIsFinalisasiKlaimBeforeARInvoiceAndSkipProcessKlaim.Value = AppSession.IsClaimFinalBeforeARInvoiceAndSkipProcessClaim ? "1" : "0";

            List<Department> lstDepartment = BusinessLayer.GetDepartmentList(string.Format("IsActive = 1 AND IsHasRegistration = 1 AND DepartmentID != '{0}'", Constant.Facility.INPATIENT));
            //lstDepartment.Insert(0, new Department { DepartmentID = "", DepartmentName = "" });
            Methods.SetComboBoxField<Department>(cboDepartment, lstDepartment, "DepartmentName", "DepartmentID");
            cboDepartment.SelectedIndex = 0;

            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(String.Format(
                                            "ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}') AND IsActive = 1 AND IsDeleted = 0",
                                            Constant.StandardCode.CARD_TYPE, //0
                                            Constant.StandardCode.PAYMENT_METHOD, //1
                                            Constant.StandardCode.PAYMENT_TYPE, //2
                                            Constant.StandardCode.CARD_PROVIDER, //3
                                            Constant.StandardCode.CASHIER_GROUP, //4
                                            Constant.StandardCode.SHIFT //5
                                        ));

            Methods.SetComboBoxField<StandardCode>(cboPaymentType, lstSc.Where(p => p.StandardCodeID == Constant.PaymentType.SETTLEMENT
                                                                                    || p.StandardCodeID == Constant.PaymentType.AR_PATIENT
                                                                                    || p.StandardCodeID == Constant.PaymentType.AR_PAYER
                                                                                ).ToList(), "StandardCodeName", "StandardCodeID");
            cboPaymentType.SelectedIndex = 0;

            Methods.SetComboBoxField<StandardCode>(cboCashierGroup, lstSc.Where(p => p.ParentID == Constant.StandardCode.CASHIER_GROUP).ToList(), "StandardCodeName", "StandardCodeID", DropDownStyle.DropDownList);
            cboCashierGroup.SelectedIndex = 0;

            List<StandardCode> lstShift = lstSc.Where(p => p.ParentID == Constant.StandardCode.SHIFT).ToList();
            Methods.SetComboBoxField<StandardCode>(cboShift, lstShift, "StandardCodeName", "StandardCodeID");
            cboShift.SelectedIndex = 0;

            Methods.SetComboBoxField<StandardCode>(cboPaymentMethod, lstSc.Where(p => p.ParentID == Constant.StandardCode.PAYMENT_METHOD && p.StandardCodeID != Constant.PaymentMethod.CREDIT && p.StandardCodeID != Constant.PaymentMethod.PAYMENT_RETURN && p.StandardCodeID != Constant.PaymentMethod.DOWN_PAYMENT).ToList(), "StandardCodeName", "StandardCodeID");
            cboPaymentMethod.SelectedIndex = 0;

            List<EDCMachine> lstEDCMachine = BusinessLayer.GetEDCMachineList("IsDeleted = 0");
            Methods.SetComboBoxField<EDCMachine>(cboEDCMachine, lstEDCMachine, "EDCMachineName", "EDCMachineID");
            cboEDCMachine.SelectedIndex = 0;

            Methods.SetComboBoxField<StandardCode>(cboCardType, lstSc.Where(p => p.ParentID == Constant.StandardCode.CARD_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            cboCardType.SelectedIndex = 0;

            List<Bank> lstBank = BusinessLayer.GetBankList(string.Format("IsDeleted = 0"));
            Methods.SetComboBoxField<Bank>(cboBank, lstBank.Where(t => !t.IsVirtualPayment).ToList(), "BankName", "BankID");
            cboBank.SelectedIndex = 0;

            Methods.SetComboBoxField<Bank>(cboBankVirtual, lstBank.Where(t => t.IsVirtualPayment).ToList(), "BankName", "BankID");
            cboBankVirtual.SelectedIndex = 0;

            Methods.SetComboBoxField<StandardCode>(cboCardProvider, lstSc.Where(p => p.ParentID == Constant.StandardCode.CARD_PROVIDER).ToList(), "StandardCodeName", "StandardCodeID");
            cboCardProvider.SelectedIndex = 0;

            divTotalRecordSelected.InnerHtml = "0";
            divTotalBillSelected.InnerHtml = "0.00";

            GetSettingParameter();

            BindGridView();
        }

        private void GetSettingParameter()
        {
            List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')",
                                                                            Constant.SettingParameter.FN_KONTROL_PEMBUATAN_TAGIHAN, //0
                                                                            Constant.SettingParameter.FN_PEMBUATAN_TAGIHAN_JIKA_TIDAK_ADA_OUTSTANDING_ORDER, //1
                                                                            Constant.SettingParameter.FN_BATAS_TANGGUNGAN_LEBIH_BESAR_DARI_TAGIHAN, //2
                                                                            Constant.SettingParameter.FN_IS_ALLOW_ROUNDING_AMOUNT, //3
                                                                            Constant.SettingParameter.FN_NILAI_PEMBULATAN_TAGIHAN, //4
                                                                            Constant.SettingParameter.FN_PEMBULATAN_TAGIHAN_KE_ATAS, //5
                                                                            Constant.SettingParameter.FN_BLOK_PEMBUATAN_TAGIHAN_SAAT_ADA_TRANSAKSI_MASIH_OPEN, //6
                                                                            Constant.SettingParameter.FILTER_PREVIOUS_TRANSACTION_DATE_INTERVAL //7
            ));

            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}')",
                                                                        AppSession.UserLogin.HealthcareID, //0
                                                                        Constant.SettingParameter.FN_BIAYA_ADM_RI_DALAM_PERSENTASE, //1
                                                                        Constant.SettingParameter.FN_NILAI_BIAYA_ADM_RI, //2
                                                                        Constant.SettingParameter.FN_NILAI_MIN_BIAYA_ADM_RI, //3
                                                                        Constant.SettingParameter.FN_NILAI_MAX_BIAYA_ADM_RI, //4
                                                                        Constant.SettingParameter.FN_BIAYA_SERVICE_RI_DALAM_PERSENTASE, //5
                                                                        Constant.SettingParameter.FN_NILAI_BIAYA_SERVICE_RI, //6
                                                                        Constant.SettingParameter.FN_NILAI_MIN_BIAYA_SERVICE_RI, //7
                                                                        Constant.SettingParameter.FN_NILAI_MAX_BIAYA_SERVICE_RI, //8
                                                                        Constant.SettingParameter.FN_SELISIH_PASIEN_BPJS_NAIK_KELAS, //9
                                                                        Constant.SettingParameter.FN_BIAYA_ADM_KELAS_TERTINGGI, //10
                                                                        Constant.SettingParameter.FN_ADMIN_HANYA_RAWAT_INAP, //11
                                                                        Constant.SettingParameter.FN_PEMBUATAN_TAGIHAN_BPJS_MENGGUNAKAN_CARA_BPJS, //12
                                                                        Constant.SettingParameter.FN_IS_GROUPER_AMOUNT_CLAIM_DEFAULT_ZERO //13
            ));

            hdnIsGrouperAmountClaimDefaultZero.Value = lstSettingParameterDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_GROUPER_AMOUNT_CLAIM_DEFAULT_ZERO).FirstOrDefault().ParameterValue;
            hdnPembuatanTagihanTidakAdaOutstandingOrder.Value = lstSettingParameter.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_PEMBUATAN_TAGIHAN_JIKA_TIDAK_ADA_OUTSTANDING_ORDER).FirstOrDefault().ParameterValue;
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridView();
                result = "refresh|";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView()
        {
            string dateFrom = Helper.GetDatePickerValue(txtPeriodFrom.Text).ToString(Constant.FormatString.DATE_FORMAT_112);
            string dateTo = Helper.GetDatePickerValue(txtPeriodTo.Text).ToString(Constant.FormatString.DATE_FORMAT_112);

            string filterExpression = string.Format("(RegistrationDate BETWEEN '{0}' AND '{1}') AND DepartmentID = '{2}'", dateFrom, dateTo, cboDepartment.Value);

            if (hdnBusinessPartnerID.Value != "" && hdnBusinessPartnerID.Value != null)
            {
                filterExpression += string.Format(" AND BusinessPartnerID = '{0}'", hdnBusinessPartnerID.Value);
            }

            if (hdnMCUPackageItemID.Value != "" && hdnMCUPackageItemID.Value != null)
            {
                filterExpression += string.Format(" AND ItemID = '{0}'", hdnMCUPackageItemID.Value);
            }

            if (hdnFilterExpressionQuickSearch.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);

            List<vRegistrationCollectivePatientReceive> lstEntity = BusinessLayer.GetvRegistrationCollectivePatientReceiveList(filterExpression);

            hdnTotalRecordAll.Value = lstEntity.Count().ToString();
            hdnTotalBillAll.Value = lstEntity.Sum(t => t.LineAmount).ToString("N2");

            divTotalRecordAll.InnerHtml = lstEntity.Count().ToString();
            divTotalBillAll.InnerHtml = lstEntity.Sum(t => t.LineAmount).ToString("N2");

            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ConsultVisitDao visitDao = new ConsultVisitDao(ctx);
            RegistrationDao registrationDao = new RegistrationDao(ctx);
            PatientChargesHdDao entityDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            PatientBillDao patientBillDao = new PatientBillDao(ctx);
            RegistrationPayerDao registrationPayerDao = new RegistrationPayerDao(ctx);
            AuditLogDao entityAuditLogDao = new AuditLogDao(ctx);
            PatientPaymentHdDao paymentHdDao = new PatientPaymentHdDao(ctx);
            PatientPaymentDtDao paymentDtDao = new PatientPaymentDtDao(ctx);
            PatientPaymentDtInfoDao paymentDtInfoDao = new PatientPaymentDtInfoDao(ctx);
            PatientBillPaymentDao patientBillPaymentDao = new PatientBillPaymentDao(ctx);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            try
            {
                if (type == "process")
                {
                    string errMessageRegTransfer = "";
                    string errMessageRegPendingTestOrder = "";
                    string errMessageRegPendingServiceOrder = "";
                    string[] paramSplit = hdnSelectedMember.Value.Split(',');
                    foreach (string id in paramSplit)
                    {
                        if (!String.IsNullOrEmpty(id))
                        {
                            int visitID = Convert.ToInt32(id);
                            string filterExp = string.Format("VisitID = '{0}'", visitID);
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            vConsultVisit4 visit = BusinessLayer.GetvConsultVisit4List(filterExp, ctx).FirstOrDefault();

                            PatientBill patientBill = new PatientBill();

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            string filterChargesHd = string.Format("VisitID = {0} AND GCTransactionStatus IN ('{1}','{2}')", visitID, Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionStatus.OPEN);
                            List<PatientChargesHd> lstPatientChargesHd = BusinessLayer.GetPatientChargesHdList(filterChargesHd, ctx);
                            List<PatientChargesDt> lstAllPatientChargesDt = new List<PatientChargesDt>();
                            string lstTransactionID = "";
                            if (lstPatientChargesHd.Count > 0)
                            {
                                lstTransactionID = string.Join(",", lstPatientChargesHd.Select(t => t.TransactionID).ToList());
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                string filterChargesDt = string.Format("TransactionID IN ({0}) AND GCTransactionDetailStatus != '{1}' AND IsDeleted = 0", lstTransactionID, Constant.TransactionStatus.VOID);
                                lstAllPatientChargesDt = BusinessLayer.GetPatientChargesDtList(filterChargesDt, ctx);
                            }

                            if (hdnPembuatanTagihanTidakAdaOutstandingOrder.Value == "0")
                            {
                                #region Tanpa Cek Outstanding Order
                                if ((visit.GCRegistrationStatus != Constant.VisitStatus.CLOSED) || (!AppSession.IsUsedReopenBilling && !AppSession.RegisteredPatient.IsBillingReopen && visit.GCRegistrationStatus != Constant.VisitStatus.CLOSED)
                                    || (AppSession.IsUsedReopenBilling && visit.IsBillingReopen && visit.GCRegistrationStatus == Constant.VisitStatus.CLOSED))
                                {
                                    #region Process
                                    if (lstPatientChargesHd.Count > 0)
                                    {
                                        Registration registration = registrationDao.Get(visit.RegistrationID);
                                        if (!registration.IsLockDown)
                                        {
                                            registration.IsLockDown = true;
                                            registration.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            registrationDao.Update(registration);
                                        }

                                        patientBill = new PatientBill();
                                        patientBill.BillingDate = DateTime.Now;
                                        patientBill.BillingTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                                        patientBill.RegistrationID = visit.RegistrationID;
                                        string transactionCode = "";
                                        switch (visit.DepartmentID)
                                        {
                                            case Constant.Facility.INPATIENT: transactionCode = Constant.TransactionCode.IP_PATIENT_BILL; break;
                                            case Constant.Facility.MEDICAL_CHECKUP: transactionCode = Constant.TransactionCode.MCU_PATIENT_BILL; break;
                                            case Constant.Facility.EMERGENCY: transactionCode = Constant.TransactionCode.ER_PATIENT_BILL; break;
                                            case Constant.Facility.PHARMACY: transactionCode = Constant.TransactionCode.PH_PATIENT_BILL; break;
                                            case Constant.Facility.DIAGNOSTIC:
                                                if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                                                    transactionCode = Constant.TransactionCode.LABORATORY_PATIENT_BILL;
                                                else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                                                    transactionCode = Constant.TransactionCode.IMAGING_PATIENT_BILL;
                                                else
                                                    transactionCode = Constant.TransactionCode.OTHER_PATIENT_BILL; break;
                                            default: transactionCode = Constant.TransactionCode.OP_PATIENT_BILL; break;
                                        }

                                        patientBill.CoverageAmount = 0;
                                        patientBill.AdministrationFeeAmount = 0;
                                        patientBill.PatientAdminFeeAmount = 0;
                                        patientBill.ServiceFeeAmount = 0;
                                        patientBill.PatientServiceFeeAmount = 0;
                                        patientBill.DiffCoverageAmount = 0;
                                        patientBill.PatientBillingNo = BusinessLayer.GenerateTransactionNo(transactionCode, patientBill.BillingDate, ctx);
                                        patientBill.GCTransactionStatus = Constant.TransactionStatus.OPEN;

                                        patientBill.TotalPatientAmount = lstAllPatientChargesDt.Sum(t => t.PatientAmount);
                                        patientBill.TotalPayerAmount = lstAllPatientChargesDt.Sum(t => t.PayerAmount);
                                        patientBill.TotalAmount = lstAllPatientChargesDt.Sum(t => t.LineAmount);

                                        patientBill.GCVoidReason = null;
                                        patientBill.VoidReason = null;
                                        patientBill.CreatedBy = AppSession.UserLogin.UserID;

                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        patientBill.PatientBillingID = patientBillDao.InsertReturnPrimaryKeyID(patientBill);

                                        DateTime maxProposedDate = Convert.ToDateTime(Constant.ConstantDate.DEFAULT_NULL);
                                        Int32 maxProposedBy = 0;
                                        foreach (PatientChargesHd patientChargesHd in lstPatientChargesHd)
                                        {
                                            if (patientChargesHd.ProposedDate != null)
                                            {
                                                if (maxProposedDate == Convert.ToDateTime(Constant.ConstantDate.DEFAULT_NULL))
                                                {
                                                    maxProposedDate = patientChargesHd.ProposedDate;
                                                    maxProposedBy = Convert.ToInt32(patientChargesHd.ProposedBy);
                                                }
                                                else
                                                {
                                                    if (patientChargesHd.ProposedDate > maxProposedDate)
                                                    {
                                                        maxProposedDate = patientChargesHd.ProposedDate;
                                                        maxProposedBy = Convert.ToInt32(patientChargesHd.ProposedBy);
                                                    }
                                                }
                                            }

                                            patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                                            patientChargesHd.PatientBillingID = patientBill.PatientBillingID;
                                            patientChargesHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            entityDao.Update(patientChargesHd);

                                            List<PatientChargesDt> lstPatientChargesDt = lstAllPatientChargesDt.Where(p => p.TransactionID == patientChargesHd.TransactionID).ToList();
                                            foreach (PatientChargesDt patientChargesDt in lstPatientChargesDt)
                                            {
                                                patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.PROCESSED; // dibuka comment nya oleh RN di 20200531
                                                patientChargesDt.IsApproved = true;
                                                patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                entityDtDao.Update(patientChargesDt);
                                            }
                                        }

                                        if (maxProposedBy != 0)
                                        {
                                            patientBill.LastChargesProposedBy = maxProposedBy;
                                            patientBill.LastChargesProposedDate = maxProposedDate;
                                        }
                                        //patientBill.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        patientBillDao.Update(patientBill);

                                        //Update Registration : Coverage Limit : if has been changed
                                        decimal coverageLimit = 0;
                                        decimal oldCoverageLimit = 0;

                                        if (oldCoverageLimit != coverageLimit)
                                        {
                                            AuditLog entityAuditLog = new AuditLog();

                                            entityAuditLog.OldValues = JsonConvert.SerializeObject(registration);

                                            registration.CoverageLimitAmount += coverageLimit;

                                            entityAuditLog.ObjectType = Constant.BusinessObjectType.REGISTRATION;
                                            entityAuditLog.NewValues = JsonConvert.SerializeObject(registration);
                                            entityAuditLog.UserID = AppSession.UserLogin.UserID;
                                            entityAuditLog.LogDate = DateTime.Now;
                                            entityAuditLog.TransactionID = registration.RegistrationID;
                                            entityAuditLogDao.Insert(entityAuditLog);

                                            if (visit.GCCustomerType == Constant.CustomerType.BPJS) registration.BPJSAmount += coverageLimit;
                                            registration.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            registrationDao.Update(registration);

                                            RegistrationPayer oRegistrationPayer = BusinessLayer.GetRegistrationPayerList(string.Format("RegistrationID = {0} AND IsPrimaryPayer = 1 AND IsDeleted = 0", visit.RegistrationID)).FirstOrDefault();
                                            if (oRegistrationPayer != null)
                                            {
                                                oRegistrationPayer.CoverageLimitAmount += coverageLimit;
                                                oRegistrationPayer.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                registrationPayerDao.Update(oRegistrationPayer);
                                            }
                                        }
                                    }
                                    #endregion
                                }
                                else
                                {
                                    result = false;
                                    if (!String.IsNullOrEmpty(errMessageRegTransfer))
                                    {
                                        errMessageRegTransfer += string.Format("|{0}", visit.RegistrationNo);
                                    }
                                    else
                                    {
                                        errMessageRegTransfer = visit.RegistrationNo;
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                #region Cek Outstanding Order
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                if (lstPatientChargesHd.Count > 0)
                                {
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    List<TestOrderHd> lstPendingTestOrderHd = BusinessLayer.GetTestOrderHdList(string.Format("VisitID IN ({0}) AND GCTransactionStatus IN ('{1}','{2}')", visit.VisitID, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL), ctx);
                                    if (lstPendingTestOrderHd.Count > 0)
                                    {
                                        result = false;
                                        if (!String.IsNullOrEmpty(errMessageRegPendingTestOrder))
                                        {
                                            errMessageRegPendingTestOrder += string.Format("|{0}", visit.RegistrationNo);
                                        }
                                        else
                                        {
                                            errMessageRegPendingTestOrder = visit.RegistrationNo;
                                        }
                                    }
                                    else
                                    {
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        List<ServiceOrderHd> lstPendingServiceOrderHd = BusinessLayer.GetServiceOrderHdList(string.Format("LinkedChargesID IN ({0}) AND GCTransactionStatus IN ('{1}','{2}')", visit.VisitID, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL), ctx);
                                        if (lstPendingServiceOrderHd.Count > 0)
                                        {
                                            result = false;
                                            if (!String.IsNullOrEmpty(errMessageRegPendingServiceOrder))
                                            {
                                                errMessageRegPendingServiceOrder += string.Format("|{0}", visit.RegistrationNo);
                                            }
                                            else
                                            {
                                                errMessageRegPendingServiceOrder = visit.RegistrationNo;
                                            }
                                        }
                                        else
                                        {
                                            if ((visit.GCRegistrationStatus != Constant.VisitStatus.CLOSED) || (!AppSession.IsUsedReopenBilling && !AppSession.RegisteredPatient.IsBillingReopen && visit.GCRegistrationStatus != Constant.VisitStatus.CLOSED)
                                                || (AppSession.IsUsedReopenBilling && visit.IsBillingReopen && visit.GCRegistrationStatus == Constant.VisitStatus.CLOSED))
                                            {
                                                #region Process
                                                if (lstPatientChargesHd.Count > 0)
                                                {
                                                    Registration registration = registrationDao.Get(visit.RegistrationID);
                                                    if (!registration.IsLockDown)
                                                    {
                                                        registration.IsLockDown = true;
                                                        registration.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                        registrationDao.Update(registration);
                                                    }

                                                    patientBill = new PatientBill();
                                                    patientBill.BillingDate = DateTime.Now;
                                                    patientBill.BillingTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                                                    patientBill.RegistrationID = visit.RegistrationID;
                                                    string transactionCode = "";
                                                    switch (visit.DepartmentID)
                                                    {
                                                        case Constant.Facility.INPATIENT: transactionCode = Constant.TransactionCode.IP_PATIENT_BILL; break;
                                                        case Constant.Facility.MEDICAL_CHECKUP: transactionCode = Constant.TransactionCode.MCU_PATIENT_BILL; break;
                                                        case Constant.Facility.EMERGENCY: transactionCode = Constant.TransactionCode.ER_PATIENT_BILL; break;
                                                        case Constant.Facility.PHARMACY: transactionCode = Constant.TransactionCode.PH_PATIENT_BILL; break;
                                                        case Constant.Facility.DIAGNOSTIC:
                                                            if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                                                                transactionCode = Constant.TransactionCode.LABORATORY_PATIENT_BILL;
                                                            else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                                                                transactionCode = Constant.TransactionCode.IMAGING_PATIENT_BILL;
                                                            else
                                                                transactionCode = Constant.TransactionCode.OTHER_PATIENT_BILL; break;
                                                        default: transactionCode = Constant.TransactionCode.OP_PATIENT_BILL; break;
                                                    }

                                                    patientBill.CoverageAmount = 0;
                                                    patientBill.AdministrationFeeAmount = 0;
                                                    patientBill.PatientAdminFeeAmount = 0;
                                                    patientBill.ServiceFeeAmount = 0;
                                                    patientBill.PatientServiceFeeAmount = 0;
                                                    patientBill.DiffCoverageAmount = 0;
                                                    patientBill.PatientBillingNo = BusinessLayer.GenerateTransactionNo(transactionCode, patientBill.BillingDate, ctx);
                                                    patientBill.GCTransactionStatus = Constant.TransactionStatus.OPEN;

                                                    patientBill.TotalPatientAmount = lstAllPatientChargesDt.Sum(t => t.PatientAmount);
                                                    patientBill.TotalPayerAmount = lstAllPatientChargesDt.Sum(t => t.PayerAmount);
                                                    patientBill.TotalAmount = lstAllPatientChargesDt.Sum(t => t.LineAmount);

                                                    patientBill.GCVoidReason = null;
                                                    patientBill.VoidReason = null;
                                                    patientBill.CreatedBy = AppSession.UserLogin.UserID;

                                                    ctx.CommandType = CommandType.Text;
                                                    ctx.Command.Parameters.Clear();
                                                    patientBill.PatientBillingID = patientBillDao.InsertReturnPrimaryKeyID(patientBill);

                                                    DateTime maxProposedDate = Convert.ToDateTime(Constant.ConstantDate.DEFAULT_NULL);
                                                    Int32 maxProposedBy = 0;
                                                    foreach (PatientChargesHd patientChargesHd in lstPatientChargesHd)
                                                    {
                                                        if (patientChargesHd.ProposedDate != null)
                                                        {
                                                            if (maxProposedDate == Convert.ToDateTime(Constant.ConstantDate.DEFAULT_NULL))
                                                            {
                                                                maxProposedDate = patientChargesHd.ProposedDate;
                                                                maxProposedBy = Convert.ToInt32(patientChargesHd.ProposedBy);
                                                            }
                                                            else
                                                            {
                                                                if (patientChargesHd.ProposedDate > maxProposedDate)
                                                                {
                                                                    maxProposedDate = patientChargesHd.ProposedDate;
                                                                    maxProposedBy = Convert.ToInt32(patientChargesHd.ProposedBy);
                                                                }
                                                            }
                                                        }

                                                        patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                                                        patientChargesHd.PatientBillingID = patientBill.PatientBillingID;
                                                        patientChargesHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                        entityDao.Update(patientChargesHd);

                                                        List<PatientChargesDt> lstPatientChargesDt = lstAllPatientChargesDt.Where(p => p.TransactionID == patientChargesHd.TransactionID).ToList();
                                                        foreach (PatientChargesDt patientChargesDt in lstPatientChargesDt)
                                                        {
                                                            patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.PROCESSED; // dibuka comment nya oleh RN di 20200531
                                                            patientChargesDt.IsApproved = true;
                                                            patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                            entityDtDao.Update(patientChargesDt);
                                                        }
                                                    }

                                                    if (maxProposedBy != 0)
                                                    {
                                                        patientBill.LastChargesProposedBy = maxProposedBy;
                                                        patientBill.LastChargesProposedDate = maxProposedDate;
                                                    }
                                                    //patientBill.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                    ctx.CommandType = CommandType.Text;
                                                    ctx.Command.Parameters.Clear();
                                                    patientBillDao.Update(patientBill);

                                                    //Update Registration : Coverage Limit : if has been changed
                                                    decimal coverageLimit = 0;
                                                    decimal oldCoverageLimit = 0;

                                                    if (oldCoverageLimit != coverageLimit)
                                                    {
                                                        AuditLog entityAuditLog = new AuditLog();

                                                        entityAuditLog.OldValues = JsonConvert.SerializeObject(registration);

                                                        registration.CoverageLimitAmount += coverageLimit;

                                                        entityAuditLog.ObjectType = Constant.BusinessObjectType.REGISTRATION;
                                                        entityAuditLog.NewValues = JsonConvert.SerializeObject(registration);
                                                        entityAuditLog.UserID = AppSession.UserLogin.UserID;
                                                        entityAuditLog.LogDate = DateTime.Now;
                                                        entityAuditLog.TransactionID = registration.RegistrationID;
                                                        entityAuditLogDao.Insert(entityAuditLog);

                                                        if (visit.GCCustomerType == Constant.CustomerType.BPJS) registration.BPJSAmount += coverageLimit;
                                                        registration.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                        registrationDao.Update(registration);

                                                        RegistrationPayer oRegistrationPayer = BusinessLayer.GetRegistrationPayerList(string.Format("RegistrationID = {0} AND IsPrimaryPayer = 1 AND IsDeleted = 0", visit.RegistrationID)).FirstOrDefault();
                                                        if (oRegistrationPayer != null)
                                                        {
                                                            oRegistrationPayer.CoverageLimitAmount += coverageLimit;
                                                            oRegistrationPayer.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                            registrationPayerDao.Update(oRegistrationPayer);
                                                        }
                                                    }
                                                }
                                                #endregion
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }

                            if (result)
                            {
                                #region Payment
                                PatientPaymentHd entityHd = new PatientPaymentHd();
                                entityHd.PaymentDate = Helper.GetDatePickerValue(Request.Form[txtPaymentDate.UniqueID]);
                                entityHd.PaymentTime = "12:00";

                                entityHd.GCPaymentType = cboPaymentType.Value.ToString();
                                entityHd.RegistrationID = visit.RegistrationID;
                                entityHd.GCCashierGroup = cboCashierGroup.Value.ToString();
                                entityHd.GCShift = cboShift.Value.ToString();
                                entityHd.TotalPatientBillAmount = lstAllPatientChargesDt.Sum(t => t.PatientAmount);
                                entityHd.TotalPayerBillAmount = lstAllPatientChargesDt.Sum(t => t.PayerAmount);
                                entityHd.TotalPaymentAmount = lstAllPatientChargesDt.Sum(t => t.LineAmount);
                                entityHd.TotalFeeAmount = 0;
                                entityHd.Remarks = "";
                                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                                string transactionCodePayment = "";
                                switch (visit.DepartmentID)
                                {
                                    case Constant.Facility.INPATIENT:
                                        switch (entityHd.GCPaymentType)
                                        {
                                            case Constant.PaymentType.SETTLEMENT: transactionCodePayment = Constant.TransactionCode.IP_PATIENT_PAYMENT_SETTLEMENT; break;
                                            case Constant.PaymentType.AR_PATIENT: transactionCodePayment = Constant.TransactionCode.IP_PATIENT_PAYMENT_AR_PATIENT; break;
                                            default: transactionCodePayment = Constant.TransactionCode.IP_PATIENT_PAYMENT_AR_PATIENT; break;
                                        } break;
                                    case Constant.Facility.MEDICAL_CHECKUP:
                                        switch (entityHd.GCPaymentType)
                                        {
                                            case Constant.PaymentType.SETTLEMENT: transactionCodePayment = Constant.TransactionCode.MCU_PATIENT_PAYMENT_SETTLEMENT; break;
                                            case Constant.PaymentType.AR_PATIENT: transactionCodePayment = Constant.TransactionCode.MCU_PATIENT_PAYMENT_AR_PATIENT; break;
                                            default: transactionCodePayment = Constant.TransactionCode.MCU_PATIENT_PAYMENT_AR_PATIENT; break;
                                        } break;
                                    case Constant.Facility.EMERGENCY:
                                        switch (entityHd.GCPaymentType)
                                        {
                                            case Constant.PaymentType.SETTLEMENT: transactionCodePayment = Constant.TransactionCode.ER_PATIENT_PAYMENT_SETTLEMENT; break;
                                            case Constant.PaymentType.AR_PATIENT: transactionCodePayment = Constant.TransactionCode.ER_PATIENT_PAYMENT_AR_PATIENT; break;
                                            default: transactionCodePayment = Constant.TransactionCode.ER_PATIENT_PAYMENT_AR_PATIENT; break;
                                        } break;
                                    case Constant.Facility.PHARMACY:
                                        switch (entityHd.GCPaymentType)
                                        {
                                            case Constant.PaymentType.SETTLEMENT: transactionCodePayment = Constant.TransactionCode.PH_PATIENT_PAYMENT_SETTLEMENT; break;
                                            case Constant.PaymentType.AR_PATIENT: transactionCodePayment = Constant.TransactionCode.PH_PATIENT_PAYMENT_AR_PATIENT; break;
                                            default: transactionCodePayment = Constant.TransactionCode.PH_PATIENT_PAYMENT_AR_PATIENT; break;
                                        } break;
                                    case Constant.Facility.DIAGNOSTIC:
                                        if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                                        {
                                            switch (entityHd.GCPaymentType)
                                            {
                                                case Constant.PaymentType.SETTLEMENT: transactionCodePayment = Constant.TransactionCode.LABORATORY_PATIENT_PAYMENT_SETTLEMENT; break;
                                                case Constant.PaymentType.AR_PATIENT: transactionCodePayment = Constant.TransactionCode.LABORATORY_PATIENT_PAYMENT_AR_PATIENT; break;
                                                default: transactionCodePayment = Constant.TransactionCode.LABORATORY_PATIENT_PAYMENT_AR_PATIENT; break;
                                            }
                                        }
                                        else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                                        {
                                            switch (entityHd.GCPaymentType)
                                            {
                                                case Constant.PaymentType.SETTLEMENT: transactionCodePayment = Constant.TransactionCode.IMAGING_PATIENT_PAYMENT_SETTLEMENT; break;
                                                case Constant.PaymentType.AR_PATIENT: transactionCodePayment = Constant.TransactionCode.IMAGING_PATIENT_PAYMENT_AR_PATIENT; break;
                                                default: transactionCodePayment = Constant.TransactionCode.IMAGING_PATIENT_PAYMENT_AR_PATIENT; break;
                                            }
                                        }
                                        else
                                        {
                                            switch (entityHd.GCPaymentType)
                                            {
                                                case Constant.PaymentType.SETTLEMENT: transactionCodePayment = Constant.TransactionCode.OTHER_PATIENT_PAYMENT_SETTLEMENT; break;
                                                case Constant.PaymentType.AR_PATIENT: transactionCodePayment = Constant.TransactionCode.OTHER_PATIENT_PAYMENT_AR_PATIENT; break;
                                                default: transactionCodePayment = Constant.TransactionCode.OTHER_PATIENT_PAYMENT_AR_PATIENT; break;
                                            }
                                        } break;
                                    default:
                                        switch (entityHd.GCPaymentType)
                                        {
                                            case Constant.PaymentType.SETTLEMENT: transactionCodePayment = Constant.TransactionCode.OP_PATIENT_PAYMENT_SETTLEMENT; break;
                                            case Constant.PaymentType.AR_PATIENT: transactionCodePayment = Constant.TransactionCode.OP_PATIENT_PAYMENT_AR_PATIENT; break;
                                            default: transactionCodePayment = Constant.TransactionCode.OP_PATIENT_PAYMENT_AR_PATIENT; break;
                                        } break;
                                }
                                entityHd.PaymentNo = BusinessLayer.GenerateTransactionNo(transactionCodePayment, entityHd.PaymentDate, ctx);
                                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                entityHd.PaymentID = paymentHdDao.InsertReturnPrimaryKeyID(entityHd);

                                PatientPaymentDt entityDt = new PatientPaymentDt();
                                entityDt.PaymentID = entityHd.PaymentID;
                                entityDt.PaymentAmount = lstAllPatientChargesDt.Sum(t => t.LineAmount);

                                if (entityHd.GCPaymentType == Constant.PaymentType.SETTLEMENT)
                                {
                                    entityDt.GCPaymentMethod = cboPaymentMethod.Value.ToString();
                                    if (entityDt.GCPaymentMethod != Constant.PaymentMethod.CASH)
                                    {
                                        entityDt.EDCMachineID = Convert.ToInt32(cboEDCMachine.Value);
                                        if (entityDt.GCPaymentMethod == Constant.PaymentMethod.VIRTUAL_PAYMENT)
                                        {
                                            entityDt.BankID = Convert.ToInt32(cboBankVirtual.Value);
                                        }
                                        else
                                        {
                                            entityDt.BankID = Convert.ToInt32(cboBank.Value);
                                        }
                                        entityDt.ReferenceNo = txtReferenceNo.Text;
                                        entityDt.GCCardType = cboCardType.Value.ToString();
                                        if (!string.IsNullOrEmpty(txtCardNumber1.Text) && !String.IsNullOrEmpty(txtCardNumber4.Text))
                                        {
                                            entityDt.CardNumber = string.Format("{0}-XXXX-XXXX-{1}", txtCardNumber1.Text, txtCardNumber4.Text);
                                        }
                                        entityDt.CardHolderName = txtHolderName.Text;
                                        entityDt.GCCardProvider = cboCardProvider.Value.ToString();
                                        entityDt.BatchNo = txtBatchNo.Text;
                                        entityDt.ApprovalCode = txtApprovalCode.Text;
                                    }

                                    if (entityDt.EDCMachineID != 0 && entityDt.EDCMachineID != null)
                                    {
                                        string filterEDC = string.Format("EDCMachineID = '{0}' AND IsDeleted = 0 AND IsChargeFeeToPatient = 1", entityDt.EDCMachineID);
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        EDCMachine edc = BusinessLayer.GetEDCMachineList(filterEDC, ctx).FirstOrDefault();
                                        if (edc != null)
                                        {
                                            string filterCC = string.Format("HealthcareID = '{0}' AND GCCardType = '{1}' AND GCCardProvider = '{2}' AND EDCMachineID = '{3}' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, cboCardType.Value, cboCardProvider.Value, entityDt.EDCMachineID);
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            CreditCard cc = BusinessLayer.GetCreditCardList(filterCC, ctx).FirstOrDefault();
                                            if (cc != null)
                                            {
                                                string filterExpressionEDCCardFee = string.Format("CreditCardID = '{0}' AND IsDeleted = 0 AND '{1}' >= EffectiveDate ORDER BY ID DESC", cc.CreditCardID, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT2));
                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();
                                                EDCCardFee edcCardFee = BusinessLayer.GetEDCCardFeeList(filterExpressionEDCCardFee, ctx).FirstOrDefault();
                                                if (edcCardFee != null)
                                                {
                                                    if (edcCardFee.SurchargeFee <= 0)
                                                    {
                                                        entityDt.CardFeeAmount = cc.CreditCardFee;
                                                    }
                                                    else
                                                    {
                                                        entityDt.CardFeeAmount = edcCardFee.SurchargeFee;
                                                    }
                                                }
                                                else
                                                {
                                                    entityDt.CardFeeAmount = cc.CreditCardFee;
                                                }
                                            }
                                        }
                                    }

                                    entityDt.CreatedBy = AppSession.UserLogin.UserID;

                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    int paymentDetailID = paymentDtDao.InsertReturnPrimaryKeyID(entityDt);

                                    PatientPaymentDtInfo dtInfo = new PatientPaymentDtInfo();
                                    dtInfo.PaymentDetailID = paymentDetailID;
                                    dtInfo.GCClaimStatus = Constant.ClaimStatus.APPROVED;
                                    dtInfo.GCFinalStatus = Constant.FinalStatus.APPROVED;
                                    dtInfo.GrouperAmountClaim = dtInfo.GrouperAmountFinal = entityDt.PaymentAmount;
                                    dtInfo.ClaimBy = dtInfo.FinalBy = AppSession.UserLogin.UserID;
                                    dtInfo.ClaimDate = dtInfo.FinalDate = DateTime.Now;
                                    dtInfo.SequenceNo = Convert.ToInt32(visit.RegistrationDate.ToString("dd"));
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    paymentDtInfoDao.Insert(dtInfo);

                                    decimal patientCashback = patientBill.TotalPatientAmount - entityDt.PaymentAmount;
                                    if (patientCashback < 0)
                                    {
                                        patientCashback = patientCashback * -1;
                                    }
                                    entityHd.CashBackAmount = patientCashback;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    paymentHdDao.Update(entityHd);
                                }
                                else if (entityHd.GCPaymentType == Constant.PaymentType.AR_PAYER)
                                {
                                    entityDt.GCPaymentMethod = Constant.PaymentMethod.CREDIT;
                                    entityDt.BusinessPartnerID = visit.BusinessPartnerID;
                                    entityDt.CardFeeAmount = 0;
                                    entityDt.CreatedBy = AppSession.UserLogin.UserID;
                                    int paymentDetailID = paymentDtDao.InsertReturnPrimaryKeyID(entityDt);

                                    if (entityDt.GCPaymentMethod == Constant.PaymentMethod.CREDIT)
                                    {
                                        string filterCustomer = string.Format("BusinessPartnerID = '{0}'", entityDt.BusinessPartnerID);
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        Customer customer = BusinessLayer.GetCustomerList(filterCustomer, ctx).FirstOrDefault();

                                        if (customer.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            PatientPaymentDtInfo dtInfo = new PatientPaymentDtInfo();
                                            dtInfo.PaymentDetailID = paymentDetailID;

                                            if (hdnIsFinalisasiKlaimAfterARInvoice.Value == "0" && hdnIsFinalisasiKlaimBeforeARInvoiceAndSkipProcessKlaim.Value == "1")
                                            {
                                                dtInfo.GCClaimStatus = Constant.ClaimStatus.APPROVED;
                                                dtInfo.ClaimBy = AppSession.UserLogin.UserID;
                                                dtInfo.ClaimDate = DateTime.Now;

                                                dtInfo.GCFinalStatus = Constant.FinalStatus.OPEN;

                                                if (hdnIsGrouperAmountClaimDefaultZero.Value == "0")
                                                {
                                                    dtInfo.GrouperAmountClaim = entityDt.PaymentAmount;
                                                    dtInfo.GrouperAmountFinal = entityDt.PaymentAmount;
                                                }
                                            }
                                            else if (hdnIsFinalisasiKlaimAfterARInvoice.Value == "1" && hdnIsFinalisasiKlaimBeforeARInvoiceAndSkipProcessKlaim.Value == "1")
                                            {
                                                dtInfo.GCClaimStatus = Constant.ClaimStatus.APPROVED;
                                                dtInfo.ClaimBy = AppSession.UserLogin.UserID;
                                                dtInfo.ClaimDate = DateTime.Now;

                                                if (hdnIsGrouperAmountClaimDefaultZero.Value == "0")
                                                {
                                                    dtInfo.GrouperAmountClaim = entityDt.PaymentAmount;
                                                    dtInfo.GrouperAmountFinal = entityDt.PaymentAmount;
                                                }
                                            }
                                            else
                                            {
                                                dtInfo.GCClaimStatus = Constant.ClaimStatus.OPEN;

                                                if (hdnIsGrouperAmountClaimDefaultZero.Value == "0")
                                                {
                                                    dtInfo.GrouperAmountClaim = entityDt.PaymentAmount;
                                                }
                                            }

                                            dtInfo.SequenceNo = Convert.ToInt32(visit.RegistrationDate.ToString("dd"));
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            paymentDtInfoDao.Insert(dtInfo);
                                        }
                                        else
                                        {
                                            PatientPaymentDtInfo dtInfo = new PatientPaymentDtInfo();
                                            dtInfo.PaymentDetailID = paymentDetailID;
                                            dtInfo.GCClaimStatus = Constant.ClaimStatus.APPROVED;
                                            dtInfo.GCFinalStatus = Constant.FinalStatus.APPROVED;
                                            dtInfo.GrouperAmountClaim = dtInfo.GrouperAmountFinal = entityDt.PaymentAmount;
                                            dtInfo.ClaimBy = dtInfo.FinalBy = AppSession.UserLogin.UserID;
                                            dtInfo.ClaimDate = dtInfo.FinalDate = DateTime.Now;
                                            dtInfo.SequenceNo = Convert.ToInt32(visit.RegistrationDate.ToString("dd"));
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            paymentDtInfoDao.Insert(dtInfo);
                                        }
                                    }
                                }
                                else if (entityHd.GCPaymentType == Constant.PaymentType.AR_PATIENT)
                                {
                                    entityDt.GCPaymentMethod = Constant.PaymentMethod.CREDIT;
                                    string filterBP = string.Format("BusinessPartnerID = (SELECT BusinessPartnerID FROM Customer WHERE GCCustomerType = '{0}')", Constant.CustomerType.PERSONAL);
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    BusinessPartners bp = BusinessLayer.GetBusinessPartnersList(filterBP, ctx).FirstOrDefault();
                                    entityDt.BusinessPartnerID = bp.BusinessPartnerID;
                                    entityDt.CardFeeAmount = 0;
                                    entityDt.CreatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    int paymentDetailID = paymentDtDao.InsertReturnPrimaryKeyID(entityDt);

                                    if (entityDt.GCPaymentMethod == Constant.PaymentMethod.CREDIT)
                                    {
                                        string filterC = string.Format("BusinessPartnerID = '{0}'", entityDt.BusinessPartnerID);
                                        vCustomer oCustomer = BusinessLayer.GetvCustomerList(filterC, ctx).FirstOrDefault();

                                        if (oCustomer.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            PatientPaymentDtInfo dtInfo = new PatientPaymentDtInfo();
                                            dtInfo.PaymentDetailID = paymentDetailID;

                                            if (hdnIsFinalisasiKlaimAfterARInvoice.Value == "0" && hdnIsFinalisasiKlaimBeforeARInvoiceAndSkipProcessKlaim.Value == "1")
                                            {
                                                dtInfo.GCClaimStatus = Constant.ClaimStatus.APPROVED;
                                                dtInfo.ClaimBy = AppSession.UserLogin.UserID;
                                                dtInfo.ClaimDate = DateTime.Now;

                                                dtInfo.GCFinalStatus = Constant.FinalStatus.OPEN;

                                                if (hdnIsGrouperAmountClaimDefaultZero.Value == "0")
                                                {
                                                    dtInfo.GrouperAmountClaim = entityDt.PaymentAmount;
                                                    dtInfo.GrouperAmountFinal = entityDt.PaymentAmount;
                                                }
                                            }
                                            else if (hdnIsFinalisasiKlaimAfterARInvoice.Value == "1" && hdnIsFinalisasiKlaimBeforeARInvoiceAndSkipProcessKlaim.Value == "1")
                                            {
                                                dtInfo.GCClaimStatus = Constant.ClaimStatus.APPROVED;
                                                dtInfo.ClaimBy = AppSession.UserLogin.UserID;
                                                dtInfo.ClaimDate = DateTime.Now;

                                                if (hdnIsGrouperAmountClaimDefaultZero.Value == "0")
                                                {
                                                    dtInfo.GrouperAmountClaim = entityDt.PaymentAmount;
                                                    dtInfo.GrouperAmountFinal = entityDt.PaymentAmount;
                                                }
                                            }
                                            else
                                            {
                                                dtInfo.GCClaimStatus = Constant.ClaimStatus.OPEN;

                                                if (hdnIsGrouperAmountClaimDefaultZero.Value == "0")
                                                {
                                                    dtInfo.GrouperAmountClaim = entityDt.PaymentAmount;
                                                }
                                            }

                                            dtInfo.SequenceNo = Convert.ToInt32(visit.RegistrationDate.ToString("dd"));
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            paymentDtInfoDao.Insert(dtInfo);
                                        }
                                        else
                                        {
                                            PatientPaymentDtInfo dtInfo = new PatientPaymentDtInfo();
                                            dtInfo.PaymentDetailID = paymentDetailID;
                                            dtInfo.GCClaimStatus = Constant.ClaimStatus.APPROVED;
                                            dtInfo.GCFinalStatus = Constant.FinalStatus.APPROVED;
                                            dtInfo.GrouperAmountClaim = dtInfo.GrouperAmountFinal = entityDt.PaymentAmount;
                                            dtInfo.ClaimBy = dtInfo.FinalBy = AppSession.UserLogin.UserID;
                                            dtInfo.ClaimDate = dtInfo.FinalDate = DateTime.Now;
                                            dtInfo.SequenceNo = Convert.ToInt32(visit.RegistrationDate.ToString("dd"));
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            paymentDtInfoDao.Insert(dtInfo);
                                        }
                                    }
                                    else
                                    {
                                        PatientPaymentDtInfo dtInfo = new PatientPaymentDtInfo();
                                        dtInfo.PaymentDetailID = paymentDetailID;
                                        dtInfo.GCClaimStatus = Constant.ClaimStatus.APPROVED;
                                        dtInfo.GCFinalStatus = Constant.FinalStatus.APPROVED;
                                        dtInfo.GrouperAmountClaim = dtInfo.GrouperAmountFinal = entityDt.PaymentAmount;
                                        dtInfo.ClaimBy = dtInfo.FinalBy = AppSession.UserLogin.UserID;
                                        dtInfo.ClaimDate = dtInfo.FinalDate = DateTime.Now;
                                        dtInfo.SequenceNo = Convert.ToInt32(visit.RegistrationDate.ToString("dd"));
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        paymentDtInfoDao.Insert(dtInfo);
                                    }
                                }
                                #endregion

                                #region Update Billing
                                decimal totalPaymentAmount = entityHd.TotalPaymentAmount;
                                string oldBillStatus = null;

                                oldBillStatus = patientBill.GCTransactionStatus;

                                patientBill.PaymentID = entityHd.PaymentID;

                                PatientBillPayment patientBillPayment = new PatientBillPayment();
                                patientBillPayment.PaymentID = entityHd.PaymentID;
                                patientBillPayment.PatientBillingID = patientBill.PatientBillingID;

                                if (entityHd.GCPaymentType == Constant.PaymentType.AR_PAYER)
                                {
                                    decimal totalRemainingOthers = patientBill.PayerRemainingAmount;
                                    if (patientBill.PayerRemainingAmount != 0)
                                    {
                                        if (patientBill.PayerRemainingAmount > 0 && totalRemainingOthers > 0)
                                        {
                                            if (patientBill.PayerRemainingAmount > totalPaymentAmount)
                                            {
                                                patientBill.TotalPayerPaymentAmount += totalPaymentAmount;
                                                patientBillPayment.PayerPaymentAmount = totalPaymentAmount;
                                                totalPaymentAmount = 0;
                                            }
                                            else
                                            {
                                                decimal payerRemainingAmount = patientBill.PayerRemainingAmount;
                                                patientBill.TotalPayerPaymentAmount += payerRemainingAmount;
                                                patientBillPayment.PayerPaymentAmount = payerRemainingAmount;
                                                totalPaymentAmount = totalPaymentAmount - patientBill.TotalPayerPaymentAmount;
                                            }
                                        }
                                        else if (patientBill.PayerRemainingAmount > 0 && totalRemainingOthers < 0)
                                        {
                                            if (patientBill.PayerRemainingAmount > (totalPaymentAmount - totalRemainingOthers))
                                            {
                                                patientBill.TotalPayerPaymentAmount += (totalPaymentAmount - totalRemainingOthers);
                                                patientBillPayment.PayerPaymentAmount = (totalPaymentAmount - totalRemainingOthers);
                                                totalPaymentAmount = 0;
                                            }
                                            else
                                            {
                                                decimal payerRemainingAmount = patientBill.PayerRemainingAmount;
                                                patientBill.TotalPayerPaymentAmount += payerRemainingAmount;
                                                patientBillPayment.PayerPaymentAmount = payerRemainingAmount;
                                                totalPaymentAmount = totalPaymentAmount - patientBill.TotalPayerPaymentAmount;
                                            }
                                        }
                                        else
                                        {
                                            if (patientBill.PayerRemainingAmount < totalPaymentAmount)
                                            {
                                                patientBill.TotalPayerPaymentAmount += totalPaymentAmount;
                                                patientBillPayment.PayerPaymentAmount = totalPaymentAmount;
                                                totalPaymentAmount = 0;
                                            }
                                            else
                                            {
                                                decimal payerRemainingAmount = patientBill.PayerRemainingAmount;
                                                patientBill.TotalPayerPaymentAmount += payerRemainingAmount;
                                                patientBillPayment.PayerPaymentAmount = payerRemainingAmount;
                                                totalPaymentAmount = totalPaymentAmount + patientBill.TotalPayerPaymentAmount;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        patientBill.TotalPayerPaymentAmount += 0;
                                        patientBillPayment.PayerPaymentAmount = 0;
                                    }
                                }
                                else if (entityHd.GCPaymentType == Constant.PaymentType.AR_PATIENT)
                                {
                                    if (patientBill.PatientRemainingAmount != 0)
                                    {
                                        if (totalPaymentAmount > patientBill.PatientRemainingAmount)
                                        {
                                            patientBillPayment.PatientPaymentAmount = patientBill.PatientRemainingAmount;
                                            patientBill.TotalPatientPaymentAmount += patientBillPayment.PatientPaymentAmount;

                                            totalPaymentAmount = totalPaymentAmount - patientBillPayment.PatientPaymentAmount;
                                        }
                                        else
                                        {
                                            patientBill.TotalPatientPaymentAmount += totalPaymentAmount;
                                            patientBillPayment.PatientPaymentAmount = totalPaymentAmount;

                                            totalPaymentAmount = totalPaymentAmount - totalPaymentAmount;
                                        }
                                    }
                                }
                                else
                                {
                                    if (entityHd.GCPaymentType == Constant.PaymentType.SETTLEMENT)
                                    {
                                        if (patientBill.PatientRemainingAmount != 0)
                                        {
                                            patientBillPayment.PatientPaymentAmount += patientBill.PatientRemainingAmount + entityHd.PatientRoundingAmount;
                                            patientBill.TotalPatientPaymentAmount += patientBillPayment.PatientPaymentAmount - entityHd.PatientRoundingAmount;

                                            if (patientBillPayment.PatientPaymentAmount != 0)
                                            {
                                                patientBillPayment.PatientRoundingAmount = entityHd.PatientRoundingAmount;
                                            }

                                            totalPaymentAmount = totalPaymentAmount - patientBillPayment.PatientPaymentAmount;
                                        }
                                    }
                                    else if (patientBill.PatientRemainingAmount < totalPaymentAmount)
                                    {
                                        totalPaymentAmount -= patientBill.PatientRemainingAmount;
                                        patientBillPayment.PatientPaymentAmount = patientBill.PatientRemainingAmount;
                                        patientBill.TotalPatientPaymentAmount = (patientBill.TotalPatientAmount - patientBill.PatientDiscountAmount);
                                    }
                                    else
                                    {
                                        patientBill.TotalPatientPaymentAmount += totalPaymentAmount;
                                        patientBillPayment.PatientPaymentAmount = totalPaymentAmount;
                                        totalPaymentAmount = 0;
                                    }
                                }


                                if (patientBill.RemainingAmount == 0)
                                {
                                    patientBill.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                                    foreach (PatientChargesHd patientChargesHd in lstPatientChargesHd)
                                    {
                                        patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                                        patientChargesHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        patientChargesHdDao.Update(patientChargesHd);
                                    }
                                }

                                if (oldBillStatus != patientBill.GCTransactionStatus)
                                {
                                    patientBill.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    patientBill.LastUpdatedDate = DateTime.Now;
                                }
                                patientBillDao.Update(patientBill);

                                patientBillPaymentDao.Insert(patientBillPayment);
                                #endregion
                            }
                        }
                    }

                    if (result)
                    {
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(errMessageRegTransfer))
                        {
                            errMessage = string.Format("Registrasi {0} sudah di tutup/batal, tagihan sudah di transfer ke rawat inap", errMessageRegTransfer);
                        }

                        if (!String.IsNullOrEmpty(errMessageRegPendingTestOrder))
                        {
                            if (String.IsNullOrEmpty(errMessage))
                            {
                                errMessage = string.Format("Registrasi {0} Masih Memiliki Order Penunjang Medis Yang Belum Direalisasi", errMessageRegPendingTestOrder);
                            }
                            else
                            {
                                errMessage += string.Format("<BR>Registrasi {0} Masih Memiliki Order Penunjang Medis Yang Belum Direalisasi", errMessageRegPendingTestOrder);
                            }
                        }

                        if (!String.IsNullOrEmpty(errMessageRegPendingServiceOrder))
                        {
                            if (String.IsNullOrEmpty(errMessage))
                            {
                                errMessage = string.Format("Registrasi {0} Masih Memiliki Order Pelayanan Yang Belum Direalisasi", errMessageRegPendingServiceOrder);
                            }
                            else
                            {
                                errMessage += string.Format("<BR>Registrasi {0} Masih Memiliki Order Pelayanan Yang Belum Direalisasi", errMessageRegPendingServiceOrder);
                            }
                        }
                        result = false;
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

            return result;
        }
    }
}
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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientBillSummaryPayment : BasePageTrx
    {
        protected string GetErrorMsgCashBackAmount()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_CASH_BACK_AMOUNT_VALIDATION);
        }
        protected string GetErrorMsgCashBackAmountCustom()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_CASH_BACK_AMOUNT_VALIDATION_CUSTOM);
        }
        public override string OnGetMenuCode()
        {
            if (hdnRequestID.Value == "KASIR")
            {
                switch (hdnDepartmentID.Value)
                {
                    case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.BILL_SUMMARY_PAYMENT_CASHIER;
                    case Constant.Facility.MEDICAL_CHECKUP: return Constant.MenuCode.MedicalCheckup.BILL_SUMMARY_PAYMENT_CASHIER;
                    case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.BILL_SUMMARY_PAYMENT_CASHIER;
                    case Constant.Facility.PHARMACY: return Constant.MenuCode.Pharmacy.BILL_SUMMARY_PAYMENT_CASHIER;
                    case Constant.Facility.DIAGNOSTIC:
                        if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                            return Constant.MenuCode.Laboratory.BILL_SUMMARY_PAYMENT_CASHIER;
                        else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                            return Constant.MenuCode.Imaging.BILL_SUMMARY_PAYMENT_CASHIER;
                        else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                            return Constant.MenuCode.Radiotheraphy.BILL_SUMMARY_PAYMENT_CASHIER;
                        return Constant.MenuCode.MedicalDiagnostic.BILL_SUMMARY_PAYMENT_CASHIER;
                    default: return Constant.MenuCode.Outpatient.BILL_SUMMARY_PAYMENT_CASHIER;
                }
            }
            else if (hdnRequestID.Value == "PIUTANG")
            {
                switch (hdnDepartmentID.Value)
                {
                    case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.BILL_SUMMARY_PAYMENT_AR;
                    case Constant.Facility.MEDICAL_CHECKUP: return Constant.MenuCode.MedicalCheckup.BILL_SUMMARY_PAYMENT_AR;
                    case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.BILL_SUMMARY_PAYMENT_AR;
                    case Constant.Facility.PHARMACY: return Constant.MenuCode.Pharmacy.BILL_SUMMARY_PAYMENT_AR;
                    case Constant.Facility.DIAGNOSTIC:
                        if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                            return Constant.MenuCode.Laboratory.BILL_SUMMARY_PAYMENT_AR;
                        else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                            return Constant.MenuCode.Imaging.BILL_SUMMARY_PAYMENT_AR;
                        else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                            return Constant.MenuCode.Radiotheraphy.BILL_SUMMARY_PAYMENT_AR;
                        return Constant.MenuCode.MedicalDiagnostic.BILL_SUMMARY_PAYMENT_AR;
                    default: return Constant.MenuCode.Outpatient.BILL_SUMMARY_PAYMENT_AR;
                }
            }
            else
            {
                switch (hdnDepartmentID.Value)
                {
                    case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.BILL_SUMMARY_PAYMENT;
                    case Constant.Facility.MEDICAL_CHECKUP: return Constant.MenuCode.MedicalCheckup.BILL_SUMMARY_PAYMENT;
                    case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.BILL_SUMMARY_PAYMENT;
                    case Constant.Facility.PHARMACY: return Constant.MenuCode.Pharmacy.BILL_SUMMARY_PAYMENT;
                    case Constant.Facility.DIAGNOSTIC:
                        if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                            return Constant.MenuCode.Laboratory.BILL_SUMMARY_PAYMENT;
                        else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                            return Constant.MenuCode.Imaging.BILL_SUMMARY_PAYMENT;
                        else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                            return Constant.MenuCode.Radiotheraphy.BILL_SUMMARY_PAYMENT;
                        return Constant.MenuCode.MedicalDiagnostic.BILL_SUMMARY_PAYMENT;
                    default: return Constant.MenuCode.Outpatient.BILL_SUMMARY_PAYMENT;
                }
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected string GetDownPaymentCaption()
        {
            return "Sisa " + hdnCaptionDownPayment.Value;
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
        }

        protected override void InitializeDataControl()
        {
            if (!Page.IsPostBack)
            {
                if (Page.Request.QueryString["id"] != null)
                {
                    hdnRequestID.Value = Page.Request.QueryString["id"];
                }
                else
                {
                    hdnRequestID.Value = "ALL";
                }

                hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();

                vRegistration3 entity = BusinessLayer.GetvRegistration3List(string.Format("RegistrationID = {0}", hdnRegistrationID.Value)).FirstOrDefault();
                hdnDefaultBusinessPartnerID.Value = entity.BusinessPartnerID.ToString();
                hdnDepartmentID.Value = entity.DepartmentID;
                hdnGCRegistrationStatus.Value = entity.GCRegistrationStatus;
                hdnPatientName.Value = entity.PatientName;
                hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
                hdnPatientGender.Value = entity.GCGender;
                hdnGCCustomerType.Value = entity.GCCustomerType;

                #region EDC
                hdnHealthcareID.Value = AppSession.UserLogin.HealthcareID;
                hdnRegistrationNo.Value = entity.RegistrationNo;
                hdnMRN.Value = entity.MRN.ToString();
                hdnMedicalNo.Value = entity.MedicalNo;
                hdnDateOfBirth.Value = entity.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT_112);
                hdnPaymentNo.Value = "0";
                #endregion

                #region Outstanding Charges to Billing

                //Charges
                int charges = 0;

                List<PatientChargesHd> entityChargesHd = BusinessLayer.GetPatientChargesHdList(string.Format(
                                                            "VisitID = {0} AND GCTransactionStatus != '{1}'",
                                                            AppSession.RegisteredPatient.VisitID,
                                                            Constant.TransactionStatus.VOID));
                foreach (PatientChargesHd vch in entityChargesHd.Where(a => a.PatientBillingID == 0 || a.PatientBillingID == null))
                {
                    charges += 1;
                }

                if (charges > 0)
                {
                    tblInfoOutstandingTransfer.Style.Remove("display");
                    trCharges.Style.Remove("display");
                }
                else
                {
                    trCharges.Style.Add("display", "none");
                }

                //Order
                vRegistrationOutstandingInfo lstInfo = BusinessLayer.GetvRegistrationOutstandingInfoList(string.Format("RegistrationID = {0} AND GCRegistrationStatus != '{1}'", AppSession.RegisteredPatient.RegistrationID, Constant.VisitStatus.CANCELLED)).FirstOrDefault();
                bool outstanding = lstInfo.ServiceOrder + lstInfo.PrescriptionOrder + lstInfo.PrescriptionReturnOrder + lstInfo.LaboratoriumOrder + lstInfo.RadiologiOrder + lstInfo.OtherOrder > 0;

                if (outstanding == true)
                {
                    tblInfoOutstandingTransfer.Style.Remove("display");
                    trOrder.Style.Remove("display");
                }
                else
                {
                    trOrder.Style.Add("display", "none");
                }

                //All
                if (charges > 0 || outstanding == true)
                {
                    tblInfoOutstandingTransfer.Style.Remove("display");
                }
                else
                {
                    tblInfoOutstandingTransfer.Style.Add("display", "none");
                }

                #endregion

                string moduleName = Helper.GetModuleName();
                string ModuleID = Helper.GetModuleID(moduleName);
                GetUserMenuAccess menu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault();
                string CRUDMode = menu.CRUDMode;
                hdnIsAllowVoid.Value = CRUDMode.Contains("X") ? "1" : "0";

                InitializeControlProperties();

                hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;


                List<SettingParameterDt> lstSetVarDt = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                                            "ParameterCode IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}')",
                                                                            Constant.SettingParameter.FN_IS_SETTLEMENT_ALLOW_WITH_ARPATIENT, //0
                                                                            Constant.SettingParameter.FN_IS_ALLOW_BACKDATED_PAYMENT, //1
                                                                            Constant.SettingParameter.FN_IS_GROUPER_AMOUNT_CLAIM_DEFAULT_ZERO, //2
                                                                            Constant.SettingParameter.FN_TGL_AR_PATIENT_SESUAI_TGL_PILIH, //3
                                                                            Constant.SettingParameter.FN_CAPTION_DOWN_PAYMENT_IN_MENU_PATIENT_PAYMENT, //4
                                                                            Constant.SettingParameter.FN_IS_ALLOW_ROUNDING_AMOUNT, //5
                                                                            Constant.SettingParameter.FN_NILAI_PEMBULATAN_TAGIHAN, //6
                                                                            Constant.SettingParameter.FN_PEMBULATAN_TAGIHAN_KE_ATAS, //7
                                                                            Constant.SettingParameter.FN_UBAH_NILAI_PEMBUALATAN_PEMBAYARAN, //8
                                                                            Constant.SettingParameter.FN_TIPE_CUSTOMER_BPJS, //9
                                                                            Constant.SettingParameter.LB_IS_SEND_TO_LIS_AFTER_SAVE_PAYMENT, //10
                                                                            Constant.SettingParameter.SA0171, //11
                                                                            Constant.SettingParameter.SA_EDC_BRIDGING, //12
                                                                            Constant.SettingParameter.SA_IS_BRIDGING_TO_PAYMENT_GATEWAY, //13
                                                                            Constant.SettingParameter.SA_IS_BRIDGING_MASPION, //14
                                                                            Constant.SettingParameter.FN_IS_ALLOW_BACKDATED_PAYMENT_PERSONAL_AR, //15
                                                                            Constant.SettingParameter.FN_IS_CLAIM_FINAL_AFTER_AR_INVOICE, //16
                                                                            Constant.SettingParameter.FN_IS_CLAIM_FINAL_BEFORE_AR_INVOICE_AND_SKIP_CLAIM //17
                                                                        ));

                hdnIsSettlementAllowWithARPatient.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_SETTLEMENT_ALLOW_WITH_ARPATIENT).FirstOrDefault().ParameterValue;

                hdnIsAllowBackDatePayment.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_ALLOW_BACKDATED_PAYMENT).FirstOrDefault().ParameterValue;
                hdnIsAllowBackDatePaymentPersonalAR.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_ALLOW_BACKDATED_PAYMENT_PERSONAL_AR).FirstOrDefault().ParameterValue;

                hdnIsGrouperAmountClaimDefaultZero.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_GROUPER_AMOUNT_CLAIM_DEFAULT_ZERO).FirstOrDefault().ParameterValue;

                hdnTanggalPiutangPribadi.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_TGL_AR_PATIENT_SESUAI_TGL_PILIH).FirstOrDefault().ParameterValue;

                hdnCaptionDownPayment.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_CAPTION_DOWN_PAYMENT_IN_MENU_PATIENT_PAYMENT).FirstOrDefault().ParameterValue;

                hdnIsAllowRoundingAmount.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_ALLOW_ROUNDING_AMOUNT).FirstOrDefault().ParameterValue;
                hdnNilaiPembulatan.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_NILAI_PEMBULATAN_TAGIHAN).FirstOrDefault().ParameterValue;
                hdnPembulatanKeAtas.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_PEMBULATAN_TAGIHAN_KE_ATAS).FirstOrDefault().ParameterValue;
                hdnUbahNilaiPembutalan.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_UBAH_NILAI_PEMBUALATAN_PEMBAYARAN).FirstOrDefault().ParameterValue;

                hdnIsCustomerTypeBPJS.Value = hdnGCCustomerType.Value == lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_TIPE_CUSTOMER_BPJS).FirstOrDefault().ParameterValue ? "1" : "0";
                hdnIsSendToLISAfterSavePayment.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.LB_IS_SEND_TO_LIS_AFTER_SAVE_PAYMENT).FirstOrDefault().ParameterValue;
                hdnCardDetailInformationMandatory.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.SA0171).FirstOrDefault().ParameterValue;

                hdnIsFinalisasiKlaimAfterARInvoice.Value = lstSetVarDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_IS_CLAIM_FINAL_AFTER_AR_INVOICE).ParameterValue;
                hdnIsFinalisasiKlaimBeforeARInvoiceAndSkipProcessKlaim.Value = lstSetVarDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_IS_CLAIM_FINAL_BEFORE_AR_INVOICE_AND_SKIP_CLAIM).ParameterValue;

                string SA_EDC_BRIDGING = lstSetVarDt.Where(p => p.ParameterCode == Constant.SettingParameter.SA_EDC_BRIDGING).FirstOrDefault().ParameterValue;
                hdnIsBridgingEdc.Value = SA_EDC_BRIDGING;
                hdnIsBridgingToPaymentGateway.Value = lstSetVarDt.Where(p => p.ParameterCode == Constant.SettingParameter.SA_IS_BRIDGING_TO_PAYMENT_GATEWAY).FirstOrDefault().ParameterValue;
                if (SA_EDC_BRIDGING == "1")
                {
                    List<PrinterLocation> lstEDCMachine = BusinessLayer.GetPrinterLocationList(string.Format("GCPrinterType IN ('{0}') AND IsDeleted=0", Constant.DirectPrintType.EDC_IP_CONFIGURATION));
                    Methods.SetComboBoxField<PrinterLocation>(cboLocationEDC, lstEDCMachine, "PrinterName", "IPAddress");
                }

                hdnIsBridgingToMaspion.Value = lstSetVarDt.Where(p => p.ParameterCode == Constant.SettingParameter.SA_IS_BRIDGING_MASPION).FirstOrDefault().ParameterValue;
            }
        }

        private void InitializeControlProperties()
        {
            string filterDeposit = string.Format("MRN = {0}", AppSession.RegisteredPatient.MRN);
            PatientDepositBalance pDepositBalance = BusinessLayer.GetPatientDepositBalanceList(filterDeposit).LastOrDefault();
            if (pDepositBalance != null)
            {
                hdnDepositBalanceEnd.Value = pDepositBalance.BalanceEND.ToString();
            }

            hdnCreditCardFeeFilterExpression.Value = string.Format("HealthcareID = '{0}' AND GCCardType = '[GCCardType]' AND GCCardProvider = '[GCCardProvider]' AND EDCMachineID = [EDCMachineID]", AppSession.UserLogin.HealthcareID);

            List<EDCMachine> lstEDCMachine = BusinessLayer.GetEDCMachineList("IsDeleted = 0");
            Methods.SetComboBoxField<EDCMachine>(cboEDCMachine, lstEDCMachine, "EDCMachineName", "EDCMachineID");
            cboEDCMachine.SelectedIndex = 0;

            //List<Bank> lstBank = BusinessLayer.GetBankList(string.Format("IsDeleted = 0 AND (GCBankType = '{0}' OR GCBankType IS NULL)", Constant.BankType.BANK_KASIR));
            //Methods.SetComboBoxField<Bank>(cboBank, lstBank, "BankName", "BankID");
            //cboBank.SelectedIndex = 0;

            List<vRegistrationPayer> lstEntityPayer = BusinessLayer.GetvRegistrationPayerList(string.Format("RegistrationID = {0} AND IsDeleted = 0", hdnRegistrationID.Value));
            Methods.SetComboBoxField<vRegistrationPayer>(cboBusinessPartner, lstEntityPayer, "BusinessPartnerName", "BusinessPartnerID");
            cboBusinessPartner.SelectedIndex = 0;

            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(String.Format(
                                            "ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}') AND IsActive = 1 AND IsDeleted = 0",
                                            Constant.StandardCode.CARD_TYPE, //0
                                            Constant.StandardCode.PAYMENT_METHOD, //1
                                            Constant.StandardCode.PAYMENT_TYPE, //2
                                            Constant.StandardCode.CARD_PROVIDER, //3
                                            Constant.StandardCode.CASHIER_GROUP, //4
                                            Constant.StandardCode.SHIFT //5
                                        ));

            Methods.SetComboBoxField<StandardCode>(cboCardType, lstSc.Where(p => p.ParentID == Constant.StandardCode.CARD_TYPE).ToList(), "StandardCodeName", "StandardCodeID");

            StandardCode entityDPOut = lstSc.FirstOrDefault(p => p.StandardCodeID == Constant.PaymentMethod.DOWN_PAYMENT);
            hdnCboDPOut.Value = string.Format("{0}|{1}", entityDPOut.StandardCodeID, entityDPOut.StandardCodeName);

            Methods.SetComboBoxField<StandardCode>(cboPaymentMethod, lstSc.Where(p => p.ParentID == Constant.StandardCode.PAYMENT_METHOD && p.StandardCodeID != Constant.PaymentMethod.CREDIT && p.StandardCodeID != Constant.PaymentMethod.PAYMENT_RETURN && p.StandardCodeID != Constant.PaymentMethod.DOWN_PAYMENT).ToList(), "StandardCodeName", "StandardCodeID");

            string downPaymentIsAllowAll = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_DOWN_PAYMENT_IS_ALLOW_ALL).ParameterValue;

            if (hdnRequestID.Value == "KASIR")
            {
                if (downPaymentIsAllowAll == "0")
                {
                    if (hdnDepartmentID.Value != Constant.Facility.INPATIENT)
                    {
                        Methods.SetComboBoxField<StandardCode>(cboPaymentType, lstSc.Where(p => p.ParentID == Constant.StandardCode.PAYMENT_TYPE
                                                                                                && p.StandardCodeID != Constant.PaymentType.DOWN_PAYMENT
                                                                                                && p.StandardCodeID != Constant.PaymentType.AR_PATIENT
                                                                                                && p.StandardCodeID != Constant.PaymentType.AR_PAYER
                                                                                                && p.StandardCodeID != Constant.PaymentType.CUSTOM
                                                                                            ).ToList(), "StandardCodeName", "StandardCodeID");
                    }
                    else
                    {
                        Methods.SetComboBoxField<StandardCode>(cboPaymentType, lstSc.Where(p => p.ParentID == Constant.StandardCode.PAYMENT_TYPE
                                                                                                && p.StandardCodeID != Constant.PaymentType.AR_PATIENT
                                                                                                && p.StandardCodeID != Constant.PaymentType.AR_PAYER
                                                                                                && p.StandardCodeID != Constant.PaymentType.CUSTOM
                                                                                            ).ToList(), "StandardCodeName", "StandardCodeID");
                    }
                }
                else
                {
                    Methods.SetComboBoxField<StandardCode>(cboPaymentType, lstSc.Where(p => p.ParentID == Constant.StandardCode.PAYMENT_TYPE
                                                                                                && p.StandardCodeID != Constant.PaymentType.AR_PATIENT
                                                                                                && p.StandardCodeID != Constant.PaymentType.AR_PAYER
                                                                                                && p.StandardCodeID != Constant.PaymentType.CUSTOM
                                                                                            ).ToList(), "StandardCodeName", "StandardCodeID");
                }
            }
            else if (hdnRequestID.Value == "PIUTANG")
            {
                Methods.SetComboBoxField<StandardCode>(cboPaymentType, lstSc.Where(p => p.ParentID == Constant.StandardCode.PAYMENT_TYPE
                                                                                        && (
                                                                                            p.StandardCodeID == Constant.PaymentType.AR_PATIENT
                                                                                            || p.StandardCodeID == Constant.PaymentType.AR_PAYER
                                                                                        )
                                                                                    ).ToList(), "StandardCodeName", "StandardCodeID");
            }
            else
            {
                if (downPaymentIsAllowAll == "0")
                {
                    if (hdnDepartmentID.Value != Constant.Facility.INPATIENT)
                    {
                        Methods.SetComboBoxField<StandardCode>(cboPaymentType, lstSc.Where(p => p.ParentID == Constant.StandardCode.PAYMENT_TYPE && p.StandardCodeID != Constant.PaymentType.DOWN_PAYMENT).ToList(), "StandardCodeName", "StandardCodeID");
                    }
                    else
                    {
                        Methods.SetComboBoxField<StandardCode>(cboPaymentType, lstSc.Where(p => p.ParentID == Constant.StandardCode.PAYMENT_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
                    }
                }
                else
                {
                    Methods.SetComboBoxField<StandardCode>(cboPaymentType, lstSc.Where(p => p.ParentID == Constant.StandardCode.PAYMENT_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
                }
            }

            Methods.SetComboBoxField<StandardCode>(cboCardProvider, lstSc.Where(p => p.ParentID == Constant.StandardCode.CARD_PROVIDER).ToList(), "StandardCodeName", "StandardCodeID");
            List<StandardCode> lstShift = lstSc.Where(p => p.ParentID == Constant.StandardCode.SHIFT).ToList();
            Methods.SetComboBoxField<StandardCode>(cboShift, lstShift, "StandardCodeName", "StandardCodeID");

            hdnListShift.Value = string.Join("|", lstShift.Select(p => string.Format("{0},{1}", p.StandardCodeID, p.TagProperty)).ToList());

            hdnARText.Value = lstSc.FirstOrDefault(p => p.StandardCodeID == Constant.PaymentMethod.CREDIT).StandardCodeName;
            hdnPaymentReturnText.Value = "Pengembalian Pembayaran"; //lstSc.FirstOrDefault(p => p.StandardCodeID == Constant.PaymentMethod.PAYMENT_RETURN).StandardCodeName;

            hdnCaptionUangMukaKeluar.Value = lstSc.FirstOrDefault(p => p.StandardCodeID == Constant.PaymentMethod.DOWN_PAYMENT).StandardCodeName;

            cboPaymentMethod.SelectedIndex = 0;
            cboCardType.SelectedIndex = 0;

            if (AppSession.IsCashier && AppSession.CashierGroup != null && AppSession.CashierGroup != "")
            {
                Methods.SetComboBoxField<StandardCode>(cboCashierGroup, lstSc.Where(p => p.ParentID == Constant.StandardCode.CASHIER_GROUP && p.StandardCodeID == AppSession.CashierGroup).ToList(), "StandardCodeName", "StandardCodeID", DropDownStyle.DropDownList);
                cboCashierGroup.SelectedIndex = 0;
            }
            else
            {
                Methods.SetComboBoxField<StandardCode>(cboCashierGroup, lstSc.Where(p => p.ParentID == Constant.StandardCode.CASHIER_GROUP).ToList(), "StandardCodeName", "StandardCodeID", DropDownStyle.DropDownList);
                cboCashierGroup.SelectedIndex = 0;
            }

            cboCardDateMonth.DataSource = Enumerable.Range(1, 12).Select(a => new
            {
                MonthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(a),
                MonthNumber = a
            });
            cboCardDateMonth.TextField = "MonthName";
            cboCardDateMonth.ValueField = "MonthNumber";
            cboCardDateMonth.EnableCallbackMode = false;
            cboCardDateMonth.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
            cboCardDateMonth.DropDownStyle = DropDownStyle.DropDownList;
            cboCardDateMonth.DataBind();

            cboCardDateYear.DataSource = Enumerable.Range(DateTime.Now.Year, 10);
            cboCardDateYear.EnableCallbackMode = false;
            cboCardDateYear.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
            cboCardDateYear.DropDownStyle = DropDownStyle.DropDownList;
            cboCardDateYear.DataBind();

            OnAddRecord();
        }

        private void SetGCShiftValue()
        {
            string timeNow = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            string[] arrListShift = hdnListShift.Value.Split('|');
            foreach (string listShift in arrListShift)
            {
                string[] temp = listShift.Split(',');
                string[] shiftTime = temp[1].Split(';');
                if (string.Compare(shiftTime[0], shiftTime[1]) < 0)
                {
                    if (string.Compare(shiftTime[0], timeNow) <= 0 && string.Compare(shiftTime[1], timeNow) >= 0)
                    {
                        hdnDefaultShift.Value = temp[0];
                        cboShift.Value = temp[0];
                        break;
                    }
                }
                else
                {
                    if (string.Compare(shiftTime[0], timeNow) >= 0 || string.Compare(shiftTime[1], timeNow) <= 0)
                    {
                        hdnDefaultShift.Value = temp[0];
                        cboShift.Value = temp[0];
                        break;
                    }
                }
            }
        }

        protected override void SetControlProperties()
        {
            ListView lvwBilling = (ListView)ddeBillingNo.FindControl("lvwBilling");
            List<vPatientBill> lst = BusinessLayer.GetvPatientBillList(string.Format("RegistrationID = {0} AND GCTransactionStatus = '{1}'", hdnRegistrationID.Value, Constant.TransactionStatus.OPEN));
            lvwBilling.DataSource = lst;
            lvwBilling.DataBind();

            Helper.SetControlEntrySetting(cboCardType, new ControlEntrySetting(true, true, true), "vgCardInformation");
            Helper.SetControlEntrySetting(cboCardProvider, new ControlEntrySetting(true, true, true), "vgCardInformation");
            Helper.SetControlEntrySetting(txtCardNumber1, new ControlEntrySetting(true, true, false), "vgCardInformation");
            Helper.SetControlEntrySetting(txtCardNumber4, new ControlEntrySetting(true, true, false), "vgCardInformation");
            Helper.SetControlEntrySetting(txtHolderName, new ControlEntrySetting(true, true, false), "vgCardInformation");
            Helper.SetControlEntrySetting(cboCardDateMonth, new ControlEntrySetting(true, true, false), "vgCardInformation");
            Helper.SetControlEntrySetting(cboCardDateYear, new ControlEntrySetting(true, true, false), "vgCardInformation");
            Helper.SetControlEntrySetting(txtBatchNo, new ControlEntrySetting(true, true, false), "vgCardInformation");
            Helper.SetControlEntrySetting(txtTraceNo, new ControlEntrySetting(true, true, false), "vgCardInformation");
            Helper.SetControlEntrySetting(txtReferenceNo, new ControlEntrySetting(true, true, false), "vgCardInformation");
            Helper.SetControlEntrySetting(txtApprovalCode, new ControlEntrySetting(true, true, false), "vgCardInformation");
            Helper.SetControlEntrySetting(txtTerminalID, new ControlEntrySetting(true, true, false), "vgCardInformation");
            Helper.SetControlEntrySetting(txtPaymentAmount, new ControlEntrySetting(true, true, false), "vgCardInformation");

            txtCardNumber1.Text = "XXXX";
            txtCardNumber4.Text = "XXXX";

            #region Set Card Detail Mandatory
            string[] cardDetail = hdnCardDetailInformationMandatory.Value.Split('|');
            const string noKartu = "1";
            const string pemegangKartu = "2";
            const string masaBerlaku = "3";
            const string jumlah = "4";
            const string terminal = "5";
            const string noBatch = "6";
            const string noTrace = "7";
            const string referenceNo = "8";
            const string approvalCode = "9";

            for (int i = 0; i < cardDetail.Length; i++)
            {
                switch (cardDetail[i])
                {
                    case noKartu:
                        lblNoKartu.Attributes.Add("class", "lblMandatory");
                        Helper.SetControlEntrySetting(txtCardNumber1, new ControlEntrySetting(true, true, true), "vgCardInformation");
                        Helper.SetControlEntrySetting(txtCardNumber4, new ControlEntrySetting(true, true, true), "vgCardInformation");
                        txtCardNumber1.Text = string.Empty;
                        txtCardNumber4.Text = string.Empty;
                        break;
                    case pemegangKartu:
                        lblPemegangKartu.Attributes.Add("class", "lblMandatory");
                        Helper.SetControlEntrySetting(txtHolderName, new ControlEntrySetting(true, true, true), "vgCardInformation");
                        break;
                    case masaBerlaku:
                        lblMasaBerlaku.Attributes.Add("class", "lblMandatory");
                        Helper.SetControlEntrySetting(cboCardDateMonth, new ControlEntrySetting(true, true, true), "vgCardInformation");
                        Helper.SetControlEntrySetting(cboCardDateYear, new ControlEntrySetting(true, true, true), "vgCardInformation");
                        break;
                    case jumlah:
                        lblJumlah.Attributes.Add("class", "lblMandatory");
                        Helper.SetControlEntrySetting(txtPaymentAmount, new ControlEntrySetting(true, true, true), "vgCardInformation");
                        break;
                    case terminal:
                        lblTerminal.Attributes.Add("class", "lblMandatory");
                        Helper.SetControlEntrySetting(txtTerminalID, new ControlEntrySetting(true, true, true), "vgCardInformation");
                        break;
                    case noBatch:
                        lblNoBatch.Attributes.Add("class", "lblMandatory");
                        Helper.SetControlEntrySetting(txtBatchNo, new ControlEntrySetting(true, true, true), "vgCardInformation");
                        break;
                    case noTrace:
                        lblNoTrace.Attributes.Add("class", "lblMandatory");
                        Helper.SetControlEntrySetting(txtTraceNo, new ControlEntrySetting(true, true, true), "vgCardInformation");
                        break;
                    case referenceNo:
                        lblReferenceNo.Attributes.Add("class", "lblMandatory");
                        Helper.SetControlEntrySetting(txtReferenceNo, new ControlEntrySetting(true, true, true), "vgCardInformation");
                        break;
                    case approvalCode:
                        lblApprovalCode.Attributes.Add("class", "lblMandatory");
                        Helper.SetControlEntrySetting(txtApprovalCode, new ControlEntrySetting(true, true, true), "vgCardInformation");
                        break;
                    default:
                        break;
                }
            }
            #endregion
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnPaymentHdID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtPaymentNo, new ControlEntrySetting(true, false, false));

            if (hdnIsAllowBackDatePayment.Value == "1")
            {
                SetControlEntrySetting(txtPaymentDate, new ControlEntrySetting(true, false, true, Constant.DefaultValueEntry.DATE_NOW));
                SetControlEntrySetting(txtPaymentTime, new ControlEntrySetting(true, false, true, Constant.DefaultValueEntry.TIME_NOW));
            }
            else
            {
                SetControlEntrySetting(txtPaymentDate, new ControlEntrySetting(false, false, true, Constant.DefaultValueEntry.DATE_NOW));
                SetControlEntrySetting(txtPaymentTime, new ControlEntrySetting(false, false, true, Constant.DefaultValueEntry.TIME_NOW));
            }

            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, false, false));

            if (hdnRequestID.Value == "KASIR" || hdnRequestID.Value == "ALL")
            {
                SetControlEntrySetting(cboPaymentType, new ControlEntrySetting(true, false, true, Constant.PaymentType.SETTLEMENT));
            }
            else
            {
                SetControlEntrySetting(cboPaymentType, new ControlEntrySetting(true, false, true, Constant.PaymentType.AR_PAYER));
            }

            SetControlEntrySetting(txtBillingNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(ddeBillingNo, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtBillingTotal, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(txtBillingOriginalPatient, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtBillingOriginalPayer, new ControlEntrySetting(false, false, false));

            if (hdnIsAllowRoundingAmount.Value == "1")
            {
                if (hdnUbahNilaiPembutalan.Value == "1")
                {
                    SetControlEntrySetting(txtPatientRoundingAmount, new ControlEntrySetting(true, false, false));
                    SetControlEntrySetting(txtPayerRoundingAmount, new ControlEntrySetting(false, false, false));
                }
                else
                {
                    SetControlEntrySetting(txtPatientRoundingAmount, new ControlEntrySetting(false, false, false));
                    SetControlEntrySetting(txtPayerRoundingAmount, new ControlEntrySetting(false, false, false));
                }
            }
            else
            {
                SetControlEntrySetting(txtPatientRoundingAmount, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(txtPayerRoundingAmount, new ControlEntrySetting(false, false, false));
            }

            SetControlEntrySetting(txtBillingTotalPatient, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtBillingTotalPayer, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(txtPaymentTotalPatient, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtPaymentTotalPayer, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(txtCashbackAmount, new ControlEntrySetting(false, false, true));

            SetControlEntrySetting(cboCardType, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(cboCardProvider, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtCardNumber1, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtCardNumber4, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtHolderName, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(cboCardDateMonth, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(cboCardDateYear, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(cboShift, new ControlEntrySetting(true, false, true, hdnDefaultShift.Value));
            SetControlEntrySetting(cboCashierGroup, new ControlEntrySetting(true, false, true, cboCashierGroup.SelectedItem.Value));
        }

        public override void OnAddRecord()
        {
            ////List<PatientPaymentHd> lstPatientPaymentHd = BusinessLayer.GetPatientPaymentHdList(string.Format("RegistrationID = {0} AND GCPaymentType = '{1}' AND GCTransactionStatus != '{2}' AND TotalPaymentAmount > TotalPatientBillAmount", hdnRegistrationID.Value, Constant.PaymentType.DOWN_PAYMENT, Constant.TransactionStatus.VOID));

            List<PatientPaymentHd> lstPatientPaymentHd = BusinessLayer.GetPatientPaymentHdList(string.Format("RegistrationID = {0} AND GCPaymentType = '{1}' AND GCTransactionStatus != '{2}'", hdnRegistrationID.Value, Constant.PaymentType.DOWN_PAYMENT, Constant.TransactionStatus.VOID));
            decimal totalOutstandingDP = lstPatientPaymentHd.Sum(p => (p.TotalPaymentAmount - p.TotalPatientBillAmount));

            List<PatientPaymentDt> lstPatientPaymentDt = BusinessLayer.GetPatientPaymentDtList(string.Format("PaymentID IN (SELECT PaymentID FROM PatientPaymentHd WHERE RegistrationID = {0} AND GCTransactionStatus != '{1}') AND GCPaymentMethod = '{2}'", hdnRegistrationID.Value, Constant.TransactionStatus.VOID, "X035^006"));
            totalOutstandingDP -= lstPatientPaymentDt.Sum(p => p.PaymentAmount);

            hdnOutstandingDP.Value = totalOutstandingDP.ToString();
            txtOutstandingDP.Text = totalOutstandingDP.ToString("N");

            cboCashierGroup.SelectedIndex = 0;
            SetGCShiftValue();
        }

        #region Load Entity
        public override int OnGetRowCount()
        {
            string filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);
            return BusinessLayer.GetvPatientPaymentHdRowCount(filterExpression);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);
            vPatientPaymentHd entity = BusinessLayer.GetvPatientPaymentHd(filterExpression, PageIndex, "PaymentID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);
            PageIndex = BusinessLayer.GetvPatientPaymentHdRowIndex(filterExpression, keyValue, "PaymentID DESC");
            vPatientPaymentHd entity = BusinessLayer.GetvPatientPaymentHd(filterExpression, PageIndex, "PaymentID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vPatientPaymentHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            trOutstandingDP.Style.Add("display", "none");
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                watermarkText = entity.TransactionStatusWatermark;
            }
            hdnTransactionStatus.Value = entity.GCTransactionStatus;
            hdnPaymentHdID.Value = entity.PaymentID.ToString();
            txtPaymentNo.Text = entity.PaymentNo;
            txtPaymentDate.Text = entity.PaymentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPaymentTime.Text = entity.PaymentTime;
            txtSourcePaymentNo.Text = entity.SourcePaymentNo;
            txtRemarks.Text = entity.Remarks;
            txtBillingNo.Text = entity.BillingNo;
            cboPaymentType.Value = entity.GCPaymentType;
            cboCashierGroup.Value = entity.GCCashierGroup;
            cboShift.Value = entity.GCShift;

            //txtBillingOriginalPatient.Text = (entity.TotalPatientBillAmount - entity.PatientRoundingAmount).ToString();
            //txtBillingOriginalPayer.Text = (entity.TotalPayerBillAmount - entity.PayerRoundingAmount).ToString();

            txtBillingOriginalPatient.Text = (entity.TotalPatientBillAmount).ToString();
            txtBillingOriginalPayer.Text = (entity.TotalPayerBillAmount).ToString();

            txtPatientRoundingAmount.Text = entity.PatientRoundingAmount.ToString();
            txtPayerRoundingAmount.Text = entity.PayerRoundingAmount.ToString();

            //txtBillingTotal.Text = (entity.TotalBillAmount - entity.PatientRoundingAmount - entity.PayerRoundingAmount).ToString();

            txtBillingTotal.Text = (entity.TotalBillAmount).ToString();

            //txtBillingTotalPatient.Text = (entity.TotalPatientBillAmount).ToString();
            //txtBillingTotalPayer.Text = (entity.TotalPayerBillAmount).ToString();

            txtBillingTotalPatient.Text = (entity.TotalPatientBillAmount + entity.PatientRoundingAmount).ToString();
            txtBillingTotalPayer.Text = (entity.TotalPayerBillAmount + entity.PayerRoundingAmount).ToString();

            decimal totalPayer = 0;
            decimal totalPatient = 0;
            if (entity.GCPaymentType == Constant.PaymentType.AR_PAYER)
                totalPayer = entity.TotalPaymentAmount;
            else
                totalPatient = entity.TotalPaymentAmount;

            if (entity.GCPaymentType == Constant.PaymentType.DOWN_PAYMENT)
                hdnPaymentAllocation.Value = (entity.TotalPaymentAmount - entity.TotalPatientBillAmount).ToString();
            else
                hdnPaymentAllocation.Value = "0";

            txtPaymentTotalPatient.Text = totalPatient.ToString();
            txtPaymentTotalPayer.Text = totalPayer.ToString();

            txtCashbackAmount.Text = entity.CashBackAmount.ToString();

            BindGrdPaymentDetail();

            divCreatedBy.InnerHtml = entity.CreatedByUser;
            divCreatedDate.InnerHtml = entity.CreatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            divLastUpdatedBy.InnerHtml = entity.LastUpdatedByUser;
            if (entity.LastUpdatedDate != null && entity.LastUpdatedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
            {
                divLastUpdatedDate.InnerHtml = entity.LastUpdatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }

            if (entity.GCTransactionStatus == Constant.TransactionStatus.VOID)
            {
                divVoidBy.InnerHtml = entity.VoidByUser;
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

        private void BindGrdPaymentDetail()
        {
            List<vPatientPaymentDt> lstDt = BusinessLayer.GetvPatientPaymentDtList(string.Format("PaymentID = {0} AND IsDeleted = 0 ORDER BY PaymentDetailID", hdnPaymentHdID.Value));
            Decimal patientAmount = lstDt.Select(p => p.PaymentAmount).Sum();
            Decimal cardFeeAmount = lstDt.Select(p => p.CardFeeAmount).Sum();

            if (cboPaymentType.Value.ToString() == Constant.PaymentType.AR_PAYER)
            {
                lvwPaymentPayerDt.DataSource = lstDt;
                lvwPaymentPayerDt.DataBind();
                tdTotalPayerEdit.InnerHtml = patientAmount.ToString("N");
                tdLineTotalPayerEdit.InnerHtml = (patientAmount).ToString("N");
            }
            else
            {
                lvwPaymentDt.DataSource = lstDt;
                lvwPaymentDt.DataBind();
                tdTotalPatientEdit.InnerHtml = patientAmount.ToString("N");
                tdTotalCardFeeEdit.InnerHtml = cardFeeAmount.ToString("N");
                tdLineTotalEdit.InnerHtml = (patientAmount + cardFeeAmount).ToString("N");
                hdnTotalPaymentAmount.Value = patientAmount.ToString();
            }
        }

        protected void cbpPaymentDt_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGrdPaymentDetail();
        }

        protected void cbpPaymentPayerDt_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGrdPaymentDetail();
        }
        #endregion

        #region Save Entity

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            Registration entityReg = BusinessLayer.GetRegistration(AppSession.RegisteredPatient.RegistrationID);

            if ((entityReg.GCRegistrationStatus != Constant.VisitStatus.CLOSED && entityReg.GCRegistrationStatus != Constant.VisitStatus.CANCELLED)
                    || (!AppSession.IsUsedReopenBilling && !AppSession.RegisteredPatient.IsBillingReopen && entityReg.GCRegistrationStatus != Constant.VisitStatus.CLOSED && entityReg.GCRegistrationStatus != Constant.VisitStatus.CANCELLED)
                    || (AppSession.IsUsedReopenBilling && AppSession.RegisteredPatient.IsBillingReopen && entityReg.GCRegistrationStatus == Constant.VisitStatus.CLOSED))
            {
                String[] listParam = hdnInlineEditingData.Value.Split('|');

                int countDt = 0;
                int countDtUangTitipanKeluar = 0;
                int countDtBankTransfer = 0;
                int countDtCash = 0;
                decimal paymentDtAmount = 0;

                foreach (String paramCheck in listParam)
                {
                    String[] data = paramCheck.Split(';');
                    if (data[0] != "0")
                    {
                        if (data.Length > 2)
                        {
                            if (data[2] == Constant.PaymentMethod.BANK_TRANSFER)
                            {
                                if (data[5] == "" || data[6] == "")
                                {
                                    countDtBankTransfer += 1;
                                }
                            }

                            if (data[2] == Constant.PaymentMethod.CASH)
                            {
                                countDtCash += 1;
                            }

                            if (data[2] == Constant.PaymentMethod.DOWN_PAYMENT)
                            {
                                countDtUangTitipanKeluar += 1;
                            }
                        }

                        if (data.Length > 6)
                        {
                            if (data[7] != "0" && data[7] != "")
                            {
                                paymentDtAmount += Convert.ToDecimal(data[7].Replace(",", ""));
                            }
                        }

                        countDt += 1;
                    }

                }

                string paymentType = cboPaymentType.Value.ToString();
                if (paymentType == Constant.PaymentType.SETTLEMENT || paymentType == Constant.PaymentType.CUSTOM || paymentType == Constant.PaymentType.DEPOSIT_OUT)
                {
                    if ((countDt - countDtCash - countDtUangTitipanKeluar) > 0)
                    {
                        decimal totalPatient = 0;
                        List<PatientBill> lstPatientBill = null;
                        if (hdnListBillingID.Value != "")
                        {
                            lstPatientBill = BusinessLayer.GetPatientBillList(string.Format("PatientBillingID IN ({0}) AND GCTransactionStatus = '{1}'", hdnListBillingID.Value, Constant.TransactionStatus.OPEN));
                        }
                        if (lstPatientBill != null)
                        {
                            foreach (PatientBill patientBill in lstPatientBill)
                            {
                                totalPatient += patientBill.PatientRemainingAmount;
                            }
                        }

                        totalPatient += Convert.ToDecimal(Request.Form[txtPatientRoundingAmount.UniqueID]);

                        if (paymentDtAmount > totalPatient)
                        {
                            errMessage = "Pembayaran dengan detail selain CASH nilainya tidak dapat melebihi total tagihan.";
                            return false;
                        }
                    }
                }

                if (hdnIsBridgingToPaymentGateway.Value == "1")
                {
                    if (hdnListBillingID.Value != "" && hdnListBillingID.Value != "0")
                    {
                        List<PatientPaymentDtVirtual> lstDtVirtual = BusinessLayer.GetPatientPaymentDtVirtualList(string.Format("PatientBillingID IN ({0}) AND IsDeleted = 0", hdnListBillingID.Value));
                        if (lstDtVirtual.Count > 0)
                        {
                            errMessage = string.Format("Tidak bisa proses pembayaran ada nomor tagihan sudah dikirim ke virtual payment.");
                            return false;
                        }
                    }
                }

                if (countDt == 0)
                {
                    errMessage = "Pembayaran tidak dapat disimpan karena belum memiliki detail bayar.";
                    return false;
                }
                else
                {
                    if (countDtBankTransfer > 0)
                    {
                        errMessage = "Pembayaran dengan Bank Transfer harus mengisi Informasi Bank terlebih dahulu.";
                        return false;
                    }
                    return true;
                }
            }
            else
            {
                errMessage = "Pembayaran tidak dapat disimpan karena status registrasi sudah tutup/batal.";
                return false;
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            String[] listParam = hdnInlineEditingData.Value.Split('|');
            String[] listParam2 = hdnInlineEditingPayerData.Value.Split('|');

            if (!IsTransactionValid(ref errMessage))
            {
                return false;
            }

            IDbContext ctx = DbFactory.Configure(true);
            PatientPaymentHdDao entityHdDao = new PatientPaymentHdDao(ctx);
            PatientPaymentDtDao entityDtDao = new PatientPaymentDtDao(ctx);
            PatientPaymentDtInfoDao entityDtInfoDao = new PatientPaymentDtInfoDao(ctx);
            PatientBillPaymentDao patientBillPaymentDao = new PatientBillPaymentDao(ctx);
            PatientBillDao patientBillDao = new PatientBillDao(ctx);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            RegistrationDao registrationDao = new RegistrationDao(ctx);
            RegistrationBPJSDao registrationBPJSDao = new RegistrationBPJSDao(ctx);
            HealthcareServiceUnitDao healthcareServiceUnitDao = new HealthcareServiceUnitDao(ctx);

            try
            {
                int checkCash = 0;
                int checkDepositOUT = 0;
                decimal depositOutAmount = 0;
                bool isDownPayment = false;

                string paymentType = cboPaymentType.Value.ToString();
                if (paymentType == Constant.PaymentType.SETTLEMENT || paymentType == Constant.PaymentType.DEPOSIT_OUT)
                {
                    foreach (String paramCheck in listParam)
                    {
                        String[] data = paramCheck.Split(';');
                        if (data[2] == Constant.PaymentMethod.CASH)
                        {
                            checkCash += 1;
                        }
                    }
                }

                if (paymentType == Constant.PaymentType.DOWN_PAYMENT)
                {
                    isDownPayment = true;
                }

                foreach (String paramCheck in listParam)
                {
                    if (!String.IsNullOrEmpty(paramCheck))
                    {
                        String[] data = paramCheck.Split(';');

                        if (data[7] != "" && data[7] != null)
                        {
                            if (data[2] == Constant.PaymentMethod.DEPOSIT_OUT)
                            {
                                checkDepositOUT += 1;
                                depositOutAmount += Convert.ToDecimal(data[7]);
                            }

                            if (cboPaymentType.Value.ToString() == Constant.PaymentType.DEPOSIT_OUT)
                            {
                                depositOutAmount += (Convert.ToDecimal(data[7]) * -1);
                            }
                        }
                    }
                }

                decimal outstandingDP = hdnOutstandingDP.Value != null && hdnOutstandingDP.Value != "" ? Convert.ToDecimal(hdnOutstandingDP.Value) : 0;
                decimal totalPayment = Convert.ToDecimal(hdnTotalPaymentAmount.Value);
                if ((((outstandingDP + totalPayment) >= 0) && isDownPayment) || ((((outstandingDP + totalPayment) >= 0) || ((outstandingDP + totalPayment) < 0)) && !isDownPayment))// cek utk uang muka ato uang titipan sisanya gak boleh minus
                {
                    if (checkCash == 0 || checkCash == 1) // cek utk pembayaran dgn Cash hanya boleh 1x saja
                    {
                        if (paymentType == Constant.PaymentType.DEPOSIT_OUT && checkDepositOUT > 0)
                        {
                            result = false;
                            errMessage = "Maaf, pembayaran jenis DEPOSIT OUT tidak bisa digunakan cara bayar DEPOSIT OUT juga.";
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                        else
                        {
                            Registration entityRegistration = registrationDao.Get(Convert.ToInt32(hdnRegistrationID.Value));
                            if (checkDepositOUT == 0 || checkDepositOUT == 1)
                            {
                                if (depositOutAmount <= Convert.ToDecimal(hdnDepositBalanceEnd.Value))
                                {
                                    List<PatientBill> lstPatientBill = null;

                                    if (paymentType == Constant.PaymentType.DOWN_PAYMENT || paymentType == Constant.PaymentType.DEPOSIT_IN)
                                    {
                                        if (hdnListBillingID.Value == "")
                                        {
                                            hdnListBillingID.Value = "0";
                                        }
                                    }

                                    if (hdnListBillingID.Value != "")
                                    {
                                        lstPatientBill = BusinessLayer.GetPatientBillList(string.Format("PatientBillingID IN ({0}) AND GCTransactionStatus = '{1}'", hdnListBillingID.Value, Constant.TransactionStatus.OPEN), ctx);
                                    }

                                    if ((paymentType == Constant.PaymentType.DOWN_PAYMENT || paymentType == Constant.PaymentType.DEPOSIT_IN || paymentType == Constant.PaymentType.DEPOSIT_OUT) || ((paymentType != Constant.PaymentType.DOWN_PAYMENT && paymentType != Constant.PaymentType.DEPOSIT_IN && paymentType != Constant.PaymentType.DEPOSIT_OUT) && lstPatientBill.Count() > 0))
                                    {
                                        if (hdnIsProcessARPatient.Value == "1" && cboPaymentType.Value.ToString() == Constant.PaymentType.SETTLEMENT)
                                        {
                                            #region SETTLEMENT WITH AR

                                            List<PatientChargesHd> lstChargesHd = new List<PatientChargesHd>();
                                            List<String> lstGCPaymentType = new List<String>();
                                            lstGCPaymentType.Add(Constant.PaymentType.SETTLEMENT);
                                            lstGCPaymentType.Add(Constant.PaymentType.AR_PATIENT);

                                            decimal totalPatientRemainingAmount = lstPatientBill.Sum(a => a.PatientRemainingAmount);
                                            decimal totalSettlementAmount = 0;
                                            decimal totalARAmount = 0;

                                            foreach (String param in listParam)
                                            {
                                                String[] data = param.Split(';');
                                                bool isChanged = data[0] == "1" ? true : false;
                                                int ID = Convert.ToInt32(data[1]);
                                                if (isChanged || ID > 0)
                                                {
                                                    totalSettlementAmount += Convert.ToDecimal(data[7].Replace(",", ""));
                                                }
                                            }

                                            foreach (String GCPaymentType in lstGCPaymentType)
                                            {
                                                if (GCPaymentType == Constant.PaymentType.AR_PATIENT)
                                                {
                                                    #region ARPatient
                                                    if (totalARAmount > 0)
                                                    {
                                                        #region Payment Hd
                                                        PatientPaymentHd entityHd = new PatientPaymentHd();

                                                        if (hdnTanggalPiutangPribadi.Value == "1")
                                                        {
                                                            entityHd.PaymentDate = Helper.GetDatePickerValue(txtPaymentDate);
                                                            entityHd.PaymentTime = txtPaymentTime.Text;
                                                        }
                                                        else
                                                        {
                                                            entityHd.PaymentDate = DateTime.Now;
                                                            entityHd.PaymentTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                                                        }

                                                        entityHd.GCPaymentType = GCPaymentType;
                                                        entityHd.RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
                                                        entityHd.GCCashierGroup = cboCashierGroup.Value.ToString();
                                                        entityHd.GCShift = cboShift.Value.ToString();

                                                        ////entityHd.PatientRoundingAmount = Convert.ToDecimal(Request.Form[txtPatientRoundingAmount.UniqueID]);

                                                        entityHd.TotalPatientBillAmount = totalARAmount;

                                                        entityHd.TotalPaymentAmount = entityHd.TotalPatientBillAmount;
                                                        entityHd.TotalFeeAmount = 0;

                                                        entityHd.Remarks = "";
                                                        entityHd.CashBackAmount = 0;

                                                        entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;

                                                        string transactionCode = "";
                                                        switch (hdnDepartmentID.Value)
                                                        {
                                                            case Constant.Facility.INPATIENT:
                                                                switch (entityHd.GCPaymentType)
                                                                {
                                                                    case Constant.PaymentType.SETTLEMENT: transactionCode = Constant.TransactionCode.IP_PATIENT_PAYMENT_SETTLEMENT; break;
                                                                    case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.IP_PATIENT_PAYMENT_AR_PATIENT; break;
                                                                    default: transactionCode = Constant.TransactionCode.IP_PATIENT_PAYMENT_AR_PATIENT; break;
                                                                } break;
                                                            case Constant.Facility.MEDICAL_CHECKUP:
                                                                switch (entityHd.GCPaymentType)
                                                                {
                                                                    case Constant.PaymentType.SETTLEMENT: transactionCode = Constant.TransactionCode.MCU_PATIENT_PAYMENT_SETTLEMENT; break;
                                                                    case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.MCU_PATIENT_PAYMENT_AR_PATIENT; break;
                                                                    default: transactionCode = Constant.TransactionCode.MCU_PATIENT_PAYMENT_AR_PATIENT; break;
                                                                } break;
                                                            case Constant.Facility.EMERGENCY:
                                                                switch (entityHd.GCPaymentType)
                                                                {
                                                                    case Constant.PaymentType.SETTLEMENT: transactionCode = Constant.TransactionCode.ER_PATIENT_PAYMENT_SETTLEMENT; break;
                                                                    case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.ER_PATIENT_PAYMENT_AR_PATIENT; break;
                                                                    default: transactionCode = Constant.TransactionCode.ER_PATIENT_PAYMENT_AR_PATIENT; break;
                                                                } break;
                                                            case Constant.Facility.PHARMACY:
                                                                switch (entityHd.GCPaymentType)
                                                                {
                                                                    case Constant.PaymentType.SETTLEMENT: transactionCode = Constant.TransactionCode.PH_PATIENT_PAYMENT_SETTLEMENT; break;
                                                                    case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.PH_PATIENT_PAYMENT_AR_PATIENT; break;
                                                                    default: transactionCode = Constant.TransactionCode.PH_PATIENT_PAYMENT_AR_PATIENT; break;
                                                                } break;
                                                            case Constant.Facility.DIAGNOSTIC:
                                                                if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                                                                {
                                                                    switch (entityHd.GCPaymentType)
                                                                    {
                                                                        case Constant.PaymentType.SETTLEMENT: transactionCode = Constant.TransactionCode.LABORATORY_PATIENT_PAYMENT_SETTLEMENT; break;
                                                                        case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.LABORATORY_PATIENT_PAYMENT_AR_PATIENT; break;
                                                                        default: transactionCode = Constant.TransactionCode.LABORATORY_PATIENT_PAYMENT_AR_PATIENT; break;
                                                                    }
                                                                }
                                                                else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                                                                {
                                                                    switch (entityHd.GCPaymentType)
                                                                    {
                                                                        case Constant.PaymentType.SETTLEMENT: transactionCode = Constant.TransactionCode.IMAGING_PATIENT_PAYMENT_SETTLEMENT; break;
                                                                        case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.IMAGING_PATIENT_PAYMENT_AR_PATIENT; break;
                                                                        default: transactionCode = Constant.TransactionCode.IMAGING_PATIENT_PAYMENT_AR_PATIENT; break;
                                                                    }
                                                                }
                                                                else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                                                                {
                                                                    switch (entityHd.GCPaymentType)
                                                                    {
                                                                        case Constant.PaymentType.SETTLEMENT: transactionCode = Constant.TransactionCode.RADIOTHERAPHY_PATIENT_PAYMENT_SETTLEMENT; break;
                                                                        case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.RADIOTHERAPHY_PATIENT_PAYMENT_AR_PATIENT; break;
                                                                        default: transactionCode = Constant.TransactionCode.RADIOTHERAPHY_PATIENT_PAYMENT_AR_PATIENT; break;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    switch (entityHd.GCPaymentType)
                                                                    {
                                                                        case Constant.PaymentType.SETTLEMENT: transactionCode = Constant.TransactionCode.OTHER_PATIENT_PAYMENT_SETTLEMENT; break;
                                                                        case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.OTHER_PATIENT_PAYMENT_AR_PATIENT; break;
                                                                        default: transactionCode = Constant.TransactionCode.OTHER_PATIENT_PAYMENT_AR_PATIENT; break;
                                                                    }
                                                                } break;
                                                            default:
                                                                switch (entityHd.GCPaymentType)
                                                                {
                                                                    case Constant.PaymentType.SETTLEMENT: transactionCode = Constant.TransactionCode.OP_PATIENT_PAYMENT_SETTLEMENT; break;
                                                                    case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.OP_PATIENT_PAYMENT_AR_PATIENT; break;
                                                                    default: transactionCode = Constant.TransactionCode.OP_PATIENT_PAYMENT_AR_PATIENT; break;
                                                                } break;
                                                        }
                                                        if (hdnIsBridgingToMaspion.Value == "1")
                                                        {
                                                            entityHd.GCBridgingStatus = Constant.MaspionProcessStatus.OPEN;
                                                        }
                                                        entityHd.PaymentNo = BusinessLayer.GenerateTransactionNo(transactionCode, entityHd.PaymentDate, ctx);
                                                        //entityHd.CreatedBy = entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                        entityHd.CreatedBy = AppSession.UserLogin.UserID;
                                                        ctx.CommandType = CommandType.Text;
                                                        ctx.Command.Parameters.Clear();
                                                        entityHd.PaymentID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
                                                        #endregion

                                                        #region Payment Dt

                                                        decimal dpAmount = Convert.ToDecimal(hdnOutstandingDP.Value.ToString().Replace(",", ""));
                                                        if (dpAmount != 0)
                                                        {
                                                            if (hdnIsProcessARPatientWithoutSettlement.Value == "1")
                                                            {
                                                                for (int i = 0; i < 2; i++)
                                                                {
                                                                    PatientPaymentDt entityDt = new PatientPaymentDt();
                                                                    entityDt.PaymentID = entityHd.PaymentID;

                                                                    string filterBP = string.Format("BusinessPartnerID = (SELECT BusinessPartnerID FROM Customer WHERE GCCustomerType = '{0}')", Constant.CustomerType.PERSONAL);
                                                                    BusinessPartners bp = BusinessLayer.GetBusinessPartnersList(filterBP, ctx).FirstOrDefault();
                                                                    entityDt.BusinessPartnerID = bp.BusinessPartnerID;

                                                                    if (i == 0)
                                                                    {
                                                                        entityDt.GCPaymentMethod = Constant.PaymentMethod.DOWN_PAYMENT;
                                                                        entityDt.PaymentAmount = Convert.ToDecimal(hdnOutstandingDP.Value.ToString().Replace(",", ""));
                                                                    }
                                                                    else
                                                                    {
                                                                        entityDt.GCPaymentMethod = Constant.PaymentMethod.CREDIT;
                                                                        entityDt.PaymentAmount = Convert.ToDecimal(hdnARPatientWithoutDP.Value.ToString().Replace(",", ""));
                                                                    }
                                                                    entityDt.CardFeeAmount = 0;
                                                                    entityDt.CreatedBy = AppSession.UserLogin.UserID;
                                                                    ctx.CommandType = CommandType.Text;
                                                                    ctx.Command.Parameters.Clear();
                                                                    int paymentDetailID = entityDtDao.InsertReturnPrimaryKeyID(entityDt);

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

                                                                            dtInfo.SequenceNo = Convert.ToInt32(entityRegistration.RegistrationDate.ToString("dd"));
                                                                            ctx.CommandType = CommandType.Text;
                                                                            ctx.Command.Parameters.Clear();
                                                                            entityDtInfoDao.Insert(dtInfo);

                                                                            RegistrationBPJS regBPJS = registrationBPJSDao.Get(entityHd.RegistrationID);
                                                                            if (regBPJS != null)
                                                                            {
                                                                                regBPJS.GCClaimStatus = dtInfo.GCClaimStatus;
                                                                                regBPJS.ClaimBy = dtInfo.ClaimBy;
                                                                                regBPJS.ClaimDate = dtInfo.ClaimDate;
                                                                                regBPJS.GCFinalStatus = dtInfo.GCFinalStatus;
                                                                                regBPJS.FinalBy = dtInfo.FinalBy;
                                                                                if (dtInfo.FinalBy != null) { if (dtInfo.FinalBy != null) { regBPJS.FinalDate = dtInfo.FinalDate; } }
                                                                                ctx.CommandType = CommandType.Text;
                                                                                ctx.Command.Parameters.Clear();
                                                                                registrationBPJSDao.Update(regBPJS);
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
                                                                            dtInfo.SequenceNo = Convert.ToInt32(entityRegistration.RegistrationDate.ToString("dd"));
                                                                            ctx.CommandType = CommandType.Text;
                                                                            ctx.Command.Parameters.Clear();
                                                                            entityDtInfoDao.Insert(dtInfo);

                                                                            RegistrationBPJS regBPJS = registrationBPJSDao.Get(entityHd.RegistrationID);
                                                                            if (regBPJS != null)
                                                                            {
                                                                                regBPJS.GCClaimStatus = dtInfo.GCClaimStatus;
                                                                                regBPJS.ClaimBy = dtInfo.ClaimBy;
                                                                                regBPJS.ClaimDate = dtInfo.ClaimDate;
                                                                                regBPJS.GCFinalStatus = dtInfo.GCFinalStatus;
                                                                                regBPJS.FinalBy = dtInfo.FinalBy;
                                                                                if (dtInfo.FinalBy != null) { regBPJS.FinalDate = dtInfo.FinalDate; }
                                                                                ctx.CommandType = CommandType.Text;
                                                                                ctx.Command.Parameters.Clear();
                                                                                registrationBPJSDao.Update(regBPJS);
                                                                            }
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
                                                                        dtInfo.SequenceNo = Convert.ToInt32(entityRegistration.RegistrationDate.ToString("dd"));
                                                                        ctx.CommandType = CommandType.Text;
                                                                        ctx.Command.Parameters.Clear();
                                                                        entityDtInfoDao.Insert(dtInfo);

                                                                        RegistrationBPJS regBPJS = registrationBPJSDao.Get(entityHd.RegistrationID);
                                                                        if (regBPJS != null)
                                                                        {
                                                                            regBPJS.GCClaimStatus = dtInfo.GCClaimStatus;
                                                                            regBPJS.ClaimBy = dtInfo.ClaimBy;
                                                                            regBPJS.ClaimDate = dtInfo.ClaimDate;
                                                                            regBPJS.GCFinalStatus = dtInfo.GCFinalStatus;
                                                                            regBPJS.FinalBy = dtInfo.FinalBy;
                                                                            if (dtInfo.FinalBy != null) { regBPJS.FinalDate = dtInfo.FinalDate; }
                                                                            ctx.CommandType = CommandType.Text;
                                                                            ctx.Command.Parameters.Clear();
                                                                            registrationBPJSDao.Update(regBPJS);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                PatientPaymentDt entityDt = new PatientPaymentDt();
                                                                entityDt.PaymentID = entityHd.PaymentID;

                                                                string filterBP = string.Format("BusinessPartnerID = (SELECT BusinessPartnerID FROM Customer WHERE GCCustomerType = '{0}')", Constant.CustomerType.PERSONAL);
                                                                BusinessPartners bp = BusinessLayer.GetBusinessPartnersList(filterBP, ctx).FirstOrDefault();
                                                                entityDt.BusinessPartnerID = bp.BusinessPartnerID;
                                                                entityDt.GCPaymentMethod = Constant.PaymentMethod.CREDIT;
                                                                entityDt.PaymentAmount = totalARAmount;
                                                                entityDt.CardFeeAmount = 0;
                                                                entityDt.CreatedBy = AppSession.UserLogin.UserID;
                                                                ctx.CommandType = CommandType.Text;
                                                                ctx.Command.Parameters.Clear();
                                                                int paymentDetailID = entityDtDao.InsertReturnPrimaryKeyID(entityDt);

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

                                                                        dtInfo.SequenceNo = Convert.ToInt32(entityRegistration.RegistrationDate.ToString("dd"));
                                                                        ctx.CommandType = CommandType.Text;
                                                                        ctx.Command.Parameters.Clear();
                                                                        entityDtInfoDao.Insert(dtInfo);

                                                                        RegistrationBPJS regBPJS = registrationBPJSDao.Get(entityHd.RegistrationID);
                                                                        if (regBPJS != null)
                                                                        {
                                                                            regBPJS.GCClaimStatus = dtInfo.GCClaimStatus;
                                                                            regBPJS.ClaimBy = dtInfo.ClaimBy;
                                                                            regBPJS.ClaimDate = dtInfo.ClaimDate;
                                                                            regBPJS.GCFinalStatus = dtInfo.GCFinalStatus;
                                                                            regBPJS.FinalBy = dtInfo.FinalBy;
                                                                            if (dtInfo.FinalBy != null) { regBPJS.FinalDate = dtInfo.FinalDate; }
                                                                            ctx.CommandType = CommandType.Text;
                                                                            ctx.Command.Parameters.Clear();
                                                                            registrationBPJSDao.Update(regBPJS);
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
                                                                        dtInfo.SequenceNo = Convert.ToInt32(entityRegistration.RegistrationDate.ToString("dd"));
                                                                        ctx.CommandType = CommandType.Text;
                                                                        ctx.Command.Parameters.Clear();
                                                                        entityDtInfoDao.Insert(dtInfo);

                                                                        RegistrationBPJS regBPJS = registrationBPJSDao.Get(entityHd.RegistrationID);
                                                                        if (regBPJS != null)
                                                                        {
                                                                            regBPJS.GCClaimStatus = dtInfo.GCClaimStatus;
                                                                            regBPJS.ClaimBy = dtInfo.ClaimBy;
                                                                            regBPJS.ClaimDate = dtInfo.ClaimDate;
                                                                            regBPJS.GCFinalStatus = dtInfo.GCFinalStatus;
                                                                            regBPJS.FinalBy = dtInfo.FinalBy;
                                                                            if (dtInfo.FinalBy != null) { regBPJS.FinalDate = dtInfo.FinalDate; }
                                                                            ctx.CommandType = CommandType.Text;
                                                                            ctx.Command.Parameters.Clear();
                                                                            registrationBPJSDao.Update(regBPJS);
                                                                        }
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
                                                                    dtInfo.SequenceNo = Convert.ToInt32(entityRegistration.RegistrationDate.ToString("dd"));
                                                                    ctx.CommandType = CommandType.Text;
                                                                    ctx.Command.Parameters.Clear();
                                                                    entityDtInfoDao.Insert(dtInfo);

                                                                    RegistrationBPJS regBPJS = registrationBPJSDao.Get(entityHd.RegistrationID);
                                                                    if (regBPJS != null)
                                                                    {
                                                                        regBPJS.GCClaimStatus = dtInfo.GCClaimStatus;
                                                                        regBPJS.ClaimBy = dtInfo.ClaimBy;
                                                                        regBPJS.ClaimDate = dtInfo.ClaimDate;
                                                                        regBPJS.GCFinalStatus = dtInfo.GCFinalStatus;
                                                                        regBPJS.FinalBy = dtInfo.FinalBy;
                                                                        if (dtInfo.FinalBy != null) { regBPJS.FinalDate = dtInfo.FinalDate; }
                                                                        ctx.CommandType = CommandType.Text;
                                                                        ctx.Command.Parameters.Clear();
                                                                        registrationBPJSDao.Update(regBPJS);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            PatientPaymentDt entityDt = new PatientPaymentDt();
                                                            entityDt.PaymentID = entityHd.PaymentID;

                                                            string filterBP = string.Format("BusinessPartnerID = (SELECT BusinessPartnerID FROM Customer WHERE GCCustomerType = '{0}')", Constant.CustomerType.PERSONAL);
                                                            BusinessPartners bp = BusinessLayer.GetBusinessPartnersList(filterBP, ctx).FirstOrDefault();
                                                            entityDt.BusinessPartnerID = bp.BusinessPartnerID;

                                                            entityDt.GCPaymentMethod = Constant.PaymentMethod.CREDIT;
                                                            entityDt.PaymentAmount = totalARAmount;

                                                            entityDt.CardFeeAmount = 0;
                                                            entityDt.CreatedBy = AppSession.UserLogin.UserID;
                                                            ctx.CommandType = CommandType.Text;
                                                            ctx.Command.Parameters.Clear();
                                                            int paymentDetailID = entityDtDao.InsertReturnPrimaryKeyID(entityDt);

                                                            string filterC = string.Format("BusinessPartnerID = '{0}'", entityDt.BusinessPartnerID);
                                                            vCustomer oCustomer = BusinessLayer.GetvCustomerList(filterC, ctx).FirstOrDefault();

                                                            if (entityDt.GCPaymentMethod == Constant.PaymentMethod.CREDIT)
                                                            {
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

                                                                    dtInfo.SequenceNo = Convert.ToInt32(entityRegistration.RegistrationDate.ToString("dd"));
                                                                    ctx.CommandType = CommandType.Text;
                                                                    ctx.Command.Parameters.Clear();
                                                                    entityDtInfoDao.Insert(dtInfo);

                                                                    RegistrationBPJS regBPJS = registrationBPJSDao.Get(entityHd.RegistrationID);
                                                                    if (regBPJS != null)
                                                                    {
                                                                        regBPJS.GCClaimStatus = dtInfo.GCClaimStatus;
                                                                        regBPJS.ClaimBy = dtInfo.ClaimBy;
                                                                        regBPJS.ClaimDate = dtInfo.ClaimDate;
                                                                        regBPJS.GCFinalStatus = dtInfo.GCFinalStatus;
                                                                        regBPJS.FinalBy = dtInfo.FinalBy;
                                                                        if (dtInfo.FinalBy != null) { regBPJS.FinalDate = dtInfo.FinalDate; }
                                                                        ctx.CommandType = CommandType.Text;
                                                                        ctx.Command.Parameters.Clear();
                                                                        registrationBPJSDao.Update(regBPJS);
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
                                                                    dtInfo.SequenceNo = Convert.ToInt32(entityRegistration.RegistrationDate.ToString("dd"));
                                                                    ctx.CommandType = CommandType.Text;
                                                                    ctx.Command.Parameters.Clear();
                                                                    entityDtInfoDao.Insert(dtInfo);

                                                                    RegistrationBPJS regBPJS = registrationBPJSDao.Get(entityHd.RegistrationID);
                                                                    if (regBPJS != null)
                                                                    {
                                                                        regBPJS.GCClaimStatus = dtInfo.GCClaimStatus;
                                                                        regBPJS.ClaimBy = dtInfo.ClaimBy;
                                                                        regBPJS.ClaimDate = dtInfo.ClaimDate;
                                                                        regBPJS.GCFinalStatus = dtInfo.GCFinalStatus;
                                                                        regBPJS.FinalBy = dtInfo.FinalBy;
                                                                        if (dtInfo.FinalBy != null) { regBPJS.FinalDate = dtInfo.FinalDate; }
                                                                        ctx.CommandType = CommandType.Text;
                                                                        ctx.Command.Parameters.Clear();
                                                                        registrationBPJSDao.Update(regBPJS);
                                                                    }
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
                                                                dtInfo.SequenceNo = Convert.ToInt32(entityRegistration.RegistrationDate.ToString("dd"));
                                                                ctx.CommandType = CommandType.Text;
                                                                ctx.Command.Parameters.Clear();
                                                                entityDtInfoDao.Insert(dtInfo);

                                                                RegistrationBPJS regBPJS = registrationBPJSDao.Get(entityHd.RegistrationID);
                                                                if (regBPJS != null)
                                                                {
                                                                    regBPJS.GCClaimStatus = dtInfo.GCClaimStatus;
                                                                    regBPJS.ClaimBy = dtInfo.ClaimBy;
                                                                    regBPJS.ClaimDate = dtInfo.ClaimDate;
                                                                    regBPJS.GCFinalStatus = dtInfo.GCFinalStatus;
                                                                    regBPJS.FinalBy = dtInfo.FinalBy;
                                                                    if (dtInfo.FinalBy != null) { regBPJS.FinalDate = dtInfo.FinalDate; }
                                                                    ctx.CommandType = CommandType.Text;
                                                                    ctx.Command.Parameters.Clear();
                                                                    registrationBPJSDao.Update(regBPJS);
                                                                }
                                                            }
                                                        }

                                                        #endregion

                                                        #region Update Billing
                                                        ctx.CommandType = CommandType.Text;
                                                        ctx.Command.Parameters.Clear();
                                                        List<PatientChargesHd> lstPatientChargesHd = BusinessLayer.GetPatientChargesHdList(string.Format("PatientBillingID IN ({0})", hdnListBillingID.Value), ctx);

                                                        decimal totalPaymentAmount = entityHd.TotalPaymentAmount;
                                                        string oldBillStatus = null;
                                                        decimal totalARAmountTemp = totalARAmount;

                                                        foreach (PatientBill patientBill in lstPatientBill)
                                                        {
                                                            if (totalPaymentAmount > 0)
                                                            {
                                                                oldBillStatus = patientBill.GCTransactionStatus;

                                                                PatientBillPayment patientBillPayment = new PatientBillPayment();
                                                                patientBillPayment.PaymentID = entityHd.PaymentID;
                                                                patientBillPayment.PatientBillingID = patientBill.PatientBillingID;

                                                                patientBill.PaymentID = entityHd.PaymentID;

                                                                if (patientBill.TotalPatientPaymentAmount < (patientBill.TotalPatientAmount - patientBill.PatientDiscountAmount))
                                                                {
                                                                    if (totalARAmountTemp > (patientBill.TotalPatientAmount - patientBill.PatientDiscountAmount - patientBill.TotalPatientPaymentAmount))
                                                                    {
                                                                        patientBillPayment.PatientPaymentAmount = (patientBill.TotalPatientAmount - patientBill.PatientDiscountAmount - patientBill.TotalPatientPaymentAmount);
                                                                        patientBill.TotalPatientPaymentAmount += (patientBill.TotalPatientAmount - patientBill.PatientDiscountAmount - patientBill.TotalPatientPaymentAmount);

                                                                        totalARAmountTemp = totalARAmountTemp - (patientBill.TotalPatientAmount - patientBill.PatientDiscountAmount - patientBill.TotalPatientPaymentAmount);
                                                                    }
                                                                    else
                                                                    {
                                                                        patientBill.TotalPatientPaymentAmount += totalARAmount;
                                                                        patientBillPayment.PatientPaymentAmount = totalARAmount;

                                                                        totalARAmountTemp = totalARAmountTemp - totalARAmount;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    totalARAmountTemp = 0;
                                                                    patientBillPayment.PatientPaymentAmount = 0;
                                                                }


                                                                if (patientBill.RemainingAmount < 1)
                                                                {
                                                                    patientBill.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                                                                    lstChargesHd = lstPatientChargesHd.Where(p => p.PatientBillingID == patientBill.PatientBillingID).ToList();
                                                                    foreach (PatientChargesHd patientChargesHd in lstChargesHd)
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
                                                            }
                                                        }
                                                        #endregion

                                                    }
                                                    #endregion
                                                }
                                                else
                                                {
                                                    #region Settlement

                                                    #region Payment Hd
                                                    PatientPaymentHd entityHd = new PatientPaymentHd();
                                                    entityHd.PaymentDate = Helper.GetDatePickerValue(txtPaymentDate);
                                                    entityHd.PaymentTime = txtPaymentTime.Text;
                                                    entityHd.GCPaymentType = cboPaymentType.Value.ToString();
                                                    entityHd.RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
                                                    entityHd.GCCashierGroup = cboCashierGroup.Value.ToString();
                                                    entityHd.GCShift = cboShift.Value.ToString();

                                                    entityHd.PatientRoundingAmount = Convert.ToDecimal(Request.Form[txtPatientRoundingAmount.UniqueID]);

                                                    entityHd.TotalPatientBillAmount = totalSettlementAmount;
                                                    entityHd.TotalPaymentAmount = totalSettlementAmount;
                                                    entityHd.CashBackAmount = 0;

                                                    entityHd.Remarks = txtRemarks.Text;
                                                    entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;

                                                    string transactionCode = "";
                                                    switch (hdnDepartmentID.Value)
                                                    {
                                                        case Constant.Facility.INPATIENT:
                                                            switch (entityHd.GCPaymentType)
                                                            {
                                                                case Constant.PaymentType.SETTLEMENT: transactionCode = Constant.TransactionCode.IP_PATIENT_PAYMENT_SETTLEMENT; break;
                                                                case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.IP_PATIENT_PAYMENT_AR_PATIENT; break;
                                                                default: transactionCode = Constant.TransactionCode.IP_PATIENT_PAYMENT_AR_PATIENT; break;
                                                            } break;
                                                        case Constant.Facility.MEDICAL_CHECKUP:
                                                            switch (entityHd.GCPaymentType)
                                                            {
                                                                case Constant.PaymentType.SETTLEMENT: transactionCode = Constant.TransactionCode.MCU_PATIENT_PAYMENT_SETTLEMENT; break;
                                                                case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.MCU_PATIENT_PAYMENT_AR_PATIENT; break;
                                                                default: transactionCode = Constant.TransactionCode.MCU_PATIENT_PAYMENT_AR_PATIENT; break;
                                                            } break;
                                                        case Constant.Facility.EMERGENCY:
                                                            switch (entityHd.GCPaymentType)
                                                            {
                                                                case Constant.PaymentType.SETTLEMENT: transactionCode = Constant.TransactionCode.ER_PATIENT_PAYMENT_SETTLEMENT; break;
                                                                case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.ER_PATIENT_PAYMENT_AR_PATIENT; break;
                                                                default: transactionCode = Constant.TransactionCode.ER_PATIENT_PAYMENT_AR_PATIENT; break;
                                                            } break;
                                                        case Constant.Facility.PHARMACY:
                                                            switch (entityHd.GCPaymentType)
                                                            {
                                                                case Constant.PaymentType.SETTLEMENT: transactionCode = Constant.TransactionCode.PH_PATIENT_PAYMENT_SETTLEMENT; break;
                                                                case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.PH_PATIENT_PAYMENT_AR_PATIENT; break;
                                                                default: transactionCode = Constant.TransactionCode.PH_PATIENT_PAYMENT_AR_PATIENT; break;
                                                            } break;
                                                        case Constant.Facility.DIAGNOSTIC:
                                                            if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                                                            {
                                                                switch (entityHd.GCPaymentType)
                                                                {
                                                                    case Constant.PaymentType.SETTLEMENT: transactionCode = Constant.TransactionCode.LABORATORY_PATIENT_PAYMENT_SETTLEMENT; break;
                                                                    case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.LABORATORY_PATIENT_PAYMENT_AR_PATIENT; break;
                                                                    default: transactionCode = Constant.TransactionCode.LABORATORY_PATIENT_PAYMENT_AR_PATIENT; break;
                                                                }
                                                            }
                                                            else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                                                            {
                                                                switch (entityHd.GCPaymentType)
                                                                {
                                                                    case Constant.PaymentType.SETTLEMENT: transactionCode = Constant.TransactionCode.IMAGING_PATIENT_PAYMENT_SETTLEMENT; break;
                                                                    case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.IMAGING_PATIENT_PAYMENT_AR_PATIENT; break;
                                                                    default: transactionCode = Constant.TransactionCode.IMAGING_PATIENT_PAYMENT_AR_PATIENT; break;
                                                                }
                                                            }
                                                            else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                                                            {
                                                                switch (entityHd.GCPaymentType)
                                                                {
                                                                    case Constant.PaymentType.SETTLEMENT: transactionCode = Constant.TransactionCode.RADIOTHERAPHY_PATIENT_PAYMENT_SETTLEMENT; break;
                                                                    case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.RADIOTHERAPHY_PATIENT_PAYMENT_AR_PATIENT; break;
                                                                    default: transactionCode = Constant.TransactionCode.RADIOTHERAPHY_PATIENT_PAYMENT_AR_PATIENT; break;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                switch (entityHd.GCPaymentType)
                                                                {
                                                                    case Constant.PaymentType.SETTLEMENT: transactionCode = Constant.TransactionCode.OTHER_PATIENT_PAYMENT_SETTLEMENT; break;
                                                                    case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.OTHER_PATIENT_PAYMENT_AR_PATIENT; break;
                                                                    default: transactionCode = Constant.TransactionCode.OTHER_PATIENT_PAYMENT_AR_PATIENT; break;
                                                                }
                                                            } break;
                                                        default:
                                                            switch (entityHd.GCPaymentType)
                                                            {
                                                                case Constant.PaymentType.SETTLEMENT: transactionCode = Constant.TransactionCode.OP_PATIENT_PAYMENT_SETTLEMENT; break;
                                                                case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.OP_PATIENT_PAYMENT_AR_PATIENT; break;
                                                                default: transactionCode = Constant.TransactionCode.OP_PATIENT_PAYMENT_AR_PATIENT; break;
                                                            } break;
                                                    }
                                                    entityHd.PaymentNo = BusinessLayer.GenerateTransactionNo(transactionCode, entityHd.PaymentDate, ctx);
                                                    entityHd.CreatedBy = AppSession.UserLogin.UserID;

                                                    #region bridging  Maspion
                                                    if (hdnIsBridgingToMaspion.Value == "1")
                                                    {
                                                        //hanya uang cash
                                                        if (checkCash > 0)
                                                        {
                                                            entityHd.GCBridgingStatus = Constant.MaspionProcessStatus.OPEN;
                                                        }
                                                    }
                                                    #endregion

                                                    ctx.CommandType = CommandType.Text;
                                                    ctx.Command.Parameters.Clear();
                                                    entityHd.PaymentID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
                                                    #endregion

                                                    #region Payment Dt
                                                    foreach (String param in listParam)
                                                    {
                                                        String[] data = param.Split(';');
                                                        bool isChanged = data[0] == "1" ? true : false;
                                                        int ID = Convert.ToInt32(data[1]);
                                                        if (isChanged || ID > 0)
                                                        {
                                                            PatientPaymentDt entityDt = new PatientPaymentDt();
                                                            entityDt.PaymentID = entityHd.PaymentID;
                                                            entityDt.GCPaymentMethod = data[2];
                                                            if (entityDt.GCPaymentMethod != Constant.PaymentMethod.CASH)
                                                            {
                                                                if (data[3] != "")
                                                                    entityDt.EDCMachineID = Convert.ToInt32(data[3]);
                                                                else
                                                                    entityDt.EDCMachineID = null;
                                                                if (data[5] != "")
                                                                    entityDt.BankID = Convert.ToInt32(data[5]);
                                                                else
                                                                    entityDt.BankID = null;
                                                                entityDt.ReferenceNo = data[6];
                                                                entityDt.GCCardType = data[10];
                                                                if (data[12] != "")
                                                                    entityDt.CardNumber = string.Format("XXXX-XXXX-XXXX-{0}", data[12]);
                                                                else
                                                                    entityDt.CardNumber = "";
                                                                entityDt.CardHolderName = data[13];

                                                                if (data[14] != "" && data[15] != "")
                                                                    entityDt.CardValidThru = string.Format("{0:00}/{1:00}", data[14].PadLeft(2, '0'), data[15].Substring(2));
                                                                else
                                                                    entityDt.CardValidThru = "";
                                                                entityDt.GCCardProvider = data[16];
                                                                if (data[17] != "")
                                                                {
                                                                    entityDt.BatchNo = data[17];
                                                                }
                                                                if (data[18] != "")
                                                                {
                                                                    entityDt.TraceNo = data[18];
                                                                }

                                                                if (data[19] != "")
                                                                {
                                                                    entityDt.ApprovalCode = data[19];
                                                                }

                                                                if (data[20] != "")
                                                                {
                                                                    entityDt.TerminalID = data[20];
                                                                }
                                                            }
                                                            entityDt.PaymentAmount = Convert.ToDecimal(data[7].Replace(",", ""));
                                                            entityDt.CardFeeAmount = Convert.ToDecimal(data[8].Replace(",", ""));
                                                            entityDt.CreatedBy = AppSession.UserLogin.UserID;
                                                            ctx.CommandType = CommandType.Text;
                                                            ctx.Command.Parameters.Clear();
                                                            int paymentDetailID = entityDtDao.InsertReturnPrimaryKeyID(entityDt);

                                                            PatientPaymentDtInfo dtInfo = new PatientPaymentDtInfo();
                                                            dtInfo.PaymentDetailID = paymentDetailID;
                                                            dtInfo.GCClaimStatus = Constant.ClaimStatus.APPROVED;
                                                            dtInfo.GCFinalStatus = Constant.FinalStatus.APPROVED;
                                                            dtInfo.GrouperAmountClaim = dtInfo.GrouperAmountFinal = entityDt.PaymentAmount;
                                                            dtInfo.ClaimBy = dtInfo.FinalBy = AppSession.UserLogin.UserID;
                                                            dtInfo.ClaimDate = dtInfo.FinalDate = DateTime.Now;
                                                            dtInfo.SequenceNo = Convert.ToInt32(entityRegistration.RegistrationDate.ToString("dd"));
                                                            ctx.CommandType = CommandType.Text;
                                                            ctx.Command.Parameters.Clear();
                                                            entityDtInfoDao.Insert(dtInfo);

                                                            RegistrationBPJS regBPJS = registrationBPJSDao.Get(entityHd.RegistrationID);
                                                            if (regBPJS != null)
                                                            {
                                                                regBPJS.GCClaimStatus = dtInfo.GCClaimStatus;
                                                                regBPJS.ClaimBy = dtInfo.ClaimBy;
                                                                regBPJS.ClaimDate = dtInfo.ClaimDate;
                                                                regBPJS.GCFinalStatus = dtInfo.GCFinalStatus;
                                                                regBPJS.FinalBy = dtInfo.FinalBy;
                                                                if (dtInfo.FinalBy != null) { regBPJS.FinalDate = dtInfo.FinalDate; }
                                                                ctx.CommandType = CommandType.Text;
                                                                ctx.Command.Parameters.Clear();
                                                                registrationBPJSDao.Update(regBPJS);
                                                            }
                                                        }
                                                    }
                                                    #endregion

                                                    #region Update Billing
                                                    decimal totalSettlementAmountTemp = totalSettlementAmount;
                                                    decimal totalSettlementAmountFix = 0;
                                                    if (hdnListBillingID.Value != "")
                                                    {
                                                        List<PatientChargesHd> lstPatientChargesHd = BusinessLayer.GetPatientChargesHdList(string.Format("PatientBillingID IN ({0})", hdnListBillingID.Value), ctx);

                                                        string oldBillStatus = null;

                                                        foreach (PatientBill patientBill in lstPatientBill)
                                                        {
                                                            oldBillStatus = patientBill.GCTransactionStatus;

                                                            patientBill.PaymentID = entityHd.PaymentID;

                                                            if (patientBill.TotalPatientPaymentAmount < (patientBill.TotalPatientAmount - patientBill.PatientDiscountAmount))
                                                            {
                                                                if (totalSettlementAmountTemp > (patientBill.TotalPatientAmount - patientBill.PatientDiscountAmount - patientBill.TotalPatientPaymentAmount))
                                                                {
                                                                    totalSettlementAmountFix = (patientBill.TotalPatientAmount - patientBill.PatientDiscountAmount - patientBill.TotalPatientPaymentAmount);
                                                                    patientBill.TotalPatientPaymentAmount += totalSettlementAmountFix;
                                                                }
                                                                else
                                                                {
                                                                    patientBill.TotalPatientPaymentAmount += totalSettlementAmountTemp;
                                                                    totalSettlementAmountFix = totalSettlementAmountTemp;
                                                                }
                                                            }

                                                            totalSettlementAmountTemp = totalSettlementAmountTemp - totalSettlementAmountFix;

                                                            PatientBillPayment patientBillPayment = new PatientBillPayment();
                                                            patientBillPayment.PaymentID = entityHd.PaymentID;
                                                            patientBillPayment.PatientBillingID = patientBill.PatientBillingID;
                                                            patientBillPayment.PatientPaymentAmount = totalSettlementAmountFix;

                                                            if (patientBillPayment.PatientPaymentAmount != 0)
                                                            {
                                                                patientBillPayment.PatientRoundingAmount = entityHd.PatientRoundingAmount;

                                                                patientBill.TotalPatientPaymentAmount -= patientBillPayment.PatientRoundingAmount;
                                                            }

                                                            if (patientBill.RemainingAmount < 1)
                                                            {
                                                                patientBill.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                                                                lstChargesHd = lstPatientChargesHd.Where(p => p.PatientBillingID == patientBill.PatientBillingID).ToList();
                                                                foreach (PatientChargesHd patientChargesHd in lstChargesHd)
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
                                                        }
                                                    }
                                                    #endregion

                                                    totalARAmount = totalPatientRemainingAmount - totalSettlementAmount + entityHd.PatientRoundingAmount;

                                                    #endregion
                                                }

                                            }

                                            ctx.CommitTransaction();

                                            #region Bridging
                                            if (hdnIsSendToLISAfterSavePayment.Value == "1")
                                            {
                                                if (AppSession.IsBridgingToLIS)
                                                {
                                                    string[] resultInfo = "0|Unknown Protocol".Split('|');
                                                    switch (AppSession.LIS_BRIDGING_PROTOCOL)
                                                    {
                                                        case Constant.LIS_Bridging_Protocol.WEB_API:
                                                            switch (AppSession.LIS_PROVIDER)
                                                            {
                                                                case Constant.LIS_PROVIDER.SOFTMEDIX:
                                                                    SendLISOrderToSoftmedix(lstChargesHd);
                                                                    break;
                                                                default:
                                                                    break;
                                                            }
                                                            break;
                                                        default:
                                                            resultInfo = "0|Unknown Protocol".Split('|');
                                                            break;
                                                    }
                                                }
                                            }

                                            #endregion

                                            #endregion
                                        }
                                        else
                                        {
                                            #region JUST 1 PAYMENT
                                            List<PatientChargesHd> lstChargesHd = new List<PatientChargesHd>();
                                            #region Payment Hd
                                            PatientPaymentHd entityHd = new PatientPaymentHd();
                                            entityHd.PaymentDate = Helper.GetDatePickerValue(Request.Form[txtPaymentDate.UniqueID]);
                                            entityHd.PaymentTime = txtPaymentTime.Text;
                                            entityHd.GCPaymentType = cboPaymentType.Value.ToString();
                                            entityHd.RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
                                            entityHd.GCCashierGroup = cboCashierGroup.Value.ToString();
                                            entityHd.GCShift = cboShift.Value.ToString();

                                            if (entityHd.GCPaymentType == Constant.PaymentType.SETTLEMENT || entityHd.GCPaymentType == Constant.PaymentType.CUSTOM || entityHd.GCPaymentType == Constant.PaymentType.DOWN_PAYMENT || entityHd.GCPaymentType == Constant.PaymentType.DEPOSIT_IN || entityHd.GCPaymentType == Constant.PaymentType.DEPOSIT_OUT)
                                            {
                                                entityHd.TotalPaymentAmount = Convert.ToDecimal(hdnTotalPaymentAmount.Value);
                                                entityHd.TotalFeeAmount = Convert.ToDecimal(hdnTotalFeeAmount.Value);
                                                if (entityHd.GCPaymentType == Constant.PaymentType.DOWN_PAYMENT && hdnListBillingID.Value != "")
                                                {
                                                    entityHd.TotalPatientBillAmount = 0;
                                                    entityHd.TotalPayerBillAmount = 0;
                                                    decimal totalPaymentAmount = entityHd.TotalPaymentAmount;
                                                    foreach (PatientBill patientBill in lstPatientBill)
                                                    {
                                                        if (totalPaymentAmount > 0)
                                                        {
                                                            if (patientBill.PatientRemainingAmount < totalPaymentAmount)
                                                                totalPaymentAmount -= patientBill.PatientRemainingAmount;
                                                            else
                                                                totalPaymentAmount = 0;
                                                            entityHd.TotalPatientBillAmount += patientBill.PatientRemainingAmount;
                                                            entityHd.TotalPayerBillAmount = Convert.ToDecimal(hdnBillingTotalPayer.Value);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    decimal totalPatient = 0, totalPayer = 0;
                                                    if (lstPatientBill != null)
                                                    {
                                                        foreach (PatientBill patientBill in lstPatientBill)
                                                        {
                                                            totalPatient += patientBill.PatientRemainingAmount;
                                                            totalPayer += patientBill.PayerRemainingAmount;
                                                            if (entityHd.GCPaymentType != Constant.PaymentType.SETTLEMENT && totalPatient > entityHd.TotalPaymentAmount)
                                                                break;
                                                        }
                                                    }
                                                    entityHd.TotalPatientBillAmount = totalPatient;
                                                    entityHd.TotalPayerBillAmount = totalPayer;
                                                }
                                            }
                                            //else if (entityHd.GCPaymentType == Constant.PaymentType.PAYMENT_RETURN)
                                            //{
                                            //    entityHd.TotalPatientBillAmount = Convert.ToDecimal(hdnBillingTotalPatient.Value);
                                            //    entityHd.TotalPayerBillAmount = Convert.ToDecimal(hdnBillingTotalPayer.Value);
                                            //    entityHd.TotalPaymentAmount = entityHd.TotalPatientBillAmount * -1;
                                            //    entityHd.TotalFeeAmount = 0;
                                            //}
                                            else
                                            {
                                                if (entityHd.GCPaymentType == Constant.PaymentType.AR_PATIENT)
                                                {
                                                    entityHd.TotalPatientBillAmount = Convert.ToDecimal(hdnBillingTotalPatient.Value);
                                                    entityHd.TotalPayerBillAmount = Convert.ToDecimal(hdnBillingTotalPayer.Value);
                                                    entityHd.TotalPaymentAmount = entityHd.TotalPatientBillAmount;
                                                    entityHd.TotalFeeAmount = 0;
                                                }
                                                else
                                                {
                                                    entityHd.TotalPaymentAmount = Convert.ToDecimal(hdnTotalPayerPaymentAmount.Value);
                                                    entityHd.TotalFeeAmount = 0;
                                                    decimal totalPayer = 0;
                                                    if (lstPatientBill != null)
                                                    {
                                                        //totalPayer = lstPatientBill.Sum(a => a.PayerRemainingAmount);
                                                        foreach (PatientBill patientBill in lstPatientBill)
                                                        {
                                                            totalPayer += patientBill.PayerRemainingAmount;
                                                            if (totalPayer > entityHd.TotalPaymentAmount)
                                                                break;
                                                        }
                                                    }
                                                    entityHd.TotalPatientBillAmount = Convert.ToDecimal(hdnBillingTotalPatient.Value);
                                                    entityHd.TotalPayerBillAmount = totalPayer;
                                                }
                                            }

                                            entityHd.Remarks = txtRemarks.Text;
                                            if (entityHd.GCPaymentType == Constant.PaymentType.SETTLEMENT || entityHd.GCPaymentType == Constant.PaymentType.CUSTOM)
                                            {
                                                if (entityHd.TotalPaymentAmount > entityHd.TotalPatientBillAmount)
                                                {
                                                    entityHd.CashBackAmount = Convert.ToDecimal(Request.Form[txtCashbackAmount.UniqueID]);
                                                }
                                                else
                                                {
                                                    entityHd.CashBackAmount = 0;
                                                }
                                            }
                                            else
                                            {
                                                entityHd.CashBackAmount = 0;
                                            }

                                            entityHd.PatientRoundingAmount = Convert.ToDecimal(Request.Form[txtPatientRoundingAmount.UniqueID]);

                                            entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                                            string transactionCode = "";
                                            switch (hdnDepartmentID.Value)
                                            {
                                                case Constant.Facility.INPATIENT:
                                                    switch (entityHd.GCPaymentType)
                                                    {
                                                        case Constant.PaymentType.DEPOSIT_OUT: transactionCode = Constant.TransactionCode.IP_DEPOSIT_OUT; break;
                                                        case Constant.PaymentType.DEPOSIT_IN: transactionCode = Constant.TransactionCode.IP_DEPOSIT_IN; break;
                                                        case Constant.PaymentType.DOWN_PAYMENT: transactionCode = Constant.TransactionCode.IP_PATIENT_PAYMENT_DP; break;
                                                        case Constant.PaymentType.SETTLEMENT: transactionCode = Constant.TransactionCode.IP_PATIENT_PAYMENT_SETTLEMENT; break;
                                                        case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.IP_PATIENT_PAYMENT_AR_PATIENT; break;
                                                        case Constant.PaymentType.CUSTOM: transactionCode = Constant.TransactionCode.IP_PATIENT_PAYMENT_CUSTOM; break;
                                                        //case Constant.PaymentType.PAYMENT_RETURN: transactionCode = Constant.TransactionCode.IP_PATIENT_PAYMENT_RETURN; break;
                                                        default: transactionCode = Constant.TransactionCode.IP_PATIENT_PAYMENT_AR_PAYER; break;
                                                    } break;
                                                case Constant.Facility.MEDICAL_CHECKUP:
                                                    switch (entityHd.GCPaymentType)
                                                    {
                                                        case Constant.PaymentType.DEPOSIT_OUT: transactionCode = Constant.TransactionCode.MCU_DEPOSIT_OUT; break;
                                                        case Constant.PaymentType.DEPOSIT_IN: transactionCode = Constant.TransactionCode.MCU_DEPOSIT_IN; break;
                                                        case Constant.PaymentType.DOWN_PAYMENT: transactionCode = Constant.TransactionCode.MCU_PATIENT_PAYMENT_DP; break;
                                                        case Constant.PaymentType.SETTLEMENT: transactionCode = Constant.TransactionCode.MCU_PATIENT_PAYMENT_SETTLEMENT; break;
                                                        case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.MCU_PATIENT_PAYMENT_AR_PATIENT; break;
                                                        case Constant.PaymentType.CUSTOM: transactionCode = Constant.TransactionCode.MCU_PATIENT_PAYMENT_CUSTOM; break;
                                                        //case Constant.PaymentType.PAYMENT_RETURN: transactionCode = Constant.TransactionCode.MCU_PATIENT_PAYMENT_RETURN; break;
                                                        default: transactionCode = Constant.TransactionCode.MCU_PATIENT_PAYMENT_AR_PAYER; break;
                                                    } break;
                                                case Constant.Facility.EMERGENCY:
                                                    switch (entityHd.GCPaymentType)
                                                    {
                                                        case Constant.PaymentType.DEPOSIT_OUT: transactionCode = Constant.TransactionCode.ER_DEPOSIT_OUT; break;
                                                        case Constant.PaymentType.DEPOSIT_IN: transactionCode = Constant.TransactionCode.ER_DEPOSIT_IN; break;
                                                        case Constant.PaymentType.DOWN_PAYMENT: transactionCode = Constant.TransactionCode.ER_PATIENT_PAYMENT_DP; break;
                                                        case Constant.PaymentType.SETTLEMENT: transactionCode = Constant.TransactionCode.ER_PATIENT_PAYMENT_SETTLEMENT; break;
                                                        case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.ER_PATIENT_PAYMENT_AR_PATIENT; break;
                                                        case Constant.PaymentType.CUSTOM: transactionCode = Constant.TransactionCode.ER_PATIENT_PAYMENT_CUSTOM; break;
                                                        //case Constant.PaymentType.PAYMENT_RETURN: transactionCode = Constant.TransactionCode.ER_PATIENT_PAYMENT_RETURN; break;
                                                        default: transactionCode = Constant.TransactionCode.ER_PATIENT_PAYMENT_AR_PAYER; break;
                                                    } break;
                                                case Constant.Facility.PHARMACY:
                                                    switch (entityHd.GCPaymentType)
                                                    {
                                                        case Constant.PaymentType.DEPOSIT_OUT: transactionCode = Constant.TransactionCode.PH_DEPOSIT_OUT; break;
                                                        case Constant.PaymentType.DEPOSIT_IN: transactionCode = Constant.TransactionCode.PH_DEPOSIT_IN; break;
                                                        case Constant.PaymentType.DOWN_PAYMENT: transactionCode = Constant.TransactionCode.PH_PATIENT_PAYMENT_DP; break;
                                                        case Constant.PaymentType.SETTLEMENT: transactionCode = Constant.TransactionCode.PH_PATIENT_PAYMENT_SETTLEMENT; break;
                                                        case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.PH_PATIENT_PAYMENT_AR_PATIENT; break;
                                                        case Constant.PaymentType.CUSTOM: transactionCode = Constant.TransactionCode.PH_PATIENT_PAYMENT_CUSTOM; break;
                                                        //case Constant.PaymentType.PAYMENT_RETURN: transactionCode = Constant.TransactionCode.PH_PATIENT_PAYMENT_RETURN; break;
                                                        default: transactionCode = Constant.TransactionCode.PH_PATIENT_PAYMENT_AR_PAYER; break;
                                                    } break;
                                                case Constant.Facility.DIAGNOSTIC:
                                                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                                                    {
                                                        switch (entityHd.GCPaymentType)
                                                        {
                                                            case Constant.PaymentType.DEPOSIT_OUT: transactionCode = Constant.TransactionCode.LABORATORY_DEPOSIT_OUT; break;
                                                            case Constant.PaymentType.DEPOSIT_IN: transactionCode = Constant.TransactionCode.LABORATORY_DEPOSIT_IN; break;
                                                            case Constant.PaymentType.DOWN_PAYMENT: transactionCode = Constant.TransactionCode.LABORATORY_PATIENT_PAYMENT_DP; break;
                                                            case Constant.PaymentType.SETTLEMENT: transactionCode = Constant.TransactionCode.LABORATORY_PATIENT_PAYMENT_SETTLEMENT; break;
                                                            case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.LABORATORY_PATIENT_PAYMENT_AR_PATIENT; break;
                                                            case Constant.PaymentType.CUSTOM: transactionCode = Constant.TransactionCode.LABORATORY_PATIENT_PAYMENT_CUSTOM; break;
                                                            //case Constant.PaymentType.PAYMENT_RETURN: transactionCode = Constant.TransactionCode.LABORATORY_PATIENT_PAYMENT_RETURN; break;
                                                            default: transactionCode = Constant.TransactionCode.LABORATORY_PATIENT_PAYMENT_AR_PAYER; break;
                                                        }
                                                    }
                                                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                                                    {
                                                        switch (entityHd.GCPaymentType)
                                                        {
                                                            case Constant.PaymentType.DEPOSIT_OUT: transactionCode = Constant.TransactionCode.IMAGING_DEPOSIT_OUT; break;
                                                            case Constant.PaymentType.DEPOSIT_IN: transactionCode = Constant.TransactionCode.IMAGING_DEPOSIT_IN; break;
                                                            case Constant.PaymentType.DOWN_PAYMENT: transactionCode = Constant.TransactionCode.IMAGING_PATIENT_PAYMENT_DP; break;
                                                            case Constant.PaymentType.SETTLEMENT: transactionCode = Constant.TransactionCode.IMAGING_PATIENT_PAYMENT_SETTLEMENT; break;
                                                            case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.IMAGING_PATIENT_PAYMENT_AR_PATIENT; break;
                                                            case Constant.PaymentType.CUSTOM: transactionCode = Constant.TransactionCode.IMAGING_PATIENT_PAYMENT_CUSTOM; break;
                                                            //case Constant.PaymentType.PAYMENT_RETURN: transactionCode = Constant.TransactionCode.IMAGING_PATIENT_PAYMENT_RETURN; break;
                                                            default: transactionCode = Constant.TransactionCode.IMAGING_PATIENT_PAYMENT_AR_PAYER; break;
                                                        }
                                                    }
                                                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                                                    {
                                                        switch (entityHd.GCPaymentType)
                                                        {
                                                            case Constant.PaymentType.DEPOSIT_OUT: transactionCode = Constant.TransactionCode.RADIOTHERAPHY_DEPOSIT_OUT; break;
                                                            case Constant.PaymentType.DEPOSIT_IN: transactionCode = Constant.TransactionCode.RADIOTHERAPHY_DEPOSIT_IN; break;
                                                            case Constant.PaymentType.DOWN_PAYMENT: transactionCode = Constant.TransactionCode.RADIOTHERAPHY_PATIENT_PAYMENT_DP; break;
                                                            case Constant.PaymentType.SETTLEMENT: transactionCode = Constant.TransactionCode.RADIOTHERAPHY_PATIENT_PAYMENT_SETTLEMENT; break;
                                                            case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.RADIOTHERAPHY_PATIENT_PAYMENT_AR_PATIENT; break;
                                                            case Constant.PaymentType.CUSTOM: transactionCode = Constant.TransactionCode.RADIOTHERAPHY_PATIENT_PAYMENT_CUSTOM; break;
                                                            default: transactionCode = Constant.TransactionCode.RADIOTHERAPHY_PATIENT_PAYMENT_AR_PAYER; break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        switch (entityHd.GCPaymentType)
                                                        {
                                                            case Constant.PaymentType.DEPOSIT_OUT: transactionCode = Constant.TransactionCode.OTHER_DEPOSIT_OUT; break;
                                                            case Constant.PaymentType.DEPOSIT_IN: transactionCode = Constant.TransactionCode.OTHER_DEPOSIT_IN; break;
                                                            case Constant.PaymentType.DOWN_PAYMENT: transactionCode = Constant.TransactionCode.OTHER_PATIENT_PAYMENT_DP; break;
                                                            case Constant.PaymentType.SETTLEMENT: transactionCode = Constant.TransactionCode.OTHER_PATIENT_PAYMENT_SETTLEMENT; break;
                                                            case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.OTHER_PATIENT_PAYMENT_AR_PATIENT; break;
                                                            case Constant.PaymentType.CUSTOM: transactionCode = Constant.TransactionCode.OTHER_PATIENT_PAYMENT_CUSTOM; break;
                                                            //case Constant.PaymentType.PAYMENT_RETURN: transactionCode = Constant.TransactionCode.OTHER_PATIENT_PAYMENT_RETURN; break;
                                                            default: transactionCode = Constant.TransactionCode.OTHER_PATIENT_PAYMENT_AR_PAYER; break;
                                                        }
                                                    } break;
                                                default:
                                                    switch (entityHd.GCPaymentType)
                                                    {
                                                        case Constant.PaymentType.DEPOSIT_OUT: transactionCode = Constant.TransactionCode.OP_DEPOSIT_OUT; break;
                                                        case Constant.PaymentType.DEPOSIT_IN: transactionCode = Constant.TransactionCode.OP_DEPOSIT_IN; break;
                                                        case Constant.PaymentType.DOWN_PAYMENT: transactionCode = Constant.TransactionCode.OP_PATIENT_PAYMENT_DP; break;
                                                        case Constant.PaymentType.SETTLEMENT: transactionCode = Constant.TransactionCode.OP_PATIENT_PAYMENT_SETTLEMENT; break;
                                                        case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.OP_PATIENT_PAYMENT_AR_PATIENT; break;
                                                        case Constant.PaymentType.CUSTOM: transactionCode = Constant.TransactionCode.OP_PATIENT_PAYMENT_CUSTOM; break;
                                                        //case Constant.PaymentType.PAYMENT_RETURN: transactionCode = Constant.TransactionCode.OP_PATIENT_PAYMENT_RETURN; break;
                                                        default: transactionCode = Constant.TransactionCode.OP_PATIENT_PAYMENT_AR_PAYER; break;
                                                    } break;
                                            }

                                            entityHd.PaymentNo = BusinessLayer.GenerateTransactionNo(transactionCode, entityHd.PaymentDate, ctx);
                                            entityHd.CreatedBy = AppSession.UserLogin.UserID;

                                            #region bridging  Maspion
                                            if (hdnIsBridgingToMaspion.Value == "1")
                                            {
                                                //hanya uang cash
                                                if (checkCash > 0)
                                                {
                                                    entityHd.GCBridgingStatus = Constant.MaspionProcessStatus.OPEN;
                                                }
                                            }
                                            #endregion
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            entityHd.PaymentID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
                                            #endregion

                                            #region Payment Dt
                                            if (entityHd.GCPaymentType == Constant.PaymentType.SETTLEMENT || entityHd.GCPaymentType == Constant.PaymentType.CUSTOM || entityHd.GCPaymentType == Constant.PaymentType.DOWN_PAYMENT || entityHd.GCPaymentType == Constant.PaymentType.DEPOSIT_IN || entityHd.GCPaymentType == Constant.PaymentType.DEPOSIT_OUT)
                                            {
                                                foreach (String param in listParam)
                                                {
                                                    String[] data = param.Split(';');
                                                    bool isChanged = data[0] == "1" ? true : false;
                                                    int ID = Convert.ToInt32(data[1]);
                                                    if (isChanged || ID > 0)
                                                    {
                                                        PatientPaymentDt entityDt = new PatientPaymentDt();
                                                        entityDt.PaymentID = entityHd.PaymentID;
                                                        entityDt.GCPaymentMethod = data[2];
                                                        if (entityDt.GCPaymentMethod != Constant.PaymentMethod.CASH)
                                                        {
                                                            if (data[3] != "")
                                                                entityDt.EDCMachineID = Convert.ToInt32(data[3]);
                                                            else
                                                                entityDt.EDCMachineID = null;
                                                            if (data[5] != "")
                                                                entityDt.BankID = Convert.ToInt32(data[5]);
                                                            else
                                                                entityDt.BankID = null;
                                                            entityDt.ReferenceNo = data[6];
                                                            entityDt.GCCardType = data[10];

                                                            if (data[11] != "")
                                                                entityDt.CardNumber = string.Format("{0}-XXXX-XXXX-{1}", data[11], data[12]);
                                                            else
                                                                entityDt.CardNumber = "";

                                                            entityDt.CardHolderName = data[13];

                                                            if (data[14] != "" && data[15] != "")
                                                                entityDt.CardValidThru = string.Format("{0:00}/{1:00}", data[14].PadLeft(2, '0'), data[15].Substring(2));
                                                            else
                                                                entityDt.CardValidThru = "";

                                                            entityDt.GCCardProvider = data[17];

                                                            if (data[18] != "")
                                                            {
                                                                entityDt.BatchNo = data[18];
                                                            }
                                                            if (data[19] != "")
                                                            {
                                                                entityDt.TraceNo = data[19];
                                                            }

                                                            if (data[21] != "")
                                                            {
                                                                entityDt.ApprovalCode = data[21];
                                                            }
                                                            if (data[22] != "")
                                                            {
                                                                entityDt.TerminalID = data[22];
                                                            }
                                                        }
                                                        entityDt.PaymentAmount = Convert.ToDecimal(data[7].Replace(",", ""));
                                                        entityDt.CardFeeAmount = Convert.ToDecimal(data[8].Replace(",", ""));
                                                        entityDt.CreatedBy = AppSession.UserLogin.UserID;
                                                        ctx.CommandType = CommandType.Text;
                                                        ctx.Command.Parameters.Clear();
                                                        int paymentDetailID = entityDtDao.InsertReturnPrimaryKeyID(entityDt);

                                                        PatientPaymentDtInfo dtInfo = new PatientPaymentDtInfo();
                                                        dtInfo.PaymentDetailID = paymentDetailID;
                                                        dtInfo.GCClaimStatus = Constant.ClaimStatus.APPROVED;
                                                        dtInfo.GCFinalStatus = Constant.FinalStatus.APPROVED;
                                                        dtInfo.GrouperAmountClaim = dtInfo.GrouperAmountFinal = entityDt.PaymentAmount;
                                                        dtInfo.ClaimBy = dtInfo.FinalBy = AppSession.UserLogin.UserID;
                                                        dtInfo.ClaimDate = dtInfo.FinalDate = DateTime.Now;
                                                        dtInfo.SequenceNo = Convert.ToInt32(entityRegistration.RegistrationDate.ToString("dd"));
                                                        ctx.CommandType = CommandType.Text;
                                                        ctx.Command.Parameters.Clear();
                                                        entityDtInfoDao.Insert(dtInfo);

                                                        RegistrationBPJS regBPJS = registrationBPJSDao.Get(entityHd.RegistrationID);
                                                        if (regBPJS != null)
                                                        {
                                                            regBPJS.GCClaimStatus = dtInfo.GCClaimStatus;
                                                            regBPJS.ClaimBy = dtInfo.ClaimBy;
                                                            regBPJS.ClaimDate = dtInfo.ClaimDate;
                                                            regBPJS.GCFinalStatus = dtInfo.GCFinalStatus;
                                                            regBPJS.FinalBy = dtInfo.FinalBy;
                                                            if (dtInfo.FinalBy != null) { regBPJS.FinalDate = dtInfo.FinalDate; }
                                                            ctx.CommandType = CommandType.Text;
                                                            ctx.Command.Parameters.Clear();
                                                            registrationBPJSDao.Update(regBPJS);
                                                        }
                                                    }
                                                }
                                            }
                                            //else if (entityHd.GCPaymentType == Constant.PaymentType.PAYMENT_RETURN)
                                            //{
                                            //    PatientPaymentDt entityDt = new PatientPaymentDt();
                                            //    entityDt.PaymentID = entityHd.PaymentID;
                                            //    entityDt.GCPaymentMethod = Constant.PaymentMethod.PAYMENT_RETURN;

                                            //    entityDt.PaymentAmount = entityHd.TotalPaymentAmount;
                                            //    entityDt.CardFeeAmount = 0;
                                            //    entityDt.CreatedBy = AppSession.UserLogin.UserID;
                                            //    entityDtDao.Insert(entityDt);
                                            //}
                                            else if (entityHd.GCPaymentType == Constant.PaymentType.AR_PAYER)
                                            {
                                                foreach (String param in listParam2)
                                                {
                                                    String[] data = param.Split(';');
                                                    bool isChanged = data[0] == "1" ? true : false;
                                                    if (isChanged)
                                                    {
                                                        PatientPaymentDt entityDt = new PatientPaymentDt();
                                                        entityDt.PaymentID = entityHd.PaymentID;
                                                        entityDt.GCPaymentMethod = Constant.PaymentMethod.CREDIT;
                                                        entityDt.BusinessPartnerID = Convert.ToInt32(data[2]);
                                                        entityDt.PaymentAmount = Convert.ToDecimal(data[3].Replace(",", ""));
                                                        entityDt.CardFeeAmount = 0;
                                                        entityDt.CreatedBy = AppSession.UserLogin.UserID;
                                                        int paymentDetailID = entityDtDao.InsertReturnPrimaryKeyID(entityDt);

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

                                                                dtInfo.SequenceNo = Convert.ToInt32(entityRegistration.RegistrationDate.ToString("dd"));
                                                                ctx.CommandType = CommandType.Text;
                                                                ctx.Command.Parameters.Clear();
                                                                entityDtInfoDao.Insert(dtInfo);

                                                                RegistrationBPJS regBPJS = registrationBPJSDao.Get(entityHd.RegistrationID);
                                                                if (regBPJS != null)
                                                                {
                                                                    regBPJS.GCClaimStatus = dtInfo.GCClaimStatus;
                                                                    regBPJS.ClaimBy = dtInfo.ClaimBy;
                                                                    regBPJS.ClaimDate = dtInfo.ClaimDate;
                                                                    regBPJS.GCFinalStatus = dtInfo.GCFinalStatus;
                                                                    regBPJS.FinalBy = dtInfo.FinalBy;
                                                                    if (dtInfo.FinalBy != null) { regBPJS.FinalDate = dtInfo.FinalDate; }
                                                                    ctx.CommandType = CommandType.Text;
                                                                    ctx.Command.Parameters.Clear();
                                                                    registrationBPJSDao.Update(regBPJS);
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
                                                                dtInfo.SequenceNo = Convert.ToInt32(entityRegistration.RegistrationDate.ToString("dd"));
                                                                ctx.CommandType = CommandType.Text;
                                                                ctx.Command.Parameters.Clear();
                                                                entityDtInfoDao.Insert(dtInfo);

                                                                RegistrationBPJS regBPJS = registrationBPJSDao.Get(entityHd.RegistrationID);
                                                                if (regBPJS != null)
                                                                {
                                                                    regBPJS.GCClaimStatus = dtInfo.GCClaimStatus;
                                                                    regBPJS.ClaimBy = dtInfo.ClaimBy;
                                                                    regBPJS.ClaimDate = dtInfo.ClaimDate;
                                                                    regBPJS.GCFinalStatus = dtInfo.GCFinalStatus;
                                                                    regBPJS.FinalBy = dtInfo.FinalBy;
                                                                    if (dtInfo.FinalBy != null) { regBPJS.FinalDate = dtInfo.FinalDate; }
                                                                    ctx.CommandType = CommandType.Text;
                                                                    ctx.Command.Parameters.Clear();
                                                                    registrationBPJSDao.Update(regBPJS);
                                                                }
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
                                                            dtInfo.SequenceNo = Convert.ToInt32(entityRegistration.RegistrationDate.ToString("dd"));
                                                            ctx.CommandType = CommandType.Text;
                                                            ctx.Command.Parameters.Clear();
                                                            entityDtInfoDao.Insert(dtInfo);

                                                            RegistrationBPJS regBPJS = registrationBPJSDao.Get(entityHd.RegistrationID);
                                                            if (regBPJS != null)
                                                            {
                                                                regBPJS.GCClaimStatus = dtInfo.GCClaimStatus;
                                                                regBPJS.ClaimBy = dtInfo.ClaimBy;
                                                                regBPJS.ClaimDate = dtInfo.ClaimDate;
                                                                regBPJS.GCFinalStatus = dtInfo.GCFinalStatus;
                                                                regBPJS.FinalBy = dtInfo.FinalBy;
                                                                if (dtInfo.FinalBy != null) { regBPJS.FinalDate = dtInfo.FinalDate; }
                                                                ctx.CommandType = CommandType.Text;
                                                                ctx.Command.Parameters.Clear();
                                                                registrationBPJSDao.Update(regBPJS);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else if (entityHd.GCPaymentType == Constant.PaymentType.AR_PATIENT)
                                            {
                                                decimal dpAmount = Convert.ToDecimal(hdnOutstandingDP.Value.ToString().Replace(",", ""));
                                                if (dpAmount != 0)
                                                {
                                                    for (int i = 0; i < 2; i++)
                                                    {
                                                        PatientPaymentDt entityDt = new PatientPaymentDt();
                                                        entityDt.PaymentID = entityHd.PaymentID;

                                                        string filterBP = string.Format("BusinessPartnerID = (SELECT BusinessPartnerID FROM Customer WHERE GCCustomerType = '{0}')", Constant.CustomerType.PERSONAL);
                                                        BusinessPartners bp = BusinessLayer.GetBusinessPartnersList(filterBP, ctx).FirstOrDefault();
                                                        entityDt.BusinessPartnerID = bp.BusinessPartnerID;

                                                        if (i == 0)
                                                        {
                                                            entityDt.GCPaymentMethod = Constant.PaymentMethod.DOWN_PAYMENT;
                                                            entityDt.PaymentAmount = Convert.ToDecimal(hdnOutstandingDP.Value.ToString().Replace(",", ""));
                                                        }
                                                        else
                                                        {
                                                            entityDt.GCPaymentMethod = Constant.PaymentMethod.CREDIT;
                                                            entityDt.PaymentAmount = Convert.ToDecimal(hdnARPatientWithoutDP.Value.ToString().Replace(",", ""));
                                                        }
                                                        entityDt.CardFeeAmount = 0;
                                                        entityDt.CreatedBy = AppSession.UserLogin.UserID;
                                                        ctx.CommandType = CommandType.Text;
                                                        ctx.Command.Parameters.Clear();
                                                        int paymentDetailID = entityDtDao.InsertReturnPrimaryKeyID(entityDt);

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

                                                                dtInfo.SequenceNo = Convert.ToInt32(entityRegistration.RegistrationDate.ToString("dd"));
                                                                ctx.CommandType = CommandType.Text;
                                                                ctx.Command.Parameters.Clear();
                                                                entityDtInfoDao.Insert(dtInfo);

                                                                RegistrationBPJS regBPJS = registrationBPJSDao.Get(entityHd.RegistrationID);
                                                                if (regBPJS != null)
                                                                {
                                                                    regBPJS.GCClaimStatus = dtInfo.GCClaimStatus;
                                                                    regBPJS.ClaimBy = dtInfo.ClaimBy;
                                                                    regBPJS.ClaimDate = dtInfo.ClaimDate;
                                                                    regBPJS.GCFinalStatus = dtInfo.GCFinalStatus;
                                                                    regBPJS.FinalBy = dtInfo.FinalBy;
                                                                    if (dtInfo.FinalBy != null) { regBPJS.FinalDate = dtInfo.FinalDate; }
                                                                    ctx.CommandType = CommandType.Text;
                                                                    ctx.Command.Parameters.Clear();
                                                                    registrationBPJSDao.Update(regBPJS);
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
                                                                dtInfo.SequenceNo = Convert.ToInt32(entityRegistration.RegistrationDate.ToString("dd"));
                                                                ctx.CommandType = CommandType.Text;
                                                                ctx.Command.Parameters.Clear();
                                                                entityDtInfoDao.Insert(dtInfo);

                                                                RegistrationBPJS regBPJS = registrationBPJSDao.Get(entityHd.RegistrationID);
                                                                if (regBPJS != null)
                                                                {
                                                                    regBPJS.GCClaimStatus = dtInfo.GCClaimStatus;
                                                                    regBPJS.ClaimBy = dtInfo.ClaimBy;
                                                                    regBPJS.ClaimDate = dtInfo.ClaimDate;
                                                                    regBPJS.GCFinalStatus = dtInfo.GCFinalStatus;
                                                                    regBPJS.FinalBy = dtInfo.FinalBy;
                                                                    if (dtInfo.FinalBy != null) { regBPJS.FinalDate = dtInfo.FinalDate; }
                                                                    ctx.CommandType = CommandType.Text;
                                                                    ctx.Command.Parameters.Clear();
                                                                    registrationBPJSDao.Update(regBPJS);
                                                                }
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
                                                            dtInfo.SequenceNo = Convert.ToInt32(entityRegistration.RegistrationDate.ToString("dd"));
                                                            ctx.CommandType = CommandType.Text;
                                                            ctx.Command.Parameters.Clear();
                                                            entityDtInfoDao.Insert(dtInfo);

                                                            RegistrationBPJS regBPJS = registrationBPJSDao.Get(entityHd.RegistrationID);
                                                            if (regBPJS != null)
                                                            {
                                                                regBPJS.GCClaimStatus = dtInfo.GCClaimStatus;
                                                                regBPJS.ClaimBy = dtInfo.ClaimBy;
                                                                regBPJS.ClaimDate = dtInfo.ClaimDate;
                                                                regBPJS.GCFinalStatus = dtInfo.GCFinalStatus;
                                                                regBPJS.FinalBy = dtInfo.FinalBy;
                                                                if (dtInfo.FinalBy != null) { regBPJS.FinalDate = dtInfo.FinalDate; }
                                                                ctx.CommandType = CommandType.Text;
                                                                ctx.Command.Parameters.Clear();
                                                                registrationBPJSDao.Update(regBPJS);
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    PatientPaymentDt entityDt = new PatientPaymentDt();
                                                    entityDt.PaymentID = entityHd.PaymentID;

                                                    string filterBP = string.Format("BusinessPartnerID = (SELECT BusinessPartnerID FROM Customer WHERE GCCustomerType = '{0}')", Constant.CustomerType.PERSONAL);
                                                    BusinessPartners bp = BusinessLayer.GetBusinessPartnersList(filterBP, ctx).FirstOrDefault();
                                                    entityDt.BusinessPartnerID = bp.BusinessPartnerID;

                                                    entityDt.GCPaymentMethod = Constant.PaymentMethod.CREDIT;
                                                    entityDt.PaymentAmount = Convert.ToDecimal(hdnARPatientWithoutDP.Value.ToString().Replace(",", ""));

                                                    entityDt.CardFeeAmount = 0;
                                                    entityDt.CreatedBy = AppSession.UserLogin.UserID;
                                                    ctx.CommandType = CommandType.Text;
                                                    ctx.Command.Parameters.Clear();
                                                    int paymentDetailID = entityDtDao.InsertReturnPrimaryKeyID(entityDt);

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

                                                            dtInfo.SequenceNo = Convert.ToInt32(entityRegistration.RegistrationDate.ToString("dd"));
                                                            ctx.CommandType = CommandType.Text;
                                                            ctx.Command.Parameters.Clear();
                                                            entityDtInfoDao.Insert(dtInfo);

                                                            RegistrationBPJS regBPJS = registrationBPJSDao.Get(entityHd.RegistrationID);
                                                            if (regBPJS != null)
                                                            {
                                                                regBPJS.GCClaimStatus = dtInfo.GCClaimStatus;
                                                                regBPJS.ClaimBy = dtInfo.ClaimBy;
                                                                regBPJS.ClaimDate = dtInfo.ClaimDate;
                                                                regBPJS.GCFinalStatus = dtInfo.GCFinalStatus;
                                                                regBPJS.FinalBy = dtInfo.FinalBy;
                                                                if (dtInfo.FinalBy != null) { regBPJS.FinalDate = dtInfo.FinalDate; }
                                                                ctx.CommandType = CommandType.Text;
                                                                ctx.Command.Parameters.Clear();
                                                                registrationBPJSDao.Update(regBPJS);
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
                                                            dtInfo.SequenceNo = Convert.ToInt32(entityRegistration.RegistrationDate.ToString("dd"));
                                                            ctx.CommandType = CommandType.Text;
                                                            ctx.Command.Parameters.Clear();
                                                            entityDtInfoDao.Insert(dtInfo);

                                                            RegistrationBPJS regBPJS = registrationBPJSDao.Get(entityHd.RegistrationID);
                                                            if (regBPJS != null)
                                                            {
                                                                regBPJS.GCClaimStatus = dtInfo.GCClaimStatus;
                                                                regBPJS.ClaimBy = dtInfo.ClaimBy;
                                                                regBPJS.ClaimDate = dtInfo.ClaimDate;
                                                                regBPJS.GCFinalStatus = dtInfo.GCFinalStatus;
                                                                regBPJS.FinalBy = dtInfo.FinalBy;
                                                                if (dtInfo.FinalBy != null) { regBPJS.FinalDate = dtInfo.FinalDate; }
                                                                ctx.CommandType = CommandType.Text;
                                                                ctx.Command.Parameters.Clear();
                                                                registrationBPJSDao.Update(regBPJS);
                                                            }
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
                                                        dtInfo.SequenceNo = Convert.ToInt32(entityRegistration.RegistrationDate.ToString("dd"));
                                                        ctx.CommandType = CommandType.Text;
                                                        ctx.Command.Parameters.Clear();
                                                        entityDtInfoDao.Insert(dtInfo);

                                                        RegistrationBPJS regBPJS = registrationBPJSDao.Get(entityHd.RegistrationID);
                                                        if (regBPJS != null)
                                                        {
                                                            regBPJS.GCClaimStatus = dtInfo.GCClaimStatus;
                                                            regBPJS.ClaimBy = dtInfo.ClaimBy;
                                                            regBPJS.ClaimDate = dtInfo.ClaimDate;
                                                            regBPJS.GCFinalStatus = dtInfo.GCFinalStatus;
                                                            regBPJS.FinalBy = dtInfo.FinalBy;
                                                            if (dtInfo.FinalBy != null) { regBPJS.FinalDate = dtInfo.FinalDate; }
                                                            ctx.CommandType = CommandType.Text;
                                                            ctx.Command.Parameters.Clear();
                                                            registrationBPJSDao.Update(regBPJS);
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                PatientPaymentDt entityDt = new PatientPaymentDt();
                                                entityDt.PaymentID = entityHd.PaymentID;

                                                string filterBP = string.Format("BusinessPartnerID = (SELECT BusinessPartnerID FROM Customer WHERE GCCustomerType = '{0}')", Constant.CustomerType.PERSONAL);
                                                BusinessPartners bp = BusinessLayer.GetBusinessPartnersList(filterBP, ctx).FirstOrDefault();
                                                entityDt.BusinessPartnerID = bp.BusinessPartnerID;

                                                entityDt.GCPaymentMethod = Constant.PaymentMethod.CREDIT;
                                                entityDt.PaymentAmount = entityHd.TotalPaymentAmount;
                                                entityDt.CardFeeAmount = 0;
                                                entityDt.CreatedBy = AppSession.UserLogin.UserID;
                                                int paymentDetailID = entityDtDao.InsertReturnPrimaryKeyID(entityDt);

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

                                                        dtInfo.SequenceNo = Convert.ToInt32(entityRegistration.RegistrationDate.ToString("dd"));
                                                        ctx.CommandType = CommandType.Text;
                                                        ctx.Command.Parameters.Clear();
                                                        entityDtInfoDao.Insert(dtInfo);

                                                        RegistrationBPJS regBPJS = registrationBPJSDao.Get(entityHd.RegistrationID);
                                                        if (regBPJS != null)
                                                        {
                                                            regBPJS.GCClaimStatus = dtInfo.GCClaimStatus;
                                                            regBPJS.ClaimBy = dtInfo.ClaimBy;
                                                            regBPJS.ClaimDate = dtInfo.ClaimDate;
                                                            regBPJS.GCFinalStatus = dtInfo.GCFinalStatus;
                                                            regBPJS.FinalBy = dtInfo.FinalBy;
                                                            if (dtInfo.FinalBy != null) { regBPJS.FinalDate = dtInfo.FinalDate; }
                                                            ctx.CommandType = CommandType.Text;
                                                            ctx.Command.Parameters.Clear();
                                                            registrationBPJSDao.Update(regBPJS);
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
                                                        dtInfo.SequenceNo = Convert.ToInt32(entityRegistration.RegistrationDate.ToString("dd"));
                                                        ctx.CommandType = CommandType.Text;
                                                        ctx.Command.Parameters.Clear();
                                                        entityDtInfoDao.Insert(dtInfo);

                                                        RegistrationBPJS regBPJS = registrationBPJSDao.Get(entityHd.RegistrationID);
                                                        if (regBPJS != null)
                                                        {
                                                            regBPJS.GCClaimStatus = dtInfo.GCClaimStatus;
                                                            regBPJS.ClaimBy = dtInfo.ClaimBy;
                                                            regBPJS.ClaimDate = dtInfo.ClaimDate;
                                                            regBPJS.GCFinalStatus = dtInfo.GCFinalStatus;
                                                            regBPJS.FinalBy = dtInfo.FinalBy;
                                                            if (dtInfo.FinalBy != null) { regBPJS.FinalDate = dtInfo.FinalDate; }
                                                            ctx.CommandType = CommandType.Text;
                                                            ctx.Command.Parameters.Clear();
                                                            registrationBPJSDao.Update(regBPJS);
                                                        }
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
                                                    dtInfo.SequenceNo = Convert.ToInt32(entityRegistration.RegistrationDate.ToString("dd"));
                                                    ctx.CommandType = CommandType.Text;
                                                    ctx.Command.Parameters.Clear();
                                                    entityDtInfoDao.Insert(dtInfo);

                                                    RegistrationBPJS regBPJS = registrationBPJSDao.Get(entityHd.RegistrationID);
                                                    if (regBPJS != null)
                                                    {
                                                        regBPJS.GCClaimStatus = dtInfo.GCClaimStatus;
                                                        regBPJS.ClaimBy = dtInfo.ClaimBy;
                                                        regBPJS.ClaimDate = dtInfo.ClaimDate;
                                                        regBPJS.GCFinalStatus = dtInfo.GCFinalStatus;
                                                        regBPJS.FinalBy = dtInfo.FinalBy;
                                                        if (dtInfo.FinalBy != null) { regBPJS.FinalDate = dtInfo.FinalDate; }
                                                        ctx.CommandType = CommandType.Text;
                                                        ctx.Command.Parameters.Clear();
                                                        registrationBPJSDao.Update(regBPJS);
                                                    }
                                                }
                                            }
                                            #endregion

                                            #region Update Billing
                                            if (hdnListBillingID.Value != "")
                                            {
                                                List<PatientChargesHd> lstPatientChargesHd = BusinessLayer.GetPatientChargesHdList(string.Format("PatientBillingID IN ({0})", hdnListBillingID.Value), ctx);
                                                decimal totalPaymentAmount = entityHd.TotalPaymentAmount;
                                                string oldBillStatus = null;
                                                foreach (PatientBill patientBill in lstPatientBill)
                                                {
                                                    //List<PatientBillPayment> lstPatientBillPaymentPerBilling = BusinessLayer.GetPatientBillPaymentList(String.Format(
                                                    //                                                                            "PatientBillingID = {0} AND PaymentID IN (SELECT pph.PaymentID FROM PatientPaymentHd pph WHERE pph.RegistrationID = {1} AND pph.GCTransactionStatus <> '{2}')",
                                                    //                                                                            patientBill.PatientBillingID,
                                                    //                                                                            patientBill.RegistrationID,
                                                    //                                                                            Constant.TransactionStatus.VOID
                                                    //                                                                        ), ctx);
                                                    //decimal patientRoundingSumAmount = lstPatientBillPaymentPerBilling.Sum(z => z.PatientRoundingAmount);
                                                    //decimal patientPaymentSumAmount = lstPatientBillPaymentPerBilling.Sum(z => z.PatientPaymentAmount);
                                                    //decimal payerRoundingSumAmount = lstPatientBillPaymentPerBilling.Sum(z => z.PayerRoundingAmount);
                                                    //decimal payerPaymentSumAmount = lstPatientBillPaymentPerBilling.Sum(z => z.PayerPaymentAmount);

                                                    oldBillStatus = patientBill.GCTransactionStatus;

                                                    patientBill.PaymentID = entityHd.PaymentID;

                                                    PatientBillPayment patientBillPayment = new PatientBillPayment();
                                                    patientBillPayment.PaymentID = entityHd.PaymentID;
                                                    patientBillPayment.PatientBillingID = patientBill.PatientBillingID;

                                                    if (entityHd.GCPaymentType == Constant.PaymentType.AR_PAYER)
                                                    {
                                                        //decimal totalRemainingOthers = lstPatientBill.Where(a => a.PatientBillingID != patientBill.PatientBillingID).ToList().Sum(b => b.PayerRemainingAmount);
                                                        if (patientBill.PayerRemainingAmount != 0)
                                                        {
                                                            //if (patientBill.PayerRemainingAmount > 0 && totalRemainingOthers > 0)
                                                            if (patientBill.PayerRemainingAmount > totalPaymentAmount)
                                                            {
                                                                //if (patientBill.PayerRemainingAmount > totalPaymentAmount)
                                                                //{
                                                                //    patientBill.TotalPayerPaymentAmount += totalPaymentAmount;
                                                                //    patientBillPayment.PayerPaymentAmount = totalPaymentAmount;
                                                                //    totalPaymentAmount = 0;
                                                                //}
                                                                //else
                                                                //{
                                                                //    decimal payerRemainingAmount = patientBill.PayerRemainingAmount;
                                                                //    patientBill.TotalPayerPaymentAmount += payerRemainingAmount;
                                                                //    patientBillPayment.PayerPaymentAmount = payerRemainingAmount;
                                                                //    totalPaymentAmount = totalPaymentAmount - patientBill.TotalPayerPaymentAmount;
                                                                //}
                                                                patientBill.TotalPayerPaymentAmount += totalPaymentAmount;
                                                                patientBillPayment.PayerPaymentAmount = totalPaymentAmount;
                                                                totalPaymentAmount = 0;
                                                            }
                                                            //else if (patientBill.PayerRemainingAmount > 0 && totalRemainingOthers < 0)
                                                            //{
                                                            //    if (patientBill.PayerRemainingAmount > (totalPaymentAmount - totalRemainingOthers))
                                                            //    {
                                                            //        patientBill.TotalPayerPaymentAmount += (totalPaymentAmount - totalRemainingOthers);
                                                            //        patientBillPayment.PayerPaymentAmount = (totalPaymentAmount - totalRemainingOthers);
                                                            //        totalPaymentAmount = 0;
                                                            //    }
                                                            //    else
                                                            //    {
                                                            //        decimal payerRemainingAmount = patientBill.PayerRemainingAmount;
                                                            //        patientBill.TotalPayerPaymentAmount += payerRemainingAmount;
                                                            //        patientBillPayment.PayerPaymentAmount = payerRemainingAmount;
                                                            //        totalPaymentAmount = totalPaymentAmount - patientBill.TotalPayerPaymentAmount;
                                                            //    }
                                                            //}
                                                            else
                                                            {
                                                                //if (patientBill.PayerRemainingAmount < totalPaymentAmount)
                                                                //{
                                                                //    patientBill.TotalPayerPaymentAmount += totalPaymentAmount;
                                                                //    patientBillPayment.PayerPaymentAmount = totalPaymentAmount;
                                                                //    totalPaymentAmount = 0;
                                                                //}
                                                                //else
                                                                //{
                                                                //    decimal payerRemainingAmount = patientBill.PayerRemainingAmount;
                                                                //    patientBill.TotalPayerPaymentAmount += payerRemainingAmount;
                                                                //    patientBillPayment.PayerPaymentAmount = payerRemainingAmount;
                                                                //    totalPaymentAmount = totalPaymentAmount + patientBill.TotalPayerPaymentAmount;
                                                                //}
                                                                decimal payerRemainingAmount = patientBill.PayerRemainingAmount;
                                                                patientBill.TotalPayerPaymentAmount += payerRemainingAmount;
                                                                patientBillPayment.PayerPaymentAmount = payerRemainingAmount;
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
                                                        decimal totalRemainingOthers = lstPatientBill.Where(a => a.PatientBillingID != patientBill.PatientBillingID).ToList().Sum(b => b.PatientRemainingAmount);
                                                        if (patientBill.PatientRemainingAmount != 0)
                                                        {
                                                            if (patientBill.PatientRemainingAmount > 0 && totalRemainingOthers > 0)
                                                            {
                                                                if (patientBill.PatientRemainingAmount > totalPaymentAmount)
                                                                {
                                                                    patientBillPayment.PatientPaymentAmount = patientBill.PatientRemainingAmount;
                                                                    patientBill.TotalPatientPaymentAmount += totalPaymentAmount;
                                                                    totalPaymentAmount = 0;
                                                                }
                                                                else
                                                                {
                                                                    decimal patientRemainingAmount = patientBill.PatientRemainingAmount;
                                                                    patientBill.TotalPatientPaymentAmount += patientRemainingAmount;
                                                                    patientBillPayment.PatientPaymentAmount = patientRemainingAmount;
                                                                    totalPaymentAmount = totalPaymentAmount - patientBill.TotalPatientPaymentAmount;
                                                                }
                                                            }
                                                            else if (patientBill.PatientRemainingAmount > 0 && totalRemainingOthers < 0)
                                                            {
                                                                if (patientBill.PatientRemainingAmount > (totalPaymentAmount - totalRemainingOthers))
                                                                {
                                                                    patientBill.TotalPatientPaymentAmount += (totalPaymentAmount - totalRemainingOthers);
                                                                    patientBillPayment.PatientPaymentAmount = (totalPaymentAmount - totalRemainingOthers);
                                                                    totalPaymentAmount = 0;
                                                                }
                                                                else
                                                                {
                                                                    decimal patientRemainingAmount = patientBill.PatientRemainingAmount;
                                                                    patientBill.TotalPatientPaymentAmount += patientRemainingAmount;
                                                                    patientBillPayment.PatientPaymentAmount = patientRemainingAmount;
                                                                    totalPaymentAmount = totalPaymentAmount - patientBill.TotalPatientPaymentAmount;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (patientBill.PatientRemainingAmount < totalPaymentAmount)
                                                                {
                                                                    patientBill.TotalPatientPaymentAmount += totalPaymentAmount;
                                                                    patientBillPayment.PatientPaymentAmount = totalPaymentAmount;
                                                                    totalPaymentAmount = 0;
                                                                }
                                                                else
                                                                {
                                                                    decimal patientRemainingAmount = patientBill.PatientRemainingAmount;
                                                                    patientBill.TotalPatientPaymentAmount += patientRemainingAmount;
                                                                    patientBillPayment.PatientPaymentAmount = patientRemainingAmount;
                                                                    totalPaymentAmount = totalPaymentAmount + patientBill.TotalPatientPaymentAmount;
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            patientBill.TotalPatientPaymentAmount += 0;
                                                            patientBillPayment.PatientPaymentAmount = 0;
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
                                                                    //patientBill.TotalPatientPaymentAmount += patientBillPayment.PatientRoundingAmount;
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
                                                        lstChargesHd = lstPatientChargesHd.Where(p => p.PatientBillingID == patientBill.PatientBillingID).ToList();
                                                        foreach (PatientChargesHd patientChargesHd in lstChargesHd)
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
                                                }
                                            }
                                            #endregion

                                            #region Auto Close Registration
                                            //if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
                                            //{
                                            //    ConsultVisit consultVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", hdnRegistrationID.Value), ctx).FirstOrDefault();
                                            //    HealthcareServiceUnit healthcareServiceUnit = healthcareServiceUnitDao.Get(consultVisit.HealthcareServiceUnitID);
                                            //    if (healthcareServiceUnit.IsAutoCloseRegistration)
                                            //    {
                                            //        int count = BusinessLayer.GetvPatientChargesHdRowCount(string.Format("VisitID = {0} AND GCTransactionStatus NOT IN ('{1},'{2}')", consultVisit.VisitID, Constant.TransactionStatus.VOID, Constant.TransactionStatus.CLOSED));
                                            //        if (count < 0)
                                            //        {
                                            //            Registration registration = registrationDao.Get(Convert.ToInt32(hdnRegistrationID.Value));
                                            //            registration.GCRegistrationStatus = Constant.RegistrationStatus.CLOSED;
                                            //            registration.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            //            registrationDao.Update(registration);
                                            //        }
                                            //    }
                                            //}
                                            retval = "";
                                            vRegistrationOutstandingInfo vRegistrationOutstandingInfo = BusinessLayer.GetvRegistrationOutstandingInfoList(string.Format("RegistrationID = {0}", hdnRegistrationID.Value), ctx).FirstOrDefault();
                                            if (vRegistrationOutstandingInfo.Billing > 0)
                                                retval = "ar";
                                            //else
                                            //{
                                            //    ConsultVisit consultVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", hdnRegistrationID.Value), ctx).FirstOrDefault();
                                            //    HealthcareServiceUnit healthcareServiceUnit = healthcareServiceUnitDao.Get(consultVisit.HealthcareServiceUnitID);
                                            //    if (healthcareServiceUnit.IsAutoCloseRegistration && vRegistrationOutstandingInfo.Billing < 1 && vRegistrationOutstandingInfo.Charges < 1)
                                            //        retval = "closed";
                                            //}
                                            #endregion

                                            ctx.CommitTransaction();

                                            #region Bridging

                                            if (hdnIsSendToLISAfterSavePayment.Value == "1")
                                            {
                                                if (AppSession.IsBridgingToLIS)
                                                {
                                                    string[] resultInfo = "0|Unknown Protocol".Split('|');
                                                    switch (AppSession.LIS_BRIDGING_PROTOCOL)
                                                    {
                                                        case Constant.LIS_Bridging_Protocol.WEB_API:
                                                            switch (AppSession.LIS_PROVIDER)
                                                            {
                                                                case Constant.LIS_PROVIDER.SOFTMEDIX:
                                                                    SendLISOrderToSoftmedix(lstChargesHd);
                                                                    break;
                                                                default:
                                                                    break;
                                                            }
                                                            break;
                                                        case Constant.LIS_Bridging_Protocol.LINK_DB:
                                                            switch (AppSession.LIS_PROVIDER)
                                                            {
                                                                case Constant.LIS_PROVIDER.HCLAB:
                                                                    SendLISOrderToHCLAB(lstChargesHd);
                                                                    break;
                                                            }
                                                            break;
                                                        default:
                                                            resultInfo = "0|Unknown Protocol".Split('|');
                                                            break;
                                                    }
                                                }
                                            }
                                            #endregion

                                            #endregion
                                        }
                                    }
                                    else
                                    {
                                        result = false;
                                        errMessage = "Pembayaran selain Uang Muka / Deposit harus disertakan nomor tagihnya !";
                                        Exception ex = new Exception(errMessage);
                                        Helper.InsertErrorLog(ex);
                                        ctx.RollBackTransaction();
                                    }
                                }
                                else
                                {
                                    result = false;
                                    errMessage = "DEPOSIT OUT tidak dapat melebihi deposit pasien <b>" + AppSession.RegisteredPatient.PatientName + " (" + AppSession.RegisteredPatient.MedicalNo + ")</b> sejumlah <b>" + Convert.ToDecimal(hdnDepositBalanceEnd.Value).ToString(Constant.FormatString.NUMERIC_2) + "</b>";
                                    Exception ex = new Exception(errMessage);
                                    Helper.InsertErrorLog(ex);
                                    ctx.RollBackTransaction();
                                }
                            }
                            else
                            {
                                result = false;
                                errMessage = "Maaf, pembayaran DEPOSIT OUT tidak bisa lebih dari 1x.";
                                Exception ex = new Exception(errMessage);
                                Helper.InsertErrorLog(ex);
                                ctx.RollBackTransaction();
                            }
                        }
                    }
                    else
                    {
                        result = false;
                        errMessage = "Maaf, pembayaran CASH tidak bisa lebih dari 1x.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Maaf, untuk nilai sisa " + hdnCaptionDownPayment.Value + " tidak boleh kurang dari nol (atau minus).";
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

        protected bool OnSaveAddRecordFromEDC(ref string errMessage, ref string retval)
        {
            bool result = true;
            String[] listParam = hdnInlineEditingData.Value.Split('|');
            String[] listParam2 = hdnInlineEditingPayerData.Value.Split('|');

            if (!IsTransactionValid(ref errMessage))
            {
                return false;
            }

            IDbContext ctx = DbFactory.Configure(true);
            PatientPaymentHdDao entityHdDao = new PatientPaymentHdDao(ctx);
            PatientPaymentDtDao entityDtDao = new PatientPaymentDtDao(ctx);
            PatientBillPaymentDao patientBillPaymentDao = new PatientBillPaymentDao(ctx);
            PatientBillDao patientBillDao = new PatientBillDao(ctx);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            RegistrationDao registrationDao = new RegistrationDao(ctx);
            HealthcareServiceUnitDao healthcareServiceUnitDao = new HealthcareServiceUnitDao(ctx);
            EDCMachineTransactionDao edcDao = new EDCMachineTransactionDao(ctx);
            try
            {
                #region JUST SETTLEMENT

                #region Payment Hd
                PatientPaymentHd entityHd = new PatientPaymentHd();
                entityHd.PaymentDate = Helper.GetDatePickerValue(txtPaymentDate);
                entityHd.PaymentTime = txtPaymentTime.Text;
                entityHd.GCPaymentType = cboPaymentType.Value.ToString();
                entityHd.RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
                entityHd.GCCashierGroup = cboCashierGroup.Value.ToString();
                entityHd.GCShift = cboShift.Value.ToString();

                List<PatientBill> lstPatientBill = null;
                if (hdnListBillingID.Value != "")
                {
                    lstPatientBill = BusinessLayer.GetPatientBillList(string.Format("PatientBillingID IN ({0}) AND GCTransactionStatus != '{1}'", hdnListBillingID.Value, Constant.TransactionStatus.VOID), ctx);
                }

                if (entityHd.GCPaymentType == Constant.PaymentType.SETTLEMENT || entityHd.GCPaymentType == Constant.PaymentType.CUSTOM || entityHd.GCPaymentType == Constant.PaymentType.DOWN_PAYMENT || entityHd.GCPaymentType == Constant.PaymentType.DEPOSIT_IN || entityHd.GCPaymentType == Constant.PaymentType.DEPOSIT_OUT)
                {
                    entityHd.TotalPaymentAmount = Convert.ToDecimal(hdnTotalPaymentAmount.Value);
                    entityHd.TotalFeeAmount = Convert.ToDecimal(hdnTotalFeeAmount.Value);
                    if (entityHd.GCPaymentType == Constant.PaymentType.DOWN_PAYMENT && hdnListBillingID.Value != "")
                    {
                        entityHd.TotalPatientBillAmount = 0;
                        entityHd.TotalPayerBillAmount = 0;
                        decimal totalPaymentAmount = entityHd.TotalPaymentAmount;
                        foreach (PatientBill patientBill in lstPatientBill)
                        {
                            if (totalPaymentAmount > 0)
                            {
                                if (patientBill.PatientRemainingAmount < totalPaymentAmount)
                                    totalPaymentAmount -= patientBill.PatientRemainingAmount;
                                else
                                    totalPaymentAmount = 0;
                                entityHd.TotalPatientBillAmount += patientBill.PatientRemainingAmount;
                                entityHd.TotalPayerBillAmount = Convert.ToDecimal(hdnBillingTotalPayer.Value);
                            }
                        }
                    }
                    else
                    {
                        decimal totalPatient = 0, totalPayer = 0;
                        if (lstPatientBill != null)
                        {
                            foreach (PatientBill patientBill in lstPatientBill)
                            {
                                totalPatient += patientBill.PatientRemainingAmount;
                                totalPayer += patientBill.PayerRemainingAmount;
                                if (entityHd.GCPaymentType != Constant.PaymentType.SETTLEMENT && totalPatient > entityHd.TotalPaymentAmount)
                                    break;
                            }
                        }
                        entityHd.TotalPatientBillAmount = totalPatient;
                        entityHd.TotalPayerBillAmount = totalPayer;
                    }
                }
                //else if (entityHd.GCPaymentType == Constant.PaymentType.PAYMENT_RETURN)
                //{
                //    entityHd.TotalPatientBillAmount = Convert.ToDecimal(hdnBillingTotalPatient.Value);
                //    entityHd.TotalPayerBillAmount = Convert.ToDecimal(hdnBillingTotalPayer.Value);
                //    entityHd.TotalPaymentAmount = entityHd.TotalPatientBillAmount * -1;
                //    entityHd.TotalFeeAmount = 0;
                //}
                else
                {
                    if (entityHd.GCPaymentType == Constant.PaymentType.AR_PATIENT)
                    {
                        entityHd.TotalPatientBillAmount = Convert.ToDecimal(hdnBillingTotalPatient.Value);
                        entityHd.TotalPayerBillAmount = Convert.ToDecimal(hdnBillingTotalPayer.Value);
                        entityHd.TotalPaymentAmount = entityHd.TotalPatientBillAmount;
                        entityHd.TotalFeeAmount = 0;
                    }
                    else
                    {
                        entityHd.TotalPaymentAmount = Convert.ToDecimal(hdnTotalPayerPaymentAmount.Value);
                        entityHd.TotalFeeAmount = 0;
                        decimal totalPayer = 0;
                        if (lstPatientBill != null)
                        {
                            foreach (PatientBill patientBill in lstPatientBill)
                            {
                                totalPayer += patientBill.PayerRemainingAmount;
                                if (totalPayer > entityHd.TotalPaymentAmount)
                                    break;
                            }
                        }
                        entityHd.TotalPatientBillAmount = Convert.ToDecimal(hdnBillingTotalPatient.Value);
                        entityHd.TotalPayerBillAmount = totalPayer;
                    }
                }

                entityHd.Remarks = txtRemarks.Text;
                if (entityHd.GCPaymentType == Constant.PaymentType.SETTLEMENT || entityHd.GCPaymentType == Constant.PaymentType.CUSTOM)
                {
                    if (entityHd.TotalPaymentAmount > entityHd.TotalPatientBillAmount)
                    {
                        entityHd.CashBackAmount = Convert.ToDecimal(Request.Form[txtCashbackAmount.UniqueID]);
                    }
                    else
                    {
                        entityHd.CashBackAmount = 0;
                    }
                }
                else
                {
                    entityHd.CashBackAmount = 0;
                }

                entityHd.PatientRoundingAmount = Convert.ToDecimal(Request.Form[txtPatientRoundingAmount.UniqueID]);

                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                string transactionCode = "";
                switch (hdnDepartmentID.Value)
                {
                    case Constant.Facility.INPATIENT:
                        switch (entityHd.GCPaymentType)
                        {
                            case Constant.PaymentType.DEPOSIT_IN: transactionCode = Constant.TransactionCode.IP_DEPOSIT_IN; break;
                            case Constant.PaymentType.DOWN_PAYMENT: transactionCode = Constant.TransactionCode.IP_PATIENT_PAYMENT_DP; break;
                            case Constant.PaymentType.SETTLEMENT: transactionCode = Constant.TransactionCode.IP_PATIENT_PAYMENT_SETTLEMENT; break;
                            case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.IP_PATIENT_PAYMENT_AR_PATIENT; break;
                            case Constant.PaymentType.CUSTOM: transactionCode = Constant.TransactionCode.IP_PATIENT_PAYMENT_CUSTOM; break;
                            //case Constant.PaymentType.PAYMENT_RETURN: transactionCode = Constant.TransactionCode.IP_PATIENT_PAYMENT_RETURN; break;
                            default: transactionCode = Constant.TransactionCode.IP_PATIENT_PAYMENT_AR_PAYER; break;
                        } break;
                    case Constant.Facility.MEDICAL_CHECKUP:
                        switch (entityHd.GCPaymentType)
                        {
                            case Constant.PaymentType.DEPOSIT_IN: transactionCode = Constant.TransactionCode.MCU_DEPOSIT_IN; break;
                            case Constant.PaymentType.DOWN_PAYMENT: transactionCode = Constant.TransactionCode.MCU_PATIENT_PAYMENT_DP; break;
                            case Constant.PaymentType.SETTLEMENT: transactionCode = Constant.TransactionCode.MCU_PATIENT_PAYMENT_SETTLEMENT; break;
                            case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.MCU_PATIENT_PAYMENT_AR_PATIENT; break;
                            case Constant.PaymentType.CUSTOM: transactionCode = Constant.TransactionCode.MCU_PATIENT_PAYMENT_CUSTOM; break;
                            //case Constant.PaymentType.PAYMENT_RETURN: transactionCode = Constant.TransactionCode.MCU_PATIENT_PAYMENT_RETURN; break;
                            default: transactionCode = Constant.TransactionCode.MCU_PATIENT_PAYMENT_AR_PAYER; break;
                        } break;
                    case Constant.Facility.EMERGENCY:
                        switch (entityHd.GCPaymentType)
                        {
                            case Constant.PaymentType.DEPOSIT_IN: transactionCode = Constant.TransactionCode.ER_DEPOSIT_IN; break;
                            case Constant.PaymentType.DOWN_PAYMENT: transactionCode = Constant.TransactionCode.ER_PATIENT_PAYMENT_DP; break;
                            case Constant.PaymentType.SETTLEMENT: transactionCode = Constant.TransactionCode.ER_PATIENT_PAYMENT_SETTLEMENT; break;
                            case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.ER_PATIENT_PAYMENT_AR_PATIENT; break;
                            case Constant.PaymentType.CUSTOM: transactionCode = Constant.TransactionCode.ER_PATIENT_PAYMENT_CUSTOM; break;
                            //case Constant.PaymentType.PAYMENT_RETURN: transactionCode = Constant.TransactionCode.ER_PATIENT_PAYMENT_RETURN; break;
                            default: transactionCode = Constant.TransactionCode.ER_PATIENT_PAYMENT_AR_PAYER; break;
                        } break;
                    case Constant.Facility.PHARMACY:
                        switch (entityHd.GCPaymentType)
                        {
                            case Constant.PaymentType.DEPOSIT_IN: transactionCode = Constant.TransactionCode.PH_DEPOSIT_IN; break;
                            case Constant.PaymentType.DOWN_PAYMENT: transactionCode = Constant.TransactionCode.PH_PATIENT_PAYMENT_DP; break;
                            case Constant.PaymentType.SETTLEMENT: transactionCode = Constant.TransactionCode.PH_PATIENT_PAYMENT_SETTLEMENT; break;
                            case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.PH_PATIENT_PAYMENT_AR_PATIENT; break;
                            case Constant.PaymentType.CUSTOM: transactionCode = Constant.TransactionCode.PH_PATIENT_PAYMENT_CUSTOM; break;
                            //case Constant.PaymentType.PAYMENT_RETURN: transactionCode = Constant.TransactionCode.PH_PATIENT_PAYMENT_RETURN; break;
                            default: transactionCode = Constant.TransactionCode.PH_PATIENT_PAYMENT_AR_PAYER; break;
                        } break;
                    case Constant.Facility.DIAGNOSTIC:
                        if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        {
                            switch (entityHd.GCPaymentType)
                            {
                                case Constant.PaymentType.DEPOSIT_IN: transactionCode = Constant.TransactionCode.LABORATORY_DEPOSIT_IN; break;
                                case Constant.PaymentType.DOWN_PAYMENT: transactionCode = Constant.TransactionCode.LABORATORY_PATIENT_PAYMENT_DP; break;
                                case Constant.PaymentType.SETTLEMENT: transactionCode = Constant.TransactionCode.LABORATORY_PATIENT_PAYMENT_SETTLEMENT; break;
                                case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.LABORATORY_PATIENT_PAYMENT_AR_PATIENT; break;
                                case Constant.PaymentType.CUSTOM: transactionCode = Constant.TransactionCode.LABORATORY_PATIENT_PAYMENT_CUSTOM; break;
                                //case Constant.PaymentType.PAYMENT_RETURN: transactionCode = Constant.TransactionCode.LABORATORY_PATIENT_PAYMENT_RETURN; break;
                                default: transactionCode = Constant.TransactionCode.LABORATORY_PATIENT_PAYMENT_AR_PAYER; break;
                            }
                        }
                        else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        {
                            switch (entityHd.GCPaymentType)
                            {
                                case Constant.PaymentType.DEPOSIT_IN: transactionCode = Constant.TransactionCode.IMAGING_DEPOSIT_IN; break;
                                case Constant.PaymentType.DOWN_PAYMENT: transactionCode = Constant.TransactionCode.IMAGING_PATIENT_PAYMENT_DP; break;
                                case Constant.PaymentType.SETTLEMENT: transactionCode = Constant.TransactionCode.IMAGING_PATIENT_PAYMENT_SETTLEMENT; break;
                                case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.IMAGING_PATIENT_PAYMENT_AR_PATIENT; break;
                                case Constant.PaymentType.CUSTOM: transactionCode = Constant.TransactionCode.IMAGING_PATIENT_PAYMENT_CUSTOM; break;
                                //case Constant.PaymentType.PAYMENT_RETURN: transactionCode = Constant.TransactionCode.IMAGING_PATIENT_PAYMENT_RETURN; break;
                                default: transactionCode = Constant.TransactionCode.IMAGING_PATIENT_PAYMENT_AR_PAYER; break;
                            }
                        }
                        else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                        {
                            switch (entityHd.GCPaymentType)
                            {
                                case Constant.PaymentType.DEPOSIT_IN: transactionCode = Constant.TransactionCode.RADIOTHERAPHY_DEPOSIT_IN; break;
                                case Constant.PaymentType.DOWN_PAYMENT: transactionCode = Constant.TransactionCode.RADIOTHERAPHY_PATIENT_PAYMENT_DP; break;
                                case Constant.PaymentType.SETTLEMENT: transactionCode = Constant.TransactionCode.RADIOTHERAPHY_PATIENT_PAYMENT_SETTLEMENT; break;
                                case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.RADIOTHERAPHY_PATIENT_PAYMENT_AR_PATIENT; break;
                                case Constant.PaymentType.CUSTOM: transactionCode = Constant.TransactionCode.RADIOTHERAPHY_PATIENT_PAYMENT_CUSTOM; break;
                                default: transactionCode = Constant.TransactionCode.RADIOTHERAPHY_PATIENT_PAYMENT_AR_PAYER; break;
                            }
                        }
                        else
                        {
                            switch (entityHd.GCPaymentType)
                            {
                                case Constant.PaymentType.DEPOSIT_IN: transactionCode = Constant.TransactionCode.OTHER_DEPOSIT_IN; break;
                                case Constant.PaymentType.DOWN_PAYMENT: transactionCode = Constant.TransactionCode.OTHER_PATIENT_PAYMENT_DP; break;
                                case Constant.PaymentType.SETTLEMENT: transactionCode = Constant.TransactionCode.OTHER_PATIENT_PAYMENT_SETTLEMENT; break;
                                case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.OTHER_PATIENT_PAYMENT_AR_PATIENT; break;
                                case Constant.PaymentType.CUSTOM: transactionCode = Constant.TransactionCode.OTHER_PATIENT_PAYMENT_CUSTOM; break;
                                //case Constant.PaymentType.PAYMENT_RETURN: transactionCode = Constant.TransactionCode.OTHER_PATIENT_PAYMENT_RETURN; break;
                                default: transactionCode = Constant.TransactionCode.OTHER_PATIENT_PAYMENT_AR_PAYER; break;
                            }
                        } break;
                    default:
                        switch (entityHd.GCPaymentType)
                        {
                            case Constant.PaymentType.DEPOSIT_IN: transactionCode = Constant.TransactionCode.OP_DEPOSIT_IN; break;
                            case Constant.PaymentType.DOWN_PAYMENT: transactionCode = Constant.TransactionCode.OP_PATIENT_PAYMENT_DP; break;
                            case Constant.PaymentType.SETTLEMENT: transactionCode = Constant.TransactionCode.OP_PATIENT_PAYMENT_SETTLEMENT; break;
                            case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.OP_PATIENT_PAYMENT_AR_PATIENT; break;
                            case Constant.PaymentType.CUSTOM: transactionCode = Constant.TransactionCode.OP_PATIENT_PAYMENT_CUSTOM; break;
                            //case Constant.PaymentType.PAYMENT_RETURN: transactionCode = Constant.TransactionCode.OP_PATIENT_PAYMENT_RETURN; break;
                            default: transactionCode = Constant.TransactionCode.OP_PATIENT_PAYMENT_AR_PAYER; break;
                        } break;
                }
                entityHd.PaymentNo = BusinessLayer.GenerateTransactionNo(transactionCode, entityHd.PaymentDate, ctx);
                //entityHd.CreatedBy = entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                entityHd.PaymentID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
                #endregion

                Int32 detailPaymentID = 0;
                #region Payment Dt
                if (entityHd.GCPaymentType == Constant.PaymentType.SETTLEMENT || entityHd.GCPaymentType == Constant.PaymentType.CUSTOM || entityHd.GCPaymentType == Constant.PaymentType.DOWN_PAYMENT || entityHd.GCPaymentType == Constant.PaymentType.DEPOSIT_IN || entityHd.GCPaymentType == Constant.PaymentType.DEPOSIT_OUT)
                {
                    foreach (String param in listParam)
                    {
                        String[] data = param.Split(';');
                        bool isChanged = data[0] == "1" ? true : false;
                        int ID = Convert.ToInt32(data[1]);
                        if (isChanged || ID > 0)
                        {
                            PatientPaymentDt entityDt = new PatientPaymentDt();
                            entityDt.PaymentID = entityHd.PaymentID;
                            entityDt.GCPaymentMethod = data[2];
                            if (entityDt.GCPaymentMethod != Constant.PaymentMethod.CASH)
                            {
                                if (data[3] != "")
                                    entityDt.EDCMachineID = Convert.ToInt32(data[3]);
                                else
                                    entityDt.EDCMachineID = null;
                                //if (data[3] != "")
                                //    entityDt.BankID = Convert.ToInt32(data[3]);
                                //else
                                //    entityDt.BankID = null;
                                entityDt.ReferenceNo = data[6];
                                entityDt.GCCardType = data[10];

                                if (data[4] != "")
                                {
                                    entityDt.CardNumber = data[4];
                                }
                                else
                                {
                                    entityDt.CardNumber = "";
                                }
                                entityDt.CardHolderName = data[13];

                                if (data[14] != "" && data[15] != "")
                                    entityDt.CardValidThru = string.Format("{0:00}/{1:00}", data[14].PadLeft(2, '0'), data[15].Substring(2));
                                else
                                    entityDt.CardValidThru = "";

                                entityDt.GCCardProvider = data[17];

                                if (data[18] != "")
                                {
                                    entityDt.BatchNo = data[18];
                                }
                                if (data[19] != "")
                                {
                                    entityDt.TraceNo = data[19];
                                }

                                if (data[21] != "")
                                {
                                    entityDt.ApprovalCode = data[21];
                                }
                                if (data[22] != "")
                                {
                                    entityDt.TerminalID = data[22];
                                }
                            }
                            entityDt.PaymentAmount = Convert.ToDecimal(data[7].Replace(",", ""));
                            entityDt.CardFeeAmount = Convert.ToDecimal(data[8].Replace(",", ""));
                            entityDt.CreatedBy = AppSession.UserLogin.UserID;
                            detailPaymentID = entityDtDao.InsertReturnPrimaryKeyID(entityDt);
                        }
                    }
                }
                //else if (entityHd.GCPaymentType == Constant.PaymentType.PAYMENT_RETURN)
                //{
                //    PatientPaymentDt entityDt = new PatientPaymentDt();
                //    entityDt.PaymentID = entityHd.PaymentID;
                //    entityDt.GCPaymentMethod = Constant.PaymentMethod.PAYMENT_RETURN;

                //    entityDt.PaymentAmount = entityHd.TotalPaymentAmount;
                //    entityDt.CardFeeAmount = 0;
                //    entityDt.CreatedBy = AppSession.UserLogin.UserID;
                //    entityDtDao.Insert(entityDt);
                //}
                else if (entityHd.GCPaymentType == Constant.PaymentType.AR_PAYER)
                {
                    foreach (String param in listParam2)
                    {
                        String[] data = param.Split(';');
                        bool isChanged = data[0] == "1" ? true : false;
                        if (isChanged)
                        {
                            PatientPaymentDt entityDt = new PatientPaymentDt();
                            entityDt.PaymentID = entityHd.PaymentID;
                            entityDt.GCPaymentMethod = Constant.PaymentMethod.CREDIT;
                            entityDt.BusinessPartnerID = Convert.ToInt32(data[2]);
                            entityDt.PaymentAmount = Convert.ToDecimal(data[3].Replace(",", ""));
                            entityDt.CardFeeAmount = 0;
                            entityDt.CreatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Insert(entityDt);
                        }
                    }
                }
                else if (entityHd.GCPaymentType == Constant.PaymentType.AR_PATIENT)
                {
                    decimal dpAmount = Convert.ToDecimal(hdnOutstandingDP.Value.ToString().Replace(",", ""));
                    if (dpAmount != 0)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            PatientPaymentDt entityDt = new PatientPaymentDt();
                            entityDt.PaymentID = entityHd.PaymentID;

                            string filterBP = string.Format("BusinessPartnerID = (SELECT BusinessPartnerID FROM Customer WHERE GCCustomerType = '{0}')", Constant.CustomerType.PERSONAL);
                            BusinessPartners bp = BusinessLayer.GetBusinessPartnersList(filterBP, ctx).FirstOrDefault();
                            entityDt.BusinessPartnerID = bp.BusinessPartnerID;

                            if (i == 0)
                            {
                                entityDt.GCPaymentMethod = Constant.PaymentMethod.DOWN_PAYMENT;
                                entityDt.PaymentAmount = Convert.ToDecimal(hdnOutstandingDP.Value.ToString().Replace(",", ""));
                            }
                            else
                            {
                                entityDt.GCPaymentMethod = Constant.PaymentMethod.CREDIT;
                                entityDt.PaymentAmount = Convert.ToDecimal(hdnARPatientWithoutDP.Value.ToString().Replace(",", ""));
                            }
                            entityDt.CardFeeAmount = 0;
                            entityDt.CreatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityDtDao.Insert(entityDt);
                        }
                    }
                    else
                    {
                        PatientPaymentDt entityDt = new PatientPaymentDt();
                        entityDt.PaymentID = entityHd.PaymentID;

                        string filterBP = string.Format("BusinessPartnerID = (SELECT BusinessPartnerID FROM Customer WHERE GCCustomerType = '{0}')", Constant.CustomerType.PERSONAL);
                        BusinessPartners bp = BusinessLayer.GetBusinessPartnersList(filterBP, ctx).FirstOrDefault();
                        entityDt.BusinessPartnerID = bp.BusinessPartnerID;

                        entityDt.GCPaymentMethod = Constant.PaymentMethod.CREDIT;
                        entityDt.PaymentAmount = Convert.ToDecimal(hdnARPatientWithoutDP.Value.ToString().Replace(",", ""));

                        entityDt.CardFeeAmount = 0;
                        entityDt.CreatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityDtDao.Insert(entityDt);
                    }
                }
                else
                {
                    PatientPaymentDt entityDt = new PatientPaymentDt();
                    entityDt.PaymentID = entityHd.PaymentID;

                    string filterBP = string.Format("BusinessPartnerID = (SELECT BusinessPartnerID FROM Customer WHERE GCCustomerType = '{0}')", Constant.CustomerType.PERSONAL);
                    BusinessPartners bp = BusinessLayer.GetBusinessPartnersList(filterBP, ctx).FirstOrDefault();
                    entityDt.BusinessPartnerID = bp.BusinessPartnerID;

                    entityDt.GCPaymentMethod = Constant.PaymentMethod.CREDIT;
                    entityDt.PaymentAmount = entityHd.TotalPaymentAmount;
                    entityDt.CardFeeAmount = 0;
                    entityDt.CreatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Insert(entityDt);
                }
                #endregion

                #region Update Billing
                if (hdnListBillingID.Value != "")
                {
                    List<PatientChargesHd> lstPatientChargesHd = BusinessLayer.GetPatientChargesHdList(string.Format("PatientBillingID IN ({0})", hdnListBillingID.Value), ctx);
                    decimal totalPaymentAmount = entityHd.TotalPaymentAmount;
                    string oldBillStatus = null;
                    foreach (PatientBill patientBill in lstPatientBill)
                    {
                        oldBillStatus = patientBill.GCTransactionStatus;

                        patientBill.PaymentID = entityHd.PaymentID;
                        PatientBillPayment patientBillPayment = new PatientBillPayment();
                        patientBillPayment.PaymentID = entityHd.PaymentID;
                        patientBillPayment.PatientBillingID = patientBill.PatientBillingID;

                        if (entityHd.GCPaymentType == Constant.PaymentType.AR_PAYER)
                        {
                            if (patientBill.PayerRemainingAmount > 0)
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
                                    patientBillPayment.PayerPaymentAmount = payerRemainingAmount;
                                    patientBill.TotalPayerPaymentAmount += payerRemainingAmount;
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
                                    patientBillPayment.PayerPaymentAmount = payerRemainingAmount;
                                    patientBill.TotalPayerPaymentAmount += payerRemainingAmount;
                                }
                            }
                        }
                        else if (entityHd.GCPaymentType == Constant.PaymentType.AR_PATIENT)
                        {
                            //patientBillPayment.PatientPaymentAmount = patientBill.TotalPatientPaymentAmount = patientBill.PatientRemainingAmount;

                            patientBill.TotalPatientPaymentAmount += totalPaymentAmount;
                            patientBillPayment.PatientPaymentAmount = totalPaymentAmount;
                            totalPaymentAmount = 0;
                        }
                        //else if (entityHd.GCPaymentType == Constant.PaymentType.PAYMENT_RETURN)
                        //    patientBillPayment.PatientPaymentAmount = patientBill.TotalPatientPaymentAmount = patientBill.PatientRemainingAmount;
                        else
                        {
                            if (entityHd.GCPaymentType == Constant.PaymentType.SETTLEMENT)
                            {
                                totalPaymentAmount -= patientBill.PatientRemainingAmount;
                                patientBillPayment.PatientPaymentAmount = patientBill.PatientRemainingAmount;
                                patientBill.TotalPatientPaymentAmount = (patientBill.TotalPatientAmount - patientBill.PatientDiscountAmount);
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

                        //if (patientBillPayment.PayerPaymentAmount != 0 || patientBillPayment.PatientPaymentAmount != 0)
                        //{
                        if (patientBill.RemainingAmount == 0)
                        {
                            //if (entityHd.GCPaymentType == Constant.PaymentType.AR_PATIENT || entityHd.GCPaymentType == Constant.PaymentType.AR_PAYER)
                            //    patientBill.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                            //else
                            patientBill.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                            List<PatientChargesHd> lstChargesHd = lstPatientChargesHd.Where(p => p.PatientBillingID == patientBill.PatientBillingID).ToList();
                            foreach (PatientChargesHd patientChargesHd in lstChargesHd)
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
                        //}

                        patientBillPaymentDao.Insert(patientBillPayment);
                    }
                }
                #endregion

                #region Auto Close Registration
                //if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
                //{
                //    ConsultVisit consultVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", hdnRegistrationID.Value), ctx).FirstOrDefault();
                //    HealthcareServiceUnit healthcareServiceUnit = healthcareServiceUnitDao.Get(consultVisit.HealthcareServiceUnitID);
                //    if (healthcareServiceUnit.IsAutoCloseRegistration)
                //    {
                //        int count = BusinessLayer.GetvPatientChargesHdRowCount(string.Format("VisitID = {0} AND GCTransactionStatus NOT IN ('{1},'{2}')", consultVisit.VisitID, Constant.TransactionStatus.VOID, Constant.TransactionStatus.CLOSED));
                //        if (count < 0)
                //        {
                //            Registration registration = registrationDao.Get(Convert.ToInt32(hdnRegistrationID.Value));
                //            registration.GCRegistrationStatus = Constant.RegistrationStatus.CLOSED;
                //            registration.LastUpdatedBy = AppSession.UserLogin.UserID;
                //            registrationDao.Update(registration);
                //        }
                //    }
                //}
                retval = "";
                vRegistrationOutstandingInfo vRegistrationOutstandingInfo = BusinessLayer.GetvRegistrationOutstandingInfoList(string.Format("RegistrationID = {0}", hdnRegistrationID.Value), ctx).FirstOrDefault();
                if (vRegistrationOutstandingInfo.Billing > 0)
                    retval = "ar";
                //else
                //{
                //    ConsultVisit consultVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", hdnRegistrationID.Value), ctx).FirstOrDefault();
                //    HealthcareServiceUnit healthcareServiceUnit = healthcareServiceUnitDao.Get(consultVisit.HealthcareServiceUnitID);
                //    if (healthcareServiceUnit.IsAutoCloseRegistration && vRegistrationOutstandingInfo.Billing < 1 && vRegistrationOutstandingInfo.Charges < 1)
                //        retval = "closed";
                //}
                #endregion

                #region EDC

                if (!String.IsNullOrEmpty(hdnEdcRequestID.Value))
                {
                    int edcID = Convert.ToInt32(hdnEdcRequestID.Value);
                    EDCMachineTransaction entityEDC = edcDao.Get(edcID);
                    entityEDC.PaymentID = entityHd.PaymentID;
                    entityEDC.PaymentDetailID = detailPaymentID;
                    entityEDC.IsFinish = true;
                    edcDao.Update(entityEDC);
                }
                #endregion
                retval = entityHd.PaymentNo;
                ctx.CommitTransaction();

                #endregion
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }

            return result;
        }

        private bool IsTransactionValid(ref string errMessage)
        {
            if (string.IsNullOrEmpty(txtPaymentTime.Text))
            {
                errMessage = "Payment Time must be entried";
                return false;
            }
            else
            {
                Regex reg = new Regex(@"^(?:[01][0-9]|2[0-3]):[0-5][0-9]$");
                if (!reg.IsMatch(txtPaymentTime.Text))
                {
                    errMessage = "Payment time must be entried in correct format (hh:mm)";
                    return false;
                }
            }
            return true;
        }
        #endregion

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            bool result = true;
            OnSaveAddRecordFromEDC(ref errMessage, ref retval);
            string paymentNo = retval;
            retval = string.Format("edc|{0}", paymentNo);
            if (!String.IsNullOrEmpty(errMessage))
            {
                result = false;
            }
            return result;
        }

        protected void cbpEDCProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;

            result = SendMessageToDesktopService();

            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = result;
        }

        private string SendMessageToDesktopService()
        {
            string result = string.Empty;

            try
            {
                StringBuilder sbMessage = new StringBuilder();
                string txnData = string.Empty;
                string localIPAddress = Methods.GetLocalIPAddress();
                string message = Methods.GenerateDesktopService(DesktopServiceMessageType.MD201, AppSession.UserLogin.HealthcareID, AppSession.RegisteredPatient, txnData);
                //string message = string.Format("MD101|{0};{1};{2};{3};{4};{5}", AppSession.UserLogin.HealthcareID, hdnMRN.Value, hdnMedicalNo.Value, hdnPatientName.Value, hdnDateOfBirth.Value, hdnGender.Value, hdnHomeAddress.Value);
                TcpClient client = new TcpClient();
                client.Connect(localIPAddress, Convert.ToInt16(hdnPort.Value));
                NetworkStream stream = client.GetStream();
                using (BinaryWriter w = new BinaryWriter(stream))
                {
                    using (BinaryReader r = new BinaryReader(stream))
                    {
                        w.Write(string.Format(@"{0}", message).ToCharArray());
                    }
                }

                result = string.Format("process|1|{0}|{1}|{2}", "Identity Card was processed successfully", string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                result = string.Format("process|0|{0}|{1}|{2}", ex.Message, string.Empty, string.Empty);
            }

            return result;
        }

        private string SendLISOrderToSoftmedix(List<PatientChargesHd> lstChargesHd)
        {
            string result = string.Empty;

            foreach (PatientChargesHd hd in lstChargesHd)
            {
                if (hd.TransactionCode == Constant.TransactionCode.LABORATORY_CHARGES)
                {
                    PatientChargesHdInfo hdInfo = BusinessLayer.GetPatientChargesHdInfo(hd.TransactionID);
                    if (hdInfo.GCLISBridgingStatus != Constant.LIS_Bridging_Status.SENT)
                    {
                        LaboratoryService oService = new LaboratoryService();

                        APIMessageLog log = new APIMessageLog();
                        log.Sender = Constant.BridgingVendor.HIS;
                        log.Recipient = Constant.BridgingVendor.LIS;
                        log.MessageDateTime = DateTime.Now;

                        string apiResult = oService.OnSendOrderToLISMethod("N", hd.TransactionID, Convert.ToInt32(hd.TestOrderID), log);
                        string[] apiResultInfo = apiResult.Split('|');
                        result = apiResultInfo[1];
                    }
                }
            }

            return result;
        }

        private string SendLISOrderToHCLAB(List<PatientChargesHd> lstChargesHd)
        {
            string result = string.Empty;

            foreach (PatientChargesHd hd in lstChargesHd)
            {
                if (hd.TransactionCode == Constant.TransactionCode.LABORATORY_CHARGES)
                {
                    PatientChargesHdInfo hdInfo = BusinessLayer.GetPatientChargesHdInfo(hd.TransactionID);
                    if (hdInfo.GCLISBridgingStatus != Constant.LIS_Bridging_Status.SENT)
                    {
                        IDbContext ctx = DbFactory.Configure(true);
                        try
                        {
                            BusinessLayer.SendToLISInterfaceDB(hd.TransactionID, ctx);
                            ctx.CommitTransaction();
                        }
                        catch (Exception ex)
                        {
                            ctx.RollBackTransaction();
                        }
                        finally
                        {
                            ctx.Close();
                        }
                    }
                }
            }

            return result;
        }

    }
}
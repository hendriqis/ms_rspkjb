using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using System.Data;
using DevExpress.Web.ASPxEditors;
using System.Globalization;
using QIS.Medinfras.Web.CommonLibs.Controls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PaymentEntry : BasePageTrx
    {
        protected string GetErrorMsgCashBackAmount()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_CASH_BACK_AMOUNT_VALIDATION);
        }
        public override string OnGetMenuCode()
        {
            //switch (hdnDepartmentID.Value)
            //{
            //    case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.PAYMENT;
            //    case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.PAYMENT;
            //    case Constant.Facility.DIAGNOSTIC:
            //        if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
            //            return Constant.MenuCode.Laboratory.PAYMENT;
            //        if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
            //            return Constant.MenuCode.Imaging.PAYMENT;
            //        return Constant.MenuCode.MedicalDiagnostic.PAYMENT;
            //    default: return Constant.MenuCode.Outpatient.PAYMENT;
            //}
            return "";
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = (hdnGCRegistrationStatus.Value != Constant.VisitStatus.CLOSED);
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString.Count > 0)
            {
                hdnRegistrationID.Value = Page.Request.QueryString["id"];
                vRegistration3 entity = BusinessLayer.GetvRegistration3List(string.Format("RegistrationID = {0}", hdnRegistrationID.Value))[0];
                ((PatientBannerCtl)ctlPatientBanner).InitializePatientBanner(entity);
                hdnDepartmentID.Value = entity.DepartmentID;
                hdnGCRegistrationStatus.Value = entity.GCRegistrationStatus;
                hdnPatientName.Value = entity.PatientName;
                hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();

                bool flagIsCustomerContract = false;
                CustomerContract entityCC = BusinessLayer.GetCustomerContract(entity.ContractID);
                if (entityCC != null)
                {
                    if (entityCC.IsControlCoverageLimit)
                    {
                        flagIsCustomerContract = true;
                    }
                }

                if (flagIsCustomerContract)
                {
                    trCoverageLimit.Style.Add("display", "none");
                    hdnCoverageLimit.Value = "0";
                }
                else
                {
                    decimal coverageLimit = entity.CoverageLimitAmount;
                    string filterExpression = string.Format("RegistrationID = {0} AND GCPaymentType = '{1}' AND GCTransactionStatus != '{2}'", hdnRegistrationID.Value, Constant.PaymentType.AR_PAYER, Constant.TransactionStatus.VOID);
                    if (entity.IsCoverageLimitPerDay)
                        filterExpression += string.Format(" AND PaymentDate = '{0}'", DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112));
                    List<PatientPaymentHd> lstPaymentHd = BusinessLayer.GetPatientPaymentHdList(filterExpression); 
                    coverageLimit -= lstPaymentHd.Sum(p => p.TotalPaymentAmount);
                    hdnCoverageLimit.Value = coverageLimit.ToString();
                }

                InitializeControlProperties();
            }
        }

        private void InitializeControlProperties()
        {
            hdnCreditCardFeeFilterExpression.Value = string.Format("HealthcareID = '{0}' AND GCCardType = '[GCCardType]' AND GCCardProvider = '[GCCardProvider]' AND EDCMachineID = [EDCMachineID]", AppSession.UserLogin.HealthcareID);

            List<EDCMachine> lstEDCMachine = BusinessLayer.GetEDCMachineList("IsDeleted = 0");
            Methods.SetComboBoxField<EDCMachine>(cboEDCMachine, lstEDCMachine, "EDCMachineName", "EDCMachineID");
            cboEDCMachine.SelectedIndex = 0;

            List<Bank> lstBank = BusinessLayer.GetBankList("IsDeleted = 0");
            Methods.SetComboBoxField<Bank>(cboBank, lstBank, "BankName", "BankID");
            cboBank.SelectedIndex = 0;

            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(String.Format("ParentID IN ('{0}','{1}','{2}','{3}') AND StandardCodeID NOT IN ('{4}') AND IsDeleted = 0", Constant.StandardCode.CARD_TYPE, Constant.StandardCode.PAYMENT_METHOD, Constant.StandardCode.PAYMENT_TYPE, Constant.StandardCode.CARD_PROVIDER, Constant.PaymentMethod.BANK_TRANSFER));
            Methods.SetComboBoxField<StandardCode>(cboCardType, lstSc.Where(p => p.ParentID == Constant.StandardCode.CARD_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboPaymentMethod, lstSc.Where(p => p.ParentID == Constant.StandardCode.PAYMENT_METHOD && p.StandardCodeID != Constant.PaymentMethod.CREDIT).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboPaymentType, lstSc.Where(p => p.ParentID == Constant.StandardCode.PAYMENT_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboCardProvider, lstSc.Where(p => p.ParentID == Constant.StandardCode.CARD_PROVIDER).ToList(), "StandardCodeName", "StandardCodeID");

            tdARPaymentMethod.InnerHtml = lstSc.FirstOrDefault(p => p.StandardCodeID == Constant.PaymentMethod.CREDIT).StandardCodeName;

            cboPaymentMethod.SelectedIndex = 0;
            cboCardType.SelectedIndex = 0;
            //cboPaymentType.Value = Constant.PaymentType.SETTLEMENT;
            //cboPaymentType.ClientEnabled = false;

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
        }

        protected override void SetControlProperties()
        {
            ListView lvwBilling = (ListView)ddeBillingNo.FindControl("lvwBilling");
            List<vPatientBill> lst = BusinessLayer.GetvPatientBillList(string.Format("RegistrationID = {0} AND GCTransactionStatus = '{1}'", hdnRegistrationID.Value, Constant.TransactionStatus.OPEN));
            lvwBilling.DataSource = lst;
            lvwBilling.DataBind();

            Helper.SetControlEntrySetting(cboCardType, new ControlEntrySetting(true, true, true), "vgCardInformation");
            Helper.SetControlEntrySetting(cboCardProvider, new ControlEntrySetting(true, true, true), "vgCardInformation");
            Helper.SetControlEntrySetting(txtCardNumber4, new ControlEntrySetting(true, true, true), "vgCardInformation");
            Helper.SetControlEntrySetting(txtHolderName, new ControlEntrySetting(true, true, true), "vgCardInformation");
            Helper.SetControlEntrySetting(cboCardDateMonth, new ControlEntrySetting(true, true, true), "vgCardInformation");
            Helper.SetControlEntrySetting(cboCardDateYear, new ControlEntrySetting(true, true, true), "vgCardInformation");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnPaymentHdID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtPaymentNo, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtPaymentDate, new ControlEntrySetting(true, false, true, Constant.DefaultValueEntry.DATE_NOW));
            SetControlEntrySetting(txtPaymentTime, new ControlEntrySetting(true, false, true, Constant.DefaultValueEntry.TIME_NOW));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(cboPaymentType, new ControlEntrySetting(true, false, true, Constant.PaymentType.DOWN_PAYMENT));

            SetControlEntrySetting(txtBillingNo, new ControlEntrySetting(false, false, false));            
            SetControlEntrySetting(ddeBillingNo, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtBillingTotal, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtCoverageLimit, new ControlEntrySetting(false, false, false, hdnCoverageLimit.Value));

            SetControlEntrySetting(txtBillingTotalPatient, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtBillingTotalPayer, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(txtPaymentTotalPatient, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtPaymentTotalPayer, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(txtCashbackAmount, new ControlEntrySetting(false, false, true));

            SetControlEntrySetting(cboCardType, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(cboCardProvider, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtCardNumber4, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtHolderName, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(cboCardDateMonth, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(cboCardDateYear, new ControlEntrySetting(true, false, false));
        }

        public override void OnAddRecord()
        {
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
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                watermarkText = entity.TransactionStatusWatermark;
            }
            hdnPaymentHdID.Value = entity.PaymentID.ToString();
            txtPaymentNo.Text = entity.PaymentNo;
            txtPaymentDate.Text = entity.PaymentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPaymentTime.Text = entity.PaymentTime;
            txtRemarks.Text = entity.Remarks;
            txtBillingNo.Text = entity.BillingNo;
            txtCoverageLimit.Text = hdnCoverageLimit.Value;

            txtBillingTotal.Text = entity.TotalBillAmount.ToString("N");
            txtBillingTotalPatient.Text = entity.TotalPatientBillAmount.ToString("N");
            txtBillingTotalPayer.Text = entity.TotalPayerBillAmount.ToString("N");

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
            txtPaymentTotalPatient.Text = totalPatient.ToString("N");
            txtPaymentTotalPayer.Text = totalPayer.ToString("N");

            txtCashbackAmount.Text = entity.CashBackAmount.ToString("N");

            BindGrdPaymentDetail();
        }

        private void BindGrdPaymentDetail()
        {
            List<vPatientPaymentDt> lstDt = BusinessLayer.GetvPatientPaymentDtList(string.Format("PaymentID = {0}", hdnPaymentHdID.Value));
            lvwPaymentDt.DataSource = lstDt;
            lvwPaymentDt.DataBind();

            Decimal patientAmount = lstDt.Select(p => p.PaymentAmount).Sum();
            Decimal cardFeeAmount = lstDt.Select(p => p.CardFeeAmount).Sum();
            tdTotalPatientEdit.InnerHtml = patientAmount.ToString("N");
            tdTotalCardFeeEdit.InnerHtml = cardFeeAmount.ToString("N");
            tdLineTotalEdit.InnerHtml = (patientAmount + cardFeeAmount).ToString("N");
        }

        protected void cbpPaymentDt_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGrdPaymentDetail();
        }
        #endregion

        #region Save Entity
        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            String[] listParam = hdnInlineEditingData.Value.Split('|');

            IDbContext ctx = DbFactory.Configure(true);
            PatientPaymentHdDao entityHdDao = new PatientPaymentHdDao(ctx);
            PatientPaymentDtDao entityDtDao = new PatientPaymentDtDao(ctx);
            PatientBillPaymentDao patientBillPaymentDao = new PatientBillPaymentDao(ctx);
            PatientBillDao patientBillDao = new PatientBillDao(ctx);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            RegistrationDao registrationDao = new RegistrationDao(ctx);
            HealthcareServiceUnitDao healthcareServiceUnitDao = new HealthcareServiceUnitDao(ctx);
            try
            {
                #region Payment Hd
                PatientPaymentHd entityHd = new PatientPaymentHd();
                entityHd.PaymentDate = Helper.GetDatePickerValue(txtPaymentDate);
                entityHd.PaymentTime = txtPaymentTime.Text;
                entityHd.GCPaymentType = cboPaymentType.Value.ToString();
                entityHd.RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);

                List<PatientBill> lstPatientBill  = null;
                if (hdnListBillingID.Value != "")
                    lstPatientBill = BusinessLayer.GetPatientBillList(string.Format("PatientBillingID IN ({0})", hdnListBillingID.Value), ctx);


                if (entityHd.GCPaymentType == Constant.PaymentType.SETTLEMENT || entityHd.GCPaymentType == Constant.PaymentType.DOWN_PAYMENT)
                {
                    entityHd.TotalPaymentAmount = Convert.ToInt32(hdnTotalPaymentAmount.Value);
                    entityHd.TotalFeeAmount = Convert.ToInt32(hdnTotalFeeAmount.Value);
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
                                entityHd.TotalPayerBillAmount = Convert.ToInt32(hdnBillingTotalPayer.Value);
                            }
                        }
                    }
                    else
                    {
                        entityHd.TotalPatientBillAmount = Convert.ToInt32(hdnBillingTotalPatient.Value);
                        entityHd.TotalPayerBillAmount = Convert.ToInt32(hdnBillingTotalPayer.Value);
                    }
                }
                else
                {
                    entityHd.TotalPatientBillAmount = Convert.ToInt32(hdnBillingTotalPatient.Value);
                    entityHd.TotalPayerBillAmount = Convert.ToInt32(hdnBillingTotalPayer.Value);
                    if (entityHd.GCPaymentType == Constant.PaymentType.AR_PATIENT)
                    {
                        entityHd.TotalPaymentAmount = entityHd.TotalPatientBillAmount;
                        entityHd.TotalFeeAmount = 0;
                    }
                    else
                    {
                        entityHd.TotalPaymentAmount = entityHd.TotalPayerBillAmount;
                        entityHd.TotalFeeAmount = 0;
                    }
                }
                
                entityHd.Remarks = txtRemarks.Text;
                if (entityHd.GCPaymentType == Constant.PaymentType.SETTLEMENT)
                {
                    entityHd.CashBackAmount = Convert.ToDecimal(Request.Form[txtCashbackAmount.UniqueID]);
                    if (entityHd.CashBackAmount > 0)
                        entityHd.TotalPaymentAmount = entityHd.TotalPatientBillAmount + entityHd.TotalPayerBillAmount;
                }
                else
                    entityHd.CashBackAmount = 0;

                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                string transactionCode = "";
                switch (hdnDepartmentID.Value)
                {
                    case Constant.Facility.INPATIENT:
                        switch (entityHd.GCPaymentType)
                        {
                            case Constant.PaymentType.DOWN_PAYMENT: transactionCode = Constant.TransactionCode.IP_PATIENT_PAYMENT_DP; break;
                            case Constant.PaymentType.SETTLEMENT: transactionCode = Constant.TransactionCode.IP_PATIENT_PAYMENT_SETTLEMENT; break;
                            case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.IP_PATIENT_PAYMENT_AR_PATIENT; break;
                            default: transactionCode = Constant.TransactionCode.IP_PATIENT_PAYMENT_AR_PAYER; break;
                        } break;
                    case Constant.Facility.EMERGENCY:
                        switch (entityHd.GCPaymentType)
                        {
                            case Constant.PaymentType.DOWN_PAYMENT: transactionCode = Constant.TransactionCode.ER_PATIENT_PAYMENT_DP; break;
                            case Constant.PaymentType.SETTLEMENT: transactionCode = Constant.TransactionCode.ER_PATIENT_PAYMENT_SETTLEMENT; break;
                            case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.ER_PATIENT_PAYMENT_AR_PATIENT; break;
                            default: transactionCode = Constant.TransactionCode.ER_PATIENT_PAYMENT_AR_PAYER; break;
                        } break;
                    case Constant.Facility.DIAGNOSTIC:
                        if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        {
                            switch (entityHd.GCPaymentType)
                            {
                                case Constant.PaymentType.DOWN_PAYMENT: transactionCode = Constant.TransactionCode.LABORATORY_PATIENT_PAYMENT_DP; break;
                                case Constant.PaymentType.SETTLEMENT: transactionCode = Constant.TransactionCode.LABORATORY_PATIENT_PAYMENT_SETTLEMENT; break;
                                case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.LABORATORY_PATIENT_PAYMENT_AR_PATIENT; break;
                                default: transactionCode = Constant.TransactionCode.LABORATORY_PATIENT_PAYMENT_AR_PAYER; break;
                            }
                        }
                        else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        {
                            switch (entityHd.GCPaymentType)
                            {
                                case Constant.PaymentType.DOWN_PAYMENT: transactionCode = Constant.TransactionCode.IMAGING_PATIENT_PAYMENT_DP; break;
                                case Constant.PaymentType.SETTLEMENT: transactionCode = Constant.TransactionCode.IMAGING_PATIENT_PAYMENT_SETTLEMENT; break;
                                case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.IMAGING_PATIENT_PAYMENT_AR_PATIENT; break;
                                default: transactionCode = Constant.TransactionCode.IMAGING_PATIENT_PAYMENT_AR_PAYER; break;
                            }
                        }
                        else
                        {
                            switch (entityHd.GCPaymentType)
                            {
                                case Constant.PaymentType.DOWN_PAYMENT: transactionCode = Constant.TransactionCode.OTHER_PATIENT_PAYMENT_DP; break;
                                case Constant.PaymentType.SETTLEMENT: transactionCode = Constant.TransactionCode.OTHER_PATIENT_PAYMENT_SETTLEMENT; break;
                                case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.OTHER_PATIENT_PAYMENT_AR_PATIENT; break;
                                default: transactionCode = Constant.TransactionCode.OTHER_PATIENT_PAYMENT_AR_PAYER; break;
                            }
                        } break;
                    default:
                        switch (entityHd.GCPaymentType)
                        {
                            case Constant.PaymentType.DOWN_PAYMENT: transactionCode = Constant.TransactionCode.OP_PATIENT_PAYMENT_DP; break;
                            case Constant.PaymentType.SETTLEMENT: transactionCode = Constant.TransactionCode.OP_PATIENT_PAYMENT_SETTLEMENT; break;
                            case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.OP_PATIENT_PAYMENT_AR_PATIENT; break;
                            default: transactionCode = Constant.TransactionCode.OP_PATIENT_PAYMENT_AR_PAYER; break;
                        } break;
                }
                entityHd.PaymentNo = BusinessLayer.GenerateTransactionNo(transactionCode, entityHd.PaymentDate, ctx);
                entityHd.CreatedBy = entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                entityHdDao.Insert(entityHd);
                entityHd.PaymentID = BusinessLayer.GetPatientPaymentHdMaxID(ctx);
                #endregion

                #region Payment Dt
                if (entityHd.GCPaymentType == Constant.PaymentType.SETTLEMENT || entityHd.GCPaymentType == Constant.PaymentType.DOWN_PAYMENT)
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
                                    entityDt.CardNumber = string.Format("XXXX-XXXX-XXXX-{0}", data[11]);
                                else
                                    entityDt.CardNumber = "";
                                entityDt.CardHolderName = data[12];
                                entityDt.CardValidThru = string.Format("{0:00}/{1:00}", data[13].PadLeft(2, '0'), data[14].Substring(2));
                                entityDt.GCCardProvider = data[16];
                            }
                            entityDt.PaymentAmount = Convert.ToDecimal(data[7].Replace(",00", "").Replace(".", ""));
                            entityDt.CardFeeAmount = Convert.ToDecimal(data[8].Replace(",00", "").Replace(".", ""));
                            entityDt.CreatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Insert(entityDt);
                        }
                    }
                }
                else
                {
                    PatientPaymentDt entityDt = new PatientPaymentDt();
                    entityDt.PaymentID = entityHd.PaymentID;
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
                        if (totalPaymentAmount > 0)
                        {
                            oldBillStatus = patientBill.GCTransactionStatus;
                            patientBill.PaymentID = entityHd.PaymentID;
                            PatientBillPayment patientBillPayment = new PatientBillPayment();
                            patientBillPayment.PaymentID = entityHd.PaymentID;
                            patientBillPayment.PatientBillingID = patientBill.PatientBillingID;

                            if (patientBill.PatientRemainingAmount < totalPaymentAmount)
                            {
                                totalPaymentAmount -= patientBill.PatientRemainingAmount;
                                if (entityHd.GCPaymentType == Constant.PaymentType.AR_PAYER)
                                    patientBillPayment.PayerPaymentAmount = patientBill.TotalPayerPaymentAmount = patientBill.TotalPatientAmount;
                                else
                                {
                                    patientBillPayment.PatientPaymentAmount = patientBill.PatientRemainingAmount;
                                    patientBill.TotalPatientPaymentAmount = patientBill.TotalPatientAmount;
                                }
                            }
                            else
                            {
                                if (entityHd.GCPaymentType == Constant.PaymentType.AR_PAYER)
                                    patientBillPayment.PayerPaymentAmount = patientBill.TotalPayerPaymentAmount += totalPaymentAmount;
                                else
                                {
                                    patientBill.TotalPatientPaymentAmount += totalPaymentAmount;
                                    patientBillPayment.PatientPaymentAmount = totalPaymentAmount;
                                }
                                totalPaymentAmount = 0;
                            }

                            if (patientBill.RemainingAmount < 1)
                            {
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
                            
                            patientBillPaymentDao.Insert(patientBillPayment);
                        }
                    }
                }
                #endregion

                #region Auto Close Registration
                if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
                {
                    ConsultVisit consultVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", hdnRegistrationID.Value), ctx).FirstOrDefault();
                    HealthcareServiceUnit healthcareServiceUnit = healthcareServiceUnitDao.Get(consultVisit.HealthcareServiceUnitID);
                    if (healthcareServiceUnit.IsAutoCloseRegistration)
                    {
                        int count = BusinessLayer.GetvPatientChargesHdRowCount(string.Format("VisitID = {0} AND GCTransactionStatus NOT IN ('{1},'{2}')", consultVisit.VisitID, Constant.TransactionStatus.VOID, Constant.TransactionStatus.CLOSED));
                        if (count < 0)
                        {
                            Registration registration = registrationDao.Get(Convert.ToInt32(hdnRegistrationID.Value));
                            registration.GCRegistrationStatus = Constant.VisitStatus.CLOSED;
                            registration.LastUpdatedBy = AppSession.UserLogin.UserID;
                            registrationDao.Update(registration);
                        }
                    }
                }
                #endregion
                ctx.CommitTransaction();
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
        #endregion

        #region Void Entity
        protected override bool OnVoidRecord(ref string errMessage)
        {
            bool result = true;

            IDbContext ctx = DbFactory.Configure(true);
            PatientPaymentHdDao entityDao = new PatientPaymentHdDao(ctx);
            RegistrationDao registrationDao = new RegistrationDao(ctx);
            PatientBillDao patientBillDao = new PatientBillDao(ctx);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            PatientBillPaymentDao patientBillPaymentDao = new PatientBillPaymentDao(ctx);
            try
            {
                Registration registration = registrationDao.Get(Convert.ToInt32(hdnRegistrationID.Value));
                
                if (registration.GCRegistrationStatus == Constant.VisitStatus.CLOSED)
                {
                    errMessage = Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_CLOSED_REGISTRATION_VALIDATION);
                    result = false;
                }
                else
                {
                    PatientPaymentHd entity = entityDao.Get(Convert.ToInt32(hdnPaymentHdID.Value));
                    entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDao.Update(entity);

                    List<PatientBillPayment> lstPatientBillPayment = BusinessLayer.GetPatientBillPaymentList(String.Format("PaymentID = {0}", hdnPaymentHdID.Value), ctx);
                    List<PatientBill> lstPatientBill = BusinessLayer.GetPatientBillList(string.Format("PatientBillingID IN (SELECT PatientBillingID FROM PatientBillPayment WHERE PaymentID = {0})", hdnPaymentHdID.Value), ctx);

                    string oldBillStatus = null; 
                    foreach (PatientBill patientBill in lstPatientBill)
                    {
                        oldBillStatus = patientBill.GCTransactionStatus;

                        PatientBillPayment objPatientBillPayment = lstPatientBillPayment.FirstOrDefault(x => x.PatientBillingID == patientBill.PatientBillingID);
                        patientBill.TotalPatientPaymentAmount -= objPatientBillPayment.PatientPaymentAmount;
                        patientBill.TotalPayerPaymentAmount -= objPatientBillPayment.PayerPaymentAmount;
                        patientBillPaymentDao.Delete(objPatientBillPayment.PaymentID, objPatientBillPayment.PatientBillingID);

                        patientBill.PaymentID = null;
                        patientBill.GCTransactionStatus = Constant.TransactionStatus.OPEN;

                        if (oldBillStatus != patientBill.GCTransactionStatus)
                        {
                            patientBill.LastUpdatedBy = AppSession.UserLogin.UserID;
                            patientBill.LastUpdatedDate = DateTime.Now;
                        }

                        patientBillDao.Update(patientBill);
                    }

                    List<PatientChargesHd> lstPatientChargesHd = BusinessLayer.GetPatientChargesHdList(string.Format("PatientBillingID IN (SELECT PatientBillingID FROM PatientBill WHERE PaymentID = {0})", hdnPaymentHdID.Value), ctx);
                    foreach (PatientChargesHd objPatientChargesHd in lstPatientChargesHd) 
                    {
                        objPatientChargesHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                        objPatientChargesHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        patientChargesHdDao.Update(objPatientChargesHd);
                    }
                }
                ctx.CommitTransaction();
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
    }
}
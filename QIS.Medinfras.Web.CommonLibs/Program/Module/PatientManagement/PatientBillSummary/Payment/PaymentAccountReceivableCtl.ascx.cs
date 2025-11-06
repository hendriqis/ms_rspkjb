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
    public partial class PaymentAccountReceivableCtl : BaseContentPopupCtl
    {
        public override void InitializeControl(string param)
        {
            hdnIsFinalisasiKlaimAfterARInvoiceCtlAR.Value = AppSession.IsClaimFinalAfterARInvoice ? "1" : "0";
            hdnIsFinalisasiKlaimBeforeARInvoiceAndSkipProcessKlaimCtlAR.Value = AppSession.IsClaimFinalBeforeARInvoiceAndSkipProcessClaim ? "1" : "0";

            hdnIsGrouperAmountClaimDefaultZeroCtlAR.Value = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_IS_GROUPER_AMOUNT_CLAIM_DEFAULT_ZERO).ParameterValue;

            string[] temp = param.Split('|');
            hdnRegistrationIDCtlAR.Value = temp[0];
            hdnDepartmentIDCtlAR.Value = temp[1];
            hdnPatientBillingIDCtlAR.Value = temp[2];
            hdnCashierGroupCtlAR.Value = temp[3];
            hdnShiftCtlAR.Value = temp[4];
            hdnTxtPaymentDateCtlAR.Value = temp[5];
            hdnTxtPaymentTimeCtlAR.Value = temp[6];
            hdnTglPiutangPribadiCtlAR.Value = temp[7];

            List<PatientBill> lst = BusinessLayer.GetPatientBillList(string.Format("PatientBillingID IN ({0})", hdnPatientBillingIDCtlAR.Value));
            decimal patientAmount = lst.Sum(p => p.PatientRemainingAmount);
            if (patientAmount < 1)
            {
                trPatientAmount.Style.Add("display", "none");
            }
            hdnTotalPatientAmountCtlAR.Value = patientAmount.ToString();
            txtPatientBillAmount.Text = patientAmount.ToString(Constant.FormatString.NUMERIC_2);

            decimal payerAmount = lst.Sum(p => p.PayerRemainingAmount);
            if (payerAmount < 1)
            {
                trPayerAmount.Style.Add("display", "none");
            }
            hdnTotalPayerAmountCtlAR.Value = payerAmount.ToString();
            txtPayerBillAmount.Text = payerAmount.ToString(Constant.FormatString.NUMERIC_2);
        }

        protected void cbpProcessPopup_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string retval = "";
            bool isCloseRegistration = false;
            int retcount = 0;
            if (OnProcessRecord(ref errMessage, ref retval, ref retcount, ref isCloseRegistration))
                result = "success";
            else
                result = "fail|" + errMessage;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = retval;
            panel.JSProperties["cpCount"] = retcount;
            panel.JSProperties["cpIsCloseRegistration"] = isCloseRegistration ? "1" : "0";
        }

        private bool OnProcessRecord(ref string errMessage, ref string retval, ref int retcount, ref bool isCloseRegistration)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientPaymentHdDao entityHdDao = new PatientPaymentHdDao(ctx);
            PatientPaymentDtDao entityDtDao = new PatientPaymentDtDao(ctx);
            PatientPaymentDtInfoDao entityDtInfoDao = new PatientPaymentDtInfoDao(ctx);
            PatientBillPaymentDao patientBillPaymentDao = new PatientBillPaymentDao(ctx);
            PatientBillDao patientBillDao = new PatientBillDao(ctx);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            RegistrationDao registrationDao = new RegistrationDao(ctx);
            HealthcareServiceUnitDao healthcareServiceUnitDao = new HealthcareServiceUnitDao(ctx);
            try
            {
                List<String> lstGCPaymentType = new List<String>();
                lstGCPaymentType.Add(Constant.PaymentType.AR_PATIENT);
                lstGCPaymentType.Add(Constant.PaymentType.AR_PAYER);
                Registration entityRegis = registrationDao.Get(Convert.ToInt32(hdnRegistrationIDCtlAR.Value));
                int businessPartnerID = entityRegis.BusinessPartnerID;

                foreach (String GCPaymentType in lstGCPaymentType)
                {
                    decimal totalAmount = 0;
                    if (GCPaymentType == Constant.PaymentType.AR_PATIENT)
                        totalAmount = Convert.ToDecimal(hdnTotalPatientAmountCtlAR.Value);
                    else
                        totalAmount = Convert.ToDecimal(hdnTotalPayerAmountCtlAR.Value);

                    if (totalAmount > 0)
                    {
                        retcount++;

                        List<PatientBill> lstPatientBill = null;
                        if (hdnPatientBillingIDCtlAR.Value != "")
                            lstPatientBill = BusinessLayer.GetPatientBillList(string.Format("PatientBillingID IN ({0})", hdnPatientBillingIDCtlAR.Value), ctx);

                        #region Payment Hd
                        PatientPaymentHd entityHd = new PatientPaymentHd();
                        entityHd.GCPaymentType = GCPaymentType;
                        entityHd.RegistrationID = Convert.ToInt32(hdnRegistrationIDCtlAR.Value);
                        entityHd.GCCashierGroup = hdnCashierGroupCtlAR.Value;
                        entityHd.GCShift = hdnShiftCtlAR.Value;

                        entityHd.TotalPatientBillAmount = Convert.ToDecimal(hdnTotalPatientAmountCtlAR.Value);
                        entityHd.TotalPayerBillAmount = Convert.ToDecimal(hdnTotalPayerAmountCtlAR.Value);
                        if (entityHd.GCPaymentType == Constant.PaymentType.AR_PATIENT)
                        {
                            if (hdnTglPiutangPribadiCtlAR.Value == "1")
                            {
                                entityHd.PaymentDate = Helper.GetDatePickerValue(hdnTxtPaymentDateCtlAR.Value);
                                entityHd.PaymentTime = hdnTxtPaymentTimeCtlAR.Value;
                            }
                            else
                            {
                                entityHd.PaymentDate = DateTime.Now;
                                entityHd.PaymentTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                            }

                            entityHd.TotalPaymentAmount = entityHd.TotalPatientBillAmount;
                            entityHd.TotalFeeAmount = 0;
                        }
                        else
                        {
                            entityHd.PaymentDate = Helper.GetDatePickerValue(hdnTxtPaymentDateCtlAR.Value);
                            entityHd.PaymentTime = hdnTxtPaymentTimeCtlAR.Value;

                            entityHd.TotalPaymentAmount = entityHd.TotalPayerBillAmount;
                            entityHd.TotalFeeAmount = 0;
                        }

                        entityHd.Remarks = "";
                        entityHd.CashBackAmount = 0;

                        entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        string transactionCode = "";
                        switch (hdnDepartmentIDCtlAR.Value)
                        {
                            case Constant.Facility.INPATIENT:
                                switch (entityHd.GCPaymentType)
                                {
                                    case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.IP_PATIENT_PAYMENT_AR_PATIENT; break;
                                    default: transactionCode = Constant.TransactionCode.IP_PATIENT_PAYMENT_AR_PAYER; break;
                                } break;
                            case Constant.Facility.MEDICAL_CHECKUP:
                                switch (entityHd.GCPaymentType)
                                {
                                    case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.MCU_PATIENT_PAYMENT_AR_PATIENT; break;
                                    default: transactionCode = Constant.TransactionCode.MCU_PATIENT_PAYMENT_AR_PAYER; break;
                                } break;
                            case Constant.Facility.EMERGENCY:
                                switch (entityHd.GCPaymentType)
                                {
                                    case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.ER_PATIENT_PAYMENT_AR_PATIENT; break;
                                    default: transactionCode = Constant.TransactionCode.ER_PATIENT_PAYMENT_AR_PAYER; break;
                                } break;
                            case Constant.Facility.PHARMACY:
                                switch (entityHd.GCPaymentType)
                                {
                                    case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.PH_PATIENT_PAYMENT_AR_PATIENT; break;
                                    default: transactionCode = Constant.TransactionCode.PH_PATIENT_PAYMENT_AR_PAYER; break;
                                } break;
                            case Constant.Facility.DIAGNOSTIC:
                                if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                                {
                                    switch (entityHd.GCPaymentType)
                                    {
                                        case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.LABORATORY_PATIENT_PAYMENT_AR_PATIENT; break;
                                        default: transactionCode = Constant.TransactionCode.LABORATORY_PATIENT_PAYMENT_AR_PAYER; break;
                                    }
                                }
                                else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                                {
                                    switch (entityHd.GCPaymentType)
                                    {
                                        case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.IMAGING_PATIENT_PAYMENT_AR_PATIENT; break;
                                        default: transactionCode = Constant.TransactionCode.IMAGING_PATIENT_PAYMENT_AR_PAYER; break;
                                    }
                                }
                                else
                                {
                                    switch (entityHd.GCPaymentType)
                                    {
                                        case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.OTHER_PATIENT_PAYMENT_AR_PATIENT; break;
                                        default: transactionCode = Constant.TransactionCode.OTHER_PATIENT_PAYMENT_AR_PAYER; break;
                                    }
                                } break;
                            default:
                                switch (entityHd.GCPaymentType)
                                {
                                    case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.OP_PATIENT_PAYMENT_AR_PATIENT; break;
                                    default: transactionCode = Constant.TransactionCode.OP_PATIENT_PAYMENT_AR_PAYER; break;
                                } break;
                        }
                        entityHd.PaymentNo = BusinessLayer.GenerateTransactionNo(transactionCode, entityHd.PaymentDate, ctx);
                        entityHd.CreatedBy = entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityHd.PaymentID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
                        #endregion

                        #region Payment Dt
                        PatientPaymentDt entityDt = new PatientPaymentDt();
                        entityDt.PaymentID = entityHd.PaymentID;
                        if (GCPaymentType == Constant.PaymentType.AR_PAYER)
                        {
                            entityDt.BusinessPartnerID = businessPartnerID;
                        }
                        else
                        {
                            entityDt.BusinessPartnerID = 1;
                        }
                        entityDt.GCPaymentMethod = Constant.PaymentMethod.CREDIT;
                        entityDt.PaymentAmount = entityHd.TotalPaymentAmount;
                        entityDt.CardFeeAmount = 0;
                        entityDt.CreatedBy = AppSession.UserLogin.UserID;
                        int paymentDetailID = entityDtDao.InsertReturnPrimaryKeyID(entityDt);

                        string filterC = string.Format("BusinessPartnerID = '{0}'", entityDt.BusinessPartnerID);
                        vCustomer oCustomer = BusinessLayer.GetvCustomerList(filterC, ctx).FirstOrDefault();

                        if (oCustomer.GCCustomerType == Constant.CustomerType.BPJS)
                        {
                            PatientPaymentDtInfo dtInfo = new PatientPaymentDtInfo();
                            dtInfo.PaymentDetailID = paymentDetailID;

                            if (hdnIsFinalisasiKlaimAfterARInvoiceCtlAR.Value == "0" && hdnIsFinalisasiKlaimBeforeARInvoiceAndSkipProcessKlaimCtlAR.Value == "1")
                            {
                                dtInfo.GCClaimStatus = Constant.ClaimStatus.APPROVED;
                                dtInfo.ClaimBy = AppSession.UserLogin.UserID;
                                dtInfo.ClaimDate = DateTime.Now;

                                dtInfo.GCFinalStatus = Constant.FinalStatus.OPEN;

                                if (hdnIsGrouperAmountClaimDefaultZeroCtlAR.Value == "0")
                                {
                                    dtInfo.GrouperAmountClaim = entityDt.PaymentAmount;
                                    dtInfo.GrouperAmountFinal = entityDt.PaymentAmount;
                                }
                            }
                            else if (hdnIsFinalisasiKlaimAfterARInvoiceCtlAR.Value == "1" && hdnIsFinalisasiKlaimBeforeARInvoiceAndSkipProcessKlaimCtlAR.Value == "1")
                            {
                                dtInfo.GCClaimStatus = Constant.ClaimStatus.APPROVED;
                                dtInfo.ClaimBy = AppSession.UserLogin.UserID;
                                dtInfo.ClaimDate = DateTime.Now;

                                if (hdnIsGrouperAmountClaimDefaultZeroCtlAR.Value == "0")
                                {
                                    dtInfo.GrouperAmountClaim = entityDt.PaymentAmount;
                                    dtInfo.GrouperAmountFinal = entityDt.PaymentAmount;
                                }
                            }
                            else
                            {
                                dtInfo.GCClaimStatus = Constant.ClaimStatus.OPEN;

                                if (hdnIsGrouperAmountClaimDefaultZeroCtlAR.Value == "0")
                                {
                                    dtInfo.GrouperAmountClaim = entityDt.PaymentAmount;
                                }
                            }

                            dtInfo.SequenceNo = Convert.ToInt32(entityRegis.RegistrationDate.ToString("dd"));
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityDtInfoDao.Insert(dtInfo);
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
                            dtInfo.SequenceNo = Convert.ToInt32(entityRegis.RegistrationDate.ToString("dd"));
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityDtInfoDao.Insert(dtInfo);
                        }
                        #endregion

                        #region Update Billing
                        List<PatientChargesHd> lstPatientChargesHd = BusinessLayer.GetPatientChargesHdList(string.Format("PatientBillingID IN ({0})", hdnPatientBillingIDCtlAR.Value), ctx);
                        decimal totalPaymentAmount = entityHd.TotalPaymentAmount;
                        foreach (PatientBill patientBill in lstPatientBill)
                        {
                            if (totalPaymentAmount != 0)
                            {
                                patientBill.PaymentID = entityHd.PaymentID;
                                PatientBillPayment patientBillPayment = new PatientBillPayment();
                                patientBillPayment.PaymentID = entityHd.PaymentID;
                                patientBillPayment.PatientBillingID = patientBill.PatientBillingID;

                                if (entityHd.GCPaymentType == Constant.PaymentType.AR_PAYER)
                                {
                                    decimal totalRemainingOthers = lstPatientBill.Where(a => a.PatientBillingID != patientBill.PatientBillingID).ToList().Sum(b => b.PayerRemainingAmount);
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

                                    //if (patientBill.PayerRemainingAmount < totalPaymentAmount)
                                    //    totalPaymentAmount -= patientBill.PayerRemainingAmount;
                                    //else
                                    //    totalPaymentAmount = 0;
                                    //patientBillPayment.PayerPaymentAmount = patientBill.PayerRemainingAmount;
                                    //patientBill.TotalPayerPaymentAmount += patientBill.PayerRemainingAmount;
                                }
                                else
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

                                    #region old
                                    //if (patientBill.PatientRemainingAmount != 0)
                                    //{
                                    //    if (totalPaymentAmount > patientBill.PatientRemainingAmount)
                                    //    {
                                    //        patientBillPayment.PatientPaymentAmount = patientBill.PatientRemainingAmount;
                                    //        patientBill.TotalPatientPaymentAmount += patientBillPayment.PatientPaymentAmount;

                                    //        totalPaymentAmount = totalPaymentAmount - patientBillPayment.PatientPaymentAmount;
                                    //    }
                                    //    else
                                    //    {
                                    //        patientBill.TotalPatientPaymentAmount += totalPaymentAmount;
                                    //        patientBillPayment.PatientPaymentAmount = totalPaymentAmount;

                                    //        totalPaymentAmount = totalPaymentAmount - totalPaymentAmount;
                                    //    }
                                    //}

                                    ////if (patientBill.PatientRemainingAmount < totalPaymentAmount)
                                    ////    totalPaymentAmount -= patientBill.PatientRemainingAmount;
                                    ////else
                                    ////    totalPaymentAmount = 0;
                                    ////patientBillPayment.PatientPaymentAmount = patientBill.PatientRemainingAmount;
                                    ////patientBill.TotalPatientPaymentAmount += patientBill.PatientRemainingAmount;
                                    #endregion
                                }

                                if (patientBillPayment.PayerPaymentAmount != 0 || patientBillPayment.PatientPaymentAmount != 0)
                                {
                                    patientBill.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                                    List<PatientChargesHd> lstChargesHd = lstPatientChargesHd.Where(p => p.PatientBillingID == patientBill.PatientBillingID).ToList();
                                    foreach (PatientChargesHd patientChargesHd in lstChargesHd)
                                    {
                                        patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                                        patientChargesHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        patientChargesHdDao.Update(patientChargesHd);
                                    }
                                    patientBill.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    patientBill.LastUpdatedDate = DateTime.Now;
                                    patientBillDao.Update(patientBill);

                                    patientBillPaymentDao.Insert(patientBillPayment);
                                }
                            }
                        }
                        #endregion

                        #region Auto Close Registration
                        //if (hdnDepartmentIDCtlAR.Value == Constant.Facility.OUTPATIENT)
                        //{
                        //    ConsultVisit consultVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", hdnRegistrationIDCtlAR.Value), ctx).FirstOrDefault();
                        //    HealthcareServiceUnit healthcareServiceUnit = healthcareServiceUnitDao.Get(consultVisit.HealthcareServiceUnitID);
                        //    if (healthcareServiceUnit.IsAutoCloseRegistration)
                        //    {
                        //        int count = BusinessLayer.GetvPatientChargesHdRowCount(string.Format("VisitID = {0} AND GCTransactionStatus NOT IN ('{1},'{2}')", consultVisit.VisitID, Constant.TransactionStatus.VOID, Constant.TransactionStatus.CLOSED));
                        //        if (count < 0)
                        //        {
                        //            Registration registration = registrationDao.Get(Convert.ToInt32(hdnRegistrationIDCtlAR.Value));
                        //            registration.GCRegistrationStatus = Constant.RegistrationStatus.CLOSED;
                        //            registration.LastUpdatedBy = AppSession.UserLogin.UserID;
                        //            registrationDao.Update(registration);
                        //        }
                        //    }
                        //}
                        //vRegistrationOutstandingInfo vRegistrationOutstandingInfo = BusinessLayer.GetvRegistrationOutstandingInfoList(string.Format("RegistrationID = {0}", hdnRegistrationIDCtlAR.Value), ctx).FirstOrDefault();

                        //ConsultVisit consultVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", hdnRegistrationIDCtlAR.Value), ctx).FirstOrDefault();
                        //HealthcareServiceUnit healthcareServiceUnit = healthcareServiceUnitDao.Get(consultVisit.HealthcareServiceUnitID);
                        //if (healthcareServiceUnit.IsAutoCloseRegistration && vRegistrationOutstandingInfo.Billing < 1 && vRegistrationOutstandingInfo.Charges < 1)
                        //    isCloseRegistration = true;
                        //else
                        //    isCloseRegistration = false;
                        #endregion

                        if (retval != "")
                            retval += "<br>";

                        if (GCPaymentType == Constant.PaymentType.AR_PATIENT)
                            retval += string.Format("Piutang Pribadi Berhasil Dibuat dengan Nomor <b>{0}</b>", entityHd.PaymentNo);
                        else
                            retval += string.Format("Piutang Instansi Berhasil Dibuat dengan Nomor <b>{0}</b>", entityHd.PaymentNo);
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
    }
}
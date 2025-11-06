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

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class PaymentAccountReceivable2Ctl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnIsGrouperAmountClaimDefaultZero.Value = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_IS_GROUPER_AMOUNT_CLAIM_DEFAULT_ZERO).ParameterValue;

            string[] temp = param.Split('|');
            hdnRegistrationID.Value = temp[0];
            hdnDepartmentID.Value = temp[1];
            hdnPatientBillingID.Value = temp[2];
            hdnCashierGroup.Value = temp[3];
            hdnShift.Value = temp[4];

            List<vPatientBill> lst = BusinessLayer.GetvPatientBillList(string.Format("PatientBillingID IN ({0})", hdnPatientBillingID.Value));
            decimal patientAmount = lst.Sum(p => p.PatientRemainingAmount);
            if (patientAmount < 1)
            {
                trPatientAmount.Style.Add("display", "none");
                BindGridView();
            }
            hdnTotalPatientAmount.Value = patientAmount.ToString();
            txtPatientBillAmount.Text = patientAmount.ToString("N");

            decimal payerAmount = lst.Sum(p => p.PayerRemainingAmount);
            if (payerAmount < 1)
            {
                trPayerAmount.Style.Add("display", "none");
            }
            hdnTotalPayerAmount.Value = payerAmount.ToString();
            txtPayerBillAmount.Text = payerAmount.ToString("N");
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);

            List<vRegistrationPayer> lstEntity = BusinessLayer.GetvRegistrationPayerList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

       protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
            }
        }

       protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridView();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
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
            String[] lstSelectedMember = hdnSelectedMember.Value.Substring(1).Split('|');
            String[] lstPayerAmount = hdnPayerAmount.Value.Substring(1).Split('|');

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
                Registration entityRegis = registrationDao.Get(Convert.ToInt32(hdnRegistrationID.Value));
                int businessPartnerID = entityRegis.BusinessPartnerID;

                decimal payerAmount = 0;
                decimal billing = Convert.ToDecimal(hdnTotalPayerAmount.Value);
                for (int a = 0; a < lstSelectedMember.Length; a++)
                {
                    payerAmount += Convert.ToDecimal(lstPayerAmount[a]);
                }

                if (payerAmount <= billing)
                {
                    foreach (String GCPaymentType in lstGCPaymentType)
                    {
                        decimal totalAmount = 0;
                        if (GCPaymentType == Constant.PaymentType.AR_PATIENT)
                            totalAmount = Convert.ToDecimal(hdnTotalPatientAmount.Value);
                        else
                            totalAmount = Convert.ToDecimal(hdnTotalPayerAmount.Value);

                        if (totalAmount > 0)
                        {
                            retcount++;
                            #region Payment Hd
                            PatientPaymentHd entityHd = new PatientPaymentHd();
                            entityHd.PaymentDate = DateTime.Now;
                            entityHd.PaymentTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                            entityHd.GCPaymentType = GCPaymentType;
                            entityHd.RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
                            entityHd.GCCashierGroup = hdnCashierGroup.Value;
                            entityHd.GCShift = hdnShift.Value;

                            List<PatientBill> lstPatientBill = null;
                            if (hdnPatientBillingID.Value != "")
                                lstPatientBill = BusinessLayer.GetPatientBillList(string.Format("PatientBillingID IN ({0})", hdnPatientBillingID.Value), ctx);

                            entityHd.TotalPatientBillAmount = Convert.ToDecimal(hdnTotalPatientAmount.Value);
                            entityHd.TotalPayerBillAmount = Convert.ToDecimal(hdnTotalPayerAmount.Value);
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

                            entityHd.Remarks = "";
                            entityHd.CashBackAmount = 0;

                            entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                            string transactionCode = "";
                            switch (hdnDepartmentID.Value)
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
                            for (int i = 0; i < lstSelectedMember.Length; i++)
                            {
                                PatientPaymentDt entityDt = new PatientPaymentDt();
                                entityDt.PaymentID = entityHd.PaymentID;
                                if (GCPaymentType == Constant.PaymentType.AR_PAYER)
                                {
                                    entityDt.BusinessPartnerID = Convert.ToInt32(lstSelectedMember[i]);
                                }
                                else
                                {
                                    entityDt.BusinessPartnerID = 1;
                                }
                                entityDt.GCPaymentMethod = Constant.PaymentMethod.CREDIT;
                                entityDt.PaymentAmount = Convert.ToDecimal(lstPayerAmount[i]);
                                entityDt.CardFeeAmount = 0;
                                entityDt.CreatedBy = AppSession.UserLogin.UserID;

                                int paymentDetailID = entityDtDao.InsertReturnPrimaryKeyID(entityDt);

                                string filterC = string.Format("BusinessPartnerID = '{0}'", lstSelectedMember[i]);
                                vCustomer oCustomer = BusinessLayer.GetvCustomerList(filterC, ctx).FirstOrDefault();

                                if (oCustomer.GCCustomerType == Constant.CustomerType.BPJS)
                                {
                                    PatientPaymentDtInfo dtInfo = new PatientPaymentDtInfo();
                                    dtInfo.PaymentDetailID = paymentDetailID;
                                    dtInfo.GCClaimStatus = Constant.ClaimStatus.OPEN;
                                    if (hdnIsGrouperAmountClaimDefaultZero.Value != "1")
                                    {
                                        dtInfo.GrouperAmountClaim = entityDt.PaymentAmount;
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
                            }

                            #region Update Billing
                            List<PatientChargesHd> lstPatientChargesHd = BusinessLayer.GetPatientChargesHdList(string.Format("PatientBillingID IN ({0})", hdnPatientBillingID.Value), ctx);
                            decimal totalPaymentAmount = entityHd.TotalPaymentAmount;
                            foreach (PatientBill patientBill in lstPatientBill)
                            {
                                if (totalPaymentAmount > 0)
                                {
                                    patientBill.PaymentID = entityHd.PaymentID;
                                    PatientBillPayment patientBillPayment = new PatientBillPayment();
                                    patientBillPayment.PaymentID = entityHd.PaymentID;
                                    patientBillPayment.PatientBillingID = patientBill.PatientBillingID;

                                    if (entityHd.GCPaymentType == Constant.PaymentType.AR_PAYER)
                                    {
                                        if (patientBill.PayerRemainingAmount < totalPaymentAmount)
                                        {
                                            totalPaymentAmount -= patientBill.PayerRemainingAmount;
                                        }
                                        else
                                        {
                                            totalPaymentAmount = 0;
                                            patientBillPayment.PayerPaymentAmount = patientBill.PayerRemainingAmount;
                                            patientBill.TotalPayerPaymentAmount += patientBill.PayerRemainingAmount;
                                        }
                                    }
                                    else
                                    {
                                        if (patientBill.PatientRemainingAmount < totalPaymentAmount)
                                        {
                                            totalPaymentAmount -= patientBill.PatientRemainingAmount;
                                        }
                                        else
                                        {
                                            totalPaymentAmount = 0;
                                            patientBillPayment.PatientPaymentAmount = patientBill.PatientRemainingAmount;
                                            patientBill.TotalPatientPaymentAmount += patientBill.PatientRemainingAmount;
                                        }
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
                            #endregion

                            if (retval != "")
                                retval += "<br>";

                            if (GCPaymentType == Constant.PaymentType.AR_PATIENT)
                                retval += string.Format("Piutang Pribadi Sudah Dibuat Dengan Nomor <b>{0}</b>", entityHd.PaymentNo);
                            else
                                retval += string.Format("Piutang Instansi Sudah Dibuat Dengan Nomor <b>{0}</b>", entityHd.PaymentNo);

                        }
                    }
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = " Pembayaran lebih besar dari tagihan.";                    
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
    }
}
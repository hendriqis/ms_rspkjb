using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.CommonLibs.Service;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PaymentVoidCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnParam.Value = param;
            hdnRegistrationIDCtlVoid.Value = hdnParam.Value.Split('|')[0];
            hdnPaymentIDCtlVoid.Value = hdnParam.Value.Split('|')[1];
            hdnOutstandingDPCtlVoid.Value = hdnParam.Value.Split('|')[2];
            List<StandardCode> lstVoidReason = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.DELETE_REASON));
            Methods.SetComboBoxField<StandardCode>(cboVoidReason, lstVoidReason, "StandardCodeName", "StandardCodeID");
            cboVoidReason.SelectedIndex = 0;

            string filterSetVar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}')",
                                                        AppSession.UserLogin.HealthcareID, //0
                                                        Constant.SettingParameter.SA_EDC_BRIDGING, //1
                                                        Constant.SettingParameter.SA_IS_BRIDGING_TO_PAYMENT_GATEWAY, //2
                                                        Constant.SettingParameter.FN_IS_VOID_PATIENTPAYMENT_USING_BLOCKER_VALIDATE_REVENUESHARING, //3
                                                        Constant.SettingParameter.FN_IS_VOID_PATIENTPAYMENT_USING_BLOCKER_VALIDATE_COPY_PAYMENT_RECONCILIATION_OR_USER_PATIENT_PAYMENT_BALANCE //4
                                                    );
            List<SettingParameterDt> lstSetVarDt = BusinessLayer.GetSettingParameterDtList(filterSetVar);

            SettingParameterDt oSetParEdc = lstSetVarDt.Where(w => w.ParameterCode == Constant.SettingParameter.SA_EDC_BRIDGING).FirstOrDefault();
            if (oSetParEdc != null)
            {
                hdnIsSetEdcBridging.Value = oSetParEdc.ParameterValue;
                if (oSetParEdc.ParameterValue == "1")
                {
                    List<PatientPaymentDt> lstDt = BusinessLayer.GetPatientPaymentDtList(string.Format("PaymentID = '{0}' AND IsDeleted=0", hdnPaymentIDCtlVoid.Value));
                    string PaymentDtID = string.Empty;
                    if (lstDt.Count > 0)
                    {

                        foreach (PatientPaymentDt row in lstDt)
                        {
                            PaymentDtID += string.Format("{0},", row.PaymentDetailID);
                        }
                        PaymentDtID = PaymentDtID.Remove(PaymentDtID.Length - 1);
                    }

                    string filterexp = string.Format("PaymentDetailID IN ({0}) AND RegistrationID='{1}' AND IsFinish = 1 AND IsVoid = 0", PaymentDtID, hdnRegistrationIDCtlVoid.Value);
                    List<EDCMachineTransaction> lstEdc = BusinessLayer.GetEDCMachineTransactionList(filterexp);
                    if (lstEdc.Count > 0)
                    {
                        hdnEdcTransaction.Value = "1";
                        trNotes.Attributes.Remove("class");
                        NoteText.InnerText = "Sudah ada pembayaran melalui EDC. Silahkan lakukan void pembayaran EDC terlebih dahulu di detail informasi kartu.";
                    }
                    else
                    {
                        hdnEdcTransaction.Value = "0";
                    }
                }
            }

            hdnIsBridgingToPaymentGatewayCtlVoid.Value = lstSetVarDt.FirstOrDefault(w => w.ParameterCode == Constant.SettingParameter.SA_IS_BRIDGING_TO_PAYMENT_GATEWAY).ParameterValue;
            hdnIsUsingBlockerValidateRevenueSharing.Value = lstSetVarDt.FirstOrDefault(w => w.ParameterCode == Constant.SettingParameter.FN_IS_VOID_PATIENTPAYMENT_USING_BLOCKER_VALIDATE_REVENUESHARING).ParameterValue;
            hdnIsUsingBlockerValidatePaymentReconOrUserPaymentBalance.Value = lstSetVarDt.FirstOrDefault(w => w.ParameterCode == Constant.SettingParameter.FN_IS_VOID_PATIENTPAYMENT_USING_BLOCKER_VALIDATE_COPY_PAYMENT_RECONCILIATION_OR_USER_PATIENT_PAYMENT_BALANCE).ParameterValue;

        }

        protected void cbpPatientPaymentVoid_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            if (OnVoidRecord(ref errMessage))
                result = "success";
            else
                result = "fail|" + errMessage;

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnVoidRecord(ref string errMessage)
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
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                Registration registration = registrationDao.Get(Convert.ToInt32(hdnRegistrationIDCtlVoid.Value));
                PatientPaymentHd entity = entityDao.Get(Convert.ToInt32(hdnPaymentIDCtlVoid.Value));

                if (hdnIsSetEdcBridging.Value == "1")
                {
                    if (hdnEdcTransaction.Value == "1")
                    {
                        errMessage = "Sudah ada pembayaran melalui EDC. Silahkan lakukan void pembayaran EDC terlebih dahulu di detail informasi kartu.";
                        result = false;
                    }
                }

                if (result)
                {
                    if (hdnIsBridgingToPaymentGatewayCtlVoid.Value == "1")
                    {
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        List<PatientPaymentDtVirtual> lstDtVirtual = BusinessLayer.GetPatientPaymentDtVirtualList(string.Format("PaymentID = {0} AND IsDeleted = 0", hdnPaymentIDCtlVoid.Value), ctx);
                        if (lstDtVirtual.Count > 0)
                        {
                            errMessage = "Sudah ada pembayaran melalui virtual payment.";
                            result = false;
                        }
                    }
                }

                if (result) // cek status registrasi tidak boleh closed
                {
                    if (registration.GCRegistrationStatus == Constant.VisitStatus.CLOSED)
                    {
                        errMessage = Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_CLOSED_REGISTRATION_VALIDATION);
                        result = false;
                    }
                }

                if (result) // cek status pembayaran (PatientPayment) apakah sudah diproses oleh yg lain
                {
                    if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
                    {
                        errMessage = "Pembayaran dengan nomor <b>" + entity.PaymentNo + "</b> ini sudah diproses oleh lainnya, harap refresh halaman ini.";
                        result = false;
                    }
                }

                if (result) // cek sudah ada pembuatan kwitansi (PaymentReceipt)
                {
                    if (entity.PaymentReceiptID != null)
                    {
                        errMessage = Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_VOID_PAYMENT_PAYMENTRECEIPT);
                        result = false;
                    }
                }

                if (result) // cek pembayaran uang muka ini sudah dipakai pelunasan
                {
                    if (entity.GCPaymentType == Constant.PaymentType.DOWN_PAYMENT)
                    {
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        List<PatientPaymentHd> lstPaymentSettlement = BusinessLayer.GetPatientPaymentHdList(string.Format(
                                                                            "RegistrationID = {0} AND GCPaymentType = '{1}' AND GCTransactionStatus != '{2}'",
                                                                            entity.RegistrationID, Constant.PaymentType.SETTLEMENT, Constant.TransactionStatus.VOID), ctx);
                        if (lstPaymentSettlement.Count > 0)
                        {
                            errMessage = Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_DOWN_PAYMENT_USED);
                            result = false;
                        }
                    }
                }

                if (result && hdnIsUsingBlockerValidateRevenueSharing.Value == "1") // cek sudah ada proses jasmed
                {
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    List<TransRevenueSharingDt> lstTransRevenueSharingDt = BusinessLayer.GetTransRevenueSharingDtList(string.Format("PatientChargesID IN (SELECT TransactionID FROM PatientChargesHd WITH(NOLOCK) WHERE PatientBillingID IN (SELECT PatientBillingID FROM PatientBillPayment WITH(NOLOCK) WHERE PaymentID = {0})) AND IsDeleted = 0 AND RSTransactionID IN (SELECT rth.RSTransactionID FROM TransRevenueSharingHd rth WHERE rth.GCTransactionStatus <> '{1}')", hdnPaymentIDCtlVoid.Value, Constant.TransactionStatus.VOID), ctx);
                    if (lstTransRevenueSharingDt.Count > 0)
                    {
                        errMessage = Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_CHARGES_REVENUE_SHARING_PROCESSED);
                        result = false;
                    }
                }

                if (result) // cek sudah ada proses transaksi rujukan ke pihak ketiga (TestPartnerTransaction)
                {
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    List<TestPartnerTransactionDt> lstReferrerTransDt = BusinessLayer.GetTestPartnerTransactionDtList(string.Format("PatientChargesID IN (SELECT TransactionID FROM PatientChargesHd WITH(NOLOCK) WHERE PatientBillingID IN (SELECT PatientBillingID FROM PatientBillPayment WITH(NOLOCK) WHERE PaymentID = {0})) AND IsDeleted = 0 AND TransactionID IN (SELECT rth.TransactionID FROM TestPartnerTransactionHd rth WITH(NOLOCK) WHERE rth.GCTransactionStatus <> '{1}')", hdnPaymentIDCtlVoid.Value, Constant.TransactionStatus.VOID), ctx);
                    if (lstReferrerTransDt.Count > 0)
                    {
                        errMessage = Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_CHARGES_TEST_PARTNER_TRANSACTION_PROCESSED);
                        result = false;
                    }
                }

                if (result) // cek sudah ada proses approval final klaim utk pengakuan piutang BPJS
                {
                    if (entity.GCPaymentType == Constant.PaymentType.AR_PAYER)
                    {
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        List<PatientPaymentDtInfo> lstPaymentDtInfo = BusinessLayer.GetPatientPaymentDtInfoList(string.Format("ISNULL(GCFinalStatus,'') = '{0}' AND PaymentDetailID IN (SELECT ppd.PaymentDetailID FROM PatientPaymentDt ppd WITH(NOLOCK) WHERE ppd.IsDeleted = 0 AND ppd.PaymentID = {1} AND ppd.BusinessPartnerID IN (SELECT c.BusinessPartnerID FROM Customer c WITH(NOLOCK) WHERE c.GCCustomerType = '{2}'))", Constant.FinalStatus.APPROVED, hdnPaymentIDCtlVoid.Value, Constant.CustomerType.BPJS), ctx);
                        if (lstPaymentDtInfo.Count > 0)
                        {
                            errMessage = "Pembayaran dengan nomor <b>" + entity.PaymentNo + "</b> ini sudah diproses approval finalisasi klaim BPJS, tidak dapat membatalkan pengakuan piutang ini.";
                            result = false;
                        }
                    }
                }

                if (result && hdnIsUsingBlockerValidatePaymentReconOrUserPaymentBalance.Value == "1") // cek sudah disalin setoran kasir rekonsiliasi di kas bank
                {
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    List<GLTransactionDt> lstGLTransactionDt = BusinessLayer.GetGLTransactionDtList(string.Format("GLTransactionID IN (SELECT GLTransactionID FROM GLTransactionHd WHERE GCTreasuryGroup = '{0}' AND GCTransactionStatus <> '{1}') AND ReferenceNo IN (SELECT CAST(PaymentDetailID AS VARCHAR(10)) FROM PatientPaymentDt WHERE GCItemDetailStatus <> '{1}' AND IsDeleted = 0 AND PaymentID IN (SELECT PaymentID FROM PatientPaymentHd WHERE GCTransactionStatus <> '{1}' AND PaymentID = {2})) AND GCItemDetailStatus <> '{1}'", Constant.TreasuryGroup.SETORAN_KASIR_REKONSILIASI, Constant.TransactionStatus.VOID, hdnPaymentIDCtlVoid.Value), ctx);
                    if (lstGLTransactionDt.Count > 0)
                    {
                        errMessage = "Maaf, Pembayaran ini tidak dapat dibatalkan karena telah disalin setoran kasir rekonsiliasi.";
                        result = false;
                    }
                }

                if (result) // cek sudah ada proses piutang (ARInvoice)
                {
                    if (entity.GCPaymentType == Constant.PaymentType.AR_PATIENT || entity.GCPaymentType == Constant.PaymentType.AR_PAYER)
                    {
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        List<vARInvoiceDt> lstARInvoiceDt = BusinessLayer.GetvARInvoiceDtList(string.Format("PaymentID = '{0}' AND GCTransactionStatus <> '{1}' AND ISNULL(GCTransactionDetailStatus,'') <> '{1}'", hdnPaymentIDCtlVoid.Value, Constant.TransactionStatus.VOID), ctx);
                        if (lstARInvoiceDt.Count > 0)
                        {
                            errMessage = Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_AR_ALREADY_PROCCESSED);
                            result = false;
                        }
                    }
                }

                if (result)
                {
                    entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    entity.GCVoidReason = cboVoidReason.Value.ToString();
                    if (cboVoidReason.Value.ToString() == Constant.DeleteReason.OTHER)
                    {
                        entity.VoidReason = txtReason.Text;
                    }
                    entity.VoidBy = AppSession.UserLogin.UserID;
                    entity.VoidDate = DateTime.Now;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityDao.Update(entity);

                    List<PatientBill> lstPatientBill = BusinessLayer.GetPatientBillList(string.Format("PatientBillingID IN (SELECT PatientBillingID FROM PatientBillPayment WHERE PaymentID = {0})", hdnPaymentIDCtlVoid.Value), ctx);
                    foreach (PatientBill patientBill in lstPatientBill)
                    {
                        List<PatientBillPayment> lstPatientBillPayment = BusinessLayer.GetPatientBillPaymentList(String.Format("PaymentID = {0}", hdnPaymentIDCtlVoid.Value), ctx);
                        PatientBillPayment objPatientBillPayment = lstPatientBillPayment.FirstOrDefault(x => x.PatientBillingID == patientBill.PatientBillingID && x.PaymentID == entity.PaymentID);

                        patientBill.TotalPatientPaymentAmount = patientBill.TotalPatientPaymentAmount - objPatientBillPayment.PatientPaymentAmount + objPatientBillPayment.PatientRoundingAmount;
                        patientBill.TotalPayerPaymentAmount = patientBill.TotalPayerPaymentAmount - objPatientBillPayment.PayerPaymentAmount + objPatientBillPayment.PayerRoundingAmount;

                        patientBill.PaymentID = null;
                        patientBill.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        patientBill.LastUpdatedBy = AppSession.UserLogin.UserID;
                        patientBill.LastUpdatedDate = DateTime.Now;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        patientBillDao.Update(patientBill);
                    }

                    List<PatientChargesHd> lstPatientChargesHd = BusinessLayer.GetPatientChargesHdList(string.Format("PatientBillingID IN (SELECT PatientBillingID FROM PatientBillPayment WHERE PaymentID = {0})", hdnPaymentIDCtlVoid.Value), ctx);
                    foreach (PatientChargesHd objPatientChargesHd in lstPatientChargesHd)
                    {
                        objPatientChargesHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                        objPatientChargesHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        patientChargesHdDao.Update(objPatientChargesHd);
                    }

                    ctx.CommitTransaction();

                    #region Bridging to Queue

                    if (AppSession.IsBridgingToQueue)
                    {
                        try
                        {
                            string[] listParam = hdnParam.Value.Split('|');
                            foreach (PatientBill bill in lstPatientBill)
                            {

                                Registration oRegistration = BusinessLayer.GetRegistration(AppSession.RegisteredPatient.RegistrationID);
                                APIMessageLog entityAPILog = new APIMessageLog()
                                {
                                    MessageDateTime = DateTime.Now,
                                    Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                                    Sender = Constant.BridgingVendor.HIS,
                                    IsSuccess = true
                                };

                                QueueService oService = new QueueService();
                                string apiResult = oService.BAR_P05(AppSession.UserLogin.HealthcareID, oRegistration, "05", bill.PatientBillingID);
                                string[] apiResultInfo = apiResult.Split('|');
                                if (apiResultInfo[0] == "0")
                                {
                                    entityAPILog.IsSuccess = false;
                                    entityAPILog.MessageText = apiResultInfo[2];
                                    entityAPILog.Response = apiResult;
                                    entityAPILog.ErrorMessage = apiResultInfo[1];
                                    BusinessLayer.InsertAPIMessageLog(entityAPILog);

                                    Exception ex = new Exception(apiResultInfo[1]);
                                    Helper.InsertErrorLog(ex);
                                }
                                else
                                {
                                    entityAPILog.MessageText = apiResultInfo[2];
                                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            Helper.InsertErrorLog(ex);
                        }
                    }

                    #endregion
                }
                else
                {
                    result = false;
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
    }
}
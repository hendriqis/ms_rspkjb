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
    public partial class ProcessTransferDownPayment : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inpatient.PROCESS_TRANSFER_DOWN_PAYMENT;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }
        
        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            vConsultVisit9 entityVisit = BusinessLayer.GetvConsultVisit9List(string.Format("RegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID)).FirstOrDefault();
            hdnRegistrationID.Value = entityVisit.RegistrationID.ToString();
            hdnDepartmentID.Value = entityVisit.DepartmentID.ToString();
            
            vConsultVisit9 entityVisitLinked = BusinessLayer.GetvConsultVisit9List(string.Format("RegistrationID = {0}", entityVisit.LinkedRegistrationID)).FirstOrDefault();
            hdnLinkedRegistrationID.Value = entityVisitLinked.RegistrationID.ToString();
            hdnLinkedDepartmentID.Value = entityVisitLinked.DepartmentID.ToString();

            BindGridDetail();
        }

        private void BindGridDetail()
        {
            string filterExpression = string.Format("RegistrationID = {0} AND GCTransactionStatus != '{1}' AND GCPaymentType = '{2}' AND IsTransfered = 0 AND SourcePaymentID IS NULL", hdnLinkedRegistrationID.Value, Constant.TransactionStatus.VOID, Constant.PaymentType.DOWN_PAYMENT);
            List<vPatientPaymentHd> lst = BusinessLayer.GetvPatientPaymentHdList(filterExpression);
            lvwView.DataSource = lst;
            lvwView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";

            if (param[0] == "transfer")
            {
                if (TransferDownPayment(param, ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else
            {
                BindGridDetail();
            }


            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool TransferDownPayment(string[] param, ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientPaymentHdDao paymentHdDao = new PatientPaymentHdDao(ctx);
            PatientPaymentDtDao paymentDtDao = new PatientPaymentDtDao(ctx);

            try
            {
                PatientPaymentHd paymentHd = paymentHdDao.Get(Convert.ToInt32(param[1]));
                if (!paymentHd.IsTransfered)
                {
                    #region Update PaymentHD
                    paymentHd.IsTransfered = true;
                    paymentHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    paymentHd.LastUpdatedDate = DateTime.Now;

                    paymentHdDao.Update(paymentHd);
                    #endregion

                    string transactionCode = "";

                    string filterPaymentDt = string.Format("PaymentID = {0} AND IsDeleted = 0", paymentHd.PaymentID);
                    List<PatientPaymentDt> lstPaymentDt = BusinessLayer.GetPatientPaymentDtList(filterPaymentDt, ctx);

                    #region Source Registration

                    #region Insert NEW PaymentHD

                    PatientPaymentHd newHdSource = new PatientPaymentHd();

                    newHdSource.RegistrationID = paymentHd.RegistrationID;
                    newHdSource.PaymentDate = Helper.GetDatePickerValue(DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT));
                    newHdSource.PaymentTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                    newHdSource.GCPaymentType = paymentHd.GCPaymentType;
                    newHdSource.GCCashierGroup = paymentHd.GCCashierGroup;
                    newHdSource.GCShift = paymentHd.GCShift;
                    newHdSource.TotalPaymentAmount = (paymentHd.TotalPaymentAmount * -1);
                    newHdSource.TotalFeeAmount = 0;
                    newHdSource.TotalPatientBillAmount = 0;
                    newHdSource.TotalPayerBillAmount = 0;
                    newHdSource.Remarks = paymentHd.Remarks;
                    newHdSource.GCTransactionStatus = Constant.TransactionStatus.OPEN;

                    switch (hdnDepartmentID.Value)
                    {
                        case Constant.Facility.INPATIENT:
                            switch (newHdSource.GCPaymentType)
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
                            switch (newHdSource.GCPaymentType)
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
                            switch (newHdSource.GCPaymentType)
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
                            switch (newHdSource.GCPaymentType)
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
                                switch (newHdSource.GCPaymentType)
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
                                switch (newHdSource.GCPaymentType)
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
                            else
                            {
                                switch (newHdSource.GCPaymentType)
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
                            switch (newHdSource.GCPaymentType)
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
                    newHdSource.PaymentNo = BusinessLayer.GenerateTransactionNo(transactionCode, newHdSource.PaymentDate, ctx);

                    newHdSource.SourcePaymentID = paymentHd.PaymentID;
                    newHdSource.CreatedBy = AppSession.UserLogin.UserID;
                    
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    newHdSource.PaymentID = paymentHdDao.InsertReturnPrimaryKeyID(newHdSource);

                    #endregion
               
                    #region Insert NEW PaymentDT

                    foreach(PatientPaymentDt paymentDt in lstPaymentDt)
                    {
                        PatientPaymentDt newDtSource = new PatientPaymentDt();

                        newDtSource.PaymentID = newHdSource.PaymentID;
                        newDtSource.GCPaymentMethod = Constant.PaymentMethod.TRANSFER_TRANSACTION;
                        newDtSource.EDCMachineID = paymentDt.EDCMachineID;
                        newDtSource.GCCardType = paymentDt.GCCardType;
                        newDtSource.GCCardProvider = paymentDt.GCCardProvider;
                        newDtSource.CardNumber = paymentDt.CardNumber;
                        newDtSource.CardHolderName = paymentDt.CardHolderName;
                        newDtSource.CardValidThru = paymentDt.CardValidThru;
                        newDtSource.BankID = paymentDt.BankID;
                        newDtSource.ReferenceNo = paymentDt.ReferenceNo;
                        newDtSource.BatchNo = paymentDt.BatchNo;
                        newDtSource.TraceNo = paymentDt.TraceNo;
                        newDtSource.ApprovalCode = paymentDt.ApprovalCode;
                        newDtSource.TerminalID = paymentDt.TerminalID;
                        newDtSource.PaymentAmount = (paymentDt.PaymentAmount * -1);
                        newDtSource.CardFeeAmount = paymentDt.CardFeeAmount;
                        newDtSource.BusinessPartnerID = paymentDt.BusinessPartnerID;
                        newDtSource.IsDeleted = false;
                        newDtSource.CreatedBy = AppSession.UserLogin.UserID;

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        paymentDtDao.Insert(newDtSource);
                    }
                    
                    #endregion

                    #endregion

                    #region Real Registration

                    #region Insert NEW PaymentHD

                    PatientPaymentHd newHd = new PatientPaymentHd();

                    newHd.RegistrationID = AppSession.RegisteredPatient.RegistrationID;
                    newHd.PaymentDate = Helper.GetDatePickerValue(DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT));
                    newHd.PaymentTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                    newHd.GCPaymentType = paymentHd.GCPaymentType;
                    newHd.GCCashierGroup = paymentHd.GCCashierGroup;
                    newHd.GCShift = paymentHd.GCShift;
                    newHd.TotalPaymentAmount = paymentHd.TotalPaymentAmount;
                    newHd.TotalFeeAmount = 0;
                    newHd.TotalPatientBillAmount = 0;
                    newHd.TotalPayerBillAmount = 0;
                    newHd.Remarks = paymentHd.Remarks;
                    newHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;

                    switch (hdnDepartmentID.Value)
                    {
                        case Constant.Facility.INPATIENT:
                            switch (newHd.GCPaymentType)
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
                            switch (newHd.GCPaymentType)
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
                            switch (newHd.GCPaymentType)
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
                            switch (newHd.GCPaymentType)
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
                                switch (newHd.GCPaymentType)
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
                                switch (newHd.GCPaymentType)
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
                            else
                            {
                                switch (newHd.GCPaymentType)
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
                            switch (newHd.GCPaymentType)
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
                    newHd.PaymentNo = BusinessLayer.GenerateTransactionNo(transactionCode, newHd.PaymentDate, ctx);

                    newHd.SourcePaymentID = paymentHd.PaymentID;
                    newHd.CreatedBy = AppSession.UserLogin.UserID;

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    newHd.PaymentID = paymentHdDao.InsertReturnPrimaryKeyID(newHd);

                    #endregion

                    #region Insert NEW PaymentDT

                    foreach (PatientPaymentDt paymentDt in lstPaymentDt)
                    {
                        PatientPaymentDt newDt = new PatientPaymentDt();

                        newDt.PaymentID = newHd.PaymentID;
                        newDt.GCPaymentMethod = Constant.PaymentMethod.TRANSFER_TRANSACTION;
                        newDt.EDCMachineID = paymentDt.EDCMachineID;
                        newDt.GCCardType = paymentDt.GCCardType;
                        newDt.GCCardProvider = paymentDt.GCCardProvider;
                        newDt.CardNumber = paymentDt.CardNumber;
                        newDt.CardHolderName = paymentDt.CardHolderName;
                        newDt.CardValidThru = paymentDt.CardValidThru;
                        newDt.BankID = paymentDt.BankID;
                        newDt.ReferenceNo = paymentDt.ReferenceNo;
                        newDt.BatchNo = paymentDt.BatchNo;
                        newDt.TraceNo = paymentDt.TraceNo;
                        newDt.ApprovalCode = paymentDt.ApprovalCode;
                        newDt.TerminalID = paymentDt.TerminalID;
                        newDt.PaymentAmount = paymentDt.PaymentAmount;
                        newDt.CardFeeAmount = paymentDt.CardFeeAmount;
                        newDt.BusinessPartnerID = paymentDt.BusinessPartnerID;
                        newDt.IsDeleted = false;
                        newDt.CreatedBy = AppSession.UserLogin.UserID;

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();

                        paymentDtDao.Insert(newDt);
                    }

                    #endregion

                    #endregion

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Pembayaran dengan nomor " + paymentHd.PaymentNo + " sudah ditransfer sebelumnya.";
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
            return result;
        }

    }
}
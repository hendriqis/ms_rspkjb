using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using QIS.Medinfras.Web.Common.API.Model;
using QIS.Medinfras.Web.CommonLibs.Service;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class SendBARNotificationCtl : BaseProcessPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            ddlNotificationType.SelectedValue = paramInfo[0];
            hdnMessageType.Value = paramInfo[0];
            hdnRegistrationID.Value = paramInfo[1];
            hdnTransactionID.Value = paramInfo[2];

            string filterExpression = "1=0";
            switch (hdnMessageType.Value)
            {
                case "00": //Data Kunjungan
                    filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);
                    vRegistration entity1 = BusinessLayer.GetvRegistrationList(filterExpression).FirstOrDefault();            
                    EntityToControl(entity1);
                    break;
                case "01": //Data Transaction
                    filterExpression = string.Format("TransactionID = {0}", hdnTransactionID.Value);
                    vPatientChargesHd entity2 = BusinessLayer.GetvPatientChargesHdList(filterExpression).FirstOrDefault();            
                    EntityToControl(entity2);
                    break;
                case "02": //Billing
                    filterExpression = string.Format("PatientBillingID = {0}", hdnTransactionID.Value);
                    vPatientBill entity3 = BusinessLayer.GetvPatientBillList(filterExpression).FirstOrDefault();
                    EntityToControl(entity3);
                    break;
                case "03": //Payment
                    //filterExpression = string.Format("PaymentID = {0}", hdnTransactionID.Value);
                    //vPatientPaymentHd entity4 = BusinessLayer.GetvPatientPaymentHdList(filterExpression).FirstOrDefault();
                    //EntityToControl(entity4);

                    filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);
                    vRegistration entity4 = BusinessLayer.GetvRegistrationList(filterExpression).FirstOrDefault();            
                    EntityToControl(entity4);
                    break;
                default:
                    break;
            }

        }

        private void EntityToControl(vRegistration entity)
        {
            if (entity != null)
            {
                txtRegistrationNo.Text = entity.RegistrationNo;
                txtTransactionNo.Text = entity.RegistrationNo;
                txtTransactionDate.Text = entity.RegistrationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtMedicalNo.Text = entity.MedicalNo;
                txtPatientName.Text = entity.PatientName;
            }
        }

        private void EntityToControl(vPatientChargesHd entity)
        {
            if (entity != null)
            {
                txtRegistrationNo.Text = entity.RegistrationNo;
                txtTransactionNo.Text = entity.TransactionNo;
                txtTransactionDate.Text = entity.TransactionDateInString;
                txtMedicalNo.Text = entity.MedicalNo;
                txtPatientName.Text = entity.PatientName;
                hdnTestOrderID.Value = entity.TestOrderID.ToString();
            }
        }

        private void EntityToControl(vPatientBill entity)
        {
            if (entity != null)
            {
                if (AppSession.RegisteredPatient != null)
                {
                    txtRegistrationNo.Text = AppSession.RegisteredPatient.RegistrationNo; 
                }
                txtTransactionNo.Text = entity.PatientBillingNo;
                txtTransactionDate.Text = entity.BillingDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) ;
                txtMedicalNo.Text = AppSession.RegisteredPatient.MedicalNo;
                txtPatientName.Text = AppSession.RegisteredPatient.PatientName;
            }
        }

        private void EntityToControl(vPatientPaymentHd entity)
        {
            if (entity != null)
            {
                if (AppSession.RegisteredPatient != null)
                {
                    txtRegistrationNo.Text = AppSession.RegisteredPatient.RegistrationNo;
                }
                txtTransactionNo.Text = entity.PaymentNo;
                txtTransactionDate.Text = entity.PaymentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtMedicalNo.Text = AppSession.RegisteredPatient.MedicalNo;
                txtPatientName.Text = AppSession.RegisteredPatient.PatientName;
            }
        }

        public override void SetProcessButtonVisibility(ref bool IsUsingProcessButton)
        {
            IsUsingProcessButton = true;
        }

        protected override bool OnProcessRecord(ref string errMessage, ref string retval)
        {
            bool result = true;

            try
            {
                string messageType = hdnMessageType.Value;
                int registrationID = Convert.ToInt32(hdnRegistrationID.Value);
                int transactionID = Convert.ToInt32(hdnTransactionID.Value);
                string referenceNo = string.Empty;
                bool isError = false;

                var processResult = SendNotification(messageType,registrationID, transactionID);
                string[] resultInfo = processResult.Split('|');

                isError = resultInfo[0] == "0";
                if (isError)
                {
                    errMessage = resultInfo[1];
                    result = false;
                }
                else
                {
                    retval = resultInfo[1];
                }

                return result;
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }

        #region Notification Services
        public string SendNotification(string messageType, int registrationID, int transactionID)
        {
            string result = "";
            try
            {
                //VisitInfo visitInfo = new VisitInfo();
                //visitInfo = ConvertVisitToDTO(entityVisit);
                //PatientData patientInfo = ConvertPatientToDTO(entityPatient);
                APIMessageLog entityAPILog = new APIMessageLog()
                {
                    MessageDateTime = DateTime.Now,
                    Recipient = Constant.BridgingVendor.QUEUE,
                    Sender = Constant.BridgingVendor.HIS,
                    IsSuccess = true
                };

                Registration entity = BusinessLayer.GetRegistration(Convert.ToInt32(hdnRegistrationID.Value));

                QueueService oService = new QueueService();
                string apiResult = oService.BAR_P05(AppSession.UserLogin.HealthcareID, entity,hdnMessageType.Value, Convert.ToInt32(hdnTransactionID.Value));
                string[] apiResultInfo = apiResult.Split('|');
                if (apiResultInfo[0] == "0")
                {
                    entityAPILog.IsSuccess = false;
                    entityAPILog.MessageText = apiResultInfo[1];
                    entityAPILog.Response = apiResultInfo[1];
                    Exception ex = new Exception(apiResultInfo[1]);
                    Helper.InsertErrorLog(ex);

                    result = string.Format("{0}|{1}", "0", apiResultInfo[1]);
                }
                else
                {
                    entityAPILog.MessageText = apiResultInfo[1];

                    result = string.Format("{0}|{1}", "1", txtTransactionNo.Text);
                }

                BusinessLayer.InsertAPIMessageLog(entityAPILog);
            }
            catch (Exception ex)
            {
                result = string.Format("{0}|{1}", "0", ex.Message);
                Helper.InsertErrorLog(ex);
            }

            return result;
        }
        #endregion
    }
}
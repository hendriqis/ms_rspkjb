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
    public partial class SendVitalSignDataCtl : BaseProcessPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            ddlNotificationType.SelectedValue = paramInfo[0];
            hdnMessageType.Value = paramInfo[0];
            hdnRegistrationID.Value = paramInfo[1];
            //hdnTransactionID.Value = paramInfo[2];

            string filterExpression = "1=0";
            switch (hdnMessageType.Value)
            {
                case "ADT^A01": //Data Kunjungan
                    filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);
                    vRegistration entity1 = BusinessLayer.GetvRegistrationList(filterExpression).FirstOrDefault();            
                    EntityToControl(entity1);
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
                string referenceNo = string.Empty;
                bool isError = false;

                var processResult = SendNotification(messageType,registrationID);
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
        public string SendNotification(string messageType, int registrationID)
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
                    Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                    Sender = Constant.BridgingVendor.HIS,
                    IsSuccess = true
                };

                Registration entity = BusinessLayer.GetRegistration(Convert.ToInt32(hdnRegistrationID.Value));

                /*reguest */
                SendVitalSignData JsonRequest = new SendVitalSignData()
                {
                    RegistrationNo = entity.RegistrationNo,
                    HL7MessageType = hdnMessageType.Value
                };

                string jsonRequest = JsonConvert.SerializeObject(JsonRequest);
                entityAPILog.MessageText = jsonRequest;

                HL7Service oService = new HL7Service();
                string apiResult = oService.SendVitalSignMessage(jsonRequest);
                ApiResponse oresult = JsonConvert.DeserializeObject<ApiResponse>(apiResult);

                if (oresult.Status == "SUCCESS")
                {
                    string[] apiResultInfo = oresult.Data.Split('|');
                    if (apiResultInfo[0] == "1")
                    {
                        entityAPILog.IsSuccess = true;
                        entityAPILog.Response = apiResult;
                        
                        result = string.Format("{0}|{1}", "1", apiResultInfo[1]);
                    }
                    else
                    {
                        entityAPILog.IsSuccess = false;
                        entityAPILog.Response = apiResult;
                        result = string.Format("{0}|{1}", "0", oresult.Remarks);
                    }

                }
                else {
                    entityAPILog.IsSuccess = false;
                    result = string.Format("{0}|{1}", "0", oresult.Remarks);
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
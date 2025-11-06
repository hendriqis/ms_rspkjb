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
using Newtonsoft.Json;
using QIS.Medinfras.Web.CommonLibs.Service;
using QIS.Medinfras.Common;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class SendResultToRISCtl : BaseProcessPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnTransactionID.Value = param;
            vPatientChargesHd1 oHeader = BusinessLayer.GetvPatientChargesHd1List(string.Format("TransactionID = {0}", hdnTransactionID.Value)).FirstOrDefault();
            if (oHeader != null)
            {
                txtTransactionNo.Text = oHeader.TransactionNo;
                BindGridView();
            }
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("TransactionID = {0}", hdnTransactionID.Value);
            List<vPatientChargesDt> lstDetail = BusinessLayer.GetvPatientChargesDtList(filterExpression);
            grdView.DataSource = lstDetail;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string[] param = e.Parameter.Split('|');
            string result = param[0] + "|";
            BindGridView();
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
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
                int transactionID = Convert.ToInt32(hdnTransactionID.Value);
                string referenceNo = string.Empty;
                bool isError = false;

                if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                {
                    if (AppSession.IsBridgingToRIS)
                    {
                        string[] resultInfo = "0|Unknown Protocol".Split('|');
                        switch (AppSession.RIS_BRIDGING_PROTOCOL)
                        {
                            case Constant.RIS_Bridging_Protocol.WEB_API:
                                var result1 = SendToMedinfrasAPI(transactionID, hdnSelectedID.Value.ToString());
                                resultInfo = ((string)result1).Split('|');
                                break;
                            default:
                                resultInfo = "0|Unknown Protocol".Split('|');
                                break;
                        }
                        referenceNo = resultInfo[1];
                        isError = resultInfo[0] == "0";
                        if (isError)
                        {
                            errMessage = resultInfo[1];
                            result = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
            }
            return result;
        }

        #region Sending Order Process

        private object SendToMedinfrasAPI(int transactionID, string lstItemID)
        {
            string result = "1|";

            try
            {
                APIMessageLog entityAPILog = new APIMessageLog()
                {
                    MessageDateTime = DateTime.Now,
                    Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                    Sender = Constant.BridgingVendor.HIS,
                    IsSuccess = true
                };
                RISService oService = new RISService();
                //string apiResult = oService.ADT_A01(AppSession.UserLogin.HealthcareID, entity, visitInfo, patientInfo);
                string apiResult = oService.SendResultToRIS_MedinfrasApi(transactionID);
                string[] apiResultInfo = apiResult.Split('|');
                if (apiResultInfo[0] == "0")
                {
                    entityAPILog.IsSuccess = false;
                    entityAPILog.MessageText = apiResultInfo[1];
                    entityAPILog.Response = apiResult;
                    entityAPILog.ErrorMessage = apiResultInfo[1];
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);

                    result = string.Format("{0}|{1}", "0", apiResultInfo[1]);

                    Exception ex = new Exception(apiResultInfo[1]);
                    Helper.InsertErrorLog(ex);
                }
                else
                {
                    entityAPILog.MessageText = apiResultInfo[1];
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                }
            }
            catch (Exception ex)
            {
                Helper.InsertErrorLog(ex);
                result = string.Format("{0}|{1}", "0", ex.Message);
            }

            return result;
        }

        private void UpdateOrderStatus(string sender, string messageType, string messageCode, string messageText, bool isSuccess, string errorMessage, vPatientChargesDt item, string messageControlID = "", string deviceNo = "")
        {
            HL7Message log = new HL7Message();
            log.MessageDateTime = DateTime.Now;
            log.MessageControlID = Convert.ToString(item.ID);
            log.Sender = sender;
            log.DeviceNo = deviceNo;
            log.RegistrationID = item.RegistrationID;
            log.RegistrationNo = item.RegistrationNo;
            log.TransactionID = item.TransactionID;
            log.DetailTransactionID = item.ID;
            log.PatientName = item.PatientName;
            log.MessageType = messageType;
            log.MessageCode = messageCode;
            log.MessageText = messageText;
            log.MessageStatus = isSuccess ? "OK" : "ERR";
            log.ErrorMessage = errorMessage;
            BusinessLayer.InsertHL7Message(log);

            if (isSuccess)
            {
                //If Success, update order status to SENT
                PatientChargesDtInfo dtInfo = BusinessLayer.GetPatientChargesDtInfo(item.ID);
                if (dtInfo != null)
                {
                    if (sender != Constant.HL7_Partner.MEDAVIS && sender != Constant.HL7_Partner.FUJIFILM && sender != Constant.HL7_Partner.ZED)
                    {
                        //Accession number is generated by HIS
                        dtInfo.ReferenceNo = messageControlID;

                        if (sender == Constant.HL7_Partner.INFINITT || sender == Constant.HL7_Partner.MEDSYNAPTIC)
                        {
                            dtInfo.ReferenceNo = item.ID.ToString();
                        }
                    }
                    dtInfo.GCRISBridgingStatus = Constant.RIS_Bridging_Status.SENT;
                    BusinessLayer.UpdatePatientChargesDtInfo(dtInfo);
                }
            }
        }

        private void UpdateErrorLogMessage(string sender, string messageType, string messageCode, string messageText, string errorMessage, vPatientChargesDt item, string messageControlID = "", string deviceNo = "")
        {
            HL7Message log = new HL7Message();
            log.MessageDateTime = DateTime.Now;
            log.MessageControlID = Convert.ToString(item.ID);
            log.Sender = sender;
            log.DeviceNo = deviceNo;
            log.RegistrationID = item.RegistrationID;
            log.RegistrationNo = item.RegistrationNo;
            log.TransactionID = item.TransactionID;
            log.DetailTransactionID = item.ID;
            log.PatientName = item.PatientName;
            log.MessageType = messageType;
            log.MessageCode = messageCode;
            log.MessageText = messageText;
            log.MessageStatus = "ERR";
            log.ErrorMessage = errorMessage;
            BusinessLayer.InsertHL7Message(log);
        }
        #endregion
    }
}
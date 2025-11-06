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
    public partial class SendInventoryNotificationCtl : BaseProcessPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            ddlNotificationType.SelectedValue = paramInfo[0];
            hdnMessageType.Value = paramInfo[0];
            hdnTransactionID.Value = paramInfo[1];

            string filterExpression = "1=0";
            switch (hdnMessageType.Value)
            {
                case "00": //Purchase Request
                    filterExpression = string.Format("PurchaseRequestID = {0}", hdnTransactionID.Value);
                    PurchaseRequestHd entity = BusinessLayer.GetPurchaseRequestHdList(filterExpression).FirstOrDefault();
                    EntityToControl(entity);
                    break;
                default:
                    break;
            }

        }

        private void EntityToControl(PurchaseRequestHd entity)
        {
            if (entity != null)
            {
                txtTransactionNo.Text = entity.PurchaseRequestNo;
                txtTransactionDate.Text = entity.TransactionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
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
                int transactionID = Convert.ToInt32(hdnTransactionID.Value);
                string referenceNo = string.Empty;
                bool isError = false;

                var processResult = SendNotification(messageType, transactionID);
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
        public string SendNotification(string messageType, int transactionID)
        {
            string result = "";
            try
            {
                APIMessageLog entityAPILog = new APIMessageLog()
                {
                    MessageDateTime = DateTime.Now,
                    Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                    Sender = Constant.BridgingVendor.HIS,
                    IsSuccess = true
                };

                InventoryService oService = new InventoryService();
                string apiResult = oService.SendInventoryNotification(AppSession.UserLogin.HealthcareID, hdnMessageType.Value, Convert.ToInt32(hdnTransactionID.Value), txtTransactionNo.Text);
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
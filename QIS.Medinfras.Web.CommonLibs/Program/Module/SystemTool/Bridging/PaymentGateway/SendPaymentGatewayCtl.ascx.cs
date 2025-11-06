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
using DevExpress.Web.ASPxCallbackPanel;
using ThoughtWorks.QRCode.Codec;
using System.Drawing;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class SendPaymentGatewayCtl : BaseViewPopupCtl
    {
         protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            // var id = billingID + "|" + billingNo + "|" + billingTotal + "|" + paymentMethod + "|" + bankID +regid;
            //682419|OPB/20210917/00002|480,000.00|X035^004|1|2|3

            //bankcode
            //paymentmethod

            txtBillingNo.Text = paramInfo[1];
            txtTotalPayment.Text = paramInfo[2];

            List<vBankChannelDt> lstBankChannelDt = BusinessLayer.GetvBankChannelDtList(string.Format("BankID = {0} AND IsDeleted = 0", paramInfo[4]));
            if (lstBankChannelDt.Count > 0)
            {
                Methods.SetComboBoxField<vBankChannelDt>(cboProviderMethod, lstBankChannelDt, "ProviderMethod", "GCProviderMethod");
            }
            else
            {
                List<StandardCode> lstProvider = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PAYMENT_GATEWAY_METHOD));
                Methods.SetComboBoxField<StandardCode>(cboProviderMethod, lstProvider, "StandardCodeName", "StandardCodeID");
            }


            string[] BillingNo = paramInfo[1].Split(',');
            string strBillingNo = string.Empty;
            if (BillingNo.Length > 0)
            {
                for (int i = 0; i < BillingNo.Length; i++)
                {
                    strBillingNo += string.Format("'{0}',", BillingNo[i]);
                }

                if (!string.IsNullOrEmpty(strBillingNo)) {
                   strBillingNo = strBillingNo.Remove(strBillingNo.Length - 1).Replace(" ","");
                   hdnstrBillingNo.Value = strBillingNo;
                }
               
                List<PatientBill> lstPatientBill = BusinessLayer.GetPatientBillList(string.Format("PatientBillingNo IN ({0}) ", strBillingNo));
                if (lstPatientBill.Count > 0) {
                    string BillingID = string.Empty;
                    foreach (PatientBill row in lstPatientBill) {
                        BillingID += string.Format("{0}-", row.PatientBillingID);
                    }

                    if (!string.IsNullOrEmpty(BillingID)) {
                        hdnBillingID.Value = BillingID.Remove(BillingID.Length - 1);
                    }
                   
                }
            }

            hdnPaymentMethod.Value = paramInfo[3];
            StandardCode oStandardCode = BusinessLayer.GetStandardCodeList(string.Format("StandardCodeID IN('{0}')", paramInfo[3])).FirstOrDefault();
            
            Bank oBank = BusinessLayer.GetBankList(string.Format("BankID='{0}'", paramInfo[4])).FirstOrDefault();
            hdnBankCode.Value = oBank.BankCode;
            txtBankName.Text = oBank.BankName;
            txtMethodPayment.Text = oStandardCode.StandardCodeName;

           
           
            vRegistration oReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID='{0}'", paramInfo[5])).FirstOrDefault();
            if (oReg != null) {
                hdnPatientName.Value = oReg.PatientName;
                hdnEmailAddress.Value = oReg.EmailAddress;
            }
            hdnGCChasier.Value = paramInfo[6];
            hdnGCShift.Value = paramInfo[7];

           divResponse.Style.Add("display", "none");

           txtBankName.Attributes.Add("readonly", "readonly");
           txtBillingNo.Attributes.Add("readonly", "readonly");
           txtMethodPayment.Attributes.Add("readonly", "readonly");
           txtTotalPayment.Attributes.Add("readonly", "readonly");
           if (!string.IsNullOrEmpty(oBank.GCVirtualPaymentChannel))
           {
               StandardCode paymentChannel = BusinessLayer.GetStandardCodeList(string.Format("StandardCodeID = '{0}'", oBank.GCVirtualPaymentChannel)).FirstOrDefault();
               hdnPaymentChannel.Value = paymentChannel.TagProperty;
               hdnPaymentChannnelID.Value = paymentChannel.StandardCodeID;
               if (hdnPaymentChannnelID.Value == Constant.StandardCode.VIRTUAL_PAYMENT.OVO)
               {
                   Patient oPatient = BusinessLayer.GetPatient(AppSession.RegisteredPatient.MRN);
                   string phone = string.Empty;
                   if (oPatient != null)
                   {
                       if (!string.IsNullOrEmpty(oPatient.MobilePhoneNo1))
                       {
                           txtPhoneNo.Text = oPatient.MobilePhoneNo1;
                       }
                       else if (!string.IsNullOrEmpty(oPatient.MobilePhoneNo2))
                       {
                           txtPhoneNo.Text = oPatient.MobilePhoneNo2;
                       }
                   }
                   trphone.Style.Remove("display");
               }
               else
               {
                   trphone.Style.Add("display", "none");
               }
           }

           List<vPatientPaymentDtVirtual> lstPatientPaymentDtVirtual = BusinessLayer.GetvPatientPaymentDtVirtualList(string.Format("PatientBillingNo IN ({0}) AND IsDeleted=0 ", hdnstrBillingNo.Value));
           if (lstPatientPaymentDtVirtual.Count > 0) {
               int lstPPdtv = lstPatientPaymentDtVirtual.GroupBy(p => p.ReferenceNo).Count();
               if (lstPPdtv > 0)
               {

                   string BillingExisting = string.Empty;
                   foreach (vPatientPaymentDtVirtual row in lstPatientPaymentDtVirtual) {
                       BillingExisting += string.Format("{0} ", row.PatientBillingNo);
                   }
                   
                   trWarning.Style.Remove("display");
                   lblWarning.InnerHtml = string.Format("Nomor tagihan ({0}) tersebut sudah di proses dan menunggu proses pembayaran.", BillingExisting);
                   btnSend.Style.Add("display", "none");
                   btnGetStatusPayment.Style.Add("display", "none");
                   btnGetStatusPayment.Style.Remove("display");
               }
               else
               {
                   trWarning.Style.Add("display", "none");
                   lblWarning.InnerHtml = "";
                   btnSend.Style.Add("display", "none");
                   btnSend.Style.Add("display", "none");
                   trphone.Style.Add("display", "none");
               }
           }
          
        }

        protected void cbpPaymentGatewayView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
           
            string result = "";
            string errMessage = "";
            int IsSend = 0;
            //IsSend = 0 => false, IsSend = 1 true, IsSend 3 => expired
            string urlPage = "";
            if (e.Parameter == "process")
            {
                result = "process|";
                if (OnSaveAddRecord(ref errMessage, ref IsSend, ref urlPage))
                {
                    result += "success|" + errMessage + "|" + IsSend;
                }
                else
                {
                    result += "fail|" + errMessage + "|" + IsSend;
                }

            }
            else if(e.Parameter == "getstatus") {
                result = "getstatus|";
                if (onCheckPayment(ref errMessage, ref IsSend))
                {
                    result += "success|" + errMessage + "|" + IsSend;
                }
                else
                {
                    result += "fail|" + errMessage + "|" + IsSend;
                }
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpCheckPaymentGatewayView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int IsSend = 0;
            //IsSend = 0 => false, IsSend = 1 true, IsSend 3 => expired
            string urlPage = "";
            if (e.Parameter == "getstatus")
            {
                result = "getstatus|";
                if (onCheckPayment(ref errMessage, ref IsSend))
                {
                    result += "success|" + errMessage + "|" + IsSend;
                }
                else
                {
                    result += "fail|" + errMessage + "|" + IsSend;
                }
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
         }
        //protected void cbpPaymentVirtualHistory_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        //{
          
        //    string result = "";
        //    string errMessage = "";
        //    int IsSend = 0;
        //    string urlPage = "";
        //    int pageCount = 1;
        //    //if (e.Parameter == "refresh")
        //    //{
        //    //    BindGridPaymentVirtualHistoryView(int pageIndex, bool isCountPageCount, ref int pageCount);
        //    //}
        //    BindGridPaymentVirtualHistoryView(1, true,ref pageCount);
        //    ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
        //    panel.JSProperties["cpResult"] = result;
        //}
        //private void BindGridPaymentVirtualHistoryView(int pageIndex, bool isCountPageCount, ref int pageCount)
        //{
        //    string filterExpression = string.Format("PatientBillingNo IN ({0}) AND IsDeleted=0 ", hdnstrBillingNo.Value);
        //    List<vPatientPaymentDtVirtual> lstEntity = BusinessLayer.GetvPatientPaymentDtVirtualList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID");
        //    gridPaymentVirtualView.DataSource = lstEntity;
        //    gridPaymentVirtualView.DataBind();
              
        //}
      
        private bool OnSaveAddRecord(ref string errMessage, ref int IsSend, ref String urlPage)
        {
             bool result = true;

             try
             {
                 string resultReponse = "";
               
                 #region Data Request 
                 String invoiceNo = txtBillingNo.Text; ///.Replace("/", "-").Replace(" ", "");
                 DokuHISVa datadt = new DokuHISVa()
                 {
                     HealthcareID = AppSession.UserLogin.HealthcareID,
                     BillingAmount = Convert.ToDecimal(txtTotalPayment.Text),
                     BillingNo = hdnBillingID.Value, //invoiceNo
                     PatientEmail = hdnEmailAddress.Value,
                     PatientName = hdnPatientName.Value

                 };

                 if (hdnPaymentChannnelID.Value == Constant.StandardCode.VIRTUAL_PAYMENT.OVO)
                 {
                     datadt.PatientPhoneNo = txtPhoneNo.Text;
                 }

                 string DataDtRequest = JsonConvert.SerializeObject(datadt);
                 string GCChasier = hdnGCChasier.Value.Split('^')[1];
                 string GCShift = hdnGCShift.Value.Split('^')[1];
                 DokuHISRequestVa dataRequest = new DokuHISRequestVa()
                 {
                     Channel = hdnPaymentChannel.Value,
                     CashierGroup = GCChasier,
                     CashierShift = GCShift,
                     PaymentMethod = hdnPaymentMethod.Value,
                     ProviderMethod = cboProviderMethod.Value.ToString(),
                     BankCode = hdnBankCode.Value,
                     JsonRequest = DataDtRequest,
                     IsCancel = false
                 };
                 #endregion
 
                 PaymentGatewayService oService = new PaymentGatewayService();
                 string apiResult = oService.DokuBcaVa(dataRequest);
                 resultReponse = apiResult;
                 string[] data = resultReponse.Split('|');
                 if (data[0] == "1")
                 {
                     if (data[1] == "SUCCESS")
                     {
                         DokuVaHisResponse respData = JsonConvert.DeserializeObject<DokuVaHisResponse>(data[3]);
                         // success
                         divResponse.Style.Remove("display");
                         hdnUrl.Value = respData.BillingPage;
                         urlPage = respData.BillingPage;
                         link.HRef = respData.BillingPage;
                         link.InnerText = respData.BillingPage;

                         qrcodeURL.ImageUrl = string.Format("data:image/png;base64,{0}", respData.Billing_QRHash);
                         if (!string.IsNullOrEmpty(respData.QRIS_Logo))
                         {
                             QRIS_Logo.Style.Remove("display");
                             QRIS_Logo.ImageUrl = string.Format("data:image/png;base64,{0}", respData.QRIS_Logo);
                         }
                          
                         if (!string.IsNullOrEmpty(respData.QRIS_NMID))
                         {
                             QRIS_NMID.Style.Remove("display");
                             QRIS_NMID.InnerText = string.Format("NMID : {0}", respData.QRIS_NMID);
                         }
                         errMessage = "Success";
                         IsSend = 1;
                         result = true;
                     }
                     else {
                         errMessage = data[2];
                         if (!string.IsNullOrEmpty(errMessage)) {
                             string[] errVendor = errMessage.Split(':');
                             if (errVendor.Length == 0)
                             {
                                 divResponse.Style.Add("display", "none");
                                 IsSend = 1;
                             }
                             
                         }
                         result = false;
                     }
                 }
                 else {
                     result = false;
                     errMessage = data[1];
                 }  
 
             }
            catch (Exception ex)
            {
                 result = false;
                 errMessage = ex.Message;
                 Helper.InsertErrorLog(ex);
            }

             return result;
        }

        private bool onCheckPayment(ref string errMessage, ref int IsSend)
        {
            bool result = true;
            try
            {

                //CHECK data payment 
                vPatientPaymentDtVirtual oPatientPaymentDtVirtual = BusinessLayer.GetvPatientPaymentDtVirtualList(string.Format(" PatientBillingID IN({0})", hdnBillingID.Value)).FirstOrDefault();
                if (oPatientPaymentDtVirtual != null)
                {
                    if (oPatientPaymentDtVirtual.PaymentID > 0)
                    {
                        hdnPaymentNo.Value = oPatientPaymentDtVirtual.PaymentNo;
                        errMessage = "Success";
                        return true;
                    }

                    //khusus QRIS tidak ada endpoint untuk cek status
                    if (!string.IsNullOrEmpty(oPatientPaymentDtVirtual.GCVirtualPaymentChannel))
                    {
                        if (oPatientPaymentDtVirtual.GCVirtualPaymentChannel == Constant.StandardCode.VIRTUAL_PAYMENT.QRIS)
                        {
                            if (oPatientPaymentDtVirtual.PaymentID == 0)
                            {
                                hdnPaymentNo.Value = oPatientPaymentDtVirtual.PaymentNo;
                                errMessage = "Menunggu Pembayaran (PENDING)";
                                return false;
                            }
                        }
                    }
                       


                }

                string resultReponse = "";
                String invoiceNo = txtBillingNo.Text; ///.Replace("/", "-").Replace(" ", "");
                DokuHISRequestVa dataRequest = new DokuHISRequestVa()
                {
                    BillingNo = hdnBillingID.Value, //invoiceNo,
                    CashierGroup = hdnGCChasier.Value.Split('^')[1].ToString(),
                    CashierShift = hdnGCShift.Value.Split('^')[1].ToString(),
                    PaymentMethod = hdnPaymentMethod.Value.Split('^')[1].ToString(),
                    ProviderMethod = oPatientPaymentDtVirtual.GCProviderMethod,
                    BankCode = hdnBankCode.Value,
                };

                PaymentGatewayService oService = new PaymentGatewayService();

                string jsonRequest = JsonConvert.SerializeObject(dataRequest);
                string apiResult = oService.DokuCheckStatusPayment(jsonRequest);

                resultReponse = apiResult;
                string[] data = resultReponse.Split('|');
                if (data[0] == "1")
                {
                    if (data[1] == "SUCCESS")
                    {
                        result = true;
                        errMessage = data[1];
                        hdnPaymentNo.Value = data[3];
                    }
                    else
                    {
                        result = false;
                        if (data[2] == "EXPIRED")
                        {
                            IsSend = 3;
                            errMessage = string.Format("Status pembayaran untuk tagihan ({0}) sudah expired. Harap melakukan pembuatan nomor tagihan baru untuk melanjutkan proses pembayaran secara virtual.", txtBillingNo.Text);

                        }
                        else {
                            errMessage = string.Format("{0}" ,data[2]);
                        }
                    }

                }
                else
                {
                    result = false;
                    errMessage = data[2];
                }

            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
            }

            return result;
        }
     
        
    }
}
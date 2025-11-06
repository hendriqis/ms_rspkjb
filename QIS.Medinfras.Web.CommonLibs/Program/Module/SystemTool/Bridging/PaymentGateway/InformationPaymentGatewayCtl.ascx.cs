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
    public partial class InformationPaymentGatewayCtl : BaseViewPopupCtl
    {
        protected int pageCount = 1;
        public override void InitializeDataControl(string param)
        {
            vRegistration oReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = '{0}'", AppSession.RegisteredPatient.RegistrationID)).FirstOrDefault();
            if (oReg != null) {
                txtRegistrationNo.Text = oReg.RegistrationNo;
                txtPatientName.Text = oReg.PatientName;
            }
            //setting parameter

            List<SettingParameterDt> lstSettingParameter = BusinessLayer.GetSettingParameterDtList(string.Format(" HealthcareID='001' AND ParameterCode IN('{0}')", Constant.SettingParameter.PaymentGatewayConfig.DOKU_NMID)).ToList();
            hdnQMIND.Value = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.PaymentGatewayConfig.DOKU_NMID).FirstOrDefault().ParameterValue;

            BindGridPaymentVirtualHistoryView(1, true, ref pageCount);
        }
        protected void cbpPaymentVirtualHistory_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            int pageCount = 1;
            BindGridPaymentVirtualHistoryView(1, true, ref pageCount);
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        private void BindGridPaymentVirtualHistoryView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("RegistrationID ='{0}' AND IsDeleted=0 ", AppSession.RegisteredPatient.RegistrationID);
            List<vPatientPaymentDtVirtualInformation> lstEntity = BusinessLayer.GetvPatientPaymentDtVirtualInformationList(filterExpression);
            gridPaymentVirtualView.DataSource = lstEntity;
            gridPaymentVirtualView.DataBind();
        }

        protected void cbpPaymentVirtualHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        
            if(e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblStatus = e.Row.FindControl("lblStatus") as Label;
                ///Label lblRemarks = e.Row.FindControl("lblRemarks") as Label;
                Label lblBank = e.Row.FindControl("lblBank") as Label; // pada saat pengajuan pertama kali belum jadi pembayaran
                HyperLink hl = (HyperLink)e.Row.FindControl("link");
                PlaceHolder plBarCode = (PlaceHolder)e.Row.FindControl("plBarCode");
                Label lblQMIND = (Label)e.Row.FindControl("lblQMIND");
                PlaceHolder plQrisLogo = (PlaceHolder)e.Row.FindControl("plBarCode");
               
                vPatientPaymentDtVirtualInformation entity = e.Row.DataItem as vPatientPaymentDtVirtualInformation;
                string fillterExpression = string.Format("ReferenceNo='{0}' and IsDeleted=0", entity.ReferenceNo);
                vPatientPaymentDtVirtual oPatientPaymentDtVirtual = BusinessLayer.GetvPatientPaymentDtVirtualList(fillterExpression).FirstOrDefault();
                if (oPatientPaymentDtVirtual != null) {

                    string remarks = string.Empty;
                    if(oPatientPaymentDtVirtual.PaymentID == 0) // belum sucess
                    {

                        DokuHISRequestVa dataLog = JsonConvert.DeserializeObject<DokuHISRequestVa>(oPatientPaymentDtVirtual.ResponseMessageText);
                        if (dataLog != null)
                        {
                            if (dataLog.Channel == "qris")
                            {
                                if (dataLog.JsonResponse != null)
                                {
                                    // QrisJsonResponse
                                    QrisJsonResponse oData = JsonConvert.DeserializeObject<QrisJsonResponse>(dataLog.JsonResponse);
                                    if (oData.responseCode == "0000")
                                    {
                                        //lable qris display
                                        lblQMIND.Visible = true;
                                        lblQMIND.Text = hdnQMIND.Value;
                                        //logo qris
                                        System.Web.UI.WebControls.Image imgLogoQris = new System.Web.UI.WebControls.Image();
                                        imgLogoQris.Height = 45;
                                        imgLogoQris.Width = 45;
                                        imgLogoQris.ImageUrl = ResolveUrl("~/Libs/Images/QRIS-Logo.png");
                                        plQrisLogo.Visible = true;
                                        plQrisLogo.Controls.Add(imgLogoQris);

                                        string contents = oData.qrCode;
                                        QRCodeEncoder qRCodeEncoder = new QRCodeEncoder();
                                        qRCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                                        qRCodeEncoder.QRCodeScale = 4;
                                        qRCodeEncoder.QRCodeVersion = 0;
                                        qRCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
                                        System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                                        System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
                                        imgBarCode.Height = 150;
                                        imgBarCode.Width = 150;

                                        using (Bitmap bitMap = qRCodeEncoder.Encode(contents, System.Text.Encoding.UTF8))
                                        {
                                            using (MemoryStream ms = new MemoryStream())
                                            {
                                                bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                                                byte[] byteImage = ms.ToArray();
                                                imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                                            }
                                            plBarCode.Visible = true;
                                            plBarCode.Controls.Add(imgBarCode);
                                        }
                                    }

                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(dataLog.JsonResponse))
                                {
                                    DOKULogResponseLog DokuDataLog = JsonConvert.DeserializeObject<DOKULogResponseLog>(dataLog.JsonResponse);
                                    if (dataLog != null)
                                    {

                                        if (DokuDataLog.virtual_account_info != null)
                                        {
                                            //virtual Account
                                            hl.NavigateUrl = DokuDataLog.virtual_account_info.how_to_pay_page;
                                            hl.Text = "Link Page";
                                            hl.Style.Remove("display");
                                            // remarks = string.Format("Link Page: {0}", DokuDataLog.virtual_account_info.how_to_pay_page);
                                        }
                                        else if (DokuDataLog.response.payment.url != null)
                                        {
                                            //checkout
                                            hl.NavigateUrl = DokuDataLog.response.payment.url;
                                            hl.Text = "Link Page";
                                            hl.Style.Remove("display");
                                            //remarks = string.Format("Link Page: {0}", DokuDataLog.response.payment.url);
                                        }
                                        else if (DokuDataLog.shopeepay_payment.redirect_url_http != null)
                                        {
                                            //shoppepay
                                            hl.NavigateUrl = DokuDataLog.shopeepay_payment.redirect_url_http;
                                            hl.Text = "Link Page";
                                            hl.Style.Remove("display");
                                            //remarks = string.Format("Link Page : {0}", DokuDataLog.shopeepay_payment.redirect_url_http);
                                        }
                                        //else {
                                        //    //DEFAULT 
                                        //    remarks = "PENDING";
                                        //}

                                    }
                                }
                            }

                        }
                    }
                   // lblRemarks.Text = remarks;
                    lblStatus.Text = oPatientPaymentDtVirtual.StatusPayment;
                    lblBank.Text = oPatientPaymentDtVirtual.BankName;
                }
            }
    
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
        private bool onCheckPayment(ref string errMessage, ref int IsSend)
        {
            bool result = true;
            try
            {

               //CHECK data payment 
               List<vPatientPaymentDtVirtual> lstPatientPaymentDtVirtual= BusinessLayer.GetvPatientPaymentDtVirtualList(string.Format(" ReferenceNo='{0}' AND IsDeleted=0", hdnReferenceNo.Value));
               string BilingID = string.Empty;
               string cashierGroup = string.Empty;
               string cashierShift = string.Empty;
               string BankCode = string.Empty;
               string gcPaymentMethod = "X035^013"; //sementara di patok, karna default nya selalu pakai virtual payment.
                if (lstPatientPaymentDtVirtual.Count > 0) {

                   vPatientPaymentDtVirtual oPatientPaymentDtVirtual = lstPatientPaymentDtVirtual.FirstOrDefault();
                   cashierGroup = oPatientPaymentDtVirtual.GCCashierGroup;
                   cashierShift = oPatientPaymentDtVirtual.GCCashierShift;
                   BankCode = oPatientPaymentDtVirtual.BankCode;

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
                           if (oPatientPaymentDtVirtual.GCVirtualPaymentChannel == Constant.StandardCode.VIRTUAL_PAYMENT.QRIS) {
                               if (oPatientPaymentDtVirtual.PaymentID == 0)
                               {
                                   hdnPaymentNo.Value = oPatientPaymentDtVirtual.PaymentNo;
                                   errMessage = "Menunggu Pembayaran (PENDING)";
                                   return false;
                               }
                           }
                       }
                       
                       
                   }
                   foreach (vPatientPaymentDtVirtual row in lstPatientPaymentDtVirtual) {
                       BilingID += string.Format("{0},", row.PatientBillingID);
                   }
                   BilingID = BilingID.Remove(BilingID.Length - 1);

                   
               }
                

                string resultReponse = "";
                String invoiceNo = hdnBillingNo.Value; ///.Replace("/", "-").Replace(" ", "");
                DokuHISRequestVa dataRequest = new DokuHISRequestVa()
                {
                    BillingNo = BilingID, //invoiceNo,
                    CashierGroup = cashierGroup.Split('^')[1].ToString(),
                    CashierShift = cashierShift.Split('^')[1].ToString(),
                    PaymentMethod = gcPaymentMethod.Split('^')[1].ToString(),
                    BankCode = BankCode,
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
                            errMessage = string.Format("Status pembayaran sudah expired. Harap melakukan pembuatan nomor tagihan baru untuk melanjutkan proses pembayaran secara virtual.");

                        }
                        else
                        {
                            errMessage = string.Format("{0}", data[2]);
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
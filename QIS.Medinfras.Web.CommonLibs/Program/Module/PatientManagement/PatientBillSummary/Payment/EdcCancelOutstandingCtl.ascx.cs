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
    public partial class EdcCancelOutstandingCtl : BaseViewPopupCtl
    {
        protected int pageCount = 1;
        public override void InitializeDataControl(string param)
        {
            hdnRegID.Value = param;
            BindDataOutstanding(); 
        }
        
        private void BindDataOutstanding()
        {
            string formatString = string.Format("RegistrationID='{0}' and IsFinish =0 AND  ResponseText Is Null AND  IsFinish =0 ", hdnRegID.Value);
            List<vEDCMachineTransaction> lstData = BusinessLayer.GetvEDCMachineTransactionList(formatString);
            gridEdcPaymentView.DataSource = lstData;
            gridEdcPaymentView.DataBind();
        }

        protected void cbpEdcPaymentOutstanding_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }
        protected void cbpEdcPaymentOutstanding_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int IsSend = 0;
            //IsSend = 0 => false, IsSend = 1 true, IsSend 3 => expired
            string urlPage = "";
            if (e.Parameter == "refresh")
            {
                
                BindDataOutstanding();
            }
            else if(e.Parameter == "cancel"){
                result = "cancel|";
                ProcessCancelPaymentEdc(ref errMessage, ref result);
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        private Boolean ProcessCancelPaymentEdc(ref String ErrMessage, ref String result)
        {
            Boolean resultprocess = true;
            int ID = Convert.ToInt32(hdnID.Value);
            IDbContext ctx = DbFactory.Configure(true);
            EDCMachineTransactionDao edcDao = new EDCMachineTransactionDao(ctx);
            try
            {
                EDCMachineTransaction oData = edcDao.Get(ID);
                if (oData != null) {

                    if (oData.PaymentID > 0 || oData.PaymentDetailID > 0 || !string.IsNullOrEmpty(oData.ResponseText))
                    {
                        resultprocess = false;
                        result += string.Format("fail|Maaf, Tidak bisa di cancel karna sudah ada transaksinya");
                    }
                    else {
                        oData.IsFinish = true;
                        oData.LastUpdatedBy = AppSession.UserLogin.UserID;
                        oData.LastUpdatedDate = DateTime.Now;
                        edcDao.Update(oData);
                        result += string.Format("success|Maaf, Tidak bisa di cancel karna sudah ada transaksinya");
                        ctx.CommitTransaction();
                    } 
                }
            }
            catch(Exception ex) {
                result += string.Format("fail|{0}", ex.Message);
                resultprocess = false;

                ctx.RollBackTransaction();
            }
            finally {
                ctx.Close();
            }

            return resultprocess;
        }
        
    }
}
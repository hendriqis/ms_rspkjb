using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;
using QIS.Medinfras.Common;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class GenerateDokumenBPJSCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramData = param.Split('|');
            hdnRegistrationID.Value = paramData[0];
            txtRegistrationNo.Text = paramData[1];
            hdnMenuIDCtl.Value = paramData[2];

            vRegistrationBPJS oregBpjs = BusinessLayer.GetvRegistrationBPJSList(string.Format("RegistrationID='{0}'", hdnRegistrationID.Value)).FirstOrDefault();
            txtSepNo.Text = oregBpjs.NoSEP;
            txtPeserta.Text = oregBpjs.NoPeserta;
            hdnSepNo.Value = oregBpjs.NoSEP;
            hdnNoPeserta.Value = oregBpjs.NoPeserta;
            hdnRegistrationNo.Value = oregBpjs.RegistrationNo;

            BindGridView(); 
        }

        protected void cbpReportProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {

            int pageCount = 1;
            string errMessage = "";
            string result ="";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                result = param[0] +"|"; 
                if (param[0] == "save")
                {

                    if (onSaveData(ref errMessage))
                    {
                        result += "success|";
                    }
                    else { 
                        result += "fail|" + errMessage;
                    }    
                    
                }
                else // refresh
                {
                    BindGridView();
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView()
        {
            string filterexpresion = string.Format("MenuID = '{0}' AND ISDELETED=0", hdnMenuIDCtl.Value);
            List<vReportByMenuMaster> lstReportMenu = BusinessLayer.GetvReportByMenuMasterList(filterexpresion);
            grdReportMaster.DataSource = lstReportMenu;
            grdReportMaster.DataBind();
        }

        private bool onSaveData( ref string errMessage)
        {
            //IDbContext Linkctx = DbFactory.Configure("medinfrasLINK", true);
            ////RegistrationBpjsReportDao regBpjsDao = new RegistrationBpjsReportDao(Linkctx);
            try
            {
                //RegistrationBpjsReport odata = new RegistrationBpjsReport();
                //odata.RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
                //odata.JobRequestBy = AppSession.UserLogin.UserID;
                //odata.JobRequestDate = DateTime.Now;
                //odata.JobStatus = 0;
                //odata.RegistrationNo = hdnRegistrationNo.Value; 
                //odata.NoPeserta = hdnNoPeserta.Value;
                //odata.NoSEP = hdnSepNo.Value;
                //odata.ReportCode = hdnReportCode.Value;
                //regBpjsDao.Insert(odata);
                //Linkctx.CommitTransaction();

                #region Send To Broker Service
                SettingParameterDt oParam = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID='001' AND ParameterCode='{0}'", Constant.SettingParameter.SA_BROKER_REPORT)).FirstOrDefault();
                if (oParam != null)
                {
                    if (string.IsNullOrEmpty(oParam.ParameterValue)) {
                        errMessage = string.Format("Alamat broker tidak ditemukan ({0})", oParam.ParameterCode);
                        return false;
                    }
                    string[] paramInfo = oParam.ParameterValue.Split(':');
                    string ipaddress = paramInfo[0];
                    string port = !string.IsNullOrEmpty(paramInfo[1]) ? paramInfo[1] : "6000";
                    string RegistrationID = hdnRegistrationID.Value;
                     
                    string msgText = string.Format("BPJS|{0}|{1}|{2}|{3}|{4}", hdnRegistrationID.Value, hdnRegistrationNo.Value, hdnSepNo.Value, hdnNoPeserta.Value, hdnReportCode.Value); 
                    string result = CommonMethods.SendMessageToListener(ipaddress, port, msgText);
                    string[] resultInfo = result.Split('|');
                    bool isSuccess = resultInfo[0] == "1";
                    int errorNo = 0; 
                    if (!isSuccess)
                        errorNo += 1;

                    //#region Update Order Status and Log HL7 Message
                    //try
                    //{
                    //    UpdateOrderStatus(Constant.HL7_Partner.MEDINFRAS, Constant.HL7_MessageType.Infinitt, messageCode, msgText, isSuccess, resultInfo[2], item, string.Empty, string.Format("{0}:{1}", ipaddress, port));
                    //}
                    //catch (Exception ex)
                    //{
                    //    result = string.Format("{0}|{1} ({2})", "0", "An error occured when update order status", ex.Message);
                    //    break;
                    //}
                    //#endregion
                }
                 
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                //Linkctx.RollBackTransaction();

                return false;
            }
          
        }
       
        
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Service;
using DevExpress.Web.ASPxCallbackPanel;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class GetNoAntrianCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {

            string[] paramInfo = param.Split('|');
            hdnTransactionIDCtl.Value = paramInfo[0];
            txtTransactionNo.Text = paramInfo[1];

            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                   "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
                                                   AppSession.UserLogin.HealthcareID, //0
                                                   Constant.SettingParameter.SA0193, //1
                                                   Constant.SettingParameter.SA0220 //2

           ));
            hdnIsBridgingMedinlinkCtl.Value = lstSettingParameterDt.Where(a => a.ParameterCode == Constant.SettingParameter.SA0193).FirstOrDefault().ParameterValue;
            hdnIsGetQueueNoMedinlinkCtl.Value = lstSettingParameterDt.Where(a => a.ParameterCode == Constant.SettingParameter.SA0220).FirstOrDefault().ParameterValue;

            vPrescriptionOrderHd oPrescriptionOrderHD = BusinessLayer.GetvPrescriptionOrderHdList(string.Format("ChargesTransactionID='{0}'", hdnTransactionIDCtl.Value)).FirstOrDefault();
            if (oPrescriptionOrderHD != null) {
                hdnDepartmentIDCtl.Value = oPrescriptionOrderHD.DepartmentID;
                
                if (!string.IsNullOrEmpty(oPrescriptionOrderHD.ReferenceNo)) { 
                    string[] oData = oPrescriptionOrderHD.ReferenceNo.Split('|');
                    txtAntrianNo.Text = oData[1];

                    if (!string.IsNullOrEmpty(oData[1])) {
                        btnAmbilAntrian.Visible = false;
                    }
                }
            }

        }

        protected void cbpGetAntrianProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            string result = "";
            Boolean isSuccess = false;
            String ErrMessage = ""; 
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "GetAntrian")
                {
                    GetNomorAntrian(ref  isSuccess, ref  ErrMessage);
                    if (isSuccess)
                    {
                        result = "GetAntrian|success|";
                    }
                    else {
                        result = "GetAntrian|fail|" + ErrMessage;
                    }
                  
                }
            }
            panel.JSProperties["cpResult"] = result;
        }

        public void GetNomorAntrian(ref Boolean isSuccess, ref String ErrMessage)
        {
            try {
                if (hdnIsBridgingMedinlinkCtl.Value == "1" && hdnIsGetQueueNoMedinlinkCtl.Value == "1")
                {
                    if (hdnDepartmentIDCtl.Value != Constant.Facility.INPATIENT)
                    {
                        GetQueueMedinlink( Request.Form[txtTransactionNo.UniqueID]);
                        isSuccess = true;
                    }
                    else
                    {
                        isSuccess = false;
                        ErrMessage = "Error : Hanya berlaku untuk pasien rawat jalan";
                    }
                }
                else {
                    isSuccess = false;
                    ErrMessage = "Error : Konfigurasi tidak aktif (kode SA0193, SA0220) ";
                }
            }
            catch (Exception ex) {
                isSuccess = false;
                ErrMessage = ex.Message ;
            }
        }
        private void GetQueueMedinlink(String TransactionNo)
        {
            QueueService oServices = new QueueService();
            oServices.MedinlinkGetQueuePharmacyViaAPIMedin(TransactionNo);
        }
    }
}
using System;
using System.Collections.Generic;
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
    public partial class TransactionInputFixedAssetCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                vPatientChargesHd entity = BusinessLayer.GetvPatientChargesHdList(string.Format("TransactionID = {0}", param)).FirstOrDefault();
                hdnTransactionIDCtl2.Value = entity.TransactionID.ToString();
                txtTransactionNoCtl2.Text = entity.TransactionNo;
                txtTransactionDateTimeCtl2.Text = entity.cfTransactionDate + " " + entity.TransactionTime;
                txtServiceUnitNameCtl2.Text = entity.ServiceUnitName;

                BindGridView();
            } 
        }
        
        private void BindGridView()
        {
            string filter = string.Format("TransactionID = {0}", hdnTransactionIDCtl2.Value);
            grdChargesFixedAsset.DataSource = BusinessLayer.GetvPatientChargesDtCustom2List(filter);
            grdChargesFixedAsset.DataBind();
        }

        protected void cbpChargesDtInfoFixedAsset_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string result = "";
            string errMessage = "";
            if (e.Parameter == "save")
            {
                result = "save|";
                if (hdnChargesDtIDCtl2.Value.ToString() != "")
                {
                    if (OnSaveEditRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                if (hdnFixedAssetIDCtl2.Value != null && hdnFixedAssetIDCtl2.Value != "" && hdnFixedAssetIDCtl2.Value != "0")
                {
                    PatientChargesDtInfo entity = BusinessLayer.GetPatientChargesDtInfo(Convert.ToInt32(hdnChargesDtIDCtl2.Value));
                    entity.FixedAssetID = Convert.ToInt32(hdnFixedAssetIDCtl2.Value);
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdatePatientChargesDtInfo(entity);
                }
                else
                {
                    result = false;
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
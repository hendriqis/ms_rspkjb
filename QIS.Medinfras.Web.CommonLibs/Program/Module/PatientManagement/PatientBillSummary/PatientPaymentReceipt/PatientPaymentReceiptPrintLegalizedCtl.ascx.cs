using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxEditors;
using QIS.Data.Core.Dal;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientPaymentReceiptPrintLegalizedCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnParam.Value = param;
            hdnRegistrationID.Value = hdnParam.Value.Split('|')[0];
            hdnPaymentReceiptIDLegalized.Value = hdnParam.Value.Split('|')[1];
            hdnDepartmentIDLegalized.Value = hdnParam.Value.Split('|')[2];

            List<SettingParameterDt> setvar = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}','{1}','{2}','{3}')",
                Constant.SettingParameter.FN_REPORTCODE_CETAK_LEGALISIR_KWITANSI_RAWATINAP, Constant.SettingParameter.FN_REPORTCODE_CETAK_LEGALISIR_KWITANSI_RAWATINAP_BAHASA_ASING,
                Constant.SettingParameter.FN_REPORTCODE_CETAK_LEGALISIR_KWITANSI_RAWATJALAN, Constant.SettingParameter.FN_REPORTCODE_CETAK_LEGALISIR_KWITANSI_RAWATJALAN_BAHASA_ASING));

            hdnReportCodeReceiptLegalized.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.FN_REPORTCODE_CETAK_LEGALISIR_KWITANSI_RAWATINAP).FirstOrDefault().ParameterValue;
            hdnReportCodeReceiptEnglishLegalized.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.FN_REPORTCODE_CETAK_LEGALISIR_KWITANSI_RAWATINAP_BAHASA_ASING).FirstOrDefault().ParameterValue;

            hdnReportCodeReceiptOutpatientLegalized.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.FN_REPORTCODE_CETAK_LEGALISIR_KWITANSI_RAWATJALAN).FirstOrDefault().ParameterValue;
            hdnReportCodeReceiptEnglishOutpatientLegalized.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.FN_REPORTCODE_CETAK_LEGALISIR_KWITANSI_RAWATJALAN_BAHASA_ASING).FirstOrDefault().ParameterValue;

            if (hdnDepartmentIDLegalized.Value != Constant.Facility.INPATIENT)
            {
                SettingParameterDt setvarDotMatrix = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode = '{0}'",
                    Constant.SettingParameter.IS_OUTPATIENT_RECEIPT_USING_DOT_MATRIX)).FirstOrDefault();

                if (setvarDotMatrix.ParameterValue == "1")
                {
                    hdnIsDotMatrixAndOutpatientLegalized.Value = "1";
                }
                else
                {
                    hdnIsDotMatrixAndOutpatientLegalized.Value = "0";
                }
            }
        }

        protected void cbpPatientPaymentReceiptLegalized_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";

            result = "success";

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpPatientPaymentReceiptLegalizedDotMatrix_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int selectedLanguage = rblReceiptOption.SelectedIndex;
            if (selectedLanguage == 0)
            {
                result = PrintKwitansiDotMatrix(ref errMessage);
            }
            else 
            {
                result = PrintKwitansiDotMatrixEng(ref errMessage);                
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private string PrintKwitansiDotMatrix(ref string errMessage)
        {
            string result = "";
            try
            {
                string id = hdnParamReport.Value;
                result = ZebraPrinting.PrintKwitansi(id, "");
            }
            catch (Exception ex)
            {
                result = "";
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            finally
            {
            }
            return result;
        }

        private string PrintKwitansiDotMatrixEng(ref string errMessage)
        {
            string result = "";
            try
            {
                string id = hdnParamReport.Value;
                result = ZebraPrinting.PrintKwitansiEng(id, "");
            }
            catch (Exception ex)
            {
                result = "";
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            finally
            {
            }
            return result;
        }
    }
}
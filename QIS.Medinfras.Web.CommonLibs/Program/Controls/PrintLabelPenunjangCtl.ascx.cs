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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PrintLabelPenunjangCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnPatientChargesIDLabelCtl.Value = param;
            BindGridView();
        }

        protected void cbpPrintProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {

            int pageCount = 1;
            string errMessage = "";
            string result ="";



            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                result = param[0] +"|";
                if (param[0] == "print")
                {

                    if (!String.IsNullOrEmpty(hdnSelectedID.Value))
                    {
                        if (PrintDrugLabel(hdnSelectedID.Value, ref errMessage))
                        {
                            result += string.Format("success|{0}", errMessage);
                        }
                        else
                        {
                            result += string.Format("fail|{0}", errMessage);
                        }
                    }
                    else
                    {
                        result += string.Format("fail|Silahkan pilih yang akan diprint.");
                    }


                }
                else {
                    BindGridView(); 
                }
                
            }
            ASPxCallbackPanel p = sender as ASPxCallbackPanel;
            p.JSProperties["cpResult"] = result;
        }

        private void BindGridView()
        {
            string filterexpresion = string.Format("TransactionID = '{0}' AND IsDeleted=0", hdnPatientChargesIDLabelCtl.Value);
            List<vPatientChargesDt> lstReportMenu = BusinessLayer.GetvPatientChargesDtList(filterexpresion);
            grdViewLabel.DataSource = lstReportMenu;
            grdViewLabel.DataBind();
        }
        private bool PrintDrugLabel(string DetailID, ref string errMessage)
        {
            bool result = true ;
            try
            {
                
                string ipAddress = HttpContext.Current.Request.UserHostAddress;
                Healthcare oHealthcare = BusinessLayer.GetHealthcare(AppSession.UserLogin.HealthcareID);
                string Initial =  oHealthcare.Initial;
                if (Initial == "RSRT")
                {
                    string filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0", ipAddress, Constant.DirectPrintType.LABEL_RADIOLOGI_2);
                    List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);
                    string printerUrl1 = lstPrinter.FirstOrDefault().PrinterName;
                    vPatientChargesDt charges = BusinessLayer.GetvPatientChargesDtList(string.Format("ID = {0}", DetailID)).FirstOrDefault();
                    ZebraPrinting.printLabelRadiologiRSRT2(charges, printerUrl1);
                }
                else {
                    string filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0", ipAddress, Constant.DirectPrintType.LABEL_RADIOLOGI);
                    List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);
                    string printerUrl1 = lstPrinter.FirstOrDefault().PrinterName;
                    vPatientChargesDt charges = BusinessLayer.GetvPatientChargesDtList(string.Format("ID = {0}", DetailID)).FirstOrDefault();
                    ZebraPrinting.printLabelRadiologi2RSSES(charges, printerUrl1);
                }
               
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
            }
            return result;
        }

    }
}
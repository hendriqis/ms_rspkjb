using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Nutrition.Program
{
    public partial class PrintDistributionList : BaseViewPopupCtl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnNutritionOrderHdID.Value = paramInfo[0];
            hdnNutritionOrderDtID.Value = paramInfo[1];
            txtNutritionOrderDate.Text = paramInfo[2];
            txtNutritionOrderTime.Text = paramInfo[3];

            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN, Constant.SettingParameter.USE_LAST_PURCHASE_EXPIRED_DATE));
            hdnPrinterType.Value = lstSettingParameterDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN).FirstOrDefault().ParameterValue;
            hdnIsUsedlastPurchaseExpiredDate.Value = lstSettingParameterDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.USE_LAST_PURCHASE_EXPIRED_DATE).FirstOrDefault().ParameterValue;
           
            BindGridView();
        }

        private void BindGridView()
        {
            String filterExpression = String.Format("ScheduleDate = '{0}' AND GCTransactionStatus NOT IN ('{1}','{2}')", Helper.GetDatePickerValue(txtNutritionOrderDate).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.TransactionStatus.VOID, Constant.TransactionStatus.OPEN);
            List<vNutritionOrderDtCustom> lstEntity = BusinessLayer.GetvNutritionOrderDtCustomList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwViewPrint_RowDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vNutritionOrderDtCustom entity = e.Item.DataItem as vNutritionOrderDtCustom;

                TextBox txtExpiredDate = e.Item.FindControl("txtExpiredDate") as TextBox;
                txtExpiredDate.Text = "";
                txtExpiredDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

                TextBox txtExpiredTime = e.Item.FindControl("txtExpiredTime") as TextBox;
                txtExpiredTime.Text = "";
                txtExpiredTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            }
        }

        protected void cbpProcessPrint_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string lstNutritionOrderDtID = hdnSelectedID.Value;
            string lstExpiredDate = hdnSelectedDate.Value;
            string lstExpiredTime = hdnSelectedTime.Value;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            if (!String.IsNullOrEmpty(lstExpiredDate))
            {
                //Update Drug Expired Date
                string[] detailIDLst = lstNutritionOrderDtID.Split(',');
                string[] expiredDateLst = lstExpiredDate.Split(',');

                int index = 0;
                foreach (string recordID in detailIDLst)
                {
                    if (!string.IsNullOrEmpty(expiredDateLst[index]))
                    {
                        DateTime expiredDate;
                        string format = Constant.FormatString.DATE_PICKER_FORMAT;
                        try
                        {
                            expiredDate = DateTime.ParseExact(expiredDateLst[index], format, CultureInfo.InvariantCulture);
                        }
                        catch (FormatException)
                        {
                        }
                    }
                    index += 1;
                }
            }
            panel.JSProperties["cpZebraPrinting"] = PrintNutritionLabel(lstNutritionOrderDtID, lstExpiredDate, lstExpiredTime);
        }

        private string PrintNutritionLabel(string lstNutritionOrderDtID, string lstExpiredDate, string lstExpiredTime)
        {
            string result = string.Empty;
            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.NT_FORMAT_CETAKAN_ETIKET_GIZI, Constant.SettingParameter.NT_JENIS_PRINTER_ETIKET_GIZI);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.NT_FORMAT_CETAKAN_ETIKET_GIZI)).FirstOrDefault().ParameterValue;
                string printerType = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.NT_JENIS_PRINTER_ETIKET_GIZI)).FirstOrDefault().ParameterValue;

                bool isBasedOnIPAddress = AppSession.IsPrinterLocationBasedOnIP;

                if (isBasedOnIPAddress)
                {
                    //Check Printer Type
                    switch (printerType)
                    {
                        case Constant.PrinterType.ZEBRA_PRINTER:
                            //Get Printer Address
                            string ipAddress = HttpContext.Current.Request.UserHostAddress;

                            filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                ipAddress, Constant.DirectPrintType.LABEL_DISTRIBUSI_GIZI);

                            List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);
                            if (lstPrinter.Count > 0)
                            {
                                string printerUrl1 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.LABEL_DISTRIBUSI_GIZI).FirstOrDefault().PrinterName;

                                if (printFormat == Constant.PrintFormat.BIXOLON_ETIKET_RSPKSB)
                                {
                                    int labelCount = Convert.ToInt16(txtJmlLabelCtl.Text);
                                    ZebraPrinting.PrintNutritionLabelBixolon(
                                        printFormat,
                                        lstNutritionOrderDtID,
                                        printerUrl1,
                                        labelCount);
                                }
                                else
                                {
                                    int labelCount = Convert.ToInt16(txtJmlLabelCtl.Text);
                                    ZebraPrinting.PrintNutritionLabel(
                                        printFormat,
                                        lstNutritionOrderDtID,
                                        printerUrl1,
                                        labelCount,
                                        lstExpiredDate,
                                        lstExpiredTime);
                                }
                            }
                            else
                            {
                                result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                            }
                            break;
                        case Constant.PrinterType.EPSON_DOT_MATRIX:
                            //Get Printer Address
                            string ipAddress2 = HttpContext.Current.Request.UserHostAddress;

                            filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                ipAddress2, Constant.DirectPrintType.LABEL_DISTRIBUSI_GIZI);

                            List<PrinterLocation> lstPrinter2 = BusinessLayer.GetPrinterLocationList(filterExp);

                            if (lstPrinter2.Count > 0)
                            {
                                string printerUrl2 = lstPrinter2.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.LABEL_DISTRIBUSI_GIZI).FirstOrDefault().PrinterName;

                                int labelCount = Convert.ToInt16(txtJmlLabelCtl.Text);
                                ZebraPrinting.PrintNutritionLabelDotMatrix(
                                    lstNutritionOrderDtID,
                                    printerUrl2,
                                    labelCount);
                            }
                            else
                                result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress2);
                            break;
                        case Constant.PrinterType.BROTHER_PRINTER:
                            string ipAddressBrother = HttpContext.Current.Request.UserHostAddress;
                            string filterExpBrother = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                    ipAddressBrother, Constant.DirectPrintType.LABEL_DISTRIBUSI_GIZI);
                            List<PrinterLocation> lstPrinterBrother = BusinessLayer.GetPrinterLocationList(filterExpBrother);

                            if (lstPrinterBrother.Count > 0)
                            {
                                string printerUrlBrother = lstPrinterBrother.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.LABEL_DISTRIBUSI_GIZI).FirstOrDefault().PrinterName;
                                BrotherPrinting.PrintLabelDistribusiGizi_RSSY(lstNutritionOrderDtID, printFormat, printerUrlBrother);
                            }
                            else
                            {
                                result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddressBrother);
                            }
                            break;
                    }
                }
                else
                {
                    result = string.Format("Printer Configuration is not available");
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class PrintDrugLabelList : BaseViewPopupCtl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnTransactionID.Value = paramInfo[0];
            string transactionNo = paramInfo[1];
            txtPresciptionNo.Text = transactionNo;
            txtPrescriptionDate.Text = paramInfo[2];
            txtPrescriptionTime.Text = paramInfo[3];
            BindGridView();
        }

        private void BindGridView()
        {
            String filterExpression = String.Format("TransactionID = {0} AND IsRFlag = 1 ORDER BY ID", hdnTransactionID.Value);
            List<vChargesForDrugLabel> lstEntity = BusinessLayer.GetvChargesForDrugLabelList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void cbpProcessPrint_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string lstRecordID = hdnSelectedID.Value;
            string lstExpiredDate = hdnSelectedExpiredDate.Value;
            string lstPrintNo = hdnSelectedPrintNo.Value;

            if (!String.IsNullOrEmpty(lstExpiredDate))
            {
                //Update Drug Expired Date
                string[] detailIDLst = lstRecordID.Split(',');
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

                            PatientChargesDt chargesDt = BusinessLayer.GetPatientChargesDt(Convert.ToInt32(recordID));
                            PrescriptionOrderDt orderDt = BusinessLayer.GetPrescriptionOrderDt(Convert.ToInt32(chargesDt.PrescriptionOrderDetailID));
                            if (orderDt != null)
                            {
                                orderDt.ExpiredDate = expiredDate;
                                orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                BusinessLayer.UpdatePrescriptionOrderDt(orderDt);
                            }
                        }
                        catch (FormatException)
                        {
                        }
                    }
                    index += 1;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            string result = PrintDrugLabel(lstRecordID, lstExpiredDate, lstPrintNo);
            panel.JSProperties["cpZebraPrinting"] = result;
        }

        private string PrintDrugLabel(string lstRecordID, string lstExpiredDate, string lstPrintNo)
        {
            string result = string.Empty;
            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}', '{3}')",
                                                    AppSession.UserLogin.HealthcareID, 
                                                    Constant.SettingParameter.FM_FORMAT_CETAKAN_ETIKET, 
                                                    Constant.SettingParameter.FM_ETIKET_UDD_PRINT_KOMPOSISI_RACIKAN,
                                                    Constant.SettingParameter.SA_LOKASI_PRINTER_IP_ADDR
                                                    );
                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.FM_FORMAT_CETAKAN_ETIKET)).FirstOrDefault().ParameterValue;
                string printDetail = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.FM_ETIKET_UDD_PRINT_KOMPOSISI_RACIKAN)).FirstOrDefault().ParameterValue;

                string SA_LOKASI_PRINTER_IP_ADDR = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.SA_LOKASI_PRINTER_IP_ADDR)).FirstOrDefault().ParameterValue;

                bool isBasedOnIPAddress = Convert.ToBoolean( Convert.ToInt16( SA_LOKASI_PRINTER_IP_ADDR));

                hdnIsUsedDispenseQty.Value = Convert.ToString(chkisUsedDispenseQty.Checked);

                if (isBasedOnIPAddress)
                {
                    //Get Printer Address
                    string ipAddress = HttpContext.Current.Request.UserHostAddress;

                    filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}','{2}') AND IsDeleted = 0", ipAddress, Constant.DirectPrintType.ETIKET_OBAT_DALAM, Constant.DirectPrintType.ETIKET_OBAT_LUAR);
                    List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);
                    if (lstPrinter.Count > 0)
                    {
                        string printerUrlOD = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.ETIKET_OBAT_DALAM).FirstOrDefault().PrinterName;
                        string printerUrlOL = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.ETIKET_OBAT_LUAR).FirstOrDefault().PrinterName;

                        bool isUsePrintingToolsOD = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.ETIKET_OBAT_DALAM).FirstOrDefault().IsUsingPrintingTools;
                        bool isUsePrintingToolsOL = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.ETIKET_OBAT_LUAR).FirstOrDefault().IsUsingPrintingTools;

                        PatientChargesHd oHeader = BusinessLayer.GetPatientChargesHd(Convert.ToInt32(hdnTransactionID.Value));

                        string filterExpression = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}')",
                                                                    AppSession.UserLogin.HealthcareID,
                                                                    Constant.SettingParameter.FM_FORMAT_CETAKAN_ETIKET,
                                                                    Constant.SettingParameter.FM_FORMAT_CETAKAN_ETIKET_RJ,
                                                                    Constant.SettingParameter.FM_FORMAT_CETAKAN_ETIKET_BY_TYPE,
                                                                    Constant.SettingParameter.FM_PRINT_ETIKET_DETAIL_OBAT_RACIKAN
                                                                    );
                        List<SettingParameterDt> lstParameterDt = BusinessLayer.GetSettingParameterDtList(filterExpression);
                        string printFormat2 = lstParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.FM_FORMAT_CETAKAN_ETIKET_RJ)).FirstOrDefault().ParameterValue;
                        Boolean isPrintByType = lstParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.FM_FORMAT_CETAKAN_ETIKET_BY_TYPE)).FirstOrDefault().ParameterValue == "1" ? true : false;

                        bool isPrintCompoundDetailLabel = false;
                        string printCompoundDetailLabel = lstParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.FM_PRINT_ETIKET_DETAIL_OBAT_RACIKAN)).FirstOrDefault().ParameterValue;

                        if (printFormat == Constant.PrintFormat.BROTHER_ETIKET_RSSY)
                        {
                                int prescOrderID = Convert.ToInt32(oHeader.PrescriptionOrderID);
                                if (prescOrderID != 0)
                                {
                                    PrescriptionOrderHd orderHd = BusinessLayer.GetPrescriptionOrderHd(prescOrderID);

                                    string lstPrescriptionOrderDtID = "";
                                    List<PrescriptionOrderDt> lstOrderDt = BusinessLayer.GetPrescriptionOrderDtList(string.Format(
                                                                                "PrescriptionOrderID = {0} AND PrescriptionOrderDetailID IN (SELECT cdt.PrescriptionOrderDetailID FROM PatientChargesDt cdt WITH(NOLOCK) WHERE ID IN ({1}))",
                                                                                orderHd.PrescriptionOrderID,
                                                                                lstRecordID
                                                                            ));
                                    foreach (PrescriptionOrderDt orderDt in lstOrderDt)
                                    {
                                        if(lstPrescriptionOrderDtID != null && lstPrescriptionOrderDtID != ""){
                                            lstPrescriptionOrderDtID += ",";
                                        }
                                        lstPrescriptionOrderDtID += orderDt.PrescriptionOrderDetailID;
                                    }

                                    BrotherPrinting.PrintDrugLabel_RSSY(
                                        orderHd,
                                        printFormat,
                                        lstPrescriptionOrderDtID,
                                        printerUrlOD,
                                        printerUrlOL,
                                        isPrintByType,
                                        hdnIsUsedDispenseQty.Value,
                                        isPrintCompoundDetailLabel,
                                        1,
                                        lstExpiredDate,
                                        lstPrintNo
                                    );
                                }
                        }
                        else if (printFormat == Constant.PrintFormat.BIXOLON_ETIKET_RSPKSB)
                        {
                            if (printDetail != "1")
                            {
                                ZebraPrinting.PrintDrugLabelFromChargesBixolon(oHeader, printFormat, lstRecordID, printerUrlOD, printerUrlOL, lstExpiredDate, lstPrintNo, hdnIsUsedDispenseQty.Value, isUsePrintingToolsOD, isUsePrintingToolsOL);
                            }
                            else
                            {
                                string id = string.Empty;
                                int prescOrderID = Convert.ToInt32(oHeader.PrescriptionOrderID);
                                if (prescOrderID != 0)
                                {
                                    PrescriptionOrderHd orderHd = BusinessLayer.GetPrescriptionOrderHd(prescOrderID);

                                    List<PrescriptionOrderDt> lstOrderDt = BusinessLayer.GetPrescriptionOrderDtList(string.Format("PrescriptionOrderID = {0}", orderHd.PrescriptionOrderID));
                                    foreach (PrescriptionOrderDt a in lstOrderDt)
                                    {
                                        id += a.PrescriptionOrderDetailID + ",";
                                    }
                                    string lstId = id.Remove(id.Length - 1, 1);
                                    if (printCompoundDetailLabel == "1")
                                    {
                                        isPrintCompoundDetailLabel = true;
                                        ZebraPrinting.PrintDrugLabel1Bixolon(
                                                      orderHd,
                                                      printFormat,
                                                      lstRecordID,
                                                      printerUrlOD,
                                                      printerUrlOL,
                                                      isPrintByType,
                                                      printFormat2,
                                                      hdnIsUsedDispenseQty.Value,
                                                      isPrintCompoundDetailLabel,
                                                      1,
                                                      lstExpiredDate,
                                                      lstPrintNo,
                                                      isUsePrintingToolsOD,
                                                      isUsePrintingToolsOL);
                                    }
                                    else
                                    {
                                        ZebraPrinting.PrintDrugLabel1Bixolon(
                                                      orderHd,
                                                      printFormat,
                                                      lstRecordID,
                                                      printerUrlOD,
                                                      printerUrlOL,
                                                      isPrintByType,
                                                      printFormat2,
                                                      hdnIsUsedDispenseQty.Value,
                                                      isPrintCompoundDetailLabel,
                                                      1,
                                                      lstExpiredDate,
                                                      lstPrintNo,
                                                      isUsePrintingToolsOD,
                                                      isUsePrintingToolsOL);
                                    }
                                }
                                else
                                {
                                    ZebraPrinting.PrintDrugLabelFromChargesBixolon(oHeader, printFormat, lstRecordID, printerUrlOD, printerUrlOL, lstExpiredDate, lstPrintNo, hdnIsUsedDispenseQty.Value, isUsePrintingToolsOD, isUsePrintingToolsOL);
                                }
                            }
                        }
                        else
                        {
                            if (printDetail != "1")
                            {
                                ZebraPrinting.PrintDrugLabelFromCharges(oHeader, printFormat, lstRecordID, printerUrlOD, printerUrlOL, lstExpiredDate, lstPrintNo, hdnIsUsedDispenseQty.Value, isUsePrintingToolsOD, isUsePrintingToolsOL);
                            }
                            else
                            {
                                string id = string.Empty;
                                int prescOrderID = Convert.ToInt32(oHeader.PrescriptionOrderID);
                                if (prescOrderID != 0)
                                {
                                    PrescriptionOrderHd orderHd = BusinessLayer.GetPrescriptionOrderHd(prescOrderID);

                                    List<PrescriptionOrderDt> lstOrderDt = BusinessLayer.GetPrescriptionOrderDtList(string.Format("PrescriptionOrderID = {0}", orderHd.PrescriptionOrderID));
                                    foreach (PrescriptionOrderDt a in lstOrderDt)
                                    {
                                        id += a.PrescriptionOrderDetailID + ",";
                                    }
                                    string lstId = id.Remove(id.Length - 1, 1);
                                    if (printCompoundDetailLabel == "1")
                                    {
                                        isPrintCompoundDetailLabel = true;
                                        ZebraPrinting.PrintDrugLabel1(
                                                      orderHd,
                                                      printFormat,
                                                      lstRecordID,
                                                      printerUrlOD,
                                                      printerUrlOL,
                                                      isPrintByType,
                                                      printFormat2,
                                                      hdnIsUsedDispenseQty.Value,
                                                      isPrintCompoundDetailLabel,
                                                      1,
                                                      lstExpiredDate,
                                                      lstPrintNo,
                                                      isUsePrintingToolsOD,
                                                      isUsePrintingToolsOL);
                                    }
                                    else
                                    {
                                        ZebraPrinting.PrintDrugLabel1(
                                                      orderHd,
                                                      printFormat,
                                                      lstRecordID,
                                                      printerUrlOD,
                                                      printerUrlOL,
                                                      isPrintByType,
                                                      printFormat2,
                                                      hdnIsUsedDispenseQty.Value,
                                                      isPrintCompoundDetailLabel,
                                                      1,
                                                      lstExpiredDate,
                                                      lstPrintNo,
                                                      isUsePrintingToolsOD,
                                                      isUsePrintingToolsOL);
                                    }
                                }
                                else
                                {
                                    ZebraPrinting.PrintDrugLabelFromCharges(oHeader, printFormat, lstRecordID, printerUrlOD, printerUrlOL, lstExpiredDate, lstPrintNo, hdnIsUsedDispenseQty.Value, isUsePrintingToolsOD, isUsePrintingToolsOL);
                                }
                            }
                        }
                    }
                    else
                    {
                        result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                    }
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
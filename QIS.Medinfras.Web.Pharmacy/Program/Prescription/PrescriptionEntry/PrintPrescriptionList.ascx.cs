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

namespace QIS.Medinfras.Web.Pharmacy.Program.Prescription.PrescriptionEntry
{
    public partial class PrintPrescriptionList : BaseViewPopupCtl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnPrescriptionOrderID.Value = paramInfo[0];
            string transactionNo = paramInfo[1];
            txtPresciptionNo.Text = transactionNo;
            txtPrescriptionDate.Text = paramInfo[2];
            txtPrescriptionTime.Text = paramInfo[3];
            hdnPrescriptionTypeCtl.Value = paramInfo[4];

            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
                                                                                                        AppSession.UserLogin.HealthcareID,
                                                                                                        Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN,
                                                                                                        Constant.SettingParameter.USE_LAST_PURCHASE_EXPIRED_DATE
                                                                                                    ));

            hdnPrinterType.Value = lstSettingParameterDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN).FirstOrDefault().ParameterValue;
            hdnIsUsedlastPurchaseExpiredDate.Value = lstSettingParameterDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.USE_LAST_PURCHASE_EXPIRED_DATE).FirstOrDefault().ParameterValue;

            if (hdnPrinterType.Value == Constant.PrinterType.DOT_MATRIX_FORMAT_1)
            {
                hdnIsDotMatrix.Value = "1";
                trPrescriptionType.Style.Remove("display");
            }
            else
            {
                hdnIsDotMatrix.Value = "0";
                trPrescriptionType.Style.Add("display", "none");
            }

            String filterExpression = string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PRESCRIPTION_TYPE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboPrescriptionType, lstStandardCode, "StandardCodeName", "StandardCodeID");
            cboPrescriptionType.SelectedIndex = 0;

            BindGridView();
        }

        private void BindGridView()
        {
            String filterExpression = String.Format("PrescriptionOrderID = {0} AND ParentID IS NULL AND OrderIsDeleted = 0 AND GCPrescriptionOrderStatus NOT IN ('{1}','{2}') AND TakenQty > 0", hdnPrescriptionOrderID.Value, Constant.OrderStatus.OPEN, Constant.OrderStatus.CANCELLED);
            List<vPrescriptionOrderDt1> lstEntity = BusinessLayer.GetvPrescriptionOrderDt1List(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwViewPrint_RowDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vPrescriptionOrderDt1 entity = e.Item.DataItem as vPrescriptionOrderDt1;

                TextBox txtExpiredDate = e.Item.FindControl("txtExpiredDate") as TextBox;
                String lastPurchaseExpiredDate = entity.LastPurchaseExpiredDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                if (hdnIsUsedlastPurchaseExpiredDate.Value == "1")
                {
                    if (lastPurchaseExpiredDate != "01-01-1900")
                    {
                        txtExpiredDate.Text = Convert.ToDateTime(entity.LastPurchaseExpiredDate).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    }
                    else
                    {
                        txtExpiredDate.Text = "";
                    }
                }
                else
                {
                    txtExpiredDate.Text = "";
                }
            }
        }

        protected void cbpProcessPrint_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string lstPrescriptionOrderDetailID = hdnSelectedID.Value;
            string lstExpiredDate = hdnSelectedDate.Value;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            if (!String.IsNullOrEmpty(lstExpiredDate))
            {
                //Update Drug Expired Date
                string[] detailIDLst = lstPrescriptionOrderDetailID.Split(',');
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
                            PrescriptionOrderDt orderDt = BusinessLayer.GetPrescriptionOrderDt(Convert.ToInt32(recordID));
                            PrescriptionOrderHd orderHd = BusinessLayer.GetPrescriptionOrderHd(Convert.ToInt32(hdnPrescriptionOrderID.Value));
                            if (orderDt != null)
                            {
                                orderDt.ExpiredDate = expiredDate;
                                orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                BusinessLayer.UpdatePrescriptionOrderDt(orderDt);
                            }
                            if (orderHd != null)
                            {
                                orderHd.GCPrescriptionType = cboPrescriptionType.Value.ToString();
                                orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                BusinessLayer.UpdatePrescriptionOrderHd(orderHd);
                            }
                        }
                        catch (FormatException)
                        {
                        }
                    }
                    index += 1;
                }
            }

            string result = "";
            if (hdnIsDotMatrix.Value != "1")
            {
                result = PrintDrugLabel(lstPrescriptionOrderDetailID);
            }
            else
            {
                result = PrintDrugLabelDotMatrix(lstPrescriptionOrderDetailID);
            }
            panel.JSProperties["cpZebraPrinting"] = result;
        }

        private string PrintDrugLabel(string lstPrescriptionOrderDetailID)
        {
            string result = string.Empty;
            try
            {
                hdnIsUsedDispenseQty.Value = Convert.ToString(chkisUsedDispenseQty.Checked);

                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}')",
                        AppSession.UserLogin.HealthcareID,
                        Constant.SettingParameter.FM_FORMAT_CETAKAN_ETIKET,
                        Constant.SettingParameter.FM_FORMAT_CETAKAN_ETIKET_RJ,
                        Constant.SettingParameter.FM_FORMAT_CETAKAN_ETIKET_BY_TYPE,
                        Constant.SettingParameter.FM_PRINT_ETIKET_DETAIL_OBAT_RACIKAN,
                        Constant.SettingParameter.SA_LOKASI_PRINTER_IP_ADDR
                    );
                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);

                string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.FM_FORMAT_CETAKAN_ETIKET)).FirstOrDefault().ParameterValue;
                string printFormatRJ = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.FM_FORMAT_CETAKAN_ETIKET_RJ)).FirstOrDefault().ParameterValue;

                Boolean isPrintByType = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.FM_FORMAT_CETAKAN_ETIKET_BY_TYPE)).FirstOrDefault().ParameterValue == "1" ? true : false;
                string SA_LOKASI_PRINTER_IP_ADDR = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.SA_LOKASI_PRINTER_IP_ADDR)).FirstOrDefault().ParameterValue;

                bool isBasedOnIPAddress = Convert.ToBoolean(Convert.ToInt16(SA_LOKASI_PRINTER_IP_ADDR));

                if (isBasedOnIPAddress)
                {
                    //Get Printer Address
                    string ipAddress = HttpContext.Current.Request.UserHostAddress;
                    int labelCount = Convert.ToInt16(txtJmlLabelCtl.Text);

                    filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}','{2}') AND IsDeleted=0",
                                                    ipAddress,
                                                    Constant.DirectPrintType.ETIKET_OBAT_DALAM,
                                                    Constant.DirectPrintType.ETIKET_OBAT_LUAR
                                                );
                    List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);
                    if (lstPrinter.Count > 0)
                    {
                        string printerUrlOD = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.ETIKET_OBAT_DALAM).FirstOrDefault().PrinterName;
                        string printerUrlOL = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.ETIKET_OBAT_LUAR).FirstOrDefault().PrinterName;
                        bool isUsePrintingTools1 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.ETIKET_OBAT_DALAM).FirstOrDefault().IsUsingPrintingTools;
                        bool isUsePrintingTools2 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.ETIKET_OBAT_LUAR).FirstOrDefault().IsUsingPrintingTools;

                        bool isPrintCompoundDetailLabel = false;
                        string printCompoundDetailLabel = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.FM_PRINT_ETIKET_DETAIL_OBAT_RACIKAN)).FirstOrDefault().ParameterValue;

                        PrescriptionOrderHd oHeader = BusinessLayer.GetPrescriptionOrderHd(Convert.ToInt32(hdnPrescriptionOrderID.Value));

                        if (printFormat == Constant.PrintFormat.BROTHER_ETIKET_RSSY)
                        {
                            BrotherPrinting.PrintDrugLabel_RSSY(
                                oHeader,
                                printFormat,
                                lstPrescriptionOrderDetailID,
                                printerUrlOD,
                                printerUrlOL,
                                isPrintByType,
                                hdnIsUsedDispenseQty.Value,
                                isPrintCompoundDetailLabel,
                                labelCount
                            );
                        }
                        else if (printFormat == Constant.PrintFormat.BIXOLON_ETIKET_RSPKSB)
                        {
                           if (printCompoundDetailLabel == "1")
                            {
                                isPrintCompoundDetailLabel = true;
                                ZebraPrinting.PrintDrugLabelBixolon(
                                    oHeader,
                                    printFormat,
                                    lstPrescriptionOrderDetailID,
                                    printerUrlOD,
                                    printerUrlOL,
                                    isPrintByType,
                                    printFormatRJ,
                                    hdnIsUsedDispenseQty.Value,
                                    isPrintCompoundDetailLabel,
                                    labelCount
                                );
                            }
                            else
                            {
                                ZebraPrinting.PrintDrugLabelBixolon(
                                    oHeader,
                                    printFormat,
                                    lstPrescriptionOrderDetailID,
                                    printerUrlOD,
                                    printerUrlOL,
                                    isPrintByType,
                                    printFormatRJ,
                                    hdnIsUsedDispenseQty.Value,
                                    isPrintCompoundDetailLabel,
                                    labelCount,
                                    "",
                                    "",
                                    isUsePrintingTools1,
                                    isUsePrintingTools2
                                );
                            }
                        }
                        else
                        {
                            if (printCompoundDetailLabel == "1")
                            {
                                isPrintCompoundDetailLabel = true;
                                ZebraPrinting.PrintDrugLabel(
                                    oHeader,
                                    printFormat,
                                    lstPrescriptionOrderDetailID,
                                    printerUrlOD,
                                    printerUrlOL,
                                    isPrintByType,
                                    printFormatRJ,
                                    hdnIsUsedDispenseQty.Value,
                                    isPrintCompoundDetailLabel,
                                    labelCount
                                );
                            }
                            else
                            {
                                ZebraPrinting.PrintDrugLabel(
                                    oHeader,
                                    printFormat,
                                    lstPrescriptionOrderDetailID,
                                    printerUrlOD,
                                    printerUrlOL,
                                    isPrintByType,
                                    printFormatRJ,
                                    hdnIsUsedDispenseQty.Value,
                                    isPrintCompoundDetailLabel,
                                    labelCount,
                                    "",
                                    "",
                                    isUsePrintingTools1,
                                    isUsePrintingTools2
                                );
                            }
                        }
                    }
                    else
                    {
                        result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                    }
                }
                else
                {
                    PrescriptionOrderHd oHeader = BusinessLayer.GetPrescriptionOrderHd(Convert.ToInt32(hdnPrescriptionOrderID.Value));
                    ZebraPrinting.PrintDrugLabel(oHeader, printFormat, lstPrescriptionOrderDetailID);
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        private string PrintDrugLabelDotMatrix(string lstPrescriptionOrderDetailID)
        {
            string result = string.Empty;
            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FM_FORMAT_CETAKAN_ETIKET);
                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.FM_FORMAT_CETAKAN_ETIKET)).FirstOrDefault().ParameterValue;

                bool isBasedOnIPAddress = AppSession.IsPrinterLocationBasedOnIP;

                if (isBasedOnIPAddress)
                {
                    //Get Printer Address
                    string ipAddress = HttpContext.Current.Request.UserHostAddress;

                    filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}','{2}') AND IsDeleted=0",
                        ipAddress, Constant.DirectPrintType.ETIKET_OBAT_DALAM, Constant.DirectPrintType.ETIKET_OBAT_LUAR);

                    List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);
                    if (lstPrinter.Count > 0)
                    {
                        string printerUrl1 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.ETIKET_OBAT_DALAM).FirstOrDefault().PrinterName;
                        string printerUrl2 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.ETIKET_OBAT_LUAR).FirstOrDefault().PrinterName;

                        PrescriptionOrderHd oHeader = BusinessLayer.GetPrescriptionOrderHd(Convert.ToInt32(hdnPrescriptionOrderID.Value));
                        //if (oHeader != null)
                        //{
                        //    oHeader.GCPrescriptionType = cboPrescriptionType.Value.ToString();
                        //    oHeader.LastUpdatedBy = AppSession.UserLogin.UserID;
                        //    BusinessLayer.UpdatePrescriptionOrderHd(oHeader);
                        //}

                        int labelCount = Convert.ToInt16(txtJmlLabelCtl.Text);
                        for (int i = 0; i < labelCount; i++)
                        {
                            ZebraPrinting.PrintDrugLabelDotMatrix(oHeader, lstPrescriptionOrderDetailID, printerUrl1, printerUrl2, cboPrescriptionType.Value.ToString());
                        }
                    }
                    else
                    {
                        result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                    }
                }
                else
                {
                    PrescriptionOrderHd oHeader = BusinessLayer.GetPrescriptionOrderHd(Convert.ToInt32(hdnPrescriptionOrderID.Value));
                    ZebraPrinting.PrintDrugLabel(oHeader, printFormat, lstPrescriptionOrderDetailID);
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
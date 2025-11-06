using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class PrintMedicationLabelCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnDispensaryServiceUnitID.Value = paramInfo[0];
            txtMedicationDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            chkIsUDDOnly.Checked = true;
            SetControlProperties();
        }

        protected void cbpPopupProcessPrint_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";

            string lstRecordID = hdnSelectedID.Value;
            string lstScheduleID = hdnSelectedScheduleID.Value;
            string lstExpiredDate = hdnSelectedExpiredDate.Value;
            string lstPrintNo = hdnSelectedPrintNo.Value;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;

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

                            PrescriptionOrderDt orderDt = BusinessLayer.GetPrescriptionOrderDt(Convert.ToInt32(recordID));
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

            result = PrintUDDDrugLabel(lstRecordID, lstScheduleID, lstExpiredDate, lstPrintNo);

            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = result;
        }

        private string PrintUDDDrugLabel(string lstRecordID, string lstScheduleID, string lstExpiredDate, string lstPrintNo)
        {
            string result = string.Empty;
            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}')",
                                                    AppSession.UserLogin.HealthcareID, 
                                                    Constant.SettingParameter.FM_FORMAT_CETAKAN_ETIKET, 
                                                    Constant.SettingParameter.PH_UDD_MEDICATION_RECEIPT,
                                                    Constant.SettingParameter.SA_LOKASI_PRINTER_IP_ADDR,
                                                    Constant.SettingParameter.FM_FORMAT_CETAKAN_LABEL_UDD
                                                );
                List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterExp);
                string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.FM_FORMAT_CETAKAN_ETIKET)).FirstOrDefault().ParameterValue;
                string printerType = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.PH_UDD_MEDICATION_RECEIPT)).FirstOrDefault().ParameterValue;
                string SA_LOKASI_PRINTER_IP_ADDR = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.SA_LOKASI_PRINTER_IP_ADDR)).FirstOrDefault().ParameterValue;
                string FM_FORMAT_CETAKAN_LABEL_UDD = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.FM_FORMAT_CETAKAN_LABEL_UDD)).FirstOrDefault().ParameterValue;
                bool isBasedOnIPAddress = Convert.ToBoolean(Convert.ToInt16(SA_LOKASI_PRINTER_IP_ADDR));
                ///AppSession.IsPrinterLocationBasedOnIP;
                
                if (isBasedOnIPAddress)
                {
                    //Get Printer Address
                    string ipAddress = HttpContext.Current.Request.UserHostAddress;

                    filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}','{2}','{3}') AND IsDeleted=0",
                                                ipAddress,
                                                Constant.DirectPrintType.LABEL_UDD,
                                                Constant.DirectPrintType.ETIKET_OBAT_DALAM,
                                                Constant.DirectPrintType.ETIKET_OBAT_LUAR
                                            );
                    List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);
                    if (lstPrinter.Count > 0)
                    {
                        string printerUrlLabelUDD = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.LABEL_UDD).FirstOrDefault().PrinterName;

                        string printerUrlOD = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.ETIKET_OBAT_DALAM).FirstOrDefault().PrinterName;
                        string printerUrlOL = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.ETIKET_OBAT_LUAR).FirstOrDefault().PrinterName;

                        bool isUsePrintingToolsOD = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.ETIKET_OBAT_DALAM).FirstOrDefault().IsUsingPrintingTools;
                        bool isUsePrintingToolsOL = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.ETIKET_OBAT_LUAR).FirstOrDefault().IsUsingPrintingTools;

                        string formatPrint = FM_FORMAT_CETAKAN_LABEL_UDD;

                        if (formatPrint == Constant.PrintFormat.BROTHER_ETIKET_UDD_RSSY)
                        {
                            BrotherPrinting.PrintDrugLabelUDD_RSSY(
                                formatPrint,
                                lstRecordID,
                                printerUrlOD,
                                printerUrlOL,
                                false,
                                "0",
                                false,
                                lstExpiredDate,
                                lstPrintNo
                            );
                        }
                        else if (formatPrint == Constant.PrintFormat.BIXOLON_ETIKET_UDD_RSPKSB)
                        {
                            if (chkIsPrintReceipt.Checked)
                            {
                                if (printerType == Constant.PrinterType.THERMAL_RECEIPT_PRINTER)
                                    ZebraPrinting.PrintUDDLabelWithTMPrinter(AppSession.RegisteredPatient.VisitID, Helper.GetDatePickerValue(txtMedicationDate.Text), cboSequence.Value.ToString(), lstScheduleID, string.Empty, printerUrlLabelUDD);
                                else
                                    ZebraPrinting.PrintUDDLabel(AppSession.RegisteredPatient.VisitID, Helper.GetDatePickerValue(txtMedicationDate.Text), cboSequence.Value.ToString(), lstScheduleID, printerUrlLabelUDD);
                            }

                            if (chkIsPrintDrugLabel.Checked)
                            {
                                ZebraPrinting.PrintUDDDrugLabelBixolon(AppSession.RegisteredPatient.VisitID, hdnTransactionNo.Value, hdnTransactionDate.Value, formatPrint, lstScheduleID, printerUrlOD, printerUrlOL, lstExpiredDate, lstPrintNo, isUsePrintingToolsOD, isUsePrintingToolsOL);
                            }
                        }
                        else
                        {
                            if (chkIsPrintReceipt.Checked)
                            {
                                if (printerType == Constant.PrinterType.THERMAL_RECEIPT_PRINTER)
                                    ZebraPrinting.PrintUDDLabelWithTMPrinter(AppSession.RegisteredPatient.VisitID, Helper.GetDatePickerValue(txtMedicationDate.Text), cboSequence.Value.ToString(), lstScheduleID, string.Empty, printerUrlLabelUDD);
                                else
                                    ZebraPrinting.PrintUDDLabel(AppSession.RegisteredPatient.VisitID, Helper.GetDatePickerValue(txtMedicationDate.Text), cboSequence.Value.ToString(), lstScheduleID, printerUrlLabelUDD);
                            }

                            if (chkIsPrintDrugLabel.Checked)
                            {
                                ZebraPrinting.PrintUDDDrugLabel(AppSession.RegisteredPatient.VisitID, hdnTransactionNo.Value, hdnTransactionDate.Value, formatPrint, lstScheduleID, printerUrlOD, printerUrlOL, lstExpiredDate, lstPrintNo, isUsePrintingToolsOD, isUsePrintingToolsOL);
                            }
                        }

                        result = string.Format("print|1|||");
                    }
                    else
                    {
                        string message = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                        result = string.Format("print|0|{0}||", message);
                    }
                }
            }
            catch (Exception ex)
            {
                result = string.Format("print|0|{0}||", ex.Message);
            }
            return result;
        }

        private void SetControlProperties()
        {
            List<StandardCode> lstSC = new List<StandardCode>();
            for (int a = 1; a <= 6; a++)
            {
                StandardCode sc = new StandardCode()
                {
                    StandardCodeID = a.ToString(),
                    StandardCodeName = a.ToString()
                };
                lstSC.Add(sc);
            }
            Methods.SetComboBoxField<StandardCode>(cboSequence, lstSC, "StandardCodeName", "StandardCodeID");
            cboSequence.SelectedIndex = 0;

            BindGridView();
        }

        protected void lvwPrintView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            //if (e.Item.ItemType == ListViewItemType.DataItem)
            //{
            //    CheckBox chkIsSelected = (CheckBox)e.Item.FindControl("chkIsProcessItem");
            //    //HtmlTextInput chkIsSelected = (CheckBox)e.Item.FindControl("chkIsProcessItem");
            //    vMedicationSchedule item = (vMedicationSchedule)e.Item.DataItem;
            //    if (item.GCMedicationStatus != Constant.MedicationStatus.OPEN)
            //    {
            //        chkIsSelected.Visible = false;
            //    }
            //}
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("VisitID = {0} AND MedicationDate = '{1}' AND GCMedicationStatus NOT IN ('{2}','{3}')", AppSession.RegisteredPatient.VisitID,
                Helper.GetDatePickerValue(txtMedicationDate.Text).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.MedicationStatus.OPEN, Constant.MedicationStatus.DISCONTINUE);
            if (cboSequence.Value != null)
            {
                filterExpression += string.Format(" AND SequenceNo = '{0}'", cboSequence.Value);
            }
            if (chkIsUDDOnly.Checked)
            {
                filterExpression += string.Format(" AND IsUsingUDD = 1");
            }

            filterExpression += string.Format(" AND IsDeleted = 0");
            List<vMedicationSchedule> lstEntity = BusinessLayer.GetvMedicationScheduleList(filterExpression);
            lvwPrintView.DataSource = lstEntity;
            lvwPrintView.DataBind();

            if (lstEntity.Count > 0)
            {
                string referenceNo = lstEntity[0].ReferenceNo;
                vPatientChargesHd1 chargesHd = BusinessLayer.GetvPatientChargesHd1List(string.Format("ReferenceNo = '{0}'", referenceNo)).FirstOrDefault();
                if (chargesHd != null)
                {
                    hdnReferenceNo.Value = referenceNo;
                    hdnTransactionNo.Value = chargesHd.TransactionNo;
                    hdnTransactionDate.Value = chargesHd.TransactionDate.ToString(Constant.FormatString.DATE_FORMAT);
                }
            }
        }

        protected void cbpPrintView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }
    }
}
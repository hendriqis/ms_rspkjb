using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Nutrition.Program
{
    public partial class NutritionDistribution : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Nutrition.NUTRITION_DISTRIBUTION;

        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected String GetItemDetailStatus()
        {
            return Constant.TransactionStatus.PROCESSED;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowEdit = false;
            base.SetCRUDMode(ref IsAllowAdd, ref IsAllowEdit, ref IsAllowDelete);
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.MEAL_TIME));
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboMealTime, lstStandardCode, "StandardCodeName", "StandardCodeID");
            cboMealTime.SelectedIndex = 0;

            string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.NT_ORDER_MEAL_NOT_ONLY_INPATIENT);
            vSettingParameterDt lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp).FirstOrDefault();
            List<GetServiceUnitUserList> lstServiceUnit = new List<GetServiceUnitUserList>();
            if (lstParam.ParameterValue == "1")
            {
                lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Empty, string.Format("DepartmentID IN ('{0}','{1}','{2}')", Constant.Facility.INPATIENT, Constant.Facility.OUTPATIENT, Constant.Facility.EMERGENCY));
            }
            else 
            {
                lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.Facility.INPATIENT, "");
            }
            lstServiceUnit.Insert(0, new GetServiceUnitUserList { ServiceUnitName = "", HealthcareServiceUnitID = 0 });
            Methods.SetComboBoxField<GetServiceUnitUserList>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
            cboServiceUnit.SelectedIndex = 0;

            txtDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtExpiredDate.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            BindGridView();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = GetFilterExpression();
            List<vNutritionOrderDtCustom> lstEntity = BusinessLayer.GetvNutritionOrderDtCustomList(filterExpression);
            hdnLstNutritionOrderDtID.Value = String.Join(",", lstEntity.Select(p => p.NutritionOrderDtID).ToList());
            hdnLstNutritionOrderHdID.Value = String.Join(",", lstEntity.Select(p => p.NutritionOrderHdID).ToList());
            hdnNutritionOrderDtID.Value = String.Join(",", lstEntity.Select(p => p.NutritionOrderDtID).ToList());
            hdnNutritionOrderHdID.Value = String.Join(",", lstEntity.Select(p => p.NutritionOrderHdID).ToList());
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        private void PrintSlipAll()
        {
            string filterExpression = String.Format("ScheduleDate = '{0}' AND GCTransactionStatus = '{1}'", DateTime.Now.ToString("yyyyMMdd"), Constant.TransactionStatus.CLOSED);
            if (Convert.ToInt32(cboServiceUnit.Value) != 0) filterExpression += String.Format(" AND HealthcareServiceUnitID = {0}", cboServiceUnit.Value);
            if (cboMealTime.Value != null && cboMealTime.Value.ToString() != "") filterExpression += String.Format(" AND GCMealTime = '{0}'", cboMealTime.Value);
            List<vNutritionOrderDtCustom> lstEntity = BusinessLayer.GetvNutritionOrderDtCustomList(filterExpression);
            foreach (vNutritionOrderDtCustom obj in lstEntity)
            {
                ZebraPrinting.PrintMealLabel(obj);
            }
        }

        private string PrintSlip(string orderID, string lstNutritionOrderDtID)
        {
            string result = string.Empty;
            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.NT_JENIS_PRINTER_ETIKET_GIZI, Constant.SettingParameter.NT_FORMAT_CETAKAN_ETIKET_GIZI);
                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                bool isBasedOnIPAddress = AppSession.IsPrinterLocationBasedOnIP;
                Healthcare entityHSU = BusinessLayer.GetHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID)).FirstOrDefault();

                if (entityHSU.Initial == "rsdo-soba")
                {
                    if (lstParam != null)
                    {
                        string printerType = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.NT_JENIS_PRINTER_ETIKET_GIZI)).FirstOrDefault().ParameterValue;
                        string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.NT_FORMAT_CETAKAN_ETIKET_GIZI)).FirstOrDefault().ParameterValue;

                        string ipAddress = HttpContext.Current.Request.UserHostAddress;
                        string filter = string.Empty;
                        filter = string.Format("NutritionOrderHdID IN ({0}) AND NutritionOrderDtID = {1} AND GCItemDetailStatus IN ('{2}', '{3}') AND StapleFood IS NOT NULL ORDER BY BedCode", orderID, lstNutritionOrderDtID,
                            Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED);
                        vNutritionOrderDtCustom entity = BusinessLayer.GetvNutritionOrderDtCustomList(filter).FirstOrDefault();

                        if (entity != null)
                        {
                            if (printerType == Constant.PrinterType.ZEBRA_PRINTER)
                            {
                                filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                        ipAddress, Constant.DirectPrintType.LABEL_DISTRIBUSI_GIZI);
                                List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);


                                if (lstPrinter.Count > 0)
                                {
                                    string printerUrl1 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.LABEL_DISTRIBUSI_GIZI).FirstOrDefault().PrinterName;
                                    ZebraPrinting.PrintNutritionLabel(printFormat, lstNutritionOrderDtID, printerUrl1);
                                }
                                else
                                {
                                    result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                }
                            }
                            else
                            {
                                filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                        ipAddress, Constant.DirectPrintType.GIZI_DISTRIBUTION_LABEL);
                                List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);

                                if (lstPrinter.Count > 0)
                                {
                                    string printerUrl1 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.GIZI_DISTRIBUTION_LABEL).FirstOrDefault().PrinterName;
                                    ZebraPrinting.PrintNutritionDistributionLabel1(Convert.ToInt32(orderID), Convert.ToInt32(lstNutritionOrderDtID), lstPrinter[0].PrinterName, txtExpiredDate.Text);
                                }
                                else
                                {
                                    result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                }
                            }
                        }
                    }
                    else
                    {
                        result = string.Format("Printer Configuration is not available");
                    }
                }
                else if (entityHSU.Initial == "RSSEBK")
                {
                    if (lstParam != null)
                    {
                        string printerType = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.NT_JENIS_PRINTER_ETIKET_GIZI)).FirstOrDefault().ParameterValue;
                        string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.NT_FORMAT_CETAKAN_ETIKET_GIZI)).FirstOrDefault().ParameterValue;

                        string ipAddress = HttpContext.Current.Request.UserHostAddress;
                        string filter = string.Empty;
                        filter = string.Format("NutritionOrderHdID IN ({0}) AND NutritionOrderDtID = {1} AND GCItemDetailStatus IN ('{2}', '{3}','{4}','{5}') AND StapleFood IS NOT NULL ORDER BY BedCode", orderID, lstNutritionOrderDtID,
                            Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED);
                        vNutritionOrderDtCustom entity = BusinessLayer.GetvNutritionOrderDtCustomList(filter).FirstOrDefault();

                        if (entity != null)
                        {
                            filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                    ipAddress, Constant.DirectPrintType.GIZI_DISTRIBUTION_LABEL);
                            List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);

                            if (lstPrinter.Count > 0)
                            {
                                string printerUrl1 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.GIZI_DISTRIBUTION_LABEL).FirstOrDefault().PrinterName;
                                ZebraPrinting.PrintNutritionDistributionLabelRSSEBK(Convert.ToInt32(orderID), Convert.ToInt32(lstNutritionOrderDtID), lstPrinter[0].PrinterName, txtExpiredDate.Text);
                            }
                            else
                            {
                                result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                            }
                        }
                    }
                    else
                    {
                        result = string.Format("Printer Configuration is not available");
                    }
                }
                else if (entityHSU.Initial == "RSSEB")
                {
                    if (lstParam != null)
                    {
                        string printerType = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.NT_JENIS_PRINTER_ETIKET_GIZI)).FirstOrDefault().ParameterValue;
                        string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.NT_FORMAT_CETAKAN_ETIKET_GIZI)).FirstOrDefault().ParameterValue;

                        string ipAddress = HttpContext.Current.Request.UserHostAddress;
                        string filter = string.Empty;
                        filter = string.Format("NutritionOrderHdID IN ({0}) AND NutritionOrderDtID = {1} AND GCItemDetailStatus IN ('{2}', '{3}','{4}','{5}') AND StapleFood IS NOT NULL ORDER BY BedCode", orderID, lstNutritionOrderDtID,
                            Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED);
                        vNutritionOrderDtCustom entity = BusinessLayer.GetvNutritionOrderDtCustomList(filter).FirstOrDefault();

                        if (entity != null)
                        {
                            filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                    ipAddress, Constant.DirectPrintType.GIZI_DISTRIBUTION_LABEL);
                            List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);

                            if (lstPrinter.Count > 0)
                            {
                                string printerUrl1 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.GIZI_DISTRIBUTION_LABEL).FirstOrDefault().PrinterName;
                                ZebraPrinting.PrintNutritionDistributionLabelRSSEBK(Convert.ToInt32(orderID), Convert.ToInt32(lstNutritionOrderDtID), lstPrinter[0].PrinterName, txtExpiredDate.Text);
                            }
                            else
                            {
                                result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                            }
                        }
                    }
                    else
                    {
                        result = string.Format("Printer Configuration is not available");
                    }
                }
                else if (entityHSU.Initial == "RSSEBS")
                {
                    if (lstParam != null)
                    {
                        string printerType = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.NT_JENIS_PRINTER_ETIKET_GIZI)).FirstOrDefault().ParameterValue;
                        string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.NT_FORMAT_CETAKAN_ETIKET_GIZI)).FirstOrDefault().ParameterValue;

                        string ipAddress = HttpContext.Current.Request.UserHostAddress;
                        string filter = string.Empty;
                        filter = string.Format("NutritionOrderHdID IN ({0}) AND NutritionOrderDtID = {1} AND GCItemDetailStatus IN ('{2}', '{3}','{4}','{5}') AND StapleFood IS NOT NULL ORDER BY BedCode", orderID, lstNutritionOrderDtID,
                            Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED);
                        vNutritionOrderDtCustom entity = BusinessLayer.GetvNutritionOrderDtCustomList(filter).FirstOrDefault();

                        if (entity != null)
                        {
                            filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                    ipAddress, Constant.DirectPrintType.GIZI_DISTRIBUTION_LABEL);
                            List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);

                            if (lstPrinter.Count > 0)
                            {
                                string printerUrl1 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.GIZI_DISTRIBUTION_LABEL).FirstOrDefault().PrinterName;
                                ZebraPrinting.PrintNutritionDistributionLabelRSSEBK(Convert.ToInt32(orderID), Convert.ToInt32(lstNutritionOrderDtID), lstPrinter[0].PrinterName, txtExpiredDate.Text);
                            }
                            else
                            {
                                result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                            }
                        }
                    }
                    else
                    {
                        result = string.Format("Printer Configuration is not available");
                    }
                }
                else if (entityHSU.Initial == "RSRT")
                {
                    if (lstParam != null)
                    {
                        string printerType = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.NT_JENIS_PRINTER_ETIKET_GIZI)).FirstOrDefault().ParameterValue;
                        string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.NT_FORMAT_CETAKAN_ETIKET_GIZI)).FirstOrDefault().ParameterValue;

                        string ipAddress = HttpContext.Current.Request.UserHostAddress;
                        string filter = string.Empty;
                        filter = string.Format("NutritionOrderHdID IN ({0}) AND NutritionOrderDtID = {1} AND GCItemDetailStatus IN ('{2}', '{3}','{4}','{5}') AND StapleFood IS NOT NULL ORDER BY BedCode", orderID, lstNutritionOrderDtID,
                            Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED);
                        vNutritionOrderDtCustom entity = BusinessLayer.GetvNutritionOrderDtCustomList(filter).FirstOrDefault();

                        if (entity != null)
                        {
                            if (printerType == Constant.PrinterType.ZEBRA_PRINTER)
                            {
                                filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                        ipAddress, Constant.DirectPrintType.LABEL_DISTRIBUSI_GIZI);
                                List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);


                                if (lstPrinter.Count > 0)
                                {
                                    string printerUrl1 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.LABEL_DISTRIBUSI_GIZI).FirstOrDefault().PrinterName;
                                    ZebraPrinting.PrintNutritionLabel(printFormat, lstNutritionOrderDtID, printerUrl1);
                                }
                                else
                                {
                                    result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                }
                            }
                            else if (printerType == Constant.PrinterType.EPSON_DOT_MATRIX)
                            {
                                filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                        ipAddress, Constant.DirectPrintType.LABEL_DISTRIBUSI_GIZI);
                                List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);

                                if (lstPrinter.Count > 0)
                                {
                                    string printerUrl1 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.LABEL_DISTRIBUSI_GIZI).FirstOrDefault().PrinterName;
                                    printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.NT_FORMAT_CETAKAN_ETIKET_GIZI)).FirstOrDefault().ParameterValue;

                                    switch (printFormat)
                                    {
                                        case Constant.Format_Label_Gizi.RSSES:
                                            ZebraPrinting.PrintNutritionLabelDotMatrixRSSES(lstNutritionOrderDtID, printerUrl1);
                                            break;
                                        default:
                                            ZebraPrinting.PrintNutritionLabelDotMatrix(lstNutritionOrderDtID, printerUrl1);
                                            break;
                                    }
                                }
                                else
                                {
                                    result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                }
                            }
                            else
                            {
                                filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                        ipAddress, Constant.DirectPrintType.GIZI_DISTRIBUTION_LABEL);
                                List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);

                                if (lstPrinter.Count > 0)
                                {
                                    string printerUrl1 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.GIZI_DISTRIBUTION_LABEL).FirstOrDefault().PrinterName;
                                    printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.NT_FORMAT_CETAKAN_ETIKET_GIZI)).FirstOrDefault().ParameterValue;

                                    switch (printFormat)
                                    {
                                        case Constant.Format_Label_Gizi.RSSES:
                                            ZebraPrinting.PrintNutritionLabelDotMatrixRSSES(lstNutritionOrderDtID, printerUrl1);
                                            break;
                                        default:
                                            ZebraPrinting.PrintNutritionDistributionLabelRSRT(Convert.ToInt32(orderID), Convert.ToInt32(lstNutritionOrderDtID), lstPrinter[0].PrinterName, txtExpiredDate.Text);
                                            break;
                                    }
                                }
                                else
                                {
                                    result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                }
                            }
                        }
                    }
                    else
                    {
                        result = string.Format("Printer Configuration is not available");
                    }
                }
                else
                {
                    if (lstParam != null)
                    {
                        string printerType = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.NT_JENIS_PRINTER_ETIKET_GIZI)).FirstOrDefault().ParameterValue;
                        string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.NT_FORMAT_CETAKAN_ETIKET_GIZI)).FirstOrDefault().ParameterValue;

                        string ipAddress = HttpContext.Current.Request.UserHostAddress;
                        string filter = string.Empty;
                        filter = string.Format("NutritionOrderHdID IN ({0}) AND NutritionOrderDtID = {1} AND GCItemDetailStatus IN ('{2}', '{3}','{4}','{5}') AND StapleFood IS NOT NULL ORDER BY BedCode", orderID, lstNutritionOrderDtID,
                            Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED);
                        vNutritionOrderDtCustom entity = BusinessLayer.GetvNutritionOrderDtCustomList(filter).FirstOrDefault();

                        if (entity != null)
                        {
                            if (printerType == Constant.PrinterType.ZEBRA_PRINTER)
                            {
                                filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                        ipAddress, Constant.DirectPrintType.LABEL_DISTRIBUSI_GIZI);
                                List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);


                                if (lstPrinter.Count > 0)
                                {
                                    if (printFormat == Constant.PrintFormat.BIXOLON_ETIKET_RSPKSB)
                                    {
                                        string printerUrl1 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.LABEL_DISTRIBUSI_GIZI).FirstOrDefault().PrinterName;
                                        ZebraPrinting.PrintNutritionLabelBixolon(printFormat, lstNutritionOrderDtID, printerUrl1);
                                    }
                                    else
                                    {
                                        string printerUrl1 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.LABEL_DISTRIBUSI_GIZI).FirstOrDefault().PrinterName;
                                        ZebraPrinting.PrintNutritionLabel(printFormat, lstNutritionOrderDtID, printerUrl1);
                                    }
                                }
                                else
                                {
                                    result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                }
                            }
                            else if (printerType == Constant.PrinterType.EPSON_DOT_MATRIX)
                            {
                                filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                        ipAddress, Constant.DirectPrintType.LABEL_DISTRIBUSI_GIZI);
                                List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);

                                if (lstPrinter.Count > 0)
                                {
                                    string printerUrl1 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.LABEL_DISTRIBUSI_GIZI).FirstOrDefault().PrinterName;
                                    printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.NT_FORMAT_CETAKAN_ETIKET_GIZI)).FirstOrDefault().ParameterValue;

                                    switch (printFormat)
                                    {
                                        case Constant.Format_Label_Gizi.RSSES:
                                            ZebraPrinting.PrintNutritionLabelDotMatrixRSSES(lstNutritionOrderDtID, printerUrl1);
                                            break;
                                        default:
                                            ZebraPrinting.PrintNutritionLabelDotMatrix(lstNutritionOrderDtID, printerUrl1);
                                            break;
                                    }
                                }
                                else
                                {
                                    result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                }
                            }
                            else if (printerType == Constant.PrinterType.BROTHER_PRINTER)
                            {
                                filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                    ipAddress, Constant.DirectPrintType.LABEL_DISTRIBUSI_GIZI);
                                List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);

                                if (lstPrinter.Count > 0)
                                {
                                    string printerUrl1 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.LABEL_DISTRIBUSI_GIZI).FirstOrDefault().PrinterName;
                                    BrotherPrinting.PrintLabelDistribusiGizi_RSSY(lstNutritionOrderDtID, printFormat, printerUrl1);
                                }
                                else
                                {
                                    result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                }
                            }
                            else
                            {
                                filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                        ipAddress, Constant.DirectPrintType.GIZI_DISTRIBUTION_LABEL);
                                List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);

                                if (lstPrinter.Count > 0)
                                {
                                    string printerUrl1 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.GIZI_DISTRIBUTION_LABEL).FirstOrDefault().PrinterName;
                                    printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.NT_FORMAT_CETAKAN_ETIKET_GIZI)).FirstOrDefault().ParameterValue;

                                    switch (printFormat)
                                    {
                                        case Constant.Format_Label_Gizi.RSSES:
                                            ZebraPrinting.PrintNutritionLabelDotMatrixRSSES(lstNutritionOrderDtID, printerUrl1);
                                            break;
                                        default:
                                            ZebraPrinting.PrintNutritionDistributionLabel1(Convert.ToInt32(orderID), Convert.ToInt32(lstNutritionOrderDtID), lstPrinter[0].PrinterName, txtExpiredDate.Text);
                                            break;
                                    }
                                }
                                else
                                {
                                    result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                }
                            }
                        }
                    }
                    else
                    {
                        result = string.Format("Printer Configuration is not available");
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;     
        }

        private string PrintAllNutritionDistributionLabel(string lstNutritionOrderDtID)
        {
            string filterExpression = string.Empty;
            string filterExpression1 = string.Empty;
            string result = string.Empty;
            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.NT_JENIS_PRINTER_ETIKET_GIZI, Constant.SettingParameter.NT_FORMAT_CETAKAN_ETIKET_GIZI);
                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                bool isBasedOnIPAddress = AppSession.IsPrinterLocationBasedOnIP;

                if (lstParam != null)
                {
                    string printerType = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.NT_JENIS_PRINTER_ETIKET_GIZI)).FirstOrDefault().ParameterValue;
                    string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.NT_FORMAT_CETAKAN_ETIKET_GIZI)).FirstOrDefault().ParameterValue;

                    Healthcare entityHSU = BusinessLayer.GetHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID)).FirstOrDefault();


                    if (entityHSU.Initial == "rsdo-soba") //ini kan label buat soba
                    {
                        if (!string.IsNullOrEmpty(lstNutritionOrderDtID))
                        {
                            filterExpression = String.Format("NutritionOrderDtID IN ({0}) AND ScheduleDate = '{1}' AND GCItemDetailStatus IN ('{2}','{3}') AND StapleFood IS NOT NULL AND StapleFood != ''", lstNutritionOrderDtID, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112), Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED);
                        }
                        else
                        {
                            filterExpression = String.Format("ScheduleDate = '{0}' AND GCItemDetailStatus IN ('{1}','{2}') AND StapleFood IS NOT NULL AND StapleFood != ''", DateTime.Now.ToString("yyyyMMdd"), Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED);
                        }

                        if (Convert.ToInt32(cboServiceUnit.Value) != 0) filterExpression += String.Format(" AND HealthcareServiceUnitID = {0}", cboServiceUnit.Value);
                        if (cboMealTime.Value != null && cboMealTime.Value.ToString() != "") filterExpression += String.Format(" AND GCMealTime = '{0}'", cboMealTime.Value);
                        filterExpression += " ORDER BY BedCode";

                        vNutritionOrderDtCustom entity = BusinessLayer.GetvNutritionOrderDtCustomList(filterExpression).FirstOrDefault();

                        List<vNutritionOrderDtCustom> lstEntity = BusinessLayer.GetvNutritionOrderDtCustomList(filterExpression);

                        string lstNutritionOrderID = string.Empty;

                        foreach (vNutritionOrderDtCustom b in lstEntity)
                        {
                            lstNutritionOrderID += b.NutritionOrderDtID + ",";
 
                        }
                        lstNutritionOrderID = lstNutritionOrderID.Remove(lstNutritionOrderID.Length - 1, 1);


                        string ipAddress = HttpContext.Current.Request.UserHostAddress;

                        if (entity != null)
                        {
                            if (printerType == Constant.PrinterType.ZEBRA_PRINTER)
                            {
                                filterExpression1 = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                        ipAddress, Constant.DirectPrintType.LABEL_DISTRIBUSI_GIZI);
                                List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExpression1);


                                if (lstPrinter.Count > 0)
                                {
                                    string printerUrl1 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.LABEL_DISTRIBUSI_GIZI).FirstOrDefault().PrinterName;
                                    ZebraPrinting.PrintNutritionLabel(printFormat, lstNutritionOrderID, printerUrl1);
                                }
                                else
                                {
                                    result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                }
                            }
                            else
                            {
                                filterExpression1 = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                        ipAddress, Constant.DirectPrintType.GIZI_DISTRIBUTION_LABEL);
                                List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExpression1);

                                if (lstPrinter.Count > 0)
                                {
                                    string printerUrl1 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.GIZI_DISTRIBUTION_LABEL).FirstOrDefault().PrinterName;
                                    foreach (vNutritionOrderDtCustom obj in lstEntity)
                                    {
                                        ZebraPrinting.PrintNutritionDistributionLabel1(obj.NutritionOrderHdID, obj.NutritionOrderDtID, lstPrinter[0].PrinterName, txtExpiredDate.Text);
                                    }
                                }
                                else
                                {
                                    result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                }
                            }
                        }
                    }
                    else if (entityHSU.Initial == "RSSEBK") //ini kan label buat RSSBEK
                    {
                        if (!string.IsNullOrEmpty(lstNutritionOrderDtID))
                        {
                            filterExpression = String.Format("NutritionOrderDtID IN ({0}) AND ScheduleDate = '{1}' AND GCItemDetailStatus IN ('{2}','{3}','{4}','{5}') AND StapleFood IS NOT NULL", lstNutritionOrderDtID, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112), Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED);
                        }
                        else
                        {
                            filterExpression = String.Format("ScheduleDate = '{0}' AND GCItemDetailStatus IN ('{1}','{2}','{3}','{4}') AND StapleFood IS NOT NULL", DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112), Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED);
                        }

                        if (Convert.ToInt32(cboServiceUnit.Value) != 0) filterExpression += String.Format(" AND HealthcareServiceUnitID = {0}", cboServiceUnit.Value);
                        if (cboMealTime.Value != null && cboMealTime.Value.ToString() != "") filterExpression += String.Format(" AND GCMealTime = '{0}'", cboMealTime.Value);
                        filterExpression += " ORDER BY BedCode";

                        vNutritionOrderDtCustom entity = BusinessLayer.GetvNutritionOrderDtCustomList(filterExpression).FirstOrDefault();

                        List<vNutritionOrderDtCustom> lstEntity = BusinessLayer.GetvNutritionOrderDtCustomList(filterExpression);

                        string lstNutritionOrderID = string.Empty;

                        foreach (vNutritionOrderDtCustom b in lstEntity)
                        {
                            lstNutritionOrderID += b.NutritionOrderDtID + ",";

                        }
                        lstNutritionOrderID = lstNutritionOrderID.Remove(lstNutritionOrderID.Length - 1, 1);


                        string ipAddress = HttpContext.Current.Request.UserHostAddress;

                        if (entity != null)
                        {
                            filterExpression1 = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                    ipAddress, Constant.DirectPrintType.GIZI_DISTRIBUTION_LABEL);
                            List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExpression1);

                            if (lstPrinter.Count > 0)
                            {
                                string printerUrl1 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.GIZI_DISTRIBUTION_LABEL).FirstOrDefault().PrinterName;
                                foreach (vNutritionOrderDtCustom obj in lstEntity)
                                {
                                    ZebraPrinting.PrintNutritionDistributionLabelRSSEBK(obj.NutritionOrderHdID, obj.NutritionOrderDtID, lstPrinter[0].PrinterName, txtExpiredDate.Text);
                                }
                            }
                            else
                            {
                                result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                            }
                        }
                    }
                    else if (entityHSU.Initial == "RSSEB")
                    {
                        if (!string.IsNullOrEmpty(lstNutritionOrderDtID))
                        {
                            filterExpression = String.Format("NutritionOrderDtID IN ({0}) AND ScheduleDate = '{1}' AND GCItemDetailStatus IN ('{2}','{3}','{4}','{5}') AND StapleFood IS NOT NULL", lstNutritionOrderDtID, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112), Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED);
                        }
                        else
                        {
                            filterExpression = String.Format("ScheduleDate = '{0}' AND GCItemDetailStatus IN ('{1}','{2}','{3}','{4}') AND StapleFood IS NOT NULL", DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112), Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED);
                        }

                        if (Convert.ToInt32(cboServiceUnit.Value) != 0) filterExpression += String.Format(" AND HealthcareServiceUnitID = {0}", cboServiceUnit.Value);
                        if (cboMealTime.Value != null && cboMealTime.Value.ToString() != "") filterExpression += String.Format(" AND GCMealTime = '{0}'", cboMealTime.Value);
                        filterExpression += " ORDER BY BedCode";

                        vNutritionOrderDtCustom entity = BusinessLayer.GetvNutritionOrderDtCustomList(filterExpression).FirstOrDefault();

                        List<vNutritionOrderDtCustom> lstEntity = BusinessLayer.GetvNutritionOrderDtCustomList(filterExpression);

                        string lstNutritionOrderID = string.Empty;

                        foreach (vNutritionOrderDtCustom b in lstEntity)
                        {
                            lstNutritionOrderID += b.NutritionOrderDtID + ",";

                        }
                        lstNutritionOrderID = lstNutritionOrderID.Remove(lstNutritionOrderID.Length - 1, 1);


                        string ipAddress = HttpContext.Current.Request.UserHostAddress;

                        if (entity != null)
                        {
                            filterExpression1 = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                    ipAddress, Constant.DirectPrintType.GIZI_DISTRIBUTION_LABEL);
                            List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExpression1);

                            if (lstPrinter.Count > 0)
                            {
                                string printerUrl1 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.GIZI_DISTRIBUTION_LABEL).FirstOrDefault().PrinterName;
                                foreach (vNutritionOrderDtCustom obj in lstEntity)
                                {
                                    ZebraPrinting.PrintNutritionDistributionLabelRSSEBK(obj.NutritionOrderHdID, obj.NutritionOrderDtID, lstPrinter[0].PrinterName, txtExpiredDate.Text);
                                }
                            }
                            else
                            {
                                result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                            }
                        }
                    }
                    else if (entityHSU.Initial == "RSSEBS")
                    {
                        if (!string.IsNullOrEmpty(lstNutritionOrderDtID))
                        {
                            filterExpression = String.Format("NutritionOrderDtID IN ({0}) AND ScheduleDate = '{1}' AND GCItemDetailStatus IN ('{2}','{3}','{4}','{5}') AND StapleFood IS NOT NULL", lstNutritionOrderDtID, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112), Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED);
                        }
                        else
                        {
                            filterExpression = String.Format("ScheduleDate = '{0}' AND GCItemDetailStatus IN ('{1}','{2}','{3}','{4}') AND StapleFood IS NOT NULL", DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112), Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED);
                        }

                        if (Convert.ToInt32(cboServiceUnit.Value) != 0) filterExpression += String.Format(" AND HealthcareServiceUnitID = {0}", cboServiceUnit.Value);
                        if (cboMealTime.Value != null && cboMealTime.Value.ToString() != "") filterExpression += String.Format(" AND GCMealTime = '{0}'", cboMealTime.Value);
                        filterExpression += " ORDER BY BedCode";

                        vNutritionOrderDtCustom entity = BusinessLayer.GetvNutritionOrderDtCustomList(filterExpression).FirstOrDefault();

                        List<vNutritionOrderDtCustom> lstEntity = BusinessLayer.GetvNutritionOrderDtCustomList(filterExpression);

                        string lstNutritionOrderID = string.Empty;

                        foreach (vNutritionOrderDtCustom b in lstEntity)
                        {
                            lstNutritionOrderID += b.NutritionOrderDtID + ",";

                        }
                        lstNutritionOrderID = lstNutritionOrderID.Remove(lstNutritionOrderID.Length - 1, 1);


                        string ipAddress = HttpContext.Current.Request.UserHostAddress;

                        if (entity != null)
                        {
                            filterExpression1 = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                    ipAddress, Constant.DirectPrintType.GIZI_DISTRIBUTION_LABEL);
                            List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExpression1);

                            if (lstPrinter.Count > 0)
                            {
                                string printerUrl1 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.GIZI_DISTRIBUTION_LABEL).FirstOrDefault().PrinterName;
                                foreach (vNutritionOrderDtCustom obj in lstEntity)
                                {
                                    ZebraPrinting.PrintNutritionDistributionLabelRSSEBK(obj.NutritionOrderHdID, obj.NutritionOrderDtID, lstPrinter[0].PrinterName, txtExpiredDate.Text);
                                }
                            }
                            else
                            {
                                result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                            }
                        }
                    }
                    if (entityHSU.Initial == "RSRT")
                    {
                        if (!string.IsNullOrEmpty(lstNutritionOrderDtID))
                        {
                            filterExpression = String.Format("NutritionOrderDtID IN ({0}) AND ScheduleDate = '{1}' AND GCItemDetailStatus IN ('{2}','{3}','{4}','{5}') AND StapleFood IS NOT NULL", lstNutritionOrderDtID, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112), Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED);
                        }
                        else
                        {
                            filterExpression = String.Format("ScheduleDate = '{0}' AND GCItemDetailStatus IN ('{1}','{2}','{3}','{4}') AND StapleFood IS NOT NULL", DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112), Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED);
                        }

                        if (Convert.ToInt32(cboServiceUnit.Value) != 0) filterExpression += String.Format(" AND HealthcareServiceUnitID = {0}", cboServiceUnit.Value);
                        if (cboMealTime.Value != null && cboMealTime.Value.ToString() != "") filterExpression += String.Format(" AND GCMealTime = '{0}'", cboMealTime.Value);
                        filterExpression += " ORDER BY BedCode";

                        vNutritionOrderDtCustom entity = BusinessLayer.GetvNutritionOrderDtCustomList(filterExpression).FirstOrDefault();

                        List<vNutritionOrderDtCustom> lstEntity = BusinessLayer.GetvNutritionOrderDtCustomList(filterExpression);

                        string ipAddress = HttpContext.Current.Request.UserHostAddress;

                        if (entity != null)
                        {
                            if (printerType == Constant.PrinterType.ZEBRA_PRINTER)
                            {
                                filterExpression1 = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                        ipAddress, Constant.DirectPrintType.LABEL_DISTRIBUSI_GIZI);
                                List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExpression1);


                                if (lstPrinter.Count > 0)
                                {
                                    string printerUrl1 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.LABEL_DISTRIBUSI_GIZI).FirstOrDefault().PrinterName;
                                    ZebraPrinting.PrintNutritionLabel(printFormat, lstNutritionOrderDtID, printerUrl1);
                                }
                                else
                                {
                                    result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                }
                            }
                            else if (printerType == Constant.PrinterType.EPSON_DOT_MATRIX)
                            {
                                filterExpression1 = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                    ipAddress, Constant.DirectPrintType.LABEL_DISTRIBUSI_GIZI);
                                List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExpression1);

                                if (lstPrinter.Count > 0)
                                {
                                    string printerUrl1 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.LABEL_DISTRIBUSI_GIZI).FirstOrDefault().PrinterName;
                                    ZebraPrinting.PrintNutritionLabelDotMatrix(lstNutritionOrderDtID,
                                    printerUrl1);
                                }
                                else
                                {
                                    result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                }
                            }
                            else
                            {
                                filterExpression1 = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                        ipAddress, Constant.DirectPrintType.GIZI_DISTRIBUTION_LABEL);
                                List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExpression1);

                                if (lstPrinter.Count > 0)
                                {
                                    string printerUrl1 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.GIZI_DISTRIBUTION_LABEL).FirstOrDefault().PrinterName;
                                    foreach (vNutritionOrderDtCustom obj in lstEntity)
                                    {
                                        ZebraPrinting.PrintNutritionDistributionLabelRSRT(obj.NutritionOrderHdID, obj.NutritionOrderDtID, lstPrinter[0].PrinterName, txtExpiredDate.Text);
                                    }
                                }
                                else
                                {
                                    result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                }
                            }
                        }

                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(lstNutritionOrderDtID))
                        {
                            filterExpression = String.Format("NutritionOrderDtID IN ({0}) AND ScheduleDate = '{1}' AND GCItemDetailStatus IN ('{2}','{3}','{4}','{5}') AND StapleFood IS NOT NULL", lstNutritionOrderDtID, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112), Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED);
                        }
                        else
                        {
                            filterExpression = String.Format("ScheduleDate = '{0}' AND GCItemDetailStatus IN ('{1}','{2}','{3}','{4}') AND StapleFood IS NOT NULL", DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112), Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED);
                        }

                        if (Convert.ToInt32(cboServiceUnit.Value) != 0) filterExpression += String.Format(" AND HealthcareServiceUnitID = {0}", cboServiceUnit.Value);
                        if (cboMealTime.Value != null && cboMealTime.Value.ToString() != "") filterExpression += String.Format(" AND GCMealTime = '{0}'", cboMealTime.Value);
                        filterExpression += " ORDER BY BedCode";

                        vNutritionOrderDtCustom entity = BusinessLayer.GetvNutritionOrderDtCustomList(filterExpression).FirstOrDefault();

                        List<vNutritionOrderDtCustom> lstEntity = BusinessLayer.GetvNutritionOrderDtCustomList(filterExpression);

                        string ipAddress = HttpContext.Current.Request.UserHostAddress;

                        if (entity != null)
                        {
                            if (printerType == Constant.PrinterType.ZEBRA_PRINTER)
                            {
                                filterExpression1 = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                        ipAddress, Constant.DirectPrintType.LABEL_DISTRIBUSI_GIZI);
                                List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExpression1);


                                if (lstPrinter.Count > 0)
                                {
                                    if (printFormat == Constant.PrintFormat.BIXOLON_ETIKET_RSPKSB)
                                    {
                                        string printerUrl1 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.LABEL_DISTRIBUSI_GIZI).FirstOrDefault().PrinterName;
                                        ZebraPrinting.PrintNutritionLabelBixolon(printFormat, lstNutritionOrderDtID, printerUrl1);
                                    }
                                    else
                                    {
                                        string printerUrl1 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.LABEL_DISTRIBUSI_GIZI).FirstOrDefault().PrinterName;
                                        ZebraPrinting.PrintNutritionLabel(printFormat, lstNutritionOrderDtID, printerUrl1);
                                    }
                                }
                                else
                                {
                                    result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                }
                            }
                            else if (printerType == Constant.PrinterType.EPSON_DOT_MATRIX)
                            {
                                filterExpression1 = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                    ipAddress, Constant.DirectPrintType.LABEL_DISTRIBUSI_GIZI);
                                List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExpression1);

                                if (lstPrinter.Count > 0)
                                {
                                    string printerUrl1 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.LABEL_DISTRIBUSI_GIZI).FirstOrDefault().PrinterName;
                                    ZebraPrinting.PrintNutritionLabelDotMatrix(lstNutritionOrderDtID,
                                    printerUrl1);
                                }
                                else
                                {
                                    result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                }
                            }
                            else if (printerType == Constant.PrinterType.BROTHER_PRINTER)
                            {
                                filterExpression1 = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                    ipAddress, Constant.DirectPrintType.LABEL_DISTRIBUSI_GIZI);
                                List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExpression1);

                                if (lstPrinter.Count > 0)
                                {
                                    string printerUrl1 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.LABEL_DISTRIBUSI_GIZI).FirstOrDefault().PrinterName;
                                    BrotherPrinting.PrintLabelDistribusiGizi_RSSY(lstNutritionOrderDtID, printFormat, printerUrl1);
                                }
                                else
                                {
                                    result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                }
                            }
                            else
                            {
                                filterExpression1 = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                        ipAddress, Constant.DirectPrintType.GIZI_DISTRIBUTION_LABEL);
                                List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExpression1);

                                if (lstPrinter.Count > 0)
                                {
                                    string printerUrl1 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.GIZI_DISTRIBUTION_LABEL).FirstOrDefault().PrinterName;
                                    foreach (vNutritionOrderDtCustom obj in lstEntity)
                                    {
                                        ZebraPrinting.PrintNutritionDistributionLabel1(obj.NutritionOrderHdID, obj.NutritionOrderDtID, lstPrinter[0].PrinterName, txtExpiredDate.Text);
                                    }
                                }
                                else
                                {
                                    result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                }
                            }
                        }
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

        private string GetFilterExpression(string lstDt = "")
        {
            String filterExpression = String.Format("ScheduleDate = '{0}' AND GCTransactionStatus NOT IN ('{1}','{2}') AND (DischargeDate IS NULL OR DischargeDate = '1900-01-01')", Helper.GetDatePickerValue(txtDate.Text), Constant.TransactionStatus.VOID, Constant.TransactionStatus.OPEN);
            if (Convert.ToInt32(cboServiceUnit.Value) != 0) filterExpression += String.Format(" AND HealthcareServiceUnitID = {0}", cboServiceUnit.Value);
            if (cboMealTime.Value != null && cboMealTime.Value.ToString() != "") filterExpression += String.Format(" AND GCMealTime = '{0}'", cboMealTime.Value);
            if (!string.IsNullOrEmpty(lstDt))
            {
                filterExpression += string.Format(" AND NutritionOrderDtID IN ({0})", lstDt);
            }
            filterExpression += " ORDER BY BedCode";

            return filterExpression;
        }

        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                if (param[1] == "single")
                {
                    if (ChangeWorkListStatus(ref errMessage)) result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (ChangeAllWorkListStatus(ref errMessage)) result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "print")
            {
                if (param[1] == "single")
                {
                    PrintSlipAll();
                }
                else if (param[1] == "all")
                {
                    string lstID = string.Empty;
                    if (param.Count() > 2)
                    {
                        lstID = param[2];
                        PrintAllNutritionDistributionLabel(lstID);
                    }
                    else
                    {
                        PrintAllNutritionDistributionLabel(lstID);
                    }
                }
                else
                {
                    result = PrintSlip(param[2], param[3]); //print single distribution label
                }
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        public bool ChangeWorkListStatus(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            NutritionOrderDtDao entityDtDao = new NutritionOrderDtDao(ctx);
            NutritionOrderHdDao entityHdDao = new NutritionOrderHdDao(ctx);
            try
            {
                NutritionOrderDt entity = entityDtDao.Get(Convert.ToInt32(hdnNutritionOrderDtID.Value));
                entity.GCItemDetailStatus = Constant.TransactionStatus.CLOSED;
                entityDtDao.Update(entity);

                string filterExpression = String.Format("NutritionOrderHdId = {0} AND GCItemDetailStatus NOT IN ('{1}','{2}')", hdnNutritionOrderHdID.Value, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
                int count = BusinessLayer.GetNutritionOrderDtRowCount(filterExpression, ctx);

                if (count == 0)
                {
                    NutritionOrderHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnNutritionOrderHdID.Value));
                    entityHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                    entityHdDao.Update(entityHd);
                }
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        public string GetListNutritionOrderHdID()
        {
            string result = string.Empty;
            string filterExpression = GetFilterExpression(hdnLstNutritionOrderDtID.Value);
            List<vNutritionOrderDtCustom> lst = BusinessLayer.GetvNutritionOrderDtCustomList(filterExpression).GroupBy(g => g.NutritionOrderHdID).Select(s => s.FirstOrDefault()).ToList();

            foreach (vNutritionOrderDtCustom dt in lst)
            {
                result += string.Format("{0},", dt.NutritionOrderHdID);
            }

            return result.Remove(result.Length - 1, 1);
        }

        public bool ChangeAllWorkListStatus(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            NutritionOrderDtDao entityDtDao = new NutritionOrderDtDao(ctx);
            NutritionOrderHdDao entityHdDao = new NutritionOrderHdDao(ctx);

            try
            {
                String filterExpression = String.Format("NutritionOrderDtID IN ({0})", hdnLstNutritionOrderDtID.Value);
                List<NutritionOrderDt> lstEntity = BusinessLayer.GetNutritionOrderDtList(filterExpression, ctx);
                foreach (NutritionOrderDt obj in lstEntity)
                {
                    if (obj.GCItemDetailStatus == Constant.TransactionStatus.PROCESSED)
                    {
                        obj.GCItemDetailStatus = Constant.TransactionStatus.CLOSED;
                        obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(obj);
                    }
                }

                filterExpression = String.Format("NutritionOrderHdID IN ({0})", GetListNutritionOrderHdID());
                List<NutritionOrderHd> lstEntityHd = BusinessLayer.GetNutritionOrderHdList(filterExpression, ctx);
                foreach (NutritionOrderHd obj in lstEntityHd)
                {
                    filterExpression = String.Format("NutritionOrderHdId = {0} AND GCItemDetailStatus NOT IN ('{1}','{2}')", obj.NutritionOrderHdID, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
                    int count = BusinessLayer.GetNutritionOrderDtRowCount(filterExpression, ctx);

                    if (count == 0)
                    {
                        obj.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                        entityHdDao.Update(obj);
                    }
                }

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        //protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        TextBox txtExpiredTimeDt = e.Row.FindControl("txtExpiredTimeDt") as TextBox;
        //        txtExpiredTimeDt.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
        //    }
        //}   
    }
}
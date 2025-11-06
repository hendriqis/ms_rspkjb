using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using System.Xml.Linq;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using System.Net.Sockets;
using System.IO;
using System.Net;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class RegistrationPrintCtl : BaseViewPopupCtl
    {

        protected string HealthcareID;
        protected int UserID;
        protected string UserFullName;
        protected string UserName;

        public override void InitializeDataControl(string param)
        {
            if (AppSession.UserLogin != null)
            {
                HealthcareID = AppSession.UserLogin.HealthcareID;
                UserID = AppSession.UserLogin.UserID;
                UserName = AppSession.UserLogin.UserName;
                UserFullName = AppSession.UserLogin.UserFullName;
            }
            if (param != "")
            {
                string moduleName = Helper.GetModuleName();
                string moduleID = Helper.GetModuleID(moduleName);

                Healthcare oHealthcare = BusinessLayer.GetHealthcare(AppSession.UserLogin.HealthcareID);
                XDocument xdoc = Helper.LoadXMLFile(this, string.Format("right_panel/{0}.xml", moduleID));
                if (xdoc != null)
                {
                    var lstQuickMenu = (from pg in xdoc.Descendants("page").Where(p => p.Attribute("menucode").Value == param)
                                        select new
                                        {
                                            Print = (from qm in pg.Descendants("print")
                                                     select new
                                                     {
                                                         Title = qm.Attribute("title").Value,
                                                         IsDisplayPrintCount = qm.Attribute("isDisplayPrintCount") == null ? "0" : qm.Attribute("isDisplayPrintCount").Value,
                                                         ReportCode = qm.Attribute("reportcode").Value,
                                                         Visible = qm.Attribute("visible") == null ? "1" : qm.Attribute("visible").Value,
                                                         HealthcareAccess = qm.Attribute("isByHealthcare") == null ? oHealthcare.Initial : qm.Attribute("isByHealthcare").Value == "1" ? qm.Attribute("healthcareName").Value : oHealthcare.Initial
                                                     })

                                        }).FirstOrDefault();
                    if (lstQuickMenu != null)
                    {
                        var lstPrint = lstQuickMenu.Print.Where(p => p.Visible == "1" && p.HealthcareAccess == oHealthcare.Initial);
                        if (lstPrint.Count() > 0)
                        {
                            rptPrint.DataSource = lstPrint;
                            rptPrint.DataBind();
                        }
                    }
                }

                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}')",
AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_FORMAT_CETAKAN_LABEL, Constant.SettingParameter.RM_JUMLAH_CETAKAN_LABEL_RD, Constant.SettingParameter.RM_JUMLAH_CETAKAN_LABEL_RJ,
Constant.SettingParameter.RM_JUMLAH_CETAKAN_LABEL_RI, Constant.SettingParameter.RM_JUMLAH_CETAKAN_LABEL_MD, Constant.SettingParameter.RM_MULTI_LOKASI_PENDAFTARAN, Constant.SettingParameter.RM_MAX_JUMLAH_LABEL);

                List<vSettingParameterDt> oParamList = BusinessLayer.GetvSettingParameterDtList(filterExp);
                if (oParamList.Count > 0)
                {
                    vSettingParameterDt oParam;

                    switch (moduleID)
                    {
                        case Constant.Module.INPATIENT:
                            oParam = oParamList.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_JUMLAH_CETAKAN_LABEL_RI)).FirstOrDefault();
                            break;
                        case Constant.Module.EMERGENCY:
                            oParam = oParamList.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_JUMLAH_CETAKAN_LABEL_RD)).FirstOrDefault();
                            break;
                        case Constant.Module.OUTPATIENT:
                            oParam = oParamList.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_JUMLAH_CETAKAN_LABEL_RJ)).FirstOrDefault();
                            break;
                        default:
                            oParam = oParamList.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_JUMLAH_CETAKAN_LABEL_MD)).FirstOrDefault();
                            break;
                    }
                    txtJmlLabelCtl.Text = oParam != null ? oParam.ParameterValue : "1";

                    bool isMultiLocation = oParamList.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_MULTI_LOKASI_PENDAFTARAN)).FirstOrDefault().ParameterValue == "1" ? true : false;
                    hdnMaxLabelNoCtl.Value = oParamList.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_MAX_JUMLAH_LABEL)).FirstOrDefault().ParameterValue;

                    hdnIsMultiLocationCtl.Value = isMultiLocation ? "1" : "0";
                    if (isMultiLocation)
                    {
                        List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.LOKASI_PENDAFTARAN));
                        Methods.SetComboBoxField(cboPrinterUrlCtl, lstStandardCode, "StandardCodeName", "StandardCodeID", DevExpress.Web.ASPxEditors.DropDownStyle.DropDownList);
                        if (cboPrinterUrlCtl.Items.Count > 0)
                        {
                            ////Set Default Value based on IP Address
                            //string ipAddress = HttpContext.Current.Request.UserHostAddress;
                            //filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType = '{1}' AND IsDeleted=0",
                            //    ipAddress, Constant.DirectPrintType.BUKTI_PEMBAYARAN);

                            ////Get Printer Address
                            //PrinterLocation oPrinter = BusinessLayer.GetPrinterLocationList(filterExp).FirstOrDefault();

                            cboPrinterUrlCtl.SelectedIndex = 0;
                        }
                    }
                }
                //SetControlProperties();
            }
        }

        protected void cbpDirectPrintProcessCtl_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string[] param = e.Parameter.Split('|');

            string reportCode = hdnReportCode.Value;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            try
            {
                int id = Convert.ToInt32(param[0]);
                string result = string.Empty;
                switch (reportCode)
                {
                    case Constant.PrintCode.BUKTI_PENDAFTARAN:
                        result = PrintBuktiPendaftaran(id);
                        break;
                    case Constant.PrintCode.KARTU_PASIEN:
                        result = PrintPatientCard(id);
                        break;
                    case Constant.PrintCode.GELANG_PASIEN:
                        result = PrintPatientWristband(id);
                        break;
                    case Constant.PrintCode.GELANG_PASIEN_ANAK:
                        result = PrintPatientChildWristband(id);
                        break;
                    default:
                        result = "";
                        break;
                }
                panel.JSProperties["cpZebraPrinting"] = result;
            }
            catch (Exception ex)
            {
                Helper.InsertErrorLog(ex);
                panel.JSProperties["cpZebraPrinting"] = "An error occured while sending command to printer";
            }
        }

        protected void cbpDirectPrintProcessCtlDirect_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string[] param = e.Parameter.Split('|');

            string reportCode = hdnReportCode.Value;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            try
            {
                int id = Convert.ToInt32(param[0]);
                string result = string.Empty;
                switch (reportCode)
                {
                    case Constant.PrintCode.BUKTI_PENDAFTARAN:
                        result = PrintBuktiPendaftaran(id);
                        break;
                    case Constant.PrintCode.KARTU_PASIEN:
                        result = PrintPatientCard(id);
                        break;
                    case Constant.PrintCode.GELANG_PASIEN:
                        result = PrintPatientWristband(id);
                        break;
                    case Constant.PrintCode.GELANG_PASIEN_ANAK:
                        result = PrintPatientChildWristband(id);
                        break;
                    case Constant.PrintCode.LABEL_MR_RSMD:
                        result = PrintLabelMR(id);
                        break;
                    case Constant.PrintCode.BUKTI_PENDAFTARAN_THERMAL:
                        result = PrintBuktiPendaftaranThermal(id);
                        break;
                    default:
                        result = "";
                        break;
                }
                panel.JSProperties["cpZebraPrinting"] = result;
            }
            catch (Exception ex)
            {
                Helper.InsertErrorLog(ex);
                panel.JSProperties["cpZebraPrinting"] = "An error occured while sending command to printer";
            }
        }

        private string PrintPatientCard(int id)
        {
            string result = string.Empty;
            try
            {
                SettingParameterDt oParameter = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_ALAMAT_PRINTER_DATACARD);
                if (oParameter != null)
                {
                    if (!String.IsNullOrEmpty(oParameter.ParameterValue))
                    {
                        try
                        {
                            Patient oPatient = BusinessLayer.GetPatient(id);
                            if (oPatient != null)
                            {
                                string msgPart1 = oPatient.Name;
                                string msgPart2 = oPatient.MedicalNo;
                                string msgPart3 = oPatient.RegisteredDate.ToString(Constant.FormatString.DATE_FORMAT);
                                string msgPart4 = oParameter.ParameterValue;
                                string msgData = String.Format("{0}|{1}|{2}|{3}", msgPart1, msgPart2, msgPart3, msgPart4);

                                TcpClient client = new TcpClient();
                                client.Connect(IPAddress.Parse(oParameter.ParameterValue), 6000);
                                NetworkStream stream = client.GetStream();
                                using (BinaryWriter w = new BinaryWriter(stream))
                                {
                                    using (BinaryReader r = new BinaryReader(stream))
                                    {
                                        w.Write(string.Format(@"{0}", msgData.ToString()).ToCharArray());
                                    }
                                }
                            }
                            else
                            {
                                result = String.Format("Cannot find registration information.");
                            }
                        }
                        catch (Exception ex)
                        {
                            result = String.Format("Cannot send information to printer service \n {0}", ex.Message);
                        }
                    }
                    else
                    {
                        result = String.Format("Printer Location is not been configured yet.");
                    }
                }
                else
                {
                    result = String.Format("Cannot find parameter information to printer service");
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        private string PrintBuktiPendaftaran(int id)
        {
            string result = string.Empty;
            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_FORMAT_BUKTI_PENDAFTARAN, Constant.SettingParameter.RM_ALAMAT_PRINTER_BUKTI_PENDAFTARAN, Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                if (lstParam != null)
                {
                    string printerType = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    string printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    if (hdnIsMultiLocationCtl.Value == "1")
                    {
                        //Get Printer Url from Location DropDown
                        StandardCode oStandardCode = BusinessLayer.GetStandardCode(cboPrinterUrlCtl.Value.ToString());
                        if (oStandardCode != null)
                        {
                            if (!String.IsNullOrEmpty(oStandardCode.TagProperty))
                            {
                                string[] tagField = oStandardCode.TagProperty.Split('|');
                                //Printer_Label| Printer_Wristband_Male;Printer_Wristband_Female;Printer_Wristband_Children_Male;Printer_Wristband_Children_Female|Printer_Wristband_Infant|Printer_RegistrationSlip
                                printerUrl = tagField[3];
                            }
                        }
                    }

                    Registration oRegistration = BusinessLayer.GetRegistration(id);
                    if (oRegistration != null)
                    {
                        vConsultVisit oVisit = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", oRegistration.RegistrationID)).FirstOrDefault();
                        if (oVisit != null)
                        {
                            switch (printerType)
                            {
                                case Constant.PrinterType.ZEBRA_PRINTER:
                                    ZebraPrinting.PrintBuktiPendaftaran(oVisit, printerUrl, printFormat);
                                    break;
                                case Constant.PrinterType.EPSON_DOT_MATRIX:
                                    ZebraPrinting.PrintBuktiPendaftaran3(oVisit, printerUrl, printFormat);
                                    break;
                                case Constant.PrinterType.DOT_MATRIX_FORMAT_1:
                                    result = ZebraPrinting.PrintBuktiPendaftaran4(oVisit, printerUrl, printFormat);
                                    break;
                                default:
                                    ZebraPrinting.PrintBuktiPendaftaran(oVisit, printerUrl, printFormat);
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        private string PrintBuktiPendaftaranThermal(int id)
        {
            string result = string.Empty;
            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_FORMAT_BUKTI_PENDAFTARAN, Constant.SettingParameter.RM_ALAMAT_PRINTER_BUKTI_PENDAFTARAN, Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                if (lstParam != null)
                {
                    string printerType = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    string printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    if (hdnIsMultiLocationCtl.Value == "1")
                    {
                        //Get Printer Url from Location DropDown
                        StandardCode oStandardCode = BusinessLayer.GetStandardCode(cboPrinterUrlCtl.Value.ToString());
                        if (oStandardCode != null)
                        {
                            if (!String.IsNullOrEmpty(oStandardCode.TagProperty))
                            {
                                string[] tagField = oStandardCode.TagProperty.Split('|');
                                //Printer_Label| Printer_Wristband_Male;Printer_Wristband_Female;Printer_Wristband_Children_Male;Printer_Wristband_Children_Female|Printer_Wristband_Infant|Printer_RegistrationSlip
                                printerUrl = tagField[3];
                            }
                        }
                    }

                    Registration oRegistration = BusinessLayer.GetRegistration(id);
                    if (oRegistration != null)
                    {
                        vConsultVisit oVisit = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", oRegistration.RegistrationID)).FirstOrDefault();
                        if (oVisit != null)
                        {
                            switch (printerType)
                            {
                                case Constant.PrinterType.ZEBRA_PRINTER:
                                    ZebraPrinting.PrintBuktiPendaftaran(oVisit, printerUrl, printFormat);
                                    break;
                                case Constant.PrinterType.EPSON_DOT_MATRIX:
                                    ZebraPrinting.PrintBuktiPendaftaran3(oVisit, printerUrl, printFormat);
                                    break;
                                case Constant.PrinterType.DOT_MATRIX_FORMAT_1:
                                    result = ZebraPrinting.PrintBuktiPendaftaranThermal(oVisit, printerUrl, printFormat);
                                    break;
                                default:
                                    ZebraPrinting.PrintBuktiPendaftaran(oVisit, printerUrl, printFormat);
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        private string PrintPatientWristband(int id)
        {
            string errorMsg = string.Empty;

            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_FORMAT_GELANG_PASIEN_DEWASA, Constant.SettingParameter.RM_FORMAT_GELANG_PASIEN_ANAK, Constant.SettingParameter.RM_FORMAT_GELANG_PASIEN_BAYI,
                    Constant.SettingParameter.RM_GELANG_PASIEN_ANAK_KHUSUS, Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_LAKI,
                    Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_PEREMPUAN, Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_ANAK,
                    Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_BAYI);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                if (lstParam != null)
                {
                    Registration oRegistration = BusinessLayer.GetRegistration(id);
                    if (oRegistration != null)
                    {
                        vConsultVisit oVisit = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", oRegistration.RegistrationID)).FirstOrDefault();
                        if (oVisit != null)
                        {

                            string printFormat = string.Empty;
                            string printerUrl = string.Empty;

                            if (hdnIsMultiLocationCtl.Value == "0")
                            {
                                printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_GELANG_PASIEN_DEWASA)).FirstOrDefault().ParameterValue;
                                //Choose Printer Url based on Patient Gender
                                switch (oVisit.GCGender)
                                {
                                    case Constant.Gender.MALE:
                                        printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_LAKI)).FirstOrDefault().ParameterValue;
                                        break;
                                    case Constant.Gender.FEMALE:
                                        printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_PEREMPUAN)).FirstOrDefault().ParameterValue;
                                        break;
                                    default:
                                        printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_LAKI)).FirstOrDefault().ParameterValue;
                                        break;
                                }
                            }
                            else
                            {
                                //Get Printer Url from Location DropDown
                                StandardCode oStandardCode = BusinessLayer.GetStandardCode(cboPrinterUrlCtl.Value.ToString());
                                if (oStandardCode != null)
                                {
                                    if (!String.IsNullOrEmpty(oStandardCode.TagProperty))
                                    {
                                        string[] tagField = oStandardCode.TagProperty.Split('|');
                                        //Printer_Label| Printer_Wristband_Male;Printer_Wristband_Female;Printer_Wristband_Children_Male;Printer_Wristband_Children_Female|Printer_Wristband_Infant|Printer_RegistrationSlip
                                        string[] wristbandUrl = tagField[1].Split(';');
                                        printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_GELANG_PASIEN_DEWASA)).FirstOrDefault().ParameterValue;
                                        //Choose Printer Url based on Patient Gender
                                        switch (oVisit.GCGender)
                                        {
                                            case Constant.Gender.MALE:
                                                printerUrl = wristbandUrl[0];
                                                break;
                                            case Constant.Gender.FEMALE:
                                                printerUrl = wristbandUrl[1];
                                                break;
                                            default:
                                                printerUrl = wristbandUrl[0];
                                                break;
                                        }
                                    }
                                }

                            }
                            if (!string.IsNullOrEmpty(printerUrl))
                            {
                                int labelCount = Convert.ToInt16(txtJmlLabelCtl.Text);
                                ZebraPrinting.PrintWristband(oVisit, printerUrl, printFormat);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            return errorMsg;
        }

        private string PrintPatientChildWristband(int id)
        {
            string errorMsg = string.Empty;

            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_FORMAT_GELANG_PASIEN_DEWASA, Constant.SettingParameter.RM_FORMAT_GELANG_PASIEN_ANAK, Constant.SettingParameter.RM_FORMAT_GELANG_PASIEN_BAYI,
                    Constant.SettingParameter.RM_GELANG_PASIEN_ANAK_KHUSUS, Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_LAKI,
                    Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_PEREMPUAN, Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_ANAK,
                    Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_BAYI);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                if (lstParam != null)
                {
                    Registration oRegistration = BusinessLayer.GetRegistration(id);
                    if (oRegistration != null)
                    {
                        vConsultVisit oVisit = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", oRegistration.RegistrationID)).FirstOrDefault();
                        if (oVisit != null)
                        {

                            string printFormat = string.Empty;
                            string printerUrl = string.Empty;

                            if (hdnIsMultiLocationCtl.Value == "0")
                            {
                                //if new born
                                if (oVisit.IsNewBorn)
                                {
                                    printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_BAYI)).FirstOrDefault().ParameterValue;
                                    printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_GELANG_PASIEN_BAYI)).FirstOrDefault().ParameterValue;
                                }
                                else
                                {
                                    bool isUsingChildrenWristband = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_GELANG_PASIEN_ANAK_KHUSUS)).FirstOrDefault().ParameterValue == "1" ? true : false;
                                    if (!isUsingChildrenWristband)
                                    {
                                        printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_GELANG_PASIEN_DEWASA)).FirstOrDefault().ParameterValue;
                                        //Choose Printer Url based on Patient Gender
                                        switch (oVisit.GCGender)
                                        {
                                            case Constant.Gender.MALE:
                                                printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_LAKI)).FirstOrDefault().ParameterValue;
                                                break;
                                            case Constant.Gender.FEMALE:
                                                printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_PEREMPUAN)).FirstOrDefault().ParameterValue;
                                                break;
                                            default:
                                                printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_LAKI)).FirstOrDefault().ParameterValue;
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_ANAK)).FirstOrDefault().ParameterValue;
                                        printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_GELANG_PASIEN_ANAK)).FirstOrDefault().ParameterValue;
                                    }
                                }
                            }
                            else
                            {
                                //Get Printer Url from Location DropDown
                                StandardCode oStandardCode = BusinessLayer.GetStandardCode(cboPrinterUrlCtl.Value.ToString());
                                if (oStandardCode != null)
                                {
                                    if (!String.IsNullOrEmpty(oStandardCode.TagProperty))
                                    {
                                        string[] tagField = oStandardCode.TagProperty.Split('|');
                                        //Printer_Label| Printer_Wristband_Male;Printer_Wristband_Female;Printer_Wristband_Children_Male;Printer_Wristband_Children_Female|Printer_Wristband_Infant|Printer_RegistrationSlip
                                        if (oVisit.IsNewBorn)
                                        {
                                            string[] wristbandUrl = tagField[2].Split(';');
                                            printerUrl = wristbandUrl[0];
                                            printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_GELANG_PASIEN_BAYI)).FirstOrDefault().ParameterValue;
                                        }
                                        else
                                        {
                                            string[] wristbandUrl = tagField[1].Split(';');
                                            switch (oVisit.GCGender)
                                            {
                                                case Constant.Gender.MALE:
                                                    printerUrl = wristbandUrl[2];
                                                    break;
                                                case Constant.Gender.FEMALE:
                                                    printerUrl = wristbandUrl[3];
                                                    break;
                                                default:
                                                    printerUrl = wristbandUrl[2];
                                                    break;
                                            }
                                            printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_GELANG_PASIEN_ANAK)).FirstOrDefault().ParameterValue;
                                        }
                                    }
                                }

                            }
                            if (!string.IsNullOrEmpty(printerUrl))
                            {
                                int labelCount = Convert.ToInt16(txtJmlLabelCtl.Text);
                                ZebraPrinting.PrintWristband(oVisit, printerUrl, printFormat);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            return errorMsg;
        }

        private string PrintLabelMR(int id)
        {
            string errorMsg = string.Empty;

            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_FORMAT_CETAKAN_LABEL, Constant.SettingParameter.RM_FORMAT_CETAKAN_LABEL_2,
                    Constant.SettingParameter.RM_JUMLAH_CETAKAN_LABEL_RD, Constant.SettingParameter.RM_JUMLAH_CETAKAN_LABEL_RJ,
                    Constant.SettingParameter.RM_JUMLAH_CETAKAN_LABEL_RI, Constant.SettingParameter.RM_JUMLAH_CETAKAN_LABEL_MD,
                    Constant.SettingParameter.RM_ALAMAT_PRINTER_LABEL_RD, Constant.SettingParameter.RM_ALAMAT_PRINTER_LABEL_RJ,
                    Constant.SettingParameter.RM_ALAMAT_PRINTER_LABEL_RI, Constant.SettingParameter.RM_ALAMAT_PRINTER_LABEL_MD);

                ZebraPrinting.PrintLabelMR(id, "");
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            return errorMsg;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientAppointmentInformation : BasePageContent
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            string module = Helper.GetModuleID(Helper.GetModuleName());
            switch (module)
            {
                case Constant.Module.OUTPATIENT: return Constant.MenuCode.Outpatient.APPOINTMENT_INFORMATION; 
                case Constant.Module.MEDICAL_DIAGNOSTIC: return Constant.MenuCode.MedicalDiagnostic.APPOINTMENT_INFORMATION; 
                case Constant.Module.LABORATORY: return Constant.MenuCode.Laboratory.APPOINTMENT_INFORMATION; 
                case Constant.Module.IMAGING: return Constant.MenuCode.Imaging.APPOINTMENT_INFORMATION; 
                case Constant.Module.MEDICAL_CHECKUP: return Constant.MenuCode.MedicalCheckup.APPOINTMENT_INFORMATION; 
                case Constant.Module.RADIOTHERAPHY: return Constant.MenuCode.Radiotheraphy.APPOINTMENT_INFORMATION; 
                default: return Constant.MenuCode.MedicalRecord.APPOINTMENT_INFORMATION; 
            }
        }

        private GetUserMenuAccess menu;

        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }

        protected string GetRefreshGridInterval()
        {
            return "1";
        }

        private string refreshGridInterval = "";

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {

                hdnHealthcareServiceUnitIDLaboratory.Value = AppSession.LaboratoryServiceUnitID;
                hdnHealthcareServiceUnitIDRadiology.Value = AppSession.ImagingServiceUnitID;

                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;
                string filterExpression = string.Format("IsUsingRegistration = 1 AND HealthcareID = '{0}' AND DepartmentID IN ('{1}','{2}','{3}','{4}','{5}')", AppSession.UserLogin.HealthcareID, Constant.Facility.OUTPATIENT, Constant.Facility.DIAGNOSTIC, Constant.Facility.IMAGING, Constant.Facility.LABORATORY, Constant.Facility.MEDICAL_CHECKUP);
                string module = Helper.GetModuleID(Helper.GetModuleName());
                switch (module)
                {
                    case Constant.Module.OUTPATIENT:
                        filterExpression = string.Format("IsUsingRegistration = 1 AND HealthcareID = '{0}' AND DepartmentID IN ('{1}')", AppSession.UserLogin.HealthcareID, Constant.Facility.OUTPATIENT);
                        hdnDepartmentID.Value = Constant.Facility.OUTPATIENT;
                        break;
                    case Constant.Module.MEDICAL_DIAGNOSTIC:
                    case Constant.Module.LABORATORY:
                    case Constant.Module.RADIOTHERAPHY:
                    case Constant.Module.IMAGING:
                        if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        {
                            filterExpression = string.Format("IsUsingRegistration = 1 AND HealthcareID = '{0}' AND IsLaboratoryUnit = 1", AppSession.UserLogin.HealthcareID);
                            hdnDepartmentID.Value = Constant.Facility.LABORATORY;
                        }
                        else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        {
                            filterExpression = string.Format("IsUsingRegistration = 1 AND HealthcareID = '{0}' AND HealthcareServiceUnitID = '{1}'", AppSession.UserLogin.HealthcareID, hdnHealthcareServiceUnitIDRadiology.Value);
                            hdnDepartmentID.Value = Constant.Facility.IMAGING;
                        }
                        else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                        {
                            filterExpression = string.Format("IsUsingRegistration = 1 AND HealthcareID = '{0}' AND HealthcareServiceUnitID = '{1}'", AppSession.UserLogin.HealthcareID, AppSession.RT0001);
                            hdnDepartmentID.Value = Constant.Facility.DIAGNOSTIC;
                        }
                        else
                        {
                            filterExpression = string.Format("IsUsingRegistration = 1 AND HealthcareID = '{0}' AND HealthcareServiceUnitID NOT IN ('{1}') AND IsLaboratoryUnit = 0 AND DepartmentID IN ('{2}')", AppSession.UserLogin.HealthcareID, hdnHealthcareServiceUnitIDRadiology.Value, Constant.Facility.DIAGNOSTIC);
                            hdnDepartmentID.Value = Constant.Facility.DIAGNOSTIC;
                        }
                        break;
                    case Constant.Module.MEDICAL_CHECKUP:
                        filterExpression = string.Format("IsUsingRegistration = 1 AND HealthcareID = '{0}' AND DepartmentID IN ('{1}')", AppSession.UserLogin.HealthcareID, Constant.Facility.MEDICAL_CHECKUP);
                        hdnDepartmentID.Value = Constant.Facility.MEDICAL_CHECKUP;
                        break;
                }


                List<vHealthcareServiceUnitCustom> lstHSU = BusinessLayer.GetvHealthcareServiceUnitCustomList(filterExpression);
                lstHSU.Insert(0, new vHealthcareServiceUnitCustom() { ServiceUnitName = "Semua", ServiceUnitID = 0 });
                Methods.SetComboBoxField<vHealthcareServiceUnitCustom>(cboServiceUnit, lstHSU, "ServiceUnitName", "HealthcareServiceUnitID");
                cboServiceUnit.SelectedIndex = 0;

                List<StandardCode> lstRegis = new List<StandardCode>();
                lstRegis.Insert(0, new StandardCode { StandardCodeName = "Semua", StandardCodeID = "0" });

                lstRegis.Insert(1, new StandardCode { StandardCodeName = "Pasien Baru", StandardCodeID = "1" });
                lstRegis.Insert(2, new StandardCode { StandardCodeName = "Registrasi", StandardCodeID = "2" });
                lstRegis.Insert(3, new StandardCode { StandardCodeName = "Batal Registrasi", StandardCodeID = "3" });
                lstRegis.Insert(4, new StandardCode { StandardCodeName = "Belum Registrasi", StandardCodeID = "4" });
                Methods.SetComboBoxField<StandardCode>(cboRegistrationStatus, lstRegis, "StandardCodeName", "StandardCodeID");
                cboRegistrationStatus.SelectedIndex = 0;

                txtAppointmentDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());
                BindGridView(1, true, ref PageCount);
            }
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else if (param[0] == "print")
                {
                    PrintTracerAppointment(Convert.ToInt32(param[1]));
                    result = "print";
                }
                else if (param[0] == "print_rm")
                {
                    PrintTracerRMAppointment(Convert.ToInt32(param[1]));
                    result = "print";
                }
                else // refresh
                {

                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvAppointmentRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_PATIENT_LIST);
            }
            string registrationStatus = cboRegistrationStatus.Value.ToString();

            List<vAppointment> lstEntity = BusinessLayer.GetvAppointmentList(filterExpression, Constant.GridViewPageSize.GRID_PATIENT_LIST, pageIndex, "HealthcareServiceUnitID, ParamedicID, QueueNo");
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }


        private string GetFilterExpression()
        {
            string healthcareServiceUnitID = "";
            healthcareServiceUnitID = cboServiceUnit.Value.ToString();

            string statusRegis = "";
            string status = "";
            statusRegis = cboRegistrationStatus.Value.ToString();

            string filterExpression = "";

            string module = Helper.GetModuleID(Helper.GetModuleName());
            switch (module)
            {
                case Constant.Module.OUTPATIENT:
                    filterExpression = string.Format("  DepartmentID IN ('{0}')", Constant.Facility.OUTPATIENT);

                    break;
                case Constant.Module.MEDICAL_DIAGNOSTIC:
                case Constant.Module.LABORATORY:
                case Constant.Module.RADIOTHERAPHY:
                case Constant.Module.IMAGING:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                    {
                        filterExpression = string.Format(" IsLaboratoryUnit = 1");
                    }
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                    {
                        filterExpression = string.Format(" HealthcareServiceUnitID = '{0}'", hdnHealthcareServiceUnitIDRadiology.Value);
                    }
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                    {
                        filterExpression = string.Format(" HealthcareServiceUnitID = '{0}'", AppSession.RT0001);
                    }
                    else
                    {
                        filterExpression = string.Format(" HealthcareServiceUnitID NOT IN ('{0}') AND IsLaboratoryUnit = 0", hdnHealthcareServiceUnitIDRadiology.Value);
                        hdnDepartmentID.Value = Constant.Facility.DIAGNOSTIC;
                    }
                    break;
                case Constant.Module.MEDICAL_CHECKUP:
                    filterExpression = string.Format(" DepartmentID IN ('{0}')", Constant.Facility.MEDICAL_CHECKUP);
                    break;
                case Constant.Module.MEDICAL_RECORD:
                    filterExpression = string.Format(" DepartmentID IN ('{0}','{1}','{2}')", Constant.Facility.OUTPATIENT, Constant.Facility.MEDICAL_CHECKUP, Constant.Facility.DIAGNOSTIC);
                    break;
            }

            if (statusRegis == "1")
            {
                status += string.Format("AND IsNewPatient = 1");
            }
            else if (statusRegis == "2")
            {
                status += string.Format("AND GCAppointmentStatus = '{0}'", Constant.AppointmentStatus.COMPLETE.ToString());
            }
            else if (statusRegis == "3")
            {
                status += string.Format("AND GCAppointmentStatus = '{0}'", Constant.AppointmentStatus.DELETED.ToString());
            }
            else if (statusRegis == "4")
            {
                status += string.Format("AND GCAppointmentStatus = '{0}'", Constant.AppointmentStatus.STARTED.ToString());
            }
            //string filterExpression = string.Format("StartDate = '{0}' ", Helper.GetDatePickerValue(txtAppointmentDate).ToString(Constant.FormatString.DATE_FORMAT_112));

            if (!chkIsPreviousEpisodePatient.Checked)
            {
                filterExpression += string.Format(" AND StartDate = '{0}' ", Helper.GetDatePickerValue(txtAppointmentDate).ToString(Constant.FormatString.DATE_FORMAT_112));
                if (healthcareServiceUnitID != "0")
                {
                    filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", healthcareServiceUnitID);
                }
                if (statusRegis != "0")
                {
                    filterExpression += string.Format("{0}", status);
                }

                if (hdnFilterExpressionQuickSearch.Value != "")
                {
                    filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
                }
            }
            else
            {
                filterExpression += string.Format(" AND StartDate LIKE '%%'");
                if (healthcareServiceUnitID != "0")
                {
                    filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", healthcareServiceUnitID);
                }
                if (statusRegis != "0")
                {
                    filterExpression += string.Format("{0}", status);
                }
                if (hdnFilterExpressionQuickSearch.Value != "")
                {
                    filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
                }
            }

            if (hdnPhysicianID.Value != "")
            {
                filterExpression += string.Format(" AND ParamedicID = {0}", hdnPhysicianID.Value);
            }
            //filterExpression += string.Format(" AND GCAppointmentStatus != '{0}'", Constant.AppointmentStatus.DELETED);
            return filterExpression;
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem dataitem = (ListViewDataItem)e.Item;
                if (!Convert.ToBoolean(DataBinder.Eval(dataitem.DataItem, "IsNewPatient")))
                {
                    System.Web.UI.HtmlControls.HtmlTableRow tr = (System.Web.UI.HtmlControls.HtmlTableRow)dataitem.FindControl("trItem");
                    //tr.BgColor = System.Drawing.Color.AliceBlue.ToString();
                    tr.Attributes.Add("class", "LvColor");
                }
            }
        }

        private void PrintTracerAppointment(int id)
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
                    vAppointment oAppointment = BusinessLayer.GetvAppointmentList(string.Format("AppointmentID = {0}", id)).FirstOrDefault();
                    if (oAppointment != null)
                    {
                        switch (printerType)
                        {
                            case Constant.PrinterType.THERMAL_RECEIPT_PRINTER:
                                //Get Printer Address
                                string ipAddress = HttpContext.Current.Request.UserHostAddress;

                                filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                    ipAddress, Constant.DirectPrintType.BUKTI_PENDAFTARAN_PERJANJIAN);
                                
                                List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);
                                if (lstPrinter.Count > 0)
                                {
                                    if (printFormat == Constant.PrintFormat.BUKTI_PENDAFTARAN_RSSY)
                                    {
                                        ZebraPrinting.PrintBuktiPerjanjianRSSY(oAppointment, lstPrinter[0].PrinterName);
                                    }
                                    else
                                    {
                                        ZebraPrinting.PrintBuktiPerjanjian(oAppointment, lstPrinter[0].PrinterName);
                                    }
                                }
                                else
                                {
                                    result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
        }

        private void PrintTracerRMAppointment(int id)
        {
            string result = string.Empty;
            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_FORMAT_CETAKAN_TRACER, Constant.SettingParameter.RM_ALAMAT_PRINTER_BUKTI_PENDAFTARAN, Constant.SettingParameter.RM_JENIS_PRINTER_TRACER);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                if (lstParam != null)
                {
                    string printerType = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_JENIS_PRINTER_TRACER)).FirstOrDefault().ParameterValue;
                    string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_CETAKAN_TRACER)).FirstOrDefault().ParameterValue;
                    // string printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    vAppointment oAppointment = BusinessLayer.GetvAppointmentList(string.Format("AppointmentID = {0}", id)).FirstOrDefault();
                    if (oAppointment != null)
                    {
                        //Get Printer Address
                        string ipAddress = HttpContext.Current.Request.UserHostAddress;

                        filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                            ipAddress, Constant.DirectPrintType.TRACER_RM);

                        List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);
                        if (lstPrinter.Count > 0)
                        {
                            switch (printerType)
                            {
                                case Constant.PrinterType.THERMAL_RECEIPT_PRINTER:
                                    ZebraPrinting.PrintTracerApmRMRSSES(oAppointment, lstPrinter[0].PrinterName);
                                    break;
                                case Constant.PrinterType.THERMAL_FORMAT_RSSES:
                                    ZebraPrinting.PrintTracerApmRMRSSES(oAppointment, lstPrinter[0].PrinterName);
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
            catch (Exception ex)
            {
                result = ex.Message;
            }
        }

    }
}
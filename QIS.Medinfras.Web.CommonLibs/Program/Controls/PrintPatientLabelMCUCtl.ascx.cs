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
    public partial class PrintPatientLabelMCUCtl : BaseViewPopupCtl
    {
        string isRadiologi = "";
        public override void InitializeDataControl(string param)
        {
            string[] parameter = param.Split('|');
            hdnRegistrationDate.Value = parameter[0];
            hdnGCCustomerType.Value = parameter[1];
            hdnBusinessPartnerID.Value = parameter[2];
            hdnDepartmentID.Value = parameter[3];

            string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_FORMAT_CETAKAN_LABEL, Constant.SettingParameter.RM_FORMAT_CETAKAN_LABEL_2,
                    Constant.SettingParameter.RM_JUMLAH_CETAKAN_LABEL_RD, Constant.SettingParameter.RM_JUMLAH_CETAKAN_LABEL_RJ,
                    Constant.SettingParameter.RM_JUMLAH_CETAKAN_LABEL_RI, Constant.SettingParameter.RM_JUMLAH_CETAKAN_LABEL_MD,
                    Constant.SettingParameter.RM_ALAMAT_PRINTER_LABEL_RD, Constant.SettingParameter.RM_ALAMAT_PRINTER_LABEL_RJ,
                    Constant.SettingParameter.RM_ALAMAT_PRINTER_LABEL_RI, Constant.SettingParameter.RM_ALAMAT_PRINTER_LABEL_MD,
                    Constant.SettingParameter.RM_REPORT_CODE_CUSTOM_LABEL, Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI,
                    Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM, Constant.SettingParameter.RM_MULTI_LOKASI_PENDAFTARAN, Constant.SettingParameter.RM_MAX_JUMLAH_LABEL);
            List<SettingParameterDt> setvar = BusinessLayer.GetSettingParameterDtList(filterExp);
            hdnHealthcareServiceUnitImagingID.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).FirstOrDefault().ParameterValue;
            hdnHealthcareServiceUnitLaboratoryID.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).FirstOrDefault().ParameterValue;
            hdnPrintFormatLabel.Value = setvar.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_CETAKAN_LABEL)).FirstOrDefault().ParameterValue;
            hdnReportCodeCustomLabel.Value = setvar.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_REPORT_CODE_CUSTOM_LABEL)).FirstOrDefault().ParameterValue;
            hdnRMFormatLabel2.Value = setvar.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_CETAKAN_LABEL_2)).FirstOrDefault().ParameterValue;

            if (setvar.Count > 0)
            {
                SettingParameterDt oParam;
                string moduleID = string.Empty;
                switch (moduleID)
                {
                    case Constant.Module.INPATIENT:
                        oParam = setvar.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_JUMLAH_CETAKAN_LABEL_RI)).FirstOrDefault();
                        break;
                    case Constant.Module.EMERGENCY:
                        oParam = setvar.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_JUMLAH_CETAKAN_LABEL_RD)).FirstOrDefault();
                        break;
                    case Constant.Module.OUTPATIENT:
                        oParam = setvar.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_JUMLAH_CETAKAN_LABEL_RJ)).FirstOrDefault();
                        break;
                    default:
                        oParam = setvar.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_JUMLAH_CETAKAN_LABEL_MD)).FirstOrDefault();
                        break;
                }
                txtJmlLabel.Text = oParam != null ? oParam.ParameterValue : "1";

                bool isMultiLocation = setvar.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_MULTI_LOKASI_PENDAFTARAN)).FirstOrDefault().ParameterValue == "1" ? true : false;
                hdnMaxLabelNo.Value = setvar.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_MAX_JUMLAH_LABEL)).FirstOrDefault().ParameterValue;

                trPrinterLocation.Visible = isMultiLocation;
                hdnIsMultiLocation.Value = isMultiLocation ? "1" : "0";
                if (isMultiLocation)
                {
                    List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.LOKASI_PENDAFTARAN));
                    Methods.SetComboBoxField(cboPrinterUrl, lstStandardCode, "StandardCodeName", "StandardCodeID", DevExpress.Web.ASPxEditors.DropDownStyle.DropDownList);
                    if (cboPrinterUrl.Items.Count > 0)
                    {
                        ////Set Default Value based on IP Address
                        //string ipAddress = HttpContext.Current.Request.UserHostAddress;
                        //filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType = '{1}' AND IsDeleted=0",
                        //    ipAddress, Constant.DirectPrintType.BUKTI_PEMBAYARAN);

                        ////Get Printer Address
                        //PrinterLocation oPrinter = BusinessLayer.GetPrinterLocationList(filterExp).FirstOrDefault();

                        cboPrinterUrl.SelectedIndex = 0;
                    }
                }
            }
        }

        private string GetListRegistrationID()
        {
            string result = string.Empty;
            string apmFromDateYear = hdnRegistrationDate.Value.Substring(6, 4);
            string apmFromDateMonth = hdnRegistrationDate.Value.Substring(3, 2);
            string apmFromDateDay = hdnRegistrationDate.Value.Substring(0, 2);
            string apmFromDateFormat = string.Format("{0}-{1}-{2}", apmFromDateYear, apmFromDateMonth, apmFromDateDay);
            DateTime apmFromDate = DateTime.ParseExact(apmFromDateFormat, Constant.FormatString.DATE_PICKER_FORMAT2, null);

            string filterExpression = string.Format("IsDeleted = 0 AND GCCustomerType = '{0}'", hdnGCCustomerType.Value);

            if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
            {
                filterExpression += string.Format(" AND DepartmentID = '{0}'", hdnDepartmentID.Value);
            }
            else if (hdnDepartmentID.Value == Constant.Facility.IMAGING)
            {
                filterExpression += string.Format(" AND DepartmentID = '{0}' AND HealthcareServiceUnitID = '{1}'", Constant.Facility.DIAGNOSTIC, hdnHealthcareServiceUnitImagingID.Value);
            }
            else if (hdnDepartmentID.Value == Constant.Facility.LABORATORY)
            {
                filterExpression += string.Format(" AND DepartmentID = '{0}' AND HealthcareServiceUnitID = '{1}'", Constant.Facility.DIAGNOSTIC, hdnHealthcareServiceUnitLaboratoryID.Value);
            }
            else if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
            {
                filterExpression += string.Format(" AND DepartmentID = '{0}' AND HealthcareServiceUnitID NOT IN ('{1}','{2}')", Constant.Facility.DIAGNOSTIC, hdnHealthcareServiceUnitImagingID.Value, hdnHealthcareServiceUnitLaboratoryID.Value);
            }
            else if (hdnDepartmentID.Value == Constant.Facility.MEDICAL_CHECKUP)
            {
                filterExpression += string.Format("AND DepartmentID = '{0}' AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vHealthcareServiceUnit WHERE DepartmentID = '{0}')", hdnDepartmentID.Value);
            }

            if (hdnGCCustomerType.Value == Constant.CustomerType.PERSONAL)
            {
                filterExpression += string.Format(" AND BusinessPartnerID = 1");
            }
            else
            {
                if (!string.IsNullOrEmpty(hdnBusinessPartnerID.Value))
                {
                    filterExpression += string.Format(" AND BusinessPartnerID = {0}", hdnBusinessPartnerID.Value);
                }
            }

            filterExpression += string.Format(" AND ((AppointmentRequestDate = '{0}' OR AppointmentDate = '{0}') AND (AppointmentID IS NOT NULL AND RegistrationID IS NOT NULL))", apmFromDate.ToString(Constant.FormatString.DATE_FORMAT_112));

            List<vAppointmentRequest> lstEntity = BusinessLayer.GetvAppointmentRequestList(filterExpression);
            if (lstEntity.Count > 0)
            {
                foreach (vAppointmentRequest entity in lstEntity)
                {
                    result += entity.RegistrationID + ",";
                }
                result = result.Remove(result.Length - 1, 1);
            }
            return result;
        }

        protected void cbpPrintPatientLabel_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "success";

            //Get Printer Url from Location DropDown
            StandardCode oStandardCode = BusinessLayer.GetStandardCode(cboPrinterUrl.Value.ToString());
            if (rbReportCode.SelectedValue == hdnReportCodeCustomLabel.Value)
            {
                hdnPrintFormatLabel.Value = hdnRMFormatLabel2.Value;
            }
            string filterExpression = string.Empty;
            if (oStandardCode != null)
            {
                string[] tagField = oStandardCode.TagProperty.Split('|');
                //Printer_Label| Printer_Wristband_Male;Printer_Wristband_Female;Printer_Wristband_Children_Male;Printer_Wristband_Children_Female|Printer_Wristband_Infant|Printer_RegistrationSlip
                string printerUrl = tagField[0];
                string lstRegID = GetListRegistrationID();
                if (!string.IsNullOrEmpty(lstRegID))
                {
                    filterExpression = string.Format("RegistrationID IN ({0})", lstRegID);

                    List<vLabelPatientRegistrationInfo> lstHd = BusinessLayer.GetvLabelPatientRegistrationInfoList(filterExpression);
                    if (oStandardCode != null)
                    {
                        if (!String.IsNullOrEmpty(oStandardCode.TagProperty))
                        {
                            if (!string.IsNullOrEmpty(printerUrl))
                            {
                                int labelCount = Convert.ToInt16(txtJmlLabel.Text);
                                int maxLabelNo = Convert.ToInt16(hdnMaxLabelNo.Value);
                                foreach (vLabelPatientRegistrationInfo oVisit in lstHd)
                                {
                                    ZebraPrinting.PrintMRLabel(oVisit, printerUrl, hdnPrintFormatLabel.Value, labelCount > maxLabelNo ? maxLabelNo : labelCount);
                                }
                            }
                        }
                    }
                }   
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}
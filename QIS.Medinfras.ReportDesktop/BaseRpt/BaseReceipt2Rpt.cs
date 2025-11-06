using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BaseReceipt2Rpt : BaseRpt
    {
        public BaseReceipt2Rpt()
        {
            InitializeComponent();
        }

        private string GetFilterExpression(string value)
        {
            StringBuilder sbResult = new StringBuilder(value);
            sbResult.Replace("@HealthcareID", appSession.HealthcareID);
            if (value.Contains("@LaboratoryID"))
            {
                string laboratoryID = BusinessLayer.GetSettingParameter(Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).ParameterValue;
                vHealthcareServiceUnit HSU = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND ServiceUnitID = {1} AND IsDeleted = 0", appSession.HealthcareID, laboratoryID))[0];
                sbResult.Replace("@LaboratoryID", HSU.HealthcareServiceUnitID.ToString());
            }
            if (value.Contains("@ImagingID"))
            {
                string imagingID = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID).ParameterValue;
                vHealthcareServiceUnit HSU = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND ServiceUnitID = {1} AND IsDeleted = 0", appSession.HealthcareID, imagingID))[0];
                sbResult.Replace("@ImagingID", HSU.HealthcareServiceUnitID.ToString());
            }
            if (value.Contains("@AnamneseID"))
            {
                string anamneseID = BusinessLayer.GetSettingParameter(Constant.SettingParameter.ANAMNESE_SERVICE_UNIT_ID).ParameterValue;
                vHealthcareServiceUnit HSU = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND ServiceUnitID = {1} AND IsDeleted = 0", appSession.HealthcareID, anamneseID))[0];
                sbResult.Replace("@AnamneseID", HSU.HealthcareServiceUnitID.ToString());
            }
            return sbResult.ToString();
        }

        public override void InitializeReport(string[] param)
        {
            string filterSetVarDt = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
                                                        appSession.HealthcareID,
                                                        Constant.SettingParameter.SA_REPORT_FOOTER_PRINTEDBY_USERNAME_FULLNAME
                                                    );
            List<SettingParameterDt> lstSerVarDt = BusinessLayer.GetSettingParameterDtList(filterSetVarDt);

            lblReportTitle.Text = reportMaster.ReportTitle1;
            lblReportSubTitle.Text = reportMaster.ReportTitle2;

            string reportPrinterBy = "1";
            if (lstSerVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.SA_REPORT_FOOTER_PRINTEDBY_USERNAME_FULLNAME).FirstOrDefault().ParameterValue != null)
            {
                reportPrinterBy = lstSerVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.SA_REPORT_FOOTER_PRINTEDBY_USERNAME_FULLNAME).FirstOrDefault().ParameterValue;
            }

            if (reportPrinterBy == "1")
            {
                lblReportProperties.Text = string.Format("{0}, Print Date/Time:{1}, User:{2}", reportMaster.ReportCode, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"), appSession.UserName);
            }
            else
            {
                lblReportProperties.Text = string.Format("{0}, Print Date/Time:{1}, User:{2}", reportMaster.ReportCode, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"), appSession.UserFullName);
            }

            //Load Healthcare Information
            if (reportMaster.IsShowHeader)
            {
                vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
                if (oHealthcare != null)
                {
                    lblHealthcareName.Text = oHealthcare.HealthcareName;
                    lblAddressLine1.Text = oHealthcare.AddressLine1;
                    lblAddressLine2.Text = oHealthcare.AddressLine2;
                    lblPhoneFaxNo.Text = string.Format("Phone/Fax : {0}", string.IsNullOrEmpty(oHealthcare.FaxNo1) ? oHealthcare.PhoneNo1 : string.Format("{0}/{1}", oHealthcare.PhoneNo1, oHealthcare.FaxNo1));
                }
            }
            if (reportMaster.GCDataSourceType == Constant.DataSourceType.VIEW)
                BindingView(reportMaster, param);
            else
                BindingStoredProcedure(reportMaster, param);
        }

        private void BindingView(ReportMaster reportMaster, string[] param)
        {
            List<vReportParameter> listReportParameter = BusinessLayer.GetvReportParameterList(string.Format("ReportID = {0} ORDER BY DisplayOrder", reportMaster.ReportID));
            string filterExpression = String.Empty;
            for (int i = 0; i < listReportParameter.Count; ++i)
            {
                string filterParameter = String.Empty;
                vReportParameter reportParameter = listReportParameter[i];
                if (reportParameter.GCFilterParameterType == Constant.FilterParameterType.FREE_TEXT)
                {
                    if (i > 0 && filterExpression != "")
                        filterExpression += " AND ";
                    filterParameter += param[i];
                    filterExpression += filterParameter;
                }
                else
                {
                    if (reportParameter.GCFilterParameterType == Constant.FilterParameterType.DATE ||
                        reportParameter.GCFilterParameterType == Constant.FilterParameterType.PAST_PERIOD ||
                        reportParameter.GCFilterParameterType == Constant.FilterParameterType.UPCOMING_PERIOD)
                    {
                        if (i > 0 && filterExpression != "")
                            filterExpression += " AND ";
                        string[] date = param[i].Split(';');
                        string startDate = date[0];
                        string endDate = date[1];
                        filterParameter = string.Format("{0} BETWEEN '{1}' AND '{2}'", reportParameter.FieldName, startDate, endDate);
                        filterExpression += filterParameter;
                    }
                    else if (reportParameter.GCFilterParameterType == Constant.FilterParameterType.COMBO_BOX || reportParameter.GCFilterParameterType == Constant.FilterParameterType.YEAR_COMBO_BOX || reportParameter.GCFilterParameterType == Constant.FilterParameterType.CUSTOM_COMBO_BOX || reportParameter.GCFilterParameterType == Constant.FilterParameterType.SEARCH_DIALOG)
                    {
                        string[] paramSplit = param[i].Split(';');
                        string value = paramSplit[0];
                        if (i > 0 && filterExpression != "")
                        {
                            if (!reportParameter.IsAllowSelectAll || value != "")
                                filterExpression += " AND ";
                        }
                        if (!reportParameter.IsAllowSelectAll || value != "")
                            filterParameter = string.Format("{0} = '{1}'", reportParameter.FieldName, value);
                        filterExpression += filterParameter;
                    }
                    else
                    {
                        if (i > 0 && filterExpression != "")
                            filterExpression += " AND ";
                        string[] paramSplit = param[i].Split(';');
                        StringBuilder sbFilterExpressionVal = new StringBuilder();
                        StringBuilder sbTemp = new StringBuilder();

                        for (int idxValue = 0; idxValue < paramSplit.Length; idxValue++)
                        {
                            string value = paramSplit[idxValue];
                            if (sbTemp.ToString() != "")
                                sbTemp.Append(",");

                            sbTemp.Append("'").Append(value).Append("'");
                        }
                        sbFilterExpressionVal.Append(" IN (").Append(sbTemp.ToString()).Append(")");
                        filterParameter = string.Format("{0}{1}", reportParameter.FieldName, sbFilterExpressionVal.ToString());
                        filterExpression += filterParameter;
                    }
                }

            }
            string additionalFilterExpression = GetFilterExpression(reportMaster.AdditionalFilterExpression);
            if (filterExpression != "" && additionalFilterExpression != "")
                filterExpression += " AND ";
            filterExpression += additionalFilterExpression;

            MethodInfo method = typeof(BusinessLayer).GetMethod(reportMaster.ObjectTypeName, new[] { typeof(string) });
            object obj = method.Invoke(null, new string[] { filterExpression });
            this.DataSource = obj;
        }

        private void BindingStoredProcedure(ReportMaster reportMaster, string[] param)
        {
            List<vReportParameter> listReportParameter = BusinessLayer.GetvReportParameterList(string.Format("ReportID = {0} ORDER BY DisplayOrder", reportMaster.ReportID));
            string filterExpression = String.Empty;
            List<Variable> lstVariable = new List<Variable>();
            for (int i = 0; i < listReportParameter.Count; ++i)
            {
                string value = param[i];
                vReportParameter reportParameter = listReportParameter[i];
                lstVariable.Add(new Variable { Code = reportParameter.FieldName, Value = GetFilterExpression(value) });
                //FormatReportParameter(i.ToString(), filterParameter);
            }
            this.DataSource = BusinessLayer.GetDataReport(reportMaster.ObjectTypeName, lstVariable);
        }
    }
}

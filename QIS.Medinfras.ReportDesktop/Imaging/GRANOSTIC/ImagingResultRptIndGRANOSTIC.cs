using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class ImagingResultRptIndGRANOSTIC : BaseRpt
    {
        private string sv_Email = "";
        private string sv_Sign = "0";
        protected List<dynamic> lstDynamic = null;

        private string GetFilterExpression(string value)
        {
            StringBuilder sbResult = new StringBuilder(value);
            sbResult.Replace("@HealthcareID", appSession.HealthcareID);
            sbResult.Replace("@UserID", appSession.UserID.ToString());
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

        private string GetFilterExpression(string value, string orderby)
        {
            StringBuilder sbResult = new StringBuilder(value);
            sbResult.Replace("@HealthcareID", appSession.HealthcareID);
            sbResult.Replace("@UserID", appSession.UserID.ToString());
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
            if (!orderby.Equals(string.Empty))
            {
                if (!sbResult.ToString().ToLower().Contains("order by"))
                {
                    sbResult.Append(" ORDER BY ");
                }
                else sbResult.Append(", ");
                sbResult.Append(orderby);
            }
            return sbResult.ToString();
        }

        public ImagingResultRptIndGRANOSTIC()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            lblReportTitle.Text = reportMaster.ReportTitle1;
            lblReportSubTitle.Text = reportMaster.ReportTitle2;

            lblReportProperties.Text = string.Format("MEDINFRAS-{0}|{1}, Print Date/Time:{2}, User:{3}",
                                                reportMaster.ReportCode, reportMaster.ReportTitle1, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"), appSession.UserName);

            //Show or Hide Header
            xrLogo.Visible = reportMaster.IsShowHeader;
            tableHeader.Visible = reportMaster.IsShowHeader;

            //Show or Hide Footer
            PageFooter.Visible = reportMaster.IsShowFooter;

            //Set Top Margin
            TopMargin.HeightF = TopMargin.HeightF + reportMaster.TopMargin;

            //Load Healthcare Information
            if (reportMaster.IsShowHeader)
            {
                vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
                if (oHealthcare != null)
                {
                    xrLogo.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/logoresultGranostic.png");
                    cHealthcareAddress.Text = oHealthcare.StreetName;
                    cHealthcareCityZipCodes.Text = string.Format("{0} {1}", oHealthcare.City, oHealthcare.ZipCode);
                    cHealthcarePhone.Text = oHealthcare.PhoneNo1;
                    cHealthcareFax.Text = oHealthcare.FaxNo1;
                    cHealthcareWebsite.Text = oHealthcare.Website;
                }
            }
            if (!IsSkipBinding())
            {
                if (reportMaster.GCDataSourceType == Constant.DataSourceType.VIEW)
                    BindingView(reportMaster, param);
                else
                    BindingStoredProcedure(reportMaster, param);
            }

            vImagingResultReport entityIR = BusinessLayer.GetvImagingResultReportList(param[0]).FirstOrDefault();

            SettingParameterDt svEmail_IS = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, Constant.SettingParameter.IS_EMAIL_RADIOLOGI);
            sv_Email = svEmail_IS.ParameterValue.ToString();

            SettingParameterDt svSign_IS = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, Constant.SettingParameter.IS_NAMA_TANDA_TANGAN_CETAKAN_HASIL_RADIOLOGI);
            sv_Sign = svSign_IS.ParameterValue.ToString();

            ttdDokter.ImageUrl = string.Format("{0}{1}/Signature/{2}.png", AppConfigManager.QISPhysicalDirectory, AppConfigManager.QISParamedicImagePath, entityIR.TestRealizationPhysicianCode);
            ttdDokter.Visible = true;

            base.InitializeReport(param);
        }

        private void lblEmail_IS_Caption_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (sv_Email == "")
            {
                lblEmail_IS_Caption.Visible = false;
            }
            else
            {
                lblEmail_IS_Caption.Visible = true;
            }
        }

        private void lblEmail_IS_Symbol_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (sv_Email == "")
            {
                lblEmail_IS_Symbol.Visible = false;
            }
            else
            {
                lblEmail_IS_Symbol.Visible = true;
            }
        }

        private void lblEmail_ISS_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (sv_Email == "")
            {
                lblEmail_ISS.Visible = false;
            }
            else
            {
                lblEmail_ISS.Visible = true;
                lblEmail_ISS.Text = sv_Email;
            }
        }

        private void lblSignCaption_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (sv_Sign == "1")
            {
                lblSignCaption.Text = "Pemeriksa,";
            }
            else
            {
                lblSignCaption.Text = "Dokter Pemeriksa,";
            }
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
                    if (reportParameter.GCFilterParameterType == Constant.FilterParameterType.TIME_RANGE)
                    {
                        if (i > 0 && filterExpression != "")
                            filterExpression += " AND ";
                        string[] value = param[i].Split(';');
                        string valueFrom = value[0];
                        string valueTo = value[1];
                        filterParameter = string.Format("{0} BETWEEN '{1}' AND '{2}'", reportParameter.FieldName, valueFrom, valueTo);
                        filterExpression += filterParameter;
                    }
                    else if (reportParameter.GCFilterParameterType == Constant.FilterParameterType.DATE ||
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
                    else if (reportParameter.GCFilterParameterType == Constant.FilterParameterType.SINGLE_DATE)
                    {
                        string[] paramSplit = param[i].Split(';');
                        string value = paramSplit[0];
                        if (i > 0 && filterExpression != "")
                            filterExpression += " AND ";
                        filterParameter = string.Format("{0} = '{1}'", reportParameter.FieldName, value);
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
            SetReportParameterText(listReportParameter, param);
            string additionalFilterExpression = GetFilterExpression(reportMaster.AdditionalFilterExpression, reportMaster.ORDERExpression);
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
            }
            SetReportParameterText(listReportParameter, param);
            lstDynamic = BusinessLayer.GetDataReport(reportMaster.ObjectTypeName, lstVariable);
            this.DataSource = lstDynamic;
        }

        private void SetReportParameterText(List<vReportParameter> listReportParameter, string[] param)
        {
            if (reportMaster.IsShowParameter)
            {
                for (int i = 0; i < listReportParameter.Count; ++i)
                {
                    string value = param[i];
                    vReportParameter reportParameter = listReportParameter[i];
                    string parameterText = string.Format("{0} : ", reportParameter.FilterParameterCaption);
                    if (reportParameter.GCFilterParameterType == Constant.FilterParameterType.FREE_TEXT) { }
                    else if (reportParameter.GCFilterParameterType == Constant.FilterParameterType.TEXT_BOX ||
                        reportParameter.GCFilterParameterType == Constant.FilterParameterType.CONSTANT ||
                        reportParameter.GCFilterParameterType == Constant.FilterParameterType.YEAR_COMBO_BOX)
                        parameterText += value;
                    else if (reportParameter.GCFilterParameterType == Constant.FilterParameterType.SINGLE_DATE)
                        parameterText += Helper.YYYYMMDDToDate(value).ToString(Constant.FormatString.DATE_FORMAT);
                    else if (reportParameter.GCFilterParameterType == Constant.FilterParameterType.SEARCH_DIALOG)
                    {
                        MethodInfo method = typeof(BusinessLayer).GetMethod(reportParameter.SearchDialogMethodName, new[] { typeof(string) });
                        object tempObj = method.Invoke(null, new string[] { string.Format("{0} = {1}", reportParameter.SearchDialogIDField, value) });
                        IList list = (IList)tempObj;
                        if (list.Count > 0)
                        {
                            object obj = list[0];
                            parameterText += obj.GetType().GetProperty(reportParameter.SearchDialogNameField).GetValue(obj, null);
                        }
                    }
                    else if (reportParameter.GCFilterParameterType == Constant.FilterParameterType.COMBO_BOX)
                    {
                        MethodInfo method = typeof(BusinessLayer).GetMethod(reportParameter.MethodName, new[] { typeof(string) });
                        object tempObj = method.Invoke(null, new string[] { string.Format("{0} = '{1}'", reportParameter.ValueFieldName, value) });
                        IList list = (IList)tempObj;
                        if (list.Count > 0)
                        {
                            object obj = list[0];
                            parameterText += obj.GetType().GetProperty(reportParameter.TextFieldName).GetValue(obj, null);
                        }
                    }
                    else if (reportParameter.GCFilterParameterType == Constant.FilterParameterType.CUSTOM_COMBO_BOX)
                    {
                        string[] lstText = reportParameter.ListText.Split('|');
                        string[] lstValue = reportParameter.ListValue.Split('|');
                        for (int j = 0; j < lstValue.Length; ++j)
                        {
                            if (lstValue[j] == value)
                                parameterText += lstText[j];
                        }
                    }
                    else if (reportParameter.GCFilterParameterType == Constant.FilterParameterType.TIME_RANGE)
                    {
                        string[] temp = value.Split(';');
                        parameterText += string.Format("{0} - {1}", temp[0], temp[1]);
                    }
                    else
                    {
                        string[] temp = value.Split(';');
                        parameterText += string.Format("{0} - {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
                    }
                    //if (reportMaster.IsShowParameter)
                    //    FormatReportParameter(i.ToString(), parameterText);
                }
            }
        }

        //private void lblSignName_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        //{
        //    if (sv_Sign == "1")
        //    {
        //        lblSignName.Text = appSession.UserFullName;
        //    }
        //    else
        //    {
        //        lblSignName.Text = GetCurrentColumnValue("TestRealizationPhysician").ToString();
        //    }
        //}

    }
}

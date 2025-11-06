using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using System.Text;
using System.Reflection;

namespace QIS.Medinfras.Report
{
    public partial class BaseDailyLandscapeRpt : BaseRpt
    {
        private string GetFilterExpression(string value)
        {
            StringBuilder sbResult = new StringBuilder(value);
            sbResult.Replace("@HealthcareID", AppSession.UserLogin.HealthcareID);
            return sbResult.ToString();
        }

        public BaseDailyLandscapeRpt()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            lblReportTitle.Text = reportMaster.ReportTitle1;
            lblReportSubtitle.Text = reportMaster.ReportTitle2;
            lblReportProperties.Text = string.Format("{0}, Print Date/Time:{1}, User ID:{2}", reportMaster.ReportCode, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"), AppSession.UserLogin.UserName);

            //Show or Hide Header
            xrLogo.Visible = reportMaster.IsShowHeader;
            lblHealthcareName.Visible = reportMaster.IsShowHeader;
            lblAddressLine1.Visible = reportMaster.IsShowHeader;
            lblAddressLine2.Visible = reportMaster.IsShowHeader;
            lblPhoneFaxNo.Visible = reportMaster.IsShowHeader;
            xrLogo.ImageUrl = page.ResolveUrl("~/Libs/Images/logo.png");

            //Set Top Margin
            TopMargin.HeightF = TopMargin.HeightF + reportMaster.TopMargin;

            //Load Site Information
            if (reportMaster.IsShowHeader)
            {
                vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID))[0];
                if (oHealthcare != null)
                {
                    lblHealthcareName.Text = oHealthcare.HealthcareName;
                    lblAddressLine1.Text = oHealthcare.StreetName;
                    lblAddressLine2.Text = oHealthcare.AddressLine2;
                    lblPhoneFaxNo.Text = string.Format("Phone/Fax : {0}", string.IsNullOrEmpty(oHealthcare.FaxNo1) ? oHealthcare.PhoneNo1 : string.Format("{0}/{1}", oHealthcare.PhoneNo1, oHealthcare.FaxNo1));
                }
            }
            if (reportMaster.GCDataSourceType == Constant.DataSourceType.VIEW)
                BindingView(reportMaster, param);
            else
                BindingStoredProcedure(reportMaster, param);


            //Show or Hide Parameter
            this.lblReportParameterTitle.Visible = reportMaster.IsShowParameter;
            this.lblParameter0.Visible = this.lblReportParameterTitle.Visible;
            this.lblParameter1.Visible = this.lblReportParameterTitle.Visible;
            this.lblParameter2.Visible = this.lblReportParameterTitle.Visible;
            this.lblParameter3.Visible = this.lblReportParameterTitle.Visible;
            this.lblParameter4.Visible = this.lblReportParameterTitle.Visible;
            this.lblParameter5.Visible = this.lblReportParameterTitle.Visible;
            this.lblParameter6.Visible = this.lblReportParameterTitle.Visible;
            this.lblParameter7.Visible = this.lblReportParameterTitle.Visible;
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
                FormatReportParameter(i.ToString(), filterParameter);

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

        private void FormatReportParameter(string parameterNo, string filterParameter)
        {
            XRControl lblParameter = this.ReportFooter.FindControl(string.Format("lblParameter{0}", parameterNo), true);
            if (lblParameter != null)
            {
                lblParameter.Visible = true;
                lblParameter.Text = filterParameter;
            }
        }
    }
}

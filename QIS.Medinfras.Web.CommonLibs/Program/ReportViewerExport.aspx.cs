using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.ReportDesktop;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using Newtonsoft.Json.Linq;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ReportViewerExport : BasePage
    {
        XtraReport rpt;
        ReportMaster reportMaster;
        Dictionary<string, object> dict;
        vHealthcare oHealthcare;

        private object GetValueFromDictionary(string keyName)
        {
            decimal n;
            bool isNumeric = decimal.TryParse(keyName, out n);
            if (isNumeric)
            {
                return n;
            }
            else
            {
                if (keyName.Contains("X."))
                {
                    return keyName;
                }
                return dict[keyName.Replace("E.", "")];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Form["param"] != null)
                hdnParam.Value = Request.Form["param"].ToString();
            string[] param = hdnParam.Value.Split('|');
            oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID))[0];
            string reportCode = Page.Request.QueryString["id"];
            List<ReportMaster> lstReportMaster = BusinessLayer.GetReportMasterList(string.Format("ReportCode = '{0}'", reportCode));
            if (lstReportMaster.Count < 1)
                throw new Exception(string.Format("Report with code {0} is not defined", reportCode));
            reportMaster = lstReportMaster[0];
            string reportClassName = reportMaster.ClassName;

            InsertReportPrintLog(reportCode, hdnParam.Value);

            BaseRpt report = GetReport(reportClassName);

            AppSessionReport appSession = new AppSessionReport();
            appSession.HealthcareID = AppSession.UserLogin.HealthcareID;
            appSession.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
            appSession.UserID = AppSession.UserLogin.UserID;
            appSession.UserName = AppSession.UserLogin.UserName;
            appSession.UserFullName = AppSession.UserLogin.UserFullName;
            appSession.ReportFooterPrintedByInfo = AppSession.ReportFooterPrintedByInfo;
            report.Init(appSession, reportMaster.ReportID, reportCode, param);
            this.ReportViewer1.Report = report;

            bool isStreamToPDF = AppSession.PreviewReportInPDF == "1" && reportMaster.IsUsingPreview == false;

            if (isStreamToPDF)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    PdfExportOptions opts = new PdfExportOptions();
                    //opts.ShowPrintDialogOnOpen = true;
                    this.ReportViewer1.Report.ExportToPdf(ms, opts);
                    ms.Seek(0, SeekOrigin.Begin);
                    byte[] _report = ms.ToArray();
                    Page.Response.ContentType = "application/pdf";
                    Page.Response.Clear();
                    Page.Response.OutputStream.Write(_report, 0, _report.Length);
                    Page.Response.End();
                }
            }
            else
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    //XlsExportOptions xlsExportOptions = new XlsExportOptions()
                    //{
                    //    ExportMode = XlsExportMode.SingleFile,
                    //    ShowGridLines = true
                    //};

                    // This returns something like C:\Users\Username:
                    string userRoot = string.Format("{0}\\_download_temp\\", AppConfigManager.QISPhysicalDirectory); 
                    // Now let's get C:\Users\Username\Downloads:

                    if (!Directory.Exists(userRoot))
                    {
                        Directory.CreateDirectory(userRoot);
                    }

                    string fileName = string.Format("{0}_{1}_{2}_{3}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, reportClassName);

                    string xlsExportFile =
                        userRoot +
                        //@"\Downloads\" +
                        fileName +
                        ".xls";

                    // Export the report.
                    //report.ExportToXls(xlsExportFile, xlsExportOptions);

                    XlsExportOptions opts = new XlsExportOptions();
                    this.ReportViewer1.Report.ExportToXls(xlsExportFile, opts);
                    //opts.ShowPrintDialogOnOpen = true;

                    try
                    {
                        System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
                        response.ClearContent();
                        response.Clear();
                        response.ContentType = "Application/x-msexcel";
                        response.AppendHeader("Content-Disposition", string.Format("attachment; filename={0}", fileName + ".xls"));
                        //response.TransmitFile(string.Format("{0}\\Files\\{1}", AppConfigManager.QISPhysicalDirectory, fileName));
                        response.TransmitFile(xlsExportFile);
                        response.Flush();
                        //response.End();
                    }
                    catch (Exception ex)
                    {

                    }

                    Response.Write("<script>window.close();</script>");
                    //Page.Response.Close();

                    File.Delete(xlsExportFile);
                    Page.Response.End();
                }
            }
        }

        void report_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

        public BaseRpt GetReport(string className)
        {
            Assembly assembly = Assembly.Load("QIS.Medinfras.ReportDesktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            Object o = assembly.CreateInstance("QIS.Medinfras.ReportDesktop." + className);
            return (BaseRpt)o;
        }

        public void reportDefaultSettings()
        {
            XRControl lblHealthcareName = rpt.Report.FindControl("lblHealthcareName", false);
            XRControl lblAddressLine1 = rpt.Report.FindControl("lblAddressLine1", false);
            XRControl lblAddressLine2 = rpt.Report.FindControl("lblAddressLine2", false);
            XRControl lblPhoneFaxNo = rpt.Report.FindControl("lblPhoneFaxNo", false);
            XRControl lblReportTitle = rpt.Report.FindControl("lblReportTitle", false);
            XRControl lblReportSubtitle = rpt.Report.FindControl("lblReportSubtitle", false);
            XRControl lblReportProperties = rpt.Report.FindControl("lblReportProperties", false);
            XRPictureBox xrLogo = (XRPictureBox)rpt.Report.FindControl("xrLogo", false);
            if (lblReportTitle != null)
            {
                lblReportTitle.Text = reportMaster.ReportTitle1;
                lblReportSubtitle.Text = reportMaster.ReportTitle2;
                lblReportProperties.Text = string.Format("MEDINFRAS - {0}, Print Date/Time:{1}, User ID:{2}", reportMaster.ReportCode, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"), AppSession.UserLogin.UserName);

                //Show or Hide Header
                xrLogo.Visible = reportMaster.IsShowHeader;
                lblHealthcareName.Visible = reportMaster.IsShowHeader;
                lblAddressLine1.Visible = reportMaster.IsShowHeader;
                lblAddressLine2.Visible = reportMaster.IsShowHeader;
                lblPhoneFaxNo.Visible = reportMaster.IsShowHeader;
                xrLogo.ImageUrl = ResolveUrl("~/Libs/Images/logo.png");

                rpt.Bands.GetBandByType(typeof(TopMarginBand)).HeightF = rpt.Bands.GetBandByType(typeof(TopMarginBand)).HeightF + reportMaster.TopMargin;
                if (reportMaster.IsShowHeader)
                {
                    if (oHealthcare != null)
                    {
                        lblHealthcareName.Text = oHealthcare.HealthcareName;
                        lblAddressLine1.Text = oHealthcare.StreetName;
                        lblAddressLine2.Text = oHealthcare.AddressLine2;
                        lblPhoneFaxNo.Text = string.Format("Phone/Fax : {0}", string.IsNullOrEmpty(oHealthcare.FaxNo1) ? oHealthcare.PhoneNo1 : string.Format("{0}/{1}", oHealthcare.PhoneNo1, oHealthcare.FaxNo1));
                    }
                }
            }
        }

        private void BindingView(string[] param)
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
            string additionalFilterExpression = GetFilterExpression(reportMaster.AdditionalFilterExpression);
            if (filterExpression != "" && additionalFilterExpression != "")
                filterExpression += " AND ";
            filterExpression += additionalFilterExpression;

            MethodInfo method = typeof(BusinessLayer).GetMethod(reportMaster.ObjectTypeName, new[] { typeof(string) });
            object obj = method.Invoke(null, new string[] { filterExpression });
            rpt.DataSource = obj;
        }

        private void BindingStoredProcedure(string[] param)
        {
            List<vReportParameter> listReportParameter = BusinessLayer.GetvReportParameterList(string.Format("ReportID = {0} ORDER BY DisplayOrder", reportMaster.ReportID));
            string filterExpression = String.Empty;
            List<Variable> lstVariable = new List<Variable>();
            for (int i = 0; i < listReportParameter.Count; ++i)
            {
                string value = param[i];
                vReportParameter reportParameter = listReportParameter[i];
                lstVariable.Add(new Variable { Code = reportParameter.FieldName, Value = GetFilterExpression(value) });

                filterExpression += string.Format("{0} = {1}|", reportParameter.FieldName, GetFilterExpression(value));
            }
            SetReportParameterText(listReportParameter, param);

            rpt.DataSource = BusinessLayer.GetDataReport(reportMaster.ObjectTypeName, lstVariable);
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
                    if (reportMaster.IsShowParameter)
                        FormatReportParameter(i.ToString(), parameterText);
                }
            }
        }

        private string GetFilterExpression(string value)
        {
            StringBuilder sbResult = new StringBuilder(value);
            sbResult.Replace("@HealthcareID", AppSession.UserLogin.HealthcareID);
            sbResult.Replace("@ParamedicID", Convert.ToString(AppSession.UserLogin.ParamedicID));
            sbResult.Replace("@UserID", AppSession.UserLogin.UserID.ToString());
            if (value.Contains("@LaboratoryID"))
            {
                string laboratoryID = BusinessLayer.GetSettingParameter(Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).ParameterValue;
                vHealthcareServiceUnit HSU = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND ServiceUnitID = {1} AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, laboratoryID))[0];
                sbResult.Replace("@LaboratoryID", HSU.HealthcareServiceUnitID.ToString());
            }
            if (value.Contains("@ImagingID"))
            {
                string imagingID = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID).ParameterValue;
                vHealthcareServiceUnit HSU = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND ServiceUnitID = {1} AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, imagingID))[0];
                sbResult.Replace("@ImagingID", HSU.HealthcareServiceUnitID.ToString());
            }
            if (value.Contains("@AnamneseID"))
            {
                string anamneseID = BusinessLayer.GetSettingParameter(Constant.SettingParameter.ANAMNESE_SERVICE_UNIT_ID).ParameterValue;
                vHealthcareServiceUnit HSU = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND ServiceUnitID = {1} AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, anamneseID))[0];
                sbResult.Replace("@AnamneseID", HSU.HealthcareServiceUnitID.ToString());
            }
            return sbResult.ToString();
        }

        private void FormatReportParameter(string parameterNo, string filterParameter)
        {
            XRControl lblParameter = rpt.Bands.GetBandByType(typeof(ReportFooterBand)).FindControl(string.Format("lblParameter{0}", parameterNo), true);
            if (lblParameter != null)
            {
                lblParameter.Visible = true;
                lblParameter.Text = filterParameter;
            }
        }

        protected bool InsertReportPrintLog(string ReportCode, string ReportParameter)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ReportPrintLogDao entityDao = new ReportPrintLogDao(ctx);
            try
            {
                if (ReportCode != null && ReportCode != "")
                {
                    ReportMaster rm = BusinessLayer.GetReportMasterList(string.Format("ReportCode = '{0}'", ReportCode), ctx).FirstOrDefault();

                    ReportPrintLog entity = new ReportPrintLog();
                    entity.ReportID = rm.ReportID;
                    entity.ReportCode = rm.ReportCode;
                    entity.ReportParameter = ReportParameter;
                    entity.PrintedBy = AppSession.UserLogin.UserID;
                    entity.PrintedDate = DateTime.Now;
                    entityDao.Insert(entity);

                    ctx.CommitTransaction();
                }
            }
            catch (Exception ex)
            {
                result = false;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
    }
}
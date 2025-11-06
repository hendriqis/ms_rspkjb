using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Mail;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.XtraReports.UI;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Program;
using QIS.Medinfras.ReportDesktop;
using PdfSharp;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf.Security;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class TransRevenueSharingSummaryDownload1 : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.TRANS_REVENUE_SHARING_SUMMARY_DOWNLOAD;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            txtPeriodFrom.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPeriodTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            string filterSetvar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_SMPT_CLIENT_CONF, Constant.SettingParameter.FN_TEMPLATE_TEXT_EMAIL_SRS, Constant.SettingParameter.FN_REPORT_CODE_FOR_EMAIL_SRS, Constant.SettingParameter.FN_LIST_CC_EMAIL_FOR_EMAIL_SRS);
            List<SettingParameterDt> lstSetvar = BusinessLayer.GetSettingParameterDtList(filterSetvar);

            hdnConf.Value = lstSetvar.Where(t => t.ParameterCode == Constant.SettingParameter.FN_SMPT_CLIENT_CONF).FirstOrDefault().ParameterValue;
            hdnTemplateEmail.Value = lstSetvar.Where(t => t.ParameterCode == Constant.SettingParameter.FN_TEMPLATE_TEXT_EMAIL_SRS).FirstOrDefault().ParameterValue;
            hdnReportCode.Value = lstSetvar.Where(t => t.ParameterCode == Constant.SettingParameter.FN_REPORT_CODE_FOR_EMAIL_SRS).FirstOrDefault().ParameterValue;
            hdnListCCEmail.Value = lstSetvar.Where(t => t.ParameterCode == Constant.SettingParameter.FN_LIST_CC_EMAIL_FOR_EMAIL_SRS).FirstOrDefault().ParameterValue;

            BindGridView();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridView();
                result = "refresh|";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView()
        {
            string dateFrom = Helper.GetDatePickerValue(txtPeriodFrom.Text).ToString(Constant.FormatString.DATE_FORMAT_112);
            string dateTo = Helper.GetDatePickerValue(txtPeriodTo.Text).ToString(Constant.FormatString.DATE_FORMAT_112);

            string filterExpression = string.Format("GCTransactionStatus IN ('{0}','{1}') AND (RSSummaryDate BETWEEN '{2}' AND '{3}')", Constant.TransactionStatus.APPROVED, Constant.TransactionStatus.CLOSED, dateFrom, dateTo);
            int paramedicID = 0;
            if (hdnParamedicID.Value != "" && hdnParamedicID.Value != null)
            {
                filterExpression += string.Format(" AND ParamedicID = '{0}'", hdnParamedicID.Value);
            }

            List<vTransRevenueSharingSummaryHd1> lstEntity = BusinessLayer.GetvTransRevenueSharingSummaryHd1List(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientNotificationDao patientNotificationDao = new PatientNotificationDao(ctx);
            try
            {
                if (type == "email")
                {
                    string[] paramSplit = hdnConf.Value.Split('|');
                    #region CreatePDF
                    string lstFile = "";
                    String[] paramID = hdnSelectedMember.Value.Substring(1).Split(',');
                    string paramList = "";
                    for (int ct = 0; ct < paramID.Length; ct++)
                    {
                        if (String.IsNullOrEmpty(paramList))
                        {
                            paramList = string.Format("{0}", paramID[ct]);
                        }
                        else
                        {
                            paramList += string.Format(",{0}", paramID[ct]);
                        }
                    }

                    string filterExpression = string.Format("RSSummaryID IN({0}) AND Revenue = 1", paramList);
                    List<vTransRevenueSharingSummaryHd> lstEntity = BusinessLayer.GetvTransRevenueSharingSummaryHdList(filterExpression);
                    if (lstEntity.Count > 0)
                    {
                        foreach (vTransRevenueSharingSummaryHd e in lstEntity)
                        {
                            string param = string.Format("RSSummaryID = '{0}'", e.RSSummaryID);
                            string reportCode = hdnReportCode.Value;
                            string HealthcareID = "001";
                            string ParamedicCode = e.ParamedicCode;
                            string fileName = string.Format("{0}_{1}_{2}", e.cfRSSummaryDateInString, e.ParamedicCode, e.ParamedicName);

                            string pathfile = GetFileReport(param, reportCode, HealthcareID, ParamedicCode, fileName, e.RSSummaryNo);
                            if (String.IsNullOrEmpty(lstFile))
                            {
                                lstFile = pathfile;
                            }
                            else
                            {
                                lstFile += string.Format("|{0}", pathfile);
                            }
                        }
                    }
                    #endregion

                    #region SendEmail
                    List<vTransRevenueSharingSummaryHd> lstParamedic = lstEntity.GroupBy(test => test.ParamedicID).Select(grp => grp.First()).ToList().OrderBy(x => x.ParamedicID).ToList();
                    foreach (vTransRevenueSharingSummaryHd e in lstParamedic)
                    {
                        MailMessage message = new MailMessage();
                        message.From = new MailAddress(paramSplit[2], string.Format("{0}", AppSession.UserLogin.HealthcareName));
                        message.To.Add(new MailAddress(e.EmailAddress, e.ParamedicName));
                        message.Subject = string.Format("Slip Honor Dokter {0}", e.ParamedicCode);
                        message.IsBodyHtml = true; //to make message body as html

                        if (!String.IsNullOrEmpty(hdnListCCEmail.Value))
                        {
                            string[] lstCC = hdnListCCEmail.Value.Split('|');
                            foreach (string cc in lstCC)
                            {
                                message.CC.Add(cc);
                            }
                        }

                        string body = hdnTemplateEmail.Value.Replace("[ParamedicName]", e.ParamedicName).Replace("[Period]", string.Format("Bulan {0} {1}", e.cfRSSummaryMonthName, e.RSSummaryDate.Year));
                        message.Body = body;

                        string path = AppConfigManager.QISPhysicalDirectory;
                        path += string.Format("{0}\\{1}\\", AppConfigManager.QISParamedicImagePath.Replace('/', '\\'), "");
                        path += string.Format("{0}\\", e.ParamedicCode);
                        if (Directory.Exists(path))
                        {
                            string[] files = Directory.GetFiles(path);
                            foreach (var file in files)
                            {
                                message.Attachments.Add(new Attachment(file));
                            }
                        }
                        SmtpClient smtp = new SmtpClient();
                        smtp.UseDefaultCredentials = false;
                        smtp.Port = Convert.ToInt32(paramSplit[1]);
                        smtp.Host = paramSplit[0];
                        smtp.EnableSsl = true;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.Credentials = new NetworkCredential(paramSplit[2], paramSplit[3]);
                        smtp.Send(message);
                        message.Dispose();

                        List<vTransRevenueSharingSummaryHd> lstData = lstEntity.Where(t => t.ParamedicID == e.ParamedicID).ToList();
                        foreach (vTransRevenueSharingSummaryHd s in lstData)
                        {
                            PatientNotification entity = new PatientNotification();
                            entity.GCMailTypeOrder = Constant.MailTypeOrder.SLIP_HONOR_DOKTER;
                            entity.MailTo = s.EmailAddress;
                            entity.ReportCode = hdnReportCode.Value;
                            entity.Remarks = s.RSSummaryNo;
                            entity.GCMailStatus = Constant.MailNotificationStatus.SEND_PROCESSED;
                            entity.SentBy = AppSession.UserLogin.UserID;
                            entity.SentDate = DateTime.Now;
                            patientNotificationDao.Insert(entity);
                        }
                    }
                    #endregion

                    #region delete
                    string[] fileLst = lstFile.Split('|');
                    if (fileLst.Count() == 1)
                    {
                        byte[] data = System.IO.File.ReadAllBytes(fileLst[0]);
                        if (File.Exists(fileLst[0]))
                        {
                            File.Delete(fileLst[0]);
                        }
                    }
                    else
                    {
                        byte[] data = System.IO.File.ReadAllBytes(fileLst[0]);
                        for (int i = 0; i < fileLst.Count(); i++)
                        {
                            if (File.Exists(fileLst[i]))
                            {
                                File.Delete(fileLst[i]);
                            }
                        }
                    }
                    #endregion

                    ctx.CommitTransaction();
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }

            return result;
        }

        static private void CopyPages(PdfDocument from, PdfDocument to)
        {
            for (int i = 0; i < from.PageCount; i++)
            {
                to.AddPage(from.Pages[i]);
            }
        }

        static private string GetFileReport(string Param, string ReportCode, string HealthcareID, string ParamedicCode, string fileName, string srsNo)
        {
            string path = AppConfigManager.QISPhysicalDirectory;
            path += string.Format("{0}\\{1}\\", AppConfigManager.QISParamedicImagePath.Replace('/', '\\'), "");
            path += string.Format("{0}\\", ParamedicCode);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //Random rand = new Random(DateTime.Now.Second);
            //int num = rand.Next();

            string FileName = string.Format("{0}_{1}.pdf", fileName, srsNo.Replace('/', '_'));
            string PathFilePDF = Path.Combine(path, FileName);
            AppSessionReport appSession = new AppSessionReport();

            string temp = Param; // "1047";
            string reportCode = ReportCode; // "MR000010";

            appSession.HealthcareID = HealthcareID;
            appSession.UserID = Convert.ToInt32(1);
            appSession.UserName = "system";
            appSession.UserFullName = "system";

            string[] param = temp.Split('|');
            List<ReportMaster> lstReportMaster = BusinessLayer.GetReportMasterList(string.Format("ReportCode = '{0}'", reportCode));
            if (lstReportMaster.Count < 1)
                throw new Exception(string.Format("Report with code {0} is not defined", reportCode));
            ReportMaster reportMaster = lstReportMaster[0];
            string reportClassName = reportMaster.ClassName;
            BaseRpt report = GetReport(reportClassName);
            report.Init(appSession, reportMaster.ReportID, reportCode, param);
            ReportPrintTool tool = new ReportPrintTool(report);
            tool.Report.CreateDocument(false);
            tool.PrintingSystem.ExportToPdf(PathFilePDF);

            #region Set Password
            PdfDocument document = PdfReader.Open(PathFilePDF, "");

            PdfSecuritySettings securitySettings = document.SecuritySettings;

            // Setting one of the passwords automatically sets the security level to 
            // PdfDocumentSecurityLevel.Encrypted128Bit.
            securitySettings.UserPassword = ParamedicCode;
            securitySettings.OwnerPassword = ParamedicCode;

            // Don't use 40 bit encryption unless needed for compatibility reasons
            //securitySettings.DocumentSecurityLevel = PdfDocumentSecurityLevel.Encrypted40Bit;

            // Restrict some rights.
            securitySettings.PermitAccessibilityExtractContent = false;
            securitySettings.PermitAnnotations = false;
            securitySettings.PermitAssembleDocument = false;
            securitySettings.PermitExtractContent = false;
            securitySettings.PermitFormsFill = true;
            securitySettings.PermitFullQualityPrint = false;
            securitySettings.PermitModifyDocument = true;
            securitySettings.PermitPrint = false;

            // Save the document...
            document.Save(PathFilePDF);
            #endregion

            return PathFilePDF;
            //byte[] data = System.IO.File.ReadAllBytes(PathFilePDF);
            //string base64 = Convert.ToBase64String(data);
            //return base64;
        }

        static public BaseRpt GetReport(string className)
        {
            Assembly assembly = Assembly.Load("QIS.Medinfras.ReportDesktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            Object o = assembly.CreateInstance("QIS.Medinfras.ReportDesktop." + className);
            return (BaseRpt)o;
        }
    }
}
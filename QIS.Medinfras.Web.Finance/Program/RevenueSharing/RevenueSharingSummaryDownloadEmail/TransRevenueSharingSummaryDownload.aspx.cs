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

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class TransRevenueSharingSummaryDownload : BasePageTrx
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

            string filterExpression = string.Format("GCTransactionStatus = '{0}' AND (RSSummaryDate BETWEEN '{1}' AND '{2}')", Constant.TransactionStatus.APPROVED, dateFrom, dateTo);
            int paramedicID = 0;
            if (hdnParamedicID.Value != "" && hdnParamedicID.Value != null)
            {
                filterExpression += string.Format(" AND ParamedicID = '{0}'", hdnParamedicID.Value);
            }

            List<vTransRevenueSharingSummaryHd> lstEntity = BusinessLayer.GetvTransRevenueSharingSummaryHdList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientNotificationDao patientNotificationDao = new PatientNotificationDao(ctx);
            try
            {
                if (type == "download")
                {
                    #region Download Document
                    string lstFile = "";

                    string dateFrom = Helper.GetDatePickerValue(txtPeriodFrom.Text).ToString(Constant.FormatString.DATE_FORMAT_112);
                    string dateTo = Helper.GetDatePickerValue(txtPeriodTo.Text).ToString(Constant.FormatString.DATE_FORMAT_112);

                    string filterExpression = string.Format("GCTransactionStatus = '{0}' AND (RSSummaryDate BETWEEN '{1}' AND '{2}')", Constant.TransactionStatus.APPROVED, dateFrom, dateTo);
                    int paramedicID = 0;
                    if (hdnParamedicID.Value != "" && hdnParamedicID.Value != null)
                    {
                        filterExpression += string.Format(" AND ParamedicID = '{0}'", hdnParamedicID.Value);
                    }

                    List<vTransRevenueSharingSummaryHd> lstEntity = BusinessLayer.GetvTransRevenueSharingSummaryHdList(filterExpression);
                    if (lstEntity.Count > 0)
                    {
                        foreach (vTransRevenueSharingSummaryHd e in lstEntity)
                        {
                            string param = string.Format("RSSummaryID = '{0}'", e.RSSummaryID);
                            string reportCode = "FN-00136";
                            string HealthcareID = "001";
                            string ParamedicCode = e.ParamedicCode;
                            string fileName = string.Format("{0}_{1}_{2}", e.cfRSSummaryDateInString, e.ParamedicCode, e.ParamedicName);

                            string pathfile = GetFileReport(param, reportCode, HealthcareID, ParamedicCode, fileName);
                            if (String.IsNullOrEmpty(lstFile))
                            {
                                lstFile = pathfile;
                            }
                            else
                            {
                                lstFile += string.Format("|{0}", pathfile);
                            }

                            //retval = pathfile;
                        }

                        string[] fileLst = lstFile.Split('|');
                        if (fileLst.Count() == 1)
                        {
                            byte[] data = System.IO.File.ReadAllBytes(fileLst[0]);
                            string base64 = Convert.ToBase64String(data);

                            if (File.Exists(fileLst[0]))
                            {
                                File.Delete(fileLst[0]);
                            }

                            retval = base64;
                        }
                        else
                        {
                            string file1 = fileLst[0];
                            for (int i = 1; i < fileLst.Count(); i++)
                            {
                                using (PdfDocument one = PdfReader.Open(file1, PdfDocumentOpenMode.Import))
                                using (PdfDocument two = PdfReader.Open(fileLst[i], PdfDocumentOpenMode.Import))
                                {
                                    CopyPages(two, one);
                                    one.Save(file1);
                                }
                            }
                            byte[] data = System.IO.File.ReadAllBytes(fileLst[0]);
                            string base64 = Convert.ToBase64String(data);

                            for (int i = 0; i < fileLst.Count(); i++)
                            {
                                if (File.Exists(fileLst[i]))
                                {
                                    File.Delete(fileLst[i]);
                                }
                            }
                            retval = base64;
                        }
                    }
                    #endregion
                }
                else if (type == "email")
                {
                    #region Download Document
                    string lstFile = "";

                    string dateFrom = Helper.GetDatePickerValue(txtPeriodFrom.Text).ToString(Constant.FormatString.DATE_FORMAT_112);
                    string dateTo = Helper.GetDatePickerValue(txtPeriodTo.Text).ToString(Constant.FormatString.DATE_FORMAT_112);

                    string filterExpression = string.Format("GCTransactionStatus = '{0}' AND (RSSummaryDate BETWEEN '{1}' AND '{2}')", Constant.TransactionStatus.APPROVED, dateFrom, dateTo);
                    int paramedicID = 0;
                    if (hdnParamedicID.Value != "" && hdnParamedicID.Value != null)
                    {
                        filterExpression += string.Format(" AND ParamedicID = '{0}'", hdnParamedicID.Value);
                    }

                    List<vTransRevenueSharingSummaryHd> lstEntity = BusinessLayer.GetvTransRevenueSharingSummaryHdList(filterExpression);
                    if (lstEntity.Count > 0)
                    {
                        foreach (vTransRevenueSharingSummaryHd e in lstEntity)
                        {
                            string param = string.Format("RSSummaryID = '{0}'", e.RSSummaryID);
                            string reportCode = "FN-00136";
                            string HealthcareID = "001";
                            string ParamedicCode = e.ParamedicCode;
                            string fileName = string.Format("{0}_{1}_{2}", e.cfRSSummaryDateInString, e.ParamedicCode, e.ParamedicName);

                            string pathfile = GetFileReport(param, reportCode, HealthcareID, ParamedicCode, fileName);
                            if (String.IsNullOrEmpty(lstFile))
                            {
                                lstFile = pathfile;
                            }
                            else
                            {
                                lstFile += string.Format("|{0}", pathfile);
                            }

                            //retval = pathfile;
                        }

                        string[] fileLst = lstFile.Split('|');
                        if (fileLst.Count() == 1)
                        {
                            byte[] data = System.IO.File.ReadAllBytes(fileLst[0]);
                            string base64 = Convert.ToBase64String(data);

                            if (File.Exists(fileLst[0]))
                            {
                                File.Delete(fileLst[0]);
                            }

                            retval = base64;
                        }
                        else
                        {
                            string file1 = fileLst[0];
                            for (int i = 1; i < fileLst.Count(); i++)
                            {
                                using (PdfDocument one = PdfReader.Open(file1, PdfDocumentOpenMode.Import))
                                using (PdfDocument two = PdfReader.Open(fileLst[i], PdfDocumentOpenMode.Import))
                                {
                                    CopyPages(two, one);
                                    one.Save(file1);
                                }
                            }
                            byte[] data = System.IO.File.ReadAllBytes(fileLst[0]);
                            string base64 = Convert.ToBase64String(data);

                            for (int i = 0; i < fileLst.Count(); i++)
                            {
                                if (File.Exists(fileLst[i]))
                                {
                                    File.Delete(fileLst[i]);
                                }
                            }
                            retval = base64;
                        }

                        PatientNotification entity = new PatientNotification();
                        entity.GCMailTypeOrder = Constant.MailTypeOrder.SLIP_HONOR_DOKTER;
                        entity.Message = retval;
                        entity.MailTo = BusinessLayer.GetParamedicMaster(Convert.ToInt32(hdnParamedicID.Value)).EmailAddress1;
                        entity.ReportCode = "FN-00136";
                        entity.GCMailStatus = Constant.MailNotificationStatus.OPENED;
                        entity.CreatedBy = entity.SentBy = AppSession.UserLogin.UserID;
                        entity.CreatedDate = DateTime.Now;
                        patientNotificationDao.Insert(entity);
                        ctx.CommitTransaction();
                    }
                    #endregion
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

        //using (PdfDocument one = PdfReader.Open("file1.pdf", PdfDocumentOpenMode.Import))
        //using (PdfDocument two = PdfReader.Open("file2.pdf", PdfDocumentOpenMode.Import))
        //using (PdfDocument outPdf = new PdfDocument())
        //{                
        //    CopyPages(one, outPdf);
        //    CopyPages(two, outPdf);

        //    outPdf.Save("file1and2.pdf");
        //}

        static private void CopyPages(PdfDocument from, PdfDocument to)
        {
            for (int i = 0; i < from.PageCount; i++)
            {
                to.AddPage(from.Pages[i]);
            }
        }

        static private string GetFileReport(string Param, string ReportCode, string HealthcareID, string ParamedicCode, string fileName)
        {
            string path = AppConfigManager.QISPhysicalDirectory;
            path += string.Format("{0}\\{1}\\", AppConfigManager.QISParamedicImagePath.Replace('/', '\\'), "");
            path += string.Format("{0}\\", ParamedicCode);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            Random rand = new Random(DateTime.Now.Second);
            int num = rand.Next();

            string FileName = string.Format("{0}_{1}.pdf", fileName, num);
            string PathFilePDF = Path.Combine(path, FileName);
            AppSessionReport appSession = new AppSessionReport();

            string temp = Param; // "1047";
            string reportCode = ReportCode; // "MR000010";

            appSession.HealthcareID = AppSession.UserLogin.HealthcareID;
            appSession.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
            appSession.UserID = AppSession.UserLogin.UserID;
            appSession.UserName = AppSession.UserLogin.UserName;
            appSession.UserFullName = AppSession.UserLogin.UserFullName;
            appSession.ReportFooterPrintedByInfo = AppSession.ReportFooterPrintedByInfo;

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
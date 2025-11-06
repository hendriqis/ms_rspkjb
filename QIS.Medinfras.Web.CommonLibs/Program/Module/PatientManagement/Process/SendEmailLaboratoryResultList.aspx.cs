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
//using PdfSharp;
//using PdfSharp.Pdf;
//using PdfSharp.Pdf.IO;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class SendEmailLaboratoryResultList : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Laboratory.SENT_EMAIL_LABORATORY_RESULT;
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

            string deptFilter = string.Format("IsHasRegistration = 1 AND IsActive = 1 AND DepartmentID != '{0}'", Constant.Facility.PHARMACY);
            List<Department> lstDepartment = BusinessLayer.GetDepartmentList(deptFilter);
            Methods.SetComboBoxField<Department>(cboDepartment, lstDepartment, "DepartmentName", "DepartmentID");
            cboDepartment.SelectedIndex = 0;

            SettingParameterDt setvarDt = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.LB_SMPT_CLIENT_CONF);
            hdnConf.Value = setvarDt.ParameterValue;
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

            List<GetRegistrationHasLabResult> lstEntity = BusinessLayer.GetRegistrationHasLabResult(dateFrom, dateTo, cboDepartment.Value.ToString(), Convert.ToInt32(hdnServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value));
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

                    string filterExpression = string.Format("VisitID IN ({0})", paramList);
                    List<vConsultVisit17> lstEntity = BusinessLayer.GetvConsultVisit17List(filterExpression);
                    List<vConsultVisit17> lstPatient = lstEntity.GroupBy(test => test.MedicalNo).Select(grp => grp.First()).ToList().OrderBy(x => x.MedicalNo).ToList();
                    if (lstPatient.Count > 0)
                    {
                        foreach (vConsultVisit17 e in lstPatient)
                        {
                            string filterResult = string.Format("VisitID = '{0}' AND GCTransactionStatus != '{1}'", e.VisitID, Constant.TransactionStatus.VOID);
                            List<LaboratoryResultHd> lstResult = BusinessLayer.GetLaboratoryResultHdList(filterResult);
                            foreach (LaboratoryResultHd hd in lstResult)
                            {
                                PatientChargesHd chargesHd = BusinessLayer.GetPatientChargesHd(hd.ChargeTransactionID);
                                string param = string.Format("ID = '{0}'", hd.ID);
                                string reportCode = "LB-00001";
                                string HealthcareID = "001";
                                string fileName = string.Format("{0}", chargesHd.TransactionNo.Replace('/','_'));

                                string pathfile = GetFileReport(param, reportCode, HealthcareID, e.MedicalNo, fileName);
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
                    }
                    #endregion

                    #region SendEmail
                    foreach (vConsultVisit17 e in lstPatient)
                    {
                        MailMessage message = new MailMessage();
                        message.From = new MailAddress(paramSplit[2], string.Format("{0}", AppSession.UserLogin.HealthcareName));
                        message.To.Add(new MailAddress(e.EmailAddress, e.PatientName));
                        message.Subject = string.Format("Hasil Pemeriksaan Laboratorium {0}", e.PatientName);
                        message.IsBodyHtml = true; //to make message body as html  
                        message.Body = string.Format("Terlampir Hasil Pemeriksaan Laboratorium {0} Untuk Periode {1} Sampai dengan {2}", e.PatientName, Helper.GetDatePickerValue(txtPeriodFrom.Text).ToString(Constant.FormatString.DATE_PICKER_FORMAT), Helper.GetDatePickerValue(txtPeriodTo.Text).ToString(Constant.FormatString.DATE_PICKER_FORMAT));

                        string path = AppConfigManager.QISPhysicalDirectory;
                        //path += string.Format("{0}\\{1}\\", AppConfigManager.QISParamedicImagePath.Replace('/', '\\'), "");
                        path += string.Format("LaboratoryResult\\{0}\\", e.MedicalNo);
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

                        PatientNotification entity = new PatientNotification();
                        entity.GCMailTypeOrder = Constant.MailTypeOrder.HASIL_LABORATORIUM;
                        entity.MailTo = e.EmailAddress;
                        entity.ReportCode = "LB-00001";
                        entity.Remarks = e.MedicalNo;
                        entity.GCMailStatus = Constant.MailNotificationStatus.SEND_PROCESSED;
                        entity.SentBy = AppSession.UserLogin.UserID;
                        entity.SentDate = DateTime.Now;
                        patientNotificationDao.Insert(entity);
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

        //static private void CopyPages(PdfDocument from, PdfDocument to)
        //{
        //    for (int i = 0; i < from.PageCount; i++)
        //    {
        //        to.AddPage(from.Pages[i]);
        //    }
        //}

        static private string GetFileReport(string Param, string ReportCode, string HealthcareID, string medicalNo, string fileName)
        {
            string path = AppConfigManager.QISPhysicalDirectory;
            //path += string.Format("{0}\\{1}\\", AppConfigManager.QISPatientDocumentsPath.Replace('/', '\\'), "");
            path += string.Format("LaboratoryResult\\{0}\\", medicalNo);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //Random rand = new Random(DateTime.Now.Second);
            //int num = rand.Next();

            string FileName = string.Format("{0}.pdf", fileName);
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
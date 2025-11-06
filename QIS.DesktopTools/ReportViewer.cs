using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.ReportDesktop;
using QIS.Medinfras.Web.Common;
using System.Xml.Linq;
using System.Configuration;

namespace QIS.DesktopTools
{
    public partial class ReportViewer : Form
    {
        public ReportViewer(string[] args)
        {
            InitializeComponent();
            try
            {
                AppSessionReport appSession = new AppSessionReport();

                string temp = "";
                string reportCode = "SA090700";
                if (args.Length > 0)
                {
                    appSession.HealthcareID = args[0];
                    appSession.UserID = Convert.ToInt32(args[1]);
                    appSession.UserName = args[2];
                    appSession.UserFullName = args[3];
                    reportCode = args[4];
                    if (args.Length > 5)
                        temp = args[5];
                }
                string[] param = temp.Split('|');

                //Initialize the connection strings
                //XDocument xmlConnection = XDocument.Load("dao.config");
                //string encryptedConnStr = xmlConnection.Element("connectionStrings").Element("add").Attribute("connectionString").Value;

                //#region Decrypt Connection String
                //string connStr = DecryptText(xmlConnection.Element("connectionStrings").Element("add").Attribute("connectionString").Value);
                //xmlConnection.Element("connectionStrings").Element("add").Attribute("connectionString").Value = connStr;
                //xmlConnection.Save("ConnectionString.config");    
                            
                //Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                //config.AppSettings.Settings["connectionStrings"].Value = "myUserId";     
                //config.Save(ConfigurationSaveMode.Modified);
                //#endregion                

                List<ReportMaster> lstReportMaster = BusinessLayer.GetReportMasterList(string.Format("ReportCode = '{0}'", reportCode));

                //#region Restore Connection Strings
                //xmlConnection.Element("connectionStrings").Element("add").Attribute("connectionString").Value = encryptedConnStr;
                //MessageBox.Show(encryptedConnStr);
                //xmlConnection.Save("ConnectionString.config"); 
                //#endregion

                if (lstReportMaster.Count < 1)
                    throw new Exception(string.Format("Report with code {0} is not defined", reportCode));
                ReportMaster reportMaster = lstReportMaster[0];
                string reportClassName = reportMaster.ClassName;
                BaseRpt report = GetReport(reportClassName);

                report.Init(appSession, reportMaster.ReportID, reportCode, param); PrintingSystem printingSystem1 = new PrintingSystem();
                report.BeforePrint += new System.Drawing.Printing.PrintEventHandler(report_BeforePrint);

                ReportPrintTool tool = new ReportPrintTool(report);
                #region Change the Visibility of Commands 
                PrintingSystemBase ps = tool.PrintingSystem;
                ps.SetCommandVisibility(PrintingSystemCommand.Save, CommandVisibility.None);
                ps.SetCommandVisibility(PrintingSystemCommand.Watermark, CommandVisibility.None);
                #endregion
                tool.ShowPreviewDialog();

                ////set ip printer
                //string url = BusinessLayer.GetPrinterLocation(130).PrinterName;

                ////hide warning margin saat print
                //report.ShowPrintMarginsWarning = false;
                //ps.ShowMarginsWarning = false;

                ////print
                //tool.Print(url);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
            }
        }

        void report_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XtraReport report = (XtraReport)sender;
            foreach (DevExpress.XtraReports.UI.Band band in report.Bands)
            {
                foreach (DevExpress.XtraReports.UI.XRControl control in band)
                {
                    if (control.GetType() == typeof(DevExpress.XtraReports.UI.XRLabel) ||
                        control.GetType() == typeof(DevExpress.XtraReports.UI.XRPageInfo))
                    {
                        FontStyle ctrlFontStyle = control.Font.Style;
                        float ctrlFontSize = control.Font.Size;
                        control.Font = new Font("Tahoma", ctrlFontSize, ctrlFontStyle);
                    }

                }
            }
        }

        public BaseRpt GetReport(string className)
        {
            Assembly assembly = Assembly.Load("QIS.Medinfras.ReportDesktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            Object o = assembly.CreateInstance("QIS.Medinfras.ReportDesktop." + className);
            return (BaseRpt)o;
        }

        private void ReportViewer_Load(object sender, EventArgs e)
        {
            this.Close();
        }

        #region Helper
        private string EncryptText(string sourceText)
        {
            string result = string.Empty;
            char[] sourceTextCharList = sourceText.ToCharArray();

            foreach (char chr in sourceTextCharList)
            {
                char encryptedChar = Convert.ToChar(((int)chr) + 237);
                result += (encryptedChar);
            }
            return result;
        }

        private string DecryptText(string encryptedText)
        {
            string result = string.Empty;
            char[] encryptedTextCharList = encryptedText.ToCharArray();

            foreach (char chr in encryptedTextCharList)
            {
                char decryptedChar = Convert.ToChar(((int)chr) - 237);
                result += (decryptedChar);
            }
            return result;
        } 
        #endregion
    }
}

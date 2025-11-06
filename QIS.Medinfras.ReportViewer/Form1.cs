using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using System.Reflection;
using QIS.Medinfras.ReportDesktop;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportViewer
{
    public partial class Form1 : Form
    {
        public Form1(string[] args)
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
                List<ReportMaster> lstReportMaster = BusinessLayer.GetReportMasterList(string.Format("ReportCode = '{0}'", reportCode));
                if (lstReportMaster.Count < 1)
                    throw new Exception(string.Format("Report with code {0} is not defined", reportCode));
                ReportMaster reportMaster = lstReportMaster[0];
                string reportClassName = reportMaster.ClassName;
                BaseRpt report = GetReport(reportClassName);
                report.Init(appSession, reportMaster.ReportID, reportCode, param);
                ReportPrintTool tool = new ReportPrintTool(report);
                tool.ShowPreviewDialog();
                //this.();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
            }
        }

        public BaseRpt GetReport(string className)
        {
            Assembly assembly = Assembly.Load("QIS.Medinfras.ReportDesktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            Object o = assembly.CreateInstance("QIS.Medinfras.ReportDesktop." + className);
            return (BaseRpt)o;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

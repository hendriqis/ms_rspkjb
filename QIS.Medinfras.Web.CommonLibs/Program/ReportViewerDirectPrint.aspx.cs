using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using System.Reflection;
using QIS.Medinfras.Report;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ReportViewerDirectPrint : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Form["param"] != null)
                hdnParam.Value = Request.Form["param"].ToString();
            string[] param = hdnParam.Value.Split('|');

            string reportCode = Page.Request.QueryString["id"];
            List<ReportMaster> lstReportMaster = BusinessLayer.GetReportMasterList(string.Format("ReportCode = '{0}'", reportCode));
            if (lstReportMaster.Count < 1)
                throw new Exception(string.Format("Report with code {0} is not defined", reportCode));
            ReportMaster reportMaster = lstReportMaster[0];
            BaseRpt report = GetReport(reportMaster.ClassName);
            report.Init(reportMaster.ReportID, reportCode, param, this);
            this.ReportViewer1.Report = report;
            //if (Page.IsCallback)
            //{
            //    string[] param = hdnParam.Value.Split('|');

            //    string reportCode = Page.Request.QueryString["id"];
            //    List<ReportMaster> lstReportMaster = BusinessLayer.GetReportMasterList(string.Format("ReportCode = '{0}'", reportCode));
            //    if (lstReportMaster.Count < 1)
            //        throw new Exception(string.Format("Report with code {0} is not defined", reportCode));
            //    ReportMaster reportMaster = lstReportMaster[0];
            //    BaseRpt report = GetReport(reportMaster.ClassName);
            //    report.Init(reportMaster.ReportID, reportCode, param, this);
            //    this.ReportViewer1.Report = report;                
            //}
            //else
            //    if (Request.Form["param"] != null)
            //        hdnParam.Value = Request.Form["param"].ToString();
        }

        public BaseRpt GetReport(string className)
        {
            Assembly assembly = Assembly.Load("QIS.Medinfras.Report, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            Object o = assembly.CreateInstance("QIS.Medinfras.Report." + className);
            return (BaseRpt)o;
        }
    }
}
using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Extensions;
using DevExpress.XtraReports.Native;
using QIS.Medinfras.Data.Service;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using QIS.Medinfras.Web.Common;
using System.Xml;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BaseRpt : DevExpress.XtraReports.UI.XtraReport
    {
        protected ReportMaster reportMaster = null;
        protected List<Words> words = null;
        protected AppSessionReport appSession = null;
        private string _AdditionalReportSubTitle = string.Empty;

        protected string AdditionalReportSubTitle
        {
            get { return _AdditionalReportSubTitle; }
            set { _AdditionalReportSubTitle = value; }
        }

        public BaseRpt()
        {
            InitializeComponent();
        }

        public void Init(AppSessionReport session, int reportID, string reportCode, string[] param)
        {
            appSession = session;
            reportMaster = BusinessLayer.GetReportMaster(reportID);
            InitializeReport(param);
        }

        public string GetLabel(string code)
        {
            return "";
        }

        public virtual void InitializeReport(string[] param)
        {
        }

        public virtual void IsHideHeader(bool isHide)
        {
        }

        protected string ResolveUrl(string url)
        {
            return url.Replace("~", AppConfigManager.QISAppVirtualDirectory);
        }

        protected virtual bool IsSkipBinding()
        {
            return false;
        }
    }
}

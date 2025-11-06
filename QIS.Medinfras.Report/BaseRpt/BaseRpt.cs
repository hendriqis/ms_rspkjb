using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Web.UI;

namespace QIS.Medinfras.Report
{
    public partial class BaseRpt : DevExpress.XtraReports.UI.XtraReport
    {
        protected ReportMaster reportMaster = null;
        protected List<Words> words = null;
        protected TemplateControl page = null;
        public BaseRpt()
        {
            InitializeComponent();
        }

        public void Init(int reportID, string reportCode, string[] param, TemplateControl page)
        {
            this.page = page;

            words = Helper.LoadWords(page);
            reportMaster = BusinessLayer.GetReportMaster(reportID);
            InitializeReport(param);
        }

        public string GetLabel(string code)
        {
            return Helper.GetWordsLabel(words, code);
        }

        public virtual void InitializeReport(string[] param)
        {
        }
    }
}

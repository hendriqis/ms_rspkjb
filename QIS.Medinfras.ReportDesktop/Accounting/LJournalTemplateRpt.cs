using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using System.Linq;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LJournalTemplateRpt : BaseDailyLandscapeRpt
    {
        public LJournalTemplateRpt()
        {
            InitializeComponent();
        }

        private List<vJournalTemplateDt> lstDt = null;
        public override void InitializeReport(string[] param)
        {
            lstDt = BusinessLayer.GetvJournalTemplateDtList("IsDeleted = 0");

            base.InitializeReport(param);
        }

        private void sbrJournalTemplateDDt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (GetCurrentColumnValue("TemplateID") != null)
            {
                XRSubreport subreport = (XRSubreport)sender;
                int TemplateID = Convert.ToInt32(GetCurrentColumnValue("TemplateID"));
                ((LJournalTemplateDtRpt)subreport.ReportSource).InitializeReport(lstDt.Where(p => p.TemplateID == TemplateID && p.Position == "D").ToList());
            }
        }

        private void sbrJournalTemplateKDt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (GetCurrentColumnValue("TemplateID") != null)
            {
                XRSubreport subreport = (XRSubreport)sender;
                int TemplateID = Convert.ToInt32(GetCurrentColumnValue("TemplateID"));
                ((LJournalTemplateDtRpt)subreport.ReportSource).InitializeReport(lstDt.Where(p => p.TemplateID == TemplateID && p.Position == "K").ToList());
            }
        }

    }
}

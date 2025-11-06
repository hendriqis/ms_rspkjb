using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using System.Collections.Generic;
using DevExpress.XtraReports.Parameters;
using System.Linq;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LJournalTemplateDt : DevExpress.XtraReports.UI.XtraReport
    {
        public LJournalTemplateDt()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<vJournalTemplateDt> lst)
        {
            this.DataSource = lst;
        }
    }
}

using System;
using System.ComponentModel;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class  EpisodeSummaryInstruction : DevExpress.XtraReports.UI.XtraReport
    {
        public EpisodeSummaryInstruction()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<vPatientInstruction> lst)
        {
            this.DataSource = lst;
        }

    }
}

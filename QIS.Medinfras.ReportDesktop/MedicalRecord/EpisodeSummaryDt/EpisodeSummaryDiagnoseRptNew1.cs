using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class EpisodeSummaryDiagnoseRptNew1 : BaseRpt
    {
        private int _lineNumber = 0;
        public EpisodeSummaryDiagnoseRptNew1()
        {
            _lineNumber = 0;
            InitializeComponent();
        }

        public void InitializeReport(List<vPatientDiagnosis> lst)
        {
            this.DataSource = lst;
        }

        private void lblNo_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lblNo.Text = (++_lineNumber).ToString();
        }
    }
}

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
    public partial class MR2OperatingRSSBB : BaseRpt
    {
        private int _lineNumber = 0;
        public MR2OperatingRSSBB()
        {
            _lineNumber = 0;
            InitializeComponent();
        }

        public void InitializeReport(List<vPatientChargesHdProcedureGroup> lst)
        {
            this.DataSource = lst;
        }

        private void lblNo_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lblNo.Text = (++_lineNumber).ToString();
        }
    }
}

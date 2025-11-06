using System;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class SubParamedicTaxBalanceRSSES : DevExpress.XtraReports.UI.XtraReport
    {
        public SubParamedicTaxBalanceRSSES()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<ParamedicTaxBalance> lst)
        {
            this.DataSource = lst;
        }

    }
}

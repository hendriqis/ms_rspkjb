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
    public partial class SubTransRegistrationDetailRSPBT : DevExpress.XtraReports.UI.XtraReport
    {
        public SubTransRegistrationDetailRSPBT()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<GetTRSSummaryDtTransRegistrationDetailRSPBT> lst)
        {
            this.DataSource = lst;
        }

    }
}

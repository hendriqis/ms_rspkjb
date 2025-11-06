using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class SubTransRegistrationDetailRSRTA6 : BaseA6Rpt
    {
        public SubTransRegistrationDetailRSRTA6()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<GetTRSSummaryDtTransRegistrationDetail3> lst)
        {
            this.DataSource = lst;
        }

    }
}

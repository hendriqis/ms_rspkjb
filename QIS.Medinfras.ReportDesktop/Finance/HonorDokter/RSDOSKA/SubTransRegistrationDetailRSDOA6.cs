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
    public partial class SubTransRegistrationDetailRSDOA6 : BaseA6Rpt
    {
        public SubTransRegistrationDetailRSDOA6()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<GetTRSSummaryDtTransRegistrationDetailRSDO> lst)
        {
            this.DataSource = lst;
        }

    }
}

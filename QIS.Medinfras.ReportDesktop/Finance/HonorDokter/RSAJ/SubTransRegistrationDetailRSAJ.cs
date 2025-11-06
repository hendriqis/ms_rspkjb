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
    public partial class SubTransRegistrationDetailRSAJ : DevExpress.XtraReports.UI.XtraReport
    {
        public SubTransRegistrationDetailRSAJ()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<GetTRSSummaryDtTransRegistrationDetail> lst)
        {
            this.DataSource = lst;
        }

    }
}

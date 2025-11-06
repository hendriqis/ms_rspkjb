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
    public partial class SubTransRevenueAdjustmentDetailPlusRSPKSB : DevExpress.XtraReports.UI.XtraReport
    {
        public SubTransRevenueAdjustmentDetailPlusRSPKSB()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<vTransRevenueSharingSummaryAdj> lst)
        {
            if (lst == null || lst.Count == 0)
            {
                this.Visible = false;     
                return;
            }
            this.DataSource = lst;
        }

    }
}

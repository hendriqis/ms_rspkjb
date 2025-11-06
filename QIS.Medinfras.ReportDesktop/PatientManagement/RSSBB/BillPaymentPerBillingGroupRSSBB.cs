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
    public partial class BillPaymentPerBillingGroupRSSBB : DevExpress.XtraReports.UI.XtraReport
    {
        public BillPaymentPerBillingGroupRSSBB()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<GetPatientChargesDtPerBillingGroup> lst)
        {
            foreach (GetPatientChargesDtPerBillingGroup pcb in lst)
            {
                if (pcb.GCPrintOption == "X223^02")
                {
                    xrDetail.Visible = true;
                }
                else
                {
                    xrDetail.Visible = false;
                }
            }
            this.DataSource = lst;
        }
    }
}

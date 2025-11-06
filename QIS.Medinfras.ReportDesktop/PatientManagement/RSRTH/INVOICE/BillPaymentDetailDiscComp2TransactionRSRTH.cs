using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BillPaymentDetailDiscComp2TransactionRSRTH : DevExpress.XtraReports.UI.XtraReport
    {
        public BillPaymentDetailDiscComp2TransactionRSRTH()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<GetPatientChargesHdDtALL11> lst)
        {
            this.DataSource = lst;
        }
    }
}

using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Linq;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BillPaymentSummaryTransactionPerBillingRSDOSOBA : DevExpress.XtraReports.UI.XtraReport
    {
        public BillPaymentSummaryTransactionPerBillingRSDOSOBA()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<GetPatientChargesHdDtPatientBill> lst)
        {
            this.DataSource = lst;
        }
    }
}

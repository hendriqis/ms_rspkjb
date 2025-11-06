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
    public partial class BillPaymentDetailDiscComp2TransactionRSSES : DevExpress.XtraReports.UI.XtraReport
    {
        public BillPaymentDetailDiscComp2TransactionRSSES()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<GetPatientChargesHdDtAllRSSES> lst)
        {
            this.DataSource = lst;
        }
    }
}

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
    public partial class BillPaymentDetailTransactionRSP : DevExpress.XtraReports.UI.XtraReport
    {
        public BillPaymentDetailTransactionRSP()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<GetPatientChargesHdDtALLWithOutDPRSRT> lst)
        {
            this.DataSource = lst;
        }
    }
}

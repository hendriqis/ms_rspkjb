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
    public partial class BNewPaymentReceipt27Dt : DevExpress.XtraReports.UI.XtraReport
    {
        public BNewPaymentReceipt27Dt()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<GetPaymentReceiptCustomDetailTransactionMCU> lst)
        {
            this.DataSource = lst;
        }

    }
}

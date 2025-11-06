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
    public partial class BNew1InvoicePaymentVirtualAccountDetail : DevExpress.XtraReports.UI.XtraReport
    {

        public BNew1InvoicePaymentVirtualAccountDetail()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<GetTransferAccountBank> lst)
        {
            //vBusinessPartnerVirtualAccount
            this.DataSource = lst;
        }

    }
}

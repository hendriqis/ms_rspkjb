using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BNewPaymentReceipt26Rekap1 : BaseRpt
    {
        public BNewPaymentReceipt26Rekap1()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<GetPaymentReceiptCustom> lstEntity)
        {
            this.DataSource = lstEntity;
        }
    }
}

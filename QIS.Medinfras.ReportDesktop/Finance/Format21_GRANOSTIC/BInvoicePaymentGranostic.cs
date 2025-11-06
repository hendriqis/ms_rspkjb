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
    public partial class BInvoicePaymentGranostic : BaseCustomDailyPotraitRpt
    {
        public BInvoicePaymentGranostic()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            base.InitializeReport(param);
            lblDatePrint.Text = string.Format("Tanggal : {0} ", DateTime.Now.ToString("dd-MM-yyyy"));
        }
    }
}

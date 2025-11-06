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
    public partial class BillPaymentDetailTransactionWithoutDiscBROSv2 : DevExpress.XtraReports.UI.XtraReport
    {
        public BillPaymentDetailTransactionWithoutDiscBROSv2()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<GetPatientChargesHdDtWithDetailPackage> lst)
        {
            this.DataSource = lst;
        }

        private void xrTableRow10_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableRow row = (XRTableRow)sender;
            string oDetailItemName1 = "";
            if (GetCurrentColumnValue("DetailItemName1") != null)
            {
                if (GetCurrentColumnValue("DetailItemName1").ToString() != "")
                {
                    oDetailItemName1 = GetCurrentColumnValue("DetailItemName1").ToString();
                }
            }

            if (oDetailItemName1 != "")
            {
                row.Visible = true;
            }
            else
            {
                row.Visible = false;
            }
        }   
    }
}

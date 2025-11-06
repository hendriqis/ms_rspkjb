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
    public partial class BNewPaymentReceiptRajalDtRSPW : DevExpress.XtraReports.UI.XtraReport
    {
        public BNewPaymentReceiptRajalDtRSPW()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<GetPatientChargesHdDtAllRSPW> lst)
        {
            List<GetPatientChargesHdDtAllRSPW> newLst = lst.Where(w => w.LineAmount > 0).GroupBy(g => g.BillingGroupID).Select(s => s.FirstOrDefault()).ToList();
            this.DataSource = newLst;
        }

        private void cItemGroupName_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String ItemCreatedDateInString = Convert.ToString(GetCurrentColumnValue("ItemCreatedDateInString"));
            if (ItemCreatedDateInString == "01-Jan-1900")
            {
                xrTable2.Visible = false;
            }
            else
            {
                xrTable2.Visible = true;
            }
        }
    }
}

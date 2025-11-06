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
    public partial class BillPaymentSummaryTransactionDM : DevExpress.XtraReports.UI.XtraReport
    {
        public BillPaymentSummaryTransactionDM()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<GetPatientChargesHdDtALL> lst)
        {
            this.DataSource = lst;
        }

        private void lblServiceUnit_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String IsChargeTransfered = Convert.ToString(GetCurrentColumnValue("IsChargeTransfered"));
            if (IsChargeTransfered == "0")
            {
                lblServiceUnit.Text = Convert.ToString(GetCurrentColumnValue("DepartmentID")) + " | " + Convert.ToString(GetCurrentColumnValue("ServiceUnitName"));
            }
            else
            {
                lblServiceUnit.Text = Convert.ToString(GetCurrentColumnValue("FromDepartmentID")) + " | " 
                                    + Convert.ToString(GetCurrentColumnValue("FromServiceUnitName")) + " | "
                                    + Convert.ToString(GetCurrentColumnValue("FromRegistrationNo"));
            }
        }

        private void cSubServiceUnit_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String IsChargeTransfered = Convert.ToString(GetCurrentColumnValue("IsChargeTransfered"));
            if (IsChargeTransfered == "0")
            {
                cSubServiceUnit.Text = "Sub Total " + Convert.ToString(GetCurrentColumnValue("DepartmentID"));
            }
            else
            {
                cSubServiceUnit.Text = "Sub Total " + Convert.ToString(GetCurrentColumnValue("FromDepartmentID"));
            }
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

            String ItemGroupID = Convert.ToString(GetCurrentColumnValue("ItemGroupID"));
            if (ItemGroupID == "0")
            {
                cItemGroupName.Text = Convert.ToString(GetCurrentColumnValue("ItemType"));
            }
            else
            {
                cItemGroupName.Text = string.Format("{0} | {1}", Convert.ToString(GetCurrentColumnValue("ItemType")), Convert.ToString(GetCurrentColumnValue("ItemGroupName1")));
            }
        }
    }
}

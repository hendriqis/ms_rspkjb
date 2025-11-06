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
    public partial class BillPaymentPerBillingSummaryTransactionRSSESPatient : DevExpress.XtraReports.UI.XtraReport
    {
        public BillPaymentPerBillingSummaryTransactionRSSESPatient()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<GetPatientChargesHdDtPerBillingRSSES> lst)
        {
            this.DataSource = lst;
        }

        private void lblServiceUnit_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String IsTransfered = Convert.ToString(GetCurrentColumnValue("IsTransfered"));
            if (IsTransfered == "0")
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
            String IsChargeTransfered = Convert.ToString(GetCurrentColumnValue("IsTransfered"));
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
        }

        private void tableItemParamedic_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String cfPrintDoctorNameItemGroup = Convert.ToString(GetCurrentColumnValue("cfPrintDoctorNameItemGroup"));
            if (cfPrintDoctorNameItemGroup == "")
            {
                tableItemParamedic.Visible = false;
            }
            else
            {
                tableItemParamedic.Visible = true;
            }

        }
    }
}

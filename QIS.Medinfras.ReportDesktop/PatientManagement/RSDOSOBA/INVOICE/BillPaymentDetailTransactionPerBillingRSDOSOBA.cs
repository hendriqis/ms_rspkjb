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
    public partial class BillPaymentDetailTransactionPerBillingRSDOSOBA : DevExpress.XtraReports.UI.XtraReport
    {
        public BillPaymentDetailTransactionPerBillingRSDOSOBA()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<GetPatientChargesHdDtPatientBill> lst)
        {
            this.DataSource = lst;
        }

        private void lblServiceUnit_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String IsTransfered = Convert.ToString(GetCurrentColumnValue("IsTransfered"));
            if (IsTransfered == "0")
            {
                lblServiceUnit.Text = Convert.ToString(GetCurrentColumnValue("DepartmentID")) + " | " + Convert.ToString(GetCurrentColumnValue("ItemServiceUnitName"));
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
                cSubServiceUnit.Text = "Sub Total " + Convert.ToString(GetCurrentColumnValue("ItemDepartmentID"));
            }
            else
            {
                cSubServiceUnit.Text = "Sub Total " + Convert.ToString(GetCurrentColumnValue("FromDepartmentID"));
            }
        }

        //private void xrLabel1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        //{
        //    String ItemCreatedDateInString = Convert.ToString(GetCurrentColumnValue("ItemCreatedDateInString"));
        //    if (ItemCreatedDateInString == "01-Jan-1900")
        //    {
        //        xrLabel1.Visible = false;
        //        xrTable2.Visible = false;
        //        xrTable4.Visible = false;
        //    }
        //    else
        //    {
        //        xrLabel1.Visible = true;
        //        xrTable2.Visible = true;
        //        xrTable4.Visible = true;
        //    }
        //}

        //private void tableDoctor_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        //{
        //    String cfPrintDoctorNameItemGroup = Convert.ToString(GetCurrentColumnValue("cfPrintDoctorNameItemGroup"));

        //    if (cfPrintDoctorNameItemGroup == "")
        //    {
        //        tableDoctor.Visible = false;
        //    }
        //    else
        //    {
        //        tableDoctor.Visible = true;
        //    }
        //}

        //private void lblLocation_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        //{
        //    String LocationName = Convert.ToString(GetCurrentColumnValue("LocationName"));

        //    if (LocationName == "")
        //    {
        //        lblLocation.Visible = false;
        //    }
        //    else
        //    {
        //        lblLocation.Visible = true;
        //    }
        //}
    }
}

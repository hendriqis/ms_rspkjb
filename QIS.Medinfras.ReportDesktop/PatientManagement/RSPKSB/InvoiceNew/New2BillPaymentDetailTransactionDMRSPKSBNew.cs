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
    public partial class New2BillPaymentDetailTransactionDMRSPKSBNew : DevExpress.XtraReports.UI.XtraReport
    {
        public New2BillPaymentDetailTransactionDMRSPKSBNew()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<GetPatientChargesHdDtALLRSPKSB> lst)
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
                cSubServiceUnit.Text = "Sub Total " + Convert.ToString(GetCurrentColumnValue("ServiceUnitName"));
            }
            else
            {
                cSubServiceUnit.Text = "Sub Total " + Convert.ToString(GetCurrentColumnValue("FromServiceUnitName"));
            }
        }

        private void cSubDepartment_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String IsChargeTransfered = Convert.ToString(GetCurrentColumnValue("IsChargeTransfered"));
            if (IsChargeTransfered == "0")
            {
                cSubDepartment.Text = "Sub Total " + Convert.ToString(GetCurrentColumnValue("DepartmentID"));
            }
            else
            {
                cSubDepartment.Text = "Sub Total " + Convert.ToString(GetCurrentColumnValue("FromDepartmentID"));
            }
        }

        private void xrLabel1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String ItemGroupID = Convert.ToString(GetCurrentColumnValue("ItemGroupID"));
            if (ItemGroupID == "0")
            {
                xrLabel1.Text = Convert.ToString(GetCurrentColumnValue("ItemType"));
            }
            else
            {
                xrLabel1.Text = string.Format("{0} | {1}", Convert.ToString(GetCurrentColumnValue("ItemType")), Convert.ToString(GetCurrentColumnValue("DrugsGroupName")));
            }


            String ItemCreatedDateInString = Convert.ToString(GetCurrentColumnValue("ItemCreatedDateInString"));
            if (ItemCreatedDateInString == "01-Jan-1900")
            {
                xrLabel1.Visible = false;
                xrTable2.Visible = false;
                xrTable4.Visible = false;
            }
            else
            {
                xrLabel1.Visible = true;
                xrTable2.Visible = true;
                xrTable4.Visible = true;
            }
        }


        private void ClassName_xrtbl(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String ItemType = Convert.ToString(GetCurrentColumnValue("GCItemType"));
            if (ItemType != Constant.ItemType.PELAYANAN)
            {
                xrTableCell18.Visible = false;
                xrTableCell18.HeightF = 0;
            }
            else
            {
                xrTableCell18.Visible = true;
            }
        }


        private void GroupHeader4_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Boolean PrintWithDoctor = Convert.ToBoolean(GetCurrentColumnValue("IsPrintWithDoctorName"));
            if (PrintWithDoctor != true)
            {
                e.Cancel = true;
            }
            else
            {
                GroupHeader4.HeightF = 19;
            }
        }

        private void xrTableCell9_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String JasaDokter = Convert.ToString(GetCurrentColumnValue("JasaDokterBedah"));
            if (JasaDokter == "0")
            {
                xrTableCell9.Text = Convert.ToString(GetCurrentColumnValue("ItemName1"));
            }
            else
            {
                xrTableCell9.Text = string.Format("{0} | {1}", Convert.ToString(GetCurrentColumnValue("ItemName1")), Convert.ToString(GetCurrentColumnValue("ParamedicName")));
            }

        }

        private void GroupHeader5_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String ItemGroup = Convert.ToString(GetCurrentColumnValue("ItemGroupCode"));
            if (ItemGroup != "20.01.01")
            {
                if (ItemGroup != "20.02.01")
                {
                    e.Cancel = true;
                }
            }
            String DrugsGroupName = Convert.ToString(GetCurrentColumnValue("DrugsGroupName"));
            if (DrugsGroupName != "20.01.01")
            {
                if (DrugsGroupName != "20.02.01")
                {
                    e.Cancel = true;
                }
            }
        }
    }
}

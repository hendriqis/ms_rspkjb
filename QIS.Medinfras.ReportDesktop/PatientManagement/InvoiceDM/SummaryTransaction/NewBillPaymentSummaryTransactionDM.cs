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
    public partial class NewBillPaymentSummaryTransactionDM : DevExpress.XtraReports.UI.XtraReport
    {

        public NewBillPaymentSummaryTransactionDM()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<GetPatientChargesHdDtALL3> lst)
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
                cSubServiceUnit.Text = "SUB TOTAL " + Convert.ToString(GetCurrentColumnValue("DepartmentID"));
            }
            else
            {
                cSubServiceUnit.Text = "SUB TOTAL " + Convert.ToString(GetCurrentColumnValue("FromDepartmentID"));
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
                cItemGroupName.Text = string.Format("{0} | {1}", Convert.ToString(GetCurrentColumnValue("ItemType")), Convert.ToString(GetCurrentColumnValue("ItemGroup")));
            }
        }

        private void xrTableCell1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

            String ItemType = Convert.ToString(GetCurrentColumnValue("GCItemType"));
            String ItemGroupCode = Convert.ToString(GetCurrentColumnValue("ItemGroupCode"));
            if (ItemType != Constant.ItemType.LABORATORIUM && ItemGroupCode != "10.01.00")
            {
                xrTableCell1.Visible = false;
                xrTableCell2.Visible = false;
                xrTableCell3.Visible = false;

            }
            else
            {
                if (ItemGroupCode == "10.01.00")
                {
                    xrTableCell1.Visible = true;
                    xrTableCell2.Visible = true;
                    xrTableCell3.Visible = true;
                    Int32  RegistrationID = Convert.ToInt32(GetCurrentColumnValue("RegistrationID"));
                    String RegistrationNo = Convert.ToString(GetCurrentColumnValue("RegistrationNo"));
                    String ItemHealthCareServiceUnitID = Convert.ToString(GetCurrentColumnValue("ItemHealthcareServiceUnitID"));
                    String ItemServiceUnitName = Convert.ToString(GetCurrentColumnValue("ItemServiceUnitName"));
                    String StartEndDate = Convert.ToString(GetCurrentColumnValue("StartEndDate"));
                    //xrTableCell1.Text = string.Format("{0} ({1})", ItemServiceUnitName,StartEndDate);

                }
                else
                {
                    xrTableCell1.Visible = false;
                    xrTableCell2.Visible = false;
                    xrTableCell3.Visible = false;

                }
            }
        }

        private void xrTable2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String ItemType = Convert.ToString(GetCurrentColumnValue("GCItemType"));
            String ItemGroupCode = Convert.ToString(GetCurrentColumnValue("ItemGroupCode"));
            if (ItemType != Constant.ItemType.LABORATORIUM && ItemGroupCode != "10.01.00")
            {
                xrTableCell11.ForeColor = Color.Black;
                xrTableCell12.ForeColor = Color.Black;
                xrTable4.HeightF = 0;
                GroupHeader4.HeightF = 0;

            }
            else
            {
                if (ItemGroupCode == "10.01.00")
                {
                    xrTableCell11.ForeColor = Color.White;
                    xrTableCell12.ForeColor = Color.White;
                }
                else
                {
                    xrTableCell11.ForeColor = Color.Black;
                    xrTableCell12.ForeColor = Color.Black;

                }
            }
        }
    }
}

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
    public partial class BillPaymentDetailTransactionByServiceUnitPatientDM : DevExpress.XtraReports.UI.XtraReport
    {
        public BillPaymentDetailTransactionByServiceUnitPatientDM()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<GetPatientChargesHdDtAllPerServiceUnit> lst)
        {
            this.DataSource = lst;
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

        private void lblServiceUnit_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String IsChargeTransfered = Convert.ToString(GetCurrentColumnValue("IsTransfered"));
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
    }
}

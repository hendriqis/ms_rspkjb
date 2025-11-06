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
    public partial class BillPaymentDetailBedAllRSDOSKA : DevExpress.XtraReports.UI.XtraReport
    {
        public BillPaymentDetailBedAllRSDOSKA()
        {
            InitializeComponent();
        }


        public void InitializeReport(int RegistrationID)
        {
            List<GetPatientChargesHdDtAllBill2Summary> lst = BusinessLayer.GetPatientChargesHdDtAllBill2SummaryList(RegistrationID);
            this.DataSource = lst;
        }
    }
}

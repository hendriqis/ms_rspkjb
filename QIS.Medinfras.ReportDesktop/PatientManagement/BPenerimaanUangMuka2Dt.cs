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
    public partial class BPenerimaanUangMuka2Dt : DevExpress.XtraReports.UI.XtraReport
    {
        public BPenerimaanUangMuka2Dt()
        {
            InitializeComponent();
        }

        public void InitializeReport(int PaymentID)
        {
            List<vPatientPaymentDt> lst = BusinessLayer.GetvPatientPaymentDtList(string.Format("PaymentID = {0} AND IsDeleted = 0", PaymentID));

            this.DataSource = lst;
        }

    }
}

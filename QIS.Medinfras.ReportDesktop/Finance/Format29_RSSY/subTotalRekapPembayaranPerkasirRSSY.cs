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
    public partial class subTotalRekapPembayaranPerkasirRSSY : DevExpress.XtraReports.UI.XtraReport
    {
        public subTotalRekapPembayaranPerkasirRSSY()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<GetPatientPaymentDtRekapReportRSSY> lst)
        {
            this.DataSource = lst;
        }

        
    }
}

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class MRPatientTransfer : DevExpress.XtraReports.UI.XtraReport
    {
        public MRPatientTransfer()
        {
            InitializeComponent();
        }

        public void InitializeReport(int RegistrationID)
        {
            string filterExpression = string.Format("RegistrationID IN ({0})", RegistrationID);
            List<vPatientTransferHistoryRegistration> entityPT = BusinessLayer.GetvPatientTransferHistoryRegistrationList(filterExpression);

            this.DataSource = entityPT;
        }
    }
}

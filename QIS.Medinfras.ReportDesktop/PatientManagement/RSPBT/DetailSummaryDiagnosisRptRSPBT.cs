using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Collections.Generic;
using DevExpress.XtraReports.Parameters;
using System.Linq;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class DetailSummaryDiagnosisRptRSPBT : DevExpress.XtraReports.UI.XtraReport
    {
        public DetailSummaryDiagnosisRptRSPBT()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0 ORDER BY GCDiagnoseType", VisitID);
            List<vPatientDiagnosis> lstentity = BusinessLayer.GetvPatientDiagnosisList(filterExpression);

            if (lstentity.Count > 0)
            {
                Detail.Visible = true;
            }
            else
            {
                Detail.Visible = false;
            }

            this.DataSource = lstentity;
            this.GroupHeader1.GroupFields.Add(new GroupField("DiagnoseType", XRColumnSortOrder.Ascending));
        }

    }
}

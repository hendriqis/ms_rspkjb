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
    public partial class EpisodeSummaryDiagnosisRptNew : DevExpress.XtraReports.UI.XtraReport
    {
        public EpisodeSummaryDiagnosisRptNew()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID)
        {
            string filterExpression = string.Format("VisitID = {0} AND GCDiagnoseType = '{1}' AND IsDeleted = 0", VisitID, Constant.DiagnoseType.MAIN_DIAGNOSIS);
            List<PatientDiagnosis> lstentity = BusinessLayer.GetPatientDiagnosisList(filterExpression);

            if (lstentity.Count > 0)
            {
                txtDiagnose.Text = lstentity.FirstOrDefault().DiagnoseID + " - " + lstentity.FirstOrDefault().DiagnosisText;
                txtDiagnosisNotes.Text = lstentity.FirstOrDefault().DiagnosisText + " (" + lstentity.FirstOrDefault().DiagnoseID + ")";
                Detail.Visible = true;
            }
            else 
            {
                Detail.Visible = false;
            }

            this.DataSource = lstentity;
        }

        //private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        //{
        //    if (isVisible == 0)
        //    {
        //        Detail.Visible = false;
        //    }
        //}
    }
}

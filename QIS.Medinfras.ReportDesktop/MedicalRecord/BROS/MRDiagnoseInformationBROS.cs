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
    public partial class MRDiagnoseInformationBROS : DevExpress.XtraReports.UI.XtraReport
    {
        public MRDiagnoseInformationBROS()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID, int MedicalResumeID)
        {
            string filterExpression = string.Format("VisitID IN ({0}) AND IsDeleted = 0", VisitID);
            List<vPatientDiagnosis> lstPD = BusinessLayer.GetvPatientDiagnosisList(filterExpression);
            List<vPatientDiagnosis> lstPDD = BusinessLayer.GetvPatientDiagnosisList(filterExpression);

            var lst = (from p in lstPD
                       select new
                       {
                           ParamedicName = p.ParamedicName,
                           PatientDiagnosisDts = lstPDD.Where(pdd => pdd.ID == p.ID).ToList()
                       }).ToList();

            this.DataSource = lst;
            DetailReport.DataMember = "PatientDiagnosisDts";
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (DetailReport.RowCount == 0)
            {
                Detail.Visible = false;
            }
        }
    }
}

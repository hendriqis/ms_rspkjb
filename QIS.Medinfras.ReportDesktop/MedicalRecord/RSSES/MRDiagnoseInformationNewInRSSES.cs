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
    public partial class MRDiagnoseInformationNewInRSSES : DevExpress.XtraReports.UI.XtraReport
    {
        public MRDiagnoseInformationNewInRSSES()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID)
        {
            string filterExpression = string.Format("VisitID IN ({0}) AND GCDiagnoseType IN ('{1}') AND IsDeleted = 0", VisitID, Constant.DiagnoseType.EARLY_DIAGNOSIS);
            vPatientDiagnosis entityPD = BusinessLayer.GetvPatientDiagnosisList(filterExpression).FirstOrDefault();
            vPatientDiagnosis entityPDD = BusinessLayer.GetvPatientDiagnosisList(filterExpression).FirstOrDefault();

            if (entityPD != null)
            {
                List<vPatientDiagnosis> lstPD = BusinessLayer.GetvPatientDiagnosisList(string.Format("ID = {0} AND IsDeleted = 0", entityPD.ID));
                List<vPatientDiagnosis> lstPDD = BusinessLayer.GetvPatientDiagnosisList(string.Format("ID = {0} AND IsDeleted = 0", entityPD.ID));
                var lst = (from p in lstPD
                           select new
                           {
                               DifferentialDate = p.DifferentialDateInString,
                               DifferentialTime = p.DifferentialTime,
                               PatientDiagnosisDts = lstPDD.Where(pdd => pdd.ID == p.ID).ToList()
                           }).ToList();
                this.DataSource = lst;
                DetailReport.DataMember = "PatientDiagnosisDts";
            }
            else
            {
                xrTable1.Visible = false;
                //xrLabel1.Visible = false;
                //xrLabel2.Visible = false;
                //xrLabel3.Visible = false;
                //xrLabel4.Visible = false;
                //xrLabel6.Visible = false;
            }
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

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
    public partial class MRDiagnoseInformationNewOutRSPKSB : DevExpress.XtraReports.UI.XtraReport
    {
        public MRDiagnoseInformationNewOutRSPKSB()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID)
        {
            string filterExpression = string.Format("VisitID IN ({0}) AND GCDiagnoseType IN ('{1}','{2}') AND IsDeleted = 0 ORDER BY ID", VisitID, Constant.DiagnoseType.MAIN_DIAGNOSIS, Constant.DiagnoseType.COMPLICATION);
            List<vPatientDiagnosis> entityPDList = BusinessLayer.GetvPatientDiagnosisList(filterExpression);

            if (entityPDList.Count() > 0)
            {
                var lst = (from p in entityPDList
                           select new
                           {
                               DifferentialDate = p.DifferentialDateInString,
                               DifferentialTime = p.DifferentialTime,
                               PatientDiagnosisDts = entityPDList.Where(pdd => pdd.ID == p.ID).ToList()
                           }).ToList();

                this.DataSource = lst;
                DetailReport.DataMember = "PatientDiagnosisDts";
            }
            else
            {
                xrTable2.Visible = false;
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

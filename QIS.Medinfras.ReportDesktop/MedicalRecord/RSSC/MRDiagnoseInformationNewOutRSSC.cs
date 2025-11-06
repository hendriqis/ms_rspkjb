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
    public partial class MRDiagnoseInformationNewOutRSSC : DevExpress.XtraReports.UI.XtraReport
    {
        public MRDiagnoseInformationNewOutRSSC()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID)
        {
            string filterExpression = string.Format("VisitID IN ({0}) AND GCDiagnoseType IN ('{1}','{2}') AND IsDeleted = 0 ORDER BY GCDiagnoseType ASC, ID DESC", VisitID, Constant.DiagnoseType.MAIN_DIAGNOSIS, Constant.DiagnoseType.COMPLICATION);
            List<vPatientDiagnosis> entityPDList = BusinessLayer.GetvPatientDiagnosisList(filterExpression);

            if (entityPDList.Count() > 0)
            {
                var lstHd = (
                    from a in entityPDList.GroupBy(aa => (aa.GCDiagnoseType + " - " + aa.GCDifferentialStatus)).Select(aa => aa.FirstOrDefault()).ToList()
                    select new
                    {
                        cfGroupingTypeStatus = (a.GCDiagnoseType + " - " + a.GCDifferentialStatus)
                    }
                ).ToList();

                var lstDt = (
                    from a in lstHd
                    select new
                    {
                        PatientDiagnosisDts = entityPDList.Where(aa => (aa.GCDiagnoseType + " - " + aa.GCDifferentialStatus) == a.cfGroupingTypeStatus).ToList()
                    }
                ).ToList();

                GroupField gfTypeStatus = new GroupField("cfGroupingTypeStatus");
                ghDiagnoseType.GroupFields.Add(gfTypeStatus);

                this.DataSource = lstDt;

                DetailReport.DataMember = "PatientDiagnosisDts";
            }
            else
            {
                xrLabel1.Visible = false;
                xrLabel2.Visible = false;
                xrLabel6.Visible = false;
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
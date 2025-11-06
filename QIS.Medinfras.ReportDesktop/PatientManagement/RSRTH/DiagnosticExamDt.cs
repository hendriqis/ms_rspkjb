using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Collections.Generic;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class DiagnosticExamDt : DevExpress.XtraReports.UI.XtraReport
    {
        public DiagnosticExamDt()
        {
            InitializeComponent();
        }

        private int ParamVisitID = 0;

        public void InitializeReport(int VisitID)
        {
            string filterExpressionHD = string.Format("VisitID = {0} AND GCTransactionStatus != '{1}' AND TransactionCode IN ('{2}','{3}') ORDER BY HealthcareServiceUnitID", VisitID, Constant.TransactionStatus.VOID, Constant.TransactionCode.LABORATORY_TEST_ORDER, Constant.TransactionCode.IMAGING_TEST_ORDER);
            string filterExpressionDT = string.Format("VisitID = {0} AND IsDeleted = 0 ORDER BY HealthcareServiceUnitID", VisitID);

            List<vTestOrderHd> lstEntityHd = BusinessLayer.GetvTestOrderHdList(filterExpressionHD);
            List<vTestOrderDt> lstEntityDt = BusinessLayer.GetvTestOrderDtList(filterExpressionDT);

            var lst = (from p in lstEntityHd
                       select new
                       {
                           TestOrderDts = lstEntityDt.Where(dt => dt.TestOrderID == p.TestOrderID).ToList().OrderBy(x => x.HealthcareServiceUnitID)
                       }).ToList();

            this.DataSource = lst;

            DetailReport.DataMember = "TestOrderDts";
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

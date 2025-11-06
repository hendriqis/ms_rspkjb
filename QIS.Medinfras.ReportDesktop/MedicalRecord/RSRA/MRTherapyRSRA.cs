using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class MRTherapyRSRA : DevExpress.XtraReports.UI.XtraReport
    {
        public MRTherapyRSRA()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID)
        {
            string filterExpressionHD = string.Format("VisitID = {0} AND GCTransactionStatus != '{1}'", VisitID, Constant.TransactionStatus.VOID);
            List<vPrescriptionOrderHd2> lstEntityHd = BusinessLayer.GetvPrescriptionOrderHd2List(filterExpressionHD);
            string filterExpressionDT = string.Format("IsDeleted = 0 AND PrescriptionOrderID IN (SELECT PrescriptionOrderID FROM PrescriptionOrderHd WHERE GCTransactionStatus != '{0}' AND VisitID = '{1}')", Constant.TransactionStatus.VOID, VisitID);
            List<vPrescriptionOrderDt6> lstEntityDt = BusinessLayer.GetvPrescriptionOrderDt6List(filterExpressionDT);

            var lstHD = (from hd in lstEntityHd
                         select new
                         {
                             PrescriptionOderNo = hd.PrescriptionOrderNo,
                             PrescriptionOrderDate = hd.PrescriptionDate.ToString(Constant.FormatString.DATE_FORMAT),
                             PrescriptionOrderTime = hd.PrescriptionTime,
                             ParamedicName = hd.ParamedicName,
                             PrescriptionOrderDts = lstEntityDt.Where(dt => dt.PrescriptionOrderID == hd.PrescriptionOrderID)
                         }
                ).ToList();

            this.DataSource = lstHD;

            DetailReport.DataMember = "PrescriptionOrderDts";
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

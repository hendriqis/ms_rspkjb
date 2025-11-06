using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using System.Collections.Generic;
using DevExpress.XtraReports.Parameters;
using System.Linq;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class EpisodeSummaryPrescriptionRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public EpisodeSummaryPrescriptionRpt()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID)
        {
            string filterExpressionHD = string.Format("VisitID = {0} AND GCTransactionStatus != '{1}' AND GCOrderStatus NOT IN ('{2}','{3}') AND GCPrescriptionType = '{4}'",
                                                            VisitID,
                                                            Constant.TransactionStatus.VOID,
                                                            Constant.OrderStatus.OPEN,
                                                            Constant.OrderStatus.CANCELLED,
                                                            Constant.PrescriptionType.DISCHARGE_PRESCRIPTION);
            
            List<vPrescriptionOrderHd> lstEntityHd = BusinessLayer.GetvPrescriptionOrderHdList(filterExpressionHD);
            string lstPrescriptionID = String.Join(",", lstEntityHd.Select(m => m.PrescriptionOrderID).ToArray());
            string filterExpressionDT = string.Empty;
            if (!string.IsNullOrEmpty(lstPrescriptionID)) filterExpressionDT = string.Format("IsDeleted = 0 AND PrescriptionOrderID IN ({0})", lstPrescriptionID);
            else filterExpressionDT = "1 = 0";
            List<vPrescriptionOrderDt> lstEntityDt = BusinessLayer.GetvPrescriptionOrderDtList(filterExpressionDT);
            
            GroupField grp = new GroupField("cfParentIDForRpt");
            GroupHeader1.GroupFields.Add(grp);

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

        private void Detail1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            bool IsCompound = Convert.ToBoolean(this.DetailReport.GetCurrentColumnValue("IsCompound"));
            if (!IsCompound) e.Cancel = true;
        }
    }
}
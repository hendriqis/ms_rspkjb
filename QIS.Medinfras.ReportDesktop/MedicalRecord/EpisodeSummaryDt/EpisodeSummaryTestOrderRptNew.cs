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
    public partial class EpisodeSummaryTestOrderRptNew : DevExpress.XtraReports.UI.XtraReport
    {
        public EpisodeSummaryTestOrderRptNew()
        {
            InitializeComponent();
        }

        private int ParamVisitID = 0;

        public void InitializeReport(int VisitID)
        {
            //List<ServiceUnitMaster> lstEnityServiceUnit = BusinessLayer.GetServiceUnitMasterList(string.Format("DepartmentID = '{0}'", Constant.Facility.DIAGNOSTIC));
            string filterExpressionHD = string.Format("VisitID = {0} AND GCTransactionStatus != '{1}'", VisitID, Constant.TransactionStatus.VOID);
            List<vTestOrderHd> lstEntityHd = BusinessLayer.GetvTestOrderHdList(filterExpressionHD);
            string filterExpressionDT = string.Format("VisitID = {0} AND IsDeleted = 0", VisitID);
            List<vTestOrderDt> lstEntityDt = BusinessLayer.GetvTestOrderDtList(filterExpressionDT);

            //var lstSU = (from su in lstEnityServiceUnit
            //             select new
            //             {
            //                 ServiceUnitName = su.ServiceUnitName,
            //                 TestOrderHds = (from hd in lstEntityHd.Where(hds => hds.ServiceUnitCode == su.ServiceUnitCode.ToString())
            //                                 select new
            //                                 {
            //                                     TestOrderNo = hd.TestOrderNo,
            //                                     TestOrderDateInString = hd.TestOrderDateInString,
            //                                     TestOrderTime = hd.TestOrderTime,
            //                                     ParamedicName = hd.ParamedicName,
            //                                     TestOrderDts = lstEntityDt.Where(dts => dts.TestOrderID == hd.TestOrderID)
            //                                 }
            //                                )
            //             }).ToList();

            var lstSU = (from su in lstEntityHd
                         select new
                         {
                             ServiceUnitName = su.ServiceUnitName,
                             TestOrderHds = (from hd in lstEntityHd.Where(hds => hds.ServiceUnitCode == su.ServiceUnitCode.ToString())
                                             select new
                                             {
                                                 TestOrderNo = hd.TestOrderNo,
                                                 TestOrderDateInString = hd.TestOrderDateInString,
                                                 TestOrderTime = hd.TestOrderTime,
                                                 ParamedicName = hd.ParamedicName,
                                                 TestOrderDts = lstEntityDt.Where(dts => dts.TestOrderID == hd.TestOrderID)
                                             }
                                            )
                         }).ToList();

            this.DataSource = lstSU;

            DetailReport.DataMember = "TestOrderHds";
            DetailReport1.DataMember = "TestOrderHds.TestOrderDts";
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

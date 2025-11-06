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
    public partial class MRTestOrderRSUKI : DevExpress.XtraReports.UI.XtraReport
    {
        public MRTestOrderRSUKI()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID)
        {
            List<vHealthcareServiceUnit> lstEnityServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM TestOrderHd WHERE VisitID = '{0}' AND GCTransactionStatus != '{1}')", VisitID, Constant.TransactionStatus.VOID));
            string filterExpressionHD = string.Format("VisitID = {0} AND GCTransactionStatus != '{1}'", VisitID, Constant.TransactionStatus.VOID);
            List<vTestOrderHdCustom2> lstEntityHd = BusinessLayer.GetvTestOrderHdCustom2List(filterExpressionHD);
            string filterExpressionDT = string.Format("VisitID = {0} AND IsDeleted = 0", VisitID);
            List<vTestOrderDtCustom1> lstEntityDt = BusinessLayer.GetvTestOrderDtCustom1List(filterExpressionDT);

            var lstSU = (from su in lstEnityServiceUnit
                         select new
                         {
                             ServiceUnitName = su.ServiceUnitName,
                             TestOrderHds = (from hd in lstEntityHd.Where(hds => hds.HealthcareServiceUnitID == su.HealthcareServiceUnitID)
                                             select new
                                             {
                                                 TestOrderNo = hd.TestOrderNo,
                                                 TestOrderDateInString = hd.cfTestOrderDateInString,
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

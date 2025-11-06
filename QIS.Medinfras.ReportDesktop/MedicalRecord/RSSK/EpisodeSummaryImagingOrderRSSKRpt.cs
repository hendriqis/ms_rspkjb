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
    public partial class EpisodeSummaryImagingOrderRSSKRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public EpisodeSummaryImagingOrderRSSKRpt()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID)
        {
            List<SettingParameterDt> lstSettingParameter = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI));
            string serviceUnitID = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).ParameterValue;

            string filterExpressionHD = string.Format("VisitID = {0} AND HealthcareServiceUnitID = {1} AND GCTransactionStatus != '{2}'", VisitID, serviceUnitID, Constant.TransactionStatus.VOID);
            List<vTestOrderHd> lstEntityHd = BusinessLayer.GetvTestOrderHdList(filterExpressionHD);
            string filterExpressionDT = string.Format("VisitID = {0} AND HealthcareServiceUnitID = {1} AND IsDeleted = 0", VisitID, serviceUnitID);
            List<vTestOrderDt> lstEntityDt = BusinessLayer.GetvTestOrderDtList(filterExpressionDT);

            var lstSU = (from hd in lstEntityHd
                         select new
                         {
                             ServiceUnitName = hd.ServiceUnitName,
                             TestOrderNo = hd.TestOrderNo,
                             TestOrderDateInString = hd.TestOrderDateInString,
                             TestOrderTime = hd.TestOrderTime,
                             ParamedicName = hd.ParamedicName,
                             TestOrderDts = lstEntityDt.Where(dts => dts.TestOrderID == hd.TestOrderID)
                         }).ToList();

            this.DataSource = lstSU;
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

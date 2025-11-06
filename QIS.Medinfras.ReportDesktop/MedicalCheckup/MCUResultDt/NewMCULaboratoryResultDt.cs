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
    public partial class NewMCULaboratoryResultDt : DevExpress.XtraReports.UI.XtraReport
    {
        public NewMCULaboratoryResultDt()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID)
        {
            //List<ServiceUnitMaster> lstEnityServiceUnit = BusinessLayer.GetServiceUnitMasterList(string.Format("DepartmentID = '{0}'", Constant.Facility.DIAGNOSTIC));
            //string filterExpressionHD = string.Format("VisitID = {0} AND GCTransactionStatus != '{1}' AND IsDeleted = 0", VisitID, Constant.TransactionStatus.VOID);
            //List<LaboratoryResultHd> lstEntityHd = BusinessLayer.GetLaboratoryResultHdList(filterExpressionHD);
            string filterExpressionDT = string.Format("VisitID = {0} AND IsDeleted = 0", VisitID);
            List<vLaboratoryResultDt> lstEntityDt = BusinessLayer.GetvLaboratoryResultDtList(filterExpressionDT);
            var lstEntityHd = lstEntityDt.GroupBy(test => test.ItemGroupName1).Select(grp => grp.First()).ToList().OrderBy(x => x.ItemGroupName1);

            var lstSU = (from su in lstEntityHd
                         select new
                         {
                             ItemGroupName = su.ItemGroupName1,
                             LaboratoryResultHds = (from hd in lstEntityHd.Where(hds => hds.ItemGroupName1 == su.ItemGroupName1.ToString())
                                                    select new
                                                    {
                                                        FractionGroupName = hd.FractionGroupName,
                                                        LaboratoryResultDts = lstEntityDt.Where(dts => dts.ItemGroupName1 == hd.ItemGroupName1)
                                                    }
                                            )
                         }).ToList();

            this.DataSource = lstSU;

            DetailReport.DataMember = "LaboratoryResultHds";
            DetailReport1.DataMember = "LaboratoryResultHds.LaboratoryResultDts";
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

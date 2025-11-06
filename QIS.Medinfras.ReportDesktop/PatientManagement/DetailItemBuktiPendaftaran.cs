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
    public partial class DetailItemBuktiPendaftaran : DevExpress.XtraReports.UI.XtraReport
    {
        public DetailItemBuktiPendaftaran()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID)
        {
            //int paramvisit = 0;
            string filterExpressionItemPackage = string.Format("VisitID = {0} AND IsDeleted = 0", VisitID);
            //ConsultVisitItemPackage EntityItemPackage = BusinessLayer.GetConsultVisitItemPackageList(filterExpressionItemPackage).FirstOrDefault();
            List<ConsultVisitItemPackage> lstEntityItemPackage = BusinessLayer.GetConsultVisitItemPackageList(filterExpressionItemPackage);
            string lstItemPackageID = string.Join(",", lstEntityItemPackage.Select(x => x.ItemID));

            List<vItemServiceDt> lstEntityDt = new List<vItemServiceDt>();
            if (!string.IsNullOrEmpty(lstItemPackageID))
            {
                string filterExpressionDt = string.Format("ItemID IN ({0}) AND IsDeleted = 0 ORDER BY ItemID ASC", lstItemPackageID);
                lstEntityDt = BusinessLayer.GetvItemServiceDtList(filterExpressionDt);

            }
            
            this.DataSource = lstEntityDt;
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

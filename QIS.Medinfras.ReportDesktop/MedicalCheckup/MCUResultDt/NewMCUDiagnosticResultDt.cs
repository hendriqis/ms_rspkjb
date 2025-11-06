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
    public partial class NewMCUDiagnosticResultDt : DevExpress.XtraReports.UI.XtraReport
    {
        public NewMCUDiagnosticResultDt()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID)
        {
            string filterSetPar = string.Format("ParameterCode = 'IS0001'");
            SettingParameter EntitySetPar = BusinessLayer.GetSettingParameterList(filterSetPar).FirstOrDefault();
            string filterExpressionHD = string.Format("VisitID = {0} AND HealthCareServiceUnitID != {1} AND IsDeleted = 0", VisitID, EntitySetPar.ParameterValue);
            vDiagnosticImagingResultHd EntityHD = BusinessLayer.GetvDiagnosticImagingResultHdList(filterExpressionHD).FirstOrDefault();

            if (EntityHD != null)
            {
                string filterExpressionDt = string.Format("ID = {0} AND IsDeleted = 0", EntityHD.ID);
                List<vDiagnosticImagingResultDt> lstEntityDt = BusinessLayer.GetvDiagnosticImagingResultDtList(filterExpressionDt);

                this.DataSource = lstEntityDt;
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

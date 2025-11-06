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
    public partial class NewMCUImagingResultDtRSPKSB : DevExpress.XtraReports.UI.XtraReport
    {
        public NewMCUImagingResultDtRSPKSB()
        {
            InitializeComponent();
        }

        public void InitializeReport(int ChargeTransactionID)
        {
            string filterSetPar = string.Format("ParameterCode = 'IS0001'");
            SettingParameterDt EntitySetPar = BusinessLayer.GetSettingParameterDtList(filterSetPar).FirstOrDefault();
            
            string filterExpressionHD = string.Format(
                    "TransactionID = {0} AND ChargesHealthcareServiceUnitID = {1} AND ImagingHdGCTransactionStatus != '{2}'",
                    ChargeTransactionID, EntitySetPar.ParameterValue, Constant.TransactionStatus.VOID);
            List<vImagingResultReport> lstEntityDt = BusinessLayer.GetvImagingResultReportList(filterExpressionHD);
            this.DataSource = lstEntityDt;
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //if (DetailReport.RowCount == 0)
            //{
            //    Detail.Visible = false;
            //}
            string paramedicCode = Convert.ToString(GetCurrentColumnValue("TestRealizationPhysicianCode"));
            ttdDokter.ImageUrl = string.Format("{0}{1}/Signature/{2}.png", AppConfigManager.QISPhysicalDirectory, AppConfigManager.QISParamedicImagePath, paramedicCode);
            ttdDokter.Visible = true;
        }

        private void GroupFooter1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
        }

    }
}

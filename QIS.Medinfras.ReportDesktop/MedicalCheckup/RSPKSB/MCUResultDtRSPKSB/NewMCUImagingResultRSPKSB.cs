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
    public partial class NewMCUImagingResultRSPKSB : DevExpress.XtraReports.UI.XtraReport
    {
        public NewMCUImagingResultRSPKSB()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID)
        {
            string filterSetPar = string.Format("ParameterCode = 'IS0001'");
            SettingParameterDt EntitySetPar = BusinessLayer.GetSettingParameterDtList(filterSetPar).FirstOrDefault();
            
            string filterExpressionImagingHD = string.Format(
            "VisitID = {0} AND ChargeTransactionID IN (SELECT pch.TransactionID FROM PatientChargesHd pch WITH(NOLOCK) WHERE HealthcareServiceUnitID = {1} AND pch.GCTransactionStatus != '{2}') AND GCTransactionStatus != '{2}'", VisitID, EntitySetPar.ParameterValue, Constant.TransactionStatus.VOID);
            List<ImagingResultHd> lstImagingEntityHd = BusinessLayer.GetImagingResultHdList(filterExpressionImagingHD);
            
            //int ChargesTransactionID = 0;
            //foreach (vImagingResultReport imagingResult in lstImagingEntityDt)
            //{
            //    if (ChargesTransactionID != 0 || ChargesTransactionID != imagingResult.TransactionID)
            //    {
            //        ChargesTransactionID = imagingResult.TransactionID;
            //    }
            //}

            this.DataSource = lstImagingEntityHd;
            
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Int32 ChargeTransactionID = Convert.ToInt32(GetCurrentColumnValue("ChargeTransactionID"));

            subImagingResultForDt.CanGrow = true;
            NewMCUImagingResultDtRSPKSB.InitializeReport(ChargeTransactionID);
        }

    }
}

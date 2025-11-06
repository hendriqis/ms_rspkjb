using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPendapatanPerKelas : BaseDailyPortraitRpt
    {
        public LPendapatanPerKelas()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
            base.InitializeReport(param);
        }

        //int qtyPatient = 0;
        //int qtyPayer = 0;
        //int qtyTot = 0;
        //int TotqtyPatient = 0;
        //int TotqtyPayer = 0;
        //int TotqtyTot = 0;
        //int TotReportQtyPatient = 0;
        //int TotReportQtyPayer = 0;
        //int TotReportQtyTot = 0;
        //int regisID;

        //private void Patient_SummaryReset(object sender, EventArgs e)
        //{
        //    qtyPatient = 0;
        //}
        //private void Payer_SummaryReset(object sender, EventArgs e)
        //{
        //    qtyPayer = 0;
        //}
        //private void Total_SummaryReset(object sender, EventArgs e)
        //{
        //    qtyTot = 0;
        //}
        //private void TotPatient_SummaryReset(object sender, EventArgs e)
        //{
        //    TotqtyPatient = 0;
        //}
        //private void TotPayer_SummaryReset(object sender, EventArgs e)
        //{
        //    TotqtyPayer = 0;
        //}
        //private void TotTotal_SummaryReset(object sender, EventArgs e)
        //{
        //    TotqtyTot = 0;
        //}
        

        //private void Patient_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        //{
        //    e.Result = qtyPatient;
        //    e.Handled = true;
        //}

        //private void Payer_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        //{
        //    e.Result = qtyPayer;
        //    e.Handled = true;
        //}

        //private void Total_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        //{
        //    e.Result = qtyTot;
        //    e.Handled = true;
        //}

        //private void TotPatient_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        //{
        //    TotReportQtyPatient += TotqtyPatient;
        //    e.Result = TotqtyPatient;
        //    e.Handled = true;
        //}

        //private void TotPayer_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        //{
        //    TotReportQtyPayer += TotqtyPayer;
        //    e.Result = TotqtyPayer;
        //    e.Handled = true;
        //}

        //private void TotTotal_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        //{
        //    TotReportQtyTot += TotqtyTot;
        //    e.Result = TotqtyTot;
        //    e.Handled = true;
        //}

        //private void TotReportPatient_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        //{
        //    e.Result = TotReportQtyPatient;
        //    e.Handled = true;
        //}

        //private void TotReportPayer_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        //{
        //    e.Result = TotReportQtyPayer;
        //    e.Handled = true;
        //}

        //private void TotReportTotal_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        //{
        //    e.Result = TotReportQtyTot;
        //    e.Handled = true;
        //}

        //private void Calculate_SummaryRowChanged(object sender, EventArgs e)
        //{
        //    int reg = Convert.ToInt32(GetCurrentColumnValue("RegistrationID"));
        //    decimal patientAmount = Convert.ToDecimal(GetCurrentColumnValue("PatientAmount"));
        //    decimal payerAmount = Convert.ToDecimal(GetCurrentColumnValue("PayerAmount"));
        //    if (reg != regisID)
        //    {
        //        regisID = reg;

        //        if (patientAmount > 0)
        //        {
        //            qtyPatient++;
        //            TotqtyPatient++;
        //        } if (payerAmount > 0)
        //        {
        //            qtyPayer++;
        //            TotqtyPayer++;
        //        }
        //        qtyTot = qtyPatient + qtyPayer;
        //        TotqtyTot = TotqtyPatient + TotqtyPayer;
        //    }
        //}

        //private void xrTableCell13_SummaryCalculated(object sender, TextFormatEventArgs e)
        //{
        //    String test;
        //}

        int counter = 0;
        private void xrTableCell2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrTableCell2.Text = (++counter).ToString();
        }

        private void GroupHeader2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            counter = 0;
        }
    }
}

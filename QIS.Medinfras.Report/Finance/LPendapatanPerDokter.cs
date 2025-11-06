using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace QIS.Medinfras.Report
{
    public partial class LPendapatanPerDokter : BaseDailyPortraitRpt
    {
        public LPendapatanPerDokter()
        {
            InitializeComponent();
        }
        int qtyPatient = 0;
        int qtyPayer = 0;
        int qtyTot = 0;
        int TotqtyPatient = 0;
        int TotqtyPayer = 0;
        int TotqtyTot = 0;
        int regisID;
        private void Patient_SummaryReset(object sender, EventArgs e)
        {
            qtyPatient = 0;
        }
        private void Payer_SummaryReset(object sender, EventArgs e)
        {
            qtyPayer = 0;
        }
        private void Total_SummaryReset(object sender, EventArgs e)
        {
            qtyTot = 0;
        }
        private void TotPatient_SummaryReset(object sender, EventArgs e)
        {
            TotqtyPatient = 0;
        }
        private void TotPayer_SummaryReset(object sender, EventArgs e)
        {
            TotqtyPayer = 0;
        }
        private void TotTotal_SummaryReset(object sender, EventArgs e)
        {
            TotqtyTot = 0;
        }
        private void Calculate_SummaryRowChanged(object sender, EventArgs e)
        {
            int reg = Convert.ToInt32(GetCurrentColumnValue("RegistrationID"));
            decimal patientAmount = Convert.ToDecimal(GetCurrentColumnValue("PatientAmount"));
            decimal payerAmount = Convert.ToDecimal(GetCurrentColumnValue("PayerAmount"));
            if (reg != regisID)
            {
                regisID = reg;

                if (patientAmount > 0)
                {
                    qtyPatient++;
                    TotqtyPatient++;
                } if (payerAmount > 0)
                {
                    qtyPayer++;
                    TotqtyPayer++;
                }
                qtyTot = qtyPatient + qtyPayer;
                TotqtyTot = TotqtyPatient + TotqtyPayer;
            }
        }

        private void Patient_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            e.Result = qtyPatient;
            e.Handled = true;
        }

        private void Payer_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            e.Result = qtyPayer;
            e.Handled = true;
        }

        private void Total_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            e.Result = qtyTot;
            e.Handled = true;
        }

        private void TotPatient_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            e.Result = TotqtyPatient;
            e.Handled = true;
        }

        private void TotPayer_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            e.Result = TotqtyPayer;
            e.Handled = true;
        }

        private void TotTotal_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            e.Result = TotqtyTot;
            e.Handled = true;
        }
    }
}

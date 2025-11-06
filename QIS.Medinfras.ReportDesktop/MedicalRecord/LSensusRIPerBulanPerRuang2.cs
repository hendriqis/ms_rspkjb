using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LSensusRIPerBulanPerRuang2 : BaseDailyPortraitRpt
    {
        public LSensusRIPerBulanPerRuang2()
        {
            InitializeComponent();
        }
    
        private Decimal SumTTXDays = 0;
        private Decimal SumDayCare = 0;

        private void lblALOS_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            //Decimal rs;
            //if (Convert.ToDecimal(lblPatientOut.Summary.GetResult()) + Convert.ToDecimal(lblDeadPatient.Summary.GetResult()) > 0)
            //    rs = (Convert.ToDecimal(lblLengthOfStay.Summary.GetResult()) / (Convert.ToDecimal(lblPatientOut.Summary.GetResult()) + Convert.ToDecimal(lblDeadPatient.Summary.GetResult())));
            //else
            //    rs = 0;
            //e.Result = rs.ToString("0.##");
            //e.Handled = true;
            Decimal rs;
            if (Convert.ToDecimal(lblPatient.Summary.GetResult()) > 0)
                rs = (Convert.ToDecimal(lblLengthOfStay.Summary.GetResult()) / (Convert.ToDecimal(lblPatient.Summary.GetResult())));
            else
                rs = 0;
            e.Result = rs.ToString("0.##");
            e.Handled = true;
        }

        private void lblBTO_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            Decimal rs;
            if (Convert.ToDecimal(lblBed.Summary.GetResult()) > 0)
                rs = (Convert.ToDecimal(lblPatientOut.Summary.GetResult()) + Convert.ToDecimal(lblDeadPatient.Summary.GetResult())) / Convert.ToDecimal(lblBed.Summary.GetResult());
            else
                rs = 0;
            e.Result = rs.ToString("0.##");
            e.Handled = true;
        }

        private void lblTOI_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            Decimal rs;
            if (Convert.ToDecimal(lblPatientOut.Summary.GetResult()) + Convert.ToDecimal(lblDeadPatient.Summary.GetResult()) > 0)
                rs = (Convert.ToDecimal(lblTTxDays.Summary.GetResult()) - Convert.ToDecimal(lblDayCare.Summary.GetResult())) / (Convert.ToDecimal(lblPatientOut.Summary.GetResult()) + Convert.ToDecimal(lblDeadPatient.Summary.GetResult()));
            else
                rs = 0;
            e.Result = rs.ToString("0.##");
            e.Handled = true;
        }

        private void lblNDR_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            Decimal rs;
            if (Convert.ToDecimal(lblPatientOut.Summary.GetResult()) + Convert.ToDecimal(lblDeadPatient.Summary.GetResult()) > 0)
                rs = Convert.ToDecimal(lblMore48Hour.Summary.GetResult()) / (Convert.ToDecimal(lblPatientOut.Summary.GetResult()) + Convert.ToDecimal(lblDeadPatient.Summary.GetResult())) * 1000;
            else
                rs = 0;
            e.Result = rs.ToString("0.##");
            e.Handled = true;
        }

        private void lblGDR_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            Decimal rs;
            if (Convert.ToDecimal(lblPatientOut.Summary.GetResult()) + Convert.ToDecimal(lblDeadPatient.Summary.GetResult()) > 0)
                rs = Convert.ToDecimal(lblDeadPatient.Summary.GetResult()) / (Convert.ToDecimal(lblPatientOut.Summary.GetResult()) + Convert.ToDecimal(lblDeadPatient.Summary.GetResult())) * 1000;
            else
                rs = 0;
            e.Result = rs.ToString("0.##");
            e.Handled = true;
        }

        private void lblTTxDays_SummaryCalculated(object sender, TextFormatEventArgs e)
        {
            if(e.Value != null)
                SumTTXDays = Convert.ToDecimal(e.Value);
        }

        private void xrLabel31_PrintOnPage(object sender, PrintOnPageEventArgs e)
        {
            Decimal rs;
            if (SumTTXDays > 0)
                rs = SumDayCare * 100 / SumTTXDays;
            else
                rs = 0;
            xrLabel31.Text = rs.ToString("0.##");
        }

        private void lblDayCare_SummaryCalculated(object sender, TextFormatEventArgs e)
        {
            if (e.Value != null)
                SumDayCare = Convert.ToDecimal(e.Value);
        }

        private void lblLengthOfStay_SummaryCalculated(object sender, TextFormatEventArgs e)
        {
        }

        
    }
}

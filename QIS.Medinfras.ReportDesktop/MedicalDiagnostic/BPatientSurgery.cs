using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;


namespace QIS.Medinfras.ReportDesktop
{
    public partial class BPatientSurgery : BaseCustomDailyPotraitRpt
    {

        public BPatientSurgery()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare h = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();
            cTanggal.Text = string.Format("{0}", h.City);

            base.InitializeReport(param);
        }
        private void xrTableCell44_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String JenisDrain = GetCurrentColumnValue("cfIsDrain").ToString();
            if (JenisDrain == "Ya")
            {
                lblJenisDrain.Visible = true;
            }
            else
            {
                lblJenisDrain.Visible = false;
            }
        }

        private void lblTamponType_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String TipeTampon = GetCurrentColumnValue("cfIsUsingTampon").ToString();
            if (TipeTampon == "Ya")
            {
                lblTamponType.Visible = true;
            }
            else
            {
                lblTamponType.Visible = false;
            }
        }

        private void lblCatherter_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String TipeCatherter = GetCurrentColumnValue("cfIsUsingCatheter").ToString();
            if (TipeCatherter == "Ya")
            {
                lblCatherter.Visible = true;
            }
            else
            {
                lblCatherter.Visible = false;
            }
        }
        private void lblSpecimen_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String Specimen = GetCurrentColumnValue("cfSpecimen").ToString();
            if (Specimen == "Ya")
            {
                lblSpecimen.Visible = true;
            }
            else
            {
                lblSpecimen.Visible = false;
            }
        }

        private void lblAntibioticsType_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String AntibioticsType = GetCurrentColumnValue("cfIsUsingAntibiotics").ToString();
            if (AntibioticsType == "Ya")
            {
                lblAntibioticsType.Visible = true;
            }
            else
            {
                lblAntibioticsType.Visible = false;
            }
        }

        private void lblAntibioticsTime_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String AntibioticsType = GetCurrentColumnValue("cfIsUsingAntibiotics").ToString();
            if (AntibioticsType == "Ya")
            {
                lblAntibioticsTime.Visible = true;
            }
            else
            {
                lblAntibioticsTime.Visible = false;
            }
        }

        private void lblCatheterFrom_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String CatheterFrom = GetCurrentColumnValue("cfIsUsingCatheter").ToString();
            if (CatheterFrom == "Ya")
            {
                lblCatheterFrom.Visible = true;
            }
            else
            {
                lblCatheterFrom.Visible = false;
            }
        }
    }
}

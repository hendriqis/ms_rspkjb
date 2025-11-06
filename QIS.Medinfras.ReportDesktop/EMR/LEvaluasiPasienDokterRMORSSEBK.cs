using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LEvaluasiPasienDokterRMORSSEBK : BaseCustomDailyPotraitA3Rpt
    {
        public LEvaluasiPasienDokterRMORSSEBK()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lblPeriod.Text = string.Format("Periode Visit : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
            base.InitializeReport(param);
        }

        private void xrTable2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
        //    string Diagnose = GetCurrentColumnValue("Diagnose").ToString();
        //    string ChiefComplaint = GetCurrentColumnValue("ChiefComplaint").ToString();
        //    string VitalSign = GetCurrentColumnValue("VitalSign").ToString();
        //    string ReviewOfSystem = GetCurrentColumnValue("ReviewOfSystem").ToString();
        //    string DoctorNotes = GetCurrentColumnValue("DoctorNotes").ToString();
        //    string LabResult = GetCurrentColumnValue("LabResult").ToString();
        //    string RadResult = GetCurrentColumnValue("RadResult").ToString();
        //    string Prescription = GetCurrentColumnValue("Prescription").ToString();

            //if (Diagnose == "" && ChiefComplaint == "" && VitalSign == "" && ReviewOfSystem == "" && DoctorNotes == "" && LabResult == "" && RadResult == "" && Prescription == "")
            //{
            //    cNo.Visible = false;
            //    cPatient.Visible = false;
            //    cRegistration.Visible = false;
            //    cDiagnose.Visible = false;
            //    cChiefComplaint.Visible = false;
            //    cVitalSign.Visible = false;
            //    cReviewOfSystem.Visible = false;
            //    cPlanningNotes.Visible = false;
            //}
            //else
            //{
            //    cNo.Visible = true;
            //    cPatient.Visible = true;
            //    cRegistration.Visible = true;
            //    cDiagnose.Visible = true;
            //    cChiefComplaint.Visible = true;
            //    cVitalSign.Visible = true;
            //    cReviewOfSystem.Visible = true;
            //    cPlanningNotes.Visible = true;
            //}
        }
    }
}

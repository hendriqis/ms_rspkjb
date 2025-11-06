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
    public partial class LEvaluasiPasienDokterRMO : BaseCustomDailyPotraitA3Rpt
    {
        public LEvaluasiPasienDokterRMO()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            StandardCode sc = BusinessLayer.GetStandardCode(param[1]);
            string start = sc.TagProperty.Substring(0, 5);
            string end = sc.TagProperty.Substring(6, 5);
            DateTime tanggal = Helper.YYYYMMDDToDate(param[0]);
            DateTime tanggalBaru = Helper.YYYYMMDDToDate(param[0]).AddDays(1);

            if (param[1] != Constant.Shift.MALAM)
            {
                lblPeriod.Text = string.Format("Periode : {0} ({1} - {2})",
                    Helper.YYYYMMDDToDate(param[0]).ToString(Constant.FormatString.DATE_FORMAT),
                    start,
                    end
                    );
            }
            else
            {
                lblPeriod.Text = string.Format("Periode : {0} {1} s/d {2} {3}",
                    tanggal.ToString(Constant.FormatString.DATE_FORMAT),
                    start,
                    tanggalBaru.ToString(Constant.FormatString.DATE_FORMAT),
                    end
                    );
            }
            base.InitializeReport(param);
        }

        private void xrTable2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //string Diagnose = GetCurrentColumnValue("Diagnose").ToString();
            //string ChiefComplaint = GetCurrentColumnValue("ChiefComplaint").ToString();
            //string VitalSign = GetCurrentColumnValue("VitalSign").ToString();
            //string ReviewOfSystem = GetCurrentColumnValue("ReviewOfSystem").ToString();
            //string DoctorNotes = GetCurrentColumnValue("DoctorNotes").ToString();
            //string LabResult = GetCurrentColumnValue("LabResult").ToString();
            //string RadResult = GetCurrentColumnValue("RadResult").ToString();
            //string Prescription = GetCurrentColumnValue("Prescription").ToString();

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

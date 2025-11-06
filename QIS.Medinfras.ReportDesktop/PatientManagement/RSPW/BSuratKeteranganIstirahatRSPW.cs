using System;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BSuratKeteranganIstirahatRSPW : BaseDailyPortrait2Rpt
    {
        public BSuratKeteranganIstirahatRSPW()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            base.InitializeReport(param);

            ConsultVisit entityCV = BusinessLayer.GetConsultVisitList(string.Format(param[0]))[0];
            Registration entity = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0}" , entityCV.RegistrationID))[0];
            vPatient entityP = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", entity.MRN))[0];
            vHealthcare entityH1 = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", appSession.HealthcareID))[0];
            ParamedicMaster entityPM = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", entityCV.ParamedicID))[0];

            lblName.Text = entityP.PatientName;
            lblDOB.Text = entityP.DateOfBirthInString;
            lblAddress.Text = entityP.HomeAddress;
            lblGender.Text = entityP.cfGender;

            if (param[5] == null || param[5] == "")
            {
                lblDiagnose.Visible = false;
                xrLabel8.Visible = false;
                xrLabel10.Visible = false;
            }
            else
            {
                lblDiagnose.Text = param[5];
            }
            String days = GetDateAbsent(DateTime.Parse(param[1]).Date, DateTime.Parse(param[2]).Date);

            String startDate = param[3];
            String endDate = param[4];
            lblDate.Text = string.Format(" {0} hari. Terhitung mulai tanggal {1} s/d {2}.", days, startDate, endDate);

            ttdDokter.ImageUrl = string.Format("{0}{1}/Signature/{2}.png", AppConfigManager.QISPhysicalDirectory, AppConfigManager.QISParamedicImagePath, entityPM.ParamedicCode);
            ttdDokter.Visible = true;

            lblPrintDate.Text = string.Format("{0}, {1}", entityH1.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));
            lblParamedic.Text = entityPM.FullName;
            base.InitializeReport(param);
        }

        public String GetDateAbsent(DateTime date1, DateTime date2)
        {
            TimeSpan span = date2.AddDays(1) - date1; 
            int day = span.Days;

            return day.ToString();
        }
    }
}

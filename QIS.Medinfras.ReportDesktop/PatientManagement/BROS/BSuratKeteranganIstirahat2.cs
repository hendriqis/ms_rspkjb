using System;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Text;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BSuratKeteranganIstirahat2 : BaseDailyPortraitRpt
    {
        public BSuratKeteranganIstirahat2()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            base.InitializeReport(param);

            Registration entity = BusinessLayer.GetRegistrationList(string.Format(param[0]))[0];
            vPatient entityP = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", entity.MRN))[0];
            vHealthcare entityH1 = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", appSession.HealthcareID))[0];
            ConsultVisit entityCV = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", entity.RegistrationID))[0];
            ParamedicMaster entityPM = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", entityCV.ParamedicID))[0];

            lblName.Text = entityP.PatientName;
            lblDOB.Text = String.Format("{0} / {1}", entityP.DateOfBirthInString, entityP.AgeInYear);
            lblOccupation.Text = entityP.Occupation;
            lblAddress.Text = entityP.HomeAddress;
            lblGender.Text = entityP.cfGender;

            if (param[5] == null || param[5] == "")
            {
                lblDiagnose.Visible = false;
                xrLabel10.Visible = false;
                xrLabel13.Visible = false;
            }
            else
            {
                lblDiagnose.Text = param[5];
            }

            String days = GetDateAbsent(DateTime.Parse(param[1]).Date, DateTime.Parse(param[2]).Date);

            String startDate = param[3];
            String endDate = param[4];
            String daysInWord = NumberInWords(Convert.ToInt32(days));
            String daysInWordTrimStart = daysInWord.TrimStart();
            String daysInWords= daysInWordTrimStart.TrimEnd();

            lblDate.Text = string.Format("{0} ({1}) hari. Terhitung mulai tanggal {2} s/d {3}.", days, daysInWords, startDate, endDate);

            lblPrintDate.Text = string.Format("{0}, {1}", entityH1.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));
            lblParamedic.Text = entityPM.FullName;

            ttdDokter.ImageUrl = string.Format("{0}{1}/Signature/{2}.png", AppConfigManager.QISPhysicalDirectory, AppConfigManager.QISParamedicImagePath, entityPM.ParamedicCode);
            ttdDokter.Visible = true;
            base.InitializeReport(param);
        }

        public String GetDateAbsent(DateTime date1, DateTime date2)
        {
            TimeSpan span = date2.AddDays(1) - date1; 
            int day = span.Days;

            return day.ToString();
        }

        public static String NumberInWords(int days)
        {
            string[] bilangan = { "", "Satu", "Dua", "Tiga", "Empat", "Lima", "Enam", "Tujuh", "Delapan", "Sembilan", "Sepuluh", "Sebelas" };
            string temp = "";
            if (days < 12)
            {
                temp = " " + bilangan[days];
            }
            else if (days < 20)
            {
                temp = NumberInWords(days - 10).ToString() + " Belas";
            }
            else if (days < 100)
            {
                temp = NumberInWords(days / 10) + " Puluh" + NumberInWords(days % 10);
            }
            else if (days < 200)
            {
                temp = " Seratus" + NumberInWords(days - 100);
            }
            else if (days < 1000)
            {
                temp = NumberInWords(days / 100) + " Ratus" + NumberInWords(days % 100);
            }
            else if (days < 2000)
            {
                temp = " Seribu" + NumberInWords(days - 1000);
            }
            else if (days < 1000000)
            {
                temp = NumberInWords(days / 1000) + " Ribu" + NumberInWords(days % 1000);
            }
            else if (days < 1000000000)
            {
                temp = NumberInWords(days / 1000000) + " Juta" + NumberInWords(days % 1000000);
            }

            return temp;
        }
    }
}

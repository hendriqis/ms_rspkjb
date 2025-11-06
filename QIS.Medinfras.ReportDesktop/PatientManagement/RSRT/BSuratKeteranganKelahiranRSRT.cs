using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BSuratKeteranganKelahiranRSRT : BaseRpt
    {
        public BSuratKeteranganKelahiranRSRT()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vPatientBirthRecordFull entity = BusinessLayer.GetvPatientBirthRecordFullList(param[0])[0];
            ConsultVisit entityVisit = BusinessLayer.GetConsultVisitList(string.Format("VisitID = '{0}'", entity.VisitID))[0];
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];

            PatientBirthRecord entityBirthRecord = BusinessLayer.GetPatientBirthRecord(entity.BirthRecordID);
            vConsultVisit1 entityVisitMom = BusinessLayer.GetvConsultVisit1List(string.Format("VisitID = {0}",entityBirthRecord.MotherVisitID))[0];

            #region Header
            if (oHealthcare != null) {
                txtTtdDate.Text = string.Format("{0}, {1}", oHealthcare.City, GetMonthFormat(DateTime.Now));
            }
            #endregion

            #region Detail
            lblSklNo.Text = string.Format("Nomor : {0}", entity.ReferenceNo);
            cellTTD.Text = entityVisitMom.ParamedicName.ToUpper();
            txtTtdName.Text = string.Format("( {0} )", entityVisitMom.ParamedicName.ToUpper());
            string gender = "";
            if (entity.Gender.ToLower() == "perempuan") {
                gender = "female";
            }
            else if (entity.Gender.ToLower() == "laki-laki")
            {
                gender = "male";
            }
            cellNamaBayi.Text = entity.BabyFullName.ToString().ToUpper() ;
            cJkBayiEng.Text = gender.ToLower();
            cJkBayiIndo.Text = string.Format("{0},", entity.Gender.ToLower());

            string dob = GetDayFormat(entity.DateOfBirth); 
            string[] dobData  = dob.Split('|');
            cellHari.Text = string.Format("{0}", dobData[0].ToUpper());
            cellHariEng.Text = dobData[1].ToUpper();
            cellTanggalLahir.Text = entity.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT_1);

            cellPanjang.Text = string.Format("{0}", entity.Length.ToString("G29"));
            decimal weightGramDecimal = entity.Weight * 1000;
            decimal weightGram = System.Math.Round(weightGramDecimal, 0, MidpointRounding.ToEven);
            cellBerat.Text = string.Format("{0}", weightGram.ToString("G29"));
            cellJam.Text = string.Format("{0}", entity.TimeOfBirth);

         
            cellNamaIbu.Text = entity.MotherFullName.ToString().ToUpper();
            cellAlamatRumah.Text = entity.MotherStreetName.ToString().ToUpper();
            cellKelainanBawaan.Text = entity.ChildNo.ToString();
            cAnakke.Text = entity.ChildNo.ToString();
            cellNoIdentitas.Text = entity.IdentityNo;
           // cellJenisKelamin.Text = entity.Gender.ToString().ToUpper();
            cellPekerjaan.Text = entity.Occupation.ToString().ToUpper();


            cellKelahiaran.Text = entity.TwinSingle.ToString().ToUpper();
            cellKondisiKelahiran.Text = entity.BornCondition.ToUpper();
            cellKelahiranDenganTindakan.Text = entity.BirthMethod.ToUpper();
            cellKelainanBawaan.Text = entity.HerediateryDefectRemarks.ToUpper();

            cellFatherName.Text = entity.FatherFullName;
            cellFatherIdentityNo.Text = entity.FatherIdentity;
            cellFatherOccupation.Text = entity.OccupationFather;
            cellFatherAddress.Text = entity.FatherStreetName;

            #endregion

            #region Footer

             
            #endregion

            base.InitializeReport(param);
        }

        public static String GetDayFormat(DateTime date)
        {
            
            string[] hari = { "Minggu", "Senin", "Selasa", "Rabu", "Kamis", "Jumat", "Sabtu" };
            string[] days = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            string hariIni = hari[(int)date.DayOfWeek];
            string day = days[(int)date.DayOfWeek];
            return string.Format("{0}|{1}", hariIni, day);
        }

        public static String GetMonthFormat(DateTime date)
        {
            string[] bulan = { "JANUARI", "FEBUARI", "MARET", "APRIL", "MEI", "JUNI", "JULI","AGUSTUS", "SEPTEMBER", "OKTOBER","NOVEMBER","DESEMBER" };
            string Month = bulan[(int)date.Month - 1];
            string days = date.Day.ToString();
            string years = date.Year.ToString();
            return string.Format("{0} {1} {2}", days, Month, years);
        }

    }
}

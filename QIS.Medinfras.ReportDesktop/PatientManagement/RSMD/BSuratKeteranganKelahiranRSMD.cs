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
    public partial class BSuratKeteranganKelahiranRSMD : BaseRpt
    {
        public BSuratKeteranganKelahiranRSMD()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vPatientBirthRecordFull entity = BusinessLayer.GetvPatientBirthRecordFullList(param[0]).FirstOrDefault();
            String filterExpression = String.Format("GCParamedicType = '{0}' AND BirthRecordID = {1}", Constant.ParamedicRole.PELAKSANA, entity.BirthRecordID);
            vPatientBirthRecordParamedic entity2 = BusinessLayer.GetvPatientBirthRecordParamedicList(filterExpression).FirstOrDefault();
            vConsultVisit entityVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = '{0}'", entity.VisitID))[0];
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            HealthcareServiceUnit entityHUS = BusinessLayer.GetHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = '{0}'", entityVisit.HealthcareServiceUnitID))[0];
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", entityHUS.HealthcareID))[0];

            #region Convert
            //jeniskelamin
            String JenisKelamin = "";
            if (entity.Gender == "Male")
            {
                JenisKelamin = "Laki - Laki";
            }
            else
            {
                JenisKelamin = "Perempuan";
            }
            
            //hari
            String Hari = entity.DateOfBirth.DayOfWeek.ToString();

            if (Hari == "Monday")
            {
                Hari = "Senin";
            }
            else if (Hari == "Tuesday")
            {
                Hari = "Selasa";
            }
            else if (Hari == "Wednesday")
            {
                Hari = "Rabu";
            }
            else if (Hari == "Thursday")
            {
                Hari = "Kamis";
            }
            else if (Hari == "Friday")
            {
                Hari = "Jum'at";
            }
            else if (Hari == "Saturday")
            {
                Hari = "Sabtu";
            }
            else if (Hari == "Sunday")
            {
                Hari = "Minggu";
            }

            //tanggal
            String Tanggal = entity.DateOfBirth.Day.ToString();
            String TanggalTTD1 = entity.LastUpdatedDate.Day.ToString();
            String TanggalTTD2 = entity.CreatedDate.Day.ToString();
            String Bulan = entity.DateOfBirth.Month.ToString();
            String BulanTTD1 = entity.LastUpdatedDate.Month.ToString();
            String BulanTTD2 = entity.CreatedDate.Month.ToString();
            String Tahun = entity.DateOfBirth.Year.ToString();
            String TahunTTD1 = entity.LastUpdatedDate.Year.ToString();
            String TahunTTD2 = entity.CreatedDate.Year.ToString();
                      
            if (Bulan == "1" || BulanTTD1 == "1" || BulanTTD2 == "1")
            {
                Bulan = "Januari";
                BulanTTD1 = "Januari";
                BulanTTD2 = "Januari";
            }
            else if (Bulan == "2" || BulanTTD1 == "2" || BulanTTD2 == "2")
            {
                Bulan = "Febuari";
                BulanTTD1 = "Febuari";
                BulanTTD2 = "Febuari";
            }
            else if (Bulan == "3" || BulanTTD1 == "3" || BulanTTD2 == "3")
            {
                Bulan = "Maret";
                BulanTTD1 = "Maret";
                BulanTTD2 = "Maret";
            }
            else if (Bulan == "4" || BulanTTD1 == "4" || BulanTTD2 == "4")
            {
                Bulan = "April";
                BulanTTD1 = "April";
                BulanTTD2 = "April";
            }
            else if (Bulan == "5" || BulanTTD1 == "5" || BulanTTD2 == "5")
            {
                Bulan = "Mei";
                BulanTTD1 = "Mei";
                BulanTTD2 = "Mei";
            }
            else if (Bulan == "6" || BulanTTD1 == "6" || BulanTTD2 == "6")
            {
                Bulan = "Juni";
                BulanTTD1 = "Juni";
                BulanTTD2 = "Juni";
            }
            else if (Bulan == "7" || BulanTTD1 == "7" || BulanTTD2 == "7")
            {
                Bulan = "Juli";
                BulanTTD1 = "Juli";
                BulanTTD2 = "Juli";
            }
            else if (Bulan == "8" || BulanTTD1 == "8" || BulanTTD2 == "8")
            {
                Bulan = "Agustus";
                BulanTTD1 = "Agustus";
                BulanTTD2 = "Agustus";
            }
            else if (Bulan == "9" || BulanTTD1 == "9" || BulanTTD2 == "9")
            {
                Bulan = "September";
                BulanTTD1 = "September";
                BulanTTD2 = "September";
            }
            else if (Bulan == "10" || BulanTTD1 == "10" || BulanTTD2 == "10")
            {
                Bulan = "Oktober";
                BulanTTD1 = "Oktober";
                BulanTTD2 = "Oktober";
            }
            else if (Bulan == "11" || BulanTTD1 == "11" || BulanTTD2 == "11")
            {
                Bulan = "November";
                BulanTTD1 = "November";
                BulanTTD2 = "November";
            }
            else if (Bulan == "12" || BulanTTD1 == "12" || BulanTTD2 == "12")
            {
                Bulan = "Desember";
                BulanTTD1 = "Desember";
                BulanTTD2 = "Desember";
            }
            
            //persalinanke
            String PersalinanKe = "";
            if (entity.ChildNo == 1) 
            {
                PersalinanKe = "(Satu)";
            }
            else if (entity.ChildNo == 2) 
            {
                PersalinanKe = "(Dua)"; 
            }
            else if (entity.ChildNo == 3)
            {
                PersalinanKe = "(Tiga)";
            }
            else if (entity.ChildNo == 4)
            {
                PersalinanKe = "(Empat)";
            }
            else if (entity.ChildNo == 5)
            {
                PersalinanKe = "(Lima)";
            }
            else if (entity.ChildNo == 6)
            {
                PersalinanKe = "(Enam)";
            }
            else if (entity.ChildNo == 7)
            {
                PersalinanKe = "(Tujuh)";
            }
            else if (entity.ChildNo == 8)
            {
                PersalinanKe = "(Delapan)";
            }
            else if (entity.ChildNo == 9)
            {
                PersalinanKe = "(Sembilan)";
            }
            else
            {
                PersalinanKe = "(Sepuluh)";
            }

            #endregion

            #region Detail
            cellNoSKL.Text = String.Format("No. {0}", entity.ReferenceNo);
            cellParamedicName.Text = entityVisit.ParamedicName;
            
            cellNamaAnak.Text = entity.BabyFullName;

            cellGender.Text = JenisKelamin;

            cellNamaIbu.Text = entity.MotherFullName;

            cellIdentityNo.Text = entity.IdentityNo;
            cellOccupation.Text = entity.Occupation;
            cellAddress.Text = entity.MotherStreetName;

            cellTanggal.Text = string.Format("{0} / {1} {2} {3}", Hari, Tanggal, Bulan, Tahun);
            cellJam.Text = entity.TimeOfBirth + " WIB";

            cellPersalinan.Text = entity.BirthMethod;
            cellPanjang.Text = String.Format("{0} cm", Convert.ToString(entity.Length.ToString("#,#")));
            cellBerat.Text = String.Format("{0} kg", Convert.ToString(entity.Weight));
            cellPersalinanKe.Text = String.Format("{0} {1}", Convert.ToString(entity.ChildNo), PersalinanKe);
            cellJenisKelahiran.Text = entity.TwinSingle;

            #endregion

            #region TTD

            if (entity.LastUpdatedDate != null && entity.LastUpdatedDateInString != "01-Jan-1900")
            {
                lblTglTTD.Text = string.Format("{0}, {1} {2} {3}", entityHealthcare.City, TanggalTTD1, BulanTTD1, TahunTTD1);
                lblTTD.Text = string.Format("({0})", entityVisit.ParamedicName);
            }
            else
            {
                lblTglTTD.Text = string.Format("{0}, {1} {2} {3}", entityHealthcare.City, TanggalTTD2, BulanTTD2, TahunTTD2);
                lblTTD.Text = string.Format("({0})", entityVisit.ParamedicName);
            }

            #endregion

            #region Footer
            //if (oHealthcare != null)
            //{
            //    cHealthcareCityZipCodes.Text = string.Format("{0} {1} {2}", oHealthcare.StreetName, oHealthcare.City, oHealthcare.ZipCode);
            //    cHealthcarePhone.Text = string.Format("Telpon : {0} Fax.: {1}", oHealthcare.PhoneNo1, oHealthcare.FaxNo1);
            //}
            #endregion


            base.InitializeReport(param);
        }

    }
}

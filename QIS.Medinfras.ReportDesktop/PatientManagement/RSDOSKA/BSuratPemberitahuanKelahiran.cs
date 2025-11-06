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
    public partial class BSuratPemberitahuanKelahiran : BaseRpt
    {
        public BSuratPemberitahuanKelahiran()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            PatientBirthRecord entityPatient = BusinessLayer.GetPatientBirthRecordList(string.Format("MotherVisitID = {0}", param[0]))[0];
            vPatientBirthRecordFull entity = BusinessLayer.GetvPatientBirthRecordFullList(string.Format("BirthRecordID = {0}", entityPatient.BirthRecordID))[0];
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];

            #region Header

            if (oHealthcare != null)
            {
                lblHealthcareName.Text = oHealthcare.HealthcareName;
            }

            #endregion

            #region Detail
            cellRMBayi.Text = string.Format("No RM Bayi : {0}", entity.MedicalNo);
            CellNoKK.Text = entity.FatherIdentity;
            CellTempatLahirAyah.Text = entity.FatherCityOfBirth;
            CellTempatLahirIbu.Text = entity.MotherCityOfBirth;
            CellPendidikanAyah.Text = entity.educationfather;
            CellPendidikanIbu.Text = entity.educationmother;
            CellNikIbu.Text = entity.IdentityNo;
            CellNikAyah.Text = entity.FatherIdentity;
            CellJam.Text = entity.TimeOfBirth;
            CellPanjang.Text = entity.Length.ToString();
            CellBerat.Text = entity.Weight.ToString();
            CellAnakKe.Text = entity.ChildNo.ToString();
            CellHari.Text = entity.cfHari;
            CellDOB.Text = entity.DateOfBirthInString;
            CellIbuDOB.Text = entity.MotherDOBInString;
            CellAyahDOB.Text = entity.FatherDOBInString;
            CellNamaIbu.Text = entity.MotherFullName;
            CellNamaAyah.Text = entity.FatherFullName;
            CellUmurIbu.Text = entity.MotherAgeInYear.ToString();
            CellRT.Text = entity.MotherRT;
            CellRW.Text = entity.MotherRW;
            CellKelurahan.Text = entity.MotherCounty;
            CellKabupaten.Text = entity.MotherCity;
            CellKecamatan.Text = entity.MotherDistrict;
            CellProvinsi.Text = entity.MotherState;
            CellUmurAyah.Text = entity.FatherAgeInYear.ToString();
            CellPekerjaanAyah.Text = entity.OccupationFather;
            CellPekerjaanIbu.Text = entity.Occupation;
            CellAgamaAyah.Text = entity.ReligionFather;
            CellAgamaIbu.Text=entity.ReligionMother;
            #endregion

            base.InitializeReport(param);
        }

    }
}

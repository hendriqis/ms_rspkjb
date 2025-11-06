using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using ThoughtWorks.QRCode.Codec;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BFormPenjaminRanapdanTindakan : BaseDailyPortrait2Rpt
    {
        public BFormPenjaminRanapdanTindakan()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            Registration oRegis = BusinessLayer.GetRegistrationList(string.Format(param[0]))[0];
            vMedicalRecord entity = BusinessLayer.GetvMedicalRecordList(string.Format("MRN = {0} AND RegistrationID = {1}", oRegis.MRN, oRegis.RegistrationID))[0];
            //vPatient entityP = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", entity.MRN))[0];
            Patient entityP = BusinessLayer.GetPatient(entity.MRN);

            //HEADER
            //PASIEN
            lblNamaPasien.Text = entity.PatientName;
            lblTglLahirPasien.Text = string.Format("{0}   {1} th", entity.DateOfBirthInString, entity.AgeInYear);
            lblAlamatPasien.Text = entity.StreetName;
            lblNIKPasien.Text = entityP.SSN;
            lblAgamaPasien.Text = entity.Agama;
            lblPekerjaanPasien.Text = entity.Pekerjaan;
            lblNoTlpnPasien.Text = string.Format("{0} | {1}", entity.PhoneNo1, entity.PhoneNo2);
            lblDokter.Text = entity.Dokter;
            lblKlsPerawatan.Text = entity.Class;

            //PENANGGUNGJAWAB
            lblNamaPenanggungjawab.Text = entity.NamaPenanggungJawab;
            lblHubPasien.Text = entity.Relasi;
            lblAlamatPenanggungjawab.Text = entity.AlamatPenanggungJawab;
            lblNoTlpnPenanggungjawab.Text = entity.TelpPenanggungJawab;
            lblBusinessPartner.Text = entity.BusinessPartnerName;
            lblTgl.Text = string.Format("{0}, {1}", oHealthcare.City, entity.ActualVisitDateInString);
            lblNamaPenanggungjawab2.Text = string.Format("({0})", entity.NamaPenanggungJawab);
            lblNamaPetugas.Text = string.Format("({0})" , appSession.UserFullName);
            if (entity.BusinessPartnerID == 1)
            {
                ckbPribadi.Checked = true;
            }
            else
            {
                ckbAsuransi.Checked = true;
            }
            base.InitializeReport(param);
        }
    }
}

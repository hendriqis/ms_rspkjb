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
    public partial class BSEPRSSBB : BaseDailyLandscapeRpt
    {
        public BSEPRSSBB()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vRegistrationBPJSInfo2 entity = BusinessLayer.GetvRegistrationBPJSInfo2List(String.Format("RegistrationID = {0}", param[0]))[0];

            lblHealthcare.Text = BusinessLayer.GetHealthcare(appSession.HealthcareID).HealthcareName;
            lblPRB.Text = entity.ProlanisPRB;
            lblKatarak.Text = entity.IsCataract;
            lblSEPNo.Text = entity.NoSEP;
            lblSEPDate.Text = entity.TanggalSEPInString;
            lblCardNo.Text = entity.NoPeserta;
            lblMRN.Text = entity.MedicalNo;
            lblPatientName.Text = entity.FullName;
            lblDOB.Text = entity.DateOfBirthInString;
            lblGender.Text = entity.Gender;

            string[] namaPoliklinik = entity.NamaPoliklinik.Split('|');
            lblPoliTujuan.Text = namaPoliklinik[1];
            lblPoliTujuan.Text = string.Format("{0} | {1}", entity.NamaPoliklinik, entity.NamaSubSpesialis);
            lblAsalFasKes.Text = entity.NamaRujukan;
            lblDiagnose.Text = entity.NamaDiagnosa;
            lblPeserta.Text = entity.JenisPeserta;
            lblCOB.Text = "-";
            lblJnsRawat.Text = entity.JenisRawat;

            if (entity.GCPatientCategory == Constant.GCPatientCategory.KARYAWAN_RS)
            {
                lblkelasRawat.Visible = false;
                lblTitikRanap.Visible = false;
                lblKlsRawat.Text = "-";
            }
            else
            {
                if (entity.DepartmentID == Constant.Facility.INPATIENT)
                {
                    lblkelasRawat.Visible = true;
                    lblTitikRanap.Visible = true;
                    lblKlsRawat.Text = entity.ClassName;
                }
                else
                {
                    lblkelasRawat.Visible = true;
                    lblTitikRanap.Visible = true;
                    lblKlsRawat.Text = "-";
                }
            }
            lblNoSKDP.Text = entity.NoSuratKontrol;
            lblNoRujukan.Text = entity.NoRujukan;
            if (entity.KodeRujukan != "1124R005")
            {
                lblTglRujukan.Text = entity.TanggalRujukan.ToString(Constant.FormatString.DATE_FORMAT);
                lblTglPulang.Text = entity.TanggalPulang.ToString(Constant.FormatString.DATE_FORMAT);
            }
            else
            {
                lblTglRujukan.Text = "-";
                lblTglPulang.Text = " ";
            }
            lblCetakanKe.Text = String.Format("Cetakan ke {0} - {1} {2}", entity.PrintNumber, DateTime.Today.ToString("MM/dd/yy"), DateTime.Now.ToString("HH:mm:ss tt"));
            logoBPJS.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/BPJS.jpg");
            base.InitializeReport(param);
        }
    }
}

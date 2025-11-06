using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BSuratJaminanPelayananRanapOld : BaseRpt
    {
        public BSuratJaminanPelayananRanapOld()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            //vRegistrationInhealthInpatient entity = BusinessLayer.GetvRegistrationInhealthInpatientList(string.Format("RegistrationID = {0}", param[0])).[0];
            vRegistrationInhealth entity = BusinessLayer.GetvRegistrationInhealthList(string.Format("RegistrationID = {0}", param[0]))[0];
            lblNoSJP.Text = entity.NoSJP;
            if (entity.TipeSJP == "COB")
            {
                lblCOB.Text = "(COB)";
            }
            else {
                lblCOB.Text = "(NON COB)";
            }
            lblNoInhealt.Text = entity.NoKartuPeserta;
            lblNoBPJS.Text = entity.NoKartuPesertaBPJS;
            lblNamaPasien.Text = entity.NamaPeserta;
            lblTglLahir.Text = entity.TanggalLahir.ToString(Constant.FormatString.DATE_FORMAT);
            lblJenisKelamin.Text = entity.JenisKelamin;
            lblPlan.Text = entity.NamaPlan;
            lblProduk.Text = entity.NamaKelas;
            lblBadanUsaha.Text = string.Empty;
            lblNoRujukan.Text = entity.NoRujukan;
            lblTglRujukan.Text = entity.TanggalRujukan.ToString(Constant.FormatString.DATE_FORMAT);
            lblFaskesPerujuk.Text = entity.NamaPPKAsalRujukan;
            lblMR.Text = entity.NoRM;
            lblDiagnosaAwalRI.Text = entity.NamaDiagnosa;
            lblDiagnosaRujukan.Text = entity.NamaDiagnosa;
            
            logoBPJS.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/BPJS.jpg");         
        }
    }
}

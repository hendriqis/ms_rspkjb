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
    public partial class BSuratJaminanPelayananRanap : BaseRpt
    {
        public BSuratJaminanPelayananRanap()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            //vRegistrationInhealthOutpatient entity = BusinessLayer.GetvRegistrationInhealthOutpatientList(string.Format("RegistrationID = {0}", param[0]))[0];
            vRegistrationInhealth entity = BusinessLayer.GetvRegistrationInhealthList(string.Format("RegistrationID = {0}", param[0]))[0];
            lblNoSJP.Text = entity.NoSJP;

            #region Benefit Inhealth
            subSuratJaminanPelayananContent.CanGrow = true;
            bSuratJaminanPelayananInhealthContent.InitializeReport(entity);
            #endregion
            //lblNoInhealt.Text = entity.NoKartuPeserta;
            //lblNamaPasien.Text = entity.NamaPeserta;
            //lblTglLahir.Text = entity.TanggalLahir.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            //if (entity.JenisKelamin == Constant.Gender.FEMALE)
            //{
            //    lblJenisKelamin.Text = "Perempuan";
            //}
            //else
            //{
            //    lblJenisKelamin.Text = "Pria";
            //}
            //lblPlan.Text = entity.NamaPlan;
            //lblKelasRawatMI.Text = entity.NamaKelas;
            //lblProduk.Text = entity.ProdukCOB;
            //lblBadanUsaha.Text = string.Format("{0} - {1}", entity.KodeBU, entity.NamaBU);
            //lblTanggalSJP.Text = entity.TanggalSJP.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            //lblNoRujukan.Text = entity.NoRujukan;
            //lblTglRujukan.Text = entity.TanggalRujukan.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            //if (!string.IsNullOrEmpty(entity.NamaPPKAsalRujukan))
            //{
            //    lblFaskesPerujuk.Text = entity.NamaPPKAsalRujukan;
            //}
            //else
            //{
            //    lblFaskesPerujuk.Text = "-";
            //}
            //lblPoli.Text = string.Format("{0} - {1}", entity.KodePoli, entity.NamaPoli);
            //lblMR.Text = entity.NoRM;
            //lblDiagnosaAwal.Text = string.Format("{0} - {1}", entity.KodeDiagnosa, entity.NamaDiagnosa);
            //lblDiagnosaAkhir.Text = string.Empty;
            //lblNamaDokter.Text = entity.NamaDokter;

            logoBPJS.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/Inhealth.jpg");

            #region Benefit Inhealth
            List<InhealthPatientBenefits> oBenefits = BusinessLayer.GetInhealthPatientBenefitsList(string.Format("MRN = {0} AND IsDeleted = 0", entity.MRN));
            if (oBenefits.Count != 0)
            {
                subInhealthBenefit.CanGrow = true;
                bSuratJaminanPelayananBeneiftInhealth.InitializeReport(oBenefits);
            }
            #endregion
        }
    }
}

using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BSuratJaminanPelayananRajalContent : BaseRpt
    {
        private int _lineNumber = 0;
        public BSuratJaminanPelayananRajalContent()
        {
            _lineNumber = 0;
            InitializeComponent();
        }

        public void InitializeReport(vRegistrationInhealth entity)
        {
            //lblNoSJP.Text = entity.NoSJP;
            lblNoInhealt.Text = entity.NoKartuPeserta;
            lblNamaPasien.Text = entity.NamaPeserta;
            lblTglLahir.Text = entity.TanggalLahir.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            if (entity.JenisKelamin == Constant.Gender.FEMALE)
            {
                lblJenisKelamin.Text = "Perempuan";
            }
            else
            {
                lblJenisKelamin.Text = "Pria";
            }
            lblPlan.Text = entity.NamaPlan;
            lblKelasRawatMI.Text = entity.NamaKelas;
            lblProduk.Text = entity.ProdukCOB;
            lblBadanUsaha.Text = string.Format("{0} - {1}", entity.KodeBU, entity.NamaBU);
            lblTanggalSJP.Text = entity.TanggalSJP.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            lblNoRujukan.Text = entity.NoRujukan;
            lblTglRujukan.Text = entity.TanggalRujukan.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            if (!string.IsNullOrEmpty(entity.NamaPPKAsalRujukan))
            {
                lblFaskesPerujuk.Text = entity.NamaPPKAsalRujukan;
            }
            else
            {
                lblFaskesPerujuk.Text = "-";
            }
            lblPoli.Text = string.Format("{0} - {1}", entity.KodePoli, entity.NamaPoli);
            lblMR.Text = entity.NoRM;
            lblDiagnosaAwal.Text = string.Format("{0} - {1}", entity.KodeDiagnosa, entity.NamaDiagnosa);
            lblDiagnosaAkhir.Text = string.Empty;
            lblNamaDokter.Text = entity.NamaDokter;

            //this.DataSource = lst;
        }
    }
}

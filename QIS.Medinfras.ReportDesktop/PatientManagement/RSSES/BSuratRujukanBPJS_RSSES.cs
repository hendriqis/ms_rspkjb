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
    public partial class BSuratRujukanBPJS_RSSES : BaseDailyPortrait1Rpt
    {
        public BSuratRujukanBPJS_RSSES()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            Healthcare h = BusinessLayer.GetHealthcare(AppSession.UserLogin.HealthcareID);
            logoBPJS.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/BPJS.jpg");
            lblHealthcare.Text = h.HealthcareName;

            vRegistrationBPJSInfo2 entity = BusinessLayer.GetvRegistrationBPJSInfo2List(String.Format("RegistrationID = {0}", param[0])).FirstOrDefault();

            lblNamaRujukanKe.Text = string.Format("{0} - {1}", entity.PPKDirujuk, entity.NamaPPKDirujuk);
            lblNoRujukanKe.Text = entity.NoRujukanKe;
            lblTanggalRujukanKe.Text = entity.TanggalRujukanKe.ToString(Constant.FormatString.DATE_FORMAT);
            lblJenisPelayananDirujuk.Text = entity.JenisPelayananDirujuk;
            lblTipeRujukanKe.Text = entity.TipeRujukanKe;
            lblCardNo.Text = entity.NoPeserta;
            lblPatientName.Text = entity.FullName;
            lblDOB.Text = entity.DateOfBirthInString;
            if (entity.Gender == "L")
                lblGender.Text = "Laki-Laki";
            else if (entity.Gender == "P")
                lblGender.Text = "Perempuan";
            lblDiagnosaDirujuk.Text = string.Format("{0} {1}", entity.DiagnoseIDDirujuk, entity.DiagnosaDirujuk);
            lblCatatanDirujuk.Text = entity.CatatanDirujuk;

            lblTanggalCetak.Text = String.Format("Tgl. Cetak {0}", DateTime.Now.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT));

            base.InitializeReport(param);
        }
    }
}

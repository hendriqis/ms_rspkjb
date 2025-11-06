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
using ThoughtWorks.QRCode.Codec;
using System.IO;
using System.Linq;
using System.Text;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BSuratRujukanBPJS_RSRTH : BaseRpt
    {
        public BSuratRujukanBPJS_RSRTH()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vRegistrationBPJSInfo2 entity = BusinessLayer.GetvRegistrationBPJSInfo2List(String.Format("RegistrationID = {0}", param[0]))[0];
            Healthcare h = BusinessLayer.GetHealthcare(AppSession.UserLogin.HealthcareID);
            lblHealthcareName.Text = h.HealthcareName;

            lblNamaRujukanKe.Text = string.Format("{0} - {1}", entity.PPKDirujuk, entity.NamaPPKDirujuk);
            lblNoRujukanKe.Text = entity.NoRujukanKe;
            lblTanggalRujukanKe.Text = entity.TanggalRujukanKe.ToString(Constant.FormatString.DATE_FORMAT);
            lblJenisPelayananDirujuk.Text = entity.JenisPelayananDirujuk;
            lblTipeRujukanKe.Text = entity.TipeRujukanKe;
            lblCardNo.Text = entity.NoPeserta;
            string gender = "";
            if (entity.Gender == "L")
                gender = "Laki-Laki";
            else if (entity.Gender == "P")
                gender = "Perempuan";
            lblPatientName.Text = string.Format("{0} ({1})", entity.FullName, gender);
            lblDOB.Text = entity.DateOfBirthInString;
            lblDiagnosaDirujuk.Text = string.Format("{0} {1}", entity.DiagnoseIDDirujuk, entity.DiagnosaDirujuk);
            lblCatatanDirujuk.Text = entity.CatatanDirujuk;

            lblTanggalCetak.Text = String.Format("Tgl. Cetak {0}", DateTime.Now.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT));

            logoBPJS.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/BPJS.jpg");
        }
    }
}

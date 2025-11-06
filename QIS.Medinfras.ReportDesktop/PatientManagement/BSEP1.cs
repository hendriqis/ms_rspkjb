using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BSEP1 : BaseRpt
    {
        public BSEP1()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vRegistrationBPJS entity = BusinessLayer.GetvRegistrationBPJSList(String.Format("RegistrationID = {0}",param[0]))[0];
            lblNoSEP.Text = entity.NoSEP;
            lblSEPDate.Text = entity.TanggalSEP.ToString(Constant.FormatString.DATE_FORMAT);
            lblNHSNo.Text = entity.NHSRegistrationNo;
            lblName.Text = entity.PatientName;
            lblDOB.Text = entity.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT);
            lblGender.Text = entity.Sex;
            lblPolTujuan.Text = entity.NamaRujukan;
            lblAsalFasKes.Text = entity.NamaPoliklinik;
            lblDiagnose.Text = entity.NamaDiagnosa;
            lblRemarks.Text = entity.Catatan;
            lblPeserta.Text = entity.PatientName;
            lblJnsRawat.Text = entity.JenisRawat;
            lblKlsRawat.Text = entity.KelasTanggungan;
            //base.InitializeReport(param);
        }
    }
}

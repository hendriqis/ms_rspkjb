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
    public partial class BSuratRujukanRS : BaseRpt
    {
        public BSuratRujukanRS()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vRegistrationBPJS entity = BusinessLayer.GetvRegistrationBPJSList(String.Format("RegistrationID = {0}", param[0]))[0];

            lblHealthcareName.Text = BusinessLayer.GetHealthcare(appSession.HealthcareID).HealthcareName;
            lblPoliBPJS.Text = entity.BPJSPoli;
            lblNama.Text = entity.PatientName;
            lblNoKartuBPJS.Text = entity.NoPeserta;
            lblDiagnosa.Text = entity.NamaDiagnosa;
            lblKeterangan.Text = "";

            lblNoRujukan.Text = entity.NoRujukan;
            lblAsalRumahSakit.Text = "";
            lblKelamin.Text = entity.Sex;
            lblDepartmentID.Text = entity.DepartmentID;

            lblTanggal.Text = DateTime.Today.ToString("MM/dd/yy");
        }
    }
}

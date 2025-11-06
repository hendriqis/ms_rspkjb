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
    public partial class BSuratRencanaInap : BaseRpt
    {
        public BSuratRencanaInap()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vRegistrationBPJS entity = BusinessLayer.GetvRegistrationBPJSList(String.Format("RegistrationID = {0}", param[0]))[0];

            lblHealthcareName.Text = BusinessLayer.GetHealthcare(appSession.HealthcareID).HealthcareName;
            lblCardNo.Text = entity.NHSRegistrationNo;
            lblDOB.Text = entity.DateOfBirth.ToString("dd MMMM yyyy");
            if (entity.PatientName != null)
            {
                if (entity.GCSex == Constant.Gender.MALE)
                {
                    lblPatientName.Text = string.Format("{0} ({1})", entity.PatientName, "Laki Laki");
                }
                else if (entity.GCSex == Constant.Gender.FEMALE)
                {
                    lblPatientName.Text = string.Format("{0} ({1})", entity.PatientName, "Perempuan");
                }
            }
            DateTime Date = DateTime.Parse(param[1]);
            if (!String.IsNullOrEmpty(entity.DiagnosaUtama))
            {
                lblDiagnose.Text = entity.DiagnosaUtama;
            }
            else
            {
                lblDiagnose.Text = entity.NamaDiagnosa; 
            }
            lblParamedicDPJP.Text = entity.ParamedicName;
            lblDiagnose.Text = entity.NamaDiagnosa;
            lblRencanaInap.Text = String.Format("{0}", Date.ToString("dd MMMM yyyy"));
            lblNoSEP.Text = string.Format("No. {0}", entity.NoSPRI);
            lblCetakanKe.Text = String.Format("Tgl.Entri: {0} | Tgl.Cetak: {1} {2}", DateTime.Today.ToString("dd-MM-yyyy"), DateTime.Today.ToString("dd-MM-yyyy"), DateTime.Now.ToString("HH:mm:ss tt"));

            logoBPJS.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/BPJS.jpg");
        }
    }
}

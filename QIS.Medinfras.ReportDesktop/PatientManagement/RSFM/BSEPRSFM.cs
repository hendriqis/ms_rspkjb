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
using System.Linq;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BSEPRSFM : BaseRpt
    {
        public BSEPRSFM()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vRegistrationBPJS entity = BusinessLayer.GetvRegistrationBPJSList(String.Format("RegistrationID = {0}", param[0])).FirstOrDefault();
            vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", param[0])).FirstOrDefault();
            Patient entityPatient = BusinessLayer.GetPatientList(string.Format("MedicalNo = '{0}'", entity.MedicalNo))[0];

            lblHealthcareName.Text = BusinessLayer.GetHealthcare(appSession.HealthcareID).HealthcareName;

            lblSEPNo.Text = entity.NoSEP;
            lblSEPDate.Text = entity.TanggalSEP.ToString(Constant.FormatString.DATE_FORMAT);
            lblCardNo.Text = entity.NHSRegistrationNo;
            lblMRN.Text = entity.MedicalNo;
            lblPatientName.Text = entity.PatientName;
            lblDOB.Text = entity.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT);
            lblNoTelepon.Text = entityPatient.MobilePhoneNo1;
            lblDokter.Text = entity.ParamedicName;
            lblJenisKunjungan.Text = entityReg.VisitTypeName;
            
            if(entity.GCSex == Constant.Gender.MALE)
            {
                lblGender.Text = "Laki-laki";
            }
            else if (entity.GCSex == Constant.Gender.FEMALE) {
                lblGender.Text = "Perempuan";
            }

            lblSubSpesialis.Text = entity.NamaSubSpesialis;
            lblAsalFasKes.Text = entity.NamaRujukan;
            lblDiagnose.Text = entity.NamaDiagnosa;
            lblRemarks.Text = entity.Catatan;
            lblPeserta.Text = entity.JenisPeserta;
            lblJnsRawat.Text = entity.JenisRawat;
            lblKlsHak.Text = entity.NamaKelasTanggungan;
            lblPenjamin.Text = "";

            lblCetakanKe.Text = String.Format("Cetakan ke {0} - {1} {2}" ,entity.PrintNumber ,DateTime.Today.ToString("MM/dd/yy") ,DateTime.Now.ToString("HH:mm:ss tt"));

            logoBPJS.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/BPJS.jpg");         
        }
    }
}

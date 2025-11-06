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
    public partial class BSEP : BaseRpt
    {
        public BSEP()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vRegistrationBPJS entity = BusinessLayer.GetvRegistrationBPJSList(String.Format("RegistrationID = {0}", param[0]))[0];

            lblHealthcareName.Text = BusinessLayer.GetHealthcare(appSession.HealthcareID).HealthcareName;

            if (entity.NoSKTM != "")
            {
                lblNoSKTM.Text = string.Format("No. SKTM : {0}", entity.NoSKTM);
            }
            else
            {
                lblNoSKTM.Text = "";
            }
            if (entity.ProlanisPRB != "")
            {
                lblPRB.Text = string.Format("{0}", entity.ProlanisPRB);
            }
            else
            {
                lblPRB.Text = "";
            }

            if (entity.IsCataract == true)
            {
                lblKatarak.Text = string.Format("* PASIEN OPERASI KATARAK");
            }
            else
            {
                lblKatarak.Text = "";
            }

            lblSEPNo.Text = entity.NoSEP;
            lblSEPDate.Text = entity.TanggalSEP.ToString(Constant.FormatString.DATE_FORMAT);
            lblCardNo.Text = entity.NHSRegistrationNo;
            lblMRN.Text = entity.MedicalNo;
            lblPatientName.Text = entity.PatientName;
            lblDOB.Text = entity.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT);
            
            if(entity.GCSex == Constant.Gender.MALE)
            {
                lblGender.Text = "L";
            }
            else if (entity.GCSex == Constant.Gender.FEMALE) {
                lblGender.Text = "P";            
            }

            lblPoliTujuan.Text = entity.NamaPoliklinik;
            lblAsalFasKes.Text = entity.NamaRujukan;
            lblDiagnose.Text = entity.NamaDiagnosa;
            lblRemarks.Text = entity.Catatan;
            lblPeserta.Text = entity.JenisPeserta;
            lblCOB.Text = "";
            lblJnsRawat.Text = entity.JenisRawat;
            lblKlsRawat.Text = entity.BPJSClassName;
            
            string[] AccidentPayer = entity.AccidentPayer.Split(',');
            string AccidentPayerInString = "";
            if (AccidentPayer.Length > 0)
            {
                for (int i = 0; i < AccidentPayer.Length; i++)
                {
                    if (AccidentPayer[i] == "1")
                    {
                        AccidentPayerInString += "BPJS";
                    }
                    if (AccidentPayer[i] == "2")
                    {
                        AccidentPayerInString += ", JASA RAHARJA";
                    }
                    if (AccidentPayer[i] == "3")
                    {
                        AccidentPayerInString += ", TASPEN";
                    }
                    if (AccidentPayer[i] == "4")
                    {
                        AccidentPayerInString += ", ASABRI";
                    }
                }
            }

            lblAccidentPayer.Text = AccidentPayerInString;

            lblCetakanKe.Text = String.Format("Cetakan ke {0} - {1} {2}" ,entity.PrintNumber ,DateTime.Today.ToString("MM/dd/yy") ,DateTime.Now.ToString("HH:mm:ss tt"));

            logoBPJS.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/BPJS.jpg");         
        }
    }
}

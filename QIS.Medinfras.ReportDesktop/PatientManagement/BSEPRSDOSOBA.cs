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
    public partial class BSEPRSDOSOBA : BaseDailyPortrait1Rpt
    {
        public BSEPRSDOSOBA()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vRegistrationBPJSInfo2 entity = BusinessLayer.GetvRegistrationBPJSInfo2List(String.Format("RegistrationID = {0}", param[0])).FirstOrDefault();
            vRegistration entityReg = BusinessLayer.GetvRegistrationList(String.Format("RegistrationID = {0}", entity.RegistrationID)).FirstOrDefault();

            lblHealthcare.Text = BusinessLayer.GetHealthcare(appSession.HealthcareID).HealthcareName;
            if (entity.ProlanisPRB == null || entity.ProlanisPRB == "")
            {
                lblPRB.Visible = false;
            }
            else
            {
                lblPRB.Text = entity.ProlanisPRB;
            }
            lblKatarak.Text = entity.IsCataract;
            lblSEPNo.Text = entity.NoSEP;
            if (entity.TanggalSEP == null || entity.TanggalSEP.ToString(Constant.FormatString.DATE_FORMAT) == "01-Jan-1900")
                lblSEPDate.Text = "";
            else
                lblSEPDate.Text = entity.TanggalSEPInString;

            lblCardNo.Text = entity.NoPeserta;
            lblMRN.Text = entity.MedicalNo;
            lblPatientName.Text = entity.FullName;
            lblDOB.Text = entity.DateOfBirthInString;
            if (entity.Gender == "L")
                lblGender.Text = "Laki-Laki";
            else if (entity.Gender == "P")
                lblGender.Text = "Perempuan";

            string[] namaPoliklinik = entity.NamaPoliklinik.Split('|');
            if (entity.NamaPoliklinik == "" || entity.NamaPoliklinik == null)
            {
                lblPoliTujuan.Text = "";
                lblPoliTujuan.Text = "";
            }
            else
            {
                lblPoliTujuan.Text = namaPoliklinik[1];
                lblPoliTujuan.Text = string.Format("{0} | {1}", entity.NamaPoliklinik, entity.NamaSubSpesialis);
            }
            lblAsalFasKes.Text = entity.NamaRujukan;
            lblDiagnose.Text = entity.NamaDiagnosa;
            tbdpjp.Text = entity.ParamedicName;
            lblPeserta.Text = entity.JenisPeserta;
            lblCOB.Text = "-";
            lblJnsRawat.Text = entity.JenisRawat;
            lblHakKelas.Text = entity.NamaKelasTanggungan;

            //if (entity.GCPatientCategory == Constant.GCPatientCategory.KARYAWAN_RS)
            //{
            //    lblkelasRawat.Visible = false;
            //    lblTitikRanap.Visible = false;
            //    lblKlsRawat.Text = "-";
            //}
            //else
            //{
            //    if (entity.DepartmentID == Constant.Facility.INPATIENT)
            //    {
            //        lblkelasRawat.Visible = true;
            //        lblTitikRanap.Visible = true;
            //        lblKlsRawat.Text = entity.ClassName;
            //    }
            //    else
            //    {
            //        lblkelasRawat.Visible = true;
            //        lblTitikRanap.Visible = true;
            //        lblKlsRawat.Text = "-";
            //    }
            //}
            lblNoSKDP.Text = entity.NoSuratKontrol;
            lblNoRujukan.Text = entity.NoRujukan;
            if (entity.KodeRujukan != BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, Constant.SettingParameter.BPJS_CODE).ParameterValue)
            {
                lblTglRujukan.Text = entity.TanggalRujukan.ToString(Constant.FormatString.DATE_FORMAT);
                lblTglPulang.Text = entity.TanggalPulang.ToString(Constant.FormatString.DATE_FORMAT);
            }
            else
            {
                lblTglRujukan.Text = "-";
                lblTglPulang.Text = " ";
            }
            
            lblCetakanKe.Text = String.Format("Cetakan ke {0} - {1} {2}", entity.PrintNumber, DateTime.Today.ToString("MM/dd/yy"), DateTime.Now.ToString("HH:mm:ss tt"));
            logoBPJS.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/BPJS.jpg");
            lblHealthcareNameCell.Text = BusinessLayer.GetHealthcare(appSession.HealthcareID).HealthcareName;

            ConsultVisit entityCV = BusinessLayer.GetConsultVisitList(String.Format("RegistrationID = {0} ORDER BY VisitID ASC", entity.RegistrationID)).LastOrDefault();
            lblQueueNo.Text = entityCV.QueueNo.ToString();
            lblUserName.Text = AppSession.UserLogin.UserFullName;
            base.InitializeReport(param);
        }
    }
}

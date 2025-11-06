using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using ThoughtWorks.QRCode.Codec;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BSuratRujukanKeRSLain : BaseDailyPortrait2Rpt
    {
        public BSuratRujukanKeRSLain()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split('|');
            vConsultVisit entity = BusinessLayer.GetvConsultVisitList(String.Format("VisitID = {0}", param[0]))[0];
            vPatient entityPatient = BusinessLayer.GetvPatientList(String.Format("MRN = {0}", entity.MRN))[0];
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            NurseChiefComplaint entityNurseChiefComplaint = BusinessLayer.GetNurseChiefComplaintList(String.Format("VisitID = {0} AND IsDeleted = 0", entity.VisitID)).FirstOrDefault();
            vChiefComplaint entityChiefComplaint = BusinessLayer.GetvChiefComplaintList(String.Format("VisitID = {0} AND IsDeleted = 0", entity.VisitID)).FirstOrDefault();
            PatientReferralExternal entityReferralExternal = BusinessLayer.GetPatientReferralExternalList(string.Format("VisitID = {0} AND IsDeleted = 0", entity.VisitID)).FirstOrDefault();
            ConsultVisit entityCV = BusinessLayer.GetConsultVisit(entity.VisitID);
            vReferrer entityReferrer = null;
            if (entityCV.ReferralTo > 0)
            {
                entityReferrer = new vReferrer();
                entityReferrer = BusinessLayer.GetvReferrerList(string.Format("BusinessPartnerID = '{0}'", entityCV.ReferralTo)).FirstOrDefault();
            }

            lblNamaPasien.Text = entity.PatientName;
            lblNoRM.Text = entity.MedicalNo;
            lblTanggalLahir.Text = entity.DateOfBirthInString;
            lblJenisKelamin.Text = entityPatient.cfGender;
            lblAlamat.Text = entity.HomeAddress;

            string tglformat = generatedate(param[3]);
            lblTanggal.Text = string.Format("{0}, {1}", entityHealthcare.City, tglformat);
            if (entityReferrer != null)
            {
                lblReferralPhysician.Text = entityReferrer.BusinessPartnerName;
                lblDi.Text = string.Format("Di {0}", !string.IsNullOrEmpty(Function.GenerateAddress(entityReferrer.StreetName, entityReferrer.County, entityReferrer.District, entityReferrer.City, "")) ? Function.GenerateAddress(entityReferrer.StreetName, entityReferrer.County, entityReferrer.District, entityReferrer.City, "") : "Tempat");
                lblTherapy.Text = string.Format("{0}", (param[2]));
                lblKebutuhanPelayanan.Text = string.Format("{0}", (param[5]));
            }
            else
            {
                lblReferralPhysician.Text = param[1];
                lblTherapy.Text = string.Format("{0}", (param[2]));
                lblDi.Text = string.Format("Di {0}", (param[4]));
                lblKebutuhanPelayanan.Text = string.Format("{0}", (param[5]));
            }

            if (entityChiefComplaint == null)
            {
                if (entityNurseChiefComplaint == null)
                {
                    lblRiwayatPenyakit.Text = "";
                }
                else
                {
                    lblRiwayatPenyakit.Text = entityNurseChiefComplaint.MedicalHistory;
                }
            }
            else
            {
                lblRiwayatPenyakit.Text = entityChiefComplaint.PastMedicalHistory;
            }

            if (entityReferralExternal != null)
            {
                lblDiagnosa.Text = entityReferralExternal.ReferralToDiagnoseText;
                lblROS.Text = entityReferralExternal.ReferralToMedicalResumeText;
                lblTherapy.Text = entityReferralExternal.ReferralToPlanningText;
            }
            else
            {
                List<vPatientDiagnosis> entityDiag = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", entity.VisitID));
                if (entityDiag == null)
                {
                    lblDiagnosa.Text = "-";
                }
                else
                {
                    List<vPatientDiagnosis> lstPatientDiagnosis = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", entity.VisitID));
                    StringBuilder diagNotes = new StringBuilder();
                    foreach (vPatientDiagnosis patientDiagnosis in lstPatientDiagnosis)
                    {
                        if (diagNotes.ToString() != "")
                            diagNotes.Append(", ");
                        diagNotes.Append(patientDiagnosis.DiagnosisText);
                    }
                    lblDiagnosa.Text = diagNotes.ToString();
                }
                xrLabel11.Visible = false;
                xrLabel14.Visible = false;
                lblROS.Visible = false;
            }
            
            lblDokterPenanggungJawab.Text = entity.ParamedicName;

            base.InitializeReport(param);
        }

        public string generatedate(String tgl)
        {
            DateTime dt = Convert.ToDateTime(tgl);
            string tanggal = dt.Day.ToString();
            string bulan = "";
            string tahun = dt.Year.ToString();

            if (dt.Month == 1)
            {
                bulan = "Januari";
            }
            else if (dt.Month == 2)
            {
                bulan = "Februari";
            }
            else if (dt.Month == 3)
            {
                bulan = "Maret";
            }
            else if (dt.Month == 4)
            {
                bulan = "April";
            }
            else if (dt.Month == 5)
            {
                bulan = "Mei";
            }
            else if (dt.Month == 6)
            {
                bulan = "Juni";
            }
            else if (dt.Month == 7)
            {
                bulan = "Juli";
            }
            else if (dt.Month == 8)
            {
                bulan = "Agustus";
            }
            else if (dt.Month == 9)
            {
                bulan = "September";
            }
            else if (dt.Month == 10)
            {
                bulan = "Oktober";
            }
            else if (dt.Month == 11)
            {
                bulan = "November";
            }
            else if (dt.Month == 12)
            {
                bulan = "Desember";
            }

            return string.Format("{0} {1} {2}", tanggal, bulan, tahun);
        }
    }
}

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
    public partial class LKonsultasiKlinik : BaseDailyPortraitRpt
    {
        public LKonsultasiKlinik()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vClinicConsult entity = BusinessLayer.GetvClinicConsultList(string.Format("RegistrationID = {0}", param[0])).FirstOrDefault();
            vConsultVisit9 entityVisit = BusinessLayer.GetvConsultVisit9List(string.Format("RegistrationID = {0}", param[0]))[0];
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];

            txtPasien.Text = string.Format("{0} | {1}, Umur {2} th.", entity.PatientName, entity.Sex, entity.AgeInYear);
            txtAnamnese.Text = entity.SubjectiveResumeText;
            txtROS.Text = entity.ROSystem;
            txtPF.Text = entity.ObjectiveResumeText;
            txtPP.Text = entity.PlanningResumeText;
            string terapipengobatan = "";
            string terapiobatpulang = "";
            string tindakanoperasi = "";
            string tindaklanjut = "";
            if (!string.IsNullOrEmpty(entity.MedicationResumeText))
            {
                terapipengobatan = "Terapi Pengobatan: " + entity.MedicationResumeText;
            }
            else
            {
                terapipengobatan = "Terapi Pengobatan: -";
            }
            if (!string.IsNullOrEmpty(entity.DischargeMedicationResumeText))
            {
                terapiobatpulang = "Terapi Obat Pulang: " + entity.DischargeMedicationResumeText;
            }
            else
            {
                terapiobatpulang = "Terapi Obat Pulang: -";
            }
            if (!string.IsNullOrEmpty(entity.SurgeryResumeText))
            {
                tindakanoperasi = "Tindakan Operasi: " + entity.SurgeryResumeText;
            }
            else
            {
                tindakanoperasi = "Tindakan Operasi: -";
            }
            if (!string.IsNullOrEmpty(entity.InstructionResumeText))
            {
                tindaklanjut = "Tindak Lanjut: " + entity.InstructionResumeText;
            }
            else
            {
                tindaklanjut = "Tindak Lanjut: -";
            }
            txtTerapi.Text = string.Format("{0} | {1} | {2} | {3}", terapipengobatan, terapiobatpulang, tindakanoperasi, tindaklanjut);

            DateTime dateNow = DateTime.Now;
            base.InitializeReport(param);
            lblPrintDate.Text = string.Format("{0}, {1}", entityHealthcare.City, dateNow.ToString(Constant.FormatString.DATE_FORMAT));
            lblParamedicName.Text = entityVisit.ParamedicName;
            txtTS.Text = entityVisit.ParamedicName;
        }
    }
}

using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BKeteranganJKN : BaseA6Rpt
    {
        public BKeteranganJKN()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();
            vConsultVisit entityVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", param[0])).FirstOrDefault();
            vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", entityVisit.RegistrationID)).FirstOrDefault();
            vRegistrationBPJS entityBpjs = BusinessLayer.GetvRegistrationBPJSList(string.Format("RegistrationID = {0}", entityVisit.RegistrationID)).FirstOrDefault();
            vPatientDiagnosis entityDiagnosis = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND GCDiagnoseType = '{1}'", entityVisit.VisitID, Constant.DiagnoseType.MAIN_DIAGNOSIS)).FirstOrDefault();

            lblNama.Text = entityReg.PatientName;
            lblRM.Text = entityReg.MedicalNo;
            lblBusinessPartner.Text = entityReg.BusinessPartnerName;
            if (entityBpjs != null)
            {
                if (!String.IsNullOrEmpty(entityBpjs.AssessmentSummaryText))
                {
                    lblDiagnosa.Text = entityBpjs.AssessmentSummaryText;
                }
                else
                {
                    if (entityDiagnosis != null)
                    {
                        lblDiagnosa.Text = entityDiagnosis.DiagnosisText;
                    }
                    else
                    {
                        lblDiagnosa.Text = "";
                    }
                }
                if (!String.IsNullOrEmpty(entityBpjs.PlanningResumeText))
                {
                    lblTerapi.Text = entityBpjs.PlanningResumeText;
                }
                else
                {
                    lblTerapi.Text = "";
                }
                if (!String.IsNullOrEmpty(entityBpjs.NoSuratRencanaKontrolBerikutnya))
                {
                    lblNoSurat.Text = entityBpjs.NoSuratRencanaKontrolBerikutnya;
                }
                else
                {
                    lblNoSurat.Text = "";
                }
                if (!String.IsNullOrEmpty(entityBpjs.NoSuratRencanaKontrolBerikutnya))
                {
                    lblNoSurat.Text = entityBpjs.NoSuratRencanaKontrolBerikutnya;
                    if (entityBpjs.cfTglRencanaKontrolInString != "01-Jan-1900")
                    {
                        lblTglRencana.Text = string.Format("Tanggal : {0}", entityBpjs.TanggalRencanaKontrol.ToString(Constant.FormatString.DATE_FORMAT));
                    }
                    else
                    {
                        lblTglRencana.Text = string.Format("Tanggal : .................................................................");
                    }
                }

                if (entityBpjs.NoSuratRencanaKontrolBerikutnya != null && entityBpjs.NoSuratRencanaKontrolBerikutnya != "")
                {
                    if (entityBpjs.TanggalRujukan != null && entityBpjs.TanggalRujukan.ToString(Constant.FormatString.DATE_FORMAT) != "01-Jan-1900")
                    {
                        lblTglRujukan.Text = string.Format("{0} s/d {1}", entityBpjs.TanggalRujukan.ToString(Constant.FormatString.DATE_FORMAT), entityBpjs.TanggalRujukan.AddDays(89).ToString(Constant.FormatString.DATE_FORMAT));
                    }
                    else
                    {
                        lblTglRujukan.Text = "";
                    }
                }
                else
                {
                    lblTglRujukan.Text = "";
                }
            }
            else
            {
                lblDiagnosa.Text = "";
                lblTerapi.Text = "";
                lblNoSurat.Text = "";
                lblTglRencana.Text = string.Format("Tanggal : .................................................................");
            }
            lblDate.Text = string.Format("{0}, {1}", entityHealthcare.City, entityReg.RegistrationDate.ToString(Constant.FormatString.DATE_FORMAT));
            lblParamedic.Text = entityVisit.ParamedicName;
            
            base.InitializeReport(param);
        }

    }
}

using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.IO;
using System.Text;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BKeteranganJKNRSSK : BaseA6Rpt
    {
        public BKeteranganJKNRSSK()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();
            vConsultVisit entityVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", param[0])).FirstOrDefault();
            vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", entityVisit.RegistrationID)).FirstOrDefault();
            vRegistrationBPJS entityBpjs = BusinessLayer.GetvRegistrationBPJSList(string.Format("RegistrationID = {0}", entityVisit.RegistrationID)).FirstOrDefault();

            if (entityHealthcare != null)
            {
                xrLogo.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/logo.png");
                xrLogo.WidthF = 180;
                xrLogo.HeightF = 180;
            }

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
                    List<vPatientDiagnosis> entityDiag = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", entityVisit.VisitID));
                    if (entityDiag == null)
                    {
                        lblDiagnosa.Text = "";
                    }
                    else
                    {
                        List<vPatientDiagnosis> lstPatientDiagnosis = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", entityVisit.VisitID));
                        StringBuilder diagNotes = new StringBuilder();
                        foreach (vPatientDiagnosis patientDiagnosis in lstPatientDiagnosis)
                        {
                            if (diagNotes.ToString() != "")
                                diagNotes.Append(", ");
                            diagNotes.Append(patientDiagnosis.DiagnosisText);
                        }
                        lblDiagnosa.Text = diagNotes.ToString();
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

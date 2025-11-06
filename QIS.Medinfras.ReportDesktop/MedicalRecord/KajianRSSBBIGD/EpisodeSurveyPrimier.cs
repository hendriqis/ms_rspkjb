using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using System.Collections.Generic;
using DevExpress.XtraReports.Parameters;
using System.Linq;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class EpisodeSurveyPrimier : DevExpress.XtraReports.UI.XtraReport
    {
        public EpisodeSurveyPrimier()
        {
            InitializeComponent();
        }

        public void InitializeReport(int RegistrationID)
        {
            //vNurseChiefComplaint entityNC = BusinessLayer.GetvNurseChiefComplaintList(string.Format("VisitID = {0}", param)).FirstOrDefault();            
            List<Registration> entityRegistration = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0}", RegistrationID));
            ConsultVisit entityCV = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", RegistrationID)).FirstOrDefault();
            List<PatientAllergy> lstpatietAllergy = BusinessLayer.GetPatientAllergyList(string.Format("VisitID = {0} AND GCAllergenType = '0127^DA' AND IsDeleted = 0", entityCV.VisitID));            
            vNurseChiefComplaint entityNC = BusinessLayer.GetvNurseChiefComplaintList(string.Format("VisitID = {0}", entityCV.VisitID)).FirstOrDefault();

            HealthcareServiceUnit hsu = BusinessLayer.GetHealthcareServiceUnit(entityCV.HealthcareServiceUnitID);
            Healthcare healthcare = BusinessLayer.GetHealthcare(hsu.HealthcareID);

            if (entityNC.GCAirway != null && entityNC.GCAirway != "")
            {
                StandardCode entitiSC_Airway = BusinessLayer.GetStandardCode(entityNC.GCAirway);
                xAirway.Text = entitiSC_Airway.StandardCodeName;
            }
            else
            {
                xAirway.Text = "";
            }
             if (entityNC.GCBreathing != null && entityNC.GCBreathing != "")
            {
                StandardCode entitiSC_Breathing = BusinessLayer.GetStandardCode(entityNC.GCBreathing);
                xBreathing.Text = entitiSC_Breathing.StandardCodeName;
            }
            else
            {
                xBreathing.Text = "";
            }

             if (entityNC.GCCirculation != null && entityNC.GCCirculation != "")
            {
                StandardCode entitiSC_Circulation = BusinessLayer.GetStandardCode(entityNC.GCCirculation);
                xCirculation.Text = entitiSC_Circulation.StandardCodeName;
            }
            else
            {
                xCirculation.Text = "";
            }
             if (entityNC.GCDisability != null && entityNC.GCDisability != "")
            {
                StandardCode entitiSC_GCDisability = BusinessLayer.GetStandardCode(entityNC.GCDisability);
                xDisability.Text = entitiSC_GCDisability.StandardCodeName;
            }
            else
            {
                xDisability.Text = "";
            }
             if (entityNC.GCExposure != null && entityNC.GCExposure != "")
            {
                StandardCode entitiSC_GCExposure = BusinessLayer.GetStandardCode(entityNC.GCExposure);
                xExposure.Text = entitiSC_GCExposure.StandardCodeName;
            }
            else
            {
                xExposure.Text = "";
            }

             if (entityNC.IsAutoAnamnesis == false)
            {
                xAT.Checked = false;
            }
            else
            {
                xAT.Checked = true;
            }

             if (entityNC.IsAlloAnamnesis == false)
            {
                xAL.Checked = false;
            }
            else
            {
                xAL.Checked = true;
            }

             if (healthcare.Initial == "PHS")
             {
                 xPenyakitTerdahulu.Text = entityNC.MedicalHistory;

                 if (lstpatietAllergy.Count > 0)
                 {
                     string allergy = "";
                     foreach (PatientAllergy e in lstpatietAllergy)
                     {
                         if (string.IsNullOrEmpty(allergy))
                         {
                             allergy = e.Allergen;
                         }
                         else
                         {
                             allergy += string.Format(" , {0}", e.Allergen);
                         }
                     }
                     xAlergiObat.Text = allergy;
                 }
             }

            //xchkIs.Text = Convert.ToString(entityNC.IsAlloAnamnesis);
           
             //if (entityNC.IsAutoAnamnesis != null)
             //{
             //    //StandardCode entitiy = BusinessLayer.GetStandardCode(entityNC.IsAutoAnamnesis);
             //    //xchkIs.Text = entityNC.IsAutoAnamnesis;

             //}
             //else
             //{
             //    xchkIs.Text = "";
             //}

             this.DataSource = entityRegistration;
       
        }

    }
}

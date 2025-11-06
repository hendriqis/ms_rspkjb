using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class NurseChiefComplainDt : DevExpress.XtraReports.UI.XtraReport
    {
        public NurseChiefComplainDt()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID)
        {
            string filterExpression = string.Format("VisitID IN ({0}) AND IsDeleted = 0 ORDER BY ID DESC", VisitID);
            vNurseChiefComplaint entityHd = BusinessLayer.GetvNurseChiefComplaintList(filterExpression).FirstOrDefault();

            string filterExpressionAllergy = string.Format("MRN = {0} AND IsDeleted = 0 ORDER BY ID DESC", entityHd.MRN);
            vPatientAllergy entityAp = BusinessLayer.GetvPatientAllergyList(filterExpressionAllergy).FirstOrDefault();

            lblTanggal.Text = string.Format("{0}", entityHd.EmergencyCaseLocation);
            lblIndikasi.Text = string.Format("{0}", entityHd.NurseChiefComplaintText);
            lblAnamnesa.Text = string.Format("{0}", entityHd.MedicalHistory);
            if (entityAp != null)
            {

                lblKomorbiditas.Text = string.Format("Alergi = {0} , Reaksi = {1}", entityAp.Allergen, entityAp.Reaction);
            }
            else
            {
                xrLabel6.Visible = false;
                lblKomorbiditas.Visible = false;
            }
        }
    }
}

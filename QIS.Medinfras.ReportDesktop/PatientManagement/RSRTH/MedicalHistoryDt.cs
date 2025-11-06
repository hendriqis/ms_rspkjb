using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Collections.Generic;
using DevExpress.XtraReports.Parameters;


namespace QIS.Medinfras.ReportDesktop
{
    public partial class MedicalHistoryDt : DevExpress.XtraReports.UI.XtraReport
    {
        public MedicalHistoryDt()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID)
        {
            vConsultVisit1 entityCV = BusinessLayer.GetvConsultVisit1List(string.Format("VisitID IN ({0})", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            vNurseChiefComplaint entity = BusinessLayer.GetvNurseChiefComplaintList(string.Format("VisitID IN ({0}) AND IsDeleted = 0 ORDER BY ID DESC", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();

            lblAnamnesa.Text = string.Format("{0}", entity.NurseChiefComplaintText);
            lblKomorbiditas.Text = string.Format("{0}", entity.MedicalHistory);

            if (entity.IsPatientAllergyExists)
            {
                List<vPatient> lstAllergyHd = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", entity.MRN));
                List<vPatientAllergy> lstAllergy = BusinessLayer.GetvPatientAllergyList(string.Format("MRN = {0} AND IsDeleted = 0 ORDER BY ID DESC", entity.MRN));

                var lst = (from p in lstAllergyHd
                           select new
                           {
                               MedicalHistoryDts = lstAllergy.Where(dt => dt.MRN == p.MRN).ToList()
                           }).ToList();

                this.DataSource = lst;
                DetailReport.DataMember = "MedicalHistoryDts";
            }
            else
            {
                xrLabel1.Visible = false;
                xrLabel2.Visible = false;
                xrLabel3.Visible = false;
                xrLabel4.Visible = false;
                xrLabel5.Visible = false;
                xrLabel6.Visible = false;
                xrLabel9.Visible = false;
                xrLabel12.Visible = false;
            }
        }
        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (DetailReport.RowCount == 0)
            {
                Detail.Visible = false;
            }
        }
    }
}

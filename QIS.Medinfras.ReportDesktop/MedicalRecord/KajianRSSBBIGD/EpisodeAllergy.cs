using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using System.Data;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class EpisodeAllergy : DevExpress.XtraReports.UI.XtraReport
    {
        public EpisodeAllergy()
        {
            InitializeComponent();
        }

        //public void InitializeReport(int MRN)
        //{
        //    List<vPatientAllergy> lstAllergy = BusinessLayer.GetvPatientAllergyList(string.Format("MRN = {0} AND IsDeleted = 0", MRN));
        //    this.DataSource = lstAllergy;
        //}

        public void InitializeReport(int RegistrationID)
        {
            Registration entityRegistration = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0}", RegistrationID)).FirstOrDefault();
            ConsultVisit entityCV = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", RegistrationID)).FirstOrDefault();
            vNurseChiefComplaint entity = BusinessLayer.GetvNurseChiefComplaintList(string.Format("VisitID = {0}", entityCV.VisitID)).FirstOrDefault();
            vPatientAllergy lstAllergy = BusinessLayer.GetvPatientAllergyList(string.Format("MRN = {0} AND IsDeleted = 0", entityRegistration.MRN)).FirstOrDefault();

            this.DataSource = entityRegistration;
        }
    }
}
